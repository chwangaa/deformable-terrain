using System;
using Improbable.Unity;
using Improbable.Unity.EditorTools.Build;
using log4net;
using UnityEditor;

namespace Improbable.CoreLibrary.WorkerSpecific.Editor
{
    public class WorkerSpecificBuild
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(WorkerSpecificBuild));

        private Action<string> currentEditScene;
        private Func<EnginePlatform, BuildTarget, Config, IPackager> currentPackager;

        public static void DevelopmentBuild()
        {
            new WorkerSpecificBuild().BuildWithWorkerSpecificHooks(UnityPlayerBuilders.BuildDevelopmentPlayers);
        }

        public static void DeploymentBuild()
        {
            new WorkerSpecificBuild().BuildWithWorkerSpecificHooks(UnityPlayerBuilders.BuildDeploymentPlayers);
        }

        private void BuildWithWorkerSpecificHooks(Action build)
        {
            StoreCurrentHooks();
            try
            {
                InstallWorkerSpecificBuildHooks();
                WorkerSpecificPrefabExporter.ExportPrefabs();
                build();
            }
            catch (Exception e)
            {
                Logger.Error(e);
                EditorUtility.DisplayDialog("Worker Specific Build Error", "An error occurred. Please check your logs for more information.", "ok");
                throw;
            }
            finally
            {
                RevertWorkerSpecificBuildHooks();
            }
        }

        private void InstallWorkerSpecificBuildHooks()
        {
            UnityPlayerBuilder.GetPackager = WorkerSpecificPackager.GetPackager;
            UnityPlayerBuilder.ProcessScene = new WorkerSpecificSceneProcessor().ProcessScene;
        }

        private void RevertWorkerSpecificBuildHooks()
        {
            UnityPlayerBuilder.GetPackager = currentPackager;
            UnityPlayerBuilder.ProcessScene = currentEditScene;
        }

        private void StoreCurrentHooks()
        {
            currentPackager = UnityPlayerBuilder.GetPackager;
            currentEditScene = UnityPlayerBuilder.ProcessScene;
        }
    }
}