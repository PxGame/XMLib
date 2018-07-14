using System;
using System.Collections.Generic;
using UnityEngine;
using XM.Services;

namespace XM
{
    /// <summary>
    /// 应用入口
    /// </summary>
    public abstract class IAppEntry : MonoBehaviour
    {
        #region Public memebers

        /// <summary>
        /// 物理更新
        /// </summary>
        public Action OnFixedUpdate { get { return _onFixedUpdate; } set { _onFixedUpdate = value; } }

        /// <summary>
        /// 延迟更新
        /// </summary>
        public Action OnLateUpdate { get { return _onLateUpdate; } set { _onLateUpdate = value; } }

        /// <summary>
        /// 更新
        /// </summary>
        public Action OnUpdate { get { return _onUpdate; } set { _onUpdate = value; } }

        /// <summary>
        /// 日志输出
        /// </summary>
        public Action<DebugType, string> OnDebugOut { get { return _onDebugOut; } set { _onDebugOut = value; } }

        /// <summary>
        /// 服务设置集合
        /// </summary>
        public ServiceSettings Settings { get { return _settings; } }

        /// <summary>
        /// 获取单例
        /// </summary>
        public static IAppEntry Inst { get { return _inst; } }

        /// <summary>
        /// 是否初始化
        /// </summary>
        public static bool IsInit { get { return null != _inst; } }

        #endregion Public memebers

        #region private members

        private Action _onFixedUpdate;
        private Action _onLateUpdate;
        private Action _onUpdate;
        private Action<DebugType, string> _onDebugOut;

        private Dictionary<Type, IService> _serviceDict = new Dictionary<Type, IService>();

        [SerializeField]
        private ServiceSettings _settings;

        private static IAppEntry _inst;

        #endregion private members

        protected virtual void Awake()
        {
            if (null != _inst)
            {
                throw new Exception("IAppEntry 重复实例化.");
            }
            _inst = this;

            Initialize();
        }

        protected virtual void OnDestroy()
        {
            Terminal();
            _inst = null;
        }

        protected virtual void FixedUpdate()
        {
            if (null != _onFixedUpdate)
            {
                _onFixedUpdate();
            }
        }

        protected virtual void LateUpdate()
        {
            if (null != _onLateUpdate)
            {
                _onLateUpdate();
            }
        }

        protected virtual void Update()
        {
            if (null != _onUpdate)
            {
                _onUpdate();
            }
        }

        #region Service Operation

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns>服务实例</returns>
        public T Get<T>() where T : IService
        {
            IService service = null;

            if (!_serviceDict.TryGetValue(typeof(T), out service))
            {
            }

            return (T)service;
        }

        /// <summary>
        /// 添加服务
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns>服务实例</returns>
        public T Add<T>() where T : IService, new()
        {
            Type type = typeof(T);
            if (_serviceDict.ContainsKey(type))
            {//已存在服务
                throw new Exception("已存在该服务 " + type.FullName);
            }

            Debug(DebugType.Normal, "添加服务 {0}", type.FullName);

            T service = new T();

            _serviceDict.Add(type, service);

            service.AddService(this);
            service.InitService();

            return service;
        }

        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="serviceTypes"></param>
        public void Adds(List<Type> serviceTypes)
        {
            Type tmpType;
            IService tmpService;
            Type[] argsTypes = new Type[] { };
            object[] args = new object[] { };

            List<IService> services = new List<IService>();
            int length = serviceTypes.Count;
            for (int i = 0; i < length; i++)
            {
                tmpType = serviceTypes[i];

                //创建对象
                tmpService = (IService)tmpType.GetConstructor(argsTypes).Invoke(args);

                if (_serviceDict.ContainsKey(tmpType))
                {//已存在服务
                    throw new Exception("已存在该服务 " + tmpType.FullName);
                }

                Debug(DebugType.Normal, "添加服务 {0}", tmpType.FullName);

                _serviceDict.Add(tmpType, tmpService);

                //注册
                tmpService.AddService(this);

                //添加到初始化列表
                services.Add(tmpService);
            }

            //初始化
            length = services.Count;
            for (int i = 0; i < length; i++)
            {
                tmpService = services[i];

                tmpService.InitService();
            }
        }

        /// <summary>
        /// 移除服务
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns></returns>
        public bool Remove<T>() where T : IService
        {
            Type type = typeof(T);
            Debug(DebugType.Normal, "移除服务 {0}", type.FullName);

            IService service = null;
            if (!_serviceDict.TryGetValue(typeof(T), out service))
            {
                return false;
            }

            service.RemoveService();

            return true;
        }

        /// <summary>
        /// 移除所有服务
        /// </summary>
        private void RemoveAll()
        {
            foreach (var servicePair in _serviceDict)
            {
                Debug(DebugType.Normal, "移除服务 {0}", servicePair.Key.FullName);

                //调用移除事件
                servicePair.Value.RemoveService();
            }
            _serviceDict.Clear();
        }

        /// <summary>
        /// 清理所有服务
        /// </summary>
        public void ClearAll()
        {
        }

        #endregion Service Operation

        /// <summary>
        /// debug 输出
        /// </summary>
        /// <param name="debugType">debug 类型 </param>
        /// <param name="format">格式化</param>
        /// <param name="args">参数</param>
        public virtual void Debug(DebugType debugType, string format, params object[] args)
        {
            if (0 == (debugType & _settings.DebugType))
            {//不符合输出要求
                return;
            }

            string msg = string.Format(format, args);

            if (null != _onDebugOut)
            {
                _onDebugOut(debugType, msg);
            }

#if UNITY_EDITOR
            UnityEngine.Debug.LogFormat("[{0}]{1}", debugType, msg);
#endif
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialize()
        {
            AddDefaults();

            OnInitilized();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        private void Terminal()
        {
            OnTerminal();

            RemoveAll();
        }

        /// <summary>
        /// 添加默认服务
        /// </summary>
        private void AddDefaults()
        {
            //获取默认服务
            List<Type> serviceTypes = DefaultServices();

            if (null == serviceTypes)
            {//没有直接返回
                return;
            }

            //添加
            Adds(serviceTypes);
        }

        /// <summary>
        /// 初始化完成
        /// </summary>
        protected virtual void OnInitilized()
        {
        }

        /// <summary>
        /// 开始关闭
        /// </summary>
        protected virtual void OnTerminal()
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
        protected virtual List<Type> DefaultServices()
        {
            //默认服务
            return new List<Type>() {
                typeof(EventService),
                typeof(PoolService),
                typeof(TaskService),
                typeof(UIService),
                typeof(SceneService),
            };
        }

        #region Specific Custom

        //服务引用

        private static EventService _event;
        private static PoolService _pool;
        private static TaskService _task;
        private static UIService _ui;
        private static SceneService _scene;

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
                    _event = _inst.Get<EventService>();
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
                    _pool = _inst.Get<PoolService>();
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
                    _task = _inst.Get<TaskService>();
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
                    _ui = _inst.Get<UIService>();
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
                    _scene = _inst.Get<SceneService>();
                }

                return _scene;
            }
        }

        #endregion Specific Custom
    }
}