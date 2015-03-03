package improbable.entitytemplates

import improbable.corelib.entity.nature.definitions.CoreLibObject
import improbable.corelib.entity.nature.{NatureApplication, NatureDescription}
import improbable.math.Vector3d

object DemoTree extends NatureDescription {
  override def activeBehaviours = {
    Set()
  }

  override val dependencies = Set[NatureDescription](CoreLibObject)

  def apply(position: Vector3d): NatureApplication = {
    application(
      natures = Seq(CoreLibObject(position))
    )
  }
}