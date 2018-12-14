/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/13/2018 11:34:11 AM
 */

using System;
using System.Reflection;

namespace XMLib
{
    /// <summary>
    /// 依赖注入容器
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// 绑定服务
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <param name="concrete">服务类型</param>
        /// <param name="isStatis">是否静态化</param>
        /// <returns>绑定数据</returns>
        IBindData Bind(string service, Type concrete, bool isStatis);

        /// <summary>
        /// 绑定一个服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实体</param>
        /// <param name="isStatic">服务是否静态化</param>
        /// <returns>服务绑定数据</returns>
        IBindData Bind(string service, Func<IContainer, object[], object> concrete, bool isStatic);

        /// <summary>
        /// 解绑服务
        /// </summary>
        /// <param name="service">服务名或别名</param>
        void UnBind(string service);

        /// <summary>
        /// 是否已绑定服务
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>是否已绑定服务</returns>
        bool HasBind(string service);

        /// <summary>
        /// 是否可以生成服务
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>是否可以生成服务</returns>
        bool CanMake(string service);

        /// <summary>
        /// 是否是静态服务
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>是否是静态服务</returns>
        bool IsStatic(string service);

        /// <summary>
        /// 是否是别名
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>是否是别名</returns>
        bool IsAlias(string name);

        /// <summary>
        /// 构造服务
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <param name="args">参数</param>
        /// <returns>实例，失败返回null</returns>
        object Make(string service, params object[] args);

        /// <summary>
        /// 释放静态化实例
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>是否成功</returns>
        bool Release(string service);

        /// <summary>
        /// 静态化一个服务
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <param name="instance">服务实例</param>
        /// <returns>实例</returns>
        object Instance(string service, object instance);

        /// <summary>
        /// 是否已经实例静态化
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>是否已经静态化</returns>
        bool HasInstance(string service);

        /// <summary>
        /// 类型转服务名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string Type2Service(Type type);

        /// <summary>
        /// 清理所有数据
        /// </summary>
        void Flush();

        /// <summary>
        /// 调用函数,函数无参时将清空输入参数以完成调用
        /// </summary>
        /// <param name="target">方法对象</param>
        /// <param name="methodInfo">方法信息</param>
        /// <param name="userParams">用户传入的参数</param>
        /// <returns>方法返回值</returns>
        object Call(object target, MethodInfo methodInfo, params object[] args);
    }
}