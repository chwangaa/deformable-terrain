using System.Collections.Generic;
using Improbable.Unity.Input.Storage;

namespace Improbable.Unity.Input.Queues
{
    public class ButtonInputQueue : InputQueue<bool>
    {
        public ButtonInputQueue(IEnumerable<InputEvent> inputEvents)
        {
            InputEvents = new Queue<InputEvent>(inputEvents);
        }

        protected override bool ExtractInputValueFromEvents()
        {
            return InputEvents.Peek().Time <= CurrentUpdateTime;
        }
    }
}