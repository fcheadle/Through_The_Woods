using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TTW.Combat
{
    public class UISelector : MonoBehaviour
    {
        [SerializeField] int abilitySlot;

        Fighter linkedActor;
        Ability linkedAbility;
        Text displayText;
        string savedText;
        private Vector2 origin;
        RectTransform rectTransform;

        void Awake()
        {
            displayText = GetComponent<Text>();
            linkedActor = null;
            linkedAbility = null;
            rectTransform = GetComponent<RectTransform>();
            displayText.color = Color.grey;
            savedText = " ";
            displayText.text = " ";
        }

        private void Start()
        {
            GameEvents.current.onAnimationStart += ClearText;
            GameEvents.current.onAnimationEnd += LoadText;
        }

        public void LinkToActor(Fighter actor, bool linkAbility)
        {
            linkedActor = actor;
            linkedAbility = actor.abilities[abilitySlot];

            if (linkAbility)
            {
                UpdateText(linkedAbility);
            }
            else
            {
                UpdateText(linkedActor);
            }
        }

        private void UpdateText(Ability ability)
        {
            displayText.text = ability.name;
        }

        private void UpdateText(Fighter fighter)
        {
            displayText.text = fighter.actor.name;
        }

        public void DestroySelf()
        {
            displayText.text = " ";
        }

        public void SlideIntoPosition(int position)
        {
            origin = rectTransform.anchoredPosition;

            if (position == 0)
            {
                LeanTween.move(rectTransform, new Vector2(264f, 832f), 0.5f).setEaseInOutSine();
            }
            if (position == 1)
            {
                LeanTween.move(rectTransform, new Vector2(264f, 800f), 0.5f).setEaseInOutSine();
            }
            if (position == 2)
            {
                LeanTween.move(rectTransform, new Vector2(264f, 768f), 0.5f).setEaseInOutSine();
            }
            if (position == 3)
            {
                LeanTween.move(rectTransform, new Vector2(264f, 736f), 0.5f).setEaseInOutSine();
            }
        }


        public void Highlight()
        {
            displayText.fontStyle = FontStyle.Bold;
            displayText.color = Color.cyan;
        }

        public void ResetHighlight()
        {
            displayText.fontStyle = FontStyle.Normal;
            displayText.color = Color.white;
        }

        public void GreyText()
        {
            displayText.fontStyle = FontStyle.Italic;
            displayText.color = Color.gray;
        }

        public void ClearText()
        {
            savedText = displayText.text;
            ResetHighlight();
            displayText.text = " ";
        }

        private void LoadText()
        {
            displayText.text = savedText;
            savedText = " ";
        }

        
    }
}