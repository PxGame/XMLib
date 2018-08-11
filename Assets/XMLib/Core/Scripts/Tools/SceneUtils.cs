using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XM.Tools
{
    /// <summary>
    /// 场景工具
    /// </summary>
    public static class SceneUtils
    {
        /// <summary>
        /// 查找场景中的所有组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="scene">场景</param>
        /// <param name="includeInactive">是否包含不活跃物体</param>
        /// <returns></returns>
        public static List<T> FindAllComponent<T>(Scene scene, bool includeInactive = false) where T : UnityEngine.Object
        {
            List<T> cps = new List<T>();

            GameObject[] roots = scene.GetRootGameObjects();
            GameObject obj;
            int length = roots.Length;
            for (int i = 0; i < length; i++)
            {
                obj = roots[i];

                T[] cp = obj.GetComponentsInChildren<T>(includeInactive);

                cps.AddRange(cp);
            }

            return cps;
        }

        /// <summary>
        ///  查找场景中所有的GameObject
        /// </summary>
        /// <param name="scene">场景</param>
        /// <param name="name">GameObject名字</param>
        /// <returns></returns>
        public static List<GameObject> FindAllGameObject(Scene scene, string name)
        {
            List<GameObject> objs = new List<GameObject>();

            GameObject[] roots = scene.GetRootGameObjects();
            GameObject obj;
            int length = roots.Length;
            for (int i = 0; i < length; i++)
            {
                obj = roots[i];

                obj.transform.ForeachChildWithSelf((t) =>
                {
                    GameObject target = t.gameObject;
                    if (0 == string.Compare(target.name, name))
                    {
                        objs.Add(target);
                    }
                });
            }

            return objs;
        }

        /// <summary>
        /// 移动对象到指定场景
        /// </summary>
        /// <param name="scene">指定场景</param>
        /// <param name="obj">对象</param>
        public static void MoveObject(Scene scene, GameObject obj)
        {
            if (obj.scene == scene)
            {//在同一场景，即不用移动
                return;
            }

            obj.transform.parent = null;
            SceneManager.MoveGameObjectToScene(obj, scene);
        }

        /// <summary>
        /// 移动对象到当前场景
        /// </summary>
        /// <param name="obj">对象</param>
        public static void MoveObjectToCurrent(GameObject obj)
        {
            Scene target = SceneManager.GetActiveScene();

            MoveObject(target, obj);
        }
    }
}