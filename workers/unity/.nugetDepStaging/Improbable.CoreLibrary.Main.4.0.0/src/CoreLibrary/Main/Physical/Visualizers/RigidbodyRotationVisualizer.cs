using Improbable.Entity.Physical;
using Improbable.Math;
using Improbable.Unity;
using Improbable.Unity.Common.Core.Math;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Improbable.CoreLib.Physical.Visualizers
{
    [EngineType(EnginePlatform.FSim)]
    public class RigidbodyRotationVisualizer : MonoBehaviour
    {
        [Require] protected RigidbodyDataReader RigidbodyData;
        [Require] protected RotationReader Rotation;

        public float RotationDifferenceThreshold = 0.01f;
        public float RotationSlerpRate = 0.2f;

        private Rigidbody cachedRigidbody;
        public Rigidbody Rigidbody
        {
            get
            {
                if (cachedRigidbody == null)
                {
                    cachedRigidbody = GetComponent<Rigidbody>();
                }
                return cachedRigidbody;
            }
        }

        protected void OnEnable()
        {
            Rotation.EulerUpdated += RotationEulerUpdated;
        }

        private void RotationEulerUpdated(Vector3d obj)
        {
            if (!Rotation.IsAuthoritativeHere)
            {
                UpdateRotation();
            }
        }
        
        private void UpdateRotation()
        {
            if (Rigidbody == null)
            {
                return;
            }

            Quaternion currentRotation = Rigidbody.rotation;
            Quaternion suggestedRotation = Quaternion.Euler(Rotation.Euler.ToUnityVector());

            if (IsWithinThreshold(currentRotation, suggestedRotation))
            {
                Rigidbody.MoveRotation(Quaternion.Slerp(currentRotation, suggestedRotation, RotationSlerpRate));
            }
        }

        private bool IsWithinThreshold(Quaternion current, Quaternion suggested)
        {
            return Quaternion.Angle(current, suggested) > RotationDifferenceThreshold;
        }
    }
}