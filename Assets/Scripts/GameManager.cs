using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private float postBattleTime;
    public List<Unit> activeTeam = new List<Unit>();
    public List<Unit> allPlayerClasses = new List<Unit>();
    [SerializeField] private GameObject baseUnit;
    [SerializeField] private List<Transform> enemySpawnPositions = new List<Transform>();
    [SerializeField] private List<Transform> playerSpawnPositions = new List<Transform>();
    [SerializeField] private Transform allyPostBattlePositionTransform;
    [SerializeField] private Transform allyBattlePositionTransform;
    [SerializeField] private Transform allyPositions;

    [SerializeField] private List<Unit> activeRoomAllUnits = new List<Unit>();
    [SerializeField] private List<UnitFunctionality> activeRoomAllUnitFunctionalitys = new List<UnitFunctionality>();
    [SerializeField] private List<UnitFunctionality> activeRoomAllies = new List<UnitFunctionality>();
    [SerializeField] private List<UnitFunctionality> activeRoomEnemies = new List<UnitFunctionality>();

    [SerializeField] private GameObject unitIcon;

    [Header("Map")]
    public MapManager map;

    [Header("Turn Order")]
    [SerializeField] private Transform turnOrderParent;
    [SerializeField] private UIElement turnOrder;
    public List<float> turnOrderIconAlphas = new List<float>();

    [Header("Units")]
    public float expIncPerLv;
    public float maxExpStarting;
    public float timePostExp;
    public float fillAmountIntervalTimeGap;
    public int expKillGainedPerLv;
    public int expKillGainedStarting;
    public int goldGainedPerUnit;
    //public int goldGainedPerUnitLevelMult;

    [Header("Player UI")]
    public UIElement playerUIElement;
    public UIElement playerWeapon;
    public UIElement playerWeaponBackButton;
    public UIElement playerWeaponBG;
    public UIElement playerAbilities;
    public UIElement playerAbilityDesc;
    public PostBattle postBattleUI;
    public OverlayUI abilityDetailsUI;
    public Text curUnitsTargetedText;
    public Text maxUnitsTargetedText;
    public UIElement endTurnButtonUI;
    public Text activeUnitNameText;

    [Header("Player Ability UI")]
    public IconUI playerIcon;
    public IconUI playerSkill1;
    public IconUI playerSkill2;
    public IconUI playerSkill3;

    [Header("Skill Buttons")]
    public ButtonFunctionality skill1;
    public ButtonFunctionality skill2;
    public ButtonFunctionality skill3;
    public UIElement skill1IconUnavailableImage;
    public UIElement skill2IconUnavailableImage;
    public UIElement skill3IconUnavailableImage;
    public UIText skill1IconEnergyCostUIText;
    public UIText skill2IconEnergyCostUIText;
    public UIText skill3IconEnergyCostUIText;

    public List<UnitFunctionality> unitsSelected = new List<UnitFunctionality>();

    [Header("Power UI")]
    public GameObject powerUITextPrefab;
    [SerializeField] private float timeBetweenPowerUIStack;
    public float powerUIHeightLvInc;
    [SerializeField] private float postHitWaitTime;
    public string missPowerText;
    public Color missPowerTextColour;
    public Color damagePowerTextColour;
    public Color healPowerTextColour;
    public int powerHitFontSize;
    public int powerMissFontSize;

    public Skill activeSkill;

    [Header("Post Battle")]
    public Rewards rewards;
    public DefeatedEnemies defeatedEnemies;

    private Room activeRoom;

    private int experienceGained;
    private int activeRoomEnemiesKilled;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        RoomManager.roomManager.ChooseRoom();
        ToggleUIElement(turnOrder, false);

        UpdateTurnOrder();

        ToggleUIElement(playerWeapon, false);
        postBattleUI.TogglePostBattleUI(false);
    }

    void UpdateAllAlliesPosition(bool postBattle)
    {
        if (!postBattle)
        {
            allyPositions.SetParent(allyBattlePositionTransform);
            allyPositions.SetPositionAndRotation(new Vector2(0,0), Quaternion.identity);
            allyPositions.position = allyBattlePositionTransform.position;

        }
        else
        {
            allyPositions.SetParent(allyPostBattlePositionTransform);
            allyPositions.SetPositionAndRotation(new Vector2(0, 0), Quaternion.identity);
            allyPositions.position = allyPostBattlePositionTransform.position;
        }
    }

    #region Setup Multiple UIs
    IEnumerator SetupPostBattleUI(bool playerWon)
    {
        yield return new WaitForSeconds(postBattleTime);

        StartCoroutine(SetupRoomPostBattle(playerWon));
        UpdateAllAlliesPosition(true);
    }

    // Toggle UI accordingly
    IEnumerator SetupRoomPostBattle(bool playerWon)
    {
        // Toggle player overlay and skill ui off
        ToggleUIElement(playerAbilities, false);
        ToggleUIElement(playerAbilityDesc, false);
        ToggleUIElement(endTurnButtonUI, false);
        ToggleUIElement(turnOrder, false);
        ResetActiveUnitTurnArrow();
        ToggleAllAlliesHealthBar(false);

        // Toggle post battle ui on
        postBattleUI.TogglePostBattleUI(true);

        postBattleUI.TogglPostBattleConditionText(playerWon);   // Update battle condition text

        defeatedEnemies.DisplayDefeatedEnemies();

        yield return new WaitForSeconds(2);

        // Give Exp to ally units
        for (int i = 0; i < activeRoomAllies.Count; i++)
        {
            activeRoomAllies[i].ToggleUnitBG(true);
            activeRoomAllies[i].UpdateUnitExp(GetExperienceGained());
        }

        yield return new WaitForSeconds(1.5f);

        rewards.FillRewardsTable(GetActiveRoom().difficultyLevel);
    }

    // After the user strikes their weapon, remove skill details and skills UI for dmg showcase
    public void SetupPlayerPostHitUI()
    {
        ToggleUIElement(playerWeaponBG, false);
        ToggleUIElement(playerWeapon, false);
        ToggleUIElement(playerWeaponBackButton, false);
        ToggleUIElement(playerAbilities, false);
        ToggleUIElement(playerAbilityDesc, false);

        ToggleEndTurnButton(true);
    }

    public void SetupPlayerSkillsUI()
    {
        ToggleUIElement(playerWeaponBG, false);
        ToggleUIElement(playerWeapon, false);
        ToggleUIElement(playerWeaponBackButton, false);

        Weapon.instance.ToggleAttackButtonInteractable(false);

        activeSkill = GetActiveUnit().basicSkill;

        ToggleUIElement(playerAbilities, true);
        ToggleUIElement(playerAbilityDesc, true);
        ToggleEndTurnButton(true);

        UpdateUnitsSelectedText();
    }

    public void SetupPlayerWeaponUI()
    {
        if (!CheckIfAnyUnitsSelected())
            return;

        Weapon.instance.DisableAlertUI();
        ToggleUIElement(playerAbilities, false);
        ToggleUIElement(playerAbilityDesc, false);

        ToggleUIElement(playerWeaponBG, true);
        ToggleUIElement(playerWeapon, true);
        ToggleUIElement(playerWeaponBackButton, true);
        ToggleEndTurnButton(false);

        Weapon.instance.StartHitLine();
        Weapon.instance.ToggleAttackButtonInteractable(true);
    }
    #endregion

    public void ReturnEnergyToUnit()
    {
        UpdateActiveUnitHealthBar(false);
        UpdateActiveUnitEnergyBar(true, true, activeSkill.skillEnergyCost);
    }

    public void StartRoom(Room room)
    {
        UpdateActiveRoom(room);

        // Reset experience gained from killed enemies
        ResetExperienceGained();

        // Spawn enemy units
        for (int i = 0; i < room.enemies.Count; i++)
        {
            Unit unit = room.enemies[i];    // Reference

            GameObject go = Instantiate(baseUnit, enemySpawnPositions[i]);
            go.transform.SetParent(enemySpawnPositions[i]);

            UnitFunctionality unitFunctionality = go.GetComponent<UnitFunctionality>();
            unitFunctionality.ResetPosition();
            unitFunctionality.UpdateUnitName(unit.name);
            unitFunctionality.UpdateUnitSprite(unit.unitSprite);
            unitFunctionality.UpdateUnitColour(unit.unitColour);
            unitFunctionality.UpdateUnitType("Enemy");
            unitFunctionality.UpdateUnitSpeed(unit.startingSpeed);
            unitFunctionality.UpdateUnitPower(unit.startingPower);

            unitFunctionality.UpdateUnitHealth(unit.startingMaxHealth, unit.startingMaxHealth);
            unitFunctionality.UpdateUnitStartTurnEnergy(unit.startingUnitStartTurnEnergyGain);

            AddActiveRoomAllUnitsFunctionality(unitFunctionality);
            AddActiveRoomAllUnits(unit);

            unitFunctionality.UpdateMaxEnergy(unit.startingEnergy);
            unitFunctionality.UpdateUnitCurEnergy(unit.startingEnergy);
            unitFunctionality.UpdateUnitLevel(1);
            //UpdateActiveUnitEnergyBar(true, true, unit.startingEnergy);
        }

        // Spawn player units
        for (int i = 0; i < activeTeam.Count; i++)
        {
            Unit unit = activeTeam[i];    // Reference

            GameObject go = Instantiate(baseUnit, playerSpawnPositions[i]);
            go.transform.SetParent(playerSpawnPositions[i]);

            UnitFunctionality unitFunctionality = go.GetComponent<UnitFunctionality>();
            unitFunctionality.ResetPosition();
            unitFunctionality.UpdateUnitName(unit.name);
            unitFunctionality.UpdateUnitSprite(unit.unitSprite);
            unitFunctionality.UpdateUnitColour(unit.unitColour);
            unitFunctionality.UpdateUnitType("Player");
            unitFunctionality.UpdateUnitSpeed(unit.startingSpeed);
            unitFunctionality.UpdateUnitPower(unit.startingPower);

            unitFunctionality.UpdateUnitHealth(unit.startingMaxHealth, unit.startingMaxHealth);
            unitFunctionality.UpdateUnitStartTurnEnergy(unit.startingUnitStartTurnEnergyGain);

            AddActiveRoomAllUnitsFunctionality(unitFunctionality);
            AddActiveRoomAllUnits(unit);

            unitFunctionality.UpdateMaxEnergy(unit.startingEnergy);
            unitFunctionality.UpdateUnitCurEnergy(unit.startingEnergy);
            unitFunctionality.UpdateUnitLevel(1);
            //UpdateActiveUnitEnergyBar(true, true, unit.startingEnergy);
        }

        // Update allies into position for battle
        UpdateAllAlliesPosition(false);

        DeselectAllUnits();
        DetermineTurnOrder();
    }

    public bool CheckIfEnergyAvailableSkill()
    {
        if (GetActiveUnitFunctionality().GetUnitCurEnergy() >= activeSkill.skillEnergyCost)
            return true;
        else
            return false;
    }

    public void UpdateActiveSkill(Skill skill)
    {
        activeSkill = skill;
    }

    public IEnumerator WeaponAttackCommand(int power)
    {
        UpdateActiveUnitEnergyBar(false);
        UpdateActiveUnitHealthBar(true);

        // Loop as many times as power text will appear
        for (int x = 0; x < activeSkill.skillAttackCount; x++)
        {
            // Loop through all selected units
            for (int i = unitsSelected.Count - 1; i >= 0; i--)
            {
                if (unitsSelected[i] == null)
                    continue;
                else
                {
                    //unitsSelected[i].ToggleSelected(false);
                    unitsSelected[i].SpawnPowerUI(power);

                    // Reset unit's prev power text for future power texts
                    if (x == activeSkill.skillAttackCount - 1)
                        unitsSelected[i].ResetPreviousPowerUI();

                    // Increase health from the units current health if a support skill was casted on it
                    if (activeSkill.curSkillType == Skill.SkillType.SUPPORT)
                        unitsSelected[i].UpdateUnitCurrentHealth(power);

                    // Decrease health from the units current health if a offense skill was casted on it
                    if (activeSkill.curSkillType == Skill.SkillType.OFFENSE)
                        unitsSelected[i].UpdateUnitCurrentHealth(-power);
                }
            }

            // Time wait in between attacks, shared across all targeted units
            yield return new WaitForSeconds(timeBetweenPowerUIStack);
        }

        for (int y = 0; y < unitsSelected.Count; y++)
        {
            if (unitsSelected[y].CheckIfUnitIsDead())
            {
                unitsSelected[y].EnsureUnitIsDead();
            }
        }

        yield return new WaitForSeconds(postHitWaitTime);

        unitsSelected.Clear();
        SetupPlayerSkillsUI();

        if (GetActiveUnit().curUnitType == Unit.UnitType.PLAYER)
        {
            UpdateSkillDetails(GetActiveUnit().basicSkill);
            UpdateAllSkillIconAvailability();
            UpdateUnitSelection(activeSkill);
            UpdateUnitsSelectedText();
        }
    }
    #region Update Unit UI
    public void UpdateActiveUnitEnergyBar(bool toggle = false, bool increasing = false, int energyAmount = 0)
    {
        //Debug.Log(GetActiveUnitFunctionality().GetUnitName() + " " + GetActiveUnitFunctionality().GetUnitCurEnergy());

        GetActiveUnitFunctionality().energyCostImage.UpdateEnergyBar((int)GetActiveUnitFunctionality().
            GetUnitCurEnergy(), energyAmount, increasing, toggle);
    }

    public void UpdateActiveUnitHealthBar(bool toggle)
    {
        GetActiveUnitFunctionality().ToggleUnitHealthBar(toggle);
    }

    public void ToggleAllAlliesHealthBar(bool toggle)
    {
        for (int i = 0; i < activeRoomAllies.Count; i++)
        {
            activeRoomAllies[i].ToggleUnitHealthBar(toggle);
        }
    }
    #endregion
    #region Update Skill Icons Overlay UI
    public void UpdateAllSkillIconAvailability()
    {
        // Update all skill icon Energy text + unavailable image

        if (GetActiveUnitFunctionality().GetUnitCurEnergy() >= GetActiveUnit().GetSkill1().skillEnergyCost)
        {
            UpdateSkillIconAvailability(skill1IconUnavailableImage, skill1IconEnergyCostUIText, GetActiveUnit().GetSkill1().skillEnergyCost.ToString(), false);
        }
        else
        {
            UpdateSkillIconAvailability(skill1IconUnavailableImage, skill1IconEnergyCostUIText, GetActiveUnit().GetSkill1().skillEnergyCost.ToString(), true);
        }

        if (GetActiveUnitFunctionality().GetUnitCurEnergy() >= GetActiveUnit().GetSkill2().skillEnergyCost)
        {
            UpdateSkillIconAvailability(skill2IconUnavailableImage, skill2IconEnergyCostUIText, GetActiveUnit().GetSkill2().skillEnergyCost.ToString(), false);
        }
        else
        {
            UpdateSkillIconAvailability(skill2IconUnavailableImage, skill2IconEnergyCostUIText, GetActiveUnit().GetSkill2().skillEnergyCost.ToString(), true);
        }

        if (GetActiveUnitFunctionality().GetUnitCurEnergy() >= GetActiveUnit().GetSkill3().skillEnergyCost)
        {
            UpdateSkillIconAvailability(skill3IconUnavailableImage, skill3IconEnergyCostUIText, GetActiveUnit().GetSkill3().skillEnergyCost.ToString(), false);
        }
        else
        {
            UpdateSkillIconAvailability(skill3IconUnavailableImage, skill3IconEnergyCostUIText, GetActiveUnit().GetSkill3().skillEnergyCost.ToString(), true);
        }

        UpdateAllSkillIconEnergyCost();        
    }

    public void UpdateSkillIconAvailability(UIElement skillIconAvailabilityImage, UIText skillIconEnergyCost, string skillEnergyCost, bool toggle)
    {
        if (toggle)
        {
            skillIconAvailabilityImage.UpdateAlpha(1);
            skillIconEnergyCost.UpdateUIText(skillEnergyCost);
        }
        else
        {
            skillIconAvailabilityImage.UpdateAlpha(0);
        }
    }
    void UpdateAllSkillIconEnergyCost()
    {
        skill1IconEnergyCostUIText.UpdateUIText(GetActiveUnit().GetSkill1().skillEnergyCost.ToString());
        skill2IconEnergyCostUIText.UpdateUIText(GetActiveUnit().GetSkill2().skillEnergyCost.ToString());
        skill3IconEnergyCostUIText.UpdateUIText(GetActiveUnit().GetSkill3().skillEnergyCost.ToString());
    }

    public void DisableAllSkillSelections()
    {
        skill1.ToggleSelected(false);
        skill2.ToggleSelected(false);
        skill3.ToggleSelected(false);
    }
    #endregion

    public void UpdateSkillDetails(Skill skill)
    {
        ToggleUIElement(playerAbilityDesc, true);

        bool tempAttack = false;

        if (skill.curSkillType == Skill.SkillType.OFFENSE)
            tempAttack = true;
        else
            tempAttack = false;

        abilityDetailsUI.UpdateSkillUI(skill.skillName, skill.skillDescr, skill.skillPower, 
            skill.skillAttackCount, tempAttack, skill.skillSelectionCount, 
            skill.skillPower, skill.skillEnergyCost, skill.skillPowerIcon, skill.skillSprite);

        UnitFunctionality activeUnit = GetActiveUnitFunctionality();

        // Update Unit Overlay Energy
        abilityDetailsUI.UpdateUnitOverlayEnergyUI(GetActiveUnitFunctionality(),
            activeUnit.GetUnitCurEnergy(), activeUnit.GetUnitMaxEnergy());

        // Update Unit Overlay Health
        abilityDetailsUI.UpdateUnitOverlayHealthUI(activeUnit, activeUnit.GetUnitCurHealth(), activeUnit.GetUnitMaxHealth());
    }



    public void RemoveUnit(UnitFunctionality unitFunctionality)
    {
        if (activeRoomEnemies.Contains(unitFunctionality))
            activeRoomEnemies.Remove(unitFunctionality);

        if (activeRoomAllies.Contains(unitFunctionality))
            activeRoomAllies.Remove(unitFunctionality);

        if (activeRoomAllUnitFunctionalitys.Contains(unitFunctionality))
            activeRoomAllUnitFunctionalitys.Remove(unitFunctionality);

        // If this is the last enemy that got killed, queue player win post battle ui
        if (activeRoomEnemies.Count == 0)
            StartCoroutine(SetupPostBattleUI(true));
        // If this is the last ally that got killed, queue player lose post battle ui
        if (activeRoomAllies.Count == 0)
            StartCoroutine(SetupPostBattleUI(false));

        AddExperienceGained(unitFunctionality.GetUnitExpKillGained());

        UpdateEnemiesKilled(unitFunctionality);

        // Remove unit from unit list
        for (int i = 0; i < activeRoomAllUnits.Count; i++)
        {
            if (unitFunctionality.GetUnitName() == activeRoomAllUnits[i].name)
            {
                activeRoomAllUnits.RemoveAt(i);
                return;
            }
        }
    }

    public void UpdateActiveUnitNameText(string name)
    {
        activeUnitNameText.text = name;
    }

    public void DetermineTurnOrder()
    {
        activeRoomAllUnitFunctionalitys.Sort(CompareUnitFunctionalitySpeed);
        activeRoomAllUnitFunctionalitys.Reverse();

        activeRoomAllUnits.Sort(CompareUnitSpeed);
        activeRoomAllUnits.Reverse();

        UpdateTurnOrder();
    }

    public void UpdateTurnOrder()
    {
        ToggleUIElement(turnOrder, true);   // Enable turn order UI

        UnitFunctionality unitFunctionalityMoving = GetActiveUnitFunctionality();
        activeRoomAllUnitFunctionalitys.RemoveAt(0);
        activeRoomAllUnitFunctionalitys.Insert(activeRoomAllUnitFunctionalitys.Count, unitFunctionalityMoving);

        Unit unitMoving = GetActiveUnit();
        activeRoomAllUnits.RemoveAt(0);
        activeRoomAllUnits.Insert(activeRoomAllUnits.Count, unitMoving);
     
        UpdateTurnOrderVisual();

        ToggleEndTurnButton(true);      // Toggle End Turn Button on

        // Toggle player UI accordingly if it's their turn or not
        if (activeRoomAllUnitFunctionalitys[0].curUnitType == UnitFunctionality.UnitType.PLAYER)
        {
            playerUIElement.UpdateAlpha(1);
            SetupPlayerSkillsUI();
            UpdatePlayerAbilityUI();
            UpdateActiveUnitNameText(GetActiveUnitFunctionality().GetUnitName());
        }
        else
            playerUIElement.UpdateAlpha(0);

        // Set the basic skill to be on by default to begin with
        activeSkill = GetActiveUnit().basicSkill;

        // If unit is at maxed energy, stop
        if (GetActiveUnitFunctionality().GetUnitCurEnergy() != GetActiveUnitFunctionality().GetUnitMaxEnergy())
            UpdateActiveUnitHealthBar(false);   // Disable unit health bar for unit start turn energy increase

        // Trigger Unit Energy regen 
        UpdateActiveUnitEnergyBar(true, true, GetActiveUnitFunctionality().unitStartTurnEnergyGain);

        if (GetActiveUnit().curUnitType == Unit.UnitType.PLAYER)
        {
            UpdateSkillDetails(activeSkill);
            UpdateAllSkillIconAvailability();
            UpdateUnitSelection(activeSkill);
        }

        UpdateActiveUnitTurnArrow();
    }

    public void AddExperienceGained(int exp)
    {
        experienceGained += exp;
    }

    public void ResetExperienceGained()
    {
        experienceGained = 0;
    }

    int GetExperienceGained()
    {
        return experienceGained;
    }

    public void UpdateUnitSelection(Skill usedSkill)
    {
        int selectedAmount = 0;

        // Clear all current selections
        ResetSelectedUnits();

        // If skill selection type is only on ENEMIES
        if (usedSkill.curSkillSelectionType == Skill.SkillSelectionType.ENEMIES)
        {
            // if the skill user is a PLAYER
            if (GetActiveUnit().curUnitType == Unit.UnitType.PLAYER)
            {
                // only select the closest ENEMY units
                for (int i = activeRoomEnemies.Count-1; i >= 0; i--)
                {
                    selectedAmount++;

                    SelectUnit(activeRoomEnemies[i]);

                    // If enough units have been selected (in order of closest)
                    if (selectedAmount == usedSkill.skillSelectionCount)
                        return;
                }
            }
            // If the skill user is an ENEMY
            else if (GetActiveUnit().curUnitType == Unit.UnitType.ENEMY)
            {
                // only select PLAYER units
                for (int i = 0; i > activeRoomAllies.Count; i++)
                {
                    selectedAmount++;

                    SelectUnit(activeRoomAllies[i]);

                    // If enough units have been selected, toggle the display
                    if (selectedAmount == usedSkill.skillSelectionCount)
                        return;
                }
            }
        }


        // If skill selection type is only on ALLIES
        if (usedSkill.curSkillSelectionType == Skill.SkillSelectionType.PLAYERS)
        {
            // if the skill user is a PLAYER
            if (GetActiveUnit().curUnitType == Unit.UnitType.PLAYER)
            {
                // only select PLAYER units
                for (int i = 0; i < activeRoomAllies.Count; i++)
                {
                    selectedAmount++;

                    SelectUnit(activeRoomAllies[i]);

                    // If enough units have been selected (in order of closest)
                    if (selectedAmount == usedSkill.skillSelectionCount)
                        return;
                }
            }
            // If the skill user is an ENEMY
            else if (GetActiveUnit().curUnitType == Unit.UnitType.ENEMY)
            {
                // only select ENEMY units
                for (int i = 0; i < activeRoomEnemies.Count; i++)
                {
                    selectedAmount++;

                    SelectUnit(activeRoomEnemies[i]);

                    // If enough units have been selected, toggle the display
                    if (selectedAmount == usedSkill.skillSelectionCount)
                        return;
                }
            }
        }
    }

    public bool CheckIfAnyUnitsSelected()
    {
        if (unitsSelected.Count == 0)
            return false;
        else
            return true;
    }

    public void ResetSelectedUnits()
    {
        DeselectAllUnits();

        for (int i = 0; i < unitsSelected.Count; i++)
        {
            unitsSelected[i].ToggleSelected(false);
        }

        unitsSelected.Clear();
    }

    private void DeselectAllUnits()
    {
        for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
        {
            activeRoomAllUnitFunctionalitys[i].ToggleSelected(false);
        }
    }

    public Unit GetActiveUnit()
    {
        return activeRoomAllUnits[0];
    }

    public UnitFunctionality GetActiveUnitFunctionality()
    {
        return activeRoomAllUnitFunctionalitys[0];
    }

    public void ToggleEndTurnButton(bool toggle)
    {
        if (toggle)
            endTurnButtonUI.UpdateAlpha(1);
        else
            endTurnButtonUI.UpdateAlpha(0);
    }

    public void ToggleUIElement(UIElement uiElement, bool toggle)
    {
        if (toggle)
            uiElement.UpdateAlpha(1);
        else
            uiElement.UpdateAlpha(0);
    }

    void UpdateEnemiesKilled(UnitFunctionality unit)
    {
        activeRoomEnemiesKilled++;

        defeatedEnemies.AddDefeatedEnemies(unit);
    }

    void ResetEnemiesKilledCount()
    {
        activeRoomEnemiesKilled = 0;
    }

    public int GetEnemiesKilledCount()
    {
        return activeRoomEnemiesKilled;
    }

    private void AddActiveRoomAllUnits(Unit unit)
    {
        activeRoomAllUnits.Add(unit);
    }

    private void AddActiveRoomAllUnitsFunctionality(UnitFunctionality unitFunctionality)
    {
        activeRoomAllUnitFunctionalitys.Add(unitFunctionality);

        if (unitFunctionality.curUnitType == UnitFunctionality.UnitType.PLAYER)
            activeRoomAllies.Add(unitFunctionality);
        else if (unitFunctionality.curUnitType == UnitFunctionality.UnitType.ENEMY)
            activeRoomEnemies.Add(unitFunctionality);
    }

    void ResetActiveUnitTurnArrow()
    {
        // Reset all current unit turn arrows
        for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
        {
            activeRoomAllUnitFunctionalitys[i].curUnitTurnArrow.UpdateAlpha(0);
        }
    }

    private void UpdateActiveUnitTurnArrow()
    {
        ResetActiveUnitTurnArrow();

        // Enable active unit turn arrow
        GetActiveUnitFunctionality().curUnitTurnArrow.UpdateAlpha(1);
    }



    private void UpdatePlayerAbilityUI()
    {
        // Update active player's portrait and colour
        playerIcon.UpdatePortrait(GetActiveUnitFunctionality().GetUnitSprite());
        playerIcon.UpdateColour(GetActiveUnitFunctionality().GetUnitColour());

        // Update player skill portraits
        playerSkill1.UpdatePortrait(GetActiveUnit().GetSkill1().skillSprite);
        playerSkill2.UpdatePortrait(GetActiveUnit().GetSkill2().skillSprite);
        playerSkill3.UpdatePortrait(GetActiveUnit().GetSkill3().skillSprite);
    }

    public void UpdateTurnOrderVisual()
    {
        // Remove all turn order icons
        for (int i = 0; i < turnOrderParent.childCount; i++)
        {
            Destroy(turnOrderParent.GetChild(i).gameObject);
        }

        // Re-add them
        for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
        {
            GameObject go = Instantiate(unitIcon, turnOrderParent);
            go.transform.SetParent(turnOrderParent);

            UnitPortrait unitPortrait = go.GetComponent<UnitPortrait>();    // Reference
            unitPortrait.UpdatePortrait(activeRoomAllUnitFunctionalitys[i].GetUnitSprite());
            unitPortrait.UpdatePortraitColour(activeRoomAllUnitFunctionalitys[i].GetUnitColour());
            unitPortrait.UpdateIconFade(i);

            if (i == 0)
                unitPortrait.UpdateNextUnitArrow(true);
            else
                unitPortrait.UpdateNextUnitArrow(false);
        }
    }

    public void UnSelectUnit(UnitFunctionality unitFunctionality)
    {
        unitFunctionality.ToggleSelected(false);

        unitsSelected.Remove(unitFunctionality);
    }

    public void SelectUnit(UnitFunctionality unitFunctionality)
    {
        // If it is not the player's turn, do not allow the selection
        if (GetActiveUnit().curUnitType == Unit.UnitType.ENEMY)
            return;

        if (activeSkill)
        {
            // If active skill can only select enemies, do not allow players to be selected
            if (activeSkill.curSkillSelectionType == Skill.SkillSelectionType.ENEMIES && unitFunctionality.curUnitType == UnitFunctionality.UnitType.PLAYER)
                return;

            // If active skill can only select allies, do not allow enemies to be selected
            if (activeSkill.curSkillSelectionType == Skill.SkillSelectionType.PLAYERS && unitFunctionality.curUnitType == UnitFunctionality.UnitType.ENEMY)
                return;
        }

        // If user selects a unit that is already selected, unselect it, and go a different path
        if (unitFunctionality.IsSelected())
        {
            UnSelectUnit(unitFunctionality);

            UpdateUnitsSelectedText();
            return;
        }

        // If the selection is maxed, replaced a unit selected with the new one.
        if (unitsSelected.Count != 0)
        {
            if (activeSkill)
            {
                if (unitsSelected.Count == activeSkill.skillSelectionCount)
                    UnSelectUnit(unitsSelected[0]);
            }
            else
            {
                if (unitsSelected.Count == GetActiveUnit().basicSelectionCount)
                    UnSelectUnit(unitsSelected[0]);
            }
        }

        unitsSelected.Add(unitFunctionality);

        // Select targeted unit
        unitFunctionality.ToggleSelected(true);

        UpdateUnitsSelectedText();
    }

    public void UpdateUnitsSelectedText()
    {
        // If a skill is selected
        if (activeSkill != null)
        {
            if (GetActiveUnitFunctionality().GetUnitCurEnergy() >= activeSkill.skillEnergyCost)
                UpdateUnitsSelectedText(unitsSelected.Count, activeSkill.skillSelectionCount);
            else
                UpdateUnitsSelectedText(0, 0);
        }
    }

    public Room GetActiveRoom()
    {
        return activeRoom;
    }

    public void UpdateActiveRoom(Room room)
    {
        activeRoom = room;
    }

    private void UpdateUnitsSelectedText(int curUnitsSelected, int maxUnitsSelected)
    {
        curUnitsTargetedText.text = curUnitsSelected.ToString();
        maxUnitsTargetedText.text = maxUnitsSelected.ToString();
    }

    private int CompareUnitSpeed(Unit unitA, Unit unitB)
    {
        if (unitA.startingSpeed < unitB.startingSpeed)
            return -1;
        else if (unitA.startingSpeed > unitB.startingSpeed)
            return 1;
        return 0;
    }

    private int CompareUnitFunctionalitySpeed(UnitFunctionality unitA, UnitFunctionality unitB)
    {
        if (unitA.curSpeed < unitB.curSpeed)
            return -1;
        else if (unitA.curSpeed > unitB.curSpeed)
            return 1;
        return 0;
    }
}
