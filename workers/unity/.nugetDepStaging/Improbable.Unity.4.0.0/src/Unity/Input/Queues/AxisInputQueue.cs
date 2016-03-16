using System.Collections.Generic;
using Improbable.Unity.Input.Storage;

namespace Improbable.Unity.Input.Queues
{
    public class AxisInputQueue : InputQueue<float>
    {
        public AxisInputQueue(IEnumerable<InputEvent> inputEvents)
        {
            InputEvents = new Queue<InputEvent>(inputEvents);
        }

        protected override float ExtractInputValueFromEvents()
        {
            var inputEvent = InputEvents.Peek();
            return inputEvent.Time <= CurrentUpdateTime ? inputEvent.AxisValue : 0;
        }
    }
}