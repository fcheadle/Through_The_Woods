using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TTW.Combat;

namespace TTW.UI
{
    public class Healthbar : MonoBehaviour
    {
        Image image;
        public Fighter fighter = null;

        // Start is called before the first frame update
        void Start()
        {
            image = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            float thing = ((fighter.GetComponent<PlayerStats>().health / fighter.GetComponent<PlayerStats>().maxHealth));
            image.fillAmount = thing;
        }
    }
}
