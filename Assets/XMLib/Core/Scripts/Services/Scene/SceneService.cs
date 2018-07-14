using UnityEngine.SceneManagement;

namespace XM.Services
{
    /// <summary>
    /// 场景服务
    /// </summary>
    /// <typeparam name="AE">程序入口类型</typeparam>
    public class SceneService<AE> : SimpleService<AE, SceneSetting> where AE : IAppEntry<AE>
    {
        #region Base

        protected override void OnAddService()
        {
        }

        protected override void OnInitService()
        {
        }

        protected override void OnRemoveService()
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