﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;
using XMLib.MonoDriver;
using XMLib.ObjectPool;
using XMLib.UIService;
using IServiceProvider = XMLib.IServiceProvider;

/// <summary>
/// 游戏管理器
/// </summary>
[DefaultExecutionOrder(-9999)]
public class GameManager : Framework
{
    private int index = 0;
    private Stack<GameObject> objs = new Stack<GameObject>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject obj = App.Make<IObjectPool>().Pop("", "");
            if (obj == null)
            {
                obj = new GameObject("" + index++);
            }
            objs.Push(obj);
            Debug.Log("Pop > " + obj);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            GameObject obj = null;
            if (objs.Count > 0)
            {
                obj = objs.Pop();
            }

            bool b = App.Make<IObjectPool>().Push("", "", obj);
            Debug.Log("Push > " + b);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            bindData = App.Make<IUIService>().Show("Input", "InputPanel", false, (status, bindData) =>
            {
                Debug.LogFormat("[{1}]{0}:Frame={2}", status, bindData, Time.frameCount);
            });
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            App.Make<IUIService>().Hide(bindData, (status, bindData) =>
            {
                Debug.LogFormat("[{1}]{0}:Frame={2}", status, bindData, Time.frameCount);
            });
        }
    }

    private IUIPanelBindData bindData;

    protected override void OnStartCompleted()
    {
        base.OnStartCompleted();

        bindData = App.Make<IUIService>().Show("Input", "InputPanel", false, (status, bindData) =>
        {
            Debug.LogFormat("[{1}]{0}:Frame={2}", status, bindData, Time.frameCount);
        });
    }
}