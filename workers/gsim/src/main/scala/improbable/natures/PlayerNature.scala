package improbable.natures

import improbable.behaviours.physical.ExtinguishFlamesBehaviour
import improbable.behaviours.player.controls.physical.PlayerBehaviour
import improbable.behaviours.player.controls.{RaycastResponderBehaviour, RaycastRequestorBehaviour, DelegateLocalPlayerCheckToOwnerBehaviour, DelegatePlayerControlsToOwnerBehaviour}
import improbable.corelib.natures.bot.BotComposedTransformNature
import improbable.corelib.natures.{NatureApplication, NatureDescription}
import improbable.corelib.util.EntityOwner
import improbable.math.{Coordinates, Vector3d}
import improbable.papi.engine.EngineId
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.physical.{RaycastResponse, RaycastRequest}
import improbable.player.LocalPlayerCheckState
import improbable.player.controls.PlayerControlsState
import improbable.player.physical.PlayerState
import improbable.util.EntityPrefabs._

object PlayerNature extends NatureDescription {

  override def dependencies: Set[NatureDescription] = Set(BotComposedTransformNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = {
    Set(
      descriptorOf[PlayerBehaviour],
      descriptorOf[DelegatePlayerControlsToOwnerBehaviour],
      descriptorOf[DelegateLocalPlayerCheckToOwnerBehaviour],
      descriptorOf[RaycastRequestorBehaviour],
      descriptorOf[RaycastResponderBehaviour],
      descriptorOf[ExtinguishFlamesBehaviour]
    )
  }

  def apply(engineId: EngineId): NatureApplication = {
    application(
      states = Seq(
        EntityOwner(ownerId = Some(engineId)),
        PlayerState(forceMagnitude = 20.0f),
        PlayerControlsState(movementDirection = Vector3d.zero),
        LocalPlayerCheckState(),
        RaycastRequest(),
        RaycastResponse()
      ),
      natures = Seq(
        BotComposedTransformNature(entityPrefab = PLAYER, initialPosition = Coordinates(0, 50, 0))
      )
    )
  }

}
