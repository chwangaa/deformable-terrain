using UnityEngine;

//todo - lipka - temp namespace for backwards compatibility reasons
namespace Improbable.Unity.Client.Camera
{
    public interface ICameraManager
    {
        UnityEngine.Camera CurrentCamera { get; set; }
        Vector3 CameraRotation { get; }
        Vector3 CameraDirection { get; }
    }
}
