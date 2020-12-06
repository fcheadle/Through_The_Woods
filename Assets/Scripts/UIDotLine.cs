using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;
using TTW.Combat;

public class UIDotLine : MonoBehaviour
{

    public Texture lineTex;
    public Transform startingTransform;
    public RectTransform endingTransform;
    private VectorLine pathLine;
    bool animationOverride = false;
    public Vector2 yOffset;


    // Start is called before the first frame update
    void Start()
    {
        pathLine = new VectorLine("Path", new List<Vector2>(), lineTex, 12.0f, LineType.Continuous);
        pathLine.color = Color.white;
        pathLine.textureScale = 1.0f;
        DrawDotLine();

        GameEvents.current.onAnimationStart += AnimationStart;
        //GameEvents.current.onAnimationEnd += AnimationEnd;
    }

    public void DestroySelf()
    {
        pathLine.points2.Clear();
        pathLine.Draw();
        GameEvents.current.onAnimationStart -= ClearDrawing;
        GameEvents.current.onAnimationEnd -= DrawDotLine;
        Destroy(this.gameObject);
    }

    public void ClearDrawing()
    {
        pathLine.points2.Clear();
        pathLine.Draw();
    }

    private void AnimationStart()
    {
        ClearDrawing();
        //animationOverride = true;
    }

    //private void AnimationEnd()
    //{
    //    animationOverride = false;
    //}

    public void DrawDotLine()
    {
        if (animationOverride) return;

        pathLine.points2.Clear();
        pathLine.points2.Add(Camera.main.WorldToScreenPoint(startingTransform.position));
        pathLine.points2.Add(Camera.main.WorldToScreenPoint(new Vector2(startingTransform.position.x, startingTransform.position.y - 5f)));
        pathLine.points2.Add(new Vector2(endingTransform.anchoredPosition.x, Camera.main.WorldToScreenPoint(new Vector2(startingTransform.position.x, startingTransform.position.y - 5f)).y));
        pathLine.points2.Add(endingTransform.anchoredPosition + yOffset);
        pathLine.Draw();
    }
}
