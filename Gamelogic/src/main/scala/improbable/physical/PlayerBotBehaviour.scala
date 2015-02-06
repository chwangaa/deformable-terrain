package improbable.physical

import improbable.controls.observer.PlayerControlsObserverDescriptor
import improbable.controls.{PlayerControlsData, PlayerControlsDataDescriptor}
import improbable.corelib.util.EntityOwnerUtils
import improbable.math.Vector3
import improbable.unity.papi.SpecificEngineConstraint

trait PlayerBotBehaviour extends PlayerBotBehaviourBase {

  entity.addObserverWithState(
    PlayerControlsObserverDescriptor(),
    SpecificEngineConstraint(EntityOwnerUtils.ownerIdOf(entity)),
    PlayerControlsDataDescriptor(Vector3.zero)
  )

  entity.watch[PlayerControlsData].bind.movementDirection {
    movementDirection =>
      rigidbodyInterface.setForce(movementDirection * state.forceMagnitude)
  }
}