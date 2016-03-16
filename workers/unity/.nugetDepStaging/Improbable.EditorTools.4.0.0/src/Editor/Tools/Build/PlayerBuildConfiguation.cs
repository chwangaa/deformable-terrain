using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Assets;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEditor;

namespace Improbable.Unity.EditorTools.Build
{
    public class Config
    {
        private static readonly IEnumerable<BuildOptions> NoBuildOptions = new List<BuildOptions>{ BuildOptions.None }; 
        public List<string> Targets;

        [JsonConverter(typeof(StringEnumConverter))] public AssetDatabaseStrategy Assets;

        public IEnumerable<BuildOptions> FlagsForPlatform(string buildTarget)
        {
            var target = Targets.Find(p => p.StartsWith(buildTarget));
            return default(string) == target ? NoBuildOptions : ParseFlags(target);
        }

        internal IEnumerable<BuildOptions> ParseFlags(string target)
        {
            var index = target.IndexOf('?');
            if (index == -1)
            {
                return NoBuildOptions;
            }
            var flags = target.Substring(index + 1).Split(',').Select<string, string>(s => s.Trim()).Select<string, BuildOptions>(ToBuildOptions);
            return flags;
        }

        private static BuildOptions ToBuildOptions(string value)
        {
            return (BuildOptions) Enum.Parse(typeof(BuildOptions), value);
        }
    }

    public class Enviroment
    {
        public Config FSim;
        public Config Client;
    }

    public class PlayerBuildConfiguation
    {
        public Enviroment Deploy;
        public Enviroment Develop;
    }
}