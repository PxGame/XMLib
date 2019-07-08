/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/20/2018 12:04:36 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace XMLib.MonoDriver
{
    /// <summary>
    /// Mono 驱动脚本
    /// </summary>
    [DefaultExecutionOrder(-5000)]
    public sealed class DriverBehaviour : MonoBehaviour
    {
        private MonoDriver _driver;

        /// <summary>
        /// 设置驱动器
        /// </summary>
        /// <param name="monoDriver">驱动器</param>
        public void SetDriver(MonoDriver monoDriver)
        {
            _driver = monoDriver;
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 如果启用 MonoBehaviour，则每个固定帧速率的帧都将调用此函数
        /// </summary>
        private void FixedUpdate()
        {
            if (null != _driver)
            {
                _driver.FixedUpdate();
            }
        }

        /// <summary>
        /// 如果启用 Behaviour，则在每一帧都将调用 LateUpdate
        /// </summary>
        private void LateUpdate()
        {
            if (null != _driver)
            {
                _driver.LateUpdate();
            }
        }

        /// <summary>
        /// 当 MonoBehaviour 将被销毁时调用此函数
        /// </summary>
        private void OnDestroy()
        {
            if (null != _driver)
            {
                _driver.OnDestroy();
            }
        }

        /// <summary>
        /// 渲染和处理 GUI 事件时调用 OnGUI
        /// </summary>
        private void OnGUI()
        {
            if (null != _driver)
            {
                _driver.OnGUI();
            }
        }

        /// <summary>
        /// 如果 MonoBehaviour 已启用，则在每一帧都调用 Update
        /// </summary>
        private void Update()
        {
            if (null != _driver)
            {
                _driver.Update();
            }
        }
    }
}