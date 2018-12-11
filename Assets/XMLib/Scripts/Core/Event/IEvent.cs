/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/11/2018 5:08:13 PM
 */

namespace XMLib
{
    /// <summary>
    /// 事件对象
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// 事件名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 事件分组
        /// </summary>
        object Group { get; }

        /// <summary>
        /// 调用事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="args">参数</param>
        /// <returns>结果</returns>
        object Call(string eventName, params object[] args);
    }
}