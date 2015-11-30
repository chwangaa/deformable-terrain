package improbable.demonstration.migrations

import improbable.migration.Implicits._
import improbable.migration._
import improbable.spec._

@Timestamp(timestamp = 1439386804473L)
object ColorMigration extends Migrations(
  state.add(
    State(
      id = "improbable.color.ColorState",
      description = "Color of entities",
      properties = Seq(
        Property("value", "Color value", Type.Vec3f)
      ),
      synchronized = true,
      queryable = false
    )
  )
)
