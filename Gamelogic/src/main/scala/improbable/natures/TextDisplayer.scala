package improbable.natures

import improbable.corelib.entity.nature.{NatureApplication, NatureDescription}
import improbable.debug.{Text, TextWriterBehaviour}

object TextDisplayer extends NatureDescription {
  override val dependencies = Set[NatureDescription]()

  override def activeBehaviours = {
    Set(descriptorOf[TextWriterBehaviour])
  }

  def apply(text: String = ""): NatureApplication = {
    application(
      states = Seq(Text())
    )
  }
}
