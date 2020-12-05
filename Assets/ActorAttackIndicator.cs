using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TTW.Combat;

namespace TTW.UI
{
    public class ActorAttackIndicator : MonoBehaviour
    {
        CombatController combatController;

        // Start is called before the first frame update
        void Start()
        {
            combatController = FindObjectOfType<CombatController>();
        }


        public void IndicateMelee(Fighter fighter)
        {
            Vector2Int targetPos = fighter.GetComponent<Mover>().GetGridPos();

            //calculate north
            for (var i=targetPos.y; i<=3; i++)
            {
                if (combatController.ListOfcells[new Vector2Int(targetPos.x, i)].isEnemyCell)
                {
                    combatController.ListOfcells[new Vector2Int(targetPos.x, i)].GetComponentInChildren<MeleeIndicator>().Indicate();
                    continue;
                }

                if (!combatController.ListOfcells[new Vector2Int(targetPos.x, i)].isOccupied && !combatController.ListOfPlayerCells[new Vector2Int(targetPos.x, i)].hasObstacle)
                {
                    combatController.ListOfcells[new Vector2Int(targetPos.x, i)].GetComponentInChildren<MeleeIndicator>().Indicate();
                }
                else
                {
                    if(i != targetPos.y)
                    {
                        break;
                    }
                }
            }
            for (var i = targetPos.y; i >= 0; i--)
            {
                if (combatController.ListOfcells[new Vector2Int(targetPos.x, i)].isEnemyCell)
                {
                    combatController.ListOfcells[new Vector2Int(targetPos.x, i)].GetComponentInChildren<MeleeIndicator>().Indicate();
                    continue;
                }

                if (!combatController.ListOfcells[new Vector2Int(targetPos.x, i)].isOccupied && !combatController.ListOfPlayerCells[new Vector2Int(targetPos.x, i)].hasObstacle)
                {
                    combatController.ListOfcells[new Vector2Int(targetPos.x, i)].GetComponentInChildren<MeleeIndicator>().Indicate();
                }
                else
                {
                    if (i != targetPos.y)
                    {
                        break;
                    }
                }
            }
            for (var i = targetPos.x; i <= 5; i++)
            {
                if (combatController.ListOfcells[new Vector2Int(i, targetPos.y)].isEnemyCell)
                {
                    combatController.ListOfcells[new Vector2Int(i, targetPos.y)].GetComponentInChildren<MeleeIndicator>().Indicate();
                    continue;
                }

                if (!combatController.ListOfcells[new Vector2Int(i, targetPos.y)].isOccupied && !combatController.ListOfPlayerCells[new Vector2Int(i, targetPos.y)].hasObstacle)
                {
                    combatController.ListOfcells[new Vector2Int(i, targetPos.y)].GetComponentInChildren<MeleeIndicator>().Indicate();
                }
                else
                {
                    if (i != targetPos.x)
                    {
                        break;
                    }
                }
            }
            for (var i = targetPos.x; i >= 0; i--)
            {
                if (combatController.ListOfcells[new Vector2Int(i, targetPos.y)].isEnemyCell)
                {
                    combatController.ListOfcells[new Vector2Int(i, targetPos.y)].GetComponentInChildren<MeleeIndicator>().Indicate();
                    continue;
                }

                if (!combatController.ListOfcells[new Vector2Int(i, targetPos.y)].isOccupied && !combatController.ListOfPlayerCells[new Vector2Int(i, targetPos.y)].hasObstacle)
                {
                    combatController.ListOfcells[new Vector2Int(i, targetPos.y)].GetComponentInChildren<MeleeIndicator>().Indicate();
                }
                else
                {
                    if (i != targetPos.x)
                    {
                        break;
                    }
                }
            }
        }

        public void IndicateCell(Cell cell)
        {
            cell.GetComponentInChildren<MeleeIndicator>().Indicate();
        }

        public void IndicateAdjacent(Cell cell)
        {
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left };

            foreach(Vector2Int direction in directions)
            {
                if (combatController.ListOfPlayerCells.ContainsKey(cell.GetGridPos() + direction))
                {
                    combatController.ListOfPlayerCells[cell.GetGridPos() + direction].GetComponentInChildren<MeleeIndicator>().Indicate();
                }
            }
        }

        public void ResetIndicators()
        {
            MeleeIndicator[] indicators = FindObjectsOfType<MeleeIndicator>();

            foreach(MeleeIndicator indicator in indicators)
            {
                indicator.GetComponentInChildren<MeleeIndicator>().StopIndicating();
            }
        }
    }
}
