using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Improbable.Entity.Physical;
using Improbable.Unity.Visualizer;
using Improbable.Unity;
using Improbable.Terrainchunk;
using TerrainGenerator;

public class terrainVisualizer : MonoBehaviour {



    public Texture2D FlatTexture;
    public Texture2D SteepTexture;
    public Material TerrainMaterial;


    private static Dictionary<Vector2, TerrainChunk> generatedChunk;

    private static Queue<Vector2> requestChunkPositionQueue;

    private static int terrain_length = 100;
    private static long seed = 1234567;
    private static TerrainSeedData.TerrainType terrain_type = TerrainSeedData.TerrainType.Perlin;
    private TerrainChunkSettings Settings;
    private NoiseProvider NoiseProvider;

    // Use this for initialization
    void Start () {
        generatedChunk = new Dictionary<Vector2, TerrainChunk>();
        requestChunkPositionQueue = new Queue<Vector2>();
        Settings = new TerrainChunkSettings(terrain_length, FlatTexture, SteepTexture, TerrainMaterial);             // create a setting variable
        NoiseProvider = new NoiseProvider(seed, terrain_type);      // create a noise provider variable, the noise correspond to the noise generator used to generate the terrain height

        for (int i = -200; i <= 200; i += terrain_length)
        {
            for(int j = -200; j <= 200; j += terrain_length)
            {
                Debug.Log("generate terrain at position " + i + " " + j);
                StartCoroutine(generateTerrainAtPositionThread(i, j));
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        var queue = new Queue<Vector2>();
        foreach (var position in requestChunkPositionQueue)
        {
            queue.Enqueue(position);
        }
        requestChunkPositionQueue = new Queue<Vector2>();

	    foreach(var position in queue)
        {
            if (isNewPosition(position))
            {
                Debug.Log(position + "is a new position");
                StartCoroutine(generateTerrainAtPositionThread(position));
            }
            else
            {
                Debug.Log("is not a new terrain position");
            }
        }
	}

    private static bool isNewPosition(Vector2 position)
    {
        return !generatedChunk.ContainsKey(position);
    }

    public static void requestChunkPosition(Vector2 position)
    {
        if (isNewPosition(position))
        {
            queueNewRequest(position);
        }

    }

    private static void queueNewRequest(Vector2 position)
    {
        requestChunkPositionQueue.Enqueue(position);
    }

    private IEnumerator generateTerrainAtPositionThread(Vector2 position)
    {
        Debug.Log("calling generator");
        int x = (int)position.x;
        int z = (int)position.y;
        return generateTerrainAtPositionThread(x, z);
    }


    private IEnumerator generateTerrainAtPositionThread(int x, int z)
    {
        TerrainChunk new_chunk = new TerrainChunk(Settings, NoiseProvider, x, z);                                  // create a terrain chunk value
        generatedChunk.Add(new Vector2(x, z), new_chunk);

        new_chunk.GenerateHeightmap();                                                                                             // generate the height map


        // busy wait until the terrains around the current position is properly done
        var height_map_ready = false;
        do
        {
            if (new_chunk.IsHeightmapReady())
                height_map_ready = true;
            yield return null;
        } while (!height_map_ready);


        Terrain terrain = new_chunk.CreateTerrain();                                                                               // use the generated height map to create a terrain
                                                                                                                                   // new_terrain.transform.parent = gameObject.transform;
                                                                                                                                   // ApplyDamage();
                                                                                                                                   // setNeighbours();
        SetChunkNeighborhood(new_chunk);

    }

    private static void SetChunkNeighborhood(TerrainChunk chunk)
    {
        TerrainChunk xUp;
        TerrainChunk xDown;
        TerrainChunk zUp;
        TerrainChunk zDown;
        generatedChunk.TryGetValue(new Vector2(chunk.Position.X + 100, chunk.Position.Z), out xUp);
        generatedChunk.TryGetValue(new Vector2(chunk.Position.X - 100, chunk.Position.Z), out xDown);
        generatedChunk.TryGetValue(new Vector2(chunk.Position.X, chunk.Position.Z + 100), out zUp);
        generatedChunk.TryGetValue(new Vector2(chunk.Position.X, chunk.Position.Z - 100), out zDown);

        if (xUp != null)
        {
            chunk.SetNeighbors(xUp, TerrainNeighbor.XUp);
            xUp.SetNeighbors(chunk, TerrainNeighbor.XDown);
        }
        if (xDown != null)
        {
            chunk.SetNeighbors(xDown, TerrainNeighbor.XDown);
            xDown.SetNeighbors(chunk, TerrainNeighbor.XUp);
        }
        if (zUp != null)
        {
            chunk.SetNeighbors(zUp, TerrainNeighbor.ZUp);
            zUp.SetNeighbors(chunk, TerrainNeighbor.ZDown);
        }
        if (zDown != null)
        {
            chunk.SetNeighbors(zDown, TerrainNeighbor.ZDown);
            zDown.SetNeighbors(chunk, TerrainNeighbor.ZUp);
        }

    }

}
