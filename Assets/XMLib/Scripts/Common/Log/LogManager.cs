/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/26/2018 9:53:19 AM
 */

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using System.Threading;
using System.Text;

namespace XMLib.Log
{
    /// <summary>
    /// 日志管理器
    /// </summary>
    [DefaultExecutionOrder(-10000)]
    public class LogManager : MonoBehaviour
    {
        /// <summary>
        /// 日志消息
        /// </summary>
        private class LogMessage
        {
            /// <summary>
            /// 日志内容
            /// </summary>
            public string condition { get; protected set; }

            /// <summary>
            /// 日志堆栈
            /// </summary>
            public string stackTrace { get; protected set; }

            /// <summary>
            /// 日志类型
            /// </summary>
            public LogType type { get; protected set; }

            /// <summary>
            /// 构造
            /// </summary>
            /// <param name="type">类型</param>
            /// <param name="condition">消息</param>
            /// <param name="stackTrace">堆栈</param>
            public LogMessage(LogType type, string condition, string stackTrace)
            {
                this.type = type;
                this.condition = condition;
                this.stackTrace = stackTrace;
            }
        }

        /// <summary>
        /// 输出文件目录路径
        /// </summary>
        private string _outFolderPath;

        /// <summary>
        /// 文件句柄
        /// </summary>
        private FileStream _fileHandle;

        /// <summary>
        /// 线程句柄
        /// </summary>
        private Thread _workThread;

        /// <summary>
        /// 信息队列
        /// </summary>
        private Queue<LogMessage> _queues;

        /// <summary>
        /// 线程结束等待时间
        /// </summary>
        private int _stopThreadTimer = 100;

        /// <summary>
        /// 线程同步句柄
        /// </summary>
        private EventWaitHandle _threadWaitHandle;

        /// <summary>
        /// 同步
        /// </summary>
        private object _syncRoot;

        /// <summary>
        /// 是否初始化
        /// </summary>
        private bool _isRunning;

        /// <summary>
        /// 缓冲区日志最大条数限制
        /// </summary>
        [SerializeField]
        private int _maxPoolSize = 100;

        #region Mono

        private void Awake()
        {
            _outFolderPath = (UnityEngine.Application.temporaryCachePath + "/Log").Replace("\\", "/").Replace("//", "/");
            Debug.LogFormat("LogManager 输出目录:{0}", _outFolderPath);

            Begin();
        }

        private void OnDestroy()
        {
            End();
        }

        #endregion Mono

        /// <summary>
        /// 构造函数
        /// </summary>
        public LogManager()
        {
            _isRunning = false;
            _syncRoot = new object();
            _queues = new Queue<LogMessage>();
            _threadWaitHandle = null;
        }

        /// <summary>
        /// 开始
        /// </summary>
        private void Begin()
        {
            string fileName = Guid.NewGuid().ToString() + ".log";
            string filePath = Path.Combine(_outFolderPath, fileName).Replace("\\", "/");

            try
            {
                if (!Directory.Exists(_outFolderPath))
                {
                    Directory.CreateDirectory(_outFolderPath);
                }

                //创建文件

                _fileHandle = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read);

                //启动处理线程
                _workThread = new Thread(OnWorkLoop);

                _workThread.IsBackground = true;

                _threadWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
                _workThread.Start(_threadWaitHandle);

                UnityEngine.Application.logMessageReceivedThreaded += OnLogReceived;

                _isRunning = true;
            }
            catch (Exception ex)
            {
                Debug.LogWarningFormat("LogManager 启动异常:{0}\n{1}", filePath, ex);
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="forceStopThread"></param>
        private void End(bool forceStopThread = false)
        {
            try
            {
                _isRunning = false;

                UnityEngine.Application.logMessageReceivedThreaded -= OnLogReceived;

                try
                {//关闭线程
                    if (null != _workThread)
                    {
                        if (
                            forceStopThread //强制退出
                            || null == _threadWaitHandle //等待句柄为null
                            || !_threadWaitHandle.WaitOne(_stopThreadTimer))//等待线程关闭
                        {//强制退出
                            _workThread.Abort();
                        }
                    }
                }
                finally
                {
                    _workThread = null;
                    _threadWaitHandle = null;
                }

                try
                {//关闭文件
                    if (null != _fileHandle)
                    {
                        _fileHandle.Close();
                    }
                }
                finally
                {
                    _fileHandle = null;
                }

                lock (_syncRoot)
                {//清理队列
                    _queues.Clear();
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarningFormat("LogManager 结束异常:{0}", ex);
            }
        }

        /// <summary>
        /// 写日志循环
        /// </summary>
        /// <param name="obj">EventWaitHandle</param>
        private void OnWorkLoop(object obj)
        {
            EventWaitHandle waitHandle = (EventWaitHandle)obj;

            LogMessage message;
            do
            {
                try
                {
                    message = null;

                    lock (_syncRoot)
                    {
                        if (0 < _queues.Count)
                        {
                            message = _queues.Dequeue();
                        }
                    }

                    if (null == message)
                    {//没有消息，线程等待1ms
                        Thread.Sleep(1);
                        continue;
                    }

                    string outMsg = "";

                    switch (message.type)
                    {
                        case LogType.Log:
                            outMsg = string.Format("{0} [{1}] {2}\n",
                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"),
                                message.type,
                                message.condition);
                            break;

                        default:
                            outMsg = string.Format("{0} [{1}] {2}\n{3}\n",
                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"),
                                message.type,
                                message.condition,
                                message.stackTrace);
                            break;
                    }

                    byte[] buffer = Encoding.UTF8.GetBytes(outMsg);
                    _fileHandle.Write(buffer, 0, buffer.Length);
                    _fileHandle.Flush();
                }
                catch (Exception)
                {//发生异常需要等待,避免占用资源
                    Thread.Sleep(1);
                }
            } while (_isRunning);

            //设置结束状态
            waitHandle.Set();
        }

        /// <summary>
        /// 日志接收回调，非线程安全
        /// </summary>
        /// <param name="condition">消息</param>
        /// <param name="stackTrace">堆栈</param>
        /// <param name="type">类型</param>
        private void OnLogReceived(string condition, string stackTrace, LogType type)
        {
            if (!_isRunning)
            {//未初始化
                return;
            }

            lock (_syncRoot)
            {
                if (_queues.Count > _maxPoolSize)
                {//缓冲区溢出，则不再添加
                    return;
                }

                _queues.Enqueue(new LogMessage(type, condition, stackTrace));
            }
        }
    }
}