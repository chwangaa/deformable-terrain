namespace Improbable.Corelib.Util
{
    public interface ITimeSource {
        float CurrentTimeInSeconds { get; }
    }
}