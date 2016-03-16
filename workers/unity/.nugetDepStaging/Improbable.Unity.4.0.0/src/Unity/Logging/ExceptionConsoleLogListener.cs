using log4net;
using UnityEngine;

namespace Improbable.Unity.Logging
{
    /// <summary>
    /// Enabled by default in GameRoot
    /// </summary>
    public class ExceptionConsoleLogListener : MonoBehaviour
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(ConsoleLogListener));

        public void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        public void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (type == LogType.Exception)
            {
                LOGGER.Error(logString + "\n" + stackTrace);
            }
        }
    }
}