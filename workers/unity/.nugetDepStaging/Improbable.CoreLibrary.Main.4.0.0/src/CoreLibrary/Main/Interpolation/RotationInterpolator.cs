using UnityEngine;

namespace Improbable.Corelib.Interpolation
{
    public class RotationInterpolator : DelayedLinearInterpolator<Quaternion>
    {
        protected override Quaternion Interpolate(Quaternion currentRotation, Quaternion newRotation, float progressRatio)
        {
            return Quaternion.Lerp(currentRotation, newRotation, progressRatio);
        }
    }
}
