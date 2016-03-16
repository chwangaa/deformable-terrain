namespace Improbable.Corelib.Util
{
    public interface IReadWriteValue<T> : IValueSource<T>
    {
        void SetValue(T value);
    }
}