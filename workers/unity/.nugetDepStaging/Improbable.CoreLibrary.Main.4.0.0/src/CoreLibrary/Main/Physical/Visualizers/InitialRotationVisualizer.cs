using Improbable.Entity.Physical;
using Improbable.Unity.Common.Core.Math;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Improbable.CoreLib.Physical.Visualizers
{
    public class InitialRotationVisualizer : MonoBehaviour
    {
        [Require] protected RotationReader Rotation;

        protected void OnEnable()
        {
            transform.rotation = Quaternion.Euler(Rotation.Euler.ToUnityVector());
        }
    }
}