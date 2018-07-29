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
            _input = null;
            _localization = null;
        }

        /// <summary>
        /// 初始化默认服务列表
        /// </summary>
        /// <returns></returns>
        protected override List<Type> DefaultServices()
        {
            List<Type> services = base.DefaultServices();

            services.Add(typeof(EventService));
            services.Add(typeof(PoolService));
            services.Add(typeof(TaskService));
            services.Add(typeof(UIService));
            services.Add(typeof(SceneService));
            services.Add(typeof(InputService));
            services.Add(typeof(LocalizationService));

            //默认服务
            return services;
        }

        #region Specific Custom

        //服务引用

        private static EventService _event;
        private static PoolService _pool;
        private static TaskService _task;
        private static UIService _ui;
        private static SceneService _scene;
        private static InputService _input;
        private static LocalizationService _localization;

        //

        /// <summary>
        /// 事件服务
        /// </summary>
        public static EventService Event
        {
            get
            {
                if (null == _event)
                {
                    _event = Inst.Get<EventService>();
                }
                return _event;
            }
        }

        /// <summary>
        /// 对象池服务
        /// </summary>
        public static PoolService Pool
        {
            get
            {
                if (null == _pool)
                {
                    _pool = Inst.Get<PoolService>();
                }
                return _pool;
            }
        }

        /// <summary>
        /// 任务服务
        /// </summary>
        public static TaskService Task
        {
            get
            {
                if (null == _task)
                {
                    _task = Inst.Get<TaskService>();
                }

                return _task;
            }
        }

        /// <summary>
        /// UI服务
        /// </summary>
        public static UIService UI
        {
            get
            {
                if (null == _ui)
                {
                    _ui = Inst.Get<UIService>();
                }

                return _ui;
            }
        }

        /// <summary>
        /// 场景服务
        /// </summary>
        public static SceneService Scene
        {
            get
            {
                if (null == _scene)
                {
                    _scene = Inst.Get<SceneService>();
                }

                return _scene;
            }
        }

        /// <summary>
        /// 输入服务
        /// </summary>
        public static InputService Input
        {
            get
            {
                if (null == _input)
                {
                    _input = Inst.Get<InputService>();
                }

                return _input;
            }
        }

        /// <summary>
        /// 本地化服务
        /// </summary>
        public static LocalizationService Localization
        {
            get
            {
                if (null == _localization)
                {
                    _localization = Inst.Get<LocalizationService>();
                }

                return _localization;
            }
        }

        #endregion Specific Custom
    }
}