package improbable.natures

import improbable.corelib.natures.base.BaseComposedTransformNature
import improbable.corelib.natures.{NatureApplication, NatureDescription}
import improbable.math.Coordinates
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.util.EntityPrefabs.TREE

object TreeNature extends NatureDescription {

  override def dependencies: Set[NatureDescription] = Set(BaseComposedTransformNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = Set.empty

  def apply(initialPosition: Coordinates): NatureApplication = {
    application(
      natures = Seq(
        BaseComposedTransformNature(entityPrefab = TREE, initialPosition = initialPosition)
      )
    )
  }

}
