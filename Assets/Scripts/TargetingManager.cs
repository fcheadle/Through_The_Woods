using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class TargetingManager : MonoBehaviour
    {
        Mover mover;

        // Start is called before the first frame update
        void Start()
        {
            mover = GetComponent<Mover>();
        }

        public bool MeleeTargeting(EnemyTarget target)
        {
            Pushable[] pushables = FindObjectsOfType<Pushable>();
            Vector2Int targetPosition = target.GetComponent<Mover>().GetGridPos();
            Vector2Int actorPosition = mover.GetGridPos();

            if (actorPosition.x == targetPosition.x)
            {

                foreach (Pushable pushable in pushables)
                {
                    Vector2Int pushablePosition = pushable.GetComponent<Mover>().GetGridPos();

                    if (pushablePosition.x == actorPosition.x)
                    {
                        if (pushablePosition.y > actorPosition.y && pushablePosition.y < targetPosition.y)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            if (actorPosition.y == targetPosition.y)
            {
                foreach (Pushable pushable in pushables)
                {
                    Vector2Int pushablePosition = pushable.GetComponent<Mover>().GetGridPos();

                    if (pushablePosition.y == actorPosition.y)
                    {
                        if (target.wing == Wing.port)
                        {
                            if (pushablePosition.x < actorPosition.x && pushablePosition.x > targetPosition.x)
                            {
                                return false;
                            }
                        }

                        if (target.wing == Wing.starboard)
                        {
                            if (pushablePosition.x > actorPosition.x && pushablePosition.x < targetPosition.x)
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }

            return false;
        }

        public bool CellTargeting(Cell cell)
        {
            if (cell.isOccupied)
            {
                return false;
            }

            if (cell.hasTrap)
            {
                return false;
            }

            if (cell.hasObstacle)
            {
                return false;
            }

            return true;
        }

        public bool SupportTargeting(Fighter fighter)
        {
            if (fighter.isDead == true)
            {
                return false;
            }

            return true;
        }
    }
}