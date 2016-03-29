package improbable.behaviours

import com.typesafe.scalalogging.Logger
import improbable.damage.{BulletState}
import improbable.math.Coordinates
import improbable.natures.TerrainDamageNature
import improbable.papi.entity.{EntityBehaviour, Entity}
import improbable.papi.world.World
import improbable.corelib.util.EntityOwnerDelegation.entityOwnerDelegation
import improbable.unity.fabric.PhysicsEngineConstraint
import improbable.util.TerrainGeneratorSetting


class DamageTerrainBehaviour(entity : Entity, logger : Logger, world: World) extends EntityBehaviour {

  override def onReady(): Unit = {
    entity.delegateState[BulletState](PhysicsEngineConstraint)


    entity.watch[BulletState].onDamageRequested {
      damage => {
        logger.info("mission completed, bullet is destroyed")
        val position = damage.position
        val x = position.x
        val z = position.z
        val terrain_length = TerrainGeneratorSetting.TERRAIN_LENGTH
        val terrain_x:Int = math.floor(x / terrain_length).toInt * terrain_length
        val terrain_z:Int = math.floor(z / terrain_length).toInt * terrain_length
        val terrain_coordinate = Coordinates(terrain_x, 0, terrain_z);
        world.entities.spawnEntity(TerrainDamageNature(Coordinates(x, 0, z), terrain_coordinate))
        world.entities.destroyEntity(entity.entityId)
      }
    }

  }
}