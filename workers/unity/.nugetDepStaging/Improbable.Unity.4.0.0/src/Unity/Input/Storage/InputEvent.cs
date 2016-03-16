using UnityEngine;

namespace Improbable.Unity.Input.Storage
{
    /// <summary>
    /// Unity Input Types
    /// </summary>
    public enum InputType
    {
        Key,
        KeyDown,
        KeyUp,
        MouseButton,
        MouseButtonDown,
        MouseButtonUp,
        Axis,
        MouseAxis,
        AxisRaw,
        MouseAxisRaw,
        Button,
        ButtonDown,
        ButtonUp
    }

    public struct InputEvent
    {
        public InputType InputType;
        public KeyCode KeyCode;
        public string InputName;
        public float AxisValue;
        public int ButtonIdentifier;
        public long Time;

        public override string ToString()
        {
            return "Input Event - InputType: " + InputType + " KeyCode: " + KeyCode + " InputName: " + InputName + " AxisValue: " + AxisValue + " ButtonIdentifier: " + ButtonIdentifier + " Time: " + Time;
        }
    }
}