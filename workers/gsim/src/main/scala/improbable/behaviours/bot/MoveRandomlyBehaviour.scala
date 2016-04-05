package improbable.behaviours.bot

import com.typesafe.scalalogging.Logger
import improbable.entity.physical.RigidbodyInterface
import improbable.math.{Coordinates, Vector3d}
import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.papi.world.World
import improbable.papi.world.entities.EntityFindByTag
import improbable.papi.world.messaging.CustomMsg
import improbable.physical.{Fire}

import scala.concurrent.duration._
import scala.util.Random

class MoveRandomlyBehaviour(world: World,
                            rigidBodyInterface: RigidbodyInterface,
                            entity: Entity,
                            logger: Logger
                            ) extends EntityBehaviour {

  private var INTENSITY = 20.0f

  override def onReady(): Unit = {
    world.timing.every(1000.milliseconds) {
      moveRandomly()
    }
  }

  private def moveRandomly(): Unit = {
    val x = randomAxis()
    val z = randomAxis()
    rigidBodyInterface.setForce(Vector3d(x, 0, z).normalised * INTENSITY)
  }

  private def randomAxis(): Double = {
    Random.nextDouble() - 0.5f
  }
}
