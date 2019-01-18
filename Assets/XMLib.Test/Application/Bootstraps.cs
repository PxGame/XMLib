/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/14/2018 12:09:49 PM
 */

using UnityEngine;

namespace XMLib.Test.ApplicationTest
{
    public class BootstrapTest1 : IBootstrap
    {
        [Priority(2)]
        public void Bootstrap()
        {
            Debug.LogFormat("BootstrapTest1");
            App.Register(new ServiceProvider2());
        }
    }

    public class BootstrapTest2 : IBootstrap
    {
        [Priority(3)]
        public void Bootstrap()
        {
            Debug.LogFormat("BootstrapTest2");
            App.Register(new ServiceProvider1());
        }
    }
}