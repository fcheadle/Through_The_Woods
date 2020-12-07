using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TTW.World
{
    public class WorldMovement : MonoBehaviour
    {
        Animator animator;
        NavMeshAgent navMeshAgent;
        float horInput;
        float verInput;
        Vector3 currentFramePosition;
        Vector3 previousFramePosition;
        Vector3 distance;
        bool stopped;

        private void Start()
        {
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();

            currentFramePosition = transform.position;
            previousFramePosition = transform.position;
            stopped = true;
        }

        void Update()
        {
            horInput = Input.GetAxis("Horizontal");
            verInput = Input.GetAxis("Vertical");

            Move();
            SwitchAnimations();
            stopped = CheckIfStopped();
        }

        private bool CheckIfStopped()
        {
            currentFramePosition = transform.position;
            distance = currentFramePosition - previousFramePosition;

            previousFramePosition = currentFramePosition;

            if (distance == new Vector3(0f, 0f, 0f))
            {
                return true;
            }

            return false;
        }

        private void SwitchAnimations()
        {
            if (horInput < 0)
            {
                ClearAnimations();
                animator.SetBool("left", true);
                return;
            }
            else if (horInput > 0)
            {
                ClearAnimations();
                animator.SetBool("right", true);
                return;
            }
            else if (verInput > 0)
            {
                ClearAnimations();
                animator.SetBool("down", true);
                return;
            }
            else if (verInput < 0)
            {
                ClearAnimations();
                animator.SetBool("up", true);
                return;
            }
            if (stopped)
            {
                ClearAnimations();
                animator.SetBool("idle", true);
            }
        }

        private void ClearAnimations()
        {
            animator.SetBool("right", false);
            animator.SetBool("left", false);
            animator.SetBool("down", false);
            animator.SetBool("up", false);
            animator.SetBool("idle", false);
        }

        private void Move()
        {
            Vector3 movement = new Vector3(horInput, 0f, verInput);
            Vector3 moveDestination = transform.position + movement;
            GetComponent<NavMeshAgent>().destination = moveDestination;
        }
    }
}
