package improbable.behaviours.player.controls

import improbable.papi.entity.{EntityBehaviour, Entity}
import improbable.physical.RaycastResponse
import improbable.unity.fabric.PhysicsEngineConstraint

/**
  * Created by chihang on 15/03/2016.
  */
class RaycastResponderBehaviour(entity : Entity) extends EntityBehaviour  {
  override def onReady(): Unit = {
    entity.delegateState[RaycastResponse](PhysicsEngineConstraint)
  }
}
