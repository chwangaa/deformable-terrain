package improbable.natures

import improbable.corelib.entity.nature.{NatureApplication, NatureDescription}
import improbable.debug.{NewTextWriterBehaviour, Text}

object TextDisplayer extends NatureDescription {
  override val dependencies = Set[NatureDescription]()

  override def activeBehaviours = {
    Set(NewTextWriterBehaviour())
  }

  def apply(text: String = ""): NatureApplication = {
    application(
      states = Seq(Text())
    )
  }
}
