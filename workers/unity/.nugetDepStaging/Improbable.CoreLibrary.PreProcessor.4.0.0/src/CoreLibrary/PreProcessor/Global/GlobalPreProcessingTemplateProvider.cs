using Improbable.Assets;
using Improbable.Unity.Assets;
using Improbable.Unity.Entity;
using UnityEngine;

namespace Improbable.Corelibrary.PreProcessor.Global
{
    /// <summary>
    ///     An IEntityTemplateProvider which enables support for running Global Pre-Processors at asset load-time.
    /// </summary>
    public class GlobalPreProcessingTemplateProvider : DefaultTemplateProvider
    {
        protected override IEntityTemplateProvider InitializeTemplateProvider(IAssetLoader<GameObject> gameObjectLoader)
        {
            var assetLoader = new PreprocessingGameObjectLoader(new GlobalPreProcessorApplyingGameObjectLoader(gameObjectLoader));
            return new AssetDatabaseTemplateProvider(new CachingAssetDatabase(assetLoader));
        }
    }
}