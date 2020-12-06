using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace TTW.Combat
{
    public class VFX : MonoBehaviour
    {
        private ParticleSystem ps;
        CinemachineImpulseSource impulseSource;

        public void Start()
        {
            impulseSource = GetComponent<CinemachineImpulseSource>();
            ps = GetComponentInChildren<ParticleSystem>();

            if (GetComponent<CinemachineImpulseSource>() != null)
            {
                impulseSource.GenerateImpulse();
            }
        }

        public void Update()
        {
            if (ps)
            {
                if (!ps.IsAlive())
                {
                    print("destroying self");
                    Destroy(this.gameObject);
                }
            }
        }
    }
}

