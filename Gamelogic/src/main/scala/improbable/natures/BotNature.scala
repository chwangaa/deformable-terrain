package improbable.natures

import improbable.corelib.natures.rigidbody.RigidbodyComposedTransformNature
import improbable.corelib.natures.{NatureApplication, NatureDescription}
import improbable.math.Coordinates
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.util.EntityPrefabs.BOT

object BotNature extends NatureDescription {

  override def dependencies = Set[NatureDescription](RigidbodyComposedTransformNature, ColoredNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = Set.empty

  def apply(initialPosition: Coordinates): NatureApplication = {
    application(
      natures = Seq(
        RigidbodyComposedTransformNature(entityPrefab = BOT, initialPosition = initialPosition, drag = 0.2f),
        ColoredNature(color = java.awt.Color.red))
    )
  }

}
