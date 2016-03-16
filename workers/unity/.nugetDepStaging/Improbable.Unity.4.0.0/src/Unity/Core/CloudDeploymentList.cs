using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Improbable.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Improbable.Unity.Core
{
    internal class CloudDeploymentList
    {
        [JsonProperty("_embedded")]
        private readonly DeploymentListEmbeddedReources embedded = new DeploymentListEmbeddedReources();

        public IList<IDeployment> Deployments
        {
            get
            {
                var cloudDeployments = embedded.deployments;
                return cloudDeployments.Cast<IDeployment>().ToList();
            }
        }
    }

    internal class DeploymentListEmbeddedReources
    {
        public DeploymentListEmbeddedReources()
        {
            deployments = new List<CloudDeployment>();
        }

        [JsonProperty("deployment")]
        [JsonConverter(typeof(InfraHalResourceListDeserializer<CloudDeployment>))]
        public IList<CloudDeployment> deployments { get; private set; }
    }

    /// <summary>
    /// This class deals with the special way that Infra serializes embedded resource list for HAL objects.
    /// </summary>
    /// 
    /// The resources returned when calling Infra's APIs are application/hal+json. They have a special behaviour for
    /// serializing embedded resources:
    ///   * empty resource list gets serilized to empty array []
    ///   * list of one resource turns into a object {....}
    ///   * list of more than one resources turns into a array of json objects [{...}, {...}, ...]
    public class InfraHalResourceListDeserializer<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            var isAssignableFrom = typeof(IEnumerable<T>).IsAssignableFrom(objectType);
            Debug.Print(isAssignableFrom.ToString());
            return true;
        }

        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                         object existingValue,
                                         JsonSerializer serializer)
        {
            var jToken = JToken.Load(reader);

            if (jToken.Type == JTokenType.Array)
            {
                return serializer.Deserialize<List<T>>(jToken.CreateReader());
            }
            if (jToken.Type == JTokenType.Object)
            {
                var deserialized = serializer.Deserialize<T>(jToken.CreateReader());
                var readJson = new List<T> { deserialized };
                return readJson;
            }

            throw new Exception("Must be object or list");
        }

        public override void WriteJson(JsonWriter writer,
                                       object value,
                                       JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}