using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class SkillData : ScriptableObject
{
    public enum SkillTeamType { ENEMIES, PLAYERS };
    public SkillTeamType curSkillTeamType;

    public enum SkillType { OFFENSE, SUPPORT };
    public SkillType curSkillType;

    public enum SkillSelectionUnitType { ENEMIES, PLAYERS };
    public SkillSelectionUnitType curSkillSelectionUnitType;
    public SkillSelectionUnitType startingSelectionUnitType;

    public enum SkillSelectionAliveType { ALIVE, DEAD };
    public SkillSelectionAliveType curskillSelectionAliveType;

    public enum SkillRangedType { MELEE, RANGED }
    public SkillRangedType curRangedType;

    public enum SkillAnimType { DEFAULT, SKILL }
    public SkillAnimType curAnimType;

    public enum SkillEffectType { HITS, INSTANT }
    public SkillEffectType curSkillEffectType;

    public enum SkillActiveType { ACTIVE, PASSIVE }
    public SkillActiveType curSkillActiveType;

    public enum SkillSelectionType { UNITS, AREAS}
    public SkillSelectionType curSkillSelectionType;

    // Position of each slot in range of the skill, from the fighter
    public Vector2 skillRangeHitArea;
    public List<Vector2> skillRangeHitAreas = new List<Vector2>();
    public int skillAreaHitCount = 1;
    [Tooltip("When on, all allowed slots in combat for skill will an attack slot")]
    public bool attackAllSelected = false;
    [Tooltip("When on, all units in allowed combat slots, are selected")]
    public bool attackAllUnits = false;
    public bool canTargetSelf = false;
    public int skillIgnoreRange = 0;
    public int startingSkillRange = 1;
    public int curSkillRange;

    public Sprite skillSprite;
    public string skillName;
    public string skillDescr;
    public string skillTabDescr;
    public int startingSkillLevel = 0;
    public int curSkillLevel = 0;
    public int pointsAdded;
    public int startingSkillPower;
    public int curSkillPower;
    [Tooltip("For each of this, each accuracy hit will create x 1 power text appear")]
    public int skillAttackAccMult = 1;
    [Tooltip("The base amount of hit lines a skill will do on (with at least bad or higher accuracy with Attack Bar")]
    public int skillBaseHitOutput = 1;
    public int skillHitAttempts = 1;
    public int projectileSpeed = 1;
    public int skillCooldown;
    public int curCooldown;
    public int skillSelectionCount;
    //
    //public int maxSkillLevel = 5;
    public bool isSelfCast;
    public bool isSpecial;
    [Tooltip("Determines whether this skill doubles the turns remaining on each target for its effect that it applies. (before applying its own)")]
    public bool isDoublingEffect;
    public bool isLeaching;
    public bool isPassive;
    public bool isBonusFromTargetAmount;
    public bool isHealingFromResult;
    public bool isLongerSkillAnim;
    public int healPowerAmount;
    public bool isReviving;
    public bool isMindControlling;
    [Tooltip("Determines that this skill cleanses an effect")]
    public bool isCleansingEffect;
    [Tooltip("Determines which effect is cleansed")]
    public enum SkillToCleanse { NONE, SPEEDUP, SPEEDDOWN, BLEED, RECOVER, EXHAUST, HASTE, SLEEP,
        POWERUP, POWERDOWN, DEFENSEUP, DEFENSEDOWN, PARRY, TAUNT, MARK, SHADOWPARTNER
    }
    public SkillToCleanse curSkillToCleanse;

    [Tooltip("Determines that this skill cleanses a RANDOM effect")]
    public bool isCleansingEffectRandom;
    [Tooltip("Remove effects on target up till this amount")]
    public int cleanseCount;

    public bool special;
    public EffectData effect;
    public EffectData effect2;
    public int effectTurnLength;
    [Tooltip("0 = 100% also, although 1 = 1 as normal upwards.")]
    public float curEffectHitChance;
    public float startingEffectHitChance;
    [Tooltip("Each of these, causes the skill to immediately apply a stack of this amount to each target")]
    public int baseEffectApplyCount = 0;
    [Tooltip("Skill has increased power to a unit with this effect")]
    public enum SkillExtraPowerToEffect { NONE, SPEEDUP, SPEEDDOWN, BLEED, RECOVER, EXHAUST, HASTE, SLEEP, 
        POWERUP, POWERDOWN, DEFENSEUP, DEFENSEDOWN, PARRY, TAUNT, MARK, SHADOWPARTNER}
    public SkillExtraPowerToEffect curSkillExtraPowerToEffect;
    [Tooltip("Percentage increase of power when targeting a unit with an effect")]
    public int percIncPower;
    public int upgradeIncTargetCount = 0;
    public int upgradeIncHitsCount = 0;
    public int upgradeIncPowerCount = 0;

    public Sprite skillPowerIcon;
    public Sprite skillProjectile;
    public bool projectileAllowSpin;
    public bool projectileAllowIdle;
    public bool projectileAllowRandPosSpawn;
    public bool projectileAllowRandRotSpawn;
    public RuntimeAnimatorController projectileAC;
    public RuntimeAnimatorController targetEffectVisualAC;
    public AudioClip skillLaunch;
    public bool repeatLaunchSFX = true;
    public bool giveExtraDesc = true;
    public AudioClip projectileLaunch;
    public AudioClip skillHit;
    public AudioClip skillHitAdditional;
    public int originalIndex;

    public bool reanimated;

    public void ResetSkillDataSkillRange()
    {
        curSkillRange = startingSkillRange;
    }

    public void SetSkillStarting()
    {
        startingSelectionUnitType = curSkillSelectionUnitType;
    }

    public int GetCalculatedSkillSelectionCount()
    {
        int val = skillSelectionCount + upgradeIncTargetCount;

        if (val > 6)
            val = 6;

        return val;
    }

    // Returns a list of vector2s which represent the slot locations on the combat grid
    // from the fighters position

    public int GetCalculatedSkillPower()
    {
        float val = upgradeIncHitsCount;

        return (int)val;
    }

    public int GetCalculatedSkillPowerStat()
    {
        //float val = startingSkillPower + (((25f / 100f) * startingSkillPower) * upgradeIncPowerCount);
        //float val = startingSkillPower;
        float val = curSkillPower + (upgradeIncPowerCount * 2);
        //Debug.Log("float in skilldata is " + val);

        float val2 = 0;

        if (healPowerAmount != 0)
            val2 = healPowerAmount + curSkillPower + (upgradeIncPowerCount * 2);

        if (healPowerAmount == 0)
            return Mathf.RoundToInt(val);
        else
            return Mathf.RoundToInt(val2);
    }

    public int GetCalculatedSkillHitAmount()
    {
        int val = skillBaseHitOutput + upgradeIncHitsCount;
        return val;
    }

    public int GetCalculatedSkillEffectStat()
    {
        //float val = upgradeIncPowerCount;
        //Debug.Log("float in skilldata is " + val);

        curEffectHitChance = startingEffectHitChance;

        int calculatedEffectHitChace = 100 - (int)curEffectHitChance;
        //Mathf.RoundToInt(val);
        return Mathf.RoundToInt(calculatedEffectHitChace);
    }

    public int GetCalculatedSkillEffectChance()
    {
        float val = upgradeIncPowerCount;

        return (int)val;
    }

    public void ResetSkill()
    {
        curSkillLevel = startingSkillLevel;
        curSkillPower = startingSkillPower;
        curEffectHitChance = startingEffectHitChance;
        pointsAdded = 0;
        curCooldown = 0;

        upgradeIncTargetCount = 0;
        upgradeIncHitsCount = 0;
        upgradeIncPowerCount = 0;

        curSkillSelectionUnitType = startingSelectionUnitType;
        reanimated = false;
    }

    public void SwitchTargetingTeam(bool toggle = true)
    {
        /*
        if (curSkillSelectionType == SkillSelectionType.PLAYERS)
            curSkillSelectionType = SkillSelectionType.ENEMIES;
        else
            curSkillSelectionType = SkillSelectionType.PLAYERS;
        */

        reanimated = toggle;

        if (!toggle)
        {
            curSkillSelectionUnitType = startingSelectionUnitType;
        }
    }
}
