using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XM.Services;

public class TestUIPanel : UIPanel
{
    protected void Initialize(string msg)
    {
        Service.Debug(XM.DebugType.GG, msg);
    }
}