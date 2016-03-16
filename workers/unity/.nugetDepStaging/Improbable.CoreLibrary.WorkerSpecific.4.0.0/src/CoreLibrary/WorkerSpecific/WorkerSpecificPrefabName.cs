using System;
using Improbable.Unity;
using Improbable.Unity.Entity;

namespace Assets.Improbable.CoreLibrary.WorkerSpecific
{
    public class WorkerSpecificPrefabName
    {
        private const string WithContextFormat = "{0}{1}{2}@{3}";
        private const string DefaultContextFormat = "{0}{1}{2}";
        public const string WorkerNameSeparator = "_";

        public static string AssetIdToPrefabName(EntityAssetId entity, EnginePlatform platform)
        {
            var context = entity.Context;
            var format = context == EntityAssetId.DEFAULT_CONTEXT ? DefaultContextFormat : WithContextFormat;
            return String.Format(format, entity.PrefabName, WorkerNameSeparator, platform.ToString().ToLower(), context);
        }
    }
}