using Improbable.Core.GameLogic.Visualizers;
using Improbable.Entity.Physical;
using Improbable.Unity.Common.Core.Math;
using Improbable.Unity.Visualizer;
using log4net;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Improbable.Corelib.Physical
{
    /// <summary>
    ///     This Visualizer syncs the RigidbodyEngineData state with Unity's Rigidbody component if we have
    ///     authority on it.
    /// </summary>
    public class RigidbodySync : MonoBehaviour
    {
        private const float UPDATE_THRESHOLD = 0.1f;
        private const float UPDATE_INTERVAL = 0.1f;
        private const float CENTER_OF_MASS_UPDATE_INTERVAL = 1.0f;

        private float lastUpdatedVelocity = 0;
        private float lastUpdatedAngularVelocity = 0;
        private float lastUpdateCenterOfMass = 0;

        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(RigidbodySync));
        [Require] protected RigidbodyEngineDataWriter State;
        private Rigidbody cachedRigidbody;

        private Vector3 lastAngularVelocity;
        private Vector3 lastCenterOfMass;
        private Vector3 lastVelocity;

        private RigidbodyVisualizer rigidbodyVisualizer;

        private Rigidbody CachedRigidbody
        {
            get { return cachedRigidbody != null ? cachedRigidbody : (cachedRigidbody = gameObject.GetComponent<Rigidbody>()); }
        }

        public bool InitialVelocitiesApplied
        {
            get { return rigidbodyVisualizer != null && rigidbodyVisualizer.InitialVelocitiesApplied; }
        }

        protected void OnEnable()
        {
            rigidbodyVisualizer = gameObject.GetComponent<RigidbodyVisualizer>();
            if (rigidbodyVisualizer == null)
            {
                LOGGER.Error("Could not find the rigidbody visualizer. It is needed to apply the initial rigidbody state before we can start observing rigidbody properties.");
            }
        }

        protected void Update()
        {
            // These are called here so that the load of sending messages is spread across frames, rather than in every FixedUpdate.
            if (AngularVelocityNeedsUpdating() || VelocityNeedsUpdating())
            {
                SendVelocityDataUpdate();
            }
            if (ShouldUpdateCenterOfMassNow())
            {
                SendCenterOfMassUpdate();
            }
        }
        
        private void SendCenterOfMassUpdate()
        {
            lastCenterOfMass = CachedRigidbody.centerOfMass;
            lastUpdateCenterOfMass = Time.time;
            State.Update.RelativeCentreOfMass(lastCenterOfMass.ToNativeVector()).FinishAndSend();
        }

        private void SendVelocityDataUpdate()
        {
            if (CachedRigidbody != null && InitialVelocitiesApplied)
            {
                lastUpdatedVelocity = Time.time;
                lastUpdatedAngularVelocity = Time.time;

                lastVelocity = CachedRigidbody.velocity;
                lastAngularVelocity = CachedRigidbody.angularVelocity;

                State.Update.
                      Velocity(new Improbable.Math.Vector3d(lastVelocity.x, lastVelocity.y, lastVelocity.z)).
                      AngularVelocity(new Improbable.Math.Vector3d(lastAngularVelocity.x, lastAngularVelocity.y, lastAngularVelocity.z)).
                      FinishAndSend();
            }
        }

        private bool IsPointWithinDistance(Vector3 v, Vector3 center, float radius)
        {
            var sqrDistance = Vector3.SqrMagnitude(v - center);
            return sqrDistance < radius * radius;
        }

        private bool ShouldUpdateCenterOfMassNow()
        {
            return CachedRigidbody != null &&
                   Time.time - lastUpdateCenterOfMass > CENTER_OF_MASS_UPDATE_INTERVAL &&
                   IsPointWithinDistance(lastCenterOfMass, CachedRigidbody.centerOfMass, UPDATE_THRESHOLD);// Most expensive test last
        }

        private bool VelocityNeedsUpdating()
        {
            return CachedRigidbody != null &&
                   Time.time - lastUpdatedVelocity > UPDATE_INTERVAL &&
                   !IsPointWithinDistance(lastVelocity, CachedRigidbody.velocity, UPDATE_THRESHOLD);
        }

        private bool AngularVelocityNeedsUpdating()
        {
            return CachedRigidbody != null &&
                   Time.time - lastUpdatedAngularVelocity > UPDATE_INTERVAL &&
                   !IsPointWithinDistance(lastAngularVelocity, CachedRigidbody.angularVelocity, UPDATE_THRESHOLD);
        }
    }
}
