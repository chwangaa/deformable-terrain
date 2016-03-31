using LibNoise.Generator;
using LibNoise;
using Improbable.Terrainchunk;

namespace TerrainGenerator
{

    public class NoiseProvider : INoiseProvider
    {
        private ModuleBase NoiseGenerator;

        public NoiseProvider(long seed, TerrainSeedData.TerrainType type)
        {
            switch(type){
                case TerrainSeedData.TerrainType.Constant:
                    NoiseGenerator = new Const(20);
                    break;
                case TerrainSeedData.TerrainType.Billow:
                    NoiseGenerator = new Billow(seed);
                    break;
                case TerrainSeedData.TerrainType.Voronoi:
                    NoiseGenerator = new Voronoi(seed);
                    break;
                default:
                    NoiseGenerator = new Perlin(seed);
                    break;
            }
        }

        public float GetValue(float x, float z)
        {
            return (float)(NoiseGenerator.GetValue(x, 0, z) / 2f) + 0.5f;
        }
    }
}