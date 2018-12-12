/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/12/2018 3:50:09 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;

namespace XMLib.Test
{
    public class ServiceProviders1 : IServiceProvider
    {
        [Priority(3)]
        public void Init()
        {
            App.Log("ServiceProviders1 Inited");
        }

        public void Register()
        {
            App.Log("ServiceProviders1 Registered");
        }
    }

    public class ServiceProviders2 : IServiceProvider
    {
        [Priority(2)]
        public void Init()
        {
            App.Log("ServiceProviders2 Inited");
        }

        public void Register()
        {
            App.Log("ServiceProviders2 Registered");
        }
    }
}