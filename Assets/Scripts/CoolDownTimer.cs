using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TTW.Combat
{
    public class CoolDownTimer : MonoBehaviour
    {
        public CoolDown cooldown;
        Image image;

        void Start()
        {
            image = GetComponent<Image>();
        }

        void Update()
        {
            float thing = ((cooldown.cd / cooldown.maxCD) - 1f) * -1f;
            image.fillAmount = thing;
        }
    }
}
