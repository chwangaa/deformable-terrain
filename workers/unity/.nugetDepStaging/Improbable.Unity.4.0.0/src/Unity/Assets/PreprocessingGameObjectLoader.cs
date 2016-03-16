using System;
using Improbable.Assets;
using UnityEngine;

namespace Improbable.Unity.Assets
{
    internal class PreprocessingGameObjectLoader : IAssetLoader<GameObject>
    {
        private readonly EnginePlatform enginePlatform = EngineTypeUtils.CurrentEnginePlatform;
        private readonly IAssetLoader<GameObject> gameObjectLoader;
        private readonly PrefabCompiler prefabCompiler;
        

        public PreprocessingGameObjectLoader(IAssetLoader<GameObject> gameObjectLoader)
        {
            this.gameObjectLoader = gameObjectLoader;
            this.prefabCompiler = new PrefabCompiler(enginePlatform);
        }

        public void LoadAsset(string prefabName, Action<GameObject> onAssetLoaded, Action<Exception> onError)
        {
            gameObjectLoader.LoadAsset(prefabName, gameObject =>
            {
                prefabCompiler.Compile(gameObject);
                onAssetLoaded(gameObject);
            }, onError);
        }
    }
}