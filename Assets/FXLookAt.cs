using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class FXLookAt : MonoBehaviour
    {
        private AttackReceiver target;
        public Vector3 targetHeightOffset = new Vector3(0f, 10f, 0f);
        public float timer = 5f;

        private void Start()
        {
            transform.position = GetComponentInParent<AbilityFX>().caster.transform.position + targetHeightOffset;
            target = GetComponentInParent<AbilityFX>().targets[0];
        }

        // Start is called before the first frame update
        void Update()
        {
            transform.LookAt(target.transform, targetHeightOffset);

            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}