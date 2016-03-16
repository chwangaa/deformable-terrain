using System;
using System.Collections.Generic;
using Improbable.Core.GameLogic.Visualizers;
using Improbable.CoreLib.Physical.Visualizers;

namespace Improbable.Corelib.PrefabExporting.PreProcessors
{
    public class StaticbodyPreProcessor : PreProcessorBase
    {
        protected override Dictionary<Type, VisualizerPreProcessorConfig> GetCommonVisualizersToAdd()
        {
            return new Dictionary<Type, VisualizerPreProcessorConfig>
            {
                { typeof(InitialPositionVisualizer), VisualizerPreProcessorConfig.DefaultInstance},
                { typeof(InitialRotationVisualizer), VisualizerPreProcessorConfig.DefaultInstance},
                { typeof(TeleportVisualizer), VisualizerPreProcessorConfig.DefaultInstance}
            };
        }
    }
}
