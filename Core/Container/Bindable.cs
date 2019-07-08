/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/17/2018 4:34:11 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;

namespace XMLib
{
    /// <summary>
    /// 可绑定对象
    /// </summary>
    public abstract class Bindable : IBindable
    {
        /// <summary>
        /// 绑定的服务名
        /// </summary>
        public string service { get { return _service; } }

        /// <summary>
        /// 容器
        /// </summary>
        protected readonly Container container;

        /// <summary>
        /// 服务名
        /// </summary>
        private readonly string _service;

        /// <summary>
        /// 是否销毁
        /// </summary>
        private bool _isDestroy;

        /// <summary>
        /// 同步对象
        /// </summary>
        private object _syncRoot;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="service">服务名</param>
        public Bindable(Container container, string service)
        {
            this.container = container;
            _service = service;

            _syncRoot = new object();
            _isDestroy = false;
        }

        /// <summary>
        /// 解除绑定
        /// </summary>
        public void Unbind()
        {
            lock (_syncRoot)
            {
                _isDestroy = true;

                ReleaseBind();
            }
        }

        /// <summary>
        /// 检查是否销毁
        /// </summary>
        protected void CheckIsDestroy()
        {
            if (_isDestroy)
            {
                throw new RuntimeException("当前绑定的数据已经销毁");
            }
        }

        /// <summary>
        /// 解除绑定
        /// </summary>
        protected abstract void ReleaseBind();

        /// <summary>
        /// 转换到字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[{0}](service:{1}, isDestroy:{2}) ", GetType().Name, _service, _isDestroy);
        }
    }
}