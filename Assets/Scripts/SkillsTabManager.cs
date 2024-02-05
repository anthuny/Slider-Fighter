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
    public UIElement hitsStatUI;
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

    void OnApplicationQuit()
    {
        // Reset skill points of each skill to 0 on ally first start

        /*
        for (int i = 0; i < activeRoomAllies.Count; i++)
        {
            activeRoomAllies[i].unitData.UpdateCurSkills(activeRoomAllies[i].GetStartingSkills());
        }
        */
    }

    public void UpdateProgressSlider()
    {
        selectedSkillBase.gameObject.GetComponent<Slot>().UpdateProgressSlider();
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

                selectedSkillBase.gameObject.GetComponent<Slot>().IncreaseProgressSlider();
            }
        }
        else if (skillUpgradeType == 1)
        {
            activeSkillBase.upgradeIncPowerCount++;
        }
        else if (skillUpgradeType == 2)
        {
            activeSkillBase.upgradeIncEffectCount++;
        }

        activeSkillBase.curSkillLevel++;
        selectedSkillBase.UpdateSkillLevelText(activeSkillBase.curSkillLevel);

        UpdateSkillUpgradesText();

        SkillAddPoint();

        UpdateSkillStatDetails();
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

        /*
        if (GetSelectedSkillSlot().GetGearOwnedBy() == Slot.SlotOwnedBy.MAIN)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornGearMainAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornGearMainAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornGearMainAlly()[x].GetSlotName() == GetSelectedGearSlot().GetSlotName())
                    {
                        // Remove saved equipped gear piece (data side)
                        if (OwnedLootInven.Instance.GetWornGearMainAlly()[x].GetCurGearType() == Slot.SlotType.HELMET)
                        {
                            UpdateEquippedGearPiece("helmMain", null, false);
                        }
                        else if (OwnedLootInven.Instance.GetWornGearMainAlly()[x].GetCurGearType() == Slot.SlotType.CHESTPIECE)
                        {
                            UpdateEquippedGearPiece("chestMain", null, false);
                        }
                        else if (OwnedLootInven.Instance.GetWornGearMainAlly()[x].GetCurGearType() == Slot.SlotType.BOOTS)
                        {
                            UpdateEquippedGearPiece("bootsMain", null, false);
                        }

                        // Update unit stats when unequiping
                        UpdateUnitStatsUnEquip(OwnedLootInven.Instance.GetWornGearMainAlly()[x]);

                        // Add gear into owned gear
                        OwnedLootInven.Instance.AddOwnedGear(OwnedLootInven.Instance.GetWornGearMainAlly()[x]);

                        // Remove worn gear
                        OwnedLootInven.Instance.RemoveWornGearAllyMain(OwnedLootInven.Instance.GetWornGearMainAlly()[x]);
                        break;
                    }
                }
            }
        }
        else if (GetSelectedBaseGearSlot().GetGearOwnedBy() == Slot.SlotOwnedBy.SECOND)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornGearSecondAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornGearSecondAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornGearSecondAlly()[x].GetSlotName() == GetSelectedGearSlot().GetSlotName())
                    {
                        // Remove saved equipped gear piece (data side)
                        if (OwnedLootInven.Instance.GetWornGearSecondAlly()[x].GetCurGearType() == Slot.SlotType.HELMET)
                        {
                            UpdateEquippedGearPiece("helmSecond", null, false);
                        }
                        else if (OwnedLootInven.Instance.GetWornGearSecondAlly()[x].GetCurGearType() == Slot.SlotType.CHESTPIECE)
                        {
                            UpdateEquippedGearPiece("chestSecond", null, false);
                        }
                        else if (OwnedLootInven.Instance.GetWornGearSecondAlly()[x].GetCurGearType() == Slot.SlotType.BOOTS)
                        {
                            UpdateEquippedGearPiece("bootsSecond", null, false);
                        }

                        // Update unit stats when unequiping
                        UpdateUnitStatsUnEquip(OwnedLootInven.Instance.GetWornGearSecondAlly()[x]);

                        // Add gear into owned gear
                        OwnedLootInven.Instance.AddOwnedGear(OwnedLootInven.Instance.GetWornGearSecondAlly()[x]);

                        // Remove worn gear
                        OwnedLootInven.Instance.RemoveWornGearAllySecond(OwnedLootInven.Instance.GetWornGearSecondAlly()[x]);
                        break;
                    }
                }
            }
        }
        else if (GetSelectedBaseGearSlot().GetGearOwnedBy() == Slot.SlotOwnedBy.THIRD)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornGearThirdAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornGearThirdAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornGearThirdAlly()[x].GetSlotName() == GetSelectedGearSlot().GetSlotName())
                    {
                        // Remove saved equipped gear piece (data side)
                        if (OwnedLootInven.Instance.GetWornGearThirdAlly()[x].GetCurGearType() == Slot.SlotType.HELMET)
                        {
                            UpdateEquippedGearPiece("helmThird", null, false);
                        }
                        else if (OwnedLootInven.Instance.GetWornGearThirdAlly()[x].GetCurGearType() == Slot.SlotType.CHESTPIECE)
                        {
                            UpdateEquippedGearPiece("chestThird", null, false);
                        }
                        else if (OwnedLootInven.Instance.GetWornGearThirdAlly()[x].GetCurGearType() == Slot.SlotType.BOOTS)
                        {
                            UpdateEquippedGearPiece("bootsThird", null, false);
                        }

                        // Update unit stats when unequiping
                        UpdateUnitStatsUnEquip(OwnedLootInven.Instance.GetWornGearThirdAlly()[x]);

                        // Add gear into owned gear
                        OwnedLootInven.Instance.AddOwnedGear(OwnedLootInven.Instance.GetWornGearThirdAlly()[x]);

                        // Remove worn gear
                        OwnedLootInven.Instance.RemoveWornGearAllyThird(OwnedLootInven.Instance.GetWornGearThirdAlly()[x]);

                        break;
                    }
                }
            }
        }

        // Remove gear icon display
        GetSelectedBaseGearSlot().ResetGearSlot(true, true);

        // Remove gear icon details (name / stats)
        ClearAllGearStats();
        */
    }

    public void EquipSkill(Slot skill)
    {
        OwnedLootInven.Instance.ToggleOwnedGearDisplay(false);



        /*
        Slot removedGear = null;

        // Remove current equipt item, and place into owned gear

        if (GetSelectedSlot().GetGearOwnedBy() == Slot.SlotOwnedBy.MAIN)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornGearMainAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornGearMainAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornGearMainAlly()[x].GetSlotName() == skill.GetSlotName())
                    {
                        // Add gear into owned gear
                        OwnedLootInven.Instance.AddOwnedGear(OwnedLootInven.Instance.GetWornGearMainAlly()[x]);

                        // Remove worn gear
                        OwnedLootInven.Instance.RemoveWornGearAllyMain(OwnedLootInven.Instance.GetWornGearMainAlly()[x]);
                        break;
                    }
                }
            }
        }
        else if (GetSelectedSlot().GetGearOwnedBy() == Slot.SlotOwnedBy.SECOND)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornGearSecondAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornGearSecondAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornGearSecondAlly()[x].GetSlotName() == skill.GetSlotName())
                    {
                        // Add gear into owned gear
                        OwnedLootInven.Instance.AddOwnedGear(OwnedLootInven.Instance.GetWornGearSecondAlly()[x]);

                        // Remove worn gear
                        OwnedLootInven.Instance.RemoveWornGearAllySecond(OwnedLootInven.Instance.GetWornGearSecondAlly()[x]);
                        break;
                    }
                }
            }
        }
        else if (GetSelectedSlot().GetGearOwnedBy() == Slot.SlotOwnedBy.THIRD)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornGearThirdAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornGearThirdAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornGearThirdAlly()[x].GetSlotName() == skill.GetSlotName())
                    {
                        // Add gear into owned gear
                        OwnedLootInven.Instance.AddOwnedGear(OwnedLootInven.Instance.GetWornGearThirdAlly()[x]);

                        // Remove worn gear
                        OwnedLootInven.Instance.RemoveWornGearAllyThird(OwnedLootInven.Instance.GetWornGearThirdAlly()[x]);
                        break;
                    }
                }
            }
        }


        // Remove gear from owned gear list when equipping
        // Add gear to worn gear
        for (int i = 0; i < OwnedLootInven.Instance.ownedGear.Count; i++)
        {
            if (OwnedLootInven.Instance.ownedGear[i].GetSlotName() == skill.GetSlotName())
            {
                removedGear = OwnedLootInven.Instance.ownedGear[i];

                OwnedLootInven.Instance.RemoveOwnedGear(removedGear);

                if (GetSelectedSlot().GetGearOwnedBy() == Slot.SlotOwnedBy.MAIN)
                    OwnedLootInven.Instance.AddWornGearAllyMain(removedGear);
                else if (GetSelectedSlot().GetGearOwnedBy() == Slot.SlotOwnedBy.SECOND)
                    OwnedLootInven.Instance.AddWornGearAllySecond(removedGear);
                else if (GetSelectedSlot().GetGearOwnedBy() == Slot.SlotOwnedBy.THIRD)
                    OwnedLootInven.Instance.AddWornGearAllyThird(removedGear);
                break;
            }
        }

        */

        /*
        GetSelectedSlot().UpdateGearBonusHealth(skill.GetBonusHealth());
        GetSelectedSlot().UpdateGearBonusHealing(skill.GetBonusHealing());
        GetSelectedSlot().UpdateGearBonusDefense(skill.GetBonusDefense());
        GetSelectedSlot().UpdateGearBonusDamage(skill.GetBonusDamage());
        GetSelectedSlot().UpdateGearBonusSpeed(skill.GetBonusSpeed());
        */



        // Update unit stats with stats from gear
        //UpdateUnitStatsEquip(skill);

        // Show combined calculated values next to unit
    }

    public void UpdateSkillUpgradesText()
    {
        selectedSkillBase.skillUpgradesUI.UpdateContentText(activeSkillBase.upgradeIncTargetCount.ToString());
        selectedSkillBase.skillUpgradesUI.UpdateContentText2(activeSkillBase.GetCalculatedSkillPower().ToString() + "%");
        selectedSkillBase.skillUpgradesUI.UpdateContentText3(activeSkillBase.GetCalculatedSkillEffectChance().ToString() + "%");

        //Debug.Log(activeSkillBase.GetCalculatedSkillSelectionCount().ToString());
    }

    void UpdateSkillStatsDisplay(int power = 0, int hits = 0, int cd = 0, int maxTargets = 0, int hitEffectChance = 0)
    {
        powerStatUI.UpdateContentText(power.ToString());
        hitsStatUI.UpdateContentText(hits.ToString());
        cdStatUI.UpdateContentText(cd.ToString());
        maxTargetsStatUI.UpdateContentText(maxTargets.ToString());
        hitEffectChanceUI.UpdateContentText(hitEffectChance.ToString());

        //UpdateSkillStatsDisplay(activeSkillBase.GetCalculatedSkillPowerStat(), activeSkillBase.skillAttackCount, activeSkillBase.skillCooldown);
    }

    void ClearSkillStatsDisplay()
    {
        powerStatUI.UpdateContentText("");
        hitsStatUI.UpdateContentText("");
        cdStatUI.UpdateContentText("");
        maxTargetsStatUI.UpdateContentText("");
        hitEffectChanceUI.UpdateContentText("");
    }

    public void UpdateSkillStatDetails()
    {
        ClearAllSkillsStats();

        if (activeSkillBase != null)
        {
            //Debug.Log(activeSkillBase);
            UpdateActiveSkillNameText(activeSkillBase.skillName);
            UpdateSkillDescription(activeSkillBase.skillTabDescr);
            UpdateSkillStatsDisplay(activeSkillBase.GetCalculatedSkillPowerStat(), activeSkillBase.skillAttackCount, activeSkillBase.skillCooldown, activeSkillBase.GetCalculatedSkillSelectionCount(), Mathf.RoundToInt(activeSkillBase.GetCalculatedSkillEffectStat()));
        }
        else
        {
            //Debug.Log("active skill base missing");
            UpdateActiveSkillNameText("");
            UpdateSkillDescription("");
            ClearSkillStatsDisplay();
        }


        /*
        // Gear Stats Update
        for (int i = 0; i < 5; i++)
        {
            GameObject spawnedStat = Instantiate(gearStatGO, gearStatsUI.transform.position, Quaternion.identity);
            spawnedStat.transform.SetParent(gearStatsUI.transform);
            spawnedStat.transform.localPosition = Vector2.zero;
            spawnedStat.transform.localScale = Vector2.one;

            UIElement statUI = spawnedStat.GetComponent<UIElement>();

            // Update Skill stat UI
            if (i == 0)
                statUI.UpdateContentText(SelectedOwnedSlot().GetBonusHealth().ToString());
            else if (i == 1)
                statUI.UpdateContentText(SelectedOwnedSlot().GetBonusDamage().ToString());
            else if (i == 2)
                statUI.UpdateContentText(SelectedOwnedSlot().GetBonusHealing().ToString());
            else if (i == 3)
                statUI.UpdateContentText(SelectedOwnedSlot().GetBonusDefense().ToString());
            else if (i == 4)
                statUI.UpdateContentText(SelectedOwnedSlot().GetBonusSpeed().ToString());


        }
        */



        //GetSelectedSlot().UpdateSlotImage(GetSelectedSlot().GetGearImage());

        // Gear Skill Name Update


        //UpdateSkillDescription(SelectedOwnedSlot().d)
        // Gear Skill Description Update


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

    public void ClearAllSkillsStats()
    {
        /*
        // Clear all gear stats
        GameObject gearStatGO = gearStatsUI.gameObject;
        for (int i = 0; i < gearStatGO.transform.childCount; i++)
        {
            Destroy(gearStatGO.transform.GetChild(i).gameObject);
        }
        */

        /*
        GameObject gearDescGO = skillDescUI.gameObject;
        for (int x = 0; x < gearDescGO.transform.childCount; x++)
        {
            Destroy(gearDescGO.transform.GetChild(x).gameObject);
        }
        */
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
        //Debug.Log("4");
        unit.UpdateUnitSkills(unitData.GetUnitSkills());
        unit.UpdateCurrentSkills(unitData.GetUnitSkills());

        //unit.GetSkillBaseSlot(0).se

        ToggleOwnedSkillSlotsClickable(true);

        SetupSkillsTab(unit);

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

    public void SetupSkillsTab(UnitFunctionality unit)
    {
        UpdateActiveUnit(unit);
        
        selectedSkillBase = skillBase1;
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

        UpdateUnspentPointsText(CalculateUnspentSkillPoints());
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

        //int maxAmount = GetActiveSkillBase().maxSkillLevel;

        // if mastery points are maxed already, stop
        //if (GetSelectedSkillSlot().GetStatPointsAdded() >= maxAmount)
        //    return;

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        GetSelectedSkillSlot().UpdateStatPoindsAdded(true, false, 0, false);
        //GetSelectedSkillSlot().UpdateContentSubText(GetSelectedSkillSlot().GetStatPointsAdded().ToString());

        /*
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
        */
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

        //int maxAmount = GetActiveSkillBase().maxSkillLevel;

        // if mastery points are at 0 already, stop
        if (GetSelectedSkillSlot().GetStatPointsAdded() <= 0)
            return;

        GetSelectedSkillSlot().UpdateStatPoindsAdded(false);
        //GetSelectedSkillSlot().UpdateContentSubText(GetSelectedSkillSlot().GetStatPointsAdded().ToString() + " / " + maxAmount);
        
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

    public void UpdateSelectedSkillBase(ButtonFunctionality statButton = null)
    {
        if (statButton == null)
        {
            //if (GameManager.Instance.activeTeam.Count == 1)
            //    return;

            selectedSkillBase = skillBase1;

            //if (GetActiveUnit().GetSkillBaseSlot(0) == null)
            //    return;

            activeSkillBase = GetActiveUnit().GetSkill(0);
        }
        else
        {
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
        ResetSkillsBaseSelection();

        selectedBaseSlot = slot;

        ResetBaseSkillUpgradesDisplay();

        slot.ToggleSlotSelection(true);

        slot.GetComponent<UIElement>().skillUpgradesUI.UpdateAlpha(1);

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
        //OwnedLootInven.Instance.FillOwnedGearSlots();

        // Bug todo - 2nd / 3rd ally arent having their gear saved.

        if (slot.curSlotStatis == Slot.SlotStatis.DEFAULT)
        {
            UpdateSelectedBaseSlot(slot);

            //selectedBaseSlot.ResetGearSlot(true, false);

            //OwnedLootInven.Instance.EnableOwnedItemsSlotSelection(GetSelectedSlot());
        }
        else
        {
            //GetSelectedBaseSlot().UpdateSlotImage(slot.GetSlotImage());
            //GetSelectedBaseSlot().UpdateCurSlotType(Slot.SlotType.SKILL);

            //GetSelectedBaseSlot().UpdateSlotImage(slot.skill.skillSprite);
            //GetSelectedBaseSlot().UpdateSlotName(slot.GetSlotName());

            //

            AdjustActiveSkills(GetActiveUnit(), slot.skill, activeSkillBase);
            SetupSkillsTab(GetActiveUnit());

            //UpdateSelectedSkillBase();



            //Debug.Log(slot.GetSlotImage().name);
            //UpdateSelectedBaseSlot(slot);
            UpdateSelectedOwnedSlot(slot);
            UpdateSelectedBaseSlot(selectedBaseSlot);
            // Update UI
            //UpdateActiveSkillNameText(slot.GetSlotName());
            UpdateSkillStatDetails();

            UpdateProgressSlider();

            OwnedLootInven.Instance.ToggleOwnedGearDisplay(false, "", false);
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
                GetSelectedBaseSlot().ToggleSlotSelection(true);

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

            UpdateSelectedOwnedSlot(slot);

            // Update UI
            UpdateSkillStatDetails();

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
        if (count == 0)
        {
            unspentPointsText.UpdateContentText("");

            // Disable add point buttons and visuals
            selectedSkillBase.gameObject.GetComponent<Slot>().ToggleSkillUpgradeButtons(false);
        }
        else
        {
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

        /*
        if (GetSelectedSkillSlot().GetStatPointsAdded() > GetActiveSkillBase().maxSkillLevel)
            GetActiveUnit().UpdateSpentSkillPoints(-1);
        */

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