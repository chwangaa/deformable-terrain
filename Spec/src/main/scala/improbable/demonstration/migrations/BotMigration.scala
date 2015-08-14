package improbable.demonstration.migrations

import improbable.migration.Implicits._
import improbable.migration._
import improbable.spec._

@Timestamp(timestamp = 1439386804473L)
object BotMigration extends Migrations(
  state.add(
    State(
      id = "improbable.physical.PlayerBotData",
      description = "Parameters for player movement",
      properties = Seq(
        Property("forceMagnitude", "scaling factor for force applied to player", Type.Float)
      ),
      events = Seq(

      ),
      synchronized = true,
      queryable = false
    )
  )
)
