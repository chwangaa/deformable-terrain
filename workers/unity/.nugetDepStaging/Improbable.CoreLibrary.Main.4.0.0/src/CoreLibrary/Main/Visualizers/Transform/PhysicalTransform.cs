using System;
using Improbable.Corelib.Physical;
using Improbable.Corelib.Util;
using Improbable.Corelibrary.Physical;
using Improbable.Logging;
using Improbable.Math;
using Improbable.Unity;
using Improbable.Unity.Common.Core.Math;
using Improbable.Unity.Visualizer;
using Improbable.Util;
using log4net;
using log4net.Appender;
using UnityEngine;
using Time = UnityEngine.Time;
using Vector3 = UnityEngine.Vector3;

namespace Improbable.Corelib.Visualizers.Transform
{
    /// <summary>
    ///     This Visualizer takes care of synchronizing the Unity position with the transform state in the case
    ///     where either the FSim or the Client have authority over the transform.
    /// </summary>
    public class PhysicalTransform : PhysicalBase<TransformData>
    {
        [Tooltip("The minimum square distance from the last sent position before sending a new state update.")]
        public float PositionNetworkUpdateSquareDistanceThreshold;
        [Tooltip("The minimum angle in degrees from the last sent rotation before sending a new state update.")]
        public float RotationNetworkUpdateAngleThreshold;

        [Header("Interpolation Configuration")]
        [Tooltip("Whether to use the RigidBody center of mass as the base pivot location.")]
        public bool UseCenterOfMassPivot;
        public Vector3? PivotOffset;

        private static readonly ILog Logger = LogManager.GetLogger(typeof(PhysicalTransform));
        private static readonly TimeSpan QuietPeriod = new TimeSpan(0, 0, 5);
        private const int MaxLogMessagesBeforeQuietPeriod = 5;
        private readonly LogLimiter invalidPositionLogLimiter = new LogLimiter(new SystemTimeSource(), QuietPeriod, MaxLogMessagesBeforeQuietPeriod);

        [Require]
        protected TransformWriter Transform;

        protected override void OnEnable()
        {
            base.OnEnable();

            CachedTransform.rotation = Transform.Rotation.ToUnityQuaternion();
            CachedTransform.position = Transform.Position.ToUnityVector();
        }

        protected override TransformData GetLatestValue()
        {
            return new TransformData()
            {
                position = CachedTransform.position,
                rotation = CachedTransform.rotation,
                pivot = GetOffsetPivotWorldPosition(),
                notInitial = true
            };
        }

        protected override bool IsPastThreshold(TransformData lastValue, TransformData newValue)
        {
            return !lastValue.notInitial || IsLastUpdateDistancePastThreshold(lastValue, newValue) || IsLastUpdateRotationPastThreshold(lastValue, newValue);
        }

        private bool IsLastUpdateRotationPastThreshold(TransformData lastTransform, TransformData newTransform)
        {
            var lastRotation = lastTransform.rotation;
            var newRotation = newTransform.rotation;

            return Quaternion.Angle(lastRotation, newRotation) >= RotationNetworkUpdateAngleThreshold;
        }

        private bool IsLastUpdateDistancePastThreshold(TransformData lastTransform, TransformData newTransform)
        {
            var lastPosition = lastTransform.position.ToCoordinates();
            var newPosition = newTransform.position.ToCoordinates();

            return !lastPosition.IsWithinSquareDistance(newPosition, PositionNetworkUpdateSquareDistanceThreshold);
        }

        protected override void OnShouldUpdate(float timeDelta, TransformData newValue)
        {
            Transform.Update
                .Timestamp(Transform.Timestamp + timeDelta)
                .Position(GetLatestPosition(newValue.position.ToCoordinates()))
                .Rotation(newValue.rotation.ToNativeQuaternion())
                .Pivot(newValue.pivot)
                .FinishAndSend();
        }

        private Coordinates? GetOffsetPivotWorldPosition()
        {
            var pivotWorldPosition = GetPivotWorldPosition();

            if (PivotOffset.HasValue && pivotWorldPosition.HasValue)
            {
                return pivotWorldPosition.Value + PivotOffset.Value.ToNativeVector3f();
            }

            return pivotWorldPosition;
        }

        private Coordinates? GetPivotWorldPosition()
        {
            if (UseCenterOfMassPivot)
            {
                if (Rigidbody == null)
                {
                    return null;
                }

                return Rigidbody.worldCenterOfMass.ToCoordinates();
            }

            return CachedTransform.position.ToCoordinates();
        }

        private Coordinates GetLatestPosition(Coordinates newPosition)
        {
            if (newPosition.IsFinite)
            {
                return newPosition;
            }
            else
            {
                if (invalidPositionLogLimiter.CanLogNow())
                {
                    Logger.ErrorFormat("Entity '{0}' (gameObject: '{1}') is trying to set position to '{2}'. Preventing this. Setting the position to last valid one: {3}",
                                       gameObject.GetEntityObject().EntityId,
                                       gameObject.name,
                                       newPosition,
                                       Transform.Position);
                    invalidPositionLogLimiter.Logged();
                }
                CachedTransform.position = Transform.Position.ToUnityVector();

                return Transform.Position;
            }
        }
    }

    public struct TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Coordinates? pivot;
        public Boolean notInitial;
    }
}
