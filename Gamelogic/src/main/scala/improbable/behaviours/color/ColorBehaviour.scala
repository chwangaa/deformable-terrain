package improbable.behaviours.color

import java.awt

import improbable.behaviours.color.ColorConversions._
import improbable.color.ColorStateWriter
import improbable.papi.entity.EntityBehaviour
import improbable.papi.entity.behaviour.EntityBehaviourInterface

trait ColorInterface extends EntityBehaviourInterface {

  def setColor(color: java.awt.Color)

}

class ColorBehaviour(colorState: ColorStateWriter) extends EntityBehaviour with ColorInterface {

  override def setColor(newColorValue: awt.Color): Unit = {
    colorState.update.value(newColorValue).finishAndSend()
  }

}
