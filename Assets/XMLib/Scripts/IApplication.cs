/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/11/2018 4:54:03 PM
 */

using System;

namespace XMLib
{
    /// <summary>
    /// 应用程序接口
    /// </summary>
    public interface IApplication : IDispatcher, IContainer
    {
        /// <summary>
        /// 是否是主线程
        /// </summary>
        bool isMainThread { get; }

        /// <summary>
        /// 终止
        /// </summary>
        void Terminate();

        /// <summary>
        /// 注册服务提供者
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        void Register(IServiceProvider serviceProvider);

        /// <summary>
        /// 服务提供者是否已经注册过
        /// </summary>
        /// <param name="provider">服务提供者</param>
        /// <returns>服务提供者是否已经注册过</returns>
        bool IsRegisted(IServiceProvider serviceProvider);

        /// <summary>
        /// 获取优先级，如果存在方法优先级定义那么优先返回方法的优先级
        /// 如果不存在优先级定义那么返回<c>int.MaxValue</c>
        /// </summary>
        /// <param name="type">获取优先级的类型</param>
        /// <param name="method">获取优先级的调用方法</param>
        /// <returns>优先级</returns>
        int GetPriority(Type type, string method = null);
    }
}