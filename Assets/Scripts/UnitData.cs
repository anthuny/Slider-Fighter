using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
public class UnitData : ScriptableObject
{
    public enum UnitType { PLAYER, ENEMY };
    public UnitType curUnitType;

    public enum RaceType { HUMAN, BEAST, ETHEREAL };
    public RaceType curRaceType;

    [SerializeField] private int unitValue;
    public Sprite unitSprite;
    public Sprite unitIcon;
    public Color unitColour;
    public new string unitName;
    public int startingSpeed;
    public int startingDefense;
    public int startingPower;
    public int startingHealingPower;
    public int startingMaxHealth;

    public float speedIncPerLv;
    public float powerIncPerLv;
    public float healingPowerIncPerLv;
    public float defenseIncPerLv;
    public float maxHealthIncPerLv;

    public int statDefense = 0;
    public int statAttack = 0;
    public int statHealing = 0;
    public int statUtility = 0;

    // The amount of units this unit can target at once with it's basic attack
    public int basicSelectionCount;
    public GameObject characterPrefab;

    [Header("Skills")]
    [SerializeField] private List<SkillData> curSkills = new List<SkillData>();
    //[SerializeField] private List<SkillBase> advancedStats = new List<SkillBase>();

    public AudioClip deathClip;
    public AudioClip hitRecievedClip;

    private int curAttackChargeTurnStart;

    public void UpdateUnitCurAttackCharge(int newCharge)
    {
        curAttackChargeTurnStart = newCharge;
    }

    public int GetCurAttackChargeTurnStart()
    {
        return curAttackChargeTurnStart;
    }



    public void UpdateCurSkills(List<SkillData> skills)
    {
        //curSkills = skills;

        /*
        List<SkillData> newSkills = new List<SkillData>();

        for (int i = 0; i < curSkills.Count; i++)
        {
            if (curSkills[i].originalIndex == 0)
            {
                newSkills.Insert(0, curSkills[i]);
            }
            else if (curSkills[i].originalIndex == 1)
            {
                newSkills.Insert(1, curSkills[i]);
            }
            else if (curSkills[i].originalIndex == 2)
            {
                newSkills.Insert(2, curSkills[i]);
            }
            else if (curSkills[i].originalIndex == 3)
            {
                newSkills.Insert(3, curSkills[i]);
            }
        }

        
         */
        curSkills.Clear();

        curSkills = skills;
    }

    public void ResetCurSkills()
    {
        curSkills.Clear();
    }
   

    public SkillData GetSkill0()
    {
        return curSkills[0];
    }

    public SkillData GetSkill1()
    {
        return curSkills[1];
    }

    public SkillData GetSkill2()
    {
        return curSkills[2];
    }

    public SkillData GetSkill3()
    {
        return curSkills[3];
    }

    public SkillData GetNoCDSkill()
    {
        for (int i = 0; i < curSkills.Count; i++)
        {
            if (curSkills[i].skillCooldown == 0)
                return curSkills[i];
        }

        return null;
    }

    public int GetUnitValue()
    {
        //int rand = Random.Range(unitValue, maxUnitValue - 1);
        return unitValue;
    }

    public List<SkillData> GetUnitSkills()
    {
        return curSkills;
    }
}
