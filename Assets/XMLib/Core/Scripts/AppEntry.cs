using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XM.Services;

namespace XM
{
    /// <summary>
    /// 应用入口
    /// </summary>
    [DefaultExecutionOrder(-1000)]
    public abstract class AppEntry : MonoBehaviour
    {
        #region 属性

        /// <summary>
        /// 日志输出
        /// </summary>
        public Action<DebugType, string> OnDebugOut { get { return _onDebugOut; } set { _onDebugOut = value; } }

        /// <summary>
        /// 服务设置集合
        /// </summary>
        public ServiceSettings Settings { get { return _settings; } }

        private Action<DebugType, string> _onDebugOut;
        private Dictionary<Type, IService> _serviceDict = new Dictionary<Type, IService>();
        private List<IService> _serviceList = new List<IService>();

        [SerializeField]
        private ServiceSettings _settings;

        private IndexList<IUpdate> _updates = new IndexList<IUpdate>();
        private IndexList<ILateUpdate> _lateUpdates = new IndexList<ILateUpdate>();
        private IndexList<IFixedUpdate> _fixedUpdates = new IndexList<IFixedUpdate>();
        private IndexList<IOnGUI> _onGUIs = new IndexList<IOnGUI>();

        protected static AppEntry _appEntry = null;

        /// <summary>
        /// 入口单例
        /// </summary>
        public static AppEntry Inst { get { return _appEntry; } }

        #endregion 属性

        #region 服务函数

        /// <summary>
        /// 初始化默认服务列表
        /// </summary>
        /// <returns>默认服务类型列表</returns>
        protected virtual ServiceTypeList GetDefaultServices()
        {
            //默认服务
            return new ServiceTypeList();
        }

        /// <summary>
        /// 注册回调
        /// </summary>
        /// <param name="service">服务实例</param>
        protected void RegistCallback(IService service)
        {
            if (service is IUpdate)
            {
                int priority = AttributeUtils.GetPriority(service.GetType(), "Update");
                _updates.Add(priority, (IUpdate)service);
                _updates.Sort();
            }
            if (service is ILateUpdate)
            {
                int priority = AttributeUtils.GetPriority(service.GetType(), "LateUpdate");
                _lateUpdates.Add(priority, (ILateUpdate)service);
                _lateUpdates.Sort();
            }
            if (service is IFixedUpdate)
            {
                int priority = AttributeUtils.GetPriority(service.GetType(), "FixedUpdate");
                _fixedUpdates.Add(priority, (IFixedUpdate)service);
                _fixedUpdates.Sort();
            }
            if (service is IOnGUI)
            {
                int priority = AttributeUtils.GetPriority(service.GetType(), "OnGUI");
                _onGUIs.Add(priority, (IOnGUI)service);
                _onGUIs.Sort();
            }
        }

        /// <summary>
        /// 反注册回调
        /// </summary>
        /// <param name="service">服务实例</param>
        protected void UnRegistCallback(IService service)
        {
            if (service is IUpdate)
            {
                _updates.RemoveAll((IUpdate)service);
            }
            if (service is ILateUpdate)
            {
                _lateUpdates.RemoveAll((ILateUpdate)service);
            }
            if (service is IFixedUpdate)
            {
                _fixedUpdates.RemoveAll((IFixedUpdate)service);
            }
            if (service is IOnGUI)
            {
                _onGUIs.RemoveAll((IOnGUI)service);
            }
        }

        /// <summary>
        /// 添加服务
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        public void Add<T>() where T : IService
        {
            AddRange(new ServiceTypeList() { typeof(T) });
        }

        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="serviceTypes">服务类型</param>
        public void AddRange(List<Type> serviceTypes)
        {
            Type tmpType;
            IService service;
            Type[] argsTypes = new Type[] { };
            object[] args = new object[] { };

            List<IService> services = new List<IService>();
            int length = serviceTypes.Count;

            //实例化
            for (int i = 0; i < length; i++)
            {
                tmpType = serviceTypes[i];

                try
                {
                    Checker.IsFalse(Exist(tmpType), "已存在 {0} 服务,不允许重复添加", tmpType.FullName);

                    //创建对象
                    service = (IService)tmpType.GetConstructor(argsTypes).Invoke(args);

                    //添加到初始化列表
                    services.Add(service);
                }
                catch (Exception ex)
                {
                    Debug(DebugType.Exception, "实例化 {0} 服务异常\n{1}", tmpType.FullName, ex);
                }
            }

            //创建
            length = services.Count;
            for (int i = 0; i < length; i++)
            {
                service = services[i];

                try
                {
                    service.CreateService((AppEntry)this);
                }
                catch (Exception ex)
                {
                    Debug(DebugType.Exception, "创建 {0} 服务异常\n{1}", service.ServiceName, ex);
                }
            }

            //初始化
            length = services.Count;
            for (int i = 0; i < length; i++)
            {
                service = services[i];

                try
                {
                    service.InitService();
                }
                catch (Exception ex)
                {
                    Debug(DebugType.Exception, "初始化 {0} 服务异常\n{1}", service.ServiceName, ex);
                }
            }

            //添加
            length = services.Count;
            for (int i = 0; i < length; i++)
            {
                service = services[i];

                try
                {
                    //添加到列表
                    _serviceList.Add(service);
                    //添加到字典
                    _serviceDict.Add(service.GetType(), service);
                }
                catch (Exception ex)
                {
                    Debug(DebugType.Exception, "添加 {0} 服务异常\n{1}", service.ServiceName, ex);
                }
            }

            //注册回调
            length = services.Count;
            for (int i = 0; i < length; i++)
            {
                service = services[i];

                try
                {
                    //注册回调
                    RegistCallback(service);

                    Debug(DebugType.Debug, "启动 {0} 服务成功", service.ServiceName);
                }
                catch (Exception ex)
                {
                    Debug(DebugType.Exception, "{0} 服务注册回调异常\n{1}", service.ServiceName, ex);
                }
            }
        }

        /// <summary>
        /// ` 清理所有服务
        /// </summary>
        public void ClearAll()
        {
            int length = _serviceList.Count;
            IService service;
            for (int i = 0; i < length; i++)
            {
                service = _serviceList[i];

                try
                {
                    service.ClearService();
                    Debug(DebugType.Debug, "清理 {0} 服务成功", service.ServiceName);
                }
                catch (Exception ex)
                {
                    Debug(DebugType.Exception, "清理 {0} 服务异常\n{1}", service.ServiceName, ex);
                }
            }
        }

        /// <summary>
        /// 是否存在服务
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns>是否存在</returns>
        public bool Exist<T>() where T : IService
        {
            return Exist(typeof(T));
        }

        /// <summary>
        /// 是否存在服务
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns>是否存在</returns>
        public bool Exist(Type serviceType)
        {
            return _serviceDict.ContainsKey(serviceType);
        }

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
                Debug(DebugType.Warning, "未获取到该类型的服务：{0}", typeof(T).FullName);
            }

            return (T)service;
        }

        /// <summary>
        /// 移除服务
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns>是否成功</returns>
        public bool Remove<T>() where T : IService
        {
            if (!Exist<T>())
            {
                return false;
            }

            T service = Get<T>();

            try
            {
                //反注册回调
                UnRegistCallback(service);
            }
            catch (Exception ex)
            {
                Debug(DebugType.Exception, "{0} 服务反注册回调异常\n{1}", service.ServiceName, ex);
            }

            try
            {
                //开始关闭
                service.DisposeBeforeService();
            }
            catch (Exception ex)
            {
                Debug(DebugType.Exception, "开始关闭 {0} 服务异常\n{1}", service.ServiceName, ex);
            }

            try
            {
                //完成关闭
                service.DisposedService();
                Debug(DebugType.Debug, "关闭 {0} 服务成功", service.ServiceName);
            }
            catch (Exception ex)
            {
                Debug(DebugType.Exception, "完成关闭 {0} 服务异常\n{1}", service.ServiceName, ex);
            }

            try
            {
                //移除
                _serviceDict.Remove(typeof(T));
                _serviceList.Remove(service);
            }
            catch (Exception ex)
            {
                Debug(DebugType.Exception, "移除 {0} 服务异常\n{1}", service.ServiceName, ex);
            }

            return true;
        }

        /// <summary>
        /// 移除所有服务
        /// </summary>
        private void RemoveAll()
        {
            //反向，先创建的后移除
            _serviceList.Reverse();

            int length = _serviceList.Count;
            IService service;

            //反注册回调
            for (int i = 0; i < length; i++)
            {
                service = _serviceList[i];

                try
                {
                    //注册回调
                    UnRegistCallback(service);
                }
                catch (Exception ex)
                {
                    Debug(DebugType.Exception, "{0} 服务反注册回调异常\n{1}", service.ServiceName, ex);
                }
            }

            //开始移除
            for (int i = 0; i < length; i++)
            {
                service = _serviceList[i];

                try
                {
                    //开始关闭
                    service.DisposeBeforeService();
                }
                catch (Exception ex)
                {
                    Debug(DebugType.Exception, "开始关闭 {0} 服务异常\n{1}", service.ServiceName, ex);
                }
            }

            //完成移除
            for (int i = 0; i < length; i++)
            {
                service = _serviceList[i];

                try
                {
                    //完成关闭
                    service.DisposedService();
                    Debug(DebugType.Debug, "关闭 {0} 服务成功", service.ServiceName);
                }
                catch (Exception ex)
                {
                    Debug(DebugType.Exception, "完成关闭 {0} 服务异常\n{1}", service.ServiceName, ex);
                }
            }

            //清理
            _serviceList.Clear();
            _serviceDict.Clear();
        }

        #endregion 服务函数

        #region Unity 函数

        protected virtual void Awake()
        {
            Checker.IsNull(_appEntry, "入口重复实例化");
            _appEntry = this;
            DontDestroyOnLoad(gameObject);

            Initialize();
        }

        protected virtual void OnDestroy()
        {
            Terminal();

            _appEntry = null;
        }

        protected virtual void FixedUpdate()
        {
            if (null != _fixedUpdates)
            {
                int length = _fixedUpdates.Count;
                IFixedUpdate fixedUpdate;
                for (int i = 0; i < length; i++)
                {
                    fixedUpdate = _fixedUpdates[i].Value;
                    try
                    {
                        fixedUpdate.FixedUpdate();
                    }
                    catch (Exception ex)
                    {
                        Debug(DebugType.Exception, "{0} 服务 FixedUpdate 发生异常\n{1}", fixedUpdate.ServiceName, ex);
                    }
                }
            }
        }

        protected virtual void LateUpdate()
        {
            if (null != _lateUpdates)
            {
                int length = _lateUpdates.Count;
                ILateUpdate lateUpdate;
                for (int i = 0; i < length; i++)
                {
                    lateUpdate = _lateUpdates[i].Value;
                    try
                    {
                        lateUpdate.LateUpdate();
                    }
                    catch (Exception ex)
                    {
                        Debug(DebugType.Exception, "{0} 服务 LateUpdate 发生异常\n{1}", lateUpdate.ServiceName, ex);
                    }
                }
            }
        }

        protected virtual void OnGUI()
        {
            if (null != _onGUIs)
            {
                int length = _onGUIs.Count;
                IOnGUI onGUI;
                for (int i = 0; i < length; i++)
                {
                    onGUI = _onGUIs[i].Value;
                    try
                    {
                        onGUI.OnGUI();
                    }
                    catch (Exception ex)
                    {
                        Debug(DebugType.Exception, "{0} 服务 OnGUI 发生异常\n{1}", onGUI.ServiceName, ex);
                    }
                }
            }
        }

        protected virtual void Update()
        {
            if (null != _updates)
            {
                int length = _updates.Count;
                IUpdate update;
                for (int i = 0; i < length; i++)
                {
                    update = _updates[i].Value;
                    try
                    {
                        update.Update();
                    }
                    catch (Exception ex)
                    {
                        Debug(DebugType.Exception, "{0} 服务 Update 发生异常\n{1}", update.ServiceName, ex);
                    }
                }
            }
        }

        #endregion Unity 函数

        #region 入口操作

        /// <summary>
        /// 安装默认服务
        /// </summary>
        private void InstallDefaultServices()
        {
            //获取默认服务
            List<Type> serviceTypes = GetDefaultServices();

            if (null == serviceTypes)
            {//没有直接返回
                return;
            }

            //添加
            AddRange(serviceTypes);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialize()
        {
            //安装默认服务
            InstallDefaultServices();

            //初始化完成
            OnInitilized();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        private void Terminal()
        {
            RemoveAll();
            OnTerminaled();
        }

        /// <summary>
        /// 服务 debug 输出
        /// </summary>
        /// <param name="debugType">输出类型</param>
        /// <param name="service">服务</param>
        /// <param name="format">格式化</param>
        /// <param name="args">参数</param>
        public virtual void Debug(DebugType debugType, IService service, string format, params object[] args)
        {
            if (0 == (debugType & _settings.DebugType))
            {//不符合输出要求
                return;
            }

            string serviceName = service != null ? service.ServiceName : "未知服务";
            string msg = string.Format(format, args);
            string outMsg = string.Format("[XM][{0}][{1}]{2}", debugType, serviceName, msg);

            if (null != _onDebugOut)
            {
                _onDebugOut(debugType, outMsg);
            }

            switch (debugType)
            {
                case DebugType.Warning:
                    UnityEngine.Debug.LogWarning(outMsg);
                    break;

                case DebugType.Exception:
                case DebugType.Error:
                case DebugType.GG:
                    UnityEngine.Debug.LogError(outMsg);
                    break;

                default:
                    UnityEngine.Debug.Log(outMsg);
                    break;
            }
        }

        /// <summary>
        /// debug 输出
        /// </summary>
        /// <param name="debugType">debug 类型</param>
        /// <param name="format">格式化</param>
        /// <param name="args">参数</param>
        public virtual void Debug(DebugType debugType, string format, params object[] args)
        {
            if (0 == (debugType & _settings.DebugType))
            {//不符合输出要求
                return;
            }

            string msg = string.Format(format, args);
            string outMsg = string.Format("[XM][{0}]{1}", debugType, msg);

            if (null != _onDebugOut)
            {
                _onDebugOut(debugType, outMsg);
            }

            switch (debugType)
            {
                case DebugType.Warning:
                    UnityEngine.Debug.LogWarning(outMsg);
                    break;

                case DebugType.Exception:
                case DebugType.Error:
                case DebugType.GG:
                    UnityEngine.Debug.LogError(outMsg);
                    break;

                default:
                    UnityEngine.Debug.Log(outMsg);
                    break;
            }
        }

        #endregion 入口操作

        #region 回调

        /// <summary>
        /// 初始化完成
        /// </summary>
        protected virtual void OnInitilized()
        {
        }

        /// <summary>
        /// 关闭完成
        /// </summary>
        protected virtual void OnTerminaled()
        {
        }

        #endregion 回调
    }
}