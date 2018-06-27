using System;
using System.Reflection;

namespace XM.Services
{
    /// <summary>
    /// 方法数据接口
    /// </summary>
    public interface IMethodData
    {
        /// <summary>
        /// 方法目标，静态方法可为null
        /// </summary>
        Object MethodTarget { get; }

        /// <summary>
        /// 方法信息
        /// </summary>
        MethodInfo MethodInfo { get; }
    }
}