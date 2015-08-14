package improbable.demonstration.migrations

import improbable.migration.Implicits._
import improbable.migration._
import improbable.spec._

@Timestamp(timestamp = 1439386804473L)
object PlayerControlsMigration extends Migrations(
  state.add(
    State(
      id = "improbable.controls.PlayerControlsData",
      description = "player controls values",
      properties = Seq(
        Property("movementDirection", "control movement direction", Type.Vec3d)
      ),
      events = Seq(

      ),
      synchronized = true,
      queryable = false
    )
  )
)
