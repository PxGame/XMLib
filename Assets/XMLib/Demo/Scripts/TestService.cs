using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XM;
using XM.Services;

public class TestService : BaseService<AppEntryTest>, IUpdate, IFixedUpdate, IOnGUI, ILateUpdate
{
    public void FixedUpdate()
    {
        Debug(DebugType.Debug, "TestService FixedUpdate");
    }

    public void LateUpdate()
    {
        Debug(DebugType.Debug, "TestService LateUpdate");
    }

    public void OnGUI()
    {
        Debug(DebugType.Debug, "TestService OnGUI");
    }

    [Priority(-100)]
    public void Update()
    {
        Debug(DebugType.Debug, "TestService Update");
    }
}