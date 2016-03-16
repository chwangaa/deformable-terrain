using Improbable.Unity.Util;
using UnityEngine;

namespace Improbable.Unity.EditorTools.WorldExportWizard.ObjExporter.ColliderExporter
{
    internal class MeshColliderToObj
    {
        public static void Export(MeshExporter meshExporter, MeshCollider collider)
        {
            var tempGameObject = new GameObject();

            tempGameObject.transform.position = collider.transform.position;
            tempGameObject.transform.rotation = collider.transform.rotation;
            tempGameObject.transform.localScale = UnityVector3Utils.Abs(collider.transform.lossyScale);

            var meshFilter = collider.sharedMesh;

            meshExporter.AppendToCurrentObj(meshFilter, tempGameObject.transform);

            Object.DestroyImmediate(tempGameObject);
        }
    }
}