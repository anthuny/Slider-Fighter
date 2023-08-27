using UnityEngine;

public class TeamSetup : MonoBehaviour
{
    public static TeamSetup Instance;

    public enum ActiveStatType { STANDARD, ADVANCED };
    public ActiveStatType activeStatType;

    [SerializeField] private int masterPointsPerLv = 2;

    public UIElement statsBase1;
    public UIElement statsBase2;
    public UIElement statsBase3;
    public UIElement statsBase4;
    public UIElement statsBase5;

    public UIElement statsAdvanced1;
    public UIElement statsAdvanced2;
    public UIElement statsAdvanced3;
    public UIElement statsAdvanced4;

    [SerializeField] private UIElement statDesc;
    [SerializeField] private UIElement statFillAmount;
    [SerializeField] private UIElement unitLevelText;
    [SerializeField] private UIElement unspentPointsText;

    [SerializeField] private UIElement statTreeType;
    [SerializeField] private Color offenseTitleColour;
    [SerializeField] private Color defenseTitleColour;
    [SerializeField] private Color utilityTitleColour;

    [SerializeField] private UnitFunctionality activeUnit;
    [SerializeField] private UIElement selectedStat;
    [SerializeField] private Stat activeStat;
    [SerializeField] private UIElement allyNameText;

    public Transform statAllyPosTrans;

    public GameObject statScrollView;

    private int statPageCount;

    private void Awake()
    {
        Instance = this;
    }

    public void ChangeStatPage(bool inc)
    {     
        int prevStatCount = statPageCount;

        if (inc)
            statPageCount++;
        else
            statPageCount--;
        
        // FORWARD
        // Moving from base to adv
        if (prevStatCount == 0 && statPageCount == 1)  // Change from Offense to Defense
            UpdateStatPage(ActiveStatType.ADVANCED);

        //BACKWARD
        // Moving from adv to base
        if (prevStatCount == 0 && statPageCount == -1)
            UpdateStatPage(ActiveStatType.ADVANCED);

        // RETURNING TO MIDDLE
        if (statPageCount == 0)
            UpdateStatPage(ActiveStatType.STANDARD);

        // If looping, go back to original page, resetting
        if (prevStatCount == 1 && statPageCount == 2 || prevStatCount == -1 && statPageCount == -2)
        {
            statPageCount = 0;
            UpdateStatPage(ActiveStatType.STANDARD);
        }
    }

    public void UpdateStatPage(ActiveStatType statType)
    {
        string statPageTypeName = "";

        if (statType == ActiveStatType.STANDARD)
        {
            activeStatType = ActiveStatType.STANDARD;
            statPageTypeName = "STANDARD";
        }
        else if (statType == ActiveStatType.ADVANCED)
        {
            activeStatType = ActiveStatType.ADVANCED;
            statPageTypeName = "ADVANCED";
        }

        statTreeType.UpdateContentText(statPageTypeName);

        UnitFunctionality unit = GetActiveUnit();
        UnitData unitData = unit.unitData;

        if (statType == ActiveStatType.STANDARD)
        {
            //unit.UpdateOffenseMasteries(masteryL1)
            statTreeType.UpdateContentTextColour(offenseTitleColour);
            unit.UpdateCurrentMasteries(unitData.GetStandardStats());
            SetupTeamSetup(unit, ActiveStatType.STANDARD);
        }
        else if (statType == ActiveStatType.ADVANCED)
        {
            statTreeType.UpdateContentTextColour(defenseTitleColour);
            unit.UpdateCurrentMasteries(unitData.GetAdvancedStats());
            SetupTeamSetup(unit, ActiveStatType.ADVANCED);
        }
    }

    public void UpdateAllyNameText()
    {
        allyNameText.UpdateContentText(GetActiveUnit().GetUnitName());
        allyNameText.UpdateContentTextColour(GetActiveUnit().GetUnitColour());
    }

    void UpdateUnitLevelText(string text)
    {
        unitLevelText.UpdateContentSubText("LV " + text);
    }

    public void ResetTeamSetup()
    {
        ResetSpendStatPoints();

        statsBase1.UpdateStatPoindsAdded(true, true);
        statsBase2.UpdateStatPoindsAdded(true, true);
        statsBase3.UpdateStatPoindsAdded(true, true);
        statsBase4.UpdateStatPoindsAdded(true, true);
        statsBase5.UpdateStatPoindsAdded(true, true);

        statsAdvanced1.UpdateStatPoindsAdded(true, true);
        statsAdvanced2.UpdateStatPoindsAdded(true, true);
        statsAdvanced3.UpdateStatPoindsAdded(true, true);
        statsAdvanced4.UpdateStatPoindsAdded(true, true);

        //UnitFunctionality unit = GetActiveUnit();

        UpdateStatPage(ActiveStatType.STANDARD);
    }

    public void SetupTeamSetup(UnitFunctionality unit, ActiveStatType statType)
    {
        UpdateActiveUnit(unit);
        // active unit face right

        //UpdateMasteryPage(masteryType);

        UpdateUnitLevelText(GetActiveUnit().GetUnitLevel().ToString());

        ResetStatSelection();
        UpdateStatDescription();

        activeStat = unit.GetCurrentStat(0);
        selectedStat = statsBase1;

        if (statType == ActiveStatType.STANDARD)
        {
            statsBase1.UpdateStatPoindsAdded(true, false, unit.statsBase1Added, true, "STANDARD");
            statsBase2.UpdateStatPoindsAdded(true, false, unit.statsBase2Added, true, "STANDARD");
            statsBase3.UpdateStatPoindsAdded(true, false, unit.statsBase3Added, true, "STANDARD");
            statsBase4.UpdateStatPoindsAdded(true, false, unit.statsBase4Added, true, "STANDARD");
            statsBase5.UpdateStatPoindsAdded(true, false, unit.statsBase5Added, true, "STANDARD");

            statsBase1.UpdateContentImage(unit.GetCurrentStat(0).statIcon);
            statsBase2.UpdateContentImage(unit.GetCurrentStat(1).statIcon);
            statsBase3.UpdateContentImage(unit.GetCurrentStat(2).statIcon);
            statsBase4.UpdateContentImage(unit.GetCurrentStat(3).statIcon);
            statsBase5.UpdateContentImage(unit.GetCurrentStat(4).statIcon);

            statsBase1.UpdateContentSubText(statsBase1.GetStatPointsAdded() + " / " + unit.GetCurrentStat(0).statMaxAmount);
            statsBase2.UpdateContentSubText(statsBase2.GetStatPointsAdded() + " / " + unit.GetCurrentStat(1).statMaxAmount);
            statsBase3.UpdateContentSubText(statsBase3.GetStatPointsAdded() + " / " + unit.GetCurrentStat(2).statMaxAmount);
            statsBase4.UpdateContentSubText(statsBase4.GetStatPointsAdded() + " / " + unit.GetCurrentStat(3).statMaxAmount);
            statsBase5.UpdateContentSubText(statsBase4.GetStatPointsAdded() + " / " + unit.GetCurrentStat(4).statMaxAmount);

            //statsAdvanced1.UpdateMasteryPoindsAdded(true, false, unit.statsAdv1Added, true, "STANDARD");
            //statsAdvanced2.UpdateMasteryPoindsAdded(true, false, unit.statsAdv2Added, true, "STANDARD");
            //statsAdvanced3.UpdateMasteryPoindsAdded(true, false, unit.statsAdv3Added, true, "STANDARD");
            //statsAdvanced4.UpdateMasteryPoindsAdded(true, false, unit.statsAdv4Added, true, "STANDARD");
        }
        else if (statType == ActiveStatType.ADVANCED)
        {
            statsAdvanced1.UpdateStatPoindsAdded(true, false, unit.statsAdv1Added, true, "ADVANCED");
            statsAdvanced2.UpdateStatPoindsAdded(true, false, unit.statsAdv2Added, true, "ADVANCED");
            statsAdvanced3.UpdateStatPoindsAdded(true, false, unit.statsAdv3Added, true, "ADVANCED");
            statsAdvanced4.UpdateStatPoindsAdded(true, false, unit.statsAdv4Added, true, "ADVANCED");

            statsAdvanced1.UpdateContentImage(unit.GetCurrentStat(5).statIcon);
            statsAdvanced1.UpdateContentImage(unit.GetCurrentStat(6).statIcon);
            statsAdvanced1.UpdateContentImage(unit.GetCurrentStat(7).statIcon);
            statsAdvanced1.UpdateContentImage(unit.GetCurrentStat(8).statIcon);

            statsAdvanced1.UpdateContentSubText(statsAdvanced1.GetStatPointsAdded() + " / " + unit.GetCurrentStat(4).statMaxAmount);
            statsAdvanced2.UpdateContentSubText(statsAdvanced1.GetStatPointsAdded() + " / " + unit.GetCurrentStat(4).statMaxAmount);
            statsAdvanced3.UpdateContentSubText(statsAdvanced1.GetStatPointsAdded() + " / " + unit.GetCurrentStat(4).statMaxAmount);
            statsAdvanced4.UpdateContentSubText(statsAdvanced1.GetStatPointsAdded() + " / " + unit.GetCurrentStat(4).statMaxAmount);

            //statsBase1.UpdateMasteryPoindsAdded(true, false, unit.statsBase1Added, true, "STANDARD");
            //statsBase2.UpdateMasteryPoindsAdded(true, false, unit.statsBase2Added, true, "STANDARD");
            //statsBase3.UpdateMasteryPoindsAdded(true, false, unit.statsBase3Added, true, "STANDARD");
            //statsBase4.UpdateMasteryPoindsAdded(true, false, unit.statsBase4Added, true, "STANDARD");
            //statsBase5.UpdateMasteryPoindsAdded(true, false, unit.statsBase5Added, true, "STANDARD");
        }

        UpdateUnspentPointsText(CalculateUnspentStatPoints());
         
        // Select L1 when setting up
        UpdateStatDescription(unit.GetCurrentStat(0));
        ToggleStatSelection(statsBase1, true);
        statsBase1.UpdateContentSubText(statsBase1.GetStatPointsAdded().ToString() + " / " + unit.GetCurrentStat(0).statMaxAmount);

        CheckIfStatShouldBeLocked();
    }

    public Stat GetActiveStat()
    {
        if (GetSelectedStat() == statsBase1)
            return GetActiveUnit().GetCurrentStat(0);
        else if (GetSelectedStat() == statsBase2)
            return GetActiveUnit().GetCurrentStat(1);
        else if (GetSelectedStat() == statsBase3)
            return GetActiveUnit().GetCurrentStat(2);
        else if (GetSelectedStat() == statsBase4)
            return GetActiveUnit().GetCurrentStat(3);
        else if (GetSelectedStat() == statsBase5)
            return GetActiveUnit().GetCurrentStat(4);

        else if (GetSelectedStat() == statsAdvanced1)
            return GetActiveUnit().GetCurrentStat(4);
        else if (GetSelectedStat() == statsAdvanced1)
            return GetActiveUnit().GetCurrentStat(5);
        else if (GetSelectedStat() == statsAdvanced1)
            return GetActiveUnit().GetCurrentStat(6);
        else if (GetSelectedStat() == statsAdvanced1)
            return GetActiveUnit().GetCurrentStat(7);
        else
            return null;
    }

    public void MasteryAddPoint()
    {
        if (GetSelectedStat() == null)
            return;

        if (GetSelectedStat().GetIsLocked())
            return;

        int maxAmount = GetActiveStat().statMaxAmount;

        // if mastery points are maxed already, stop
        if (GetSelectedStat().GetStatPointsAdded() >= maxAmount)
            return;

        string activeMasteryType2 = "";

        if (activeStatType == ActiveStatType.STANDARD)
            activeMasteryType2 = "STANDARD";
        else if (activeStatType == ActiveStatType.ADVANCED)
            activeMasteryType2 = "ADVANCED";

        GetSelectedStat().UpdateStatPoindsAdded(true, false, 0, false, activeMasteryType2);
        GetSelectedStat().UpdateContentSubText(GetSelectedStat().GetStatPointsAdded().ToString() + " / " + maxAmount);

        CheckIfStatShouldBeLocked();
    }

    public void CheckIfStatShouldBeLocked()
    {
        
        // Check if mastery should be locked or not
        /*
        statsBase1.CheckIfThreshholdPassed(true);
        statsBase2.CheckIfThreshholdPassed(true);
        statsBase3.CheckIfThreshholdPassed(true);
        statsBase4.CheckIfThreshholdPassed(true);
        statsBase5.CheckIfThreshholdPassed(true);
        */

        statsAdvanced1.CheckIfThreshholdPassed(true);
        statsAdvanced2.CheckIfThreshholdPassed(true);
        statsAdvanced3.CheckIfThreshholdPassed(true);
        statsAdvanced4.CheckIfThreshholdPassed(true);

        /*
        // Force L2/R2 and L4/R4 to be unique. only of of each can be used
        if (statsBase2.GetMasteryPointsAdded() >= 1)
            masteryR2.UpdateIsLocked(true);
        else if (masteryR2.GetMasteryPointsAdded() >= 1)
            statsBase2.UpdateIsLocked(true);

        if (statsBase4.GetMasteryPointsAdded() >= 1)
            masteryR4.UpdateIsLocked(true);
        else if (masteryR4.GetMasteryPointsAdded() >= 1)
            statsBase4.UpdateIsLocked(true);
        */
    }

    public void StatRemovePoint()
    {
        if (GetSelectedStat() == null)
            return;

        int maxAmount = GetActiveStat().statMaxAmount;

        // if mastery points are at 0 already, stop
        if (GetSelectedStat().GetStatPointsAdded() <= 0)
            return;

        GetSelectedStat().UpdateStatPoindsAdded(false);
        GetSelectedStat().UpdateContentSubText(GetSelectedStat().GetStatPointsAdded().ToString() + " / " + maxAmount);
        
        /*
        // Force L2/R2 and L4/R4 to be unique. only of of each can be used
        if (statsBase2.GetMasteryPointsAdded() != 0)
        {
            masteryR2.UpdateIsLocked(true);
            statsBase2.UpdateIsLocked(false);
            //masteryL2.CheckIfThreshholdPassed();
        }
        else if (masteryR2.GetMasteryPointsAdded() != 0)
        {
            statsBase2.UpdateIsLocked(true);
            masteryR2.UpdateIsLocked(false);
           // masteryR2.CheckIfThreshholdPassed();
        }

        if (masteryR2.GetMasteryPointsAdded() != 0)
        {
            statsBase2.UpdateIsLocked(true);
            masteryR2.UpdateIsLocked(false);
            //masteryR2.CheckIfThreshholdPassed();
        }
        else if (statsBase2.GetMasteryPointsAdded() != 0)
        {
            masteryR2.UpdateIsLocked(true);
            statsBase2.UpdateIsLocked(false);
            //masteryL2.CheckIfThreshholdPassed();
        }

        if (statsBase4.GetMasteryPointsAdded() != 0)
        {
            masteryR4.UpdateIsLocked(true);
            statsBase4.UpdateIsLocked(false);
        }
        else if (masteryR4.GetMasteryPointsAdded() != 0)
        {
            statsBase4.UpdateIsLocked(true);
            masteryR4.UpdateIsLocked(false);
        }

        if (masteryR4.GetMasteryPointsAdded() != 0)
        {
            statsBase4.UpdateIsLocked(true);
            masteryR4.UpdateIsLocked(false);
        }
        else if (statsBase4.GetMasteryPointsAdded() != 0)
        {
            masteryR4.UpdateIsLocked(true);
            statsBase4.UpdateIsLocked(false);
        }

        statsBase2.CheckIfThreshholdPassed();
        masteryR2.CheckIfThreshholdPassed();
        statsBase3.CheckIfThreshholdPassed();
        masteryR3.CheckIfThreshholdPassed();
        statsBase4.CheckIfThreshholdPassed(true);
        masteryR4.CheckIfThreshholdPassed(true);

        */
    }

    public void UpdateSelectedStat(ButtonFunctionality statButton)
    {
        if (statButton.curMasteryType == ButtonFunctionality.MasteryType.STATSTANDARD1)
        {
            selectedStat = statsBase1;
            activeStat = GetActiveUnit().GetCurrentStat(0);
        }
        else if (statButton.curMasteryType == ButtonFunctionality.MasteryType.STATSTANDARD2)
        {
            selectedStat = statsBase2;
            activeStat = GetActiveUnit().GetCurrentStat(1);
        }
        else if (statButton.curMasteryType == ButtonFunctionality.MasteryType.STATSTANDARD3)
        {
            selectedStat = statsBase3;
            activeStat = GetActiveUnit().GetCurrentStat(2);
        }
        else if (statButton.curMasteryType == ButtonFunctionality.MasteryType.STATSTANDARD4)
        {
            selectedStat = statsBase4;
            activeStat = GetActiveUnit().GetCurrentStat(3);
        }
        else if (statButton.curMasteryType == ButtonFunctionality.MasteryType.STATSTANDARD5)
        {
            selectedStat = statsBase5;
            activeStat = GetActiveUnit().GetCurrentStat(4);
        }

        else if (statButton.curMasteryType == ButtonFunctionality.MasteryType.STATADVANCED1)
        {
            selectedStat = statsAdvanced1;
            activeStat = GetActiveUnit().GetCurrentStat(5);
        }
        else if (statButton.curMasteryType == ButtonFunctionality.MasteryType.STATADVANCED2)
        {
            selectedStat = statsAdvanced2;
            activeStat = GetActiveUnit().GetCurrentStat(6);
        }
        else if (statButton.curMasteryType == ButtonFunctionality.MasteryType.STATADVANCED3)
        {
            selectedStat = statsAdvanced3;
            activeStat = GetActiveUnit().GetCurrentStat(7);
        }
        else if (statButton.curMasteryType == ButtonFunctionality.MasteryType.STATADVANCED4)
        {
            selectedStat = statsAdvanced4;
            activeStat = GetActiveUnit().GetCurrentStat(8);
        }
    }

    public UIElement GetSelectedStat()
    {
        return selectedStat;
    }
    public void UpdateActiveUnit(UnitFunctionality unit)
    {
        activeUnit = unit;
    }

    public UnitFunctionality GetActiveUnit()
    {
        return activeUnit;
    }

    public void ResetStatSelection()
    {
        statsBase1.ToggleSelected(false);
        statsBase2.ToggleSelected(false);
        statsBase3.ToggleSelected(false);
        statsBase4.ToggleSelected(false);
        statsBase5.ToggleSelected(false);

        statsAdvanced1.ToggleSelected(false);
        statsAdvanced2.ToggleSelected(false);
        statsAdvanced3.ToggleSelected(false);
        statsAdvanced4.ToggleSelected(false);
    }

    public void ToggleStatSelection(UIElement mastery, bool toggle)
    {
        mastery.ToggleSelected(toggle);
    }

    public void UpdateStatDescription(Stat stat = null)
    {
        if (stat == null)
        {
            statDesc.UpdateContentText("");
            statDesc.UpdateContentSubText("");
            statFillAmount.UpdateContentSubText("");
            return;
        }

        statDesc.UpdateContentText(stat.statName);
        statDesc.UpdateContentSubText(stat.statDesc);
        statFillAmount.UpdateContentSubText(" / " + stat.statMaxAmount.ToString());
    }

    public void UpdateUnspentPointsText(int count)
    {
        unspentPointsText.UpdateContentSubText(count.ToString());
    }

    public int CalculateUnspentStatPoints()
    {
        int points = (GetActiveUnit().GetUnitLevel() * masterPointsPerLv) - GetActiveUnit().GetSpentMasteryPoints();
        return points;
    }

    public void UpdateUnspentStatPoints(bool toggle)
    {
        if (toggle)
        {
            GetActiveUnit().UpdateSpentMasteryPoints(1);

            if (activeStatType == ActiveStatType.STANDARD)
                UpdateSpentStandardStatPoints(1);
            else if (activeStatType == ActiveStatType.ADVANCED)
                UpdateSpentAdvancedStatPoints(1);
        }
        else
        {
            GetActiveUnit().UpdateSpentMasteryPoints(-1);

            if (activeStatType == ActiveStatType.STANDARD)
                UpdateSpentStandardStatPoints(-1);
            else if (activeStatType == ActiveStatType.ADVANCED)
                UpdateSpentAdvancedStatPoints(-1);
        }

        if (GetSelectedStat().GetStatPointsAdded() > GetActiveStat().statMaxAmount)
            GetActiveUnit().UpdateSpentMasteryPoints(-1);

        UpdateUnspentPointsText(CalculateUnspentStatPoints());
    }

    public int GetActiveStatTypeSpentPoints()
    {
        if (activeStatType == ActiveStatType.STANDARD)
            return GetSpentStandardStatPoints();
        else
            return GetSpentAdvancedStatPoints();
    }

    public int GetSpentStandardStatPoints()
    {
        return GetActiveUnit().statsSpentBasePoints;
    }

    public int GetSpentAdvancedStatPoints()
    {
        return GetActiveUnit().statsSpentAdvPoints;
    }

    public void UpdateSpentStandardStatPoints(int pointsAdded)
    {
        GetActiveUnit().statsSpentBasePoints += pointsAdded;
    }

    public void UpdateSpentAdvancedStatPoints(int pointsAdded)
    {
        GetActiveUnit().statsSpentAdvPoints += pointsAdded;
    }


    void ResetSpendStatPoints()
    {
        for (int i = 0; i < GameManager.Instance.activeRoomAllies.Count; i++)
        {
            GameManager.Instance.activeRoomAllies[i].ResetSpentStatPoints();
        }
    }
}