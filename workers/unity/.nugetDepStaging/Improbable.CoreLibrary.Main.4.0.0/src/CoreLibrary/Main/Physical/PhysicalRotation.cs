using Improbable.Corelib.Util;
using Improbable.Entity.Physical;
using Improbable.Unity.Common.Core.Math;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Improbable.Corelib.Physical
{
    public class PhysicalRotation : PhysicalBase<Vector3>
    {
        [Tooltip("The minimum square distance from the last sent rotation before sending a new state update.")]
        public float RotationNetworkUpdateSquareDistanceThreshold;

        [Require] protected RotationWriter Rotation;

        protected override void OnEnable()
        {
            base.OnEnable();

            CachedTransform.rotation = Quaternion.Euler(Rotation.Euler.ToUnityVector());
        }

        protected override Vector3 GetLatestValue()
        {
            return CachedTransform.rotation.eulerAngles;
        }

        protected override bool IsPastThreshold(Vector3 lastRotation, Vector3 newRotation)
        {
            return Vector3Utils.SquareDistance(lastRotation, newRotation) >= RotationNetworkUpdateSquareDistanceThreshold;
        }

        protected override void OnShouldUpdate(float timeDelta, Vector3 newRotation)
        {
            Rotation.Update
                 .Timestamp(Rotation.Timestamp + timeDelta)
                 .Euler(newRotation.ToNativeVector())
                 .FinishAndSend();
        }
    }
}