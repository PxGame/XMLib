/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/18/2018 12:00:48 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace XMLib.Test.ContainerTest
{
    public class TestRunner
    {
        [Test]
        public void Run()
        {
            string[] services = new string[] {
                "服务1 ",
                "服务2 "
            };

            Container container = new Container();

            container
                .OnResolving((data, obj) => { Debug.Log("Container OnResolving:" + obj); })
                .OnAfterResolving((data, obj) => { Debug.Log("Container OnAfterResolving:" + obj); })
                .OnRelease((data, obj) => { Debug.Log("Container OnRelease:" + obj); });

            container.Bind(services[0], typeof(Service1), false).Alias("1")
                .OnResolving((data, obj) => { Debug.Log("OnResolving:" + obj); })
                .OnAfterResolving((data, obj) => { Debug.Log("OnAfterResolving:" + obj); })
                ;//.OnRelease((data, obj) => { Debug.Log("OnRelease:" + obj); });

            container.Bind(services[1], typeof(Service2), true)
                .OnResolving((data, obj) => { Debug.Log("OnResolving:" + obj); })
                .OnAfterResolving((data, obj) => { Debug.Log("OnAfterResolving:" + obj); })
                .OnRelease((data, obj) => { Debug.Log("OnRelease:" + obj); });

            object obj1 = container.Make("1");
            Debug.Log(obj1);

            object obj3 = container.Make("1", "我是参数");
            Debug.Log(obj3);

            object obj2 = container.Make(services[1]);
            Debug.Log(obj2);

            container.Release(services[1]);
            container.Release(services[0]);

            container.Flush();
        }

        //[UnityTest]
        //public IEnumerator RunSync()
        //{
        //    yield break;
        //}
    }
}