using UnityEngine;
using Improbable.Entity.Physical;
using Improbable.Unity.Visualizer;
using Improbable.Unity;
using Improbable.Terrainchunk;
using System.Collections;
using System.Collections.Generic;

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

        public List<TerrainDamage> damages = new List<TerrainDamage>();

        [Require]
        protected TerrainseedReader terrain_seed_reader;

        private void OnEnable()
        {
            transform.position = Position.Value.ToUnityVector();
            assembleDamages();
            generateNewTerrain();
            Debug.Log("terrain is already available");
            
        }
        
        private void assembleDamages()
        {
            float radius = terrain_length / 2;
            var terrain_center = transform.position + new Vector3(radius, 0, radius);
            Collider[] hitColliders = Physics.OverlapSphere(terrain_center, radius * 1.5f);
            int i = 0;
            Debug.Log("assemblying the demages");
            while (i < hitColliders.Length)
            {
                TerrainDamage terrain_damage = (TerrainDamage)hitColliders[i].gameObject.GetComponent("TerrainDamage");
                if (terrain_damage != null)
                {
                    damages.Add(terrain_damage);
                    Debug.Log("a damage is found");
                }
                i++;
            }
            if (hitColliders.Length == 0)
            {
                Debug.Log("no damages found");
            }
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


            Settings = new TerrainChunkSettings(129, 129, terrain_length, 40, FlatTexture, SteepTexture, TerrainMaterial);             // create a setting variable
            NoiseProvider = new NoiseProvider(seed);                                                                                   // create a noise provider variable
            TerrainChunk new_chunk = new TerrainChunk(terrain_length, Settings, NoiseProvider, x, z);                                  // create a terrain chunk value
            new_chunk.GenerateHeightmap();                                                                                             // generate the height map


            // busy wait until the terrains around the current position is properly done
            var height_map_ready = false;
            do
            {
                if (new_chunk.IsHeightmapReady())
                    height_map_ready = true;
                yield return null;
            } while (!height_map_ready);

            var new_terrain = new_chunk.CreateTerrain();                                                                               // use the generated height map to create a terrain
            // new_terrain.transform.parent = gameObject.transform;
            // Debug.Log("generated terrain at position " + gameObject.transform.position + "with seed valued " + seed.ToString());
            terrain = new_terrain;              // hold the terrain as a class variable
            ApplyDamage();
            setNeighbours();
        }
        
        private void ApplyDamage()
        {
            Debug.Log("applying damages");
            foreach(var damage in damages)
            {
                damage.applyDamageToHeightMap(terrain);
            }
            if(damages.Count != 0)
            {
                Debug.Log("damages applied");
            }
            terrain.Flush();
        }

        public void addNewDamage(TerrainDamage damage)
        {
            Debug.Log("attempt to add new damage to the terrain object");
            damages.Add(damage);
            damage.applyDamageToHeightMap(terrain);
            terrain.Flush();
        }
        

        private void OnDisable()
        {
                Destroy(terrain.gameObject);
                terrain = null;
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
            if(terrain == null)
            {
                Debug.Log("error, terrain is null");
            }
            else
            {
                terrain.SetNeighbors(left_neighbor, top_neighbor, right_neighbor, bottom_neighbor);
                terrain.Flush();
            }
        }

        private void onDestroy()
        {
            Debug.Log("on destroy called ");
            if(terrain != null)
                Destroy(terrain.gameObject);
            Destroy(gameObject);
        }
    }
}