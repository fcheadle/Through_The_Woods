using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TTW.Combat
{
    public class CoolDownTimer : MonoBehaviour
    {
        [SerializeField] float speed = 500f;

        public CoolDown cooldown;
        Image image;
        RectTransform rectTransform;
        Animator animator;
        State state;

        public bool isMoving = false;
        bool freeze = false;
        bool play = false;
        public bool ready = false;
        bool channel = false;
        bool cd = false;
        bool animationOverride = false;
        public float fillRatio = 0f;
        Vector3 offset;


        void Start()
        {
            image = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
            animator = GetComponent<Animator>();
            state = cooldown.state;
            offset = new Vector3(0f, 15f, 0f);

            GameEvents.current.onAnimationStart += PauseForAnimation;
            GameEvents.current.onAnimationEnd += AnimationEnd;
        }

        void Update()
        {
            fillRatio = ((cooldown.cd / cooldown.maxCD) - 1f) * -1f;
            image.fillAmount = fillRatio;

            CoolDownInactive();

            //if (cooldown.GetComponent<StateMachine>().state == State.animating)
            //{
            //    AnimationPlay();
            //}
            //else if (cooldown.GetComponent<StateMachine>().state == State.channeling)
            //{
            //    AnimationChannel();
            //}
            //else if (cooldown.GetComponent<StateMachine>().state == State.cooldown)
            //{
            //    AnimationOnCoolDown();
            //}
            //else if (cooldown.GetComponent<StateMachine>().state == State.neutral)
            //{
            //    print("returning to neutral");
            //    AnimationAttackReady();
            //}
            //else if (cooldown.GetComponent<StateMachine>().state == State.paused)
            //{
            //    AnimationPause();
            //}

        }

        private void CoolDownInactive()
        {
            if (fillRatio >= 1f)
            {
                image.enabled = false;
            }
            else
            {
                if (animationOverride == true) return;

                image.enabled = true;
                rectTransform.position = Camera.main.WorldToScreenPoint(cooldown.transform.position + offset);
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

        //public void MoveToPosition(Vector3 destination)
        //{
            

        //    var newSpeed = speed * Time.deltaTime;

        //    LeanTween.moveY(GetComponent<RectTransform>(), destination.y, 3f).setEaseInOutSine();

        //}

        //public void AnimationPause()
        //{
        //    if (freeze == false)
        //    {
        //        AnimationValuesReset();
        //        freeze = true;
        //        animator.SetBool("animationFreeze", freeze);
        //    }
        //}

        //public void AnimationPlay()
        //{
        //    if (play == false)
        //    {
        //        AnimationValuesReset();
        //        play = true;
        //        animator.SetBool("playAnimation", play);

        //    }
        //}

        //public void AnimationAttackReady()
        //{
        //    if (ready == false)
        //    {
        //        AnimationValuesReset();
        //        ready = true;
        //        animator.SetBool("coolDownComplete", ready);
        //    }
        //}

        //public void AnimationChannel()
        //{
        //    if (channel == false)
        //    {
        //        AnimationValuesReset();
        //        channel = true;
        //        animator.SetBool("channelStarted", channel);
        //    }
        //}

        //public void AnimationOnCoolDown()
        //{
        //    if (cd == false)
        //    {
        //        AnimationValuesReset();
        //        cd = true;
        //        animator.SetBool("coolDownStarted", cd);
        //    }
        //}

        //public void AnimationValuesReset()
        //{
        //    freeze = false;
        //    play = false;
        //    ready = false;
        //    channel = false;
        //    cd = false;
        //    animator.SetBool("coolDownStarted", cd);
        //    animator.SetBool("channelStarted", channel);
        //    animator.SetBool("coolDownComplete", ready);
        //    animator.SetBool("playAnimation", play);
        //    animator.SetBool("animationFreeze", freeze);
        //}
    }
}
