using UnityEngine;

namespace XM.Services.Input
{
    /// <summary>
    /// 输入服务设置
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "XMLib/Input Setting")]
    public class InputSetting : SimpleSetting
    {
        [SerializeField]
        protected ActiveInputMethod _inputMethod;

        /// <summary>
        /// 输入模式
        /// </summary>
        public ActiveInputMethod InputMethod { get { return _inputMethod; } }
    }
}