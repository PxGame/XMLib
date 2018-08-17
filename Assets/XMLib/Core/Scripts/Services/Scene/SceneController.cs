using System.Collections;
using UnityEngine;

namespace XM.Services.Scene
{
    /// <summary>
    /// 场景控制
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns>迭代器</returns>
        public virtual IEnumerator Initialize()
        {
            yield break;
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <returns>迭代器</returns>
        public virtual IEnumerator Dispose()
        {
            yield break;
        }
    }
}