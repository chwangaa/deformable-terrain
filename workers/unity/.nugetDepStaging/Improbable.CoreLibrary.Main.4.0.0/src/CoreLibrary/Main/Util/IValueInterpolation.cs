namespace Improbable.Corelib.Util
{
    public interface IValueInterpolation<T>
    {
        T InterpolatedValue(ref T valueA, ref T valueB, float ratio);
    }
}