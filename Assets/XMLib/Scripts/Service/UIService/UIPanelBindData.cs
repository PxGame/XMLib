/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/18/2019 4:52:45 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;

namespace XMLib.UIService
{
    /// <summary>
    /// 面板绑定数据
    /// </summary>
    public class UIPanelBindData : IUIPanelBindData
    {
        /// <summary>
        /// 面板Id
        /// </summary>
        public Guid Id { get; protected set; }

        /// <summary>
        /// 层名字
        /// </summary>
        public string layerName { get; protected set; }

        /// <summary>
        /// 面板名字
        /// </summary>
        public string panelName { get; protected set; }

        /// <summary>
        /// 是否在堆栈
        /// </summary>
        public bool inStack { get; protected set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="layerName">层名字</param>
        /// <param name="panelName">面板名字</param>
        /// <param name="isStack">是否在堆栈中</param>
        public UIPanelBindData(string layerName, string panelName, bool isStack)
        {
            Id = Guid.NewGuid();

            this.layerName = layerName;
            this.panelName = panelName;
            this.inStack = inStack;
        }
    }
}