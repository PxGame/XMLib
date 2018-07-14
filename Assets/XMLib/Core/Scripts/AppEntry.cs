using System;
using System.Collections.Generic;
using XM.Services;

namespace XM
{
    /// <summary>
    /// 应用入口
    /// </summary>
    public class AppEntry : IAppEntry<AppEntry>
    {
        /// <summary>
        /// 开始关闭
        /// </summary>
        protected override void OnTerminal()
        {
            //static变量，服务关闭，引用设null
            _event = null;
            _pool = null;
            _task = null;
            _ui = null;
            _scene = null;
        }

        /// <summary>
        /// 初始化默认服务列表
        /// </summary>
        /// <returns></returns>
        protected override List<Type> DefaultServices()
        {
            List<Type> services = base.DefaultServices();

            services.Add(typeof(EventService<AppEntry>));
            services.Add(typeof(PoolService<AppEntry>));
            services.Add(typeof(TaskService<AppEntry>));
            services.Add(typeof(UIService<AppEntry>));
            services.Add(typeof(SceneService<AppEntry>));

            //默认服务
            return services;
        }

        #region Specific Custom

        //服务引用

        private static EventService<AppEntry> _event;
        private static PoolService<AppEntry> _pool;
        private static TaskService<AppEntry> _task;
        private static UIService<AppEntry> _ui;
        private static SceneService<AppEntry> _scene;

        //

        /// <summary>
        /// 事件服务
        /// </summary>
        public static EventService<AppEntry> Event
        {
            get
            {
                if (null == _event)
                {
                    _event = Inst.Get<EventService<AppEntry>>();
                }
                return _event;
            }
        }

        /// <summary>
        /// 对象池服务
        /// </summary>
        public static PoolService<AppEntry> Pool
        {
            get
            {
                if (null == _pool)
                {
                    _pool = Inst.Get<PoolService<AppEntry>>();
                }
                return _pool;
            }
        }

        /// <summary>
        /// 任务服务
        /// </summary>
        public static TaskService<AppEntry> Task
        {
            get
            {
                if (null == _task)
                {
                    _task = Inst.Get<TaskService<AppEntry>>();
                }

                return _task;
            }
        }

        /// <summary>
        /// UI服务
        /// </summary>
        public static UIService<AppEntry> UI
        {
            get
            {
                if (null == _ui)
                {
                    _ui = Inst.Get<UIService<AppEntry>>();
                }

                return _ui;
            }
        }

        /// <summary>
        /// 场景服务
        /// </summary>
        public static SceneService<AppEntry> Scene
        {
            get
            {
                if (null == _scene)
                {
                    _scene = Inst.Get<SceneService<AppEntry>>();
                }

                return _scene;
            }
        }

        #endregion Specific Custom
    }
}