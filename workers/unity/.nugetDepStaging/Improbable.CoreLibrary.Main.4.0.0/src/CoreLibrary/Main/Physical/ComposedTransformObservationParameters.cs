using System;
using UnityEngine;

namespace Improbable.Corelib.Physical
{
    [Obsolete("Position and Rotation parameters are now set on the PhysicalPosition and PhysicalRotation MonoBehaviours directly.")]
    public class ComposedTransformObservationParameters : MonoBehaviour
    {
        public float PositionNetworkUpdatePeriodThreshold = DefaultPhysicalParameters.PositionNetworkUpdatePeriodThreshold;
        public float PositionNetworkUpdateSquareDistanceThreshold = DefaultPhysicalParameters.PositionNetworkUpdateSquareDistanceThreshold;
        public float RotationNetworkUpdatePeriodThreshold = DefaultPhysicalParameters.RotationNetworkUpdatePeriodThreshold;
        public float RotationNetworkUpdateSquareDistanceThreshold = DefaultPhysicalParameters.RotationNetworkUpdateSquareDistanceThreshold;
    }
}