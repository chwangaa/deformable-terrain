package improbable.natures

import improbable.behaviours.ReportToTerrainGeneratorBehaviour
import improbable.behaviours.physical.ExtinguishFlamesBehaviour
import improbable.behaviours.player.controls.physical.PlayerBehaviour
import improbable.behaviours.player.controls.{RaycastResponderBehaviour, RaycastRequestorBehaviour, DelegateLocalPlayerCheckToOwnerBehaviour, DelegatePlayerControlsToOwnerBehaviour}
import improbable.corelib.natures.bot.BotComposedTransformNature
import improbable.corelib.natures.{NatureApplication, NatureDescription}
import improbable.corelib.util.EntityOwner
import improbable.math.{Coordinates, Vector3d}
import improbable.papi.engine.EngineId
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.physical.{Generatorreport, RaycastResponse, RaycastRequest}
import improbable.player.LocalPlayerCheckState
import improbable.player.controls.PlayerControlsState
import improbable.player.physical.PlayerState
import improbable.util.EntityPrefabs._
import improbable.util.TerrainGeneratorSetting

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
      //descriptorOf[ReportToTerrainGeneratorBehaviour]
    )
  }

  def apply(engineId: EngineId, checkout_radius:Int = TerrainGeneratorSetting.PLAYER_VIEW_RADIUS, report_period:Int = 200): NatureApplication = {
    application(
      states = Seq(
        EntityOwner(ownerId = Some(engineId)),
        PlayerState(forceMagnitude = 20.0f),
        PlayerControlsState(movementDirection = Vector3d.zero),
        LocalPlayerCheckState(),
        RaycastRequest(),
        RaycastResponse()
        //Generatorreport(true, report_period, checkout_radius)
      ),
      natures = Seq(
        BotComposedTransformNature(entityPrefab = PLAYER, mass = 10, hasGravity = false, initialPosition = Coordinates(0, 40, 0))
      )
    )
  }

}
