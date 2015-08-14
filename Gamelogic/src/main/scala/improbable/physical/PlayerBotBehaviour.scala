package improbable.physical

import improbable.controls.PlayerControlsData
import improbable.entity.physical.RigidbodyInterface
import improbable.papi.entity.{Entity, EntityBehaviour}

class PlayerBotBehaviour(entity: Entity,
                         playerBotData: PlayerBotDataWriter,
                         rigidBodyInterface: RigidbodyInterface) extends EntityBehaviour {

  entity.watch[PlayerControlsData].bind.movementDirection {
    movementDirection =>
      rigidBodyInterface.setForce(movementDirection * playerBotData.forceMagnitude)
  }
}
