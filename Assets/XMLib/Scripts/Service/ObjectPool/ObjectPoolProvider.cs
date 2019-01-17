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
        /// 对象池最大大小，超过时直接删除
        /// </summary>
        private int _maxSize;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="maxSize">对象池最大大小，超过时直接删除</param>
        public ObjectPoolProvider(int maxSize = 10)
        {
            _maxSize = maxSize;
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
                .OnAfterResolving(
                    (instance) =>
                        {
                            ObjectPool objectPool = (ObjectPool)instance;

                            //初始化
                            objectPool.Init(_maxSize);
                        }
                );
        }
    }
}