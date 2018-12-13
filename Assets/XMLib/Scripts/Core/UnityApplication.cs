/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/12/2018 12:14:15 PM
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// Unity 程序
    /// </summary>
    public class UnityApplication : Application
    {
        /// <summary>
        /// Unity 对象
        /// </summary>
        private readonly MonoBehaviour _behaviour;

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="behaviour"></param>
        public UnityApplication(MonoBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {//使用Unity的协程
            _behaviour.StartCoroutine(CoroutineInit());
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="serviceProvider">服务实例</param>
        public override void Register(IServiceProvider serviceProvider)
        {//使用Unity的协程
            _behaviour.StartCoroutine(CoroutineRegister(serviceProvider));
        }
    }
}