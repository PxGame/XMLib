/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/15/2019 12:20:28 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace XMLib.ObjectPool
{
    /// <summary>
    /// 对象池提供者
    /// </summary>
    public sealed class ObjectPoolProvider : IServiceProvider
    {
        /// <summary>
        /// 服务设置
        /// </summary>
        //private readonly ObjectPoolSetting _setting;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ObjectPoolProvider()
        {
            //_setting = App.Make<ObjectPoolSetting>();
        }

        /// <summary>
        /// 服务提供者初始化
        /// <para>可设置优先级</para>
        /// </summary>
        [Priority()]
        public void Init()
        {
            GameObject obj = new GameObject("PoolRoot", typeof(PoolRoot));
            PoolRoot poolRoot = obj.GetComponent<PoolRoot>();

            //pool根单例
            App.Instance<PoolRoot>(poolRoot);

            //创建服务并初始化
            App.Make<IObjectPool>();
        }

        /// <summary>
        /// 当注册服务提供者
        /// </summary>
        public void Register()
        {
            App.Singleton<ObjectPool>()
                .Alias<IObjectPool>();
        }
    }
}