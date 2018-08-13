namespace XM.Services.Task
{
    /// <summary>
    /// 任务接口 可将子类作为一个独立的任务传给TaskService去执行
    /// </summary>
    public interface ITaskData
    {
        /// <summary>
        /// 服务引用
        /// </summary>
        TaskService Service { get; }

        /// <summary>
        /// 执行
        /// </summary>
        void Call();
    }
}