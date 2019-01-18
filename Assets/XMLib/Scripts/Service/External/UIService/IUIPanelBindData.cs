/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/18/2019 5:00:34 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;

namespace XMLib
{
    /// <summary>
    /// 面板绑定数据
    /// </summary>
    public interface IUIPanelBindData
    {
        /// <summary>
        /// 面板Id
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 层名字
        /// </summary>
        string layerName { get; }

        /// <summary>
        /// 面板名字
        /// </summary>
        string panelName { get; }

        /// <summary>
        /// 是否在堆栈
        /// </summary>
        bool inStack { get; }
    }
}