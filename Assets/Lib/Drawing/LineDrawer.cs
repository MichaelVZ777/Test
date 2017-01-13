using UnityEngine;
using System.Collections.Generic;

namespace UnityLib.Drawing
{
    public class LineDrawer : SingletonMonoManager<LineDrawer>
    {
        public List<LineInfo> lines;
        public List<PointInfo> points;
        public List<PointInfo> rawPoints;

        void Awake()
        {
            lines = new List<LineInfo>();
            points = new List<PointInfo>();
            rawPoints = new List<PointInfo>();
        }

        public static void Toggle()
        {
            Instance.enabled = !Instance.enabled;
        }

        public static void Draw(Vector2 pointA, Vector2 pointB, Color color, float width)
        {
            if (!Instance.enabled || !Instance.gameObject.activeInHierarchy)
                return;

            var lineInfo = new LineInfo { pointA = pointA, pointB = pointB, color = color, width = width };
            Instance.lines.Add(lineInfo);
        }

        public static void Draw(Vector2 point, Color color, int size)
        {
            if (!Instance.enabled || !Instance.gameObject.activeInHierarchy)
                return;

            var pointInfo = new PointInfo { position = point, color = color, size = size };
            Instance.points.Add(pointInfo);
        }

        public static void DrawRaw(Vector2 point, Color color, int size)
        {
            if (!Instance.enabled || !Instance.gameObject.activeInHierarchy)
                return;

            var pointInfo = new PointInfo { position = point, color = color, size = size };
            Instance.rawPoints.Add(pointInfo);
        }

        void OnGUI()
        {
            if (Event.current.type != EventType.Repaint)
                return;

            foreach (var line in lines)
                Drawing.DrawGUILine(line.pointA, line.pointB, line.color, line.width, false);

            foreach (var point in points)
                DrawImage.DrawGUIPoint(point.position, point.color, point.size);

            foreach (var point in rawPoints)
                DrawImage.DrawPoint(point.position, point.color, point.size);

            lines.Clear();
            points.Clear();
            rawPoints.Clear();
        }

        public class LineInfo
        {
            public Vector2 pointA;
            public Vector2 pointB;
            public Color color;
            public float width;
        }

        public class PointInfo
        {
            public Vector2 position;
            public Color color;
            public int size;
        }
    }
}