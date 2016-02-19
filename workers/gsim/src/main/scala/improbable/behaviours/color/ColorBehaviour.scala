package improbable.behaviours.color

import java.awt

import improbable.behaviours.color.ColorConversions._
import improbable.colorState.ColorStateWriter
import improbable.papi.entity.EntityBehaviour

class ColorBehaviour(colorState: ColorStateWriter) extends EntityBehaviour with ColorInterface {

  override def setColor(newColorValue: awt.Color): Unit = {
    colorState.update.value(newColorValue).finishAndSend()
  }

}
