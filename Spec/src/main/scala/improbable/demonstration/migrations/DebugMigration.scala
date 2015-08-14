package improbable.demonstration.migrations

import improbable.migration.Implicits._
import improbable.migration._
import improbable.spec._

@Timestamp(timestamp = 1439386804473L)
object DebugMigration extends Migrations(
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
  )
)
