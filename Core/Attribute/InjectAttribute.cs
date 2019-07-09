/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/18/2018 5:23:28 PM
 */

using System;

namespace XMLib
{
    /// <summary>
    /// 注入标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface |
                    AttributeTargets.Parameter |
                    AttributeTargets.Property)]
    public class InjectAttribute : Attribute
    {
        /// <summary>
        /// 注入服务的别名或者服务名
        /// </summary>
        public string alias { get; private set; }

        /// <summary>
        /// 声明注入
        /// </summary>
        /// <param name="alias">依赖服务的别名或者服务名</param>
        public InjectAttribute(string alias)
        {
            this.alias = alias;
        }

        /// <summary>
        /// 声明注入
        /// </summary>
        public InjectAttribute() { }
    }
}