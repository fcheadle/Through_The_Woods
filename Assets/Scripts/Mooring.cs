using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TTW.Combat
{
    [CreateAssetMenu(fileName = "New Mooring", menuName = "Mooring")]

    public class Mooring : ScriptableObject
    {
        public Ability[] ability = new Ability[12];
    }
}
