package improbable.behaviours

import com.typesafe.scalalogging.Logger
import improbable.papi.entity.{EntityBehaviour, Entity}
import improbable.papi.world.World
import improbable.physical.{Ignite, Extinguish, RaycastResponse}

// the whole purpose of this behaviour is to add a tag to terrain entities
class TerrainInstanceBehaviour(entity : Entity, logger : Logger, world: World) extends EntityBehaviour {
  override def onReady(): Unit = {
    // add tag to the entity so movement event can be received here
    // listen to the movement events
    world.messaging.onReceive {
      case ChangeTagToPersistentTerrain() =>
        {
          entity.removeTag("Terrain")
          entity.addTag("PersistentTerrain")
          logger.info(entity.position.toString() + " is now a persistent terrain")
        }
    }
  }
}