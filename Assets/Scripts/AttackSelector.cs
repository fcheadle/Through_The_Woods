using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace TTW.UI
{
    public class AttackSelector : MonoBehaviour

    {
        [SerializeField] int attackPosition = 1;

        public int ChangeActor()
        {
            //print("state is now actorMove");
            return attackPosition;
        }
    }
}
