/**
 * Creates mapping from input identifier (i.e. axis name) to its corresponding input queue.
 */
using System.Collections.Generic;
using System.Linq;
using Improbable.Unity.Input.Queues;

namespace Improbable.Unity.Input.Storage
{
    public abstract class InputTimeline<TInputIdentifier, TInputValue>
    {
        protected Dictionary<InputType, Dictionary<TInputIdentifier, IInputQueue<TInputValue>>> RecordedInputs;
        protected List<InputEvent> CapturedInput;

        protected abstract List<InputType> InputTypes { get; }
        protected abstract TInputIdentifier GetInputIdentifier(InputEvent input);
        protected abstract void CreateInputQueue(InputType inputType, TInputIdentifier inputName);

        protected InputTimeline(IEnumerable<InputEvent> capturedInput)
        {
            CapturedInput = capturedInput.ToList();
            CreateInputQueueDictionaries();
        }

        public void Update(long timeInMillis)
        {
            foreach (var queues in RecordedInputs.Values)
            {
                foreach (var inputQueue in queues)
                {
                    inputQueue.Value.Update(timeInMillis);
                }
            }
        }

        protected TInputValue GetCurrentInputValue(InputType inputType, TInputIdentifier inputIdentifier)
        {
            IInputQueue<TInputValue> inputEvents;
            if (RecordedInputs[inputType].TryGetValue(inputIdentifier, out inputEvents))
            {
                return inputEvents.CurrentInputValue;
            }
            return default(TInputValue);
        }

        private void CreateInputQueueDictionaries()
        {
            RecordedInputs = new Dictionary<InputType, Dictionary<TInputIdentifier, IInputQueue<TInputValue>>>();

            foreach (var inputType in InputTypes)
            {
                RecordedInputs.Add(inputType, new Dictionary<TInputIdentifier, IInputQueue<TInputValue>>());
                CreateInputQueuesForInputType(inputType);
            }
        }

        private void CreateInputQueuesForInputType(InputType inputType)
        {
            var inputNames = GetDistinctInputs(inputType);
            foreach (var inputName in inputNames)
            {
                CreateInputQueue(inputType, inputName);
            }
        }

        private IEnumerable<TInputIdentifier> GetDistinctInputs(InputType inputType)
        {
            return CapturedInput.Where(input => input.InputType == inputType)
                                .Select<InputEvent, TInputIdentifier>(GetInputIdentifier).Distinct();
        }
    }
}