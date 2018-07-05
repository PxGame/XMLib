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
    }
}