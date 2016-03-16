using Improbable.Entity.Physical;
using Improbable.Math;
using Improbable.Unity;
using Improbable.Unity.Common.Core.Math;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Improbable.TestGameLogic.Entities.Visualizers.Physical
{
  [EngineType(EnginePlatform.FSim)]
    public class TargetRotationVisualizer : MonoBehaviour
    {
        [Require] protected TargetRotationReader Rotation;

        protected void OnEnable()
        {
            Rotation.EulerUpdated += UpdateRotationIfNotAuthoritative;
            UpdateRotation(Rotation.Euler);
        }

        private void UpdateRotationIfNotAuthoritative(Vector3d rotation)
        {
            if (!Rotation.IsAuthoritativeHere)
            {
                UpdateRotation(rotation);
            }
        }

        private void UpdateRotation(Vector3d rotation)
        {
            transform.rotation = Quaternion.Euler(rotation.ToUnityVector());
        }
    }
}