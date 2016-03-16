using System;
using System.Collections.Generic;
using Improbable.Core.GameLogic.Visualizers;
using Improbable.Corelib.Physical;
using Improbable.Corelib.Visualizers.Transform;
using UnityEngine;

namespace Improbable.Corelib.PrefabExporting.PreProcessors
{
    public class RigidbodyTransformPreProcessor : TransformPreProcessorBase
    {
        protected override Dictionary<Type, VisualizerPreProcessorConfig> GetClientVisualizersToAdd()
        {
            var visualizers = base.GetClientVisualizersToAdd();

            visualizers.Add(typeof(RigidbodyVisualizer), new VisualizerPreProcessorConfig(ShouldAddClientAuthoritativeVisualizers));
            visualizers.Add(typeof(RigidbodySync), new VisualizerPreProcessorConfig(ShouldAddClientAuthoritativeVisualizers));
            visualizers.Add(typeof(RigidbodyTransformVisualizer), new VisualizerPreProcessorConfig(ShouldAddClientAuthoritativeVisualizers));

            return visualizers;
        }

        protected override Dictionary<Type, VisualizerPreProcessorConfig> GetFSimVisualizersToAdd()
        {
            var visualizers = base.GetFSimVisualizersToAdd();

            visualizers.Add(typeof(RigidbodyVisualizer), new VisualizerPreProcessorConfig(ShouldAddFSimAuthoritativeVisualizers));
            visualizers.Add(typeof(RigidbodySync), new VisualizerPreProcessorConfig(ShouldAddFSimAuthoritativeVisualizers));
            visualizers.Add(typeof(RigidbodyTransformVisualizer), new VisualizerPreProcessorConfig(ShouldAddFSimAuthoritativeVisualizers));

            return visualizers;
        }

        protected override bool GetExpectsIRigidbodyVisualizer()
        {
            return true;
        }
    }
}