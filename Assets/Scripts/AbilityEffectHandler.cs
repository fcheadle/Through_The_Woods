using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using TTW.Combat;

public class AbilityEffectHandler : MonoBehaviour
{
    public PlayableDirector lightStage;
    public PlayableDirector lightBackdrop;
    public PlayableDirector lightBackground;
    public float animationTimer = 0f;

    public ProtectGhostFX protectFX;

    public List<TimelineAsset> timelines;

    private void Awake()
    {
        lightStage = GameObject.FindGameObjectWithTag("stageLight").GetComponent<PlayableDirector>();
        lightBackdrop = GameObject.FindGameObjectWithTag("midgroundLight").GetComponent<PlayableDirector>();
        lightBackground = GameObject.FindGameObjectWithTag("backgroundLight").GetComponent<PlayableDirector>();
    }

    private void Start()
    {
        GameEvents.current.AnimationStart();
        
    }

    // Update is called once per frame
    public void DimTheLights()
    {
        lightStage.Play(timelines[0]);
        lightBackdrop.Play(timelines[0]);
        lightBackground.Play(timelines[0]);
    }

    public void AnimationTimerStart(float time)
    {
        animationTimer = time;
    }

    public void LightsOn()
    {
        print("lights back on");
        lightStage.Play(timelines[1]);
        lightBackdrop.Play(timelines[1]);
        lightBackground.Play(timelines[1]);
    }

    public void EndOfAttack()
    {
        GameEvents.current.AnimationEnd();
        Destroy(this.gameObject);
    }
}
