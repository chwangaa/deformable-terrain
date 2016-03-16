using System;
using System.Collections.Generic;
using Improbable.Corelib.LagDetection;
using Improbable.Corelib.Metrics.Visualizers;

namespace Improbable.Corelib.PrefabExporting.PreProcessors
{
    public class LagDetectingPreProcessor : PreProcessorBase
    {
        public bool MeasureEngineToGSimLatency;

        protected override Dictionary<Type, VisualizerPreProcessorConfig> GetCommonVisualizersToAdd()
        {
            return new Dictionary<Type, VisualizerPreProcessorConfig>
            {
                { typeof(ClientLagDetector), new VisualizerPreProcessorConfig(ShouldAddClientLagDetector) }
            };
        }

        protected override Dictionary<Type, VisualizerPreProcessorConfig> GetFSimVisualizersToAdd()
        {
            return new Dictionary<Type, VisualizerPreProcessorConfig>
            {
                { typeof(PhysicsLagPingReceiver), VisualizerPreProcessorConfig.DefaultInstance },
                { typeof(GSimLatency), new VisualizerPreProcessorConfig(ShouldAddGSimLatencyVisualizer) }
            };
        }

        private bool ShouldAddGSimLatencyVisualizer()
        {
            return MeasureEngineToGSimLatency;
        }

        public bool ShouldAddClientLagDetector()
        {
            return PreProcessorTools.IsPlayerPrefab(gameObject);
        }
    }
}
