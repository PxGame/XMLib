using UnityEngine.SceneManagement;

namespace XM.Services.Scene
{
    /// <summary>
    /// 场景服务
    /// </summary>
    /// <typeparam name="AE">程序入口类型</typeparam>
    public class SceneService : SimpleService<AppEntry, SceneSetting>
    {
        #region Operation

        public void Load(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        #endregion Operation
    }
}