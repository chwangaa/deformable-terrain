using System;
using Improbable.Core.Entity;
using Improbable.Fapi.Protocol;
using Improbable.Messages;
using Improbable.Unity.Entity;
using Improbable.Util.Metrics;

namespace Improbable.Unity.MessageProcessors
{
    internal class AddEntityMessageProcessor : MessageProcessorDispatcher<AddEntity>
    {
        private readonly IUniverse Universe;
        private readonly IEntityFactory EntityFactory;
        private readonly IGauge EntityCount;

        public AddEntityMessageProcessor(IUniverse universe, IEntityFactory entityFactory, IGauge entityCount)
        {
            Universe = universe;
            EntityFactory = entityFactory;
            EntityCount = entityCount;
        }

        protected override void ProcessMsg(AddEntity addEntity)
        {
            try
            {
                if (!Universe.ContainsEntity(addEntity.EntityId))
                {
                    var entity = EntityFactory.MakeEntity(addEntity.EntityId, addEntity.Prefab, addEntity.Context);
                    Universe.AddEntity(addEntity.EntityId, entity);
                    EntityCount.Increment();
                }
                else
                {
                    throw new InvalidOperationException(string.Format("Trying to add the entity '{0}' (with prefab '{1}', context '{2}') that already exists.", addEntity.EntityId, addEntity.Prefab, addEntity.Context));
                }

            }
            finally
            {
                addEntity.ReturnToPool();                
            }
        }
    }
}