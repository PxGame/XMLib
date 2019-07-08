/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/15/2019 12:20:58 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// 对象池接口
    /// </summary>
    public interface IObjectPool : IDisposable
    {
        /// <summary>
        /// 弹出
        /// </summary>
        /// <param name="typeName">类型名</param>
        /// <param name="objectName">对象名</param>
        /// <returns></returns>
        GameObject Pop(string typeName, string objectName);

        /// <summary>
        /// 压入
        /// </summary>
        /// <param name="typeName">类型名</param>
        /// <param name="objectName">对象名</param>
        /// <param name="obj">对象实例</param>
        /// <returns>是否压入成功,失败将直接删除</returns>
        bool Push(string typeName, string objectName, GameObject obj);

        /// <summary>
        /// 压入
        /// </summary>
        /// <param name="item">对象池元素</param>
        /// <returns>是否压入成功,失败将直接删除或者传入的对象为null</returns>
        bool Push(IObjectPoolItem item);

        /// <summary>
        /// 压入，前提是对象上有实现IObjectPoolItem的组件
        /// </summary>
        /// <param name="obj">对象池元素</param>
        /// <returns>是否压入成功,失败将直接删除或者传入的对象为null</returns>
        bool Push(GameObject obj);

        /// <summary>
        /// 清理
        /// </summary>
        void Clear();

        /// <summary>
        /// 清理指定类型
        /// </summary>
        /// <param name="typeName">类型名</param>
        void Clear(string typeName);

        /// <summary>
        /// 清理指定类型且指定名字
        /// </summary>
        /// <param name="typeName">类型名</param>
        /// <param name="objectName">对象名</param>
        void Clear(string typeName, string objectName);

        /// <summary>
        /// 获取指定类型且指定名字的对象数量
        /// </summary>
        /// <param name="typeName">类型名</param>
        /// <param name="objectName">对象名</param>
        /// <returns>数量</returns>
        int GetCount(string typeName, string objectName);

        /// <summary>
        /// 存在对象
        /// </summary>
        /// <param name="typeName">类型名</param>
        /// <param name="objectName">对象名</param>
        /// <returns>是否存在</returns>
        bool HasObj(string typeName, string objectName);
    }
}