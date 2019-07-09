/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/17/2019 11:54:46 AM
 */

namespace XMLib
{
    /// <summary>
    /// 事件工具
    /// </summary>
    public static class EventUtil
    {
        #region UI 相关

        /// <summary>
        /// 按钮抬起事件名
        /// </summary>
        /// <param name="buttonName">按钮名</param>
        /// <returns>完整事件名</returns>
        public static string UIButtonUp(string buttonName)
        {
            return "UI.Button." + buttonName + ".Up";
        }

        /// <summary>
        /// 按钮按下事件名
        /// </summary>
        /// <param name="buttonName">按钮名</param>
        /// <returns>完整事件名</returns>
        public static string UIButtonDown(string buttonName)
        {
            return "UI.Button." + buttonName + ".Down";
        }

        /// <summary>
        /// 按钮点击事件名
        /// </summary>
        /// <param name="buttonName">按钮名</param>
        /// <returns>完整事件名</returns>
        public static string UIButtonClick(string buttonName)
        {
            return "UI.Button." + buttonName + ".Click";
        }

        #endregion UI 相关
    }
}