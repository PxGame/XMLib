/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/12/2018 10:03:51 AM
 */

using System;
using NUnit.Framework;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;
using System.Collections;

namespace XMLib.Test
{
    public class DispatcherTest
    {
        [Test]
        public void Test()
        {
            Application app = new Application();
            app.Bootstrap();
            app.Init();

            App.On("5", () =>
            {
            });

            Stopwatch watch = Stopwatch.StartNew();

            int length = 10000;
            for (int i = 0; i < length; i++)
            {
                App.Trigger("5");
            }

            watch.Stop();

            float sec = watch.ElapsedMilliseconds;
            float freq = sec / length;

            Debug.LogFormat("1个事件 执行 {0} 次 共 {1} ms 平均每次 {2} ms", length, sec, freq);
        }
    }
}