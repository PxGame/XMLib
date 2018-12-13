/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/13/2018 11:31:54 AM
 */

using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace XMLib
{
    /// <summary>
    /// 依赖注入容器
    /// </summary>
    public class Container : IContainer
    {
        /// <summary>
        /// 调用函数,函数无参时将清空输入参数以完成调用
        /// </summary>
        /// <param name="target">方法对象</param>
        /// <param name="methodInfo">方法信息</param>
        /// <param name="userParams">用户传入的参数</param>
        /// <returns>方法返回值</returns>
        public object Call(object target, MethodInfo methodInfo, params object[] args)
        {
            object[] targetArgs = args;
            //获取函数参数
            ParameterInfo[] argInfos = methodInfo.GetParameters();
            if (0 >= argInfos.Length)
            {//函数为无参,忽略输入的参数
                targetArgs = new object[0];
            }

            try
            {
                return methodInfo.Invoke(target, targetArgs);
            }
            catch (Exception ex)
            {
                string msg = string.Format(
                    "<color=red>事件调用异常:目标对象 ({0}) , 调用函数 ({1}) , 声明类型({2})</color>",
                    target,
                    methodInfo,
                    methodInfo.DeclaringType);
                throw new RuntimeException(msg, ex);
            }
        }
    }
}