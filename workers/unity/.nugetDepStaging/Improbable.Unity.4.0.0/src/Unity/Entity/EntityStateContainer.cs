using System;
using System.Collections.Generic;
using Improbable.Entity.State;
using Improbable.Unity.State;
using log4net;

namespace Improbable.Unity.Common.Entity.State
{
    // TODO: matej: Move the entity state container to Core once the tests are moved.

    /// <summary>
    ///     Default implementation of IEntityStateContainer
    /// </summary>
    public class EntityStateContainer : IEntityStateContainer
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(EntityStateContainer));
        
        private readonly EntityId entityId;

        // this contains the concrete state type (e.g. DynamicSlots).  It implements IReadWriteEntityState, and the Writer version (which implements the Reader version)
        private readonly Dictionary<string, IReadWriteEntityState> nameToInstanceMap = new Dictionary<string, IReadWriteEntityState>();
        private readonly Dictionary<int, IReadWriteEntityState> fieldIdToStateMap = new Dictionary<int, IReadWriteEntityState>();

        private readonly IStateSender updateStateSender;
        private readonly IStateMetadataLookup stateMetadataLookup;

        private readonly List<IEntityStateSubscriber> subscriberList = new List<IEntityStateSubscriber>();

        public EntityStateContainer(EntityId entityId, IStateSender updateStateSender, IStateMetadataLookup stateMetadataLookup)
        {
            this.entityId = entityId;
            this.updateStateSender = updateStateSender;
            this.stateMetadataLookup = stateMetadataLookup;
        }

        public void AddSubscriber(IEntityStateSubscriber subscriber)
        {
            subscriberList.Add(subscriber);
        }

        public void RemoveSubscriber(IEntityStateSubscriber subscriber)
        {
            subscriberList.Remove(subscriber);
        }

        public void AddState(IEntityStateUpdate stateData)
        {
            var state = stateData.CreateState(entityId, updateStateSender);
            fieldIdToStateMap.Add(stateData.StateUpdateFieldId, state);

            var canonicalName = stateMetadataLookup.GetCanonicalStateName(state.GetType());

            if (!string.IsNullOrEmpty(canonicalName))
            {
                nameToInstanceMap.Add(canonicalName, state); 
            }
            else
            {
                LOGGER.ErrorFormat("Unable to get canonical name for state {0}", state.GetType());
            }
            
            OnStateAdded(state);
        }

        // This works for the internal type, or any type that has a CanonicalName attribute on it.
        public IReadWriteEntityState GetInternalStateInstance(Type stateType)
        {
            string stateName = stateMetadataLookup.GetCanonicalStateName(stateType);
            IReadWriteEntityState entityState;

            if (!string.IsNullOrEmpty(stateName) && nameToInstanceMap.TryGetValue(stateName, out entityState))
            {
                return entityState;
            }

            return null;
        }

        public void UpdateStateFromNetwork(IEntityStateUpdate stateData)
        {
            IReadWriteEntityState state;
            if (fieldIdToStateMap.TryGetValue(stateData.StateUpdateFieldId, out state))
            {
                state.UpdateFromNetwork(stateData);
            }
            else
            {
                throw new InvalidOperationException(string.Format("Trying to update the state '{0}', but it doesn't exist in this container.", stateData.StateUpdateFieldId));
            }
        }

        public void RemoveState(int stateFieldId)
        {
            var state = fieldIdToStateMap[stateFieldId];
            OnStateRemoved(state);
            nameToInstanceMap.Remove(stateMetadataLookup.GetCanonicalStateName(state.GetType()));
            fieldIdToStateMap.Remove(stateFieldId);
        }

        public void DelegateState(string canonicalStateName)
        {
            IReadWriteEntityState state;
            if (nameToInstanceMap.TryGetValue(canonicalStateName, out state))
            {
                state.SetAuthoritativeHere(true);
                OnStateDelegated(state);
            }
            else
            {
                throw new Exception(string.Format("State {0} could not be delegated as it is not present", canonicalStateName));
            }
        }

        public void UndelegateState(string canonicalStateName)
        {
            IReadWriteEntityState state;
            if (nameToInstanceMap.TryGetValue(canonicalStateName, out state))
            {
                state.SetAuthoritativeHere(false);
                OnStateUndelegated(state);
            }
            else
            {
                throw new Exception(string.Format("State {0} could not be undelegated as it is not present", canonicalStateName));
            }
        }

        // ReSharper disable ForCanBeConvertedToForeach
        private void OnStateDelegated(IReadWriteEntityState state)
        {
            for (int index = 0; index < subscriberList.Count; ++index)
            {
                var stateSubscriber = subscriberList[index];
                stateSubscriber.OnStateDelegated(state);
            }
        }

        private void OnStateUndelegated(IReadWriteEntityState state)
        {
            for (int index = 0; index < subscriberList.Count; ++index)
            {
                var stateSubscriber = subscriberList[index];
                stateSubscriber.OnStateUndelegated(state);
            }
        }

        private void OnStateAdded(IReadWriteEntityState state)
        {
            for (int index = 0; index < subscriberList.Count; ++index)
            {
                var stateSubscriber = subscriberList[index];
                stateSubscriber.OnStateAdded(state);
            }
        }

        private void OnStateRemoved(IReadWriteEntityState state)
        {
            for (int index = 0; index < subscriberList.Count; ++index)
            {
                var stateSubscriber = subscriberList[index];
                stateSubscriber.OnStateRemoved(state);
            }
        }
        // ReSharper restore ForCanBeConvertedToForeach
    }
}
