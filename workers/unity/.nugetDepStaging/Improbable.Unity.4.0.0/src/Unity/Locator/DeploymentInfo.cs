using Newtonsoft.Json;

namespace Improbable.Unity.Locator
{
    public class DeploymentInfo
    {
        [JsonConstructor]
        public DeploymentInfo(string deployment_name, string receptionist_ip, string receptionist_port)
        {
            DeploymentName = deployment_name;
            ReceptionistIp = receptionist_ip;
            ReceptionistPort = receptionist_port;
        }

        public string DeploymentId { get; private set; }
        public string DeploymentName { get; private set; }

        public string ReceptionistIp { get; private set; }
        public string ReceptionistPort { get; private set; }
    }
}