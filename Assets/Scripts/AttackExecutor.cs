using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using SWS;

namespace TTW.Combat
{
    public class AttackExecutor : MonoBehaviour
    {

        CombatController combatController;
        Pusher pusher;
        splineMove spline;
        OnRailsMover onRailsMover;
        CoolDown coolDown;
        Fighter fighter;
        public bool attackInProgress;

        float savedCD = 0f; 


        private void Awake()
        {
            combatController = FindObjectOfType<CombatController>();
            pusher = GetComponent<Pusher>();
            spline = GetComponent<splineMove>();
            onRailsMover = GetComponent<OnRailsMover>();
            coolDown = GetComponent<CoolDown>();

            if (GetComponent<Fighter>() != null)
            {
                fighter = GetComponent<Fighter>();
            }

        }

        private void Start()
        {
            GameEvents.current.onAnimationEnd += PostAnimation;
        }

        #region actor specific

        //for actor
        public void PerformAttack(Ability ability, List<AttackReceiver> targets, Fighter actor, bool isLinkedAbility)
        {
            PreAnimation(ability, targets, actor, isLinkedAbility);

            if (isLinkedAbility) return;

            savedCD = ability.attackCD;
            attackInProgress = true;
        }

        private void PreAnimation(Ability ability, List<AttackReceiver> targets, Fighter actor, bool isLinkedAbility)
        {
            if (!isLinkedAbility)
            {
                targets = ApplyMadness(targets, actor.GetComponent<StatusEffectManager>());
            }

            List<AttackReceiver> finalTargetList = new List<AttackReceiver>();

            foreach (AttackReceiver target in targets)
            {
                //check for mirroring, apply mirroring
                if (target.IsMirroring() && ability.damageType == DamageType.magical)
                {
                    finalTargetList.Add(actor.GetComponent<AttackReceiver>());
                    target.isMirroring = false;
                    continue;
                }

                //check for protection
                //if (target.IsProtected())
                //{
                //    finalTargetList.Add(target.GetProtector());
                //    continue;
                //}

                finalTargetList.Add(target);
            }

            foreach (AttackReceiver target in finalTargetList)
            {
                float rawDamage = 0f;
                float finalDamage = 0f;
                Ability finalAbility = ability;
                AttackReceiver finalTarget = target;
                AttackExecutor finalCaster = actor.GetComponent<AttackExecutor>();

                //APPLY TARGETS' STATES AND ATTRIBUTES

                //check for countering, apply countering
                if (target.IsCountering() && ability.damageType == DamageType.physical)
                {
                    if (target.GetComponent<Fighter>() == null)
                    {
                        finalAbility = target.GetComponent<EnemySpells>().counterAttack;
                        finalCaster = target.GetComponent<AttackExecutor>();

                        //reversal
                        if (finalAbility.reversal)
                        {
                            finalAbility = ability;
                            finalCaster = actor.GetComponent<AttackExecutor>();
                        }

                        finalTarget = this.GetComponent<AttackReceiver>();
                    }
                }

                //log raw damage, damage type
                if (finalAbility.damageType == DamageType.physical)
                {
                    rawDamage = finalCaster.GetComponent<PlayerStats>().strength * finalAbility.damageModifier;
                }
                if (finalAbility.damageType == DamageType.magical)
                {
                    rawDamage = finalAbility.damageFlat;
                }

                //check if attack hits (contains blindness method) if not end attack
                if (!CheckIfAttackHit(finalAbility, finalTarget, finalCaster.GetComponent<StatusEffectManager>()))
                {
                    print("attack MISSED");
                    continue;
                }

                //check for critical hit

                if (finalAbility.attackVariant == AttackVariant.critical)
                {
                    if (CriticalHitSuccess(finalCaster.GetComponent<PlayerStats>()))
                    {
                        rawDamage = rawDamage * finalCaster.GetComponent<PlayerStats>().critRatio;
                    }
                }

                //reduce for guarding
                if (finalTarget.IsGuarding() && finalAbility.damageType == DamageType.physical)
                {
                    rawDamage = rawDamage * 0.5f;
                    finalTarget.isGuarding = false;
                }

                //reduce for cloaking
                if (finalTarget.IsCloaked() && finalAbility.damageType == DamageType.magical)
                {
                    rawDamage = rawDamage * 0.5f;
                    finalTarget.isCloaked = false;
                }

                //reduce for zodiac modifiers
                if (finalAbility.damageType == DamageType.magical)
                {
                    rawDamage = rawDamage * ZodiacModifier(finalAbility.magicType, finalTarget.GetZodiac());
                }

                //reduce for armor
                if (finalAbility.damageType == DamageType.physical)
                {
                    rawDamage = Mathf.Abs(rawDamage - target.GetArmor());
                }

                //desperation
                if (finalAbility.desperation)
                {
                    print("is desperation attack");
                    if (target.isCountering)
                    {
                        rawDamage = (target.GetComponent<PlayerStats>().maxHealth - target.GetComponent<PlayerStats>().health) + rawDamage;
                    }
                    else
                    {
                        rawDamage = (GetComponent<PlayerStats>().maxHealth - GetComponent<PlayerStats>().health) + rawDamage;
                    }
                    
                }

                finalDamage = rawDamage;


                //check for invulnerability and phasing
                if (finalTarget.IsInvulnerable() && finalAbility.damageType == DamageType.physical)
                {
                    finalDamage = 0f;
                    finalTarget.isInvulnerable = false;
                }

                if (finalTarget.IsPhased() && finalAbility.damageType == DamageType.magical)
                {
                    finalDamage = 0f;
                    print("target is phased");
                    finalTarget.isPhased = false;
                }

                //apply nerfs and buffs
                ApplyNerfBuff(finalAbility.statChange, finalAbility.statChangeValue, target.GetComponent<StatHandler>());

                //apply status effect
                ApplyStatusEffect(finalAbility.statusEffect, finalAbility.statusEffectTimer, finalTarget);

                //apply new neutral state
                ApplyNeutralState(ability, GetComponent<AttackReceiver>());

                if (finalAbility.neutralState == NeutralState.protect)
                {
                    if (finalTarget != actor.GetComponent<AttackReceiver>())
                    {
                        finalTarget.protector = actor.GetComponent<AttackReceiver>();
                        finalTarget.isProtected = true;
                        actor.GetComponent<AttackReceiver>().isProtecting = true;
                    }
                }

                //APPLY ATTACKS' STATES AND ATTRIBUTES
                
                if (finalAbility.neutralState == NeutralState.guardian)
                {
                    if (finalTarget != actor.GetComponent<AttackReceiver>())
                    {
                        finalTarget.protector = actor.GetComponent<AttackReceiver>();
                    }
                    finalTarget.hasGuardian = true;
                }

                if (finalAbility.damageType == DamageType.healing)
                {
                    finalTarget.Heal(finalAbility.healAmount, ability.canRevive);
                }
                else
                {
                    finalTarget.TakeDamage(finalDamage);
                    print(finalTarget + " took " + finalDamage + " damage from " + finalAbility + " cast by " + finalCaster);
                }

                //apply lifesteal w/ modified damage values
                if (finalAbility.attackVariant == AttackVariant.lifesteal)
                {
                    actor.GetComponent<AttackReceiver>().Heal(finalDamage, false);
                }

                //apply splash damage to targets nearby
                if (finalAbility.attackVariant == AttackVariant.splash)
                {
                    SplashDamageForActor(finalTarget, finalDamage, 0.5f);
                }

                //UI will be paused until end of attack where these stats will be reflected visually.
                //Enemy animations will have keyframe triggers that trigger actor hurt animations. none of that will be done here.

                if (target.isCountering)
                {
                    target.isCountering = false;
                }
            }

            if (isLinkedAbility)
            {
                return;
            }

            if (ability.helpPlease)
            {
                Fighter[] fighters = FindObjectsOfType<Fighter>();

                foreach(Fighter fighter in fighters)
                {
                    fighter.GetComponent<CoolDown>().cd = 0;
                }
            }

            if (ability.swanSong)
            {
                actor.GetComponent<PlayerStats>().health = 0f;
            }

            if (ability.legendary)
            {
                actor.usedLegendary = true;
            }

            StartAnimations(ability, actor, targets);
        }

        private void StartAnimations(Ability ability, Fighter actor, List<AttackReceiver> targets)
        {
            //When all three animation states have been accounted for, perform queued action and end attack
            GameEvents.current.AnimationStart();


            //check for paralysis, if ability requires moving ability ends without proceeding
            if (actor.GetComponent<StatusEffectManager>().statusEffectBad == StatusEffect.paralyze)
            {
                print(actor + " is PARALYZED!");
                return;
            }

            //DISPLACEMENT GOES HERE
            if (ability.swap)
            {
                if (targets.Count != 1) return;

                Vector3 casterPosition = actor.transform.position;
                Vector3 targetPosition = targets[0].transform.position;

                actor.transform.position = targetPosition;
                targets[0].transform.position = casterPosition;
            }

            if (ability.push)
            {

                if (actor.GetComponent<Mover>().GetGridPos().x == targets[0].GetComponent<Mover>().GetGridPos().x)
                {
                    if (actor.GetComponent<Mover>().GetGridPos().y > targets[0].GetComponent<Mover>().GetGridPos().y)
                    {
                        actor.GetComponent<Pusher>().ExecutePushGlobalAlly(targets[0].GetComponent<Pushable>(), Push.south);
                    }
                    else
                    {
                        actor.GetComponent<Pusher>().ExecutePushGlobalAlly(targets[0].GetComponent<Pushable>(), Push.north);
                    }
                }

                if (actor.GetComponent<Mover>().GetGridPos().y == targets[0].GetComponent<Mover>().GetGridPos().y)
                {
                    if (actor.GetComponent<Mover>().GetGridPos().x > targets[0].GetComponent<Mover>().GetGridPos().x)
                    {
                        actor.GetComponent<Pusher>().ExecutePushGlobalAlly(targets[0].GetComponent<Pushable>(), Push.east);
                    }
                    else
                    {
                        actor.GetComponent<Pusher>().ExecutePushGlobalAlly(targets[0].GetComponent<Pushable>(), Push.west);
                    }
                }
            }

            if (ability.pull)
            {
                if (actor.GetComponent<Mover>().GetGridPos().x == targets[0].GetComponent<Mover>().GetGridPos().x)
                {
                    if (actor.GetComponent<Mover>().GetGridPos().y > targets[0].GetComponent<Mover>().GetGridPos().y)
                    {
                        actor.GetComponent<Pusher>().ExecutePushGlobalAlly(targets[0].GetComponent<Pushable>(), Push.north);
                    }
                    else
                    {
                        actor.GetComponent<Pusher>().ExecutePushGlobalAlly(targets[0].GetComponent<Pushable>(), Push.south);
                    }
                }

                if (actor.GetComponent<Mover>().GetGridPos().y == targets[0].GetComponent<Mover>().GetGridPos().y)
                {
                    if (actor.GetComponent<Mover>().GetGridPos().x > targets[0].GetComponent<Mover>().GetGridPos().x)
                    {
                        actor.GetComponent<Pusher>().ExecutePushGlobalAlly(targets[0].GetComponent<Pushable>(), Push.west);
                    }
                    else
                    {
                        actor.GetComponent<Pusher>().ExecutePushGlobalAlly(targets[0].GetComponent<Pushable>(), Push.east);
                    }
                }
            }

            //start animation timer (timer reads as animating)

                //receive confirmation that movers are no longer moving, rails are no longer moving, and animation is no longer animating then proceed to post animation
        }

        #endregion

        #region enemy specific

        //for enemy
        public void PerformAttack(EnemyAttack ability, List<AttackReceiver> targets, EnemyTarget enemy, bool isLinkedAbility)
        {
            
            PreAnimation(ability, targets, enemy, isLinkedAbility);

            if (isLinkedAbility) return;

            attackInProgress = true;

            savedCD = ability.attackCD;
        }

        private void PreAnimation(EnemyAttack ability, List<AttackReceiver> targets, EnemyTarget enemy, bool isLinkedAbility)
        {
            if (!isLinkedAbility)
            {
                targets = ApplyMadness(targets, enemy.GetComponent<StatusEffectManager>());
            }
            
            List<AttackReceiver> finalTargetList = new List<AttackReceiver>();

            foreach (AttackReceiver target in targets)
            {
                //check for mirroring, apply mirroring
                if (target.IsMirroring() && ability.damageType == DamageType.magical)
                {
                    finalTargetList.Add(enemy.GetComponent<AttackReceiver>());
                    target.isMirroring = false;
                    continue;
                }

                //check for protection

                //if (target.IsProtected())
                //{
                //    finalTargetList.Add(target.GetProtector());
                //    continue;
                //}

                finalTargetList.Add(target);
            }

            

            foreach (AttackReceiver target in finalTargetList)
            {
                float rawDamage = 0f;
                float finalDamage = 0f;
                EnemyAttack finalAbility = ability;
                AttackReceiver finalTarget = target;
                AttackExecutor finalCaster = enemy.GetComponent<AttackExecutor>();

                //check for countering, apply countering
                if (target.IsCountering() && ability.damageType == DamageType.physical)
                {
                    if (target.GetComponent<EnemyTarget>() == null)
                    {
                        finalAbility = target.GetComponent<SpellsEquipped>().counterAttack;
                        finalCaster = target.GetComponent<AttackExecutor>();

                        //reversal
                        if (finalAbility.reversal)
                        {
                            finalAbility = ability;
                            finalCaster = enemy.GetComponent<AttackExecutor>();
                        }

                        finalTarget = this.GetComponent<AttackReceiver>();

                    }
                }

                //log raw damage, damage type
                if (finalAbility.damageType == DamageType.physical)
                {
                    rawDamage = finalCaster.GetComponent<PlayerStats>().strength * finalAbility.damageModifier;
                }
                if (finalAbility.damageType == DamageType.magical)
                {
                    rawDamage = finalAbility.damageFlat;
                }

                //check if attack hits (contains blindness method) if not end attack
                if (!CheckIfAttackHit(finalAbility, finalTarget, finalCaster.GetComponent<StatusEffectManager>()))
                {
                    print("attack MISSED");
                    continue;
                }

                //check for critical hit

                if (finalAbility.attackVariant == AttackVariant.critical)
                {
                    if (CriticalHitSuccess(enemy.GetComponent<PlayerStats>()))
                    {
                        rawDamage = rawDamage * finalCaster.GetComponent<PlayerStats>().critRatio;
                    }
                }

                //reduce for guarding
                if (finalTarget.IsGuarding() && finalAbility.damageType == DamageType.physical)
                {
                    rawDamage = rawDamage * 0.5f;
                    finalTarget.isGuarding = false;
                }

                //reduce for cloaking
                if (finalTarget.IsCloaked() && finalAbility.damageType == DamageType.magical)
                {
                    rawDamage = rawDamage * 0.5f;
                    finalTarget.isCloaked = false;
                }

                if (finalTarget.HasGuardian() && finalTarget.protector != null)
                {
                    Vector3 protectorPosition = finalTarget.protector.transform.position;
                    Vector3 targetPosition = finalTarget.transform.position;
                    AttackReceiver protectedActor = finalTarget;
                    finalTarget = finalTarget.protector;

                    finalTarget.transform.position = targetPosition;
                    protectedActor.transform.position = protectorPosition;

                    finalTarget.GetComponent<CoolDown>().cd = 0f;
                }

                if (finalTarget.IsProtected() && finalTarget.protector != null)
                {
                    finalTarget = finalTarget.protector;
                    finalTarget.isProtected = false;
                    finalTarget.protector.isProtecting = false;
                }

                //reduce for zodiac modifiers
                if (finalAbility.damageType == DamageType.magical)
                {
                    rawDamage = rawDamage * ZodiacModifier(finalAbility.magicType, finalTarget.GetZodiac());
                }

                //reduce for armor
                if (finalAbility.damageType == DamageType.physical)
                {
                    rawDamage = Mathf.Abs(rawDamage - finalTarget.GetArmor());
                }

                //desperation
                if (finalAbility.desperation)
                {
                    //this is supposed to be target and not finalTarget
                    rawDamage = (target.GetComponent<PlayerStats>().maxHealth - target.GetComponent<PlayerStats>().health) + rawDamage;
                }

                finalDamage = rawDamage;

                //check for invulnerability and phasing
                if (finalTarget.IsInvulnerable() && finalAbility.damageType == DamageType.physical)
                {
                    finalDamage = 0f;
                    print("target is invulnerable");
                    finalTarget.isInvulnerable = false;
                }

                if (finalTarget.IsPhased() && finalAbility.damageType == DamageType.magical)
                {
                    finalDamage = 0f;
                    finalTarget.isPhased = false;
                    print("target is phased");
                }

                //apply nerfs and buffs
                ApplyNerfBuff(finalAbility.statChange, finalAbility.statChangeValue, finalTarget.GetComponent<StatHandler>());

                //apply status effect
                ApplyStatusEffect(finalAbility.statusEffect, finalAbility.statusEffectTimer, finalTarget);

                //apply new neutral state
                ApplyNeutralState(finalAbility, GetComponent<AttackReceiver>());

                if (finalAbility.damageType == DamageType.healing)
                {
                    finalTarget.Heal(ability.healAmount, false);
                }
                else
                {
                    finalTarget.TakeDamage(finalDamage);
                    print(finalTarget + " took " + finalDamage + " damage from " + finalAbility + " cast by " + finalCaster);
                }

                //apply lifesteal w/ modified damage values
                if (finalAbility.attackVariant == AttackVariant.lifesteal)
                {
                    enemy.GetComponent<AttackReceiver>().Heal(finalDamage, false);
                }

                //apply splash damage to targets nearby
                if (finalAbility.attackVariant == AttackVariant.splash)
                {
                    SplashDamageForEnemy(finalTarget, finalDamage, 0.5f);
                }

                //UI will be paused until end of attack where these stats will be reflected visually.
                //Enemy animations will have keyframe triggers that trigger actor hurt animations. none of that will be done here.

                if (target.isCountering)
                {
                    target.isCountering = false;
                }
            }

            if (ability.enemyCreate != null)
            {
                CreateEnemy(ability);
            }

            if (ability.trapCreate != null)
            {
                CreateTrap(ability);
            }

            if (ability.pushableCreate != null)
            {
                CreatePushable(ability);
            }

            if (isLinkedAbility)
            {
                return;
            }

            StartAnimations(ability, enemy);
        }

        private void StartAnimations(EnemyAttack ability, EnemyTarget enemyTarget)
        {
            //When all three animation states have been accounted for, perform queued action and end attack
            GameEvents.current.AnimationStart();

            //start push (movers read as in motion)
            if (ability.push != Push.none)
            {
                List<Pushable> pushables = new List<Pushable>();
                

                if (ability.pushIsGlobal)
                {
                    pushables = pusher.MakeListOfPushablesGlobal(ability);
                    foreach (Pushable pushable in pushables)
                    {
                        pusher.ExecutePushGlobal(pushable, ability);
                    }
                }
                else
                {
                    pushables = pusher.MakeListOfPushablesRelative(ability, enemyTarget);
                    foreach (Pushable pushable in pushables)
                    {
                        pusher.ExecutePushRelative(pushable, ability, enemyTarget.wing);
                    }
                }
            }


            //check for paralysis, if ability requires moving ability ends without proceeding
            if (enemyTarget.GetComponent<StatusEffectManager>().statusEffectBad == StatusEffect.paralyze)
            {
                print(enemyTarget + " is PARALYZED!");
                return;
            }

            //else start moving (rails read as in motion)
            if (ability.usesRails)
            {
                PerformTravelDestination(ability);
             
            }

            //start animation timer (timer reads as animating)

            //receive confirmation that movers are no longer moving, rails are no longer moving, and animation is no longer animating then proceed to post animation
        }

        private void PostAnimation()
        {
            if (!attackInProgress) return;

            //end attack reset everything
            attackInProgress = false;
            coolDown.SetCoolDown(savedCD);
            print("cooling down");
        }

        private void ApplyStatusEffect(StatusEffect statusEffect, float statusTimer, AttackReceiver target)
        {
            switch (statusEffect)
            {
                case StatusEffect.angel:
                case StatusEffect.dasein:
                case StatusEffect.genera:
                    target.ApplyGoodStatusEffect(statusEffect, statusTimer);
                    break;
                case StatusEffect.bubble:
                    target.bubbleHealth = target.maxBubbleHealth;
                    target.bubbleInstances = target.maxBubbleInstances;
                    target.ApplyGoodStatusEffect(statusEffect, statusTimer);
                    break;

                case StatusEffect.burn:
                case StatusEffect.blind:
                case StatusEffect.madness:
                case StatusEffect.paralyze:
                    target.ApplyBadStatusEffect(statusEffect, statusTimer);
                    break;

                case StatusEffect.dispel:
                    target.Dispel();
                    break;
            }
        }

        private void ApplyNeutralState(EnemyAttack ability, AttackReceiver target)
        {
            switch (ability.neutralState)
            {
                case Combat.NeutralState.cloak:
                    target.isCloaked = true;
                    break;
                case Combat.NeutralState.counter:
                    target.isCountering = true;
                    if (ability.changeCounterAttack != null)
                    {
                        target.SetNewCounter(ability.changeCounterAttack);
                    }
                    break;
                case Combat.NeutralState.guard:
                    target.isGuarding = true;
                    break;
                case Combat.NeutralState.invulnerable:
                    target.isInvulnerable = true;
                    break;
                case Combat.NeutralState.mirror:
                    target.isMirroring = true;
                    break;
                case Combat.NeutralState.phase:
                    target.isPhased = true;
                    break;
                case Combat.NeutralState.protect:
                    if (target != this.gameObject.GetComponent<AttackReceiver>())
                    {
                        target.protector = this.gameObject.GetComponent<AttackReceiver>();
                    }
                    break;
                case Combat.NeutralState.focus:
                    target.isFocused = true;
                    break;
                default:
                    break;
            }
        }

        private void ApplyNeutralState(Ability ability, AttackReceiver target)
        {
            switch (ability.neutralState)
            {
                case Combat.NeutralState.cloak:
                    target.isCloaked = true;
                    break;
                case Combat.NeutralState.counter:
                    target.isCountering = true;
                    if (ability.changeCounterAttack != null)
                    {
                        target.SetNewCounter(ability.changeCounterAttack);
                    }
                    break;
                case Combat.NeutralState.guard:
                    target.isGuarding = true;
                    break;
                case Combat.NeutralState.invulnerable:
                    target.isInvulnerable = true;
                    break;
                case Combat.NeutralState.mirror:
                    target.isMirroring = true;
                    break;
                case Combat.NeutralState.phase:
                    target.isPhased = true;
                    break;
                case Combat.NeutralState.protect:
                    if (target != this.gameObject.GetComponent<AttackReceiver>())
                    {
                        target.protector = this.gameObject.GetComponent<AttackReceiver>();
                    }
                    break;
                case Combat.NeutralState.focus:
                    target.isFocused = true;
                    break;
                default:
                    break;
            }
        }

        private float ZodiacModifier(MagicType magicType, Zodiac zodiac)
        {

            if (magicType == MagicType.prysm)
            {
                return 1.75f;
            }

            switch (zodiac)
            {
                case Zodiac.spirit:
                    if (magicType == MagicType.sun)
                    {
                        return 1.75f;
                    }
                    else if (magicType == MagicType.moon)
                    {
                        return 0.25f;
                    }
                    else if (magicType == MagicType.lamp)
                    {
                        return 0.75f;
                    }
                    else if (magicType == MagicType.electric)
                    {
                        return 1.25f;
                    }
                    break;

                case Zodiac.oak:
                    if (magicType == MagicType.sun)
                    {
                        return 0.5f;
                    }
                    else if (magicType == MagicType.moon)
                    {
                        return 1.5f;
                    }
                    else if (magicType == MagicType.lamp)
                    {
                        return 1.75f;
                    }
                    else if (magicType == MagicType.electric)
                    {
                        return 0.25f;
                    }
                    break;

                case Zodiac.urn:
                    if (magicType == MagicType.sun)
                    {
                        return 0.75f;
                    }
                    else if (magicType == MagicType.moon)
                    {
                        return 1f;
                    }
                    else if (magicType == MagicType.lamp)
                    {
                        return 0.5f;
                    }
                    else if (magicType == MagicType.electric)
                    {
                        return 1.75f;
                    }
                    break;

                case Zodiac.sword:
                    if (magicType == MagicType.sun)
                    {
                        return 1f;
                    }
                    else if (magicType == MagicType.moon)
                    {
                        return 1.25f;
                    }
                    else if (magicType == MagicType.lamp)
                    {
                        return 1.25f;
                    }
                    else if (magicType == MagicType.electric)
                    {
                        return 0.5f;
                    }
                    break;

                case Zodiac.beast:
                    if (magicType == MagicType.sun)
                    {
                        return 1.25f;
                    }
                    else if (magicType == MagicType.moon)
                    {
                        return 0.5f;
                    }
                    else if (magicType == MagicType.lamp)
                    {
                        return 1.5f;
                    }
                    else if (magicType == MagicType.electric)
                    {
                        return 0.75f;
                    }
                    break;

                case Zodiac.coin:
                    if (magicType == MagicType.sun)
                    {
                        return 0.25f;
                    }
                    else if (magicType == MagicType.moon)
                    {
                        return 1.75f;
                    }
                    else if (magicType == MagicType.lamp)
                    {
                        return 1f;
                    }
                    else if (magicType == MagicType.electric)
                    {
                        return 1f;
                    }
                    break;

                case Zodiac.torch:
                    if (magicType == MagicType.sun)
                    {
                        return 1.5f;
                    }
                    else if (magicType == MagicType.moon)
                    {
                        return 0.75f;
                    }
                    else if (magicType == MagicType.lamp)
                    {
                        return 0.25f;
                    }
                    else if (magicType == MagicType.electric)
                    {
                        return 1.5f;
                    }
                    break;
            }

            return 1f;
        }

        private bool CriticalHitSuccess(PlayerStats stats)
        {

            float critRoll = UnityEngine.Random.Range(0f, 255f);

            if (critRoll > stats.criticalHit)
            {
                print("CRITICAL HIT!");
                return true;
            }

            return false;
        }

        private bool CheckIfAttackHit(EnemyAttack ability, AttackReceiver fighter, StatusEffectManager statusEffectManager)
        {
            if (statusEffectManager.statusEffectBad == StatusEffect.blind && ability.damageType == DamageType.physical)
            {
                print("ATTACK MISSED!");
                return false;
            }

            if (statusEffectManager.statusEffectBad == StatusEffect.blind && ability.damageType == DamageType.mixed)
            {
                print("ATTACK MISSED!");
                return false;
            }

            float randomNumber = UnityEngine.Random.Range(0f, 255f);

            float evasionValue = (fighter.GetEvasion() * 35) - 350;

            if (randomNumber >= evasionValue)
            {
                return true;
            }
            else
            {
                print("ATTACK MISSED!");
                return false;
            }
        }

        private bool CheckIfAttackHit(Ability ability, AttackReceiver fighter, StatusEffectManager statusEffectManager)
        {
            if (statusEffectManager.statusEffectBad == StatusEffect.blind && ability.damageType == DamageType.physical)
            {
                print("ATTACK MISSED!");
                return false;
            }

            if (statusEffectManager.statusEffectBad == StatusEffect.blind && ability.damageType == DamageType.mixed)
            {
                print("ATTACK MISSED!");
                return false;
            }

            float randomNumber = UnityEngine.Random.Range(0f, 255f);

            float evasionValue = (fighter.GetEvasion() * CombatController.current.evasionDifferential) - (CombatController.current.evasionDifferential * 10);

            if (randomNumber >= evasionValue)
            {
                return true;
            }
            else
            {
                print("ATTACK MISSED!");
                return false;
            }
        }

        private List<AttackReceiver> ApplyMadness(List<AttackReceiver> targets, StatusEffectManager statusEffectManager)
        {
            if (statusEffectManager.statusEffectBad == StatusEffect.madness)
            {
                AttackReceiver[] attackReceivers = FindObjectsOfType<AttackReceiver>();

                int randomInt = UnityEngine.Random.Range(0, attackReceivers.Length);


                targets.Clear();
                targets.Add(attackReceivers[randomInt]);
            }

            return targets;

        }

        private void ApplyNerfBuff(StatChange statChange, int statChangeValue, StatHandler statHandler)
        {
            if (statChange == StatChange.humble)
            {
                statHandler.Humble();
                return;
            }

            if (statChange == StatChange.random)
            {
                statChangeValue = UnityEngine.Random.Range(-3, 3);
            }

            if (statChangeValue < 0)
            {
                switch (statChange)
                {
                    case StatChange.speed:
                        statHandler.ApplyNerf(StatHandler.StatChanges.SpeedNerf, Math.Abs(statChangeValue));
                        break;
                    case StatChange.strength:
                        statHandler.ApplyNerf(StatHandler.StatChanges.StrengthNerf, Math.Abs(statChangeValue));
                        break;
                    case StatChange.armor:
                        statHandler.ApplyNerf(StatHandler.StatChanges.ArmorNerf, Math.Abs(statChangeValue));
                        break;
                    case StatChange.evasion:
                        statHandler.ApplyNerf(StatHandler.StatChanges.EvasionNerf, Math.Abs(statChangeValue));
                        break;
                    case StatChange.all:
                        statHandler.ApplyNerf(StatHandler.StatChanges.SpeedNerf, Math.Abs(statChangeValue));
                        statHandler.ApplyNerf(StatHandler.StatChanges.StrengthNerf, Math.Abs(statChangeValue));
                        statHandler.ApplyNerf(StatHandler.StatChanges.ArmorNerf, Math.Abs(statChangeValue));
                        statHandler.ApplyNerf(StatHandler.StatChanges.EvasionNerf, Math.Abs(statChangeValue));
                        break;
                    case StatChange.random:
                        statHandler.ApplyNerf(StatHandler.StatChanges.Random, Math.Abs(statChangeValue));
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (statChange)
                {
                    case StatChange.speed:
                        statHandler.ApplyBuff(StatHandler.StatChanges.SpeedBuff, Math.Abs(statChangeValue));
                        break;
                    case StatChange.strength:
                        statHandler.ApplyBuff(StatHandler.StatChanges.StrengthBuff, Math.Abs(statChangeValue));
                        break;
                    case StatChange.armor:
                        statHandler.ApplyBuff(StatHandler.StatChanges.ArmorBuff, Math.Abs(statChangeValue));
                        break;
                    case StatChange.evasion:
                        statHandler.ApplyBuff(StatHandler.StatChanges.EvasionBuff, Math.Abs(statChangeValue));
                        break;
                    case StatChange.all:
                        statHandler.ApplyBuff(StatHandler.StatChanges.SpeedBuff, Math.Abs(statChangeValue));
                        statHandler.ApplyBuff(StatHandler.StatChanges.StrengthBuff, Math.Abs(statChangeValue));
                        statHandler.ApplyBuff(StatHandler.StatChanges.ArmorBuff, Math.Abs(statChangeValue));
                        statHandler.ApplyBuff(StatHandler.StatChanges.EvasionBuff, Math.Abs(statChangeValue));
                        break;
                    case StatChange.random:
                        statHandler.ApplyBuff(StatHandler.StatChanges.Random, Math.Abs(statChangeValue));
                        break;
                    default:
                        break;
                }
            }
        }

        private void SplashDamageForEnemy(AttackReceiver directTarget, float newDamage, float splashRatio)
        {
            AttackReceiver[] allTargets = FindObjectsOfType<AttackReceiver>();
            foreach (AttackReceiver target in allTargets)
            {
                if (target == directTarget) continue;
                if (target.GetComponent<Fighter>() == null) continue;

                if ((target.GetComponent<Mover>().GetGridPos() + Vector2Int.down) == directTarget.GetComponent<Mover>().GetGridPos())
                {
                    target.TakeDamage(newDamage * splashRatio);
                    print(this.name + " has splashed " + target.ToString() + " for " + (newDamage * splashRatio) + " damage!");
                }
                if ((target.GetComponent<Mover>().GetGridPos() + Vector2Int.up) == directTarget.GetComponent<Mover>().GetGridPos())
                {
                    target.TakeDamage(newDamage * splashRatio);
                    print(this.name + " has splashed " + target.ToString() + " for " + (newDamage * splashRatio) + " damage!");
                }
                if ((target.GetComponent<Mover>().GetGridPos() + Vector2Int.right) == directTarget.GetComponent<Mover>().GetGridPos())
                {
                    target.TakeDamage(newDamage * splashRatio);
                    print(this.name + " has splashed " + target.ToString() + " for " + (newDamage * splashRatio) + " damage!");
                }
                if ((target.GetComponent<Mover>().GetGridPos() + Vector2Int.left) == directTarget.GetComponent<Mover>().GetGridPos())
                {
                    target.TakeDamage(newDamage * splashRatio);
                    print(this.name + " has splashed " + target.ToString() + " for " + (newDamage * splashRatio) + " damage!");
                }
            }
        }

        private void SplashDamageForActor(AttackReceiver directTarget, float newDamage, float splashRatio)
        {
            AttackReceiver[] allTargets = FindObjectsOfType<AttackReceiver>();
            foreach (AttackReceiver target in allTargets)
            {
                if (target == directTarget) continue;
                if (target.GetComponent<EnemyTarget>() == null) continue;

                if ((target.GetComponent<Mover>().GetGridPos() + Vector2Int.down) == directTarget.GetComponent<Mover>().GetGridPos())
                {
                    target.TakeDamage(newDamage * splashRatio);
                    print(this.name + " has splashed " + target.ToString() + " for " + (newDamage * splashRatio) + " damage!");
                }
                if ((target.GetComponent<Mover>().GetGridPos() + Vector2Int.up) == directTarget.GetComponent<Mover>().GetGridPos())
                {
                    target.TakeDamage(newDamage * splashRatio);
                    print(this.name + " has splashed " + target.ToString() + " for " + (newDamage * splashRatio) + " damage!");
                }
                if ((target.GetComponent<Mover>().GetGridPos() + Vector2Int.right) == directTarget.GetComponent<Mover>().GetGridPos())
                {
                    target.TakeDamage(newDamage * splashRatio);
                    print(this.name + " has splashed " + target.ToString() + " for " + (newDamage * splashRatio) + " damage!");
                }
                if ((target.GetComponent<Mover>().GetGridPos() + Vector2Int.left) == directTarget.GetComponent<Mover>().GetGridPos())
                {
                    target.TakeDamage(newDamage * splashRatio);
                    print(this.name + " has splashed " + target.ToString() + " for " + (newDamage * splashRatio) + " damage!");
                }
            }
        }

        private void CreateEnemy(EnemyAttack ability)
        {
            Cell[] cells = FindObjectsOfType<Cell>();
            List<Cell> cellList = new List<Cell>();
            Cell chosenCell;


            foreach (Cell cell in cells)
            {
                if (cell.isEnemyCell && !cell.isOccupied)
                {
                    cellList.Add(cell);
                }
            }

            if (cellList.Count == 0)
            {
                return;
            }

            chosenCell = cellList[UnityEngine.Random.Range(0, cellList.Count)];
            var newEnemy = Instantiate(ability.enemyCreate);

            newEnemy.transform.position = chosenCell.transform.position;

        }

        //AtkEx
        private void CreateTrap(EnemyAttack ability)
        {
            print("creating trap");

            if (ability.targettingClass == TargettingClass.relative)
            {
                foreach (Vector2Int position in ability.relativeTargetCells)
                {
                    if (!combatController.ListOfPlayerCells[position].isOccupied)
                    {
                        combatController.ListOfPlayerCells[position].SetTrap(ability.trapCreate);
                    }
                }
            }

            if (ability.targettingClass == TargettingClass.global)
            {
                foreach (Vector2Int position in ability.globalTargetCells)
                {
                    if (!combatController.ListOfPlayerCells[position].isOccupied)
                    {
                        combatController.ListOfPlayerCells[position].SetTrap(ability.trapCreate);
                    }
                }
            }
        }

        //AtkEx
        private void CreatePushable(EnemyAttack ability)
        {
            print("creating pushable");

            if (ability.targettingClass == TargettingClass.relative)
            {
                foreach (Vector2Int position in ability.relativeTargetCells)
                {
                    if (!combatController.ListOfPlayerCells[position].isOccupied)
                    {
                        combatController.ListOfPlayerCells[position].SetPushable(ability.pushableCreate);
                    }
                }
            }

            if (ability.targettingClass == TargettingClass.global)
            {
                foreach (Vector2Int position in ability.globalTargetCells)
                {
                    if (!combatController.ListOfPlayerCells[position].isOccupied)
                    {
                        combatController.ListOfPlayerCells[position].SetPushable(ability.pushableCreate);
                    }
                }
            }
        }

        private void PerformTravelDestination(EnemyAttack ability)
        {
            //SetPathOrientation(ability);
            spline.pathContainer = ability.onRailsPath[0];
            onRailsMover.PerformAttackPath(spline);
        }

        #endregion


    }
}
