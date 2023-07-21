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
    public int skillCooldown;
    public int skillSelectionCount;
    public bool isSelfCast;
    public bool special;
    public EffectData effect;
    public int effectTurnLength;
    public float effectHitChance;
    [Tooltip("Skill has increased power to a unit with this effect")]
    public enum SkillExtraPowerToEffect { NONE, SPEEDUP, SPEEDDOWN, BLEED, RECOVER, EXHAUST, HASTE, SLEEP, 
        POWERUP, POWERDOWN, DEFENSEUP, DEFENSEDOWN, PARRY, TAUNT, MARK, SHADOWPARTNER}
    public SkillExtraPowerToEffect curSkillExtraPowerToEffect;
    [Tooltip("Percentage increase of power when targeting a unit with an effect")]
    public int percIncPower;
    public Sprite skillPowerIcon;
}
