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

case class MovementEvent(position: Coordinates, radius: Int) extends CustomMsg

class MoveRandomlyBehaviour(world: World,
                            rigidBodyInterface: RigidbodyInterface,
                            entity: Entity,
                            logger: Logger
                            ) extends EntityBehaviour {

  private var INTENSITY = 5.0f

  override def onReady(): Unit = {
    world.timing.every(1.seconds) {
      moveRandomly()
    }

    entity.watch[Fire].bind.onFire {
      isOnFire =>
          INTENSITY = 8f
    }
  }

  private def moveRandomly(): Unit = {
    val x = randomAxis()
    val z = randomAxis()

    rigidBodyInterface.setForce(Vector3d(x, 0.0f, z).normalised * INTENSITY)

  }

  private def randomAxis(): Double = {
    Random.nextDouble() - 0.5f
  }
}
