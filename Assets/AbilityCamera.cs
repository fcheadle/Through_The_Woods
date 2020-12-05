using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

namespace TTW.Combat
{
    public class AbilityCamera : MonoBehaviour
    {
        [SerializeField] [Range(1, 3)] int cameraActive = 1;
        [SerializeField] CinemachineVirtualCamera homeCamera;
        [SerializeField] CinemachineVirtualCamera followCamera; 
        [SerializeField] CinemachineVirtualCamera targetCamera;

        Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
            followCamera.Follow = GetComponent<AbilityFX>().caster.transform;
            targetCamera.Follow = GetComponent<AbilityFX>().targets[0].transform;
            
        }

        private void Update()
        {
            if (cameraActive == 1)
            {
                animator.SetTrigger("cameraOne");
            }
            if (cameraActive == 2)
            {
                animator.SetTrigger("cameraTwo");
            }
            if (cameraActive == 3)
            {
                animator.SetTrigger("cameraThree");
            }
        }
    }
}
