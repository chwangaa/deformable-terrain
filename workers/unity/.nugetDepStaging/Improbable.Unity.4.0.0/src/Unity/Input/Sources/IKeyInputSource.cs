using UnityEngine;

namespace Improbable.Unity.Input.Sources
{
    public interface IKeyInputSource
    {
        bool GetKey(KeyCode keyCode);
        bool GetKeyDown(KeyCode keyCode);
        bool GetKeyUp(KeyCode keyCode);
    }
}