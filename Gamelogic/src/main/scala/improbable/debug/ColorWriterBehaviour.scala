package improbable.debug

import java.awt

import improbable.papi.entity.behaviour.EntityBehaviourInterface

trait ColorInterface extends EntityBehaviourInterface {
  def setColor(color: java.awt.Color)
}

trait ColorWriterBehaviour extends ColorWriterBehaviourBase {
  import ColorConversions._

  override def setColor(color: awt.Color): Unit = {
    state.update.value(color).finishAndSend()
  }
}