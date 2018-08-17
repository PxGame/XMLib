using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XM.Services.Scene
{
    /// <summary>
    /// 场景服务
    /// </summary>
    /// <typeparam name="AE">程序入口类型</typeparam>
    public class SceneService : SimpleService<AppEntry, SceneSetting>
    {
        #region 函数

        private Coroutine loadCoroutine;

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName">场景名</param>
        /// <returns>是否开始加载</returns>
        public bool LoadAsync(string sceneName)
        {
            if (null != loadCoroutine)
            {
                return false;
            }

            loadCoroutine = StartCoroutine(_LoadAsync(sceneName));

            return true;
        }

        private IEnumerator _LoadAsync(string sceneName)
        {
            SceneController lastScene = Find<SceneController>();
            if (null != lastScene)
            {//释放场景
                yield return lastScene.Dispose();
            }

            //加载场景
            yield return SceneManager.LoadSceneAsync(sceneName);

            SceneController nextScene = Find<SceneController>();
            if (null != nextScene)
            {//初始化场景
                yield return nextScene.Initialize();
            }

            loadCoroutine = null;
        }

        /// <summary>
        /// 查找控制器
        /// </summary>
        /// <typeparam name="T">控制器类型</typeparam>
        /// <returns>控制器</returns>
        public T Find<T>() where T : SceneController
        {
            T controller = GameObject.FindObjectOfType<T>();
            return controller;
        }

        #endregion 函数
    }
}