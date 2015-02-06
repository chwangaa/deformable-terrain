package improbable.game

import improbable.corelib.entity.nature.definitions.CoreLibPhysicalObject
import improbable.math.Vector3d
import improbable.papi.entity.EntityPrefab
import improbable.papi.worldapp.WorldApp

trait Demo extends WorldApp {
  CoreLibPhysicalObject(Vector3d.zero).spawn(world, EntityPrefab("PlayerBox"))
}