/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/15/2019 2:32:39 PM
 */

using System;
using UnityEngine;

namespace XMLib.ObjectPool
{
    /// <summary>
    /// ObjectPool 服务设置
    /// </summary>
    [Serializable]
    public class ObjectPoolSetting : ServiceSetting
    {
        /// <summary>
        /// 对象池最大大小，超过时直接删除
        /// </summary>
        [SerializeField]
        [Tooltip("对象池最大大小，超过时直接删除")]
        private int _maxSize = 10;

        /// <summary>
        /// 获取服务提供者实例
        /// </summary>
        /// <returns>服务提供者实例</returns>
        public override IServiceProvider NewServiceProvider()
        {
            return new ObjectPoolProvider(_maxSize);
        }
    }
}