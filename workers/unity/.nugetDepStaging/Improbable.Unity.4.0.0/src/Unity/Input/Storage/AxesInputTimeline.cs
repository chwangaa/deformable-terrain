using System.Collections.Generic;
using System.Linq;
using Improbable.Unity.Input.Queues;
using Improbable.Unity.Input.Sources;

namespace Improbable.Unity.Input.Storage
{
    public class AxesInputTimeline : InputTimeline<string, float>, IAxisInputSource
    {
        public AxesInputTimeline(IEnumerable<InputEvent> capturedInput) : base(capturedInput) {}

        public float GetAxisRaw(string axisName)
        {
            return GetCurrentInputValue(InputType.AxisRaw, axisName);
        }

        public float GetAxis(string axisName)
        {
            return GetCurrentInputValue(InputType.Axis, axisName);
        }

        protected override List<InputType> InputTypes
        {
            get
            {
                return new List<InputType>
                {
                    InputType.AxisRaw,
                    InputType.Axis,
                    InputType.MouseAxis,
                    InputType.MouseAxisRaw
                };
            }
        }

        protected override string GetInputIdentifier(InputEvent input)
        {
            return input.InputName;
        }

        protected override void CreateInputQueue(InputType inputType, string inputName)
        {
            RecordedInputs[inputType].Add(inputName, new AxisInputQueue(CapturedInput.Where(input => input.InputName == inputName)));
        }
    }
}