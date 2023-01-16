using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class SkillData : ScriptableObject
{
    public enum SkillType { OFFENSE, SUPPORT };
    public SkillType curSkillType;

    public enum SkillSelectionType { ENEMIES, PLAYERS };
    public SkillSelectionType curSkillSelectionType;

    public enum SkillGameType { BASIC, PRIMARY, SECONDARY, ALTERNATE }
    public SkillGameType curSkillGameType;

    public enum SkillRangedType { MELEE, RANGED }
    public SkillRangedType curRangedType;

    public Sprite skillSprite;
    public string skillName;
    public string skillDescr;
    public int skillPower;
    public int skillAttackCount;
    public int projectileSpeed = 1;
    public int skillEnergyCost;
    public int skillSelectionCount;
    public EffectData effect;
    public int effectTurnLength;
    public int effectOnTargetBonusPower;
    public Sprite skillPowerIcon;
}
