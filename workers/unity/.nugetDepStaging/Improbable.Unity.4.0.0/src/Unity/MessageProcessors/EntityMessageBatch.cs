using System.Collections.Generic;
using Improbable.Entity;
using Improbable.Entity.State;
using Improbable.Fapi.Protocol;
using Improbable.Messages;
using Improbable.Util.Collections;

namespace Improbable.Unity.MessageProcessors
{
    internal class EntityMessageBatch : IObjectPoolable
    {
        private readonly Dictionary<int, IEntityStateUpdate> states = new Dictionary<int, IEntityStateUpdate>();
        private readonly IList<IEntityMessage> allMessages = new List<IEntityMessage>();

        private static readonly ObjectPool<EntityMessageBatch> Pool = new ObjectPool<EntityMessageBatch>();

        private void Init(EntityId entityId)
        {
            EntityId = entityId;
        }

        private void Reset()
        {
            EntityId = EntityId.InvalidEntityId;
            allMessages.Clear();
            states.Clear();
        }

        public static EntityMessageBatch Create(EntityId entityId)
        {
            var newObj = Pool.Get();
            newObj.Init(entityId);

            return newObj;
        }

        public EntityId EntityId { get; private set; }

        public void Add(IEntityMessage msg)
        {
            allMessages.Add(msg);

            var stateUpdate = msg as ToEngineStateUpdate;
            if (stateUpdate != null)
            {
                MergeState(stateUpdate.StateUpdate);
            }
        }

        public IList<IEntityMessage> AllMessages
        {
            get { return allMessages; }
        }

        private void MergeState(IEntityStateUpdate stateUpdate)
        {
            IEntityStateUpdate existingStateUpdate;
            states.TryGetValue(stateUpdate.StateUpdateFieldId, out existingStateUpdate);
            if (existingStateUpdate == null)
            {
                states.Add(stateUpdate.StateUpdateFieldId, stateUpdate);
            }
            else
            {
                existingStateUpdate.Aggregate(stateUpdate);
            }
        }

        public void ReturnToPool()
        {
            Reset();
            Pool.Return(this);
        }
    }
}