using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class Pathfinder : MonoBehaviour
    {
        Cell startingCell, endingCell;
        bool isRunning = true;
        Cell searchCenter;
        private bool LoadCellsOnce = false;

        Vector2Int[] directions =
        {
        Vector2Int.up,
        Vector2Int.left,
        Vector2Int.down,
        Vector2Int.right
        };

        public Dictionary<Vector2Int, Cell> grid = new Dictionary<Vector2Int, Cell>();
        public Queue<Cell> pathQueue = new Queue<Cell>();
        public List<Cell> finalPath = new List<Cell>();

        public List<Cell> GetFinalPath(Cell start, Cell end)
        {
            isRunning = true;
            finalPath = new List<Cell>();
            pathQueue = new Queue<Cell>();
            startingCell = start;
            endingCell = end;
            LoadCells();
            Pathfind();
            return finalPath;
        }

        private void Pathfind()
        {
            pathQueue.Enqueue(startingCell);

            while (pathQueue.Count > 0 && isRunning)
            {
                searchCenter = pathQueue.Dequeue();
                searchCenter.isExplored = true;
                CheckIfPathIsComplete();
                ExploreNeighbors();
            }
        }

        private void CheckIfPathIsComplete()
        {

            if (searchCenter == endingCell && isRunning)
            {
                Cell currentCell = endingCell;
                isRunning = false;
                finalPath.Add(endingCell);
                while (currentCell != startingCell)
                {
                    finalPath.Add(currentCell.exploredFrom);
                    currentCell = currentCell.exploredFrom;
                }
                finalPath.Reverse();
            }
        }

        private void ExploreNeighbors()
        {
            if (!isRunning) { return; }
            foreach (Vector2Int direction in directions)
            {
                if (grid.ContainsKey(searchCenter.GetGridPos() + direction))
                {
                    QueueNewNeighbors(direction);
                }
            }
        }

        private void QueueNewNeighbors(Vector2Int direction)
        {
            Cell neighbor = grid[(searchCenter.GetGridPos() + direction)];

            if (!neighbor.isExplored || pathQueue.Contains(neighbor))
            {
                if (!neighbor.isOccupied)
                {
                    if (!neighbor.isEnemyCell)
                    {
                        pathQueue.Enqueue(neighbor);
                        neighbor.exploredFrom = searchCenter;
                    }
                }
            }
        }

        private void NoAvailablePath()
        {
            isRunning = false;
            finalPath.Clear();
            finalPath.Add(startingCell);
        }

        private void LoadCells()
        {
            var cells = FindObjectsOfType<Cell>();

            if (!LoadCellsOnce)
            {
                foreach (Cell cell in cells)
                {
                    bool isOverlapping = grid.ContainsKey(cell.GetGridPos());

                    if (isOverlapping)
                    {
                        Debug.LogWarning("Overlapping cells @: " + cell);
                    }

                    else
                    {
                        grid.Add(cell.GetGridPos(), cell);
                    }
                }

                LoadCellsOnce = true;
            }
        }
    }
}
