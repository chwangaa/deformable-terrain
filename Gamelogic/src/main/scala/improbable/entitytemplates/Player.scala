// Copyright (c) 2014 All Right Reserved, Improbable Worlds Ltd.
package improbable.entitytemplates

import improbable.corelib.entity.nature.definitions._
import improbable.corelib.entity.nature.{NatureApplication, NatureDescription}
import improbable.corelib.util.EntityOwnerDescriptor
import improbable.math.Vector3
import improbable.papi.engine.EngineId
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.physical.{NewPlayerBotBehaviour, PlayerBotDataDescriptor}

object Player extends NatureDescription {
  override def activeBehaviours: Set[EntityBehaviourDescriptor] = {
    Set(NewPlayerBotBehaviour())
  }

  override val dependencies = Set[NatureDescription](CoreLibBotObject)

  def apply(engineId: EngineId): NatureApplication = {
    application(
      states = Seq(EntityOwnerDescriptor(engineId), PlayerBotDataDescriptor(forceMagnitude = 10.0f)),
      natures = Seq(CoreLibBotObject(Vector3(0, 0.5, 0)))
    )
  }
}