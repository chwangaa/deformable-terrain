using System.Collections.Generic;
using System.Linq;
using Improbable.Unity.Input.Queues;
using Improbable.Unity.Input.Sources;
using UnityEngine;

namespace Improbable.Unity.Input.Storage
{
    internal class KeyInputTimeline : InputTimeline<KeyCode, bool>, IKeyInputSource
    {
        public KeyInputTimeline(IEnumerable<InputEvent> capturedInput) : base(capturedInput) {}

        public bool GetKey(KeyCode keyCode)
        {
            return GetCurrentInputValue(InputType.Key, keyCode);
        }

        public bool GetKeyDown(KeyCode keyCode)
        {
            return GetCurrentInputValue(InputType.KeyDown, keyCode);
        }

        public bool GetKeyUp(KeyCode keyCode)
        {
            return GetCurrentInputValue(InputType.KeyUp, keyCode);
        }

        protected override List<InputType> InputTypes
        {
            get
            {
                return new List<InputType>
                {
                    InputType.Key,
                    InputType.KeyDown,
                    InputType.KeyUp
                };
            }
        }

        protected override KeyCode GetInputIdentifier(InputEvent input)
        {
            return input.KeyCode;
        }

        protected override void CreateInputQueue(InputType inputType, KeyCode inputName)
        {
            RecordedInputs[inputType].Add(inputName, new ButtonInputQueue(CapturedInput.Where(input => input.KeyCode == inputName)));
        }
    }
}