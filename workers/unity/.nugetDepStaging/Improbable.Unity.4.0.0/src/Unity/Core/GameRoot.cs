using System;
using System.Collections.Generic;
using Improbable.Bridge;
using Improbable.Core;
using Improbable.Core.Entity;
using Improbable.Core.Network;
using Improbable.Core.Network.RakNet;
using Improbable.Core.Network.Tcp;
using Improbable.Messages;
using Improbable.Unity.Camera;
using Improbable.Unity.Client.Camera;
using Improbable.Unity.Common.Core;
using Improbable.Unity.ComponentFactory;
using Improbable.Unity.Entity;
using Improbable.Unity.Input;
using Improbable.Unity.Input.Sources;
using Improbable.Unity.Logging;
using Improbable.Unity.MessageProcessors;
using Improbable.Unity.Receptionist;
using Improbable.Unity.RovingOrigin;
using Improbable.Unity.Util;
using Improbable.Util.Metrics;
using log4net;
using UnityEngine;

namespace Improbable.Unity.Core
{
    /// <summary>
    ///     EntryPoint to the improbable fabric.
    /// </summary>
    internal class GameRoot : IContextRoot
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(GameRoot));

        private readonly EngineConfiguration engineConfiguration;
        private readonly IEntityTemplateProvider templateProvider;
        private readonly IDeployment deployment;
        private readonly string queuingCompleteToken;
        private readonly GameObject gameEntry;

        private readonly IUniverse universe;
        private readonly BridgeCommunicator bridgeCommunicator;
        private PooledPrefabFactory pooledPrefabFactory;
        private readonly TypedMessageDispatcher mainTypedMessageDispatcher;
        private readonly DeferEntityCreationDispatcher deferEntityCreationDispatcher;
        private readonly IMetricsFactory metricsFactory;
        private readonly StateUpdateMessageProcessor entityStateMessageHandler;

        /// <param name="gameEntry">
        ///     The object the improbable architecture will bind components to. By convention this is usually
        ///     the same as the script invoking this constructor
        /// </param>
        /// <param name="engineConfiguration">Runtime Configuration for this engine</param>
        /// <param name="deployment"></param>
        /// <param name="queuingCompleteToken"></param>
        public GameRoot(GameObject gameEntry, EngineConfiguration engineConfiguration, IContainer container, IEntityTemplateProvider templateProvider, IDeployment deployment, string queuingCompleteToken)
        {
            this.gameEntry = gameEntry;
            this.engineConfiguration = engineConfiguration;
            this.templateProvider = templateProvider;
            this.container = container;
            this.deployment = deployment;
            this.queuingCompleteToken = queuingCompleteToken;
            this.gameEntry.AddComponent<ExceptionConsoleLogListener>();
            universe = new Universe();

            var engineEvents = gameEntry.AddComponent<UnityEngineEvents>();

            entityStateMessageHandler = new StateUpdateMessageProcessor(universe);
            mainTypedMessageDispatcher = new TypedMessageDispatcher();
            deferEntityCreationDispatcher = new DeferEntityCreationDispatcher(universe,
                                                                              entityStateMessageHandler,
                                                                              mainTypedMessageDispatcher, engineConfiguration.EntityCreationLimitPerFrame);
            engineEvents.Frame += deferEntityCreationDispatcher.ProcessDeferredMessagesBatch;

            bridgeCommunicator = CreateBridgeCommunicator(engineEvents);
            metricsFactory = new MetricsFactory(bridgeCommunicator);

            deferEntityCreationDispatcher.MetricsFactory = metricsFactory;

            var unityEntityPrefabFactory = CreateEntityPrefabFactory(metricsFactory);
            var entityFactory = CreateUnityEntityFactory(unityEntityPrefabFactory);

            SetupMessageProcessors(entityFactory);

            container.Bind<IUniverse>().AsSingle(universe);
            container.Bind<IInputSource>().AsSingle(new InputSourceManager());
            container.Bind<ICameraManager>().AsSingle<UnityCameraManager>();

            container.Bind<IMetricsCollector>().AsSingle(metricsFactory.Collector);
            container.Bind<IMetricsPublisher>().AsSingle(metricsFactory.Publisher);
        }


        public IContainer container { get; private set; }

        private void SubscribeRovingOrigin()
        {
            CoordinateSystem.LocalOriginMoved += CoordinateRemapper.CoodinateSystemOnLocalOriginMoved;
        }

        /// <summary>
        ///     Prepare to receive messages from Improbable fabric and start the connection.
        /// </summary>
        public void Start()
        {
            PrePoolPrefabs(engineConfiguration.PrefabToPool);
            ConfigureEngine();
            SubscribeRovingOrigin();
            SetupFabricConnection(deployment, queuingCompleteToken);
        }

        private void PrePoolPrefabs(IEnumerable<KeyValuePair<string, int>> prefabCounts)
        {
            if (pooledPrefabFactory == null)
            {
                Logger.Warn("Pools not initialized. PooledPrefabFactory is null");
                return;
            }

            foreach (var prefabNameToCount in prefabCounts)
            {
                // Prepooling only supports default context at the moment.  Ultimately all this code should be pulled out into user code.
                var entityAsset = new EntityAssetId(prefabNameToCount.Key, EntityAssetId.DEFAULT_CONTEXT);
                var requestedCountInPool = prefabNameToCount.Value;
                templateProvider.PrepareTemplate(entityAsset, (assetId) =>
                {
                    GameObject prefab = templateProvider.GetEntityTemplate(entityAsset);
                    pooledPrefabFactory.PreparePool(prefab, assetId, requestedCountInPool);
                }, (exception) => Logger.ErrorFormat("Problem initialising pool for entity {0} @ {1} : {2}", entityAsset.PrefabName, entityAsset.Context, exception.Message));
            }
        }

        private BridgeCommunicator CreateBridgeCommunicator(UnityEngineEvents engineEvents)
        {
            var communicator = new BridgeCommunicator(engineEvents,
                                                      deferEntityCreationDispatcher,
                                                      engineConfiguration.MsgProcessLimitPerFrame);
            communicator.Connected += AddClientMetricsComponent;
            if (engineConfiguration.UseInstrumentation)
            {
                gameEntry.AddComponent<MetricsUnityGui>();
                gameEntry.AddComponent<EngineTypeDisplay>();
                communicator.Connected += OnConnected; // TODO: two subscriptions for one class seems excessive.
            }
            if (engineConfiguration.Log4netConfigXml.Length > 0)
            {
                EngineBridgeAppender.BridgeCommunicator = communicator;
            }
            else
            {
                throw new Exception("No log4net configuration specified!");
            }
            communicator.Disconnected += OnDisconnected;
            return communicator;
        }

        private void OnDisconnected()
        {
            if (engineConfiguration.ShouldReconnect)
            {
                Application.LoadLevel(Application.loadedLevel);
            }
            else
            {
                Application.Quit();
            }
        }

        private IPrefabFactory<GameObject> CreateEntityPrefabFactory(IMetricsFactory factory)
        {
            IPrefabFactory<GameObject> unityPrefabFactory;
            if (engineConfiguration.UsePrefabPooling)
            {
                pooledPrefabFactory = new PooledPrefabFactory();
                unityPrefabFactory = pooledPrefabFactory;
            }
            else
            {
                unityPrefabFactory = new UnityPrefabFactory();
            }
            return new PrefabFactoryMetrics(metricsFactory, unityPrefabFactory);
        }

        private UnityEntityFactory CreateUnityEntityFactory(IPrefabFactory<GameObject> unityPrefabFactory)
        {
            return new UnityEntityFactory(bridgeCommunicator, container, unityPrefabFactory, templateProvider);
        }

        private void AddClientMetricsComponent()
        {
            if (engineConfiguration.EngineType == EngineTypeUtils.ToEngineName(EnginePlatform.Client))
            {
                gameEntry.AddComponent<ClientMetrics>();
            }
        }

        private void OnConnected()
        {
            gameEntry.AddComponent<EngineMetricsFPS>().SetupDependencies(bridgeCommunicator, engineConfiguration.FixedUpdateRate);

            SetupMetricsReporter();

            SetupEngineMetricsMemoryUsage();
        }

        private void SetupEngineMetricsMemoryUsage()
        {
            EngineMetricsMemoryUsage engineMetricsMemoryUsage = gameEntry.AddComponent<EngineMetricsMemoryUsage>();
            container.Inject(engineMetricsMemoryUsage);
            engineMetricsMemoryUsage.SetupDependencies();
        }

        private void SetupMetricsReporter()
        {
            var metricsReporter = gameEntry.GetComponent<MetricsReporter>();

            if (metricsReporter != null)
            {
                metricsReporter.SetupDependencies(bridgeCommunicator, metricsFactory.Publisher);
            }
            else
            {
                Logger.WarnFormat("MetricsReporter component is not present on GameObject '{0}'. No metrics will be sent to Fabric.", gameEntry.name);
            }
        }

        private void SetupFabricConnection(IDeployment deployment, string queuingCompleteToken)
        {
            var receptionistConnector = gameEntry.AddComponent<ReceptionistConnector>();
            receptionistConnector.SetupDependencies(deployment, queuingCompleteToken, engineConfiguration, bridgeCommunicator, CreateLinkFactory());
        }

        private INetworkLinkFactory CreateLinkFactory()
        {
            INetworkLinkFactory linkFactory = null;
            switch (engineConfiguration.LinkProtocol)
            {
                case LinkProtocol.Tcp:
                    linkFactory = new TcpLinkFactory(engineConfiguration.MultiplexLevel);
                    break;
                case LinkProtocol.RakNet:
                    linkFactory = new RakNetLinkFactory();
                    break;
            }
            return linkFactory;
        }

        private void SetupMessageProcessors(IEntityFactory entityFactory)
        {
            var entityCount = metricsFactory.Collector.Gauge("Entity Count");
            var addEntityMessageHandler = new AddEntityMessageProcessor(universe, entityFactory, entityCount); // Always defer
            var removeEntityMessageHandler = new RemoveEntityMessageProcessor(universe, entityCount);

            var addStateMessageHandler = new AddStateMessageProcessor(universe);
            var removeStateMessageHandler = new RemoveStateMessageProcessor(universe);
            var delegateStateMessageHandler = new DelegateStateMessageProcessor(universe);
            var undelegateStateMessageHandler = new UndelegateStateMessageProcessor(universe);
            var assetLoadRequestHandler = new AssetLoadRequestProcessor(bridgeCommunicator, templateProvider);
            var pingMessageHandler = new PingMessageProcessor(bridgeCommunicator);
            var opCodesMessageHandler = new OpCodesDispatcherProcessor(mainTypedMessageDispatcher);
            var batchedSingleEntityMsgProcessor = new EntityMessageBatchProcessor(mainTypedMessageDispatcher, entityStateMessageHandler);

            mainTypedMessageDispatcher.RegisterMessageProcessors(opCodesMessageHandler,
                                                                 addEntityMessageHandler,
                                                                 removeEntityMessageHandler,
                                                                 addStateMessageHandler,
                                                                 removeStateMessageHandler,
                                                                 delegateStateMessageHandler,
                                                                 undelegateStateMessageHandler,
                                                                 pingMessageHandler,
                                                                 assetLoadRequestHandler,
                                                                 batchedSingleEntityMsgProcessor);
        }

        private void ConfigureEngine()
        {
            var configurator = new UnityEngineConfigurator(); // TODO: this doesnt look good
            configurator.ConfigureEngine(engineConfiguration);
        }
    }
}