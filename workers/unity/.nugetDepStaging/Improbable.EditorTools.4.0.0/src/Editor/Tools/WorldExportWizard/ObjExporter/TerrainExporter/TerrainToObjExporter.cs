using UnityEngine;

namespace Improbable.Unity.EditorTools.WorldExportWizard.ObjExporter.TerrainExporter
{
    internal class TerrainToObjExporter
    {
        public static void Export(MeshExporter meshExporter, Terrain terrain, TerrainSaveResolution saveResolution)
        {
            var mesh = TerrainToMeshExporter.Export(terrain, saveResolution, Quaternion.identity);
            meshExporter.AppendToCurrentObj(mesh);
        }
    }
}