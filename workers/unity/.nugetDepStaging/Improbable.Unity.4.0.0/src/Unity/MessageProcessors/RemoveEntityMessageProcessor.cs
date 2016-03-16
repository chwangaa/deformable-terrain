using System;
using Improbable.Core.Entity;
using Improbable.Fapi.Protocol;
using Improbable.Messages;
using Improbable.Util.Metrics;

namespace Improbable.Unity.MessageProcessors
{
    public class RemoveEntityMessageProcessor : MessageProcessorDispatcher<RemoveEntity>
    {
        private readonly IUniverse Universe;
        private readonly IGauge EntityCount;

        public RemoveEntityMessageProcessor(IUniverse universe, IGauge entityCount)
        {
            Universe = universe;
            EntityCount = entityCount;
        }

        protected override void ProcessMsg(RemoveEntity removeEntity)
        {
            try
            {
                if (Universe.ContainsEntity(removeEntity.EntityId))
                {
                    Universe.Destroy(removeEntity.EntityId);
                    EntityCount.Decrement();
                }
                else
                {
                    throw new InvalidOperationException(string.Format("Trying to destroy an entity we don't have: {0}", removeEntity.EntityId));
                }
            }
            finally
            {
                removeEntity.ReturnToPool();
            }
        }   
    }
}