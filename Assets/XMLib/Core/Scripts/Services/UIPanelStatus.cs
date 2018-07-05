using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// 面板状态
    /// </summary>
    public enum UIPanelStatus
    {
        Enter = 0x01,
        Leave = 0x02,
        Pause = 0x04,
        Resume = 0x08,

        Entering = 0x10,
        Leaving = 0x20,
        Pausing = 0x40,
        Resuming = 0x80,
    }
}