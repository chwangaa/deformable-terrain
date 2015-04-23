package improbable.natures

import improbable.corelib.Prefab
import improbable.corelib.entity.nature.definitions.CoreLibObject
import improbable.corelib.entity.nature.{NatureApplication, NatureDescription}
import improbable.math.Vector3d

object Tree extends NatureDescription {
  override val dependencies = Set[NatureDescription](CoreLibObject)

  override def activeBehaviours = {
    Set()
  }

  def apply(position: Vector3d): NatureApplication = {
    application(
      states = Seq(Prefab("Tree")),
      natures = Seq(CoreLibObject(position))
    )
  }
}