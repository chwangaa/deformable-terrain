using UnityEngine;
using Improbable.Entity.Physical;
using Improbable.Unity.Visualizer;
using Improbable.Unity;
using Improbable.Terrainchunk;
using System.Collections;
using Improbable.Damage;

public class terrain_damage_initializer : MonoBehaviour {

    [Require]
    protected PositionReader Position;

    [Require]
    protected DamagedStateReader damage_reader;

    int radius;

    Vector3 damage_position;

    Vector3 terrain_position;

    // Use this for initialization
    void Start () {
	
	}

    void OnEnable()
    {
        damage_position = damage_reader.Position.ToUnityVector();
        radius = damage_reader.Radius;
        terrain_position = Position.Value.ToUnityVector();

        findTerrainAndApplyDamage();
    }

    void findTerrainAndApplyDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(damage_position, 5);
        int i = 0;
        while (i < hitColliders.Length)
        {
            Terrain terrain = (Terrain)hitColliders[i].gameObject.GetComponent("Terrain");
            if (terrain != null)
            {
                Debug.Log("terrain position is " + terrain.transform.position + " suppose to be " + terrain_position);
                applyDamageToTerrain(terrain, radius);
            }
            else
            {
                Debug.Log("the colliding object is " + hitColliders[i].gameObject.name);
            }
            i++;
        }
    }

    public void applyDamageToTerrain(Terrain terrain, float craterRadius)
    {

        //get the heights only once keep it and reuse, precalculate as much as possible
        TerrainData data = terrain.terrainData;

        Vector3 terrain_position = terrain.gameObject.transform.position;

        int hmWidth = data.heightmapWidth;
        int hmHeight = data.heightmapHeight;

        // scale the size in meter to size in resolution
        float scaling_factor = hmWidth / data.size.x;
        int heightMapStartPosX = (int)((damage_position.x - terrain_position.x - craterRadius) * scaling_factor);
        int heightMapStartPosZ = (int)((damage_position.z - terrain_position.z - craterRadius) * scaling_factor);

        // normalize to ensure all the resolution is within the boundary
        int damageSizeX = (int)(craterRadius * 2 * scaling_factor);
        int damageSizeZ = damageSizeX;

        Debug.Log(heightMapStartPosX + "  " + heightMapStartPosZ + " " + damageSizeX + " " + damageSizeZ + " " + hmWidth);

        if (heightMapStartPosX < 0)
        {
            damageSizeX += heightMapStartPosX;
            heightMapStartPosX = 0;
        }
        if (heightMapStartPosZ < 0)
        {
            damageSizeZ += heightMapStartPosZ;
            heightMapStartPosZ = 0;
        }

        if (heightMapStartPosX + damageSizeX >= hmWidth)
        {
            damageSizeX = hmWidth - heightMapStartPosX;

        }
        if (heightMapStartPosZ + damageSizeZ >= hmHeight)
        {
            damageSizeZ = hmHeight - heightMapStartPosZ;
        }



        float[,] heights = terrain.terrainData.GetHeights((int)heightMapStartPosX, (int)heightMapStartPosZ, damageSizeX, damageSizeZ);



        // this creates a spherical damage
        for (int i = 0; i < heights.GetLength(0); i++) //width
        {
            for (int j = 0; j < heights.GetLength(1); j++) //height
            {
                heights[i, j] = Mathf.Clamp(heights[i, j] - 0.1f, 0, 1);
            }

        }
        data.SetHeights((int)heightMapStartPosX, (int)heightMapStartPosZ, heights);
        terrain.Flush();
        Debug.Log("fushed succesfully");
    }
}
