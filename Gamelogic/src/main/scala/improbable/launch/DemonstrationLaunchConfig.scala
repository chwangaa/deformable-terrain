package improbable.launch

import improbable.apps._
import improbable.corelib.launcher.DefaultLaunchConfig

class DemonstrationLaunchConfig(dynamicallySpoolUpEngines: Boolean) extends DefaultLaunchConfig(
  Seq(classOf[CubeSpawner], classOf[PlayerLifeCycleManager], classOf[TreeSpawner]),
  dynamicallySpoolUpEngines
)

object ManualEngineStartupLaunchConfig extends DemonstrationLaunchConfig(dynamicallySpoolUpEngines = false)

object AutomaticEngineStartupLaunchConfig extends DemonstrationLaunchConfig(dynamicallySpoolUpEngines = true)
