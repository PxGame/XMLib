/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/20/2018 4:27:23 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;

namespace XMLib
{
    /// <summary>
    /// 绑定数据扩展
    /// </summary>
    public static class BindDataExtend
    {
        /// <summary>
        /// 解决服务时触发的回调
        /// </summary>
        /// <param name="bindData">绑定数据</param>
        /// <param name="action">解决事件</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData OnResolving(this IBindData bindData, Action<object> action)
        {
            Checker.Requires<ArgumentNullException>(action != null);
            return bindData.OnResolving((_, instance) =>
            {
                action(instance);
            });
        }

        /// <summary>
        /// 解决服务时触发的回调
        /// </summary>
        /// <param name="bindData">绑定数据</param>
        /// <param name="action">解决事件</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData OnResolving(this IBindData bindData, Action action)
        {
            Checker.Requires<ArgumentNullException>(action != null);
            return bindData.OnResolving((_, instance) =>
            {
                action();
            });
        }

        /// <summary>
        /// 解决服务后触发的回调
        /// </summary>
        /// <param name="bindData">绑定数据</param>
        /// <param name="action">解决事件</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData OnAfterResolving(this IBindData bindData, Action<object> action)
        {
            Checker.Requires<ArgumentNullException>(action != null);
            return bindData.OnAfterResolving((_, instance) =>
            {
                action(instance);
            });
        }

        /// <summary>
        /// 解决服务后触发的回调
        /// </summary>
        /// <param name="bindData">绑定数据</param>
        /// <param name="action">解决事件</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData OnAfterResolving(this IBindData bindData, Action action)
        {
            Checker.Requires<ArgumentNullException>(action != null);
            return bindData.OnAfterResolving((_, instance) =>
            {
                action();
            });
        }

        /// <summary>
        /// 当静态服务被释放时
        /// </summary>
        /// <param name="bindData">绑定数据</param>
        /// <param name="action">处理事件</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData OnRelease(this IBindData bindData, Action<object> action)
        {
            Checker.Requires<ArgumentNullException>(action != null);
            return bindData.OnRelease((_, instance) =>
            {
                action(instance);
            });
        }

        /// <summary>
        /// 当静态服务被释放时
        /// </summary>
        /// <param name="bindData">绑定数据</param>
        /// <param name="action">处理事件</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData OnRelease(this IBindData bindData, Action action)
        {
            Checker.Requires<ArgumentNullException>(action != null);
            return bindData.OnRelease((_, __) =>
            {
                action();
            });
        }
    }
}