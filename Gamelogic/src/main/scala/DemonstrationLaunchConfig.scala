import improbable.apps._
import improbable.corelib.launcher.DefaultLaunchConfig

class DemonstrationLaunchConfig(dynamicallySpoolUpEngines: Boolean) extends DefaultLaunchConfig(
  Seq(classOf[CubeSpawner], classOf[PlayerLifeCycleManager], classOf[TreeSpawner]),
  dynamicallySpoolUpEngines
)

object ManualEngineStartupLaunchConfig extends DemonstrationLaunchConfig(false)

object AutomaticEngineStartupLaunchConfig extends DemonstrationLaunchConfig(true)
