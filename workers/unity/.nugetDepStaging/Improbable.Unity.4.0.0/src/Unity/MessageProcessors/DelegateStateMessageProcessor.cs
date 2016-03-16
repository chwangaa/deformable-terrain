using Improbable.Core.Entity;
using Improbable.Fapi.Protocol;

namespace Improbable.Unity.MessageProcessors
{
    internal class DelegateStateMessageProcessor : EntityStateMessageProcessorBase<DelegateState>
    {
        public DelegateStateMessageProcessor(IUniverse universe) : base(universe) { }

        protected override void ProcessMsg(DelegateState delegateState)
        {
            var entityStateContainer = GetEntityStateContainer(delegateState.EntityId);
            entityStateContainer.DelegateState(delegateState.CanonicalStateName);
            delegateState.ReturnToPool();
        }
    }
}