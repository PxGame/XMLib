/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/8/2019 11:16:56 PM
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace XMLib.FSM
{
    /// <summary>
    /// 状态机
    /// </summary>
    public class StateMachine<T> : IStateMachine<T>
    {
        /// <summary>
        /// 状态机目标
        /// </summary>
        protected T _target;

        /// <summary>
        /// 当前状态
        /// </summary>
        protected IState<T> _currentState;

        /// <summary>
        /// 上一个状态
        /// </summary>
        protected IState<T> _previousState;

        /// <summary>
        /// 全局状态
        /// </summary>
        protected IState<T> _globalState;

        /// <summary>
        /// 当前状态
        /// </summary>
        public IState<T> CurrentState { get { return _currentState; } }

        /// <summary>
        /// 上一个状态
        /// </summary>
        public IState<T> PreviousState { get { return _previousState; } }

        /// <summary>
        /// 全局状态
        /// </summary>
        public IState<T> GlobalState { get { return _globalState; } }

        /// <summary>
        /// 目标对象
        /// </summary>
        /// <value></value>
        public T Target { get { return _target; } }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="target">状态机目标</param>
        public StateMachine (T target)
        {
            _target = target;
        }

        /// <summary>
        /// 初始化前一个状态
        /// </summary>
        /// <param name="state">状态</param>
        public void SetPreviousState (IState<T> state)
        {
            _previousState = state;
        }

        /// <summary>
        /// 初始化当前状态
        /// </summary>
        /// <param name="state">状态</param>
        public void SetCurrentState (IState<T> state)
        {
            _currentState = state;
        }

        /// <summary>
        /// 初始化全局状态
        /// </summary>
        /// <param name="state">状态</param>
        public void SetGlobalState (IState<T> state)
        {
            _globalState = state;
        }

        /// <summary>
        /// 状态更新
        /// </summary>
        public void Update ()
        {
            if (null != _globalState)
            { //全局状态更新
                _globalState.Update (_target);
            }

            if (null != _currentState)
            { //当前状态更新
                _currentState.Update (_target);
            }
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="state">状态</param>
        public void ChangeState (IState<T> state)
        {
            _previousState = _currentState;
            _currentState.Exit (_target);
            _currentState = state;
            _currentState.Enter (_target);
        }

        /// <summary>
        /// 改变状态回前一个状态
        /// </summary>
        public void RevertToPreviousState ()
        {
            ChangeState (_previousState);
        }

        /// <summary>
        /// 当前状态判断
        /// </summary>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public bool IsInState (IState<T> state)
        {
            return _currentState == state;
        }
    }
}