using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Improbable.Entity.Physical;
using Improbable.Unity.Visualizer;
using Improbable.Unity;
using Improbable.Terrainchunk;
using System.Linq;

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

        private Terrain terrain;

        [Require]
        protected TerrainseedReader terrain_seed_reader;

        private void OnEnable()
        {

            transform.position = Position.Value.ToUnityVector();


            var x = (int)gameObject.transform.position.x;
            var z = (int)gameObject.transform.position.z;
            var seed = terrain_seed_reader.Seed;
            var terrain_length = terrain_seed_reader.TerrainLength;
            Settings = new TerrainChunkSettings(129, 129, terrain_length, 40, FlatTexture, SteepTexture, TerrainMaterial);
            NoiseProvider = new NoiseProvider(seed);

            TerrainChunk new_chunk = new TerrainChunk(terrain_length, Settings, NoiseProvider, x, z);
            new_chunk.GenerateHeightmap();
            while (!new_chunk.IsHeightmapReady())
            {
                // Debug.Log("height map not yet ready, busy wait");
            }

            var new_terrain = new_chunk.CreateTerrain();
            // new_terrain.transform.parent = gameObject.transform;
            Debug.Log("generated terrain at position " + gameObject.transform.position + "with seed valued " + seed.ToString());
            terrain = new_terrain;
        }

        private void OnDisable()
        {
            Destroy(terrain.gameObject);
            Debug.Log("destroy the terrain because the entity is disabled");
        }
    }
}