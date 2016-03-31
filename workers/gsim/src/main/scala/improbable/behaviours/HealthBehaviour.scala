
package improbable.behaviours

import com.typesafe.scalalogging.Logger
import improbable.behaviours.color.ColorInterface
import improbable.papi.entity.{EntityBehaviour, Entity}
import improbable.papi.world.World
import improbable.papi.world.messaging.CustomMsg
import improbable.physical.{Bla, BlaWriter}
import scala.concurrent.duration._

case class HealthDamage(damage: Int) extends CustomMsg


class HealthBehaviour(entity : Entity, logger : Logger, world: World, health:BlaWriter, colorInterface: ColorInterface) extends EntityBehaviour {
  override def onReady(): Unit = {
    // add tag to the entity so movement event can be received here
    world.messaging.onReceive {
      case HealthDamage(damage) => {
        reduceHealthBy(damage)
      }
    }
  }

  def reduceHealthBy(damage:Int): Unit = {
    val current_health_level:Int = health.life
    val new_health = current_health_level - damage
    if(new_health <= 0){
      dealWithZeroHealthEntity()
    }
    else{
      health.update.life(new_health).finishAndSend()
    }
  }

  def dealWithZeroHealthEntity():Unit = {
    world.entities.destroyEntity(entity.entityId)
    colorInterface.setColor(java.awt.Color.RED)
    health.update.triggerBlaDyingRequested().finishAndSend()
    world.timing.after(500.milliseconds){
      entity.destroy()
    }
  }
}
