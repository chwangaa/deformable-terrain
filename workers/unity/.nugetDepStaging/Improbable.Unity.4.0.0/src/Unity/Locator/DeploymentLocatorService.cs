using System;
using System.Collections;
using System.Collections.Generic;
using Improbable.Auth;
using log4net;
using Newtonsoft.Json;
using UnityEngine;

namespace Improbable.Unity.Locator
{
    internal class DeploymentLocatorService : MonoBehaviour
    {
        public string Url { get; set; }

        private const string TOKEN_TYPE = "Bearer";

        private const string ERROR_RETRIEVING_RESPONSE_FORMAT =
            "Error retrieving response: {0}";

        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(DeploymentLocatorService));

        public void GetDeploymentInfo(ImprobableToken token, Action<DeploymentInfo> onComplete)
        {
            StartCoroutine(GetDeploymentInfoCoroutine(token, onComplete));
        }

        public void GetAssetDatabaseUrl(ImprobableToken token, Action<string> onComplete)
        {
            StartCoroutine(GetAssetDatabaseUrlCoroutine(token, onComplete));
        }

        private IEnumerator GetAssetDatabaseUrlCoroutine(ImprobableToken accessToken, Action<string> onComplete)
        {
            DeploymentInfo deploymentInfo = null;
            yield return StartCoroutine(GetDeploymentInfoCoroutine(accessToken, (info) => deploymentInfo = info));
            var receptionistUrl = string.Format("http://{0}:1238", deploymentInfo.ReceptionistIp);
            onComplete(receptionistUrl);
        }

        private IEnumerator GetDeploymentInfoCoroutine(ImprobableToken accessToken, Action<DeploymentInfo> onComplete)
        {
            var headers = new Dictionary<string, string>();
            AddAuthHeader(headers, accessToken.AccessToken);

            var www = new WWW(Url + "/game_locator/deployments/" + accessToken.DeploymentId, null, headers);
            yield return www;
            CheckResponseErrors(www);
            onComplete(Deserialize<DeploymentInfo>(www.text));
        }

        private T Deserialize<T>(string data)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch (JsonException e)
            {
                LOGGER.Error("Could not deserialize response", e);
                throw;
            }
        }

        private void CheckResponseErrors(WWW www)
        {
            if (!string.IsNullOrEmpty(www.error))
            {
                throw new ApplicationException(string.Format(ERROR_RETRIEVING_RESPONSE_FORMAT, www.error));
            }
        }

        private void AddAuthHeader(Dictionary<string, string> headers, string token)
        {
            headers["Authorization"] = string.Format("{0} {1}", TOKEN_TYPE, token);
        }
    }
}