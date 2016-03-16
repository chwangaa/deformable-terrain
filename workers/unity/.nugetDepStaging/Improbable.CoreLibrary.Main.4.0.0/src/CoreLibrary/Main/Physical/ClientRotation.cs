using Improbable.Entity.Physical;
using Improbable.Unity.Common.Core.Math;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Improbable.Corelib.Physical
{
    public class ClientRotation : MonoBehaviour
    {
        [Require] protected TargetRotationWriter State;

        protected void FixedUpdate()
        {
            var rotation = transform.rotation.eulerAngles;
            State.Update.Euler(rotation.ToNativeVector()).FinishAndSend();
        }
    }
}
