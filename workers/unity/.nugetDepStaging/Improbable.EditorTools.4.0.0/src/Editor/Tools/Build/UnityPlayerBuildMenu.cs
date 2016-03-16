using System.Collections.Generic;
using Improbable.Unity.EditorTools.Util;
using UnityEditor;
using UnityEngine;

namespace Improbable.Unity.EditorTools.Build
{
    public static class UnityPlayerBuilderMenu
    {
        [MenuItem("Improbable/Build/Build Deployment Players %#&3")]
        public static void BuildDeploymentPlayers()
        {
            UnityPlayerBuilders.BuildDeploymentPlayers();
        }

        [MenuItem("Improbable/Build/Build Development Players %#&2")]
        public static void BuildDevelopmentPlayers()
        {
            UnityPlayerBuilders.BuildDevelopmentPlayers();
        }

        [MenuItem("Improbable/Build/Clean All Players %#&1")]
        public static void CleanAllPlayers()
        {
            UnityPathUtil.EnsureDirectoryClean(UnityPlayerBuilder.PlayerBuildDirectory);
            UnityPathUtil.EnsureDirectoryClean(UnityPlayerBuilder.PlayerBuildScratchDirectory);
            Debug.Log("Player Directory Cleaned");
        }

        public static void IncrementallyPatchAllPlayers()
        {
            PatchPlayers(UnityPlayerBuilders.DeploymentPlayerBuilders);
            PatchPlayers(UnityPlayerBuilders.DevelopmentPlayerBuilders);
        }

        public static void PatchPlayers(IEnumerable<UnityPlayerBuilder> playerBuilders)
        {
            foreach (var playerBuilder in playerBuilders)
            {
                playerBuilder.PatchPlayerIfExists();
            }
        }

        public static void CleanPlayers(IEnumerable<UnityPlayerBuilder> playerBuilders)
        {
            foreach (var playerBuilder in playerBuilders)
            {
                playerBuilder.Clean();
            }
        }
    }
}