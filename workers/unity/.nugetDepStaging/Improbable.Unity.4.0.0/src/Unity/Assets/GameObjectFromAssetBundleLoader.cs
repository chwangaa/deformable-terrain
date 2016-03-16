using System;
using Improbable.Assets;
using log4net;
using UnityEngine;

namespace Improbable.Unity.Assets
{
    internal class GameObjectFromAssetBundleLoader : IAssetLoader<GameObject>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(GameObjectFromAssetBundleLoader));
        private readonly IAssetLoader<AssetBundle> assetBundleLoader;

        public GameObjectFromAssetBundleLoader(IAssetLoader<AssetBundle> assetBundleLoader)
        {
            this.assetBundleLoader = assetBundleLoader;
        }

        public void LoadAsset(string prefabName, Action<GameObject> onGameObjectLoaded, Action<Exception> onError)
        {
            assetBundleLoader.LoadAsset(prefabName,
                                        loadedAssetBundle => OnAssetBundleLoaded(loadedAssetBundle, prefabName, onGameObjectLoaded, onError),
                                        ex => OnAssetLoadFailure(prefabName, onError, ex));
        }

        private static void OnAssetBundleLoaded(AssetBundle loadedAssetBundle, string prefabName, Action<GameObject> onGameObjectLoaded, Action<Exception> onError)
        {
            var gameObject = loadedAssetBundle.LoadAsset(prefabName) as GameObject;
            loadedAssetBundle.Unload(unloadAllLoadedObjects: false);
            try
            {
                if (gameObject == null)
                {
                    onError(new Exception(string.Format("Could not load the game object from asset '{0}'.", prefabName)));
                }
                else
                {
                    Logger.DebugFormat("Asset '{0}' loaded.", prefabName);
                    onGameObjectLoaded(gameObject);
                }
            }
            catch (Exception ex)
            {
                onError(ex);
            }
        }

        private static void OnAssetLoadFailure(string prefabName, Action<Exception> onError, Exception ex)
        {
            Logger.ErrorFormat("Loading asset '{0}' failed. {1}", prefabName, ex);
            onError(ex);
        }
    }
}