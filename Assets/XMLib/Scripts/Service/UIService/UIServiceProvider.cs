/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/14/2019 10:10:04 PM
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XMLib.UIService
{
    /// <summary>
    /// UI 驱动提供者
    /// </summary>
    public sealed class UIServiceProvider : IServiceProvider, ICoroutineInit
    {
        /// <summary>
        /// 服务设置
        /// </summary>
        private readonly UIServiceSetting _setting;

        private GameObject _preUIRoot;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="setting">服务设置</param>
        public UIServiceProvider(UIServiceSetting setting)
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
        }

        /// <summary>
        /// 协同初始化
        /// </summary>
        /// <returns>迭代</returns>
        public IEnumerator CoroutineInit()
        {
            ResourceRequest req = Resources.LoadAsync<GameObject>(_setting.uiRootPath);
            yield return req;

            _preUIRoot = (GameObject)req.asset;
            Checker.NotNull(_preUIRoot);

            //创建服务并初始化
            App.Make<UIService>();
        }

        /// <summary>
        /// 当注册服务提供者
        /// </summary>
        public void Register()
        {
            App.Singleton<UIService>()
                .Alias<IUIService>()
                .OnAfterResolving<UIService>(OnAfterResolving);
        }

        /// <summary>
        /// 实例创建后
        /// </summary>
        /// <param name="instance">实例</param>
        private void OnAfterResolving(UIService instance)
        {
            GameObject obj = GameObject.Instantiate(_preUIRoot);
            obj.name = "UIRoot";
            GameObject.DontDestroyOnLoad(obj);

            UIRoot uiRoot = obj.GetComponent<UIRoot>();
            Checker.NotNull(uiRoot);

            instance.Init(uiRoot);
        }
    }
}