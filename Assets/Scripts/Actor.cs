using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{

    public enum Zodiac {
        spirit,
        oak,
        urn,
        sword,
        beast,
        coin,
        torch };

    [CreateAssetMenu(fileName = "New Actor", menuName = "Actor")]

    public class Actor : ScriptableObject
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
        public Mooring mooring;
    }
}