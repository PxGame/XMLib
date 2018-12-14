/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/12/2018 12:17:42 PM
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// Unity 框架
    /// </summary>
    [DisallowMultipleComponent]
    public class Framework : MonoBehaviour, IBootstrap
    {
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
        protected IApplication Application { get { return _application; } }

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
        protected virtual void OnBootstrap()
        {
            App.Log("框架引导开始");
        }

        /// <summary>
        ///  当注册服务提供者
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        protected virtual void OnRegisterProvider(IServiceProvider serviceProvider)
        {
            App.Log("{0} 服务注册中", serviceProvider.GetType());
        }

        /// <summary>
        /// 所有引导完成
        /// </summary>
        protected virtual void OnBootstraped()
        {
            App.Log("框架引导结束");
        }

        /// <summary>
        /// 当初始化开始之前
        /// </summary>
        protected virtual void OnInit()
        {
            App.Log("框架初始化开始");
        }

        /// <summary>
        /// 当服务提供者初始化结束后
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        protected virtual void OnProviderInit(IServiceProvider serviceProvider)
        {
            App.Log("{0} 服务初始化开始", serviceProvider.GetType());
        }

        /// <summary>
        /// 当服务提供者初始化进行前
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        protected virtual void OnProviderInited(IServiceProvider serviceProvider)
        {
            App.Log("{0} 服务初始化结束", serviceProvider.GetType());
        }

        /// <summary>
        /// 当初始化完成之后
        /// </summary>
        protected virtual void OnInited()
        {
            App.Log("框架初始化完成");
        }

        /// <summary>
        /// 框架启动完成
        /// </summary>
        protected virtual void OnStartCompleted()
        {
            App.Log("框架启动完成");
        }

        /// <summary>
        /// 框架终止前
        /// </summary>
        protected virtual void OnTerminate()
        {
            App.Log("框架终止开始");
        }

        /// <summary>
        /// 框架终止后
        /// </summary>
        protected virtual void OnTerminated()
        {
            App.Log("框架终止结束");
        }

        #endregion 事件

        #endregion Framework
    }
}