using UnityEngine;
using System.Collections.Generic;
using UnityLib.Procedural;

namespace UnityLib.MeshTool
{
    public class BoxDrawer : SingletonMonoManager<BoxDrawer>
    {
        Mesh mesh;
        Queue<Data> drawQueue;
        Material material;

        void Awake()
        {
            mesh = ProcedualPrimitives.CreateCubeMesh();
            drawQueue = new Queue<Data>();
            material = new Material(Shader.Find("Hidden/DebugDraw"));
        }

        public static void Draw(Vector3 center, Vector3 halfSize, Quaternion rotation, Color color)
        {
            var matrix = Matrix4x4.TRS(center, rotation, halfSize);
            Instance.drawQueue.Enqueue(new Data { matrix = matrix, color = color });
        }

        void LateUpdate()
        {
            while (drawQueue.Count > 0)
            {
                var data = drawQueue.Dequeue();
                material.SetColor("_Color", data.color);
                //            material.SetPass(0);
                Graphics.DrawMesh(mesh, data.matrix, material, 0);
            }
        }

        class Data
        {
            public Matrix4x4 matrix;
            public Color color;
        }
    }
}