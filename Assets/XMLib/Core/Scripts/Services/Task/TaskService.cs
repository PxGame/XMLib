using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace XM.Services.Task
{
    /// <summary>
    /// 任务服务
    /// </summary>
    /// <typeparam name="AE">程序入口类型</typeparam>
    public class TaskService : SimpleService<AppEntry, TaskSetting>, IUpdate
    {
        #region 属性

        /// <summary>
        /// 当前线程是否是主线程
        /// </summary>
        public bool IsMainThread { get { return (Thread.CurrentThread.ManagedThreadId == _mainThreadId); } }

        /// <summary>
        /// 主线程ID
        /// </summary>
        private int _mainThreadId = 0;

        /// <summary>
        /// 同步
        /// </summary>
        private readonly object SyncRoot = new object();

        /// <summary>
        /// 任务堆栈
        /// </summary>
        private Stack<ITaskData> _tasks = new Stack<ITaskData>();

        /// <summary>
        /// 一次循环Task处理上限
        /// </summary>
        private int _maxTaskProcess = int.MaxValue;

        #endregion 属性

        #region 重写

        protected override void OnServiceCreate()
        {
            _mainThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        protected override void OnServiceClear()
        {
            Clear();
        }

        #endregion 重写

        #region IUpdate 接口实现

        /// <summary>
        /// 在主线程循环中调用
        /// </summary>
        public void Update()
        {
            lock (SyncRoot)
            {
                try
                {
                    //主线程任务执行
                    ITaskData taskData;
                    int cnt = _maxTaskProcess;//限制执行次数
                    while (0 < cnt && 0 < _tasks.Count)
                    {
                        taskData = _tasks.Pop();
                        taskData.Call();
                        --cnt;
                    }
                    //
                }
                catch (Exception ex)
                {
                    Debug(DebugType.Exception, "主线程循环任务执行异常:{0}", ex);
                }
            }
        }

        #endregion IUpdate 接口实现

        #region 主线程调用

        /// <summary>
        /// 主线程执行
        /// </summary>
        /// <param name="task">任务</param>
        public void RunOnMainThread(ITaskData data)
        {
            lock (SyncRoot)
            {
                _tasks.Push(data);
            }
        }

        /// <summary>
        /// 主线程执行
        /// </summary>
        /// <param name="task">任务</param>
        public void RunOnMainThread(Action task)
        {
            TaskData data = new TaskData(this, task.Target, task.Method, null);
            lock (SyncRoot)
            {
                _tasks.Push(data);
            }
        }

        /// <summary>
        /// 主线程执行
        /// </summary>
        /// <param name="methodTarget">函数目标对象</param>
        /// <param name="methodInfo">函数信息</param>
        /// <param name="resultCallback">结果回调函数</param>
        /// <param name="methodArgs">函数参数</param>
        public void RunOnMainThread(object methodTarget, MethodInfo methodInfo, Action<object> resultCallback, params object[] methodArgs)
        {
            TaskData data = new TaskData(this, methodTarget, methodInfo, resultCallback, methodArgs);
            lock (SyncRoot)
            {
                _tasks.Push(data);
            }
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            lock (SyncRoot)
            {
                _tasks.Clear();
            }
        }

        #endregion 主线程调用

        #region 子线程调用

#if UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID

        /// <summary>
        /// 线程池中执行
        /// </summary>
        /// <param name="task">任务</param>
        /// <returns>是否成功</returns>
        public bool RunOnThreadPool(Action task)
        {
            return ThreadPool.QueueUserWorkItem(RunActionThread, task);
        }

        /// <summary>
        /// 线程池中执行
        /// </summary>
        /// <param name="task">任务</param>
        /// <returns>是否成功</returns>
        public bool RunOnThreadPool(ITaskData task)
        {
            return ThreadPool.QueueUserWorkItem(RunTaskThread, task);
        }

#endif

        /// <summary>
        /// 线程池中执行
        /// </summary>
        /// <param name="methodTarget">函数目标对象</param>
        /// <param name="methodInfo">函数信息</param>
        /// <param name="resultCallback">结果回调函数</param>
        /// <param name="methodArgs">函数参数</param>
        public void RunOnThreadPool(object methodTarget, MethodInfo methodInfo, Action<object> resultCallback, params object[] methodArgs)
        {
            TaskData data = new TaskData(this, methodTarget, methodInfo, resultCallback, methodArgs);
            RunOnThreadPool(data);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="action">任务</param>
        public void RunActionThread(object action)
        {
            ((Action)action)();
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="task">任务</param>
        public void RunTaskThread(object task)
        {
            ((ITaskData)task).Call();
        }

        #endregion 子线程调用
    }
}