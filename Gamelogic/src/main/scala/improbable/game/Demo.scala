package improbable.game

import improbable.entitytemplates.DemoBot
import improbable.math.Vector3d
import improbable.papi.entity.EntityPrefab
import improbable.papi.worldapp.WorldApp

trait Demo extends WorldApp {
  Range(1, 200).foreach {
    _ =>
    DemoBot(Vector3d.zero).spawn(world, EntityPrefab("PlayerCube"))
  }
}
