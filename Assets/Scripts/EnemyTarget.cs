using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TTW.Combat
{
    public enum Wing { none, port, starboard, bow };

    public class EnemyTarget : MonoBehaviour
    {
        Mover mover;
        public Enemy enemy;
        public float health;
        float maxHealth;
        public int speed;
        public int strength;
        public int evasion;
        public int armor;
        
        public Wing wing;
        CombatController combatController;
        CoolDown coolDown;
        Channel channel;
        DeathHandler deathHandler;
        public Zodiac zodiac;
        EnemyAttack counterAttack;



        private void Awake()
        {
            mover = GetComponent<Mover>();
            coolDown = GetComponent<CoolDown>();
            channel = GetComponent<Channel>();
            combatController = FindObjectOfType<CombatController>();
            deathHandler = GetComponent<DeathHandler>();
            
            zodiac = enemy.zodiac;
            UpdateWingPosition();

            speed = enemy.speed;
            strength = enemy.strength;
            evasion = enemy.evasion;
            armor = enemy.armor;
            health = enemy.health;
            maxHealth = enemy.health;

            counterAttack = enemy.counterAttack;

        }

        //keep
        public void UpdateWingPosition()
        {
            if (mover.GetGridPos().y == 3)
            {
                wing = Wing.bow;
                return;
            }

            if (mover.GetGridPos().x == 0)
            {
                wing = Wing.port;
                return;
            }

            if (mover.GetGridPos().x == 5)
            {
                wing = Wing.starboard;
                return;
            }
        }
    }
}
