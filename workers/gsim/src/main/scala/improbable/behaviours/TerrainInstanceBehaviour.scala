package improbable.behaviours

import com.typesafe.scalalogging.Logger
import improbable.math.Coordinates
import improbable.papi.entity.{EntityBehaviour, Entity}
import improbable.papi.world.World
import improbable.papi.world.messaging.CustomMsg
import improbable.physical.RaycastResponse
import improbable.terrainchunk.{BuildingRequest, DamageRequestWriter, Damage, TerrainDamageWriter}
import improbable.unity.fabric.PhysicsEngineConstraint

case class TerrainDamage(position:Coordinates, radius:Int) extends CustomMsg
case class TerrainDestroyRequest() extends CustomMsg
class TerrainInstanceBehaviour(entity : Entity, logger : Logger, world: World, damages:TerrainDamageWriter, damage_request:DamageRequestWriter) extends EntityBehaviour {
  override def onReady(): Unit = {
    // add tag to the entity so movement event can be received here
    entity.addTag("Terrain")

    /** The main duty of this behaviour is to listen for terrainDamge messages, which is initiated by the damage entities
      * and forwarded by the terrain generator. A new message is sent, and listend by c# code to apply the damage to the terrain
      */
    world.messaging.onReceive {
      case TerrainDamage(position, radius) =>{
        applyDamageAt(position, radius)
      }
      case TerrainDestroyRequest() =>{
        if(damages.damages.isEmpty){  // if the terrain is not damaged, accept the destroy request
          entity.destroy()
        }
        else{
          logger.info("ignore destroying damaged terrain")
        }
      }
    }


  }

  /**
    *
    * @param position center of the damage
    * @param radius radius of the damage
    */
  def applyDamageAt(position: Coordinates, radius: Int):Unit = {
    val new_damage:Damage = Damage(position, radius)
    val old_damages:List[Damage] = damages.damages
    val new_damages:List[Damage] = old_damages.+:(new_damage) // add the damage to the state list
    damages.update.damages(new_damages).finishAndSend() // finish and send
    damage_request.update.triggerDamageRequested(new_damage).finishAndSend()
  }
}