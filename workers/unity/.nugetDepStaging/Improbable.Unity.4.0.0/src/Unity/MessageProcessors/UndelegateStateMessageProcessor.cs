using Improbable.Core.Entity;
using Improbable.Entity.State;
using Improbable.Fapi.Protocol;

namespace Improbable.Unity.MessageProcessors
{
    public class UndelegateStateMessageProcessor : EntityStateMessageProcessorBase<UndelegateState>
    {
        public UndelegateStateMessageProcessor(IUniverse universe) : base(universe) {}

        protected override void ProcessMsg(UndelegateState undelegateState)
        {
            IEntityStateContainer entityStateContainer = GetEntityStateContainer(undelegateState.EntityId);
            entityStateContainer.UndelegateState(undelegateState.CanonicalStateName);
            undelegateState.ReturnToPool();
        }
    }
}