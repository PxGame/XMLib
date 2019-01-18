/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/15/2019 12:20:28 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;

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
        private ObjectPoolSetting _setting;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="setting">服务设置</param>
        public ObjectPoolProvider(ObjectPoolSetting setting)
        {
            _setting = setting;
        }

        /// <summary>
        /// 服务提供者初始化
        /// <para>可设置优先级</para>
        /// </summary>
        [Priority()]
        public void Init()
        {
            App.Make<IObjectPool>();
        }

        /// <summary>
        /// 当注册服务提供者
        /// </summary>
        public void Register()
        {
            App.Singleton<ObjectPool>()
                .Alias<IObjectPool>()
                .OnAfterResolving<ObjectPool>(OnAfterResolving);
        }

        /// <summary>
        /// 实例创建后
        /// </summary>
        /// <param name="instance">实例</param>
        private void OnAfterResolving(ObjectPool instance)
        {
            //初始化
            instance.Init(_setting.maxSize);
        }
    }
}