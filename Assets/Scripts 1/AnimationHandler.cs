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
        public ProtectGhostFX myProtectFX;

        public float animTimer = 200f;
        public float maxAnimTimer = 1000f;
        public bool isAnimating = false;

        public Animator animator;

        public List<AttackReceiver> savedTargets;
        EnemyAttack savedEnemyAttack;
        Ability savedAbility;
        float savedDamage;


        void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            combatController = FindObjectOfType<CombatController>();
            stateMachine = GetComponent<StateMachine>();
            savedTargets = new List<AttackReceiver>();
            savedDamage = 0f;
        }

        private void Update()
        {
            AnimationTimer();
            DeathCheck();
        }

        private void DeathCheck()
        {
            if (combatController.currentState == CombatController.State.animationFreeze) return;

            if (GetComponent<PlayerStats>().isDead)
            {
                GetDead();
            }
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
                        //print("animation ending via timer");
                    }
                }
            }
        }

        public void SetDamage(int damage)
        {
            animator.SetInteger("damage", damage);
        }

        public void SetAnimationTimer(float newTimer)
        {
            animTimer = newTimer;
        }

        public void EndWeaponAttack()
        {
            //print("end attack animation");
            animator.SetBool("weaponAttack", false);
            animator.SetBool("rangedAttack", false);
            animator.SetBool("isCasting", false);
        }

        public void DoWeaponAttack()
        {
            animator.SetBool("weaponAttack", true);
        }

        public void DoMagicalAttack()
        {
            animator.SetBool("isCasting", true);
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

        public void Run()
        {
            animator.SetBool("isRunning", true);
        }

        public void StopRun()
        {
            animator.SetBool("isRunning", false);
        }

        public void Defend()
        {
            //print("starting defense");
            animator.SetBool("isDefending", true);
        }

        public void StopDefend()
        {
            animator.SetBool("isDefending", false);
        }

        public void GetHurtLight()
        {
            //if (damage == 0f)
            //{
            //    return;
            //}

            //print("object " + this + " is taking light damage");
            
            if (GetComponent<PlayerStats>().isDead)
            {
                print("character is dead");
                animator.SetTrigger("takeDamageLight");
                StopDefend();

                GetDead();
            }
            else
            {
                print("object " + this + " is taking light damage");
                StopDefend();
                animator.SetTrigger("takeDamageLight");
            }
        }

        public void GetDead()
        {
            animator.SetBool("isDead", true);
        }

        public void DealDamage()
        {
            print("savedTarget count = " + savedTargets.Count);

            if (savedTargets.Count < 1) return;

            print("dealing damage");
            print("enemies saved: " + savedTargets.Count);



            foreach (AttackReceiver target in savedTargets)
            {
                //print("target name: " + target);

                //if (target.isProtected)
                //{
                //    savedTargets.Add(GetTargetProtector(target));
                //    return;
                //}

                target.GetComponent<AnimationHandler>().GetHurtLight();

                //if (savedAbility.endingVFX != null)
                //{
                //    Vector3 yOffset = new Vector3(0f, 5f, 0f);
                //    VFX newVFX = Instantiate(savedAbility.endingVFX);
                //    newVFX.transform.position = target.transform.position + yOffset;
                //}
            }

            
            ClearTargetCache();

        }

        public void DealDamageByInstance(AttackReceiver targetInstance)
        {
            if (!savedTargets.Contains(targetInstance)) return;

            //print("dealing damage by instance");
            //print("enemies saved: " + savedTargets.Count);


            print("target name: " + targetInstance);
            if (targetInstance.isProtected)
            {
                savedTargets.Remove(targetInstance);
                SpawnProtectorVFX(targetInstance);
                targetInstance = GetTargetProtector(targetInstance);
            }

            targetInstance.GetComponent<AnimationHandler>().GetHurtLight();

            savedTargets.Remove(targetInstance);

            if (savedTargets.Count < 1)
            {
                ClearTargetCache();
            }

            //EDIT THIS TO SCALE ANIMATION BASED ON INCOMING DAMAGE, USE SAVED ENEMY ATTACK AND SAVED ABILITY FOR THIS
        }

        public void DoRangedAttack()
        {
            animator.SetBool("rangedAttack", true);
        }

        public void SpawnProjectiles()
        {
            //if (savedAbility.projectile != null)
            //{
            //    foreach (AttackReceiver target in savedTargets)
            //    {
            //        Projectile newProjectile = Instantiate(savedAbility.projectile);
            //        newProjectile.originPosition = transform.position;
            //        newProjectile.targetTransform = target.transform;
            //        newProjectile.caster = this;
            //    }
            //}

            EndWeaponAttack();
        }

        private void SpawnProtectorVFX(AttackReceiver attackReceiver)
        {
            Vector3 yOffset = new Vector3(5f, 5f, 0f);
            ProtectGhostFX protectFX = Instantiate(myProtectFX);
            protectFX.transform.position = attackReceiver.transform.position + yOffset;
            protectFX.currentSprite = attackReceiver.protector.GetComponent<FXHandler>().protectFX;
        }

        private static AttackReceiver GetTargetProtector(AttackReceiver targetInstance)
        {
            AttackReceiver oldTarget = targetInstance;
            
            print("switching target to protector");
            targetInstance = targetInstance.protector;
            oldTarget.isProtected = false;
            targetInstance.isProtecting = false;
            return targetInstance;
        }

        public void ClearTargetCache()
        {
            savedTargets.Clear();
            savedAbility = null;
            savedDamage = 0f;
            savedEnemyAttack = null;
        }

        public void SaveAttackInfoTargets(Ability ability, List<AttackReceiver> targets)
        {
            savedAbility = ability;

            foreach (AttackReceiver target in targets)
            {
                //print("save target name as: " + target);
                savedTargets.Add(target);
            }
        }

        public void SaveAttackInfoDamage(float damage)
        {
            savedDamage = damage;
        }

        public void SaveAttackInfoTargets(EnemyAttack ability, List<AttackReceiver> targets)
        {
            savedEnemyAttack = ability;

            foreach (AttackReceiver target in targets)
            {
                //print("save target name as: " + target);
                savedTargets.Add(target);
            }
        }
    }
}
