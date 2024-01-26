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

    public enum SkillAnimType { DEFAULT, SKILL }
    public SkillAnimType curAnimType;

    public Sprite skillSprite;
    public string skillName;
    public string skillDescr;
    public string skillTabDescr;
    public int skillPower;
    [Tooltip("For each of this, each accuracy hit will create x 1 power text appear")]
    public int skillAttackAccMult = 1;
    [Tooltip("The base amount of hit lines a skill will do on (with at least bad or higher accuracy with Attack Bar")]
    public int skillAttackCount = 0;
    public int projectileSpeed = 1;
    public int skillCooldown;
    public int skillSelectionCount;
    public int maxSkillLevel = 5;
    public bool isSelfCast;
    [Tooltip("Determines whether this skill doubles the turns remaining on each target for its effect that it applies. (before applying its own)")]
    public bool isDoublingEffect;
    public bool isLeaching;
    public bool isPassive;
    public bool special;
    public EffectData effect;
    public int effectTurnLength;
    [Tooltip("0 = 100% also, although 1 = 1 as normal upwards.")]
    public float effectHitChance;
    [Tooltip("Each of these, causes the skill to immediately apply a stack of this amount to each target")]
    public int baseEffectApplyCount = 0;
    [Tooltip("Skill has increased power to a unit with this effect")]
    public enum SkillExtraPowerToEffect { NONE, SPEEDUP, SPEEDDOWN, BLEED, RECOVER, EXHAUST, HASTE, SLEEP, 
        POWERUP, POWERDOWN, DEFENSEUP, DEFENSEDOWN, PARRY, TAUNT, MARK, SHADOWPARTNER}
    public SkillExtraPowerToEffect curSkillExtraPowerToEffect;
    [Tooltip("Percentage increase of power when targeting a unit with an effect")]
    public int percIncPower;
    public Sprite skillPowerIcon;
    public Sprite skillProjectile;
    public bool projectileAllowSpin;
    public RuntimeAnimatorController projectileAC;
    public AudioClip skillLaunch;
    public AudioClip skillHit;
    public int originalIndex;
}
