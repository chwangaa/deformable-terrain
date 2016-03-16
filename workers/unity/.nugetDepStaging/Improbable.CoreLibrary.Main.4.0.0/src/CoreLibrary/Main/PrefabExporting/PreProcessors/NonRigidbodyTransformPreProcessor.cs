using System;
using System.Collections.Generic;
using Improbable.Corelib.Physical;
using Improbable.Corelib.Visualizers.Transform;
using UnityEngine;

namespace Improbable.Corelib.PrefabExporting.PreProcessors
{
    public class NonRigidbodyTransformPreProcessor : TransformPreProcessorBase
    {
        protected override Dictionary<Type, VisualizerPreProcessorConfig> GetFSimVisualizersToAdd()
        {
            var visualizers = base.GetFSimVisualizersToAdd();

            visualizers.Add(typeof(TransformVisualizer), VisualizerPreProcessorConfig.DefaultInstance);

            return visualizers;
        }

        protected override bool GetExpectsIRigidbodyVisualizer()
        {
            return false;
        }
    }
}