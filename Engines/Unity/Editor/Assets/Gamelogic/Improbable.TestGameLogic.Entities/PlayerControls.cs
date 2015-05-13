using System.Net.Sockets;
using Improbable.Unity.Common.Core.Math;
using Improbable.Unity.Input.Sources;
using IoC;
using UnityEngine;
using Vector3 = Improbable.Math.Vector3;

namespace Improbable.Controls.Observer
{
    public class PlayerControls : PlayerControlsBase
    {
        [Inject]
        public IInputSource InputSource { protected get; set; }

        private Transform CameraRoot;

        private void OnEnable()
        {
            CameraRoot = transform.FindChild("CameraRoot");
        }

        private void Update()
        {
            State.Update.MovementDirection(
                GetMovementDirection(new Vector3(InputSource.GetAxis("Horizontal"), 0, InputSource.GetAxis("Vertical"))))
                .FinishAndSend();
        }

        private Vector3 GetMovementDirection(Vector3 inputDirection)
        {
            var resultDirection = CameraRoot.transform.rotation * inputDirection.ToUnityVector();
            UnityEngine.Debug.Log(resultDirection);
            return resultDirection.normalized.ToNativeVector();
        }
    }
}