using Improbable.Math;

namespace Improbable.Corelib.Interpolation
{
    public class RelativePositionInterpolator : DelayedLinearInterpolator<Vector3d>
    {
        protected override Vector3d Interpolate(Vector3d currentPosition, Vector3d newPosition, float progressRatio)
        {
            var valueDelta = newPosition - currentPosition;
            return valueDelta * progressRatio + currentPosition;
        }
    }
}