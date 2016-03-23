package improbable.natures

import improbable.behaviours.ReportToTerrainGeneratorNature
import improbable.behaviours.color.SetColorFromFireBehaviour
import improbable.corelib.natures.rigidbody.RigidbodyComposedTransformNature
import improbable.corelib.natures.{NatureApplication, NatureDescription}
import improbable.entity.physical.FreezeConstraints
import improbable.math.Coordinates
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.physical.{Generatorreport, Fire}
import improbable.util.EntityPrefabs._

object TreeNature extends NatureDescription {

  override def dependencies: Set[NatureDescription] = Set(RigidbodyComposedTransformNature, ColoredNature, FlammableNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = Set[EntityBehaviourDescriptor](
    descriptorOf[SetColorFromFireBehaviour],
    descriptorOf[ReportToTerrainGeneratorNature])

  def apply(initialPosition: Coordinates, onFire:Boolean = false): NatureApplication = {
    application(
      states = Seq(Fire(onFire), Generatorreport(false, 0, 0, isPersistent = true)
      ),
      natures = Seq(
        RigidbodyComposedTransformNature(entityPrefab = TREE, initialPosition = initialPosition, drag = 0.2f, mass = 10, rotationConstraints = FreezeConstraints(x = true, y = true, z = true)),
        ColoredNature(java.awt.Color.green),
        FlammableNature(onFire)
      )
    )
  }

}
