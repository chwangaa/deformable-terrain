package improbable.natures

import improbable.corelib.entity.nature.{NatureApplication, NatureDescription}
import improbable.debug.{Color, ColorConversions, NewColorWriterBehaviour}

object Colored extends NatureDescription {
  override val dependencies = Set[NatureDescription]()

  override def activeBehaviours = {
    Set(NewColorWriterBehaviour())
  }

  def apply(color: java.awt.Color = java.awt.Color.white): NatureApplication = {
    import ColorConversions._
    application(
      states = Seq(Color(color))
    )
  }
}

