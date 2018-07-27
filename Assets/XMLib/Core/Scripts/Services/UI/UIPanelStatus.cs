using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// UI面板状态
    /// </summary>
    public enum UIPanelStatus
    {
        Enter = 0x01,
        Leave = 0x02,
        Pause = 0x04,
        Resume = 0x08,
    }
}