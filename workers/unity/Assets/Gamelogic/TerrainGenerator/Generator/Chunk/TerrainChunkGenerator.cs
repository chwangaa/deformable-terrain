using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Improbable.Entity.Physical;
using Improbable.Unity.Visualizer;
using Improbable.Unity;
using Improbable.Terrainchunk;

namespace TerrainGenerator
{
    public class TerrainChunkGenerator : MonoBehaviour
    {
        public Material TerrainMaterial;
        [Require]
        protected PositionReader Position;

        public Texture2D FlatTexture;
        public Texture2D SteepTexture;

        private TerrainChunkSettings Settings;
        private NoiseProvider NoiseProvider;

        public Terrain terrain = null;

        private int terrain_length;
        private long seed;
        private int x;
        private int z;

        private Terrain left_neighbor = null;
        private Terrain right_neighbor = null;
        private Terrain bottom_neighbor = null;
        private Terrain top_neighbor = null;

        [Require]
        protected TerrainseedReader terrain_seed_reader;

        private void OnEnable()
        {
                generateNewTerrain();
        }

        private void generateNewTerrain()
        {
            // get the position
            transform.position = Position.Value.ToUnityVector();
            x = (int)gameObject.transform.position.x;
            z = (int)gameObject.transform.position.z;

            seed = terrain_seed_reader.Seed;                                                                                       // get the random seed value
            terrain_length = terrain_seed_reader.TerrainLength;                                                                    // get the terrain size
            

            Settings = new TerrainChunkSettings(129, 129, terrain_length, 40, FlatTexture, SteepTexture, TerrainMaterial);             // create a setting variable
            NoiseProvider = new NoiseProvider(seed);                                                                                   // create a noise provider variable
            TerrainChunk new_chunk = new TerrainChunk(terrain_length, Settings, NoiseProvider, x, z);                                  // create a terrain chunk value
            new_chunk.GenerateHeightmap();                                                                                             // generate the height map
            // busy wait until the height map is generated
            while (!new_chunk.IsHeightmapReady())
            {
            }
            var new_terrain = new_chunk.CreateTerrain();                                                                               // use the generated height map to create a terrain
            // new_terrain.transform.parent = gameObject.transform;
            Debug.Log("generated terrain at position " + gameObject.transform.position + "with seed valued " + seed.ToString());
            terrain = new_terrain;              // hold the terrain as a class variable
            setNeighbours();
        }

        private void OnDisable()
        {
            if(terrain.gameObject != null)
                Destroy(terrain.gameObject);
        }

        private void setNeighbours()
        {
            float radius = terrain_length * 1.1f;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
            int i = 0;
            while (i < hitColliders.Length)
            {
                TerrainChunkGenerator terrain_generator = (TerrainChunkGenerator)hitColliders[i].gameObject.GetComponent("TerrainChunkGenerator");
                if (terrain_generator != null)
                    setNeighbour(terrain_generator);
                i++;
            }
            if(hitColliders.Length == 0)
            {
                Debug.Log("no neighbours found");
            }
            else
            {
                updateNeighbour();
            }
        }

        private void setNeighbour(TerrainChunkGenerator neighbor)
        {
            int neighbor_x = (int)neighbor.transform.position.x;
            int neighbor_z = (int)neighbor.transform.position.z;
            Terrain neighbor_terrain = neighbor.terrain;
            if (neighbor_x == x - 100 && neighbor_z == z)
            {
                left_neighbor = neighbor_terrain;
                neighbor.right_neighbor = terrain;
            }
            else if (neighbor_x == x + 100 && neighbor_z == z)
            {
                right_neighbor = neighbor_terrain;
                neighbor.left_neighbor = terrain;

            }
            else if (neighbor_z == z - 100 && neighbor_x == x)
            {
                bottom_neighbor = neighbor_terrain;
                neighbor.top_neighbor = terrain;

            }
            else if (neighbor_z == z + 100 && neighbor_x == x)
            {
                top_neighbor = neighbor_terrain;
                neighbor.bottom_neighbor = terrain;
            }
            else {
                return; // this means no neighbour has been reset, return immediately
            }
            neighbor.updateNeighbour();     // call neighbour's updateNeighbour function
        }



        public void updateNeighbour()
        {
            Debug.Log("update the neighbours");
            terrain.SetNeighbors(left_neighbor, top_neighbor, right_neighbor, bottom_neighbor);
            terrain.Flush();
        }

        private void onDestroy()
        {
            Debug.Log("on destroy called ");
            Destroy(terrain.gameObject);
            Destroy(gameObject);
        }
    }
}