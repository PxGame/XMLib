using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM
{
    /// <summary>
    /// 应用入口
    /// </summary>
    public class IAppEntry : MonoBehaviour
    {
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

        #region private members

        private Action _onFixedUpdate;
        private Action _onLateUpdate;
        private Action _onUpdate;

        private Dictionary<Type, IService> _serviceDict = new Dictionary<Type, IService>();

        /// <summary>
        /// debug 等级
        /// </summary>
        [Tooltip("0-一般 1-警告 2-错误 3-GG ")]
        [SerializeField]
        private int _debugLevel = 0;

        #endregion private members

        protected virtual void Awake()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            Terminal();
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

            Debug("添加服务 {0}", type.FullName);

            T service = new T();

            _serviceDict.Add(type, service);

            service.AddService(this);
            service.InitService();

            return service;
        }

        /// <summary>
        /// 移除服务
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns></returns>
        public bool Remove<T>() where T : IService
        {
            Type type = typeof(T);
            Debug("移除服务 {0}", type.FullName);

            IService service = null;
            if (!_serviceDict.TryGetValue(typeof(T), out service))
            {
                return false;
            }

            service.RemoveService();

            return true;
        }

        /// <summary>
        /// debug 输出
        /// </summary>
        /// <param name="level">debug 等级= 0-一般 1-警告 2-错误 3-GG </param>
        /// <param name="format">格式化</param>
        /// <param name="args">参数</param>
        public void Debug(int level, string format, params object[] args)
        {
            if (level < _debugLevel)
            {
                return;
            }

            UnityEngine.Debug.LogFormat(format, args);
        }

        /// <summary>
        /// 一般等级debug输出
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void Debug(string format, params object[] args)
        {
            Debug(0, format, args);
        }

        /// <summary>
        /// 移除所有服务
        /// </summary>
        private void RemoveAll()
        {
            foreach (var servicePair in _serviceDict)
            {
                Debug("移除服务 {0}", servicePair.Key.FullName);

                //调用移除事件
                servicePair.Value.RemoveService();
            }
            _serviceDict.Clear();
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
            List<Type> types = DefaultServices();

            if (null == types)
            {//没有直接返回
                return;
            }

            Type tmpType;
            IService tmpService;
            Type[] argsTypes = new Type[] { };
            object[] args = new object[] { };

            List<IService> services = new List<IService>();
            int length = types.Count;
            for (int i = 0; i < length; i++)
            {
                tmpType = types[i];

                //创建对象
                tmpService = (IService)tmpType.GetConstructor(argsTypes).Invoke(args);

                if (_serviceDict.ContainsKey(tmpType))
                {//已存在服务
                    throw new Exception("已存在该服务 " + tmpType.FullName);
                }

                Debug("添加服务 {0}", tmpType.FullName);

                _serviceDict.Add(tmpType, tmpService);

                //注册
                tmpService.AddService(this);
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
        }

        /// <summary>
        /// 初始化默认服务列表
        /// </summary>
        /// <returns></returns>
        protected virtual List<Type> DefaultServices()
        {
            return new List<Type>();
        }
    }
}