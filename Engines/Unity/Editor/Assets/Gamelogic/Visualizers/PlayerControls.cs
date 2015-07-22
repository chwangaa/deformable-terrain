using Improbable.Controls;
using Improbable.Unity.Input.Sources;
using Improbable.Unity.Visualizer;
using IoC;
using UnityEngine;
using Vector3 = Improbable.Math.Vector3;

namespace Assets.Gamelogic.Visualizers
{
    public class PlayerControls : MonoBehaviour
    {
        [Inject] public IInputSource InputSource { protected get; set; }

        [Require] private PlayerControlsDataWriter playerControls;

        private void FixedUpdate()
        {
            playerControls.Update.MovementDirection(GetMovementDirection()).FinishAndSend();
        }

        private Vector3 GetMovementDirection()
        {
            return new Vector3(InputSource.GetAxis("Horizontal"), 0, InputSource.GetAxis("Vertical"));
        }
    }
}