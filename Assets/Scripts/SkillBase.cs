using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Base", menuName = "Skill")]
public class SkillBase : ScriptableObject
{
    public Sprite skillBaseIcon;
    public string skillBaseName;
    public string skillBaseDesc;
    public int skillBaseMaxAmount;
}
