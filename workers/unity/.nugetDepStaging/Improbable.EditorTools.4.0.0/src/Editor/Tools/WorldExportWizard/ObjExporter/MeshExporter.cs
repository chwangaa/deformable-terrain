using System.IO;
using System.Text;
using UnityEngine;

namespace Improbable.Unity.EditorTools.WorldExportWizard.ObjExporter
{
    /// <summary>
    ///     There is an assumed 1 to 1 mapping between vertices and normals.
    /// </summary>
    internal class MeshExporter
    {
        private readonly StringBuilder objBuilder = new StringBuilder();
        private int vertexOffset = 1;

        public void AppendToCurrentObj(Mesh mesh)
        {
            AppendToCurrentObj(mesh, null);
        }

        public void AppendToCurrentObj(Mesh mesh, Transform transform)
        {
            var meshName = mesh.name;

            StartObject(meshName);
            WriteVertices(mesh, transform);
            WriteNormals(mesh, transform);
            WriteFaces(mesh);
            vertexOffset += mesh.vertices.Length;
        }

        public void WriteObjToFile(string filename)
        {
            File.WriteAllText(filename, objBuilder.ToString());
            Reset();
        }

        private void StartObject(string meshName)
        {
            objBuilder.AppendFormat("o {0} \n", meshName);
        }

        private void WriteVertices(Mesh mesh, Transform transform)
        {
            foreach (var vertex in mesh.vertices)
            {
                var transformedVert = vertex;
                if (transform != null)
                {
                    transformedVert = transform.TransformPoint(vertex);
                }
                WriteVertex(transformedVert);
            }
            objBuilder.Append("\n");
        }

        private void WriteNormals(Mesh mesh, Transform transform)
        {
            foreach (var normal in mesh.normals)
            {
                var transformedNormal = normal;
                if (transform != null)
                {
                    transformedNormal = transform.TransformDirection(normal);
                }
                WriteNormal(transformedNormal);
            }
            objBuilder.Append("\n");
        }

        private void WriteFaces(Mesh mesh)
        {
            for (var material = 0; material < mesh.subMeshCount; material++)
            {
                var triangles = mesh.GetTriangles(material);

                for (var i = 0; i < triangles.Length; i += 3)
                {
                    var vert1 = triangles[i];
                    var vert2 = triangles[i + 1];
                    var vert3 = triangles[i + 2];

                    WriteFace(vert1, vert2, vert3);
                }
                objBuilder.Append("\n");
            }
            objBuilder.Append("\n");
        }

        /// <summary>
        ///     We negate the x-component to change the "handedness" of the vectors.
        /// </summary>
        private void WriteVertex(Vector3 vertex)
        {
            objBuilder.AppendFormat("v {0} {1} {2} \n", -vertex.x, vertex.y, vertex.z);
        }

        /// <summary>
        ///     We negate the x-component to changed the "handedness" of the vectors.
        /// </summary>
        private void WriteNormal(Vector3 normal)
        {
            objBuilder.AppendFormat("vn {0} {1} {2} \n", -normal.x, normal.y, normal.z);
        }

        /// <summary>
        ///     We reverse the triangle winding due to change in "handedness" of the vectors.
        /// </summary>
        private void WriteFace(int vertIndex1, int vertIndex2, int vertIndex3)
        {
            objBuilder.AppendFormat("f {1}//{1} {0}//{0} {2}//{2} \n", vertIndex1 + vertexOffset, vertIndex2 + vertexOffset, vertIndex3 + vertexOffset);
        }

        private void Reset()
        {
            vertexOffset = 1;
            objBuilder.Length = 0;
        }
    }
}