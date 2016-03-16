using System;
using Assets.Improbable.CoreLibrary.WorkerSpecific;
using Improbable.Assets;
using Improbable.Unity;
using Improbable.Unity.Entity;
using UnityEngine;

namespace Assets.Improbable.Core.TemplateProviders
{
    /// <summary>
    ///     IEntityTemplateProvider which loads assets converting EntityAssetIds into Worker-Specific names
    /// </summary>
    internal class WorkerSpecificAssetDatabaseTemplateProvider : IEntityTemplateProvider
    {
        private readonly IAssetDatabase<GameObject> AssetDatabase;

        public WorkerSpecificAssetDatabaseTemplateProvider(IAssetDatabase<GameObject> assetDatabase)
        {
            AssetDatabase = assetDatabase;
        }

        public void PrepareTemplate(EntityAssetId assetId, Action<EntityAssetId> onSuccess, Action<Exception> onError)
        {
            AssetDatabase.LoadAsset(AssetIdToPrefabName(assetId), _ => onSuccess(assetId), onError);
        }

        public GameObject GetEntityTemplate(EntityAssetId assetId)
        {
            GameObject templateObject;
            if (!AssetDatabase.TryGet(AssetIdToPrefabName(assetId), out templateObject))
            {
                throw new MissingComponentException(string.Format("Prefab: {0} for context {1} ({2}) cannot be found.", assetId.PrefabName, assetId.Context, AssetIdToPrefabName(assetId)));
            }
            return templateObject;
        }

        private static string AssetIdToPrefabName(EntityAssetId assetId)
        {
            return WorkerSpecificPrefabName.AssetIdToPrefabName(assetId, EngineTypeUtils.CurrentEnginePlatform);
        }
    }
}