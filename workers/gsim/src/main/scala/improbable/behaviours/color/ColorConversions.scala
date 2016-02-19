package improbable.behaviours.color

import improbable.math.Vector3f

import scala.language.implicitConversions

object ColorConversions {

  implicit def toVector(color: java.awt.Color): Vector3f = {
    Vector3f(color.getRed, color.getGreen, color.getBlue) / 255.0f
  }

}
