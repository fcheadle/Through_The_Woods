using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class Pusher : MonoBehaviour
    {
        CombatController combatController;

        // Start is called before the first frame update
        void Start()
        {
            combatController = FindObjectOfType<CombatController>();
        }

        public List<Pushable> MakeListOfPushablesRelative(EnemyAttack ability, EnemyTarget enemyTarget)
        {

            Pushable[] pushableArray = FindObjectsOfType<Pushable>();
            List<Pushable> listOfPushables = new List<Pushable>(pushableArray);
            List<Pushable> deleteMeList = new List<Pushable>();
            Mover mover = enemyTarget.GetComponent<Mover>();

            if (enemyTarget.wing == Wing.port || enemyTarget.wing == Wing.starboard)
            {
                listOfPushables.Sort(SortByX);
            }
            else
            {
                listOfPushables.Sort(SortByY);
                listOfPushables.Reverse();
            }


            if (enemyTarget.wing == Wing.starboard)
            {
                listOfPushables.Reverse();
            }

            bool deleteRest = false;
            Vector2Int interferenceCheck;


            foreach (Pushable pushable in listOfPushables)
            {

                if (enemyTarget.wing == Wing.port)
                {
                    if (ability.push == Push.north) interferenceCheck = new Vector2Int(pushable.GetGridPos().x + 1, pushable.GetGridPos().y);
                    else interferenceCheck = new Vector2Int(pushable.GetGridPos().x - 1, pushable.GetGridPos().y);
                }

                else if (enemyTarget.wing == Wing.starboard)
                {
                    if (ability.push == Push.north) interferenceCheck = new Vector2Int(pushable.GetGridPos().x - 1, pushable.GetGridPos().y);
                    else interferenceCheck = new Vector2Int(pushable.GetGridPos().x + 1, pushable.GetGridPos().y);
                }
                else
                {
                    if (ability.push == Push.north) interferenceCheck = new Vector2Int(pushable.GetGridPos().x, pushable.GetGridPos().y - 1);
                    else interferenceCheck = new Vector2Int(pushable.GetGridPos().x, pushable.GetGridPos().y + 1);
                }

                if ((enemyTarget.wing == Wing.starboard) || (enemyTarget.wing == Wing.port))
                {
                    if (pushable.GetGridPos().y != mover.GetGridPos().y)
                    {
                        deleteMeList.Add(pushable);
                        continue;
                    }
                }
                else
                {
                    if (pushable.GetGridPos().x != mover.GetGridPos().x)
                    {
                        deleteMeList.Add(pushable);
                        continue;
                    }
                }

                if (pushable.GetComponent<Obstacle>() != null)
                {
                    deleteMeList.Add(pushable);
                    deleteRest = true;
                }

                if (combatController.ListOfcells.ContainsKey(interferenceCheck))
                {
                    if (combatController.ListOfcells[interferenceCheck].isOccupied)
                    {
                        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();
                        foreach (Obstacle obstacle in obstacles)
                        {
                            if (obstacle.GetComponent<Pushable>().GetGridPos() == interferenceCheck)
                            {
                                deleteMeList.Add(pushable);
                                break;
                            }
                        }
                    }

                    if (combatController.ListOfcells[interferenceCheck].isEnemyCell)
                    {
                        deleteMeList.Add(pushable);
                    }

                    if (combatController.ListOfcells[interferenceCheck] == null)
                    {
                        deleteMeList.Add(pushable);
                    }
                }

                if (deleteRest == true)
                {
                    deleteMeList.Add(pushable);
                }
            }

            foreach (Pushable pushable in deleteMeList)
            {
                listOfPushables.Remove(pushable);
            }

            if (ability.push == Push.north)
            {
                listOfPushables.Reverse();
            }

            return listOfPushables;
        }

        public List<Pushable> MakeListOfPushablesGlobal(EnemyAttack ability)
        {

            Pushable[] pushableArray = FindObjectsOfType<Pushable>();
            List<Pushable> listOfPushables = new List<Pushable>(pushableArray);
            List<Pushable> deleteMeList = new List<Pushable>();

            if (ability.push == Push.north)
            {
                listOfPushables.Sort(SortByY);
                listOfPushables.Reverse();
            }
            else if (ability.push == Push.west)
            {
                listOfPushables.Sort(SortByX);
                listOfPushables.Reverse();
            }
            else if (ability.push == Push.east)
            {
                listOfPushables.Sort(SortByX);
            }
            else if (ability.push == Push.south)
            {
                listOfPushables.Sort(SortByY);
            }

            bool deleteRest = false;
            Vector2Int interferenceCheck;


            foreach (Pushable pushable in listOfPushables)
            {

                if (ability.push == Push.west)
                    interferenceCheck = new Vector2Int(pushable.GetGridPos().x + 1, pushable.GetGridPos().y);
                else if (ability.push == Push.east)
                    interferenceCheck = new Vector2Int(pushable.GetGridPos().x - 1, pushable.GetGridPos().y);
                else if (ability.push == Push.north)
                    interferenceCheck = new Vector2Int(pushable.GetGridPos().x, pushable.GetGridPos().y + 1);
                else
                    interferenceCheck = new Vector2Int(pushable.GetGridPos().x, pushable.GetGridPos().y - 1);

                if (pushable.GetComponent<Obstacle>() != null)
                {
                    deleteMeList.Add(pushable);
                }

                if (combatController.ListOfcells.ContainsKey(interferenceCheck))
                {
                    if (combatController.ListOfcells[interferenceCheck].isOccupied)
                    {
                        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();
                        foreach (Obstacle obstacle in obstacles)
                        {
                            if (obstacle.GetComponent<Pushable>().GetGridPos() == interferenceCheck)
                            {
                                deleteMeList.Add(pushable);
                                break;
                            }
                        }
                    }

                    if (combatController.ListOfcells[interferenceCheck].isEnemyCell)
                    {
                        deleteMeList.Add(pushable);
                    }

                    if (combatController.ListOfcells[interferenceCheck] == null)
                    {
                        deleteMeList.Add(pushable);
                    }
                }

                if (deleteRest == true)
                {
                    deleteMeList.Add(pushable);
                }
            }

            foreach (Pushable pushable in deleteMeList)
            {
                listOfPushables.Remove(pushable);
            }

            return listOfPushables;
        }

        public void ExecutePushRelative(Pushable pushable, EnemyAttack ability, Wing wing)
        {

            Vector2Int[] targetCellCoords = new Vector2Int[ability.pushForce];
            Cell[] targetCells = new Cell[ability.pushForce];

            int pushMaximum = 0;

            if (wing == Wing.port)
            {

                for (var i = ability.pushForce - 1; i > 0; i--)
                {
                    if (ability.push == Push.north)
                    {
                        targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x + i, pushable.GetGridPos().y);
                    }


                    if (ability.push == Push.south)
                    {
                        targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x - i, pushable.GetGridPos().y);
                    }

                    if (!combatController.ListOfPlayerCells.ContainsKey(targetCellCoords[i])) continue;


                    targetCells[i] = combatController.ListOfPlayerCells[targetCellCoords[i]];

                    if (targetCells[i].isOccupied || targetCells[i].isEnemyCell || targetCells[i] == null) continue;



                    pushMaximum = i;

                    break;
                }


                for (var i = 0; i < ability.pushForce; i++)
                {

                    if (ability.push == Push.north)
                    {
                        targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x + i, pushable.GetGridPos().y);
                    }
                    else
                    {
                        targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x - i, pushable.GetGridPos().y);
                    }

                    if (!combatController.ListOfPlayerCells.ContainsKey(targetCellCoords[i])) continue;

                    targetCells[i] = combatController.ListOfPlayerCells[targetCellCoords[i]];

                    if (targetCells[i].hasObstacle == true)
                    {
                        if (ability.push == Push.north)
                        {
                            if (combatController.ListOfPlayerCells[new Vector2Int(pushable.GetGridPos().x + 1, pushable.GetGridPos().y)].isOccupied)
                            {
                                pushMaximum = 0;
                                break;
                            }
                        }
                        else
                        {
                            if (combatController.ListOfPlayerCells[new Vector2Int(pushable.GetGridPos().x - 1, pushable.GetGridPos().y)].isOccupied)
                            {
                                pushMaximum = 0;
                                break;
                            }
                        }

                        if (pushMaximum > i - 1)
                        {
                            pushMaximum = i - 1;
                        }
                    }
                }
            }

            if (wing == Wing.starboard)
            {
                for (var i = ability.pushForce - 1; i > 0; i--)
                {

                    if (ability.push == Push.north)
                    {
                        targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x - i, pushable.GetGridPos().y);
                    }
                    else
                    {
                        targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x + i, pushable.GetGridPos().y);
                    }


                    if (!combatController.ListOfPlayerCells.ContainsKey(targetCellCoords[i])) continue;


                    targetCells[i] = combatController.ListOfPlayerCells[targetCellCoords[i]];

                    if (targetCells[i].isOccupied || targetCells[i].isEnemyCell || targetCells[i] == null) continue;

                    pushMaximum = i;

                    break;
                }
                for (var i = 0; i < ability.pushForce; i++)
                {

                    if (ability.push == Push.north)
                    {
                        targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x - i, pushable.GetGridPos().y);
                    }
                    else
                    {
                        targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x + i, pushable.GetGridPos().y);
                    }

                    if (!combatController.ListOfPlayerCells.ContainsKey(targetCellCoords[i])) continue;

                    targetCells[i] = combatController.ListOfPlayerCells[targetCellCoords[i]];

                    if (targetCells[i].hasObstacle == true)
                    {

                        if (ability.push == Push.north)
                        {
                            if (combatController.ListOfPlayerCells[new Vector2Int(pushable.GetGridPos().x - 1, pushable.GetGridPos().y)].isOccupied)
                            {
                                pushMaximum = 0;
                                break;
                            }
                        }
                        else
                        {
                            if (combatController.ListOfPlayerCells[new Vector2Int(pushable.GetGridPos().x + 1, pushable.GetGridPos().y)].isOccupied)
                            {
                                pushMaximum = 0;
                                break;
                            }
                        }

                        if (pushMaximum > i - 1)
                        {
                            pushMaximum = i - 1;
                        }
                    }
                }
            }

            if (wing == Wing.bow)
            {

                for (var i = ability.pushForce - 1; i > 0; i--)
                {
                    if (ability.push == Push.north)
                    {
                        targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x, pushable.GetGridPos().y - i);
                    }
                    else
                    {
                        targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x, pushable.GetGridPos().y + i);
                    }

                    if (!combatController.ListOfPlayerCells.ContainsKey(targetCellCoords[i])) continue;


                    targetCells[i] = combatController.ListOfPlayerCells[targetCellCoords[i]];

                    if (targetCells[i].isOccupied || targetCells[i].isEnemyCell || targetCells[i] == null) continue;

                    pushMaximum = i;

                    break;
                }
                for (var i = 0; i < ability.pushForce; i++)
                {
                    if (ability.push == Push.north)
                    {
                        targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x, pushable.GetGridPos().y - i);
                    }
                    else
                    {
                        targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x, pushable.GetGridPos().y + i);
                    }

                    if (!combatController.ListOfPlayerCells.ContainsKey(targetCellCoords[i])) continue;

                    targetCells[i] = combatController.ListOfPlayerCells[targetCellCoords[i]];

                    if (targetCells[i].hasObstacle == true)
                    {
                        if (ability.push == Push.north)
                        {
                            if (combatController.ListOfPlayerCells[new Vector2Int(pushable.GetGridPos().x, pushable.GetGridPos().y - 1)].isOccupied)
                            {
                                pushMaximum = 0;
                                break;
                            }
                        }
                        else
                        {
                            if (combatController.ListOfPlayerCells[new Vector2Int(pushable.GetGridPos().x, pushable.GetGridPos().y + 1)].isOccupied)
                            {
                                pushMaximum = 0;
                                break;
                            }
                        }

                        if (pushMaximum > i - 1)
                        {
                            pushMaximum = i - 1;
                        }
                    }
                }
            }

            if (pushMaximum > 0)
            {
                pushable.GetComponent<Mover>().Pathfind(targetCells[pushMaximum]);
                pushable.GetComponent<Mover>().isBeingPushed = true;
                targetCells[pushMaximum].isOccupied = true;
            }
        }

        public void ExecutePushGlobal(Pushable pushable, EnemyAttack ability)
        {

            Vector2Int[] targetCellCoords = new Vector2Int[ability.pushForce];
            Cell[] targetCells = new Cell[ability.pushForce];

            int pushMaximum = 0;

            if (ability.push == Push.west)
            {

                for (var i = ability.pushForce - 1; i > 0; i--)
                {
                    targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x + i, pushable.GetGridPos().y);

                    if (!combatController.ListOfPlayerCells.ContainsKey(targetCellCoords[i])) continue;

                    targetCells[i] = combatController.ListOfPlayerCells[targetCellCoords[i]];

                    if (targetCells[i].isOccupied || targetCells[i].isEnemyCell || targetCells[i] == null) continue;

                    pushMaximum = i;

                    break;
                }


                for (var i = 0; i < ability.pushForce; i++)
                {

                    targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x + i, pushable.GetGridPos().y);

                    if (!combatController.ListOfPlayerCells.ContainsKey(targetCellCoords[i])) continue;

                    targetCells[i] = combatController.ListOfPlayerCells[targetCellCoords[i]];

                    if (targetCells[i].hasObstacle == true)
                    {
                        if (combatController.ListOfPlayerCells[new Vector2Int(pushable.GetGridPos().x + 1, pushable.GetGridPos().y)].isOccupied)
                        {
                            pushMaximum = 0;
                            break;
                        }

                        if (pushMaximum > i - 1)
                        {
                            pushMaximum = i - 1;
                        }
                    }
                }
            }

            else if (ability.push == Push.east)
            {

                for (var i = ability.pushForce - 1; i > 0; i--)
                {
                    targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x - i, pushable.GetGridPos().y);

                    if (!combatController.ListOfPlayerCells.ContainsKey(targetCellCoords[i])) continue;

                    targetCells[i] = combatController.ListOfPlayerCells[targetCellCoords[i]];

                    if (targetCells[i].isOccupied || targetCells[i].isEnemyCell || targetCells[i] == null) continue;

                    pushMaximum = i;

                    break;
                }


                for (var i = 0; i < ability.pushForce; i++)
                {

                    targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x - i, pushable.GetGridPos().y);

                    if (!combatController.ListOfPlayerCells.ContainsKey(targetCellCoords[i])) continue;

                    targetCells[i] = combatController.ListOfPlayerCells[targetCellCoords[i]];

                    if (targetCells[i].hasObstacle == true)
                    {
                        if (combatController.ListOfPlayerCells[new Vector2Int(pushable.GetGridPos().x - 1, pushable.GetGridPos().y)].isOccupied)
                        {
                            pushMaximum = 0;
                            break;
                        }

                        if (pushMaximum > i - 1)
                        {
                            pushMaximum = i - 1;
                        }
                    }
                }
            }

            else if (ability.push == Push.north)
            {

                for (var i = ability.pushForce - 1; i > 0; i--)
                {
                    targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x, pushable.GetGridPos().y + i);

                    if (!combatController.ListOfPlayerCells.ContainsKey(targetCellCoords[i])) continue;

                    targetCells[i] = combatController.ListOfPlayerCells[targetCellCoords[i]];

                    if (targetCells[i].isOccupied || targetCells[i].isEnemyCell || targetCells[i] == null) continue;

                    pushMaximum = i;

                    break;
                }


                for (var i = 0; i < ability.pushForce; i++)
                {

                    targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x, pushable.GetGridPos().y + i);

                    if (!combatController.ListOfPlayerCells.ContainsKey(targetCellCoords[i])) continue;

                    targetCells[i] = combatController.ListOfPlayerCells[targetCellCoords[i]];

                    if (targetCells[i].hasObstacle == true)
                    {
                        if (combatController.ListOfPlayerCells[new Vector2Int(pushable.GetGridPos().x, pushable.GetGridPos().y + 1)].isOccupied)
                        {
                            pushMaximum = 0;
                            break;
                        }

                        if (pushMaximum > i - 1)
                        {
                            pushMaximum = i - 1;
                        }
                    }
                }
            }

            else
            {

                for (var i = ability.pushForce - 1; i > 0; i--)
                {
                    targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x, pushable.GetGridPos().y - i);

                    if (!combatController.ListOfPlayerCells.ContainsKey(targetCellCoords[i])) continue;

                    targetCells[i] = combatController.ListOfPlayerCells[targetCellCoords[i]];

                    if (targetCells[i].isOccupied || targetCells[i].isEnemyCell || targetCells[i] == null) continue;

                    pushMaximum = i;

                    break;
                }


                for (var i = 0; i < ability.pushForce; i++)
                {

                    targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x, pushable.GetGridPos().y - i);

                    if (!combatController.ListOfPlayerCells.ContainsKey(targetCellCoords[i])) continue;

                    targetCells[i] = combatController.ListOfPlayerCells[targetCellCoords[i]];

                    if (targetCells[i].hasObstacle == true)
                    {
                        if (combatController.ListOfPlayerCells[new Vector2Int(pushable.GetGridPos().x, pushable.GetGridPos().y - 1)].isOccupied)
                        {
                            pushMaximum = 0;
                            break;
                        }

                        if (pushMaximum > i - 1)
                        {
                            pushMaximum = i - 1;
                        }
                    }
                }
            }

            if (pushMaximum > 0)
            {
                pushable.GetComponent<Mover>().Pathfind(targetCells[pushMaximum]);
                pushable.GetComponent<Mover>().isBeingPushed = true;
                targetCells[pushMaximum].isOccupied = true;
            }
        }

        public void ExecutePushGlobalAlly(Pushable pushable, Push push)
        {

            Vector2Int[] targetCellCoords = new Vector2Int[4];

            Cell[] targetCells = new Cell[4];

            int pushMaximum = 0;

            if (push == Push.west)
            {

                for (var i = 4 - 1; i > 0; i--)
                {
                    targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x + i, pushable.GetGridPos().y);

                    if (!combatController.ListOfPlayerCells.ContainsKey(targetCellCoords[i])) continue;

                    targetCells[i] = combatController.ListOfPlayerCells[targetCellCoords[i]];

                    if (targetCells[i].isOccupied || targetCells[i].isEnemyCell || targetCells[i] == null) continue;

                    pushMaximum = i;

                    break;
                }


                for (var i = 0; i < 4; i++)
                {

                    targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x + i, pushable.GetGridPos().y);

                    if (!combatController.ListOfPlayerCells.ContainsKey(targetCellCoords[i])) continue;

                    targetCells[i] = combatController.ListOfPlayerCells[targetCellCoords[i]];

                    if (targetCells[i].hasObstacle == true)
                    {
                        if (combatController.ListOfPlayerCells[new Vector2Int(pushable.GetGridPos().x + 1, pushable.GetGridPos().y)].isOccupied)
                        {
                            pushMaximum = 0;
                            break;
                        }

                        if (pushMaximum > i - 1)
                        {
                            pushMaximum = i - 1;
                        }
                    }
                }
            }

            else if (push == Push.east)
            {

                for (var i = 4 - 1; i > 0; i--)
                {
                    targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x - i, pushable.GetGridPos().y);

                    if (!combatController.ListOfPlayerCells.ContainsKey(targetCellCoords[i])) continue;

                    targetCells[i] = combatController.ListOfPlayerCells[targetCellCoords[i]];

                    if (targetCells[i].isOccupied || targetCells[i].isEnemyCell || targetCells[i] == null) continue;

                    pushMaximum = i;

                    break;
                }


                for (var i = 0; i < 4; i++)
                {

                    targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x - i, pushable.GetGridPos().y);

                    if (!combatController.ListOfPlayerCells.ContainsKey(targetCellCoords[i])) continue;

                    targetCells[i] = combatController.ListOfPlayerCells[targetCellCoords[i]];

                    if (targetCells[i].hasObstacle == true)
                    {
                        if (combatController.ListOfPlayerCells[new Vector2Int(pushable.GetGridPos().x - 1, pushable.GetGridPos().y)].isOccupied)
                        {
                            pushMaximum = 0;
                            break;
                        }

                        if (pushMaximum > i - 1)
                        {
                            pushMaximum = i - 1;
                        }
                    }
                }
            }

            else if (push == Push.north)
            {

                for (var i = 4 - 1; i > 0; i--)
                {
                    targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x, pushable.GetGridPos().y + i);

                    if (!combatController.ListOfPlayerCells.ContainsKey(targetCellCoords[i])) continue;

                    targetCells[i] = combatController.ListOfPlayerCells[targetCellCoords[i]];

                    if (targetCells[i].isOccupied || targetCells[i].isEnemyCell || targetCells[i] == null) continue;

                    pushMaximum = i;

                    break;
                }


                for (var i = 0; i < 4; i++)
                {

                    targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x, pushable.GetGridPos().y + i);

                    if (!combatController.ListOfPlayerCells.ContainsKey(targetCellCoords[i])) continue;

                    targetCells[i] = combatController.ListOfPlayerCells[targetCellCoords[i]];

                    if (targetCells[i].hasObstacle == true)
                    {
                        if (combatController.ListOfPlayerCells[new Vector2Int(pushable.GetGridPos().x, pushable.GetGridPos().y + 1)].isOccupied)
                        {
                            pushMaximum = 0;
                            break;
                        }

                        if (pushMaximum > i - 1)
                        {
                            pushMaximum = i - 1;
                        }
                    }
                }
            }

            else
            {

                for (var i = 4 - 1; i > 0; i--)
                {
                    targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x, pushable.GetGridPos().y - i);

                    if (!combatController.ListOfPlayerCells.ContainsKey(targetCellCoords[i])) continue;

                    targetCells[i] = combatController.ListOfPlayerCells[targetCellCoords[i]];

                    if (targetCells[i].isOccupied || targetCells[i].isEnemyCell || targetCells[i] == null) continue;

                    pushMaximum = i;

                    break;
                }


                for (var i = 0; i < 4; i++)
                {

                    targetCellCoords[i] = new Vector2Int(pushable.GetGridPos().x, pushable.GetGridPos().y - i);

                    if (!combatController.ListOfPlayerCells.ContainsKey(targetCellCoords[i])) continue;

                    targetCells[i] = combatController.ListOfPlayerCells[targetCellCoords[i]];

                    if (targetCells[i].hasObstacle == true)
                    {
                        if (combatController.ListOfPlayerCells[new Vector2Int(pushable.GetGridPos().x, pushable.GetGridPos().y - 1)].isOccupied)
                        {
                            pushMaximum = 0;
                            break;
                        }

                        if (pushMaximum > i - 1)
                        {
                            pushMaximum = i - 1;
                        }
                    }
                }
            }

            if (pushMaximum > 0)
            {
                pushable.GetComponent<Mover>().Pathfind(targetCells[pushMaximum]);
                pushable.GetComponent<Mover>().isBeingPushed = true;
                targetCells[pushMaximum].isOccupied = true;
            }
        }

        //CC.AA
        public static int SortByX(Fighter actor1, Fighter actor2)
        {
            return actor1.GetComponent<Mover>().GetGridPos().x.CompareTo(actor2.GetComponent<Mover>().GetGridPos().x);
        }

        //CC.AA
        public static int SortByX(Pushable pushable1, Pushable pushable2)
        {
            return pushable1.GetGridPos().x.CompareTo(pushable2.GetGridPos().x);
        }

        //CC.AA
        public static int SortByX(Destroyable destroyable1, Destroyable destroyable2)
        {
            return destroyable1.GetComponent<Pushable>().GetGridPos().x.CompareTo(destroyable2.GetComponent<Pushable>().GetGridPos().x);
        }

        //CC.AA
        public static int SortByY(Fighter actor1, Fighter actor2)
        {
            return actor1.GetComponent<Mover>().GetGridPos().y.CompareTo(actor2.GetComponent<Mover>().GetGridPos().y);
        }

        //CC.AA
        public static int SortByY(Pushable pushable1, Pushable pushable2)
        {
            return pushable1.GetGridPos().y.CompareTo(pushable2.GetGridPos().y);
        }

        //CC.AA
        public static int SortByY(Destroyable destroyable1, Destroyable destroyable2)
        {
            return destroyable1.GetComponent<Pushable>().GetGridPos().y.CompareTo(destroyable2.GetComponent<Pushable>().GetGridPos().y);
        }
    }
}
