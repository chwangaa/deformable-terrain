namespace Improbable.Unity.Input.Sources
{
    public interface IButtonInputSource
    {
        bool GetButton(string buttonName);
        bool GetButtonDown(string buttonName);
        bool GetButtonUp(string buttonName);
    }
}