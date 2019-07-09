/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/18/2019 4:52:45 PM
 */

using System;

namespace XMLib
{
    /// <summary>
    /// 面板绑定数据
    /// </summary>
    public class UIPanelBindData : IUIPanelBindData
    {
        /// <summary>
        /// 面板Id
        /// </summary>
        public Guid id { get; protected set; }

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
        /// 显示名字
        /// </summary>
        public string DisplayName { get { return string.Format("{0} <{1}>", panelName, id); } }

        /// <summary>
        /// 服务句柄
        /// </summary>
        protected readonly UIService uiService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="service">服务实例</param>
        /// <param name="layerName">层名字</param>
        /// <param name="panelName">面板名字</param>
        /// <param name="inStack">是否在堆栈中</param>
        public UIPanelBindData(UIService service, string layerName, string panelName, bool inStack)
        {
            uiService = service;
            id = Guid.NewGuid();

            this.layerName = layerName;
            this.panelName = panelName;
            this.inStack = inStack;
        }

        /// <summary>
        /// 资源路径
        /// </summary>
        public string path { get { return uiService.GetPanelPath(panelName); } }

        public override string ToString()
        {
            return string.Format("[{0} - {1}](inStack={2}, id={3})", layerName, panelName, inStack, id);
        }
    }
}