using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class AbilityLightingController : MonoBehaviour
    {
        [SerializeField] public float lightSourceIntensity;
        [SerializeField] public Quaternion lightSourceRotation;

        public void AddLightSource(Light light)
        {
            lightSourceIntensity = light.intensity;
            lightSourceRotation = light.transform.rotation;
        }
    }
}
