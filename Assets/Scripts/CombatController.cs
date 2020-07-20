using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TTW.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TTW.Graphics;

namespace TTW.Combat
{
    public class CombatController : MonoBehaviour
    {
        int yPosition = 1;
        int xPosition = 1;
        Vector2Int gridPosition;
        Cell targetCell;

        [SerializeField] public List<Fighter> actorInPosition = new List<Fighter>();
        [SerializeField] public EnemyTarget[] enemyInPosition = null;
        [SerializeField] public int selectedPosition = 0;
        [SerializeField] public int highlightedActorPosition = 0;
        [SerializeField] public int highlightedAllyPosition = 0;
        [SerializeField] public int highlightedAttackPosition = 0;
        [SerializeField] public int highlightedEnemyPosition = 0;
        [SerializeField] public List<PlayerSelector> selectorInPosition = new List<PlayerSelector>();
        [SerializeField] public AttackSelector[] attackSelectorInPosition = new AttackSelector[4];

        public float evasionDifferential = 40f;

        public List<AttackReceiver> selectedTargets = null;
        public static CombatController current;

        public Dictionary<Vector2Int, Cell> ListOfcells = new Dictionary<Vector2Int, Cell>();
        public Dictionary<Vector2Int, Cell> ListOfPlayerCells = new Dictionary<Vector2Int, Cell>();

        public enum State
        {
            actorSelect,
            actorMove,
            actorAttackSelect,
            attackTargetSelect,
            animationFreeze,
            pause
        };

        public State currentState = State.actorSelect;
        public State savedState = State.actorSelect;

        public bool pressedA = false;

        ActorTargetingType actorTargetingType = ActorTargetingType.none;

        private void Awake()
        {
            current = this;
        }

        void Start()
        {
            enemyInPosition = FindObjectsOfType<EnemyTarget>();
            UpdatePosition();

            GameEvents.current.onAnimationStart += AnimationFreeze;
            GameEvents.current.onAnimationEnd += AnimationFreezeEnd;
            GameEvents.current.onActorSelectRanged += RangedSelect;
            GameEvents.current.onActorSelectMelee += MeleeSelect;
            GameEvents.current.onActorSelectCell += CellSelect;
            GameEvents.current.onActorSelectSupport += SupportSelect;
            GameEvents.current.onActorSelectSelf += SelfSelect;
            GameEvents.current.onActorSelectAllies += AlliesSelect;
            GameEvents.current.onActorSelectFoes += FoesSelect;
            GameEvents.current.onActorSelectAdjacent += AdjacentSelect;
            GameEvents.current.onActorSelectSupportNotSelf += SupportNotSelfSelect;
        }

        private void Update()
        {

            CheckGridStatus();

            if (currentState == State.actorSelect)
            {
                ResetCellHighlights();

                if (currentState == State.animationFreeze) return;

                MoveThroughActorSelect();
                HighlightActorSelectors();
                SelectActor();
            }

            else if (currentState == State.actorMove)
            {
                EventSystem.current.SetSelectedGameObject(null);
                MoveThroughGrid();

                if (currentState == State.animationFreeze) return;

                ConfirmMove();
                HighlightCell();
                CheckDeathStatus();
            }

            else if (currentState == State.actorAttackSelect)
            {
                ResetCellHighlights();

                if (currentState == State.animationFreeze) return;

                MoveThroughAttackSelect();
                HighlightAttackSelectors();
                SelectAttack();
                CheckDeathStatus();
            }

            else if (currentState == State.attackTargetSelect)
            {

                EventSystem.current.SetSelectedGameObject(null);

                if (currentState == State.animationFreeze) return;

                CheckDeathStatus();

                if (actorTargetingType == ActorTargetingType.melee || actorTargetingType == ActorTargetingType.ranged)
                {
                    SelectTargetEnemy();
                    HighlightTargetEnemy();
                    ConfirmEnemy();
                }

                if (actorTargetingType == ActorTargetingType.cell)
                {
                    SelectTargetCell();
                }

                if (actorTargetingType == ActorTargetingType.support)
                {
                    SelectTargetAlly();
                    HighlightTargetAlly();
                    ConfirmAlly(true, false);
                }

                if (actorTargetingType == ActorTargetingType.adjacent)
                {
                    SelectTargetAlly();
                    HighlightTargetAlly();
                    ConfirmAlly(false, true);
                }

                if (actorTargetingType == ActorTargetingType.supportNotSelf)
                {
                    SelectTargetAlly();
                    HighlightTargetAlly();
                    ConfirmAlly(false, false);
                }

                if (actorTargetingType == ActorTargetingType.self)
                {
                    SelectSelf();
                    HighlightTargetAlly();
                    ConfirmAlly(true, false);
                }

                if (actorTargetingType == ActorTargetingType.allfoes)
                {
                    HighlightAllEnemies();
                    ConfirmAllEnemies();
                }

                if (actorTargetingType == ActorTargetingType.allallies)
                {
                    HighlightAllAllies();
                    ConfirmAllAllies();
                }
            }
        }

        private void CheckDeathStatus()
        {
            if (actorInPosition[selectedPosition].GetComponent<PlayerStats>().isDead)
            {
                ResetState();
            }
        }

        private void ConfirmAlly(bool includeSelf, bool adjacentOnly)
        {
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left };

            if (actorInPosition[selectedPosition].GetComponent<Channel>().isChanneling) return;


            if (Input.GetKeyDown(KeyCode.A))
            {
                if (actorInPosition[selectedPosition] == actorInPosition[highlightedAllyPosition] && !includeSelf)
                {
                    print("cannot select self!");
                    return;
                }

                if (adjacentOnly)
                {
                    bool goodToGo = false;

                    foreach (Vector2Int direction in directions)
                    {
                        if (actorInPosition[highlightedAllyPosition].GetComponent<Mover>().GetGridPos() == actorInPosition[selectedPosition].GetComponent<Mover>().GetGridPos() + direction)
                        {
                            goodToGo = true;
                        }
                    }

                    if (goodToGo == false)
                    {
                        print("ally not adjacent!");
                        return;
                    }
                }

                print("confirmed Ally");

                selectedTargets.Add(actorInPosition[highlightedAllyPosition].GetComponent<AttackReceiver>());

                actorInPosition[selectedPosition].StartChannel(selectedTargets, highlightedAttackPosition);

                ResetState();
            }
        }

        private void ConfirmAllAllies()
        {
            if (actorInPosition[selectedPosition].GetComponent<Channel>().isChanneling) return;

            Fighter[] allies = FindObjectsOfType<Fighter>();

            if (Input.GetKeyDown(KeyCode.A))
            {

                foreach (Fighter ally in allies)
                {
                    selectedTargets.Add(ally.GetComponent<AttackReceiver>());
                    
                }

                print("confirmed all allies");
                actorInPosition[selectedPosition].StartChannel(selectedTargets, highlightedAttackPosition);

                ResetState();
            }
        }

        private void ConfirmEnemy()
        {
            if (actorInPosition[selectedPosition].GetComponent<Channel>().isChanneling) return;

            if (Input.GetKeyDown(KeyCode.A))
            {
                ResetHighlightEnemies();

                if (!actorInPosition[selectedPosition].CheckAttack(enemyInPosition[highlightedEnemyPosition], highlightedAttackPosition))
                {
                    currentState = State.actorAttackSelect;
                    return;
                }

                selectedTargets.Add(enemyInPosition[highlightedEnemyPosition].GetComponent<AttackReceiver>());

                actorInPosition[selectedPosition].StartChannel(selectedTargets, highlightedAttackPosition);

                ResetState();
            }
        }

        private void ResetHighlightEnemies()
        {
            for (var i = 0; i < enemyInPosition.Length; i++)
            {
                enemyInPosition[i].GetComponent<TTW.Graphics.Outline>().enabled = false;
            }
        }

        private void ConfirmAllEnemies()
        {
            if (actorInPosition[selectedPosition].GetComponent<Channel>().isChanneling) return;

            if (Input.GetKeyDown(KeyCode.A))
            {
                ResetHighlightEnemies();

                //AnimationFreeze();

                foreach (EnemyTarget enemy in enemyInPosition)
                {
                    selectedTargets.Add(enemy.GetComponent<AttackReceiver>());
                }

                actorInPosition[selectedPosition].StartChannel(selectedTargets, highlightedAttackPosition);

                ResetState();
            }
        }

        private void ConfirmTargetCell()
        {
            if (actorInPosition[selectedPosition].GetComponent<Channel>().isChanneling) return;

            if (Input.GetKeyDown(KeyCode.A))
            {
                if (actorInPosition[highlightedActorPosition].GetComponent<TargetingManager>().CellTargeting(ListOfPlayerCells[gridPosition]))
                {
                    actorInPosition[selectedPosition].PerformAttack(ListOfPlayerCells[gridPosition], highlightedAttackPosition);

                    ResetState();
                }
            }
        }

        private void HighlightTargetAlly()
        {
           
            Fighter[] allies = FindObjectsOfType<Fighter>();

            foreach (Fighter ally in allies)
            {
                if (ally == actorInPosition[highlightedAllyPosition])
                {
                    print(actorInPosition[highlightedAllyPosition] + "selected");
                }
            }
        }

        private void CheckGridStatus()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Cell[] allcells = FindObjectsOfType<Cell>();

                foreach (Cell cell in allcells)
                {
                    print("cell " + cell + " Occupied Status is " + cell.isOccupied);
                    print("cell " + cell + " Explored Status is " + cell.isExplored);
                    print("cell " + cell + " Enemy Cell Status is " + cell.isEnemyCell);
                }
            }
        }

        public void AddNewKey(Vector2Int key, Cell value)
        {
            if (!ListOfcells.ContainsKey(key))
            {
                ListOfcells.Add(key, value);
            }
        }

        public void AddNewPlayerCellKey(Vector2Int key, Cell value)
        {
            if (!ListOfPlayerCells.ContainsKey(key))
            {
                ListOfPlayerCells.Add(key, value);
            }
        }

        public void AnimationFreeze()
        {
            if (currentState != State.animationFreeze)
            {
                savedState = currentState;
            }
            currentState = State.animationFreeze;
        }

        private void HighlightTargetEnemy()
        {
            EnemyTarget[] totalEnemies = FindObjectsOfType<EnemyTarget>();

            foreach (EnemyTarget enemy in totalEnemies)
            {
                if (enemy == enemyInPosition[highlightedEnemyPosition])
                {
                    enemy.GetComponent<TTW.Graphics.Outline>().enabled = true;
                }
                else
                {
                    enemy.GetComponent<TTW.Graphics.Outline>().enabled = false;
                }
            }
        }

        private void HighlightAllEnemies()
        {
            EnemyTarget[] totalEnemies = FindObjectsOfType<EnemyTarget>();

            foreach (EnemyTarget enemy in totalEnemies)
            {
                enemy.GetComponent<TTW.Graphics.Outline>().enabled = true;
                print("all enemies selected");
            }
        }

        private void HighlightAllAllies()
        {
            Fighter[] allies = FindObjectsOfType<Fighter>();

            foreach (Fighter ally in allies)
            {
                print("all allies selected");
            }
        }

        private void SelectTargetEnemy()
        {
            EnemyTarget[] totalEnemies = FindObjectsOfType<EnemyTarget>();
            

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (highlightedEnemyPosition > 0)
                {
                    highlightedEnemyPosition--;
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (highlightedEnemyPosition < totalEnemies.Length - 1)
                {
                    highlightedEnemyPosition++;
                }
            }
        }

        private void SelectTargetCell()
        {
            MoveThroughGrid();
            HighlightCell();
            ConfirmTargetCell();
        }

        private void SelectTargetAlly()
        {
            Fighter[] allies = FindObjectsOfType<Fighter>();



            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (highlightedAllyPosition > 0)
                {
                    highlightedAllyPosition--;
                    print("currently highlighting: " + highlightedAllyPosition);
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (highlightedAllyPosition < allies.Length - 1)
                {
                    highlightedAllyPosition++;
                    print("currently highlighting: " + highlightedAllyPosition);
                }
            }
        }

        private void SelectSelf()
        {
            Fighter[] allies = FindObjectsOfType<Fighter>();

            for (var i = 0; i<allies.Length; i++)
            {
                if (actorInPosition[i] == actorInPosition[highlightedActorPosition])
                {
                    highlightedAllyPosition = i;
                    print("I am " + actorInPosition[highlightedActorPosition]);
                }
            }
        }

        private void SelectAttack()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                //currentState = State.attackTargetSelect;
                if (actorInPosition[selectedPosition].abilities[highlightedAttackPosition].legendary && actorInPosition[selectedPosition].usedLegendary)
                {
                    print("legendary ability already used!");
                    return;
                }


                actorInPosition[selectedPosition].ReturnActorSelection();

                ResetCellHighlights();
            }
        }

        private void HighlightAttackSelectors()
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(attackSelectorInPosition[highlightedAttackPosition].gameObject);
        }

        private void MoveThroughAttackSelect()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (highlightedAttackPosition > 0)
                {
                    highlightedAttackPosition--;
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (highlightedAttackPosition < attackSelectorInPosition.Length - 1)
                {
                    highlightedAttackPosition++;
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentState = State.actorMove;
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        private void MoveThroughActorSelect()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (highlightedActorPosition > 0)
                {
                    highlightedActorPosition--;
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (highlightedActorPosition+1 < actorInPosition.Count)
                {
                    highlightedActorPosition++;
                }
            }
        }

        private void MoveThroughGrid()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && xPosition > 1)
            {
                 xPosition--;
                 UpdatePosition();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) && xPosition < 4)
            {
                xPosition++;
                UpdatePosition();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && yPosition < 2)
            {
                yPosition++;
                UpdatePosition();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) && yPosition > 0)
            {
                yPosition--;
                UpdatePosition();
            }
        }

        private void ConfirmMove()
        {

            if (Input.GetKeyDown(KeyCode.A))
            {

                Cell[] cells = FindObjectsOfType<Cell>();

                foreach (Cell cell in cells)
                {
                    foreach (Fighter actor in actorInPosition)
                    {
                        Mover mover = actor.GetComponent<Mover>();

                        if (cell.GetGridPos() == gridPosition && actorInPosition[selectedPosition] == actor)
                        {
                            if (cell.GetGridPos() == mover.GetGridPos())
                            {
                                currentState = State.actorAttackSelect;
                            }
                            else
                            {
                                if (!cell.isOccupied)
                                {
                                    if (CheckIfParalyzed(actorInPosition[selectedPosition].GetComponent<StatusEffectManager>()))
                                    {
                                        print(actorInPosition[selectedPosition].name + " is PARALYZED!");
                                        return;
                                    }


                                    print(selectedPosition + " is the selected position");
                                    print(actorInPosition[selectedPosition].name + " is moving");
                                    mover.Pathfind(cell);
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool CheckIfParalyzed(StatusEffectManager statusEffectManager)
        {
            if (statusEffectManager.statusEffectBad == StatusEffect.paralyze)
            {
                return true;
            }

            return false;
        }

        private void SelectActor()
        {

            if (
                selectorInPosition[highlightedActorPosition].gameObject == EventSystem.current.currentSelectedGameObject
                &&
                actorInPosition[highlightedActorPosition].GetComponent<CoolDown>().isOnCD == false
                &&
                actorInPosition[highlightedActorPosition].GetComponent<PlayerStats>().isDead == false
                &&
                actorInPosition[highlightedActorPosition].GetComponent<Channel>().isChanneling == false
                )
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    selectedPosition = highlightedActorPosition;
                    currentState = State.actorMove;
                }
            }

            if (actorInPosition[highlightedActorPosition].GetComponent<PlayerStats>().isDead == true)
            {
                RemoveActor();
            }
        }

        private void RemoveActor()
        {
            print("remove character from battle? Press A to confirm");

            if (Input.GetKeyDown(KeyCode.A))
            {
                actorInPosition[highlightedActorPosition].Remove();
            }
        }

        private void UpdatePosition()
        {
            gridPosition = new Vector2Int(xPosition, yPosition);
            print(gridPosition + " cell is currently highlighted");
            //ListOfcells[gridPosition].GetComponent<TTW.Graphics.Outline>().enabled = true;
        }

        private void HighlightCell()
        {
            Cell[] cells = FindObjectsOfType<Cell>();
            foreach (Cell cell in cells)
            {
                if (cell.GetGridPos() == gridPosition)
                {
                    cell.GetComponent<TTW.Graphics.Outline>().enabled = true;
                    

                    if (cell.GetGridPos() == actorInPosition[selectedPosition].GetComponent<Mover>().GetGridPos())
                    {
                        cell.DisplayActorIcon();
                        cell.DoConfirmationColor();
                        cell.GetComponent<TTW.Graphics.Outline>().OutlineColor = cell.GetComponent<TTW.Graphics.Outline>().confirmColor;
                        continue;
                    }

                    if (cell.isOccupied)
                    {
                        cell.DisplayInvalidIcon();
                        cell.DoErrorColor();
                        cell.GetComponent<TTW.Graphics.Outline>().OutlineColor = cell.GetComponent<TTW.Graphics.Outline>().invalidColor;
                        continue;
                    }

                    cell.DisplayMoveIcon();
                    cell.DoHighlightColor();
                    cell.GetComponent<TTW.Graphics.Outline>().OutlineColor = cell.GetComponent<TTW.Graphics.Outline>().selectionColor;
                }
                else
                {
                    cell.DoDefaultColor();
                    cell.GetComponent<TTW.Graphics.Outline>().enabled = false;
                    cell.DestroyIcons();
                }
            }
        }

        private void HighlightActorSelectors()
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(selectorInPosition[highlightedActorPosition].gameObject);
        }

        private void ResetCellHighlights()
        {
            Cell[] cells = FindObjectsOfType<Cell>();
            foreach (Cell cell in cells)
            {
                cell.DoDefaultColor();
            }
        }

        public void AnimationFreezeEnd()
        {
            currentState = savedState;
        }

        private void RangedSelect()
        {
            currentState = State.attackTargetSelect;
            actorTargetingType = ActorTargetingType.ranged;
        }

        private void MeleeSelect()
        {
            currentState = State.attackTargetSelect;
            actorTargetingType = ActorTargetingType.melee;
        }

        private void CellSelect()
        {
            currentState = State.attackTargetSelect;
            actorTargetingType = ActorTargetingType.cell;
        }

        private void SupportSelect()
        {
            currentState = State.attackTargetSelect;
            actorTargetingType = ActorTargetingType.support;
        }

        private void SelfSelect()
        {
            currentState = State.attackTargetSelect;
            actorTargetingType = ActorTargetingType.self;
        }

        private void AlliesSelect()
        {
            currentState = State.attackTargetSelect;
            actorTargetingType = ActorTargetingType.allallies;
        }

        private void FoesSelect()
        {
            currentState = State.attackTargetSelect;
            actorTargetingType = ActorTargetingType.allfoes;
        }

        private void AdjacentSelect()
        {
            currentState = State.attackTargetSelect;
            actorTargetingType = ActorTargetingType.adjacent;
        }

        private void SupportNotSelfSelect()
        {
            currentState = State.attackTargetSelect;
            actorTargetingType = ActorTargetingType.supportNotSelf;
        }

        public void ResetState()
        {
            print("reseting state");
            currentState = State.actorSelect;
            savedState = currentState;
            selectedTargets.Clear();
        }

        public void ResetHighlighters()
        {
            selectedPosition = 0;
            highlightedActorPosition = 0;
            highlightedAllyPosition = 0;
            highlightedAttackPosition = 0;
            highlightedEnemyPosition = 0;
        }

        public bool IsEndAnimationReady()
        {
            Mover[] movers = FindObjectsOfType<Mover>(); //for pushClear
            OnRailsMover[] onRailsMovers = FindObjectsOfType<OnRailsMover>(); //for railsClear
            AnimationHandler[] animationHandlers = FindObjectsOfType<AnimationHandler>(); //for animClear

            foreach (Mover mover in movers)
            {
                if (mover.isBeingPushed)
                {
                    print("something is being pushed");
                    return false;
                    
                }
            }

            foreach (OnRailsMover onRailsMover in onRailsMovers)
            {
                if (onRailsMover.isMoving)
                {
                    print("something is being railed");
                    return false;
                }
            }

            foreach (AnimationHandler animationHandler in animationHandlers)
            {
                if (animationHandler.isAnimating)
                {
                    print("something is being animated");
                    return false;
                }
            }

            return true;
        }
    }
}
