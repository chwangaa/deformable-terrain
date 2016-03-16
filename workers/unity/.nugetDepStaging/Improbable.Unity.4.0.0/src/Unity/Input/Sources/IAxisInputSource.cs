namespace Improbable.Unity.Input.Sources
{
    public interface IAxisInputSource
    {
        float GetAxisRaw(string axisName);
        float GetAxis(string axisName);
    }
}