using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.UI
{
    public class ArcRenderer : MonoBehaviour
    {
        [SerializeField] int vertices = 20;

        private Vector3[] positions = new Vector3[20];

        public LineRenderer[] lineRenderers = new LineRenderer[9];

        bool animationOverride = false;
        int saveRenderCount = 0;


        // Start is called before the first frame update
        void Start()
        {
            foreach(LineRenderer lineRenderer in lineRenderers)
            {
                lineRenderer.positionCount = vertices;
            }
            ClearLineRenderers();
            GameEvents.current.onAnimationStart += AnimationStart;
            GameEvents.current.onAnimationEnd += AnimationEnd;
        }

        public void ClearLineRenderers()
        {
            foreach (LineRenderer lineRenderer in lineRenderers)
            {
                lineRenderer.enabled = false;
            }
        }

        private void AnimationStart()
        {
            ClearLineRenderers();
            animationOverride = true;
        }

        private void AnimationEnd()
        {
            animationOverride = false;
        }

        public void ActivateLineRenderers(int renderCount)
        {
            saveRenderCount = renderCount;

            if (animationOverride) return;

            for (int i = 0; i < renderCount; i++)
            {

                lineRenderers[i].enabled = true;
            }
        }

        public void DrawQuadraticCurve(Vector3 startPoint, Vector3 endPoint, Vector3 peakPoint, int lineCount)
        {
            for (int i = 1; i < vertices + 1; i++)
            {
                float t = i / (float)vertices;
                positions[i - 1] = CalculateQuadraticBezierPoint(t, startPoint, endPoint, peakPoint);
            }
            lineRenderers[lineCount].SetPositions(positions);
        }

        public void DrawQuadraticCurveMultiple(Vector3 startPoint, Vector3[] endPoints, float height)
        {
            Vector3 heightVector = new Vector3(0, height, 0);

            for (int i = 0; i < endPoints.Length; i++)
            {
                DrawQuadraticCurve(startPoint, (startPoint + endPoints[i]) / 2 + heightVector, endPoints[i], i);
            }
        }

        private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 startPoint, Vector3 endPoint, Vector3 peakPoint)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            Vector3 p = uu * startPoint;
            p += 2 * u * t * endPoint;
            p += tt * peakPoint;

            return p;
        }

        public void SetColor(Color color)
        {
            lineRenderers[0].startColor = Color.white;
            lineRenderers[0].endColor = color; 
        }
    }
}
