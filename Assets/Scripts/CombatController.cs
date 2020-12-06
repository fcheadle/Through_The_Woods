using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TTW.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TTW.Combat
{
    public class CombatController : MonoBehaviour
    {
        int yPosition = 1;
        int xPosition = 1;
        Vector2Int gridPosition;
        Cell targetCell;

        public FollowActor uiSelector;
        public Canvas uiCanvas;
        public UIDotLine uiDotLine;

        [SerializeField] public List<Fighter> actorInPosition = new List<Fighter>();
        [SerializeField] public List<Fighter> actorOnBench = new List<Fighter>();
        [SerializeField] public List<Fighter> actorsRemoved = new List<Fighter>();
        [SerializeField] public EnemyTarget[] enemyInPosition = null;
        [SerializeField] public int selectedPosition = 0;
        [SerializeField] public int highlightedActorPosition = 0;
        [SerializeField] public int highlightedActiveActorPosition = 0;
        [SerializeField] public int highlightedBenchActorPosition = 0;
        [SerializeField] public int highlightedAllyPosition = 0;
        [SerializeField] public int highlightedAttackPosition = 0;
        [SerializeField] public int highlightedEnemyPosition = 0;
        [SerializeField] public UISelector[] selectorInPosition = new UISelector[4];

        [SerializeField] public BenchSelector[] benchSelectorInPosition = new BenchSelector[10];


        private Fighter[] actor = new Fighter[13];

        private BenchSelector bs1;
        private BenchSelector bs2;
        private BenchSelector bs3;
        private BenchSelector bs4;
        private BenchSelector bs5;
        private BenchSelector bs6;
        private BenchSelector bs7;
        private BenchSelector bs8;
        private BenchSelector bs9;
        private BenchSelector bs10;

        public float evasionDifferential = 40f;

        public List<AttackReceiver> selectedTargets = null;
        public static CombatController current;

        public Dictionary<Vector2Int, Cell> ListOfcells = new Dictionary<Vector2Int, Cell>();
        public Dictionary<Vector2Int, Cell> ListOfPlayerCells = new Dictionary<Vector2Int, Cell>();

        public CoolDownTimer[] actorCoolDownInPosition = new CoolDownTimer[3];
        public ChannelTimer[] actorChannelInPosition = new ChannelTimer[3];

        MovementPathMaker pathMaker;

        public enum State
        {
            actorSelect,
            actorMove,
            actorAttackSelect,
            attackTargetSelect,
            animationFreeze,
            pause,
            swapBenchSelect,
            swapBench
        };

        public State currentState = State.actorSelect;
        public State savedState = State.actorSelect;
        public State previousState = State.actorSelect;

        public bool pressedA = false;

        ActorTargetingType actorTargetingType = ActorTargetingType.none;

        int targetableEnemyInPosition = 0;
        int targetableAllyInPosition = 0;
        List<int> targetablePositions = new List<int>();
        List<int> targetableAllyPositions = new List<int>();
        List<int> availableFighters = new List<int>();
        bool doEstablishGrid = false;

        private void Awake()
        {
            current = this;
        }

        void Start()
        {

            enemyInPosition = FindObjectsOfType<EnemyTarget>();
            UpdatePosition();
            CreateActors();

            uiSelector.SetNewTarget(actorInPosition[highlightedActorPosition].gameObject, false);
            pathMaker = FindObjectOfType<MovementPathMaker>();

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
            GameEvents.current.onActorSelectCardinalAllies += CardinalAllySelect;
        }

        private void Update()
        {
            //print("path: " + pathMaker.noPath);
            CheckGridStatus();
            GoBack();

            if (currentState == State.actorSelect)
            {
                doEstablishGrid = true;

                previousState = State.actorSelect;

                if (currentState == State.animationFreeze) return;

                if (uiSelector.currentState != UIState.selectPlayer)
                {
                    uiSelector.SetState(UIState.selectPlayer);
                    uiSelector.SetNewTarget(actorInPosition[highlightedActorPosition].gameObject, true);
                }

                if (actorInPosition[highlightedActorPosition].GetComponent<StateMachine>().state == Combat.State.channeling)
                {
                    uiSelector.SetColor(new Color(0.6f, 0f, 1f));
                }
                else if (actorInPosition[highlightedActorPosition].GetComponent<StateMachine>().state == Combat.State.cooldown)
                {
                    uiSelector.SetColor(new Color(0.15f, 0.15f, 0.15f));
                }
                else
                {
                    uiSelector.SetColor(Color.white);
                }

                if (!CheckHighlightedActorForCoolDown(highlightedActorPosition))
                {
                    SpawnActorBoxes();
                }
                else
                {
                    DestroySelectors();
                }

                MoveThroughActorSelect();
                SelectActor();
            }

            else if (currentState == State.swapBenchSelect)
            {
                DestroyBenchBoxes();

                if (uiSelector.currentState != UIState.swapBench)
                {
                    Bench bench = FindObjectOfType<Bench>();
                    uiSelector.SetState(UIState.swapBench);
                    uiSelector.SetNewTarget(bench.gameObject, true);
                }

                previousState = State.actorSelect;

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    Bench bench = FindObjectOfType<Bench>();
                    bench.GetComponent<MeshRenderer>().enabled = false;
                    currentState = State.actorMove;
                    bool unbrokenPath = pathMaker.CheckPath(ListOfPlayerCells[actorInPosition[highlightedActorPosition].GetComponent<Mover>().GetGridPos()], ListOfPlayerCells[gridPosition]);
                    pathMaker.ClearPath();
                    pathMaker.DrawPath(ListOfPlayerCells[actorInPosition[highlightedActorPosition].GetComponent<Mover>().GetGridPos()], ListOfPlayerCells[gridPosition]);
                    HighlightCell(unbrokenPath);
                }

                if (Input.GetKeyDown(KeyCode.A))
                {
                    currentState = State.swapBench;
                    CreateBenchIcons();
                }
            }

            else if (currentState == State.swapBench)
            {
                previousState = State.swapBenchSelect;
                MoveThroughBench();
                SwitchBenchedActor(highlightedActorPosition, highlightedBenchActorPosition);
            }

            else if (currentState == State.actorMove)
            {
                previousState = State.actorSelect;
                //EventSystem.current.SetSelectedGameObject(null);

                if (doEstablishGrid == true)
                {
                    doEstablishGrid = false;
                    EstablishGridPosition();
                    HighlightCell(false);
                }

                MoveThroughGrid();
                //

                if (currentState == State.animationFreeze) return;

                ConfirmMove();
                CheckDeathStatus();
            }

            else if (currentState == State.actorAttackSelect)
            {
                doEstablishGrid = true;
                previousState = State.actorMove;
                //attackSelector1.LinkToActor(actorInPosition[selectedPosition]);
                //attackSelector2.LinkToActor(actorInPosition[selectedPosition]);
                //attackSelector3.LinkToActor(actorInPosition[selectedPosition]);
                //attackSelector4.LinkToActor(actorInPosition[selectedPosition]);

                if (currentState == State.animationFreeze) return;

                SpawnAttackBoxes();

                if (uiSelector.currentState != UIState.selectAttack)
                {
                    uiSelector.SetState(UIState.selectAttack);
                    uiSelector.SetNewTarget(actorInPosition[selectedPosition].gameObject, true);
                    GetComponent<ActorAttackIndicator>().ResetIndicators();
                    ResetHighlightEnemies();
                }

                MoveThroughAttackSelect();
                SelectAttack();
                CheckDeathStatus();
            }

            else if (currentState == State.attackTargetSelect)
            {
                previousState = State.actorAttackSelect;

                EventSystem.current.SetSelectedGameObject(null);

                DestroySelectors();

                if (currentState == State.animationFreeze) return;

                CheckDeathStatus();

                CalculateAttackTargets();
            }
        }

        private void CalculateAttackTargets()
        {
            if (actorTargetingType == ActorTargetingType.melee)
            {
                if (uiSelector.currentState != UIState.selectMelee)
                {
                    targetablePositions.Clear();

                    uiSelector.SetState(UIState.selectMelee);

                    GetComponent<ActorAttackIndicator>().IndicateMelee(actorInPosition[selectedPosition]);

                    foreach (EnemyTarget enemy in enemyInPosition)
                    {
                        if (actorInPosition[selectedPosition].CheckAttack(enemy, highlightedAttackPosition))
                        {
                            for (int i = 0; i < enemyInPosition.Length; i++)
                            {
                                if (enemyInPosition[i] == enemy)
                                {
                                    targetablePositions.Add(i);
                                }
                            }
                        }
                    }

                    if (targetablePositions.Count > 0)
                    {
                        uiSelector.SetNewTarget(enemyInPosition[targetablePositions[0]].gameObject, true);
                    }
                    else
                    {
                        uiSelector.SetState(UIState.selectInvalid);
                        return;
                    }
                }

                SelectTargetEnemyMelee(targetablePositions);
                ConfirmEnemy(enemyInPosition[targetablePositions[targetableEnemyInPosition]].GetComponent<AttackReceiver>());
            }

            if (actorTargetingType == ActorTargetingType.ranged)
            {

                if (uiSelector.currentState != UIState.selectRanged)
                {
                    uiSelector.SetState(UIState.selectRanged);
                    EstablishEnemyTarget();
                    uiSelector.SetNewTarget(enemyInPosition[highlightedEnemyPosition].gameObject, true);
                    RenderTargetLinesSingle(actorInPosition[selectedPosition].transform.position, 5f, enemyInPosition[highlightedEnemyPosition].transform.position, Color.magenta);
                }

                SelectTargetEnemyRanged();
                ConfirmEnemy(enemyInPosition[highlightedEnemyPosition].GetComponent<AttackReceiver>());
            }

            if (actorTargetingType == ActorTargetingType.cell)
            {
                if (uiSelector.currentState != UIState.selectCell)
                {
                    uiSelector.SetState(UIState.selectCell);
                    uiSelector.SetNewTarget(ListOfPlayerCells[gridPosition].gameObject, true);
                }
                SelectTargetCell();
            }

            if (actorTargetingType == ActorTargetingType.support)
            {
                if (uiSelector.currentState != UIState.selectAlly)
                {
                    uiSelector.SetState(UIState.selectAlly);
                    EstablishAllyTarget();
                    uiSelector.SetNewTarget(actorInPosition[highlightedAllyPosition].gameObject, true);
                    RenderTargetLinesSingle(actorInPosition[selectedPosition].transform.position, 5f, actorInPosition[highlightedAllyPosition].transform.position, Color.magenta);
                }

                SelectTargetAlly();
                ConfirmAlly(true, false, actorInPosition[highlightedAllyPosition]);
            }

            if (actorTargetingType == ActorTargetingType.cardinalAlly)
            {
                if (uiSelector.currentState != UIState.selectCardinalAlly)
                {
                    uiSelector.SetState(UIState.selectCardinalAlly);
                    EstablishCardinalAllyTarget();
                    uiSelector.SetNewTarget(actorInPosition[highlightedAllyPosition].gameObject, true);
                    GetComponent<ActorAttackIndicator>().IndicateMelee(actorInPosition[selectedPosition]);
                }

                SelectTargetCardinalAlly();
                ConfirmAlly(true, false, actorInPosition[highlightedAllyPosition]);
            }

            if (actorTargetingType == ActorTargetingType.adjacent)
            {
                if (uiSelector.currentState != UIState.selectAlly)
                {
                    targetableAllyPositions.Clear();

                    uiSelector.SetState(UIState.selectAlly);

                    GetComponent<ActorAttackIndicator>().IndicateAdjacent(ListOfPlayerCells[actorInPosition[selectedPosition].GetComponent<Mover>().GetGridPos()]);

                    Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.left, Vector2Int.down };

                    for (var i = 0; i < actorInPosition.Count; i++)
                    {
                        foreach (Vector2Int direction in directions)
                        {
                            if (actorInPosition[i].GetComponent<Mover>().GetGridPos() == actorInPosition[selectedPosition].GetComponent<Mover>().GetGridPos() + direction)
                            {

                                targetableAllyPositions.Add(i);
                            }
                        }
                    }

                    if (targetableAllyPositions.Count > 0)
                    {

                        uiSelector.SetNewTarget(actorInPosition[targetableAllyPositions[0]].gameObject, true);
                    }
                    else
                    {

                        uiSelector.SetState(UIState.selectInvalid);
                        return;
                    }
                }

                SelectTargetAllyAdjacent();
                ConfirmAlly(false, true, actorInPosition[targetableAllyPositions[targetableAllyInPosition]]);
            }

            if (actorTargetingType == ActorTargetingType.supportNotSelf)
            {
                if (uiSelector.currentState != UIState.selectAlly)
                {
                    uiSelector.SetState(UIState.selectAlly);
                    EstablishAllyTarget();
                    uiSelector.SetNewTarget(actorInPosition[highlightedAllyPosition].gameObject, true);
                    RenderTargetLinesSingle(actorInPosition[selectedPosition].transform.position, 5f, actorInPosition[highlightedAllyPosition].transform.position, Color.magenta);
                }

                SelectTargetAlly();
                ConfirmAlly(false, false, actorInPosition[highlightedAllyPosition]);
            }

            if (actorTargetingType == ActorTargetingType.self)
            {
                if (uiSelector.currentState != UIState.selectAlly)
                {
                    Vector3 arcPeakHeight = new Vector3(0, 20f, 0);
                    Vector3 actorHeightOffset = new Vector3(0, 5f, 0);
                    uiSelector.SetState(UIState.selectAlly);
                    uiSelector.SetNewTarget(actorInPosition[selectedPosition].gameObject, true);
                }

                SelectSelf();
                ConfirmAlly(true, false, actorInPosition[selectedPosition]);
            }

            if (actorTargetingType == ActorTargetingType.allfoes)
            {
                if (uiSelector.currentState != UIState.selectRanged)
                {
                    Vector3 actorHeightOffset = new Vector3(0, 5f, 0);
                    EnemyTarget[] enemies = FindObjectsOfType<EnemyTarget>();
                    Vector3[] enemyPositions = new Vector3[enemies.Length];
                    for (int i = 0; i < enemies.Length; i++)
                    {
                        enemyPositions[i] = enemies[i].transform.position;
                    }
                    uiSelector.SetState(UIState.selectRanged);
                    uiSelector.SetNewTarget(actorInPosition[highlightedAllyPosition].gameObject, true);

                    RenderTargetLinesMultiple(actorInPosition[selectedPosition].transform.position, 5f, enemyPositions, Color.magenta);
                }

                ConfirmAllEnemies();
            }

            if (actorTargetingType == ActorTargetingType.allallies)
            {
                if (uiSelector.currentState != UIState.selectAlly)
                {
                    Fighter[] fighters = FindObjectsOfType<Fighter>();
                    Vector3[] fighterPositions = new Vector3[fighters.Length];

                    for (int i = 0; i < fighters.Length; i++)
                    {
                        fighterPositions[i] = fighters[i].transform.position;
                    }
                    uiSelector.SetState(UIState.selectAlly);
                    uiSelector.SetNewTarget(actorInPosition[highlightedAllyPosition].gameObject, true);

                    RenderTargetLinesMultiple(actorInPosition[selectedPosition].transform.position, 5f, fighterPositions, Color.magenta);

                }

                ConfirmAllAllies();
            }
        }

        private void SpawnActorBoxes()
        {
            if (selectorInPosition[0].GetComponent<Text>().text == " ")
            {

                for (int i = 0; i < 4; i++)
                {

                    if (selectorInPosition[i] != null)
                    {
                        selectorInPosition[i].ClearText();
                    }
                }

                for (int i = 0; i < actorInPosition.Count; i++)
                {
                    if (actorInPosition[i] != null)
                        {
                            Vector2 spacing = new Vector2(264f, 832f - (32 * i));
                            selectorInPosition[i].LinkToActor(actorInPosition[i], false);
                            selectorInPosition[i].GetComponent<RectTransform>().anchoredPosition = spacing;
                            selectorInPosition[i].SlideIntoPosition(i);
                        }
                }

                UpdateUIDotLine(actorInPosition[selectedPosition].transform, selectorInPosition[1].GetComponent<RectTransform>());
                UpdateActorUI();
            }
        }

        private void DestroyBenchBoxes()
        {
            if (bs1 != null)
            {
                bs1.DestroySelf();

                if (bs2 != null)
                {
                    bs2.DestroySelf();
                }
                if (bs3 != null)
                {
                    bs3.DestroySelf();
                }
                if (bs4 != null)
                {
                    bs4.DestroySelf();
                }
                if (bs5 != null)
                {
                    bs5.DestroySelf();
                }
                if (bs6 != null)
                {
                    bs6.DestroySelf();
                }
                if (bs7 != null)
                {
                    bs7.DestroySelf();
                }
                if (bs8 != null)
                {
                    bs8.DestroySelf();
                }
                if (bs9 != null)
                {
                    bs9.DestroySelf();
                }
                if (bs10 != null)
                {
                    bs10.DestroySelf();
                }
            }
        }

        private void MoveThroughBench()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (highlightedBenchActorPosition > 0)
                {
                    highlightedBenchActorPosition--;
                }
                else
                {
                    highlightedBenchActorPosition = actorOnBench.Count - 1;
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (highlightedBenchActorPosition < (actorOnBench.Count - 1))
                {
                    highlightedBenchActorPosition++;
                }
                else
                {
                    highlightedBenchActorPosition = 0;
                }
            }
        }

        private void CreateBenchIcons()
        {
            Bench bench = FindObjectOfType<Bench>();

            if (bs1 == null)
            {
                Vector3 spacing = new Vector3(0, 5f, 0);

                if (benchSelectorInPosition[0] != null)
                {
                    bs1 = Instantiate(benchSelectorInPosition[0]);
                    bs1.transform.position = bench.transform.position + spacing;
                    bs1.SlideIntoPosition(1);
                }

                if (benchSelectorInPosition[1] != null)
                {
                    bs2 = Instantiate(benchSelectorInPosition[1]);
                    bs2.transform.position = bench.transform.position + spacing;
                    bs2.SlideIntoPosition(2);
                }

                if (benchSelectorInPosition[2] != null)
                {
                    bs3 = Instantiate(benchSelectorInPosition[2]);
                    bs3.transform.position = bench.transform.position + spacing;
                    bs3.SlideIntoPosition(3);
                }

                if (benchSelectorInPosition[3] != null)
                {
                    bs4 = Instantiate(benchSelectorInPosition[3]);
                    bs4.transform.position = bench.transform.position + spacing;
                    bs4.SlideIntoPosition(4);
                }

                if (benchSelectorInPosition[4] != null)
                {
                    bs5 = Instantiate(benchSelectorInPosition[4]);
                    bs5.transform.position = bench.transform.position + spacing;
                    bs5.SlideIntoPosition(5);
                }

                if (benchSelectorInPosition[5] != null)
                {
                    bs6 = Instantiate(benchSelectorInPosition[5]);
                    bs6.transform.position = bench.transform.position + spacing;
                    bs6.SlideIntoPosition(6);
                }

                if (benchSelectorInPosition[6] != null)
                {
                    bs7 = Instantiate(benchSelectorInPosition[6]);
                    bs7.transform.position = bench.transform.position + spacing;
                    bs7.SlideIntoPosition(7);
                }

                if (benchSelectorInPosition[7] != null)
                {
                    bs8 = Instantiate(benchSelectorInPosition[7]);
                    bs8.transform.position = bench.transform.position + spacing;
                    bs8.SlideIntoPosition(8);
                }

                if (benchSelectorInPosition[8] != null)
                {
                    bs9 = Instantiate(benchSelectorInPosition[8]);
                    bs9.transform.position = bench.transform.position + spacing;
                    bs9.SlideIntoPosition(9);
                }

                if (benchSelectorInPosition[9] != null)
                {
                    bs10 = Instantiate(benchSelectorInPosition[9]);
                    bs10.transform.position = bench.transform.position + spacing;
                    bs10.SlideIntoPosition(10);
                }
            }
        }

        private void SelectTargetAllyAdjacent()
        {
            if (targetableAllyPositions.Count < 1)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {

                if (targetableAllyInPosition > 0)
                {
                    targetableAllyInPosition--;
                }
                else
                {
                    targetableAllyInPosition = targetableAllyPositions.Count - 1;
                }

                uiSelector.SetNewTarget(actorInPosition[targetableAllyPositions[targetableAllyInPosition]].gameObject, true);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (targetableAllyInPosition < targetableAllyPositions.Count - 1)
                {
                    targetableAllyInPosition++;
                }
                else
                {
                    targetableAllyInPosition = 0;
                }

                uiSelector.SetNewTarget(actorInPosition[targetableAllyPositions[targetableAllyInPosition]].gameObject, true);
            }
        }

        private void EstablishEnemyTarget()
        {
            highlightedEnemyPosition = 0;
        }

        private void EstablishAllyTarget()
        {
            for (int i = 0; i < actorInPosition.Count - 1; i++)
            {
                if (actorInPosition[i] != actorInPosition[selectedPosition])
                {
                    if (actorInPosition[i].GetComponent<PlayerStats>().isDead == true)
                    {
                        continue;
                    }

                    highlightedAllyPosition = i;
                    return;
                }
            }

            highlightedAllyPosition = selectedPosition;
        }

        private void EstablishCardinalAllyTarget()
        {
            for (int i = 0; i < actorInPosition.Count; i++)
            {
                if (actorInPosition[i] != actorInPosition[selectedPosition])
                {
                    if (actorInPosition[i].GetComponent<PlayerStats>().isDead == true)
                    {
                        continue;
                    }

                    if (actorInPosition[i].GetComponent<Mover>().GetGridPos().x != actorInPosition[selectedPosition].GetComponent<Mover>().GetGridPos().x &&
                        actorInPosition[i].GetComponent<Mover>().GetGridPos().y != actorInPosition[selectedPosition].GetComponent<Mover>().GetGridPos().y)
                    {
                        continue;
                    }

                    highlightedAllyPosition = i;
                    return;
                }
            }

            highlightedAllyPosition = selectedPosition;
        }

        private void RenderTargetLinesSingle(Vector3 caster, float yOffset, Vector3 target, Color color)
        {
            ArcRenderer arcRenderer = FindObjectOfType<ArcRenderer>();
            Vector3 heightOffset = new Vector3(0, yOffset, 0);
            Vector3 arcPeakHeight = new Vector3(0, 20f, 0);

            arcRenderer.ActivateLineRenderers(1);
            arcRenderer.DrawQuadraticCurve(caster + heightOffset, ((caster + target) / 2) + arcPeakHeight, target, 0);
            arcRenderer.SetColor(color);
        }

        private void RenderTargetLinesMultiple(Vector3 caster, float yOffset, Vector3[] targets, Color color)
        {
            ArcRenderer arcRenderer = FindObjectOfType<ArcRenderer>();
            Vector3 heightOffset = new Vector3(0, yOffset, 0);

            arcRenderer.ActivateLineRenderers(targets.Length);
            arcRenderer.DrawQuadraticCurveMultiple(caster + heightOffset, targets, 20f);
            arcRenderer.SetColor(color);
        }

        private void EstablishGridPosition()
        {
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left };

            gridPosition = actorInPosition[selectedPosition].GetComponent<Mover>().GetGridPos();

            uiSelector.SetState(UIState.confirmMovement);

            foreach (Vector2Int direction in directions)
            {
                if (ListOfcells.ContainsKey(actorInPosition[selectedPosition].GetComponent<Mover>().GetGridPos() + direction))
                {
                    if (!ListOfcells[actorInPosition[selectedPosition].GetComponent<Mover>().GetGridPos() + direction].isEnemyCell)
                    {
                        if (!ListOfcells[actorInPosition[selectedPosition].GetComponent<Mover>().GetGridPos() + direction].hasObstacle)
                        {
                            if (!ListOfcells[actorInPosition[selectedPosition].GetComponent<Mover>().GetGridPos() + direction].isOccupied)
                            {
                                gridPosition = actorInPosition[selectedPosition].GetComponent<Mover>().GetGridPos() + direction;
                            }
                        }
                    }
                }
            }

            xPosition = gridPosition.x;
            yPosition = gridPosition.y;

            uiSelector.SetNewTarget(ListOfPlayerCells[gridPosition].gameObject, true);
            pathMaker.DrawPath(ListOfPlayerCells[actorInPosition[highlightedActorPosition].GetComponent<Mover>().GetGridPos()], ListOfPlayerCells[gridPosition]);
        }

        private void CreateActors()
        {
            int calculateBench = actorOnBench.Count;
            int calculateParty = actorInPosition.Count;

            //print("calculate party: " + calculateParty);

            if (actorInPosition[0] != null)
            {
                actor[0] = Instantiate(actorInPosition[0]);
                actorInPosition[0] = actor[0];
                actor[0].transform.position = ListOfPlayerCells[new Vector2Int(2, 0)].transform.position;
                actorCoolDownInPosition[0].cooldown = actorInPosition[0].GetComponent<CoolDown>();
                actorCoolDownInPosition[0].GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(actorInPosition[0].transform.position);
                actorChannelInPosition[0].channel = actorInPosition[0].GetComponent<Channel>();
                actorChannelInPosition[0].GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(actorInPosition[0].transform.position);

                //print("added: " + actor[0]);
            }

            if (calculateParty < 2) return;

            if (actorInPosition[1] != null)
            {
                actor[1] = Instantiate(actorInPosition[1]);
                actorInPosition[1] = actor[1];
                actor[1].transform.position = ListOfPlayerCells[new Vector2Int(3, 0)].transform.position;
                //print("added: " + actor[1]);
            }

            if (calculateParty < 3) return;

            if (actorInPosition[2] != null)
            {
                actor[2] = Instantiate(actorInPosition[2]);
                actorInPosition[2] = actor[2];
                actor[2].transform.position = ListOfPlayerCells[new Vector2Int(4, 0)].transform.position;
                //print("added: " + actor[2]);
            }

            if (calculateBench < 1) return;

            if (actorOnBench[0] != null)
            {
                actor[3] = Instantiate(actorOnBench[0]);
                actorOnBench[0] = actor[3];
                actor[3].gameObject.SetActive(false);
            }

            if (calculateBench < 2) return;

            if (actorOnBench[1] != null)
            {
                actor[4] = Instantiate(actorOnBench[1]);
                actorOnBench[1] = actor[4];
                actor[4].gameObject.SetActive(false);
            }

            if (calculateBench < 3) return;

            if (actorOnBench[2] != null)
            {
                actor[5] = Instantiate(actorOnBench[2]);
                actorOnBench[2] = actor[5];
                actor[5].gameObject.SetActive(false);
            }

            if (calculateBench < 4) return;

            if (actorOnBench[3] != null)
            {
                actor[6] = Instantiate(actorOnBench[3]);
                actorOnBench[3] = actor[6];
                actor[6].gameObject.SetActive(false);
            }

            if (calculateBench < 5) return;

            if (actorOnBench[4] != null)
            {
                actor[7] = Instantiate(actorOnBench[4]);
                actorOnBench[4] = actor[7];
                actor[7].gameObject.SetActive(false);
            }

            if (calculateBench < 6) return;

            if (actorOnBench[5] != null)
            {
                actor[8] = Instantiate(actorOnBench[5]);
                actorOnBench[5] = actor[8];
                actor[8].gameObject.SetActive(false);
            }

            if (calculateBench < 7) return;

            if (actorOnBench[6] != null)
            {
                actor[9] = Instantiate(actorOnBench[6]);
                actorOnBench[6] = actor[9];
                actor[9].gameObject.SetActive(false);
            }

            if (calculateBench < 8) return;

            if (actorOnBench[7] != null)
            {
                actor[10] = Instantiate(actorOnBench[7]);
                actorOnBench[7] = actor[10];
                actor[10].gameObject.SetActive(false);
            }

            if (calculateBench < 9) return;

            if (actorOnBench[8] != null)
            {
                actor[11] = Instantiate(actorOnBench[8]);
                actorOnBench[8] = actor[11];
                actor[11].gameObject.SetActive(false);
            }

            if (calculateBench < 10) return;

            if (actorOnBench[9] != null)
            {
                actor[12] = Instantiate(actorOnBench[9]);
                actorOnBench[9] = actor[12];
                actor[12].gameObject.SetActive(false);
            }
        }

        public void AddBenchedActor(Fighter removedFighter)
        {
            if (actorOnBench.Count > 0)
            {
                actorInPosition.Add(actorOnBench[0]);
                actorOnBench.Remove(actorOnBench[0]);
                actorInPosition[2].gameObject.SetActive(true);
                actorInPosition[2].transform.position = removedFighter.transform.position;
            }
        }

        public void SwitchBenchedActor(int actorPosition, int benchPosition)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Fighter saveActor = actorInPosition[actorPosition];

                actorInPosition[actorPosition] = actorOnBench[benchPosition];
                actorOnBench[benchPosition] = saveActor;
                actorInPosition[actorPosition].gameObject.SetActive(true);
                actorInPosition[actorPosition].transform.position = saveActor.transform.position;
                saveActor.gameObject.SetActive(false);
                DestroyBenchBoxes();
                currentState = previousState;
            }
        }

        private void DestroySelectors()
        {

            for (int i = 0; i < 4; i++)
            {
                if (selectorInPosition[i] != null)
                {
                    selectorInPosition[i].DestroySelf();
                }
            }

            UIDotLineClear();
        }

        private void UIDotLineClear()
        {
            if (uiDotLine != null)
            {
                uiDotLine.ClearDrawing();
            }
        }

        private void GoBack()
        {
            if (currentState == State.animationFreeze) return;

            if (Input.GetKeyDown(KeyCode.S))
            {
                if (previousState != State.actorMove)
                {
                    RemoveDrawnPath();
                    
                }
                else
                {
                    DestroySelectors();
                }
                currentState = previousState;
                highlightedAttackPosition = 0;
                ResetSelectorHighlights();
            }
        }

        private void SpawnAttackBoxes()
        {
            if (selectorInPosition[0].GetComponent<Text>().text == " ")
            {
                for (int i = 0; i < 4; i++)
                {
                    if (selectorInPosition[i] != null)
                    {
                        selectorInPosition[i].ClearText();
                    }
                }

                for (int i = 0; i < selectorInPosition.Length; i++)
                {
                    if (selectorInPosition[i] != null)
                    {
                        Vector2 spacing = new Vector2(264f, 832f);
                        selectorInPosition[i].LinkToActor(actorInPosition[selectedPosition], true);
                        selectorInPosition[i].GetComponent<RectTransform>().anchoredPosition = spacing;
                        selectorInPosition[i].SlideIntoPosition(i);
                    }
                }

                UpdateUIDotLine(actorInPosition[selectedPosition].transform, selectorInPosition[1].GetComponent<RectTransform>());
            }
        }

        private void UpdateUIDotLine(Transform transform, RectTransform rectTransform)
        {
             uiDotLine.startingTransform = transform;
             uiDotLine.endingTransform = rectTransform;
             uiDotLine.DrawDotLine();
        }

        private void CheckDeathStatus()
        {
            if (actorInPosition[selectedPosition].GetComponent<PlayerStats>().isDead)
            {
                ResetState();
            }
        }

        #region Confirm

        private void ConfirmAlly(bool includeSelf, bool adjacentOnly, Fighter target)
        {
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left };

            if (actorInPosition[selectedPosition].GetComponent<Channel>().isChanneling) return;


            if (Input.GetKeyDown(KeyCode.A))
            {
                if (actorInPosition[selectedPosition] == target && !includeSelf)
                {
                    //print("cannot select self!");
                    return;
                }

                if (adjacentOnly)
                {
                    bool goodToGo = false;

                    foreach (Vector2Int direction in directions)
                    {
                        if (target.GetComponent<Mover>().GetGridPos() == actorInPosition[selectedPosition].GetComponent<Mover>().GetGridPos() + direction)
                        {
                            goodToGo = true;
                        }
                    }

                    if (goodToGo == false)
                    {
                        //print("ally not adjacent!");
                        return;
                    }
                }

                selectedTargets.Add(target.GetComponent<AttackReceiver>());

                ArcRenderer arcRenderer = FindObjectOfType<ArcRenderer>();
                arcRenderer.ClearLineRenderers();

                ResetState();
                actorInPosition[selectedPosition].StartChannel(selectedTargets, highlightedAttackPosition);
                ResetTargets();

                GetComponent<ActorAttackIndicator>().ResetIndicators();
                ResetHighlightEnemies();

            }
        }

        private void ConfirmAllAllies()
        {
            if (actorInPosition[selectedPosition].GetComponent<Channel>().isChanneling) return;

            Fighter[] allies = FindObjectsOfType<Fighter>();
            ArcRenderer arcRenderer = FindObjectOfType<ArcRenderer>();

            if (Input.GetKeyDown(KeyCode.A))
            {

                foreach (Fighter ally in allies)
                {
                    selectedTargets.Add(ally.GetComponent<AttackReceiver>());

                }

                ResetState();
                actorInPosition[selectedPosition].StartChannel(selectedTargets, highlightedAttackPosition);

                ResetTargets();
                arcRenderer.ClearLineRenderers();
            }
        }

        private void ConfirmEnemy(AttackReceiver target)
        {
            if (actorInPosition[selectedPosition].GetComponent<Channel>().isChanneling) return;

            if (Input.GetKeyDown(KeyCode.A))
            {
                ResetHighlightEnemies();

                selectedTargets.Add(target);

                ResetState();

                actorInPosition[selectedPosition].StartChannel(selectedTargets, highlightedAttackPosition);

                ResetTargets();

                GetComponent<ActorAttackIndicator>().ResetIndicators();

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

                ResetState();

                actorInPosition[selectedPosition].StartChannel(selectedTargets, highlightedAttackPosition);

                ResetTargets();
            }
        }

        private void ConfirmTargetCell()
        {
            if (actorInPosition[selectedPosition].GetComponent<Channel>().isChanneling) return;

            if (Input.GetKeyDown(KeyCode.A))
            {
                if (actorInPosition[highlightedActorPosition].GetComponent<TargetingManager>().CellTargeting(ListOfPlayerCells[gridPosition]))
                {
                    GetComponent<ActorAttackIndicator>().ResetIndicators();

                    ResetState();

                    actorInPosition[selectedPosition].PerformAttack(ListOfPlayerCells[gridPosition], highlightedAttackPosition);

                    ResetTargets();
                }
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
                                highlightedAttackPosition = 0;
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

                                    mover.Pathfind(cell);
                                    pathMaker.DrawPath(ListOfPlayerCells[gridPosition], ListOfPlayerCells[gridPosition]);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Highlight

        private void HighlightTargetEnemy()
        {
            //print("deprecated method.");
        }

        private void HighlightAllEnemies()
        {
            //print("deprecated method.");
        }

        private void HighlightAllAllies()
        {
            Fighter[] allies = FindObjectsOfType<Fighter>();

            foreach (Fighter ally in allies)
            {
                //print("all allies selected");
            }
        }

        private void HighlightTargetAlly()
        {

            Fighter[] allies = FindObjectsOfType<Fighter>();

            foreach (Fighter ally in allies)
            {
                if (ally == actorInPosition[highlightedAllyPosition])
                {
                    //print(actorInPosition[highlightedAllyPosition] + "selected");
                }
            }
        }

        private void HighlightCell(bool doInvalid)
        {
            Cell[] cells = FindObjectsOfType<Cell>();
            foreach (Cell cell in cells)
            {
                if (cell.GetGridPos() == gridPosition)
                {
                    //cell.GetComponent<TTW.Graphics.Outline>().enabled = true;


                    if (cell.GetGridPos() == actorInPosition[selectedPosition].GetComponent<Mover>().GetGridPos())
                    {
                        if (uiSelector.currentState != UIState.confirmMovement)
                        {
                            uiSelector.SetState(UIState.confirmMovement);
                            uiSelector.SetNewTarget(cell.gameObject, false);
                        }
                        continue;
                    }

                    if (cell.isOccupied)
                    {
                        if (uiSelector.currentState != UIState.selectInvalid)
                        {
                            uiSelector.SetState(UIState.selectInvalid);
                            uiSelector.SetNewTarget(cell.gameObject, false);
                            //print("occupied");
                        }
                        continue;
                    }


                    if (doInvalid)
                    {
                        if (uiSelector.currentState != UIState.selectInvalid)
                        {
                            uiSelector.SetState(UIState.selectInvalid);
                            uiSelector.SetNewTarget(cell.gameObject, false);
                            //print("no path");
                        }
                        continue;
                    }

                    if (uiSelector.currentState != UIState.selectMovement)
                    {
                        uiSelector.SetState(UIState.selectMovement);
                        uiSelector.SetNewTarget(cell.gameObject, false);
                    }
                }
                else
                {
                    //cell.DoDefaultColor();
                    cell.DestroyIcons();
                }
            }
        }

        private void HighlightActorSelectors()
        {
            if (highlightedActorPosition < actorInPosition.Count)
            {
                uiSelector.SetNewTarget(actorInPosition[highlightedActorPosition].gameObject, false);
            }
        }

        #endregion

        #region Select

        private void SelectTargetEnemyRanged()
        {
            EnemyTarget[] totalEnemies = FindObjectsOfType<EnemyTarget>();
            Vector3 arcPeakHeight = new Vector3(0, 20f, 0);
            Vector3 actorHeightOffset = new Vector3(0, 5f, 0);

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (highlightedEnemyPosition > 0)
                {
                    highlightedEnemyPosition--;
                }
                else
                {
                    highlightedEnemyPosition = totalEnemies.Length - 1;
                }

                uiSelector.SetNewTarget(enemyInPosition[highlightedEnemyPosition].gameObject, true);

                ArcRenderer arcRenderer = FindObjectOfType<ArcRenderer>();
                arcRenderer.DrawQuadraticCurve
                    (actorInPosition[selectedPosition].transform.position + actorHeightOffset,
                    ((actorInPosition[selectedPosition].transform.position + enemyInPosition[highlightedEnemyPosition].transform.position) / 2) + arcPeakHeight,
                    enemyInPosition[highlightedEnemyPosition].transform.position,
                    0);

            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (highlightedEnemyPosition < totalEnemies.Length - 1)
                {
                    highlightedEnemyPosition++;
                }
                else
                {
                    highlightedEnemyPosition = 0;
                }

                uiSelector.SetNewTarget(enemyInPosition[highlightedEnemyPosition].gameObject, true);


                ArcRenderer arcRenderer = FindObjectOfType<ArcRenderer>();
                arcRenderer.DrawQuadraticCurve
                    (actorInPosition[selectedPosition].transform.position + actorHeightOffset,
                    ((actorInPosition[selectedPosition].transform.position + enemyInPosition[highlightedEnemyPosition].transform.position) / 2) + arcPeakHeight,
                    enemyInPosition[highlightedEnemyPosition].transform.position,
                    0);
            }
        }

        private void SelectTargetEnemyMelee(List<int> targetablePositions)
        {

            if (uiSelector.currentTarget == actorInPosition[selectedPosition])
            {
                uiSelector.SetState(UIState.selectMelee);
                uiSelector.SetNewTarget(enemyInPosition[targetablePositions[targetableEnemyInPosition]].gameObject, true);
            }


            if (targetablePositions.Count < 1)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (targetableEnemyInPosition > 0)
                {
                    targetableEnemyInPosition--;

                }
                else
                {
                    targetableEnemyInPosition = targetablePositions.Count - 1;
                }

                uiSelector.SetState(UIState.selectMelee);
                uiSelector.SetNewTarget(enemyInPosition[targetablePositions[targetableEnemyInPosition]].gameObject, true);

            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (targetableEnemyInPosition < targetablePositions.Count - 1)
                {
                    targetableEnemyInPosition++;
                }
                else
                {
                    targetableEnemyInPosition = 0;
                }

                uiSelector.SetState(UIState.selectMelee);
                uiSelector.SetNewTarget(enemyInPosition[targetablePositions[targetableEnemyInPosition]].gameObject, true);

            }
        }

        private void SelectTargetCell()
        {
            MoveThroughGridTargetCell();
            //HighlightCell();
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
                }
                else
                {
                    highlightedAllyPosition = allies.Length - 1;
                }

                uiSelector.SetNewTarget(actorInPosition[highlightedAllyPosition].gameObject, false);

                RenderTargetLinesSingle(actorInPosition[selectedPosition].transform.position, 5f, actorInPosition[highlightedAllyPosition].transform.position, Color.magenta);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (highlightedAllyPosition < allies.Length - 1)
                {
                    highlightedAllyPosition++;
                }
                else
                {
                    highlightedAllyPosition = 0;
                }

                uiSelector.SetNewTarget(actorInPosition[highlightedAllyPosition].gameObject, false);

                RenderTargetLinesSingle(actorInPosition[selectedPosition].transform.position, 5f, actorInPosition[highlightedAllyPosition].transform.position, Color.magenta);
            }
        }

        private void SelectTargetCardinalAlly()
        {
            Fighter[] allies = FindObjectsOfType<Fighter>();

            List<Fighter> cardinalAllies = new List<Fighter>();

            foreach (Fighter ally in allies)
            {
                if (ally == actorInPosition[selectedPosition])
                {
                    continue;
                }

                if (ally.GetComponent<Mover>().GetGridPos().x == actorInPosition[selectedPosition].GetComponent<Mover>().GetGridPos().x ||
                        ally.GetComponent<Mover>().GetGridPos().y == actorInPosition[selectedPosition].GetComponent<Mover>().GetGridPos().y)
                {
                    cardinalAllies.Add(ally);
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (cardinalAllies.Count == 0)
                {
                    return;
                }

                if (highlightedAllyPosition > 0)
                {

                    highlightedAllyPosition--;
                }
                else
                {
                    highlightedAllyPosition = allies.Length - 1;
                }


                while (!cardinalAllies.Contains(actorInPosition[highlightedAllyPosition]))
                {

                    if (highlightedAllyPosition > 0)
                    {
                        
                        highlightedAllyPosition--;
                    }
                    else
                    {
                        highlightedAllyPosition = allies.Length - 1;
                    }
                }

                uiSelector.SetNewTarget(actorInPosition[highlightedAllyPosition].gameObject, false);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (cardinalAllies.Count == 0)
                {
                    return;
                }

                if (highlightedAllyPosition < allies.Length - 1)
                {
                    highlightedAllyPosition++;
                }
                else
                {
                    highlightedAllyPosition = 0;
                }

                while (!cardinalAllies.Contains(actorInPosition[highlightedAllyPosition]))
                {

                    if (highlightedAllyPosition < allies.Length - 1)
                    {
                        highlightedAllyPosition++;
                    }
                    else
                    {
                        highlightedAllyPosition = 0;
                    }
                }

                uiSelector.SetNewTarget(actorInPosition[highlightedAllyPosition].gameObject, false);
            }
        }

        private void SelectSelf()
        {
            Fighter[] allies = FindObjectsOfType<Fighter>();

            for (var i = 0; i < allies.Length; i++)
            {
                if (actorInPosition[i] == actorInPosition[highlightedActorPosition])
                {
                    highlightedAllyPosition = i;
                    //print("I am " + actorInPosition[highlightedActorPosition]);
                }
            }
        }

        private void SelectAttack()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                ResetSelectorHighlights();

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

        private void SelectActor()
        {

            if (actorInPosition[highlightedActorPosition].GetComponent<CoolDown>().isOnCD == false
                &&
                actorInPosition[highlightedActorPosition].GetComponent<PlayerStats>().isDead == false
                &&
                actorInPosition[highlightedActorPosition].GetComponent<Channel>().isChanneling == false)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    //print("test selcet actor");

                    selectedPosition = highlightedActorPosition;

                    if (highlightedActorPosition < actorInPosition.Count)
                    {
                        currentState = State.actorMove;
                        DestroySelectors();

                        //used to smooth out animations
                        if (actorInPosition[selectedPosition].GetComponent<Mover>().GetGridPos() == ListOfPlayerCells[gridPosition].GetGridPos())
                        {
                            uiSelector.SetState(UIState.confirmMovement);
                            uiSelector.SetNewTarget(actorInPosition[selectedPosition].gameObject, true);
                        }
                    }
                    else
                    {
                        currentState = State.swapBench;
                    }

                }
            }

            if (highlightedActorPosition >= actorInPosition.Count) return;

            if (actorInPosition[highlightedActorPosition].GetComponent<PlayerStats>().isDead == true)
            {
                RemoveActor();
            }
        }

        #endregion

        #region MoveThrough

        private void MoveThroughAttackSelect()
        {

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ResetSelectorHighlights();
                if (highlightedAttackPosition > 0)
                {
                    highlightedAttackPosition--;
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ResetSelectorHighlights();
                if (highlightedAttackPosition < selectorInPosition.Length - 1)
                {
                    highlightedAttackPosition++;
                }
            }

            UpdateAttackUI();
        }

        private void UpdateAttackUI()
        {
            for (int i = 0; i < 4; i++)
            {
                if (highlightedAttackPosition == i)
                {
                    selectorInPosition[i].Highlight();
                }
            }
        }

        private void MoveThroughActorSelect()
        {
            
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                CalculateAvailableActors();
                ActorSelectionUpArrow();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                CalculateAvailableActors();
                ActorSelectionDownArrow();
            }
        }

        private void CalculateAvailableActors()
        {
            availableFighters.Clear();

            for (int i = 0; i < actorInPosition.Count; i++)
            {
                if (!CheckHighlightedActorForCoolDown(i))
                {
                    if (!CheckHighlightedActorForChanneling(i))
                    {

                        if (!CheckHighlightedActorForAnimating(i))
                        {
                            availableFighters.Add(i);
                        }
                    }
                }
            }
        }

        private bool CheckHighlightedActorForChanneling(int highlightedActor)
        {
            if (actorInPosition[highlightedActor].GetComponent<StateMachine>().state != Combat.State.channeling)
            {
                //SpawnActorBoxes();
                return false;
            }
            else
            {
                //DestroySelectors();
                return true;
            }
        }

        private bool CheckHighlightedActorForAnimating(int highlightedActor)
        {
            if (actorInPosition[highlightedActor].GetComponent<StateMachine>().state != Combat.State.animating)
            {
                //SpawnActorBoxes();
                return false;
            }
            else
            {
                //DestroySelectors();
                return true;
            }
        }

        private void ActorSelectionDownArrow()
        {
            ResetSelectorHighlights();

            if (availableFighters.Count == 0) return;

            if (highlightedActorPosition + 1 < (actorInPosition.Count))
            {
                highlightedActorPosition++;
            }
            else
            {
                highlightedActorPosition = 0;
            }

            while (!availableFighters.Contains(highlightedActorPosition))
            {
                if (highlightedActorPosition + 1 < (actorInPosition.Count))
                {
                    highlightedActorPosition++;
                }
                else
                {
                    highlightedActorPosition = 0;
                }
            }
            
            UpdateActorUI();
        }

        private void ActorSelectionUpArrow()
        {
            ResetSelectorHighlights();

            if (availableFighters.Count == 0) return;

            if (highlightedActorPosition > 0)
            {
                highlightedActorPosition--;
            }
            else
            {
                highlightedActorPosition = actorInPosition.Count - 1;
            }

            while (!availableFighters.Contains(highlightedActorPosition))
            {
                if (highlightedActorPosition > 0)
                {
                    highlightedActorPosition--;
                }
                else
                {
                    highlightedActorPosition = actorInPosition.Count - 1;
                }
            }
                
            UpdateActorUI();
        }

        private void ResetSelectorHighlights()
        {
            for (int i = 0; i < 4; i++)
            {
                if (selectorInPosition[i] != null)
                {
                    selectorInPosition[i].ResetHighlight();
                }
            }
        }

        private void PassToNextActor()
        {
            print("passing to next actor");

            CalculateAvailableActors();

            print("available Fighters: " + availableFighters.Count);

            if (availableFighters.Count == 0) return;

            while (!availableFighters.Contains(highlightedActorPosition))
            {
                if (highlightedActorPosition <= actorInPosition.Count)
                {
                    highlightedActorPosition++;
                }
                else
                {
                    highlightedActorPosition = 0;
                }
            }
        }

        public void UpdateActorUI()
        {
            ResetSelectorHighlights();

            for (int i = 0; i < 3; i++)
            {
                if (selectorInPosition[i] != null)
                {
                    if (highlightedActorPosition == i)
                    {
                        selectorInPosition[i].Highlight();
                        uiDotLine.startingTransform = actorInPosition[i].transform;
                        uiDotLine.DrawDotLine();
                    }

                    if (actorInPosition[i].GetComponent<StateMachine>().state == Combat.State.cooldown || actorInPosition[i].GetComponent<StateMachine>().state == Combat.State.channeling)
                    {
                        selectorInPosition[i].GreyText();
                    }

                }
            }

            HighlightActorSelectors();
        }

        private bool CheckHighlightedActorForCoolDown(int highlightedActor)
        {
            if (actorInPosition[highlightedActor].GetComponent<StateMachine>().state != Combat.State.cooldown)
            {
                //SpawnActorBoxes();
                return false;
            }
            else
            {
                //DestroySelectors();
                return true;
            }
        }

        private void MoveThroughActiveActorSelect()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (highlightedActiveActorPosition > 0)
                {
                    highlightedActiveActorPosition--;
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (highlightedActiveActorPosition + 1 < (actorInPosition.Count))
                {
                    highlightedActiveActorPosition++;
                }
            }
        }

        private void MoveThroughGrid()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && xPosition > 1)
            {
                xPosition--;
                UpdatePosition();
                bool unbrokenPath = pathMaker.CheckPath(ListOfPlayerCells[actorInPosition[highlightedActorPosition].GetComponent<Mover>().GetGridPos()], ListOfPlayerCells[gridPosition]);
                pathMaker.ClearPath();
                pathMaker.DrawPath(ListOfPlayerCells[actorInPosition[highlightedActorPosition].GetComponent<Mover>().GetGridPos()], ListOfPlayerCells[gridPosition]);
                HighlightCell(unbrokenPath);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) && xPosition < 4)
            {
                xPosition++;
                UpdatePosition();
                bool unbrokenPath = pathMaker.CheckPath(ListOfPlayerCells[actorInPosition[highlightedActorPosition].GetComponent<Mover>().GetGridPos()], ListOfPlayerCells[gridPosition]);
                pathMaker.ClearPath();
                pathMaker.DrawPath(ListOfPlayerCells[actorInPosition[highlightedActorPosition].GetComponent<Mover>().GetGridPos()], ListOfPlayerCells[gridPosition]);
                HighlightCell(unbrokenPath);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && yPosition < 2)
            {
                yPosition++;
                UpdatePosition();
                bool unbrokenPath = pathMaker.CheckPath(ListOfPlayerCells[actorInPosition[highlightedActorPosition].GetComponent<Mover>().GetGridPos()], ListOfPlayerCells[gridPosition]);
                pathMaker.ClearPath();
                pathMaker.DrawPath(ListOfPlayerCells[actorInPosition[highlightedActorPosition].GetComponent<Mover>().GetGridPos()], ListOfPlayerCells[gridPosition]);
                HighlightCell(unbrokenPath);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) && yPosition > 0)
            {
                // REMOVED BENCH OPENING FOR NOW
                // UNCOMMENT WHEN READY TO REIMPLEMENT

                //if (gridPosition.y == 0)
                //{
                //    OpenBench();
                //    ResetCellHighlights();
                //    RemoveDrawnPath();
                //    return;
                //}

                yPosition--;
                UpdatePosition();
                bool unbrokenPath = pathMaker.CheckPath(ListOfPlayerCells[actorInPosition[highlightedActorPosition].GetComponent<Mover>().GetGridPos()], ListOfPlayerCells[gridPosition]);
                pathMaker.ClearPath();
                pathMaker.DrawPath(ListOfPlayerCells[actorInPosition[highlightedActorPosition].GetComponent<Mover>().GetGridPos()], ListOfPlayerCells[gridPosition]);
                HighlightCell(unbrokenPath);
            }
        }

        private void RemoveDrawnPath()
        {
            pathMaker.DrawPath(ListOfPlayerCells[gridPosition], ListOfPlayerCells[gridPosition]);
        }

        private void OpenBench()
        {
            print("opening bench");
            currentState = State.swapBenchSelect;
            Bench bench = FindObjectOfType<Bench>();
            bench.GetComponent<MeshRenderer>().enabled = true;
        }

        private void MoveThroughGridTargetCell()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && xPosition > 1)
            {
                GetComponent<ActorAttackIndicator>().ResetIndicators();
                xPosition--;
                UpdatePosition();
                GetComponent<ActorAttackIndicator>().IndicateCell(ListOfPlayerCells[gridPosition]);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) && xPosition < 4)
            {
                GetComponent<ActorAttackIndicator>().ResetIndicators();
                xPosition++;
                UpdatePosition();
                GetComponent<ActorAttackIndicator>().IndicateCell(ListOfPlayerCells[gridPosition]);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && yPosition < 2)
            {
                GetComponent<ActorAttackIndicator>().ResetIndicators();
                yPosition++;
                UpdatePosition();
                GetComponent<ActorAttackIndicator>().IndicateCell(ListOfPlayerCells[gridPosition]);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) && yPosition > 0)
            {
                GetComponent<ActorAttackIndicator>().ResetIndicators();
                yPosition--;
                UpdatePosition();
                GetComponent<ActorAttackIndicator>().IndicateCell(ListOfPlayerCells[gridPosition]);
            }
        }

        #endregion

        #region Reset

        public void ResetState()
        {
            //print("resetting state");
            currentState = State.actorSelect;
            savedState = currentState;
        }

        public void ResetTargets()
        {
            selectedTargets.Clear();
            PassToNextActor();
        }

        public void ResetHighlighters()
        {
            selectedPosition = 0;
            highlightedActorPosition = 0;
            highlightedAllyPosition = 0;
            highlightedAttackPosition = 0;
            highlightedEnemyPosition = 0;
        }

        private void ResetHighlightEnemies()
        {
            //print("resetting enemy highlights");
            ArcRenderer arcRenderer = FindObjectOfType<ArcRenderer>();
            arcRenderer.ClearLineRenderers();

        }

        private void ResetActorSelection()
        {
            foreach (Fighter fighter in actorInPosition)
            {
                fighter.GetComponent<UIActorSelector>().NotSelected();
            }
        }

        private void ResetCellHighlights()
        {
            Cell[] cells = FindObjectsOfType<Cell>();
            foreach (Cell cell in cells)
            {
                //cell.DoDefaultColor();
            }
        }

        #endregion

        private void CheckGridStatus()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //print("state: " + currentState);
                //print("previous state: " + previousState);
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

        private bool CheckIfParalyzed(StatusEffectManager statusEffectManager)
        {
            if (statusEffectManager.statusEffectBad == StatusEffect.paralyze)
            {
                return true;
            }

            return false;
        }

        private void RemoveActor()
        {
            //print("remove character from battle? Press A to confirm");

            if (Input.GetKeyDown(KeyCode.A))
            {
                actorInPosition[highlightedActorPosition].Remove();
            }
        }

        private void UpdatePosition()
        {
            gridPosition = new Vector2Int(xPosition, yPosition);
            uiSelector.SetNewTarget(ListOfPlayerCells[gridPosition].gameObject, false);
            //ListOfcells[gridPosition].GetComponent<TTW.Graphics.Outline>().enabled = true;
        }

        private void ShowActorSelection(Fighter fighter)
        {
            fighter.GetComponent<UIActorSelector>().Selected();
        }

        public void AnimationFreeze()
        {
            if (currentState != State.animationFreeze)
            {
                savedState = currentState;
            }
            currentState = State.animationFreeze;
            //print("freezing animation");
        }

        public void AnimationFreezeEnd()
        {
            currentState = savedState;
            if (currentState == State.actorMove)
            {
                HighlightCell(false);
                
            }
            if (currentState == State.attackTargetSelect)
            {
                uiSelector.SetState(UIState.selectAttack);
            }
            if (currentState == State.actorSelect)
            {
                uiDotLine.DrawDotLine();
                UpdateActorUI();
            }


            if (currentState == State.actorAttackSelect)
            {
                uiDotLine.DrawDotLine();
                UpdateAttackUI();
            }
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

        private void CardinalAllySelect()
        {
            currentState = State.attackTargetSelect;
            actorTargetingType = ActorTargetingType.cardinalAlly;
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

        public bool IsEndAnimationReady()
        {
            Mover[] movers = FindObjectsOfType<Mover>(); //for pushClear
            OnRailsMover[] onRailsMovers = FindObjectsOfType<OnRailsMover>(); //for railsClear
            AnimationHandler[] animationHandlers = FindObjectsOfType<AnimationHandler>(); //for animClear

            foreach (Mover mover in movers)
            {
                if (mover.isBeingPushed)
                {
                    //print("something is being pushed");
                    return false;

                }
            }

            foreach (OnRailsMover onRailsMover in onRailsMovers)
            {
                if (onRailsMover.isMoving)
                {
                    //print("something is being railed");
                    return false;
                }
            }

            foreach (AnimationHandler animationHandler in animationHandlers)
            {
                if (animationHandler.isAnimating)
                {
                    //print("something is being animated");
                    return false;
                }
            }

            //print("ending animation freeze");
            return true;
        }
    }
}
