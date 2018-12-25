/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/14/2018 5:17:03 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;

namespace XMLib
{
    /// <summary>
    /// 绑定数据
    /// </summary>
    public class BindData : Bindable, IBindData
    {
        /// <summary>
        /// 构造服务
        /// </summary>
        private readonly Func<IContainer, object[], object> _concrete;

        /// <summary>
        /// 是否是单例
        /// </summary>
        private readonly bool _isStatic;

        /// <summary>
        /// 服务构造修饰器
        /// </summary>
        private List<Action<IBindData, object>> _resolving;

        /// <summary>
        /// 在服务构建修饰器之后的修饰器
        /// </summary>
        private List<Action<IBindData, object>> _afterResolving;

        /// <summary>
        /// 服务构造修饰器
        /// </summary>
        private List<Action<IBindData, object>> _release;

        /// <summary>
        /// 同步对象
        /// </summary>
        private object _syncRoot;

        /// <summary>
        /// 服务实现
        /// </summary>
        public Func<IContainer, object[], object> Concrete { get { return _concrete; } }

        /// <summary>
        /// 是否是静态服务
        /// </summary>
        public bool IsStatic { get { return _isStatic; } }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="container">父级容器</param>
        /// <param name="service">服务名</param>
        /// <param name="concrete">构造回调</param>
        /// <param name="isStatic">是否是单例</param>
        public BindData(Container container, string service, Func<IContainer, object[], object> concrete, bool isStatic)
            : base(container, service)
        {
            _concrete = concrete;
            _isStatic = isStatic;

            _syncRoot = new object();
        }

        #region IBindData

        /// <summary>
        /// 为服务设定一个别名
        /// </summary>
        /// <typeparam name="T">别名</typeparam>
        /// <returns>服务绑定数据</returns>
        public IBindData Alias<T>()
        {
            return Alias(Container.Type2Service(typeof(T)));
        }

        /// <summary>
        /// 为服务设定一个别名
        /// </summary>
        /// <param name="alias">别名</param>
        /// <returns>服务绑定数据</returns>
        public IBindData Alias(string alias)
        {
            lock (_syncRoot)
            {
                CheckIsDestroy();
                Checker.NotEmptyOrNull(alias, "alias");

                Container.Alias(alias, Service);
                return this;
            }
        }

        /// <summary>
        /// 解决服务时事件之后的回调
        /// </summary>
        /// <param name="closure">解决事件</param>
        /// <returns>服务绑定数据</returns>
        public IBindData OnAfterResolving(Action<IBindData, object> closure)
        {
            AddEvent(closure, ref _afterResolving);
            return this;
        }

        /// <summary>
        /// 解决服务时触发的回调
        /// </summary>
        /// <param name="closure">解决事件</param>
        /// <returns>服务绑定数据</returns>
        public IBindData OnResolving(Action<IBindData, object> closure)
        {
            AddEvent(closure, ref _resolving);
            return this;
        }

        /// <summary>
        /// 当静态服务被释放时
        /// </summary>
        /// <param name="closure">处理事件</param>
        /// <returns>服务绑定数据</returns>
        public IBindData OnRelease(Action<IBindData, object> closure)
        {
            if (!_isStatic)
            {
                throw new RuntimeException("服务[" + Service + "]不是单例,不能调用OnRelease方法");
            }

            AddEvent(closure, ref _release);
            return this;
        }

        #endregion IBindData

        /// <summary>
        /// 执行服务修饰器
        /// </summary>
        /// <param name="instance">服务实例</param>
        /// <returns>服务实例</returns>
        public object TriggerResolving(object instance)
        {
            return Container.Trigger(this, instance, _resolving);
        }

        /// <summary>
        /// 执行服务修饰器之后的回调
        /// </summary>
        /// <param name="instance">服务实例</param>
        /// <returns>服务实例</returns>
        public object TriggerAfterResolving(object instance)
        {
            return Container.Trigger(this, instance, _afterResolving);
        }

        /// <summary>
        /// 执行服务释放处理器
        /// </summary>
        /// <param name="instance">服务实例</param>
        /// <returns>服务实例</returns>
        public object TriggerRelease(object instance)
        {
            return Container.Trigger(this, instance, _release);
        }

        /// <summary>
        /// 添加事件到列表
        /// </summary>
        /// <param name="closure">事件对象</param>
        /// <param name="list">事件列表</param>
        private void AddEvent(Action<IBindData, object> closure, ref List<Action<IBindData, object>> list)
        {
            Checker.NotNull(closure, "closure");

            lock (_syncRoot)
            {
                CheckIsDestroy();

                if (null == list)
                {//创建数组
                    list = new List<Action<IBindData, object>>();
                }

                list.Add(closure);
            }
        }

        /// <summary>
        /// 转换到字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[{0}]({1},isStatic:{2})", GetType().Name, base.ToString(), _isStatic);
        }

        #region Bindable

        /// <summary>
        /// 释放绑定
        /// </summary>
        protected override void ReleaseBind()
        {
            Container.UnBind(this);
        }

        #endregion Bindable
    }
}