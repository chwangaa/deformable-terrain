using Improbable.Corelib.Interpolation;
using Improbable.Corelib.Util;
using Improbable.Corelibrary.Physical;
using Improbable.Math;
using Improbable.Unity;
using Improbable.Unity.Common.Core.Math;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Improbable.Corelib.Visualizers.Transform
{
    [EngineType(EnginePlatform.Client)]
    public class InterpolatingTransformVisualizer : MonoBehaviour
    {
        public bool UsePivot;
        public float MinAngleToInterpolateBetween;
        public float MinDistanceToInterpolateBetween;
        public float MaxSecondsToInterpolateAfterLastUpdate;

        [Require] protected TransformReader Transform;

        private readonly RotationInterpolator rotationInterpolator = new RotationInterpolator();
        private readonly PositionInterpolator pivotInterpolator = new PositionInterpolator();

        private UnityEngine.Transform cachedTransform;
        private Vector3 pivotOffset;
        private bool pivotAvailable;
        private float lastUpdateTime;

        private Coordinates GetPivot()
        {
            return UsePivot && Transform.Pivot.HasValue ? Transform.Pivot.Value : Transform.Position;
        }

        private Coordinates GetPosition()
        {
            return Transform.Position;
        }

        private Quaternion GetRotation()
        {
            return Transform.Rotation.ToUnityQuaternion();
        }

        private float SecondsSinceLastUpdate
        {
            get { return Time.time - lastUpdateTime; }
        }

        protected void OnEnable()
        {
            cachedTransform = transform;            

            cachedTransform.rotation = GetRotation();
            cachedTransform.position = GetPosition().ToUnityVector();

            Transform.AuthorityChanged += TransformAuthorityChanged;

            lastUpdateTime = Time.time;
        }

        protected void Reset()
        {
            ResetRotationInterpolator();
            ResetPivotInterpolator();

            UpdatePivot();
        }

        protected void Update()
        {
            if (Transform.IsAuthoritativeHere)
            {
                return;
            }

            var deltaTimeToAdvance = Time.deltaTime;

            var targetRotation = rotationInterpolator.GetInterpolatedValue(deltaTimeToAdvance);
            var targetPivot = pivotInterpolator.GetInterpolatedValue(deltaTimeToAdvance);

            if (!TimeExceedsThreshold())
            {
                SetPositionAroundPivot(targetPivot, targetRotation);
                SetRotationAroundPivot(targetRotation);
            }
        }

        private void TransformAuthorityChanged(bool isAuthoritativeHere)
        {
            if (!isAuthoritativeHere)
            {
                // We don't want to interpolate to the last state where we didn't have
                // authority, so we need to reset the interpolators here.
                Reset();
                Transform.PropertyUpdated += TransformUpdated;
            }
            else
            {
                // Stop interpolating as long as we have authority
                Transform.PropertyUpdated -= TransformUpdated;
            }
        }

        private void ResetRotationInterpolator()
        {
            rotationInterpolator.Reset(GetRotation(), Transform.Timestamp);
        }

        private void ResetPivotInterpolator()
        {
            pivotInterpolator.Reset(GetPivot(), Transform.Timestamp);
        }

        private bool TimeExceedsThreshold()
        {
            return SecondsSinceLastUpdate < MaxSecondsToInterpolateAfterLastUpdate;
        }

        private void TransformUpdated()
        {
            lastUpdateTime = Time.time;

            UpdateRotationInterpolator();
            UpdatePivotInterpolator();

            UpdatePivot();
        }

        private void UpdateRotationInterpolator()
        {
            rotationInterpolator.AddValue(GetRotation(), Transform.Timestamp);
        }

        private void UpdatePivotInterpolator()
        {
            if (PivotAvailabilityHasChanged())
            {
                pivotAvailable = Transform.Pivot.HasValue;
                ResetPivotInterpolator();
            }
            else
            {
                pivotInterpolator.AddValue(GetPivot(), Transform.Timestamp);
            }
        }

        private bool PivotAvailabilityHasChanged()
        {
            return pivotAvailable != Transform.Pivot.HasValue;
        }

        private void UpdatePivot()
        {
            pivotOffset = Quaternion.Inverse(GetRotation()) * (GetPivot() - GetPosition()).ToUnityVector();
        }

        private void SetPositionAroundPivot(Coordinates targetPivot, Quaternion targetRotation)
        {
            var targetOffsetPivot = targetRotation * pivotOffset;
            var targetPosition = targetPivot.ToUnityVector() - targetOffsetPivot;
            
            if (PositionExceedsThreshold(targetPosition))
            {
                cachedTransform.position = targetPosition;                
            }
        }

        private bool PositionExceedsThreshold(Vector3 targetPosition)
        {
            return Vector3Utils.SquareDistance(cachedTransform.position, targetPosition) > MinDistanceToInterpolateBetween;
        }

        private void SetRotationAroundPivot(Quaternion targetRotation)
        {
            if (RotationExceedsThreshold(targetRotation))
            {
                cachedTransform.rotation = targetRotation;
            }
        }

        private bool RotationExceedsThreshold(Quaternion targetRotation)
        {
            return Quaternion.Angle(cachedTransform.rotation, targetRotation) > MinAngleToInterpolateBetween;
        }
    }
}