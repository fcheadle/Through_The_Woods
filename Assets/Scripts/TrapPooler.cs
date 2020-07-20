using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class TrapPooler : MonoBehaviour
    {
        public List<GameObject> pooledTraps;
        public GameObject trapToPool;
        public int amountToPool;

        public static TrapPooler SharedInstance;

        void Awake()
        {
            SharedInstance = this;
        }

        private void Start()
        {
            pooledTraps = new List<GameObject>();
            for (int i = 0; i < amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(trapToPool);
                obj.SetActive(false);
                pooledTraps.Add(obj);
            }
        }

        public GameObject GetPooledTrap()
        {
            
            for (int i = 0; i < pooledTraps.Count; i++)
            {
                
                if (!pooledTraps[i].activeInHierarchy)
                {
                    return pooledTraps[i];
                }
            }
              
            return null;
        }
    }
}
