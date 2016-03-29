using UnityEngine;
using Improbable.Entity.Physical;
using Improbable.Unity.Visualizer;
using Improbable.Unity;
using TerrainGenerator;
using Improbable.Damage;

public class createTerrainDamage : MonoBehaviour
{

    [Require]
    protected PositionReader Position;

    [Require]
    protected DamageStateReader DamageStateReader;

    // Use this for initialization
    void Start()
    {
    }

    public Vector3 center;
    protected int hmWidth; // heightmap width
    protected int hmHeight; // heightmap height
    protected const float DEPTH_METER_CONVERT = 0.05f;
    protected const float TEXTURE_SIZE_MULTIPLIER = 1.25f;
    private float[,] heightMapBackup;
    private float[,,] alphaMapBackup;
    
    
    void OnEnable()
    {
        transform.position = Position.Value.ToUnityVector();
        
        center = gameObject.transform.position;
        var terrain_center = DamageStateReader.Position.ToUnityVector();
        Collider[] hitColliders = Physics.OverlapSphere(terrain_center, 10);
        Debug.Log("terrain damage created");
        Debug.Log("there are " + hitColliders.Length.ToString() + " object in the nearby region");
        int i = 0;
        
        while (i < hitColliders.Length)
        {
            TerrainChunkGenerator terrain_generator = (TerrainChunkGenerator)hitColliders[i].gameObject.GetComponent("TerrainChunkGenerator");
            Debug.Log("try to grab the terrain generator object");
            if (terrain_generator != null)
            {
                Terrain chunk = terrain_generator.terrain;
                if (chunk != null)
                {
                    Debug.Log("terrain is found");
                    applyDamageToHeightMap(chunk);
                    chunk.Flush();
                }
                else
                {
                    Debug.Log("terrain is not null, error!");
                }
            }
            i++;
        }
        
        
        if (hitColliders.Length == 0)
        {
            Debug.Log("no terrain found, error!");
        }
    }
    

    public void applyDamageToHeightMap(Terrain chunk)
    {
        Debug.Log("applying damage to the height map");
        //get the heights only once keep it and reuse, precalculate as much as possible
        float craterSizeInMeters = 5;
        TerrainData data = chunk.terrainData;

        hmWidth = data.heightmapWidth;
        hmHeight = data.heightmapHeight;
        
        // Vector3 terrainPos = GetRelativeTerrainPositionFromPos(center, chunk, hmWidth, hmHeight);
        int heightMapCraterWidth = (int)(craterSizeInMeters * (hmWidth / data.size.x));
        int heightMapCraterLength = (int)(craterSizeInMeters * (hmHeight / data.size.z));


        /*
        int heightMapStartPosX = (int)(terrainPos.x - (heightMapCraterWidth / 2));
        int heightMapStartPosZ = (int)(terrainPos.z - (heightMapCraterLength / 2));
        */
        Debug.Log("the center of the explosition is " + center.ToString());
        Debug.Log("the terrain is at the position " + chunk.gameObject.transform.position.ToString());
        Vector3 position_offset = center - chunk.gameObject.transform.position;
        Debug.Log("the position offset is " + position_offset.ToString());

        int heightMapStartPosX = (int)position_offset.x;
        int heightMapStartPosZ = (int)position_offset.z;
        // make sure the boundary condition is not violated
        heightMapCraterLength = (heightMapCraterLength + heightMapStartPosZ < 100) ? heightMapCraterLength : 100 - heightMapStartPosZ;
        heightMapCraterWidth = (heightMapCraterWidth + heightMapStartPosX < 100) ? heightMapCraterWidth : 100 - heightMapStartPosX;
        
        var s = string.Format("the positions are {0}, {1}, {2}, {3}", heightMapStartPosX, heightMapStartPosZ, heightMapCraterWidth, heightMapCraterLength);
        Debug.Log(s);
        float[,] heights = data.GetHeights(heightMapStartPosX, heightMapStartPosZ, heightMapCraterWidth, heightMapCraterLength);
        float circlePosX;
        float circlePosY;
        float distanceFromCenter;
        float depthMultiplier;

        float deformationDepth = (craterSizeInMeters / 3.0f);

        // we set each sample of the terrain in the size to the desired height
        for (int i = 0; i < heightMapCraterLength; i++) //width
        {
            for (int j = 0; j < heightMapCraterWidth; j++) //height
            {
                circlePosX = (j - (heightMapCraterWidth / 2)) / (hmWidth / data.size.x);
                circlePosY = (i - (heightMapCraterLength / 2)) / (hmHeight / data.size.z);
                distanceFromCenter = Mathf.Abs(Mathf.Sqrt(circlePosX * circlePosX + circlePosY * circlePosY));
                //convert back to values without skew

                if (distanceFromCenter < (craterSizeInMeters / 2.0f))
                {
                    depthMultiplier = ((craterSizeInMeters / 2.0f - distanceFromCenter) / (craterSizeInMeters / 2.0f));

                    depthMultiplier += 0.1f;
                    depthMultiplier += Random.value * .1f;

                    depthMultiplier = Mathf.Clamp(depthMultiplier, 0, 1);
                    heights[i, j] = Mathf.Clamp(heights[i, j] - deformationDepth * depthMultiplier, 0, 1);
                }

            }
        }

        // set the new height
        Debug.Log("reset height map");
        data.SetHeights(heightMapStartPosX, heightMapStartPosZ, heights);
    }


    protected Vector3 GetNormalizedPositionRelativeToTerrain(Vector3 pos, Terrain terrain)
    {
        //code based on: http://answers.unity3d.com/questions/3633/modifying-terrain-height-under-a-gameobject-at-runtime
        // get the normalized position of this game object relative to the terrain
        Vector3 tempCoord = (pos - terrain.gameObject.transform.position);
        Vector3 coord;
        coord.x = tempCoord.x / terrain.terrainData.size.x;
        coord.y = tempCoord.y / terrain.terrainData.size.y;
        coord.z = tempCoord.z / terrain.terrainData.size.z;

        return coord;
    }

    protected Vector3 GetRelativeTerrainPositionFromPos(Vector3 pos, Terrain terrain, int mapWidth, int mapHeight)
    {
        Vector3 coord = GetNormalizedPositionRelativeToTerrain(pos, terrain);
        // get the position of the terrain heightmap where this game object is
        return new Vector3((coord.x * mapWidth), 0, (coord.z * mapHeight));
    }
}
