using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;
using XMLib.MonoDriver;
using IServiceProvider = XMLib.IServiceProvider;

public class GameManager : Framework
{
    public override void Bootstrap()
    {
        base.Bootstrap();

        LoadProvider();
    }

    private void LoadProvider()
    {
        foreach (IServiceProvider provider in new IServiceProvider[] {
            new MonoDriverProvider()
        })
        {
            App.Register(provider);
        }
    }
}
