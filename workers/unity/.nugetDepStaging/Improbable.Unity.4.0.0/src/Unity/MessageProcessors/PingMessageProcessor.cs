using Improbable.Fapi.Protocol;
using Improbable.Messages;
using Improbable.Core.Network;

namespace Improbable.Unity.MessageProcessors
{
    internal class PingMessageProcessor : MessageProcessorDispatcher<Ping>
    {
        private readonly IMessageSender MessageSender;

        public PingMessageProcessor (IMessageSender messageSender)
        {
            MessageSender = messageSender;
        }

        protected override void ProcessMsg (Ping pingMsg)
        {
            Pong.Enqueue(MessageSender, pingMsg.Timestamp);
            pingMsg.ReturnToPool();
        }
    }
}