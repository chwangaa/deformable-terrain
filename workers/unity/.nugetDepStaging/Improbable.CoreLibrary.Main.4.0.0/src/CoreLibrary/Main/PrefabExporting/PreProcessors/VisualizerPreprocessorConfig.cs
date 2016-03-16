using System;
using UnityEngine;

namespace Improbable.Corelib.PrefabExporting.PreProcessors
{
    public class VisualizerPreProcessorConfig
    {
        public Func<bool> ShouldAddVisualizer { get; private set; }
        public Action<GameObject, Type> AddVisualizer { get; private set; }

        public static VisualizerPreProcessorConfig DefaultInstance { get; private set; }

        static VisualizerPreProcessorConfig()
        {
            DefaultInstance = new VisualizerPreProcessorConfig();
        }

        public VisualizerPreProcessorConfig(Func<bool> shouldAddVisualizer = null,
                                            Action<GameObject, Type> addVisualizer = null)
        {
            ShouldAddVisualizer = shouldAddVisualizer ?? AlwaysAddVisualizer();
            AddVisualizer = addVisualizer ?? DefaultAddVisualizer;
        }

        private static Func<bool> AlwaysAddVisualizer()
        {
            return () => true;
        }

        private static void DefaultAddVisualizer(GameObject gameObject, Type visualizerType)
        {
            gameObject.AddComponent(visualizerType);
        }
    }
}