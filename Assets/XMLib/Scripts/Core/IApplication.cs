/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/11/2018 4:54:03 PM
 */

using System.Reflection;

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
        bool IsMainThread { get; }

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
    }
}