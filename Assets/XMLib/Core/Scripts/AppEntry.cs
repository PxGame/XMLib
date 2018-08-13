using XM.Services;
using XM.Services.Event;
using XM.Services.Input;
using XM.Services.Localization;
using XM.Services.Pool;
using XM.Services.Scene;
using XM.Services.Task;
using XM.Services.UI;

namespace XM
{
    /// <summary>
    /// 应用入口
    /// </summary>
    public class AppEntry : IAppEntry
    {
        #region 重写

        /// <summary>
        /// 初始化默认服务列表
        /// </summary>
        /// <returns></returns>
        protected override ServiceTypeList GetDefaultServices()
        {
            ServiceTypeList services = base.GetDefaultServices();

            services.Add<EventService>();
            services.Add<PoolService>();
            services.Add<TaskService>();
            services.Add<UIService>();
            services.Add<SceneService>();
            services.Add<InputService>();
            services.Add<LocalizationService>();

            //默认服务
            return services;
        }

        #endregion 重写
    }
}