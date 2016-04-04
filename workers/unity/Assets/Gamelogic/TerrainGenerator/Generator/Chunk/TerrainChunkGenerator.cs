using UnityEngine;
using Improbable.Entity.Physical;
using Improbable.Unity.Visualizer;
using Improbable.Unity;
using Improbable.Terrainchunk;
using System.Collections;

namespace TerrainGenerator
{
    public class TerrainChunkGenerator : MonoBehaviour
    {
        public Material TerrainMaterial;
        [Require]
        protected PositionReader Position;

        [Require]
        protected TerrainDamageReader damages_reader;

        [Require]
        protected DamageRequestReader damage_request_reader;

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
            damage_request_reader.DamageRequested += HandleDamageRequested;         // add the damage request handler
            transform.position = Position.Value.ToUnityVector();                    // update the position
            generateNewTerrain();                                                   // generate terrains in the neighbourhood
        }

        void HandleDamageRequested(DamageRequestEvent obj){


            
            Damage damage = obj.Damage;
            Vector3 center = damage.Position.ToUnityVector();

            Instantiate(Resources.Load("EntityPrefabs/Explosion"), center, Quaternion.identity);

            int radius = damage.Radius;
            applyDamageToHeightMap(center, radius);
            updateNeighbour();
            terrain.Flush();
        }

        private void generateNewTerrain()
        {

            StartCoroutine(createTerrainCoroutine());
        }

        private IEnumerator createTerrainCoroutine()
        {
            // get the position
            transform.position = Position.Value.ToUnityVector();
            x = (int)gameObject.transform.position.x;
            z = (int)gameObject.transform.position.z;

            seed = terrain_seed_reader.Seed;                                                                                       // get the random seed value
            terrain_length = terrain_seed_reader.TerrainLength;                                                                    // get the terrain size

            var terrain_type = terrain_seed_reader.Nature;


            Settings = new TerrainChunkSettings(terrain_length, FlatTexture, SteepTexture, TerrainMaterial);             // create a setting variable
            NoiseProvider = new NoiseProvider(seed, terrain_type);      // create a noise provider variable, the noise correspond to the noise generator used to generate the terrain height
            TerrainChunk new_chunk = new TerrainChunk(Settings, NoiseProvider, x, z);                                  // create a terrain chunk value
            new_chunk.GenerateHeightmap();                                                                                             // generate the height map


            // busy wait until the terrains around the current position is properly done
            var height_map_ready = false;
            do
            {
                if (new_chunk.IsHeightmapReady())
                    height_map_ready = true;
                yield return null;
            } while (!height_map_ready);


            terrain = new_chunk.CreateTerrain();                                                                               // use the generated height map to create a terrain
            // new_terrain.transform.parent = gameObject.transform;
            ApplyDamage();
            setNeighbours();
            terrain.Flush();        // Flushing is required for the effect to take place
        }
        
        private void ApplyDamage()
        {
            var damages = damages_reader.Damages;
            Debug.Log("there are " + damages.Count + " damages checked out");
            foreach (var damage in damages)
            {                
                Vector3 center = damage.Position.ToUnityVector();
                int radius = damage.Radius;
                applyDamageToHeightMap(center, radius);       
            }
        }


        private void OnDisable()
        {
            if (terrain)
            {
                Destroy(terrain.gameObject);
                terrain = null;
            }
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
            if(i > 0)   // if one or more neighbours get updated, do flush
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
            if(terrain != null)
            {
                terrain.SetNeighbors(left_neighbor, top_neighbor, right_neighbor, bottom_neighbor);
                terrain.Flush();
            }
        }

        private void onDestroy()
        {
            if(terrain != null)
                Destroy(terrain.gameObject);
            Destroy(gameObject);
        }

        public void applyDamageToHeightMap(Vector3 damage_position, float craterRadius)
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
            if(heightMapStartPosZ < 0)
            {
                damageSizeZ += heightMapStartPosZ;
                heightMapStartPosZ = 0;
            }

            if(heightMapStartPosX + damageSizeX >= hmWidth)
            {
                damageSizeX = hmWidth - heightMapStartPosX;

            }
            if(heightMapStartPosZ + damageSizeZ >= hmHeight)
            {
                damageSizeZ = hmHeight - heightMapStartPosZ;
            }


            // Debug.Log("check out the area at " + heightMapStartPosX + " " + heightMapStartPosZ + "area of " + damageSizeX + " " + damageSizeZ + " starting at " + terrain_position + " damage center " + damage_position);
            // Debug.Log(hmWidth + " " + hmHeight);
            float[,] heights = terrain.terrainData.GetHeights((int)heightMapStartPosX, (int)heightMapStartPosZ, damageSizeX, damageSizeZ);

            // Debug.Log(heights.GetLength(0));
            // Debug.Log(heights.GetLength(1));

            // this creates a spherical damage
            for (int i = 0; i < heights.GetLength(0); i++) //width
            {
                for (int j = 0; j < heights.GetLength(1); j++) //height
                {
                    heights[i, j] = Mathf.Clamp(heights[i, j] - 0.1f, 0, 1);
                }

            }
            data.SetHeights((int)heightMapStartPosX, (int)heightMapStartPosZ, heights);
        }
    }



}