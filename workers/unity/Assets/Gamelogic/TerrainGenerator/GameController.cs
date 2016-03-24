using System.Collections;
using TerrainGenerator;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{   
    /*
    private const int Radius = 4;

    private Vector2i PreviousPlayerChunkPosition;

    private bool scene_freezed = true;

    public Transform Player;

    public TerrainChunkGenerator Generator;

    public Button StartButton;

    public void Start()
    {
        StartCoroutine(initializePlayerNeighbourhood(Player.position));
    }

    private IEnumerator initializePlayerNeighbourhood(Vector3 center){
        var canActivateCharacter = false;

        Generator.UpdateTerrain(Player.position, Radius);

        // busy wait until the terrains around the current position is properly done
        do
        {
            var exists = Generator.IsTerrainAvailable(center);
            if (exists)
                canActivateCharacter = true;
            yield return null;
        } while (!canActivateCharacter);

        PreviousPlayerChunkPosition = Generator.GetChunkPosition(Player.position);
        // properly position the player
        Player.position = new Vector3(center.x, Generator.GetTerrainHeight(Player.position) + 0.5f, center.z);
        // activate the player
        if(canActivateCharacter){
            Player.gameObject.SetActive(true);
            scene_freezed = false;
        }
    }

    private void Update()
    {
        if (Player.gameObject.activeSelf && !scene_freezed)
        {
            var playerChunkPosition = Generator.GetChunkPosition(Player.position);
            if (!playerChunkPosition.Equals(PreviousPlayerChunkPosition))
            {
                Generator.UpdateTerrain(Player.position, Radius);
                PreviousPlayerChunkPosition = playerChunkPosition;
            }
        }
        else{
            Debug.Log("the game is not ready");
        }
    }

    public void teleportPlayer(){
        Player.gameObject.SetActive(false);
        scene_freezed = true;
        // Random.seed = (int)System.DateTime.Now.Ticks;
        var x = Random.Range(-1000.0F, 1000.0F);
        var z = Random.Range(-1000.0F, 1000.0F);
        StartCoroutine(initializePlayerNeighbourhood(new Vector3(x, 1, z)));

    }
    */
}