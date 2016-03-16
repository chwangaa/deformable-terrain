using System;
using System.Collections.Generic;
using Improbable.Corelib.PrefabExporting.PreProcessors;
using Improbable.Corelib.Slots.Visualizers;
using Improbable.CoreLib.Visualizers.Slots;

namespace Improbable.Corelib.Slots
{
    public class DynamicSlotsContainerVisualizers : PreProcessorBase
    {
        protected override Dictionary<Type, VisualizerPreProcessorConfig> GetCommonVisualizersToAdd()
        {
            return new Dictionary<Type, VisualizerPreProcessorConfig>
            {
                { typeof(SlotContainerVisualizer), VisualizerPreProcessorConfig.DefaultInstance},
                { typeof(VisualityWatcher), VisualizerPreProcessorConfig.DefaultInstance},
                { typeof(DynamicSlotsVisualizer), VisualizerPreProcessorConfig.DefaultInstance}
            };
        }
    }
}