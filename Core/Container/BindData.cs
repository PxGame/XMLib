/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/14/2018 5:17:03 PM
 */

using System;
using System.Collections.Generic;

namespace XMLib
{
    /// <summary>
    /// 绑定数据
    /// </summary>
    public class BindData
    {
        /// <summary>
        /// 构造服务
        /// </summary>
        public Func<Container, object[], object> concrete { get { return _concrete; } }

        /// <summary>
        /// 是否是单例
        /// </summary>
        public bool isStatic { get { return _isStatic; } }

        /// <summary>
        /// 绑定的服务名
        /// </summary>
        public string service { get { return _service; } }

        /// <summary>
        /// 容器
        /// </summary>
        private readonly Container _container;

        /// <summary>
        /// 服务名
        /// </summary>
        private readonly string _service;

        /// <summary>
        /// 构造服务
        /// </summary>
        private readonly Func<Container, object[], object> _concrete;

        /// <summary>
        /// 是否是单例
        /// </summary>
        private readonly bool _isStatic;

        /// <summary>
        /// 服务构造修饰器
        /// </summary>
        private List<Func<BindData, object, object>> _resolving;

        /// <summary>
        /// 服务构造修饰器
        /// </summary>
        private List<Action<BindData, object>> _release;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="container">父级容器</param>
        /// <param name="service">服务名</param>
        /// <param name="concrete">构造回调</param>
        /// <param name="isStatic">是否是单例</param>
        public BindData(Container container, string service, Func<Container, object[], object> concrete, bool isStatic)
        {
            _resolving = new List<Func<BindData, object, object>>();
            _release = new List<Action<BindData, object>>();

            _container = container;
            _service = service;
            _concrete = concrete;
            _isStatic = isStatic;
        }

        /// <summary>
        /// 解除绑定
        /// </summary>
        public void Unbind()
        {
            _container.Unbind(this);
        }

        /// <summary>
        /// 为服务设定一个别名
        /// </summary>
        /// <typeparam name="T">别名</typeparam>
        /// <returns>服务绑定数据</returns>
        public BindData Alias<T>()
        {
            return Alias(_container.Type2Service(typeof(T)));
        }

        /// <summary>
        /// 为服务设定一个别名
        /// </summary>
        /// <param name="alias">别名</param>
        /// <returns>服务绑定数据</returns>
        public BindData Alias(string alias)
        {
            _container.Alias(alias, service);
            return this;
        }

        /// <summary>
        /// 解决服务时触发的回调
        /// </summary>
        /// <param name="onCallback">解决事件</param>
        /// <returns>服务绑定数据</returns>
        public BindData OnResolving(Func<BindData, object, object> onCallback)
        {
            if (_resolving == null)
            {
                _resolving = new List<Func<BindData, object, object>>();
            }
            _resolving.Add(onCallback);

            return this;
        }

        /// <summary>
        /// 当静态服务被释放时
        /// </summary>
        /// <param name="onCallback">处理事件</param>
        /// <returns>服务绑定数据</returns>
        public BindData OnRelease(Action<BindData, object> onCallback)
        {
            if (!_isStatic)
            {
                throw new RuntimeException("服务不是单例,不能调用OnRelease方法 > {0}", service);
            }

            if (_release == null)
            {
                _release = new List<Action<BindData, object>>();
            }
            _release.Add(onCallback);

            return this;
        }

        /// <summary>
        /// 执行服务修饰器
        /// </summary>
        /// <param name="instance">服务实例</param>
        /// <returns>服务实例</returns>
        public object TriggerOnResolving(object instance)
        {
            if (_resolving == null)
            {
                return instance;
            }

            foreach (var func in _resolving)
            {
                instance = func.Invoke(this, instance);
            }

            return instance;
        }

        /// <summary>
        /// 执行服务释放处理器
        /// </summary>
        /// <param name="instance">服务实例</param>
        /// <returns>服务实例</returns>
        public void TriggerOnRelease(object instance)
        {
            if (_release == null)
            {
                return;
            }

            foreach (var func in _release)
            {
                func.Invoke(this, instance);
            }
        }
    }
}