/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/20/2018 4:00:17 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;

namespace XMLib
{
    /// <summary>
    /// 容器方法扩展
    /// </summary>
    public static class ContainerExtend
    {
        /// <summary>
        /// 类型转为服务名
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <param name="container">服务容器</param>
        /// <returns>服务名</returns>
        public static string Type2Service<TService>(this IContainer container)
        {
            return container.Type2Service(typeof(TService));
        }

        /// <summary>
        /// 获取服务的绑定数据,如果绑定不存在则返回null
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="container">服务容器</param>
        /// <returns>服务绑定数据或者null</returns>
        public static IBindData GetBind<TService>(this IContainer container)
        {
            return container.GetBind(container.Type2Service(typeof(TService)));
        }

        /// <summary>
        /// 是否已经绑定了服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="container">服务容器</param>
        /// <returns>代表服务是否被绑定</returns>
        public static bool HasBind<TService>(this IContainer container)
        {
            return container.HasBind(container.Type2Service(typeof(TService)));
        }

        /// <summary>
        /// 是否已经实例静态化
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="container">服务容器</param>
        /// <returns>是否已经静态化</returns>
        public static bool HasInstance<TService>(this IContainer container)
        {
            return container.HasInstance(container.Type2Service(typeof(TService)));
        }

        /// <summary>
        /// 服务是否已经被解决过
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="container">服务容器</param>
        /// <returns>是否已经被解决过</returns>
        public static bool IsResolved<TService>(this IContainer container)
        {
            return container.IsResolved(container.Type2Service(typeof(TService)));
        }

        /// <summary>
        /// 是否可以生成服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="container">服务容器</param>
        /// <returns>服务是否可以被构建</returns>
        public static bool CanMake<TService>(this IContainer container)
        {
            return container.CanMake(container.Type2Service(typeof(TService)));
        }

        /// <summary>
        /// 服务是否是静态化的,如果服务不存在也将返回false
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="container">服务容器</param>
        /// <returns>服务是否是静态化的</returns>
        public static bool IsStatic<TService>(this IContainer container)
        {
            return container.IsStatic(container.Type2Service(typeof(TService)));
        }

        /// <summary>
        /// 是否是别名
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="container">服务容器</param>
        /// <returns>是否是别名</returns>
        public static bool IsAlias<TService>(this IContainer container)
        {
            return container.IsAlias(container.Type2Service(typeof(TService)));
        }

        /// <summary>
        /// 为服务设定一个别名
        /// </summary>
        /// <typeparam name="TAlias">别名</typeparam>
        /// <typeparam name="TService">服务名</typeparam>
        public static IContainer Alias<TAlias, TService>(this IContainer container)
        {
            return container.Alias(container.Type2Service(typeof(TAlias)), container.Type2Service(typeof(TService)));
        }

        #region Bind

        /// <summary>
        /// 常规绑定一个服务
        /// </summary>
        /// <typeparam name="TService">服务名，同时也是服务实现</typeparam>
        /// <param name="container">服务容器</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData Bind<TService>(this IContainer container)
        {
            return container.Bind(container.Type2Service(typeof(TService)), typeof(TService), false);
        }

        /// <summary>
        /// 常规绑定一个服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <typeparam name="TAlias">服务别名</typeparam>
        /// <param name="container">服务容器</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData Bind<TService, TAlias>(this IContainer container)
        {
            return container.Bind(container.Type2Service(typeof(TService)), typeof(TService), false)
                .Alias(container.Type2Service(typeof(TAlias)));
        }

        /// <summary>
        /// 常规绑定一个服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="container">服务容器</param>
        /// <param name="concrete">服务实现</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData Bind<TService>(this IContainer container, Func<IContainer, object[], object> concrete)
        {
            Checker.Requires<ArgumentNullException>(concrete != null);
            return container.Bind(container.Type2Service(typeof(TService)), concrete, false);
        }

        /// <summary>
        /// 常规绑定一个服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="container">服务容器</param>
        /// <param name="concrete">服务实现</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData Bind<TService>(this IContainer container, Func<object> concrete)
        {
            return container.Bind(container.Type2Service(typeof(TService)), (c, p) => concrete.Invoke(), false);
        }

        /// <summary>
        /// 常规绑定一个服务
        /// </summary>
        /// <param name="container">服务容器</param>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实现</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData Bind(this IContainer container, string service,
            Func<IContainer, object[], object> concrete)
        {
            return container.Bind(service, concrete, false);
        }

        /// <summary>
        /// 如果服务不存在那么则绑定服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <typeparam name="TAlias">服务别名</typeparam>
        /// <param name="container">服务容器</param>
        /// <param name="bindData">如果绑定失败则返回历史绑定对象</param>
        /// <returns>是否完成绑定</returns>
        public static bool BindIf<TService, TAlias>(this IContainer container, out IBindData bindData)
        {
            if (container.BindIf(container.Type2Service(typeof(TService)), typeof(TService), false, out bindData))
            {
                bindData.Alias(container.Type2Service(typeof(TAlias)));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 如果服务不存在那么则绑定服务
        /// </summary>
        /// <typeparam name="TService">服务名，同时也是服务实现</typeparam>
        /// <param name="container">服务容器</param>
        /// <param name="bindData">如果绑定失败则返回历史绑定对象</param>
        /// <returns>是否完成绑定</returns>
        public static bool BindIf<TService>(this IContainer container, out IBindData bindData)
        {
            return container.BindIf(container.Type2Service(typeof(TService)), typeof(TService), false, out bindData);
        }

        /// <summary>
        /// 如果服务不存在那么则绑定服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="container">服务容器</param>
        /// <param name="concrete">服务实现</param>
        /// <param name="bindData">如果绑定失败则返回历史绑定对象</param>
        /// <returns>是否完成绑定</returns>
        public static bool BindIf<TService>(this IContainer container, Func<IContainer, object[], object> concrete, out IBindData bindData)
        {
            return container.BindIf(container.Type2Service(typeof(TService)), concrete, false, out bindData);
        }

        /// <summary>
        /// 如果服务不存在那么则绑定服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="container">服务容器</param>
        /// <param name="concrete">服务实现</param>
        /// <param name="bindData">如果绑定失败则返回历史绑定对象</param>
        /// <returns>是否完成绑定</returns>
        public static bool BindIf<TService>(this IContainer container, Func<object> concrete, out IBindData bindData)
        {
            Checker.Requires<ArgumentNullException>(concrete != null);
            return container.BindIf(container.Type2Service(typeof(TService)), (c, p) => concrete.Invoke(), false,
                out bindData);
        }

        /// <summary>
        /// 如果服务不存在那么则绑定服务
        /// </summary>
        /// <param name="container">服务容器</param>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实现</param>
        /// <param name="bindData">如果绑定失败则返回历史绑定对象</param>
        /// <returns>是否完成绑定</returns>
        public static bool BindIf(this IContainer container, string service,
            Func<IContainer, object[], object> concrete, out IBindData bindData)
        {
            return container.BindIf(service, concrete, false, out bindData);
        }

        #endregion Bind

        #region Single Bind

        /// <summary>
        /// 静态化一个服务,实例值会经过解决修饰器
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="container">服务容器</param>
        /// <param name="instance">实例值</param>
        public static object Instance<TService>(this IContainer container, object instance)
        {
            return container.Instance(container.Type2Service(typeof(TService)), instance);
        }

        /// <summary>
        /// 以单例的形式绑定一个服务
        /// </summary>
        /// <param name="container">服务容器</param>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实现</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData Singleton(this IContainer container, string service,
            Func<IContainer, object[], object> concrete)
        {
            return container.Bind(service, concrete, true);
        }

        /// <summary>
        /// 以单例的形式绑定一个服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <typeparam name="TAlias">服务别名</typeparam>
        /// <param name="container">服务容器</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData Singleton<TService, TAlias>(this IContainer container)
        {
            return container.Bind(container.Type2Service(typeof(TService)), typeof(TService), true)
                .Alias(container.Type2Service(typeof(TAlias)));
        }

        /// <summary>
        /// 以单例的形式绑定一个服务
        /// </summary>
        /// <typeparam name="TService">服务名，同时也是服务实现</typeparam>
        /// <param name="container">服务容器</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData Singleton<TService>(this IContainer container)
        {
            return container.Bind(container.Type2Service(typeof(TService)), typeof(TService), true);
        }

        /// <summary>
        /// 以单例的形式绑定一个服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="container">服务容器</param>
        /// <param name="concrete">服务实现</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData Singleton<TService>(this IContainer container,
            Func<IContainer, object[], object> concrete)
        {
            return container.Bind(container.Type2Service(typeof(TService)), concrete, true);
        }

        /// <summary>
        /// 以单例的形式绑定一个服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="container">服务容器</param>
        /// <param name="concrete">服务实现</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData Singleton<TService>(this IContainer container,
            Func<object> concrete)
        {
            Checker.Requires<ArgumentNullException>(concrete != null);
            return container.Bind(container.Type2Service(typeof(TService)), (c, p) => concrete.Invoke(), true);
        }

        /// <summary>
        /// 如果服务不存在那么则绑定服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <typeparam name="TAlias">服务别名</typeparam>
        /// <param name="container">服务容器</param>
        /// <param name="bindData">如果绑定失败则返回历史绑定对象</param>
        /// <returns>是否完成绑定</returns>
        public static bool SingletonIf<TService, TAlias>(this IContainer container, out IBindData bindData)
        {
            if (container.BindIf(container.Type2Service(typeof(TService)), typeof(TService), true, out bindData))
            {
                bindData.Alias(container.Type2Service(typeof(TAlias)));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 如果服务不存在那么则绑定服务
        /// </summary>
        /// <typeparam name="TService">服务名，同时也是服务实现</typeparam>
        /// <param name="container">服务容器</param>
        /// <param name="bindData">如果绑定失败则返回历史绑定对象</param>
        /// <returns>是否完成绑定</returns>
        public static bool SingletonIf<TService>(this IContainer container, out IBindData bindData)
        {
            return container.BindIf(container.Type2Service(typeof(TService)), typeof(TService), true, out bindData);
        }

        /// <summary>
        /// 如果服务不存在那么则绑定服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="container">服务容器</param>
        /// <param name="concrete">服务实现</param>
        /// <param name="bindData">如果绑定失败则返回历史绑定对象</param>
        /// <returns>是否完成绑定</returns>
        public static bool SingletonIf<TService>(this IContainer container, Func<IContainer, object[], object> concrete, out IBindData bindData)
        {
            return container.BindIf(container.Type2Service(typeof(TService)), concrete, true, out bindData);
        }

        /// <summary>
        /// 如果服务不存在那么则绑定服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="container">服务容器</param>
        /// <param name="concrete">服务实现</param>
        /// <param name="bindData">如果绑定失败则返回历史绑定对象</param>
        /// <returns>是否完成绑定</returns>
        public static bool SingletonIf<TService>(this IContainer container, Func<object> concrete, out IBindData bindData)
        {
            Checker.Requires<ArgumentNullException>(concrete != null);
            return container.BindIf(container.Type2Service(typeof(TService)), (c, p) => concrete.Invoke(), true,
                out bindData);
        }

        /// <summary>
        /// 如果服务不存在那么则绑定服务
        /// </summary>
        /// <param name="container">服务容器</param>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实现</param>
        /// <param name="bindData">如果绑定失败则返回历史绑定对象</param>
        /// <returns>是否完成绑定</returns>
        public static bool SingletonIf(this IContainer container, string service,
            Func<IContainer, object[], object> concrete, out IBindData bindData)
        {
            return container.BindIf(service, concrete, true, out bindData);
        }

        #endregion Single Bind

        /// <summary>
        /// 解除服务绑定
        /// </summary>
        /// <typeparam name="TService">解除绑定的服务</typeparam>
        /// <param name="container">服务容器</param>
        public static void UnBind<TService>(this IContainer container)
        {
            container.UnBind(container.Type2Service(typeof(TService)));
        }

        /// <summary>
        /// 释放服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="container">服务容器</param>
        public static bool Release<TService>(this IContainer container)
        {
            return container.Release(container.Type2Service(typeof(TService)));
        }

        /// <summary>
        /// 根据实例对象释放静态化实例
        /// </summary>
        /// <param name="container">服务容器</param>
        /// <param name="instances">需要释放静态化实例对象</param>
        /// <param name="reverse">以相反的顺序开始释放</param>
        /// <returns>只要有一个没有释放成功那么返回false, <paramref name="instances"/>为没有释放掉的实例</returns>
        public static bool Release(this IContainer container, ref object[] instances, bool reverse = true)
        {
            if (instances == null || instances.Length <= 0)
            {
                return true;
            }

            if (reverse)
            {
                Array.Reverse(instances);
            }

            var errorIndex = 0;

            for (var index = 0; index < instances.Length; index++)
            {
                if (instances[index] == null)
                {
                    continue;
                }

                if (!container.Release(container.Type2Service(instances[index].GetType())))
                {
                    instances[errorIndex++] = instances[index];
                }
            }

            Array.Resize(ref instances, errorIndex);

            if (reverse && errorIndex > 0)
            {
                Array.Reverse(instances);
            }

            return errorIndex <= 0;
        }

        #region Call

        /// <summary>
        /// 以依赖注入的形式调用一个方法
        /// </summary>
        /// <param name="container">服务容器</param>
        /// <param name="method">方法</param>
        /// <param name="userParams">用户参数</param>
        public static void Call<T1>(this IContainer container, Action<T1> method, params object[] userParams)
        {
            Checker.Requires<ArgumentNullException>(method != null);
            container.Call(method.Target, method.Method, userParams);
        }

        /// <summary>
        /// 以依赖注入的形式调用一个方法
        /// </summary>
        /// <param name="container">服务容器</param>
        /// <param name="method">方法</param>
        /// <param name="userParams">用户参数</param>
        public static void Call<T1, T2>(this IContainer container, Action<T1, T2> method, params object[] userParams)
        {
            Checker.Requires<ArgumentNullException>(method != null);
            container.Call(method.Target, method.Method, userParams);
        }

        /// <summary>
        /// 以依赖注入的形式调用一个方法
        /// </summary>
        /// <param name="container">服务容器</param>
        /// <param name="method">方法</param>
        /// <param name="userParams">用户参数</param>
        public static void Call<T1, T2, T3>(this IContainer container, Action<T1, T2, T3> method, params object[] userParams)
        {
            Checker.Requires<ArgumentNullException>(method != null);
            container.Call(method.Target, method.Method, userParams);
        }

        /// <summary>
        /// 以依赖注入的形式调用一个方法
        /// </summary>
        /// <param name="container">服务容器</param>
        /// <param name="method">方法</param>
        /// <param name="userParams">用户参数</param>
        public static void Call<T1, T2, T3, T4>(this IContainer container, Action<T1, T2, T3, T4> method, params object[] userParams)
        {
            Checker.Requires<ArgumentNullException>(method != null);
            container.Call(method.Target, method.Method, userParams);
        }

        /// <summary>
        /// 以依赖注入形式调用一个方法
        /// </summary>
        /// <param name="container">服务容器</param>
        /// <param name="target">方法对象</param>
        /// <param name="method">方法名</param>
        /// <param name="userParams">用户传入的参数</param>
        /// <returns>方法返回值</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/>,<paramref name="method"/>为<c>null</c>或者空字符串</exception>
        public static object Call(this IContainer container, object target, string method, params object[] userParams)
        {
            Checker.Requires<ArgumentNullException>(target != null);
            Checker.NotEmptyOrNull(method, "method");

            var methodInfo = target.GetType().GetMethod(method);
            return container.Call(target, methodInfo, userParams);
        }

        #endregion Call

        #region Wrap

        /// <summary>
        /// 包装一个依赖注入形式调用的一个方法
        /// </summary>
        /// <param name="container">服务容器</param>
        /// <param name="method">方法</param>
        /// <param name="userParams">用户传入的参数</param>
        /// <returns>包装方法</returns>
        public static Action Wrap<T1>(this IContainer container, Action<T1> method, params object[] userParams)
        {
            return () =>
            {
                if (method != null)
                {
                    container.Call(method.Target, method.Method, userParams);
                }
            };
        }

        /// <summary>
        /// 包装一个依赖注入形式调用的一个方法
        /// </summary>
        /// <param name="container">服务容器</param>
        /// <param name="method">方法</param>
        /// <param name="userParams">用户传入的参数</param>
        /// <returns>包装方法</returns>
        public static Action Wrap<T1, T2>(this IContainer container, Action<T1, T2> method, params object[] userParams)
        {
            return () =>
            {
                if (method != null)
                {
                    container.Call(method.Target, method.Method, userParams);
                }
            };
        }

        /// <summary>
        /// 包装一个依赖注入形式调用的一个方法
        /// </summary>
        /// <param name="container">服务容器</param>
        /// <param name="method">方法</param>
        /// <param name="userParams">用户传入的参数</param>
        /// <returns>包装方法</returns>
        public static Action Wrap<T1, T2, T3>(this IContainer container, Action<T1, T2, T3> method, params object[] userParams)
        {
            return () =>
            {
                if (method != null)
                {
                    container.Call(method.Target, method.Method, userParams);
                }
            };
        }

        /// <summary>
        /// 包装一个依赖注入形式调用的一个方法
        /// </summary>
        /// <param name="container">服务容器</param>
        /// <param name="method">方法</param>
        /// <param name="userParams">用户传入的参数</param>
        /// <returns>包装方法</returns>
        public static Action Wrap<T1, T2, T3, T4>(this IContainer container, Action<T1, T2, T3, T4> method, params object[] userParams)
        {
            return () =>
            {
                if (method != null)
                {
                    container.Call(method.Target, method.Method, userParams);
                }
            };
        }

        #endregion Wrap

        /// <summary>
        /// 构造一个服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="container">服务容器</param>
        /// <param name="userParams">用户提供的参数</param>
        /// <returns>服务实例</returns>
        public static TService Make<TService>(this IContainer container, params object[] userParams)
        {
            return (TService)container.Make(container.Type2Service(typeof(TService)), userParams);
        }

        /// <summary>
        /// 构造一个服务
        /// </summary>
        /// <param name="container">服务容器</param>
        /// <param name="type">服务类型</param>
        /// <param name="userParams">用户提供的参数</param>
        /// <returns>服务实例</returns>
        public static object Make(this IContainer container, Type type, params object[] userParams)
        {
            var service = container.Type2Service(type);
            IBindData binder;
            container.BindIf(service, type, false, out binder);
            return container.Make(service, userParams);
        }

        /// <summary>
        /// 当静态服务被释放时
        /// </summary>
        /// <param name="container">服务容器</param>
        /// <param name="callback">处理释放时的回调</param>
        /// <returns>当前容器实例</returns>
        public static IContainer OnRelease(this IContainer container, Action<object> callback)
        {
            Checker.Requires<ArgumentNullException>(callback != null);
            return container.OnRelease((_, instance) => callback(instance));
        }

        /// <summary>
        /// 当服务被解决时，生成的服务会经过注册的回调函数
        /// </summary>
        /// <param name="container">服务容器</param>
        /// <param name="callback">回调函数</param>
        /// <returns>当前容器对象</returns>
        public static IContainer OnResolving(this IContainer container, Action<object> callback)
        {
            Checker.Requires<ArgumentNullException>(callback != null);
            return container.OnResolving((_, instance) =>
            {
                callback(instance);
            });
        }
    }
}