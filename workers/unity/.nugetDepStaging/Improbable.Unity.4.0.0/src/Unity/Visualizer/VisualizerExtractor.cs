using System;
using System.Collections.Generic;
using log4net;
using UnityEngine;

namespace Improbable.Unity.Visualizer
{
    internal class VisualizerExtractor : IVisualizerExtractor
    {
        private readonly GameObject gameObject;
        private readonly IVisualizerMetadataLookup visualizerMetadata;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(VisualizerExtractor));

        public VisualizerExtractor(GameObject gameObject, IVisualizerMetadataLookup visualizerMetadata)
        {
            this.gameObject = gameObject;
            this.visualizerMetadata = visualizerMetadata;
        }

        public IList<object> ExtractVisualizers()
        {
            var foundVisualizers = new List<object>();
            if (gameObject != null)
            {
                var componentsInChildren = gameObject.GetComponentsInChildren<MonoBehaviour>();
                if (componentsInChildren == null)
                {
                    Logger.DebugFormat("GetComponentsInChildren returned null for GameObject: {0}", gameObject.name);
                }
                else
                {
                    for (int index = 0; index < componentsInChildren.Length; index++)
                    {
                        var visualizer = componentsInChildren[index];
                        if (visualizer == null)
                        {
                            Logger.DebugFormat("GetComponentsInChildren returned a null element for GameObject {0}", gameObject.name);
                            continue;
                        }
                        var visualizerType = visualizer.GetType();
                        if (IsVisualizer(visualizerType))
                        {
                            foundVisualizers.Add(visualizer);
                        }
                    }
                }
            }
            return foundVisualizers;
        }

        private bool IsVisualizer(Type type)
        {
            return visualizerMetadata.IsVisualizer(type);
        }
    }
}
