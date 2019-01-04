/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/4/2019 1:43:52 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// 忽略射线检测范围
    /// </summary>
    public class IgnoreRaycastScope : IDisposable
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        private GameObject _target;

        /// <summary>
        /// 保存的layer
        /// </summary>
        private int _layer;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="target">目标对象</param>
        public IgnoreRaycastScope(GameObject target)
        {
            _target = target;

            _layer = _target.layer;

            //设置为忽略的层
            _target.layer = Physics2D.IgnoreRaycastLayer;
        }

        #region IDisposable Support

        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                if (null != _target)
                {//还原层级
                    _target.layer = _layer;
                }

                disposedValue = true;
            }
        }

        ~IgnoreRaycastScope()
        {
            Dispose(false);
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}