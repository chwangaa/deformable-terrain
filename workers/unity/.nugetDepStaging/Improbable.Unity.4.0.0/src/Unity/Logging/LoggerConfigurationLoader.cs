using System;
using System.IO;
using UnityEngine;
using log4net;
using log4net.Config;
using Debug = UnityEngine.Debug;

namespace Improbable.Unity.Logging
{
    public static class LoggerConfigurationLoader
    {
        private const string DefaultConfig = "log4net-default.xml";

        public static void LoadConfigFile(string configXmlFileName)
        {
            ConfigureWithXml(configXmlFileName);
        }

        public static void Shutdown()
        {
            var hierarchy = (log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository();
            hierarchy.Shutdown();
        }

        private static void ConfigureWithXml(string configXmlFileName)
        {
            if (!configXmlFileName.EndsWith(".xml"))
            {
                Debug.LogError("You must specify a xml configuration file.");
            }
            if (TryUsingCustomLoggingFile(configXmlFileName))
            {
                return;
            }
            Debug.LogFormat("Falling back to default log4net configuration file: {0}", DefaultConfig);
            TryUsingCustomLoggingFile(DefaultConfig);
        }

        private static bool TryUsingCustomLoggingFile(string name)
        {
            try
            {
                var nameWithoutSuffix = new string(name.ToCharArray(0, name.Length - 4));
                var textAsset = (TextAsset) Resources.Load(nameWithoutSuffix, typeof(TextAsset));

                if (textAsset == null)
                {
                    Debug.LogErrorFormat("Failed to configure logging from file {0}", name);
                    return false;
                }

                Debug.Log("Loaded " + nameWithoutSuffix + ".xml");
                var stream = new MemoryStream(textAsset.bytes);

                XmlConfigurator.Configure(stream);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("Failed to configure logging from file {0}: {1}", name, e.Message);
                return false;
            }
        }
    }
}
