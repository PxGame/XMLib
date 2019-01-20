/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/20/2019 12:20:24 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;

namespace XMLib
{
    /// <summary>
    /// 面板操作状态
    /// </summary>
    public enum UIOperationStatus
    {
        PreEnter,
        Enter,
        LateEnter,

        PreLeave,
        Leave,
        LateLeave,

        PreResume,
        Resume,
        LateResume,

        PrePause,
        Pause,
        LatePause,

        Complete
    }
}