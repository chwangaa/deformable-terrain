using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Improbable.Assets;
using Improbable.Unity.Core;
using log4net;
using UnityEngine;

namespace Improbable.Unity.Assets
{
    public class AssetBundleDownloader : MonoBehaviour, IAssetLoader<AssetBundle>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AssetBundleDownloader));
        private static readonly XmlSerializer MetaDataSerializer = new XmlSerializer(typeof(Container));
        private MachineCache<byte[], AssetBundle> assetCache;
        private MachineCache<CacheEntry> metaDataCache;
        private string cachePath;

        /// <summary>
        /// The method to invoke to resolve a prefab name to a URL. Throws an <see cref="KeyNotFoundException"/> if the asset is unknown.
        /// </summary>
        public Func<string, string> GetAssetUrl { get; set; }

        public AssetBundleDownloader()
        {
            GetAssetUrl = s =>
            {
                throw new InvalidOperationException("GetAssetUrl has not been assigned.");
            };
        }

        public void Awake()
        {
            cachePath = Path.Combine(Application.persistentDataPath, "cache" + EngineConfiguration.Instance.EngineType);
            Directory.CreateDirectory(cachePath);
            assetCache = new MachineCache<byte[], AssetBundle>(Path.Combine(cachePath, "assets"), new AssetBundlePersistenceStrategy());
            metaDataCache = new MachineCache<CacheEntry>(Path.Combine(cachePath, "asset-metadata"), new AssetMetadataPersistenceStrategy());
        }

        public void LoadAsset(string prefabName, Action<AssetBundle> onAssetLoaded, Action<Exception> onError)
        {
            try
        {
            var assetUri = GetAssetUrl(prefabName);

                Logger.InfoFormat("Requesting asset '{0}' from: {1}", prefabName, assetUri);
                StartCoroutine(DownloadAsync(assetUri, assetUri, www => HandleDownloadAssetResponse(assetUri, onAssetLoaded, onError, www)));
            }
            catch (Exception ex)
            {
                onError(ex);
            }
        }

        private void HandleDownloadAssetResponse(string url, Action<AssetBundle> response, Action<Exception> onFailure, WWW www)
        {
            if (www.error != null)
            {
                Logger.WarnFormat("Requesting asset '{0}' failed. Url: {1}. Error: {2}", GetAssetName(url), url, www.error);
                onFailure(new ApplicationException(www.error));
            }
            else if (www.responseHeaders.ContainsKey("STATUS") && www.responseHeaders["STATUS"].Contains("304"))
            {
                Logger.DebugFormat("Unmodified asset '{0}', attempting to load from cache...", GetAssetName(url));

                AssetBundle assetBundle;
                if (assetCache.TryGet(GetAssetName(url), out assetBundle))
                {
                    response(assetBundle);
                }
                else
                {
                    Logger.ErrorFormat("Failed to load asset '{0}' from cache. Corrupted cache. Please delete the cache folder: '{1}'.", GetAssetName(url), cachePath);
                    onFailure(new ApplicationException("Cache likely corrupted."));
                }
            }
            else if (www.assetBundle != null)
            {
                Logger.DebugFormat("Caching asset '{0}'...", GetAssetName(url));
                UpdateCache(url, www);
                response(www.assetBundle);
            }
            else
            {
                string responseHeadersString = "";
                foreach (var responseHeader in www.responseHeaders)
                {
                    responseHeadersString += " [" + responseHeader.Key + ": " + responseHeader.Value + "]";
                }
                Logger.WarnFormat("Unhandled response for {0}. Downloaded {1} bytes, got the following response headers: {2}", url, www.bytesDownloaded, responseHeadersString);
                onFailure(new ApplicationException("Unhandled response."));
            }
        }

        private static string GetAssetName(string assetUrl)
        {
            return new Uri(assetUrl).PathAndQuery;
        }

        private void UpdateCache(string url, WWW www)
        {
            var added = assetCache.TryAddOrUpdate(GetAssetName(url), www.bytes);

            if (added)
            {
                AddEntry(url, www);
            }
            else
            {
                Logger.WarnFormat("Failed to cache: {0}", url);
            }
        }

        private void AddEntry(string url, WWW www)
        {
            if (!HasValidators(www))
            {
                Logger.WarnFormat("ETag and/or Last-Modified missing. Cannot cache: {0}", url);
                Logger.DebugFormat("Keys: {0}", string.Join("\n", www.responseHeaders.Keys.ToArray()));
                Debug.Log("ETag and/or Last-Modified missing. Cannot cache: " + url);
            }
            else
            {
                var entry = CreateCacheEntry(url, www);
                if (!metaDataCache.TryAddOrUpdate(GetAssetName(url), entry))
                {
                    Logger.WarnFormat("Failed to add cache metadata for: {0}", url);
                }
            }
        }

        private static bool HasValidators(WWW www)
        {
            return www.responseHeaders.ContainsKey("LAST-MODIFIED") && www.responseHeaders.ContainsKey("ETAG");
        }

        private static CacheEntry CreateCacheEntry(string url, WWW www)
        {
            string responseHeader = www.responseHeaders["LAST-MODIFIED"];
            string entityTag = www.responseHeaders["ETAG"];
            var cacheEntry = new CacheEntry { EntityTag = entityTag, Modified = responseHeader, Url = url, LastFetched = DateTime.UtcNow };
            return cacheEntry;
        }

        private IEnumerator DownloadAsync(string originalUrl, string redirectedUrl, Action<WWW> response)
        {
            var headers = GetCacheHeaders(originalUrl);
            var www = headers != null ? new WWW(redirectedUrl, null, headers) : new WWW(redirectedUrl);
            yield return www;

            var redirectUrl = GetRedirectUrl(www);
            if (redirectUrl != null)
            {
                Logger.DebugFormat("Requested asset at {0}, but following redirect to {1}", redirectedUrl, redirectUrl);
                StartCoroutine(DownloadAsync(originalUrl, redirectUrl, response));
            }
            else
            {
                response(www);
            }
        }

        private Dictionary<string, string> GetCacheHeaders(string url)
        {
            CacheEntry entry;
            if (metaDataCache.TryGet(GetAssetName(url), out entry))
            {
                Logger.DebugFormat("Cached asset bundle found. Request with headers {0}", GetAssetName(url));
                return new Dictionary<string, string>
                {
                    { "If-Modified-Since", entry.Modified },
                    { "If-None-Match", entry.EntityTag },
                };
            }
            return null;
        }

        private static string GetRedirectUrl(WWW www)
        {
            string status = "";
            if (www.responseHeaders.TryGetValue("STATUS", out status))
            {
                if (status.Contains("302"))
                {
                    string location = "";
                    if (www.responseHeaders.TryGetValue("LOCATION", out location))
                    {
                        return location;
                    }
                    else
                    {
                        Logger.ErrorFormat("Requested {0} and got a 302, but no location header", www.url);
                    }
                }
            }
            return null;
        }

        private class AssetBundlePersistenceStrategy : MachineCache<byte[], AssetBundle>.IPersistenceStrategy
        {
            public void WriteToCacheFile(string outputCacheFile, byte[] resource)
            {
                File.WriteAllBytes(outputCacheFile, resource);
            }

            public AssetBundle ReadFromCacheFile(string inputCacheFile)
            {
                return AssetBundle.CreateFromFile(inputCacheFile);
            }
        }

        private class AssetMetadataPersistenceStrategy : MachineCache<CacheEntry, CacheEntry>.IPersistenceStrategy
        {
            public CacheEntry ReadFromCacheFile(string filename)
            {
                using (var file = File.OpenRead(filename))
                {
                    var container = (Container) MetaDataSerializer.Deserialize(file);
                    return container.CacheEntry;
                }
            }

            public void WriteToCacheFile(string filename, CacheEntry entry)
            {
                using (var file = File.CreateText(filename))
                {
                    var container = new Container
                    {
                        CacheEntry = entry
                    };
                    MetaDataSerializer.Serialize(file, container);
                }
            }
        }

        public class CacheEntry
        {
            [XmlAttribute] public string EntityTag;

            [XmlAttribute] public DateTime LastFetched;
            [XmlAttribute] public string Modified;
            [XmlAttribute] public string Url;
        }

        [XmlRoot("Cache")]
        public class Container
        {
            [XmlElement] public CacheEntry CacheEntry;
        }
    }
}
