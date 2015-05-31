package improbable.physical

import improbable.controls.PlayerControlsData

trait PlayerBotBehaviour extends PlayerBotBehaviourBase {

  entity.watch[PlayerControlsData].bind.movementDirection {
    movementDirection =>
      rigidbodyInterface.setForce(movementDirection * state.forceMagnitude)
  }
}