using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Speaker", menuName = "Data/New Speaker")]
public class TouhouSpeaker : ScriptableObject
{
    public string speakerName;
    public Color nameColor;
}
