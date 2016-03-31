package improbable.natures

import improbable.behaviours.{BulletDamageBehaviour}
import improbable.corelib.natures.rigidbody.RigidbodyComposedTransformNature
import improbable.corelib.natures.{NatureApplication, NatureDescription}
import improbable.damage.BulletState
import improbable.entity.physical.FreezeConstraints
import improbable.math.Coordinates
import improbable.natures
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.util.EntityPrefabs._

object BulletNature extends NatureDescription {

  override def dependencies: Set[NatureDescription] = Set(RigidbodyComposedTransformNature, ColoredNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = Set[EntityBehaviourDescriptor](
    descriptorOf[BulletDamageBehaviour])

  def apply(initialPosition: Coordinates, color:java.awt.Color = java.awt.Color.WHITE, radius:Int = 3): NatureApplication = {
    application(
      states = Seq(
        BulletState(radius)
      ),
      natures = Seq(
        RigidbodyComposedTransformNature(entityPrefab = BULLET, initialPosition = initialPosition, mass = 1, rotationConstraints = FreezeConstraints(x = true, y = true, z = true)),
        ColoredNature(color)
      )
    )
  }
}