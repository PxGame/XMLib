using System;

namespace XM.Services
{
    /// <summary>
    /// 任务数据接口
    /// </summary>
    public interface ITaskData : IMethodData
    {
        /// <summary>
        /// 方法参数
        /// </summary>
        object[] MethodArgs { get; }

        /// <summary>
        /// 数据结果返回
        /// </summary>
        Action<Object> ResultCallback { get; }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <returns></returns>
        object Call();
    }
}