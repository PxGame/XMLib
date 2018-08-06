namespace XM.Services.Event
{
    /// <summary>
    /// 事件监听接口
    /// </summary>
    public interface IEventData : IMethodData
    {
        /// <summary>
        /// 事件名
        /// </summary>
        string EventName { get; }

        /// <summary>
        /// 事件调用
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        object Call(params object[] args);

        /// <summary>
        /// 最大调用次数
        /// </summary>
        int InvokeMaxCount { get; }

        /// <summary>
        /// 当前调用次数
        /// </summary>
        int CurrentInvokeCount { get; }

        /// <summary>
        /// 检查是否有效
        /// </summary>
        /// <returns></returns>
        bool IsVaild();

        /// <summary>
        /// 需要移除
        /// </summary>
        /// <returns></returns>
        bool CheckRemove();
    }
}