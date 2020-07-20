using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MagicType { sun, moon, lamp, electric, prysm }

[CreateAssetMenu(fileName = "New Magic", menuName = "Magic")]

public class Magic : ScriptableObject
{
    public MagicType magicType;
}
