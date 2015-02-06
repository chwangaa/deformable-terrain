package improbable.physical

import improbable.math.Vector3d

import scala.concurrent.duration._
import scala.util.Random

trait MoveRandomlyBehaviour extends MoveRandomlyBehaviourBase {

  val INTENSITY = 20.0f

  world.timing.every(1.seconds) {
    moveRandomly()
  }

  private def moveRandomly(): Unit = {
    val x = randomAxis()
    val z = randomAxis()
    rigidbodyInterface.setForce(Vector3d(x, 0.0f, z).normalised * INTENSITY)
  }

  private def randomAxis(): Double = {
   Random.nextDouble() - 0.5f
  }

}