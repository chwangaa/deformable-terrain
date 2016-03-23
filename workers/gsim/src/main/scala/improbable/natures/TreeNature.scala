package improbable.natures

import improbable.behaviours.bot.MoveRandomlyBehaviour
import improbable.behaviours.color.SetColorFromFireBehaviour
import improbable.corelib.natures.base.BaseComposedTransformNature
import improbable.corelib.natures.rigidbody.RigidbodyComposedTransformNature
import improbable.corelib.natures.{NatureApplication, NatureDescription}
import improbable.entity.physical.FreezeConstraints
import improbable.math.Coordinates
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.physical.{Fire}
import improbable.util.EntityPrefabs._

object TreeNature extends NatureDescription {

  override def dependencies: Set[NatureDescription] = Set(RigidbodyComposedTransformNature, ColoredNature, FlammableNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = Set[EntityBehaviourDescriptor](
    descriptorOf[SetColorFromFireBehaviour])

  def apply(initialPosition: Coordinates, onFire:Boolean = false): NatureApplication = {
    application(
      states = Seq(Fire(onFire)),
      natures = Seq(
        RigidbodyComposedTransformNature(entityPrefab = TREE, initialPosition = initialPosition, drag = 0.2f, mass = 100, rotationConstraints = FreezeConstraints(x = true, y = true, z = true)),
        ColoredNature(java.awt.Color.green),
        FlammableNature(onFire)

      )
    )
  }

}
