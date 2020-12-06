using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class DeathHandler : MonoBehaviour
    {
        public EnemyAttack deathAttack = null;

        AIController aiController;

        // Start is called before the first frame update
        void Start()
        {
            aiController = GetComponent<AIController>();
        }

        public void PerformDeathAttack()
        {
            if (deathAttack == null) return;

            aiController.DeathAttack(deathAttack);
        }
    }
}
