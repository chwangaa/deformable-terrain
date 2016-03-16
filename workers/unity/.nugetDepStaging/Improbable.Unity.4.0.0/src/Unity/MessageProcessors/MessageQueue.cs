using System.Collections.Generic;
using Improbable.Fapi.Protocol;
using Improbable.Messages;
using log4net;

namespace Improbable.Unity.MessageProcessors
{
    internal class MessageQueue
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MessageQueue));

        private readonly Queue<EntityMessageBatch> deferredMessages = new Queue<EntityMessageBatch>();
        private readonly IDictionary<EntityId, EntityMessageBatch> entityIdToMessage = new Dictionary<EntityId, EntityMessageBatch>();

        internal int Count
        {
            get { return deferredMessages.Count; }
        }

        internal bool IsMessageForEntityInQueue(EntityId entityId)
        {
            return entityIdToMessage.ContainsKey(entityId);
        }

        internal void Enqueue(IEntityMessage message)
        {
            AddMessageToQueue(message);
        }

        internal EntityMessageBatch Dequeue()
        {
            var message = deferredMessages.Dequeue();
            entityIdToMessage.Remove(message.EntityId);
            return message;
        }

        private void AddMessageToQueue(object message)
        {
            var opCodes = message as OpCodes;
            if (opCodes != null)
            {
                AddOpCodesToQueue(opCodes);
            }
            else
            {
                var entityMessage = message as IEntityMessage;
                if (entityMessage != null)
                {
                    AddEntityMessageToQueue(entityMessage);
                }
                else
                {
                    Logger.Error(string.Format("Got a message in message queue of weird type {0}", message.GetType().FullName));
                }
            }
        }

        private void AddOpCodesToQueue(OpCodes opCodes)
        {
            for (var i = 0; i < opCodes.Codes.Count; ++i)
            {
                AddMessageToQueue(opCodes.Codes[i]);
            }
        }

        private void AddEntityMessageToQueue(IEntityMessage message)
        {
            EntityMessageBatch entityMessage;
            var entityId = message.EntityId;
            if (!entityIdToMessage.TryGetValue(entityId, out entityMessage))
            {
                entityMessage = EntityMessageBatch.Create(entityId);
                entityIdToMessage[entityId] = entityMessage;
                deferredMessages.Enqueue(entityMessage);
            }
            entityMessage.Add(message);
        }
    }
}