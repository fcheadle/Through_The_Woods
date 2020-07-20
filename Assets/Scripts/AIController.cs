using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using SWS;

namespace TTW.Combat
{

    public class AIController : MonoBehaviour
    {
        [SerializeField] float testAnimationFrameLength = 10f;

        public EnemyAttack[] abilities = new EnemyAttack[10];
        public Fighter[] targets = new Fighter[3];
        public int maxAnimationTimer = 100;
        public EnemyAttack selectedAbility = null;
        public EnemyAttack linkedAbility = null;
        public List<AttackReceiver> selectedTargets = null;
        public List<AttackReceiver> linkedTargets = null;
        public EnemyAttack safeAbility = null;
        Mover mover;
        OnRailsMover onRailsMover;
        CoolDown cooldown;
        Channel channel;
        AnimationHandler animationHandler;
        CombatController combatController;
        AttackIndicator attackIndicator;
        AttackReceiver attackReceiver;
        AttackExecutor attackExecutor;
        EnemyTarget enemyTarget;
        splineMove spline;
        //bool setPathOrientation = false;
        bool attackInProgress = false;
        public int pushablesCurrentMoving = 0;
        Fighter meleeTarget;
        StateMachine stateMachine;

        EnemyTarget[] allEnemies;
        Fighter[] allActors;
        Pushable[] allPushables;

        [SerializeField] Color normalColor = Color.grey;
        [SerializeField] Color attackColor = Color.red;

        void Start()
        {
            abilities = GetComponent<EnemySpells>().ability;
            safeAbility = GetComponent<EnemySpells>().safeAbility;
            targets = FindObjectsOfType<Fighter>();
            mover = GetComponent<Mover>();
            onRailsMover = GetComponent<OnRailsMover>();
            cooldown = GetComponent<CoolDown>();
            channel = GetComponent<Channel>();
            attackIndicator = GetComponent<AttackIndicator>();
            attackReceiver = GetComponent<AttackReceiver>();
            attackExecutor = GetComponent<AttackExecutor>();
            enemyTarget = GetComponent<EnemyTarget>();
            spline = GetComponent<splineMove>();
            stateMachine = GetComponent<StateMachine>();
            animationHandler = GetComponent<AnimationHandler>();

            allEnemies = FindObjectsOfType<EnemyTarget>();
            allActors = FindObjectsOfType<Fighter>();
            allPushables = FindObjectsOfType<Pushable>();

            combatController = FindObjectOfType<CombatController>();
            cooldown.isOnCD = true;
            channel.isChanneling = false;
            channel.channel = 0f;


            //perform attack once will be used to check if the current attack is in progress
            
        }

        void Update()
        {
            if (stateMachine.state == State.cooldown)
            {
                if (!cooldown.isOnCD)
                {
                    stateMachine.Neutral();
                }
            }
            if (stateMachine.state == State.neutral)
            {
                PresetAttack();
            }
            if (stateMachine.state == State.channeling)
            {
                if (!channel.isChanneling)
                {
                    foreach (AttackReceiver receiver in selectedTargets)
                    {
                        print(this + " is targetting " + receiver);
                    }

                    attackExecutor.PerformAttack(selectedAbility, selectedTargets, enemyTarget, false);

                    if (selectedAbility.linkedAbility != null)
                    { 
                        attackExecutor.PerformAttack(linkedAbility, linkedTargets, enemyTarget, true);
                    }

                    stateMachine.Animate();
                    animationHandler.SetAnimationTimer(testAnimationFrameLength);
                }
            }
        }

        private void PresetAttack()
        {
            selectedAbility = SelectRandomAttack();

            if (selectedAbility.linkedAbility != null)
            {
                linkedAbility = selectedAbility.linkedAbility;
            }

            //apply madness if necessary here
            selectedTargets = GetTargets(selectedAbility, enemyTarget);
            if (selectedAbility.linkedAbility != null && linkedAbility.targettingClass != selectedAbility.targettingClass)
            {
                linkedTargets = GetTargets(linkedAbility, enemyTarget);
            }


            float channelTime = selectedAbility.attackChannelTime;
            channel.isChanneling = true;

            //fix to setTargettedCells
            //attackIndicator.HighlightTargettedCells(selectedAbility, enemyTarget);
            

            stateMachine.StartChanneling(channelTime);

        }

        public void DeathAttack(EnemyAttack ability)
        {
            selectedAbility = ability;

            //apply madness if necessary here
            selectedTargets = GetTargets(selectedAbility, enemyTarget);

            float channelTime = selectedAbility.attackChannelTime;

            //fix to setTargettedCells
            //attackIndicator.HighlightTargettedCells(selectedAbility, enemyTarget);

            attackExecutor.PerformAttack(selectedAbility, selectedTargets, enemyTarget, false);

            stateMachine.StartChanneling(channelTime);

            selectedTargets.Clear();

            GetComponent<PlayerStats>().isDead = true;
        }

        //cycles through abilities and tests to see if one works until one does
        private EnemyAttack SelectRandomAttack()
        {
            int randomAttackIndex;
            EnemyAttack randomAbility;
            List<EnemyAttack> listOfAbilities = new List<EnemyAttack>();

            foreach(EnemyAttack ability in abilities)
            {
                if (TestAbility(ability))
                {
                    listOfAbilities.Add(ability);
                }
            }

            if (listOfAbilities.Count > 0)
            {
                randomAttackIndex = UnityEngine.Random.Range(0, listOfAbilities.Count);
                randomAbility = listOfAbilities[randomAttackIndex];

                return randomAbility;
            }
            else
            {
                return safeAbility;
            }
        }

        //tests to see if ability can be used in given context
        private bool TestAbility(EnemyAttack selectedAbility)
        {
            if (selectedAbility.targettingClass == TargettingClass.melee)
            {
                Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left };

                foreach (Fighter fighter in allActors)
                {
                    foreach (Vector2Int direction in directions)
                    {
                        if (fighter.GetComponent<Mover>().GetGridPos() + direction == mover.GetGridPos())
                        {
                            if (!fighter.isDead && !fighter.isIntangible)
                            {
                                meleeTarget = fighter;
                                return true;
                            }
                        }
                    }
                }

                return false;
            }

            if (selectedAbility.targettingClass == TargettingClass.support)
            {
                if (allEnemies.Length <= 1)
                {
                    return false;
                }
            }

            if (selectedAbility.usesRails)
            {
                if (selectedAbility.targettingClass == TargettingClass.global)
                {
                    if (combatController.ListOfcells[selectedAbility.LandingCell].isOccupied)
                    {
                        return false;
                    }
                }

                if (selectedAbility.targettingClass == TargettingClass.relative)
                {
                    Vector2Int[] targetCells = { selectedAbility.LandingCell };

                    if (combatController.ListOfcells[TranslateRelativeTargetCells(targetCells, enemyTarget)[0]].isOccupied)
                    {
                        return false;
                    }
                }
            }

            if (selectedAbility.RequiredCells.Length > 0)
            {
                bool matchFound = false;

                foreach (Vector2Int coord in selectedAbility.RequiredCells)
                {
                    if (mover.GetGridPos() == coord)
                    {
                        matchFound = true;
                    }
                }

                if (!matchFound)
                {
                    return false;
                }
            }

            if (selectedAbility.RequiredWing != Wing.none)
            {
                if (enemyTarget.wing != selectedAbility.RequiredWing)
                {
                    return false;
                }
            }

            return true;
        }



        //switch (stateMachine.state)
        //{

        //    //PlayerState
        //    case State.channeling:
        //        channel.CheckChannel();
        //        Targetting(selectedAbility);
        //        if (!channel.isChanneling)
        //        {
        //            state = State.animating;
        //        }
        //        break;

        //    //PlayerState
        //    case State.animating:

        //        print(attackInProgress);

        //        if (!attackInProgress)
        //        {
        //            AIController[] enemies = FindObjectsOfType<AIController>();
        //            foreach(AIController enemy in enemies)
        //            {
        //                if (enemy.attackInProgress == true && enemy.gameObject!= this.gameObject)
        //                {
        //                    return;
        //                }
        //            }
        //            attackInProgress = true;
        //            PerformAttack(selectedAbility);
        //        }

        //foreach (Cell cell in attackIndicator.convertedCells)
        //{
        //    cell.enemyHighlight = false;
        //    attackIndicator.areCellsColored = false;
        //    cell.DoDefaultColor();
        //    enemyTarget.SaveMaterial();
        //    enemyTarget.DoDefaultMaterial();
        //}
        //            break;
        //    }
        //}


        /* GOALS WITHIN THIS CODE
         * 1. Segment out a combat controller utilized public method pool. This pool will handle calculations that are universal such as zodiac and magic interactions
         * 2. AIController should handle the decisions of the AI. Namely, seeing which abilities are available to it. Checking to see which ones are best. and picking between them.
         * 3. An Attack Executor (should be shared with FIGHTER class) Class should handle distributing damage and effects. It will use the singleton, Combat Controller, to access universal algorithms in calculating damage.
         * 4. Animation needs to be accounted for with a separate timer. The timer will tick normally when not moving or pushing else it will wait for moving or pushing to complete then issue the same "END OF ANIMATION" method.
         * 5. AIController should resemble a player brain. Any limitations of the enemy class not related to decision making should be relegated to another class.
         * 6. NEW CLASSES: CC.AttackAlgorithms, AnimationTimer, AttackExecutor, AttackReceiver, PlayerState
         */


        //Pre targeting for relative global and support targeting
        //dont think this needed.

        private List<AttackReceiver> GetTargets(EnemyAttack ability, EnemyTarget enemy)
        {
            List<AttackReceiver> newTargets = new List<AttackReceiver>();
            List<Pushable> translatedTargetList = new List<Pushable>();

            switch (ability.targettingClass)
            {
                case TargettingClass.global:

                    foreach (Pushable pushable in allPushables)
                    {
                        foreach (Vector2Int position in ability.globalTargetCells)
                        {
                            if (pushable.GetComponent<Mover>().GetGridPos() == position)
                            {
                                translatedTargetList.Add(pushable);
                            }
                        }
                    }

                    //remove targets based on beeline targeting
                    if (ability.attackTarget == AttackTarget.beeline)
                    {
                        switch (enemyTarget.wing)
                        {
                            case Wing.bow:
                                translatedTargetList.Sort(SortByY);
                                translatedTargetList.Reverse();
                                break;
                            case Wing.port:
                                translatedTargetList.Sort(SortByX);
                                break;
                            case Wing.starboard:
                                translatedTargetList.Sort(SortByX);
                                translatedTargetList.Reverse();
                                break;
                            default:
                                print("NO WING DEFINED");
                                break;
                        }
                    }

                    //final list
                    foreach (Pushable pushable in translatedTargetList)
                    {
                        if (pushable.GetComponent<AttackReceiver>() != null)
                        {
                            newTargets.Add(pushable.GetComponent<AttackReceiver>());
                        }

                        if (ability.attackTarget == AttackTarget.beeline)
                        {
                            break;
                        }
                    }


                    break;

                case TargettingClass.relative:

                    Vector2Int[] translatedTargetCells = TranslateRelativeTargetCells(ability.relativeTargetCells, enemy);

                    foreach (Pushable pushable in allPushables)
                    {
                        foreach (Vector2Int position in translatedTargetCells)
                        {
                            if (pushable.GetComponent<Mover>().GetGridPos() == position)
                            {
                                translatedTargetList.Add(pushable);
                            }
                        }
                    }

                    //remove targets based on beeline targeting
                    if (ability.attackTarget == AttackTarget.beeline)
                    {
                        switch (enemyTarget.wing)
                        {
                            case Wing.bow:
                                translatedTargetList.Sort(SortByY);
                                translatedTargetList.Reverse();
                                break;
                            case Wing.port:
                                translatedTargetList.Sort(SortByX);
                                break;
                            case Wing.starboard:
                                translatedTargetList.Sort(SortByX);
                                translatedTargetList.Reverse();
                                break;
                            default:
                                print("NO WING DEFINED");
                                break;
                        }
                    }

                    //final list
                    foreach (Pushable pushable in translatedTargetList)
                    {
                        if (pushable.GetComponent<AttackReceiver>() != null)
                        {
                            newTargets.Add(pushable.GetComponent<AttackReceiver>());
                        }

                        if (pushable.GetComponent<Destroyable>() != null)
                        {
                            pushable.GetComponent<Destroyable>().DestroySelf();
                        }

                        if (ability.attackTarget == AttackTarget.beeline)
                        {
                            break;
                        }
                    }

                    break;

                case TargettingClass.support:

                    EnemyTarget randomEnemy = SelectRandomSupportTarget();

                    newTargets.Add(randomEnemy.GetComponent<AttackReceiver>());
                    break;

                case TargettingClass.self:

                    newTargets.Add(attackReceiver);
                    break;

                    //this case inherits "melee Target" from the TestAbility method. This is to reduce redundancy in finding available targets.
                case TargettingClass.melee:

                    if (meleeTarget == null)
                    {
                        print("no melee targets, ATTACK FILTERING ERROR");
                    }

                    newTargets.Add(meleeTarget.GetComponent<AttackReceiver>());

                    break;

                case TargettingClass.homing:

                    Fighter randomActor = SelectRandomHomingTarget();

                    if (randomActor != null)
                    {
                        newTargets.Add(randomActor.GetComponent<AttackReceiver>());
                    }

                    break;

                case TargettingClass.allAllies:

                    foreach (Fighter fighter in allActors)
                    {
                        if (!fighter.isDead)
                        {
                            if (!fighter.isIntangible)
                            {
                                newTargets.Add(fighter.GetComponent<AttackReceiver>());
                            }
                        }
                    }

                    break;

                case TargettingClass.allFoes:

                    foreach (EnemyTarget target in allEnemies)
                    {
                        newTargets.Add(target.GetComponent<AttackReceiver>());
                    }

                    break;

                default:
                    print("no valid targeting class error.");
                    break;
            }

            return newTargets;
        }

        private Fighter SelectRandomHomingTarget()
        {
            List<Fighter> availableActorTargets = new List<Fighter>();

            foreach (Fighter fighter in allActors)
            {
                if (!fighter.isIntangible && !fighter.isDead)
                {
                    availableActorTargets.Add(fighter);
                }
            }

            if (availableActorTargets.Count > 0)
            {
                int randomSelection = UnityEngine.Random.Range(0, availableActorTargets.Count);

                return availableActorTargets[randomSelection];
            }
            else
            {
                return null;
            }
        }

        //self explanitory
        private EnemyTarget SelectRandomSupportTarget()
        {

            //this should get filtered out but as a failsafe.
            if (allEnemies.Length <= 1)
            {
                print("no support targets, ATTACK FILTERING ERROR");
                return enemyTarget;
            }

            int randomSelection = UnityEngine.Random.Range(0, allEnemies.Length);

            while (allEnemies[randomSelection] == enemyTarget)
            {
                randomSelection = UnityEngine.Random.Range(0, allEnemies.Length);
            }

            return allEnemies[randomSelection];
        }

        //translates from global coordinates to relative coordinates
        private Vector2Int[] TranslateRelativeTargetCells(Vector2Int[] relativeTargetCells, EnemyTarget enemy)
        {
            Vector2Int[] convertedVector = new Vector2Int[relativeTargetCells.Length];
            int enemyX = enemy.GetComponent<Mover>().GetGridPos().x;
            int enemyY = enemy.GetComponent<Mover>().GetGridPos().y;


            for (var i = 0; i < relativeTargetCells.Length; i++)
            {
                switch (enemy.wing)
                {
                    case Wing.port:
                        convertedVector[i] = new Vector2Int(enemyX + relativeTargetCells[i].x, enemyY + relativeTargetCells[i].y);
                        break;
                    case Wing.starboard:
                        convertedVector[i] = new Vector2Int(enemyX - relativeTargetCells[i].x, enemyY + relativeTargetCells[i].y);
                        break;
                    case Wing.bow:
                        convertedVector[i] = new Vector2Int(enemyX - relativeTargetCells[i].y, enemyY - relativeTargetCells[i].x);
                        break;
                    default:
                        convertedVector[i] = new Vector2Int(0, 0);
                        Debug.LogError("enemy wing is not defined.");
                        break;
                }
            }

            return convertedVector;
        }








        //these are universal equations and should be accepssible by a public method base
        //CC.AA

        //CC.AA
        public static int SortByX(Fighter actor1, Fighter actor2)
        {
            return actor1.GetComponent<Mover>().GetGridPos().x.CompareTo(actor2.GetComponent<Mover>().GetGridPos().x);
        }

        //CC.AA
        public static int SortByX(Pushable pushable1, Pushable pushable2)
        {
            return pushable1.GetGridPos().x.CompareTo(pushable2.GetGridPos().x);
        }

        //CC.AA
        public static int SortByX(Destroyable destroyable1, Destroyable destroyable2)
        {
            return destroyable1.GetComponent<Pushable>().GetGridPos().x.CompareTo(destroyable2.GetComponent<Pushable>().GetGridPos().x);
        }

        //CC.AA
        public static int SortByY(Fighter actor1, Fighter actor2)
        {
            return actor1.GetComponent<Mover>().GetGridPos().y.CompareTo(actor2.GetComponent<Mover>().GetGridPos().y);
        }

        //CC.AA
        public static int SortByY(Pushable pushable1, Pushable pushable2)
        {
            return pushable1.GetGridPos().y.CompareTo(pushable2.GetGridPos().y);
        }

        //CC.AA
        public static int SortByY(Destroyable destroyable1, Destroyable destroyable2)
        {
            return destroyable1.GetComponent<Pushable>().GetGridPos().y.CompareTo(destroyable2.GetComponent<Pushable>().GetGridPos().y);
        }


        //same all the way down

    }
}