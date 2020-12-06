using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class ProtectGhostFX : MonoBehaviour
    {
        SpriteRenderer fxSprite;
        public Sprite currentSprite;

        void Start()
        {
            fxSprite = GetComponent<SpriteRenderer>();
            CopySpriteRendererImage(currentSprite);
        }


        public void CopySpriteRendererImage(Sprite newSprite)
        {
            Color tmp = Color.cyan;

            tmp.a = 0.25f;
            fxSprite.sprite = newSprite;
            fxSprite.size = new Vector2(8f, 12f);
            fxSprite.color = tmp;
        }
    }
}
