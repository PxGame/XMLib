using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XM;
using XM.Services;

public class AppEntryTest : AppEntry
{
    protected override ServiceTypeList GetDefaultServices()
    {
        var s = base.GetDefaultServices();
        s.Add<TestService>();
        s.Add<TestService2>();
        return s;
    }
}