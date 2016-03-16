using Improbable.Corelib.Util;
using Improbable.Entity.Physical;
using Improbable.Math;

namespace Improbable.Corelib.Csp.Position
{
    public class PositionStateValueSource : IValueSource<Coordinates>
    {
        private readonly PositionReader serverPosition;

        public PositionStateValueSource(PositionReader serverPosition)
        {
            this.serverPosition = serverPosition;
        }

        public Coordinates GetValue()
        {
            return serverPosition.Value;
        }
    }
}