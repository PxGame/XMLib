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
        private List<ServiceSetting> _settings = new List<ServiceSetting>();

        private void OnEnable()
        {
            _settings.Add(new InputDriver.InputDriverSetting());
            _settings.Add(new MonoDriver.MonoDriverSetting());
            _settings.Add(new ObjectPool.ObjectPoolSetting());
            _settings.Add(new UIDriver.UIDriverSetting());
        }

        /// <summary>
        /// 获取迭代器
        /// </summary>
        /// <returns>迭代器</returns>
        public IEnumerator GetEnumerator()
        {
            return _settings.GetEnumerator();
        }
    }
}