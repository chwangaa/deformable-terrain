namespace Improbable.Corelib.Util
{
    public interface IValueSource<out T>
    {
        T GetValue();
    }
}