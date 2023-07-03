using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSetup : MonoBehaviour
{
    public static TeamSetup Instance;

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

    [SerializeField] private UnitFunctionality activeUnit;
    [SerializeField] private UIElement selectedMastery;
    [SerializeField] private Mastery activeMastery;

    private int spentMasteryPoints = 0;

    private void Awake()
    {
        Instance = this;
    }

    public int GetSpendMasteryPoints()
    {
        return spentMasteryPoints;
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
        SetupTeamSetup(unit);
    }

    public void SetupTeamSetup(UnitFunctionality unit)
    {
        UpdateActiveUnit(unit);

        UpdateUnitLevelText(GetActiveUnit().GetUnitLevel().ToString());

        ResetMasterySelection();
        UpdateMasteryDescription();
        UpdateUnspentPointsText(CalculateUnspentPoints());

        masteryL1.UpdateContentImage(unit.GetMastery(0).masteryIcon);
        masteryL2.UpdateContentImage(unit.GetMastery(1).masteryIcon);
        masteryL3.UpdateContentImage(unit.GetMastery(2).masteryIcon);
        masteryL4.UpdateContentImage(unit.GetMastery(3).masteryIcon);

        masteryR1.UpdateContentImage(unit.GetMastery(4).masteryIcon);
        masteryR2.UpdateContentImage(unit.GetMastery(5).masteryIcon);
        masteryR3.UpdateContentImage(unit.GetMastery(6).masteryIcon);
        masteryR4.UpdateContentImage(unit.GetMastery(7).masteryIcon);

        masteryL1.UpdateContentSubText(masteryL1.GetMasteryPointsAdded() + " / " + unit.GetMastery(0).masteryMaxAmount);
        masteryL2.UpdateContentSubText(masteryL2.GetMasteryPointsAdded() + " / " + unit.GetMastery(1).masteryMaxAmount);
        masteryL3.UpdateContentSubText(masteryL3.GetMasteryPointsAdded() + " / " + unit.GetMastery(2).masteryMaxAmount);
        masteryL4.UpdateContentSubText(masteryL4.GetMasteryPointsAdded() + " / " + unit.GetMastery(3).masteryMaxAmount);

        masteryR1.UpdateContentSubText(masteryR1.GetMasteryPointsAdded() + " / " + unit.GetMastery(4).masteryMaxAmount);
        masteryR2.UpdateContentSubText(masteryR2.GetMasteryPointsAdded() + " / " + unit.GetMastery(5).masteryMaxAmount);
        masteryR3.UpdateContentSubText(masteryR3.GetMasteryPointsAdded() + " / " + unit.GetMastery(6).masteryMaxAmount);
        masteryR4.UpdateContentSubText(masteryR4.GetMasteryPointsAdded() + " / " + unit.GetMastery(7).masteryMaxAmount);
        
        // Select L1 when setting up
        UpdateMasteryDescription(unit.GetMastery(0));
        ToggleMasterySelection(masteryL1, true);
        masteryL1.UpdateContentSubText(masteryL1.GetMasteryPointsAdded().ToString() + " / " + unit.GetMastery(0).masteryMaxAmount);

        activeMastery = unit.GetMastery(0);
        selectedMastery = masteryL1;

        // Check if mastery should be locked or not
        masteryL1.CheckIfThreshholdPassed(true);
        masteryL2.CheckIfThreshholdPassed(true);
        masteryL3.CheckIfThreshholdPassed(true);
        masteryL4.CheckIfThreshholdPassed(true);

        masteryR1.CheckIfThreshholdPassed(true);
        masteryR2.CheckIfThreshholdPassed(true);
        masteryR3.CheckIfThreshholdPassed(true);
        masteryR4.CheckIfThreshholdPassed(true);
    }

    void UpdateUnitLevelText(string text)
    {
        unitLevelText.UpdateContentSubText("LV " + text);
    }

    public Mastery GetActiveMastery()
    {
        if (GetSelectedMastery() == masteryL1)
            return GetActiveUnit().GetMastery(0);
        else if (GetSelectedMastery() == masteryL2)
            return GetActiveUnit().GetMastery(1);
        else if (GetSelectedMastery() == masteryL3)
            return GetActiveUnit().GetMastery(2);
        else if (GetSelectedMastery() == masteryL4)
            return GetActiveUnit().GetMastery(3);

        else if (GetSelectedMastery() == masteryR1)
            return GetActiveUnit().GetMastery(4);
        else if (GetSelectedMastery() == masteryR2)
            return GetActiveUnit().GetMastery(5);
        else if (GetSelectedMastery() == masteryR3)
            return GetActiveUnit().GetMastery(6);
        else if (GetSelectedMastery() == masteryR4)
            return GetActiveUnit().GetMastery(7);
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

        GetSelectedMastery().UpdateMasteryPoindsAdded(true);
        GetSelectedMastery().UpdateContentSubText(GetSelectedMastery().GetMasteryPointsAdded().ToString() + " / " + maxAmount);

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
            activeMastery = GetActiveUnit().GetMastery(0);
        }
        else if (masteryButton.curMasteryType == ButtonFunctionality.MasteryType.L2)
        {
            selectedMastery = masteryL2;
            activeMastery = GetActiveUnit().GetMastery(1);
        }
        else if (masteryButton.curMasteryType == ButtonFunctionality.MasteryType.L3)
        {
            selectedMastery = masteryL3;
            activeMastery = GetActiveUnit().GetMastery(2);
        }
        else if (masteryButton.curMasteryType == ButtonFunctionality.MasteryType.L4)
        {
            selectedMastery = masteryL4;
            activeMastery = GetActiveUnit().GetMastery(3);
        }

        else if (masteryButton.curMasteryType == ButtonFunctionality.MasteryType.R1)
        {
            selectedMastery = masteryR1;
            activeMastery = GetActiveUnit().GetMastery(4);
        }
        else if (masteryButton.curMasteryType == ButtonFunctionality.MasteryType.R2)
        {
            selectedMastery = masteryR2;
            activeMastery = GetActiveUnit().GetMastery(5);
        }
        else if (masteryButton.curMasteryType == ButtonFunctionality.MasteryType.R3)
        {
            selectedMastery = masteryR3;
            activeMastery = GetActiveUnit().GetMastery(6);
        }
        else if (masteryButton.curMasteryType == ButtonFunctionality.MasteryType.R4)
        {
            selectedMastery = masteryR4;
            activeMastery = GetActiveUnit().GetMastery(7);
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
            spentMasteryPoints++;
        else
            spentMasteryPoints--;

        if (GetSelectedMastery().GetMasteryPointsAdded() > GetActiveMastery().masteryMaxAmount)
            spentMasteryPoints--;

        UpdateUnspentPointsText(CalculateUnspentPoints());
    }

    int GetSpentMasteryPoints()
    {
        return spentMasteryPoints;
    }

    void ResetSpendMasteryPoints()
    {
        spentMasteryPoints = 0;
    }
}