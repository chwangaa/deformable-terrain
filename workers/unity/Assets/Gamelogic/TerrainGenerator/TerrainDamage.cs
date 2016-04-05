using UnityEngine;
using TerrainGenerator;
using Improbable.Entity.Physical;
using Improbable.Unity.Visualizer;
using Improbable.Unity;
using Improbable.Damage;
using IoC;
public class TerrainDamage : MonoBehaviour
{

    [Require] protected BulletStateWriter BulletWriter;

    [Require] protected PositionReader Position;

    [Require]
    protected BulletExplosionReader explosionReader;

    void OnEnable()
    {
        transform.position = Position.Value.ToUnityVector();
        explosionReader.BulletExploded += handleExplosionEvent;
    }

    void OnCollisionEnter(UnityEngine.Collision other)
    {
        if (other != null)
        {
            var center = gameObject.transform.position;
            var center_coord = CoordinateSystem.ToCoordinates(center);


            Terrain terrain = (Terrain)other.gameObject.GetComponent("Terrain");
            if (terrain != null)
            {
                BulletWriter.Update.TriggerDamageRequested(center_coord).FinishAndSend();
            }
            else
            {
                var entityId = other.transform.gameObject.EntityId();
                BulletWriter.Update.TriggerEntityDamageRequested(entityId).FinishAndSend();
            }
        }
    }

    void handleExplosionEvent(BulletExplosionEvent obj)
    {


        Debug.Log("explosion event received");
        Vector3 center = obj.Target.ToUnityVector();

        Instantiate(Resources.Load("EntityPrefabs/Explosion"), center, Quaternion.identity);
    }
}