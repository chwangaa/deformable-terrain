using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using log4net;
using Newtonsoft.Json;
using UnityEngine;

namespace Improbable.Unity.Core
{
    /// <summary>
    ///     Used to exchange steam tokens for improbable tokens to communicating with the infra apis.
    /// </summary>
    public class SteamTokenExchange : MonoBehaviour
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SteamTokenExchange));

        public void ExchangeSteamTokenForLoginToken(string steamToken,
                                                    string locatorUrl,
                                                    string appName,
                                                    string deploymentTag,
                                                    Action<string> processLoginToken,
                                                    Action<Exception> onError)
        {
            StartCoroutine(ExchangeToken(steamToken, locatorUrl, appName, deploymentTag, processLoginToken, onError));
        }

        private IEnumerator ExchangeToken(string steamToken,
                                          string locatorUrl,
                                          string appName,
                                          string deploymentTag, Action<string> processLoginToken, Action<Exception> onError)
        {
            var requestData = new Dictionary<string, string>
            {
                { "deployment_tag", deploymentTag },
                { "app_name", appName },
                { "steam_ticket", steamToken }
            };

            var headers = new Dictionary<string, string>
            {
                {"Content-Type", "application/json"}
            };

            Logger.Info(string.Format("Exchanging steam token {0} for improbable token", steamToken));
            var www = new WWW(
                string.Format(@"{0}/locator/v2/tokens/steam", locatorUrl),
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(requestData)),
                headers);
            yield return www;

            if (www.error != null)
            {
                onError(new Exception("Could not exchange steam token due to " + www.error));
            }
            else
            {
                var response = JsonConvert.DeserializeObject<TokenExchangeResponse>(www.text);
                processLoginToken(response.Token);
            }
        }
    }

    internal class TokenExchangeResponse
    {
        [JsonProperty("token")]
        public string Token { get; private set; }
    }
}