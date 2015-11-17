package improbable.behaviours.physical

import improbable.controls.PlayerControlsState
import improbable.entity.physical.RigidbodyInterface
import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.physical.PlayerStateWriter

class PlayerBotBehaviour(entity: Entity,
                         playerState: PlayerStateWriter,
                         rigidbody: RigidbodyInterface) extends EntityBehaviour {

  entity.watch[PlayerControlsState].bind.movementDirection {
    movementDirection =>
      rigidbody.setForce(movementDirection * playerState.forceMagnitude)
  }

}
