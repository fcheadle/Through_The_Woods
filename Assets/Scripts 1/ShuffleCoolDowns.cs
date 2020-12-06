using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TTW.Combat;
using System;

namespace TTW.UI
{
    public class ShuffleCoolDowns : MonoBehaviour
    {
        public List<CoolDownTimer> cooldowns;
        public List<CoolDownTimer> activeCooldowns;
        public List<CoolDownTimer> completedCooldowns;
        Vector3[] destinations;

        // Start is called before the first frame update
        void Start()
        {
            cooldowns = new List<CoolDownTimer>(FindObjectsOfType<CoolDownTimer>());
            activeCooldowns = new List<CoolDownTimer>();
            completedCooldowns = new List<CoolDownTimer>();
            destinations = new Vector3[cooldowns.Count];
            
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void SortCoolDowns()
        {
            activeCooldowns = new List<CoolDownTimer>();

            foreach (CoolDownTimer cd in cooldowns)
            {
                if (cd.cooldown.isOnCD)
                {
                    activeCooldowns.Add(cd);

                    if (completedCooldowns.Contains(cd))
                    {
                        completedCooldowns.Remove(cd);
                    }
                }
                else
                {
                    if (!completedCooldowns.Contains(cd))
                    {
                        completedCooldowns.Add(cd);
                    }
                }
            }

            activeCooldowns.Sort(SortByCoolDown);

            if (completedCooldowns.Count > 0)
            {
                for (var i = 0; i < completedCooldowns.Count; i++)
                {

                    destinations[i] = new Vector3(completedCooldowns[i].GetComponent<RectTransform>().position.x + 50, (i * 150 * -1) + 300, completedCooldowns[i].GetComponent<RectTransform>().position.z);

                    //completedCooldowns[i].MoveToPosition(destinations[i]);
                }
            }

            if (activeCooldowns.Count > 0)
            {
                for (var i = 0; i < activeCooldowns.Count; i++)
                {

                    destinations[i] = new Vector3(activeCooldowns[i].GetComponent<RectTransform>().position.x, ((i + completedCooldowns.Count) * 150 * -1) + 300, activeCooldowns[i].GetComponent<RectTransform>().position.z);

                    //activeCooldowns[i].MoveToPosition(destinations[i]);
                }
            }

            activeCooldowns.Clear();
        }

        static int SortByCoolDown(CoolDownTimer coolDown1, CoolDownTimer coolDown2)
        {
                return coolDown1.cooldown.cd.CompareTo(coolDown2.cooldown.cd);
        }
    }
}

