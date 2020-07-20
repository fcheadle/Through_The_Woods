using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class PlayerStats : MonoBehaviour
    {

        public float maxHealth = 0;
        public float health = 0;
        public float strength = 0;
        public float speed = 0;
        public float evasion = 0;
        public float armor = 0;
        public Zodiac zodiac;

        public bool isDead = false;

        //these are static values that will be pulled from the combatController list of static values;
        public float counterRatio = 1.5f;
        public float splashRatio = 0.5f;
        public float critRatio = 1.5f;
        public float criticalHit = 180f;

        Fighter fighter;
        EnemyTarget enemyTarget;

        // Start is called before the first frame update
        void Start()
        {
            if (GetComponent<Fighter>() != null)
            {
                fighter = GetComponent<Fighter>();
                ResetStats(true);
            }

            if (GetComponent<EnemyTarget>() != null)
            {
                enemyTarget = GetComponent<EnemyTarget>();
                ResetStats(true);
            }
        }

        private void Update()
        {
            DeathCheck();
        }

        private void DeathCheck()
        {
            if (health <= 0)
            {
                if (GetComponent<StatusEffectManager>().statusEffectGood == StatusEffect.dasein)
                {
                    print("survived by their inner commitment to just being!");
                    health = 1f;
                    return;
                }

                isDead = true;
            }
            else
            {
                isDead = false;
            }
        }

        public void ResetStats(bool doIncludeHealth)
        {
            if (GetComponent<Fighter>() != null)
            {
                if (doIncludeHealth)
                {
                    health = fighter.health;
                    maxHealth = fighter.health;
                }
                strength = fighter.strength;
                speed = fighter.speed;
                evasion = fighter.evasion;
                armor = fighter.armor;
                zodiac = fighter.zodiac;
            }

            if (GetComponent<EnemyTarget>() != null)
            {
                if (doIncludeHealth)
                {
                    health = enemyTarget.health;
                    maxHealth = enemyTarget.health;
                }
                strength = enemyTarget.strength;
                speed = enemyTarget.speed;
                evasion = enemyTarget.evasion;
                armor = enemyTarget.armor;
                zodiac = enemyTarget.zodiac;
            }
        }
    }
}
