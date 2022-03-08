using UnityEngine;
using Object = UnityEngine.Object;

namespace Utilities
{
    [AddComponentMenu("Utilities/Logger")]
    public class Logger : MonoBehaviour
    {
        #region Variables

        [Header("Settings")] [SerializeField] private bool _showLogs;

        [SerializeField] private string _prefix;

        [SerializeField] private Color _prefixColor;

        private string _hexColor;

        #endregion

        private void OnValidate()
        {
            _hexColor = "#" + ColorUtility.ToHtmlStringRGBA(_prefixColor);
        }

        public void Log(object message, Object sender)
        {
            if (!_showLogs) return;
            Debug.Log($"<color={_hexColor}>{_prefix}</color>: {message}", sender);
        }
    }
}