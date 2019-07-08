/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/8/2019 11:18:56 PM
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace XMLib.FSM
{
    /// <summary>
    /// 状态接口
    /// </summary>
    public interface IState<T>
    {
        /// <summary>
        /// 进入
        /// </summary>
        /// <param name="target">目标</param>
        void Enter (T target);

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="target">目标</param>
        void Exit (T target);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="target">目标</param>
        void Update (T target);
    }
}