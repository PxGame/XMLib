/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/17/2018 4:33:01 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;

namespace XMLib
{
    /// <summary>
    /// 被绑定的对象
    /// </summary>
    public interface IBindable
    {
        /// <summary>
        /// 当前绑定的名字
        /// </summary>
        string Service { get; }

        /// <summary>
        /// 移除绑定
        /// <para>如果进行的是服务绑定 , 那么在解除绑定时如果是静态化物体将会触发释放</para>
        /// </summary>
        void Unbind();
    }
}