using System;
using System.Collections.Generic;
using System.IO;
using Improbable.Assets;
using Improbable.Core;
using Improbable.Unity.Assets;
using Improbable.Unity.Core;
using IoC;
using log4net;
using UnityEngine;

namespace Improbable.Unity.Entity
{
    /// <summary>
    ///     The DefaultTemplateProvider switches between three strategies, based on whether it's running in the editor, is
    ///     configured to use local prefabs or has a streaming strategy set.
    /// </summary>
    public class DefaultTemplateProvider : MonoBehaviour, IEntityTemplateProvider
    {
        private static readonly CommandLineArguments CommandLine = new CommandLineArguments();
        private static readonly ILog Log = LogManager.GetLogger(typeof(DefaultTemplateProvider));
        private Dictionary<string, string> prefabsToUrls;
        private List<Action> pendingPrepareTemplates;

        // These can be overridden on the command line.
        public bool UseLocalPrefabs;
        public AssetDatabaseStrategy LoadingStrategy = AssetDatabaseStrategy.Streaming;
        public string LocalAssetDatabasePath = "../../build/assembly/";

        [Inject]
        public IDeployment Deployment { set; private get; }
        
        // The template provider can't be instantiated during construction as Application.isEditor doesn't work.
        private IEntityTemplateProvider templateProvider;

        private IEntityTemplateProvider TemplateProvider
        {
            get
            {
                if (templateProvider == null)
                {
                    var gameObjectLoader = InitializeAssetLoader();
                    templateProvider = InitializeTemplateProvider(gameObjectLoader);
                }
                return templateProvider;
            }
        }

        protected virtual IAssetLoader<GameObject> InitializeAssetLoader()
        {
            UseLocalPrefabs = CommandLine.GetConfigValue(ConfigNames.USE_LOCAL_PREFABS, UseLocalPrefabs);
            LoadingStrategy = CommandLine.GetConfigValue(ConfigNames.ASSET_DATABASE_STRATEGY, Deployment.AssetDatabaseStrategy);
            LocalAssetDatabasePath = CommandLine.GetConfigValue(ConfigNames.LOCAL_ASSET_DATABASE_PATH, LocalAssetDatabasePath);

            IAssetLoader<GameObject> gameObjectLoader;

            if (Application.isEditor && UseLocalPrefabs)
            {
                gameObjectLoader = new PrefabGameObjectLoader();
                Log.Info("Loading local prefabs in the editor.");
            }
            else
            {
                switch (LoadingStrategy)
                {
                    case AssetDatabaseStrategy.Local:
                        var path = Path.GetFullPath(LocalAssetDatabasePath);
                        Log.InfoFormat("Working folder is {0}", Environment.CurrentDirectory);
                        gameObjectLoader = new GameObjectFromAssetBundleLoader(new LocalAssetBundleLoader(path));
                        Log.InfoFormat("Loading local asset database from {0}.", path);
                        break;
                    case AssetDatabaseStrategy.Streaming:
                        pendingPrepareTemplates = new List<Action>();
                        var assetBundleDownloader = gameObject.AddComponent<AssetBundleDownloader>();
                        assetBundleDownloader.GetAssetUrl = GetAssetUrl;

                        var exponentialBackoffRetryAssetLoader = gameObject.GetComponent<ExponentialBackoffRetryAssetLoader>()
                                                                 ?? gameObject.AddComponent<ExponentialBackoffRetryAssetLoader>();
                        exponentialBackoffRetryAssetLoader.AssetLoader = assetBundleDownloader;
                        exponentialBackoffRetryAssetLoader.MaxRetries
                            = CommandLine.GetConfigValue(ConfigNames.MAX_ASSET_LOADING_RETRIES, exponentialBackoffRetryAssetLoader.MaxRetries);
                        exponentialBackoffRetryAssetLoader.StartBackoffTimeout
                            = CommandLine.GetConfigValue(ConfigNames.ASSET_LOADING_RETRY_BACKOFF_MILLISECONDS, exponentialBackoffRetryAssetLoader.StartBackoffTimeout);

                        gameObjectLoader = new GameObjectFromAssetBundleLoader(exponentialBackoffRetryAssetLoader);
                        Log.InfoFormat("Loading assets remotely from {0} {1}.", EngineConfiguration.Instance.AppName, EngineConfiguration.Instance.AssemblyName);

                        StartCoroutine(CloudAssemblyArtifactResolver.ResolveAssetUrls(Deployment, EngineConfiguration.Instance, OnAssetsResolved, OnAssetResolveFailed));
                        break;
                    default:
                        throw new Exception(string.Format("Unknown loading strategy '{0}'", LoadingStrategy));
                }
            }
            return gameObjectLoader;
        }

        protected virtual IEntityTemplateProvider InitializeTemplateProvider(IAssetLoader<GameObject> gameObjectLoader)
        {
            return new AssetDatabaseTemplateProvider(new CachingAssetDatabase(new PreprocessingGameObjectLoader(gameObjectLoader)));
        }

        public virtual void PrepareTemplate(EntityAssetId assetId, Action<EntityAssetId> onSuccess, Action<Exception> onError)
        {
            // TemplateProvider is initialized-on-access, so ensure we're all setup before checking pendingPrepareTemplates
            var provider = TemplateProvider;
            if (pendingPrepareTemplates != null)
            {
                pendingPrepareTemplates.Add(() => provider.PrepareTemplate(assetId, onSuccess, onError));
                return;
            }

            provider.PrepareTemplate(assetId, onSuccess, onError);
        }

        public virtual GameObject GetEntityTemplate(EntityAssetId assetId)
        {
            return TemplateProvider.GetEntityTemplate(assetId);
        }

        private string GetAssetUrl(string prefab)
        {
            var canonicalName = prefab.ToLowerInvariant();
            string url;
            if (!prefabsToUrls.TryGetValue(canonicalName, out url))
            {
                throw new KeyNotFoundException(string.Format("Trying to load a non-existent asset named '{0}'", prefab));
            }
            return url;
        }

        private static void OnAssetResolveFailed(Exception err)
        {
            throw err;
        }

        private void OnAssetsResolved(Dictionary<string, string> mapping)
        {
            prefabsToUrls = mapping;
            InvokePendingPrepareTemplates();
        }

        private void InvokePendingPrepareTemplates()
        {
            // Start all pending PrepareTemplate requests now that we've finished resolving
            for (var i = 0; i < pendingPrepareTemplates.Count; i++)
            {
                pendingPrepareTemplates[i]();
            }
            pendingPrepareTemplates = null;
        }
    }
}
