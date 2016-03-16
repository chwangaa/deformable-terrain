using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Improbable.Unity.EditorTools.Build
{
    internal static class UnityPlayerBuilders
    {
        public static readonly string PlayerConfigurationFilePath = Path.Combine(Application.dataPath, "player-build-config.json");

        public static IEnumerable<UnityPlayerBuilder> DeploymentPlayerBuilders
        {
            get { return ConfiguredBuilders(LoadConfiguation().Deploy); }
        }

        public static IEnumerable<UnityPlayerBuilder> DevelopmentPlayerBuilders
        {
            get { return ConfiguredBuilders(LoadConfiguation().Develop); }
        }

        public static void BuildDeploymentPlayers()
        {
            BuildPlayers(DeploymentPlayerBuilders);
        }

        public static void BuildDevelopmentPlayers()
        {
            BuildPlayers(DevelopmentPlayerBuilders);
        }

        private static IEnumerable<UnityPlayerBuilder> ConfiguredBuilders(Enviroment env)
        {
            var clientBuilders = ToPlatformBuilders(EnginePlatform.Client, env.Client);
            var fsimBuilders = ToPlatformBuilders(EnginePlatform.FSim, env.FSim);

            return clientBuilders.Concat(fsimBuilders);
        }

        private static IEnumerable<UnityPlayerBuilder> ToPlatformBuilders(EnginePlatform platform, Config config)
        {
            if (config == null)
            {
                return new List<UnityPlayerBuilder>();
            }

            var targets = config.Targets.Select<string, BuildTarget>(ToRuntimePlatform);
            return targets.Select<BuildTarget, UnityPlayerBuilder>(buildTarget => new UnityPlayerBuilder(platform, buildTarget, config));
        }

        private static BuildTarget ToRuntimePlatform(string platform)
        {
            if (platform.Contains("?"))
            {
                platform = platform.Substring(0, platform.IndexOf("?", StringComparison.Ordinal));
            }
            if (platform.ToLower() != "current")
            {
                return (BuildTarget) Enum.Parse(typeof(BuildTarget), platform);
            }
            return CurrentPlatform();
        }

        private static BuildTarget CurrentPlatform()
        {
            return Application.platform == RuntimePlatform.WindowsEditor
                ? BuildTarget.StandaloneWindows
                : BuildTarget.StandaloneOSXIntel64;
        }

        private static PlayerBuildConfiguation LoadConfiguation()
        {
            if (File.Exists(PlayerConfigurationFilePath))
            {
                return JsonConvert.DeserializeObject<PlayerBuildConfiguation>(File.ReadAllText(PlayerConfigurationFilePath));
            }
            return DefaultPlayerBuildConfiguration.Generate();
        }

        private static void BuildPlayers(IEnumerable<UnityPlayerBuilder> playerBuilders)
        {
            var exceptions = 0;
            var threads = new List<Thread>();

            // Make sure we can iterate through the players twice
            playerBuilders = new List<UnityPlayerBuilder>(playerBuilders);
            foreach (var playerBuilder in playerBuilders)
            {
                playerBuilder.Clean();
            }
            foreach (var playerBuilder in playerBuilders)
            {
                playerBuilder.EnsurePlayerUpdated();
                var builder = playerBuilder;
                var thread = new Thread(() =>
                {
                    try
                    {
                        builder.PackagePlayer();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                        Interlocked.Increment(ref exceptions);
                        throw;
                    }
                });
                thread.Start();
                threads.Add(thread);
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
            if (exceptions > 0)
            {
                throw new Exception(string.Format("Building {0} of the players failed. Please look at logs.", exceptions));
            }
            Debug.Log("Finished building players.");
        }
    }
}