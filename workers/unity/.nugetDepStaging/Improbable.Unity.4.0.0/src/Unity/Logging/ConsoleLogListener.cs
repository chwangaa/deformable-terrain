using log4net;
using UnityEngine;

namespace Improbable.Unity.Logging
{
    /// <remarks>
    /// Add this behaviour when you want to direct messages sent directly
    /// to Unitys Debug.* methods to the improbable logger. 
    /// NB it can not be used in combination with UnityConsoleAppender.
    /// </remarks>
    public class ConsoleLogListener : MonoBehaviour
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
            var message = logString;
            switch (type)
            {
                case LogType.Warning:
                    LOGGER.Warn(message);
                    break;
                case LogType.Assert:
                case LogType.Log:
                    LOGGER.Info(message);
                    break;
                case LogType.Error:
                    LOGGER.Error(message);
                    break;
            }
        }
    }
}
