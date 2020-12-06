using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    // Start is called before the first frame update
    void Awake()
    {
        current = this;
    }


    public event Action onAnimationStart;
    public event Action onAnimationEnd;

    public event Action onGetActorSelection;
    public event Action onActorSelectCell;
    public event Action onActorSelectAllies;
    public event Action onActorSelectFoes;
    public event Action onActorSelectMelee;
    public event Action onActorSelectRanged;
    public event Action onActorSelectSupport;
    public event Action onActorSelectSelf;
    public event Action onActorSelectSupportNotSelf;
    public event Action onActorSelectAdjacent;
    public event Action onActorSelectCardinalAllies;


    public void AnimationStart()
    {
        if (onAnimationStart != null)
        {
            onAnimationStart();
        }
    }

    public void AnimationEnd()
    {
        if (onAnimationEnd != null)
        {
            onAnimationEnd();
        }
    }

    public void ActorSelectMelee()
    {
        if (onActorSelectMelee != null)
        {
            onActorSelectMelee();
        }
    }

    public void ActorSelectRanged()
    {
        if (onActorSelectRanged != null)
        {
            onActorSelectRanged();
        }
    }

    public void ActorSelectCell()
    {
        if (onActorSelectCell != null)
        {
            onActorSelectCell();
        }
    }

    public void ActorSelectSupport()
    {
        if (onActorSelectSupport != null)
        {
            onActorSelectSupport();
        }
    }

    public void ActorSelectSelf()
    {
        if (onActorSelectSelf != null)
        {
            onActorSelectSelf();
        }
    }

    public void ActorSelectAllies()
    {
        if (onActorSelectAllies != null)
        {
            onActorSelectAllies();
        }
    }

    public void ActorSelectCardinalAllies()
    {
        if (onActorSelectCardinalAllies != null)
        {
            onActorSelectCardinalAllies();
        }
    }

    public void ActorSelectFoes()
    {
        if (onActorSelectFoes != null)
        {
            onActorSelectFoes();
        }
    }

    public void ActorSelectSupportNotSelf()
    {
        if (onActorSelectSupportNotSelf != null)
        {
            onActorSelectSupportNotSelf();
        }
    }

    public void ActorSelectAdjacent()
    {
        if (onActorSelectAdjacent != null)
        {
            onActorSelectAdjacent();
        }
    }

    public void GetActorSelection()
    {
        if (onGetActorSelection != null)
        {
            onGetActorSelection();
        }
    }
}
