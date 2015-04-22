package improbable.game

import improbable.entitytemplates.{DemoTree, DemoBot}
import improbable.math.Vector3d
import improbable.papi.entity.EntityPrefab
import improbable.papi.worldapp.WorldApp

import scala.util.Random

trait TreeSpawner extends WorldApp {
  val prefab = EntityPrefab("Cube")

  spawnKindling()
  spawnIgnition()
  spawnRandomTrees()

  private def spawnKindling(): Unit = {
    Range(1, 200).foreach {
      _ => DemoBot(Vector3d.zero).spawn(world, prefab)
    }
  }

  private def spawnIgnition(): Unit = {
    DemoBot(Vector3d.unitX * 50.0f, onFire = true).spawn(world, prefab)
  }

  private def spawnRandomTrees(): Unit = {
    Range(1, 1000).foreach { _ =>
      spawnRandomTree()
    }
  }

  private def spawnRandomTree(): Unit = {
    val x = (Random.nextDouble() - 0.5f) * 1000.0f
    val z = (Random.nextDouble() - 0.5f) * 1000.0f
    DemoTree(Vector3d(x, 0.5, z)).spawn(world, EntityPrefab("Tree"))
  }
}