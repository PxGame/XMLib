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

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName">场景名</param>
        public void Load(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        #endregion 函数
    }
}