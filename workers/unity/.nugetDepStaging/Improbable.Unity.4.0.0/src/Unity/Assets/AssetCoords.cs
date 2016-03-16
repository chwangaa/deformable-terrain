
using Improbable.Unity.Entity;

namespace Improbable.Unity.Assets
{
    class AssetCoords
    {
        public readonly string PrefabName;
        public readonly string Name;
        public readonly string Engine;
        public readonly string Context;

        public AssetCoords(string prefabName)
        {
            this.PrefabName = prefabName;
            Name = !prefabName.Contains("@") ? prefabName : prefabName.Split('@')[0];
            Engine = EngineTypeUtils.ToEngineName(EngineTypeUtils.CurrentEnginePlatform);
            Context = !prefabName.Contains("@") ? EntityAssetId.DEFAULT_CONTEXT : prefabName.Split('@')[1];
        }

        public bool IsDefaultContext()
        {
            return (Context.ToLower() == EntityAssetId.DEFAULT_CONTEXT.ToLower());
        }
    }
}
