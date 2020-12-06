using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class AbilitySpriteController : MonoBehaviour
    {
        AnimationHandler casterAnimation = null;
        List<AnimationHandler> targetAnimations = new List<AnimationHandler>();

        void Start()
        {
            casterAnimation = GetComponent<AbilityFX>().caster.GetComponent<AnimationHandler>();

            foreach(AttackReceiver target in GetComponent<AbilityFX>().targets)
            {
                targetAnimations.Add(target.GetComponent<AnimationHandler>());
            }
        }

        public void CasterPerformMelee()
        {
            casterAnimation.DoWeaponAttack();
        }

        public void CasterPerformRanged()
        {
            casterAnimation.DoRangedAttack();
        }

        public void CasterPerformMagic()
        {
            casterAnimation.DoMagicalAttack();
        }

        public void CasterPerformGuard()
        {
            casterAnimation.Defend();
        }

        public void TargetsReactToAbility()
        {
            foreach (AnimationHandler handler in targetAnimations)
            {
                handler.GetHurtLight();
            }
        }
    }
}
