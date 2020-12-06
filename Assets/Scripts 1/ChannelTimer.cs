using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TTW.Combat
{
    public class ChannelTimer : MonoBehaviour
    {
        public Channel channel;
        Image image;

        RectTransform rectTransform;

        public bool isMoving = false;
        public bool ready = false;
        bool animationOverride = false;
        public float fillRatio = 0f;
        Vector3 offset;



        void Start()
        {
            image = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
            offset = new Vector3(0f, 15f, 0f);

            GameEvents.current.onAnimationStart += PauseForAnimation;
            GameEvents.current.onAnimationEnd += AnimationEnd;
        }

        void Update()
        {
            fillRatio = ((channel.channel / channel.maxChannel) - 1f) * -1f;
            image.fillAmount = fillRatio;

            ChannelInactive();
        }

        private void ChannelInactive()
        {
            if (fillRatio >= 1f)
            {
                image.enabled = false;
            }
            else
            {
                if (animationOverride == true) return;

                image.enabled = true;
                rectTransform.position = Camera.main.WorldToScreenPoint(channel.transform.position + offset);
            }
        }

        private void PauseForAnimation()
        {
            image.enabled = false;
            animationOverride = true;
        }

        private void AnimationEnd()
        {
            animationOverride = false;
        }
    }
}