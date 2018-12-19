/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/12/2018 10:03:51 AM
 */

using NUnit.Framework;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace XMLib.Test.DispatcherTest
{
    public class TestRunner
    {
        [Test]
        public void Run()
        {
            Application app = new Application();
            app.Bootstrap();
            app.Init();

            int events = 100;
            int cnt = 50;
            for (int i = 0; i < events; i++)
            {
                for (int j = 0; j < cnt; j++)
                {
                    App.On("" + i, () =>
                    {
                    });
                }
            }
            Stopwatch watch = Stopwatch.StartNew();

            Random rand = new Random();
            int length = 10000;
            for (int i = 0; i < length; i++)
            {
                int eventIndex = rand.Next(0, events - 1);
                App.Trigger("" + eventIndex);
            }

            watch.Stop();

            float sec = watch.ElapsedMilliseconds;
            float freq = sec / length;
            float singlefreq = freq / cnt;

            Debug.LogFormat("{4} 种事件每种注册{5}个  随机执行 {0} 次事件 共 {1} ms 平均每次 {2} ms 单个事件 {3} ms", length, sec, freq, singlefreq, events, cnt);
        }
    }
}