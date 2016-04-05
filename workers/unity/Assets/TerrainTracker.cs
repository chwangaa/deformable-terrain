using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Improbable.Unity;
using Improbable.Entity.Physical;
using Improbable.Unity.Visualizer;
using System.Linq;

public class TerrainTracker : MonoBehaviour {

    [Require]
    protected PositionReader Position;

    private Vector2 PreviousPlayerChunkPosition;

    private int terrain_length = 100;

    private int radius = 200;



    // Use this for initialization
    void Start () {
        PreviousPlayerChunkPosition = getCurrentPosition();
    }

    void OnEnable()
    {
        generateNeighbourhood();
    }
	
	// Update is called once per frame
	void Update () {
        if (PositionChanged())
        {
            generateNeighbourhood();
        }
        
    }

    private void generateNeighbourhood()
    {
        HashSet<Vector2> neighbour_terrains = findCoordinatesOfTerrainsThatNeedToBeGenerated();
        askGeneratorForTheseTerrains(neighbour_terrains);
    }

    private HashSet<Vector2> findCoordinatesOfTerrainsThatNeedToBeGenerated()
    {
        float x = PreviousPlayerChunkPosition.x;
        float z = PreviousPlayerChunkPosition.y;
        HashSet<Vector2> neighbour_terrains = new HashSet<Vector2>();
        var start_position = getTerrainCoordinateForObjectPosition(x-radius, z-radius);
        var end_position = getTerrainCoordinateForObjectPosition(x + radius, z + radius);

        for(float i = start_position.x; i < end_position.x; i += terrain_length)
        {
            for(float j = start_position.y; j < end_position.y; j += terrain_length)
            {
                neighbour_terrains.Add(new Vector2(i, j));
            }
        }

        return neighbour_terrains;
    }



    private void askGeneratorForTheseTerrains(IEnumerable<Vector2> new_terrain_coordiantes)
    {
        foreach(var new_terrain_coordinate in new_terrain_coordiantes)
        {
            terrainVisualizer.requestChunkPosition(new_terrain_coordinate);
        }
    }

    private bool PositionChanged()
    {
        var newPosition = getCurrentPosition();
        if((newPosition - PreviousPlayerChunkPosition).magnitude < 100)
        {
            return false;
        }
        else
        {
            PreviousPlayerChunkPosition = newPosition;
            return true;
        }
    }

    private Vector2 getCurrentPosition()
    {
        var position = Position.Value.ToUnityVector();
        return new Vector2(position.x, position.z);
    }


    private Vector2 getTerrainCoordinateForObjectPosition(float x, float z)
    {

        int terrain_x = (int)Mathf.Floor(x / terrain_length) * terrain_length;
        int terrain_z = (int)Mathf.Floor(z / terrain_length) * terrain_length;
        return new Vector2(terrain_x, terrain_z);
    }
}
