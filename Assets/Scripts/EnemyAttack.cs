using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using SWS;


namespace TTW.Combat
{
    public enum MagicType { none, sun, moon, lamp, electric, prysm};
    public enum StatusEffect { none, paralyze, madness, burn, blind, genera, angel, dasein, stop, dispel, bubble };
    public enum AttackTarget { none, beeline, volley };
    public enum Push { none, north, south, east, west};
    public enum TargettingClass { none, global, relative, support, homing, self, allAllies, allFoes, melee };


    [CreateAssetMenu(fileName = "New Enemy Attack", menuName = "Enemy Attack")]

    public class EnemyAttack : ScriptableObject
    {
        [Header("Name and Description")]
        public new string name;
        public string description;
        public Sprite artwork;

        [Header("Linked Ability")]
        public EnemyAttack linkedAbility;

        [Header("Targeting")]
        public TargettingClass targettingClass;
        public AttackTarget attackTarget;
        public Vector2Int[] globalTargetCells = new Vector2Int[12];
        public Vector2Int[] relativeTargetCells = new Vector2Int[12];

        [Header("Timers")]
        public float attackChannelTime;
        public float attackCD;

        [Header("Damage and Healing")]
        public DamageType damageType;
        public MagicType magicType;
        public float damageModifier;
        public float damageFlat;
        public float healAmount;

        [Header("Neutral State")]
        public NeutralState neutralState;
        public EnemyAttack changeCounterAttack;

        [Header("Status Effects")]
        public StatusEffect statusEffect;
        public float statusEffectTimer;

        [Header("Buffing and Nerfing")]
        public StatChange statChange;
        public int statChangeValue;

        [Header("Attack Variants")]
        public AttackVariant attackVariant;

        [Header("Pushing")]
        public bool pushIsGlobal;
        public Push push;
        public int pushForce;

        [Header("Object Creation")]
        public GameObject enemyCreate;
        public Trap trapCreate;
        public Pushable pushableCreate;

        [Header("Moving on Rails")]
        public bool usesRails;
        public PathManager[] onRailsPath = new PathManager[1];
        public Vector2Int[] RequiredCells = new Vector2Int[0];
        public Wing RequiredWing;
        public Vector2Int LandingCell; //uses relative and global targeting classes based on movement type

        [Header("Special Exceptions")]
        public bool desperation;
        public bool reversal;
    }
}
