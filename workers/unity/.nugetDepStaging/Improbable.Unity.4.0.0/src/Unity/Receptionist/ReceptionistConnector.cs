using Improbable.Core;
using Improbable.Core.Network;
using Improbable.Fapi.Protocol;
using Improbable.Logging;
using Improbable.Network;
using Improbable.Unity.Core;
using log4net;
using UnityEngine;

namespace Improbable.Unity.Receptionist
{
    public class ReceptionistConnector : MonoBehaviour
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(ReceptionistConnector));
        private IBridgeCommunicator bridgeConnectionSetter;
        private EngineConfiguration engineConfiguration;
        private INetworkLinkFactory networkLinkFactory;
        
        private LogWriter logWriter;

        public bool LoggingActive = false;
        private IDeployment deployment;
        private string queuingCompleteToken;

        public void SetupDependencies(IDeployment deployment,
                                      string queuingCompleteToken,
                                      EngineConfiguration engineConfiguration,
                                      IBridgeCommunicator bridgeConnectionSetter,
                                      INetworkLinkFactory networkLinkFactory)
        {
            this.deployment = deployment;
            this.queuingCompleteToken = queuingCompleteToken;
            this.networkLinkFactory = networkLinkFactory;
            this.engineConfiguration = engineConfiguration;
            this.bridgeConnectionSetter = bridgeConnectionSetter;
            LoggingActive = engineConfiguration.ProtocolLoggingOnStartup;
            const int logFilesToKeep = 2;
            logWriter = new LogWriter(new FileLogStorage(engineConfiguration.ProtocolLogPrefix, ".log", engineConfiguration.ProtocolLogMaxFileBytes, logFilesToKeep))
            {
                Active = LoggingActive
            };
        }

        public void OnValidate()
        {
            logWriter.Active = LoggingActive;
        }

        public void Start()
        {
            UnityEngine.Camera.SetupCurrent(UnityEngine.Camera.main);
            AttemptLogin();
        }

        private void AttemptLogin()
        {
            var receptionistClient = new ReceptionistClient(
                deployment,
                engineConfiguration.EngineType,
                engineConfiguration.MetaData,
                engineConfiguration.LinkProtocol,
                engineConfiguration.HeartbeatInterval,
                engineConfiguration.MultiplexLevel,
                engineConfiguration.EngineId,
                engineConfiguration.LoginToken,
                queuingCompleteToken
                );
            ConnectToReceptionistAndProcessResponse(receptionistClient);
        }

        private void ConnectToReceptionistAndProcessResponse(ReceptionistClient receptionistClient)
        {
            StartCoroutine(receptionistClient.RequestClientManagerConnectionDetails(engineConfiguration.MaxReceptionistConnectionRetries, receptionistResponse =>
            {
                if (receptionistResponse != null)
                {
                    var host = engineConfiguration.UseInternalIpForBridge ? receptionistResponse.internalHost : receptionistResponse.externalHost;
                    ConnectToClientActor(host, receptionistResponse.port);
                }
                else
                {
                    LOGGER.Debug("Unable to connect to the receptionist");
                }
            }));
        }

        private void ConnectToClientActor(string hostname, int port)
        {
            LOGGER.Info("Connecting to ClientBridge: " + hostname + ":" + port);
            var bridgeConnection = new MigratableNetworkLink(networkLinkFactory, engineConfiguration.UseInternalIpForBridge, logWriter);
            bridgeConnection.StartConnection(hostname, port);
            bridgeConnectionSetter.SetupConnection(bridgeConnection);
            bridgeConnectionSetter.Send(new EngineReady());
        }

        private void OnDestroy()
        {
            logWriter.Dispose();
        }
    }
}
