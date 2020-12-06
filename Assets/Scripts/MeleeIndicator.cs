using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TTW.Combat;

public class MeleeIndicator : MonoBehaviour
{
    MeshRenderer meshRenderer;
    public bool saveState;
    public bool indicate = false;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        GameEvents.current.onAnimationStart += AnimationStart;
        GameEvents.current.onAnimationEnd += AnimationEnd;
        saveState = false;
    }

    private void AnimationStart()
    {
        if (indicate == true)
        {
            saveState = meshRenderer.enabled;
            meshRenderer.enabled = false;
        } 
    }

    private void AnimationEnd()
    {
        if (indicate == true)
        {
            if (saveState == true)
            {
                meshRenderer.enabled = true;
                saveState = false;
            }
        }
    }

    public void Indicate()
    {
        indicate = true;
        meshRenderer.enabled = true;
    }

    public void StopIndicating()
    {
        indicate = false;
        meshRenderer.enabled = false;
        saveState = false;
    }
}
