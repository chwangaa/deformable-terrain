using UnityEngine;
using log4net;

namespace Improbable.Unity.Core
{
    // TODO: this seems odd. What is the responsibility of this class?
    /// <summary>
    /// Given an engine configuration, applies it by setting actual engine values
    /// </summary>
    class UnityEngineConfigurator
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(GameRoot));

        public void ConfigureEngine(EngineConfiguration engineConfiguration)
        {
            SetTargetFps(engineConfiguration.TargetFps);
            SetFixedUpdateRate(engineConfiguration.FixedUpdateRate);
        }

        private void SetTargetFps(int targetFps)
        {
            LOGGER.Info("Setting Target FPS: " + targetFps);
            Application.targetFrameRate = targetFps;
        }

        private void SetFixedUpdateRate(int fixedUpdateRate)
        {
            LOGGER.Info("Setting Fixed Update Rate: " + fixedUpdateRate);
            float fixedDeltaTime = 1.0f / fixedUpdateRate;
            Time.fixedDeltaTime = fixedDeltaTime;
        }
    }
}
