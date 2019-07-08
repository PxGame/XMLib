/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/12/2018 12:17:42 PM
 */

using System;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// Unity 框架
    /// </summary>
    [DefaultExecutionOrder(-10000)]
    [DisallowMultipleComponent]
    public class Framework : MonoBehaviour, IBootstrap
    {
        /// <summary>
        /// 设置
        /// </summary>
        [Tooltip("设置")]
        [SerializeField]
        protected FrameworkSetting _setting;

        #region Unity

        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _application = new UnityApplication(this);
            _application.Bootstrap(GetBootstraps());
            _application.Init();
        }

        protected virtual void OnDestroy()
        {
            _application.Terminate();
        }

        #endregion Unity

        #region Framework

        /// <summary>
        /// 应用实例
        /// </summary>
        private Application _application;

        /// <summary>
        /// 应用实例
        /// </summary>
        protected IApplication application { get { return _application; } }

        /// <summary>
        /// 入口引导
        /// </summary>
        [Priority(-10000)]
        public virtual void Bootstrap()
        {
            App.On(ApplicationEvents.OnBootstrap, OnBootstrap);
            App.On<IServiceProvider>(ApplicationEvents.OnRegisterProvider, OnRegisterProvider);
            App.On(ApplicationEvents.OnBootstraped, OnBootstraped);
            App.On(ApplicationEvents.OnInit, OnInit);
            App.On<IServiceProvider>(ApplicationEvents.OnProviderInit, OnProviderInit);
            App.On<IServiceProvider>(ApplicationEvents.OnProviderInited, OnProviderInited);
            App.On(ApplicationEvents.OnInited, OnInited);
            App.On(ApplicationEvents.OnStartCompleted, OnStartCompleted);
            App.On(ApplicationEvents.OnTerminate, OnTerminate);
            App.On(ApplicationEvents.OnTerminated, OnTerminated);

            //处理设置
            ProcessSettings();
        }

        /// <summary>
        /// 加载服务通过设置
        /// </summary>
        private void ProcessSettings()
        {
            foreach (IServiceSetting setting in _setting)
            {
                //注册服务设置
                string serviceName = App.Type2Service(setting.GetType());
                App.Instance(serviceName, setting);

                //注册服务提供者
                IServiceProvider provider = setting.NewServiceProvider();
                App.Register(provider);
            }
        }

        /// <summary>
        /// 获取引导
        /// </summary>
        /// <returns>引导实例数组</returns>
        protected virtual IBootstrap[] GetBootstraps()
        {
            IBootstrap[] components = GetComponents<IBootstrap>();
            return components;
        }

        #region 事件

        /// <summary>
        ///  当引导程序开始之前
        /// </summary>
        protected virtual void OnBootstrap() { }

        /// <summary>
        ///  当注册服务提供者
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        protected virtual void OnRegisterProvider(IServiceProvider serviceProvider) { }

        /// <summary>
        /// 所有引导完成
        /// </summary>
        protected virtual void OnBootstraped() { }

        /// <summary>
        /// 当初始化开始之前
        /// </summary>
        protected virtual void OnInit() { }

        /// <summary>
        /// 当服务提供者初始化结束后
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        protected virtual void OnProviderInit(IServiceProvider serviceProvider) { }

        /// <summary>
        /// 当服务提供者初始化进行前
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        protected virtual void OnProviderInited(IServiceProvider serviceProvider) { }

        /// <summary>
        /// 当初始化完成之后
        /// </summary>
        protected virtual void OnInited() { }

        /// <summary>
        /// 框架启动完成
        /// </summary>
        protected virtual void OnStartCompleted() { }

        /// <summary>
        /// 框架终止前
        /// </summary>
        protected virtual void OnTerminate() { }

        /// <summary>
        /// 框架终止后
        /// </summary>
        protected virtual void OnTerminated() { }

        #endregion 事件

        #endregion Framework
    }
}