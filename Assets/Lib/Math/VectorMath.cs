using UnityEngine;
using System.Collections;

namespace UnityLib.Math
{
    public class VectorMath
    {
        public static Vector2 GetClosestPointOnLine(Vector2 a, Vector2 b, Vector2 p)
        {
            var ap = p - a;
            var ab = b - a;
            var apDotab = Vector2.Dot(ap, ab);
            var dis = apDotab / ab.sqrMagnitude;
            if (dis > 0 && dis < 1)
                return a + ab * dis;
            else
                return new Vector2(float.MaxValue, float.MaxValue);
        }

        public static Vector2 ClosestPointOnLine(Vector2 a, Vector2 b, Vector2 p)
        {
            var ab = b - a;
            return a + ab * (Vector2.Dot((p - a), ab) / ab.sqrMagnitude);
        }

        public static Vector2 GetIntersect(Vector2 a1, Vector2 b1, Vector2 a2, Vector2 b2)
        {
            var m1 = (b1.y - a1.y) / (b1.x - a1.x);
            var m2 = (b2.y - a2.y) / (b2.x - a2.x);

            //y=mx+c
            //-c = mx-y
            //c=-mx+y

            var i1 = m1 * -a1.x + a1.y;
            var i2 = m2 * -a2.x + a2.y;

            //m1*x+i1 = m2*x+i2
            //(m1-m2)*x+(i1-i2) = 0
            //(i1-i2)/-(m1-m2)

            var x = (i1 - i2) / (m2 - m1);

            return new Vector2(x, m1 * x + i1);
        }

        public static Vector2 GetIntersect(Vector2 a, float ma, Vector2 b, float mb)
        {


            var i1 = ma * -a.x + a.y;
            var i2 = mb * -b.x + b.y;
            var x = (i1 - i2) / (mb - ma);
            return new Vector2(x, ma * x + i1);
        }

        public static float Determinant(Vector2 a, Vector2 b, Vector2 p)
        {
            return Mathf.Sign((b.x - a.x) * (p.y - a.y) - (b.y - a.y) * (p.x - a.x));
        }
    }
}