using System;
using System.Collections.Generic;
using UnityEngine;
using XM;
using XM.Services;

[DefaultExecutionOrder(-500)]
public class AppEntry : IAppEntry
{
    protected override List<Type> DefaultServices()
    {
        return new List<Type>()
        {
            typeof(TaskService),
            typeof(EventService),
            typeof(PoolService),
            typeof(UIService),
        };
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Get<PoolService>().Pop("Capsule");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject obj = GameObject.Find("Capsule");
            if (obj != null)
            {
                Get<PoolService>().Push(obj);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Get<PoolService>().Clear();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Get<UIService>().ShowPanel("Panel1", false, () => { Debug(DebugType.Normal, "show Panel1 complete"); }, "MMP");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Get<UIService>().ShowPanel("Panel2", false, () => { Debug(DebugType.Normal, "show Panel2 complete"); });
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Get<UIService>().HidePanel(false, () => { Debug(DebugType.Normal, "hide Panel complete"); });
        }
    }
}