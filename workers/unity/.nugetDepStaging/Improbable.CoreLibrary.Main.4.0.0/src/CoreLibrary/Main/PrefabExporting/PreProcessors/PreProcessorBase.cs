using System;
using System.Collections.Generic;
using Improbable.Unity;
using Improbable.Unity.Export;
using log4net;
using UnityEngine;

namespace Improbable.Corelib.PrefabExporting.PreProcessors
{
    public abstract class PreProcessorBase : MonoBehaviour, IPrefabExportProcessor
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PreProcessorBase));

        public virtual void ExportProcess(EnginePlatform enginePlatform)
        {
            AddVisualizers(enginePlatform, gameObject);
        }

        public void AddVisualizers(EnginePlatform enginePlatform, GameObject targetGameObject)
        {
            AddVisualizersToGameObject(GetCommonVisualizersToAdd(), targetGameObject);

            switch (enginePlatform)
            {
                case EnginePlatform.Client:
                    AddVisualizersToGameObject(GetClientVisualizersToAdd(), targetGameObject);
                    break;
                case EnginePlatform.FSim:
                    AddVisualizersToGameObject(GetFSimVisualizersToAdd(), targetGameObject);
                    break;
                default:
                    Logger.WarnFormat("Engine platform '{0}' is not supported", enginePlatform);
                    break;
            }
        }

        private static void AddVisualizersToGameObject(Dictionary<Type, VisualizerPreProcessorConfig> visualizersToAdd,
                                                       GameObject targetGameObject)
        {
            foreach (var visualizerToAdd in visualizersToAdd)
            {
                var visualizerType = visualizerToAdd.Key;
                var addVisualizerContainer = visualizerToAdd.Value;

                if (addVisualizerContainer.ShouldAddVisualizer())
                {
                    addVisualizerContainer.AddVisualizer(targetGameObject, visualizerType);
                }
            }
        }

        protected virtual Dictionary<Type, VisualizerPreProcessorConfig> GetCommonVisualizersToAdd()
        {
            return new Dictionary<Type, VisualizerPreProcessorConfig>();
        }

        protected virtual Dictionary<Type, VisualizerPreProcessorConfig> GetClientVisualizersToAdd()
        {
            return new Dictionary<Type, VisualizerPreProcessorConfig>();
        }

        protected virtual Dictionary<Type, VisualizerPreProcessorConfig> GetFSimVisualizersToAdd()
        {
            return new Dictionary<Type, VisualizerPreProcessorConfig>();
        }
    }
}