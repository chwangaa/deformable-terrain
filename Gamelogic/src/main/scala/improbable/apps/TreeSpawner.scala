package improbable.apps

import improbable.natures.{Tree, Bot}
import improbable.math.Vector3d
import improbable.papi.entity.EntityPrefab
import improbable.papi.worldapp.WorldApp

import scala.util.Random

trait TreeSpawner extends WorldApp {

  Thread.sleep(500)
  spawnRandomTrees()
  spawnKindling()
  spawnIgnition()

  private def spawnKindling(): Unit = {
    Range(1, 200).foreach {
      _ =>
        Thread.sleep(200)
        world.entities.spawnEntity(Bot(Vector3d.unitY * 20.0f + Vector3d.unitX * 10.0f))
    }
  }

  private def spawnIgnition(): Unit = {
    world.entities.spawnEntity(Bot(Vector3d.unitX * 50.0f, onFire = true))
  }

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