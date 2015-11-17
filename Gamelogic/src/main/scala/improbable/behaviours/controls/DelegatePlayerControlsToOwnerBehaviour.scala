package improbable.behaviours.controls

import improbable.controls.PlayerControlsState
import improbable.corelib.util.EntityOwnerDelegation.entityOwnerDelegation
import improbable.papi.entity.{Entity, EntityBehaviour}

class DelegatePlayerControlsToOwnerBehaviour(entity: Entity) extends EntityBehaviour {

  entity.delegateStateToOwner[PlayerControlsState]

}
