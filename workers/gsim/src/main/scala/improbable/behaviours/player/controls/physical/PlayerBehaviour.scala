package improbable.behaviours.player.controls.physical

import com.typesafe.scalalogging.Logger
import improbable.behaviours.player.controls.RaycastRequestorInterface
import improbable.entity.physical.RigidbodyInterface
import improbable.math.{Vector3d, Coordinates}
import improbable.natures.{BotNature, BulletNature, TreeNature}
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



    entity.watch[PlayerControlsState].onReduceheightRequested{
      payload =>
        rigidbodyInterface.setForce(Vector3d(0, -50f, 0).normalised)
    }

    // on plant request is used to spawn some bots now
    entity.watch[PlayerControlsState].onPlantRequested{
      payload =>
        spawnHundredsBots()
    }


  }

  def spawnHundredsBots(): Unit = {
    Range.inclusive(1, 100).foreach {
      i =>
          world.entities.spawnEntity(BotNature(getRandomCoordinateNearMe(entity.position)))
    }
  }

  def getRandomCoordinateNearMe(position: Coordinates): Coordinates = {
    val x = position.x + Random.nextDouble() * 1000 - 500
    val y = position.y
    val z = position.z + Random.nextDouble() * 1000 - 500
    return Coordinates(x, y, z)
  }


}
