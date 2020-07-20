using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public enum State
    {
        neutral,
        channeling,
        animating,
        cooldown
    }

    public class StateMachine : MonoBehaviour
    {
        Channel channel;

        public State state;
        State savedState;

        // Start is called before the first frame update
        void Start()
        {
            channel = GetComponent<Channel>();
            savedState = state;
        }

        public void StartChanneling(float channelTime)
        {
            state = State.channeling;
            channel.StartChannel(channelTime);
        }

        public void Neutral()
        {
            state = State.neutral;
        }

        public void Animate()
        {
            state = State.animating;
        }
    }
}


