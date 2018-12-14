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
        /// <summary>
        /// 绑定数据
        /// </summary>
        private readonly Dictionary<string, IBindData> _binds;

        /// <summary>
        /// 静态服务
        /// </summary>
        private readonly Dictionary<string, object> _instances;

        /// <summary>
        /// 静态服务反转
        /// </summary>
        private readonly Dictionary<object, string> _instanceReverse;

        /// <summary>
        /// 别名
        /// </summary>
        private readonly Dictionary<string, string> _aliases;

        /// <summary>
        /// 别名反转
        /// </summary>
        private readonly Dictionary<string, List<string>> _aliasesReverse;

        /// <summary>
        /// 静态服务构建
        /// </summary>
        private readonly SortList<string, int> _instanceTiming;

        /// <summary>
        /// 静态服务递增ID
        /// </summary>
        private int _instanceId;

        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object _syncRoot;

        /// <summary>
        /// 是否在清空过程中
        /// </summary>
        private bool _flushing;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Container()
        {
            _binds = new Dictionary<string, IBindData>();
            _instances = new Dictionary<string, object>();
            _instanceReverse = new Dictionary<object, string>();
            _aliases = new Dictionary<string, string>();
            _aliasesReverse = new Dictionary<string, List<string>>();
            _instanceTiming = new SortList<string, int>();
            _instanceId = 0;
            _syncRoot = new object();

            //
            _flushing = false;
        }

        /// <summary>
        /// 绑定服务
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <param name="concrete">服务类型</param>
        /// <param name="isStatis">是否静态化</param>
        /// <returns>绑定数据</returns>
        public IBindData Bind(string service, Type concrete, bool isStatis)
        {
            return null;
        }

        /// <summary>
        /// 绑定一个服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实体</param>
        /// <param name="isStatic">服务是否静态化</param>
        /// <returns>服务绑定数据</returns>
        public IBindData Bind(string service, Func<IContainer, object[], object> concrete, bool isStatic)
        {
            return null;
        }

        /// <summary>
        /// 解绑服务
        /// </summary>
        /// <param name="service">服务名或别名</param>
        public void UnBind(string service)
        {
        }

        /// <summary>
        /// 是否已绑定服务
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>是否已绑定服务</returns>
        public bool HasBind(string service)
        {
            return false;
        }

        /// <summary>
        /// 是否可以生成服务
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>是否可以生成服务</returns>
        public bool CanMake(string service)
        {
            return false;
        }

        /// <summary>
        /// 是否是静态服务
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>是否是静态服务</returns>
        public bool IsStatic(string service)
        {
            return false;
        }

        /// <summary>
        /// 是否是别名
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>是否是别名</returns>
        public bool IsAlias(string name)
        {
            return false;
        }

        /// <summary>
        /// 构造服务
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <param name="args">参数</param>
        /// <returns>实例，失败返回null</returns>
        public object Make(string service, params object[] args)
        {
            return null;
        }

        /// <summary>
        /// 静态化一个服务
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <param name="instance">服务实例</param>
        /// <returns>实例</returns>
        public object Instance(string service, object instance)
        {
            return null;
        }

        /// <summary>
        /// 是否已经实例静态化
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>是否已经静态化</returns>
        public bool HasInstance(string service)
        {
            return false;
        }

        /// <summary>
        /// 释放静态化实例
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>是否成功</returns>
        public bool Release(string service)
        {
            return false;
        }

        /// <summary>
        /// 类型转服务名
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>服务名</returns>
        public string Type2Service(Type type)
        {
            return type.ToString();
        }

        /// <summary>
        /// 清理所有数据
        /// </summary>
        public virtual void Flush()
        {
            lock (_syncRoot)
            {
                try
                {
                    _flushing = true;
                }
                finally
                {
                    _flushing = false;
                }
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
    }
}