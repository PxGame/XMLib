/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/14/2018 10:35:30 AM
 */

using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace XMLib.Test.ApplicationTest
{
    public class TestRunner
    {
        [UnityTest]
        public IEnumerator Run()
        {
            GameObject obj = new GameObject();
            obj.AddComponent<FrameworkTest>();

            bool waitFlag = false;
            App.On(ApplicationEvents.OnStartCompleted, () =>
            {
                waitFlag = true;
            });

            yield return new WaitUntil(() => waitFlag);

            int waitFrame = 3;

            while (waitFrame > 0)
            {
                yield return new WaitForEndOfFrame();

                waitFrame--;
            }

            int cnt = 1000000;
            using (var watcher = new TimeWatcher("Service1 Make 1000000 次"))
            {
                while (cnt > 0)
                {
                    App.Make<Service1>();
                    cnt--;
                }
            }
            int cnt2 = 1000000;
            using (var watcher = new TimeWatcher("Service2 Make 1000000 次"))
            {
                while (cnt2 > 0)
                {
                    App.Make<Service2>("123");
                    cnt2--;
                }
            }

            App.UnBind<MonoDriver.MonoDriver>();

            GameObject.Destroy(obj);
        }
    }
}