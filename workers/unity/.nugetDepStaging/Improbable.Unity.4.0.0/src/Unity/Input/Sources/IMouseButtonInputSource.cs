namespace Improbable.Unity.Input.Sources
{
    public interface IMouseButtonInputSource
    {
        bool GetMouseButton(int buttonIdentifier);
        bool GetMouseButtonUp(int buttonIdentifier);
        bool GetMouseButtonDown(int buttonIdentifier);
    }
}