using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private float postBattleTime;
    public List<UnitData> activeTeam = new List<UnitData>();
    //public List<UnitData> allPlayerClasses = new List<UnitData>();
    public UIElement currentRoom;
    [SerializeField] private GameObject baseUnit;
    [SerializeField] private List<Transform> enemySpawnPositions = new List<Transform>();
    [SerializeField] private List<Transform> allySpawnPositions = new List<Transform>();
    [SerializeField] private Transform allyPostBattlePositionTransform;
    [SerializeField] private Transform allyTurnPositionTransform;
    [SerializeField] private Transform enemyTurnPositionTransform;
    [SerializeField] private Transform allyPositions;

    public List<UnitFunctionality> activeRoomAllUnitFunctionalitys = new List<UnitFunctionality>();
    public List<UnitFunctionality> oldActiveRoomAllUnitFunctionalitys = new List<UnitFunctionality>();
    public List<UnitFunctionality> activeRoomAllies = new List<UnitFunctionality>();
    public List<UnitFunctionality> activeRoomEnemies = new List<UnitFunctionality>();
    private List<UnitFunctionality> oldActiveRoomEnemies = new List<UnitFunctionality>();

    [SerializeField] private GameObject unitIcon;

    [Header("Team Setup")]
    [SerializeField] private UIElement teamSetup;
    public UIElement toMapButton;
    [SerializeField] private Transform teamSetupAllyPosition;

    [Header("Map")]
    public MapManager map;

    [Header("Shop")]
    [SerializeField] private float shopRemoveSelectTime = 1.5f;

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
    public float allyMeleeSkillWaitTime = .5f;
    public float allyRangedSkillWaitTime;
    public float enemyMeleeSkillWaitTime = .5f;
    public float enemyRangedSkillWaitTime;

    [Header("Player UI")]
    public UIElement playerUIElement;
    public UIElement playerWeapon;
    public UIElement playerWeaponChild;
    public UIElement playerWeaponBackButton;
    public UIElement playerWeaponBG;
    public UIElement playerAbilities;
    public UIElement playerAbilityDesc;
    public PostBattle postBattleUI;
    public OverlayUI abilityDetailsUI;
    public Text curUnitsTargetedText;
    public Text maxUnitsTargetedText;
    public UIElement endTurnButtonUI;
    public TextMeshProUGUI activeUnitNameText;

    [Header("Player Ability UI")]
    public IconUI playerIcon;
    public IconUI playerSkill0;
    public IconUI playerSkill1;
    public IconUI playerSkill2;
    public IconUI playerSkill3;

    [Header("Skill Buttons")]
    public ButtonFunctionality skill0;
    public ButtonFunctionality skill1;
    public ButtonFunctionality skill2;
    public ButtonFunctionality skill3;
    public UIElement skill0IconUnavailableImage;
    public UIElement skill1IconUnavailableImage;
    public UIElement skill2IconUnavailableImage;
    public UIElement skill3IconUnavailableImage;
    public UIText skill0IconCooldownUIText;
    public UIText skill1IconCooldownUIText;
    public UIText skill2IconCooldownUIText;
    public UIText skill3IconCooldownUIText;

    public List<UnitFunctionality> unitsSelected = new List<UnitFunctionality>();

    [Header("Power UI")]
    [Tooltip("Randomness percentage for power output")]
    public float randomPerc;
    public GameObject powerUITextPrefab;
    [SerializeField] private float timeBetweenPowerUIStack;
    [SerializeField] private float timeBetweenProjectile;
    public float powerUIHeightLvInc;
    [SerializeField] private float postHitWaitTime;
    public string missPowerText;
    public string parryPowerText;
    public TMP_ColorGradient gradientSkillMiss;
    public TMP_ColorGradient gradientSkillParry;
    public TMP_ColorGradient gradientSkillAttack;
    public TMP_ColorGradient gradientSkillSupport;
    public int powerHitFontSize;
    public int powerMissFontSize;
    public int powerSkillParryFontSize;
    public int maxPowerUICount = 5;
    public float powerHorizontalRandomness;

    [Header("Skills UI")]
    public float skillAlertAppearTime;
    public SkillData activeSkill;
    public TMP_ColorGradient gradientSkillAlert;
    public TMP_ColorGradient gradientLevelUpAlert;

    [Header("Post Battle")]
    public Rewards rewards;
    public DefeatedEnemies defeatedEnemies;

    [Header("Enemy")]
    public float enemyAttackWaitTime = 1f;
    public float enemyEffectWaitTime = 1f;
    public float enemySkillThinkTime = 1f;
    public float unitPowerUIWaitTime = .5f;

    public ButtonFunctionality attackButton;
    public ButtonFunctionality weaponBackButton;
    //public ButtonFunctionality endTurnButton;
    public ButtonFunctionality skill0Button;
    public ButtonFunctionality skill1Button;
    public ButtonFunctionality skill2Button;
    public ButtonFunctionality skill3Button;

    private int experienceGained;
    private int activeRoomEnemiesKilled;
    [SerializeField] private Transform enemyPositionMainCombatTrans;
    [SerializeField] private Transform enemyPositionPlayerTurnTrans;
    [SerializeField] private Transform enemyPositionsCurrent;

    public bool playerLost;

    private bool allowSelection;
    bool hasBeenLuckyHit;
    bool selectingUnitsAllowed;
    bool firstTimeRoomStart = true;
    public Weapon activeWeapon;
    public int powerUISpawnCount;


    private void Awake()
    {
        Instance = this;

        //SpawnAllies(true);
    }
    private void Start()
    {
        Application.targetFrameRate = 60;

        //map.Setup();
        //ShopManager.Instance.UpdatePlayerGold(0);
        //map.mapOverlay.ResetPlayerGoldText();
        ToggleTeamSetup(false);
    }

    public Weapon GetActiveWeapon()
    {
        return activeWeapon;
    }

    public void SetActiveWeapon()
    {
        activeWeapon = playerWeapon.GetComponent<Weapon>();
    }

    public UnitData GetUnitData(int count)
    {
        return activeTeam[count];
    }

    public void ResetButton(ButtonFunctionality button)
    {
        button.EnableButton();
    }

    public void DisableButton(ButtonFunctionality button)
    {
        button.DisableButton();
    }

    public void EnableButton(ButtonFunctionality button)
    {
        button.EnableButton();
    }
    public void UpdateEnemyPosition(bool playerTurn)
    {
        if (playerTurn)
        {
            enemyPositionsCurrent.position = enemyPositionPlayerTurnTrans.position;
        }
        else
        {
            enemyPositionsCurrent.position = enemyPositionMainCombatTrans.position;
        }
    }

    public void Setup()
    {
        // Destroy previous room
        ResetRoom();

        RoomManager.Instance.SelectFloor();

        StartRoom(RoomManager.Instance.GetActiveRoom(), RoomManager.Instance.GetActiveFloor());

        //ToggleUIElement(turnOrder, false);

        //UpdateTurnOrder();

        ToggleUIElement(currentRoom, true);

        ToggleUIElement(playerWeapon, false);

        ToggleMap(false);
        postBattleUI.TogglePostBattleUI(false);
    }

    public void SpawnAllies(bool allAllies = true)
    {
        // Spawn player units
        if (activeRoomAllies.Count == 0)
        {
            for (int i = 0; i < activeTeam.Count; i++)
            {
                UnitData unit = activeTeam[i];    // Reference

                GameObject go = Instantiate(baseUnit, allySpawnPositions[i]);
                go.transform.SetParent(allySpawnPositions[i]);

                UnitFunctionality unitFunctionality = go.GetComponent<UnitFunctionality>();
                
                // Set ally correct position based on team size
                if (i == 0)
                    unitFunctionality.SetPositionAndParent(allySpawnPositions[1]);
                else if (i == 1)
                    unitFunctionality.SetPositionAndParent(allySpawnPositions[0]);
                else if (i == 2)
                    unitFunctionality.SetPositionAndParent(allySpawnPositions[2]);

                unitFunctionality.UpdateUnitName(unit.unitName);
                unitFunctionality.UpdateUnitSprite(unit.characterPrefab);
                unitFunctionality.UpdateUnitColour(unit.unitColour);
                unitFunctionality.UpdateUnitType("Player");
                unitFunctionality.UpdateUnitSpeed(unit.startingSpeed);
                unitFunctionality.UpdateUnitPower(unit.startingPower);
                unitFunctionality.UpdateUnitDefense(unit.startingDefense);
                unitFunctionality.UpdateCurrentMasteries(unit.GetStandardStats());
                unitFunctionality.UpdateUnitVisual(unit.unitSprite);
                unitFunctionality.UpdateUnitIcon(unit.unitIcon);

                unitFunctionality.UpdateUnitHealth(unit.startingMaxHealth, unit.startingMaxHealth);
                //unitFunctionality.UpdateUnitStartTurnEnergy(unit.startingUnitStartTurnEnergyGain);

                AddActiveRoomAllUnitsFunctionality(unitFunctionality);
                //AddActiveRoomAllUnits(unit);

                //unitFunctionality.UpdateMaxEnergy(unit.startingEnergy);
                //unitFunctionality.UpdateUnitCurEnergy(unit.startingEnergy);
                unitFunctionality.UpdateUnitLevel(1);

                //unitFunctionality.UpdateUnitProjectileSprite(unit.projectileSprite);

                // reposition ally to team setup position
                //go.transform.position = teamSetupAllyPosition.position;
                ToggleAllowSelection(false);
                unitFunctionality.ToggleUnitHealthBar(false);

                unitFunctionality.unitData = unit;

                if (i == 0)
                    TeamSetup.Instance.UpdateActiveUnit(GetActiveUnitFunctionality());
            }
        }
        
    }

    public void UpdateMasteryAllyPosition()
    {
        for (int i = 0; i < activeRoomAllies.Count; i++)
        {
            if (i == 1)
            {
                UnitFunctionality oldUnit = activeRoomAllUnitFunctionalitys[0];
                activeRoomAllUnitFunctionalitys[0] = activeRoomAllUnitFunctionalitys[1];
                activeRoomAllUnitFunctionalitys[1] = oldUnit;


            }
            else if (i == 2)
            {
                UnitFunctionality oldUnit = activeRoomAllUnitFunctionalitys[1];
                activeRoomAllUnitFunctionalitys[1] = activeRoomAllUnitFunctionalitys[2];
                activeRoomAllUnitFunctionalitys[2] = oldUnit;
            }

            // Update position 1 to be visiible, rest not
            activeRoomAllUnitFunctionalitys[0].UpdateIsVisible(true);
            activeRoomAllUnitFunctionalitys[0].transform.position = allyPositions.GetChild(0).transform.position;
            if (i != 0)
                activeRoomAllUnitFunctionalitys[i].UpdateIsVisible(false);
        }
    }

    public void UpdateAllysPositionCombat()
    {
        if (activeRoomAllUnitFunctionalitys.Count >= 1)
        {
            activeRoomAllUnitFunctionalitys[0].transform.position = allyPositions.GetChild(0).transform.position;
            activeRoomAllUnitFunctionalitys[0].ResetPosition();

            if (activeRoomAllUnitFunctionalitys.Count >= 2)
            {
                activeRoomAllUnitFunctionalitys[1].transform.position = allyPositions.GetChild(1).transform.position;
                activeRoomAllUnitFunctionalitys[1].ResetPosition();

                if (activeRoomAllUnitFunctionalitys.Count >= 3)
                {
                    activeRoomAllUnitFunctionalitys[2].transform.position = allyPositions.GetChild(2).transform.position;
                    activeRoomAllUnitFunctionalitys[2].ResetPosition();
                }
            }
        }
    }
    public void ToggleTeamSetup(bool toggle)
    {
        TeamSetup.Instance.statScrollView.SetActive(toggle);

        if (toggle)
        {
            teamSetup.UpdateAlpha(1);
            //SpawnAllies(false);

            UpdateAllAlliesPosition(false, true, true);
            TeamSetup.Instance.UpdateActiveUnit(GetActiveUnitFunctionality());

            oldActiveRoomAllUnitFunctionalitys = activeRoomAllUnitFunctionalitys;

            // Track the last mastery type from the active unit
            TeamSetup.ActiveStatType masteryType = TeamSetup.ActiveStatType.STANDARD;
            if (TeamSetup.Instance.GetActiveUnit().GetLastOpenedMastery() == UnitFunctionality.LastOpenedMastery.STANDARD)
                masteryType = TeamSetup.ActiveStatType.STANDARD;
            else if (TeamSetup.Instance.GetActiveUnit().GetLastOpenedMastery() == UnitFunctionality.LastOpenedMastery.ADVANCED)
                masteryType = TeamSetup.ActiveStatType.ADVANCED;

            TeamSetup.Instance.UpdateStatPage(masteryType);

            TeamSetup.Instance.UpdateAllyNameText();
        }
        else
        {
            teamSetup.UpdateAlpha(0);
        }
    }

    public void UpdateAllyVisibility(bool toggle, bool teamPage = false, bool shop = false)
    {   
        //  If enabling
        if (toggle)
        {
            if (shop)
            {
                // Toggle ally visibility
                for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
                {
                    activeRoomAllUnitFunctionalitys[i].UpdateIsVisible(true);
                    activeRoomAllUnitFunctionalitys[i].ToggleUnitHealthBar(true);
                    activeRoomAllUnitFunctionalitys[i].ToggleUnitAttackBar(false);
                }
            }
            if (teamPage)   // Team page
            {
                // Toggle ally visibility
                for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
                {
                    activeRoomAllUnitFunctionalitys[0].UpdateIsVisible(true);
                    activeRoomAllUnitFunctionalitys[0].ToggleUnitHealthBar(false);
                    activeRoomAllUnitFunctionalitys[0].ToggleUnitAttackBar(false);

                    activeRoomAllUnitFunctionalitys[0].gameObject.transform.position = allyPositions.GetChild(0).transform.position;

                    activeRoomAllUnitFunctionalitys[i].UpdateFacingDirection(true);

                    if (i != 0)
                    {
                        activeRoomAllUnitFunctionalitys[i].UpdateIsVisible(false);
                    }
                }
            }
            else    // Combat
            {
                // Toggle ally visibility
                for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
                {
                    activeRoomAllUnitFunctionalitys[i].UpdateIsVisible(true);
                    activeRoomAllUnitFunctionalitys[i].ToggleUnitHealthBar(true);
                    activeRoomAllUnitFunctionalitys[i].ToggleUnitAttackBar(true);
                }
            }
        }
        else    // if disabling
        {
            // Toggle ally visibility
            for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
            {
                activeRoomAllUnitFunctionalitys[i].UpdateIsVisible(toggle);
            }
        }
    }

    public void ResetRoom(bool enemies = true)
    {
        defeatedEnemies.ResetDefeatedEnemies();

        powerUISpawnCount = 0;

        if (!enemies)
        {
            for (int i = 0; i < allySpawnPositions.Count; i++)
            {
                if (allySpawnPositions[i].childCount >= 1)
                    Destroy(allySpawnPositions[i].transform.GetChild(0).gameObject);
            }

            for (int i = 0; i < activeRoomAllies.Count; i++)
            {
                activeRoomAllies.RemoveAt(i);
            }

            activeRoomAllies.Clear();
        }

        if (enemies)
        {
            for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
            {
                if (activeRoomAllUnitFunctionalitys[i].curUnitType == UnitFunctionality.UnitType.ENEMY)
                    activeRoomAllUnitFunctionalitys.RemoveAt(i);
            }
            //activeRoomAllUnitFunctionalitys.Clear();

            for (int i = 0; i < enemySpawnPositions.Count; i++)
            {
                if (enemySpawnPositions[i].childCount >= 1)
                    Destroy(enemySpawnPositions[i].transform.GetChild(0).gameObject);
            }

            for (int i = 0; i < activeRoomEnemies.Count; i++)
            {
                activeRoomEnemies.RemoveAt(i);
            }

            activeRoomEnemies.Clear();
        }
    }

    public void ToggleMap(bool toggle, bool generateMap = false, bool increaseFloor = false)
    {
        if (!toggle)
        {
            ResetAlliesExpVisual();            

            map.mapOverlay.ToggleBottomBG(false);
            map.gameObject.SetActive(false);

            map.ToggleMapVisibility(false, false);

            ToggleSkillSelection(false);
        }
        else
        {
            if (!increaseFloor)
                ResetAlliesExpVisual();

            map.mapOverlay.ToggleBottomBG(true);
            map.gameObject.SetActive(true);

            map.ToggleMapVisibility(true, generateMap, increaseFloor);

            ToggleSkillSelection(true);
        }
    }

    void UpdateAllAlliesPosition(bool postBattle, bool playersTurn = true, bool masteryPosition = false, bool shopPosition = false)
    {
        if (shopPosition)
        {
            allyPositions.SetParent(ShopManager.Instance.unitsPositionShopTrans);
            allyPositions.SetPositionAndRotation(new Vector2(0, 0), Quaternion.identity);
            allyPositions.position = ShopManager.Instance.unitsPositionShopTrans.position;
            return;
        }
        if (!postBattle)
        {
            if (masteryPosition)
            {
                allyPositions.SetParent(TeamSetup.Instance.statAllyPosTrans);
                allyPositions.SetPositionAndRotation(new Vector2(0, 0), Quaternion.identity);
                allyPositions.position = TeamSetup.Instance.statAllyPosTrans.position;

                return;
            }

            if (playersTurn)
            {
                allyPositions.SetParent(allyTurnPositionTransform);
                allyPositions.SetPositionAndRotation(new Vector2(0, 0), Quaternion.identity);
                allyPositions.position = allyTurnPositionTransform.position;
            }
            else
            {
                allyPositions.SetParent(enemyTurnPositionTransform);
                allyPositions.SetPositionAndRotation(new Vector2(0, 0), Quaternion.identity);
                allyPositions.position = enemyTurnPositionTransform.position;
            }
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

        RoomManager.Instance.ToggleInteractable(false);
    }

    // Toggle UI accordingly
    IEnumerator SetupRoomPostBattle(bool playerWon)
    {
        // Remove ally unit's effects
        /*
        for (int i = 0; i < activeRoomAllies.Count; i++)
        {
            activeRoomAllies[i].ResetEffects();
        }
        */
      
        // Toggle player overlay and skill ui off
        ToggleUIElement(playerAbilities, false);
        ToggleUIElement(playerAbilityDesc, false);
        ToggleUIElement(endTurnButtonUI, false);
        ToggleUIElement(turnOrder, false);
        ResetActiveUnitTurnArrow();
        ToggleAllAlliesStatBar(true);
       

        // Toggle post battle ui on
        postBattleUI.TogglePostBattleUI(true);

        postBattleUI.TogglePostBattleConditionText(playerWon);   // Update battle condition text

        defeatedEnemies.DisplayDefeatedEnemies();

        if (playerWon)
        {
            postBattleUI.ToggleExpGainedUI(true);
            postBattleUI.ToggleRewardsUI(true);

            yield return new WaitForSeconds(0);

            // Give Exp to ally units
            for (int i = 0; i < activeRoomAllies.Count; i++)
            {
                activeRoomAllies[i].ToggleUnitBG(true);
                activeRoomAllies[i].UpdateUnitExp(GetExperienceGained());
                activeRoomAllies[i].ToggleHideEffects(playerWon);
            }

            yield return new WaitForSeconds(0);

            playerLost = false;

            rewards.FillRewardsTable(5);
        }
        // If player LOST, reset game
        else
        {
            // Set correct post battle UI
            postBattleUI.ToggleExpGainedUI(false);
            postBattleUI.ToggleRewardsUI(false);

            playerLost = true;

            // Reset map
            //MapManager.instance.ToggleMapVisibility(true, true);
        }
    }

    // After the user strikes their weapon, remove skill details and skills UI for dmg showcase
    public void SetupPlayerPostHitUI()
    {
        ToggleUIElement(playerWeaponBG, false);
        ToggleUIElement(playerWeapon, false);
        ToggleUIElementFull(playerWeaponChild, false);
        ToggleUIElement(playerWeaponBackButton, false);
        ToggleUIElement(playerAbilities, false);
        ToggleUIElement(playerAbilityDesc, false);

        ToggleEndTurnButton(false);
    }

    public void SetupPlayerSkillsUI(SkillData activeSkill = null)
    {
        ToggleUIElement(playerWeaponBG, false);
        ToggleUIElement(playerWeapon, false);
        ToggleUIElementFull(playerWeaponChild, false);
        ToggleUIElement(playerWeaponBackButton, false);

        Weapon.Instance.ToggleAttackButtonInteractable(false);

        if (activeSkill != null)
            this.activeSkill = activeSkill;
        else
            this.activeSkill = GetActiveUnitFunctionality().unitData.skill0;

        ToggleUIElement(playerAbilities, true);
        ToggleUIElement(playerAbilityDesc, true);
        ToggleEndTurnButton(true);

        UpdateUnitsSelectedText();

        UpdateEnemyPosition(true);
    }

    public void SetupPlayerWeaponUI()
    {
        if (!CheckIfAnyUnitsSelected())
            return;

        Weapon.Instance.DisableAlertUI();
        Weapon.Instance.ToggleEnabled(true);
        Weapon.Instance.StartHitLine();

        ToggleUIElement(playerAbilities, false);
        ToggleUIElement(playerAbilityDesc, false);

        ToggleUIElement(playerWeaponBG, true);
        ToggleUIElementFull(playerWeaponChild, true);

        ToggleUIElement(playerWeapon, true);
        SetActiveWeapon();

        ToggleUIElement(playerWeaponBackButton, true);
        ToggleEndTurnButton(false);

        Weapon.Instance.ToggleAttackButtonInteractable(true);
    }
    #endregion

    /*
    public void ReturnEnergyToUnit()
    {
        UpdateActiveUnitHealthBar(false);
        UpdateActiveUnitEnergyBar(true, true, activeSkill.skillEnergyCost);
    }
    */

    public void ClearRoom()
    {
        //ResetRoom
    }

    public void StartRoom(RoomMapIcon room, FloorData activeFloor)
    {
        // Reset experience gained from killed enemies
        ResetExperienceGained();

        // If room type is enemy, spawn enemy room
        if (room.curRoomType == RoomMapIcon.RoomType.ENEMY || room.curRoomType == RoomMapIcon.RoomType.BOSS)
        {
            // Update background
            BackgroundManager.Instance.UpdateBackground(BackgroundManager.Instance.GetCombatForest());

            // Determine enemy unit value
            int roomChallengeCount = (RoomManager.Instance.GetFloorCount()) + RoomManager.Instance.GetFloorDifficulty();

            int spawnEnemyPosIndex = 0;


            // Spawn enemy type
            for (int i = 0; i < roomChallengeCount; i++)
            {
                GameObject go = null;

                int spawnEnemyIndex = Random.Range(0, activeFloor.enemyUnits.Count);

                UnitData unit = activeFloor.enemyUnits[spawnEnemyIndex];  // Reference

                if (spawnEnemyPosIndex <= enemySpawnPositions.Count -1)
                {
                    go = Instantiate(baseUnit, enemySpawnPositions[spawnEnemyPosIndex]);
                    go.transform.SetParent(enemySpawnPositions[spawnEnemyPosIndex]);
                    spawnEnemyPosIndex++;
                }                
                else
                {
                    Destroy(go);
                    break;
                }

                UnitFunctionality unitFunctionality = go.GetComponent<UnitFunctionality>();
                unitFunctionality.UpdateUnitType("Enemy");
                unitFunctionality.ResetPosition();
                unitFunctionality.UpdateUnitName(unit.unitName);
                unitFunctionality.UpdateUnitSprite(unit.characterPrefab);
                //unitFunctionality.UpdateUnitColour(unit.unitColour);
                unitFunctionality.UpdateUnitSpeed(unit.startingSpeed);
                unitFunctionality.UpdateUnitPower(unit.startingPower);
                unitFunctionality.UpdateUnitDefense(unit.startingDefense);

                unitFunctionality.UpdateUnitVisual(unit.unitSprite);
                unitFunctionality.UpdateUnitIcon(unit.unitIcon);

                unitFunctionality.UpdateUnitHealth(unit.startingMaxHealth, unit.startingMaxHealth);
                //unitFunctionality.UpdateUnitStartTurnEnergy(unit.startingUnitStartTurnEnergyGain);

                AddActiveRoomAllUnitsFunctionality(unitFunctionality);
                //AddActiveRoomAllUnits(unit);

                //unitFunctionality.UpdateMaxEnergy(unit.startingEnergy);
                //unitFunctionality.UpdateUnitCurEnergy(unit.startingEnergy);
                unitFunctionality.UpdateUnitLevel(1);

                int unitValue = activeFloor.enemyUnits[spawnEnemyIndex].GetUnitValue() + (RoomManager.Instance.GetFloorCount());

                unitFunctionality.UpdateUnitValue(unitValue);
                //unitFunctionality.UpdateUnitProjectileSprite(unit.projectileSprite);

                if (i != 0)
                    i--;

                i += unitValue;

                unitFunctionality.unitData = unit;
            }

            for (int x = 0; x < activeRoomAllUnitFunctionalitys.Count; x++)
            {
                // Update unit energy bar on
                UpdateActiveUnitStatBar(activeRoomAllUnitFunctionalitys[x], true, true);
                activeRoomAllUnitFunctionalitys[x].ResetIsDead();

                activeRoomAllUnitFunctionalitys[x].ToggleHideEffects(false);
                activeRoomAllUnitFunctionalitys[x].ResetUnitCurAttackCharge();

                activeRoomAllUnitFunctionalitys[x].CalculateUnitAttackChargeTurnStart();
                activeRoomAllUnitFunctionalitys[x].UpdateUnitCurAttackCharge();
            }

            // Randomise enemy spawn positions
            oldActiveRoomEnemies = activeRoomEnemies;
            Shuffle(activeRoomEnemies);

            List<int> rands = new List<int>();

            for (int i = 0; i < oldActiveRoomEnemies.Count; i++)
            {
                int rand = Random.Range(0, enemySpawnPositions.Count);

                if (rands.Contains(rand))
                {
                    if (i != 0)
                        i--;
                    continue;
                }

                rands.Add(rand);

                activeRoomEnemies[i].SetPositionAndParent(enemySpawnPositions[rand].transform);
            }


            RoomManager.Instance.ToggleInteractable(true);

            UpdateAllyVisibility(true, false);

            playerUIElement.UpdateAlpha(1);     // Enable player UI
            ToggleUIElement(turnOrder, true);  // Enable turn order

            // Update allies into position for battle
            UpdateAllAlliesPosition(false, GetActiveUnitType());

            ToggleAllowSelection(true);

            MapManager.Instance.mapOverlay.ToggleTeamPageButton(false);

            UpdateTurnOrder();
        }

        // If room type is shop, spawn shop room
        else if (room.curRoomType == RoomMapIcon.RoomType.SHOP)
        {
            // Update background
            BackgroundManager.Instance.UpdateBackground(BackgroundManager.Instance.GetShopForest());

            UpdateAllyVisibility(true, false);

            // Update unit energy bar off
            for (int i = 0; i < activeRoomAllies.Count; i++)
            {
                UpdateActiveUnitStatBar(activeRoomAllies[i], true, false);
            }

            playerUIElement.UpdateAlpha(0);     // Disable player UI
            ToggleUIElement(turnOrder, false);  // Disable turn order
            ResetSelectedUnits();   // Disable all unit selections
            //ToggleAllAlliesHealthBar(false);    // Disable all unit health bar visual
            ToggleAllowSelection(false);

            ShopManager.Instance.FillShopItems(false, true);

            // Update allies into position for shop
            UpdateAllAlliesPosition(false, GetActiveUnitType(), false, true);
            return;
        }
    }

    public bool GetActiveUnitType()
    {
        bool toggle = false;
        if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            toggle = true;
        else
            toggle = false;
        
        return toggle;
    }

    /*
    public bool CheckIfEnergyAvailableSkill()
    {
        if (GetActiveUnitFunctionality().GetUnitCurEnergy() >= activeSkill.skillEnergyCost)
            return true;
        else
            return false;
    }
    */

    public void UpdateActiveSkill(SkillData skill)
    {
        activeSkill = skill;
    }

    public int AdjustSkillPowerTargetEffectAmp(float power)
    {
        for (int i = 0; i < unitsSelected.Count; i++)
        {
            for (int y = 0; y < unitsSelected[i].GetEffects().Count; y++)
            {
                if (unitsSelected[i].GetEffects()[y].effectName == GetActiveSkill().curSkillExtraPowerToEffect.ToString())
                {
                    float adjustedPower = (1f + (GetActiveSkill().percIncPower / 100f)) * power;
                    return RandomisePower((int)adjustedPower);
                }
                else
                    RandomisePower((int)power);
            }
        }

        return RandomisePower((int)power);
    }

    int RandomisePower(int power)
    {
        float powerToRandomise = ((randomPerc / 100f) * power);
        int powerToRandomiseInt = (int)powerToRandomise;
        float randPower = Random.Range(power - powerToRandomiseInt, (power + powerToRandomiseInt+1));
        return (int)randPower;
    }

    public UnitFunctionality IsAllyTaunting()
    {
        for (int i = 0; i < activeRoomAllies.Count; i++)
        {
            if (activeRoomAllies[i].isTaunting)
                return activeRoomAllies[i];
            else
                continue;
        }

        return null;
    }

    public List<UnitFunctionality> IsEnemyTaunting()
    {
        List<UnitFunctionality> units = new List<UnitFunctionality>();

        for (int i = 0; i < activeRoomEnemies.Count; i++)
        {
            if (activeRoomEnemies[i].isTaunting)
            {
                units.Add(activeRoomEnemies[i]);
            }
            else
                continue;
        }

        return units;
    }
        
    public IEnumerator WeaponAttackCommand(int power, int hitMultCount = 0, int effectHitAcc = -1)
    {
        UnitFunctionality castingUnit = GetActiveUnitFunctionality();

        GetActiveUnitFunctionality().effects.UpdateAlpha(1);

        // If the skill is supposed to deal no power, don't spawn projectiles for it
        if (GetActiveSkill().skillPower != 0)
        {
            if (GetActiveSkill().curRangedType == SkillData.SkillRangedType.RANGED)
            {
                GetActiveUnitFunctionality().GetAnimator().SetTrigger("SkillFlg");

                yield return new WaitForSeconds(allyRangedSkillWaitTime);

                for (int w = 0; w < GetActiveSkill().skillAttackCount; w++)
                {
                    // Loop through all selected units, spawn projectiles, if target is dead stop.
                    for (int z = unitsSelected.Count - 1; z >= 0; z--)
                    {
                        if (unitsSelected[z] == null)
                            continue;
                        else
                        {
                            GetActiveUnitFunctionality().SpawnProjectile(unitsSelected[z].transform);
                        }
                    }

                    yield return new WaitForSeconds(timeBetweenProjectile);
                }

                yield return new WaitForSeconds(unitPowerUIWaitTime);
            }
            else
            {
                GetActiveUnitFunctionality().GetAnimator().SetTrigger("AttackFlg");
                yield return new WaitForSeconds(allyMeleeSkillWaitTime / 2f);
            }
        }

        /*
        if (GetActiveSkill().curRangedType == SkillData.SkillRangedType.RANGED)
            yield return new WaitForSeconds(rangedSkillWaitTime);
        else
            yield return new WaitForSeconds(meleeSkillWaitTime);
        */

        // For no power skills
        if (GetActiveSkill().skillPower == 0)
        {
            GetActiveUnitFunctionality().GetAnimator().SetTrigger("SkillFlg");

            // Loop through all selected units
            for (int x = 0; x < unitsSelected.Count; x++)
            {
                if (x > unitsSelected.Count)
                    break;

                // If active skil has an effect AND it's not a self cast, apply it to selected targets
                if (GetActiveSkill().effect != null && !GetActiveSkill().isSelfCast)
                    unitsSelected[x].AddUnitEffect(GetActiveSkill().effect, unitsSelected[x], GetActiveSkill().effectTurnLength, effectHitAcc);

                #if !UNITY_EDITOR
                    Vibration.Vibrate(15);
                #endif
            }
        }
        else
        {
            for (int z = 0; z < activeRoomAllUnitFunctionalitys.Count; z++)
            {
                activeRoomAllUnitFunctionalitys[z].attacked = false;
            }

            // Loop as many times as power text will appear
            for (int x = 0; x < activeSkill.skillAttackCount; x++)
            {
                if (unitsSelected[0] == null)
                    continue;

                // Loop through all selected units
                for (int i = unitsSelected.Count - 1; i >= 0; i--)
                {
                    bool parrying = false;

                    int originalPower = AdjustSkillPowerTargetEffectAmp(power);

                    if (unitsSelected[i] == null || unitsSelected[i].isDead)
                        continue;
                    else
                    {
                        if (unitsSelected[i].isParrying)
                        {
                            parrying = true;

                            float absPower = Mathf.Abs(originalPower);
                            float tempPower = (float)(unitsSelected[i].curRecieveDamageAmp / 100f) * absPower;
                            float newPower = absPower + tempPower;

                            // Cause power on casting unit
                            if (activeSkill.curSkillType == SkillData.SkillType.OFFENSE)
                                GetActiveUnitFunctionality().SpawnPowerUI(GetActiveUnitFunctionality().GetUnitPowerInc() * newPower);
                            else
                                GetActiveUnitFunctionality().SpawnPowerUI(GetActiveUnitFunctionality().GetUnitPowerInc() * originalPower);

                            // Increase health on casting unit
                            if (activeSkill.curSkillType == SkillData.SkillType.SUPPORT)
                                GetActiveUnitFunctionality().UpdateUnitCurHealth(originalPower);

                            // Decrease health on casting unit
                            if (activeSkill.curSkillType == SkillData.SkillType.OFFENSE)
                                GetActiveUnitFunctionality().UpdateUnitCurHealth((int)newPower, true);

                            // Reset unit's prev power text for future power texts
                            if (x == activeSkill.skillAttackCount - 1)
                                GetActiveUnitFunctionality().ResetPreviousPowerUI();

                            // If active skill has an effect AND it's not a self cast, apply it to selected targets
                            //if (GetActiveSkill().effect != null && !GetActiveSkill().isSelfCast)
                                //GetActiveUnitFunctionality().AddUnitEffect(GetActiveSkill().effect, GetActiveUnitFunctionality());

                            // ATTACKING A PARRYING UNIT
                            // If skill is supposed to cause power, continue
                            if (activeSkill.skillPower != 0)
                            {
                                float absPower2 = Mathf.Abs(originalPower);
                                float tempPower2 = ((float)unitsSelected[i].curRecieveDamageAmp / 100f) * absPower2;
                                float newPower2 = absPower2 + tempPower2;

                                // Cause power on selected unit
                                if (activeSkill.curSkillType == SkillData.SkillType.OFFENSE)
                                {
                                    int orderCount = 2;
                                    if (hasBeenLuckyHit)
                                    {
                                        hasBeenLuckyHit = false;
                                        orderCount--;
                                    }
                                    unitsSelected[i].SpawnPowerUI(GetActiveUnitFunctionality().GetUnitPowerInc() * newPower2, true, false, null, x + orderCount);

                                    CheckAttackForItem(unitsSelected[i], GetActiveUnitFunctionality(), (int)newPower, x, orderCount);
                                }

                                // Increase health from the units current health if a support skill was casted on it
                                if (activeSkill.curSkillType == SkillData.SkillType.SUPPORT)
                                    unitsSelected[i].UpdateUnitCurHealth(originalPower);

                                if (!parrying)
                                {
                                    // Decrease health from the units current health if a offense skill was casted on it
                                    if (activeSkill.curSkillType == SkillData.SkillType.OFFENSE)
                                        unitsSelected[i].UpdateUnitCurHealth((int)newPower2, true);
                                }

                                // Reset unit's prev power text for future power texts
                                if (x == activeSkill.skillAttackCount - 1)
                                    unitsSelected[i].ResetPreviousPowerUI();

                                // If active skill has an effect AND it's not a self cast, apply it to selected targets
                                if (GetActiveSkill().effect != null && !GetActiveSkill().isSelfCast)
                                    unitsSelected[i].AddUnitEffect(GetActiveSkill().effect, unitsSelected[i], GetActiveSkill().effectTurnLength, effectHitAcc);

                            }
                        }

                        // Attacking a non parrying unit
                        else
                        {
                            float absPower = Mathf.Abs(originalPower);
                            float tempPower = ((float)unitsSelected[i].curRecieveDamageAmp / 100f) * absPower;
                            float newPower = absPower + tempPower;

                            int orderCount;

                            // Cause power
                            if (activeSkill.curSkillType == SkillData.SkillType.OFFENSE)
                            {
                                orderCount = 2;
                                if (hasBeenLuckyHit)
                                {
                                    hasBeenLuckyHit = false;
                                    orderCount--;
                                }
                                unitsSelected[i].SpawnPowerUI(GetActiveUnitFunctionality().GetUnitPowerInc() * newPower, false, true, null, x + orderCount);

                                CheckAttackForItem(unitsSelected[i], GetActiveUnitFunctionality(), (int)newPower, x, orderCount);
                            }
                            else
                            {
                                orderCount = 2;
                                if (hasBeenLuckyHit)
                                {
                                    hasBeenLuckyHit = false;
                                    orderCount--;
                                }

                                unitsSelected[i].SpawnPowerUI(GetActiveUnitFunctionality().GetUnitPowerInc() * originalPower, false, false, null, x + orderCount);
                            }

                            // Increase health on targeting unit
                            if (activeSkill.curSkillType == SkillData.SkillType.SUPPORT)
                                unitsSelected[i].UpdateUnitCurHealth(originalPower);

                            // Decrease health on targeting unit
                            if (activeSkill.curSkillType == SkillData.SkillType.OFFENSE)
                            {
                                float health = GetActiveUnitFunctionality().GetUnitPowerInc() * newPower;         
                                unitsSelected[i].UpdateUnitCurHealth((int)health, true);
                            }



                            // Reset unit's prev power text for future power texts
                            if (x == activeSkill.skillAttackCount - 1)
                                unitsSelected[i].ResetPreviousPowerUI();

                            // If active skill has an effect AND it's not a self cast, apply it to selected targets
                            if (GetActiveSkill().effect != null && !GetActiveSkill().isSelfCast)
                                unitsSelected[i].AddUnitEffect(GetActiveSkill().effect, unitsSelected[i], GetActiveSkill().effectTurnLength, effectHitAcc);
                        }

                        #if !UNITY_EDITOR
                            Vibration.Vibrate(15);
                        #endif

                    }
                }

                // Time wait in between attacks, shared across all targeted units
                yield return new WaitForSeconds(timeBetweenPowerUIStack);
            }

            // Reset each units power UI
            for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
            {
                activeRoomAllUnitFunctionalitys[i].ResetPowerUI();
            }
        }

        // If skill is self cast, do it here
        if (GetActiveSkill().isSelfCast)
            if (GetActiveSkill().effect != null)
            {
                GetActiveUnitFunctionality().AddUnitEffect(GetActiveSkill().effect, GetActiveUnitFunctionality(), GetActiveSkill().effectTurnLength, effectHitAcc);
                
                #if !UNITY_EDITOR
                    Vibration.Vibrate(15);
                #endif
            }

        // Disable unit selection just before attack
        for (int y = 0; y < unitsSelected.Count; y++)
        {
            unitsSelected[y].ToggleSelected(false);
        }

        if (GetActiveSkill().curSkillGameType == SkillData.SkillGameType.BASIC)
            GetActiveUnitFunctionality().SetSkill0CooldownMax();
        else if (GetActiveSkill().curSkillGameType == SkillData.SkillGameType.PRIMARY)
            GetActiveUnitFunctionality().SetSkill1CooldownMax();
        else if (GetActiveSkill().curSkillGameType == SkillData.SkillGameType.SECONDARY)
            GetActiveUnitFunctionality().SetSkill2CooldownMax();
        else if (GetActiveSkill().curSkillGameType == SkillData.SkillGameType.ALTERNATE)
            GetActiveUnitFunctionality().SetSkill3CooldownMax();

        yield return new WaitForSeconds(postHitWaitTime);
    
        // If active unit is player, setup player UI
        if (GetActiveUnitFunctionality().unitData.curUnitType == UnitData.UnitType.PLAYER)
        {
            //SetupPlayerUI();
            StartCoroutine(GetActiveUnitFunctionality().UnitEndTurn());  // end unit turn
        }
        else   // If active unit is enemy, check whether to attack again or end turn
        {
            /*
            // If enemy kills all allies in selection, end it's turn
            if (unitsSelected.Count == 0)
            {
                for (int i = 0; i < unitsSelected.Count; i++)
                {
                    if (unitsSelected[i] == null)
                    {
                        if (i == unitsSelected.Count - 1)
                        {
                            // End turn
                            ToggleEndTurnButton(false);
                            StartCoroutine(GetActiveUnitFunctionality().UnitEndTurn());
                            break;
                        }
                        continue;
                    }
                    else
                        break;
                }
            }
            */
            // If enemy didnt kill selection, attack again

                //yield return new WaitForSeconds(postHitWaitTime*3);
            StartCoroutine(GetActiveUnitFunctionality().UnitEndTurn());  // end unit turn
        }
    }

    private void CheckAttackForItem(UnitFunctionality unitTarget, UnitFunctionality unitCaster, int power, int xCount, int orderCount)
    {
        // If the unit has lucky dice, roll for another attack
        List<Item> items = GetActiveUnitFunctionality().GetEquipItems();
        int count = GetActiveUnitFunctionality().GetEquipItems().Count;

        for (int g = 0; g < count; g++)
        {
            int procChance = items[g].procChance;

            power = AdjustSkillPowerTargetEffectAmp(power);
            float absPowerLucky = Mathf.Abs(power);
            float tempPowerLucky = (unitTarget.curRecieveDamageAmp / 100f) * absPowerLucky;
            float newPowerLucky = absPowerLucky + tempPowerLucky;

            if (items[g].itemName == "Lucky Dice")
            {
                if (CheckItemHitChance(GetActiveUnitFunctionality().GetEquipItemCount("Lucky Dice"), procChance))
                {
                    hasBeenLuckyHit = true;
                    unitTarget.SpawnPowerUI(GetActiveUnitFunctionality().GetUnitPowerInc() * newPowerLucky, false, false, null, xCount + orderCount + 1, true);
                    unitTarget.UpdateUnitCurHealth((int)newPowerLucky, true);
                    continue;
                }
            }

            else if (items[g].itemName == "Blood Tip Dagger")
            {
                if (CheckItemHitChance(GetActiveUnitFunctionality().GetEquipItemCount("Blood Tip Dagger"), procChance))
                {
                    // Calculate to heal for 5% of the caster's max health
                    float newPower = (items[g].power / 100f) * GetActiveUnitFunctionality().GetUnitMaxHealth();

                    unitCaster.SpawnPowerUI(GetActiveUnitFunctionality().GetUnitPowerInc() * newPower, false, false, null, xCount + orderCount + 1, false, true);
                    unitCaster.UpdateUnitCurHealth((int)newPower, false);
                    continue;
                }
            }
        }
    }

    public void SetupPlayerUI()
    {
        SetupPlayerSkillsUI();
        UpdateSkillDetails(GetActiveUnitFunctionality().unitData.skill0);
        UpdateAllSkillIconAvailability();
        UpdateUnitSelection(activeSkill);
        UpdateUnitsSelectedText();
        EnableSkill0Selection();
    }
    #region Update Unit UI
    public void UpdateActiveUnitEnergyBar(bool toggle = false, bool increasing = false, int energyAmount = 0, bool enemy = false)
    {
        //GetActiveUnitFunctionality().effects.UpdateAlpha(0);
        //Debug.Log(GetActiveUnitFunctionality().GetUnitName() + " " + GetActiveUnitFunctionality().GetUnitCurEnergy());

        //GetActiveUnitFunctionality().energyCostImage.UpdateEnergyBar((int)GetActiveUnitFunctionality().
           //GetUnitCurEnergy(), energyAmount, increasing, toggle, enemy);
    }

    public void UpdateActiveUnitStatBar(UnitFunctionality unit, bool attackBar, bool toggle)
    {
        if (attackBar)
            unit.ToggleUnitAttackBar(toggle);
        else
            unit.ToggleUnitHealthBar(toggle);
    }

    public void ToggleAllAlliesStatBar(bool toggle)
    {
        for (int i = 0; i < activeRoomAllies.Count; i++)
        {
            activeRoomAllies[i].ToggleUnitHealthBar(toggle);
            activeRoomAllies[i].ToggleUnitAttackBar(toggle);
        }
    }
    #endregion
    #region Update Skill Icons Overlay UI
    public void UpdateAllSkillIconAvailability()
    {
        // Update all skill icon Energy text + unavailable image
        if (GetActiveUnitFunctionality().GetSkill0CurCooldown() > 0)
        {
            //GetActiveUnitFunctionality().DecreaseSkill1Cooldown();

            if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                UpdateSkillIconAvailability(skill0IconUnavailableImage, skill0IconCooldownUIText, GetActiveUnitFunctionality().unitData.GetSkill0().skillCooldown.ToString(), false);
                DisableButton(skill0Button);
                skill0IconCooldownUIText.UpdateUIText(GetActiveUnitFunctionality().GetSkill1CurCooldown().ToString());
            }
        }
        else
        {
            if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                UpdateSkillIconAvailability(skill0IconUnavailableImage, skill0IconCooldownUIText, GetActiveUnitFunctionality().unitData.GetSkill0().skillCooldown.ToString(), true);
                EnableButton(skill0Button);
                skill0IconCooldownUIText.UpdateUIText(GetActiveUnitFunctionality().GetSkill0CurCooldown().ToString());
            }
        }

        // Update all skill icon Energy text + unavailable image
        if (GetActiveUnitFunctionality().GetSkill1CurCooldown() > 0)
        {
            //GetActiveUnitFunctionality().DecreaseSkill1Cooldown();

            if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                UpdateSkillIconAvailability(skill1IconUnavailableImage, skill1IconCooldownUIText, GetActiveUnitFunctionality().unitData.GetSkill1().skillCooldown.ToString(), false);
                DisableButton(skill1Button);
                skill1IconCooldownUIText.UpdateUIText(GetActiveUnitFunctionality().GetSkill1CurCooldown().ToString());
            }
        }
        else
        {
            if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                UpdateSkillIconAvailability(skill1IconUnavailableImage, skill1IconCooldownUIText, GetActiveUnitFunctionality().unitData.GetSkill1().skillCooldown.ToString(), true);
                EnableButton(skill1Button);
                skill1IconCooldownUIText.UpdateUIText(GetActiveUnitFunctionality().GetSkill1CurCooldown().ToString());
            }
        }

        if (GetActiveUnitFunctionality().GetSkill2CurCooldown() > 0)
        {
            //GetActiveUnitFunctionality().DecreaseSkill2Cooldown();

            if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                UpdateSkillIconAvailability(skill2IconUnavailableImage, skill2IconCooldownUIText, GetActiveUnitFunctionality().unitData.GetSkill2().skillCooldown.ToString(), false);
                DisableButton(skill2Button);
                skill2IconCooldownUIText.UpdateUIText(GetActiveUnitFunctionality().GetSkill2CurCooldown().ToString());
            }
        }
        else
        {
            if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                UpdateSkillIconAvailability(skill2IconUnavailableImage, skill2IconCooldownUIText, GetActiveUnitFunctionality().unitData.GetSkill2().skillCooldown.ToString(), true);
                EnableButton(skill2Button);
                skill2IconCooldownUIText.UpdateUIText(GetActiveUnitFunctionality().GetSkill2CurCooldown().ToString());
            }
        }

        if (GetActiveUnitFunctionality().GetSkill3CurCooldown() > 0)
        {
            //GetActiveUnitFunctionality().DecreaseSkill1Cooldown();

            if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                UpdateSkillIconAvailability(skill3IconUnavailableImage, skill3IconCooldownUIText, GetActiveUnitFunctionality().unitData.GetSkill3().skillCooldown.ToString(), false);
                DisableButton(skill3Button);
                skill3IconCooldownUIText.UpdateUIText(GetActiveUnitFunctionality().GetSkill3CurCooldown().ToString());
            }
        }
        else
        {
            if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                UpdateSkillIconAvailability(skill3IconUnavailableImage, skill3IconCooldownUIText, GetActiveUnitFunctionality().unitData.GetSkill3().skillCooldown.ToString(), true);
                EnableButton(skill3Button);
                skill3IconCooldownUIText.UpdateUIText(GetActiveUnitFunctionality().GetSkill3CurCooldown().ToString());
            }
        }

        //UpdateAllSkillIconCooldown();        
    }

    public void UpdateSkillIconAvailability(UIElement skillIconAvailabilityImage, UIText skillIconCooldown, string skillCooldown, bool toggle)
    {
        if (!toggle)
        {
            skillIconAvailabilityImage.UpdateAlpha(1);
            skillIconCooldown.UpdateUIText(skillCooldown);
        }
        else
        {
            skillIconAvailabilityImage.UpdateAlpha(0);
        }
    }
    void UpdateAllSkillIconCooldown()
    {
        skill1IconCooldownUIText.UpdateUIText(GetActiveUnitFunctionality().unitData.GetSkill1().skillCooldown.ToString());
        skill2IconCooldownUIText.UpdateUIText(GetActiveUnitFunctionality().unitData.GetSkill2().skillCooldown.ToString());
        skill3IconCooldownUIText.UpdateUIText(GetActiveUnitFunctionality().unitData.GetSkill3().skillCooldown.ToString());
    }

    public void DisableAllSkillSelections(bool ignoreSkill0 = false)
    {
        if (ignoreSkill0)
        {
            skill1.ToggleSelected(false);
            skill2.ToggleSelected(false);
            skill3.ToggleSelected(false);
        }
        else
        {
            skill0.ToggleSelected(false);
            skill1.ToggleSelected(false);
            skill2.ToggleSelected(false);
            skill3.ToggleSelected(false);
        }
    }

    public void EnableSkill0Selection()
    {
        skill0.ToggleSelected(true);
    }

    public void ToggleSkillSelection(bool toggle)
    {
        skill0.GetComponent<CanvasGroup>().interactable = toggle;
        skill0.GetComponent<CanvasGroup>().blocksRaycasts = toggle;
        skill1.GetComponent<CanvasGroup>().interactable = toggle;
        skill1.GetComponent<CanvasGroup>().blocksRaycasts = toggle;
        skill2.GetComponent<CanvasGroup>().interactable = toggle;
        skill2.GetComponent<CanvasGroup>().blocksRaycasts = toggle;
        skill3.GetComponent<CanvasGroup>().interactable = toggle;
        skill3.GetComponent<CanvasGroup>().blocksRaycasts = toggle;
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
            skill.skillPower, skill.skillCooldown, skill.skillAttackCount, skill.skillPowerIcon, skill.skillSprite, skill.special);

        UnitFunctionality activeUnit = GetActiveUnitFunctionality();

        /*
        // Update Unit Overlay Energy
        abilityDetailsUI.UpdateUnitOverlayEnergyUI(GetActiveUnitFunctionality(),
            activeUnit.GetUnitCurEnergy(), activeUnit.GetUnitMaxEnergy());
        */

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

        UpdateEnemiesKilled(unitFunctionality);

        // If this is the last enemy that got killed, queue player win post battle ui
        if (activeRoomEnemies.Count == 0)
            StartCoroutine(SetupPostBattleUI(true));
        // If this is the last ally that got killed, queue player lose post battle ui
        if (activeRoomAllies.Count == 0)
            StartCoroutine(SetupPostBattleUI(false));

        AddExperienceGained(unitFunctionality.GetUnitExpKillGained());

        /*
        // Remove unit from unit list
        for (int i = 0; i < activeRoomAllUnits.Count; i++)
        {
            if (unitFunctionality.GetUnitName() == activeRoomAllUnitFunctionalitys[i].GetUnitName())
            {
                activeRoomAllUnits.RemoveAt(i);
                break;
            }
        }
        */

        //if (GetActiveUnit().curUnitType == UnitData.UnitType.PLAYER)
        //UpdateTurnOrder(true);
    }

    public void UpdateActiveUnitNameText(string name)
    {
        activeUnitNameText.text = name;
    }

    public void DetermineTurnOrder(bool roundStart = false)
    {
        /*
        // Save other type of data holding 
        for (int x = 0; x < activeRoomAllUnits.Count; x++)
        {
            activeRoomAllUnits[x].UpdateUnitCurAttackCharge(activeRoomAllUnitFunctionalitys[x].GetCurAttackCharge());
        }
        */

        activeRoomAllUnitFunctionalitys.Sort(CompareUnitFunctionalitySpeed);
        activeRoomAllUnitFunctionalitys.Reverse();

        //activeRoomAllUnits.Sort(CompareUnitAttackBar);
        //activeRoomAllUnits.Reverse();
    }

    public void AddSpeedBuffUnit()
    {
        // loop through each unit in room
        for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
        {
            // loop through each effect on each unit
            for (int x = 0; x < activeRoomAllUnitFunctionalitys[i].GetEffects().Count; x++)
            {
                // If unit has speed up effect
                if (activeRoomAllUnitFunctionalitys[i].GetEffects()[x].curEffectName == Effect.EffectName.SPEEDUP)
                {
                    if (activeRoomAllUnitFunctionalitys[i].GetIsSpeedUp())
                        break;

                    activeRoomAllUnitFunctionalitys[i].ToggleIsSpeedUp(true);

                    // units current speed
                    float curSpeed = activeRoomAllUnitFunctionalitys[i].curSpeed;
                    // units new speed
                    float newSpeed = curSpeed + ((activeRoomAllUnitFunctionalitys[i].GetEffects()[x].powerPercent / 100f) * curSpeed);

                    activeRoomAllUnitFunctionalitys[i].UpdateUnitOldSpeed((int)curSpeed);

                    // updating new speed
                    activeRoomAllUnitFunctionalitys[i].UpdateUnitSpeed((int)newSpeed);
                    activeRoomAllUnitFunctionalitys[i].CalculateUnitAttackChargeTurnStart();
                    break;
                }
            }
        }

        //activeRoomAllUnitFunctionalitys.Sort(CompareUnitFunctionalitySpeed);
        //activeRoomAllUnitFunctionalitys.Reverse();

        //activeRoomAllUnits.Sort(CompareUnitSpeed);
        //activeRoomAllUnits.Reverse();

        //UpdateTurnOrderVisual();
    }

    public void AdjustUnitDefense(bool inc)
    {
        if (inc)
        {
            // loop through each unit in room
            for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
            {
                // loop through each effect on each unit
                for (int x = 0; x < activeRoomAllUnitFunctionalitys[i].GetEffects().Count; x++)
                {
                    // If unit has defense up effect
                    if (activeRoomAllUnitFunctionalitys[i].GetEffects()[x].curEffectName == Effect.EffectName.DEFENSEUP)
                    {
                        if (activeRoomAllUnitFunctionalitys[i].GetIsDefenseUp())
                            break;

                        activeRoomAllUnitFunctionalitys[i].ToggleIsDefenseUp(true);

                        // units current def
                        float curDef = activeRoomAllUnitFunctionalitys[i].curDefense;
                        // units new def
                        float newDef = curDef + activeRoomAllUnitFunctionalitys[i].GetEffects()[x].powerPercent;

                        activeRoomAllUnitFunctionalitys[i].UpdateUnitOldDefense((int)curDef);

                        // updating new defense
                        activeRoomAllUnitFunctionalitys[i].UpdateUnitDefense((int)newDef);
                        break;
                    }
                }
            }
        }
        else
        {
            // loop through each unit in room
            for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
            {
                // loop through each effect on each unit
                for (int x = 0; x < activeRoomAllUnitFunctionalitys[i].GetEffects().Count; x++)
                {
                    // If unit has defense up effect
                    if (activeRoomAllUnitFunctionalitys[i].GetEffects()[x].curEffectName == Effect.EffectName.DEFENSEDOWN)
                    {
                        if (activeRoomAllUnitFunctionalitys[i].GetIsDefenseDown())
                            break;

                        activeRoomAllUnitFunctionalitys[i].ToggleIsDefenseDown(true);

                        // units current def
                        float curDef = activeRoomAllUnitFunctionalitys[i].curDefense;
                        // units new def
                        float newDef = curDef - activeRoomAllUnitFunctionalitys[i].GetEffects()[x].powerPercent;

                        activeRoomAllUnitFunctionalitys[i].UpdateUnitOldDefense((int)curDef);

                        // updating new defense
                        activeRoomAllUnitFunctionalitys[i].UpdateUnitDefense((int)newDef);
                        break;
                    }
                }
            }
        }
    }

    public void RemoveSpeedEffectUnit(UnitFunctionality unit)
    {
        unit.UpdateUnitSpeed((int)GetActiveUnitFunctionality().GetOldSpeed());
        unit.ResetOldSpeed();
        unit.ToggleIsSpeedUp(false);
    }

    public void RemoveDefenseUpEffectUnit(UnitFunctionality unit)
    {
        unit.UpdateUnitDefense((int)GetActiveUnitFunctionality().GetOldDefense());
        unit.ResetOldDefense();
        unit.ToggleIsDefenseUp(false);
    }

    public void RemoveDefenseDownEffectUnit(UnitFunctionality unit)
    {
        unit.UpdateUnitDefense((int)GetActiveUnitFunctionality().GetOldDefense());
        unit.ResetOldDefense();
        unit.ToggleIsDefenseDown(false);
    }

    void IncreaseAttackBarAllUnits()
    {
        for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
        {
            activeRoomAllUnitFunctionalitys[i].CalculateUnitAttackChargeTurnStart();
            activeRoomAllUnitFunctionalitys[i].UpdateUnitCurAttackCharge();

            //activeRoomAllUnits[i].UpdateUnitCurAttackCharge(activeRoomAllUnitFunctionalitys[i].GetCurAttackCharge());
        }
    }

    public void UpdateTurnOrder(bool unitDied = false, bool roundStart = false)
    {
        ToggleSkillSelection(true);

        ToggleUIElement(turnOrder, true);   // Enable turn order UI

        ResetSelectedUnits();
        ToggleSelectingUnits(true);
        ToggleAllowSelection(true);

        GetActiveUnitFunctionality().DecreaseSkill0Cooldown();
        GetActiveUnitFunctionality().DecreaseSkill1Cooldown();
        GetActiveUnitFunctionality().DecreaseSkill2Cooldown();
        GetActiveUnitFunctionality().DecreaseSkill3Cooldown();

        UpdateAllSkillIconAvailability();   // Update active unit's skill cooldowns to toggle 'On Cooldown' before switching to new active unit

        GetActiveUnitFunctionality().DecreaseEffectTurnsLeft(false);

        DetermineTurnOrder();

        IncreaseAttackBarAllUnits();

        UpdateAllSkillIconAvailability();   // Update active unit's skill cooldowns to toggle 'On Cooldown' before switching to new active unit

        // Reset active unit attack charge
        GetActiveUnitFunctionality().ResetUnitCurAttackCharge();
        GetActiveUnitFunctionality().unitData.UpdateUnitCurAttackCharge(GetActiveUnitFunctionality().GetCurAttackCharge());

        //Trigger Start turn effects
        GetActiveUnitFunctionality().DecreaseEffectTurnsLeft(true, false);

        // Trigger Unit Energy regen 
        //UpdateActiveUnitEnergyBar(true, true, GetActiveUnitFunctionality().unitStartTurnEnergyGain);


        // Toggle player UI accordingly if it's their turn or not
        if (activeRoomAllUnitFunctionalitys[0].curUnitType == UnitFunctionality.UnitType.PLAYER)
        {
            playerUIElement.UpdateAlpha(1);
            SetupPlayerUI();
            SetupPlayerSkillsUI();

            UpdatePlayerAbilityUI();
            UpdateActiveUnitNameText(GetActiveUnitFunctionality().GetUnitName());

            // Set the basic skill to be on by default to begin with
            activeSkill = GetActiveUnitFunctionality().unitData.skill0;

            ToggleEndTurnButton(true);      // Toggle End Turn Button on
        }
        else
        {
            GetActiveUnitFunctionality().ToggleIdleBattle(true);
            //StartCoroutine(activeRoomAllUnitFunctionalitys[0].StartUnitTurn());
            playerUIElement.UpdateAlpha(0);

            ToggleEndTurnButton(false);      // Toggle End Turn Button on

            // If no allies or enemies remain, do not resume enemies turn
            if (activeRoomAllies.Count >= 1 && activeRoomEnemies.Count >= 1)
            {
                StartCoroutine(activeRoomAllUnitFunctionalitys[0].StartUnitTurn());
            }

            UpdateEnemyPosition(false);

        }

        if (GetActiveUnitFunctionality().unitData.curUnitType == UnitData.UnitType.PLAYER)
        {
            UpdateSkillDetails(activeSkill);
            
            UpdateUnitSelection(activeSkill);
            UpdateEnemyPosition(true);
        }

        UpdateActiveUnitTurnArrow();

        // Update allies into position for battle/shop
        UpdateAllAlliesPosition(false, GetActiveUnitType());
    }

    // If something casts taunt, ally buff skills cant be used - bug

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
            if (GetActiveUnitFunctionality().unitData.curUnitType == UnitData.UnitType.PLAYER)
            {
                // If any enemies are taunting, select them
                if (IsEnemyTaunting().Count >= 1)
                {
                    for (int i = IsEnemyTaunting().Count-1; i >= 0; i--)
                    {
                        selectedAmount++;
                        SelectUnit(IsEnemyTaunting()[i]);

                        // If enough units have been selected FOR ability max targets, or max amount of enemy units tanking
                        if (selectedAmount == usedSkill.skillSelectionCount || selectedAmount == IsEnemyTaunting().Count)
                            return;
                    }
                }

                // only select the closest ENEMY units
                for (int x = activeRoomEnemies.Count - 1; x >= 0; x--)
                {
                    // if no enemies are taunting, start selecting
                    selectedAmount++;
                    SelectUnit(activeRoomEnemies[x]);

                    // If enough units have been selected (in order of closest)
                    if (selectedAmount == usedSkill.skillSelectionCount)
                        return;
                }
            }
            // If the skill user is an ENEMY
            else if (GetActiveUnitFunctionality().unitData.curUnitType == UnitData.UnitType.ENEMY)
            {
                // only select PLAYER units, in random fashion
                for (int x = 0; x < 20; x++)
                {
                    int rand = Random.Range(0, activeRoomAllies.Count);

                    if (activeRoomAllies[rand].IsSelected())
                        continue;
                    else
                        SelectUnit(activeRoomAllies[rand]);

                    selectedAmount++;

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
            if (GetActiveUnitFunctionality().unitData.curUnitType == UnitData.UnitType.PLAYER)
            {
                // only select PLAYER units
                for (int i = 0; i < activeRoomAllies.Count; i++)
                {
                    selectedAmount++;

                    // If self cast, cast on self, otherwise, continue for whomever
                    if (usedSkill.isSelfCast)
                        SelectUnit(GetActiveUnitFunctionality());
                    else
                        SelectUnit(activeRoomAllies[i]);

                    // If enough units have been selected (in order of closest)
                    if (selectedAmount == usedSkill.skillSelectionCount)
                        return;
                }
            }
            // If the skill user is an ENEMY
            else if (GetActiveUnitFunctionality().unitData.curUnitType == UnitData.UnitType.ENEMY)
            {
                // only select ENEMY units
                for (int i = 0; i < activeRoomEnemies.Count; i++)
                {
                    int rand = Random.Range(0, activeRoomEnemies.Count);

                    // If skill requires only 1 unit selection - (full team target wont work without this)
                    if (usedSkill.skillSelectionCount == 1)
                    {
                        // If enemy chooses itself for ally attack, reselect to target any other ally 
                        if (activeRoomEnemies[rand] == GetActiveUnitFunctionality())
                        {
                            if (i != 0)
                            {
                                i--;
                                continue;
                            }
                            else
                                continue;
                        }
                    }

                    selectedAmount++;

                    // If self cast, cast on self, otherwise, continue for whomever
                    if (usedSkill.isSelfCast)
                        SelectUnit(GetActiveUnitFunctionality());
                    else
                    {
                        if (!activeRoomEnemies[rand].IsSelected())
                            SelectUnit(activeRoomEnemies[rand]);
                        else
                        {
                            if (i != 0)
                            {
                                i--;

                                if (selectedAmount != 0)
                                    selectedAmount--;
                            }
                        }
                    }

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

        UpdateUnitsSelectedText();
    }

    private void DeselectAllUnits()
    {
        for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
        {
            activeRoomAllUnitFunctionalitys[i].ToggleSelected(false);
        }
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

    public void ToggleUIElementFull(UIElement uiElement, bool toggle)
    {
        uiElement.gameObject.SetActive(toggle);
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
        //activeRoomAllUnits.Add(unit);
    }

    private void AddActiveRoomAllUnitsFunctionality(UnitFunctionality unitFunctionality)
    {
        activeRoomAllUnitFunctionalitys.Add(unitFunctionality);

        if (unitFunctionality.curUnitType == UnitFunctionality.UnitType.PLAYER)
            activeRoomAllies.Add(unitFunctionality);
        else if (unitFunctionality.curUnitType == UnitFunctionality.UnitType.ENEMY)
            activeRoomEnemies.Add(unitFunctionality);
    }

    public void ResetActiveUnitTurnArrow()
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
        //playerIcon.UpdatePortrait(GetActiveUnitFunctionality().GetUnitIcon());

        // Update player skill portraits
        playerSkill0.UpdatePortrait(GetActiveUnitFunctionality().unitData.GetSkill0().skillSprite);
        playerSkill1.UpdatePortrait(GetActiveUnitFunctionality().unitData.GetSkill1().skillSprite);
        playerSkill2.UpdatePortrait(GetActiveUnitFunctionality().unitData.GetSkill2().skillSprite);
        playerSkill3.UpdatePortrait(GetActiveUnitFunctionality().unitData.GetSkill3().skillSprite);

        //playerSkill0.ToggleSelectImage(false);
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

    public void UnSelectUnit(UnitFunctionality unit, bool shop = false)
    {
        // Enable exit button until player has selected ally with item
        if (shop)
        {
            MapManager.Instance.exitShopRoom.UpdateAlpha(1);
            ShopManager.Instance.selectAlly = false;
        }

        unit.ToggleSelected(false);
        unitsSelected.Remove(unit);
    }

    public void ToggleAllowSelection(bool toggle)
    {
        allowSelection = toggle;
    }

    public bool GetAllowSelection()
    {
        return allowSelection;
    }

    IEnumerator WaitTimeThenDeselect(float time, UnitFunctionality unit)
    {
        yield return new WaitForSeconds(time);

        UnSelectUnit(unit, true);
    }
    public void SelectUnit(UnitFunctionality unit)
    {
        if (!GetAllowSelection())
            return;

        // If current room is a shop
        if (RoomManager.Instance.GetActiveRoom().curRoomType == RoomMapIcon.RoomType.SHOP)
        {
            ToggleAllowSelection(false);
            ShopManager.Instance.shopSelectAllyPrompt.UpdateAlpha(0);

            unit.AddOwnedItems(ShopManager.Instance.GetUnassignedItem());

            if (ShopManager.Instance.GetUnassignedItem().healthItem)
            {
                float healthToRegen = (ShopManager.Instance.GetUnassignedItem().power / 100f) * unit.GetUnitMaxHealth();
                unit.SpawnPowerUI(healthToRegen, false, false, null, 1, false, true);
                unit.UpdateUnitCurHealth((int)healthToRegen, false, false);
            }

            ShopManager.Instance.UpdateUnAssignedItem(null);

            // If item is health item, do the effect of it
            StartCoroutine(WaitTimeThenDeselect(shopRemoveSelectTime, unit));            
        }

        if (activeSkill)
        {
            // Dont select other units if its a self cast
            if (activeSkill.isSelfCast && unit != GetActiveUnitFunctionality())
                return;

            // If active skill can only select allies, do not allow enemies to be selected
            //if (activeSkill.curSkillSelectionType == SkillData.SkillSelectionType.PLAYERS && unitFunctionality.curUnitType == UnitFunctionality.UnitType.ENEMY)
            //  return;

            if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                if (activeSkill.curSkillSelectionType == SkillData.SkillSelectionType.PLAYERS && unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                    return;

                if (activeSkill.curSkillSelectionType == SkillData.SkillSelectionType.ENEMIES && unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                    return;
            }
        }

        /*
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
                if (unitsSelected.Count == GetActiveUnitFunctionality().unitData.basicSelectionCount)
                    UnSelectUnit(unitsSelected[0]);
            }
        }
        */

        // If user selects a unit that is already selected, unselect it, and go a different path
        if (unit.IsSelected() && GetSelectingUnitsAllowed())
        {
            ToggleSelectingUnits(false);
            //UnSelectUnit(unit);
            //UpdateUnitsSelectedText();

            PlayerAttack();
            return;
        }

        if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
        {
            // If enemies are taunting, do not allow selection if this unit to select is not taunting also
            if (IsEnemyTaunting().Count >= 1)
            {
                for (int i = 0; i < IsEnemyTaunting().Count; i++)
                {
                    // If active unit is ally, and is selecting enemies, consider enemies parrying
                    if (GetActiveSkill().curSkillSelectionType == SkillData.SkillSelectionType.ENEMIES)
                    {
                        if (IsEnemyTaunting()[i] == unit)
                        {
                            // remove previous target
                            if (unitsSelected.Count == GetActiveSkill().skillSelectionCount)
                                ResetSelectedUnits();

                            // Select targeted unit
                            unitsSelected.Add(unit);
                            unit.ToggleSelected(true);
                            UpdateUnitsSelectedText();
                        }
                    }
                    else  // If active unit is ally, and is selecting allies, disregard parry, and continue
                    {
                        // remove previous target
                        if (unitsSelected.Count == GetActiveSkill().skillSelectionCount)
                            ResetSelectedUnits();

                        // Select targeted unit
                        unitsSelected.Add(unit);
                        unit.ToggleSelected(true);
                        UpdateUnitsSelectedText();
                        break;
                    }
                }
            }
            else // If no enemies are taunting
            {
                // remove previous target
                if (unitsSelected.Count == GetActiveSkill().skillSelectionCount)
                {
                    unitsSelected[0].ToggleSelected(false);
                    unitsSelected.RemoveAt(0);
                }

                // Select targeted unit
                unitsSelected.Add(unit);
                unit.ToggleSelected(true);
                UpdateUnitsSelectedText();
            }
        }
        else
        {
            // If enemies are taunting, do not allow selection if this unit to select is not taunting also
            if (IsAllyTaunting())
            {

            }
            else // If no enemies are taunting
            {
                // Select targeted unit
                unitsSelected.Add(unit);
                unit.ToggleSelected(true);
                UpdateUnitsSelectedText();
            }
        }

        //Debug.Log(unit.IsSelected());
    }

    public void ToggleSelectingUnits(bool toggle)
    {
        selectingUnitsAllowed = toggle;
    }

    public bool GetSelectingUnitsAllowed()
    {
        return selectingUnitsAllowed;
    }

    public void PlayerAttack()
    {       
        DisableButton(skill1Button);
        DisableButton(skill2Button);
        DisableButton(skill3Button);

        GetActiveUnitFunctionality().TriggerTextAlert(GetActiveSkill().skillName, 1, false);

        StartCoroutine(AttackButtonCont());
    }

    IEnumerator AttackButtonCont()
    {
        yield return new WaitForSeconds(skillAlertAppearTime / 2);
        SetupPlayerWeaponUI();
    }

    public void UpdateUnitsSelectedText()
    {
        // If a skill is selected
        if (activeSkill != null)
        {
            UpdateUnitsSelectedText(unitsSelected.Count, activeSkill.skillSelectionCount);
        }
    }

    private void UpdateUnitsSelectedText(int curUnitsSelected, int maxUnitsSelected)
    {
        curUnitsTargetedText.text = curUnitsSelected.ToString();
        maxUnitsTargetedText.text = maxUnitsSelected.ToString();
    }

    public void ReduceAllEnemyHealth()
    {
        if (activeRoomEnemies.Count == 0)
            return;

        for (int i = 0; i < activeRoomEnemies.Count; i++)
        {
            activeRoomEnemies[i].UpdateUnitMaxHealth(1);
        }
    }

    public void ReduceAllPlayerHealth()
    {
        if (activeRoomAllies.Count == 0)
            return;

        for (int i = 0; i < activeRoomAllies.Count; i++)
        {
            activeRoomAllies[i].UpdateUnitMaxHealth(1);
        }
    }

    private int CompareUnitAttackBar(UnitData unitA, UnitData unitB)
    {
        if (unitA.GetCurAttackChargeTurnStart() < unitB.GetCurAttackChargeTurnStart())
            return -1;
        else if (unitA.GetCurAttackChargeTurnStart() > unitB.GetCurAttackChargeTurnStart())
            return 1;
        else
            return 0;
    }

    private int CompareUnitFunctionalitySpeed(UnitFunctionality unitA, UnitFunctionality unitB)
    {
        if (unitA.GetCurAttackCharge() < unitB.GetCurAttackCharge())
            return -1;
        else if (unitA.GetCurAttackCharge() > unitB.GetCurAttackCharge())
            return 1;
        else
            return 0;
    }

    private bool CheckItemHitChance(int itemCount, int itemProcChance)
    {
        int luckyDice = itemCount * itemProcChance;
        int rand = Random.Range(1, 101);

        if (luckyDice >= rand)
            return true;
        else
            return false;
    }

    public List<T> Shuffle<T>(List<T> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            T temp = _list[i];
            int randomIndex = Random.Range(i, _list.Count);
            _list[i] = _list[randomIndex];
            _list[randomIndex] = temp;
        }

        return _list;
    }
}
