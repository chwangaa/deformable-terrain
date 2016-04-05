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

class JumpingAroundBehaviour(world: World,
                            rigidBodyInterface: RigidbodyInterface,
                            entity: Entity,
                            logger: Logger
                           ) extends EntityBehaviour {

  override def onReady(): Unit = {
    world.timing.every(1000.milliseconds) {
      moveRandomly();
    }
  }

  private def moveRandomly(): Unit = {
    val direction = Random.nextInt(4)

    direction match {
      case 0 =>
        moveRight()
      case 1 =>
        moveLeft()
      case 2 =>
        moveDown()
      case _ =>
        moveUp()
    }
  }

  private def moveRight(): Unit = {
    rigidBodyInterface.setForce(Vector3d(100, 0, 0))
    rigidBodyInterface.setVelocity(Vector3d(100, 0, 0))
  }

  private def moveLeft(): Unit = {
    rigidBodyInterface.setForce(Vector3d(-100, 0, 0))
    rigidBodyInterface.setVelocity(Vector3d(-100, 0, 0))
  }

  private def moveUp(): Unit = {
    rigidBodyInterface.setForce(Vector3d(0, 0, 100))
    rigidBodyInterface.setVelocity(Vector3d(0, 0, 100))
  }

  private def moveDown(): Unit = {
    rigidBodyInterface.setForce(Vector3d(0, 0, -100))
    rigidBodyInterface.setVelocity(Vector3d(0, 0, -100))
  }


}
