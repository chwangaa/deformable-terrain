using System;
using System.Collections.Generic;
using Improbable.Core.Network;
using Improbable.Fapi.Protocol;
using log4net.Appender;
using log4net.Core;
using log4net.Util;

namespace Improbable.Unity.Logging
{
    public class EngineBridgeAppender : AppenderSkeleton
    {
        private static readonly IDictionary<String, Protocol.EngineLogMessage.LogLevel> LOGGING_LEVELS =
            new Dictionary<String, Protocol.EngineLogMessage.LogLevel>
        {
            { "DEBUG", Protocol.EngineLogMessage.LogLevel.DEBUG },
            { "INFO", Protocol.EngineLogMessage.LogLevel.INFO },
            { "WARN", Protocol.EngineLogMessage.LogLevel.WARN },
            { "ERROR", Protocol.EngineLogMessage.LogLevel.ERROR },
            { "FATAL", Protocol.EngineLogMessage.LogLevel.FATAL }
        };

        public EngineBridgeAppender()
        {
            if (BridgeCommunicator == null)
            {
                LogLog.Error(GetType(), "EngineBridgeAppender does not have a BridgeCommunicator set before the Appender being constructed.");
            }
        }

        public static IBridgeCommunicator BridgeCommunicator { set; private get; }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (BridgeCommunicator != null)
            {
                EngineLogMessage.Enqueue(
                    BridgeCommunicator, 
                    LOGGING_LEVELS[loggingEvent.Level.DisplayName],
                    loggingEvent.LoggerName,
                    loggingEvent.RenderedMessage,
                    loggingEvent.TimeStamp.ToString("o"));
            }
        }
    }
}