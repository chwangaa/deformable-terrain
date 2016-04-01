package improbable.behaviours.player.controls.physical

import com.typesafe.scalalogging.Logger
import improbable.behaviours.player.controls.RaycastRequestorInterface
import improbable.entity.physical.RigidbodyInterface
import improbable.math.{Vector3d, Coordinates}
import improbable.natures.{BulletNature, TreeNature}
import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.papi.world.World
import improbable.player.controls.PlayerControlsState
import improbable.player.physical.PlayerStateWriter
import improbable.util.FiringGameSetting.PLAYER_BULLET_RADIUS
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
        rigidbodyInterface.setForce(Vector3d(moveX, 0, moveZ).normalised * playerState.forceMagnitude)
      }
    }



    entity.watch[PlayerControlsState].onFiringRequested {
      payload => {
        val position = payload.position
        val new_x = position.x + Random.nextFloat() * 10
        val new_y = position.y + Random.nextFloat() * 10
        val new_z = position.z + 10
        val coordinate = new Coordinates(new_x, new_y, new_z)
        val target = new Coordinates(new_x, 0, new_z)
        logger.info(s"fire event received! Creating bullet at position $coordinate")
        world.entities.spawnEntity(BulletNature(initialPosition = coordinate, target = target, radius = PLAYER_BULLET_RADIUS))
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
        rigidbodyInterface.setForce(Vector3d(0, 50f, 0).normalised)
    }


  }


}
