/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/13/2018 11:31:54 AM
 */

using System;
using System.Collections.Generic;
using System.Reflection;

namespace XMLib
{
    /// <summary>
    /// 依赖注入容器
    /// </summary>
    public class Container : IContainer
    {
        #region 参数

        /// <summary>
        /// 别名
        /// </summary>
        private readonly Dictionary<string, string> _aliases;

        /// <summary>
        /// 别名反转
        /// </summary>
        private readonly Dictionary<string, List<string>> _aliasesReverse;

        /// <summary>
        /// 绑定数据
        /// </summary>
        private readonly Dictionary<string, BindData> _binds;

        /// <summary>
        /// 静态服务反转
        /// </summary>
        private readonly Dictionary<object, string> _instanceReverse;

        /// <summary>
        /// 静态服务
        /// </summary>
        private readonly Dictionary<string, object> _instances;

        /// <summary>
        /// 静态服务构建
        /// </summary>
        private readonly SortList<string, int> _instanceTiming;

        /// <summary>
        /// 解决过的服务
        /// </summary>
        private readonly HashSet<string> _resolved;

        /// <summary>
        /// 是否在清空过程中
        /// </summary>
        private bool _flushing;

        /// <summary>
        /// 静态服务递增ID
        /// </summary>
        private int _instanceId;

        /// <summary>
        /// 在服务构建修饰器之后的修饰器
        /// </summary>
        private readonly List<Action<IBindData, object>> _afterResolving;

        /// <summary>
        /// 服务构造修饰器
        /// </summary>
        private readonly List<Action<IBindData, object>> _release;

        /// <summary>
        /// 服务构造修饰器
        /// </summary>
        private readonly List<Action<IBindData, object>> _resolving;

        /// <summary>
        /// 重定义事件
        /// </summary>
        private readonly Dictionary<string, List<Action<object>>> _rebound;

        /// <summary>
        /// 同步实例
        /// </summary>
        private object _syncRoot;

        #endregion 参数

        /// <summary>
        /// 构造函数
        /// </summary>
        public Container()
        {
            _binds = new Dictionary<string, BindData>();
            _instances = new Dictionary<string, object>();
            _instanceReverse = new Dictionary<object, string>();
            _aliases = new Dictionary<string, string>();
            _aliasesReverse = new Dictionary<string, List<string>>();
            _instanceTiming = new SortList<string, int>();
            _instanceTiming.Forward = false;

            _resolving = new List<Action<IBindData, object>>();
            _afterResolving = new List<Action<IBindData, object>>();
            _release = new List<Action<IBindData, object>>();
            _rebound = new Dictionary<string, List<Action<object>>>();

            _resolved = new HashSet<string>();

            _instanceId = 0;

            //
            _flushing = false;

            _syncRoot = new object();
        }

        #region 构造绑定相关

        /// <summary>
        /// 解绑服务
        /// </summary>
        /// <param name="bindData">绑定实例</param>
        internal void UnBind(IBindable bindData)
        {
            lock (_syncRoot)
            {
                CheckIsFlusing();

                Release(bindData.Service);

                //移除别名
                List<string> serviceList;
                if (_aliasesReverse.TryGetValue(bindData.Service, out serviceList))
                {
                    foreach (string alias in serviceList)
                    {//移除所有绑定的别名
                        _aliases.Remove(alias);
                    }
                    //移除反向查找列表
                    _aliasesReverse.Remove(bindData.Service);
                }

                //移除绑定
                _binds.Remove(bindData.Service);
            }
        }

        /// <summary>
        /// 检查是否释放中
        /// </summary>
        protected void CheckIsFlusing()
        {
            if (_flushing)
            {
                throw new RuntimeException("当前容器正在释放中");
            }
        }

        /// <summary>
        /// 格式化服务名
        /// </summary>
        /// <param name="service">服务名</param>
        /// <returns>格式化服务名</returns>
        protected string FormatService(string service)
        {
            return service.Trim();
        }

        /// <summary>
        /// 包装一个类型，可以被用来生成服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="serviceType">类型</param>
        /// <returns>根据类型生成的服务</returns>
        protected virtual Func<IContainer, object[], object> WrapperTypeBuilder(string service, Type serviceType)
        {
            service = FormatService(service);
            return (container, args) => ((Container)container).CreateInstance(GetBindFillable(service), serviceType, args);
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="bindData">绑定数据</param>
        /// <param name="serviceType">服务类型</param>
        /// <param name="args">参数</param>
        /// <returns>实例</returns>
        private object CreateInstance(BindData bindData, Type serviceType, object[] args)
        {
            if (IsUnableType(serviceType))
            {
                return null;
            }

            args = GetConstructorsInjectParams(bindData, serviceType, args);

            try
            {
                if (args == null || 0 >= args.Length)
                {
                    return Activator.CreateInstance(serviceType);
                }

                return Activator.CreateInstance(serviceType, args);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取服务对应的绑定数据，如果没有则创建一个空的
        /// </summary>
        /// <param name="service">服务名</param>
        /// <returns>绑定数据</returns>
        private BindData GetBindFillable(string service)
        {
            Checker.NotEmptyOrNull(service, "service");

            BindData data = null;

            if (!_binds.TryGetValue(service, out data))
            {
                data = MakeEmptyBindData(service);
            }

            return data;
        }

        /// <summary>
        /// 是否是基础类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>是否是基础类型</returns>
        private bool IsBasicType(Type type)
        {
            return type == null || type.IsPrimitive || type == typeof(string);
        }

        /// <summary>
        /// 是否是无法实例化的类型
        /// </summary>
        /// <param name="type">服务类型</param>
        /// <returns>是否是无法实例化的类型</returns>
        private bool IsUnableType(Type type)
        {
            return type == null || type.IsAbstract || type.IsInterface || type.IsArray || type.IsEnum;
        }

        /// <summary>
        /// 创建一个空的绑定数据
        /// </summary>
        /// <param name="service">服务名</param>
        /// <returns>绑定数据</returns>
        private BindData MakeEmptyBindData(string service)
        {
            return new BindData(this, service, null, false);
        }

        /// <summary>
        /// 解决服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="args">参数</param>
        /// <returns>服务实例</returns>
        private object Resolve(string service, params object[] args)
        {
            Checker.NotEmptyOrNull(service, "service");
            lock (_syncRoot)
            {
                service = AliasToService(service);

                object instance;

                if (_instances.TryGetValue(service, out instance))
                {//返回单例对象
                    return instance;
                }

                BindData bindData = GetBindFillable(service);

                ///构建实例
                instance = Build(bindData, args);

                if (bindData.IsStatic)
                {
                    instance = Instance(bindData.Service, instance);
                }
                else
                {
                    TriggerOnResolving(bindData, instance);
                }

                _resolved.Add(service);

                return instance;
            }
        }

        /// <summary>
        /// 释放单例
        /// </summary>
        /// <param name="instance">单例实例</param>
        private void DisposeInstance(object instance)
        {
            if (instance is IDisposable)
            {
                ((IDisposable)instance).Dispose();
            }
        }

        /// <summary>
        /// 构建服务
        /// </summary>
        /// <param name="bindData">绑定数据</param>
        /// <param name="args">用户传入参数</param>
        /// <returns>服务实例</returns>
        private object Build(BindData bindData, object[] args)
        {
            object instance;

            if (null != bindData.Concrete)
            {
                instance = bindData.Concrete(this, args);
            }
            else
            {
                //推测服务类型
                Type type = SpeculatedServiceType(bindData.Service);
                instance = CreateInstance(bindData, type, args);
            }

            return instance;
        }

        /// <summary>
        /// 获取最终服务名
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>获取最终服务名</returns>
        private string AliasToService(string service)
        {
            service = FormatService(service);
            string alias;
            if (!_aliases.TryGetValue(service, out alias))
            {
                alias = service;
            }

            return alias;
        }

        /// <summary>
        /// 根据服务名推测服务的类型
        /// </summary>
        /// <param name="service">服务名</param>
        /// <returns>服务类型</returns>
        protected virtual Type SpeculatedServiceType(string service)
        {//可先不实现
            return null;
        }

        #endregion 构造绑定相关

        #region 事件调用

        /// <summary>
        /// 添加事件到列表
        /// </summary>
        /// <param name="closure">事件对象</param>
        /// <param name="list">事件列表</param>
        private void AddEvent(Action<IBindData, object> closure, List<Action<IBindData, object>> list)
        {
            Checker.NotNull(closure, "closure");

            lock (_syncRoot)
            {
                CheckIsFlusing();
                list.Add(closure);
            }
        }

        /// <summary>
        ///  触发消息
        /// </summary>
        /// <param name="bindData">绑定实例</param>
        /// <param name="instance">服务实例</param>
        /// <param name="list">事件列表</param>
        /// <returns>服务实例</returns>
        internal object Trigger(IBindData bindData, object instance, List<Action<IBindData, object>> list)
        {
            if (null == list || 0 >= list.Count)
            {
                return instance;
            }

            foreach (var evt in list)
            {
                evt(bindData, instance);
            }

            return instance;
        }

        /// <summary>
        /// 触发全局解决修饰器之后的修饰器回调
        /// </summary>
        /// <param name="bindData">服务绑定数据</param>
        /// <param name="instance">服务实例</param>
        /// <returns>被修饰器修饰后的服务实例</returns>
        private object TriggerOnAfterResolving(BindData bindData, object instance)
        {
            instance = bindData.TriggerAfterResolving(instance);
            return Trigger(bindData, instance, _afterResolving);
        }

        /// <summary>
        /// 触发全局释放修饰器
        /// </summary>
        /// <param name="bindData">服务绑定数据</param>
        /// <param name="instance">服务实例</param>
        /// <returns>被修饰器修饰后的服务实例</returns>
        private object TriggerOnRelease(BindData bindData, object instance)
        {
            instance = bindData.TriggerRelease(instance);
            return Trigger(bindData, instance, _release);
        }

        /// <summary>
        /// 触发全局解决修饰器
        /// </summary>
        /// <param name="bindData">服务绑定数据</param>
        /// <param name="instance">服务实例</param>
        /// <returns>被修饰器修饰后的服务实例</returns>
        private object TriggerOnResolving(BindData bindData, object instance)
        {
            instance = bindData.TriggerResolving(instance);
            instance = Trigger(bindData, instance, _resolving);

            //目前阶段，直接调用After
            return TriggerOnAfterResolving(bindData, instance);
        }

        /// <summary>
        /// 触发服务重定义事件
        /// </summary>
        /// <param name="service">发生重定义的服务</param>
        /// <param name="instance">服务实例（如果为空将会从容器请求）</param>
        private void TriggerOnRebound(string service, object instance = null)
        {//木有看懂=。=，先放着
        }

        #endregion 事件调用

        #region 依赖注入

        /// <summary>
        /// 获取构造函数依赖
        /// </summary>
        /// <param name="bindData">服务绑定数据</param>
        /// <param name="serviceType">服务类型</param>
        /// <param name="args">用户传入参数</param>
        /// <returns>参数列表</returns>
        private object[] GetConstructorsInjectParams(Bindable bindData, Type serviceType, object[] args)
        {
            ConstructorInfo[] constructors = serviceType.GetConstructors();
            if (constructors.Length <= 0)
            {
                return null;
            }

            Exception exception = null;
            foreach (ConstructorInfo info in constructors)
            {//遍历构造函数
                try
                {
                    //获取依赖
                    return GetDependencies(bindData, info.GetParameters(), args);
                }
                catch (Exception ex)
                {
                    if (exception == null)
                    {
                        exception = ex;
                    }
                }
            }

            Checker.Requires<RuntimeException>(exception != null);
            throw exception;
        }

        /// <summary>
        /// 获取参数依赖
        /// </summary>
        /// <param name="bindData">绑定数据</param>
        /// <param name="parameterInfo">参数信息</param>
        /// <param name="args">用户参数</param>
        /// <returns>参数列表</returns>
        private object[] GetDependencies(Bindable bindData, ParameterInfo[] parameterInfo, object[] args)
        {
            if (0 >= parameterInfo.Length)
            {
                return null;
            }

            object[] results = new object[parameterInfo.Length];

            int length = results.Length;
            for (int i = 0; i < length; i++)
            {
                ParameterInfo info = parameterInfo[i];

                object arg = null;
                if (null == arg)
                {
                    arg = GetCompactInjectUserParams(info, ref args);
                }

                if (null == arg)
                {
                    arg = GetDependenciesFromUserParams(info, ref args);
                }

                if (null == arg)
                {
                    throw new RuntimeException("未匹配到参数");
                }

                results[i] = arg;
            }

            return results;
        }

        /// <summary>
        /// 从用户传入的参数中获取依赖
        /// </summary>
        /// <param name="info">参数信息</param>
        /// <param name="args">用户参数</param>
        /// <returns>参数</returns>
        private object GetDependenciesFromUserParams(ParameterInfo info, ref object[] args)
        {
            if (null == args)
            {
                return null;
            }

            int length = args.Length;

            for (int i = 0; i < length; i++)
            {//遍历查找可用参数
                object arg = args[i];

                if (ChangeType(ref arg, info.ParameterType))
                {//转换成功
                    ArrayUtil.RemoveAt(ref args, i);//移除可用参数列表
                    return arg;
                }
            }

            return null;
        }

        /// <summary>
        /// 参数类型转换
        /// </summary>
        /// <param name="arg">需要转换的参数</param>
        /// <param name="conversionType">需要转换的类型</param>
        /// <returns>是否成功</returns>
        private bool ChangeType(ref object arg, Type conversionType)
        {
            try
            {
                if (null == arg || conversionType.IsInstanceOfType(arg))
                {
                    return true;
                }

                if (arg is IConvertible && typeof(IConvertible).IsAssignableFrom(conversionType))
                {
                    arg = Convert.ChangeType(arg, conversionType);
                    return true;
                }
            }
            catch (Exception)
            {//忽略该异常
            }

            return false;
        }

        /// <summary>
        /// 紧缩注入参数
        /// </summary>
        /// <param name="info">参数信息</param>
        /// <param name="args">参数列表</param>
        /// <returns>参数</returns>
        private object GetCompactInjectUserParams(ParameterInfo info, ref object[] args)
        {
            if (!CheckCompactInjectUserParams(info, args))
            {
                return null;
            }

            object[] result = args;
            args = null;

            if (typeof(object) == info.ParameterType
                && null != result
                && 1 == result.Length)
            {//返回object
                return result[0];
            }

            //返回 object[]
            return result;
        }

        private bool CheckCompactInjectUserParams(ParameterInfo info, object[] args)
        {
            if (null == args || args.Length <= 0)
            {
                return false;
            }

            return info.ParameterType == typeof(object[])
                || info.ParameterType == typeof(object);
        }

        /// <summary>
        /// 获取字段需求的服务
        /// </summary>
        /// <param name="property">字段</param>
        /// <returns>服务名</returns>
        protected virtual string GetPropertyNeedsService(PropertyInfo property)
        {
            InjectAttribute injectAttr = (InjectAttribute)property.GetCustomAttributes(typeof(InjectAttribute), false)[0];
            if (string.IsNullOrEmpty(injectAttr.Alias))
            {
                return Type2Service(property.PropertyType);
            }
            else
            {
                return injectAttr.Alias;
            }
        }

        /// <summary>
        /// 获取参数需求的服务
        /// </summary>
        /// <param name="arg">参数</param>
        /// <returns>服务名</returns>
        protected virtual string GetParamNeedsService(ParameterInfo arg)
        {
            string needService = Type2Service(arg.ParameterType);
            if (!arg.IsDefined(typeof(InjectAttribute), false))
            {
                return needService;
            }

            InjectAttribute injectAttr = (InjectAttribute)arg.GetCustomAttributes(typeof(InjectAttribute), false)[0];
            if (!string.IsNullOrEmpty(injectAttr.Alias))
            {
                needService = injectAttr.Alias;
            }

            return needService;
        }

        #endregion 依赖注入

        #region IContainer

        /// <summary>
        /// 服务别名
        /// </summary>
        /// <param name="alias">别名</param>
        /// <param name="service">服务名</param>
        /// <returns>容器</returns>
        public IContainer Alias(string alias, string service)
        {
            Checker.NotEmptyOrNull(alias, "alias");
            Checker.NotEmptyOrNull(service, "service");

            if (alias == service)
            {
                throw new RuntimeException("别名和服务名相同:[" + service + "]");
            }

            alias = FormatService(alias);
            service = FormatService(service);
            lock (_syncRoot)
            {
                CheckIsFlusing();

                if (_aliases.ContainsKey(alias))
                {
                    throw new RuntimeException("别名已经存在:[" + alias + "]");
                }

                if (!_binds.ContainsKey(service) && !_instances.ContainsKey(service))
                {
                    throw new RuntimeException("服务[" + service + "]未找到绑定数据,应该先Bind或Instance");
                }

                //添加到别称
                _aliases.Add(alias, service);

                //添加到反向查找列表
                List<string> serviceList;
                if (!_aliasesReverse.TryGetValue(service, out serviceList))
                {
                    serviceList = new List<string>();
                    _aliasesReverse[service] = serviceList;
                }
                serviceList.Add(alias);
            }

            return this;
        }

        /// <summary>
        /// 绑定服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实现</param>
        /// <param name="isStatis">是否是单例</param>
        /// <returns>绑定数据</returns>
        public IBindData Bind(string service, Type concrete, bool isStatis)
        {
            Checker.NotNull(concrete, "concrete");

            if (IsUnableType(concrete))
            {
                throw new RuntimeException("绑定的服务类型[" + concrete + "]不能被实例化");
            }

            Func<IContainer, object[], object> concreteFunc = WrapperTypeBuilder(service, concrete);
            return Bind(service, concreteFunc, isStatis);
        }

        /// <summary>
        /// 绑定服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务构造</param>
        /// <param name="isStatic">是否是单例</param>
        /// <returns>绑定数据</returns>
        public IBindData Bind(string service, Func<IContainer, object[], object> concrete, bool isStatic)
        {
            Checker.NotEmptyOrNull(service, "service");
            Checker.NotNull(concrete, "concrete");

            service = FormatService(service);

            lock (_syncRoot)
            {
                CheckIsFlusing();

                if (_binds.ContainsKey(service))
                {
                    throw new RuntimeException("该服务[" + service + "]已经绑定");
                }

                if (_instances.ContainsKey(service))
                {
                    throw new RuntimeException("单例中存在该服务[" + service + "]实例");
                }

                if (_aliases.ContainsKey(service))
                {
                    throw new RuntimeException("别名中存在该服务[" + service + "]名");
                }

                BindData bindData = new BindData(this, service, concrete, isStatic);
                _binds.Add(service, bindData);

                if (IsResolved(service))
                {
                    if (isStatic)
                    {//如果是单例，则直接解决
                        Resolve(service);
                    }
                    else
                    {
                        TriggerOnRebound(service);
                    }
                }

                return bindData;
            }
        }

        /// <summary>
        /// 调用函数,函数无参时将清空输入参数以完成调用
        /// </summary>
        /// <param name="target">方法对象</param>
        /// <param name="methodInfo">方法信息</param>
        /// <param name="userParams">用户传入的参数</param>
        /// <returns>方法返回值</returns>
        public object Call(object target, MethodInfo methodInfo, params object[] args)
        {
            object[] targetArgs = args;
            //获取函数参数
            ParameterInfo[] argInfos = methodInfo.GetParameters();
            if (0 >= argInfos.Length)
            {//函数为无参,忽略输入的参数
                targetArgs = new object[0];
            }

            try
            {
                return methodInfo.Invoke(target, targetArgs);
            }
            catch (Exception ex)
            {
                string msg = string.Format(
                    "<color=red>事件调用异常:目标对象 ({0}) , 调用函数 ({1}) , 声明类型({2})</color>",
                    target,
                    methodInfo,
                    methodInfo.DeclaringType);
                throw new RuntimeException(msg, ex);
            }
        }

        /// <summary>
        /// 是否可以生成服务
        /// </summary>
        /// <param name="service">服务名或者别名</param>
        /// <returns>是否可以生成服务</returns>
        public bool CanMake(string service)
        {
            Checker.NotEmptyOrNull(service, "service");

            lock (_syncRoot)
            {
                service = AliasToService(service);

                if (HasBind(service) || HasInstance(service))
                {
                    return true;
                }

                Type type = SpeculatedServiceType(service);
                return !IsBasicType(type) && !IsUnableType(type);
            }
        }

        /// <summary>
        /// 清理所有数据
        /// </summary>
        public void Flush()
        {
            lock (_syncRoot)
            {
                try
                {
                    _flushing = true;

                    foreach (string service in _instanceTiming)
                    {
                        Release(service);
                    }

                    Checker.Requires<RuntimeException>(_instances.Count <= 0);

                    _resolving.Clear();
                    _afterResolving.Clear();
                    _release.Clear();
                    _aliases.Clear();
                    _aliasesReverse.Clear();
                    _instances.Clear();
                    _instanceReverse.Clear();
                    _instanceTiming.Clear();
                    _binds.Clear();
                    _resolved.Clear();
                    _instanceId = 0;
                }
                finally
                {
                    _flushing = false;
                }
            }
        }

        /// <summary>
        /// 获取服务的绑定数据,如果绑定不存在则返回null（只有进行过bind才视作绑定）
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>服务绑定数据或者null</returns>
        public IBindData GetBind(string service)
        {
            Checker.NotEmptyOrNull(service, "service");
            lock (_syncRoot)
            {
                service = AliasToService(service);
                BindData bindData = null;
                if (_binds.TryGetValue(service, out bindData))
                {
                }
                return bindData;
            }
        }

        /// <summary>
        /// 存在绑定
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>存在绑定</returns>
        public bool HasBind(string service)
        {
            return GetBind(service) != null;
        }

        /// <summary>
        /// 是否已经实例静态化
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>是否已经静态化</returns>
        public bool HasInstance(string service)
        {
            Checker.NotEmptyOrNull(service, "service");
            lock (_syncRoot)
            {
                service = AliasToService(service);
                return _instances.ContainsKey(service);
            }
        }

        /// <summary>
        /// 静态化一个服务
        /// </summary>
        /// <param name="service">服务名或者别名</param>
        /// <param name="instance">服务实例</param>
        /// <returns>被修饰器处理后的新的实例</returns>
        public object Instance(string service, object instance)
        {
            Checker.NotEmptyOrNull(service, "service");

            lock (_syncRoot)
            {
                CheckIsFlusing();

                //转换到映射名
                service = AliasToService(service);

                //获取绑定
                IBindData bindData = GetBind(service);

                if (null != bindData)
                {
                    if (!bindData.IsStatic)
                    {
                        throw new RuntimeException("该服务[" + service + "]不是单例绑定");
                    }
                }
                else
                {
                    bindData = MakeEmptyBindData(service);
                }

                //调用解决中事件
                instance = TriggerOnResolving((BindData)bindData, instance);

                if (null != instance)
                {
                    string realService;
                    if (_instanceReverse.TryGetValue(instance, out realService))
                    {
                        if (realService != service)
                        {
                            throw new RuntimeException("该实例已经绑定到单例服务[" + realService + "]");
                        }
                    }
                }

                bool isResolved = IsResolved(service);

                //释放单例
                Release(service);

                //添加到单例列表
                _instances.Add(service, instance);

                if (null != instance)
                {//添加到单例反向查找列表
                    _instanceReverse.Add(instance, service);
                }

                if (!_instanceTiming.Contains(service))
                {//添加实例化顺序
                    _instanceTiming.Add(service, _instanceId++);
                }

                if (isResolved)
                {
                    TriggerOnRebound(service, instance);
                }

                return instance;
            }
        }

        /// <summary>
        /// 是否是别名
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>是否是别名</returns>
        public bool IsAlias(string name)
        {
            Checker.NotEmptyOrNull(name, "service");
            name = FormatService(name);
            return _aliases.ContainsKey(name);
        }

        /// <summary>
        /// 服务是否是静态化的,如果服务不存在也将返回false
        /// </summary>
        /// <param name="service">服务名或者别名</param>
        /// <returns>是否是静态化的</returns>
        public bool IsStatic(string service)
        {
            Checker.NotEmptyOrNull(service, "service");
            IBindData bindData = GetBind(service);
            return null != bindData && bindData.IsStatic;
        }

        /// <summary>
        /// 构造服务
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <param name="args">用户传入的参数</param>
        /// <returns>服务实例，如果构造失败那么返回null</returns>
        public object Make(string service, params object[] args)
        {
            return Resolve(service, args);
        }

        /// <summary>
        /// 释放某个静态化实例
        /// </summary>
        /// <param name="service">服务名或别名</param>
        public bool Release(string service)
        {
            Checker.NotEmptyOrNull(service, "service");

            lock (_syncRoot)
            {
                service = AliasToService(service);

                object instance;
                if (!_instances.TryGetValue(service, out instance))
                {//未找到单例实例
                    return false;
                }

                //获取绑定数据
                BindData bindData = GetBindFillable(service);

                //触发单例释放事件
                TriggerOnRelease(bindData, instance);

                if (null != instance)
                {//释放单例
                    DisposeInstance(instance);

                    //移除反向查找列表
                    _instanceReverse.Remove(instance);
                }

                //移除实例列表
                _instances.Remove(service);

                return true;
            }
        }

        /// <summary>
        /// 类型转化为服务名
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>服务名</returns>
        public string Type2Service(Type type)
        {
            return type.ToString();
        }

        /// <summary>
        /// 解绑数据绑定
        /// </summary>
        /// <param name="service">服务名或别名</param>
        public void UnBind(string service)
        {
            service = AliasToService(service);
            IBindData bindData = GetBind(service);
            if (null != bindData)
            {//解绑
                bindData.Unbind();
            }
        }

        /// <summary>
        /// 服务是否已经被解决过
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>是否已经被解决过</returns>
        public bool IsResolved(string service)
        {
            Checker.NotEmptyOrNull(service, "service");
            lock (_syncRoot)
            {
                service = AliasToService(service);
                return _resolved.Contains(service) || _instances.ContainsKey(service);
            }
        }

        /// <summary>
        /// 解决服务时事件之后的回调
        /// </summary>
        /// <param name="closure">解决事件</param>
        /// <returns>服务绑定数据</returns>
        public IContainer OnAfterResolving(Action<IBindData, object> closure)
        {
            AddEvent(closure, _afterResolving);
            return this;
        }

        /// <summary>
        /// 当静态服务被释放时
        /// </summary>
        /// <param name="closure">处理释放时的回调</param>
        /// <returns>当前容器实例</returns>
        public IContainer OnRelease(Action<IBindData, object> closure)
        {
            AddEvent(closure, _release);
            return this;
        }

        /// <summary>
        /// 当服务被解决时，生成的服务会经过注册的回调函数
        /// </summary>
        /// <param name="closure">回调函数</param>
        /// <returns>当前容器对象</returns>
        public IContainer OnResolving(Action<IBindData, object> closure)
        {
            AddEvent(closure, _resolving);
            return this;
        }

        /// <summary>
        /// 当一个已经被解决的服务，发生重定义时触发
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="callback">回调</param>
        /// <returns>服务容器</returns>
        public IContainer OnRebound(string service, Action<object> callback)
        {
            Checker.NotNull(callback, "callback");

            lock (_syncRoot)
            {
                CheckIsFlusing();
                service = AliasToService(service);

                List<Action<object>> list;
                if (!_rebound.TryGetValue(service, out list))
                {
                    list = new List<Action<object>>();
                    _rebound[service] = list;
                }

                list.Add(callback);
            }

            return this;
        }

        #endregion IContainer
    }
}