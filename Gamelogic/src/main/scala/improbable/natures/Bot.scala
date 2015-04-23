package improbable.natures

import improbable.corelib.Prefab
import improbable.corelib.entity.nature.definitions.CoreLibPhysicalObject
import improbable.corelib.entity.nature.{NatureApplication, NatureDescription}
import improbable.demo.Flammable
import improbable.math._

object Bot extends NatureDescription {
  override val dependencies = Set[NatureDescription](CoreLibPhysicalObject, TextDisplayer, Colored)

  override def activeBehaviours = {
    Set()
  }

  def apply(position: Vector3d, onFire: Boolean = false): NatureApplication = {
    application(
      states = Seq(
        Flammable(onFire),
        Prefab("Cube")
      ),
      natures = Seq(
        CoreLibPhysicalObject(position, drag = 0.2f),
        TextDisplayer(),
        Colored(java.awt.Color.red))
    )
  }
}