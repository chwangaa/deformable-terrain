import com.typesafe.sbt.packager.SettingsHelper
import com.typesafe.sbt.packager.archetypes.JavaAppPackaging.autoImport._
import com.typesafe.sbt.packager.universal.UniversalPlugin.autoImport._
import scala.util.parsing.json._

val nexus = Map(
  "snapshots" -> ("snapshots" at "https://releases.service.improbable.io/content/repositories/snapshots/"),
  "releases" -> ("releases" at "https://releases.service.improbable.io/content/repositories/releases/"))

resolvers.in(Global) ++= nexus.values.toSeq

credentials.in(Global) ++= (Path.userHome / ".ivy2" * "*credentials").get.map(Credentials(_))

val projectManifestText = IO.read(file("../../spatialos.json")).trim
val projectManifestObj:Option[Any] = JSON.parseFull(projectManifestText)
val projectManifest:Map[String,Any] = projectManifestObj.get.asInstanceOf[Map[String, Any]]
val currentVersion = projectManifest.get("project_version").get.asInstanceOf[String]
val improbableVersion = projectManifest.get("sdk_version").get.asInstanceOf[String]
val projectName = projectManifest.get("name").get.asInstanceOf[String]

lazy val rootProject = Project(id = projectName, base = file("."))
  .settings(scalaVersion := "2.11.7")
  .settings(version := currentVersion)
  .settings(libraryDependencies += "improbable" %% "deployment" % improbableVersion)
  .settings(libraryDependencies += "improbable" %% "corelibrary-deprecated" % improbableVersion)
  .settings(unmanagedSourceDirectories.in(Compile) += baseDirectory.value / "generated")
  .settings(libraryDependencies += "improbable" %% "universal-tiny-client" % improbableVersion)
  .enablePlugins(JavaAppPackaging)
  .settings(distZipSettings: _*)

lazy val distZipSettings: Seq[Def.Setting[_]] = {
  SettingsHelper.makeDeploymentSettings(Universal, packageBin.in(Universal), "zip") ++ Seq(
    mainClass.in(Compile) := Some("improbable.deployment.GameLauncher"),
    packageName.in(Universal) := "gsim",
    bashScriptExtraDefines ++= Seq(
      "-XX:+UseG1GC",
      "-XX:+UnlockExperimentalVMOptions",
      "-XX:G1NewSizePercent=25",
      "-XX:G1MaxNewSizePercent=75",
      "-XX:MaxGCPauseMillis=20"
    ).map(args => s"""addJava "$args""""))
}

dist := {
  val distZipPath = dist.value
  val outputDir = baseDirectory.value / ".." / ".." / "build" / "assembly" / "worker"
  IO.createDirectory(outputDir)

  val targetZipPath = outputDir / "gsim.zip"
  IO.copyFile(distZipPath, targetZipPath, preserveLastModified = false)

  streams.value.log.info(s"Copied $distZipPath to $targetZipPath")

  targetZipPath
}


// allow toolbelt to see the stack's stdout
fork in runMain := false

//4926da6cf56f0740af08783ba26abc68af9c2e41
