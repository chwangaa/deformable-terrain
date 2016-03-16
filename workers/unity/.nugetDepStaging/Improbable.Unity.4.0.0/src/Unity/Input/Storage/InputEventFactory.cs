using UnityEngine;

namespace Improbable.Unity.Input.Storage
{
    public static class InputEventFactory
    {
        public static InputEvent CreateAxisInputEvent(InputType inputType, string inputName, float axisValue, long time = 0)
        {
            return new InputEvent
            {
                InputType = inputType,
                InputName = inputName,
                AxisValue = axisValue,
                Time = time
            };
        }

        public static InputEvent CreateKeyInputEvent(InputType inputType, KeyCode keycode, long time = 0)
        {
            return new InputEvent
            {
                InputType = inputType,
                KeyCode = keycode,
                Time = time
            };
        }

        public static InputEvent CreateMouseButtonInputEvent(InputType inputType, int buttonIdentifier, long time = 0)
        {
            return new InputEvent
            {
                InputType = inputType,
                ButtonIdentifier = buttonIdentifier,
                Time = time
            };
        }

        public static InputEvent CreateButtonInputEvent(InputType inputType, string inputName, long time = 0)
        {
            return new InputEvent
            {
                InputType = inputType,
                InputName = inputName,
                Time = time
            };
        }
    }
}