using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TTW.Combat;

namespace TTW.UI
{
    public class UpdateAttackName : MonoBehaviour
    {

        //THIS CLASS IS FOR DEBUGGING PURPOSES AND NEEDS TO BE REMOVED ONCE UI FOR ATTACKS IS FINISHED

        public Text attackName;
        public int attackPosition;

        CombatController combatController;
        

        // Start is called before the first frame update
        void Start()
        {
            combatController = FindObjectOfType<CombatController>();
        }

        // Update is called once per frame
        void Update()
        {
            attackName.text = combatController.actorInPosition[combatController.highlightedActorPosition].abilities[attackPosition].ToString();
        }
    }
}
