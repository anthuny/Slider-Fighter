using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
public class Unit : ScriptableObject
{
    public enum UnitType { PLAYER, ENEMY };
    public UnitType curUnitType;

    public Sprite unitSprite;
    public new string name;
    public int startingSpeed;
    public int startingPower;
    public int startingMaxHealth;
    public int startingEnergy;
    public int startingUnitStartTurnEnergyGain;
    // The amount of units this unit can target at once with it's basic attack
    public int basicSelectionCount;

    public Skill basicSkill;
    public Skill skill1;
    public Skill skill2;
    public Skill skill3;

    public Skill GetSkill1()
    {
        return skill1;
    }

    public Skill GetSkill2()
    {
        return skill2;
    }

    public Skill GetSkill3()
    {
        return skill3;
    }
}