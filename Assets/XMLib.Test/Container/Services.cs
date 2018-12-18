/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/18/2018 12:14:02 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace XMLib.Test.ContainerTest
{
    public class Service1 : IDisposable
    {
        public void Dispose()
        {
            Debug.LogFormat("{0} 被释放了，呜呜呜...", this);
        }

        public override string ToString()
        {
            return "我是" + GetType().Name;
        }
    }

    public class Service2 : IDisposable
    {
        public void Dispose()
        {
            Debug.LogFormat("{0} 被释放了，呜呜呜...", this);
        }

        public override string ToString()
        {
            return "我是" + GetType().Name;
        }
    }
}