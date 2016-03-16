using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Improbable.Core;
using Improbable.Unity;
using Improbable.Core.Network;
using Improbable.Core.Serialization;
using Improbable.Fapi.Receptionist;
using log4net;
using Newtonsoft.Json;
using UnityEngine;

namespace Improbable.Unity.Receptionist
{
    internal class ReceptionistClient
    {
        private const float ReconnectionPeriodInSeconds = 0.5f;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ReceptionistClient));
        private readonly string loginToken;
        private readonly string queuingCompleteToken;
        private readonly string engineId;
        private readonly int? heartbeatInterval;
        private readonly int? multiplexLevel;
        private readonly LinkProtocol linkProtocol;
        private readonly string metaData;
        private readonly string platform;
        private readonly IDeployment deployment;

        public ReceptionistClient(IDeployment deployment, string platform, Dictionary<string, string> metaData,
                                  LinkProtocol linkProtocol, int? heartbeatInterval, int? multiplexLevel,
                                  string engineId, string loginToken, string queuingCompleteToken)
        {
            AddDefaultDisplayNameToMetaData(metaData);
            this.deployment = deployment;
            this.platform = platform;
            this.metaData = JsonConvert.SerializeObject(metaData);
            this.linkProtocol = linkProtocol;
            this.multiplexLevel = multiplexLevel;
            this.heartbeatInterval = heartbeatInterval;
            this.engineId = engineId;
            this.loginToken = loginToken;
            this.queuingCompleteToken = queuingCompleteToken;
        }

        public IEnumerator RequestClientManagerConnectionDetails(int maximumNumberOfRetries, Action<ConnectionDetails> result)
        {
            Logger.Debug(string.Format("Connecting to receptionist url: {0}", deployment.ReceptionistUrl));

            var numberOfRetries = 0;

            var headers = ReceptionistRequestHeaders();
            var body = GetLoginBody();

            var postData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body));
            var www = new WWW(deployment.ReceptionistUrl, postData, headers);
            yield return www;

            ConnectionDetails connectionDetails;
            while (!TryDeserializeResponse(www, out connectionDetails) && numberOfRetries <= maximumNumberOfRetries)
            {
                Logger.Error("Reconnecting to the receptionist " + www.error + " Body:" + www.text);
                numberOfRetries++;
                yield return new WaitForSeconds(ReconnectionPeriodInSeconds);
                www = new WWW(deployment.ReceptionistUrl, postData, headers);
                yield return www;
            }
            result(connectionDetails);
        }

        public Dictionary<string, object> GetLoginBody()
        {
            var loginBody = new Dictionary<string, object>
            {
                { "metaData", metaData },
                { "engine", platform },
                { "linkProtocol", linkProtocol.ToString() },
                { "engineId", engineId },
                { "queuingToken", queuingCompleteToken }
            };

            if (heartbeatInterval != null)
            {
                loginBody.Add("heartbeatInterval", heartbeatInterval);
            }
            if (multiplexLevel != null)
            {
                loginBody.Add("multiplexLevel", multiplexLevel);
            }

            return loginBody;
        }

        private Dictionary<string, string> ReceptionistRequestHeaders()
        {
            var headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" }
            };

            if (!string.IsNullOrEmpty(loginToken))
            {
                headers["Authorization"] = string.Format("Bearer {0}", loginToken);
            }
            return headers;
        }

        private static bool TryDeserializeResponse(WWW www, out ConnectionDetails connectionDetails)
        {
            if (string.IsNullOrEmpty(www.error))
            {
                var receptionistResponse = JsonNetSerializer.GetInstance.DeserializeObject<ReceptionistResponse>(www.text);
                if (receptionistResponse.connectionDetails != null)
                {
                    connectionDetails = receptionistResponse.connectionDetails;
                    return true;
                }
                Logger.Error("Receptionist returned with error: " + receptionistResponse.message);
            }
            connectionDetails = null;
            return false;
        }

        private void AddDefaultDisplayNameToMetaData(Dictionary<string, string> existingMetaData)
        {
            if (!existingMetaData.ContainsKey("displayName") && !string.IsNullOrEmpty(loginToken))
            {
                existingMetaData.Add("displayName", User.GetDisplayNameFromLoginToken(loginToken));
            }
        }
    }
}
