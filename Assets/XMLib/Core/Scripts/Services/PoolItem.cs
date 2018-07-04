﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// 对象池物体
    /// </summary>
    public class PoolItem : MonoBehaviour
    {
        /// <summary>
        /// 池名字
        /// </summary>
        public string PoolName { get { return _poolName; } set { _poolName = value; } }

        [SerializeField]
        protected string _poolName = "";

        /// <summary>
        /// 进入对象池
        /// </summary>
        public void EnterPool()
        {
            OnEnterPool();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 离开对象池
        /// </summary>
        public void LeavePool()
        {
            gameObject.SetActive(true);
            OnLeavePool();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void Destory()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// 进入对象池
        /// </summary>
        protected virtual void OnEnterPool()
        {
        }

        /// <summary>
        /// 离开对象池
        /// </summary>
        protected virtual void OnLeavePool()
        {
        }
    }
}