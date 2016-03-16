using System;

namespace Improbable.Unity.Entity
{
    public struct EntityAssetId : IEquatable<EntityAssetId>
    {
        public const string DEFAULT_CONTEXT = "Default";

        private readonly string prefabName, context;

        public EntityAssetId(string prefabName, string context)
        {
            this.prefabName = prefabName;
            this.context = context;
        }

        #region Getters

        public string PrefabName
        {
            get { return prefabName; }
        }

        public string Context
        {
            get { return context; }
        }

        #endregion

        #region Equals, Equals<T>, ==, !=, HashCode, ToString Implementation

        public bool Equals(EntityAssetId other)
        {
            return string.Equals(prefabName, other.prefabName) && string.Equals(context, other.context);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is EntityAssetId && Equals((EntityAssetId) obj);
        }

        public override string ToString()
        {
            return String.Format("Asset({0} context [{1}])", prefabName, context);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((prefabName != null ? prefabName.GetHashCode() : 0) * 397) ^ (context != null ? context.GetHashCode() : 0);
            }
        }

        public static bool operator ==(EntityAssetId id1, EntityAssetId id2)
        {
            return id1.Equals(id2);
        }

        public static bool operator !=(EntityAssetId id1, EntityAssetId id2)
        {
            return !(id1 == id2);
        }

        #endregion
    }
}