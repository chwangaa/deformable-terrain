package improbable.game

import improbable.entitytemplates.DemoBot
import improbable.math.Vector3d
import improbable.papi.entity.EntityPrefab
import improbable.papi.worldapp.WorldApp

trait Demo extends WorldApp {
  Thread.sleep(5000)
  Range(1, 600).foreach {
    _ =>
      Thread.sleep(300)
    DemoBot(Vector3d.zero).spawn(world, EntityPrefab("PlayerCube"))
  }
  DemoBot(Vector3d.unitX * 50.0f, true).spawn(world, EntityPrefab("PlayerCube"))
}
