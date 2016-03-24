using UnityEngine;
using TerrainGenerator;
using Improbable.Entity.Physical;
using Improbable.Unity.Visualizer;
using Improbable.Unity;

public class TerrainDamage : MonoBehaviour {

    [Require]
    protected PositionReader Position;


    // Use this for initialization
    void Start () {
    }

    void OnEnable()
    {
        transform.position = Position.Value.ToUnityVector();

    }

    public Vector3 center;
    protected int hmWidth; // heightmap width
    protected int hmHeight; // heightmap height
    protected int alphaMapWidth;
    protected int alphaMapHeight;
    protected int numOfAlphaLayers;
    protected const float DEPTH_METER_CONVERT = 0.05f;
    protected const float TEXTURE_SIZE_MULTIPLIER = 1.25f;
    private float[,] heightMapBackup;
    private float[,,] alphaMapBackup;

    void OnCollisionEnter(UnityEngine.Collision other)
    {
        Debug.Log("on collision enter");
        Debug.Log("trigger damage upon collision");
        center = gameObject.transform.position;

        Terrain terrain_generator = (Terrain)other.gameObject.GetComponent("Terrain");
        if (terrain_generator != null)
        {
            Debug.Log("the collided object is a terrain");
            // terrain_generator.addNewDamage(this);
            this.gameObject.SetActive(false);
            applyDamageToHeightMap(terrain_generator);
            terrain_generator.Flush();
        }
        Destroy(this.gameObject);
    }

    public void applyDamageToHeightMap(Terrain chunk)
    {
        Debug.Log("applying damage to the height map");
        //get the heights only once keep it and reuse, precalculate as much as possible
        float craterSizeInMeters = 20;
        TerrainData data = chunk.terrainData;

        hmWidth = data.heightmapWidth;
        hmHeight = data.heightmapHeight;
        alphaMapWidth = data.alphamapWidth;
        alphaMapHeight = data.alphamapHeight;
        numOfAlphaLayers = data.alphamapLayers;

        Vector3 terrainPos = GetRelativeTerrainPositionFromPos(center, chunk, hmWidth, hmHeight);//terr.terrainData.heightmapResolution/terr.terrainData.heightmapWidth
        int heightMapCraterWidth = (int)(craterSizeInMeters * (hmWidth / data.size.x));
        int heightMapCraterLength = (int)(craterSizeInMeters * (hmHeight / data.size.z));
        int heightMapStartPosX = (int)(terrainPos.x - (heightMapCraterWidth / 2));
        int heightMapStartPosZ = (int)(terrainPos.z - (heightMapCraterLength / 2));


        float[,] heights = chunk.terrainData.GetHeights(heightMapStartPosX, heightMapStartPosZ, heightMapCraterWidth, heightMapCraterLength);
        float circlePosX;
        float circlePosY;
        float distanceFromCenter;
        float depthMultiplier;

        float deformationDepth = (craterSizeInMeters / 3.0f) / data.size.y;

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
