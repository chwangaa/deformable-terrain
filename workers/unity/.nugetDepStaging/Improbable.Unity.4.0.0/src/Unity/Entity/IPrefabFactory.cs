namespace Improbable.Unity.Entity
{
    // TODO: Is this a good name for a class with Destroy?
    /// <summary>
    /// Creating and destorying entities from 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPrefabFactory<T> 
    {
        /// <summary>
        /// instantiates a GameObject from the given prefab
        /// </summary>
        /// <param name="prefabGameObject">The prefab to instantiate</param>
        /// <param name="assetId">The details of the asset to create the component from.</param>
        /// <returns>a new instance of the given prefabGameObject</returns>
        T MakeComponent(T prefabGameObject, EntityAssetId assetId);

        /// <summary>
        /// Destroys an existing GameObject
        /// </summary>
        /// <param name="gameObject">the object that is in the game</param>
        /// <param name="assetId">The details of the asset that the object was instantiated from</param>
        void DespawnComponent(T gameObject, EntityAssetId assetId); // TODO: prefabName is the pool leaking out.
    }
}