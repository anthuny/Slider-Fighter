using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mastery", menuName = "Mastery")]
public class Mastery : ScriptableObject
{
    public Sprite masteryIcon;
    public string masteryName;
    public string masteryDesc;
    public int masteryMaxAmount;
}
