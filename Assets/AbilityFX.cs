using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class AbilityFX : MonoBehaviour
    {
        [SerializeField] public VFX castingVFX = null;
        [SerializeField] public VFX landingVFX = null;
        [SerializeField] public Projectile projectile = null;
        [SerializeField] public AttackExecutor caster = null;
        [SerializeField] public List<AttackReceiver> targets = new List<AttackReceiver>();

        public Vector3 targetHeightOffset = new Vector3(0f, 10f, 0f);

        public void CreateCastingVFX()
        {
            VFX newVFX = Instantiate(castingVFX);
            newVFX.transform.position = caster.transform.position;
        }

        public void CreateProjectile()
        {
            foreach(AttackReceiver target in targets)
            {
                Projectile newProjectile = Instantiate(projectile);
                newProjectile.originPosition = caster.transform.position;
                newProjectile.caster = caster.GetComponent<AnimationHandler>();
                newProjectile.targetTransform = target.transform;
            }
        }

        public void CreateLandingVFX()
        {
            foreach (AttackReceiver target in targets)
            {
                VFX newVFX = Instantiate(landingVFX);
                newVFX.transform.position = target.transform.position + targetHeightOffset;
            }
        }

        public void AddTarget(AttackReceiver target)
        {
            targets.Add(target);
        }
    }
}
