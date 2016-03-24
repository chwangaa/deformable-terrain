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

        float lastActionTime;

        public void LateUpdate()
        {

            PlayerControls.Update.MovementDirection(GetMovementDirection()).FinishAndSend();
            if (InputSource.GetButton("Fire1"))
            {
                Debug.Log("Extinguish Fire event fired");
                PlayerControls.Update.TriggerExtinguishRequested().FinishAndSend();
            }
            if (InputSource.GetButton("Jump"))
            {
                Debug.Log("Plant tree event fired");
                PlayerControls.Update.TriggerPlantRequested().FinishAndSend();
            }
            if (InputSource.GetButton("Mouse X"))
            {
                PlayerControls.Update.TriggerReduceheightRequested().FinishAndSend();
            }
            if (InputSource.GetButton("Mouse Y"))
            {
                PlayerControls.Update.TriggerAddheightRequested().FinishAndSend();
            }
        }

        private Vector3 GetMovementDirection()
        {
            return new Vector3(InputSource.GetAxis("Horizontal"), 0, InputSource.GetAxis("Vertical"));
        }
    }
}