package improbable.demonstration.migrations

import improbable.migration._
import improbable.spec._
import Implicits._

@Timestamp(timestamp = 1439386804473L)
object InitialMigration extends Migrations(
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
  ),
  state.add(
    State(
      id = "improbable.debug.Color",
      description = "Color of entities",
      properties = Seq(
        Property("value", "color value", Type.Vec3f)
      ),
      events = Seq(

      ),
      synchronized = true,
      queryable = false
    )
  ),
  state.add(
    State(
      id = "improbable.debug.Text",
      description = "debug text for an entity",
      properties = Seq(

      ),
      events = Seq(
        Event("improbable.debug.EmitText", "emit debug text", properties = Seq(
          Property("content", "debug text string", Type.String)
        ))
      ),
      synchronized = true,
      queryable = false
    )
  ),
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
