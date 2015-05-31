package improbable.apps

import improbable.entity.physical.Position
import improbable.math.Vector3d
import improbable.natures.Bot
import improbable.papi.entity.{EntityPrefab, EntityRecordTemplate}
import improbable.papi.worldapp.WorldApp

trait CubeSpawner extends WorldApp {
  spawnCubes()

  private def spawnCubes(): Unit = {
    Range(1, 200).foreach {
      _ =>
        Thread.sleep(200)

        world.entities.spawnEntity(EntityRecordTemplate(
          EntityPrefab("Cube"),
          Nil,
          Seq(Position(0, Vector3d(1, 2, 3)))
        ))
        world.entities.spawnEntity(Bot(Vector3d.unitY * 20.0f + Vector3d.unitX * 10.0f))
    }
  }
}