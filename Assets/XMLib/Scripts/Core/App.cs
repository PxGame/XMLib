/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/11/2018 2:49:00 PM
 */

using System;

namespace XMLib
{
    /// <summary>
    /// 应用程序
    /// </summary>
    public abstract class App
    {
        /// <summary>
        /// 当新建Application时
        /// </summary>
        public static event Action<IApplication> OnNewApplication;

        /// <summary>
        /// 实例
        /// </summary>
        private static IApplication _instance;

        /// <summary>
        ///实例
        /// </summary>
        public static IApplication Handler
        {
            get
            {
                if (_instance == null)
                {
                    return New();
                }
                return _instance;
            }
            set
            {
                _instance = value;
                if (OnNewApplication != null)
                {
                    OnNewApplication.Invoke(_instance);
                }
            }
        }

        /// <summary>
        /// 创建一个实例
        /// </summary>
        /// <returns>实例</returns>
        private static IApplication New()
        {
            return Application.New();
        }
    }
}