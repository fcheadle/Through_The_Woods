using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TTW.Combat;

namespace TTW.UI
{
    public class MovementPathMaker : MonoBehaviour
    {
        UVPosition[] uvPositions;
        public bool noPath = false;

        private void Start()
        {
            GameEvents.current.onAnimationStart += DisabledForAnimation;
            GameEvents.current.onAnimationEnd += EnabledForAnimation;
            uvPositions = FindObjectsOfType<UVPosition>();
        }

        public bool CheckPath(Cell startCell, Cell finalCell)
        {
            List<Cell> checkList = GetComponent<Pathfinder>().GetFinalPath(startCell, finalCell);

            if (checkList.Count <= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DrawPath(Cell startCell, Cell finalCell)
        {

            foreach (UVPosition uv in uvPositions)
            {
                uv.ResetUV();
            }

            if (startCell == finalCell) return;

            List<Cell> drawList = GetComponent<Pathfinder>().GetFinalPath(startCell, finalCell);


            for (var i=0; i<drawList.Count; i++)
            {

                if (i == 0)
                {

                    if (drawList[i + 1].GetGridPos() + Vector2Int.up == drawList[i].GetGridPos())
                    {
                        drawList[i].GetComponentInChildren<UVPosition>().tileX = -2f;
                        drawList[i].GetComponentInChildren<UVPosition>().tileY = -1f;
                    }
                    if (drawList[i + 1].GetGridPos() + Vector2Int.down == drawList[i].GetGridPos())
                    {
                        drawList[i].GetComponentInChildren<UVPosition>().tileX = -1f;
                        drawList[i].GetComponentInChildren<UVPosition>().tileY = -1f;
                    }
                    if (drawList[i + 1].GetGridPos() + Vector2Int.right == drawList[i].GetGridPos())
                    {
                        drawList[i].GetComponentInChildren<UVPosition>().tileX = -1f;
                        drawList[i].GetComponentInChildren<UVPosition>().tileY = -2f;
                    }
                    if (drawList[i + 1].GetGridPos() + Vector2Int.left == drawList[i].GetGridPos())
                    {
                        drawList[i].GetComponentInChildren<UVPosition>().tileX = -2f;
                        drawList[i].GetComponentInChildren<UVPosition>().tileY = -2f;
                    }
                }

                if (i - 1 >= 0 && i + 1 <= drawList.Count-1)
                {
                    if (drawList[i].GetGridPos().x == drawList[i - 1].GetGridPos().x && drawList[i].GetGridPos().x == drawList[i + 1].GetGridPos().x)
                    {
                        drawList[i].GetComponentInChildren<UVPosition>().tileX = 1f;
                        drawList[i].GetComponentInChildren<UVPosition>().tileY = -1f;
                    }
                    if (drawList[i].GetGridPos().y == drawList[i - 1].GetGridPos().y && drawList[i].GetGridPos().y == drawList[i + 1].GetGridPos().y)
                    {
                        drawList[i].GetComponentInChildren<UVPosition>().tileX = 0f;
                        drawList[i].GetComponentInChildren<UVPosition>().tileY = -1f;
                    }
                    if (drawList[i - 1].GetGridPos() + Vector2Int.up == drawList[i].GetGridPos() && drawList[i + 1].GetGridPos() + Vector2Int.right == drawList[i].GetGridPos())
                    {
                        drawList[i].GetComponentInChildren<UVPosition>().tileX = 1f;
                        drawList[i].GetComponentInChildren<UVPosition>().tileY = 0f;
                    }
                    if (drawList[i - 1].GetGridPos() + Vector2Int.up == drawList[i].GetGridPos() && drawList[i + 1].GetGridPos() + Vector2Int.left == drawList[i].GetGridPos())
                    {
                        drawList[i].GetComponentInChildren<UVPosition>().tileX = 0f;
                        drawList[i].GetComponentInChildren<UVPosition>().tileY = 0f;
                    }
                    if (drawList[i - 1].GetGridPos() + Vector2Int.down == drawList[i].GetGridPos() && drawList[i + 1].GetGridPos() + Vector2Int.right == drawList[i].GetGridPos())
                    {
                        drawList[i].GetComponentInChildren<UVPosition>().tileX = -1f;
                        drawList[i].GetComponentInChildren<UVPosition>().tileY = 0f;
                    }
                    if (drawList[i - 1].GetGridPos() + Vector2Int.down == drawList[i].GetGridPos() && drawList[i + 1].GetGridPos() + Vector2Int.left == drawList[i].GetGridPos())
                    {
                        drawList[i].GetComponentInChildren<UVPosition>().tileX = -2f;
                        drawList[i].GetComponentInChildren<UVPosition>().tileY = 0f;
                    }

                    if (drawList[i - 1].GetGridPos() + Vector2Int.right == drawList[i].GetGridPos() && drawList[i + 1].GetGridPos() + Vector2Int.up == drawList[i].GetGridPos())
                    {
                        drawList[i].GetComponentInChildren<UVPosition>().tileX = 1f;
                        drawList[i].GetComponentInChildren<UVPosition>().tileY = 0f;
                    }
                    if (drawList[i - 1].GetGridPos() + Vector2Int.right == drawList[i].GetGridPos() && drawList[i + 1].GetGridPos() + Vector2Int.down == drawList[i].GetGridPos())
                    {
                        drawList[i].GetComponentInChildren<UVPosition>().tileX = -1f;
                        drawList[i].GetComponentInChildren<UVPosition>().tileY = 0f;
                    }
                    if (drawList[i - 1].GetGridPos() + Vector2Int.left == drawList[i].GetGridPos() && drawList[i + 1].GetGridPos() + Vector2Int.up == drawList[i].GetGridPos())
                    {
                        drawList[i].GetComponentInChildren<UVPosition>().tileX = 0f;
                        drawList[i].GetComponentInChildren<UVPosition>().tileY = 0f;
                    }
                    if (drawList[i - 1].GetGridPos() + Vector2Int.left == drawList[i].GetGridPos() && drawList[i + 1].GetGridPos() + Vector2Int.down == drawList[i].GetGridPos())
                    {
                        drawList[i].GetComponentInChildren<UVPosition>().tileX = -2f;
                        drawList[i].GetComponentInChildren<UVPosition>().tileY = 0f;
                    }
                }

                if (i == drawList.Count-1)
                {
                    if (drawList[i - 1].GetGridPos() + Vector2Int.up == drawList[i].GetGridPos())
                    {
                        drawList[i].GetComponentInChildren<UVPosition>().tileX = -2f;
                        drawList[i].GetComponentInChildren<UVPosition>().tileY = 1f;
                    }
                    if (drawList[i - 1].GetGridPos() + Vector2Int.right == drawList[i].GetGridPos())
                    {
                        drawList[i].GetComponentInChildren<UVPosition>().tileX = 1f;
                        drawList[i].GetComponentInChildren<UVPosition>().tileY = 1f;
                    }
                    if (drawList[i - 1].GetGridPos() + Vector2Int.down == drawList[i].GetGridPos())
                    {
                        drawList[i].GetComponentInChildren<UVPosition>().tileX = -1f;
                        drawList[i].GetComponentInChildren<UVPosition>().tileY = 1f;
                    }
                    if (drawList[i - 1].GetGridPos() + Vector2Int.left == drawList[i].GetGridPos())
                    {
                        drawList[i].GetComponentInChildren<UVPosition>().tileX = 0f;
                        drawList[i].GetComponentInChildren<UVPosition>().tileY = 1f;
                    }
                }

                drawList[i].GetComponentInChildren<UVPosition>().UpdateUV();
            }



            ClearPath();
        }

        public void ClearPath()
        {
            Cell[] cells = FindObjectsOfType<Cell>();

            foreach (Cell cell in cells)
            {
                cell.isExplored = false;
                cell.exploredFrom = null;
            }
        }

        private void EnabledForAnimation()
        {
            foreach (UVPosition uvPosition in uvPositions)
            {
                uvPosition.GetComponent<MeshRenderer>().enabled = true;
            }

        }

        private void DisabledForAnimation()
        {
            foreach (UVPosition uvPosition in uvPositions)
            {
                uvPosition.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
