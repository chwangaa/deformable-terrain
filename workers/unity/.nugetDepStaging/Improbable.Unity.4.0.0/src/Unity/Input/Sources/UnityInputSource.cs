using UnityEngine;

namespace Improbable.Unity.Input.Sources
{
    public class UnityInputSource : IInputSource
    {
        public float GetAxisRaw(string axisName)
        {
            return UnityEngine.Input.GetAxisRaw(axisName);
        }

        public float GetAxis(string axisName)
        {
            return UnityEngine.Input.GetAxis(axisName);
        }

        public bool GetKey(KeyCode keyCode)
        {
            return UnityEngine.Input.GetKey(keyCode);
        }

        public bool GetKeyDown(KeyCode keyCode)
        {
            return UnityEngine.Input.GetKeyDown(keyCode);
        }

        public bool GetKeyUp(KeyCode keyCode)
        {
            return UnityEngine.Input.GetKeyUp(keyCode);
        }

        public bool GetMouseButton(int buttonIdentifier)
        {
            return UnityEngine.Input.GetMouseButton(buttonIdentifier);
        }

        public bool GetMouseButtonUp(int buttonIdentifier)
        {
            return UnityEngine.Input.GetMouseButtonUp(buttonIdentifier);
        }

        public bool GetMouseButtonDown(int buttonIdentifier)
        {
            return UnityEngine.Input.GetMouseButtonDown(buttonIdentifier);
        }

        public bool GetButton(string buttonName)
        {
            return UnityEngine.Input.GetButton(buttonName);
        }

        public bool GetButtonDown(string buttonName)
        {
            return UnityEngine.Input.GetButtonDown(buttonName);
        }

        public bool GetButtonUp(string buttonName)
        {
            return UnityEngine.Input.GetButtonUp(buttonName);
        }
    }
}