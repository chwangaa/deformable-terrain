package improbable.natures

import improbable.behaviours.bot.MoveRandomlyBehaviour
import improbable.corelib.natures.{NatureApplication, RigidbodyEntity, NatureDescription}
import improbable.math.Coordinates
import improbable.util.EntityPrefabs.BOT

object BotNature extends NatureDescription {

  override val dependencies = Set[NatureDescription](RigidbodyEntity, ColoredNature)

  override def activeBehaviours = {
    Set(descriptorOf[MoveRandomlyBehaviour])
  }

  def apply(initialPosition: Coordinates, onFire: Boolean = false): NatureApplication = {
    application(
      natures = Seq(
        RigidbodyEntity(prefab = BOT, position = initialPosition, drag = 0.2f),
        ColoredNature(color = java.awt.Color.red))
    )
  }

}
