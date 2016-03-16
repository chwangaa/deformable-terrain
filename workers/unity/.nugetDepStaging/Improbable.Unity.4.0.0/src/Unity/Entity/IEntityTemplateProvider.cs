using System;
using UnityEngine;

namespace Improbable.Unity.Entity
{
 
    /// <summary>
    /// An IEntityTemplateProvider can look up a GameObject to use as a template for an EntityAssetId.  This will be the prefab and context.
    /// </summary>
    public interface IEntityTemplateProvider
    {
        /// <summary>
        /// PrepareTemplate is an asynchronous method guaranteed to be called at least once before the GameObject template required for a particular assetId is requested.
        /// Implementors must call onSuccess once the IEntityTemplateProvider is ready to accept GetEntityTemplate calls, and onError if it was unable to get ready.
        /// </summary>
        /// <param name="assetId">The id of the entity asset.  This includes the prefab name and the context.</param>
        /// <param name="onSuccess">the continuation to call if preparation for the entity asset was successful.</param>
        /// <param name="onError">the continuation to call if preparation for the entity asset failed.</param>
        void PrepareTemplate(EntityAssetId assetId, Action<EntityAssetId> onSuccess, Action<Exception> onError);
        
        /// <summary>
        /// GetEntityTemplate must return a template GameObject that will be instantiated to make new instances of entities with the same EntityAssetId.  Subsequent calls with the
        /// same assetId should return the same GameObject.
        /// 
        /// PrepareTemplate will always have been called at least once with this assetId first.
        /// </summary>
        /// <param name="assetId">The id of the entity asset.  This includes the prefab name and the context.</param>
        /// <returns></returns>
        GameObject GetEntityTemplate(EntityAssetId assetId);
    }
}