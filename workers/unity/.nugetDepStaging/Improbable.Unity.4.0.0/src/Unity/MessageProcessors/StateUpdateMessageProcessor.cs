using Improbable.Core.Entity;
using Improbable.Fapi.Protocol;

namespace Improbable.Unity.MessageProcessors
{
    public class StateUpdateMessageProcessor : EntityStateMessageProcessorBase<ToEngineStateUpdate>
    {
        public StateUpdateMessageProcessor(IUniverse universe) : base(universe) { }

        protected override void ProcessMsg(ToEngineStateUpdate stateUpdate)
        {
            var entityStateContainer = GetEntityStateContainer(stateUpdate.EntityId);
            entityStateContainer.UpdateStateFromNetwork(stateUpdate.StateUpdate);
            stateUpdate.ReturnToPool();
        }
    }
}