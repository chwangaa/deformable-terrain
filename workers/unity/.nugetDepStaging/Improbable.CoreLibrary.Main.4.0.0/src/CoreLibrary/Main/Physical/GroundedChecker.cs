using Improbable.Unity.Visualizer;
using log4net;
using UnityEngine;
using Improbable.Entity;

namespace Improbable.Corelib.Physical
{
    /// <summary>
    ///     Calculates for a CapsuleCollider whether it is touching the ground or not
    ///     To understand this code you need to understand how unity's phyiscal event ordering works
    ///     http://wiki.unity3d.com/index.php/Event_Execution_Order
    ///     Note- this implementation is very efficient for stationary entities
    /// </summary>
    public class GroundedChecker : MonoBehaviour
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(GroundedChecker));
        [Require] protected GroundedWriter State;

        private float maximumInclineVerticalComponent;

        private Collider currentContactCollider;
        private Collider previousContactCollider;

        protected void Start()
        {
            if (GetComponentInChildren<CapsuleCollider>() == null)
            {
                LOGGER.Error("The grounding system requires a capsule collider in the game object's hierarchy.");
            }
            maximumInclineVerticalComponent = Mathf.Cos(State.MaximumInclineDegrees * Mathf.Deg2Rad);
        }

        /// <summary>
        ///     Called before the physics simulation for the frame
        /// </summary>
        protected void FixedUpdate()
        {
            var rigidbodyComponent = GetComponent<Rigidbody>();

            if (rigidbodyComponent!=null && !rigidbodyComponent.IsSleeping())
            {
                if (currentContactCollider != previousContactCollider)
                {
                    bool isGroundedOnSomething = currentContactCollider != null && currentContactCollider.gameObject != null;

                    var groundEntityObject = isGroundedOnSomething ? currentContactCollider.gameObject.GetEntityObject() : null;
                    State.Update
                         .GroundEntityId(groundEntityObject == null ? EntityId.InvalidEntityId : groundEntityObject.EntityId)
                         .IsGrounded(isGroundedOnSomething)
                         .FinishAndSend();
                    previousContactCollider = currentContactCollider;
                }
                currentContactCollider = null;
            }
        }

        /// <summary>
        ///     Called after the physics simulation for the frame, if any collisions have been maintained
        /// </summary>
        protected void OnCollisionStay(Collision collision)
        {
            foreach (var contact in collision.contacts)
            {
                if (IsGroundNormal(contact.normal))
                {
                    currentContactCollider = contact.otherCollider;
                    return;
                }
            }
        }

        public bool IsGroundNormal(Vector3 normal)
        {
            return normal.y > maximumInclineVerticalComponent;
        }
    }
}
