using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TTW.Combat;

namespace TTW.UI
{
    public class PlayerSelector : MonoBehaviour
    {
        public int position = 0;
        Animator animator;
        CombatController combatController;

        private void Start()
        {
            combatController = FindObjectOfType<CombatController>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (combatController.actorInPosition[combatController.highlightedActorPosition] == combatController.actorInPosition[position])
            {
                animationStart();
            }
            else
            {
                animationStop();
            }
        }

        public void animationStart()
        {
            animator.SetBool("selected", true);
        }

        public void animationStop()
        {
            animator.SetBool("selected", false);
        }
    }
}