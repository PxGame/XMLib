/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/14/2019 10:12:16 PM
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace XMLib.UIService
{
    /// <summary>
    /// UI 服务
    /// </summary>
    public class UIService : IUIService
    {
        /// <summary>
        /// 根节点
        /// </summary>
        private UIRoot _uiRoot;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="uiRoot">根节点</param>
        public void Init(UIRoot uiRoot)
        {
            _uiRoot = uiRoot;
        }
    }
}