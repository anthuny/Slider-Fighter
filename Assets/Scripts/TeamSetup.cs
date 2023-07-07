using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSetup : MonoBehaviour
{
    public static TeamSetup Instance;

    public enum ActiveMasteryType { OFFENSE, DEFENSE, UTILITY };
    public ActiveMasteryType activeMasteryType;

    [SerializeField] private int masterPointsPerLv = 2;

    public UIElement masteryL1;
    public UIElement masteryL2;
    public UIElement masteryL3;
    public UIElement masteryL4;

    public UIElement masteryR1;
    public UIElement masteryR2;
    public UIElement masteryR3;
    public UIElement masteryR4;

    [SerializeField] private UIElement masteryDesc;
    [SerializeField] private UIElement masteryFillAmount;
    [SerializeField] private UIElement unitLevelText;
    [SerializeField] private UIElement unspentPointsText;

    [SerializeField] private UIElement masteryTreeType;
    [SerializeField] private Color offenseTitleColour;
    [SerializeField] private Color defenseTitleColour;
    [SerializeField] private Color utilityTitleColour;

    [SerializeField] private UnitFunctionality activeUnit;
    [SerializeField] private UIElement selectedMastery;
    [SerializeField] private Mastery activeMastery;

    public GameObject masteryScrollView;

    private int spentMasteryTotalPoints = 0;
    private int spentOffenseMasteryPoints = 0;
    private int spentDefenseMasteryPoints = 0;
    private int spentUtilityMasteryPoints = 0;

    private int masteryCount;

    public int masteryOffenseL1AddedCount;
    public int masteryOffenseL2AddedCount;
    public int masteryOffenseL3AddedCount;
    public int masteryOffenseL4AddedCount;

    public int masteryOffenseR1AddedCount;
    public int masteryOffenseR2AddedCount;
    public int masteryOffenseR3AddedCount;
    public int masteryOffenseR4AddedCount;


    public int masteryDefenseL1AddedCount;
    public int masteryDefenseL2AddedCount;
    public int masteryDefenseL3AddedCount;
    public int masteryDefenseL4AddedCount;

    public int masteryDefenseR1AddedCount;
    public int masteryDefenseR2AddedCount;
    public int masteryDefenseR3AddedCount;
    public int masteryDefenseR4AddedCount;


    public int masteryUtilityL1AddedCount;
    public int masteryUtilityL2AddedCount;
    public int masteryUtilityL3AddedCount;
    public int masteryUtilityL4AddedCount;

    public int masteryUtilityR1AddedCount;
    public int masteryUtilityR2AddedCount;
    public int masteryUtilityR3AddedCount;
    public int masteryUtilityR4AddedCount;

    private void Awake()
    {
        Instance = this;
    }

    public void ChangeMasteryType(bool inc)
    {
        int prevMasteryCount = masteryCount;

        if (inc)
        {
            masteryCount++;
        }
        else
        {
            masteryCount--;
        }
        
        // FORWARD
        // Moving from offense to defense
        if (prevMasteryCount == 0 && masteryCount == 1)  // Change from Offense to Defense
            UpdateMasteryPage(ActiveMasteryType.DEFENSE);
        // Moving from defense to utility
        if (prevMasteryCount == 1 && masteryCount == 2)
            UpdateMasteryPage(ActiveMasteryType.UTILITY);
        // Moving from defense back to offense
        if (prevMasteryCount == 1 && masteryCount == 0)
            UpdateMasteryPage(ActiveMasteryType.OFFENSE);

        // Moving from utility to offense
        if (prevMasteryCount == 2 && masteryCount == 3)
            UpdateMasteryPage(ActiveMasteryType.OFFENSE);
        // Moving from utility back to defense
        if (prevMasteryCount == 2 && masteryCount == 1)
            UpdateMasteryPage(ActiveMasteryType.DEFENSE);


        //BACKWARD
        // Moving from offense to utility
        if (prevMasteryCount == 0 && masteryCount == -1)
            UpdateMasteryPage(ActiveMasteryType.UTILITY);

        // moving from utility to defense
        if (prevMasteryCount == -1 && masteryCount == -2)
            UpdateMasteryPage(ActiveMasteryType.DEFENSE);
        // Moving from utility back to offense
        if (prevMasteryCount == -1 && masteryCount == 0)
            UpdateMasteryPage(ActiveMasteryType.OFFENSE);

        // moving from defense to offense
        if (prevMasteryCount == -2 && masteryCount == -3)
            UpdateMasteryPage(ActiveMasteryType.OFFENSE);
        // moving from defense back to utility
        if (prevMasteryCount == -2 && masteryCount == -1)
            UpdateMasteryPage(ActiveMasteryType.UTILITY);

        if (masteryCount == 3 || masteryCount == -3)
            masteryCount = 0;
    }

    public void UpdateMasteryPage(ActiveMasteryType masteryType)
    {
        string masteryTypeName = "";

        if (masteryType == ActiveMasteryType.OFFENSE)
        {
            activeMasteryType = ActiveMasteryType.OFFENSE;
            masteryTypeName = "OFFENSE";
        }
        else if (masteryType == ActiveMasteryType.DEFENSE)
        {
            activeMasteryType = ActiveMasteryType.DEFENSE;
            masteryTypeName = "DEFENSE";
        }
        else if (masteryType == ActiveMasteryType.UTILITY)
        {
            masteryTypeName = "UTILITY";
            activeMasteryType = ActiveMasteryType.UTILITY;
        }

        masteryTreeType.UpdateContentText(masteryTypeName);

        //masteryOffenseL1AddedCount = masteryL1.GetMasteryPointsAdded();

        UnitFunctionality unit = GetActiveUnit();
        UnitData unitData = GameManager.Instance.GetUnitData(0);

        if (masteryType == ActiveMasteryType.OFFENSE)
        {
            //unit.UpdateOffenseMasteries(masteryL1)
            masteryTreeType.UpdateContentTextColour(offenseTitleColour);
            unit.UpdateCurrentMasteries(unitData.GetOffenseMasteries());
            SetupTeamSetup(unit, ActiveMasteryType.OFFENSE);
        }
        else if (masteryType == ActiveMasteryType.DEFENSE)
        {
            masteryTreeType.UpdateContentTextColour(defenseTitleColour);
            unit.UpdateCurrentMasteries(unitData.GetDefenseMasteries());
            SetupTeamSetup(unit, ActiveMasteryType.DEFENSE);
        }
        else if (masteryType == ActiveMasteryType.UTILITY)
        {
            masteryTreeType.UpdateContentTextColour(utilityTitleColour);
            unit.UpdateCurrentMasteries(unitData.GetUtilityMasteries());
            SetupTeamSetup(unit, ActiveMasteryType.UTILITY);
        }
    }

    void UpdateUnitLevelText(string text)
    {
        unitLevelText.UpdateContentSubText("LV " + text);
    }

    public void ResetTeamSetup()
    {
        ResetSpendMasteryPoints();

        masteryL1.UpdateMasteryPoindsAdded(true, true);
        masteryL2.UpdateMasteryPoindsAdded(true, true);
        masteryL3.UpdateMasteryPoindsAdded(true, true);
        masteryL4.UpdateMasteryPoindsAdded(true, true);

        masteryR1.UpdateMasteryPoindsAdded(true, true);
        masteryR2.UpdateMasteryPoindsAdded(true, true);
        masteryR3.UpdateMasteryPoindsAdded(true, true);
        masteryR4.UpdateMasteryPoindsAdded(true, true);

        UnitFunctionality unit = GetActiveUnit();

        UpdateMasteryPage(ActiveMasteryType.OFFENSE);
    }

    public void SetupTeamSetup(UnitFunctionality unit, ActiveMasteryType masteryType)
    {
        UpdateActiveUnit(unit);
        //UpdateMasteryPage(masteryType);

        UpdateUnitLevelText(GetActiveUnit().GetUnitLevel().ToString());

        ResetMasterySelection();
        UpdateMasteryDescription();

        activeMastery = unit.GetCurrentMastery(0);
        selectedMastery = masteryL1;

        if (masteryType == ActiveMasteryType.OFFENSE)
        {
            masteryL1.UpdateMasteryPoindsAdded(true, false, masteryOffenseL1AddedCount, true, "OFFENSE");
            masteryL2.UpdateMasteryPoindsAdded(true, false, masteryOffenseL2AddedCount, true, "OFFENSE");
            masteryL3.UpdateMasteryPoindsAdded(true, false, masteryOffenseL3AddedCount, true, "OFFENSE");
            masteryL4.UpdateMasteryPoindsAdded(true, false, masteryOffenseL4AddedCount, true, "OFFENSE");
            masteryR1.UpdateMasteryPoindsAdded(true, false, masteryOffenseR1AddedCount, true, "OFFENSE");
            masteryR2.UpdateMasteryPoindsAdded(true, false, masteryOffenseR2AddedCount, true, "OFFENSE");
            masteryR3.UpdateMasteryPoindsAdded(true, false, masteryOffenseR3AddedCount, true, "OFFENSE");
            masteryR4.UpdateMasteryPoindsAdded(true, false, masteryOffenseR4AddedCount, true, "OFFENSE");
        }
        else if (masteryType == ActiveMasteryType.DEFENSE)
        {
            masteryL1.UpdateMasteryPoindsAdded(true, false, masteryDefenseL1AddedCount, true, "DEFENSE");
            masteryL2.UpdateMasteryPoindsAdded(true, false, masteryDefenseL2AddedCount, true, "DEFENSE");
            masteryL3.UpdateMasteryPoindsAdded(true, false, masteryDefenseL3AddedCount, true, "DEFENSE");
            masteryL4.UpdateMasteryPoindsAdded(true, false, masteryDefenseL4AddedCount, true, "DEFENSE");
            masteryR1.UpdateMasteryPoindsAdded(true, false, masteryDefenseR1AddedCount, true, "DEFENSE");
            masteryR2.UpdateMasteryPoindsAdded(true, false, masteryDefenseR2AddedCount, true, "DEFENSE");
            masteryR3.UpdateMasteryPoindsAdded(true, false, masteryDefenseR3AddedCount, true, "DEFENSE");
            masteryR4.UpdateMasteryPoindsAdded(true, false, masteryDefenseR4AddedCount, true, "DEFENSE");
        }
        else if (masteryType == ActiveMasteryType.UTILITY)
        {
            masteryL1.UpdateMasteryPoindsAdded(true, false, masteryUtilityL1AddedCount, true, "UTILITY");
            masteryL2.UpdateMasteryPoindsAdded(true, false, masteryUtilityL2AddedCount, true, "UTILITY");
            masteryL3.UpdateMasteryPoindsAdded(true, false, masteryUtilityL3AddedCount, true, "UTILITY");
            masteryL4.UpdateMasteryPoindsAdded(true, false, masteryUtilityL4AddedCount, true, "UTILITY");
            masteryR1.UpdateMasteryPoindsAdded(true, false, masteryUtilityR1AddedCount, true, "UTILITY");
            masteryR2.UpdateMasteryPoindsAdded(true, false, masteryUtilityR2AddedCount, true, "UTILITY");
            masteryR3.UpdateMasteryPoindsAdded(true, false, masteryUtilityR3AddedCount, true, "UTILITY");
            masteryR4.UpdateMasteryPoindsAdded(true, false, masteryUtilityR4AddedCount, true, "UTILITY");
        }


        UpdateUnspentPointsText(CalculateUnspentPoints());
         
        masteryL1.UpdateContentImage(unit.GetCurrentMastery(0).masteryIcon);
        masteryL2.UpdateContentImage(unit.GetCurrentMastery(1).masteryIcon);
        masteryL3.UpdateContentImage(unit.GetCurrentMastery(2).masteryIcon);
        masteryL4.UpdateContentImage(unit.GetCurrentMastery(3).masteryIcon);

        masteryR1.UpdateContentImage(unit.GetCurrentMastery(4).masteryIcon);
        masteryR2.UpdateContentImage(unit.GetCurrentMastery(5).masteryIcon);
        masteryR3.UpdateContentImage(unit.GetCurrentMastery(6).masteryIcon);
        masteryR4.UpdateContentImage(unit.GetCurrentMastery(7).masteryIcon);

        masteryL1.UpdateContentSubText(masteryL1.GetMasteryPointsAdded() + " / " + unit.GetCurrentMastery(0).masteryMaxAmount);
        masteryL2.UpdateContentSubText(masteryL2.GetMasteryPointsAdded() + " / " + unit.GetCurrentMastery(1).masteryMaxAmount);
        masteryL3.UpdateContentSubText(masteryL3.GetMasteryPointsAdded() + " / " + unit.GetCurrentMastery(2).masteryMaxAmount);
        masteryL4.UpdateContentSubText(masteryL4.GetMasteryPointsAdded() + " / " + unit.GetCurrentMastery(3).masteryMaxAmount);

        masteryR1.UpdateContentSubText(masteryR1.GetMasteryPointsAdded() + " / " + unit.GetCurrentMastery(4).masteryMaxAmount);
        masteryR2.UpdateContentSubText(masteryR2.GetMasteryPointsAdded() + " / " + unit.GetCurrentMastery(5).masteryMaxAmount);
        masteryR3.UpdateContentSubText(masteryR3.GetMasteryPointsAdded() + " / " + unit.GetCurrentMastery(6).masteryMaxAmount);
        masteryR4.UpdateContentSubText(masteryR4.GetMasteryPointsAdded() + " / " + unit.GetCurrentMastery(7).masteryMaxAmount);

        // Select L1 when setting up
        UpdateMasteryDescription(unit.GetCurrentMastery(0));
        ToggleMasterySelection(masteryL1, true);
        masteryL1.UpdateContentSubText(masteryL1.GetMasteryPointsAdded().ToString() + " / " + unit.GetCurrentMastery(0).masteryMaxAmount);

        CheckIfMasteryShouldBeLocked();
    }

    public Mastery GetActiveMastery()
    {
        if (GetSelectedMastery() == masteryL1)
            return GetActiveUnit().GetCurrentMastery(0);
        else if (GetSelectedMastery() == masteryL2)
            return GetActiveUnit().GetCurrentMastery(1);
        else if (GetSelectedMastery() == masteryL3)
            return GetActiveUnit().GetCurrentMastery(2);
        else if (GetSelectedMastery() == masteryL4)
            return GetActiveUnit().GetCurrentMastery(3);

        else if (GetSelectedMastery() == masteryR1)
            return GetActiveUnit().GetCurrentMastery(4);
        else if (GetSelectedMastery() == masteryR2)
            return GetActiveUnit().GetCurrentMastery(5);
        else if (GetSelectedMastery() == masteryR3)
            return GetActiveUnit().GetCurrentMastery(6);
        else if (GetSelectedMastery() == masteryR4)
            return GetActiveUnit().GetCurrentMastery(7);
        else
            return null;
    }

    public void MasteryAddPoint()
    {
        if (GetSelectedMastery() == null)
            return;

        if (GetSelectedMastery().GetIsLocked())
            return;

        int maxAmount = GetActiveMastery().masteryMaxAmount;

        // if mastery points are maxed already, stop
        if (GetSelectedMastery().GetMasteryPointsAdded() >= maxAmount)
            return;

        string activeMasteryType2 = "";


        if (activeMasteryType == ActiveMasteryType.OFFENSE)
            activeMasteryType2 = "OFFENSE";
        else if (activeMasteryType == ActiveMasteryType.DEFENSE)
            activeMasteryType2 = "DEFENSE";
        else if (activeMasteryType == ActiveMasteryType.UTILITY)
            activeMasteryType2 = "UTILITY";

        GetSelectedMastery().UpdateMasteryPoindsAdded(true, false, 0, false, activeMasteryType2);
        GetSelectedMastery().UpdateContentSubText(GetSelectedMastery().GetMasteryPointsAdded().ToString() + " / " + maxAmount);

        CheckIfMasteryShouldBeLocked();
    }

    public void CheckIfMasteryShouldBeLocked()
    {
        // Check if mastery should be locked or not
        masteryL1.CheckIfThreshholdPassed(true);
        masteryL2.CheckIfThreshholdPassed(true);
        masteryL3.CheckIfThreshholdPassed(true);
        masteryL4.CheckIfThreshholdPassed(true);

        masteryR1.CheckIfThreshholdPassed(true);
        masteryR2.CheckIfThreshholdPassed(true);
        masteryR3.CheckIfThreshholdPassed(true);
        masteryR4.CheckIfThreshholdPassed(true);

        // Force L2/R2 and L4/R4 to be unique. only of of each can be used
        if (masteryL2.GetMasteryPointsAdded() >= 1)
            masteryR2.UpdateIsLocked(true);
        else if (masteryR2.GetMasteryPointsAdded() >= 1)
            masteryL2.UpdateIsLocked(true);

        if (masteryL4.GetMasteryPointsAdded() >= 1)
            masteryR4.UpdateIsLocked(true);
        else if (masteryR4.GetMasteryPointsAdded() >= 1)
            masteryL4.UpdateIsLocked(true);
    }

    public void MasteryRemovePoint()
    {
        if (GetSelectedMastery() == null)
            return;

        int maxAmount = GetActiveMastery().masteryMaxAmount;

        // if mastery points are at 0 already, stop
        if (GetSelectedMastery().GetMasteryPointsAdded() <= 0)
            return;

        GetSelectedMastery().UpdateMasteryPoindsAdded(false);
        GetSelectedMastery().UpdateContentSubText(GetSelectedMastery().GetMasteryPointsAdded().ToString() + " / " + maxAmount);

        // Force L2/R2 and L4/R4 to be unique. only of of each can be used
        if (masteryL2.GetMasteryPointsAdded() != 0)
        {
            masteryR2.UpdateIsLocked(true);
            masteryL2.UpdateIsLocked(false);
            //masteryL2.CheckIfThreshholdPassed();
        }
        else if (masteryR2.GetMasteryPointsAdded() != 0)
        {
            masteryL2.UpdateIsLocked(true);
            masteryR2.UpdateIsLocked(false);
           // masteryR2.CheckIfThreshholdPassed();
        }

        if (masteryR2.GetMasteryPointsAdded() != 0)
        {
            masteryL2.UpdateIsLocked(true);
            masteryR2.UpdateIsLocked(false);
            //masteryR2.CheckIfThreshholdPassed();
        }
        else if (masteryL2.GetMasteryPointsAdded() != 0)
        {
            masteryR2.UpdateIsLocked(true);
            masteryL2.UpdateIsLocked(false);
            //masteryL2.CheckIfThreshholdPassed();
        }

        if (masteryL4.GetMasteryPointsAdded() != 0)
        {
            masteryR4.UpdateIsLocked(true);
            masteryL4.UpdateIsLocked(false);
        }
        else if (masteryR4.GetMasteryPointsAdded() != 0)
        {
            masteryL4.UpdateIsLocked(true);
            masteryR4.UpdateIsLocked(false);
        }

        if (masteryR4.GetMasteryPointsAdded() != 0)
        {
            masteryL4.UpdateIsLocked(true);
            masteryR4.UpdateIsLocked(false);
        }
        else if (masteryL4.GetMasteryPointsAdded() != 0)
        {
            masteryR4.UpdateIsLocked(true);
            masteryL4.UpdateIsLocked(false);
        }

        masteryL2.CheckIfThreshholdPassed();
        masteryR2.CheckIfThreshholdPassed();
        masteryL3.CheckIfThreshholdPassed();
        masteryR3.CheckIfThreshholdPassed();
        masteryL4.CheckIfThreshholdPassed(true);
        masteryR4.CheckIfThreshholdPassed(true);
    }

    public void UpdateSelectedMastery(ButtonFunctionality masteryButton)
    {
        if (masteryButton.curMasteryType == ButtonFunctionality.MasteryType.L1)
        {
            selectedMastery = masteryL1;
            activeMastery = GetActiveUnit().GetCurrentMastery(0);
        }
        else if (masteryButton.curMasteryType == ButtonFunctionality.MasteryType.L2)
        {
            selectedMastery = masteryL2;
            activeMastery = GetActiveUnit().GetCurrentMastery(1);
        }
        else if (masteryButton.curMasteryType == ButtonFunctionality.MasteryType.L3)
        {
            selectedMastery = masteryL3;
            activeMastery = GetActiveUnit().GetCurrentMastery(2);
        }
        else if (masteryButton.curMasteryType == ButtonFunctionality.MasteryType.L4)
        {
            selectedMastery = masteryL4;
            activeMastery = GetActiveUnit().GetCurrentMastery(3);
        }

        else if (masteryButton.curMasteryType == ButtonFunctionality.MasteryType.R1)
        {
            selectedMastery = masteryR1;
            activeMastery = GetActiveUnit().GetCurrentMastery(4);
        }
        else if (masteryButton.curMasteryType == ButtonFunctionality.MasteryType.R2)
        {
            selectedMastery = masteryR2;
            activeMastery = GetActiveUnit().GetCurrentMastery(5);
        }
        else if (masteryButton.curMasteryType == ButtonFunctionality.MasteryType.R3)
        {
            selectedMastery = masteryR3;
            activeMastery = GetActiveUnit().GetCurrentMastery(6);
        }
        else if (masteryButton.curMasteryType == ButtonFunctionality.MasteryType.R4)
        {
            selectedMastery = masteryR4;
            activeMastery = GetActiveUnit().GetCurrentMastery(7);
        }
    }

    public UIElement GetSelectedMastery()
    {
        return selectedMastery;
    }
    public void UpdateActiveUnit(UnitFunctionality unit)
    {
        activeUnit = unit;
    }

    UnitFunctionality GetActiveUnit()
    {
        return activeUnit;
    }

    public void ResetMasterySelection()
    {
        masteryL1.ToggleSelected(false);
        masteryL2.ToggleSelected(false);
        masteryL3.ToggleSelected(false);
        masteryL4.ToggleSelected(false);

        masteryR1.ToggleSelected(false);
        masteryR2.ToggleSelected(false);
        masteryR3.ToggleSelected(false);
        masteryR4.ToggleSelected(false);
    }

    public void ToggleMasterySelection(UIElement mastery, bool toggle)
    {
        mastery.ToggleSelected(toggle);
    }

    public void UpdateMasteryDescription(Mastery mastery = null)
    {
        if (mastery == null)
        {
            masteryDesc.UpdateContentText("");
            masteryDesc.UpdateContentSubText("");
            masteryFillAmount.UpdateContentSubText("");
            return;
        }

        masteryDesc.UpdateContentText(mastery.masteryName);
        masteryDesc.UpdateContentSubText(mastery.masteryDesc);
        masteryFillAmount.UpdateContentSubText(" / " + mastery.masteryMaxAmount.ToString());
    }

    public void UpdateUnspentPointsText(int count)
    {
        unspentPointsText.UpdateContentSubText(count.ToString());
    }

    public int CalculateUnspentPoints()
    {
        int points = (GetActiveUnit().GetUnitLevel() * masterPointsPerLv) - GetSpentMasteryPoints();
        return points;
    }

    public void UpdateUnspentMasteryPoints(bool toggle)
    {
        if (toggle)
        {
            spentMasteryTotalPoints++;

            if (activeMasteryType == ActiveMasteryType.OFFENSE)
                UpdateSpentOffenseMasteryPoints(1);
            else if (activeMasteryType == ActiveMasteryType.DEFENSE)
                UpdateSpentDefenseMasteryPoints(1);
            else if (activeMasteryType == ActiveMasteryType.UTILITY)
                UpdateSpentUtilityMasteryPoints(1);
        }

        else
        {
            spentMasteryTotalPoints--;

            if (activeMasteryType == ActiveMasteryType.OFFENSE)
                UpdateSpentOffenseMasteryPoints(-1);
            else if (activeMasteryType == ActiveMasteryType.DEFENSE)
                UpdateSpentDefenseMasteryPoints(-1);
            else if (activeMasteryType == ActiveMasteryType.UTILITY)
                UpdateSpentUtilityMasteryPoints(-1);
        }

        if (GetSelectedMastery().GetMasteryPointsAdded() > GetActiveMastery().masteryMaxAmount)
            spentMasteryTotalPoints--;

        UpdateUnspentPointsText(CalculateUnspentPoints());
    }

    public int GetSpentMasteryPoints()
    {
        return spentMasteryTotalPoints;
    }

    public int GetActiveMasteryTypeSpentPoints()
    {
        if (activeMasteryType == ActiveMasteryType.OFFENSE)
            return GetSpentOffenseMasteryPoints();
        else if (activeMasteryType == ActiveMasteryType.DEFENSE)
            return GetSpentDefenseMasteryPoints();
        else
            return GetSpentUtilityMasteryPoints();
    }

    public int GetSpentOffenseMasteryPoints()
    {
        return spentOffenseMasteryPoints;
    }

    public int GetSpentDefenseMasteryPoints()
    {
        return spentDefenseMasteryPoints;
    }

    public int GetSpentUtilityMasteryPoints()
    {
        return spentUtilityMasteryPoints;
    }

    public void UpdateSpentOffenseMasteryPoints(int pointsAdded)
    {
        spentOffenseMasteryPoints += pointsAdded;
    }

    public void UpdateSpentDefenseMasteryPoints(int pointsAdded)
    {
        spentDefenseMasteryPoints += pointsAdded;
    }

    public void UpdateSpentUtilityMasteryPoints(int pointsAdded)
    {
        spentUtilityMasteryPoints += pointsAdded;
    }

    void ResetSpendMasteryPoints()
    {
        spentMasteryTotalPoints = 0;
        spentOffenseMasteryPoints = 0;
        spentDefenseMasteryPoints = 0;
        spentUtilityMasteryPoints = 0;
    }
}