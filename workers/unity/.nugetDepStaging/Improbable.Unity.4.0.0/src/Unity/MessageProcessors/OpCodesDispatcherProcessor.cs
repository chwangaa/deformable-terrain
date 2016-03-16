using Improbable.Fapi.Protocol;
using Improbable.Messages;

namespace Improbable.Unity.MessageProcessors
{
    internal class OpCodesDispatcherProcessor : MessageProcessorDispatcher<OpCodes>
    {
        private readonly IMessageProcessor MessageProcessor;

        public OpCodesDispatcherProcessor(IMessageProcessor messageProcessor)
        {
            MessageProcessor = messageProcessor;
        }

        protected override void ProcessMsg(OpCodes opcCodes)
        {
            // Prevents allocation of enumerator
            // ReSharper disable once ForCanBeConvertedToForeach
            for (int index = 0; index < opcCodes.Codes.Count; ++index)
            {
                var opCode = opcCodes.Codes[index];
                MessageProcessor.ProcessMsg(opCode);
            }

            opcCodes.ReturnToPool();
        }
    }
}