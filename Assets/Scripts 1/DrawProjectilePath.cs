using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TTW.Combat;

public class DrawProjectilePath : MonoBehaviour
{
    public Transform shootPoint;
    public LineRenderer lineVisual;
    public int lineSegment = 10;
    public GameObject target1;
    public GameObject target2;

    void Start()
    {
        lineVisual.positionCount = lineSegment;
    }

    // Update is called once per frame
    void Update()
    {
        LaunchProjectile();
    }

    void LaunchProjectile()
    {
        Vector3 vo = CalculateVelocty(target1.transform.position, target2.transform.position, 1f);

        Visualize(vo);
    }

    void Visualize(Vector3 vo)
    {
        for (int i = 0; i < lineSegment; i++)
        {
            Vector3 pos = CalculatePosInTime(vo, i / (float)lineSegment);
            lineVisual.SetPosition(i, pos);
        }
    }

    Vector3 CalculateVelocty(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXz = distance;
        distanceXz.y = 0f;

        float sY = distance.y;
        float sXz = distanceXz.magnitude;

        float Vxz = sXz * time;
        float Vy = (sY / time) + (0.5f * Mathf.Abs(Physics.gravity.y/2) * time);

        Vector3 result = distanceXz.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }

    Vector3 CalculatePosInTime(Vector3 vo, float time)
    {
        Vector3 Vxz = vo;
        Vxz.y = 0f;

        Vector3 result = shootPoint.position + vo * time;
        float sY = (-0.25f * Mathf.Abs(Physics.gravity.y) * (time * time)) + (vo.y * time * 2f) + shootPoint.position.y;

        result.y = sY;

        return result;
    }
}