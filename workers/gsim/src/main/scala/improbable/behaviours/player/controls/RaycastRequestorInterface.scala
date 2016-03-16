package improbable.behaviours.player.controls

import improbable.papi.entity.behaviour.EntityBehaviourInterface

trait RaycastRequestorInterface extends EntityBehaviourInterface {
  def performRaycast(): Unit
}