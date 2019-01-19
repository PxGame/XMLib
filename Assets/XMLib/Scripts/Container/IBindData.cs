/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/14/2018 3:39:37 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;

namespace XMLib
{
    /// <summary>
    /// 绑定数据
    /// </summary>
    public interface IBindData
    {
        /// <summary>
        /// 当前绑定的名字
        /// </summary>
        string service { get; }

        /// <summary>
        /// 服务实现
        /// </summary>
        Func<IContainer, object[], object> concrete { get; }

        /// <summary>
        /// 是否是静态服务
        /// </summary>
        bool isStatic { get; }

        /// <summary>
        /// 为服务设定一个别名
        /// </summary>
        /// <typeparam name="T">别名</typeparam>
        /// <returns>服务绑定数据</returns>
        IBindData Alias<T>();

        /// <summary>
        /// 为服务设定一个别名
        /// </summary>
        /// <param name="alias">别名</param>
        /// <returns>服务绑定数据</returns>
        IBindData Alias(string alias);

        /// <summary>
        /// 解决服务时触发的回调
        /// </summary>
        /// <param name="closure">解决事件</param>
        /// <returns>服务绑定数据</returns>
        IBindData OnResolving(Action<IBindData, object> closure);

        /// <summary>
        /// 解决服务时事件之后的回调
        /// </summary>
        /// <param name="closure">解决事件</param>
        /// <returns>服务绑定数据</returns>
        IBindData OnAfterResolving(Action<IBindData, object> closure);

        /// <summary>
        /// 当服务被释放时
        /// </summary>
        /// <param name="closure">处理事件</param>
        /// <returns>服务绑定数据</returns>
        IBindData OnRelease(Action<IBindData, object> closure);

        /// <summary>
        /// 移除绑定
        /// <para>如果进行的是服务绑定 , 那么在解除绑定时如果是静态化物体将会触发释放</para>
        /// </summary>
        void Unbind();
    }
}