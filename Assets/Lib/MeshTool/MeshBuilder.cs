using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace UnityLib.MeshTool
{
    public class MeshBuilder
    {
        List<CombineInstance> combines;

        public MeshBuilder()
        {
            combines = new List<CombineInstance>();
        }

        public void AddMesh(Mesh mesh, Matrix4x4 transformMatrix)
        {
            var combine = new CombineInstance { mesh = mesh, transform = transformMatrix };
            combines.Add(combine);
        }

        public Mesh Build()
        {
            var mesh = new Mesh();
            mesh.CombineMeshes(combines.ToArray());
            return mesh;
        }

        public void Clear()
        {
            combines.Clear();
        }
    }
}