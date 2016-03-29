package improbable.natures

import improbable.behaviours.{DamageTerrainBehaviour}
import improbable.corelib.natures.rigidbody.RigidbodyComposedTransformNature
import improbable.corelib.natures.{NatureApplication, NatureDescription}
import improbable.damage.BulletState
import improbable.entity.physical.FreezeConstraints
import improbable.math.Coordinates
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.util.EntityPrefabs._

object BulletNature extends NatureDescription {

  override def dependencies: Set[NatureDescription] = Set(RigidbodyComposedTransformNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = Set[EntityBehaviourDescriptor](
    descriptorOf[DamageTerrainBehaviour])

  def apply(initialPosition: Coordinates, onFire: Boolean = false): NatureApplication = {
    application(
      states = Seq(
        BulletState()
      ),
      natures = Seq(
        RigidbodyComposedTransformNature(entityPrefab = BULLET, initialPosition = initialPosition, drag = 0.2f, mass = 100, rotationConstraints = FreezeConstraints(x = true, y = true, z = true))
      )
    )
  }
}