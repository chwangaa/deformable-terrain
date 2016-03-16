namespace Improbable.Unity.Input.Queues
{
    public interface IInputQueue<out TInputType>
    {
        TInputType CurrentInputValue { get; }
        void Update(long elapsedMillis);
    }
}