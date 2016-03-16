using System.Collections.Generic;
using System.Linq;
using Improbable.Core.Entity;
using Improbable.Unity.Visualizer;
using Improbable.Util.Collections;
using IoC;
using log4net;
using UnityEngine;

namespace Improbable.Corelib.Worker.Checkout
{
    public class EntityLoadingChecker : MonoBehaviour
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(EntityLoadingChecker));

        [Require] protected EntityLoadingControlReader LoadingControl;

        [Require] protected EntityLoadingResponseWriter LoadingResponse;

        [Inject] public IUniverse Universe { private get; set; }

        private HashSet<EntityId> entitiesLeft;

        protected void OnEnable()
        {
            LoadingControl.EntitiesUpdated += LoadEntities;
        }

        private void LoadEntities(IReadOnlyList<EntityId> entityIds)
        {
            LOGGER.DebugFormat("Waiting for {0} entities to load", entityIds.Count);
            LoadingResponse.Update.Loaded(false).FinishAndSend();
            entitiesLeft = new HashSet<EntityId>(entityIds);
        }

        protected void Update()
        {
            if (LoadingControl.LoadedState == EntityLoadingControlData.EntityLoadingStates.Requested)
            {
                var entitiesJustFound = UpdateEntitiesLeft();
                if (entitiesLeft.Count == 0)
                {
                    LOGGER.Debug("Entities have loaded");
                    LoadingResponse.Update.Loaded(true).FinishAndSend();
                }
                else if (entitiesJustFound > 0)
                {
                    LOGGER.DebugFormat("Entities left {0}", string.Join(",", entitiesLeft.Select(e => e.ToString()).ToArray()));
                }
            }
        }

        private int UpdateEntitiesLeft()
        {
            return entitiesLeft.RemoveWhere(entitiyId => Universe.ContainsEntity(entitiyId));
        }
    }
}