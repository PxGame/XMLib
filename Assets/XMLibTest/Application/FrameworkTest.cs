/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/12/2018 4:18:47 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;

namespace XMLib.Test
{
    public class FrameworkTest : Framework
    {
        protected override void OnBootstraped()
        {
            base.OnBootstraped();

            //throw new Exception("OnBootstraped 异常");
        }
    }
}