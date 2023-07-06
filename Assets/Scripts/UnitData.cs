using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
public class UnitData : ScriptableObject
{
    public enum UnitType { PLAYER, ENEMY };
    public UnitType curUnitType;

    [SerializeField] private int minUnitValue;
    [SerializeField] private int maxUnitValue;
    public Sprite unitSprite;
    public Sprite unitIcon;
    public Color unitColour;
    public new string name;
    public int startingSpeed;
    public int startingArmor;
    public int startingPower;
    public int startingMaxHealth;
    public int startingEnergy;
    public int startingUnitStartTurnEnergyGain;
    // The amount of units this unit can target at once with it's basic attack
    public int basicSelectionCount;
    public GameObject characterPrefab;
    public Sprite projectileSprite;

    public SkillData basicSkill;
    public SkillData skill1;
    public SkillData skill2;
    public SkillData skill3;

    [Header("Mastery Tree")]
    [SerializeField] private List<Mastery> offenseMasteries = new List<Mastery>();
    [SerializeField] private List<Mastery> defenseMasteries = new List<Mastery>();
    [SerializeField] private List<Mastery> utilityMasteries = new List<Mastery>();


    public SkillData GetSkill1()
    {
        return skill1;
    }

    public SkillData GetSkill2()
    {
        return skill2;
    }

    public SkillData GetSkill3()
    {
        return skill3;
    }

    public int GetUnitValue()
    {
        int rand = Random.Range(minUnitValue, maxUnitValue - 1);
        return rand;
    }

    public List<Mastery> GetOffenseMasteries()
    {
        return offenseMasteries;
    }

    public List<Mastery> GetDefenseMasteries()
    {
        return defenseMasteries;
    }

    public List<Mastery> GetUtilityMasteries()
    {
        return utilityMasteries;
    }
}
