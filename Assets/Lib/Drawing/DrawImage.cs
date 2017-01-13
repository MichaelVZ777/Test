using UnityEngine;
using System.Reflection;

namespace UnityLib.Drawing
{
    public class DrawImage
    {
        static Rect lineRect = new Rect(0, 0, 1, 1);
        static Material blendMaterial;
        //static Material blitMaterial;
        static Texture2D blankTexture;

        static DrawImage()
        {
            Initialize();
        }

        private static void Initialize()
        {
            blendMaterial = (Material)typeof(GUI).GetMethod("get_blendMaterial", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
            //blitMaterial = (Material)typeof(GUI).GetMethod("get_blitMaterial", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);

            blankTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            blankTexture.SetPixel(0, 1, Color.white);
            blankTexture.Apply();
        }

        public static void DrawGUIPoint(Vector2 position, Color color, int size)
        {
            DrawPoint(new Vector2(position.x, Screen.height - position.y), color, size);
        }

        public static void DrawPoint(Vector2 position, Color color, int size)
        {
            Matrix4x4 matrix = Matrix4x4.identity;
            matrix.m00 = size;
            matrix.m01 = 0;
            matrix.m03 = position.x; //offset
            matrix.m10 = 0;
            matrix.m11 = size;
            matrix.m13 = position.y; //offset

            // Use GL matrix and Graphics.DrawTexture rather than GUI.matrix and GUI.DrawTexture,
            // for better performance. (Setting GUI.matrix is slow, and GUI.DrawTexture is just a
            // wrapper on Graphics.DrawTexture.)
            GL.PushMatrix();
            GL.MultMatrix(matrix);
            //        Graphics.DrawTexture(lineRect, blankTexture, lineRect, 0, 0, 0, 0, color, blendMaterial);
            //Replaced by:
            GUI.color = color;//this and...
            GUI.DrawTexture(lineRect, blankTexture);//this

            GL.PopMatrix();
        }

        public static void DrawTexture(Vector2 position, Texture texture, int size, Color color)
        {
            Matrix4x4 matrix = Matrix4x4.identity;
            matrix.m00 = size;
            matrix.m01 = 0;
            matrix.m03 = position.x; //offset
            matrix.m10 = 0;
            matrix.m11 = size;
            matrix.m13 = position.y; //offset

            GL.PushMatrix();
            GL.MultMatrix(matrix);
            Graphics.DrawTexture(lineRect, texture, lineRect, 0, 0, 0, 0, color, blendMaterial);
            //Replaced by:
            //        GUI.color = color;//this and...
            //        GUI.DrawTexture(lineRect, texture);//this

            GL.PopMatrix();
        }
    }
}