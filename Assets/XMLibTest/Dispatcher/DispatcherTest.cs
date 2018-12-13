/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/12/2018 10:03:51 AM
 */

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace XMLib.Test
{
    internal class EventTest
    {
        private Dispatcher _dispatcher;

        public EventTest(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            OnFun();
        }

        public void OnFun()
        {
            _dispatcher.On("5", () =>
            {
                throw new System.Exception("GG");
            });
        }
    }

    public class DispatcherTest : MonoBehaviour
    {
        private Dispatcher dispatcher = new Dispatcher();

        private IEvent _test;

        private EventTest t;

        private void Awake()
        {
            dispatcher.On("1", this, "OnAction");
            dispatcher.On<string>("2", OnAction);
            dispatcher.On("3", this, "OnFunc");
            dispatcher.On("3", this, "OnFunc");
            dispatcher.Listen<string, string>("4", OnFunc);
            _test = dispatcher.Listen<string, string>("4", OnFunc);

            //dispatcher.On("5", OnPriority1);
            //dispatcher.On("5", OnPriority2);
            //dispatcher.On("5", OnPriority3);
            //dispatcher.On("5", () =>
            //{
            //    throw new System.Exception("GG");
            //    Debug.Log("OnPriority4");
            //});

            t = new EventTest(dispatcher);

            //dispatcher.On("5", t.OnFun);
        }

        private void OnAction(string msg)
        {
            Debug.Log(msg);
        }

        public string OnFunc(string msg)
        {
            return "Func:" + msg;
        }

        [Priority(1000)]
        private void OnPriority1()
        {
            //Debug.Log("OnPriority1");
        }

        [Priority(1002)]
        private void OnPriority2()
        {
            throw new System.Exception("GG");
            //Debug.Log("OnPriority2");
        }

        [Priority(999)]
        private static void OnPriority3()
        {
            throw new System.Exception("GG");
            //Debug.Log("OnPriority3");
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

            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                Stopwatch watch = Stopwatch.StartNew();

                int length = 1;//0000;
                for (int i = 0; i < length; i++)
                {
                    dispatcher.Trigger("5");
                }

                watch.Stop();

                float sec = watch.ElapsedMilliseconds;
                float freq = sec / length;

                Debug.LogFormat("1个事件 执行 {0} 次 共 {1} ms 平均每次 {2} ms", length, sec, freq);
            }
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                dispatcher.Off("5");
            }
        }
    }
}