/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/12/2018 10:03:51 AM
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XMLib.Test
{
    public class DispatcherTest : MonoBehaviour
    {
        private Dispatcher dispatcher = new Dispatcher();

        private IEvent _test;

        private void Awake()
        {
            dispatcher.On("1", this, "OnAction");
            dispatcher.On<string>("2", OnAction);
            dispatcher.On("3", this, "OnFunc");
            dispatcher.On("3", this, "OnFunc");
            dispatcher.Listen<string, string>("4", OnFunc);
            _test = dispatcher.Listen<string, string>("4", OnFunc);
        }

        private void OnAction(string msg)
        {
            Debug.Log(msg);
        }

        public string OnFunc(string msg)
        {
            return "Func:" + msg;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                dispatcher.Trigger("1", "Test1");
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                dispatcher.Trigger("2", "Test2");
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                List<object> values = dispatcher.Trigger("3", "Test3");
                foreach (var item in values)
                {
                    Debug.Log(item);
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                List<object> values = dispatcher.Trigger("4", "Test4");
                foreach (var item in values)
                {
                    Debug.Log(item);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                object obj = dispatcher.TriggerHalt("3", "Test5");
                Debug.Log(obj);
                object obj2 = dispatcher.TriggerHalt("4", "Test6");
                Debug.Log(obj2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                dispatcher.Off("3");
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                dispatcher.Off(_test);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                dispatcher.Off(this);
            }
        }
    }
}