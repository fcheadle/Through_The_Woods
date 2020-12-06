using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TTW.UI
{
    public class UIActorSelector : MonoBehaviour
    {

        GameObject selectionIcon;

        void Start()
        {
            selectionIcon = GetChild(this.gameObject, "Selection Icons");
        }

        private GameObject GetChild(GameObject inside, string wanted)
        {
            foreach (Transform child in inside.transform)
            {
                if (child.name == wanted) return child.gameObject;
            }
            return null;
        }

        public void Selected()
        {
            selectionIcon.gameObject.SetActive(true);
        }

        public void NotSelected()
        {
            selectionIcon.gameObject.SetActive(false);
        }
    }
}

