using Improbable.Unity.Input.Sources;
using UnityEngine;

namespace Improbable.Unity.Input
{
    public class InputSourceManager : IInputSource
    {
        public IInputSource CurrentInputSource { get; set; }
        public static InputSourceManager Instance;

        public InputSourceManager()
        {
            Instance = this;
            CurrentInputSource = new UnityInputSource();
        }

        public float GetAxisRaw(string axisName)
        {
            return CurrentInputSource.GetAxisRaw(axisName);
        }

        public float GetAxis(string axisName)
        {
            return CurrentInputSource.GetAxis(axisName);
        }

        public bool GetKey(KeyCode keyCode)
        {
            return CurrentInputSource.GetKey(keyCode);
        }

        public bool GetKeyUp(KeyCode keyCode)
        {
            return CurrentInputSource.GetKeyUp(keyCode);
        }

        public bool GetKeyDown(KeyCode keyCode)
        {
            return CurrentInputSource.GetKeyDown(keyCode);
        }

        public bool GetMouseButtonDown(int code)
        {
            return CurrentInputSource.GetMouseButtonDown(code);
        }

        public bool GetMouseButton(int buttonIdentifier)
        {
            return CurrentInputSource.GetMouseButton(buttonIdentifier);
        }

        public bool GetMouseButtonUp(int buttonIdentifier)
        {
            return CurrentInputSource.GetMouseButtonUp(buttonIdentifier);
        }

        public bool GetButton(string buttonName)
        {
            return CurrentInputSource.GetButton(buttonName);
        }

        public bool GetButtonDown(string buttonName)
        {
            return CurrentInputSource.GetButtonDown(buttonName);
        }

        public bool GetButtonUp(string buttonName)
        {
            return CurrentInputSource.GetButtonUp(buttonName);
        }
    }
}