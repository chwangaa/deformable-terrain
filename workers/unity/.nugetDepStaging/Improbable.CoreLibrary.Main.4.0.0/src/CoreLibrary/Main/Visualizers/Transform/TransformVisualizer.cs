using Improbable.Corelib.Util;
using Improbable.Corelibrary.Physical;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Improbable.Corelib.Visualizers.Transform
{
    [EngineType(EnginePlatform.FSim)]
    public class TransformVisualizer : MonoBehaviour
    {
        [Require] protected TransformReader Transform;

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

        private void UpdatePosition(Improbable.Math.Coordinates position)
        {
            transform.position = position.ToUnityVector();
        }

        private void UpdateRotation(Math.Quaternion rotation)
        {
            transform.rotation = rotation.ToUnityQuaternion();
        }
    }
}