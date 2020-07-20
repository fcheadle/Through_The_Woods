using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{


    [CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]

    public class Enemy : ScriptableObject
    {
        public new string name;
        public Sprite artwork;

        public int health;
        public int cooldown;
        public int strength;
        public int armor;
        public int evasion;
        public int speed;
        public Zodiac zodiac;
        public EnemyAttack counterAttack;
    }
}