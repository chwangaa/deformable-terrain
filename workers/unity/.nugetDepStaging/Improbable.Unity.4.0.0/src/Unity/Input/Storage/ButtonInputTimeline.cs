using System.Collections.Generic;
using System.Linq;
using Improbable.Unity.Input.Queues;
using Improbable.Unity.Input.Sources;

namespace Improbable.Unity.Input.Storage
{
    internal class ButtonInputTimeline : InputTimeline<string, bool>, IButtonInputSource
    {
        public ButtonInputTimeline(IEnumerable<InputEvent> capturedInput) : base(capturedInput) {}

        public bool GetButton(string buttonName)
        {
            return GetCurrentInputValue(InputType.Button, buttonName);
        }

        public bool GetButtonDown(string buttonName)
        {
            return GetCurrentInputValue(InputType.ButtonDown, buttonName);
        }

        public bool GetButtonUp(string buttonName)
        {
            return GetCurrentInputValue(InputType.ButtonUp, buttonName);
        }

        protected override List<InputType> InputTypes
        {
            get
            {
                return new List<InputType>
                {
                    InputType.Button,
                    InputType.ButtonDown,
                    InputType.ButtonUp
                };
            }
        }

        protected override string GetInputIdentifier(InputEvent input)
        {
            return input.InputName;
        }

        protected override void CreateInputQueue(InputType inputType, string inputName)
        {
            RecordedInputs[inputType].Add(inputName, new ButtonInputQueue(CapturedInput.Where(input => input.InputName == inputName)));
        }
    }
}