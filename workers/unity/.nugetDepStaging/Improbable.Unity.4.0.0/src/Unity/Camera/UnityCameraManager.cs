using Improbable.Unity.Client.Camera;
using UnityEngine;

namespace Improbable.Unity.Camera
{
    public class UnityCameraManager : ICameraManager
    {
        public UnityCameraManager()
        {
            CurrentCamera = UnityEngine.Camera.current;
        }

        public UnityEngine.Camera CurrentCamera { get; set; }

        public Vector3 CameraRotation
        {
            get { return CurrentCamera.transform.eulerAngles; }
        }

        public Vector3 CameraDirection
        {
            get { return CurrentCamera.transform.forward; }
        }
    }
}