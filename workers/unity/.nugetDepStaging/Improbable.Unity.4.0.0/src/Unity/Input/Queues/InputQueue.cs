/**
 * Timeline of input events. Calculates input value for current time.
 */
using System.Collections.Generic;
using Improbable.Unity.Input.Storage;

namespace Improbable.Unity.Input.Queues
{
    public abstract class InputQueue<TInputValue> : IInputQueue<TInputValue>
    {
        public TInputValue CurrentInputValue { get; protected set; }

        protected long PreviousUpdateTime;
        protected Queue<InputEvent> InputEvents;
        protected long CurrentUpdateTime;

        protected abstract TInputValue ExtractInputValueFromEvents();

        public virtual void Update(long elapsedMillis)
        {
            PreviousUpdateTime = CurrentUpdateTime;
            CurrentUpdateTime = elapsedMillis;

            RemoveEventsBeforeLastUpdate();
            CalculateCurrentInputValues();
        }

        private void RemoveEventsBeforeLastUpdate()
        {
            while (InputEvents.Count > 0 && InputEvents.Peek().Time < PreviousUpdateTime)
            {
                InputEvents.Dequeue();
            }
        }

        private void CalculateCurrentInputValues()
        {
            CurrentInputValue = InputEvents.Count == 0 ? default(TInputValue) : ExtractInputValueFromEvents();
        }
    }
}