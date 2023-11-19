using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
public class UnitData : ScriptableObject
{
    public enum UnitType { PLAYER, ENEMY };
    public UnitType curUnitType;

    [SerializeField] private int unitValue;
    public Sprite unitSprite;
    public Sprite unitIcon;
    public Color unitColour;
    public new string unitName;
    public int startingSpeed;
    public int startingDefense;
    public int startingPower;
    public int startingMaxHealth;

    public float speedIncPerLv;
    public float powerIncPerLv;
    public float healingPowerIncPerLv;
    public float defenseIncPerLv;
    public float maxHealthIncPerLv;

    public int startingEnergy;
    public int startingUnitStartTurnEnergyGain;
    // The amount of units this unit can target at once with it's basic attack
    public int basicSelectionCount;
    public GameObject characterPrefab;
    public Sprite projectileSprite;

    public SkillData skill0;
    public SkillData skill1;
    public SkillData skill2;
    public SkillData skill3;

    [Header("Mastery Tree")]
    [SerializeField] private List<Stat> standardStats = new List<Stat>();
    [SerializeField] private List<Stat> advancedStats = new List<Stat>();

    public AudioClip deathClip;

    private int curAttackChargeTurnStart;

    public void UpdateUnitCurAttackCharge(int newCharge)
    {
        curAttackChargeTurnStart = newCharge;
    }

    public int GetCurAttackChargeTurnStart()
    {
        return curAttackChargeTurnStart;
    }

    public SkillData GetSkill0()
    {
        return skill0;
    }

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
        //int rand = Random.Range(unitValue, maxUnitValue - 1);
        return unitValue;
    }

    public List<Stat> GetStandardStats()
    {
        return standardStats;
    }

    public List<Stat> GetAdvancedStats()
    {
        return advancedStats;
    }
}
