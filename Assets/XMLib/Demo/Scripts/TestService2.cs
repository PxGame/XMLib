using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XM;
using XM.Services;

public class TestService2 : BaseService<AppEntryTest>, IUpdate, IFixedUpdate, IOnGUI, ILateUpdate
{
    public void FixedUpdate()
    {
        Debug(DebugType.Debug, "TestService2 FixedUpdate");
    }

    public void LateUpdate()
    {
        Debug(DebugType.Debug, "TestService2 LateUpdate");
    }

    public void OnGUI()
    {
        Debug(DebugType.Debug, "TestService2 OnGUI");
    }

    [Priority(100)]
    public void Update()
    {
        Debug(DebugType.Debug, "TestService2 Update");
    }
}