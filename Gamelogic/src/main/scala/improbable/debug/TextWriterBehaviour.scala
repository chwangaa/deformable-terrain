package improbable.debug

import improbable.papi.entity.behaviour.EntityBehaviourInterface

trait TextInterface extends EntityBehaviourInterface {
  def setText(text: String): Unit

  def clearText(): Unit
}


trait TextWriterBehaviour extends TextWriterBehaviourBase {
  override def setText(text: String): Unit = {
    state.update.content(text).finishAndSend()
  }

  override def clearText(): Unit = {
    state.update.content("").finishAndSend()
  }

  import scala.concurrent.duration._

  var x = 0
  world.timing.every(5.seconds) {
    x += 1
    state.update.triggerEmitText(s"Lads: $x").finishAndSend()
  }
}
