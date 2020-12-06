using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class StatHandler : MonoBehaviour
    {

        [SerializeField] int statInstances = 0;
        [SerializeField] float currentTimer = 0f;
        [SerializeField] float timerTotal = 20f;

        Queue<StatChanges> currentStatChanges = new Queue<StatChanges>();
        Queue<int> statChangeAmount = new Queue<int>();
        Queue<float> statTimeStamp = new Queue<float>();
        [SerializeField] List<float> statsToList = new List<float>();
        [SerializeField] List<int> statAmountsToList = new List<int>();

        public enum StatChanges
        {
            SpeedBuff,
            SpeedNerf,
            ArmorBuff,
            ArmorNerf,
            EvasionBuff,
            EvasionNerf,
            StrengthBuff,
            StrengthNerf,
            Random
        };

        Fighter fighter;
        EnemyTarget enemyTarget;
       
        int buffMax = 3;


        // Start is called before the first frame update
        void Start()
        {
            if (GetComponent<Fighter>() != null)
            {
                fighter = GetComponent<Fighter>();
            }
            if (GetComponent<EnemyTarget>() != null)
            {
                enemyTarget = GetComponent<EnemyTarget>();
            }
        }

        private void Update()
        {

            statsToList = new List<float>(statTimeStamp);
            statAmountsToList = new List<int>(statChangeAmount);

            StatTimer();
            statInstances = currentStatChanges.Count;
        }

        public void ApplyBuff(StatChanges thisStatChange, int statAmount)
        {
            int totalBuffsOfType = 0;

            foreach (StatChanges statChange in currentStatChanges)
            {
                if (statChange == thisStatChange)
                {
                    totalBuffsOfType++;
                }
            }

            if (totalBuffsOfType < buffMax)
            {
                currentStatChanges.Enqueue(thisStatChange);
                statChangeAmount.Enqueue(statAmount);

                if (currentTimer == 0)
                {
                    currentTimer = timerTotal;
                }

                AddStatChange(thisStatChange, statAmount, GetComponent<PlayerStats>());
            }
        }

        public void ApplyNerf(StatChanges thisStatChange, int statAmount)
        { 
        
            int totalNerfsOfType = 0;

            foreach (StatChanges statChange in currentStatChanges)
            {
                if (statChange == thisStatChange)
                {
                    totalNerfsOfType++;
                }
            }

            if (totalNerfsOfType < buffMax)
            {
                currentStatChanges.Enqueue(thisStatChange);
                statChangeAmount.Enqueue(statAmount);

                if (currentTimer == 0)
                {
                    currentTimer = timerTotal;
                }

                AddStatChange(thisStatChange, statAmount, GetComponent<PlayerStats>());
            }
        }

        private void StatTimer()
        {
            if (statInstances == 0) return;

            if (currentTimer > 0)
            {
                currentTimer -= Time.deltaTime;
            }
            else
            {
                RemoveStatChange(currentStatChanges.Peek(), statChangeAmount.Peek(), GetComponent<PlayerStats>());

                currentStatChanges.Dequeue();
                statChangeAmount.Dequeue();
                currentTimer = timerTotal;
            }

            if (currentTimer < 0)
            {
                currentTimer = 0;
            }
        }

        public void Humble()
        {
            currentStatChanges.Clear();
            statChangeAmount.Clear();
            statTimeStamp.Clear();
            GetComponent<PlayerStats>().ResetStats(false);
            statInstances = 0;
            currentTimer = 0;
        }

        private void RemoveStatChange(StatChanges stat, int statAmount, PlayerStats target)
        {
            switch (stat)
            {
                case StatChanges.ArmorBuff:
                    target.armor -= statAmount;
                    break;
                case StatChanges.ArmorNerf:
                    target.armor += statAmount;
                    break;
                case StatChanges.StrengthBuff:
                    target.strength -= statAmount;
                    break;
                case StatChanges.StrengthNerf:
                    target.strength += statAmount;
                    break;
                case StatChanges.SpeedBuff:
                    target.speed -= statAmount;
                    break;
                case StatChanges.SpeedNerf:
                    target.speed += statAmount;
                    break;
                case StatChanges.EvasionBuff:
                    target.evasion -= statAmount;
                    break;
                case StatChanges.EvasionNerf:
                    target.evasion += statAmount;
                    break;
                default:
                    break;
            }
        }

        private void AddStatChange(StatChanges stat, int statAmount, PlayerStats target)
        {
            if (stat == StatChanges.Random)
            {
                stat = (StatChanges)UnityEngine.Random.Range(0, 7);
            }

            switch (stat)
            {
                case StatChanges.ArmorBuff:
                    target.armor += statAmount;
                    break;
                case StatChanges.ArmorNerf:
                    target.armor -= statAmount;
                    break;
                case StatChanges.StrengthBuff:
                    target.strength += statAmount;
                    break;
                case StatChanges.StrengthNerf:
                    target.strength -= statAmount;
                    break;
                case StatChanges.SpeedBuff:
                    target.speed += statAmount;
                    break;
                case StatChanges.SpeedNerf:
                    target.speed -= statAmount;
                    break;
                case StatChanges.EvasionBuff:
                    target.evasion += statAmount;
                    break;
                case StatChanges.EvasionNerf:
                    target.evasion -= statAmount;
                    break;
                default:
                    break;
            }
        }
    }
}