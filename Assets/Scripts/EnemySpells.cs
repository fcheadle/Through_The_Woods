using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class EnemySpells : MonoBehaviour
    {
        public EnemyAttack[] ability = new EnemyAttack[10];
        public EnemyAttack safeAbility;
        [SerializeField] public Ability counterAttack = null;
        [SerializeField] public EnemyAttack deathAttack = null;

        private void Start()
        {
            if (safeAbility == null)
            {
                safeAbility = ability[0];
            } 
        }
    }
}
