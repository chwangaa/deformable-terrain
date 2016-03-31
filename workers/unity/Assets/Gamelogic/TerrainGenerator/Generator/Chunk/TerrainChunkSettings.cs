using UnityEngine;

namespace TerrainGenerator
{
    public class TerrainChunkSettings
    {
        public int HeightmapResolution { get; private set; }

        public int AlphamapResolution { get; private set; }

        public int Length { get; private set; }

        public int Height { get; private set; }

        public Texture2D FlatTexture { get; private set; }

        public Texture2D SteepTexture { get; private set; }

        public Material TerrainMaterial { get; private set; }

        public TerrainChunkSettings(int length, Texture2D flatTexture, Texture2D steepTexture, Material terrainMaterial)
        {
            HeightmapResolution = 129;
            AlphamapResolution = 129;
            Length = length;
            Height = 30;
            FlatTexture = flatTexture;
            SteepTexture = steepTexture;
            TerrainMaterial = terrainMaterial;
        }
    }
}