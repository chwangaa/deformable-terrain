using Improbable.Entity.Physical;
using Improbable.Unity;
using Improbable.Unity.Common.Core.Math;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Improbable.Corelib.Physical
{
    public class CollisionBehaviour : MonoBehaviour
    {
        [Require] protected CollisionStateWriter State;

        protected void OnCollisionEnter(UnityEngine.Collision collision)
        {
            if (!enabled || collision.contacts.Length < 1)
            {
                return;
            }

            var gameObjectHit = collision.collider.gameObject;
            var entityHit = gameObjectHit.GetEntityObject();
            var contact = collision.contacts[0];
            var collisionPoint = contact.point.ToCoordinates();
            var collisionNormal = contact.normal.ToNativeVector();
            var relativeVelocity = collision.relativeVelocity.ToNativeVector();

            if (entityHit == null)
            {
                State.Update
                     .TriggerCollision(null, collisionPoint, collisionNormal, relativeVelocity)
                     .FinishAndSend();
            }
            else
            {
                State.Update
                     .TriggerCollision(entityHit.EntityId, collisionPoint, collisionNormal, relativeVelocity)
                     .FinishAndSend();
            }
        }
    }
}
