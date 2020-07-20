using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;


namespace TTW.Combat
{
    public class OnRailsMover : MonoBehaviour
    {
        public bool isMoving;

        Mover mover;
        EnemyTarget enemyTarget;
        CombatController combatController;
        splineMove spline;

        bool doCheckFinalCellEnemyAttack = false;

        void Start()
        {
            mover = GetComponent<Mover>();
            enemyTarget = GetComponent<EnemyTarget>();
            combatController = FindObjectOfType<CombatController>();
            spline = GetComponent<splineMove>();
        }

        void Update()
        {
            if (doCheckFinalCellEnemyAttack)
            {
                CheckFinalCellEnemyAttack();
            }
        }

        private void CheckFinalCellEnemyAttack()
        {
            if (spline.waypoints[spline.currentPoint] == spline.waypoints[spline.waypoints.Length-1])
            {
                EndEnemyAttack();
                combatController.ListOfcells[GetComponent<Mover>().GetGridPos()].isOccupied = true;
            }

        }

        private void EndEnemyAttack()
        {

            mover.UpdateCell();
            enemyTarget.UpdateWingPosition();
            doCheckFinalCellEnemyAttack = false;
            isMoving = false;

            if (combatController.IsEndAnimationReady())
            {
                GameEvents.current.AnimationEnd();
            }
        }

        public void PerformAttackPath(splineMove pathMover)
        {
            GameEvents.current.AnimationStart();
            pathMover.StartMove();
            spline.currentPoint = 0;
            doCheckFinalCellEnemyAttack = true;
            isMoving = true;
            combatController.ListOfcells[GetComponent<Mover>().GetGridPos()].isOccupied = false;
        }
    }
}


