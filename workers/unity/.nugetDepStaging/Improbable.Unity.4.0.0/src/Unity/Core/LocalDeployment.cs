using System;
using Improbable.Assets;
using Improbable.Core;

namespace Improbable.Unity.Core
{
    internal class LocalDeployment : IDeployment
    {
        private readonly string ip;
        private readonly string assemblyName;

        public LocalDeployment(EngineConfiguration config)
        {
            ReceptionistUrl = string.Format("http://{0}:7777/login", config.Ip);
            if (!string.IsNullOrEmpty(config.AppName) && !string.IsNullOrEmpty(config.AssemblyName))
            {
                AssetDatabaseStrategy = AssetDatabaseStrategy.Streaming;
            }
            else
            {
                AssetDatabaseStrategy = AssetDatabaseStrategy.Local;
            }

            if (!string.IsNullOrEmpty(config.AppName))
            {
                AppName = config.AppName;
            }
            else
            {
                AppName = "local_app";
            }

            assemblyName = config.AssemblyName;
        }

        public string Name
        {
            get { return "local"; }
        }

        public string AppName { get; private set; }

        public string AssemblyName
        {
            get
            {
                if (AssetDatabaseStrategy == AssetDatabaseStrategy.Local)
                {
                    throw new NotImplementedException("This method should not be called on a local fake deployment");
                }
                return assemblyName;
            }
        }

        public string ReceptionistUrl { get; private set; }

        public string QueueingUrl
        {
            get { throw new NotImplementedException("This method should not be called on a local fake deployment"); }
        }

        public AssetDatabaseStrategy AssetDatabaseStrategy { get; private set; }
    }
}