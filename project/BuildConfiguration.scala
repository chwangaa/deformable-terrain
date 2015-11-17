import improbable.build._
import improbable.build.fabric._
import improbable.build.unity._
import improbable.build.util.Versions
import improbable.sdk.SdkInfo

object BuildConfiguration extends improbable.build.ImprobableBuild(
  projectName = "demonstration",
  organisation = "improbable",
  version = Versions.fetchVersion("demonstration"),
  buildSettings = Seq(FabricBuildSettings(), UnityPlayerProject()),
  dependencies = List(new SimulationLibrary("improbable", "core-library", SdkInfo.version)),
  isLibrary = false
)
