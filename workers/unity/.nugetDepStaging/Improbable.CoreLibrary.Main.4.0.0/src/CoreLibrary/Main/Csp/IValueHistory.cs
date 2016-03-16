using Improbable.Corelib.Interpolation;

namespace Improbable.Corelib.Csp
{
    public interface IValueHistory<T>
    {
        T ValueAtTime(float absoluteTimeInSeconds);
        void Reset(T initialValue, float currentTimeInSeconds);
        void RecordValue(T value, float absoluteTimeInSeconds);
        void ApplyCorrection(IHistoryCorrection<T> correctionToApply, float absoluteSecondsInPast);
        int Count { get; }
        TimestampedValue<T> this[int index] { get; set; }
    }
}