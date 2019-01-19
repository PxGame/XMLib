/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/11/2018 11:28:16 AM
 */

using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace XMLib.Test.SortListTest
{
    public class TestRunner
    {
        [Test]
        public void Run()
        {
            SortList<string, int> data = new SortList<string, int>();
            data.Add("6", 6);
            data.Add("3", 3);
            data.Add("7", 3);
            data.Add("2", 20);
            data.Add("5", 100);
            data.Add("4", 40);
            data.Add("1", 1);

            data.Remove("5");
            data.RemoveIndex(0);

            data.forward = false;
            List<string> toData = new List<string>(data);
            data.forward = true;
            List<string> toData2 = data.ToList();

            string sort = "";
            foreach (var item in data)
            {
                sort += item + ",";
            }
            Debug.Log(sort);

            sort = "to:";
            foreach (var item in toData)
            {
                sort += item + ",";
            }
            Debug.Log(sort);

            sort = "to2:";
            foreach (var item in toData2)
            {
                sort += item + ",";
            }
            Debug.Log(sort);

            Debug.LogFormat("value 6 => weight {0}", data.GetWeight("6"));
            Debug.LogFormat("value 3 => index {0}", data.GetIndex("3"));
            Debug.LogFormat("index 0 => value {0}", data.GetValue(0));
        }
    }
}