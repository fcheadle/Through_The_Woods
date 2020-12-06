using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TTW.UI;
using System;

namespace TTW.Combat
{
    public class CoolDown : MonoBehaviour
    {
        public float cd = 200f;
        public float maxCD = 1000f;
        public bool isOnCD = false;
        [SerializeField] float originalSpeedRatio = 0.1f;
        public float speedRatio;

        CombatController combatController;
        StateMachine stateMachine;
        ShuffleCoolDowns uiController;
        public State state;

        private void Start()
        {
            combatController = FindObjectOfType<CombatController>();
            stateMachine = GetComponent<StateMachine>();
            uiController = FindObjectOfType<ShuffleCoolDowns>();
            uiController.SortCoolDowns();
            state = State.neutral;
        }

        private void Update()
        {
            CheckCoolDown();
            TransferState();
            speedRatio = originalSpeedRatio * GetComponent<PlayerStats>().speed;
        }

        private void TransferState()
        {
            state = stateMachine.state;
        }

        private void CheckCoolDown()
        {
            if (cd > 0)
            {
                if (combatController.currentState == CombatController.State.animationFreeze)
                {
                    return;
                }

                isOnCD = true;
                cd -= Time.deltaTime * speedRatio;

                if (stateMachine.state != State.cooldown)
                {
                    stateMachine.Cooldown();
                }
            }

            if (cd <= 0)
            {
                if (isOnCD)
                {
                    isOnCD = false;
                    uiController.SortCoolDowns();
                    stateMachine.Neutral();
                    
                }
                
                //stateMachine.state = State.neutral;
            }
        }

        public void SetCoolDown(float newCD)
        {
            cd = newCD;
            maxCD = newCD;
            isOnCD = true;
            uiController.SortCoolDowns();
            stateMachine.Cooldown();
        }
    }
}
