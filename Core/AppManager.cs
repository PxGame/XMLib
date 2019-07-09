using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// 应用程序管理类
    /// </summary>
    public abstract class AppManager : MonoBehaviour
    {
        public bool isMainThread { get { return _mainThreadId == Thread.CurrentThread.ManagedThreadId; } }
        public Container container { get { return _container; } }
        public Dispatcher dispatcher { get { return _dispatcher; } }

        private readonly Container _container;
        private readonly Dispatcher _dispatcher;
        private readonly MonoDriver _monoDriver;
        private readonly int _mainThreadId;

        public AppManager()
        {
            //
            _mainThreadId = Thread.CurrentThread.ManagedThreadId;

            //
            _container = new Container();
            _dispatcher = new Dispatcher(_container);
            _monoDriver = new MonoDriver(this, () => isMainThread);

            //
            _container.Instance<MonoDriver>(_monoDriver);

            //
            _container.OnResolving(OnGlobalResolving);
            _container.OnRelease(OnGlobalRelease);
        }

        #region 处理静态服务

        protected virtual void OnGlobalRelease(BindData bindData, object instance)
        {
            _monoDriver.OnGlobalRelease(bindData, instance);
        }

        protected virtual object OnGlobalResolving(BindData bindData, object instance)
        {
            _monoDriver.OnGlobalResolving(bindData, instance);
            return instance;
        }

        #endregion 处理静态服务

        #region Mono

        private static AppManager _inst = null;
        public static AppManager Inst { get { return _inst; } }

        protected virtual void Awake()
        {
            Debug.Log("AppManager 启动");

            if (_inst != null)
            {
                throw new RuntimeException("管理器重复实例化");
            }
            _inst = this;

            DontDestroyOnLoad(gameObject);
        }

        protected virtual void Update()
        {
            _monoDriver.Update();
        }

        protected virtual void LateUpdate()
        {
            _monoDriver.LateUpdate();
        }

        protected virtual void FixedUpdate()
        {
            _monoDriver.FixedUpdate();
        }

        protected virtual void OnDestroy()
        {
            _monoDriver.OnDestroy();

            _container.Flush();

            Debug.Log("AppManager 关闭");
        }

        protected virtual void OnGUI()
        {
            _monoDriver.OnGUI();
        }

        #endregion Mono

        #region static

        public static Coroutine StartCrt(IEnumerator routine)
        {
            return Inst.StartCoroutine(routine);
        }

        public static void StopCrt(Coroutine routine)
        {
            Inst.StopCoroutine(routine);
        }

        #region Event Dispatcher

        /// <summary>
        /// 是否存在事件的监听
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <returns>是否存在</returns>
        public static bool HasListener(string eventName)
        {
            return Inst.dispatcher.HasListener(eventName);
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="args">参数</param>
        /// <returns>结果</returns>
        public static List<object> Trigger(string eventName, params object[] args)
        {
            return Inst.dispatcher.Trigger(eventName, args);
        }

        /// <summary>
        /// 触发一个事件，遇到第一个事件存在处理结果后终止，并获取事件监听的返回结果
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="args">参数</param>
        /// <returns>结果</returns>
        public static object TriggerHalt(string eventName, params object[] args)
        {
            return Inst.dispatcher.TriggerHalt(eventName, args);
        }

        /// <summary>
        /// 快速触发事件，严格映射输入参数
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="args">参数</param>
        /// <returns>结果</returns>
        public static List<object> TriggerFast(string eventName, params object[] args)
        {
            return Inst.dispatcher.TriggerFast(eventName, args);
        }

        /// <summary>
        /// 快速触发一个事件，严格映射输入参数，遇到第一个事件存在处理结果后终止，并获取事件监听的返回结果
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="args">参数</param>
        /// <returns>结果</returns>
        public static object TriggerHaltFast(string eventName, params object[] args)
        {
            return Inst.dispatcher.TriggerHaltFast(eventName, args);
        }

        /// <summary>
        /// 注册一个事件监听器
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="target">目标对象</param>
        /// <param name="methodInfo">调用方法</param>
        /// <param name="group">分组</param>
        /// <returns>监听</returns>
        public static DispatchEvent On(string eventName, object target, MethodInfo methodInfo, object group = null)
        {
            return Inst.dispatcher.On(eventName, target, methodInfo, group);
        }

        /// <summary>
        /// 解除注册的事件监听器
        /// </summary>
        /// <param name="target">
        /// 事件解除目标
        /// <para>如果传入的是字符串(<code>string</code>)将会解除对应事件名的所有事件</para>
        /// <para>如果传入的是事件对象(<code>DispatchEvent</code>)那么解除对应事件</para>
        /// <para>如果传入的是分组(<code>object</code>)会解除该分组下的所有事件</para>
        /// </param>
        public static void Off(object target)
        {
            Inst.dispatcher.Off(target);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="target">调用目标</param>
        /// <param name="method">处理函数名</param>
        /// <returns>对象</returns>
        public static DispatchEvent On(string eventName, object target, string method = null)
        {
            return Inst.dispatcher.On(eventName, target, method);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static DispatchEvent On(string eventName, Action method, object group = null)
        {
            return Inst.dispatcher.On(eventName, method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static DispatchEvent On<T1>(string eventName, Action<T1> method, object group = null)
        {
            return Inst.dispatcher.On<T1>(eventName, method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static DispatchEvent On<T1, T2>(string eventName, Action<T1, T2> method, object group = null)
        {
            return Inst.dispatcher.On<T1, T2>(eventName, method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static DispatchEvent On<T1, T2, T3>(string eventName, Action<T1, T2, T3> method, object group = null)
        {
            return Inst.dispatcher.On<T1, T2, T3>(eventName, method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static DispatchEvent On<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> method, object group = null)
        {
            return Inst.dispatcher.On<T1, T2, T3, T4>(eventName, method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static DispatchEvent Listen<TResult>(string eventName, Func<TResult> method, object group = null)
        {
            return Inst.dispatcher.Listen<TResult>(eventName, method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static DispatchEvent Listen<T1, TResult>(string eventName, Func<T1, TResult> method, object group = null)
        {
            return Inst.dispatcher.Listen<T1, TResult>(eventName, method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static DispatchEvent Listen<T1, T2, TResult>(string eventName, Func<T1, T2, TResult> method, object group = null)
        {
            return Inst.dispatcher.Listen<T1, T2, TResult>(eventName, method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static DispatchEvent Listen<T1, T2, T3, TResult>(string eventName, Func<T1, T2, T3, TResult> method, object group = null)
        {
            return Inst.dispatcher.Listen<T1, T2, T3, TResult>(eventName, method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static DispatchEvent Listen<T1, T2, T3, T4, TResult>(string eventName, Func<T1, T2, T3, T4, TResult> method, object group = null)
        {
            return Inst.dispatcher.Listen<T1, T2, T3, T4, TResult>(eventName, method, group);
        }

        #endregion Event Dispatcher

        #region Container

        public static string Type2Service<TService>()
        {
            return Inst.container.Type2Service<TService>();
        }

        public static object Instance<TService>(object instance)
        {
            return Inst.container.Instance<TService>(instance);
        }

        public static bool Release<TService>()
        {
            return Inst.container.Release<TService>();
        }

        public static bool Release(ref object[] instances, bool reverse = true)
        {
            return Inst.container.Release(ref instances, reverse);
        }

        public static BindData GetBind<TService>()
        {
            return Inst.container.GetBind<TService>();
        }

        public static void Unbind<TService>()
        {
            Inst.container.Unbind<TService>();
        }

        public static bool HasBind<TService>()
        {
            return Inst.container.HasBind<TService>();
        }

        public static bool HasIntstance<TService>()
        {
            return Inst.container.HasIntstance<TService>();
        }

        public static bool IsResolved<TService>()
        {
            return Inst.container.IsResolved<TService>();
        }

        public static bool CanMake<TService>()
        {
            return Inst.container.CanMake<TService>();
        }

        public static bool IsStatic<TService>()
        {
            return Inst.container.IsStatic<TService>();
        }

        public static bool IsAlias<TService>()
        {
            return Inst.container.IsAlias<TService>();
        }

        public static Container Alias<TAlias, TService>()
        {
            return Inst.container.Alias<TAlias, TService>();
        }

        #region Bind

        public static BindData Bind<TService>()
        {
            return Inst.container.Bind<TService>();
        }

        public static BindData Bind<TService, TConcrete>()
        {
            return Inst.container.Bind<TService, TConcrete>();
        }

        public static BindData Bind<TService>(Func<Container, object[], object> concrete)
        {
            return Inst.container.Bind<TService>(concrete);
        }

        public static BindData Bind<TService>(Func<object> concrete)
        {
            return Inst.container.Bind<TService>(concrete);
        }

        public static BindData Bind(string service, Func<Container, object[], object> concrete)
        {
            return Inst.container.Bind(service, concrete);
        }

        public static bool BindIf<TService, TConcrete>(out BindData bindData)
        {
            return Inst.container.BindIf<TService, TConcrete>(out bindData);
        }

        public static bool BindIf<TService>(out BindData bindData)
        {
            return Inst.container.BindIf<TService>(out bindData);
        }

        public static bool BindIf<TService>(Func<Container, object[], object> concrete, out BindData bindData)
        {
            return Inst.container.BindIf<TService>(concrete, out bindData);
        }

        public static bool BindIf<TService>(Func<object[], object> concrete, out BindData bindData)
        {
            return Inst.container.BindIf<TService>(concrete, out bindData);
        }

        public static bool BindIf<TService>(Func<object> concrete, out BindData bindData)
        {
            return Inst.container.BindIf<TService>(concrete, out bindData);
        }

        public static bool BindIf(string service,
            Func<Container, object[], object> concrete, out BindData bindData)
        {
            return Inst.container.BindIf(service, concrete, out bindData);
        }

        #endregion Bind

        #region Singleton

        public static BindData Singleton(string service,
            Func<Container, object[], object> concrete)
        {
            return Inst.container.Singleton(service, concrete);
        }

        public static BindData Singleton<TService, TConcrete>()
        {
            return Inst.container.Singleton<TService, TConcrete>();
        }

        public static BindData Singleton<TService>()
        {
            return Inst.container.Singleton<TService>();
        }

        public static BindData Singleton<TService>(Func<Container, object[], object> concrete)
        {
            return Inst.container.Singleton<TService>(concrete);
        }

        public static BindData Singleton<TService>(Func<object[], object> concrete)
        {
            return Inst.container.Singleton<TService>(concrete);
        }

        public static BindData Singleton<TService>(Func<object> concrete)
        {
            return Inst.container.Singleton<TService>(concrete);
        }

        public static bool SingletonIf<TService, TConcrete>(out BindData bindData)
        {
            return Inst.container.SingletonIf<TService, TConcrete>(out bindData);
        }

        public static bool SingletonIf<TService>(out BindData bindData)
        {
            return Inst.container.SingletonIf<TService>(out bindData);
        }

        public static bool SingletonIf<TService>(Func<Container, object[], object> concrete, out BindData bindData)
        {
            return Inst.container.SingletonIf<TService>(concrete, out bindData);
        }

        public static bool SingletonIf<TService>(Func<object> concrete, out BindData bindData)
        {
            return Inst.container.SingletonIf<TService>(concrete, out bindData);
        }

        public static bool SingletonIf<TService>(Func<object[], object> concrete, out BindData bindData)
        {
            return Inst.container.SingletonIf<TService>(concrete, out bindData);
        }

        public static bool SingletonIf(string service, Func<Container, object[], object> concrete, out BindData bindData)
        {
            return Inst.container.SingletonIf(service, concrete, out bindData);
        }

        #endregion Singleton

        #region Make

        public static TService Make<TService>(params object[] userParams)
        {
            return Inst.container.Make<TService>(userParams);
        }

        public static object Make(Type type, params object[] userParams)
        {
            return Inst.container.Make(type, userParams);
        }

        #endregion Make

        #endregion Container

        #endregion static
    }
}