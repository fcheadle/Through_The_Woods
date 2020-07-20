using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class SpellsEquipped : MonoBehaviour
    {
        public Ability[] ability = new Ability[4];

        [SerializeField] public EnemyAttack counterAttack = null;
    }
}