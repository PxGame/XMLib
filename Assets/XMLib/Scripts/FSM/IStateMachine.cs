/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/9/2019 12:06:03 AM
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace XMLib.FSM
{
    /// <summary>
    /// 状态机接口
    /// </summary>
    public interface IStateMachine<T>
    {
        /// <summary>
        /// 当前状态
        /// </summary>
        IState<T> currentState { get; }

        /// <summary>
        /// 上一个状态
        /// </summary>
        IState<T> previousState { get; }

        /// <summary>
        /// 全局状态
        /// </summary>
        IState<T> globalState { get; }

        /// <summary>
        /// 目标对象
        /// </summary>
        /// <value></value>
        T target { get; }

        /// <summary>
        /// 初始化前一个状态
        /// </summary>
        /// <param name="state">状态</param>
        void SetPreviousState (IState<T> state);

        /// <summary>
        /// 初始化当前状态
        /// </summary>
        /// <param name="state">状态</param>
        void SetCurrentState (IState<T> state);

        /// <summary>
        /// 初始化全局状态
        /// </summary>
        /// <param name="state">状态</param>
        void SetGlobalState (IState<T> state);

        /// <summary>
        /// 状态更新
        /// </summary>
        void Update ();

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="state">状态</param>
        void ChangeState (IState<T> state);

        /// <summary>
        /// 改变状态回前一个状态
        /// </summary>
        void RevertToPreviousState ();

        /// <summary>
        /// 当前状态判断
        /// </summary>
        /// <param name="state">状态</param>
        /// <returns></returns>
        bool IsInState (IState<T> state);
    }
}