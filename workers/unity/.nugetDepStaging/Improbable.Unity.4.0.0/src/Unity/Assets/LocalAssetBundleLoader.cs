﻿using System;
using System.IO;
using Assets.Improbable.Gel.Util;
using Improbable.Assets;
using Improbable.Unity.Util;
using UnityEngine;

namespace Improbable.Unity.Assets
{
    /// <summary>
    ///   A filesystem-based asset bundle loader. 
    ///   This implementation loads unity3d files from assets stored in the file strucuture required by the asset database.   
    /// </summary>
    internal class LocalAssetBundleLoader : IAssetLoader<AssetBundle>
    {
        private readonly string entityPrefabsPath;

        /// <param name="assetBundlesDir">The directory where to find asset bundles.</param>
        public LocalAssetBundleLoader(string assetBundlesDir)
        {
            var entityPrefabSubdir = UnityPrefabPaths.IsDirectoryUnderSpatialProject(assetBundlesDir) ? "unity" : "EntityPrefab";
            entityPrefabsPath = Path.Combine(assetBundlesDir, entityPrefabSubdir);
        }

        public void LoadAsset(string prefabName, Action<AssetBundle> onAssetLoaded, Action<Exception> onError)
        {
            var assetBundlePath = Path.Combine(entityPrefabsPath, prefabName.ToLower());
            
            try
            {
                if (!File.Exists(assetBundlePath))
                {
                    throw new IOException(string.Format("Failed to load prefab's '{0}' asset bundle from file '{1}'.\n", prefabName, assetBundlePath)
                        + "Asset is either missing or the local asset bundle path is incorrect.");
                }
                var assetBundle = AssetBundle.CreateFromFile(assetBundlePath);
                if (assetBundle == null)
                {
                    throw new Exception(string.Format("Failed to load prefab's '{0}' asset bundle from file '{1}'.\n", prefabName, assetBundlePath)
                        + "Asset is most likely corrupted.");
                }
                onAssetLoaded(assetBundle);
            }
            catch (Exception e)
            {
                onError(e);
            }

        }
    }
}