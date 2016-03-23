using Improbable.Player;
using Improbable.Player.Controls;
using Improbable.Unity.Input.Sources;
using Improbable.Unity.Visualizer;
using IoC;
using UnityEngine;
using Vector3 = Improbable.Math.Vector3d;

namespace Assets.Gamelogic.Visualizers
{
    public class PlayerControlsVisualizer : MonoBehaviour
    {
        [Inject] public IInputSource InputSource { protected get; set; }

        [Require] protected LocalPlayerCheckStateWriter LocalPlayerCheck;
        [Require] protected PlayerControlsStateWriter PlayerControls;


        public void LateUpdate()
        {
            PlayerControls.Update.MovementDirection(GetMovementDirection()).FinishAndSend();
            if (InputSource.GetButton("Fire1"))
            {
                Debug.Log("check if Fire1 is handled in Unity");
                PlayerControls.Update.TriggerExtinguishRequested().FinishAndSend();
            }
            if (InputSource.GetButton("Jump"))
            {
                Debug.Log("Space button being pressed");
                GetComponent<Rigidbody>().isKinematic = true;
                PlayerControls.Update.TriggerPlantRequested().FinishAndSend();
            }
        }

        private Vector3 GetMovementDirection()
        {
            return new Vector3(InputSource.GetAxis("Horizontal"), 0, InputSource.GetAxis("Vertical"));
        }
    }
}