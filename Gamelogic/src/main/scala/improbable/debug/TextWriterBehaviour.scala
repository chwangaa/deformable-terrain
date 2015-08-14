package improbable.debug

import improbable.papi.entity.EntityBehaviour
import improbable.papi.entity.behaviour.EntityBehaviourInterface

trait TextInterface extends EntityBehaviourInterface {
  def emitText(text: String): Unit
}

class TextWriterBehaviour(text: TextWriter) extends EntityBehaviour with TextInterface {
  override def emitText(newTextValue: String): Unit = {
    text.update.triggerEmitText(newTextValue).finishAndSend()
  }
}
