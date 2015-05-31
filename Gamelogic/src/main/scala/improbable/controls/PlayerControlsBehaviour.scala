package improbable.controls

import improbable.corelib.util.EntityOwnerUtils
import improbable.unity.papi.SpecificEngineConstraint

trait PlayerControlsBehaviour extends PlayerControlsBehaviourBase {

  entity.delegateState[PlayerControlsData](SpecificEngineConstraint(EntityOwnerUtils.ownerIdOf(entity)))
}