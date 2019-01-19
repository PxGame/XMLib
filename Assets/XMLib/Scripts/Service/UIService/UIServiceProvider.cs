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

        /// <summary>
        /// 构造函数
        /// </summary>
        public UIServiceProvider()
        {
            _setting = App.Make<UIServiceSetting>();
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
            //加载预制
            ResourceRequest req = Resources.LoadAsync<GameObject>(_setting.uiRootPath);
            yield return req;

            //获取预制
            GameObject preObj = (GameObject)req.asset;
            Checker.NotNull(preObj);

            //创建预制
            GameObject obj = GameObject.Instantiate(preObj);
            obj.name = "UIRoot";
            GameObject.DontDestroyOnLoad(obj);

            //获取组件
            UIRoot uiRoot = obj.GetComponent<UIRoot>();
            Checker.NotNull(uiRoot);

            //ui根单例
            App.Instance<UIRoot>(uiRoot);

            //创建服务并初始化
            App.Make<UIService>();
        }

        /// <summary>
        /// 当注册服务提供者
        /// </summary>
        public void Register()
        {
            App.Singleton<UIService>()
                .Alias<IUIService>();
        }
    }
}