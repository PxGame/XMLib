using XM.Services;
using XM.Services.Event;
using XM.Services.Input;
using XM.Services.Localization;
using XM.Services.Pool;
using XM.Services.Scene;
using XM.Services.Task;
using XM.Services.UI;

namespace XM
{
    /// <summary>
    /// 应用入口
    /// </summary>
    public class AppEntry : IAppEntry<AppEntry>
    {
        #region 重写

        /// <summary>
        /// 关闭完成
        /// </summary>
        protected override void OnTerminaled()
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
        protected override ServiceTypeList<AppEntry> GetDefaultServices()
        {
            ServiceTypeList<AppEntry> services = base.GetDefaultServices();

            services.Add<EventService>();
            services.Add<PoolService>();
            services.Add<TaskService>();
            services.Add<UIService>();
            services.Add<SceneService>();
            services.Add<InputService>();
            services.Add<LocalizationService>();

            //默认服务
            return services;
        }

        #endregion 重写

        #region 自定义

        #region 服务静态引用单例

        private static EventService _event;
        private static PoolService _pool;
        private static TaskService _task;
        private static UIService _ui;
        private static SceneService _scene;
        private static InputService _input;
        private static LocalizationService _localization;

        #endregion 服务静态引用单例

        #region 服务单例

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

        #endregion 服务单例

        #endregion 自定义
    }
}