/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/14/2018 5:17:03 PM
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace XMLib
{
    /// <summary>
    /// 容器
    /// </summary>
    public class Container
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
        private readonly Dictionary<object, string> _instancesReverse;

        /// <summary>
        /// 静态服务
        /// </summary>
        private readonly Dictionary<string, object> _instances;

        /// <summary>
        /// 静态服务构建
        /// </summary>
        private readonly SortList<string, int> _instancesTiming;

        /// <summary>
        /// 解决过的服务
        /// </summary>
        private readonly HashSet<string> _resolved;

        /// <summary>
        /// 编译堆栈
        /// </summary>
        private readonly Stack<string> _buildStack;

        /// <summary>
        /// 静态服务递增ID
        /// </summary>
        private int _instanceId;

        /// <summary>
        /// 服务构造修饰器
        /// </summary>
        private readonly List<Action<BindData, object>> _release;

        /// <summary>
        /// 服务构造修饰器
        /// </summary>
        private readonly List<Func<BindData, object, object>> _resolving;

        #endregion 参数

        #region 公开

        public Container()
        {
            _binds = new Dictionary<string, BindData>();
            _instances = new Dictionary<string, object>();
            _instancesReverse = new Dictionary<object, string>();
            _aliases = new Dictionary<string, string>();
            _aliasesReverse = new Dictionary<string, List<string>>();
            _instancesTiming = new SortList<string, int>();
            _instancesTiming.forward = false;

            _buildStack = new Stack<string>();

            _resolving = new List<Func<BindData, object, object>>();
            _release = new List<Action<BindData, object>>();

            _resolved = new HashSet<string>();

            _instanceId = 0;
        }

        /// <summary>
        /// 解绑服务
        /// </summary>
        /// <param name="service">服务名</param>
        public void Unbind(string service)
        {
            service = AliasToService(service);
            BindData bindData = GetBind(service);
            if (null != bindData)
            { //解绑
                bindData.Unbind();
            }
        }

        /// <summary>
        /// 解绑服务
        /// </summary>
        /// <param name="bindData">绑定实例</param>
        public void Unbind(BindData bindData)
        {
            Release(bindData.service);

            List<string> serviceList;
            if (_aliasesReverse.TryGetValue(bindData.service, out serviceList))
            {
                foreach (string alias in serviceList)
                {
                    _aliases.Remove(alias);
                }
                //移除反向查找列表
                _aliasesReverse.Remove(bindData.service);
            }

            _binds.Remove(bindData.service);
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
        /// 调用函数,函数无参时将清空输入参数以完成调用
        /// </summary>
        /// <param name="target">方法对象</param>
        /// <param name="methodInfo">方法信息</param>
        /// <param name="args">用户传入的参数</param>
        /// <returns>方法返回值</returns>
        public object Call(object target, MethodInfo methodInfo, params object[] args)
        {
            if (!methodInfo.IsStatic && null == target)
            {
                throw new RuntimeException("非静态函数调用必须指定调用实例");
            }

            ParameterInfo[] argInfos = methodInfo.GetParameters();
            args = GetDependcencies(argInfos, args);
            return methodInfo.Invoke(target, args);
        }

        /// <summary>
        /// 服务别名
        /// </summary>
        /// <param name="alias">别名</param>
        /// <param name="service">服务名</param>
        /// <returns>容器</returns>
        public Container Alias(string alias, string service)
        {
            if (alias == service)
            {
                throw new RuntimeException("别名和服务名相同 > {0}", service);
            }

            alias = FormatService(alias);
            service = FormatService(service);

            if (_aliases.ContainsKey(alias))
            {
                throw new RuntimeException("别名已经存在 > {0}", alias);
            }

            if (!_binds.ContainsKey(service) && !_instances.ContainsKey(service))
            {
                throw new RuntimeException("服务未找到绑定数据,应该先Bind或Instance > {0}", service);
            }

            _aliases.Add(alias, service);

            List<string> serviceList;
            if (!_aliasesReverse.TryGetValue(service, out serviceList))
            {
                serviceList = new List<string>();
                _aliasesReverse[service] = serviceList;
            }
            serviceList.Add(alias);

            return this;
        }

        /// <summary>
        /// 绑定服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="serviceType">服务实现</param>
        /// <param name="isStatis">是否是单例</param>
        /// <returns>绑定数据</returns>
        public BindData Bind(string service, Type serviceType, bool isStatic)
        {
            if (IsUnableType(serviceType))
            {
                throw new RuntimeException("绑定的服务不能被实例化 > {0}", service);
            }

            Func<Container, object[], object> concreteFunc = WrapperTypeBuilder(service, serviceType);

            return Bind(service, concreteFunc, isStatic);
        }

        /// <summary>
        /// 绑定服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务构造</param>
        /// <param name="isStatic">是否是单例</param>
        /// <returns>绑定数据</returns>
        public BindData Bind(string service, Func<Container, object[], object> concrete, bool isStatic)
        {
            service = FormatService(service);

            if (_binds.ContainsKey(service))
            {
                throw new RuntimeException("服务已经绑定 > {0}", service);
            }

            if (_instances.ContainsKey(service))
            {
                throw new RuntimeException("服务已经绑定单例 > {0}", service);
            }

            if (_aliases.ContainsKey(service))
            {
                throw new RuntimeException("服务已存在别名中 > {0}", service);
            }

            BindData bindData = new BindData(this, service, concrete, isStatic);
            _binds.Add(service, bindData);

            if (IsResolved(service))
            {
                if (isStatic)
                {
                    Resolve(service);
                }
                else
                {
                    TriggerOnRebound(service);
                }
            }

            return bindData;
        }

        /// <summary>
        /// 如果服务不存在那么则绑定服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实现</param>
        /// <param name="isStatic">服务是否是静态的</param>
        /// <param name="bindData">如果绑定失败则返回历史绑定对象</param>
        /// <returns>是否成功绑定</returns>
        public bool BindIf(string service, Func<Container, object[], object> concrete, bool isStatic, out BindData bindData)
        {
            var bind = GetBind(service);
            if (bind == null && (HasInstance(service) || IsAlias(service)))
            {
                bindData = null;
                return false;
            }
            bindData = bind ?? Bind(service, concrete, isStatic);
            return bind == null;
        }

        /// <summary>
        /// 如果服务不存在那么则绑定服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实现</param>
        /// <param name="isStatic">服务是否是静态的</param>
        /// <param name="bindData">如果绑定失败则返回历史绑定对象</param>
        /// <returns>是否成功绑定</returns>
        public bool BindIf(string service, Type concrete, bool isStatic, out BindData bindData)
        {
            if (!IsUnableType(concrete))
            {
                service = FormatService(service);
                return BindIf(service, WrapperTypeBuilder(service, concrete), isStatic, out bindData);
            }

            bindData = null;
            return false;
        }

        /// <summary>
        /// 释放某个静态化实例
        /// </summary>
        /// <param name="mixed">服务名或实例</param>
        public bool Release(object mixed)
        {
            if (mixed == null)
            {
                return false;
            }

            string service;
            object instance = null;

            if (!(mixed is string))
            {
                service = GetServiceWithInstanceObject(mixed);
            }
            else
            {
                service = AliasToService((string)mixed);

                if (!_instances.TryGetValue(service, out instance))
                {
                    service = GetServiceWithInstanceObject(mixed);
                }
            }

            if (instance == null &&
                (string.IsNullOrEmpty(service) || !_instances.TryGetValue(service, out instance)))
            {
                return false;
            }

            BindData bindData = GetBindFillable(service);
            TriggerOnRelease(bindData, instance);

            if (null != instance)
            {
                if (instance is IDisposable)
                {
                    ((IDisposable)instance).Dispose();
                }

                _instancesReverse.Remove(instance);
            }

            _instances.Remove(service);

            _instancesTiming.Remove(service);

            return true;
        }

        /// <summary>
        /// 清理所有数据
        /// </summary>
        public void Flush()
        {
            try
            {
                foreach (string service in _instancesTiming)
                {
                    Release(service);
                }

                _resolving.Clear();
                _release.Clear();
                _aliases.Clear();
                _aliasesReverse.Clear();
                _instances.Clear();
                _instancesReverse.Clear();
                _instancesTiming.Clear();
                _buildStack.Clear();
                _binds.Clear();
                _resolved.Clear();
                _instanceId = 0;
            }
            finally
            {
            }
        }

        /// <summary>
        /// 服务是否已经被解决过
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>是否已经被解决过</returns>
        public bool IsResolved(string service)
        {
            service = AliasToService(service);
            return _resolved.Contains(service) || _instances.ContainsKey(service);
        }

        /// <summary>
        /// 获取最终服务名
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>获取最终服务名</returns>
        public string AliasToService(string service)
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
        /// 是否可以生成服务
        /// </summary>
        /// <param name="service">服务名或者别名</param>
        /// <returns>是否可以生成服务</returns>
        public bool CanMake(string service)
        {
            service = AliasToService(service);

            if (HasBind(service) || HasInstance(service))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 是否已经实例静态化
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>是否已经静态化</returns>
        public bool HasInstance(string service)
        {
            service = AliasToService(service);
            return _instances.ContainsKey(service);
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
        /// 获取服务的绑定数据,如果绑定不存在则返回null（只有进行过bind才视作绑定）
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>服务绑定数据或者null</returns>
        public BindData GetBind(string service)
        {
            service = AliasToService(service);
            BindData bindData = null;
            if (_binds.TryGetValue(service, out bindData)) { }
            return bindData;
        }

        /// <summary>
        /// 是否是别名
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>是否是别名</returns>
        public bool IsAlias(string name)
        {
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
            BindData bindData = GetBind(service);
            return null != bindData && bindData.isStatic;
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

        #endregion 公开

        #region 事件

        /// <summary>
        /// 当静态服务被释放时
        /// </summary>
        /// <param name="onCallback">处理释放时的回调</param>
        /// <returns>当前容器实例</returns>
        public Container OnRelease(Action<BindData, object> onCallback)
        {
            _release.Add(onCallback);
            return this;
        }

        /// <summary>
        /// 当服务被解决时，生成的服务会经过注册的回调函数
        /// </summary>
        /// <param name="onCallback">回调函数</param>
        /// <returns>当前容器对象</returns>
        public Container OnResolving(Func<BindData, object, object> onCallback)
        {
            _resolving.Add(onCallback);
            return this;
        }

        #endregion 事件

        #region 私有

        /// <summary>
        /// 触发服务重定义事件
        /// </summary>
        /// <param name="service">发生重定义的服务</param>
        /// <param name="instance">服务实例（如果为空将会从容器请求）</param>
        private void TriggerOnRebound(string service, object instance = null)
        {//重定向,目前不需要处理
        }

        /// <summary>
        /// 触发全局解决修饰器
        /// </summary>
        /// <param name="bindData">服务绑定数据</param>
        /// <param name="instance">服务实例</param>
        /// <returns>被修饰器修饰后的服务实例</returns>
        private object TriggerOnResolving(BindData bindData, object instance)
        {
            instance = bindData.TriggerOnResolving(instance);

            foreach (var func in _resolving)
            {
                instance = func.Invoke(bindData, instance);
            }

            return instance;
        }

        /// <summary>
        /// 触发全局释放修饰器
        /// </summary>
        /// <param name="bindData">服务绑定数据</param>
        /// <param name="instance">服务实例</param>
        /// <returns>被修饰器修饰后的服务实例</returns>
        private void TriggerOnRelease(BindData bindData, object instance)
        {
            bindData.TriggerOnRelease(instance);

            foreach (var func in _release)
            {
                func.Invoke(bindData, instance);
            }
        }

        /// <summary>
        /// 获取静态服务服务名
        /// </summary>
        /// <param name="instance">静态服务实例</param>
        /// <returns>服务名</returns>
        private string GetServiceWithInstanceObject(object instance)
        {
            string service = null;
            if (_instancesReverse.TryGetValue(instance, out service))
            {
            }

            return service;
        }

        /// <summary>
        /// 解决服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="args">参数</param>
        /// <returns>服务实例</returns>
        private object Resolve(string service, params object[] args)
        {
            service = AliasToService(service);

            object instance;

            if (_instances.TryGetValue(service, out instance))
            {
                return instance;
            }

            if (_buildStack.Contains(service))
            {
                throw new RuntimeException("服务出现循环依赖 > {0}", service);
            }

            _buildStack.Push(service);

            try
            {
                BindData bindData = GetBindFillable(service);

                instance = Build(bindData, args);

                if (bindData.isStatic)
                {
                    instance = Instance(bindData.service, instance);
                }
                else
                {
                    instance = TriggerOnResolving(bindData, instance);
                }

                _resolved.Add(service);

                return instance;
            }
            finally
            {
                _buildStack.Pop();
            }
        }

        /// <summary>
        /// 静态化一个服务
        /// </summary>
        /// <param name="service">服务名或者别名</param>
        /// <param name="instance">服务实例</param>
        /// <returns>被修饰器处理后的新的实例</returns>
        private object Instance(string service, object instance)
        {
            service = AliasToService(service);

            BindData bindData = GetBind(service);

            if (null != bindData)
            {
                if (!bindData.isStatic)
                {
                    throw new RuntimeException("服务不是单例绑定 > {0}", service);
                }
            }
            else
            {
                bindData = MakeEmptyBindData(service);
            }

            instance = TriggerOnResolving(bindData, instance);

            if (null != instance)
            {
                string realService;
                if (_instancesReverse.TryGetValue(instance, out realService))
                {
                    if (0 != string.Compare(realService, service))
                    {
                        throw new RuntimeException("实例已经绑定到单例服务 > {0}", realService);
                    }
                }
            }

            bool isResolved = IsResolved(service);

            Release(service);

            _instances.Add(service, instance);

            if (null != instance)
            {
                _instancesReverse.Add(instance, service);
            }

            if (!_instancesTiming.Contains(service))
            {
                _instancesTiming.Add(service, _instanceId++);
            }

            if (isResolved)
            {
                TriggerOnRebound(service, instance);
            }

            return instance;
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

            if (null == bindData.concrete)
            {
                throw new RuntimeException("未设置服务构建方法  > {0}", bindData.service);
            }

            instance = bindData.concrete(this, args);

            return instance;
        }

        /// <summary>
        /// 获取服务对应的绑定数据，如果没有则创建一个空的
        /// </summary>
        /// <param name="service">服务名</param>
        /// <returns>绑定数据</returns>
        private BindData GetBindFillable(string service)
        {
            BindData bindData = null;
            if (string.IsNullOrEmpty(service) || !_binds.TryGetValue(service, out bindData))
            {
                bindData = MakeEmptyBindData(service);
            }

            return bindData;
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
        /// 格式化服务名
        /// </summary>
        /// <param name="service">服务名</param>
        /// <returns>格式化服务名</returns>
        private string FormatService(string service)
        {
            return service.Trim();
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
        /// 包装一个类型，可以被用来生成服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="serviceType">类型</param>
        /// <returns>根据类型生成的服务</returns>
        private Func<Container, object[], object> WrapperTypeBuilder(string service, Type serviceType)
        {
            service = FormatService(service);
            return (container, args) =>
            {
                return container.CreateInstance(GetBindFillable(service), serviceType, args);
            };
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
                throw new RuntimeException("服务不能被实例化 > {0}", bindData.service);
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
                throw new RuntimeException(ex, "服务实例化失败 > {0}", bindData.service);
            }
        }

        /// <summary>
        /// 获取构造函数依赖
        /// </summary>
        /// <param name="bindData">服务绑定数据</param>
        /// <param name="serviceType">服务类型</param>
        /// <param name="args">用户传入参数</param>
        /// <returns>参数列表</returns>
        private object[] GetConstructorsInjectParams(BindData bindData, Type serviceType, object[] args)
        {
            ConstructorInfo[] constructors = serviceType.GetConstructors();
            if (constructors.Length <= 0)
            {
                return null;
            }

            Exception exception = null;
            foreach (ConstructorInfo info in constructors)
            {
                try
                {
                    return GetDependcencies(info.GetParameters(), args);
                }
                catch (Exception ex)
                {
                    if (exception == null)
                    {
                        exception = ex;
                    }
                }
            }

            throw new RuntimeException(exception, "获取服务构造函数参数失败 > {0}", bindData.service);
        }

        private object[] GetDependcencies(ParameterInfo[] infos, object[] args)
        {
            if (infos.Length == 0)
            {
                return null;
            }

            object[] results = new object[infos.Length];

            int length = results.Length;

            for (int i = 0; i < length; i++)
            {
                ParameterInfo info = infos[i];

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
                    string needService = GetParamNeedsService(info);

                    if (info.ParameterType.IsClass ||
                        info.ParameterType.IsInterface)
                    {
                        arg = ResolveClass(needService, info);
                    }
                    else
                    {
                        arg = ResolvePrimitive(needService, info);
                    }
                }

                if (!CanInject(info.ParameterType, arg))
                {
                    throw new RuntimeException("未匹配到参数 > {0}", info.ParameterType);
                }

                results[i] = arg;
            }

            return results;
        }

        /// <summary>
        /// 解决基本类型
        /// </summary>
        /// <param name="bindData">绑定数据</param>
        /// <param name="service">服务名</param>
        /// <param name="info">参数信息</param>
        /// <returns></returns>
        private object ResolvePrimitive(string service, ParameterInfo info)
        {
            if (CanMake(service))
            { //可以创建
                return Make(service);
            }

            //获取默认值，不一定有效，
            //在.net 4.7.2版本中有HasDefaultValue的方法可以判断
            if (info.IsOptional)
            {
                return info.DefaultValue;
            }

            throw new RuntimeException("服务参数为不支持的基本类型 > {0}", service);
        }

        /// <summary>
        /// 解决类类型
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="info">参数信息</param>
        /// <returns>结果</returns>
        private object ResolveClass(string service, ParameterInfo info)
        {
            return Make(service);
        }

        /// <summary>
        /// 获取参数需求的服务
        /// </summary>
        /// <param name="info">参数</param>
        /// <returns>服务名</returns>
        private string GetParamNeedsService(ParameterInfo info)
        {
            string needService = Type2Service(info.ParameterType);

            return needService;
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
            {
                object arg = args[i];

                if (ChangeType(ref arg, info.ParameterType))
                { //转换成功
                    ArrayList arrayOpt = new ArrayList(args);
                    arrayOpt.RemoveAt(i);//移除可用参数列表
                    args = arrayOpt.ToArray();
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
            { //忽略该异常
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

            if (typeof(object) == info.ParameterType &&
                null != result &&
                1 == result.Length)
            { //返回object
                return result[0];
            }

            //返回 object[]
            return result;
        }

        /// <summary>
        /// 检查紧缩注入参数
        /// </summary>
        /// <param name="info">参数信息</param>
        /// <param name="args">参数列表</param>
        /// <returns></returns>
        private bool CheckCompactInjectUserParams(ParameterInfo info, object[] args)
        {
            if (null == args || args.Length <= 0)
            {
                return false;
            }

            return info.ParameterType == typeof(object[]) ||
                info.ParameterType == typeof(object);
        }

        private bool CanInject(Type type, object instance)
        {
            return null != instance || type.IsInstanceOfType(instance);
        }

        #endregion 私有

        #region 扩展

        public string Type2Service<TService>()
        {
            return Type2Service(typeof(TService));
        }

        public object Instance<TService>(object instance)
        {
            return Instance(Type2Service(typeof(TService)), instance);
        }

        public bool Release<TService>()
        {
            return Release(Type2Service(typeof(TService)));
        }

        public bool Release(ref object[] instances, bool reverse = true)
        {
            if (instances == null || instances.Length <= 0)
            {
                return true;
            }

            if (reverse)
            {
                Array.Reverse(instances);
            }

            var errorIndex = 0;

            for (var index = 0; index < instances.Length; index++)
            {
                if (instances[index] == null)
                {
                    continue;
                }

                if (!Release(instances[index]))
                {
                    instances[errorIndex++] = instances[index];
                }
            }

            Array.Resize(ref instances, errorIndex);

            if (reverse && errorIndex > 0)
            {
                Array.Reverse(instances);
            }

            return errorIndex <= 0;
        }

        public BindData GetBind<TService>()
        {
            return GetBind(Type2Service(typeof(TService)));
        }

        public void Unbind<TService>()
        {
            Unbind(Type2Service(typeof(TService)));
        }

        public bool HasBind<TService>()
        {
            return HasBind(Type2Service(typeof(TService)));
        }

        public bool HasIntstance<TService>()
        {
            return HasInstance(Type2Service(typeof(TService)));
        }

        public bool IsResolved<TService>()
        {
            return IsResolved(Type2Service(typeof(TService)));
        }

        public bool CanMake<TService>()
        {
            return CanMake(Type2Service(typeof(TService)));
        }

        public bool IsStatic<TService>()
        {
            return IsStatic(Type2Service(typeof(TService)));
        }

        public bool IsAlias<TService>()
        {
            return IsAlias(Type2Service(typeof(TService)));
        }

        public Container Alias<TAlias, TService>()
        {
            return Alias(
                Type2Service(typeof(TAlias)),
                Type2Service(typeof(TService))
                );
        }

        #region Bind

        public BindData Bind<TService>()
        {
            return Bind(
                Type2Service(typeof(TService)),
                typeof(TService),
                false);
        }

        public BindData Bind<TService, TConcrete>()
        {
            return Bind(
                Type2Service(typeof(TService)),
                typeof(TConcrete),
                false);
        }

        public BindData Bind<TService>(Func<Container, object[], object> concrete)
        {
            return Bind(
                Type2Service(typeof(TService)),
                concrete,
                false);
        }

        public BindData Bind<TService>(Func<object> concrete)
        {
            return Bind(
                Type2Service(typeof(TService)),
                (c, p) => concrete.Invoke(),
                false);
        }

        public BindData Bind(string service, Func<Container, object[], object> concrete)
        {
            return Bind(service, concrete, false);
        }

        public bool BindIf<TService, TConcrete>(out BindData bindData)
        {
            return BindIf(Type2Service(typeof(TService)), typeof(TConcrete), false, out bindData);
        }

        public bool BindIf<TService>(out BindData bindData)
        {
            return BindIf(Type2Service(typeof(TService)), typeof(TService), false, out bindData);
        }

        public bool BindIf<TService>(Func<Container, object[], object> concrete, out BindData bindData)
        {
            return BindIf(Type2Service(typeof(TService)), concrete, false, out bindData);
        }

        public bool BindIf<TService>(Func<object[], object> concrete, out BindData bindData)
        {
            return BindIf(Type2Service(typeof(TService)), (c, args) => concrete(args), false,
                out bindData);
        }

        public bool BindIf<TService>(Func<object> concrete, out BindData bindData)
        {
            return BindIf(Type2Service(typeof(TService)), (c, p) => concrete.Invoke(), false,
                out bindData);
        }

        public bool BindIf(string service,
            Func<Container, object[], object> concrete, out BindData bindData)
        {
            return BindIf(service, concrete, false, out bindData);
        }

        #endregion Bind

        #region Singleton

        public BindData Singleton(string service,
            Func<Container, object[], object> concrete)
        {
            return Bind(service, concrete, true);
        }

        public BindData Singleton<TService, TConcrete>()
        {
            return Bind(Type2Service(typeof(TService)), typeof(TConcrete), true);
        }

        public BindData Singleton<TService>()
        {
            return Bind(Type2Service(typeof(TService)), typeof(TService), true);
        }

        public BindData Singleton<TService>(Func<Container, object[], object> concrete)
        {
            return Bind(Type2Service(typeof(TService)), concrete, true);
        }

        public BindData Singleton<TService>(Func<object[], object> concrete)
        {
            return Bind(Type2Service(typeof(TService)), (c, p) => concrete.Invoke(p), true);
        }

        public BindData Singleton<TService>(Func<object> concrete)
        {
            return Bind(Type2Service(typeof(TService)), (c, p) => concrete.Invoke(), true);
        }

        public bool SingletonIf<TService, TConcrete>(out BindData bindData)
        {
            return BindIf(Type2Service(typeof(TService)), typeof(TConcrete), true, out bindData);
        }

        public bool SingletonIf<TService>(out BindData bindData)
        {
            return BindIf(Type2Service(typeof(TService)), typeof(TService), true, out bindData);
        }

        public bool SingletonIf<TService>(Func<Container, object[], object> concrete, out BindData bindData)
        {
            return BindIf(Type2Service(typeof(TService)), concrete, true, out bindData);
        }

        public bool SingletonIf<TService>(Func<object> concrete, out BindData bindData)
        {
            return BindIf(Type2Service(typeof(TService)), (c, p) => concrete.Invoke(), true,
                out bindData);
        }

        public bool SingletonIf<TService>(Func<object[], object> concrete, out BindData bindData)
        {
            return BindIf(Type2Service(typeof(TService)), (c, args) => concrete(args), true,
                out bindData);
        }

        public bool SingletonIf(string service,
            Func<Container, object[], object> concrete, out BindData bindData)
        {
            return BindIf(service, concrete, true, out bindData);
        }

        #endregion Singleton

        #region Make

        public TService Make<TService>(params object[] userParams)
        {
            return (TService)Make(Type2Service(typeof(TService)), userParams);
        }

        public object Make(Type type, params object[] userParams)
        {
            BindData binder;
            var service = Type2Service(type);
            BindIf(service, type, false, out binder);
            return Make(service, userParams);
        }

        #endregion Make

        #region Event

        public Container OnRelease(Action<object> callback)
        {
            return OnRelease((_, instance) => callback(instance));
        }

        public Container OnRelease<T>(Action<T> closure)
        {
            return OnRelease((_, instance) =>
            {
                if (instance is T)
                {
                    closure((T)instance);
                }
            });
        }

        public Container OnRelease<T>(Action<BindData, T> closure)
        {
            return OnRelease((bindData, instance) =>
            {
                if (instance is T)
                {
                    closure(bindData, (T)instance);
                }
            });
        }

        public Container OnResolving(Action<object> callback)
        {
            return OnResolving((_, instance) =>
            {
                callback(instance);
                return instance;
            });
        }

        public Container OnResolving<T>(Action<T> closure)
        {
            return OnResolving((_, instance) =>
            {
                if (instance is T)
                {
                    closure((T)instance);
                }
                return instance;
            });
        }

        public Container OnResolving<T>(Action<BindData, T> closure)
        {
            return OnResolving((bindData, instance) =>
            {
                if (instance is T)
                {
                    closure(bindData, (T)instance);
                }
                return instance;
            });
        }

        #endregion Event

        #endregion 扩展
    }
}