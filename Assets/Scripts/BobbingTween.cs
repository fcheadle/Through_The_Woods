using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.UI
{
    public class BobbingTween : MonoBehaviour
    {
        float originY;

        // Start is called before the first frame update
        void Start()
        {
            originY = transform.position.y;
            LeanTween.moveY(this.gameObject, originY + 2f, 2f).setEaseInOutSine().setLoopPingPong();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
