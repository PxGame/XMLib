using UnityEngine.SceneManagement;

namespace XM.Services
{
    /// <summary>
    /// 场景服务
    /// </summary>
    /// <typeparam name="AE">程序入口类型</typeparam>
    public class SceneService : SimpleService<AppEntry, SceneSetting>
    {
        #region Base

        protected override void OnCreateService()
        {
        }

        protected override void OnInitService()
        {
        }

        protected override void OnDisposeService()
        {
        }

        protected override void OnClearService()
        {
        }

        #endregion Base

        #region Operation

        public void Load(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        #endregion Operation
    }
}