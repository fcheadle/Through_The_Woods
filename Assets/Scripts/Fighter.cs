using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class Fighter : MonoBehaviour
    {

        public EnemyTarget enemyTarget;
        CoolDown cooldown;
        Channel channel;
        float cd = 0f;
        float maxCD = 100f;
        public float health = 0;
        public float maxHealth;
        public float strength = 0;
        public float speed = 0;
        public float evasion = 0;
        public float armor = 0;
        const float armorRatio = 0.01f;
        public Zodiac zodiac;
        public Actor actor;
        public EnemyTarget newTarget;
        

        //rest states
        public bool isGuarding = false;
        public bool isCloaked = false;
        public bool isInvulnerable = false;
        public bool isIntangible = false;
        public bool isPhased = false;
        public bool isMirrored = false;
        public bool isCountering = false;
        public bool isDead = false;
        public Fighter protector = null;
        public Ability counterAbility = null;

        public float counterValue;
        public static Fighter art;

        public bool usedLegendary = false;

        SpellsEquipped spellsEquipped = null;

        public Ability[] abilities = new Ability[4];
        Ability savedAbility;

        CombatController combatController;
        TargetingManager targetingManager;
        AttackExecutor attackExecutor;
        AnimationHandler animationHandler;
        PlayerStats playerStats;
        //Renderer materialRenderer;
        Mover mover;

        void Awake()
        {
            cooldown = GetComponent<CoolDown>();
            cooldown.isOnCD = false;

            art = this;

            health = actor.health;
            maxHealth = actor.health;
            strength = actor.armor;
            speed = actor.speed;
            evasion = actor.evasion;
            armor = actor.armor;
            zodiac = actor.zodiac;

            spellsEquipped = GetComponent<SpellsEquipped>();
            abilities[0] = spellsEquipped.ability[0];
            abilities[1] = spellsEquipped.ability[1];
            abilities[2] = spellsEquipped.ability[2];
            abilities[3] = spellsEquipped.ability[3];

            animationHandler = GetComponent<AnimationHandler>();

            combatController = FindObjectOfType<CombatController>();
            mover = GetComponent<Mover>();
            cooldown = GetComponent<CoolDown>();
            channel = GetComponent<Channel>();
            targetingManager = GetComponent<TargetingManager>();
            attackExecutor = GetComponent<AttackExecutor>();
            playerStats = GetComponent<PlayerStats>();
            //materialRenderer = GetComponent<Renderer>();


            savedAbility = null;
        }

        private void Start()
        {
            GameEvents.current.onGetActorSelection += ReturnActorSelection;
        }

        private void Update()
        {
            animationHandler.SetDamage(playerStats.AnimateHealth());
        }

        //public void TakeDamage(float damage, DamageType damageType)
        //{

        //    if (statusEffectGood == StatusEffect.bubble)
        //    {
        //        BubbleShield(damage);
        //        return;
        //    }

        //    if (isGuarding && damageType == DamageType.physical)
        //    {
        //        health -= (damage/2);
        //        print(this.name + " took " + (damage/2) + " " + damageType.ToString() + " damage.");
        //    }
        //    else if (isCloaked && damageType == DamageType.magical)
        //    {
        //        health -= (damage / 2);
        //        print(this.name + " took " + (damage/2) + " " + damageType.ToString() + " damage.");
        //    }
        //    else
        //    {
        //        health -= damage;
        //        print(this.name + " took " + (damage) + " " + damageType.ToString() + " damage.");
        //    }


        //    if (health < 1f)
        //    {
        //        if (statusEffectGood != StatusEffect.dasein)
        //        {
        //            health = 0;
        //            isDead = true;
        //        }
        //        else
        //        {
        //            health = 1f;
        //        }
        //    }
        //}

        public void Heal(float healing)
        {
            health += healing;

            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }

        public void GoOnCoolDown()
        {
            cd = maxCD;
        }

        public void StartChannel(List<AttackReceiver> targets, int abilityInPosition)
        {
            GetComponent<AttackReceiver>().BreakNeutralState(false);

            if (abilities[abilityInPosition].attackChannelTime > 0)
            {
                channel.StartChannelActor(targets, abilities[abilityInPosition]);
            }
            else
            {
                PerformAttack(targets, abilities[abilityInPosition]);
            }
        }

        //used for all other attack targeting types
        public void PerformAttack(List<AttackReceiver> targets, Ability ability)
        {
            GetComponent<AttackReceiver>().BreakNeutralState(false);


            attackExecutor.PerformAttack(ability, targets, this, false);

            if (ability.linkedAbility != null)
            {
                attackExecutor.PerformAttack(ability.linkedAbility, targets, this, true);
            }
        }

        //ONLY USED FOR CELLS
        public void PerformAttack(Cell cell, int abilityInPosition)
        {
            print("performing " + abilities[abilityInPosition] +  " at: " + cell.GetGridPos());

            savedAbility = abilities[abilityInPosition];

            if (savedAbility.trap != null)
            {
                CreateTrap(cell, savedAbility);
            }

            if (savedAbility.pushableCreate != null)
            {
                CreatePushable(cell, savedAbility);
            }

            if (savedAbility.leap)
            {

                Vector3 cellPosition = cell.transform.position;
                Vector3 casterPosition = transform.position;

                transform.position = cellPosition;
            }

            EndOfAttack();
            GoOnCoolDown();
        }

        private void CreatePushable(Cell cell, Ability savedAbility)
        {
            cell.SetPushable(savedAbility.pushableCreate);
        }

        private void CreateTrap(Cell cell, Ability ability)
        {
            cell.SetTrap(ability.trap);
        }

        public void Remove()
        {

            for (var i = 0; i < combatController.actorInPosition.Count; i++)
            {
                if (combatController.actorInPosition[i] == this)
                {
                    combatController.actorInPosition.Remove(this);
                    combatController.actorsRemoved.Add(this);
                    //combatController.selectorInPosition.Remove(combatController.selectorInPosition[i]);

                    combatController.ResetHighlighters();
                    combatController.AddBenchedActor(this);

                    gameObject.SetActive(false);

                    break;
                }
            }
        }

        public void ReturnActorSelection()
        {
            int abilityInPosition = combatController.highlightedAttackPosition;

            switch (abilities[abilityInPosition].actorTargetingType)
            {
                case ActorTargetingType.melee:
                    GameEvents.current.ActorSelectMelee();
                    //print("attack type is melee");
                    break;
                case ActorTargetingType.ranged:
                    GameEvents.current.ActorSelectRanged();
                    //print("attack type is ranged");
                    break;
                case ActorTargetingType.cell:
                    GameEvents.current.ActorSelectCell();
                    //print("attack type is cell");
                    break;
                case ActorTargetingType.support:
                    GameEvents.current.ActorSelectSupport();
                    //print("attack type is support");
                    break;
                case ActorTargetingType.self:
                    GameEvents.current.ActorSelectSelf();
                    //print("attack type is self");
                    break;
                case ActorTargetingType.allallies:
                    GameEvents.current.ActorSelectAllies();
                    //print("attack type is allAllies");
                    break;
                case ActorTargetingType.allfoes:
                    GameEvents.current.ActorSelectFoes();
                    //print("attack type is allFoes");
                    break;
                case ActorTargetingType.supportNotSelf:
                    GameEvents.current.ActorSelectSupportNotSelf();
                    //print("attack type is supportNotSelf");
                    break;
                case ActorTargetingType.adjacent:
                    GameEvents.current.ActorSelectAdjacent();
                    //print("attack type is adjacent");
                    break;
                case ActorTargetingType.cardinalAlly:
                    GameEvents.current.ActorSelectCardinalAllies();
                    //print("attack type is select cardinal ally");
                    break;
                default:
                    Debug.LogError("no valid targeting type detected.");
                    break;
            }
        }

        public bool CheckAttack(EnemyTarget target, int abilityInPosition)
        {

            if (abilities[abilityInPosition].actorTargetingType == ActorTargetingType.melee)
            {
                if (!targetingManager.MeleeTargeting(target))
                {
                    return false;
                }
            }

            return true;

        }

        //private void DoAbility(EnemyTarget target, Ability ability)
        //{
        //    if (ability.statusEffect != StatusEffect.none)
        //    {
        //        target.ApplyBadStatusEffect(ability.statusEffect);
        //    }

        //    DistributeDamage(ability, target);
        //    EndOfAttack();
        //}

        //private void DoAbility(Fighter target, Ability ability)
        //{
        //    if (ability.statusEffect != StatusEffect.none)
        //    {
        //        target.ApplyBadStatusEffect(ability.statusEffect);
        //    }

        //    DistributeDamage(ability, target);
        //    EndOfAttack();
        //}

        public void AttackAnimationEnd()
        {
            //combatController.AnimationFreezeEnd();
            //combatController.ResetState();
            animationHandler.EndWeaponAttack();
        }

        //private bool CheckAbilityOneHit(EnemyTarget newTarget)
        //{
        //    if (statusEffectBad == StatusEffect.blind && savedAbility.damageType == DamageType.physical)
        //    {
        //        print("ATTACK MISSED!");
        //        return false;
        //    }

        //    if (statusEffectBad == StatusEffect.blind && savedAbility.damageType == DamageType.mixed)
        //    {
        //        print("ATTACK MISSED!");
        //        return false;
        //    }

        //    float randomNumber = UnityEngine.Random.Range(0f, 255f);

        //    float evasionValue = (newTarget.evasion * 35) - 350;

        //    if (randomNumber >= evasionValue)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        print("ATTACK MISSED!");
        //        return false;
        //    }
        //}

        private void EndOfAttack()
        {
            cooldown.cd = savedAbility.attackCD;
            cooldown.maxCD = savedAbility.attackCD;
            animationHandler.EndWeaponAttack();
        }

        //private void DistributeDamage(Ability ability, EnemyTarget enemyTarget)
        //{
        //    float newDamage = 0f;

        //    if (ability.damageType == DamageType.physical)
        //    {
        //        newDamage = strength * ability.attackDamageModifier;
        //        newDamage -= (newDamage * armorRatio * enemyTarget.armor);
        //        if (ability.attackVariant == AttackVariant.critical)
        //        {
        //            newDamage = CriticalStrike(newDamage);
        //        }
        //        enemyTarget.TakeDamage(newDamage, ability.damageType);

        //    }
        //    if (ability.damageType == DamageType.magical)
        //    {
        //        newDamage = ZodiacModifier(ability, enemyTarget.zodiac) * ability.attackDamageFlat;
        //        enemyTarget.TakeDamage(newDamage, ability.damageType);
        //    }
        //    if (ability.damageType == DamageType.mixed)
        //    {
        //        float damageInstance1 = strength * ability.attackDamageModifier;
        //        damageInstance1 = damageInstance1 - (damageInstance1 * armorRatio * enemyTarget.armor);
        //        float damageInstance2 = ZodiacModifier(ability, enemyTarget.zodiac) * ability.attackDamageFlat;
        //        enemyTarget.TakeDamage(damageInstance1, DamageType.physical);
        //        enemyTarget.TakeDamage(damageInstance2, DamageType.magical);
        //    }
        //    if (ability.damageType == DamageType.healing)
        //    {
        //        float newHealing = ability.healAmount;
        //        enemyTarget.Heal(newHealing);
        //    }
        //    if (ability.attackVariant == AttackVariant.lifesteal)
        //    {
        //        Heal(newDamage);
        //        print(this.name + " has lifestealed " + newDamage.ToString() + " health points from " + enemyTarget.name);
        //    }
        //    if (ability.attackVariant == AttackVariant.splash)
        //    {
        //        SplashDamage(enemyTarget, newDamage);
        //    }
        //}

        //private void DistributeDamage(Ability ability, Fighter actor)
        //{
        //    float newDamage = 0f;

        //    if (ability.damageType == DamageType.physical)
        //    {
        //        newDamage = strength * ability.attackDamageModifier;
        //        newDamage = newDamage - (newDamage * armorRatio * actor.armor);
        //        if (ability.attackVariant == AttackVariant.critical)
        //        {
        //            newDamage = CriticalStrike(newDamage);
        //        }
        //        actor.TakeDamage(newDamage, ability.damageType);

        //    }
        //    if (ability.damageType == DamageType.magical)
        //    {
        //        newDamage = ZodiacModifier(ability, actor.zodiac) * ability.attackDamageFlat;
        //        actor.TakeDamage(newDamage, ability.damageType);
        //    }
        //    if (ability.damageType == DamageType.mixed)
        //    {
        //        float damageInstance1 = strength * ability.attackDamageModifier;
        //        damageInstance1 = damageInstance1 - (damageInstance1 * armorRatio * actor.armor);
        //        float damageInstance2 = ZodiacModifier(ability, actor.zodiac) * ability.attackDamageFlat;
        //        actor.TakeDamage(damageInstance1, DamageType.physical);
        //        actor.TakeDamage(damageInstance2, DamageType.magical);
        //    }
        //    if (ability.damageType == DamageType.healing)
        //    {
        //        float newHealing = ability.healAmount;
        //        actor.Heal(newHealing);
        //    }
        //    if (ability.attackVariant == AttackVariant.lifesteal)
        //    {
        //        enemyTarget.Heal(newDamage);
        //        print(this.name + " has lifestealed " + newDamage.ToString() + " health points from " + actor.name);
        //    }
        //    if (ability.attackVariant == AttackVariant.splash)
        //    {
        //        SplashDamage(actor, newDamage);
        //    }
        //}

        //private float CriticalStrike(float newDamage)
        //{
        //    float critRoll = UnityEngine.Random.Range(0f, 255f);

        //    if (critRoll > criticalHit)
        //    {
        //        newDamage *= critRatio;
        //        print(this.name + " has crit for " + newDamage.ToString() + " damage!");
        //    }

        //    return newDamage;
        //}

        //private void SplashDamage(EnemyTarget enemyTarget, float newDamage)
        //{
        //    EnemyTarget[] allEnemies = FindObjectsOfType<EnemyTarget>();
        //    foreach (EnemyTarget enemy in allEnemies)
        //    {
        //        if ((enemyTarget.GetComponent<Mover>().GetGridPos() + Vector2Int.down) == enemy.GetComponent<Mover>().GetGridPos())
        //        {
        //            enemy.TakeDamage(newDamage * splashRatio, DamageType.physical);
        //            print(this.name + " has splashed " + enemy.ToString() + " for " + (newDamage * splashRatio) + " damage!");
        //        }
        //        if ((enemyTarget.GetComponent<Mover>().GetGridPos() + Vector2Int.up) == enemy.GetComponent<Mover>().GetGridPos())
        //        {
        //            enemy.TakeDamage(newDamage * splashRatio, DamageType.physical);
        //            print(this.name + " has splashed " + enemy.ToString() + " for " + (newDamage * splashRatio) + " damage!");
        //        }
        //        if ((enemyTarget.GetComponent<Mover>().GetGridPos() + Vector2Int.right) == enemy.GetComponent<Mover>().GetGridPos())
        //        {
        //            enemy.TakeDamage(newDamage * splashRatio, DamageType.physical);
        //            print(this.name + " has splashed " + enemy.ToString() + " for " + (newDamage * splashRatio) + " damage!");
        //        }
        //        if ((enemyTarget.GetComponent<Mover>().GetGridPos() + Vector2Int.left) == enemy.GetComponent<Mover>().GetGridPos())
        //        {
        //            enemy.TakeDamage(newDamage * splashRatio, DamageType.physical);
        //            print(this.name + " has splashed " + enemy.ToString() + " for " + (newDamage * splashRatio) + " damage!");
        //        }
        //    }
        //}

        //private void SplashDamage(Fighter fighter, float newDamage)
        //{
        //    Fighter[] allActors = FindObjectsOfType<Fighter>();
        //    foreach (Fighter actor in allActors)
        //    {
        //        if (actor == fighter) continue;
        //        if ((actor.GetComponent<Mover>().GetGridPos() + Vector2Int.down) == fighter.GetComponent<Mover>().GetGridPos())
        //        {
        //            actor.TakeDamage(newDamage * enemyTarget.splashRatio, DamageType.physical);
        //            print(this.name + " has splashed " + actor.ToString() + " for " + (newDamage * enemyTarget.splashRatio) + " damage!");
        //        }
        //        if ((actor.GetComponent<Mover>().GetGridPos() + Vector2Int.up) == fighter.GetComponent<Mover>().GetGridPos())
        //        {
        //            actor.TakeDamage(newDamage * enemyTarget.splashRatio, DamageType.physical);
        //            print(this.name + " has splashed " + actor.ToString() + " for " + (newDamage * enemyTarget.splashRatio) + " damage!");
        //        }
        //        if ((actor.GetComponent<Mover>().GetGridPos() + Vector2Int.right) == fighter.GetComponent<Mover>().GetGridPos())
        //        {
        //            actor.TakeDamage(newDamage * enemyTarget.splashRatio, DamageType.physical);
        //            print(this.name + " has splashed " + actor.ToString() + " for " + (newDamage * enemyTarget.splashRatio) + " damage!");
        //        }
        //        if ((actor.GetComponent<Mover>().GetGridPos() + Vector2Int.left) == fighter.GetComponent<Mover>().GetGridPos())
        //        {
        //            actor.TakeDamage(newDamage * enemyTarget.splashRatio, DamageType.physical);
        //            print(this.name + " has splashed " + actor.ToString() + " for " + (newDamage * enemyTarget.splashRatio) + " damage!");
        //        }
        //    }
        //}

        private float ZodiacModifier(Ability ability, Zodiac zodiac)
        {

            if (ability.magicType == MagicType.prysm)
            {
                return 1.75f;
            }

            switch (zodiac)
            {
                case Zodiac.spirit:
                    if (ability.magicType == MagicType.sun)
                    {
                        return 1.75f;
                    }
                    else if (ability.magicType == MagicType.moon)
                    {
                        return 0.25f;
                    }
                    else if (ability.magicType == MagicType.lamp)
                    {
                        return 0.75f;
                    }
                    else if (ability.magicType == MagicType.electric)
                    {
                        return 1.25f;
                    }
                    break;

                case Zodiac.oak:
                    if (ability.magicType == MagicType.sun)
                    {
                        return 0.5f;
                    }
                    else if (ability.magicType == MagicType.moon)
                    {
                        return 1.5f;
                    }
                    else if (ability.magicType == MagicType.lamp)
                    {
                        return 1.75f;
                    }
                    else if (ability.magicType == MagicType.electric)
                    {
                        return 0.25f;
                    }
                    break;

                case Zodiac.urn:
                    if (ability.magicType == MagicType.sun)
                    {
                        return 0.75f;
                    }
                    else if (ability.magicType == MagicType.moon)
                    {
                        return 1f;
                    }
                    else if (ability.magicType == MagicType.lamp)
                    {
                        return 0.5f;
                    }
                    else if (ability.magicType == MagicType.electric)
                    {
                        return 1.75f;
                    }
                    break;

                case Zodiac.sword:
                    if (ability.magicType == MagicType.sun)
                    {
                        return 1f;
                    }
                    else if (ability.magicType == MagicType.moon)
                    {
                        return 1.25f;
                    }
                    else if (ability.magicType == MagicType.lamp)
                    {
                        return 1.25f;
                    }
                    else if (ability.magicType == MagicType.electric)
                    {
                        return 0.5f;
                    }
                    break;

                case Zodiac.beast:
                    if (ability.magicType == MagicType.sun)
                    {
                        return 1.25f;
                    }
                    else if (ability.magicType == MagicType.moon)
                    {
                        return 0.5f;
                    }
                    else if (ability.magicType == MagicType.lamp)
                    {
                        return 1.5f;
                    }
                    else if (ability.magicType == MagicType.electric)
                    {
                        return 0.75f;
                    }
                    break;

                case Zodiac.coin:
                    if (ability.magicType == MagicType.sun)
                    {
                        return 0.25f;
                    }
                    else if (ability.magicType == MagicType.moon)
                    {
                        return 1.75f;
                    }
                    else if (ability.magicType == MagicType.lamp)
                    {
                        return 1f;
                    }
                    else if (ability.magicType == MagicType.electric)
                    {
                        return 1f;
                    }
                    break;

                case Zodiac.torch:
                    if (ability.magicType == MagicType.sun)
                    {
                        return 1.5f;
                    }
                    else if (ability.magicType == MagicType.moon)
                    {
                        return 0.75f;
                    }
                    else if (ability.magicType == MagicType.lamp)
                    {
                        return 0.25f;
                    }
                    else if (ability.magicType == MagicType.electric)
                    {
                        return 1.5f;
                    }
                    break;
            }

            return 1f;
        }





        //public void DoHighlightMaterial()
        //{
        //    materialRenderer.material = highlightMaterial;
        //}

        //public void DoDefaultMaterial()
        //{
        //    materialRenderer.material = defaultMaterial;
        //}

        //public void DoDeadMaterial()
        //{
        //    materialRenderer.material = deadMaterial;
        //}
    }
}
