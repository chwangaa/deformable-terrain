package improbable.behaviours.player.controls.physical

import com.typesafe.scalalogging.Logger
import improbable.behaviours.player.controls.RaycastRequestorInterface
import improbable.entity.physical.RigidbodyInterface
import improbable.math.{Vector3d, Coordinates}
import improbable.natures.{BulletNature, TerrainDamageNature, TreeNature}
import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.papi.world.World
import improbable.papi.world.entities.EntityFindByTag
import improbable.player.controls.PlayerControlsState
import improbable.player.physical.PlayerStateWriter

import scala.util.Random

class PlayerBehaviour(entity: Entity,
                      playerState: PlayerStateWriter,
                      rigidbodyInterface: RigidbodyInterface,
                      logger: Logger,
                      raycastInterface: RaycastRequestorInterface,
                      world: World) extends EntityBehaviour {

  override def onReady(): Unit = {
    entity.watch[PlayerControlsState].bind.movementDirection {
      movementDirection => {
        val moveX = movementDirection.x
        val moveZ = movementDirection.z
        rigidbodyInterface.setForce(Vector3d(moveX, 0, moveZ) * playerState.forceMagnitude)
      }
    }



    entity.watch[PlayerControlsState].onExtinguishRequested {
      payload => {
        val position = entity.position
        val new_x = entity.position.x + Random.nextFloat() * 20
         val new_y = entity.position.y + Random.nextFloat() * 5  // the y-axis is strictly below the player position
        val new_z = entity.position.z + Random.nextFloat() * 20
        val coordinate = new Coordinates(new_x, new_y, new_z)
        logger.info("fire event received! Creating bullet")
        world.entities.spawnEntity(BulletNature(coordinate))
      }
    }

    entity.watch[PlayerControlsState].onPlantRequested {
      payload => {
        // we want to avoid the tree generated from hitting the player, so add some random disturbance
        val position = entity.position
        val new_x = entity.position.x + Random.nextFloat() * 10 - 5
        val new_y = entity.position.y - Random.nextFloat() * 5  // the y-axis is strictly below the player position
        val new_z = entity.position.z + Random.nextFloat() * 10 -5
        val coordinate = new Coordinates(new_x, new_y, new_z)
        world.entities.spawnEntity(TreeNature(coordinate))
        logger.info("a new tree generated")
      }
    }

    entity.watch[PlayerControlsState].onReduceheightRequested{
      payload =>
        rigidbodyInterface.setForce(Vector3d(0, -50f, 0).normalised)
    }


    entity.watch[PlayerControlsState].onAddheightRequested{
      payload =>
        rigidbodyInterface.setForce(Vector3d(0, 10f, 0).normalised)
    }


  }


}
