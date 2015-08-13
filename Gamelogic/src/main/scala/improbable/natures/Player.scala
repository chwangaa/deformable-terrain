// Copyright (c) 2014 All Right Reserved, Improbable Worlds Ltd.
package improbable.natures

import improbable.controls.{PlayerControlsBehaviour, PlayerControlsDataDescriptor}
import improbable.corelib.entity.nature.definitions._
import improbable.corelib.entity.nature.{NatureApplication, NatureDescription}
import improbable.corelib.util.EntityOwnerDescriptor
import improbable.math.{Coordinates, Vector3d}
import improbable.papi.engine.EngineId
import improbable.papi.entity.EntityPrefab
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.physical.{PlayerBotBehaviour, PlayerBotDataDescriptor}

object Player extends NatureDescription {
  override val dependencies = Set[NatureDescription](BotEntity)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = {
    Set(descriptorOf[PlayerBotBehaviour], descriptorOf[PlayerControlsBehaviour])
  }

  def apply(prefab: EntityPrefab, engineId: EngineId): NatureApplication = {
    application(
      states = Seq(EntityOwnerDescriptor(Some(engineId)), PlayerBotDataDescriptor(forceMagnitude = 20.0f), PlayerControlsDataDescriptor(Vector3d.zero)),
      natures = Seq(BotEntity(prefab, Coordinates(0, 0.5, 0)))
    )
  }
}
