using System.Collections.Generic;
using System.IO;
using Improbable.Assets;
using Newtonsoft.Json;
using UnityEditor;

namespace Improbable.Unity.EditorTools.Build
{
    internal static class DefaultPlayerBuildConfiguration
    {
        private static readonly List<string> CurrentPlatformTargetList = new List<string> { "Current" };

        internal static PlayerBuildConfiguation Generate()
        {
            var config = new PlayerBuildConfiguation
            {
                Deploy = new Enviroment
                {
                    FSim = new Config
                    {
                        Assets = AssetDatabaseStrategy.Streaming,
                        Targets = new List<string>
                        {
                            BuildTarget.StandaloneLinux64.ToString()
                        }
                    },
                    Client = new Config
                    {
                        Assets = AssetDatabaseStrategy.Streaming,
                        Targets = new List<string>
                        {
                            BuildTarget.StandaloneWindows.ToString(),
                            BuildTarget.StandaloneOSXIntel64.ToString()
                        }
                    }
                },
                Develop = new Enviroment
                {
                    FSim = new Config
                    {
                        Assets = AssetDatabaseStrategy.Streaming,
                        Targets = CurrentPlatformTargetList
                    },
                    Client = new Config
                    {
                        Assets = AssetDatabaseStrategy.Streaming,
                        Targets = CurrentPlatformTargetList
                    },
                }
            };
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(UnityPlayerBuilders.PlayerConfigurationFilePath, json);
            return config;
        }
    }
}