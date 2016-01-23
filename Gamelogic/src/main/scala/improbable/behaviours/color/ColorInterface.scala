package improbable.behaviours.color

import improbable.papi.entity.behaviour.EntityBehaviourInterface

trait ColorInterface extends EntityBehaviourInterface {

  def setColor(color: java.awt.Color)

}
