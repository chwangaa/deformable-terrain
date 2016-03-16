using System;
using Improbable.Assets;
using UnityEngine;

namespace Improbable.Unity.Entity
{
    /// <summary>
    /// Wraps an IAssetDatabase to work as an IEntityTemplateProvider.
    /// The biggest difference is that IAssetDatabases work with string identifiers for their assets, while IEntityTemplatePRoviders use EntityAssetId instances that include a context.
    /// As such, this classes uses a simple scheme to build string identifiers out of EntityAssetIds - if the context is default, the id is simply the name otherwise it's name@context.
    /// </summary>
    public class AssetDatabaseTemplateProvider : IEntityTemplateProvider
    {
        private readonly IAssetDatabase<GameObject> assetDatabase;

        public AssetDatabaseTemplateProvider(IAssetDatabase<GameObject> assetDatabase)
        {
            this.assetDatabase = assetDatabase;
        }

        public void PrepareTemplate(EntityAssetId assetId, Action<EntityAssetId> onSuccess, Action<Exception> onError)
        {
            assetDatabase.LoadAsset(AssetIdToPrefabName(assetId), _ => onSuccess(assetId), onError);
        }

        public GameObject GetEntityTemplate(EntityAssetId assetId)
        {
            GameObject templateObject;
            if (!assetDatabase.TryGet(AssetIdToPrefabName(assetId), out templateObject))
            {
                throw new MissingComponentException(string.Format("Prefab: {0} for context {1} ({2}) cannot be found.", assetId.PrefabName, assetId.Context, AssetIdToPrefabName(assetId)));
            }
            return templateObject;
        }

        private static string AssetIdToPrefabName(EntityAssetId assetId)
        {
            var context = assetId.Context;
            var prefab = assetId.PrefabName;
            return context == EntityAssetId.DEFAULT_CONTEXT ? prefab : string.Format("{0}@{1}", prefab, context);
        }
    }
}