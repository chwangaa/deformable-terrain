package improbable.apps

import improbable.math.Vector3d
import improbable.natures.Tree
import improbable.papi.worldapp.WorldApp

import scala.util.Random

trait TreeSpawner extends WorldApp {
  spawnRandomTrees()

  private def spawnRandomTrees(): Unit = {
    Range(1, 1000).foreach { _ =>
      spawnRandomTree()
    }
  }

  private def spawnRandomTree(): Unit = {
    val x = (Random.nextDouble() - 0.5f) * 1000.0f
    val z = (Random.nextDouble() - 0.5f) * 1000.0f
    world.entities.spawnEntity(Tree(Vector3d(x, 0.5, z)))
  }
}