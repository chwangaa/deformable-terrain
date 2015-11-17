package improbable.apps

import com.typesafe.scalalogging.Logger
import improbable.math.Coordinates
import improbable.natures.TreeNature
import improbable.papi.world.AppWorld
import improbable.papi.worldapp.{WorldApp, WorldAppLifecycle}

import scala.util.Random

class TreeSpawner(val world: AppWorld,
                  override val logger: Logger,
                  val lifecycle: WorldAppLifecycle) extends WorldApp {

  spawnRandomTrees()

  private def spawnRandomTrees(): Unit = {
    Range(1, TreeSpawner.NUMBER_OF_TREES).foreach { _ =>
      spawnRandomTree()
    }
  }

  private def spawnRandomTree(): Unit = {
    val x = (Random.nextDouble() - 0.5f) * TreeSpawner.DISTANCE
    val z = (Random.nextDouble() - 0.5f) * TreeSpawner.DISTANCE
    world.entities.spawnEntity(TreeNature(Coordinates(x, 0.5, z)))
  }

}

object TreeSpawner {
  val DISTANCE = 500
  val NUMBER_OF_TREES = 500
}
