using System;
using System.Collections.Generic;
using UnityEngine;
using XM;
using XM.Services;

[DefaultExecutionOrder(-500)]
public class AppEntry : IAppEntry
{
    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AppEntry.Pool.Pop("Capsule");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject obj = GameObject.Find("Capsule");
            if (obj != null)
            {
                AppEntry.Pool.Push(obj);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AppEntry.Pool.Clear();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AppEntry.UI.ShowPanel("Panel1", false, () => { Debug(DebugType.Normal, "show Panel1 complete"); }, "MMP");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            AppEntry.UI.ShowPanel("Panel2", false, () => { Debug(DebugType.Normal, "show Panel2 complete"); });
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            AppEntry.UI.HidePanel(false, () => { Debug(DebugType.Normal, "hide Panel complete"); });
        }
    }
}