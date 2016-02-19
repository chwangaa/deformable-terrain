using System;
using System.Collections.Generic;
using Improbable.Core;
using Improbable.Core.Network;
using Improbable.Fapi.Receptionist;
using Improbable.Unity;
using Improbable.Unity.Core;
using UnityEngine;

namespace Improbable
{
    public class Bootstrap : MonoBehaviour, IBootstrapHandler
    {
        public string AssemblyName;
        public string ReceptionistIp = "localhost";
        public int ReceptionistPort = 7777;
        public EnginePlatform EngineType = EnginePlatform.Client;
        public int FixedUpdateRate = 20;
        public int TargetFps = 120;
        public bool UsePrefabPooling = true;
        public LinkProtocol LinkProtocol = LinkProtocol.Tcp;

        public void Start()
        {
            EngineConfiguration.Instance.AssemblyName = AssemblyName;
            EngineConfiguration.Instance.Ip = ReceptionistIp;
            EngineConfiguration.Instance.Port = ReceptionistPort;
            EngineConfiguration.Instance.TargetFps = TargetFps;
            EngineConfiguration.Instance.FixedUpdateRate = FixedUpdateRate;
            EngineConfiguration.Instance.UsePrefabPooling = UsePrefabPooling;
            EngineConfiguration.Instance.PrefabToPool = Prepool();
            EngineConfiguration.Instance.EngineType = EngineTypeUtils.ToEngineName(EngineType);
            EngineConfiguration.Instance.UseInstrumentation = true;
            EngineConfiguration.Instance.IsDebugMode = true;
            EngineConfiguration.Instance.LinkProtocol = LinkProtocol;
            EngineConfiguration.Instance.AppName = "boxes_tutorial";
            EngineConfiguration.Instance.MsgProcessLimitPerFrame = 0;
            EngineConfiguration.Instance.Log4netConfigXml = "log4net-local.xml";

            EngineLifecycleManager.StartGame(this, gameObject);
        }

        private static Dictionary<string, int> Prepool()
        {
            return new Dictionary<string, int>();
        }
        
        public void OnDeploymentListRetrieved(IList<IDeployment> deployments, Action<IDeployment> handleChosenDeployment)
        {
            handleChosenDeployment(deployments[0]);
        }

        public void OnQueuingStarted()
        {
            Debug.Log("Queueing started");
        }

        public void OnQueuingUpdate(IQueueStatus status)
        {
            Debug.Log(status);
        }

        public void OnQueuingCompleted(IQueueStatus status)
        {
            Debug.Log("Queueing complete");
        }

        public void OnBootstrapError(Exception exception)
        {
            Debug.LogError("Exception: " + exception.Message);
        }

        public void BeginPreconnectionTasks(IDeployment deployment, IContainer container, Action onCompletedPreconnectionTasks)
        {
            onCompletedPreconnectionTasks();
        }
    }
}
