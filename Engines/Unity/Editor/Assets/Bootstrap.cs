using Improbable.Core.Network;
using Improbable.Unity;
using Improbable.Unity.Core;
using UnityEngine;

namespace Improbable
{
    public class Bootstrap : MonoBehaviour
    {
        public string ReceptionistIp = "localhost";
        public int ReceptionistPort = 7777;
        public EnginePlatform EngineType = EnginePlatform.Client;
        public int FixedUpdateRate = 20;
        public int TargetFps = 120;
        public bool UseLocalPrefabs = false;
        public bool UsePrefabPooling = true;
        public bool Instrument = true;
        public bool IsDebugMode = true;
        public LinkProtocol LinkProtocol = LinkProtocol.Tcp;
        public int MsgProcessLimitPerFrame = 100;

        private void Start()
        {
            var engineConfiguration = new EngineConfiguration
            {
                Ip = ReceptionistIp,
                Port = ReceptionistPort,
                TargetFps = TargetFps,
                FixedUpdateRate = FixedUpdateRate,
                UsePrefabPooling = UsePrefabPooling,
                UseLocalPrefabs = UseLocalPrefabs,
                EngineType = EngineTypeUtils.ToEngineName(EngineType),
                UseInstrumentation = Instrument,
                IsDebugMode = IsDebugMode,
                LinkProtocol = LinkProtocol,
				AccountsUrl = "https://account.improbable.io/",
				ImprobableAppId = "BANTER",
				AccessToken = "WOOO",
				MsgProcessLimitPerFrame = MsgProcessLimitPerFrame
            };
            var game = new GameRoot(gameObject, engineConfiguration);
            game.Start();
        }
    }
}