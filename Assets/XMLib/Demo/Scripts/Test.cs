using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AppEntryTest.Pool.Pop("Capsule");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject obj = GameObject.Find("Capsule");
            if (obj != null)
            {
                AppEntryTest.Pool.Push(obj);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AppEntryTest.Pool.Clear();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AppEntryTest.UI.ShowPanel("Panel1", "MMP");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            AppEntryTest.UI.ShowPanel("Panel2");
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            AppEntryTest.UI.HidePanel();
        }
    }
}