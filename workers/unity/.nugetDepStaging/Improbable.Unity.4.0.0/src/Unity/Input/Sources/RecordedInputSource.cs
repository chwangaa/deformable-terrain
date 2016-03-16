using System.Collections.Generic;
using System.Linq;
using Improbable.Unity.Input.Storage;
using UnityEngine;

namespace Improbable.Unity.Input.Sources
{
    public class RecordedInputSource : IInputSource
    {
        private readonly AxesInputTimeline axesInputTimeline;
        private readonly KeyInputTimeline keyInputTimeline;
        private readonly ButtonInputTimeline buttonInputTimeline;
        private readonly MouseButtonInputTimeline mouseButtonInputTimeline;

        public RecordedInputSource(IEnumerable<InputEvent> capturedInput)
        {
            var capturedInputList = capturedInput.ToList();
            axesInputTimeline = new AxesInputTimeline(capturedInputList);
            keyInputTimeline = new KeyInputTimeline(capturedInputList);
            buttonInputTimeline = new ButtonInputTimeline(capturedInputList);
            mouseButtonInputTimeline = new MouseButtonInputTimeline(capturedInputList);
        }

        public void Update(long timeInMillis)
        {
            axesInputTimeline.Update(timeInMillis);
            buttonInputTimeline.Update(timeInMillis);
            keyInputTimeline.Update(timeInMillis);
            mouseButtonInputTimeline.Update(timeInMillis);
        }

        public float GetAxisRaw(string axisName)
        {
            return axesInputTimeline.GetAxisRaw(axisName);
        }

        public float GetAxis(string axisName)
        {
            return axesInputTimeline.GetAxis(axisName);
        }

        public bool GetKey(KeyCode keyCode)
        {
            return keyInputTimeline.GetKey(keyCode);
        }

        public bool GetKeyDown(KeyCode keyCode)
        {
            return keyInputTimeline.GetKeyDown(keyCode);
        }

        public bool GetKeyUp(KeyCode keyCode)
        {
            return keyInputTimeline.GetKeyUp(keyCode);
        }

        public bool GetMouseButton(int buttonIdentifier)
        {
            return mouseButtonInputTimeline.GetMouseButton(buttonIdentifier);
        }

        public bool GetMouseButtonUp(int buttonIdentifier)
        {
            return mouseButtonInputTimeline.GetMouseButtonUp(buttonIdentifier);
        }

        public bool GetMouseButtonDown(int buttonIdentifier)
        {
            return mouseButtonInputTimeline.GetMouseButtonDown(buttonIdentifier);
        }

        public bool GetButton(string buttonName)
        {
            return buttonInputTimeline.GetButton(buttonName);
        }

        public bool GetButtonDown(string buttonName)
        {
            return buttonInputTimeline.GetButtonDown(buttonName);
        }

        public bool GetButtonUp(string buttonName)
        {
            return buttonInputTimeline.GetButtonUp(buttonName);
        }
    }
}