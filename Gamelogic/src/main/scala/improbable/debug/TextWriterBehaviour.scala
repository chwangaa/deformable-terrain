package improbable.debug

import improbable.papi.entity.behaviour.EntityBehaviourInterface

trait TextInterface extends EntityBehaviourInterface {
  def emitText(text: String): Unit
}


trait TextWriterBehaviour extends TextWriterBehaviourBase {
  override def emitText(text: String): Unit = {
    state.update.triggerEmitText(text).finishAndSend()
  }
}
