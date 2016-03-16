package improbable.behaviours.physical

import com.typesafe.scalalogging.Logger
import improbable.papi.entity.{EntityBehaviour, Entity}
import improbable.papi.world.World
import improbable.physical.{Extinguish, RaycastResponse}

class ExtinguishFlamesBehaviour(entity : Entity, logger : Logger, world: World) extends EntityBehaviour {
  entity.watch[RaycastResponse].onRaycastResponded {
    payload => {
      logger.info("Raycast hit entity: " + payload.entityId)
      world.messaging.sendToEntity(payload.entityId, Extinguish)
    }
  }
}