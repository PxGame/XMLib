/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/15/2019 12:33:32 PM
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// 应用设置
    /// </summary>
    [CreateAssetMenu(menuName = "XMLib/Framework Setting", fileName = "FrameworkSetting")]
    [Serializable]
    public class FrameworkSetting : ScriptableObject, IEnumerable
    {
        [SerializeField]
        private InputService.InputServiceSetting InputService;

        [SerializeField]
        private MonoDriver.MonoDriverSetting MonoDriver;

        [SerializeField]
        private ObjectPool.ObjectPoolSetting ObjectPool;

        [SerializeField]
        private UIService.UIServiceSetting UIService;

        /// <summary>
        /// 获取迭代器
        /// </summary>
        /// <returns>迭代器</returns>
        public IEnumerator GetEnumerator()
        {
            List<IServiceSetting> settings = new List<IServiceSetting>();

            if (InputService.enable)
            {
                settings.Add(InputService);
            }
            if (MonoDriver.enable)
            {
                settings.Add(MonoDriver);
            }
            if (ObjectPool.enable)
            {
                settings.Add(ObjectPool);
            }
            if (UIService.enable)
            {
                settings.Add(UIService);
            }

            return settings.GetEnumerator();
        }
    }
}