package improbable.controls

import improbable.corelib.util.EntityOwnerDelegation.entityOwnerDelegation
import improbable.papi.entity.{Entity, EntityBehaviour}

class PlayerControlsBehaviour(entity: Entity) extends EntityBehaviour {

  entity.delegateStateToOwner[PlayerControlsData]

}
