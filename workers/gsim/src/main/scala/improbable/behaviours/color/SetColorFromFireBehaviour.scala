package improbable.behaviours.color

import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.physical.Fire

class SetColorFromFireBehaviour(colorInterface : ColorInterface, entity : Entity) extends EntityBehaviour{
  entity.watch[Fire].bind.onFire {
    isOnFire =>
      val color = if (isOnFire) java.awt.Color.red else java.awt.Color.green
      colorInterface.setColor(color)
  }
}