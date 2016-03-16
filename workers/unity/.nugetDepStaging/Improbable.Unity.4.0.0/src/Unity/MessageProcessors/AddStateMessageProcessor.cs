using Improbable.Core.Entity;
using Improbable.Fapi.Protocol;

namespace Improbable.Unity.MessageProcessors
{
    internal class AddStateMessageProcessor : EntityStateMessageProcessorBase<AddState> 
    {
        public AddStateMessageProcessor(IUniverse universe) : base(universe) { }

        protected override void ProcessMsg(AddState addState)
        {
            var entityStateUpdate = addState.InitialState;
            var entityStateContainer = GetEntityStateContainer(entityStateUpdate.EntityId);
            entityStateContainer.AddState(entityStateUpdate);
            addState.ReturnToPool();
        }
    }
}