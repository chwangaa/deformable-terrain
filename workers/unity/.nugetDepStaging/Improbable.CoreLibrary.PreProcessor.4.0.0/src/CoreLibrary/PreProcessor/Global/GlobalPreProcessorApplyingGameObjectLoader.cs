using System;
using Improbable.Assets;
using UnityEngine;

namespace Improbable.Corelibrary.PreProcessor.Global
{
    /// <summary>
    ///     Asset loader which adds all Pre-Processors defined in GlobalPreprocessors.PreProcessors
    /// </summary>
    internal class GlobalPreProcessorApplyingGameObjectLoader : IAssetLoader<GameObject>
    {
        private readonly IAssetLoader<GameObject> gameObjectLoader;
        private readonly GameObjectComponentAdder gameObjectComponentAdder;

        public GlobalPreProcessorApplyingGameObjectLoader(IAssetLoader<GameObject> gameObjectLoader)
        {
            this.gameObjectLoader = gameObjectLoader;
            gameObjectComponentAdder = new GameObjectComponentAdder(GlobalPreProcessors.Preprocessors);
        }

        public void LoadAsset(string prefabName, Action<GameObject> onAssetLoaded, Action<Exception> onError)
        {
            gameObjectLoader.LoadAsset(prefabName, gameObject =>
            {
                gameObjectComponentAdder.AddComponentsTo(gameObject);
                onAssetLoaded(gameObject);
            }, onError);
        }
    }
}