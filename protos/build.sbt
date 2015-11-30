import sbt.Keys._

// Basic project information
organization.in(Global) := "improbable"

name := "demonstration-protos"

version := "1.0-SNAPSHOT"

scalaVersion := "2.11.7"

// Run configuration
fork in run := true

mainClass.in(Compile) := Some("improbable.worldspec.tools.ProtoV2")


// Dependencies
libraryDependencies += "improbable" % "spatialos_migrations_2.11" % "1.0-SNAPSHOT"


// Test dependencies
libraryDependencies += "org.scalatest" % "scalatest_2.11" % "2.2.4" % "test"

libraryDependencies += "org.mockito" % "mockito-all" % "1.9.5" % "test"


// Publish settings
val nexus = Map(
  "snapshots" -> ("snapshots" at "http://nexus.resources.local:8081/content/repositories/snapshots/"),
  "releases" -> ("releases" at "http://nexus.resources.local:8081/content/repositories/releases/")
)

resolvers.in(Global) ++= nexus.values.toSeq

publishTo.in(Global) := {if (isSnapshot.value) nexus.get("snapshots") else nexus.get("releases")}

credentials.in(Global) ++= (Path.userHome / ".ivy2" * "*credentials").get.map(Credentials(_))