package improbable.behaviours

import com.typesafe.scalalogging.Logger
import improbable.damage.{BulletState}
import improbable.entity.physical.RigidbodyInterface
import improbable.math.{Vector3d, Coordinates}
import improbable.papi.entity.{EntityBehaviour, Entity}
import improbable.papi.world.World
import improbable.papi.world.entities.EntityFindByTag
import improbable.unity.fabric.PhysicsEngineConstraint
import improbable.util.{TerrainCoordinateMapping}
import improbable.util.FiringGameSetting.{BULLET_DAMAGE_VALUE, BULLET_SPEED}
import scala.util.Random
import scala.concurrent.duration._

class BulletDamageBehaviour(entity : Entity, logger : Logger, world: World, rigidBodyInterface: RigidbodyInterface) extends EntityBehaviour {

  override def onReady(): Unit = {
    entity.delegateState[BulletState](PhysicsEngineConstraint)

    /* making the bullet flying to the target */
    val target_position = entity.watch[BulletState].target.get
    val bullet_direction = target_position.-(entity.position).normalised
    rigidBodyInterface.setForce(bullet_direction.normalised * BULLET_SPEED)

    /**
      * This listener listens for terrain damage events
      */
    entity.watch[BulletState].onDamageRequested {
      payload => {
        /**
          *In this implementation, the entity creating the damage is in charge of figuring out which terrain to apply the damage to
          *A message containing the terrain coordinate is sent to the terrain generator
          *The terrain generator then forward the request to the corresponding terrain entity
          *The terrain entity then applies the damage
         */

        val damage_position = payload.position
        val radius:Int = entity.watch[BulletState].radius.get

        val terrain_coordinates = TerrainCoordinateMapping.findCoordinatesOfTerrainsThatNeedToBeGenerated(damage_position, radius)
        logger.info(s"the damage position is $damage_position");


        val terrain_generators = world.entities.find(EntityFindByTag("TerrainGenerator"))
        if(!terrain_generators.isEmpty){
          val terrain_generator_id = terrain_generators.last.entityId // find the terrain generator id
          terrain_coordinates.foreach(
            terrain_coordinate =>{
              world.messaging.sendToEntity(terrain_generator_id, DamageTerrainIdAtPosition(terrain_coordinate, damage_position, radius))   // ask terrain generator to apply the damage
              logger.info(s"the terrain coordinate of $terrain_coordinate needs to respond to damage request")
            }
          )
        }
        world.entities.destroyEntity(entity.entityId) // destroy the bullet

        /**
          * optionally, we can do a search in the local area and apply damages to the nearby entities
          * probably should add a tag to assist the search
          */
        world.entities.find(damage_position, radius).foreach(
          entity =>{
            val entity_id = entity.entityId
            val damage_value = BULLET_DAMAGE_VALUE / (damage_position.distanceTo(entity.position) + 1).toInt
            world.messaging.sendToEntity(entity_id, HealthDamage(damage_value))
          }
        )
      }
    }

    /**
      * this listener listens for other entities damage events. A message is sent iff the entity implements HealthDamage
      */
    entity.watch[BulletState].onEntityDamageRequested{
      payload => {
        val entity_id = payload.entityId
        world.messaging.sendToEntity(entity_id, HealthDamage(BULLET_DAMAGE_VALUE))
        world.entities.destroyEntity(entity.entityId)
      }
    }

    /**
      * let it fly for a while, then kill it anyway, lack of fuels
      */
    world.timing.after(10.seconds){
      entity.destroy()
    }

  }

}