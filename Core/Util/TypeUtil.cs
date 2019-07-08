/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 2019/4/21 0:51:17
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// 类型工具
    /// </summary>
    public class TypeUtil
    {
        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="fullName">类型名</param>
        /// <returns>类型</returns>
        public static Type GetType(string fullName)
        {
            return Type.GetType(fullName, false, true);
        }

        /// <summary>
        ///  创建实例
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        /// <param name="fullName">类型名</param>
        /// <returns>实例</returns>
        public static T NewObject<T>(string fullName)
        {
            Type type = GetType(fullName);

            object obj = Activator.CreateInstance(type);

            return (T)obj;
        }

        /// <summary>
        /// 查找所有子类
        /// </summary>
        /// <typeparam name="T">父类型</typeparam>
        /// <returns>所有类型</returns>
        public static List<Type> FindAllChildType<T>()
        {
            Type rootType = typeof(T);
            return FindAllChildType(rootType);
        }

        /// <summary>
        /// 查找所有子类
        /// </summary>
        /// <param name="rootType">父类型</param>
        /// <returns>所有类型</returns>
        public static List<Type> FindAllChildType(Type rootType)
        {
            List<Type> childs = new List<Type>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (rootType.IsAssignableFrom(type))
                    {
                        if (type != rootType && type.IsClass && !type.IsAbstract)
                        {
                            childs.Add(type);
                        }
                    }
                }
            }

            return childs;
        }
    }
}