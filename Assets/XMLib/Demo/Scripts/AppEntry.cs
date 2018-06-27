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
        };
    }

    private List<IEventData> eventDatas = new List<IEventData>();

    private void OnTest(string msg)
    {
        throw new Exception("hahaha");
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Get<TaskService>().RunOnMainThread(() =>
            {
                Debug("Run !");
            });
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            IEventData eventData = Get<EventService>().Add<string>("Test1", OnTest, 3);

            eventDatas.Add(eventData);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            int length = eventDatas.Count;
            for (int i = 0; i < length; i++)
            {
                Get<EventService>().Remove(eventDatas[i]);
            }
            eventDatas.Clear();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Get<EventService>().Remove(this);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Get<EventService>().Call("Test1", "XM");
        }
    }
}