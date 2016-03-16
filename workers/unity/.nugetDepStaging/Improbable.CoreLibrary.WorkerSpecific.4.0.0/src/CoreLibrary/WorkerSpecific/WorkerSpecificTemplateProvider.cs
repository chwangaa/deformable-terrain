using Improbable.Assets;
using Improbable.Corelibrary.PreProcessor.Global;
using Improbable.Unity.Assets;
using Improbable.Unity.Entity;
using UnityEngine;

namespace Assets.Improbable.Core.TemplateProviders
{
    /// <summary>
    ///     The WorkerSpecificTemplateProvider provides a way for users to switch between loading Worker specific assets,
    ///     which are compiled at export time, and runtime pre-processed assets.
    /// </summary>
    public class WorkerSpecificTemplateProvider : GlobalPreProcessingTemplateProvider
    {
        public bool UseWorkerSpecificNames;

        protected override IEntityTemplateProvider InitializeTemplateProvider(IAssetLoader<GameObject> gameObjectLoader)
        {
            if (!UseLocalPrefabs && UseWorkerSpecificNames)
            {
                return new WorkerSpecificAssetDatabaseTemplateProvider(new CachingAssetDatabase(gameObjectLoader));
            }
            return base.InitializeTemplateProvider(gameObjectLoader);
        }
    }
}