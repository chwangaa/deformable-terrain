using Improbable.Unity.Util;
using UnityEngine;

namespace Improbable.Unity.EditorTools.WorldExportWizard.ObjExporter.ColliderExporter
{
    internal class BoxColliderToObj
    {
        public static void Export(MeshExporter meshExporter, BoxCollider collider)
        {
            var tempPrimitiveBox = GameObject.CreatePrimitive(PrimitiveType.Cube);

            tempPrimitiveBox.transform.position = collider.transform.TransformPoint(collider.center);
            tempPrimitiveBox.transform.rotation = collider.transform.rotation;
            tempPrimitiveBox.transform.localScale = UnityVector3Utils.Abs(Vector3.Scale(collider.transform.lossyScale, collider.size));

            var meshFilter = tempPrimitiveBox.GetComponent<MeshFilter>();

            meshExporter.AppendToCurrentObj(meshFilter.sharedMesh, tempPrimitiveBox.transform);

            Object.DestroyImmediate(tempPrimitiveBox);
        }
    }
}