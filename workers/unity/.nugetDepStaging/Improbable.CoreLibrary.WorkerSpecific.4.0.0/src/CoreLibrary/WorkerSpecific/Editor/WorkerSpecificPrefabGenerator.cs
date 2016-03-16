using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.Unity;
using Improbable.Unity.Assets;
using UnityEditor;
using UnityEngine;

namespace Improbable.CoreLibrary.WorkerSpecific.Editor
{
    /// <summary>
    /// Class responsible for generating a set of Worker Specific prefabs.
    /// </summary>
    internal class WorkerSpecificPrefabGenerator
    {
        private static readonly EnginePlatform[] EnginePlatforms = (EnginePlatform[]) Enum.GetValues(typeof(EnginePlatform));

        /// <summary>
        /// Generates a set of Worker-Specific prefabs
        /// These will be loaded, compiled and saved with Worker-Specific names. 
        /// Compilation of prefab entails that all its Pre-Processors will have been run and are ready to load on a given platform.
        /// </summary>
        /// <param name="guids">Guids of prefabs to generate</param>
        /// <param name="processPrefab">Additional processing to be done on loaded prefab before it is compiled</param>
        /// <returns></returns>
        public static List<string> Generate(IEnumerable<string> guids, Action<EnginePlatform, GameObject> processPrefab)
        {
            var prefabPaths = CreateEnginePrefabs(guids, processPrefab);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            return prefabPaths;
        }

        private static IEnumerable<string> AssetPaths(IEnumerable<string> guids)
        {
            return guids.Select<string, string>(AssetDatabase.GUIDToAssetPath);
        }

        private static List<string> CreateEnginePrefabs(IEnumerable<string> guids, Action<EnginePlatform, GameObject> processPrefab)
        {
            var assets = new List<string>();
            foreach (var sourceAssetPath in AssetPaths(guids))
            {
                foreach (var enginePlatform in EnginePlatforms)
                {
                    var enginePrefabPath = CompilePrefab(processPrefab, enginePlatform, sourceAssetPath);
                    assets.Add(enginePrefabPath);
                }
            }
            return assets;
        }

        private static string CompilePrefab(Action<EnginePlatform, GameObject> processPrefab, EnginePlatform enginePlatform, string sourceAssetPath)
        {
            var enginePrefabPath = CreateEnginePrefabFile(enginePlatform, sourceAssetPath);
            var gameObject = LoadPrefab(enginePrefabPath);
            processPrefab(enginePlatform, gameObject);
            CompilePrefab(enginePlatform, gameObject);
            return enginePrefabPath;
        }


        private static string CreateEnginePrefabFile(EnginePlatform engine, string sourcePrefabPath)
        {
            var engineSpecificPrefabPath = WorkerSpecificPrefabPath.PrefabPath(engine, sourcePrefabPath);
            File.Copy(sourcePrefabPath, engineSpecificPrefabPath, true);
            return engineSpecificPrefabPath;
        }

        private static GameObject LoadPrefab(string enginePrefabPath)
        {
            if (!File.Exists(enginePrefabPath))
            {
                throw new FileNotFoundException(string.Format("Could not find prefab at {0} for compilation", enginePrefabPath));
            }
            AssetDatabase.ImportAsset(enginePrefabPath);

            var enginePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(enginePrefabPath);
            if (enginePrefab == null)
            {
                throw new Exception(string.Format("Could not load {0}", Path.GetFileNameWithoutExtension(enginePrefabPath)));
            }
            return enginePrefab;
        }

        private static void CompilePrefab(EnginePlatform engine, GameObject prefab)
        {
            var compiler = new PrefabCompiler(engine);
            compiler.Compile(prefab);
        }
    }
}