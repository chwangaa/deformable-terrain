using Improbable.Game.G3.Entity.Player;
using Improbable.Unity.Client.Camera;
using Improbable.Unity.Visualizer;
using IoC;
using UnityEngine;

namespace Improbable.Corelib.Player
{
    public class PlayerOrientationController : MonoBehaviour
    {
        [Require] protected PlayerOrientationControllerDataWriter State;

        [Inject] public ICameraManager CameraManager { protected get; set; }

        protected void Start()
        {
            Camera[] componentsInChildren = gameObject.GetComponentsInChildren<Camera>();
            foreach (Camera componentsInChild in componentsInChildren)
            {
                if (componentsInChild.name.Contains("FPS"))
                {
                    CameraManager.CurrentCamera = componentsInChild;
                }
            }
        }

        protected void Update()
        {
            HandleControllerUpdate();
        }

        private void HandleControllerUpdate()
        {
            State.Update
                 .Azimuth(GetPlayerAzimuth())
                 .Pitch(GetPlayerPitch())
                 .FinishAndSend();
        }

        private float GetPlayerAzimuth()
        {
            return GetPlayerOrientation().x;
        }

        private float GetPlayerPitch()
        {
            float playerPitch = GetPlayerOrientation().y;
            return playerPitch;
        }

        private Vector2 GetPlayerOrientation()
        {
            Vector3 eulerAngles = gameObject.transform.rotation.eulerAngles;
            return new Vector2(eulerAngles.y, CalculatePitch());
        }

        //todo paul - this is silly but it's because we are using legacy FPS camera atm.
        private float CalculatePitch()
        {
            float possiblePitch = CameraManager.CurrentCamera.transform.eulerAngles.x;
            if (possiblePitch > 90)
            {
                possiblePitch -= 360;
            }
            return -possiblePitch;
        }

    }
}