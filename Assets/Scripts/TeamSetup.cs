using UnityEngine;

public class TeamSetup : MonoBehaviour
{
    public static TeamSetup Instance;

    public enum ActiveStatType { STANDARD, ADVANCED };
    public ActiveStatType activeStatType;

    [SerializeField] private int masteryPointsPerLv = 2;

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
    [SerializeField] private UIElement teamPageAlert;

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

    public void TriggerStat(string statType, float power)
    {
        // Basic Stats
        if (statType == "HPBONUS")
        {
            float tempAddedHealth = ((power*100) / 100f) * GameManager.Instance.GetActiveUnitFunctionality().GetUnitMaxHealth();
            float newMaxHealth = GameManager.Instance.GetActiveUnitFunctionality().GetUnitMaxHealth() + tempAddedHealth;
            GameManager.Instance.GetActiveUnitFunctionality().UpdateUnitMaxHealth((int)newMaxHealth);
        }
        else if (statType == "DMGBONUS")
        {
            GetActiveUnit().UpdatePowerIncPerLv((int)power);
        }
        else if (statType == "HEALINGBONUS")
        {
            GetActiveUnit().UpdateHealingPowerIncPerLv((int)power);
        }
        else if (statType == "DEFENSEBONUS")
        {
            GetActiveUnit().UpdateDefenseIncPerLv((int)power);
        }
        else if (statType == "SPEEDBONUS")
        {
            GetActiveUnit().UpdateSpeedIncPerLv((int)power);
        }


        /*
        // Advanced Stats
        else if (statType == "DMGLINEBONUS")
        {
            GetActiveUnit().IncDamageLineBonus();
        }
        else if (statType == "HEALINGLINEBONUS")
        {
            GetActiveUnit().IncHealingLineBonus();
        }
        else if (statType == "COOLDOWNREDUCBONUS")
        {
            GetActiveUnit().IncCooldownReducBonus();
        }
        else if (statType == "ITEMPROCBONUS")
        {
            GetActiveUnit().RerollItemCount();
        }
        */
    }

    public void ToggleMasteryPage(bool standard, bool toggle)
    {
        if (standard)
        {
            if (toggle)
            {
                statsBase1.UpdateAlpha(1);
                statsBase2.UpdateAlpha(1);
                statsBase3.UpdateAlpha(1);
                statsBase4.UpdateAlpha(1);
                statsBase5.UpdateAlpha(1);
            }
            else
            {
                statsBase1.UpdateAlpha(0);
                statsBase2.UpdateAlpha(0);
                statsBase3.UpdateAlpha(0);
                statsBase4.UpdateAlpha(0);
                statsBase5.UpdateAlpha(0);
            }
        }
        else
        {
            if (toggle)
            {
                statsAdvanced1.UpdateAlpha(1);
                statsAdvanced2.UpdateAlpha(1);
                statsAdvanced3.UpdateAlpha(1);
                statsAdvanced4.UpdateAlpha(1);
            }
            else
            {
                statsAdvanced1.UpdateAlpha(0);
                statsAdvanced2.UpdateAlpha(0);
                statsAdvanced3.UpdateAlpha(0);
                statsAdvanced4.UpdateAlpha(0);
            }
        }
    }

    public void ResetStatPageCount()
    {
        statPageCount = 0;
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

        // RETURNING TO STANDARD
        if (statPageCount == 0)
            UpdateStatPage(ActiveStatType.STANDARD);

        // If looping, go back to original page, resetting
        if (prevStatCount == 1 && statPageCount == 2 || prevStatCount == -1 && statPageCount == -2)
        {
            statPageCount = 0;
            UpdateStatPage(ActiveStatType.STANDARD);
        }

        if (activeStatType == ActiveStatType.STANDARD)
        {
            // Toggle all advanced masteries OFF when standard page is ON
            ToggleMasteryPage(false, false);

            ToggleMasteryPage(true, true);
        }
        else
        {
            // Toggle all standard masteries OFF when advanced page is ON
            ToggleMasteryPage(true, false);

            ToggleMasteryPage(false, true);
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

            unit.UpdateStandardMasteries(unitData.GetStandardStats());
            unit.UpdateCurrentMasteries(unitData.GetStandardStats());

            SetupTeamSetup(unit, ActiveStatType.STANDARD);
        }
        else if (statType == ActiveStatType.ADVANCED)
        {
            statTreeType.UpdateContentTextColour(defenseTitleColour);

            unit.UpdateAdvancedMasteries(unitData.GetAdvancedStats());
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

        UpdateUnspentPointsText(CalculateUnspentStatPoints());

        UpdateUnitLevelText(GetActiveUnit().GetUnitLevel().ToString());

        ResetStatSelection();
        UpdateStatDescription();

        activeStat = unit.GetCurrentStat(0);

        if (statType == ActiveStatType.STANDARD)
        {
            selectedStat = statsBase1;

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

            statsBase1.ToggleButton(true);
            statsBase2.ToggleButton(true);
            statsBase3.ToggleButton(true);
            statsBase4.ToggleButton(true);
            statsBase5.ToggleButton(true);

            statsAdvanced1.ToggleButton(false);
            statsAdvanced2.ToggleButton(false);
            statsAdvanced3.ToggleButton(false);
            statsAdvanced4.ToggleButton(false);

            statsAdvanced1.UpdateAlpha(0);
            statsAdvanced2.UpdateAlpha(0);
            statsAdvanced3.UpdateAlpha(0);
            statsAdvanced4.UpdateAlpha(0);

            statsBase1.UpdateAlpha(1);
            statsBase2.UpdateAlpha(1);
            statsBase3.UpdateAlpha(1);
            statsBase4.UpdateAlpha(1);
            statsBase5.UpdateAlpha(1);

            // Select Standard default when setting up
            UpdateStatDescription(unit.GetCurrentStat(0));
            ToggleStatSelection(statsBase1, true);
            //statsBase1.UpdateContentSubText(statsBase1.GetStatPointsAdded().ToString() + " / " + unit.GetCurrentStat(0).statMaxAmount);

        }
        else if (statType == ActiveStatType.ADVANCED)
        {
            selectedStat = statsAdvanced1;

            statsAdvanced1.UpdateStatPoindsAdded(true, false, unit.statsAdv1Added, true, "ADVANCED");
            statsAdvanced2.UpdateStatPoindsAdded(true, false, unit.statsAdv2Added, true, "ADVANCED");
            statsAdvanced3.UpdateStatPoindsAdded(true, false, unit.statsAdv3Added, true, "ADVANCED");
            statsAdvanced4.UpdateStatPoindsAdded(true, false, unit.statsAdv4Added, true, "ADVANCED");

            statsAdvanced1.UpdateContentImage(unit.GetCurrentStat(0).statIcon);
            statsAdvanced2.UpdateContentImage(unit.GetCurrentStat(1).statIcon);
            statsAdvanced3.UpdateContentImage(unit.GetCurrentStat(2).statIcon);
            statsAdvanced4.UpdateContentImage(unit.GetCurrentStat(3).statIcon);

            statsAdvanced1.UpdateContentSubText(statsAdvanced1.GetStatPointsAdded() + " / " + unit.GetCurrentStat(0).statMaxAmount);
            statsAdvanced2.UpdateContentSubText(statsAdvanced2.GetStatPointsAdded() + " / " + unit.GetCurrentStat(1).statMaxAmount);
            statsAdvanced3.UpdateContentSubText(statsAdvanced3.GetStatPointsAdded() + " / " + unit.GetCurrentStat(2).statMaxAmount);
            statsAdvanced4.UpdateContentSubText(statsAdvanced4.GetStatPointsAdded() + " / " + unit.GetCurrentStat(3).statMaxAmount);

            statsAdvanced1.ToggleButton(true);
            statsAdvanced2.ToggleButton(true);
            statsAdvanced3.ToggleButton(true);
            statsAdvanced4.ToggleButton(true);

            statsBase1.ToggleButton(false);
            statsBase2.ToggleButton(false);
            statsBase3.ToggleButton(false);
            statsBase4.ToggleButton(false);
            statsBase5.ToggleButton(false);

            statsBase1.UpdateAlpha(0);
            statsBase2.UpdateAlpha(0);
            statsBase3.UpdateAlpha(0);
            statsBase4.UpdateAlpha(0);
            statsBase5.UpdateAlpha(0);

            statsAdvanced1.UpdateAlpha(1);
            statsAdvanced2.UpdateAlpha(1);
            statsAdvanced3.UpdateAlpha(1);
            statsAdvanced4.UpdateAlpha(1);

            // Select Advanced default when setting up
            UpdateStatDescription(unit.GetCurrentStat(0));
            ToggleStatSelection(statsAdvanced1, true);
            //statsAdvanced1.UpdateContentSubText(statsAdvanced1.GetStatPointsAdded().ToString() + " / " + unit.GetCurrentStat(0).statMaxAmount);
        }

        UpdateUnspentPointsText(CalculateUnspentStatPoints());
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
            return GetActiveUnit().GetCurrentStat(0);
        else if (GetSelectedStat() == statsAdvanced2)
            return GetActiveUnit().GetCurrentStat(1);
        else if (GetSelectedStat() == statsAdvanced3)
            return GetActiveUnit().GetCurrentStat(2);
        else if (GetSelectedStat() == statsAdvanced4)
            return GetActiveUnit().GetCurrentStat(3);
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

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        string activeMasteryType2 = "";

        if (activeStatType == ActiveStatType.STANDARD)
            activeMasteryType2 = "STANDARD";
        else if (activeStatType == ActiveStatType.ADVANCED)
            activeMasteryType2 = "ADVANCED";

        GetSelectedStat().UpdateStatPoindsAdded(true, false, 0, false, activeMasteryType2);
        GetSelectedStat().UpdateContentSubText(GetSelectedStat().GetStatPointsAdded().ToString() + " / " + maxAmount);

        if (GetSelectedStat().curStatType == UIElement.StatType.STATSTANDARD1)
        {
            TriggerStat("HPBONUS", 0.05f);
        }
        else if (GetSelectedStat().curStatType == UIElement.StatType.STATSTANDARD2)
        {
            TriggerStat("DMGBONUS", 0.05f);
        }
        if (GetSelectedStat().curStatType == UIElement.StatType.STATSTANDARD3)
        {
            TriggerStat("HEALINGBONUS", 0.05f);
        }
        else if (GetSelectedStat().curStatType == UIElement.StatType.STATSTANDARD4)
        {
            TriggerStat("SPEEDBONUS", 0.05f);
        }
        else if (GetSelectedStat().curStatType == UIElement.StatType.STATSTANDARD5)
        {
            TriggerStat("DEFENSEBONUS", 0.5f);
        }

        else if (GetSelectedStat().curStatType == UIElement.StatType.STATADVANCED1)
        {
            TriggerStat("DMGLINEBONUS", 1f);
        }
        else if (GetSelectedStat().curStatType == UIElement.StatType.STATADVANCED2)
        {
            TriggerStat("HEALINGLINEBONUS", 1F);
        }
        else if (GetSelectedStat().curStatType == UIElement.StatType.STATADVANCED3)
        {
            TriggerStat("COOLDOWNREDUCBONUS", 1f);
        }
        else if (GetSelectedStat().curStatType == UIElement.StatType.STATADVANCED4)
        {
            TriggerStat("ITEMPROCBONUS", 1f);
        }

        //CheckIfStatShouldBeLocked();
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
            activeStat = GetActiveUnit().GetStandardMastery(0);
        }
        else if (statButton.curMasteryType == ButtonFunctionality.MasteryType.STATSTANDARD2)
        {
            selectedStat = statsBase2;
            activeStat = GetActiveUnit().GetStandardMastery(1);
        }
        else if (statButton.curMasteryType == ButtonFunctionality.MasteryType.STATSTANDARD3)
        {
            selectedStat = statsBase3;
            activeStat = GetActiveUnit().GetStandardMastery(2);
        }
        else if (statButton.curMasteryType == ButtonFunctionality.MasteryType.STATSTANDARD4)
        {
            selectedStat = statsBase4;
            activeStat = GetActiveUnit().GetStandardMastery(3);
        }
        else if (statButton.curMasteryType == ButtonFunctionality.MasteryType.STATSTANDARD5)
        {
            selectedStat = statsBase5;
            activeStat = GetActiveUnit().GetStandardMastery(4);
        }

        else if (statButton.curMasteryType == ButtonFunctionality.MasteryType.STATADVANCED1)
        {
            selectedStat = statsAdvanced1;
            activeStat = GetActiveUnit().GetAdvancedMastery(0);
        }
        else if (statButton.curMasteryType == ButtonFunctionality.MasteryType.STATADVANCED2)
        {
            selectedStat = statsAdvanced2;
            activeStat = GetActiveUnit().GetAdvancedMastery(1);
        }
        else if (statButton.curMasteryType == ButtonFunctionality.MasteryType.STATADVANCED3)
        {
            selectedStat = statsAdvanced3;
            activeStat = GetActiveUnit().GetAdvancedMastery(2);
        }
        else if (statButton.curMasteryType == ButtonFunctionality.MasteryType.STATADVANCED4)
        {
            selectedStat = statsAdvanced4;
            activeStat = GetActiveUnit().GetAdvancedMastery(3);
        }
    }

    public UIElement GetSelectedStat()
    {
        return selectedStat;
    }
    public void UpdateActiveUnit(UnitFunctionality unit)
    {
        // Disable unit level image in team setup tab
        unit.ToggleUnitLevelImage(false);

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
        //statFillAmount.UpdateContentSubText(" / " + stat.statMaxAmount.ToString());
    }

    public void UpdateUnspentPointsText(int count)
    {
        unspentPointsText.UpdateContentSubText(count.ToString());


    }

    public int CalculateUnspentStatPoints()
    {
        int points = (GetActiveUnit().GetUnitLevel() * masteryPointsPerLv) - GetActiveUnit().GetSpentMasteryPoints();

        int unitsCombinedLevel = 0;
        int spentPoints = 0;

        for (int i = 0; i < GameManager.Instance.activeTeam.Count; i++)
        {
            unitsCombinedLevel += GameManager.Instance.activeRoomAllies[i].GetUnitLevel() * masteryPointsPerLv;
            spentPoints += GameManager.Instance.activeRoomAllies[i].GetSpentMasteryPoints();
        }

        // Display alert if points remain
        if (unitsCombinedLevel > spentPoints)
        {
            teamPageAlert.UpdateAlpha(1);
        }
        // Hide alerrt if no points remain
        else
            teamPageAlert.UpdateAlpha(0);

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