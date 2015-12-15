resolvers := Seq(
  "Improbable Nexus Repository Releases" at s"https://releases.service.improbable.io/content/repositories/releases/",
  "Improbable Nexus Repository Snapshots" at s"https://releases.service.improbable.io/content/repositories/snapshots/",
  "Spray Repository" at "http://repo.spray.io/"
) ++ resolvers.value

credentials.in(Global) ++= (Path.userHome / ".ivy2" * "*credentials").get.map(Credentials(_))

val everythingVersion = IO.read(file("project/everything.version")).trim

addSbtPlugin("improbable" % "fabric-sdk-build-plugin" % everythingVersion)
addSbtPlugin("improbable" % "unity-sdk-build-plugin" % everythingVersion)
