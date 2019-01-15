/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/15/2019 12:33:32 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// 应用设置
    /// </summary>
    [CreateAssetMenu(menuName = "XMLib/Framework Setting", fileName = "FrameworkSetting")]
    [Serializable]
    public class FrameworkSetting : ScriptableObject
    {
        [SerializeField]
        private List<ServiceSetting> _settings;

        private Dictionary<Type, ServiceSetting> _settingDict;

        private void Init()
        {
            if (null != _settingDict)
            {
                return;
            }

            _settingDict = new Dictionary<Type, ServiceSetting>();

            foreach (var setting in _settings)
            {
                _settingDict.Add(setting.GetType(), setting);
            }
        }

        public T Get<T>() where T : ServiceSetting
        {
            Init();
            return (T)_settingDict[typeof(T)];
        }
    }
}