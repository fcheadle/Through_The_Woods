using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class AnimationInProgress : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;
        CombatController combatController;

        // Start is called before the first frame update
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            combatController = GetComponentInParent<CombatController>();
        }

        // Update is called once per frame
        void Update()
        {
            if (combatController.currentState == CombatController.State.animationFreeze)
            {
                spriteRenderer.enabled = true;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }
    }
}
