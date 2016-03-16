using System;
using log4net;
using UnityEngine;

namespace Improbable.Unity
{
    class ClientMetrics : MonoBehaviour
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(ClientMetrics));

        public void OnEnable()
        {
            LOGGER.Info("ClientMetrics OS=" + SystemInfo.operatingSystem);
            LOGGER.Info("ClientMetrics ProcType=" + SystemInfo.processorType);
            LOGGER.Info("ClientMetrics ProcCount=" + SystemInfo.processorCount);
            LOGGER.Info("ClientMetrics graphicsDeviceID=" + SystemInfo.graphicsDeviceID);
            LOGGER.Info("ClientMetrics graphicsDeviceVendorID=" + SystemInfo.graphicsDeviceVendorID);
            LOGGER.Info("ClientMetrics graphicsDeviceName=" + SystemInfo.graphicsDeviceName);
            LOGGER.Info("ClientMetrics graphicsMemorySize=" + SystemInfo.graphicsMemorySize);
            LOGGER.Info("ClientMetrics systemMemorySize=" + SystemInfo.systemMemorySize);
        }
    }
}
