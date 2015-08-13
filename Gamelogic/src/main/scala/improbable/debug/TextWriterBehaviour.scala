package improbable.debug

import improbable.papi.entity.EntityBehaviour
import improbable.papi.entity.behaviour.EntityBehaviourInterface

trait TextInterface extends EntityBehaviourInterface {
  def emitText(text: String): Unit
}

class TextWriterBehaviour(textWriter: TextWriter) extends EntityBehaviour with TextInterface {
  override def emitText(text: String): Unit = {
    textWriter.update.triggerEmitText(text).finishAndSend()
  }
}
