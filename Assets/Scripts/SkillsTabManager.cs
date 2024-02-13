using System.Collections;
using System.Collections.Generic;
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

    public UIElement holdingButton;

    public UIElement skillDescUI;
    public UIElement powerStatUI;
    public UIElement hitsRemainingStatUI;
    public UIElement baseHitsStatsUI;
    public UIElement cdStatUI;
    public UIElement maxTargetsStatUI;
    public UIElement hitEffectChanceUI;
    [SerializeField] private UIElement skillNameText;
    [SerializeField] private UIElement gearStatsUI;
    [SerializeField] private GameObject gearStatGO;
    [SerializeField] private UIElement unitLevelText;
    [SerializeField] private UIElement unspentPointsText;
    [SerializeField] private UIElement teamPageAlert;

    [SerializeField] private UnitFunctionality activeUnit;
    public UIElement selectedSkillBase;
    public SkillData activeSkillBase;
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

    private void Start()
    {
        UpdateActiveSkillNameText("");
        UpdateSkillDescription("");
    }

    public void UpdateProgressSlider(SkillData skill)
    {
        selectedSkillBase.gameObject.GetComponent<Slot>().UpdateProgressSlider(skill, false);
    }

    public void SkillPointAdd(int skillUpgradeType = 0, bool doProgressSlider = true)
    {
        // Check to disable buttons if no points remain for the ally  

        // Increase skill point
        if (skillUpgradeType == 0)
        {
            // cap 
            if (activeSkillBase.upgradeIncTargetCount > 4)
                return;

            if (doProgressSlider)
            {
                // Button Click SFX
                AudioManager.Instance.Play("Button_Click");

                selectedSkillBase.gameObject.GetComponent<Slot>().UpdateProgressSlider(activeSkillBase, true);

                activeSkillBase.curSkillLevel++;
            }
        }
        else if (skillUpgradeType == 1)
        {
            activeSkillBase.upgradeIncPowerCount++;
            activeSkillBase.curSkillLevel++;
        }
        else if (skillUpgradeType == 2)
        {
            activeSkillBase.upgradeIncEffectCount++;
            activeSkillBase.curSkillLevel++;
        }

        if (skillUpgradeType == 0)
        {
            if (doProgressSlider)
            {
                selectedSkillBase.UpdateSkillLevelText(activeSkillBase.curSkillLevel);

                UpdateSkillUpgradesText();

                SkillAddPoint();

                UpdateSkillStatDetails();
                return;
            }
        }
        else
        {
            selectedSkillBase.UpdateSkillLevelText(activeSkillBase.curSkillLevel);

            UpdateSkillUpgradesText();

            SkillAddPoint();

            UpdateSkillStatDetails();
        }


    }

    public void ResetAllySkllls(UnitFunctionality unit)
    {
        List<SkillData> skills = unit.GetAllSkills();

        for (int x = 0; x < skills.Count; x++)
        {
            skills[x].ResetSkill();
        }
    }


    public void ToggleToMapButton(bool toggle)
    {
        toMapButton.ToggleButton(toggle);
    }

    public void UpdateActiveSkillNameText(string name)
    {
        skillNameText.UpdateContentText(name);
    }

    public void UnequipSkill()
    {    
        if (GetSelectedSkillSlot() == null)
            return;


        for (int i = 0; i < GameManager.Instance.activeRoomAllUnitFunctionalitys.Count; i++)
        {
            if (i == 0)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (activeSkillBase.skillName == GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetSkill(x).skillName)
                    {
                        selectedBaseSlot.ResetGearSlot(true, false);
                        selectedBaseSlot.UpdateSlotImage(TeamGearManager.Instance.clearSlotSprite);
                        selectedBaseSlot.UpdateCurSlotType(Slot.SlotType.EMPTY);
                    }
                }

            }
        }
    }

    public void UpdateSkillUpgradesText()
    {
        selectedSkillBase.skillUpgradesUI.UpdateContentText(activeSkillBase.upgradeIncTargetCount.ToString());
        selectedSkillBase.skillUpgradesUI.UpdateContentText2(activeSkillBase.GetCalculatedSkillPower().ToString());
        selectedSkillBase.skillUpgradesUI.UpdateContentText3(activeSkillBase.GetCalculatedSkillEffectChance().ToString() + "%");

        //Debug.Log(activeSkillBase.GetCalculatedSkillSelectionCount().ToString());
    }

    void ToggleAllStatsVisual(bool toggle)
    {
        if (toggle)
        {
            powerStatUI.UpdateAlpha(1);
            hitsRemainingStatUI.UpdateAlpha(1);
            baseHitsStatsUI.UpdateAlpha(1);
            cdStatUI.UpdateAlpha(1);
            maxTargetsStatUI.UpdateAlpha(1);
            hitEffectChanceUI.UpdateAlpha(1);
        }
        else
        {
            powerStatUI.UpdateAlpha(0);
            hitsRemainingStatUI.UpdateAlpha(0);
            baseHitsStatsUI.UpdateAlpha(0);
            cdStatUI.UpdateAlpha(0);
            maxTargetsStatUI.UpdateAlpha(0);
            hitEffectChanceUI.UpdateAlpha(0);
        }
    }

    void UpdateSkillStatsDisplay(int power = 0, int hitsRemainingMax = 0, int baseHits = 0, int cd = 0, int maxTargets = 0, int hitEffectChance = 0)
    {
        ToggleAllStatsVisual(true);

        powerStatUI.UpdateContentText(power.ToString());
        hitsRemainingStatUI.UpdateContentText(hitsRemainingMax.ToString());
        baseHitsStatsUI.UpdateContentText(baseHits.ToString());
        cdStatUI.UpdateContentText(cd.ToString());
        maxTargetsStatUI.UpdateContentText(maxTargets.ToString());
        hitEffectChanceUI.UpdateContentText(hitEffectChance.ToString());

        //UpdateSkillStatsDisplay(activeSkillBase.GetCalculatedSkillPowerStat(), activeSkillBase.skillAttackCount, activeSkillBase.skillCooldown);
    }

    void ClearSkillStatsDisplay()
    {
        powerStatUI.UpdateContentText("");
        hitsRemainingStatUI.UpdateContentText("");
        baseHitsStatsUI.UpdateContentText("");
        cdStatUI.UpdateContentText("");
        maxTargetsStatUI.UpdateContentText("");
        hitEffectChanceUI.UpdateContentText("");

        ToggleAllStatsVisual(false);
    }

    public void UpdateSkillStatDetails()
    {
        if (activeSkillBase != null)
        {
            //Debug.Log(activeSkillBase);
            UpdateActiveSkillNameText(activeSkillBase.skillName);
            UpdateSkillDescription(activeSkillBase.skillTabDescr);
            UpdateSkillStatsDisplay(activeSkillBase.GetCalculatedSkillPowerStat(), activeSkillBase.skillHitAttempts, activeSkillBase.skillBaseHitOutput + activeSkillBase.upgradeIncPowerCount, activeSkillBase.skillCooldown, activeSkillBase.GetCalculatedSkillSelectionCount(), Mathf.RoundToInt(activeSkillBase.GetCalculatedSkillEffectStat()));
        }
        else
        {
            //Debug.Log("active skill base missing");
            UpdateActiveSkillNameText("");
            UpdateSkillDescription("");
            ClearSkillStatsDisplay();
        }
    }

    public void UpdateSkillStatDetailsSpecific(SkillData skill)
    {
        if (skill != null)
        {
            //Debug.Log(activeSkillBase);
            UpdateActiveSkillNameText(skill.skillName);
            UpdateSkillDescription(skill.skillTabDescr);
            UpdateSkillStatsDisplay(skill.GetCalculatedSkillPowerStat(), skill.skillHitAttempts, skill.skillBaseHitOutput + skill.upgradeIncPowerCount, skill.skillCooldown, skill.GetCalculatedSkillSelectionCount(), Mathf.RoundToInt(skill.GetCalculatedSkillEffectStat()));
        }
        else
        {
            //Debug.Log("active skill base missing");
            UpdateActiveSkillNameText("");
            UpdateSkillDescription("");
            ClearSkillStatsDisplay();
        }
    }


    public void UpdateSkillDescription(string desc)
    {
        skillDescUI.UpdateContentText(desc);
    }

    public Slot SelectedOwnedSlot()
    {
        return selectedOwnedSlot;
    }

    public void UpdateSelectedOwnedSlot(Slot gear)
    {
        selectedOwnedSlot = gear;
    }

    public void TriggerStat(string statType, float power)
    {
        // Basic Stats
        if (statType == "HPBONUS")
        {
            float tempAddedHealth = (power / 100f) * GameManager.Instance.GetActiveAlly().GetUnitMaxHealth();
            float newMaxHealth = GameManager.Instance.GetActiveAlly().GetUnitMaxHealth() + tempAddedHealth;
            GameManager.Instance.GetActiveAlly().UpdateUnitMaxHealth((int)newMaxHealth, true);
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
        //Debug.Log("4");
        unit.UpdateUnitSkills(unitData.GetUnitSkills());
        unit.UpdateCurrentSkills(unitData.GetUnitSkills());

        //unit.GetSkillBaseSlot(0).se

        ToggleOwnedSkillSlotsClickable(true);

        SetupSkillsTab(unit);

        if (GameManager.Instance.activeRoomAllies.Count == 1)
            GameManager.Instance.GetActiveAlly().SetPositionAndParent(GameManager.Instance.allySpawnPositions[0]);
    }

    // Ensure there is only 1 of each skill, skills added, have 
    public void AdjustActiveSkills(UnitFunctionality unit, SkillData skillAdded, SkillData skillRemoved)
    {
        List<SkillData> skills = new List<SkillData>();

        skills = unit.GetAllSkills();

        int skillAddedIndex = 0;
        int skillRemovedIndex = 0;

        if (skills.Contains(skillAdded))
        {
            skillRemovedIndex = skills.IndexOf(skillRemoved);
            skillAddedIndex = skills.IndexOf(skillAdded);
            skills[skillAddedIndex] = skillRemoved;
            skills[skillRemovedIndex] = skillAdded;

            //Debug.Log("Added index " + skillAddedIndex);
            //Debug.Log("Removed index " + skillRemovedIndex);
            //Debug.Log("5");
            //unit.UpdateUnitSkills(skills);
        }
        else
        {
            Debug.Log("Failed to find skill " + skillAdded.skillName);
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

        /*
        skillBase1.UpdateStatPoindsAdded(true, true);
        skillBase2.UpdateStatPoindsAdded(true, true);
        skillBase3.UpdateStatPoindsAdded(true, true);
        skillBase4.UpdateStatPoindsAdded(true, true);
        */

        //UnitFunctionality unit = GetActiveUnit();

        UpdateStatPage();
    }

    public void SetupSkillsTab(UnitFunctionality unit, bool updateSkillUpgrades = true)
    {
        UpdateActiveUnit(unit);

        //selectedSkillBase = skillBase1;
        if (updateSkillUpgrades)
            UpdateUnspentPointsText(CalculateUnspentSkillPoints());

        UpdateUnitLevelText(GetActiveUnit().GetUnitLevel().ToString());

        //ResetSkillsBaseSelection();
        //UpdateSkillDescription();


        //activeSkillBase = unit.GetCurrentSkillBase(0);

        //ToggleSkillBaseSelection(skillBase1, true);

        UpdateSkillStatDetails();
        
        OwnedLootInven.Instance.ToggleOwnedGearDisplay(false);

        skillBase1.UpdateContentImage(unit.GetSkill(0).skillSprite);
        skillBase2.UpdateContentImage(unit.GetSkill(1).skillSprite);
        skillBase3.UpdateContentImage(unit.GetSkill(2).skillSprite);
        skillBase4.UpdateContentImage(unit.GetSkill(3).skillSprite);

        //skillBase1.UpdateContentImage(unit.GetSkillBaseSLot)

        skillBase1.GetSkillLevelText().UpdateContentText(GetActiveUnit().GetSkill(0).curSkillLevel.ToString());
        skillBase2.GetSkillLevelText().UpdateContentText(GetActiveUnit().GetSkill(1).curSkillLevel.ToString());
        skillBase3.GetSkillLevelText().UpdateContentText(GetActiveUnit().GetSkill(2).curSkillLevel.ToString());
        skillBase4.GetSkillLevelText().UpdateContentText(GetActiveUnit().GetSkill(3).curSkillLevel.ToString());

        skillBase1.ToggleButton(true);
        skillBase2.ToggleButton(true);
        skillBase3.ToggleButton(true);
        skillBase4.ToggleButton(true);

        skillBase1.UpdateAlpha(1);
        skillBase2.UpdateAlpha(1);
        skillBase3.UpdateAlpha(1);
        skillBase4.UpdateAlpha(1);

        UpdateLockedSkills();

        // Select Standard default when setting up
        //UpdateSkillDescription(unit.GetCurrentSkillBase(0));
        //ToggleSkillBaseSelection(skillBase1, true);
        //statsBase1.UpdateContentSubText(statsBase1.GetStatPointsAdded().ToString() + " / " + unit.GetCurrentStat(0).statMaxAmount);

        if (updateSkillUpgrades)
            UpdateUnspentPointsText(CalculateUnspentSkillPoints());

        for (int i = 0; i < GameManager.Instance.activeRoomAllies.Count; i++)
        {
            GameManager.Instance.activeRoomAllies[i].ToggleUnitDisplay(true);
        }

    }

    public void UpdateLockedSkills()
    {
        skillBase1.ToggleLockedSkill();
        skillBase2.ToggleLockedSkill();
        skillBase3.ToggleLockedSkill();
        skillBase4.ToggleLockedSkill();
    }

    public SkillData GetActiveSkillBase()
    {
        if (GetSelectedSkillSlot() == skillBase1)
            return GetActiveUnit().GetSkill(0);
        else if (GetSelectedSkillSlot() == skillBase2)
            return GetActiveUnit().GetSkill(1);
        else if (GetSelectedSkillSlot() == skillBase3)
            return GetActiveUnit().GetSkill(2);
        else if (GetSelectedSkillSlot() == skillBase4)
            return GetActiveUnit().GetSkill(3);
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


        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        GetSelectedSkillSlot().UpdateStatPoindsAdded(true, false, 0, false);

    }

    public void StatRemovePoint()
    {
        if (GetSelectedSkillSlot() == null)
            return;

        //int maxAmount = GetActiveSkillBase().maxSkillLevel;

        // if mastery points are at 0 already, stop
        if (GetSelectedSkillSlot().GetStatPointsAdded() <= 0)
            return;

        GetSelectedSkillSlot().UpdateStatPoindsAdded(false);
    }

    public void UpdateSelectedSkillBase(ButtonFunctionality statButton = null)
    {
        if (statButton == null)
        {
            //if (GameManager.Instance.activeTeam.Count == 1)
            //    return;
            Debug.Log("force selecting skill 1");
            selectedSkillBase = skillBase1;

            //if (GetActiveUnit().GetSkillBaseSlot(0) == null)
            //    return;

            activeSkillBase = GetActiveUnit().GetSkill(0);
        }
        else
        {
            Debug.Log("selecting skill updating base  " + statButton.curMasteryType);

            if (statButton.curMasteryType == ButtonFunctionality.MasteryType.Skill1)
            {
                selectedSkillBase = skillBase1;
                activeSkillBase = GetActiveUnit().GetSkill(0);
            }
            else if (statButton.curMasteryType == ButtonFunctionality.MasteryType.Skill2)
            {
                selectedSkillBase = skillBase2;
                activeSkillBase = GetActiveUnit().GetSkill(1);
            }
            else if (statButton.curMasteryType == ButtonFunctionality.MasteryType.Skill3)
            {
                selectedSkillBase = skillBase3;
                activeSkillBase = GetActiveUnit().GetSkill(2);
            }
            else if (statButton.curMasteryType == ButtonFunctionality.MasteryType.Skill4)
            {
                selectedSkillBase = skillBase4;
                activeSkillBase = GetActiveUnit().GetSkill(3);
            }

            UpdateSelectedOwnedSlot(statButton.slot);

            // Set skill
            statButton.slot.skill = activeSkillBase;
        }

        UpdateSkillStatDetails();
        UpdateSkillUpgradesText();
    }

    public UIElement GetSelectedSkillSlot()
    {
        return selectedSkillBase;
    }
    public void UpdateActiveUnit(UnitFunctionality unit)
    {
        // Disable unit level image in team setup tab
        //unit.ToggleUnitLevelImage(false);

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

        ResetBaseSkillUpgradesDisplay();
    }

    public void ToggleSkillBaseSelection(UIElement skill, bool toggle)
    {
        //Debug.Log("b");
        skill.ToggleSelected(toggle);
    }

    public Slot GetSelectedBaseSlot()
    {
        return selectedBaseSlot;
    }


    public Slot GetSelectedSlot()
    {
        return selectedBaseSlot;
    }

    public void UpdateSelectedBaseSlot(Slot slot)
    {
        Debug.Log("setting slot " + slot.GetSlotName());
        ResetSkillsBaseSelection();

        selectedBaseSlot = slot;

        ResetBaseSkillUpgradesDisplay();

        slot.ToggleSlotSelection(true);

        if (slot.GetComponent<UIElement>().skillUpgradesUI != null)
            slot.GetComponent<UIElement>().skillUpgradesUI.UpdateAlpha(1);

    }

    public void ToggleSelectedSlotDetailsButton(bool toggle = true)
    {
        Debug.Log("Toggling sign " + toggle);

        skillBase1.slot.ToggleEquipButton(false);
        skillBase2.slot.ToggleEquipButton(false);
        skillBase3.slot.ToggleEquipButton(false);
        skillBase4.slot.ToggleEquipButton(false);

        if (selectedBaseSlot != null)
            selectedBaseSlot.ToggleEquipButton(toggle);
    }

    void ResetBaseSkillUpgradesDisplay()
    {
        skillBase1.skillUpgradesUI.UpdateAlpha(0);
        skillBase2.skillUpgradesUI.UpdateAlpha(0);
        skillBase3.skillUpgradesUI.UpdateAlpha(0);
        skillBase4.skillUpgradesUI.UpdateAlpha(0);
    }

    public void SkillSelection(Slot slot, bool select = false)
    {
        // Disable all gear selection border

        if (OwnedLootInven.Instance.GetOwnedLootOpened())
            ResetSkillsBaseSelection();

        // Enable selected gear slot border
        slot.ToggleSlotSelection(true);



        UpdateProgressSlider(slot.skill);
        UpdateSkillStatDetails();
        if (slot.curSlotStatis == Slot.SlotStatis.DEFAULT)
        {
            UpdateSelectedBaseSlot(slot);
            AdjustActiveSkills(GetActiveUnit(), slot.skill, activeSkillBase);
            UpdateSkillStatDetails();
        }
        else
        {
            UpdateSelectedOwnedSlot(slot);

            if (select)
            {
                UpdateSelectedBaseSlot(selectedBaseSlot);
                //UpdateSelectedBaseSlot(slot);

                AdjustActiveSkills(GetActiveUnit(), slot.skill, activeSkillBase);
                SetupSkillsTab(GetActiveUnit(), false);
                UpdateSkillStatDetails();
            }
            else
            {
                OwnedLootInven.Instance.ResetOwnedSlotEquipButton();
                OwnedLootInven.Instance.ownedLootSlots[OwnedLootInven.Instance.ownedLootSlots.IndexOf(slot)].ToggleEquipButton(true);
                UpdateSkillStatDetailsSpecific(slot.skill);
            }


            //if (select)
            //OwnedLootInven.Instance.ToggleOwnedGearDisplay(false, "", false);

            // Toggle main gear selection on
            if (!slot.isEmpty)
            {
                UpdateSelectedOwnedSlot(slot);
                GetSelectedSlot().ToggleSlotSelection(true);
            }
        }

        ToggleSelectedSlotDetailsButton(true);
    }

    public void UpdateUnspentPointsText(int count)
    {
        if (count == 0)
        {
            unspentPointsText.UpdateContentText("");

            // Disable add point buttons and visuals
            if (selectedSkillBase != null)
                selectedSkillBase.gameObject.GetComponent<Slot>().ToggleSkillUpgradeButtons(false);
        }
        else
        {
            if (selectedSkillBase != null)
                selectedSkillBase.gameObject.GetComponent<Slot>().ToggleSkillUpgradeButtons(true);
            unspentPointsText.UpdateContentText(count.ToString());
        }
    }

    public int CalculateUnspentSkillPoints()
    {
        int points = (GetActiveUnit().GetUnitLevel() * skillPointsPerLv) - GetActiveUnit().GetSpentSkillPoints();

        //Debug.Log("points = " + points);

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