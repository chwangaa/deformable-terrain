using System;
using System.Collections.Generic;
using Improbable.Corelib.Physical;
using Improbable.Corelib.Visualizers.Transform;
using UnityEngine;

namespace Improbable.Corelib.PrefabExporting.PreProcessors
{
    public abstract class TransformPreProcessorBase : PreProcessorBase
    {
        [Header("State Update Rate Configuration")]
        [Tooltip("The minimum number of seconds between sending each Transform state update.")]
        public float NetworkUpdatePeriodThreshold = DefaultPhysicalParameters.NetworkUpdatePeriodThreshold;
        [Tooltip("The minimum number of seconds between sending each Transform state update when slotted (i.e., when this entities HierarchyNode exists and has a parent).")]
        public float SlottedNetworkUpdatePeriodThreshold = DefaultPhysicalParameters.SlottedNetworkUpdatePeriodThreshold;
        [Tooltip("The minimum square distance from the last sent position before sending a new Transform state update.")]
        public float PositionNetworkUpdateSquareDistanceThreshold = DefaultPhysicalParameters.PositionNetworkUpdateSquareDistanceThreshold;
        [Tooltip("The minimum angle in degrees from the last sent rotation before sending a new Transform state update.")]
        public float RotationNetworkUpdateAngleThreshold = DefaultPhysicalParameters.RotationNetworkUpdateAngleThreshold;

        [Header("Interpolation Configuration")]
        [Tooltip("Whether to use the RigidBody center of mass as the base pivot location.")]
        public bool UseCenterOfMassPivot;
        public Vector3? PivotOffset;

        [Tooltip("The minimum angle in degrees from the last interpolated rotation to the target interpolated rotation before setting the rotation to the target.")]
        public float MinAngleToInterpolateBetween = DefaultPhysicalParameters.MinAngleToInterpolateBetween;
        [Tooltip("The minimum square distance from the last interpolated position to the target interpolated position before setting the position to the target.")]
        public float MinDistanceToInterpolateBetween = DefaultPhysicalParameters.MinDistanceToInterpolateBetween;
        [Tooltip("The number of seconds after receiving a Transform state update that we will continue to interpolate before ceasing interpolation.")]
        public float MaxSecondsToInterpolateAfterLastUpdate = DefaultPhysicalParameters.MaxSecondsToInterpolateAfterLastUpdate;

        [Header("Transform Authority")]
        [Tooltip("Whether delegating transform authority to clients should be supported")]
        public bool CanBeClientAuthoritative = DefaultPhysicalParameters.CanBeClientAuthoritative;
        [Tooltip("Whether delegating transform authority to FSims should be supported")]
        public bool CanBeFSimAuthoritative = DefaultPhysicalParameters.CanBeFSimAuthoritative;

        protected override Dictionary<Type, VisualizerPreProcessorConfig> GetCommonVisualizersToAdd()
        {
            return new Dictionary<Type, VisualizerPreProcessorConfig>
            {
                { typeof(TeleportTransformVisualizer), VisualizerPreProcessorConfig.DefaultInstance },
                { typeof(InterpolatingTransformVisualizer), new VisualizerPreProcessorConfig(addVisualizer: AddInterpolatingTransformVisualizer) }
            };
        }

        protected override Dictionary<Type, VisualizerPreProcessorConfig> GetClientVisualizersToAdd()
        {
            return new Dictionary<Type, VisualizerPreProcessorConfig>
            {
                { typeof(PhysicalTransform), new VisualizerPreProcessorConfig(ShouldAddClientAuthoritativeVisualizers, AddPhysicalTransformVisualizer) },
            };
        }

        protected override Dictionary<Type, VisualizerPreProcessorConfig> GetFSimVisualizersToAdd()
        {
            return new Dictionary<Type, VisualizerPreProcessorConfig>
            {
                { typeof(PhysicalTransform), new VisualizerPreProcessorConfig(ShouldAddFSimAuthoritativeVisualizers, AddPhysicalTransformVisualizer) },
            };
        }

        protected bool ShouldAddClientAuthoritativeVisualizers()
        {
            return CanBeClientAuthoritative;
        }

        protected bool ShouldAddFSimAuthoritativeVisualizers()
        {
            return CanBeFSimAuthoritative;
        }

        protected void AddInterpolatingTransformVisualizer(GameObject targetGameObject, Type visualizerType)
        {
            var interpolatingTransformVisualizer = targetGameObject.AddComponent<InterpolatingTransformVisualizer>();
            interpolatingTransformVisualizer.UsePivot = UseCenterOfMassPivot || PivotOffset != null;
            interpolatingTransformVisualizer.MinDistanceToInterpolateBetween = MinDistanceToInterpolateBetween;
            interpolatingTransformVisualizer.MinAngleToInterpolateBetween = MinAngleToInterpolateBetween;
            interpolatingTransformVisualizer.MaxSecondsToInterpolateAfterLastUpdate = MaxSecondsToInterpolateAfterLastUpdate;
        }

        protected void AddPhysicalTransformVisualizer(GameObject targetGameObject, Type visualizerType)
        {
            var physicalTransform = targetGameObject.AddComponent<PhysicalTransform>();
            physicalTransform.NetworkUpdatePeriodThreshold = NetworkUpdatePeriodThreshold;
            physicalTransform.SlottedNetworkUpdatePeriodThreshold = SlottedNetworkUpdatePeriodThreshold;
            physicalTransform.PositionNetworkUpdateSquareDistanceThreshold = PositionNetworkUpdateSquareDistanceThreshold;
            physicalTransform.RotationNetworkUpdateAngleThreshold = RotationNetworkUpdateAngleThreshold;
            physicalTransform.UseCenterOfMassPivot = UseCenterOfMassPivot;
            physicalTransform.PivotOffset = PivotOffset;
            physicalTransform.ExpectsIRigidbodyVisualizer = GetExpectsIRigidbodyVisualizer();
        }

        protected abstract bool GetExpectsIRigidbodyVisualizer();
    }
}