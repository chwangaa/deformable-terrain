package improbable.apps

import improbable.math.Vector3d
import improbable.natures.Tree
import improbable.papi.worldapp.WorldApp

import scala.util.Random

trait TreeSpawner extends WorldApp {
  spawnRandomTrees()

  private def spawnRandomTrees(): Unit = {
    Range(1, TreeSpawner.NUMBER_OF_TREES).foreach { _ =>
      spawnRandomTree()
    }
  }

  private def spawnRandomTree(): Unit = {
    val x = (Random.nextDouble() - 0.5f) * TreeSpawner.DISTANCE
    val z = (Random.nextDouble() - 0.5f) * TreeSpawner.DISTANCE
    world.entities.spawnEntity(Tree(Vector3d(x, 0.5, z)))
  }
}

object TreeSpawner {
  val DISTANCE = 3000f
  val NUMBER_OF_TREES = 10000
}