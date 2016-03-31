using System.Threading;
using UnityEngine;

namespace TerrainGenerator
{
    public class TerrainChunk
    {
        public Vector2i Position { get; private set; }

        private Terrain terrain { get; set; }

        public TerrainData Data { get; set; }

        private TerrainChunkSettings Settings { get; set; }

        private NoiseProvider NoiseProvider { get; set; }

        private TerrainChunkNeighborhood Neighborhood { get; set; }

        private float[,] Heightmap { get; set; }

        private object HeightmapThreadLockObject { get; set; }


        public TerrainChunk(TerrainChunkSettings settings, NoiseProvider noiseProvider, int x, int z)
        {
            HeightmapThreadLockObject = new object();

            Settings = settings;
            NoiseProvider = noiseProvider;
            Neighborhood = new TerrainChunkNeighborhood();
            Position = new Vector2i(x, z);
        }

        #region Heightmap stuff

        public void GenerateHeightmap()
        {
            var thread = new Thread(GenerateHeightmapThread);
            thread.Start();
        }

        private void GenerateHeightmapThread()
        {
            //int scale = Scale;
            int scale = Settings.Length;
            lock (HeightmapThreadLockObject)
            {
                var heightmap = new float[Settings.HeightmapResolution, Settings.HeightmapResolution];

                for (var zRes = 0; zRes < Settings.HeightmapResolution; zRes++)
                {
                    for (var xRes = 0; xRes < Settings.HeightmapResolution; xRes++)
                    {
                        var xCoordinate = Position.X / scale + (float)xRes / (Settings.HeightmapResolution - 1);
                        var zCoordinate = Position.Z / scale + (float)zRes / (Settings.HeightmapResolution - 1);

                        heightmap[zRes, xRes] = NoiseProvider.GetValue(xCoordinate, zCoordinate);
                    }
                }

                Heightmap = heightmap;
            }
        }

        public bool IsHeightmapReady()
        {
            return terrain == null && Heightmap != null;
        }

        public float GetTerrainHeight(Vector3 worldPosition)
        {
            return terrain.SampleHeight(worldPosition);
        }

        #endregion

        #region Main terrain generation

        public Terrain CreateTerrain()
        {
            Data = new TerrainData();
            Data.heightmapResolution = Settings.HeightmapResolution;
            Data.alphamapResolution = Settings.AlphamapResolution;
            Data.SetHeights(0, 0, Heightmap);
            ApplyTextures(Data);

            Data.size = new Vector3(Settings.Length, Settings.Height, Settings.Length);
            var newTerrainGameObject = Terrain.CreateTerrainGameObject(Data);
            newTerrainGameObject.transform.position = new Vector3(Position.X, 0, Position.Z);

            terrain = newTerrainGameObject.GetComponent<Terrain>();
            terrain.heightmapPixelError = 8;
            terrain.materialType = UnityEngine.Terrain.MaterialType.Custom;
            terrain.materialTemplate = Settings.TerrainMaterial;
            terrain.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            return terrain;
        }

        private void ApplyTextures(TerrainData terrainData)
        {
            var flatSplat = new SplatPrototype();
            var steepSplat = new SplatPrototype();

            flatSplat.texture = Settings.FlatTexture;
            steepSplat.texture = Settings.SteepTexture;

            terrainData.splatPrototypes = new SplatPrototype[]
            {
                flatSplat,
                steepSplat
            };

            terrainData.RefreshPrototypes();

            var splatMap = new float[terrainData.alphamapResolution, terrainData.alphamapResolution, 2];

            for (var zRes = 0; zRes < terrainData.alphamapHeight; zRes++)
            {
                for (var xRes = 0; xRes < terrainData.alphamapWidth; xRes++)
                {
                    var normalizedX = (float)xRes / (terrainData.alphamapWidth - 1);
                    var normalizedZ = (float)zRes / (terrainData.alphamapHeight - 1);

                    var steepness = terrainData.GetSteepness(normalizedX, normalizedZ);
                    var steepnessNormalized = Mathf.Clamp(steepness / 1.5f, 0, 1f);

                    splatMap[zRes, xRes, 0] = 1f - steepnessNormalized;
                    splatMap[zRes, xRes, 1] = steepnessNormalized;
                }
            }

            terrainData.SetAlphamaps(0, 0, splatMap);
        }


        #endregion
    }
}