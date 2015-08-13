package improbable.debug

import java.awt

import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.papi.entity.behaviour.EntityBehaviourInterface

trait ColorInterface extends EntityBehaviourInterface {
  def setColor(color: java.awt.Color)
}

class ColorWriterBehaviour(colorWriter: ColorWriter) extends EntityBehaviour with ColorInterface{
  import ColorConversions._

  override def setColor(color: awt.Color): Unit = {
    colorWriter.update.value(color).finishAndSend()
  }
}
