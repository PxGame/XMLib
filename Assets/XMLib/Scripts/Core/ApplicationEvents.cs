/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/12/2018 10:45:12 AM
 */

using System.Collections;
using System.Collections.Generic;
using System;

namespace XMLib
{
    /// <summary>
    /// 应用程序事件
    /// </summary>
    public static class ApplicationEvents
    {
        /// <summary>
        /// 当引导程序开始之前
        /// <para>void Func(IApplication app)</para>
        /// </summary>
        public static readonly string OnBootstrap = "XMLib.ApplicationEvents.OnBootstrap";

        /// <summary>
        /// 当引导程序进行中
        /// <para>void Func(IBootstrap bootstrap)</para>
        /// <para>object Func(IBootstrap bootstrap)</para>
        /// <para>如果返回非null对象,则不会调用当前引导</para>
        /// </summary>
        public static readonly string Bootstrapping = "XMLib.ApplicationEvents.Bootstrapping";

        /// <summary>
        /// 当引导完成时
        /// <para>void Func(IApplication app)</para>
        /// </summary>
        public static readonly string OnBootstraped = "XMLib.ApplicationEvents.OnBootstraped";

        /// <summary>
        /// 当注册服务提供者
        /// </summary>
        public static readonly string OnRegisterProvider = "XMLib.ApplicationEvents.OnRegisterProvider";

        /// <summary>
        /// 当初始化开始之前
        /// <para>void Func(IApplication app)</para>
        /// </summary>
        public static readonly string OnInit = "XMLib.ApplicationEvents.OnInit";

        /// <summary>
        /// 当服务提供者初始化进行前
        /// <para>void Func(IServiceProvider serviceProvider)</para>
        /// </summary>
        public static readonly string OnProviderInit = "XMLib.ApplicationEvents.OnProviderInit";

        /// <summary>
        /// 当服务提供者初始化结束后
        /// <para>void Func(IServiceProvider serviceProvider)</para>
        /// </summary>
        public static readonly string OnProviderInited = "XMLib.ApplicationEvents.OnProviderInited";

        /// <summary>
        /// 当初始化完成之后
        /// <para>void Func(IApplication app)</para>
        /// </summary>
        public static readonly string OnInited = "XMLib.ApplicationEvents.OnInited";

        /// <summary>
        /// 当程序启动完成
        /// <para>void Func(IApplication app)</para>
        /// </summary>
        public static readonly string OnStartCompleted = "XMLib.ApplicationEvents.OnStartCompleted";

        /// <summary>
        /// 当程序终止之前
        /// <para>void Func(IApplication app)</para>
        /// </summary>
        public static readonly string OnTerminate = "XMLib.ApplicationEvents.OnTerminate";

        /// <summary>
        /// 当程序终止之后
        /// <para>void Func(IApplication app)</para>
        /// </summary>
        public static readonly string OnTerminated = "XMLib.ApplicationEvents.OnTerminated";
    }
}