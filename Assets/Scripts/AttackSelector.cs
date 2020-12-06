using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TTW.Combat;

namespace TTW.UI
{
    public class AttackSelector : MonoBehaviour

    {
        [SerializeField] int attackPosition = 1;
        [SerializeField] Sprite highlightedSprite;
        [SerializeField] Sprite lowlightedSprite;

        private Vector3 origin;
        CombatController combatController;
        Text text;
        bool destroySelf = false;
        float destructionCountDown = 0.15f;

        private void Awake()
        {
            origin = transform.position;
            combatController = FindObjectOfType<CombatController>();
            text = GetComponentInChildren<Text>();
        }

        private void Update()
        {
            if (combatController.currentState == CombatController.State.actorAttackSelect)
            {
                if (combatController.highlightedAttackPosition == attackPosition)
                {
                    Highlight();
                }
                else
                {
                    StopHighlight();
                }
            }
            if (destroySelf)
            {
                if (destructionCountDown > 0)
                {
                    destructionCountDown -= Time.deltaTime;
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
        }

        private void Highlight()
        {
            GetComponent<SpriteRenderer>().sprite = highlightedSprite;
        }

        private void StopHighlight()
        {
            GetComponent<SpriteRenderer>().sprite = lowlightedSprite;
        }

        public void SlideIntoPosition(int position)
        {
            origin = transform.position;

            if(position == 1)
            {
                LeanTween.move(gameObject, new Vector3(origin.x - 5f, origin.y + 5f, origin.z), 0.25f).setEaseInOutSine();
            }
            if (position == 2)
            {
                LeanTween.move(gameObject, new Vector3(origin.x - 1.75f, origin.y + 7f, origin.z), 0.25f).setEaseInOutSine();
            }
            if (position == 3)
            {
                LeanTween.move(gameObject, new Vector3(origin.x + 1.75f, origin.y + 7f, origin.z), 0.25f).setEaseInOutSine();
            }
            if (position == 4)
            {
                LeanTween.move(gameObject, new Vector3(origin.x + 5f, origin.y + 5f, origin.z), 0.25f).setEaseInOutSine();
            }

        }

        public int ChangeActor()
        {
            //print("state is now actorMove");
            return attackPosition;
        }

        internal void DestroySelf()
        {
            destroySelf = true;
            LeanTween.move(gameObject, origin, 0.15f).setEaseInOutSine();
        }
    }
}
