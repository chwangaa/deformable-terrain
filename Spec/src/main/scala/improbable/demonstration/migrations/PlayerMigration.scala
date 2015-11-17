package improbable.demonstration.migrations

import improbable.migration.Implicits._
import improbable.migration._
import improbable.spec._

@Timestamp(timestamp = 1439386804473L)
object PlayerMigration extends Migrations(
  state.add(
    State(
      id = "improbable.player.physical.PlayerState",
      description = "Parameters for player movement",
      properties = Seq(
        Property("forceMagnitude", "Scaling factor for force applied to player", Type.Float)
      ),
      synchronized = true,
      queryable = false
    )
  )
)
