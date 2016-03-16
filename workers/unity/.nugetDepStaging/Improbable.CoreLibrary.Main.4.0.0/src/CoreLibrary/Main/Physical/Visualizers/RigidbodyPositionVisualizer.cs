using Improbable.Corelib.Physical.Visualizers;
using Improbable.Entity.Physical;
using Improbable.Math;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Improbable.CoreLib.Physical.Visualizers
{
    [EngineType(EnginePlatform.FSim)]
    public class RigidbodyPositionVisualizer : MonoBehaviour
    {
        [Require] protected PositionReader Position;
        [Require] protected RigidbodyDataReader RigidbodyData;

        public float PositionDifferenceThreshold = 0.01f;
        public float PositionLerpRate = 0.2f;

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
            Position.ValueUpdated += PositionValueUpdated;
        }

        private void PositionValueUpdated(Coordinates newPosition)
        {
            if (!Position.IsAuthoritativeHere)
            {
                UpdatePosition(newPosition);
            }
        }

        private void UpdatePosition(Coordinates newPosition)
        {
            if (Rigidbody == null)
            {
                return;
            }

            var currentPosition = Rigidbody.position.ToCoordinates();
            if (IsWithinThreshold(currentPosition, newPosition))
            {
                Rigidbody.MovePosition(Vector3.Lerp(currentPosition.ToUnityVector(), newPosition.ToUnityVector(), PositionLerpRate));
            }
        }

        private bool IsWithinThreshold(Coordinates current, Coordinates suggested)
        {
            return current.IsWithinDistance(suggested, PositionDifferenceThreshold);
        }
    }
}