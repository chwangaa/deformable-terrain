using Improbable.Assets;
using Improbable.Core;
using Newtonsoft.Json;

namespace Improbable.Unity.Core
{
    internal class CloudDeployment : IDeployment
    {
        public CloudDeployment(string name, string appName, string assemblyName)
        {
            Name = name;
            AppName = appName;
            AssemblyName = assemblyName;
        }

        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; private set; }

        [JsonProperty("app", Required = Required.Always)]
        public string AppName { get; private set; }

        [JsonProperty("asset_repo", Required = Required.Always)]
        public string AssemblyName { get; private set; }
        
        public string ReceptionistUrl
        {
            get { return string.Format(@"{0}/locator/v2/deployments/{1}/{2}/login", InfraLocatorUrl, AppName, Name); }
        }

        public string QueueingUrl
        {
            get { return string.Format(@"{0}/locator/v2/queue/{1}/{2}", InfraLocatorUrl, AppName, Name); }
        }

        private static string InfraLocatorUrl
        {
            get { return EngineConfiguration.Instance.InfraLocatorUrl; }
        }

        public AssetDatabaseStrategy AssetDatabaseStrategy { get { return AssetDatabaseStrategy.Streaming; } }
    }
}