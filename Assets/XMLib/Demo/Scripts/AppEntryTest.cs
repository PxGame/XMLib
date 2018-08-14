using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XM;
using XM.Services;
using XM.Services.Event;
using XM.Services.Input;
using XM.Services.Localization;
using XM.Services.Pool;
using XM.Services.Scene;
using XM.Services.Task;
using XM.Services.UI;

public class AppEntryTest : AppEntry
{
    protected override ServiceTypeList GetDefaultServices()
    {
        ServiceTypeList s = new ServiceTypeList();
        s.Add<PoolService>();
        s.Add<UIService>();
        s.Add<SceneService>();
        s.Add<InputService>();
        s.Add<LocalizationService>();
        s.Add<EventService>();
        //s.Add<TaskService>();
        s.Add<TestService>();
        //s.Add<TestService2>();
        return s;
    }
}