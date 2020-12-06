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
        cooldown,
        paused
    }

    public class StateMachine : MonoBehaviour
    {
        Channel channel;
        CombatController combatController;

        public State state;
        State savedState;

        // Start is called before the first frame update
        void Start()
        {
            combatController = FindObjectOfType<CombatController>();
            channel = GetComponent<Channel>();
            savedState = state;
        }

        public void Channel()
        {
            state = State.channeling;
        }

        public void Neutral()
        {
            state = State.neutral;
            if (this.GetComponent<Fighter>() != null)
            {
                if (combatController.currentState == CombatController.State.actorSelect)
                {
                    combatController.UpdateActorUI();
                }
            }
        }

        public void Animate()
        {
            state = State.animating;
        }

        public void Pausing()
        {
            state = State.paused;
        }

        public void Cooldown()
        {
            state = State.cooldown;
            if (this.GetComponent<Fighter>() != null)
            {
                if (combatController.currentState == CombatController.State.actorSelect)
                {
                    combatController.UpdateActorUI();
                }
            }
        }
    }
}


