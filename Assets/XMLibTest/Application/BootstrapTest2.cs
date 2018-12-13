/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/12/2018 3:44:18 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace XMLib.Test
{
    public class BootstrapTest2 : MonoBehaviour, IBootstrap
    {
        [Priority(2)]
        public void Bootstrap()
        {
            App.Log("BootstrapTest2");
            App.Register(new ServiceProviders1());
        }
    }
}