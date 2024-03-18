using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IziHardGames.Grids.D2.ForUnity
{
    public class ComponentGrid : MonoBehaviour
    {
        [Header("Settings")]
        public int xCount = 1;
        public int yCount = 1;
        public int zCount = 1;
        public Vector3 orientation01x = Vector3.right;
        public Vector3 orientation01z = Vector3.forward;
        public Vector3 origin;
        public Vector3 cellSize;

        [Header("OnDrawGizmos()")]
        public Color color = Color.white;

        [Header("Generated")]
        /// <summary>
        /// XYZ
        /// </summary>
        public Vector3[] positionsOfCellCenters = Array.Empty<Vector3>();
        public int countCells;
        public bool isGenerated;

        [Header("Operations")]
        public MonoBehaviour[] components = Array.Empty<MonoBehaviour>();

        [ContextMenu("izhg: Generate Grid")]
        public void GenerateGrid()
        {
            countCells = xCount * yCount * zCount;
            positionsOfCellCenters = new Vector3[countCells];
            int i = default;
            float xHalf = cellSize.x / 2;
            float yHalf = cellSize.y / 2;
            float zHalf = cellSize.z / 2;

            for (int z = 0; z < zCount; z++)
            {
                for (int y = 0; y < yCount; y++)
                {
                    for (int x = 0; x < xCount; x++, i++)
                    {
                        positionsOfCellCenters[i] = new Vector3(x * cellSize.x + xHalf, y * cellSize.y + yHalf, z * cellSize.z + zHalf) + origin;
                    }
                }
            }
            isGenerated = true;
        }
        [ContextMenu("izhg: ForeachPos")]
        public void ForeachCenter()
        {
            if (!isGenerated) throw new System.InvalidOperationException($"You must call {nameof(GenerateGrid)} before that operation");
            if (components != null)
            {
                int i = default;
                for (int z = 0; z < zCount; z++)
                {
                    for (int y = 0; y < yCount; y++)
                    {
                        for (int x = 0; x < xCount; x++, i++)
                        {
                            foreach (var component in components)
                            {
                                if (component is IGridCellProcessor processor)
                                {
                                    processor.HandleCell(positionsOfCellCenters[i], x, y, z);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            Vector3 orientation01y = Vector3.Cross(orientation01x, orientation01z);
            for (int x = 0; x < this.xCount + 1; x++)
            {
                Vector3 startX = origin + (x * cellSize.x) * orientation01x;
                Debug.DrawLine(startX, startX + orientation01y * (yCount * cellSize.y), color);
            }

            for (int y = 0; y < yCount + 1; y++)
            {
                Vector3 startY = origin + (y * cellSize.y) * orientation01y;
                Debug.DrawLine(startY, startY + orientation01x * (xCount * cellSize.x), color);
            }

            for (int z = 0; z < zCount + 1; z++)
            {
                Vector3 startZ = origin + (z * cellSize.z) * orientation01z;
                Debug.DrawLine(startZ, startZ + orientation01x * (xCount * cellSize.x), color);
            }
        }
    }
}