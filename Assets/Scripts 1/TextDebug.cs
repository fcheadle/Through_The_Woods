using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDebug : MonoBehaviour
{
    public Text Score_UIText; 

    void Update()
    {
        Score_UIText.text = transform.parent.name + ": " + transform.parent.GetComponent<RectTransform>().position.y.ToString();
    }

}
