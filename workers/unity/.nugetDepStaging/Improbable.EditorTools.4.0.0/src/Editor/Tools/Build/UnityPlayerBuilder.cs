using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Improbable.Gel.Util;
using Improbable.Assets;
using Improbable.Unity.EditorTools.Util;
using UnityEditor;
using UnityEngine;

namespace Improbable.Unity.EditorTools.Build
{
    public class UnityPlayerBuilder
    {
        private const string CsharpAssemblyPath = "Library/ScriptAssemblies/Assembly-CSharp.dll";
        private const string ImprobableLibraryPath = "Assets/Improbable";

        /// <summary>
        ///     If you have logic that needs to run during packaging, create a custom IPackager in your unity editor folder and
        ///     configure it with something like the following code:
        ///     <code>
        /// [InitializeOnLoad]
        /// public static class CustomEnginePackager
        /// {
        ///     static CustomEnginePackager()
        ///     {
        ///         UnityPlayerBuilder.GetPackager = (EnginePlatform engineType, BuildTarget buildTarget, Config config) =>
        ///         {
        ///             Debug.Log("Invoking the custom packager exporter...");
        ///             return new MyCustomIPackager();
        ///         };
        ///     }
        /// }
        /// </code>
        /// </summary>
        public static Func<EnginePlatform, BuildTarget, Config, IPackager> GetPackager = GetDefaultPackager;

        /// <summary>
        ///     Hook which can be used to modify your scene before compiling and building your player.
        /// </summary>
        public static Action<string> ProcessScene = (sceneName) => { };

        public readonly BuildTarget BuildTarget;
        public readonly EnginePlatform EngineType;
        private readonly Config config;
        private readonly BuildOptions options;
        private readonly PlatformData platformData;

        public UnityPlayerBuilder(EnginePlatform engineType, BuildTarget buildTarget, Config config)
        {
            EngineType = engineType;
            BuildTarget = buildTarget;
            this.config = config;
            options = GenerateFlag(config.FlagsForPlatform(buildTarget.ToString()));
            platformData = CreatePlatformData(engineType, buildTarget);
        }

        public static string PlayerBuildScratchDirectory
        {
            get
            {
                if (EditorPaths.HasSpatialOsJson)
                {
                    return Path.GetFullPath("build/worker");
                }
                return Path.GetFullPath(Path.Combine(EditorPaths.dataDirectory, "target/AssetDatabase/EngineExecutable"));
            }
        }

        public static string PlayerBuildDirectory
        {
            get
            {
                if (EditorPaths.HasSpatialOsJson)
                {
                    return PathUtil.Combine(Directory.GetCurrentDirectory(), EditorPaths.assetDatabaseDirectory, "worker");
                }
                return PathUtil.Combine(Directory.GetCurrentDirectory(), EditorPaths.assetDatabaseDirectory, "EngineExecutable");
            }
        }

        private static string DataDirectory
        {
            get { return Path.Combine(Directory.GetCurrentDirectory(), EditorPaths.dataDirectory); }
        }

        private string SceneName
        {
            get { return EngineType == EnginePlatform.Client ? "ClientScene" : "PhysicsServerScene"; }
        }

        private string AssemblyPathWithinPackage
        {
            get { return Path.Combine(DataFolderName, platformData.AssemblyPathWithinPackage); }
        }


        private string DataFolderName
        {
            get { return string.Format("Unity{0}@{1}{2}", EngineType, platformData.BuildContext, platformData.DataFolderExtension); }
        }

        private string ExecutableName
        {
            get { return string.Format("Unity{0}@{1}{2}", EngineType, platformData.BuildContext, platformData.ExecutableExtension); }
        }

        private string PackageName
        {
            get { return string.Format("Unity{0}@{1}", EngineType, platformData.BuildContext); }
        }

        private string PackagePath
        {
            get { return Path.Combine(PlayerBuildScratchDirectory, PackageName); }
        }

        private string ZipPath
        {
            get { return Path.Combine(PlayerBuildDirectory, PackageName + ".zip"); }
        }

        private bool PlayerIsBuilt
        {
            get
            {
                if (File.Exists(ZipPath))
                {
                    using (var zipFile = new IonicZipPackage(ZipPath))
                    {
                        return zipFile.Comment == BuildConfigComment;
                    }
                }
                return false;
            }
        }

        private bool PlayerNotBuilt
        {
            get { return !PlayerIsBuilt; }
        }

        private string BuildConfigComment
        {
            get { return String.Format("EngineType={0};BuildTarget={1};EmbedAssets={2};BuildOptions={3}", EngineType, BuildTarget, config.Assets == AssetDatabaseStrategy.Local, options); }
        }

        public static IPackager GetDefaultPackager(EnginePlatform engineType, BuildTarget buildTarget, Config config)
        {
            var embedAssets = config.Assets == AssetDatabaseStrategy.Local;
            return embedAssets ? new AssetEmbeddingPackager(DataDirectory) as IPackager : new SimplePackager();
        }

        public void Clean()
        {
            UnityPathUtil.EnsureDirectoryRemoved(PackagePath);
            UnityPathUtil.EnsureFileRemoved(ZipPath);
        }

        public void EnsurePlayerUpdated()
        {
            if (PlayerNotBuilt)
            {
                PathUtil.EnsureDirectoryExists(PlayerBuildDirectory);
                PathUtil.EnsureDirectoryExists(PlayerBuildScratchDirectory);
                BuildPlayer();
            }
            Debug.Log("Build Complete for " + BuildConfigComment);
        }

        public void PatchPlayerIfExists()
        {
            PatchPlayerIfExists(false);
        }

        public void PackagePlayer()
        {
            IPackager packager = GetPackager(EngineType, BuildTarget, config);

            Debug.LogFormat("Packaging using packager {0}", packager.GetType().Name);
            try
            {
                using (var package = new Package(ZipPath, BuildConfigComment))
                {
                    packager.Prepare(package, PackagePath);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        private static PlatformData CreatePlatformData(EnginePlatform engineType, BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows:
                    return new PlatformData("Managed", "Windows", "_Data", ".exe");
                case BuildTarget.StandaloneOSXIntel64:
                    return new PlatformData("Contents/Data/Managed", "Mac", ".app", "");
                case BuildTarget.StandaloneLinux64:
                    return new PlatformData("Managed", "Linux", "_Data", "");
            }
            throw new ArgumentException("Unsupported platform " + engineType);
        }


        private void PatchPlayerIfExists(bool ignoreTimestamps)
        {
            if (PlayerIsBuilt)
            {
                PatchPlayer(ignoreTimestamps);
            }
        }

        private void PatchPlayer(bool ignoreTimestamps)
        {
            if (File.Exists(CsharpAssemblyPath))
            {
                using (var zipFile = new IonicZipPackage(ZipPath))
                {
                    var assemblyPaths = AllAssembliesWithin(ImprobableLibraryPath)
                        .Concat(new[] { CsharpAssemblyPath });
                    var filteredAssemblyPaths = assemblyPaths.Where(path => AssemblyNeedsPatching(path) || ignoreTimestamps).ToList();

                    foreach (var assemblyPath in filteredAssemblyPaths)
                    {
                        zipFile.UpdateFile(assemblyPath, AssemblyPathWithinPackage);
                    }

                    zipFile.UpdateFile(CsharpAssemblyPath, AssemblyPathWithinPackage);
                    Debug.Log(string.Format("Patched updated {0} Assemblies into {1}", filteredAssemblyPaths.Count(), ZipPath));
                }
            }
            else
            {
                /**
                 * Rob - This is unavoidable due to this problem with Unity: http://answers.unity3d.com/questions/524987/new-scripts-not-found-when-building-in-batch-mode.html
                 */
                Debug.Log(string.Format("Patch to {0} aborted as compilation has not yet completed for the Editor. This is to be expected with attempting to patch with a freshly cleaned repository", ZipPath));
            }
        }

        private static IEnumerable<string> AllAssembliesWithin(string path)
        {
            return Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories);
        }

        private void BuildPlayer()
        {
            if (!EditorApplication.OpenScene(ToScenePath(SceneName)))
            {
                throw new ApplicationException("Can't open " + SceneName);
            }
            ProcessScene(SceneName);
            BuildPlayer(PackagePath);
            Debug.Log(string.Format("Built player into {0}", PackagePath));
        }

        private void BuildPlayer(string buildDir)
        {
            var tempExecutablePath = Path.Combine(buildDir, ExecutableName);

            BuildPipeline.BuildPlayer(new[] { ToScenePath(SceneName) },
                                      tempExecutablePath,
                                      BuildTarget,
                                      options);
        }

        private BuildOptions GenerateFlag(IEnumerable<BuildOptions> flagList)
        {
            return flagList.Aggregate((a, b) => a | b);
        }

        private static string ToScenePath(string sceneName)
        {
            return Path.Combine("Assets", sceneName + ".unity");
        }

        private bool AssemblyNeedsPatching(string assemblyPath)
        {
            return PathUtil.PathModificationTime(assemblyPath) > PathUtil.PathModificationTime(ZipPath) &&
                   File.Exists(ZipPath);
        }
    }

    internal class PlatformData
    {
        public readonly string AssemblyPathWithinPackage;
        public readonly string BuildContext;
        public readonly string DataFolderExtension;
        public readonly string ExecutableExtension;

        public PlatformData(string assemblyPathWithinPackage, string buildContext, string dataFolderExtension, string executableExtension)
        {
            AssemblyPathWithinPackage = assemblyPathWithinPackage;
            BuildContext = buildContext;
            DataFolderExtension = dataFolderExtension;
            ExecutableExtension = executableExtension;
        }
    }
}
