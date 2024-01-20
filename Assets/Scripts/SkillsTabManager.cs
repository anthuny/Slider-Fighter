using UnityEngine;

public class SkillsTabManager : MonoBehaviour
{
    public static SkillsTabManager Instance;

    [SerializeField] private ButtonFunctionality toMapButton;


    [SerializeField] private int skillPointsPerLv = 2;

    public UIElement skillBase1;
    public UIElement skillBase2;
    public UIElement skillBase3;
    public UIElement skillBase4;

    [SerializeField] private UIElement skillDesc;
    [SerializeField] private UIElement skillFillAmount;
    [SerializeField] private UIElement unitLevelText;
    [SerializeField] private UIElement unspentPointsText;
    [SerializeField] private UIElement teamPageAlert;

    [SerializeField] private UnitFunctionality activeUnit;
    [SerializeField] private UIElement selectedSkillBase;
    [SerializeField] private SkillBase activeSkillBase;
    [SerializeField] private UIElement allyNameText;

    public Slot selectedOwnedSlot;
    public Slot selectedBaseSlot;

    public Transform statAllyPosTrans;

    public GameObject statScrollView;

    public bool playerInSkillTab;

    public ButtonFunctionality gearTabArrowLeftButton;
    public ButtonFunctionality gearTabArrowRightButton;

    private int statPageCount;

    private void Awake()
    {
        Instance = this;
    }

    public void ToggleToMapButton(bool toggle)
    {
        toMapButton.ToggleButton(toggle);
    }

    public void TriggerStat(string statType, float power)
    {
        // Basic Stats
        if (statType == "HPBONUS")
        {
            float tempAddedHealth = (power / 100f) * GameManager.Instance.GetActiveUnitFunctionality().GetUnitMaxHealth();
            float newMaxHealth = GameManager.Instance.GetActiveUnitFunctionality().GetUnitMaxHealth() + tempAddedHealth;
            GameManager.Instance.GetActiveUnitFunctionality().UpdateUnitMaxHealth((int)newMaxHealth, true);
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

    public void ToggleMasteryPagebool (bool toggle)
    {
        if (toggle)
        {
            skillBase1.UpdateAlpha(1);
            skillBase2.UpdateAlpha(1);
            skillBase3.UpdateAlpha(1);
            skillBase4.UpdateAlpha(1);
        }
        else
        {
            skillBase1.UpdateAlpha(0);
            skillBase2.UpdateAlpha(0);
            skillBase3.UpdateAlpha(0);
            skillBase4.UpdateAlpha(0);
        }
    }

    public void ResetStatPageCount()
    {
        statPageCount = 0;
    }

    /*
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
            UpdateStatPage();

        //BACKWARD
        // Moving from adv to base
        if (prevStatCount == 0 && statPageCount == -1)
            UpdateStatPage();

        // RETURNING TO STANDARD
        if (statPageCount == 0)
            UpdateStatPage();

        // If looping, go back to original page, resetting
        if (prevStatCount == 1 && statPageCount == 2 || prevStatCount == -1 && statPageCount == -2)
        {
            statPageCount = 0;
            UpdateStatPage();
        }

    }
    */

    public void UpdateStatPage()
    {
        //ToggleToMapButton(true);

        // Active unit level image for team page
        for (int i = 0; i < GameManager.Instance.activeRoomAllies.Count; i++)
        {
            GameManager.Instance.activeRoomAllies[i].ToggleUnitLevelImage(true);
        }

        UnitFunctionality unit = GetActiveUnit();
        UnitData unitData = unit.unitData;

        unit.UpdateSkillBaseStats(unitData.GetStandardStats());
        unit.UpdateCurrentSkillBaseSlots(unitData.GetStandardStats());

        ToggleOwnedSkillSlotsClickable(true);

        SetupSkillsTab(unit);

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

        skillBase1.UpdateStatPoindsAdded(true, true);
        skillBase2.UpdateStatPoindsAdded(true, true);
        skillBase3.UpdateStatPoindsAdded(true, true);
        skillBase4.UpdateStatPoindsAdded(true, true);

        //UnitFunctionality unit = GetActiveUnit();

        UpdateStatPage();
    }

    public void SetupSkillsTab(UnitFunctionality unit)
    {
        UpdateActiveUnit(unit);

        UpdateUnspentPointsText(CalculateUnspentSkillPoints());

        UpdateUnitLevelText(GetActiveUnit().GetUnitLevel().ToString());

        ResetSkillsBaseSelection();
        //UpdateSkillDescription();

        activeSkillBase = unit.GetCurrentSkillBase(0);

        selectedSkillBase = skillBase1;

        skillBase1.UpdateContentImage(unit.GetCurrentSkillBase(0).skillBaseIcon);
        skillBase2.UpdateContentImage(unit.GetCurrentSkillBase(1).skillBaseIcon);
        skillBase3.UpdateContentImage(unit.GetCurrentSkillBase(2).skillBaseIcon);
        skillBase4.UpdateContentImage(unit.GetCurrentSkillBase(3).skillBaseIcon);

        skillBase1.UpdateContentSubText(skillBase1.GetStatPointsAdded() + " / " + unit.GetCurrentSkillBase(0).skillBaseMaxAmount);
        skillBase2.UpdateContentSubText(skillBase2.GetStatPointsAdded() + " / " + unit.GetCurrentSkillBase(1).skillBaseMaxAmount);
        skillBase3.UpdateContentSubText(skillBase3.GetStatPointsAdded() + " / " + unit.GetCurrentSkillBase(2).skillBaseMaxAmount);
        skillBase4.UpdateContentSubText(skillBase4.GetStatPointsAdded() + " / " + unit.GetCurrentSkillBase(3).skillBaseMaxAmount);

        skillBase1.ToggleButton(true);
        skillBase2.ToggleButton(true);
        skillBase3.ToggleButton(true);
        skillBase4.ToggleButton(true);

        skillBase1.UpdateAlpha(1);
        skillBase2.UpdateAlpha(1);
        skillBase3.UpdateAlpha(1);
        skillBase4.UpdateAlpha(1);

        // Select Standard default when setting up
        //UpdateSkillDescription(unit.GetCurrentSkillBase(0));
        //ToggleSkillBaseSelection(skillBase1, true);
        //statsBase1.UpdateContentSubText(statsBase1.GetStatPointsAdded().ToString() + " / " + unit.GetCurrentStat(0).statMaxAmount);

        UpdateUnspentPointsText(CalculateUnspentSkillPoints());
    }

    public SkillBase GetActiveSkillBase()
    {
        if (GetSelectedSkillSlot() == skillBase1)
            return GetActiveUnit().GetCurrentSkillBase(0);
        else if (GetSelectedSkillSlot() == skillBase2)
            return GetActiveUnit().GetCurrentSkillBase(1);
        else if (GetSelectedSkillSlot() == skillBase3)
            return GetActiveUnit().GetCurrentSkillBase(2);
        else if (GetSelectedSkillSlot() == skillBase4)
            return GetActiveUnit().GetCurrentSkillBase(3);
        else
            return null;
    }

    // TODo 
    public void SkillAddPoint()
    {
        if (GetSelectedSkillSlot() == null)
            return;

        if (GetSelectedSkillSlot().GetIsLocked())
            return;

        int maxAmount = GetActiveSkillBase().skillBaseMaxAmount;

        // if mastery points are maxed already, stop
        if (GetSelectedSkillSlot().GetStatPointsAdded() >= maxAmount)
            return;

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        GetSelectedSkillSlot().UpdateStatPoindsAdded(true, false, 0, false);
        GetSelectedSkillSlot().UpdateContentSubText(GetSelectedSkillSlot().GetStatPointsAdded().ToString() + " / " + maxAmount);

        if (GetSelectedSkillSlot().curStatType == UIElement.StatType.SKILLSLOT1)
        {
            TriggerStat("HPBONUS", 1.5f);
        }
        else if (GetSelectedSkillSlot().curStatType == UIElement.StatType.SKILLSLOT2)
        {
            TriggerStat("DMGBONUS", 5f);
        }
        if (GetSelectedSkillSlot().curStatType == UIElement.StatType.SKILLSLOT3)
        {
            TriggerStat("HEALINGBONUS", 5f);
        }
        else if (GetSelectedSkillSlot().curStatType == UIElement.StatType.SKILLSLOT4)
        {
            TriggerStat("SPEEDBONUS", 5f);
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
        if (GetSelectedSkillSlot() == null)
            return;

        int maxAmount = GetActiveSkillBase().skillBaseMaxAmount;

        // if mastery points are at 0 already, stop
        if (GetSelectedSkillSlot().GetStatPointsAdded() <= 0)
            return;

        GetSelectedSkillSlot().UpdateStatPoindsAdded(false);
        GetSelectedSkillSlot().UpdateContentSubText(GetSelectedSkillSlot().GetStatPointsAdded().ToString() + " / " + maxAmount);
        
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

    public void UpdateSelectedSkillBase(ButtonFunctionality statButton)
    {
        if (statButton.curMasteryType == ButtonFunctionality.MasteryType.Skill1)
        {
            selectedSkillBase = skillBase1;
            activeSkillBase = GetActiveUnit().GetSkillBaseSLot(0);
        }
        else if (statButton.curMasteryType == ButtonFunctionality.MasteryType.Skill2)
        {
            selectedSkillBase = skillBase2;
            activeSkillBase = GetActiveUnit().GetSkillBaseSLot(1);
        }
        else if (statButton.curMasteryType == ButtonFunctionality.MasteryType.Skill3)
        {
            selectedSkillBase = skillBase3;
            activeSkillBase = GetActiveUnit().GetSkillBaseSLot(2);
        }
        else if (statButton.curMasteryType == ButtonFunctionality.MasteryType.Skill4)
        {
            selectedSkillBase = skillBase4;
            activeSkillBase = GetActiveUnit().GetSkillBaseSLot(3);
        }
    }

    public UIElement GetSelectedSkillSlot()
    {
        return selectedSkillBase;
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

    public void ResetSkillsBaseSelection()
    {
        //Debug.Log("a");
        skillBase1.ToggleSelected(false);
        skillBase2.ToggleSelected(false);
        skillBase3.ToggleSelected(false);
        skillBase4.ToggleSelected(false);
    }

    public void ToggleSkillBaseSelection(UIElement skill, bool toggle)
    {
        //Debug.Log("b");
        skill.ToggleSelected(toggle);
    }

    public Slot GetSelectedOwnedSlot()
    {
        return selectedBaseSlot;
    }


    public Slot GetSelectedSlot()
    {
        return selectedBaseSlot;
    }

    public void UpdateSelectedBaseSlot(Slot gear)
    {
        selectedBaseSlot = gear;

        gear.ToggleSlotSelection(true);
    }

    public void SkillSelection(Slot slot, bool select = false)
    {
        // Disable all gear selection border
        ResetSkillsBaseSelection();

        // Enable selected gear slot border
        slot.ToggleSlotSelection(true);
        //OwnedLootInven.Instance.FillOwnedGearSlots();

        // Bug todo - 2nd / 3rd ally arent having their gear saved.

        if (slot.curSlotStatis == Slot.SlotStatis.DEFAULT)
        {
            UpdateSelectedBaseSlot(slot);

            //OwnedLootInven.Instance.EnableOwnedItemsSlotSelection(GetSelectedSlot());
        }
        else
        {

            UpdateSelectedBaseSlot(slot);
            //OwnedLootInven.Instance.EnableOwnedItemsSlotSelection(GetSelectedSlot());

            if (!slot.isEmpty && select)
            {
                /*
                GearPiece gearPiece = new GearPiece();

                if (selectedBaseSlot.GetGearOwnedBy() == Slot.SlotOwnedBy.MAIN)
                {
                    if (slot.curGearType == Slot.SlotType.HELMET)
                    {
                        gearPiece.UpdateGearPiece(slot.GetGearName(), slot.GetCurGearType().ToString(), slot.GetRarity().ToString(), slot.GetGearImage(), slot.GetBonusHealth(), slot.GetBonusDamage(), slot.GetBonusHealing(), slot.GetBonusDefense(), slot.GetBonusSpeed());
                        UpdateEquippedGearPiece("helmMain", gearPiece);
                    }
                    else if (slot.curGearType == Slot.SlotType.CHESTPIECE)
                    {
                        gearPiece.UpdateGearPiece(slot.GetGearName(), slot.GetCurGearType().ToString(), slot.GetRarity().ToString(), slot.GetGearImage(), slot.GetBonusHealth(), slot.GetBonusDamage(), slot.GetBonusHealing(), slot.GetBonusDefense(), slot.GetBonusSpeed());
                        UpdateEquippedGearPiece("chestMain", gearPiece);
                    }
                    else if (slot.curGearType == Slot.SlotType.LEGGINGS)
                    {
                        gearPiece.UpdateGearPiece(slot.GetGearName(), slot.GetCurGearType().ToString(), slot.GetRarity().ToString(), slot.GetGearImage(), slot.GetBonusHealth(), slot.GetBonusDamage(), slot.GetBonusHealing(), slot.GetBonusDefense(), slot.GetBonusSpeed());
                        UpdateEquippedGearPiece("legsMain", gearPiece);
                    }
                    else if (slot.curGearType == Slot.SlotType.BOOTS)
                    {
                        gearPiece.UpdateGearPiece(slot.GetGearName(), slot.GetCurGearType().ToString(), slot.GetRarity().ToString(), slot.GetGearImage(), slot.GetBonusHealth(), slot.GetBonusDamage(), slot.GetBonusHealing(), slot.GetBonusDefense(), slot.GetBonusSpeed());
                        UpdateEquippedGearPiece("bootsMain", gearPiece);
                    }
                }
                if (selectedBaseSlot.GetGearOwnedBy() == Slot.SlotOwnedBy.SECOND)
                {
                    if (slot.curGearType == Slot.SlotType.HELMET)
                    {
                        gearPiece.UpdateGearPiece(slot.GetGearName(), slot.GetCurGearType().ToString(), slot.GetRarity().ToString(), slot.GetGearImage(), slot.GetBonusHealth(), slot.GetBonusDamage(), slot.GetBonusHealing(), slot.GetBonusDefense(), slot.GetBonusSpeed());
                        UpdateEquippedGearPiece("helmSecond", gearPiece);
                    }
                    else if (slot.curGearType == Slot.SlotType.CHESTPIECE)
                    {
                        gearPiece.UpdateGearPiece(slot.GetGearName(), slot.GetCurGearType().ToString(), slot.GetRarity().ToString(), slot.GetGearImage(), slot.GetBonusHealth(), slot.GetBonusDamage(), slot.GetBonusHealing(), slot.GetBonusDefense(), slot.GetBonusSpeed());
                        UpdateEquippedGearPiece("chestSecond", gearPiece);
                    }
                    else if (slot.curGearType == Slot.SlotType.LEGGINGS)
                    {
                        gearPiece.UpdateGearPiece(slot.GetGearName(), slot.GetCurGearType().ToString(), slot.GetRarity().ToString(), slot.GetGearImage(), slot.GetBonusHealth(), slot.GetBonusDamage(), slot.GetBonusHealing(), slot.GetBonusDefense(), slot.GetBonusSpeed());
                        UpdateEquippedGearPiece("legsSecond", gearPiece);
                    }
                    else if (slot.curGearType == Slot.SlotType.BOOTS)
                    {
                        gearPiece.UpdateGearPiece(slot.GetGearName(), slot.GetCurGearType().ToString(), slot.GetRarity().ToString(), slot.GetGearImage(), slot.GetBonusHealth(), slot.GetBonusDamage(), slot.GetBonusHealing(), slot.GetBonusDefense(), slot.GetBonusSpeed());
                        UpdateEquippedGearPiece("bootsSecond", gearPiece);
                    }
                }
                if (selectedBaseSlot.GetGearOwnedBy() == Slot.SlotOwnedBy.THIRD)
                {
                    if (slot.curGearType == Slot.SlotType.HELMET)
                    {
                        gearPiece.UpdateGearPiece(slot.GetGearName(), slot.GetCurGearType().ToString(), slot.GetRarity().ToString(), slot.GetGearImage(), slot.GetBonusHealth(), slot.GetBonusDamage(), slot.GetBonusHealing(), slot.GetBonusDefense(), slot.GetBonusSpeed());
                        UpdateEquippedGearPiece("helmThird", gearPiece);
                    }
                    else if (slot.curGearType == Slot.SlotType.CHESTPIECE)
                    {
                        gearPiece.UpdateGearPiece(slot.GetGearName(), slot.GetCurGearType().ToString(), slot.GetRarity().ToString(), slot.GetGearImage(), slot.GetBonusHealth(), slot.GetBonusDamage(), slot.GetBonusHealing(), slot.GetBonusDefense(), slot.GetBonusSpeed());
                        UpdateEquippedGearPiece("chestThird", gearPiece);
                    }
                    else if (slot.curGearType == Slot.SlotType.LEGGINGS)
                    {
                        gearPiece.UpdateGearPiece(slot.GetGearName(), slot.GetCurGearType().ToString(), slot.GetRarity().ToString(), slot.GetGearImage(), slot.GetBonusHealth(), slot.GetBonusDamage(), slot.GetBonusHealing(), slot.GetBonusDefense(), slot.GetBonusSpeed());
                        UpdateEquippedGearPiece("legsThird", gearPiece);
                    }
                    else if (slot.curGearType == Slot.SlotType.BOOTS)
                    {
                        gearPiece.UpdateGearPiece(slot.GetGearName(), slot.GetCurGearType().ToString(), slot.GetRarity().ToString(), slot.GetGearImage(), slot.GetBonusHealth(), slot.GetBonusDamage(), slot.GetBonusHealing(), slot.GetBonusDefense(), slot.GetBonusSpeed());
                        UpdateEquippedGearPiece("bootsThird", gearPiece);
                    }
                }
                */
                // Toggle main gear selection on
                GetSelectedSlot().ToggleSlotSelection(true);

                // Display inven
                //EquipItem(slot);
            }
        }

        // Reference owned item, if the player currently already owns the selected piece of gear.
        //GearPiece newGearPiece = OwnedGearInven.Instance.GetGearPiece(gear);


        // If gear is NOT empty, put gear in it
        if (!slot.isEmpty)
        {
            /*
            // Initialize gear base data
            gear.UpdateGearImage(newGearPiece.gearIcon);
            gear.UpdateGearName(newGearPiece.gearName);
            gear.UpdateGearBonusHealth(newGearPiece.bonusHealth);
            gear.UpdateGearBonusDamage(newGearPiece.bonusDamage);
            gear.UpdateGearBonusHealing(newGearPiece.bonusHealing);
            gear.UpdateGearBonusDefense(newGearPiece.bonusDefense);
            gear.UpdateGearBonusSpeed(newGearPiece.bonusSpeed);

            */
            // Update UI
            //UpdateGearStatDetails();
            //UpdateGearNameText(slot.GetGearName());
        }

        // If gear IS empty, dont put gear in it, display it as empty
        else
        {
            /*
            if (gear.GetCurGearStatis() != Gear.GearStatis.DEFAULT)
            {
                // Initialize gear base data
                gear.ResetGearSlot(true);

            }
            else
            {
                // Initialize gear base data
                //gear.ResetGearSlot(true, true);
            }
            */

            //ClearAllGearStats();
            //UpdateGearNameText("");
        }
    }

    /*
    public void UpdateGearNameText(string name)
    {
        gearNameText.UpdateContentText(name);
    }

    public void UpdateGearStatDetails()
    {
        ClearAllGearStats();

        // Gear Stats Update
        for (int i = 0; i < 5; i++)
        {
            GameObject spawnedStat = Instantiate(gearStatGO, gearStatsUI.transform.position, Quaternion.identity);
            spawnedStat.transform.SetParent(gearStatsUI.transform);
            spawnedStat.transform.localPosition = Vector2.zero;
            spawnedStat.transform.localScale = Vector2.one;

            UIElement statUI = spawnedStat.GetComponent<UIElement>();

            // Update gear stat UI
            if (i == 0)
                statUI.UpdateContentText(GetSelectedGearSlot().GetBonusHealth().ToString());
            else if (i == 1)
                statUI.UpdateContentText(GetSelectedGearSlot().GetBonusDamage().ToString());
            else if (i == 2)
                statUI.UpdateContentText(GetSelectedGearSlot().GetBonusHealing().ToString());
            else if (i == 3)
                statUI.UpdateContentText(GetSelectedGearSlot().GetBonusDefense().ToString());
            else if (i == 4)
                statUI.UpdateContentText(GetSelectedGearSlot().GetBonusSpeed().ToString());

            // Gear Stat Name Update
            gearNameText.UpdateContentText(GetSelectedGearSlot().GetGearName());
        }

        // Gear Stat Description Update


    }
    public void UpdateSkillDescription(SkillBase stat = null)
    {
        if (stat == null)
        {
            skillDesc.UpdateContentText("");
            skillDesc.UpdateContentSubText("");
            //skillFillAmount.UpdateContentSubText("");
            return;
        }

        skillDesc.UpdateContentText(stat.skillBaseName);
        skillDesc.UpdateContentSubText(stat.skillBaseDesc);
        //statFillAmount.UpdateContentSubText(" / " + stat.statMaxAmount.ToString());
    }

    */
    public void UpdateUnspentPointsText(int count)
    {
        unspentPointsText.UpdateContentSubText(count.ToString());
    }

    public int CalculateUnspentSkillPoints()
    {
        int points = (GetActiveUnit().GetUnitLevel() * skillPointsPerLv) - GetActiveUnit().GetSpentSkillPoints();

        int unitsCombinedLevel = 0;
        int spentPoints = 0;

        //  error check with starting with allies 
        if (GameManager.Instance.activeRoomAllies.Count > 0)
        {
            for (int i = 0; i < GameManager.Instance.activeTeam.Count; i++)
            {
                unitsCombinedLevel += GameManager.Instance.activeRoomAllies[i].GetUnitLevel() * skillPointsPerLv;
                spentPoints += GameManager.Instance.activeRoomAllies[i].GetSpentSkillPoints();
            }
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

    public void UpdateUnspentSkillPoints(bool toggle)
    {
        if (toggle)
        {
            GetActiveUnit().UpdateSpentSkillPoints(1);

                UpdateSpentSkillPoints(1);
        }
        else
        {
            GetActiveUnit().UpdateSpentSkillPoints(-1);

                UpdateSpentSkillPoints(-1);
        }

        if (GetSelectedSkillSlot().GetStatPointsAdded() > GetActiveSkillBase().skillBaseMaxAmount)
            GetActiveUnit().UpdateSpentSkillPoints(-1);

        UpdateUnspentPointsText(CalculateUnspentSkillPoints());
    }

    public int GetActiveSkillSpentPoints()
    {
        return GetSpentSkillStatPoints();
    }

    public int GetSpentSkillStatPoints()
    {
        return GetActiveUnit().SkillSpentPoints;
    }

    public void UpdateSpentSkillPoints(int pointsAdded)
    {
        GetActiveUnit().SkillSpentPoints += pointsAdded;
    }

    void ResetSpendStatPoints()
    {
        for (int i = 0; i < GameManager.Instance.activeRoomAllies.Count; i++)
        {
            GameManager.Instance.activeRoomAllies[i].ResetSpentStatPoints();
        }
    }

    public void ToggleOwnedSkillSlotsClickable(bool toggle)
    {
        int count = OwnedLootInven.Instance.ownedLootSlots.Count;

        //Debug.Log("Toggling owned skills clickable " + toggle);

        for (int i = 0; i < count; i++)
        {
            if (toggle)
            {
                //Debug.Log("enabling");
                OwnedLootInven.Instance.ownedLootSlots[i].GetSlotUI().ToggleButton(true);
                OwnedLootInven.Instance.ownedLootSlots[i].ToggleMainSlot(true);
                //OwnedGearInven.Instance.ownedGearSlots[i].ToggleOwnedGearButton(true);

                if (OwnedLootInven.Instance.ownedLootSlots[i].isEmpty)
                    OwnedLootInven.Instance.ownedLootSlots[i].ToggleEquipButton(false);
                else
                    OwnedLootInven.Instance.ownedLootSlots[i].ToggleEquipButton(true);
            }
            else
            {
                //Debug.Log("disabling");
                OwnedLootInven.Instance.ownedLootSlots[i].GetSlotUI().ToggleButton(false);
                OwnedLootInven.Instance.ownedLootSlots[i].ToggleMainSlot(false);
                //OwnedGearInven.Instance.ownedGearSlots[i].ToggleOwnedGearButton(false);
                OwnedLootInven.Instance.ownedLootSlots[i].ToggleEquipButton(false);
            }
        }
    }
}