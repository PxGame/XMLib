/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/12/2018 10:03:51 AM
 */

using NUnit.Framework;
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