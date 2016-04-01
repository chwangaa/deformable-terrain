package improbable.natures

import improbable.behaviours.{ReportToTerrainGeneratorBehaviour, BulletDamageBehaviour}
import improbable.corelib.natures.rigidbody.RigidbodyComposedTransformNature
import improbable.corelib.natures.{NatureApplication, NatureDescription}
import improbable.damage.BulletState
import improbable.entity.physical.FreezeConstraints
import improbable.math.Coordinates
import improbable.natures
import improbable.natures.BotNature._
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.physical.Generatorreport
import improbable.util.EntityPrefabs._

object BulletNature extends NatureDescription {

  override def dependencies: Set[NatureDescription] = Set(RigidbodyComposedTransformNature, ColoredNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = Set[EntityBehaviourDescriptor](
    descriptorOf[BulletDamageBehaviour],
    descriptorOf[ReportToTerrainGeneratorBehaviour]
  )

  def apply(initialPosition: Coordinates, color:java.awt.Color = java.awt.Color.WHITE, radius:Int = 10, target: Coordinates): NatureApplication = {
    application(
      states = Seq(
        BulletState(radius, target),
        Generatorreport(true, 500, 100)
      ),
      natures = Seq(
        RigidbodyComposedTransformNature(entityPrefab = BULLET, initialPosition = initialPosition, mass = 1, rotationConstraints = FreezeConstraints(x = true, y = true, z = true)),
        ColoredNature(color)
      )
    )
  }
}