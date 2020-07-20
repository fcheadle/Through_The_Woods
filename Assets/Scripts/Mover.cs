using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class Mover : MonoBehaviour
    {
        public Vector2Int gridPos;
        const int gridSize = 10;
        bool isMovingToTarget = false;
        public bool isBeingPushed = false;
        public float speed = 10.0f;
        Cell target = null;
        public Cell currentCell = null;
        Dictionary<Vector2Int, Cell> grid = new Dictionary<Vector2Int, Cell>();
        AnimationHandler animationHandler;
        CombatController combatController;


        private Cell finalCell = null;

        [SerializeField] List<Cell> path;

        void Start()
        {
            GetCurrentCell();
            currentCell.isOccupied = true;
            animationHandler = GetComponent<AnimationHandler>();
            combatController = FindObjectOfType<CombatController>();
        }

        private void Update()
        {
            GetCurrentCell();


            //Is moving using pathfinding
            if (isMovingToTarget)
            {
                MoveToTarget(target);
            }
        }

        public void GetCurrentCell()
        {
            gridPos = GetGridPos();

            Cell[] cells = FindObjectsOfType<Cell>();

            foreach (Cell cell in cells)
            {
                if (gridPos == cell.GetGridPos())
                {
                    currentCell = cell;
                }
            }
        }

        public Vector2Int GetGridPos()
        {
            return new Vector2Int
            (
                Mathf.RoundToInt(transform.position.x / gridSize),
                Mathf.RoundToInt(transform.position.z / gridSize)
            );
        }

        public void Pathfind(Cell endingCell)
        {
            finalCell = endingCell;
            GetCurrentCell();
            Pathfinder pathfinder = FindObjectOfType<Pathfinder>();
            ClearPath(pathfinder);
            path = pathfinder.GetFinalPath(currentCell, endingCell);
            if (path.Count > 1)
            {
                StartCoroutine(MoveOnPath(path));
                currentCell.isOccupied = false;
                GameEvents.current.AnimationStart();
                GetComponent<AttackReceiver>().BreakNeutralState(true);
            }
            else
            {
                Debug.LogWarning("No available path");
            }
        }

        private static void ClearPath(Pathfinder pathfinder)
        {
            Cell[] cells = FindObjectsOfType<Cell>();

            foreach (Cell cell in cells)
            {
                cell.isExplored = false;
                cell.exploredFrom = null;
            }
        }

        IEnumerator MoveOnPath(List<Cell> path)
        {
            foreach (Cell coordinate in path)
            {
                if (coordinate == path[0]) continue;
                //transform.position = coordinate.transform.position;

                if (animationHandler != null)
                {
                    if (coordinate.GetGridPos().x > GetGridPos().x)
                    {
                        animationHandler.MoveRight();
                    }

                    if (coordinate.GetGridPos().x < GetGridPos().x)
                    {
                        animationHandler.MoveLeft();
                    }

                    if (coordinate.GetGridPos().y > GetGridPos().y)
                    {
                        animationHandler.MoveDown();
                    }

                    if (coordinate.GetGridPos().y < GetGridPos().y)
                    {
                        animationHandler.MoveUp();
                    }
                }

                isMovingToTarget = true;
                target = coordinate;

                yield return new WaitForSeconds(0.6f);
            }
        }

        public void MoveToTarget(Cell target)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);


            if (target == finalCell)
            {
                if (Vector3.Distance(transform.position, target.transform.position) < 1f)
                {
                    if (target.hasTrap)
                    {
                        PerformTrap(target);
                    }

                    if (animationHandler != null)
                    {
                        animationHandler.BeIdle();
                    }
                    isMovingToTarget = false;
                    target.DoConfirmationColor();
                    target.isOccupied = true;

                    isBeingPushed = false;

                    if (combatController.IsEndAnimationReady())
                    {
                        GameEvents.current.AnimationEnd();
                    }
                }
            }
        }

        private void PerformTrap(Cell target)
        {
            if (GetComponent<Fighter>() != null)
            {
                target.currentTrap.TrapTrigger(GetComponent<AttackReceiver>());
                target.DestroyTrap();
            }
        }

        public void DoMoveToTarget(Cell newTarget)
        {
            isMovingToTarget = true;
            target = newTarget;
        }

        public void UpdateCell()
        {
            gridPos = GetGridPos();
        }
    }
}
