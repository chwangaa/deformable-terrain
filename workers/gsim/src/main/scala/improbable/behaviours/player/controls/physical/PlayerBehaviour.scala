package improbable.behaviours.player.controls.physical

import com.typesafe.scalalogging.Logger
import improbable.behaviours.bot.MovementEvent
import improbable.behaviours.player.controls.RaycastRequestorInterface
import improbable.entity.physical.RigidbodyInterface
import improbable.math.Coordinates
import improbable.natures.TreeNature
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
        logger.info("player movement received")
        rigidbodyInterface.setForce(movementDirection.normalised * playerState.forceMagnitude)
      }
    }



    entity.watch[PlayerControlsState].onExtinguishRequested {
      payload => {
        raycastInterface.performRaycast()
      }
    }

    entity.watch[PlayerControlsState].onPlantRequested {
      payload => {
        // add some displacement to the places where the tree should be generated
        val center = entity.position
        val x = center.x + (Random.nextFloat() * 10 - 10)
        val y = center.y + (Random.nextFloat() * 10 - 10)
        val z = center.z + (Random.nextFloat() * 10 - 10)
        world.entities.spawnEntity(TreeNature(Coordinates(x, y, z)))
        logger.info("tree is being planted")
      }
    }

  }


}
