using System.Collections.Generic;
using System;
using UnityEngine;
using UnityLib.Drawing;

namespace UnityLib.DataStructure
{
    public class SpatialGrid
    {
        public bool debug;

        List<GameObject>[,] grid;
        Dictionary<GameObject, Index> indexes;

        float minX;
        float maxX;
        float minY;
        float maxY;
        float gridSize;

        public SpatialGrid(float minX, float maxX, float minY, float maxY, float gridSize)
        {
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;
            this.gridSize = gridSize;

            float width = maxX - minX;
            float height = maxY - minY;

            int xSize = Mathf.CeilToInt(width / gridSize);
            int ySize = Mathf.CeilToInt(height / gridSize);

            grid = new List<GameObject>[xSize, ySize];
            indexes = new Dictionary<GameObject, Index>();
        }

        public void Add(GameObject o)
        {
            if (indexes.ContainsKey(o))
                throw new InvalidOperationException("Already Added");

            var index = CalculateBucketIndex(o);
            indexes[o] = index;

            if (grid[index.x, index.y] == null)
                grid[index.x, index.y] = new List<GameObject>();
            grid[index.x, index.y].Add(o);
        }

        public void Remove(GameObject o)
        {
            if (!indexes.ContainsKey(o))
                return;

            var index = indexes[o];
            grid[index.x, index.y].Remove(o);
            indexes.Remove(o);
        }

        public void Update(GameObject o)
        {
            if (!indexes.ContainsKey(o))
                throw new InvalidOperationException("Not tracked");

            var newIndex = CalculateBucketIndex(o);
            var oldIndex = indexes[o];

            if (newIndex.x == oldIndex.x && newIndex.y == oldIndex.y)
                return;

            grid[oldIndex.x, oldIndex.y].Remove(o);
            grid[newIndex.x, newIndex.y].Add(o);
        }

        public List<GameObject> Query(Vector2 point, float size)
        {
            var result = new List<GameObject>();
            var index = CalculateBucketIndex(point);
            for (int x = index.x - 1; x <= index.x + 1; x++)
                for (int y = index.y - 1; y <= index.y + 1; y++)
                    if (x >= 0 && y >= 0 && x < grid.GetLength(0) && y < grid.GetLength(1))
                    {
                        if (debug)
                            LineDrawer.Draw(new Vector2(x * gridSize, (y + 1) * gridSize), new Color(x * 0.1f, y * 0.1f, 1), (int)gridSize);

                        QueryBucket(grid[x, y], result, point, size);
                    }
            if (debug)
                LineDrawer.Draw(new Vector2(index.x * gridSize, (index.y + 1) * gridSize), Color.white, (int)gridSize);

            return result;
        }

        void QueryBucket(List<GameObject> bucket, List<GameObject> result, Vector2 point, float size)
        {
            if (bucket == null)
                return;

            for (int i = 0; i < bucket.Count; i++)
            {
                var position = GetPosition(bucket[i]);
                if ((point - position).sqrMagnitude < size * size)
                    result.Add(bucket[i]);
            }
        }

        public void Clear()
        {
            indexes.Clear();
            grid = new List<GameObject>[grid.GetLength(0), grid.GetLength(1)];
        }

        Index CalculateBucketIndex(GameObject o)
        {
            return CalculateBucketIndex(GetPosition(o));
        }

        Index CalculateBucketIndex(Vector2 position)
        {
            var index = new Index();
            index.x = Mathf.FloorToInt((position.x - minX) / gridSize);
            index.y = Mathf.FloorToInt((position.y - minY) / gridSize);

            if (position.x > maxX || position.x < minX || position.y > maxY || position.y < minY || float.IsNaN(position.x) || float.IsNaN(position.y))
                throw new InvalidOperationException(position + " outside query range");

            if (index.x >= grid.GetLength(0) || index.y >= grid.GetLength(1) || index.x < 0 || index.y < 0)
                throw new InvalidOperationException(position + " " + index.x + " " + index.y);

            return index;
        }

        Vector2 GetPosition(GameObject o)
        {
            return o.transform.position;
        }

        public int Count { get { return indexes.Count; } }

        public struct Index
        {
            public int x;
            public int y;
        }
    }
}