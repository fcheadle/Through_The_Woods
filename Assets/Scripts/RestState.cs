using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class RestState : MonoBehaviour
    {
        Fighter fighter;
        EnemyTarget enemyTarget;
        public NeutralState state;

        // Start is called before the first frame update
        void Start()
        {
            if (GetComponent<Fighter>() != null) fighter = GetComponent<Fighter>();
            if (GetComponent<EnemyTarget>() != null) enemyTarget = GetComponent<EnemyTarget>();
        }

        //    //public void Dead()
        //    //{
        //    //    if (GetComponent<Fighter>() != null)
        //    //    {
        //    //        fighter.isDead = true;
        //    //        state = NeutralState.dead;
        //    //    }
        //    //    if (GetComponent<EnemyTarget>() != null)
        //    //    {
        //    //        enemyTarget.isDead = true;
        //    //        state = NeutralState.dead;
        //    //    }
        //    //}

        //    public void GuardState()
        //    {
        //        if (GetComponent<Fighter>() != null)
        //        {
        //            fighter.isGuarding = true;
        //            state = NeutralState.guard;
        //        }
        //        if (GetComponent<EnemyTarget>() != null)
        //        {
        //            enemyTarget.isGuarding = true;
        //            state = NeutralState.guard;
        //        }
        //    }

        //    public void CloakState()
        //    {
        //        if (GetComponent<Fighter>() != null)
        //        {
        //            fighter.isCloaked = true;
        //            state = NeutralState.cloak;
        //        }
        //        if (GetComponent<EnemyTarget>() != null)
        //        {
        //            enemyTarget.isCloaked = true;
        //            state = NeutralState.cloak;
        //        }
        //    }

        //    public void InvulnerableState()
        //    {
        //        if (GetComponent<Fighter>() != null)
        //        {
        //            fighter.isInvulnerable = true;
        //            state = NeutralState.invulnerable;
        //        }
        //        if (GetComponent<EnemyTarget>() != null)
        //        {
        //            enemyTarget.isInvulnerable = true;
        //            state = NeutralState.invulnerable;
        //        }
        //    }

        //    public void PhaseState()
        //    {
        //        if (GetComponent<Fighter>() != null)
        //        {
        //            fighter.isPhased = true;
        //            state = NeutralState.phase;
        //        }
        //        if (GetComponent<EnemyTarget>() != null)
        //        {
        //            enemyTarget.isPhased = true;
        //            state = NeutralState.phase;
        //        }
        //    }

        //    public void MirroredState()
        //    {
        //        if (GetComponent<Fighter>() != null)
        //        {
        //            fighter.isMirrored = true;
        //            state = NeutralState.mirror;
        //        }
        //        if (GetComponent<EnemyTarget>() != null)
        //        {
        //            enemyTarget.isMirrored = true;
        //            state = NeutralState.mirror;
        //        }
        //    }

        //    public void CounteredState()
        //    {
        //        if (GetComponent<Fighter>() != null)
        //        {
        //            fighter.isCountering = true;
        //            state = NeutralState.counter;
        //        }
        //        if (GetComponent<EnemyTarget>() != null)
        //        {
        //            enemyTarget.isCountering = true;
        //            state = NeutralState.counter;
        //        }
        //    }

        //    public void ProtectState(Fighter protector)
        //    {
        //        fighter.protector = protector;
        //        state = NeutralState.protect;
        //    }

        //    public void ProtectState(EnemyTarget protector)
        //    {
        //        enemyTarget.protector = protector;
        //        state = NeutralState.protect;
        //    }
    }
}
