using LibNoise.Generator;

namespace TerrainGenerator
{
    public class NoiseProvider : INoiseProvider
    {
        private Perlin PerlinNoiseGenerator;

        public NoiseProvider(long seed)
        {
            PerlinNoiseGenerator = new Perlin(seed);
        }

        public float GetValue(float x, float z)
        {
            return (float)(PerlinNoiseGenerator.GetValue(x, 0, z) / 2f) + 0.5f;
        }
    }
}