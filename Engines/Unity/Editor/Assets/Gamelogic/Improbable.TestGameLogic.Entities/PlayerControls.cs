using Improbable.Math;
using Improbable.Unity.Input.Sources;
using IoC;

namespace Improbable.Controls.Observer
{
    public class PlayerControls : PlayerControlsBase
    {
        [Inject]
        public IInputSource InputSource { protected get; set; }

        private void FixedUpdate()
        {
            State.Update.MovementDirection(GetMovementDirection()).FinishAndSend();
        }

        private Vector3 GetMovementDirection()
        {
            return new Vector3(InputSource.GetAxis("Horizontal"), 0, InputSource.GetAxis("Vertical"));
        }
    }
}