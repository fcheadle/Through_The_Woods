using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.UI
{

    public enum UIState
    {
        selectPlayer,
        selectMovement,
        confirmMovement,
        selectInvalid,
        selectAttack,
        selectMelee,
        selectRanged,
        selectAlly,
        selectCardinalAlly,
        selectCell,
        highlightChannel,
        highlightCD,
        swapBench
    }

    public class FollowActor : MonoBehaviour
    {
        public GameObject currentTarget;
        GameObject oldTarget;

        [SerializeField] Sprite selectPlayer;
        [SerializeField] Sprite selectMovement;
        [SerializeField] Sprite selectInvalid;
        [SerializeField] Sprite selectConfirm;
        [SerializeField] Sprite selectAttack;
        [SerializeField] Sprite selectEnemy;
        [SerializeField] Sprite selectAlly;
        [SerializeField] Sprite selectCell;

        public UIState currentState;
        public float currentY = 10;
        [SerializeField] float selectPlayerY;
        [SerializeField] float selectMovementY;
        [SerializeField] float selectInvalidY;
        [SerializeField] float selectConfirmY;
        [SerializeField] float selectAttackY;
        [SerializeField] float selectEnemyY;
        [SerializeField] float selectAllyY;
        [SerializeField] float selectCellY;



        // Start is called before the first frame update
        void Start()
        {
            currentTarget = gameObject;
            oldTarget = gameObject;

            GameEvents.current.onAnimationStart += DisabledForAnimation;
            GameEvents.current.onAnimationEnd += EnabledForAnimation;
        }

        // Update is called once per frame
        void Update()
        {
            FollowSelectedActor();
        }

        private void FollowSelectedActor()
        {

            if (currentTarget != oldTarget && currentTarget != null)
            {
                oldTarget = currentTarget;
                LeanTween.move(gameObject, new Vector3(oldTarget.transform.position.x, currentY, oldTarget.transform.position.z), 0.25f).setEaseInOutSine();
            }

            if (currentTarget == oldTarget && !LeanTween.isTweening(gameObject) && currentTarget != null)
            {
                transform.position = new Vector3(currentTarget.transform.position.x, currentY, currentTarget.transform.position.z);
            }
        }

        public void SetNewTarget(GameObject newTarget, bool doReset)
        {
            if (doReset)
            {
                oldTarget = null;
            }

            currentTarget = newTarget;
        }

        public void SetState(UIState state)
        {
            currentTarget = null;
            oldTarget = null;
            currentState = state;
            Color orange = new Color(1f, 0.6f, 0f);
            Color purple = new Color(0.6f, 0f, 1f);

            switch (state)
            {
                case UIState.selectPlayer:
                    SetColor(Color.white);
                    SetIcon(selectPlayer);
                    SetY(selectPlayerY);
                    //GetComponentInChildren<BobbingTween>().enabled = true;
                    break;
                case UIState.selectMovement:
                    //SetColor(Color.yellow);
                    SetIcon(selectMovement);
                    SetY(selectMovementY);
                    //GetComponentInChildren<BobbingTween>().enabled = false;
                    break;
                case UIState.confirmMovement:
                    //SetColor(Color.green);
                    SetIcon(selectConfirm);
                    SetY(selectConfirmY);
                    break;
                case UIState.selectInvalid:
                    //SetColor(Color.red);
                    SetIcon(selectInvalid);
                    SetY(selectInvalidY);
                    break;
                case UIState.selectAttack:
                    //SetColor(Color.cyan);
                    SetIcon(selectAttack);
                    SetY(selectAttackY);
                    break;
                case UIState.selectMelee:
                    //SetColor(orange);
                    SetIcon(selectEnemy);
                    SetY(selectAttackY);
                    break;
                case UIState.selectRanged:
                    //SetColor(orange);
                    SetIcon(selectEnemy);
                    SetY(selectAttackY);
                    break;
                case UIState.selectAlly:
                    //SetColor(Color.magenta);
                    SetIcon(selectPlayer);
                    SetY(selectAttackY);
                    break;
                case UIState.selectCardinalAlly:
                    //SetColor(Color.magenta);
                    SetIcon(selectPlayer);
                    SetY(selectAttackY);
                    break;
                case UIState.selectCell:
                    //SetColor(orange);
                    SetIcon(selectEnemy);
                    SetY(selectAttackY);
                    break;
                case UIState.highlightChannel:
                    print("highlighting channel");
                    SetColor(Color.magenta);
                    SetIcon(selectEnemy);
                    SetY(selectAttackY);
                    break;
                case UIState.swapBench:
                    //SetColor(Color.yellow);
                    SetIcon(selectEnemy);
                    SetY(selectAttackY);
                    break;
                default:
                    print("unknown UI State");
                    break;
            }
        }

        private void SetY(float newY)
        {
            currentY = newY;
        }

        public void SetColor(Color color)
        {
            if (color != GetComponentInChildren<SpriteRenderer>().color)
            {
                GetComponentInChildren<SpriteRenderer>().color = color;
                GetComponentInChildren<Light>().color = color;
            }
        }

        public void SetIcon(Sprite sprite)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = sprite;
        }

        public void EnabledForAnimation()
        {
            GetComponentInChildren<SpriteRenderer>().enabled = true;
            GetComponentInChildren<Light>().enabled = true;
        }

        public void DisabledForAnimation()
        {
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            GetComponentInChildren<Light>().enabled = false;
        }
    }
}