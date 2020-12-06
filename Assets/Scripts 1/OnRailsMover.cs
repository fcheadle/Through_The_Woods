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
        Wing savedWing;

        void Start()
        {
            mover = GetComponent<Mover>();
            enemyTarget = GetComponent<EnemyTarget>();
            combatController = FindObjectOfType<CombatController>();
            spline = GetComponent<splineMove>();
            savedWing = GetComponent<EnemyTarget>().wing;
        }

        void Update()
        {
            if (doCheckFinalCellEnemyAttack)
            {
                CheckFinalCellEnemyAttack();
            }

            if (isMoving)
            {
                CollisionDetector();
            }
        }

        private void CheckFinalCellEnemyAttack()
        {
            if (spline.waypoints[spline.currentPoint] == spline.waypoints[spline.waypoints.Length-1])
            {
                EndEnemyAttack();
                GetComponent<AnimationHandler>().StopRun();
                combatController.ListOfcells[GetComponent<Mover>().GetGridPos()].isOccupied = true;

                if (GetComponent<EnemyTarget>().wing != savedWing)
                {
                    GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
                }
            }

        }

        private void CollisionDetector()
        {
            if (GetComponent<AnimationHandler>().savedTargets.Count < 1) return;

            Fighter[] fighters = FindObjectsOfType<Fighter>();

            foreach(Fighter fighter in fighters)
            {
                if (this.GetComponent<Mover>().GetGridPos() == fighter.GetComponent<Mover>().GetGridPos())
                {
                    
                    GetComponent<AnimationHandler>().DealDamageByInstance(fighter.GetComponent<AttackReceiver>());
                }
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
            //GameEvents.current.AnimationStart();
            pathMover.StartMove();
            spline.currentPoint = 0;
            doCheckFinalCellEnemyAttack = true;
            isMoving = true;
            combatController.ListOfcells[GetComponent<Mover>().GetGridPos()].isOccupied = false;
            GetComponent<AnimationHandler>().Run();
            savedWing = GetComponent<EnemyTarget>().wing;
        }
    }
}


