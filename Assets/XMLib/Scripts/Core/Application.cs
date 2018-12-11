/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/11/2018 3:56:05 PM
 */

using System;
using System.Threading;

namespace XMLib
{
    public class Application : IApplication
    {
        #region 类型定义

        /// <summary>
        /// 启动流程
        /// </summary>
        public enum LaunchProcess
        {
            /// <summary>
            /// 构建阶段
            /// </summary>
            Construct = 0,

            /// <summary>
            /// 引导流程之前
            /// </summary>
            Bootstrap = 1,

            /// <summary>
            /// 引导流程进行中
            /// </summary>
            Bootstrapping = 2,

            /// <summary>
            /// 引导流程结束之后
            /// </summary>
            Bootstraped = 3,

            /// <summary>
            /// 初始化开始之前
            /// </summary>
            Init = 4,

            /// <summary>
            /// 初始化中
            /// </summary>
            Initing = 5,

            /// <summary>
            /// 初始化完成后
            /// </summary>
            Inited = 6,

            /// <summary>
            /// 框架运行中
            /// </summary>
            Running = 7,

            /// <summary>
            /// 框架终止之前
            /// </summary>
            Terminate = 8,

            /// <summary>
            /// 框架终止进行中
            /// </summary>
            Terminating = 9,

            /// <summary>
            /// 框架终止之后
            /// </summary>
            Terminated = 10,
        }

        #endregion 类型定义

        /// <summary>
        /// 是否是主线程
        /// </summary>
        public bool IsMainThread { get { return _mainThreadId == Thread.CurrentThread.ManagedThreadId; } }

        /// <summary>
        /// 启动流程
        /// </summary>
        public LaunchProcess Process
        {
            get { return _process; }
            private set { _process = value; }
        }

        /// <summary>
        /// 启动流程
        /// </summary>
        private LaunchProcess _process;

        /// <summary>
        /// 主线程id
        /// </summary>
        private int _mainThreadId;

        /// <summary>
        /// 是否引导完成
        /// </summary>
        private bool _isBootstraped;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Application()
        {
            App.Handler = this;

            _mainThreadId = Thread.CurrentThread.ManagedThreadId;

            //
            _isBootstraped = false;

            //
            Process = LaunchProcess.Construct;
        }

        /// <summary>
        /// 创建一个实例
        /// </summary>
        /// <returns>实例</returns>
        public static Application New()
        {
            return new Application();
        }

        /// <summary>
        /// 引导程序
        /// </summary>
        /// <param name="bootstraps"></param>
        public virtual void Bootstrap(params IBootstrap[] bootstraps)
        {
            Checker.Requires<ArgumentNullException>(bootstraps != null);

            if (_isBootstraped || Process != LaunchProcess.Construct)
            {//已调用或非构造状态
                throw new RuntimeException("Bootstrap() 函数不能重复调用");
            }
        }
    }
}