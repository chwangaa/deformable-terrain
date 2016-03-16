using System;
using System.Collections;
using System.Collections.Generic;
using Improbable.Core;
using Newtonsoft.Json;
using UnityEngine;

namespace Improbable.Unity.Core
{
    internal static class 
        CloudAssemblyArtifactResolver
    {
        public static IEnumerator ResolveAssetUrls(IDeployment deployment, EngineConfiguration engineConfiguration, Action<Dictionary<string, string>> onAssetsResolved, Action<Exception> onFailed)
        {
            var headers = new Dictionary<string, string>
            {
                { "Accept", "application/json" }
            };

            var www = new WWW(string.Format("{0}/assembly_content/v3/{1}/{2}/artifacts", engineConfiguration.InfraServiceUrl, deployment.AppName, deployment.AssemblyName), null, headers);
            yield return www;

            if (www.error != null)
            {
                onFailed(new Exception("Failed to retrieve assembly list: " + www.text));
            }

            var assetUrls = new Dictionary<string, string>();

            var response = JsonConvert.DeserializeObject<AssemblyResponse>(www.text);
            for (var i = 0; i < response.Artifacts.Count; i++)
            {
                var artifact = response.Artifacts[i];
                assetUrls[artifact.ArtifactId.Name] = artifact.Url;
            }

            onAssetsResolved(assetUrls);
        }

        private class AssemblyResponse
        {
            [JsonProperty("artifacts")]
            public IList<AssemblyArtifact> Artifacts { get; set; }
        }

        private class ArtifactId
        {
            [JsonProperty("name")]
            public string Name { get; set; }
        }

        private class AssemblyArtifact
        {
            [JsonProperty("artifact_id")]
            public ArtifactId ArtifactId { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }
        }       
    }
}