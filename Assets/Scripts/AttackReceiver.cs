using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class AttackReceiver : MonoBehaviour
    {
        [SerializeField] public bool isMirroring = false;
        [SerializeField] public bool isCountering = false;
        [SerializeField] public bool isProtected = false;
        [SerializeField] public bool isGuarding = false;
        [SerializeField] public bool isCloaked = false;
        [SerializeField] public bool isPhased = false;
        [SerializeField] public bool isFocused = false;
        [SerializeField] public bool isInvulnerable = false;
        [SerializeField] public bool hasGuardian = false;
        [SerializeField] public AttackReceiver protector = null;
        [SerializeField] public bool isProtecting = false;


        PlayerStats stats;
        StatusEffectManager statusEffectManager;
        AnimationHandler animationHandler;

        public float bubbleHealth = 100f;
        public float maxBubbleHealth = 100f;
        public int bubbleInstances = 1;
        public int maxBubbleInstances = 1;


        // Start is called before the first frame update
        void Start()
        {
            stats = GetComponent<PlayerStats>();
            statusEffectManager = GetComponent<StatusEffectManager>();
            animationHandler = GetComponent<AnimationHandler>();
        }

        public bool IsMirroring()
        {
            return isMirroring;
        }

        public bool IsCountering()
        {
            return isCountering;
        }

        public bool IsProtected()
        {
            return isProtected;
        }

        public bool IsFocused()
        {
            return isFocused;
        }

        public bool IsGuarding()
        {
            return isGuarding;
        }

        public bool IsCloaked()
        {
            return isCloaked;
        }

        public bool IsPhased()
        {
            return isPhased;
        }

        public bool IsInvulnerable()
        {
            return isInvulnerable;
        }

        public bool HasGuardian()
        {
            return hasGuardian;
        }

        public Zodiac GetZodiac()
        {
            return stats.zodiac;
        }

        public float GetArmor()
        {
            return stats.armor;
        }

        public float GetEvasion()
        {
            return stats.evasion;
        }

        public float GetStrength()
        {
            return stats.strength;
        }

        public float GetSpeed()
        {
            return stats.speed;
        }

        public void Heal(float healing, bool canRevive)
        {
            if (GetComponent<PlayerStats>().isDead && !canRevive)
            {
                print(this + " is Dead. :(");
                return;
            }


            stats.health += healing;

            print("healing " + healing + " amount");

            if (stats.health > stats.maxHealth)
            {
                stats.health = stats.maxHealth;
            }
        }

        public void TakeDamage(float damage)
        {

            if (statusEffectManager.statusEffectGood == StatusEffect.bubble)
            {
                damage = ApplyBubbleShield(damage);
            }

            if (damage <= 0f) return;

            if (GetComponent<Channel>().isChanneling && !isFocused)
            {
                if (GetComponent<Fighter>() != null)
                {
                    GetComponent<Channel>().BreakChannel();
                }
            }

            stats.health -= damage;

            if (stats.health < 0f)
            {
                stats.health = 0f;
            }
        }

        public AttackReceiver GetProtector()
        {
            return protector;
        }

        public void Dispel()
        {
            statusEffectManager.statusEffectGood = StatusEffect.none;
            statusEffectManager.statusEffectBad = StatusEffect.none;
        }

        public void ApplyGoodStatusEffect(StatusEffect newStatusEffect, float statusTimer)
        {
            if (statusEffectManager.statusEffectGood == StatusEffect.none)
            {
                statusEffectManager.statusEffectGood = newStatusEffect;
                statusEffectManager.goodStatusTimer = statusTimer;
            }
        }

        public void ApplyBadStatusEffect(StatusEffect newStatusEffect, float statusTimer)
        {
            if (statusEffectManager.statusEffectBad == StatusEffect.none)
            {
                statusEffectManager.statusEffectBad = newStatusEffect;
                statusEffectManager.badStatusTimer = statusTimer;
            }
        }

        private float ApplyBubbleShield(float damage)
        {
            float returnDamage = damage - bubbleHealth;

            bubbleHealth -= damage;
            
            print("bubble shield absorbed " + damage + " incoming damage.");

            if (bubbleHealth <= 0)
            {
                bubbleInstances--;
                bubbleHealth = maxBubbleHealth;

                if (bubbleInstances <= 0)
                {
                    statusEffectManager.statusEffectGood = StatusEffect.none;
                    //bubbleInstances = maxBubbleInstances;
                }
            }
            else
            {
                return 0;
            }

            return returnDamage;
        }

        public void SetNewCounter(EnemyAttack counter)
        {
            GetComponent<SpellsEquipped>().counterAttack = counter;
        }

        public void SetNewCounter(Ability counter)
        {
            GetComponent<EnemySpells>().counterAttack = counter;
        }

        public void BreakNeutralState(bool includeProtectee)
        {
            print("breaking neutral state");
            isMirroring = false;
            isCountering = false;

            isGuarding = false;
            animationHandler.StopDefend();
            isCloaked = false;
            isPhased = false;
            isFocused = false;
            isInvulnerable = false;

            //potentially damage reduction when adjacent?
            //for now protector can move around freely

            //if (isProtecting)
            //{
            //    AttackReceiver[] allies = FindObjectsOfType<AttackReceiver>();
            //    isProtecting = false;
            //    foreach (AttackReceiver ally in allies)
            //    {
            //        if (ally.protector == this)
            //        {
            //            ally.isProtected = false;
            //        }
            //    }
            //}


            if (isProtected && includeProtectee)
            {
                isProtected = false;
                protector.isProtecting = false;
            }
        }
    }
}


