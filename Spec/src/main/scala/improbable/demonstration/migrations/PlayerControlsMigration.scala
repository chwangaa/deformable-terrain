package improbable.demonstration.migrations

import improbable.migration.Implicits._
import improbable.migration._
import improbable.spec._

@Timestamp(timestamp = 1439386804473L)
object PlayerControlsMigration extends Migrations(
  state.add(
    State(
      id = "improbable.player.controls.PlayerControlsState",
      description = "Player controls values",
      properties = Seq(
        Property("movementDirection", "Control movement direction", Type.Vec3d)
      ),
      synchronized = true,
      queryable = false
    )
  )
)
