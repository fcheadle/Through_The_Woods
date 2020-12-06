using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TTW.UI;
using TTW.Combat;

public class UITextDebug : MonoBehaviour
{
    public Text Score_UIText;


    void Update()
    {
        List<CoolDownTimer> coolDownTimers = transform.parent.GetComponent<ShuffleCoolDowns>().cooldowns;

        foreach (CoolDownTimer cd in coolDownTimers)
        {
            Score_UIText.text = cd.ToString();
        }
        
    }

}
