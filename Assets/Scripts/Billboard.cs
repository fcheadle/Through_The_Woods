using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.World
{
    public class Billboard : MonoBehaviour
    {
        float fixedRotation = 360f;

        void Update()
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, fixedRotation, transform.eulerAngles.z);
        }
    }
}
