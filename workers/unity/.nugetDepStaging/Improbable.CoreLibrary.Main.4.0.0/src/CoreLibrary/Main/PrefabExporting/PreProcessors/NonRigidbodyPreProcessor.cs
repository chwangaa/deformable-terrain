using System;
using System.Collections.Generic;
using Improbable.Core.GameLogic.Visualizers;
using Improbable.Corelib.Physical;
using Improbable.CoreLib.Physical.Visualizers;
using Improbable.TestGameLogic.Entities.Visualizers.Physical;
using UnityEngine;

namespace Improbable.Corelib.PrefabExporting.PreProcessors
{
    public class NonRigidbodyPreProcessor : PreProcessorBase
    {
        public float PositionNetworkUpdatePeriodThreshold = DefaultPhysicalParameters.PositionNetworkUpdatePeriodThreshold;
        public float SlottedPositionNetworkUpdatePeriodThreshold = DefaultPhysicalParameters.SlottedPositionNetworkUpdatePeriodThreshold;
        public float PositionNetworkUpdateSquareDistanceThreshold = DefaultPhysicalParameters.PositionNetworkUpdateSquareDistanceThreshold;
        public float RotationNetworkUpdatePeriodThreshold = DefaultPhysicalParameters.RotationNetworkUpdatePeriodThreshold;
        public float SlottedRotationNetworkUpdatePeriodThreshold = DefaultPhysicalParameters.SlottedRotationNetworkUpdatePeriodThreshold;
        public float RotationNetworkUpdateSquareDistanceThreshold = DefaultPhysicalParameters.RotationNetworkUpdateSquareDistanceThreshold;

        protected override Dictionary<Type, VisualizerPreProcessorConfig> GetCommonVisualizersToAdd()
        {
            return new Dictionary<Type, VisualizerPreProcessorConfig>
            {
                { typeof(InitialPositionVisualizer), VisualizerPreProcessorConfig.DefaultInstance },
                { typeof(InitialRotationVisualizer), VisualizerPreProcessorConfig.DefaultInstance },
                { typeof(PositionVisualizer), VisualizerPreProcessorConfig.DefaultInstance },
                { typeof(TargetRotationVisualizer), VisualizerPreProcessorConfig.DefaultInstance },
                { typeof(InterpolatingPositionVisualizer), VisualizerPreProcessorConfig.DefaultInstance },
                { typeof(InterpolatingRotationVisualizer), VisualizerPreProcessorConfig.DefaultInstance },
                { typeof(TeleportVisualizer), VisualizerPreProcessorConfig.DefaultInstance },
                { typeof(HierarchyNodeInfo), VisualizerPreProcessorConfig.DefaultInstance}
            };
        }

        protected override Dictionary<Type, VisualizerPreProcessorConfig> GetFSimVisualizersToAdd()
        {
            return new Dictionary<Type, VisualizerPreProcessorConfig>
            {
                { typeof(PhysicalPosition), new VisualizerPreProcessorConfig(addVisualizer: AddPhysicalPosition) },
                { typeof(PhysicalRotation), new VisualizerPreProcessorConfig(addVisualizer: AddPhysicalRotation) }
            };
        }

        public void AddPhysicalPosition(GameObject targetGameObject, Type visualizerType)
        {
            var physicalPosition = targetGameObject.AddComponent<PhysicalPosition>();
            physicalPosition.NetworkUpdatePeriodThreshold = PositionNetworkUpdatePeriodThreshold;
            physicalPosition.SlottedNetworkUpdatePeriodThreshold = PositionNetworkUpdatePeriodThreshold;
            physicalPosition.PositionNetworkUpdateSquareDistanceThreshold = PositionNetworkUpdateSquareDistanceThreshold;
        }

        public void AddPhysicalRotation(GameObject targetGameObject, Type visualizerType)
        {
            var physicalRotation = targetGameObject.AddComponent<PhysicalRotation>();
            physicalRotation.NetworkUpdatePeriodThreshold = RotationNetworkUpdatePeriodThreshold;
            physicalRotation.SlottedNetworkUpdatePeriodThreshold = SlottedRotationNetworkUpdatePeriodThreshold;
            physicalRotation.RotationNetworkUpdateSquareDistanceThreshold = RotationNetworkUpdateSquareDistanceThreshold;
        }
    }
}
