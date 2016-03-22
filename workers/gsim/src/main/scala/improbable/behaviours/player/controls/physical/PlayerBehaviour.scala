package improbable.behaviours.player.controls.physical

import com.typesafe.scalalogging.Logger
import improbable.behaviours.bot.MovementEvent
import improbable.behaviours.player.controls.RaycastRequestorInterface
import improbable.entity.physical.RigidbodyInterface
import improbable.math.Coordinates
import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.papi.world.World
import improbable.papi.world.entities.EntityFindByTag
import improbable.player.controls.PlayerControlsState
import improbable.player.physical.PlayerStateWriter

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
        rigidbodyInterface.setForce(movementDirection * playerState.forceMagnitude)
      }
    }



    entity.watch[PlayerControlsState].onExtinguishRequested {
      payload => {
        raycastInterface.performRaycast()
      }
    }
  }


}
