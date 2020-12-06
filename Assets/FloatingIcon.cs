using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingIcon : MonoBehaviour
{
    Vector3 origin;
    public float timer = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
        LeanTween.move(gameObject, new Vector3(origin.x, origin.y - 5f, origin.z), 2.5f).setEaseInOutSine();
    }

    // Update is called once per frame
    private void Update()
    {
        DestroyTimer();
    }

    private void DestroyTimer()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
