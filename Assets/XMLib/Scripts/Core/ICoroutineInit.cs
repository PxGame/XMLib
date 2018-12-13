/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/13/2018 3:42:17 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;

namespace XMLib
{
    /// <summary>
    /// 协同初始化
    /// </summary>
    public interface ICoroutineInit
    {
        /// <summary>
        /// 服务提供者初始化
        /// </summary>
        /// <returns>迭代器</returns>
        IEnumerator CoroutineInit();
    }
}