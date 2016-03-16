using Improbable.Corelib.Util;
using Improbable.Math;

namespace Improbable.Corelib.Csp.Position
{
    public class PositionInterpolation : IValueInterpolation<Coordinates>
    {
        public Coordinates InterpolatedValue(ref Coordinates valueA, ref Coordinates valueB, float ratio)
        {
            var valueDelta = valueB - valueA;
            return valueDelta * ratio + valueA;
        }
    }
}