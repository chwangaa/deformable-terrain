package improbable.behaviours.physical

import improbable.entity.physical.RigidbodyInterface
import improbable.math.Vector3d
import improbable.papi.entity.EntityBehaviour
import improbable.papi.world.World

import scala.concurrent.duration._
import scala.util.Random

class MoveRandomlyBehaviour(world: World,
                            rigidBodyInterface: RigidbodyInterface) extends EntityBehaviour {

  val INTENSITY = 20.0f

  world.timing.every(1.seconds) {
    moveRandomly()
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
