// Copyright (c) 2014 All Right Reserved, Improbable Worlds Ltd.
package improbable.natures

import improbable.controls.{NewPlayerControlsBehaviour, PlayerControlsDataDescriptor}
import improbable.corelib.entity.nature.definitions._
import improbable.corelib.entity.nature.{NatureApplication, NatureDescription}
import improbable.corelib.util.EntityOwnerDescriptor
import improbable.math.Vector3d
import improbable.papi.engine.EngineId
import improbable.papi.entity.EntityPrefab
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.physical.{NewPlayerBotBehaviour, PlayerBotDataDescriptor}

object Player extends NatureDescription {
  override val dependencies = Set[NatureDescription](BotEntity)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = {
    Set(NewPlayerBotBehaviour(), NewPlayerControlsBehaviour())
  }

  def apply(prefab: EntityPrefab, engineId: EngineId): NatureApplication = {
    application(
      states = Seq(EntityOwnerDescriptor(Some(engineId)), PlayerBotDataDescriptor(forceMagnitude = 20.0f), PlayerControlsDataDescriptor(Vector3d.zero)),
      natures = Seq(BotEntity(prefab, Vector3d(0, 0.5, 0)))
    )
  }
}