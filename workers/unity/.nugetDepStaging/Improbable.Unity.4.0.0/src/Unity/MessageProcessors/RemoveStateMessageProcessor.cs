using Improbable.Core.Entity;
using Improbable.Fapi.Protocol;

namespace Improbable.Unity.MessageProcessors
{
    public class RemoveStateMessageProcessor : EntityStateMessageProcessorBase<RemoveState>
    {
        public RemoveStateMessageProcessor(IUniverse universe) : base(universe) { }

        protected override void ProcessMsg(RemoveState removeState)
        {
            var entityStateContainer = GetEntityStateContainer(removeState.EntityId);
            entityStateContainer.RemoveState(removeState.StateFieldId);

            removeState.ReturnToPool();
        }
    }
}