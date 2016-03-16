using System;
using Improbable.Core.Entity;
using Improbable.Fapi.Protocol;
using Improbable.Logging;
using Improbable.Messages;
using Improbable.Util;
using Improbable.Util.Metrics;
using log4net;

namespace Improbable.Unity.MessageProcessors
{
    // ReSharper disable ForCanBeConvertedToForeach
    internal class DeferEntityCreationDispatcher : IMessageProcessor
    {
        internal const string MessagesQueueSizeMetricName = "Deferred Entity Creation Queue Size";
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DeferEntityCreationDispatcher));
        // TODO: Change the log limiter to use something like exponential back-off instead of this hard-coded limit (see Jira SDK-442).
        // NOTE: Unity takes about 2ms per log message. Which means 10ms per frame will just keep the FSims from getting overloaded under normal load.
        private const int MaxLogMessagesBeforeQuietPeriod = 5;
        private static readonly TimeSpan QuietPeriod = new TimeSpan(0, 0, 1);
        private static readonly LogLimiter LogLimiter = new LogLimiter(new SystemTimeSource(), QuietPeriod, MaxLogMessagesBeforeQuietPeriod);

        private readonly IUniverse Universe;
        private readonly IMessageProcessor StateUpdateProcessor;
        private readonly IMessageProcessor TypedMessageDispatcher;
        private readonly int MaxMessagesPerFrame;
        private IMetricsFactory metricsFactory;
        private readonly MessageQueue entityMessageQueue = new MessageQueue();
        private IGauge queueSizeGauge;

        internal DeferEntityCreationDispatcher(IUniverse universe, IMessageProcessor stateUpdateProcessor, IMessageProcessor typedMessageDispatcher, int maxMessagesPerFrame)
        {
            Logger.Info(string.Format("Setting Creation limit to {0}", maxMessagesPerFrame));
            Universe = universe;
            StateUpdateProcessor = stateUpdateProcessor;
            TypedMessageDispatcher = typedMessageDispatcher;
            MaxMessagesPerFrame = maxMessagesPerFrame;
        }

        public void ProcessMsg(object message)
        {
            var opCodes = message as OpCodes;
            if (opCodes != null)
            {
                for (var i = 0; i < opCodes.Codes.Count; i++)
                {
                    ProcessMsg(opCodes.Codes[i]);
                }
                opCodes.ReturnToPool();
            }
            else
            {
                ProcessSingleMessage(message);
            }
        }

        private void ProcessSingleMessage(object message)
        {
            var entityMessage = message as IEntityMessage;
            if (entityMessage != null)
            {
                ProcessEntityMsg(entityMessage);
            }
            else
            {
                DispatchSingleMessageWithErrorHandling(message);
            }
        }

        public IMetricsFactory MetricsFactory
        {
            get { return metricsFactory; }
            set
            {
                if (value != metricsFactory)
                {
                    metricsFactory = value;
                    queueSizeGauge = metricsFactory == null ? null : metricsFactory.Collector.Gauge(MessagesQueueSizeMetricName);
                }
            }
        }

        internal void ProcessDeferredMessagesBatch()
        {
            for (int processedMessagesCount = 0; CanProcessAnotherMessage(processedMessagesCount); ++processedMessagesCount)
            {
                Dispatch(DequeueMessage());
            }
        }

        internal int MessageQueueCount
        {
            get { return entityMessageQueue.Count; }
        }

        private bool CanProcessAnotherMessage(int processedMessagesCount)
        {
            return (processedMessagesCount < MaxMessagesPerFrame || !IsRateLimitingEnabled) && MessageQueueCount != 0;
        }

        private bool IsRateLimitingEnabled
        {
            get { return MaxMessagesPerFrame != 0; }
        }

        private void ProcessEntityMsg(IEntityMessage message)
        {
            var addEntity = message as AddEntity;
            if (addEntity != null || entityMessageQueue.IsMessageForEntityInQueue(message.EntityId))
            {
                EnqueueEntityMessage(message);
            }
            else if (Universe.ContainsEntity(message.EntityId))
            {
                DispatchSingleMessageWithErrorHandling(message);
            }
            else
            {
                Logger.ErrorFormat("Tried to process an entity message {0} for unknown entity ID {1}.", message.GetType().Name, message.EntityId);
            }
        }

        private void Dispatch(object message)
        {
            var opCodes = message as OpCodes;
            if (opCodes != null)
            {
                DispatchOpCodes(opCodes);
            }
            else
            {
                DispatchSingleMessageWithErrorHandling(message);
            }
        }

        private void DispatchOpCodes(OpCodes opCodes)
        {
            for (int i = 0; i < opCodes.Codes.Count; ++i)
            {
                Dispatch(opCodes.Codes[i]);
            }
        }

        private void DispatchSingleMessageWithErrorHandling(object message)
        {
            try
            {
                DispatchSingleMessage(message);
            }
            catch (Exception exception)
            {
                LogExceptionForMessage(message, exception);
            }
        }

        private void DispatchSingleMessage(object message)
        {
            if (message is ToEngineStateUpdate)
            {
                StateUpdateProcessor.ProcessMsg(message);
            }
            else
            {
                TypedMessageDispatcher.ProcessMsg(message);
            }
        }

        private void EnqueueEntityMessage(IEntityMessage message)
        {
            entityMessageQueue.Enqueue(message);
            RefreshQueueSizeMetric();
        }

        private object DequeueMessage()
        {
            var message = entityMessageQueue.Dequeue();
            RefreshQueueSizeMetric();
            return message;
        }

        private void RefreshQueueSizeMetric()
        {
            if (queueSizeGauge != null)
            {
                queueSizeGauge.Set(entityMessageQueue.Count);
            }
        }

        private static void LogExceptionForMessage(object message, Exception exception)
        {
            if (LogLimiter.CanLogNow())
            {
                Logger.ErrorFormat("{0} with: {1}", GetErrorMessage(message), exception);
                LogLimiter.Logged();
                if (!LogLimiter.CanLogNow())
                {
                    Logger.InfoFormat("Reached log limit of {0} messages. Entering quiet period for {1} seconds.", LogLimiter.MaxMessageCount, LogLimiter.QuietPeriod.Seconds);
                }
            }
        }

        private static string GetErrorMessage(object message)
        {
            var entityMessage = message as IEntityMessage;
            if (entityMessage != null)
            {
                return string.Format("Failed to process message {0} on Entity {1}", entityMessage.GetType().Name, entityMessage.EntityId);
            }
            return string.Format("Failed to process message {0}", message.GetType().Name);
        }
    }

    // ReSharper restore ForCanBeConvertedToForeach
}