/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 2019/7/8 22:13:30
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace XMLib
{
    public class AssemblyUtility
    {
        public static List<Type> FindAllSubclass<T>()
        {
            List<Type> results = new List<Type>();

            Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblys)
            {
                Type[] types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (!type.IsAbstract && type.IsSubclassOf(typeof(T)))
                    {
                        results.Add(type);
                    }
                }
            }

            return results;
        }
    }
}