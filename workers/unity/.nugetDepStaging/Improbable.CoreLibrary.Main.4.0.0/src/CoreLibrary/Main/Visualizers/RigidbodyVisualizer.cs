using Improbable.Corelib.Physical.Visualizers;
using Improbable.Entity.Physical;
using Improbable.Unity.Visualizer;
using log4net;
using UnityEngine;

namespace Improbable.Core.GameLogic.Visualizers
{
    /// <summary>
    ///     This Visualizer adds Rigidbody components to all entities with the rigidbody nature. It should
    ///     probably be always be active on FSims, but it might make sense to disable this on clients even
    ///     if they are authoritative on transforms to reduce the overhead of the created rigid bodies,
    ///     at the cost of potentially making collisions with entities (that have Rigidbody components on
    ///     the FSim side) look non-physical. It also binds Rigidbody parameters to those updated by the
    ///     RigisbodyData state as long as we don't have authority on it using RigidbodyParametersBinder.
    /// </summary>
    public class RigidbodyVisualizer : MonoBehaviour, IRigidbodyVisualizer, IRigidbodyVelocitiesVisualizer
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(RigidbodyVisualizer));
        public int MaxConstructionAttempts = 100;

        private Rigidbody cachedRigidbody;
        [Require] protected RigidbodyDataReader rigidbodyData;
        [Require] protected RigidbodyEngineDataReader rigidbodyEngineData;
        private RigidbodyParametersBinder rigidbodyParametersBinder;
        private RigidbodyVelocitiesBinder rigidbodyVelocitiesBinder;

        private int currentAttempt;
        private bool isRigidbodyBeingDestroyed;

        public bool InitialVelocitiesApplied
        {
            get { return rigidbodyParametersBinder != null && rigidbodyVelocitiesBinder.InitialVelocitiesApplied; }
        }

        public RigidbodyEngineDataReader RigidbodyEngineData
        {
            get { return rigidbodyEngineData; }
        }

        public RigidbodyDataReader RigidbodyData
        {
            get { return rigidbodyData; }
        }

        public Rigidbody Rigidbody
        {
            get { return cachedRigidbody; }
        }

        protected void OnEnable()
        {
            currentAttempt = 0;
            var rigidbody = gameObject.GetComponent<Rigidbody>();
            if (!isRigidbodyBeingDestroyed && rigidbody != null)
            {
                LOGGER.WarnFormat("{0} was added to a GameObject ({1}), but it already had a Rigidbody." +
                                  "You may have a Rigidbody on your prefab, or perhaps have another active" +
                                  "Visualizer that adds a Rigidbody", GetType().Name, gameObject.name);
                UseCurrentRigidbody(rigidbody);
            }
            else
            {
                TryCreateRigidbody();
            }
        }

        protected void OnDisable()
        {
            if (cachedRigidbody != null)
            {
                rigidbodyParametersBinder.StopListeningToRigidbodyParameters();
                rigidbodyVelocitiesBinder.StopBinding();
                isRigidbodyBeingDestroyed = true;
                Destroy(Rigidbody);
                cachedRigidbody = null;
                RigidbodyData.AuthorityChanged -= RigidbodyDataAuthorityChanged;
                RigidbodyEngineData.AuthorityChanged -= RigidbodyEngineDataAuthorityChanged;
            }
        }

        /**
         * Todo: Change this to a coroutine once IEnumerator compiles.
         */
        protected void FixedUpdate()
        {
            if (cachedRigidbody == null)
            {
                TryCreateRigidbody();
            }
            else
            {
                UpdateRigidbody();
            }
        }

        private void TryCreateRigidbody()
        {
            var rigidbody = gameObject.GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                CreateRigidbody();
            }
            else
            {
                if (currentAttempt > MaxConstructionAttempts)
                {
                    LOGGER.ErrorFormat("Exceeded {0} attempts to construct a Rigidbody.", MaxConstructionAttempts);
                    UseCurrentRigidbody(rigidbody);
                }
                else
                {
                    currentAttempt++;
                }
            }
        }

        private void CreateRigidbody()
        {
            cachedRigidbody = RigidbodyParametersBinder.AddRigidbody(gameObject);
            CreateBinder();
        }

        private void UseCurrentRigidbody(Rigidbody rigidbody)
        {
            cachedRigidbody = rigidbody;
            CreateBinder();
        }

        private void CreateBinder()
        {
            rigidbodyParametersBinder = new RigidbodyParametersBinder(this);
            rigidbodyVelocitiesBinder = new RigidbodyVelocitiesBinder(this);

            RigidbodyData.AuthorityChanged += RigidbodyDataAuthorityChanged;
            RigidbodyEngineData.AuthorityChanged += RigidbodyEngineDataAuthorityChanged;

            rigidbodyParametersBinder.UpdateParameters();
        }

        private void UpdateRigidbody()
        {
            rigidbodyParametersBinder.FixedUpdate();
            rigidbodyVelocitiesBinder.FixedUpdate();
        }

        private void RigidbodyDataAuthorityChanged(bool isAuthoritativeHere)
        {
            if (!isAuthoritativeHere)
            {
                rigidbodyParametersBinder.ListenToRigidbodyParameters();
            }
            else
            {
                rigidbodyParametersBinder.StopListeningToRigidbodyParameters();
            }
        }

        private void RigidbodyEngineDataAuthorityChanged(bool isAuthoritativeHere)
        {
            if (!isAuthoritativeHere)
            {
                rigidbodyVelocitiesBinder.StartBinding();
            }
            else
            {
                rigidbodyVelocitiesBinder.StopBinding();
            }
        }
    }
}