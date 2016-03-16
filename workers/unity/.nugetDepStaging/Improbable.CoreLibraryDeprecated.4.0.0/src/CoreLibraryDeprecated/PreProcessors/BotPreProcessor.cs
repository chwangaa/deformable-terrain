﻿using System;
using System.Collections.Generic;
using Improbable.Corelib.Physical;
using Improbable.Core.GameLogic.Visualizers;
﻿using Improbable.Corelib.PrefabExporting.PreProcessors;

namespace Improbable.TestGameLogic.Entities.PreProcessors
{
    public class BotPreProcessor : RigidbodyPreProcessor
    {
        protected override Dictionary<Type, VisualizerPreProcessorConfig> GetCommonVisualizersToAdd()
        {
            var commonVisualizersToAdd = base.GetCommonVisualizersToAdd();

            commonVisualizersToAdd.Add(typeof(ForceBasedBotMovementVisualizer), VisualizerPreProcessorConfig.DefaultInstance);
            commonVisualizersToAdd.Add(typeof(GroundedChecker), VisualizerPreProcessorConfig.DefaultInstance);

            return commonVisualizersToAdd;
        }
    }
}