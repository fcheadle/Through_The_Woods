using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TTW.UI
{
    public class UIAlphaFade : MonoBehaviour
    {

        [SerializeField] private float myAlpha;
        [SerializeField] private float yPosition;

        private Image image;
        private RectTransform rectTransform;

        
        

        // Start is called before the first frame update
        void Start()
        {
            image = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
            
        }

        // Update is called once per frame
        void Update()
        {
            AlphaFade();
        }

        private void AlphaFade()
        {
            var colorWithNewAlpha = image.color;

            colorWithNewAlpha.a = ScreenPositionToAlpha();

            image.color = colorWithNewAlpha;
        }

        private float ScreenPositionToAlpha()
        {
            float newAlpha = Mathf.InverseLerp(250, 700, rectTransform.position.y);

            return newAlpha;
        }

        
    }
}
