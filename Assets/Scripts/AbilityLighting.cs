using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class AbilityLighting : MonoBehaviour
    {
        [SerializeField] AbilityLightingController lightingController;
        [SerializeField] float intensity;
        [SerializeField] Quaternion rotation;

        Light abilityLight;

        // Start is called before the first frame update
        void Start()
        {
            abilityLight = GetComponent<Light>();
        }

        // Update is called once per frame
        void Update()
        {
            if (lightingController != null)
            {
                abilityLight.intensity = lightingController.lightSourceIntensity;
                abilityLight.transform.rotation = lightingController.lightSourceRotation;
            }
        }
    }

}