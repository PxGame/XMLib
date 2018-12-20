/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/20/2018 12:06:54 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;

namespace XMLib.MonoDriver
{
    /// <summary>
    /// 驱动器
    /// </summary>
    public interface IMonoDriver
    {
        /// <summary>
        /// 在主线程中调用
        /// </summary>
        /// <param name="action">协程，执行会处于主线程</param>
        void MainThread(IEnumerator action);

        /// <summary>
        /// 在主线程中调用
        /// </summary>
        /// <param name="action">回调，回调的内容会处于主线程</param>
        void MainThread(Action action);

        /// <summary>
        /// 启动协程
        /// </summary>
        /// <param name="routine">协程</param>
        /// <returns>协程</returns>
        UnityEngine.Coroutine StartCoroutine(IEnumerator routine);

        /// <summary>
        /// 停止协程
        /// </summary>
        /// <param name="routine">协程</param>
        void StopCoroutine(IEnumerator routine);

        /// <summary>
        /// 卸载对象
        /// </summary>
        /// <param name="obj">对象</param>
        void Detach(object obj);

        /// <summary>
        /// 装载对象
        /// </summary>
        /// <param name="obj">对象</param>
        void Attach(object obj);
    }
}