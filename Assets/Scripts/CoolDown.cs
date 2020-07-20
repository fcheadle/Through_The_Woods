using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private void Start()
        {
            combatController = FindObjectOfType<CombatController>();
            stateMachine = GetComponent<StateMachine>();
        }

        private void Update()
        {
            CheckCoolDown();
            speedRatio = originalSpeedRatio * GetComponent<PlayerStats>().speed;
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
                stateMachine.state = State.cooldown;
            }

            if (cd <= 0)
            {
                isOnCD = false;
                //stateMachine.state = State.neutral;
            }
        }

        public void SetCoolDown(float newCD)
        {
            cd = newCD;
            maxCD = newCD;
        }
    }
}
