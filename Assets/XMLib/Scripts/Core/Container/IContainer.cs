/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/13/2018 11:34:11 AM
 */

using System.Reflection;

namespace XMLib
{
    /// <summary>
    /// 依赖注入容器
    /// </summary>
    public interface IContainer
    {
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