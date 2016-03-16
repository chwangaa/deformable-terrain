using System.Collections.Generic;
using Improbable.Core.Entity;
using Improbable.Entity.State;
using Improbable.Messages;
using Improbable.Unity.Entity;

namespace Improbable.Unity.MessageProcessors
{
    public abstract class EntityStateMessageProcessorBase<TMsg> : MessageProcessorDispatcher<TMsg>
    {
        private readonly IUniverse universe;

        protected EntityStateMessageProcessorBase(IUniverse universe)
        {
            this.universe = universe;
        }

        protected IEntityStateContainer GetEntityStateContainer(EntityId entityId)
        {
            var entity = GetEntity(entityId);
            return entity.EntityStateContainer;
        }

        protected IEntityObject GetEntity(EntityId entityId)
        {
            var entity = universe.Get(entityId);
            if (entity == null)
            {
                throw new KeyNotFoundException(string.Format("Unable to find entity {0}", entityId));
            }
            return entity;
        }
    }
}