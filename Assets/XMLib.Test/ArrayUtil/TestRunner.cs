/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/19/2018 11:44:37 AM
 */

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using UnityEngine;

namespace XMLib.Test.ArrayUtilTest
{
    public class TestRunner
    {
        [Test]
        public void Run()
        {
            int[] source = new int[]
            {
                1,2,3,4,5,6,7,8,9
            };

            Log("source", source);

            int[] split = ArrayUtil.Splice(ref source, 0, 1);
            Log("split[0,1]", split);
            Log("source", source);
            int[] split2 = ArrayUtil.Splice(ref source, 2, 3);
            Log("split2[2,3]", split2);
            Log("source", source);
            int remove1 = ArrayUtil.RemoveAt(ref source, 2);
            Log("remove1[2]", remove1);
            Log("source", source);

            int[] split3 = ArrayUtil.Splice(ref source, 2, -2);
            Log("split3[2,-2]", split3);
            Log("source", source);

            int[] split4 = ArrayUtil.Splice(ref source, 1, -2);
            Log("split4[2,-2]", split4);
            Log("source", source);
        }

        private void Log<T>(string title, T[] source)
        {
            string str = "";

            foreach (var item in source)
            {
                str += " " + item;
            }

            Debug.Log(title + ":" + str);
        }

        private void Log(string title, object obj)
        {
            Debug.Log(title + ":" + obj);
        }

        [UnityTest]
        public IEnumerator RunSync()
        {
            yield break;
        }
    }
}