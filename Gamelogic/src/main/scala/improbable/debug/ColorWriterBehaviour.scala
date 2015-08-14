package improbable.debug

import java.awt

import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.papi.entity.behaviour.EntityBehaviourInterface

trait ColorInterface extends EntityBehaviourInterface {
  def setColor(color: java.awt.Color)
}

class ColorWriterBehaviour(color: ColorWriter) extends EntityBehaviour with ColorInterface{
  import ColorConversions._

  override def setColor(newColorValue: awt.Color): Unit = {
    color.update.value(newColorValue).finishAndSend()
  }
}
