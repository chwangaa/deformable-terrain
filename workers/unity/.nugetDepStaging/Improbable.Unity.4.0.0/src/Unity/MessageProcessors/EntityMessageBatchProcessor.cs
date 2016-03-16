using Improbable.Fapi.Protocol;
using Improbable.Messages;

namespace Improbable.Unity.MessageProcessors
{
    internal class EntityMessageBatchProcessor : MessageProcessorDispatcher<EntityMessageBatch>
    {
        private readonly IMessageProcessor dispatcher;
        private readonly StateUpdateMessageProcessor entityStateMessageHandler;

        public EntityMessageBatchProcessor(IMessageProcessor dispatcher, StateUpdateMessageProcessor entityStateMessageHandler)
        {
            this.dispatcher = dispatcher;
            this.entityStateMessageHandler = entityStateMessageHandler;
        }

        protected override void ProcessMsg(EntityMessageBatch entityMsgs)
        {
            for (var i = 0; i < entityMsgs.AllMessages.Count; i++)
            {
                var msg = entityMsgs.AllMessages[i];
                var stateUpdate = msg as ToEngineStateUpdate;
                if (stateUpdate != null)
                {
                    entityStateMessageHandler.ProcessMsg(stateUpdate);
                }
                else
                {
                    dispatcher.ProcessMsg(msg);
                }
            }
            entityMsgs.ReturnToPool();
        }
    }
}