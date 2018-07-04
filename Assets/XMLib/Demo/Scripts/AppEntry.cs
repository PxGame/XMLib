using System;
using System.Collections;
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
            GameObject obj = Get<PoolService>().Pop("123");
            if (obj == null)
            {
                obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                PoolItem item = obj.AddComponent<PoolItem>();
                obj.name = "123";
                item.PoolName = "123";
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject obj = GameObject.Find("123");
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