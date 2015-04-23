package improbable.debug

import improbable.math.Vector3f

/**
 * Created by rjfwhite on 23/04/2015.
 */
object ColorConversions {
  implicit def toVector(color: java.awt.Color): Vector3f = {
    Vector3f(color.getRed, color.getGreen, color.getBlue) / 255.0f
  }
}
