using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    public enum SkillType { OFFENSE, SUPPORT };
    public SkillType curSkillType;

    public enum SkillSelectionType { ENEMIES, PLAYERS };
    public SkillSelectionType curSkillSelectionType;

    public enum SkillGameType { BASIC, PRIMARY, SECONDARY, ALTERNATE }
    public SkillGameType curSkillGameType;

    public Sprite skillSprite;
    public string skillName;
    public string skillDescr;
    public int skillPower;
    public int skillAttackCount;
    public int skillCooldown;
    public int skillEnergyCost;
    public int skillSelectionCount;
    public Sprite skillPowerIcon;
}
