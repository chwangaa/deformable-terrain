using System.Collections.Generic;
using System.Linq;
using Improbable.Unity.Input.Queues;
using Improbable.Unity.Input.Sources;

namespace Improbable.Unity.Input.Storage
{
    internal class MouseButtonInputTimeline : InputTimeline<int, bool>, IMouseButtonInputSource
    {
        public MouseButtonInputTimeline(IEnumerable<InputEvent> capturedInput) : base(capturedInput) {}

        public bool GetMouseButton(int buttonIdentifier)
        {
            return GetCurrentInputValue(InputType.MouseButton, buttonIdentifier);
        }

        public bool GetMouseButtonUp(int buttonIdentifier)
        {
            return GetCurrentInputValue(InputType.MouseButtonUp, buttonIdentifier);
        }

        public bool GetMouseButtonDown(int buttonIdentifier)
        {
            return GetCurrentInputValue(InputType.MouseButtonDown, buttonIdentifier);
        }

        protected override List<InputType> InputTypes
        {
            get
            {
                return new List<InputType>
                {
                    InputType.MouseButton,
                    InputType.MouseButtonDown,
                    InputType.MouseButtonUp
                };
            }
        }

        protected override int GetInputIdentifier(InputEvent input)
        {
            return input.ButtonIdentifier;
        }

        protected override void CreateInputQueue(InputType inputType, int inputName)
        {
            RecordedInputs[inputType].Add(inputName, new ButtonInputQueue(CapturedInput.Where(input => input.ButtonIdentifier == inputName)));
        }
    }
}