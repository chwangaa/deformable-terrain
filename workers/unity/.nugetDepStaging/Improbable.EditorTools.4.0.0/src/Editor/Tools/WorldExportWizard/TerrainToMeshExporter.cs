using Improbable.Unity.EditorTools.WorldExportWizard.ObjExporter.TerrainExporter;
using UnityEngine;

namespace Improbable.Unity.EditorTools.WorldExportWizard
{
    internal class TerrainToMeshExporter
    {
        public static Mesh Export(Terrain terrain, TerrainSaveResolution saveResolution, Quaternion rotation)
        {
            var heightmapWidth = terrain.terrainData.heightmapWidth;
            var heightmapHeight = terrain.terrainData.heightmapHeight;
            var terrainResolution = (int) Mathf.Pow(2, (int) saveResolution);
            var numWidthSamples = (heightmapWidth - 1) / terrainResolution + 1;
            var numHeightSamples = (heightmapHeight - 1) / terrainResolution + 1;
            var meshScale = GetMeshScale(terrain.terrainData.size, terrainResolution, heightmapWidth, heightmapHeight);
            var heightMapSamples = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight);

            //Warn: Unity meshes have a maximum size of 64k vertices. 
            var mesh = new Mesh
            {
                name = terrain.name,
                vertices = BuildTerrainVertices(numWidthSamples, numHeightSamples, meshScale, heightMapSamples, terrainResolution, terrain.transform.position, rotation),
                triangles = BuildTerrainTriangles(numWidthSamples, numHeightSamples)
            };

            return mesh;
        }

        private static Vector3[] BuildTerrainVertices(int numWidthSamples, int numHeightSamples, Vector3 meshScale, float[,] heightMapSamples, int terrainResolution, Vector3 terrainPosition, Quaternion rotation)
        {
            var vertices = new Vector3[numWidthSamples * numHeightSamples];

            for (var x = 0; x < numWidthSamples; x++)
            {
                for (var y = 0; y < numHeightSamples; y++)
                {
                    var basePosition = Vector3.Scale(meshScale, new Vector3(x, heightMapSamples[y * terrainResolution, x * terrainResolution], y));
                    basePosition = basePosition + terrainPosition;
                    basePosition = rotation * basePosition;
                    vertices[y * numWidthSamples + x] = basePosition;
                }
            }

            return vertices;
        }

        private static int[] BuildTerrainTriangles(int numWidthSamples, int numHeightSamples)
        {
            var index = 0;
            var triangles = new int[(numWidthSamples - 1) * (numHeightSamples - 1) * 6];

            for (var x = 0; x < numWidthSamples - 1; x++)
            {
                for (var y = 0; y < numHeightSamples - 1; y++)
                {
                    var topLeftCoordinate = (y * numWidthSamples) + x;
                    var bottomLeftCoordinate = ((y + 1) * numWidthSamples) + x;

                    triangles[index++] = topLeftCoordinate;
                    triangles[index++] = bottomLeftCoordinate;
                    triangles[index++] = topLeftCoordinate + 1;

                    triangles[index++] = bottomLeftCoordinate;
                    triangles[index++] = bottomLeftCoordinate + 1;
                    triangles[index++] = topLeftCoordinate + 1;
                }
            }

            return triangles;
        }

        private static Vector3 GetMeshScale(Vector3 terrainSize, int terrainResolution, int heightmapWidth, int heightmapHeight)
        {
            return new Vector3(terrainSize.x / (heightmapWidth - 1) * terrainResolution, terrainSize.y, terrainSize.z / (heightmapHeight - 1) * terrainResolution);
        }
    }
}