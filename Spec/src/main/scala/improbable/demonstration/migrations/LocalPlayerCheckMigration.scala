package improbable.demonstration.migrations

import improbable.migration.Implicits._
import improbable.migration._
import improbable.spec._

@Timestamp(timestamp = 1439386804473L)
object LocalPlayerCheckMigration extends Migrations(
  state.add(
    State(
      id = "improbable.player.LocalPlayerCheckState",
      description = "Dummy state to be delegated so that visualizers can identify when they are the local player",
      synchronized = true,
      queryable = false
    )
  )
)
