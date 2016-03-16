namespace Improbable.Corelib.Interpolation
{
    public struct TimestampedValue<TValue>
    {
        public TValue Value;
        public float Timestamp;

        public TimestampedValue(TValue value, float timestamp)
        {
            Value = value;
            Timestamp = timestamp;
        }
    }
}