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
        private InputService.InputServiceSetting inputService;

        [SerializeField]
        private MonoDriver.MonoDriverSetting monoDriver;

        [SerializeField]
        private ObjectPool.ObjectPoolSetting objectPool;

        [SerializeField]
        private UIService.UIServiceSetting uiService;

        /// <summary>
        /// 获取迭代器
        /// </summary>
        /// <returns>迭代器</returns>
        public IEnumerator GetEnumerator()
        {
            List<IServiceSetting> settings = new List<IServiceSetting>();

            if (inputService.enable)
            {
                settings.Add(inputService);
            }
            if (monoDriver.enable)
            {
                settings.Add(monoDriver);
            }
            if (objectPool.enable)
            {
                settings.Add(objectPool);
            }
            if (uiService.enable)
            {
                settings.Add(uiService);
            }

            return settings.GetEnumerator();
        }
    }
}