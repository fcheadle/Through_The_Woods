using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TTW.Combat;

namespace TTW.UI
{
    public class BenchSelector : MonoBehaviour

    {
        [SerializeField] int benchPosition = 1;
        [SerializeField] Sprite highlightedSprite;
        [SerializeField] Sprite lowlightedSprite;
        [SerializeField] Actor actor;

        private Vector3 origin;
        CombatController combatController;
        bool destroySelf = false;
        float destructionCountDown = 0.15f;

        private void Awake()
        {
            origin = transform.position;
            combatController = FindObjectOfType<CombatController>();
        }

        private void Update()
        {
            if (combatController.currentState == CombatController.State.swapBench)
            {
                if (combatController.highlightedBenchActorPosition == benchPosition)
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

            if (position == 1)
            {
                LeanTween.move(gameObject, new Vector3(origin.x - 5f, origin.y, origin.z), 0.25f).setEaseInOutSine();
            }
            if (position == 2)
            {
                LeanTween.move(gameObject, new Vector3(origin.x - 2f, origin.y, origin.z), 0.25f).setEaseInOutSine();
            }
            if (position == 3)
            {
                LeanTween.move(gameObject, new Vector3(origin.x + 1f, origin.y, origin.z), 0.25f).setEaseInOutSine();
            }
            if (position == 4)
            {
                LeanTween.move(gameObject, new Vector3(origin.x + 4f, origin.y, origin.z), 0.25f).setEaseInOutSine();
            }
            if (position == 5)
            {
                LeanTween.move(gameObject, new Vector3(origin.x + 7f, origin.y, origin.z), 0.25f).setEaseInOutSine();
            }
            if (position == 6)
            {
                LeanTween.move(gameObject, new Vector3(origin.x + 10f, origin.y, origin.z), 0.25f).setEaseInOutSine();
            }
            if (position == 7)
            {
                LeanTween.move(gameObject, new Vector3(origin.x + 13f, origin.y, origin.z), 0.25f).setEaseInOutSine();
            }
            if (position == 8)
            {
                LeanTween.move(gameObject, new Vector3(origin.x + 16f, origin.y, origin.z), 0.25f).setEaseInOutSine();
            }
            if (position == 9)
            {
                LeanTween.move(gameObject, new Vector3(origin.x + 19f, origin.y, origin.z), 0.25f).setEaseInOutSine();
            }
            if (position == 10)
            {
                LeanTween.move(gameObject, new Vector3(origin.x + 22f, origin.y, origin.z), 0.25f).setEaseInOutSine();
            }
        }

        internal void DestroySelf()
        {
            destroySelf = true;
            LeanTween.move(gameObject, origin, 0.15f).setEaseInOutSine();
        }
    }
}
