using Improbable.Unity.EditorTools.Util;
using Improbable.Unity.EditorTools.WorldExportWizard.ObjExporter.ColliderExporter;
using Improbable.Unity.EditorTools.WorldExportWizard.ObjExporter.TerrainExporter;
using UnityEngine;

namespace Improbable.Unity.EditorTools.WorldExportWizard.ObjExporter
{
    internal class WorldObjExporter
    {
        public static void Export(string worldName, TerrainSaveResolution terrainSaveResolution)
        {
            var meshExporter = new MeshExporter();

            foreach (var collider in Object.FindObjectsOfType<BoxCollider>())
            {
                BoxColliderToObj.Export(meshExporter, collider);
            }

            foreach (var collider in Object.FindObjectsOfType<MeshCollider>())
            {
                MeshColliderToObj.Export(meshExporter, collider);
            }

            foreach (var terrain in Object.FindObjectsOfType<Terrain>())
            {
                TerrainToObjExporter.Export(meshExporter, terrain, terrainSaveResolution);
            }

            meshExporter.WriteObjToFile(EditorPaths.ObjExportPath(worldName));
        }
    }
}