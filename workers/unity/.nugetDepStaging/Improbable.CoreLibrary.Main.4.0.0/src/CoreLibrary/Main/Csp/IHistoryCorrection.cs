namespace Improbable.Corelib.Csp
{
    public interface IHistoryCorrection<T> {
        void CorrectValue(ref T value, float timestamp);
    }
}