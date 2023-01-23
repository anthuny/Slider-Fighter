using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private float postBattleTime;
    public List<UnitData> activeTeam = new List<UnitData>();
    //public List<UnitData> allPlayerClasses = new List<UnitData>();
    public UIElement currentRoom;
    [SerializeField] private GameObject baseUnit;
    [SerializeField] private List<Transform> enemySpawnPositions = new List<Transform>();
    [SerializeField] private List<Transform> allySpawnPositions = new List<Transform>();
    [SerializeField] private Transform allyPostBattlePositionTransform;
    [SerializeField] private Transform allyBattlePositionTransform;
    [SerializeField] private Transform allyPositions;

    [SerializeField] private List<UnitData> activeRoomAllUnits = new List<UnitData>();
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
    public GameObject unitProjectile;
    public float minProjectileKillDist;
    public float randomXDist;
    public float triggerSkillAlertTime = .5f;

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
    public TMP_ColorGradient gradientSkillMiss;
    public TMP_ColorGradient gradientSkillAttack;
    public TMP_ColorGradient gradientSkillSupport;
    public int powerHitFontSize;
    public int powerMissFontSize;

    [Header("Skills UI")]
    public float skillAlertAppearTime;
    public SkillData activeSkill;
    public TMP_ColorGradient gradientSkillAlert;

    [Header("Post Battle")]
    public Rewards rewards;
    public DefeatedEnemies defeatedEnemies;

    [Header("Enemy")]
    public float enemyAttackWaitTime = 1f;
    public float enemyThinkTime = 1f;
    public float unitPowerUIWaitTime = .5f;

    private int experienceGained;
    private int activeRoomEnemiesKilled;
    [HideInInspector]
    public int playerGold;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        map.Setup();
        playerGold = 0;
        map.mapOverlay.ResetPlayerGoldText();
    }

    public void Setup()
    {
        // Destroy previous room
        ResetRoom();

        RoomManager.instance.SelectFloor();

        StartRoom(RoomManager.instance.GetActiveFloor());

        //ToggleUIElement(turnOrder, false);

        //UpdateTurnOrder();

        ToggleUIElement(currentRoom, true);

        ToggleUIElement(playerWeapon, false);
        ToggleMap(false);
        postBattleUI.TogglePostBattleUI(false);
    }

    public void ResetRoom()
    {
        defeatedEnemies.ResetDefeatedEnemies();

        if (activeRoomEnemies.Count <= 0)
            return;

        for (int i = 0; i < enemySpawnPositions.Count; i++)
        {
            Destroy(enemySpawnPositions[i].transform.GetChild(0).gameObject);
        }

        for (int i = 0; i < allySpawnPositions.Count; i++)
        {
            Destroy(allySpawnPositions[i].transform.GetChild(0).gameObject);
        }

        activeRoomAllUnits.Clear();

        for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
        {
            activeRoomAllUnitFunctionalitys.RemoveAt(i);
        }

        activeRoomAllUnitFunctionalitys.Clear();

        /*
        for (int i = 0; i < activeRoomAllies.Count; i++)
        {
            activeRoomAllies.RemoveAt(i);
        }

        activeRoomAllies.Clear();

        */
        for (int i = 0; i < activeRoomEnemies.Count; i++)
        {
            activeRoomEnemies.RemoveAt(i);
        }

        activeRoomEnemies.Clear();
    }

    public void ToggleMap(bool toggle, bool generateMap = false)
    {
        if (!toggle)
        {
            ResetAlliesExpVisual();
                
            map.ToggleMapVisibility(false, generateMap);
            map.gameObject.SetActive(false);
        }
        else
        {
            ResetAlliesExpVisual();

            map.gameObject.SetActive(true);
            map.ToggleMapVisibility(true, generateMap);
        }
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

        postBattleUI.TogglePostBattleConditionText(playerWon);   // Update battle condition text

        defeatedEnemies.DisplayDefeatedEnemies();

        yield return new WaitForSeconds(0);

        // Give Exp to ally units
        for (int i = 0; i < activeRoomAllies.Count; i++)
        {
            activeRoomAllies[i].ToggleUnitBG(true);
            activeRoomAllies[i].UpdateUnitExp(GetExperienceGained());
        }

        yield return new WaitForSeconds(0);

        rewards.FillRewardsTable(5);
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

    public void StartRoom(FloorData activeFloor)
    {
        // Reset experience gained from killed enemies
        ResetExperienceGained();

        // Determine enemy unit value
        int roomChallengeCount = (RoomManager.instance.GetFloorCount() + 1) + RoomManager.instance.GetFloorDifficulty();


        int spawnEnemyPosIndex = 0;
        int spawnEnemyIndex = 0;

        // Spawn enemy type
        for (int i = 0; i < roomChallengeCount; i++)
        {
            UnitData unit = activeFloor.enemyUnits[spawnEnemyIndex];  // Reference

            GameObject go = Instantiate(baseUnit, enemySpawnPositions[spawnEnemyPosIndex]);
            go.transform.SetParent(enemySpawnPositions[spawnEnemyPosIndex]);

            UnitFunctionality unitFunctionality = go.GetComponent<UnitFunctionality>();
            unitFunctionality.ResetPosition();
            unitFunctionality.UpdateUnitName(unit.name);
            unitFunctionality.UpdateUnitSprite(unit.characterPrefab);
            //unitFunctionality.UpdateUnitColour(unit.unitColour);
            unitFunctionality.UpdateUnitType("Enemy");
            unitFunctionality.UpdateUnitSpeed(unit.startingSpeed);
            unitFunctionality.UpdateUnitPower(unit.startingPower);
            unitFunctionality.UpdateUnitArmor(unit.startingArmor);

            unitFunctionality.UpdateUnitVisual(unit.unitSprite);
            unitFunctionality.UpdateUnitIcon(unit.unitIcon);

            unitFunctionality.UpdateUnitHealth(unit.startingMaxHealth, unit.startingMaxHealth);
            unitFunctionality.UpdateUnitStartTurnEnergy(unit.startingUnitStartTurnEnergyGain);

            AddActiveRoomAllUnitsFunctionality(unitFunctionality);
            AddActiveRoomAllUnits(unit);

            unitFunctionality.UpdateMaxEnergy(unit.startingEnergy);
            unitFunctionality.UpdateUnitCurEnergy(unit.startingEnergy);
            unitFunctionality.UpdateUnitLevel(1);

            int unitValue = activeFloor.enemyUnits[spawnEnemyIndex].GetUnitValue() * (RoomManager.instance.GetFloorCount() + 1);

            unitFunctionality.UpdateUnitValue(unitValue);

            //unitFunctionality.UpdateUnitVisuals();

            i += unitValue;

            int rand = Random.Range(0, 2);
            if (rand == 1)
            {
                if (spawnEnemyIndex < activeFloor.enemyUnits.Count - 1)
                    spawnEnemyIndex++;
                else
                    spawnEnemyIndex = 0;
            }

            if (spawnEnemyPosIndex < enemySpawnPositions.Count - 1)
                spawnEnemyPosIndex++;
            else if (spawnEnemyPosIndex >= (enemySpawnPositions.Count - 1))
                break;
        }

        if (activeRoomAllies.Count == 0)
        {
            // Spawn player units
            for (int i = 0; i < activeTeam.Count; i++)
            {
                UnitData unit = activeTeam[i];    // Reference

                GameObject go = Instantiate(baseUnit, allySpawnPositions[i]);
                go.transform.SetParent(allySpawnPositions[i]);

                UnitFunctionality unitFunctionality = go.GetComponent<UnitFunctionality>();
                unitFunctionality.ResetPosition();
                unitFunctionality.UpdateUnitName(unit.name);
                unitFunctionality.UpdateUnitSprite(unit.characterPrefab);
                //unitFunctionality.UpdateUnitColour(unit.unitColour);
                unitFunctionality.UpdateUnitType("Player");
                unitFunctionality.UpdateUnitSpeed(unit.startingSpeed);
                unitFunctionality.UpdateUnitPower(unit.startingPower);
                unitFunctionality.UpdateUnitArmor(unit.startingArmor);

                unitFunctionality.UpdateUnitVisual(unit.unitSprite);
                unitFunctionality.UpdateUnitIcon(unit.unitIcon);

                unitFunctionality.UpdateUnitHealth(unit.startingMaxHealth, unit.startingMaxHealth);
                unitFunctionality.UpdateUnitStartTurnEnergy(unit.startingUnitStartTurnEnergyGain);

                AddActiveRoomAllUnitsFunctionality(unitFunctionality);
                AddActiveRoomAllUnits(unit);

                unitFunctionality.UpdateMaxEnergy(unit.startingEnergy);
                unitFunctionality.UpdateUnitCurEnergy(unit.startingEnergy);
                unitFunctionality.UpdateUnitLevel(1);

                //unitFunctionality.UpdateUnitVisuals();
            }
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

    public void UpdateActiveSkill(SkillData skill)
    {
        activeSkill = skill;
    }

    public IEnumerator WeaponAttackCommand(int power)
    {
        GetActiveUnitFunctionality().effects.UpdateAlpha(0);

        if (GetActiveSkill().curRangedType == SkillData.SkillRangedType.RANGED)
        {
            GetActiveUnitFunctionality().GetAnimator().SetTrigger("SkillFlg");

            yield return new WaitForSeconds(triggerSkillAlertTime);

            // Loop through all selected units, spawn projectiles
            for (int z = unitsSelected.Count - 1; z >= 0; z--)
            {
                if (unitsSelected[z] == null)
                    continue;
                else
                {
                    for (int w = 0; w < GetActiveSkill().skillAttackCount; w++)
                    {
                        GetActiveUnitFunctionality().SpawnProjectile(unitsSelected[z].transform);
                        yield return new WaitForSeconds(0.005f);
                    }
                }
            }

            yield return new WaitForSeconds(unitPowerUIWaitTime);
        }
        else
        {
            GetActiveUnitFunctionality().GetAnimator().SetTrigger("AttackFlg");
        }

        // Loop as many times as power text will appear
        for (int x = 0; x < activeSkill.skillAttackCount; x++)
        {
            // Disable unit selection just before attack
            for (int y = 0; y < unitsSelected.Count; y++)
            {
                unitsSelected[y].ToggleSelected(false);
            }

            // Loop through all selected units
            for (int i = unitsSelected.Count - 1; i >= 0; i--)
            {
                if (unitsSelected[i] == null)
                    continue;
                else
                {
                    // Cause power on selected unit
                    unitsSelected[i].SpawnPowerUI(GetActiveUnitFunctionality().GetUnitPowerInc() * power);

                    if (GetActiveSkill().effect != null)
                    {
                        unitsSelected[i].AddUnitEffect(GetActiveSkill().effect);
                    }

  
                    // Reset unit's prev power text for future power texts
                    if (x == activeSkill.skillAttackCount - 1)
                        unitsSelected[i].ResetPreviousPowerUI();

                    // Increase health from the units current health if a support skill was casted on it
                    if (activeSkill.curSkillType == SkillData.SkillType.SUPPORT)
                        unitsSelected[i].UpdateUnitCurHealth(power);

                    // Decrease health from the units current health if a offense skill was casted on it
                    if (activeSkill.curSkillType == SkillData.SkillType.OFFENSE)
                        unitsSelected[i].UpdateUnitCurHealth(-power);
                }
            }

            // Time wait in between attacks, shared across all targeted units
            yield return new WaitForSeconds(timeBetweenPowerUIStack);
        }

        for (int y = 0; y < unitsSelected.Count; y++)
        {
            if (unitsSelected[y].CheckIfUnitIsDead())
            {
                StartCoroutine(unitsSelected[y].EnsureUnitIsDead());
            }
        }

        yield return new WaitForSeconds(postHitWaitTime);

        //unitsSelected.Clear();

        if (GetActiveUnit().curUnitType == UnitData.UnitType.PLAYER)
        {
            SetupPlayerSkillsUI();
            UpdateSkillDetails(GetActiveUnit().basicSkill);
            UpdateAllSkillIconAvailability();
            UpdateUnitSelection(activeSkill);
            UpdateUnitsSelectedText();
        }
    }
    #region Update Unit UI
    public void UpdateActiveUnitEnergyBar(bool toggle = false, bool increasing = false, int energyAmount = 0, bool enemy = false)
    {
        //Debug.Log(GetActiveUnitFunctionality().GetUnitName() + " " + GetActiveUnitFunctionality().GetUnitCurEnergy());

        GetActiveUnitFunctionality().energyCostImage.UpdateEnergyBar((int)GetActiveUnitFunctionality().
            GetUnitCurEnergy(), energyAmount, increasing, toggle, enemy);
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

    public void UpdateSkillDetails(SkillData skill)
    {
        ToggleUIElement(playerAbilityDesc, true);

        bool tempAttack = false;

        if (skill.curSkillType == SkillData.SkillType.OFFENSE)
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

        GetActiveUnitFunctionality().DecreaseEffectTurnsLeft(false);

        ResetSelectedUnits();

        UnitFunctionality unitFunctionalityMoving = GetActiveUnitFunctionality();
        activeRoomAllUnitFunctionalitys.RemoveAt(0);
        activeRoomAllUnitFunctionalitys.Insert(activeRoomAllUnitFunctionalitys.Count, unitFunctionalityMoving);

        UnitData unitMoving = GetActiveUnit();
        activeRoomAllUnits.RemoveAt(0);
        activeRoomAllUnits.Insert(activeRoomAllUnits.Count, unitMoving);
     
        UpdateTurnOrderVisual();

        // Trigger Unit Energy regen 
        UpdateActiveUnitEnergyBar(true, true, GetActiveUnitFunctionality().unitStartTurnEnergyGain);

        // Toggle player UI accordingly if it's their turn or not
        if (activeRoomAllUnitFunctionalitys[0].curUnitType == UnitFunctionality.UnitType.PLAYER)
        {
            //activeRoomAllUnitFunctionalitys[0].StartUnitTurn();
            playerUIElement.UpdateAlpha(1);
            SetupPlayerSkillsUI();
            UpdatePlayerAbilityUI();
            UpdateActiveUnitNameText(GetActiveUnitFunctionality().GetUnitName());

            // Set the basic skill to be on by default to begin with
            activeSkill = GetActiveUnit().basicSkill;

            ToggleEndTurnButton(true);      // Toggle End Turn Button on
        }
        else
        {
            activeRoomAllUnitFunctionalitys[0].ToggleIdleBattle(true);
            playerUIElement.UpdateAlpha(0);

            ToggleEndTurnButton(false);      // Toggle End Turn Button on

            StartCoroutine(activeRoomAllUnitFunctionalitys[0].StartUnitTurn());
        }

        // If unit is at maxed energy, stop
        if (GetActiveUnitFunctionality().GetUnitCurEnergy() != GetActiveUnitFunctionality().GetUnitMaxEnergy())
            UpdateActiveUnitHealthBar(false);   // Disable unit health bar for unit start turn energy increase



        if (GetActiveUnit().curUnitType == UnitData.UnitType.PLAYER)
        {
            UpdateSkillDetails(activeSkill);
            UpdateAllSkillIconAvailability();
            UpdateUnitSelection(activeSkill);
        }

        UpdateActiveUnitTurnArrow();

        //Trigger Start turn effects
        GetActiveUnitFunctionality().DecreaseEffectTurnsLeft(true);
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

    public void ResetAlliesExpVisual()
    {
        for (int i = 0; i < activeRoomAllies.Count; i++)
        {
            activeRoomAllies[i].ToggleUnitExpVisual(false);
            activeRoomAllies[i].StopCoroutine(activeRoomAllies[i].UpdateUnitExpVisual(0));
        }
    }

    public void UpdateUnitSelection(SkillData usedSkill)
    {
        int selectedAmount = 0;

        // Clear all current selections
        ResetSelectedUnits();

        // If skill selection type is only on ENEMIES
        if (usedSkill.curSkillSelectionType == SkillData.SkillSelectionType.ENEMIES)
        {
            // if the skill user is a PLAYER
            if (GetActiveUnit().curUnitType == UnitData.UnitType.PLAYER)
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
            else if (GetActiveUnit().curUnitType == UnitData.UnitType.ENEMY)
            {
                // only select PLAYER units, in random fashion
                for (int x = 0; x < activeRoomAllies.Count; x++)
                {
                    selectedAmount++;

                    int rand = Random.Range(0, activeRoomAllies.Count);
                    SelectUnit(activeRoomAllies[rand]);

                    // If enough units have been selected, toggle the display
                    if (selectedAmount == usedSkill.skillSelectionCount)
                        return;
                }
            }
        }

        // If skill selection type is only on ALLIES
        if (usedSkill.curSkillSelectionType == SkillData.SkillSelectionType.PLAYERS)
        {
            // if the skill user is a PLAYER
            if (GetActiveUnit().curUnitType == UnitData.UnitType.PLAYER)
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
            else if (GetActiveUnit().curUnitType == UnitData.UnitType.ENEMY)
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

    public UnitData GetActiveUnit()
    {
        return activeRoomAllUnits[0];
    }

    public SkillData GetActiveSkill()
    {
        return activeSkill;
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

    public void ResetEnemiesKilledCount()
    {
        activeRoomEnemiesKilled = 0;
    }

    public int GetEnemiesKilledCount()
    {
        return activeRoomEnemiesKilled;
    }

    private void AddActiveRoomAllUnits(UnitData unit)
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
        playerIcon.UpdatePortrait(GetActiveUnitFunctionality().GetUnitIcon());
        playerIcon.UpdateColour(GetActiveUnitFunctionality().GetUnitColour());

        // Update player skill portraits
        playerSkill1.UpdatePortrait(GetActiveUnit().GetSkill1().skillSprite);
        playerSkill2.UpdatePortrait(GetActiveUnit().GetSkill2().skillSprite);
        playerSkill3.UpdatePortrait(GetActiveUnit().GetSkill3().skillSprite);

        playerSkill1.ToggleSelectImage(false);
        playerSkill2.ToggleSelectImage(false);
        playerSkill3.ToggleSelectImage(false);
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
            unitPortrait.UpdatePortrait(activeRoomAllUnitFunctionalitys[i].GetUnitIcon());
            //unitPortrait.UpdatePortraitColour(activeRoomAllUnitFunctionalitys[i].GetUnitColour());
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
        /*
        // If it is not the player's turn, do not allow the selection
        if (GetActiveUnit().curUnitType == UnitData.UnitType.ENEMY)
            return;
        */
        if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
        {
            if (activeSkill)
            {
                // If active skill can only select enemies, do not allow players to be selected
                if (activeSkill.curSkillSelectionType == SkillData.SkillSelectionType.ENEMIES && unitFunctionality.curUnitType == UnitFunctionality.UnitType.PLAYER)
                    return;

                // If active skill can only select allies, do not allow enemies to be selected
                if (activeSkill.curSkillSelectionType == SkillData.SkillSelectionType.PLAYERS && unitFunctionality.curUnitType == UnitFunctionality.UnitType.ENEMY)
                    return;
            }
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

    private void UpdateUnitsSelectedText(int curUnitsSelected, int maxUnitsSelected)
    {
        curUnitsTargetedText.text = curUnitsSelected.ToString();
        maxUnitsTargetedText.text = maxUnitsSelected.ToString();
    }

    private int CompareUnitSpeed(UnitData unitA, UnitData unitB)
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
