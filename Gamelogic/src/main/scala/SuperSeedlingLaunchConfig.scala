import improbable.corelib.launcher.DefaultLaunchConfig
import improbable.game.{DemoDescriptor, PlayerLifeCycleManagerDescriptor}

object ManualEngineStartupLaunchConfig extends SuperSeedlingLaunchConfig(false)

object AutomaticEngineStartupLaunchConfig extends SuperSeedlingLaunchConfig(true)

class SuperSeedlingLaunchConfig(dynamicallySpoolUpEngines: Boolean) extends DefaultLaunchConfig(
  Seq(PlayerLifeCycleManagerDescriptor, DemoDescriptor),
  dynamicallySpoolUpEngines
)
