using Improbable.Corelib.Physical.Visualizers;
using Improbable.Corelib.Util;
using Improbable.Corelibrary.Physical;
using Improbable.Entity.Physical;
using Improbable.Math;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Improbable.Corelib.Visualizers.Transform
{
    /// <summary>
    ///     This Visualizer tracks transform updates for entities we don't have transform authority on and moves
    ///     their Rigidbody component accordingly in case they have one. This is useful if we for example want to
    ///     collide with an entity we don't have transform authority on, but still want the collision to look
    ///     physical on both sides.
    /// </summary>
    public class RigidbodyTransformVisualizer : MonoBehaviour
    {
        [Require] protected TransformReader Transform;
        [Require] protected RigidbodyDataReader RigidbodyData;

        public float PositionDifferenceThreshold = 0.01f;
        public float PositionLerpRate = 0.2f;

        public float RotationDifferenceThreshold = 0.01f;
        public float RotationSlerpRate = 0.2f;

        private IRigidbodyVisualizer cachedRigidbodyVisualizer;
        public Rigidbody Rigidbody
        {
            get
            {
                if (cachedRigidbodyVisualizer == null)
                {
                    cachedRigidbodyVisualizer = GetComponent<IRigidbodyVisualizer>();
                }
                return cachedRigidbodyVisualizer.Rigidbody;
            }
        }

        protected void OnEnable()
        {
            Transform.AuthorityChanged += TransformAuthorityChanged;

            UpdatePosition(Transform.Position);
            UpdateRotation(Transform.Rotation);
        }

        private void TransformAuthorityChanged(bool isAuthoritativeHere)
        {
            if (!isAuthoritativeHere)
            {
                Transform.PositionUpdated += UpdatePosition;
                Transform.RotationUpdated += UpdateRotation;
            }
            else
            {
                // Stop updating as long as we have authority
                Transform.PositionUpdated -= UpdatePosition;
                Transform.RotationUpdated -= UpdateRotation;
            }
        }

        private void UpdatePosition(Coordinates newPosition)
        {
            if (Rigidbody == null)
            {
                return;
            }

            var currentPosition = Rigidbody.position.ToCoordinates();
            if (IsPositionWithinThreshold(currentPosition, newPosition))
            {
                Rigidbody.MovePosition(Vector3.Lerp(currentPosition.ToUnityVector(), newPosition.ToUnityVector(), PositionLerpRate));
            }
        }

        private bool IsPositionWithinThreshold(Coordinates current, Coordinates suggested)
        {
            return current.IsWithinDistance(suggested, PositionDifferenceThreshold);
        }

        private void UpdateRotation(Improbable.Corelib.Math.Quaternion rotation)
        {
            if (Rigidbody == null)
            {
                return;
            }

            Quaternion currentRotation = Rigidbody.rotation;
            Quaternion suggestedRotation = Transform.Rotation.ToUnityQuaternion();

            if (IsRotationWithinThreshold(currentRotation, suggestedRotation))
            {
                Rigidbody.MoveRotation(Quaternion.Slerp(currentRotation, suggestedRotation, RotationSlerpRate));
            }
        }

        private bool IsRotationWithinThreshold(Quaternion current, Quaternion suggested)
        {
            return Quaternion.Angle(current, suggested) > RotationDifferenceThreshold;
        }
    }
}