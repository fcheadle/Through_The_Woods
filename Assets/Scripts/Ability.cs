using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TTW.Combat
{
    public enum ActorTargetingType { none, melee, ranged, support, self, cell, allallies, allfoes, supportNotSelf, adjacent };
    public enum DamageType { physical, magical, mixed, healing };
    public enum NeutralState { none, guard, protect, counter, cloak, invulnerable, phase, mirror, bubble, guardian, focus };
    public enum StatChange { none, speed, armor, evasion, strength, humble, random, all };
    public enum Displacement { none, dash, swap, gravity, repel };
    public enum AttackVariant { none, lifesteal, critical, splash };


    [CreateAssetMenu(fileName = "New Ability", menuName = "Ability")]

    public class Ability : ScriptableObject
    {
        [Header("Name and Description")]
        public new string name;
        public string description;
        public Sprite artwork;

        [Header("Linked Ability")]
        public Ability linkedAbility;

        [Header("Targeting")]
        public ActorTargetingType actorTargetingType;

        [Header("Timers")]
        public float attackChannelTime;
        public float attackCD;

        [Header("Damage and Healing")]
        public DamageType damageType;
        public MagicType magicType;
        public float damageModifier;
        public float damageFlat;
        public float healAmount;
        public bool canRevive;

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
        public Displacement displacement;

        [Header("Object Creation")]
        public Trap trap;
        public Pushable pushableCreate;

        [Header("Special Rules")]
        public bool desperation;
        public bool reversal;
        public bool helpPlease;
        public bool guardian;
        public bool nakama;
        public bool swap;
        public bool push;
        public bool pull;
        public bool leap;
        public bool swanSong;
        public bool legendary;
    }
}
