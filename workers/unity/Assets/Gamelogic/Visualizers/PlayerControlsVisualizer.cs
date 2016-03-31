using Improbable.Player;
using Improbable.Player.Controls;
using Improbable.Unity.Input.Sources;
using Improbable.Unity.Visualizer;
using IoC;
using Improbable.Unity;
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
            if (InputSource.GetButtonDown("Fire1"))
            {
                Debug.Log("Extinguish Fire event fired");
                var hit_position = CoordinateSystem.ToCoordinates(transform.position);

                PlayerControls.Update.TriggerFiringRequested(hit_position).FinishAndSend();
            }
            if (InputSource.GetButtonDown("Jump"))
            {
                Debug.Log("Plant tree event fired");
                PlayerControls.Update.TriggerPlantRequested().FinishAndSend();
            }
            if (InputSource.GetButtonDown("Mouse X"))
            {
                PlayerControls.Update.TriggerReduceheightRequested().FinishAndSend();
            }
            if (InputSource.GetButtonDown("Mouse Y"))
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