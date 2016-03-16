using Improbable.Math;


namespace Improbable.Corelib.Interpolation
{
    public class PositionInterpolator : DelayedLinearInterpolator<Coordinates>
    {
        protected override Coordinates Interpolate(Coordinates currentPosition, Coordinates newPosition, float progressRatio)
        {
            var valueDelta = newPosition - currentPosition;
            return valueDelta * progressRatio + currentPosition;
        }
    }
}