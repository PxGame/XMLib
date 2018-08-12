using System;
using System.Collections.Generic;
using UnityEngine;
using XM.Services;

namespace XM
{
    /// <summary>
    /// 应用入口
    /// </summary>
    /// <typeparam name="AE">程序入口类型</typeparam>
    public abstract class IAppEntry<AE> : MonoBehaviour where AE : IAppEntry<AE>
    {
        #region 属性

        /// <summary>
        /// 获取单例
        /// </summary>
        public static AE Inst { get { return _inst; } }

        /// <summary>
        /// 是否初始化
        /// </summary>
        public static bool IsInit { get { return null != _inst; } }

        /// <summary>
        /// 日志输出
        /// </summary>
        public Action<DebugType, string> OnDebugOut { get { return _onDebugOut; } set { _onDebugOut = value; } }

        /// <summary>
        /// 服务设置集合
        /// </summary>
        public ServiceSettings Settings { get { return _settings; } }

        private static AE _inst = null;
        private Action<DebugType, string> _onDebugOut;
        private Dictionary<Type, IService<AE>> _serviceDict = new Dictionary<Type, IService<AE>>();
        private List<IService<AE>> _serviceList = new List<IService<AE>>();

        [SerializeField]
        private ServiceSettings _settings;

        #endregion 属性

        #region 服务函数

        /// <summary>
        /// 初始化默认服务列表
        /// </summary>
        /// <returns>默认服务类型列表</returns>
        protected virtual ServiceTypeList<AE> GetDefaultServices()
        {
            //默认服务
            return new ServiceTypeList<AE>();
        }

        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="serviceTypes">服务类型</param>
        public void AddRange(ServiceTypeList<AE> serviceTypes)
        {
            Type tmpType;
            IService<AE> service;
            Type[] argsTypes = new Type[] { };
            object[] args = new object[] { };

            List<IService<AE>> services = new List<IService<AE>>();
            int length = serviceTypes.Count;
            for (int i = 0; i < length; i++)
            {
                tmpType = serviceTypes[i];
                Checker.IsFalse(Exist(tmpType), "已存在该服务 {0}", tmpType.FullName);

                //创建对象
                service = (IService<AE>)tmpType.GetConstructor(argsTypes).Invoke(args);

                Debug(DebugType.Debug, "添加服务 {0}", service.ServiceName);

                //添加到初始化列表
                services.Add(service);
            }

            //创建
            length = services.Count;
            for (int i = 0; i < length; i++)
            {
                service = services[i];

                service.CreateService((AE)this);
            }

            //初始化
            length = services.Count;
            for (int i = 0; i < length; i++)
            {
                service = services[i];

                service.InitService();
            }

            //添加到已有列表
            length = services.Count;
            for (int i = 0; i < length; i++)
            {
                service = services[i];

                //添加到列表
                _serviceList.Add(service);
                //添加到字典
                _serviceDict.Add(service.GetType(), service);
            }
        }

        /// <summary>
        /// 清理所有服务
        /// </summary>
        public void ClearAll()
        {
            int length = _serviceList.Count;
            IService<AE> service;
            for (int i = 0; i < length; i++)
            {
                service = _serviceList[i];

                Debug(DebugType.Debug, "清理服务 {0}", service.ServiceName);
                service.ClearService();
            }
        }

        /// <summary>
        /// 是否存在服务
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns>是否存在</returns>
        public bool Exist<T>() where T : IService<AE>
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
        public T Get<T>() where T : IService<AE>
        {
            IService<AE> service = null;

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
        public bool Remove<T>() where T : IService<AE>
        {
            if (!Exist<T>())
            {
                return false;
            }

            T service = Get<T>();

            //开始关闭
            service.DisposeBeforeService();
            //完成关闭
            service.DisposedService();

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
            IService<AE> service;

            //开始移除
            for (int i = 0; i < length; i++)
            {
                service = _serviceList[i];

                Debug(DebugType.Debug, "开始移除服务 {0}", service.ServiceName);
                service.DisposeBeforeService();
            }

            //完成移除
            for (int i = 0; i < length; i++)
            {
                service = _serviceList[i];

                Debug(DebugType.Debug, "完成移除服务 {0}", service.ServiceName);
                service.DisposedService();
            }

            //清理
            _serviceList.Clear();
            _serviceDict.Clear();
        }

        #endregion 服务函数

        #region Unity 函数

        protected virtual void Awake()
        {
            Checker.IsNull(_inst, "IAppEntry 重复实例化");

            _inst = this as AE;
            Checker.NotNull(_inst, "单例初始化失败");

            Initialize();
        }

        protected virtual void OnDestroy()
        {
            Terminal();

            _inst = null;
        }

        #endregion Unity 函数

        #region 入口操作

        /// <summary>
        /// 安装默认服务
        /// </summary>
        private void InstallDefaultServices()
        {
            //获取默认服务
            ServiceTypeList<AE> serviceTypes = GetDefaultServices();

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

            if (null != _onDebugOut)
            {
                _onDebugOut(debugType, msg);
            }

#if UNITY_EDITOR
            UnityEngine.Debug.LogFormat("[{0}]{1}", debugType, msg);
#endif
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