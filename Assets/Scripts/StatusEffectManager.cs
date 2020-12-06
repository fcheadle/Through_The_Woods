using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class StatusEffectManager : MonoBehaviour
    {
        [SerializeField] public StatusEffect statusEffectGood;
        [SerializeField] public StatusEffect statusEffectBad;

        CombatController combatController;
        AttackReceiver attackReceiver;
        PlayerStats playerStats;

        float burnTimer;
        [SerializeField] float burnTimerMax = 2f;
        [SerializeField] float burnDamage = 2f;

        float generaTimer;
        [SerializeField] float generaTimerMax = 2f;
        [SerializeField] float generaHeal = 2f;

        [SerializeField] public float goodStatusTimer = 20f;
        float goodStatusTimerMax;

        [SerializeField] public float badStatusTimer = 20f;
        float badStatusTimerMax;



        // Start is called before the first frame update
        void Start()
        {
            combatController = FindObjectOfType<CombatController>();
            attackReceiver = GetComponent<AttackReceiver>();
            playerStats = GetComponent<PlayerStats>();

            goodStatusTimerMax = goodStatusTimer;
            badStatusTimerMax = badStatusTimer;
        }

        // Update is called once per frame
        void Update()
        {
            if (statusEffectGood != StatusEffect.none)
            {
                GoodStatusEffectTimer();
                GoodStatusEffectManager();
            }

            if (statusEffectBad != StatusEffect.none)
            {
                BadStatusEffectTimer();
                BadStatusEffectManager();
            }
        }

        private void BadStatusEffectManager()
        {
            switch (statusEffectBad)
            {
                case StatusEffect.burn:
                    if (burnTimer > 0)
                    {
                        if (combatController.currentState != CombatController.State.animationFreeze)
                            burnTimer -= Time.deltaTime;
                    }
                    else
                    {
                        attackReceiver.TakeDamage(burnDamage);
                        burnTimer = burnTimerMax;
                    }
                    break;
            }
        }

        private void GoodStatusEffectManager()
        {
            switch (statusEffectGood)
            {
                case StatusEffect.angel:
                    if (playerStats.isDead == true)
                    {
                        attackReceiver.Heal(playerStats.maxHealth, true);
                        playerStats.isDead = false;
                        statusEffectGood = StatusEffect.none;
                    }
                    break;

                case StatusEffect.genera:
                    if (generaTimer > 0)
                    {
                        if (combatController.currentState != CombatController.State.animationFreeze)
                            generaTimer -= Time.deltaTime;
                    }
                    else
                    {
                        attackReceiver.Heal(generaHeal, false);
                        generaTimer = generaTimerMax;
                    }
                    break;

                default:
                    break;
            }
        }

        private void GoodStatusEffectTimer()
        {
            if (goodStatusTimer > 0)
            {
                if (combatController.currentState != CombatController.State.animationFreeze)
                    goodStatusTimer -= Time.deltaTime;
            }
            else
            {
                statusEffectGood = StatusEffect.none;
                goodStatusTimer = goodStatusTimerMax;
            }
        }

        private void BadStatusEffectTimer()
        {
            if (badStatusTimer > 0)
            {
                if (combatController.currentState != CombatController.State.animationFreeze)
                    badStatusTimer -= Time.deltaTime;
            }
            else
            {
                statusEffectBad = StatusEffect.none;
                badStatusTimer = badStatusTimerMax;
            }
        }
    }
}
