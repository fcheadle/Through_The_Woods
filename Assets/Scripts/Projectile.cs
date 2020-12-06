using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class Projectile : MonoBehaviour
    {
        public Transform targetTransform;
        public Vector3 targetOffset;
        public int lineSegment = 10;
        public float flightTime = 1f;

        private Camera cam;
        private Rigidbody rb;
        private Mover mover;
        public Vector3 originPosition;
        public float deathTimer = 3f;
        public VFX deathvfx;
        public AnimationHandler caster;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            transform.position = originPosition + targetOffset;
            mover = GetComponent<Mover>();

            LaunchProjectile();
        }

        private void Update()
        {
            DestroyOnTarget();
            DeathTimer();
        }

        private void DeathTimer()
        {
            if (deathTimer >= 0)
            {
                deathTimer -= Time.deltaTime;
            }
            else
            {
                caster.DealDamage();
                Destroy(this.gameObject);
            }
        }

        private void DestroyOnTarget()
        {
            if (mover.GetGridPos() == targetTransform.GetComponent<Mover>().GetGridPos())
            {
                VFX newVFX = Instantiate(deathvfx);
                newVFX.transform.position = targetTransform.position + targetOffset;
                //caster.DealDamage();
                targetTransform.GetComponent<AnimationHandler>().GetHurtLight();
                Destroy(this.gameObject);
            }
        }

        void LaunchProjectile()
        {
            Vector3 vo = CalculateVelocty(targetTransform.position, originPosition, flightTime);

            transform.rotation = Quaternion.identity;
            rb.velocity = vo;
        }

        Vector3 CalculateVelocty(Vector3 target, Vector3 origin, float time)
        {
            Vector3 distance = target - origin;
            Vector3 distanceXz = distance;
            distanceXz.y = 0f;

            float sY = distance.y;
            float sXz = distanceXz.magnitude;

            float Vxz = sXz / time;
            float Vy = (sY / time) + (0.5f * Mathf.Abs(Physics.gravity.y) * time);

            Vector3 result = distanceXz.normalized;
            result *= Vxz;
            result.y = Vy;

            return result;
        }
    }
}



