using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class AnimationHandler : MonoBehaviour
    {
        public Sprite front;
        public Sprite back;
        public Sprite left;
        public Sprite right;
        SpriteRenderer spriteRenderer;
        CombatController combatController;
        StateMachine stateMachine;

        public float animTimer = 200f;
        public float maxAnimTimer = 1000f;
        public bool isAnimating = false;

        public Animator animator;


        void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            combatController = FindObjectOfType<CombatController>();
            stateMachine = GetComponent<StateMachine>();

        }

        private void Update()
        {
            AnimationTimer();
        }

        private void AnimationTimer()
        {
            if (animTimer > 0)
            {

                isAnimating = true;
                animTimer -= Time.deltaTime;
            }

            if (animTimer <= 0)
            {

                if (isAnimating == true)
                {
                    isAnimating = false;

                    if (combatController.IsEndAnimationReady())
                    {
                        GameEvents.current.AnimationEnd();
                    }
                }
            }
        }

        public void SetAnimationTimer(float newTimer)
        {
            animTimer = newTimer;
            
        }

        public void EndWeaponAttack()
        {
            animator.SetBool("weaponAttack", false);

            print("checking animation end conditions");

            if (combatController.IsEndAnimationReady())
            {
                GameEvents.current.AnimationEnd();
                print("animation ending");
            }


        }

        public void DoWeaponAttack()
        {
            animator.SetBool("weaponAttack", true);
        }

        public void MoveRight()
        {
            animator.SetBool("idle", false);
            animator.SetBool("left", false);
            animator.SetBool("up", false);
            animator.SetBool("down", false);
            animator.SetBool("right", true);
        }

        public void MoveLeft()
        {
            animator.SetBool("idle", false);
            animator.SetBool("left", true);
            animator.SetBool("up", false);
            animator.SetBool("down", false);
            animator.SetBool("right", false);
        }

        public void MoveUp()
        {
            animator.SetBool("idle", false);
            animator.SetBool("left", false);
            animator.SetBool("up", true);
            animator.SetBool("down", false);
            animator.SetBool("right", false);
        }

        public void MoveDown()
        {
            animator.SetBool("idle", false);
            animator.SetBool("left", false);
            animator.SetBool("up", false);
            animator.SetBool("down", true);
            animator.SetBool("right", false);
        }

        public void BeIdle()
        {
            animator.SetBool("idle", true);
            animator.SetBool("left", false);
            animator.SetBool("up", false);
            animator.SetBool("down", false);
            animator.SetBool("right", false);
        }
    }
}
