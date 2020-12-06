using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class Channel : MonoBehaviour
    {
        public float channel = 0f;
        public float maxChannel = 1000f;
        public bool isChanneling = false;
        [Range(0f,2f)][SerializeField] float speedCoefficient = 0.1f;
        public float speedRatio;

        bool returnActorValues = false;

        CombatController combatController;
        StateMachine stateMachine;

        //for use with fighter channeling attacks
        [SerializeField] List<AttackReceiver> savedActorTargets = null;
        Ability savedAbility;
        Fighter fighter;

        private void Start()
        {
            combatController = FindObjectOfType<CombatController>();
            stateMachine = GetComponent<StateMachine>();

            if (GetComponent<Fighter>() != null)
            {
                fighter = GetComponent<Fighter>();
                savedAbility = null;
            }
        }

        private void Update()
        {

            CheckChannel();
            speedRatio = CalculateSpeed();
        }

        private float CalculateSpeed()
        {
            return GetComponent<PlayerStats>().speed * speedCoefficient;
        }
         
        public void CheckChannel()
        {
            if (channel > 0)
            {
                if (combatController.currentState == CombatController.State.animationFreeze)
                {
                    //print("channel paused for animation");
                    return;
                }

                isChanneling = true;
                channel -= Time.deltaTime * speedRatio;
            }
            if (channel <= 0)
            {
                isChanneling = false;

                if (returnActorValues)
                {
                    returnActorValues = false;

                    fighter.PerformAttack(savedActorTargets, savedAbility);
                }
            }
        }

        public void StartChannel(float channelTime)
        {
            maxChannel = channelTime;
            channel = channelTime;
            stateMachine.Channel();
        }

        public void StartChannelActor(List<AttackReceiver> targets, Ability ability)
        {
            savedActorTargets.Clear();
            stateMachine.Channel();
            maxChannel = ability.attackChannelTime;
            channel = ability.attackChannelTime;
            savedAbility = ability;
            returnActorValues = true;

            foreach (AttackReceiver receiver in targets)
            {
                savedActorTargets.Add(receiver);
            }
        }

        public void BreakChannel()
        {
            channel = 0f;
            returnActorValues = false;
            isChanneling = false;
            stateMachine.Neutral();
        }
    }
}
