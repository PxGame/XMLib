namespace XM.Services.Task
{
    /// <summary>
    /// 任务接口
    /// 可将子类作为一个独立的任务传给TaskService去执行
    /// </summary>
    public interface ITaskData
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <returns></returns>
        void Call();
    }
}