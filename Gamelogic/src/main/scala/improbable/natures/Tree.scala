package improbable.natures

import improbable.corelib.entity.nature.definitions.StaticBodyEntity
import improbable.corelib.entity.nature.{NatureApplication, NatureDescription}
import improbable.math.Vector3d
import improbable.papi.entity.EntityPrefab

object Tree extends NatureDescription {
  override val dependencies = Set[NatureDescription](StaticBodyEntity)

  override def activeBehaviours = {
    Set()
  }

  def apply(position: Vector3d): NatureApplication = {
    application(
      natures = Seq(StaticBodyEntity(EntityPrefab("Tree"), position))
    )
  }
}