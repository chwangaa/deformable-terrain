using System;
using System.Collections.Generic;
using Improbable.Core;
using Improbable.Core.Network;

namespace Improbable.Unity.Core
{
    public class EngineConfiguration : Configuration
    {
        private readonly CompositeConfiguration configuration;
        private readonly DictionaryConfiguration defaultvalues;

        private EngineConfiguration()
        {
            defaultvalues = new DictionaryConfiguration
            {
                { ConfigNames.AUTH_PROVIDER, "dummy" },
                { ConfigNames.DEPLOYMENT_ID, string.Empty },
                { ConfigNames.DEPLOYMENT_TAG, "prod" },
                { ConfigNames.ENGINE_TYPE, null },
                { ConfigNames.ENTITY_CREATION_LIMIT_PER_FRAME, 250 },
                { ConfigNames.FIXED_UPDATE_RATE, 20 },
                { ConfigNames.HEARTBEAT_INTERVAL, string.Empty },
                { ConfigNames.INFRA_LOCATOR_URL, "https://locator.improbable.io" },
                { ConfigNames.INFRA_SERVICE_URL, "https://api.spatial.improbable.io" },
                { ConfigNames.IS_DEBUG_MODE, false },
                { ConfigNames.LINK_PROTOCOL, LinkProtocol.Tcp.ToString() },
                { ConfigNames.LOCATOR_URL, "https://locator.improbable.io" },
                { ConfigNames.LOGGER_CONFIG, "log4net-default.xml" },
                { ConfigNames.LOGIN_TOKEN, "" },
                { ConfigNames.MAX_PRECACHING_CONCURRENT_CONNECTIONS, 5 },
                { ConfigNames.MAX_RECEPTIONIST_CONNECTION_RETRIES, 2000 },
                { ConfigNames.META_DATA, new Dictionary<string, string>() },
                { ConfigNames.MULTIPLEX_LEVEL, string.Empty },
                { ConfigNames.MSG_PROCESS_LIMIT_PER_FRAME, 5000 },
                { ConfigNames.PREFABS_TO_POOL, new Dictionary<string, int>() },
                { ConfigNames.PROTOCOL_LOG_PREFIX, "protocol-" },
                { ConfigNames.PROTOCOL_LOGGING_ON_STARTUP, false },
                { ConfigNames.PROTOCOL_LOG_MAX_FILE_BYTES, 100 * 1024 * 1024 },
                { ConfigNames.RECEPTIONIST_IP, "localhost" },
                { ConfigNames.RECEPTIONIST_PORT, -1 },
                { ConfigNames.REFRESH_TOKEN, string.Empty },
                { ConfigNames.SHOULD_RECONNECT, true },
                { ConfigNames.STEAM_TOKEN, "" },
                { ConfigNames.TARGET_FPS, 60 },
                { ConfigNames.USE_INSTRUMENTATION, true },
                { ConfigNames.USE_INTERNAL_IP_FOR_BRIDGE, false },
                { ConfigNames.USE_PER_INSTANCE_ASSET_CACHE, true },
                { ConfigNames.USE_PREFAB_POOLING, false },
                { ConfigNames.USE_TERRAIN_STREAMING, true },
                { ConfigNames.USE_HTTPS, false }
            };
            configuration = new CompositeConfiguration(new CommandLineArguments(), defaultvalues);
        }

        private static EngineConfiguration instance = new EngineConfiguration();
        public static EngineConfiguration Instance { get { return instance; } }

        public string EngineId
        {
            get { return GetConfigValue(ConfigNames.ENGINE_ID, EngineType + Guid.NewGuid()); }
        }

        public int? HeartbeatInterval
        {
            get
            {
                var intervalString = GetConfigValue<string>(ConfigNames.HEARTBEAT_INTERVAL);
                return string.IsNullOrEmpty(intervalString) ? (int?) null : Convert.ToInt32(intervalString);
            }
            set { defaultvalues[ConfigNames.HEARTBEAT_INTERVAL] = value.ToString(); }
        }

        public int MsgProcessLimitPerFrame
        {
            get { return GetConfigValue<int>(ConfigNames.MSG_PROCESS_LIMIT_PER_FRAME); }
            set { defaultvalues[ConfigNames.MSG_PROCESS_LIMIT_PER_FRAME] = value; }
        }

        public int EntityCreationLimitPerFrame
        {
            get { return GetConfigValue<int>(ConfigNames.ENTITY_CREATION_LIMIT_PER_FRAME); }
            set { defaultvalues[ConfigNames.ENTITY_CREATION_LIMIT_PER_FRAME] = value; }
        }

        [Obsolete("This is no longer supported")]
        public bool UseTerrainStreaming
        {
            get { return configuration.GetConfigValue<bool>(ConfigNames.USE_TERRAIN_STREAMING); }
            set { defaultvalues[ConfigNames.USE_TERRAIN_STREAMING] = value; }
        }

        public bool UseInternalIpForBridge
        {
            get { return configuration.GetConfigValue<bool>(ConfigNames.USE_INTERNAL_IP_FOR_BRIDGE); }
            set { defaultvalues[ConfigNames.USE_INTERNAL_IP_FOR_BRIDGE] = value; }
        }

        public string RefreshToken
        {
            get { return GetConfigValue<string>(ConfigNames.REFRESH_TOKEN); }
            set { defaultvalues[ConfigNames.REFRESH_TOKEN] = value; }
        }

        public string ImprobableAppId
        {
            get { return GetConfigValue<string>(ConfigNames.IMPROBABLE_APP_ID); }
            set { defaultvalues[ConfigNames.IMPROBABLE_APP_ID] = value; }
        }

        public string DeploymentId
        {
            get { return GetConfigValue<string>(ConfigNames.DEPLOYMENT_ID); }
            set { defaultvalues[ConfigNames.DEPLOYMENT_ID] = value; }
        }

        public string AuthProvider
        {
            get { return GetConfigValue<string>(ConfigNames.AUTH_PROVIDER); }
            set { defaultvalues[ConfigNames.AUTH_PROVIDER] = value; }
        }

        public string AccountsUrl
        {
            get { return GetConfigValue<string>(ConfigNames.ACCOUNTS_URL); }
            set { defaultvalues[ConfigNames.ACCOUNTS_URL] = value; }
        }

        public string EngineType
        {
            get { return GetConfigValue<string>(ConfigNames.ENGINE_TYPE); }
            set { defaultvalues[ConfigNames.ENGINE_TYPE] = value; }
        }

        public string Ip
        {
            get { return GetConfigValue<string>(ConfigNames.RECEPTIONIST_IP); }
            set { defaultvalues[ConfigNames.RECEPTIONIST_IP] = value; }
        }

        public int Port
        {
            get { return GetConfigValue<int>(ConfigNames.RECEPTIONIST_PORT); }
            set { defaultvalues[ConfigNames.RECEPTIONIST_PORT] = value; }
        }

        public Dictionary<string, string> MetaData
        {
            get { return GetConfigValue<Dictionary<string, string>>(ConfigNames.META_DATA); }
            set { defaultvalues[ConfigNames.META_DATA] = value; }
        }

        public LinkProtocol LinkProtocol
        {
            get { return (LinkProtocol) Enum.Parse(typeof(LinkProtocol), GetConfigValue<string>(ConfigNames.LINK_PROTOCOL)); }
            set { defaultvalues[ConfigNames.LINK_PROTOCOL] = value.ToString(); }
        }

        public int? MultiplexLevel
        {
            get
            {
                var levelString = GetConfigValue<string>(ConfigNames.MULTIPLEX_LEVEL);
                return string.IsNullOrEmpty(levelString) ? (int?) null : Convert.ToInt32(levelString);
            }
            set { defaultvalues[ConfigNames.MULTIPLEX_LEVEL] = value.ToString(); }
        }

        public int TargetFps
        {
            get { return GetConfigValue<int>(ConfigNames.TARGET_FPS); }
            set { defaultvalues[ConfigNames.TARGET_FPS] = value; }
        }

        public int FixedUpdateRate
        {
            get { return GetConfigValue<int>(ConfigNames.FIXED_UPDATE_RATE); }
            set { defaultvalues[ConfigNames.FIXED_UPDATE_RATE] = value; }
        }

        public bool UsePrefabPooling
        {
            get { return GetConfigValue<bool>(ConfigNames.USE_PREFAB_POOLING); }
            set { defaultvalues[ConfigNames.USE_PREFAB_POOLING] = value; }
        }

        public bool UseInstrumentation
        {
            get { return GetConfigValue<bool>(ConfigNames.USE_INSTRUMENTATION); }
            set { defaultvalues[ConfigNames.USE_INSTRUMENTATION] = value; }
        }

        public bool IsDebugMode
        {
            get { return GetConfigValue<bool>(ConfigNames.IS_DEBUG_MODE); }
            set { defaultvalues[ConfigNames.IS_DEBUG_MODE] = value; }
        }

        public string Log4netConfigXml
        {
            get { return GetConfigValue<string>(ConfigNames.LOGGER_CONFIG); }
            set { defaultvalues[ConfigNames.LOGGER_CONFIG] = value; }
        }

        public bool UsePerInstanceAssetCache
        {
            get { return GetConfigValue<bool>(ConfigNames.USE_PER_INSTANCE_ASSET_CACHE); }
            set { defaultvalues[ConfigNames.USE_PER_INSTANCE_ASSET_CACHE] = value; }
        }

        public bool UseHttps
        {
            get { return GetConfigValue<bool>(ConfigNames.USE_HTTPS); }
            set { defaultvalues[ConfigNames.USE_HTTPS] = value; }
        }

        public string LocatorUrl
        {
            get { return GetConfigValue<string>(ConfigNames.LOCATOR_URL); }
            set { defaultvalues[ConfigNames.LOCATOR_URL] = value; }
        }

        public int MaxReceptionistConnectionRetries
        {
            get { return GetConfigValue<int>(ConfigNames.MAX_RECEPTIONIST_CONNECTION_RETRIES); }
            set { defaultvalues[ConfigNames.MAX_RECEPTIONIST_CONNECTION_RETRIES] = value; }
        }

        public bool ShouldReconnect
        {
            get { return GetConfigValue<bool>(ConfigNames.SHOULD_RECONNECT); }
            set { defaultvalues[ConfigNames.SHOULD_RECONNECT] = value; }
        }

        public string ProtocolLogPrefix
        {
            get { return GetConfigValue<string>(ConfigNames.PROTOCOL_LOG_PREFIX); }
            set { defaultvalues[ConfigNames.PROTOCOL_LOG_PREFIX] = value; }
        }

        public int ProtocolLogMaxFileBytes
        {
            get { return GetConfigValue<int>(ConfigNames.PROTOCOL_LOG_MAX_FILE_BYTES); }
            set { defaultvalues[ConfigNames.PROTOCOL_LOG_MAX_FILE_BYTES] = value; }
        }

        public bool ProtocolLoggingOnStartup
        {
            get { return GetConfigValue<bool>(ConfigNames.PROTOCOL_LOGGING_ON_STARTUP); }
            set { defaultvalues[ConfigNames.PROTOCOL_LOGGING_ON_STARTUP] = value; }
        }

        public int MaxPrecachingConcurrentConnections
        {
            get { return GetConfigValue<int>(ConfigNames.MAX_PRECACHING_CONCURRENT_CONNECTIONS); }
            set { defaultvalues[ConfigNames.MAX_PRECACHING_CONCURRENT_CONNECTIONS] = value; }
        }

        public string InfraLocatorUrl
        {
            get { return GetConfigValue<string>(ConfigNames.INFRA_LOCATOR_URL); }
            set { defaultvalues[ConfigNames.INFRA_LOCATOR_URL] = value; }
        }

        public string InfraServiceUrl
        {
            get { return GetConfigValue<string>(ConfigNames.INFRA_SERVICE_URL); }
            set { defaultvalues[ConfigNames.INFRA_SERVICE_URL] = value; }
        }

        public IEnumerable<KeyValuePair<string, int>> PrefabToPool
        {
            get { return GetConfigValue<IEnumerable<KeyValuePair<string, int>>>(ConfigNames.PREFABS_TO_POOL); }
            set { defaultvalues[ConfigNames.PREFABS_TO_POOL] = value; }
        }

        public string SteamToken
        {
            get { return GetConfigValue<string>(ConfigNames.STEAM_TOKEN); }
            set { defaultvalues[ConfigNames.STEAM_TOKEN] = value; }
        }

        /// <summary>
        ///     Used for fetching deployments with the given tag to allow the users to choose the shard to connect to.
        ///     NOTE: This is only used for running client for deployed games.
        /// </summary>
        public string DeploymentTag
        {
            get { return GetConfigValue<string>(ConfigNames.DEPLOYMENT_TAG); }
            set { defaultvalues[ConfigNames.DEPLOYMENT_TAG] = value; }
        }

        /// <summary>
        ///     This specifies the name of the application. There's no default values for this.
        ///     This must be set by the client.
        ///     NOTE: This is only used for using the client against deployed games.
        /// </summary>
        public string AppName
        {
            get { return GetConfigValue<string>(ConfigNames.APP_NAME); }
            set { defaultvalues[ConfigNames.APP_NAME] = value; }
        }

        /// <summary>
        ///     This specifies the name of the assembly to use. There's no default values for this.
        ///     NOTE: This is only used for using the client against deployed games.
        /// </summary>
        public string AssemblyName
        {
            get { return GetConfigValue<string>(ConfigNames.ASSEMBLY_NAME); }
            set { defaultvalues[ConfigNames.ASSEMBLY_NAME] = value; }
        }

        public string LoginToken
        {
            get { return GetConfigValue<string>(ConfigNames.LOGIN_TOKEN); }
            set { defaultvalues[ConfigNames.LOGIN_TOKEN] = value; }
        }

        public override bool TryGetConfigValue<T>(string configName, out T value)
        {
            return configuration.TryGetConfigValue(configName, out value);
        }
    }
}