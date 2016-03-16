using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Improbable.Unity.Core;
using Improbable.Unity.Entity;
using log4net;

namespace Improbable.Unity.Assets
{
    public class ConcurrentAssetPrecacher
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ConcurrentAssetPrecacher));
        private readonly IList<EntityAssetId> assetsToPrecache;
        private readonly IEntityTemplateProvider entityTemplateProvider;
        private readonly int maxConcurrentConnections;
        private readonly Action onComplete;
        private readonly Action<int> onProgress;
        private int nextAssetToPrecacheIndex;
        private int concurrentDownloads;
        private int completedPrecachedCount;

        private ConcurrentAssetPrecacher(IEntityTemplateProvider entityTemplateProvider, IList<EntityAssetId> assetsToPrecache, int maxConcurrentConnections, Action onComplete, Action<int> onProgress)
        {
            if (maxConcurrentConnections < 1)
            {
                throw new ArgumentException("Maximum concurrent connections must be greater than 0.", "maxConcurrentConnections");
            }
            if (entityTemplateProvider == null)
            {
                throw new ArgumentNullException("entityTemplateProvider");
            }
            this.assetsToPrecache = assetsToPrecache.Shuffled();
            this.entityTemplateProvider = entityTemplateProvider;
            this.maxConcurrentConnections = maxConcurrentConnections;
            this.onComplete = onComplete;
            this.onProgress = onProgress;
        }

        public static IEnumerator Precache(IList<EntityAssetId> assetsToPrecache,
                                           IEntityTemplateProvider entityTemplateProvider,
                                           int maxConcurrentConnections,
                                           Action onComplete,
                                           Action<int> onProgress = null)
        {
            return new ConcurrentAssetPrecacher(entityTemplateProvider,
                                                assetsToPrecache,
                                                maxConcurrentConnections,
                                                onComplete,
                                                onProgress)
                .Precache();
        }

        [Obsolete("Use the IList<EntityAssetId> version of ConcurrentAssetPrecacher.Precache")]
        public static IEnumerator Precache(IList<string> assetsToPrecache,
                                           IEntityTemplateProvider entityTemplateProvider,
                                           int maxConcurrentConnections,
                                           Action onComplete,
                                           Action<int> onProgress = null)
        {
            IList<EntityAssetId> assetIds = new EntityAssetId[assetsToPrecache.Count];
            for (var i = 0; i < assetsToPrecache.Count; i++)
            {
                var parts = assetsToPrecache[i].Split('@');

                assetIds[i] = new EntityAssetId(parts[0], parts.Count() > 1 ? parts.Last() : EntityAssetId.DEFAULT_CONTEXT);
            }
            return Precache(assetIds, entityTemplateProvider, maxConcurrentConnections, onComplete, onProgress);
        }

        private IEnumerator Precache()
        {
            ReportProgress();
            while (HasMoreToPrecache)
            {
                while (HasUnstartedAssetsToPrecache && HasSpareConnections)
                {
                    StartPrecachingAsset();
                }
                yield return null;
            }
            onComplete();
        }

        private bool HasSpareConnections
        {
            get { return concurrentDownloads < maxConcurrentConnections; }
        }

        private bool HasUnstartedAssetsToPrecache
        {
            get { return nextAssetToPrecacheIndex < TotalPrecacheCount; }
        }

        private bool HasMoreToPrecache
        {
            get { return completedPrecachedCount < TotalPrecacheCount; }
        }

        private int TotalPrecacheCount
        {
            get { return assetsToPrecache.Count; }
        }

        private void StartPrecachingAsset()
        {
            var assetToPrecache = assetsToPrecache[nextAssetToPrecacheIndex];
            OnAssetPrecacheStarted(assetToPrecache);
            entityTemplateProvider.PrepareTemplate(assetToPrecache,
                                    OnAssetPrecached,
                                    ex => OnAssetPrecacheFailed(assetToPrecache, ex));
        }

        private void OnAssetPrecacheStarted(EntityAssetId entityId)
        {
            ++nextAssetToPrecacheIndex;
            ++concurrentDownloads;
            Logger.DebugFormat("Starting to precache asset {0}@{1}. Concurrent downloads: {2}.", entityId.PrefabName, entityId.Context, concurrentDownloads);
        }

        private void OnAssetPrecached(EntityAssetId asset)
        {
            OnPrecachingAssetStopped();
            Logger.DebugFormat("Finished precaching asset {0}@{1}. Progress: {2} out of {3}", asset.PrefabName, asset.Context, completedPrecachedCount, TotalPrecacheCount);
        }

        private void OnAssetPrecacheFailed(EntityAssetId asset, Exception ex)
        {
            OnPrecachingAssetStopped();
            Logger.Error(String.Format("Failed to precache asset '{0}@{1}.", asset.PrefabName, asset.Context), ex);
        }

        private void OnPrecachingAssetStopped()
        {
            ++completedPrecachedCount;
            --concurrentDownloads;
            ReportProgress();
        }

        private void ReportProgress()
        {
            if (onProgress != null)
            {
                onProgress(completedPrecachedCount);
            }
        }
    }
}