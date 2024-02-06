using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public UnitData startingUnit;

    //public bool devMode;

    [SerializeField] private float postBattleTime;
    public List<UnitData> activeTeam = new List<UnitData>();
    //public List<UnitData> allPlayerClasses = new List<UnitData>();
    public UIElement currentRoom;
    [SerializeField] private GameObject baseUnit;
    [SerializeField] private List<Transform> enemySpawnPositions = new List<Transform>();
    public List<Transform> allySpawnPositions = new List<Transform>();
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

    [Header("Combat - Unit Levels")]
    public int levelupHealPerc = 20;
    public int combatCompleteHealPerc = 25;

    [SerializeField] private Color easyDiffEnemyLvColour;
    [SerializeField] private Color mediumDiffEnemyLvColour;
    [SerializeField] private Color hardDiffEnemyLvColour;
    [SerializeField] private Color chaosDiffEnemyLvColour;
    [SerializeField] private int easydiffEnemyLvDifference;
    [SerializeField] private int mediumdiffEnemyLvDifference;
    [SerializeField] private int harddiffEnemyLvDifference;
    [SerializeField] private int chaosdiffEnemyLvDifference;
    [Space(1)]
    //[SerializeField] private int heroRoomIncChallengeCount = 2;
    [SerializeField] private int heroRoomMinEnemiesIncCount = 1;
    [SerializeField] private int heroRoomMaxEnemiesIncCount = 2;

    [Tooltip("Duration of the flash.")]
    public float hitFlashDuration = 0.05f;

    [Header("Hero Retrieval")]
    [SerializeField] private float postFightTimeWait = 1.5f;

    [Header("Team Setup")]
    [SerializeField] private UIElement skillsTab;
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
    public float timeTillNextTargetEffetVisual = 0.02f;
    public float allyMeleeSkillWaitTime = .5f;
    public float allyRangedSkillWaitTime;
    public float enemyMeleeSkillWaitTime = .5f;
    public float enemyRangedSkillWaitTime;
    public float maxHeldTimeTooltip;
    public float leachEffectGainWait = .20f;

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
    public int randomBaseOffset = 15;
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
    public float skillEffectDepleteAppearTime = .5f;
    public SkillData activeSkill;
    public TMP_ColorGradient gradientSkillAlert;
    public TMP_ColorGradient gradientLevelUpAlert;
    [SerializeField] private CanvasGroup skill0CG;
    [SerializeField] private CanvasGroup skill1CG;
    [SerializeField] private CanvasGroup skill2CG;
    [SerializeField] private CanvasGroup skill3CG;

    [Header("Post Battle")]
    public GearRewards gearRewards;
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
    bool roomDefeated;

    public UnitData spawnedUnit;
    public UnitFunctionality spawnedUnitFunctionality;

    [HideInInspector]
    public bool playerWon;

    public bool combatOver;
    public int unitID;

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
        SkillsTabChangeAlly(false);

        // Map music
        AudioManager.Instance.Play("MapLoop");
    }

    public void UpdateAllAlliesLevelColour()
    {
        for (int i = 0; i < activeRoomAllies.Count; i++)
        {
            activeRoomAllies[i].UpdateUnitLevelColour(easyDiffEnemyLvColour);
        }
    }

    public void UpdateAllEnemiesLevelColour()
    {
        int enemyLv = 0;
        float allyAverageLv = 0;

        int levelDiff = 0;

        for (int i = 0; i < activeRoomAllies.Count; i++)
        {
            allyAverageLv += activeRoomAllies[i].GetUnitLevel();
        }

        // Get average for ally levels
        allyAverageLv /= activeRoomAllies.Count;


        // Update each enemy unit's level text colour
        for (int i = 0; i < activeRoomEnemies.Count; i++)
        {
            if (activeRoomEnemies[i].GetUnitLevel() > (int)allyAverageLv)
            {
                levelDiff = activeRoomEnemies[i].GetUnitLevel() - (int)allyAverageLv;
            }
            else if ((int)allyAverageLv >= activeRoomEnemies[i].GetUnitLevel())
            {
                levelDiff = 0;
            }


            if (levelDiff >= chaosdiffEnemyLvDifference)
            {
                activeRoomEnemies[i].UpdateUnitLevelColour(chaosDiffEnemyLvColour);
                continue;
            }
            else if (levelDiff >= mediumdiffEnemyLvDifference)
            {
                activeRoomEnemies[i].UpdateUnitLevelColour(hardDiffEnemyLvColour);
                continue;
            }
            else if (levelDiff >= easydiffEnemyLvDifference)
            {
                activeRoomEnemies[i].UpdateUnitLevelColour(mediumDiffEnemyLvColour);
                continue;
            }
            else if (levelDiff < easydiffEnemyLvDifference)
            {
                activeRoomEnemies[i].UpdateUnitLevelColour(easyDiffEnemyLvColour);
                continue;
            }
            else
            {
                activeRoomEnemies[i].UpdateUnitLevelColour(easyDiffEnemyLvColour);
                continue;
            }
        }
    }

    public Weapon GetActiveWeapon()
    {
        return activeWeapon;
    }

    public void SetActiveWeapon()
    {
        activeWeapon = playerWeapon.GetComponent<Weapon>();
    }

    public void AddUnitToTeam(UnitData unit)
    {
        activeTeam.Add(unit);
    }

    public UnitData GetUnitData(int count)
    {
        return activeTeam[count];
    }

    public UnitFunctionality GetUnitFunctionality(UnitData unit)
    {
        for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
        {
            if (activeRoomAllUnitFunctionalitys[i].GetUnitName() == unit.unitName)
            {
                return activeRoomAllUnitFunctionalitys[i];
            }
            else
                continue;
        }

        return null;
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

    public void SpawnAllies(bool spawnHeroAlly = false, bool byPass = false)
    {
        // Spawn player units
        if (activeRoomAllies.Count == 0 || spawnHeroAlly)
        {
            UnitData unit = null;  // Initialize

            // If spawning hero ally from end of hero room
            if (spawnHeroAlly)
            {
                List<string> ownedUnitNames = new List<string>();

                for (int i = 0; i < activeTeam.Count; i++)
                {
                    ownedUnitNames.Add(activeTeam[i].unitName);
                }

                // Loop through all allies in Characters Carasel
                for (int t = 0; t < CharacterCarasel.Instance.GetAllAllies().Count; t++)
                {
                    for (int v = 0; v < ownedUnitNames.Count; v++)
                    {
                        if (!ownedUnitNames.Contains(CharacterCarasel.Instance.GetAllAllies()[t].unitName))
                        {
                            unit = CharacterCarasel.Instance.GetAlly(t);    // Reference a unit that is not on the current ally team

                            UnitFunctionality unitFunct = GetActiveUnitFunctionality();
                            UnitData unitData = unitFunct.unitData;
                            //unitData.ResetCurSkills();
                            //unitFunct.SetStartingSkills(unitData.GetUnitSkills());
                            //unitData.UpdateCurSkills(unitFunct.GetStartingSkills());
                            //Debug.Log("3");
                            unitFunct.UpdateUnitSkills(unitData.GetUnitSkills());
                            unitFunct.UpdateCurrentSkills(unitData.GetUnitSkills());

                            unitFunct.ResetSkill0Cooldown();
                            unitFunct.ResetSkill1Cooldown();
                            unitFunct.ResetSkill2Cooldown();
                            unitFunct.ResetSkill3Cooldown();

                            SkillsTabManager.Instance.ResetAllySkllls(unitFunct);
                        }
                    }
                }

                if (unit == null && !byPass)
                {
                    Debug.Log("No ally found that is not already on team!");
                    StartCoroutine(SetupPostBattleUI(playerWon));
                    return;
                }

            }

            // Spawn ally
            for (int i = 0; i < 1; i++)
            {
                // If naturally spawning an ally, use default method
                if (!spawnHeroAlly)
                    unit = activeTeam[i];    // Reference



                GameObject go = null;
                if (!spawnHeroAlly)
                {
                    go = Instantiate(baseUnit, allySpawnPositions[i]);
                    go.transform.SetParent(allySpawnPositions[i]);
                }
                else
                {
                    go = Instantiate(baseUnit, HeroRoomManager.Instance.GetSpawnLocTrans());
                    go.transform.SetParent(HeroRoomManager.Instance.GetSpawnLocTrans());

                    ItemRewardManager.Instance.ResetRewardsTable();
                }

                UnitFunctionality unitFunctionality = go.GetComponent<UnitFunctionality>();



                UIElement unitUI = go.GetComponent<UIElement>();

                HeroRoomManager.Instance.UpdateHero(unitUI);

                /*
                // If there is already this ally in the team, make this double spawned ally = true
                // If spawning hero ally from end of hero room
                if (spawnHeroAlly)
                {
                    List<string> ownedUnitNames = new List<string>();

                    // Loop through all allies
                    for (int t = 0; t < activeRoomAllUnitFunctionalitys.Count; t++)
                    {
                        string unitName = activeRoomAllUnitFunctionalitys[t].GetUnitName();
                        if (ownedUnitNames.Contains(unitName))
                            activeRoomAllUnitFunctionalitys[t].unitDouble = true;

                        ownedUnitNames.Add(unitName);
                    }
                }
                */

                // Set ally correct position based on team size
                if (!spawnHeroAlly)
                {
                    if (i == 0)
                        unitFunctionality.SetPositionAndParent(allySpawnPositions[1]);
                    else if (i == 1)
                        unitFunctionality.SetPositionAndParent(allySpawnPositions[0]);
                    else if (i == 2)
                        unitFunctionality.SetPositionAndParent(allySpawnPositions[2]);
                }
                // If spawning a new unit from hero room, add unit to team
                else
                {
                    unitFunctionality.heroRoomUnit = true;

                    spawnedUnit = unit;
                    spawnedUnitFunctionality = unitFunctionality;
                }


                unitFunctionality.UpdateUnitName(unit.unitName);
                unitFunctionality.UpdateUnitSprite(unit.characterPrefab);
                unitFunctionality.UpdateUnitColour(unit.unitColour);
                unitFunctionality.UpdateUnitType("Player");
                unitFunctionality.UpdateUnitValue(unit.GetUnitValue());
                unitFunctionality.UpdateUnitSpeed(unit.startingSpeed);
                unitFunctionality.UpdateUnitPower(unit.startingPower);
                unitFunctionality.UpdateUnitDefense(unit.startingDefense);

                unitFunctionality.UpdateSpeedIncPerLv((int)unit.speedIncPerLv);
                unitFunctionality.UpdatePowerIncPerLv((int)unit.powerIncPerLv);
                unitFunctionality.UpdateHealingPowerIncPerLv((int)unit.healingPowerIncPerLv);
                unitFunctionality.UpdateDefenseIncPerLv(unit.defenseIncPerLv);
                unitFunctionality.UpdateMaxHealthIncPerLv((int)unit.maxHealthIncPerLv);

                unitFunctionality.UpdateUnitVisual(unit.unitSprite);
                unitFunctionality.UpdateUnitIcon(unit.unitIcon);

                unitFunctionality.UpdateUnitHealth(unit.startingMaxHealth, unit.startingMaxHealth);


                unitFunctionality.startingDamage = unit.startingPower;
                unitFunctionality.startingHealth = unit.startingMaxHealth;
                unitFunctionality.startingHealing = unit.startingHealingPower;
                unitFunctionality.startingDefense = unit.startingDefense;
                unitFunctionality.startingSpeed = unit.startingSpeed;

                unitFunctionality.deathClip = unit.deathClip;
                unitFunctionality.hitRecievedClip = unit.hitRecievedClip;

                AddActiveRoomAllUnitsFunctionality(unitFunctionality);

                unitFunctionality.UpdateUnitLevel(1);
                UpdateAllAlliesLevelColour();


                //SkillsTabManager.Instance.SetupSkillsTab(unitFunctionality);

                if (spawnHeroAlly)
                    ToggleAllowSelection(true);

                unitFunctionality.ToggleUnitHealthBar(true);

                //unitFunctionality.ToggleUnitHealthBar(false);

                unitFunctionality.unitData = unit;


                if (byPass)
                {
                    if (activeRoomAllies.Count == 0)
                        unitFunctionality.SetPositionAndParent(allySpawnPositions[1]);
                    else if (activeRoomAllies.Count == 1)
                        unitFunctionality.SetPositionAndParent(allySpawnPositions[0]);
                    if (activeRoomAllies.Count == 2)
                        unitFunctionality.SetPositionAndParent(allySpawnPositions[2]);

                    unitFunctionality.heroRoomUnit = true;

                    spawnedUnit = unit;
                    spawnedUnitFunctionality = unitFunctionality;

                    AddUnitToTeam(unitFunctionality.unitData);
                }

                if (i == 0 && !spawnHeroAlly)
                    SkillsTabManager.Instance.UpdateActiveUnit(GetActiveUnitFunctionality());

                UnitData unitData = unitFunctionality.unitData;
                //Debug.Log(unitData.unitName);
                //unitData.ResetCurSkills();
                //unitFunctionality.SetStartingSkills();
                //unitData.UpdateCurSkills(unitData.GetUnitSkills());
                /*
                for (int x = 0; x < unitData.GetUnitSkills().Count; x++)
                {
                    Debug.Log(unitData.GetUnitSkills()[x].skillName);
                }
                */

                //Debug.Log(unitData.GetUnitSkills());
                unitFunctionality.UpdateUnitSkills(unitData.GetUnitSkills());
                unitFunctionality.UpdateCurrentSkills(unitData.GetUnitSkills());

                unitFunctionality.ResetSkill0Cooldown();
                unitFunctionality.ResetSkill1Cooldown();
                unitFunctionality.ResetSkill2Cooldown();
                unitFunctionality.ResetSkill3Cooldown();

                SkillsTabManager.Instance.ResetAllySkllls(unitFunctionality);

                //activeRoomAllies.Add(unitFunctionality);

                SkillsTabChangeAlly(true);
                SkillsTabChangeAlly(false);
                // If spawn hero ally from hero room, only spawn 1 ally
                if (spawnHeroAlly)
                    break;
            }
        }     
    }


    public void UpdateActiveAllyDisplay()
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
            {
                //Debug.Log("bb");
                activeRoomAllUnitFunctionalitys[i].UpdateIsVisible(false);
            }

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
    public void SkillsTabChangeAlly(bool toggle)
    {
        SkillsTabManager.Instance.statScrollView.SetActive(toggle);

        if (toggle)
        {
            // Update which ally is active
            UpdateActiveAllyDisplay();

            // Update skills details when ally swap
            //SkillsTabManager.Instance.UpdateSelectedSkillBase();
            //SkillsTabManager.Instance.UpdateSkillStatDetails();

            SkillsTabManager.Instance.gearTabArrowLeftButton.ToggleButton(true);
            SkillsTabManager.Instance.gearTabArrowRightButton.ToggleButton(true);

            // Update UI
            SkillsTabManager.Instance.activeSkillBase = null;
            SkillsTabManager.Instance.UpdateActiveSkillNameText("");
            SkillsTabManager.Instance.UpdateSkillStatDetails();
            SkillsTabManager.Instance.ResetSkillsBaseSelection();

            skillsTab.UpdateAlpha(1);
            //SpawnAllies(false);
            SkillsTabManager.Instance.ToggleToMapButton(true);
            UpdateAllAlliesPosition(false, true, true);
            SkillsTabManager.Instance.UpdateActiveUnit(GetActiveUnitFunctionality());

            oldActiveRoomAllUnitFunctionalitys = activeRoomAllUnitFunctionalitys;
            
            SkillsTabManager.Instance.UpdateAllyNameText();

            SkillsTabManager.Instance.UpdateStatPage();
        }
        else
        {
            SkillsTabManager.Instance.gearTabArrowLeftButton.ToggleButton(false);
            SkillsTabManager.Instance.gearTabArrowRightButton.ToggleButton(false);
            SkillsTabManager.Instance.ToggleToMapButton(false);
            skillsTab.UpdateAlpha(0);

            UpdateAllyVisibility(true, false, false);
            UpdateAllysPositionCombat();
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
                        //Debug.Log("aa");
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
                    activeRoomAllUnitFunctionalitys[i].ToggleActionNextBar(true);
                }
            }
        }
        else    // if disabling
        {
            // Toggle ally visibility
            for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
            {

                if (!toggle)
                    //Debug.Log("ccc");

                activeRoomAllUnitFunctionalitys[i].UpdateIsVisible(toggle);

            }
        }
    }

    public void ResetRoom(bool enemies = true)
    {
        defeatedEnemies.ResetDefeatedEnemies();

        combatOver = false;

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

            if (activeRoomAllies.Count == 0)
                activeTeam.Clear();
        }

        if (enemies)
        {
            for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
            {
                if (activeRoomAllUnitFunctionalitys[i].curUnitType == UnitFunctionality.UnitType.ENEMY)
                    activeRoomAllUnitFunctionalitys.RemoveAt(i);
            }
            //activeRoomAllUnitFunctionalitys.Clear();

            for (int x = 0; x < enemySpawnPositions.Count; x++)
            {
                if (enemySpawnPositions[x].childCount >= 1)
                    Destroy(enemySpawnPositions[x].transform.GetChild(0).gameObject);
            }

            for (int y = 0; y < activeRoomEnemies.Count; y++)
            {
                activeRoomEnemies.RemoveAt(y);
            }

            activeRoomEnemies.Clear();

            roomDefeated = false;
        }
    }

    public void ToggleMap(bool toggle, bool generateMap = false, bool increaseFloor = false, bool playMapLoop = true)
    {
        // Turning map OFF
        if (!toggle)
        {
            ResetAlliesExpVisual();            

            map.mapOverlay.ToggleBottomBG(false);
            map.gameObject.SetActive(false);

            map.ToggleMapVisibility(false, false);

            //ToggleSkillVisibility(false);
        }
        // Turn map ON
        else
        {
            HeroRoomManager.Instance.TogglePlayedOffered(false);

            if (!increaseFloor)
                ResetAlliesExpVisual();

            // Play Map music
            if (playMapLoop)
                AudioManager.Instance.PauseMapMusic(false);

            map.mapOverlay.ToggleBottomBG(true);
            map.gameObject.SetActive(true);

            map.ToggleMapVisibility(true, generateMap, increaseFloor);

            ToggleSkillVisibility(true);
        }
    }

    public void UpdateAllAlliesPosition(bool postBattle, bool playersTurn = true, bool masteryPosition = false, bool shopPosition = false)
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
                allyPositions.SetParent(SkillsTabManager.Instance.statAllyPosTrans);
                allyPositions.SetPositionAndRotation(new Vector2(0, 0), Quaternion.identity);
                allyPositions.position = SkillsTabManager.Instance.statAllyPosTrans.position;

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

    public void SetupItemRewards()
    {
        ResetSelectedUnits();

        combatOver = true;

        // Toggle player overlay and skill ui off
        ToggleUIElement(playerAbilities, false);
        ToggleUIElement(playerAbilityDesc, false);
        ToggleUIElement(endTurnButtonUI, false);
        ToggleUIElement(turnOrder, false);
        ResetActiveUnitTurnArrow();
        ToggleAllAlliesStatBar(false);

        // Remove all unit effects and level image for item rewards
        for (int i = 0; i < activeRoomAllies.Count; i++)
        {
            activeRoomAllies[i].ResetEffects();
            activeRoomAllies[i].ToggleUnitLevelImage(false);
        }

        StartCoroutine(SetupItemRewardsCo());
    }

    IEnumerator SetupItemRewardsCo()
    {
        yield return new WaitForSeconds(ItemRewardManager.Instance.postCombatTillItemTime2);

        // Stop combat music
        AudioManager.Instance.StopCombatMusic();

        // If item rewards are NOT disabled, offer them
        if (!ItemRewardManager.Instance.disableItemRewards)
            ItemRewardManager.Instance.FillItemsTable();
        // If item rewards ARE disabled, DONT offer items, skip to post battle sequence
        else
            StartCoroutine(SetupPostBattleUI(playerWon));
    }

    #region Setup Multiple UIs
    public IEnumerator SetupPostBattleUI(bool playerWon)
    {
        if (!playerWon)
        {
            activeRoomAllUnitFunctionalitys.Clear();
        }
        else
        {
            // Remove dead allies from team when post battle starts, on a win
            for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
            {
                if (activeRoomAllUnitFunctionalitys[i].isDead)
                {
                    RemoveUnit(activeRoomAllUnitFunctionalitys[i]);
                }

            }
        }

        ItemRewardManager.Instance.ToggleItemRewards(false);
        ItemRewardManager.Instance.ToggleConfirmItemButton(false);

        if (RoomManager.Instance.GetActiveRoom().curRoomType == RoomMapIcon.RoomType.HERO)
        {

        }

        yield return new WaitForSeconds(ItemRewardManager.Instance.postItemTillPostCombat);

        // SFX
        if (playerWon)
            AudioManager.Instance.Play("Room_Win");
        else
            AudioManager.Instance.Play("Room_Lose");

        // Stop combat music
        AudioManager.Instance.StopCombatMusic();

        if (RoomManager.Instance.GetActiveRoom().curRoomType != RoomMapIcon.RoomType.HERO && RoomManager.Instance.GetActiveRoom().curRoomType != RoomMapIcon.RoomType.BOSS)
        {
            RoomManager.Instance.IncrementDefaultRoomsCleared();
        }

        roomDefeated = true;

        UpdateActiveSkill(null);

        //if (RoomManager.Instance.GetActiveRoom().curRoomType != RoomMapIcon.RoomType.HERO)
        //    StartCoroutine(SetupRoomPostBattle(playerWon));

        UpdateAllAlliesPosition(true);


        // If completed room WAS NOT a hero room
        if (RoomManager.Instance.GetActiveRoom().curRoomType != RoomMapIcon.RoomType.HERO || !playerWon)
        {
            ToggleSkillVisibility(false);

            RoomManager.Instance.ToggleInteractable(false);

            // Enable unit level image for post combat
            for (int i = 0; i < activeRoomAllies.Count; i++)
            {
                activeRoomAllies[i].ToggleUnitLevelImage(true);
            }
        }
        // If completed room WAS a hero room
        else
        {
            ToggleSkillVisibility(false);

            // If player has 3 allies already, do not offer a 4th, go to post battle (TEMP SOLUTION) TODO: Make prompt to swap
            if (activeTeam.Count < 3)
            {
                //StartCoroutine(HeroRetrievalScene());
            }
            else
            {
                ToggleSkillVisibility(false);

                RoomManager.Instance.ToggleInteractable(false);

                // Enable unit level image for post combat
                for (int i = 0; i < activeRoomAllies.Count; i++)
                {
                    activeRoomAllies[i].ToggleUnitLevelImage(true);
                }
            }
        }

        PostBattle.Instance.TogglePostBattleConditionText(playerWon);

        // Ads
        if (playerWon && RoomManager.Instance.GetActiveRoom().curRoomType == RoomMapIcon.RoomType.BOSS)
        {
            AdManager.Instance.ShowForcedAd();
        }

        if (!playerWon)
        {
            // Enable post battle to map button for next post battle scene
            StartCoroutine(PostBattle.Instance.ToggleButtonPostBattleMap(true));
        }

        StartCoroutine(SetupRoomPostBattle(playerWon));
    }

    public IEnumerator HeroRetrievalScene()
    {
        ItemRewardManager.Instance.ToggleItemRewards(false);
        ItemRewardManager.Instance.ToggleConfirmItemButton(false);

        yield return new WaitForSeconds(postFightTimeWait);

        //RoomManager.Instance.ToggleInteractable(true);
        ToggleAllowSelection(true);
        ToggleSelectingUnits(true);

        // Toggle player overlay and skill ui off
        ToggleUIElement(playerAbilities, false);
        ToggleUIElement(playerAbilityDesc, false);
        ToggleUIElement(endTurnButtonUI, false);
        ToggleUIElement(turnOrder, false);
        ResetActiveUnitTurnArrow();

        HeroRoomManager.Instance.SpawnHero();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            SpawnAllies(true, true);
    }

    // Toggle UI accordingly
    public IEnumerator SetupRoomPostBattle(bool playerWon)
    {
        //StartCoroutine(SetupPostBattleUI(playerWon));

        // Remove ally unit's effects
        /*
        for (int i = 0; i < activeRoomAllies.Count; i++)
        {
            activeRoomAllies[i].ResetEffects();
        }
        */

        //ResetSelectedUnits();
      
        // Toggle player overlay and skill ui off
        ToggleUIElement(playerAbilities, false);
        ToggleUIElement(playerAbilityDesc, false);
        ToggleUIElement(endTurnButtonUI, false);
        ToggleUIElement(turnOrder, false);
        ResetActiveUnitTurnArrow();
        ToggleAllAlliesStatBar(true);
       
        // Toggle post battle ui on
        postBattleUI.TogglePostBattleUI(true);

        yield return new WaitForSeconds(.3f);

        if (playerWon)
        {
            defeatedEnemies.DisplayDefeatedEnemies();
        }

        


        if (playerWon)
        {
            // Enable unit level image for post combat
            for (int i = 0; i < activeRoomAllies.Count; i++)
            {
                activeRoomAllies[i].ToggleUnitLevelImage(true);

                // Give combat win heal to alive allies
                float combatWinHeal = ((float)combatCompleteHealPerc / 100f) * activeRoomAllies[i].GetUnitMaxHealth();
                activeRoomAllies[i].ResetPowerUI();
                activeRoomAllies[i].UpdateUnitCurHealth((int)combatWinHeal, false, false);
                activeRoomAllies[i].SpawnPowerUI(combatWinHeal, false, false, null, false);
            }

            postBattleUI.ToggleExpGainedUI(true);

            UnitFunctionality unitFunctionality = null;

            // Give Exp to ally units
            for (int i = 0; i < activeRoomAllies.Count; i++)
            {
                activeRoomAllies[i].ToggleUnitBG(false);

                // Give EXP to ally units, NOT a unit that was just added to player's party
                if (!activeRoomAllies[i].heroRoomUnit)
                    activeRoomAllies[i].UpdateUnitExp(GetExperienceGained());
                else
                {
                    unitFunctionality = activeRoomAllies[i];
                    unitFunctionality.ResetUnitCurAttackCharge();
                }


                activeRoomAllies[i].ToggleHideEffects(playerWon);
            }



            yield return new WaitForSeconds(.15f);

            postBattleUI.ToggleRewardsUI(true);


            // Reset hero room unit to be a normal unit (code purpose only)
            if (unitFunctionality != null)
                unitFunctionality.heroRoomUnit = false;

            yield return new WaitForSeconds(0);

            playerLost = false;


        }
        // If player LOST, reset game
        else
        {
            // Enable unit level image for post combat
            for (int i = 0; i < activeRoomAllies.Count; i++)
            {
                activeRoomAllies[i].ToggleUnitLevelImage(true);
            }

            // Set correct post battle UI
            postBattleUI.ToggleExpGainedUI(false);
            postBattleUI.ToggleRewardsUI(false);

            playerLost = true;

            ResetRoom(false);

            TeamGearManager.Instance.ResetGearOwned();

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

        /*
        if (activeSkill != null)
            this.activeSkill = activeSkill;
        else
        {
            if (GetActiveUnitFunctionality().unitData.GetNoCDSkill() != null)
                this.activeSkill = GetActiveUnitFunctionality().unitData.GetNoCDSkill();
            else
                this.activeSkill = null;
        }
        */

        //asd
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

        AudioManager.Instance.PauseCombatMusic(true);

        AudioManager.Instance.Play("AttackBarLoop");

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

    int GetActiveRoomAllyPowerLevels()
    {
        int combinedPowerLevel = 0;
        for (int i = 0; i < activeRoomAllies.Count; i++)
        {
            combinedPowerLevel += activeRoomAllies[i].GetUnitLevel() + activeRoomAllies[i].GetUnitValue();
            //Debug.Log("unit value = " + activeRoomAllies[i].GetUnitValue());
        }

        return combinedPowerLevel;
    }

    int GetActiveRoomEnemyPowerLevels()
    {
        int combinedPowerLevel = 0;
        for (int i = 0; i < activeRoomEnemies.Count; i++)
        {
            combinedPowerLevel += activeRoomEnemies[i].GetUnitLevel() + activeRoomEnemies[i].GetUnitValue();
        }

        return combinedPowerLevel;
    }

    public void StartRoom(RoomMapIcon room, FloorData activeFloor)
    {
        // Reset experience gained from killed enemies
        ResetExperienceGained();

        // If room type is enemy, spawn enemy room
        if (room.curRoomType == RoomMapIcon.RoomType.ENEMY || room.curRoomType == RoomMapIcon.RoomType.HERO || room.curRoomType == RoomMapIcon.RoomType.BOSS)
        {
            // Stop Map music
            AudioManager.Instance.PauseMapMusic(true);

            // Combat battle music
            AudioManager.Instance.Play("Combat");

            // Update background
            BackgroundManager.Instance.UpdateBackground(BackgroundManager.Instance.GetCombatForest());

            int spawnEnemyPosIndex = 0;

            FloorData floor = RoomManager.Instance.GetActiveFloor();

            int enemySpawnFirst = Random.Range(floor.minRoomValue, floor.maxRoomValue + 1);
            int enemySpawnValue = (enemySpawnFirst * (RoomManager.Instance.GetFloorCount()*2));

            /*
            if (activeTeam.Count != 1)
                enemySpawnValue *= (activeTeam.Count*3);
            */

            enemySpawnValue *= (RoomManager.Instance.GetFloorCount() * RoomManager.Instance.GetFloorCount());

            // If room is hero, spawn additional enemies
            if (room.curRoomType == RoomMapIcon.RoomType.HERO)
            {
                enemySpawnValue += (Random.Range(heroRoomMinEnemiesIncCount, heroRoomMaxEnemiesIncCount + 1) * RoomManager.Instance.GetFloorCount());
            }
            else if (room.curRoomType == RoomMapIcon.RoomType.BOSS)
            {
                enemySpawnValue += (Random.Range(heroRoomMinEnemiesIncCount, heroRoomMaxEnemiesIncCount + 1) * RoomManager.Instance.GetFloorCount() + 2);
            }
            // If room is just a regular enemy room, increment enemyspawnvalue by rooms cleared
            else
            {
                enemySpawnValue += RoomManager.Instance.GetRoomsCleared();
            }

            // Spawn enemy type
            for (int i = 0; i < enemySpawnValue; i++)
            {
                GameObject go = null;

                int spawnEnemyIndex = Random.Range(0, activeFloor.enemyUnits.Count);

                UnitData unit = activeFloor.enemyUnits[spawnEnemyIndex];  // Reference

                // Check if there are remaining enemy unit spawn locations left
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

                unitFunctionality.UpdateUnitValue(unit.GetUnitValue());

                int unitLevel = RoomManager.Instance.GetFloorCount() + (Random.Range(RoomManager.Instance.GetFloorCount() - 1, RoomManager.Instance.GetFloorCount() + 1));



                //Debug.Log("i = " + i);
                i += unit.GetUnitValue() + unitLevel;

                if (unitLevel == 0)
                {
                    unitLevel = 1;
                }


                if (i >= enemySpawnValue)
                {
                    if (unitLevel > 1)
                        unitLevel--;
                }
                else
                {
                    if (i != 0)
                        i--;
                }


                unitFunctionality.UpdateUnitDamageHits(unitLevel - 1);
                unitFunctionality.UpdateUnitHealingHits(unitLevel - 1);

                unitFunctionality.UpdateUnitLevel(unitLevel, 0, true);
                //Debug.Log("i = " + i);

                //Debug.Log("unit val " + unit.GetUnitValue());
                //Debug.Log("unit lv " + unitLevel);
                // Set unit stats
                unitFunctionality.UpdateUnitType("Enemy");
                unitFunctionality.ResetPosition();
                unitFunctionality.UpdateUnitName(unit.unitName);
                unitFunctionality.UpdateUnitSprite(unit.characterPrefab);

                unitFunctionality.UpdateSpeedIncPerLv((int)unit.speedIncPerLv);
                unitFunctionality.UpdatePowerIncPerLv((int)unit.powerIncPerLv);
                unitFunctionality.UpdateHealingPowerIncPerLv((int)unit.healingPowerIncPerLv);
                unitFunctionality.UpdateDefenseIncPerLv((int)unit.defenseIncPerLv);
                unitFunctionality.UpdateMaxHealthIncPerLv((int)unit.maxHealthIncPerLv);

                unitFunctionality.startingDamage = unit.startingPower;
                unitFunctionality.startingHealth = unit.startingMaxHealth;
                unitFunctionality.startingHealing = unit.startingHealingPower;
                unitFunctionality.startingDefense = unit.startingDefense;
                unitFunctionality.startingSpeed = unit.startingSpeed;

                unitFunctionality.unitData = unit;

                UnitData unitData = unitFunctionality.unitData;
                //unitData.ResetCurSkills();
                //unitFunctionality.SetStartingSkills(unitData.GetUnitSkills());
                //unitData.UpdateCurSkills(unitFunctionality.GetStartingSkills());

                //Debug.Log("2");
                unitFunctionality.UpdateUnitSkills(unitData.GetUnitSkills());
                unitFunctionality.UpdateCurrentSkills(unitData.GetUnitSkills());

                unitFunctionality.ResetSkill0Cooldown();
                unitFunctionality.ResetSkill1Cooldown();
                unitFunctionality.ResetSkill2Cooldown();
                unitFunctionality.ResetSkill3Cooldown();

                SkillsTabManager.Instance.ResetAllySkllls(unitFunctionality);

                // Apply to unit whether any of its skills leaches.
                if (unit.GetSkill0().isLeaching)
                    unitFunctionality.ToggleIsPoisonLeaching(true);
                else if (unit.GetSkill1().isLeaching)
                    unitFunctionality.ToggleIsPoisonLeaching(true);
                else if (unit.GetSkill2().isLeaching)
                    unitFunctionality.ToggleIsPoisonLeaching(true);
                else if (unit.GetSkill3().isLeaching)
                    unitFunctionality.ToggleIsPoisonLeaching(true);

                // If enemy unit spawned is NOT lv 1, spawn with level bonus stats
                if (unitFunctionality.GetUnitLevel() != 1)
                {
                    unitFunctionality.UpdateUnitSpeed(((unitFunctionality.GetUnitLevel()-1) * unitFunctionality.GetSpeedIncPerLv()) + unit.startingSpeed);
                    unitFunctionality.UpdateUnitPower(((unitFunctionality.GetUnitLevel()-1) * unitFunctionality.GetPowerIncPerLv()) + unit.startingPower);
                    unitFunctionality.UpdateUnitHealingPower(((unitFunctionality.GetUnitLevel()-1) * (int)unitFunctionality.GetHealingPowerIncPerLv()) + unit.startingHealingPower);
                    unitFunctionality.UpdateUnitDefense(((unitFunctionality.GetUnitLevel()-1) * (int)unitFunctionality.GetDefenseIncPerLv()) + unit.startingDefense);
                    unitFunctionality.UpdateUnitMaxHealth(((unitFunctionality.GetUnitLevel()-1) * (int)unitFunctionality.GetMaxHealthIncPerLv()) + unit.startingMaxHealth, true);
                }
                // If enemy spawned IS level 1, spawn with starting stats
                else
                {
                    unitFunctionality.UpdateUnitSpeed(unit.startingSpeed);
                    unitFunctionality.UpdateUnitPower(unit.startingPower);
                    unitFunctionality.UpdateUnitHealingPower(unit.startingHealingPower);
                    unitFunctionality.UpdateUnitDefense(unit.startingDefense);
                    unitFunctionality.UpdateUnitMaxHealth(unit.startingMaxHealth, true);
                }

                unitFunctionality.UpdateUnitVisual(unit.unitSprite);
                unitFunctionality.UpdateUnitIcon(unit.unitIcon);

                unitFunctionality.deathClip = unit.deathClip;
                unitFunctionality.hitRecievedClip = unit.hitRecievedClip;

                AddActiveRoomAllUnitsFunctionality(unitFunctionality);


            }

            int curChallengeCount = 0;

            // Determine enemy unit value
            int floorDiff = RoomManager.Instance.GetFloorDifficulty();

            // Loop through all units in room
            for (int x = 0; x < activeRoomAllUnitFunctionalitys.Count; x++)
            {
                // Enable allies level image for combat
                if (activeRoomAllUnitFunctionalitys[x].curUnitType == UnitFunctionality.UnitType.PLAYER)
                {
                    // Disable unit level image in team setup tab
                    activeRoomAllUnitFunctionalitys[x].ToggleUnitLevelImage(true);
                    //activeRoomAllies[i].ToggleActionNextBar(true);
                }
                // If unit in room is an ENEMY
                else
                {
                    /*
                    int roomChallengeCount = (RoomManager.Instance.GetFloorCount()) + floorDiff;

                    // Randomise enemy unit Level
                    int rand = Random.Range(1, 11);
                    if (rand >= 1 && rand < 8) // 70%
                    {
                        // Stay same level
                        activeRoomAllUnitFunctionalitys[x].UpdateUnitLevel(1 + RoomManager.Instance.GetFloorCount() - 1, 0, true);
                    }
                    else if (rand >= 8 && rand < 10) // 20%
                    {
                        // Increase lv mildly
                        activeRoomAllUnitFunctionalitys[x].UpdateUnitLevel(RoomManager.Instance.GetFloorCount(), 0, true);
                    }
                    else if (rand >= 9) // 10%
                    {
                        // Ensure player is NOT on floor one, Increase lv greatly
                        if (RoomManager.Instance.GetFloorCount() != 1)
                        {
                            activeRoomAllUnitFunctionalitys[x].UpdateUnitLevel(RoomManager.Instance.GetFloorCount() + 1, 0, true);
                        }
                        else
                        {
                            // Stay same level
                            activeRoomAllUnitFunctionalitys[x].UpdateUnitLevel(1 + RoomManager.Instance.GetFloorCount() - 1, 0, true);
                        }
                    }

                    // Calculate unit value
                    int unitValue = activeRoomAllUnitFunctionalitys[x].GetUnitLevel() + activeRoomAllUnitFunctionalitys[x].GetUnitValue();

                    // Add to unit value to current challenge count. If reached max difficulty, stop.
                    if (curChallengeCount < roomChallengeCount)
                        curChallengeCount += unitValue;
                    else
                        break;
                    */
                }

                // Update unit energy bar on
                UpdateActiveUnitStatBar(activeRoomAllUnitFunctionalitys[x], true, true);

                activeRoomAllUnitFunctionalitys[x].ResetIsDead();

                activeRoomAllUnitFunctionalitys[x].ToggleHideEffects(false);

                activeRoomAllUnitFunctionalitys[x].UpdateUnitCurAttackCharge();

                // Reset position for battle
                activeRoomAllUnitFunctionalitys[x].gameObject.GetComponent<RectTransform>().localPosition = Vector2.zero;
            }

            UpdateAllAlliesLevelColour();
            UpdateAllEnemiesLevelColour();

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
            // Map open SFX
            AudioManager.Instance.Play("Shop_Entered");

            // Stop Map music
            AudioManager.Instance.PauseMapMusic(true);

            // Update background
            BackgroundManager.Instance.UpdateBackground(BackgroundManager.Instance.GetShopForest());

            UpdateAllyVisibility(true, false);

            // Update unit energy bar off
            for (int i = 0; i < activeRoomAllies.Count; i++)
            {
                UpdateActiveUnitStatBar(activeRoomAllies[i], true, false);
                activeRoomAllies[i].ToggleActionNextBar(false);
            }

            playerUIElement.UpdateAlpha(0);     // Disable player UI
            ToggleUIElement(turnOrder, false);  // Disable turn order
            ResetSelectedUnits();   // Disable all unit selections
            //ToggleAllAlliesHealthBar(false);    // Disable all unit health bar visual
            ToggleAllowSelection(false);

            ShopManager.Instance.FillShopItems(false, true);

            // Update allies into position for shop
            UpdateAllAlliesPosition(false, GetActiveUnitType(), false, true);

            MapManager.Instance.mapOverlay.ToggleTeamPageButton(false);
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
                    float adjustedPower = (1f + ((float)GetActiveSkill().percIncPower / 100f)) * power;
                    return RandomisePower((int)adjustedPower);
                }
                else
                    RandomisePower((int)power);
            }
        }

        return RandomisePower((int)power);
    }

    public int RandomisePower(int power)
    {
        float powerToRandomise = ((randomPerc / 100f) * power);
        int powerToRandomiseInt = (int)powerToRandomise;
        float randPower = Random.Range(power - powerToRandomiseInt - randomBaseOffset, (power + powerToRandomiseInt+1 + randomBaseOffset));
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
        
    IEnumerator UpdateSelectedUnitsEffectVisual()
    {
        //Debug.Log("is using thing");
        // Display effect visual to each selected unit before the power is shown
        if (GetActiveSkill().skillLaunch != null)
        {
            if (GetActiveSkill().skillProjectile == null)
            {
                yield return new WaitForSeconds(allyRangedSkillWaitTime / 1.5f);

                // Projectile Launch SFX
                AudioManager.Instance.Play(GetActiveSkill().skillLaunch.name);
            }
        }

        yield return new WaitForSeconds(allyRangedSkillWaitTime/3);

        // Display effect visual to each selected unit before the power is shown
        for (int i = 0; i < unitsSelected.Count; i++)
        {
            if (GetActiveSkill().targetEffectVisualAC != null)
            {
                unitsSelected[i].UpdateEffectVisualAnimator(GetActiveSkill().targetEffectVisualAC);

                if (GetActiveSkill().skillHit != null && GetActiveSkill().skillProjectile == null)
                    AudioManager.Instance.Play(GetActiveSkill().skillHit.name);

                yield return new WaitForSeconds(timeTillNextTargetEffetVisual);
            }
        }
    }

    public IEnumerator WeaponAttackCommand(int power, int hitCount = 0, int effectHitAcc = -1)
    {
        // Reset each units power UI
        for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
        {
            activeRoomAllUnitFunctionalitys[i].ResetPowerUI();
        }

        GetActiveUnitFunctionality().effects.UpdateAlpha(1);

        UnitFunctionality castingUnit = GetActiveUnitFunctionality();

        if (GetActiveSkill().curRangedType == SkillData.SkillRangedType.RANGED)
        {
            GetActiveUnitFunctionality().GetAnimator().SetTrigger("SkillFlg");

            // Display active unit hits remaining text, Update hits remaining text
            GetActiveUnitFunctionality().ToggleHitsRemainingText(true);
            if (power == 0)
                GetActiveUnitFunctionality().UpdateHitsRemainingText(effectHitAcc);
            else
                GetActiveUnitFunctionality().UpdateHitsRemainingText(hitCount);

            // Display effect visual to each selected unit before the power is shown
            StartCoroutine(UpdateSelectedUnitsEffectVisual());

            yield return new WaitForSeconds(allyRangedSkillWaitTime);

            // If skill has no projectile, dont spawn it. stops white square from spawning
            if (GetActiveSkill().skillProjectile != null)
            {
                for (int w = 0; w < hitCount; w++)
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
            }

            if (GetActiveSkill().targetEffectVisualAC != null)
                yield return new WaitForSeconds(unitPowerUIWaitTime);
            else
                yield return new WaitForSeconds(allyRangedSkillWaitTime/2);
        }
        else
        {
            GetActiveUnitFunctionality().GetAnimator().SetTrigger("AttackFlg");

            // Display active unit hits remaining text, Update hits remaining text
            GetActiveUnitFunctionality().ToggleHitsRemainingText(true);
            if (power == 0)
                GetActiveUnitFunctionality().UpdateHitsRemainingText(effectHitAcc);
            else
                GetActiveUnitFunctionality().UpdateHitsRemainingText(hitCount);

            // Display effect visual to each selected unit before the power is shown
            StartCoroutine(UpdateSelectedUnitsEffectVisual());

            yield return new WaitForSeconds(allyMeleeSkillWaitTime);

            // Attack launch SFX
            //AudioManager.Instance.Play("Attack_Sword");
            if (GetActiveSkill().targetEffectVisualAC != null)
                yield return new WaitForSeconds(unitPowerUIWaitTime);
            else
                yield return new WaitForSeconds(allyMeleeSkillWaitTime / 2);

        }

        // For no power skills
        if (GetActiveSkill().curSkillPower == 0)
        {
           // GetActiveUnitFunctionality().GetAnimator().SetTrigger("SkillFlg");

            // Loop through all selected units
            for (int x = 0; x < unitsSelected.Count; x++)
            {
                if (x > unitsSelected.Count)
                    break;

                // If active skil has an effect AND it's not a self cast, apply it to selected targets
                if (GetActiveSkill().effect != null && !GetActiveSkill().isSelfCast)
                {
                    int val = effectHitAcc;

                    // If active skill doubles current effects, do it
                    if (GetActiveSkill().isDoublingEffect)
                    {
                        if (unitsSelected[x].GetEffect("POISON"))
                        {
                            int val2 = unitsSelected[x].GetEffect("POISON").GetTurnCountRemaining();

                            int maxEffectCount = EffectManager.instance.GetMaxEffectTurnsRemaining();
                            if (val2 > maxEffectCount)
                                val2 = maxEffectCount;

                            unitsSelected[x].AddUnitEffect(GetActiveSkill().effect, unitsSelected[x], GetActiveSkill().effectTurnLength, val2);
                        }
                    }
                    
                    unitsSelected[x].AddUnitEffect(GetActiveSkill().effect, unitsSelected[x], GetActiveSkill().effectTurnLength, val);
                }

                // If skill targets dead targets, do so
                if (GetActiveSkill().curskillSelectionAliveType == SkillData.SkillSelectionAliveType.DEAD)
                {
                    if (unitsSelected[x].isDead)
                    {
                        // Revive unit
                        unitsSelected[x].ReviveUnit();
                    }
                }

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

            for (int i = 0; i < unitsSelected.Count; i++)
            {
                // If skill removes random effects, do so
                if (activeSkill.isCleansingEffectRandom)
                {
                    for (int c = 0; c < activeSkill.cleanseCount; c++)
                    {
                        unitsSelected[i].DecreaseRandomNegativeEffect();
                        yield return new WaitForSeconds(.25f);
                    }
                }
            }

            // Loop as many times as power text will appear
            for (int x = 0; x < hitCount; x++)
            {
                // If we've cycled through all units selected, Disable hits remaining text
                // if (x == hitCount-1)
                //GetActiveUnitFunctionality().ToggleHitsRemainingText(false);

                /*
                for (int d = 0; d < unitsSelected.Count; d++)
                {

                }
                */

                // If 1 enemy is trying to target an ally, dont?
                if (unitsSelected.Count == 0)
                {
                    GetActiveUnitFunctionality().ToggleHitsRemainingText(false);
                    break;
                }

                if (unitsSelected[0] == null)
                {
                    GetActiveUnitFunctionality().ToggleHitsRemainingText(false);
                    continue;
                }

                GetActiveUnitFunctionality().UpdateHitsRemainingText(hitCount - x);

                // Loop through all selected units
                for (int i = unitsSelected.Count - 1; i >= 0; i--)
                {
                    bool parrying = false;

                    int originalPower = AdjustSkillPowerTargetEffectAmp(power);

                    // Helps catch the null error
                    if (i < unitsSelected.Count)
                    {
                        if (unitsSelected[i] == null)
                            continue;

                        if (unitsSelected[i].isDead)
                            continue;
                    }

                    float absPower = Mathf.Abs(originalPower);
                    //float tempPower = ((float)unitsSelected[i].curRecieveDamageAmp / 100f) * absPower;
                    float newPower = absPower;

                    float newHealingPower = originalPower;

                    // If unit missed, give them 0 power output
                    if (effectHitAcc == 0)
                    {
                        newPower = 0;
                        newHealingPower = 0;
                    }

                    if (GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT)
                    {
                        // Check if target unit has poison, half the heal if it does
                        for (int y = 0; y < unitsSelected[i].GetEffects().Count; y++)
                        {
                            if (unitsSelected[i].activeEffects[y].curEffectName == Effect.EffectName.POISON)
                            {
                                newHealingPower /= 2;
                                //Debug.Log("Healing power halfed");
                                break;
                            }
                        }
                    }

                    int orderCount;

                    bool blocked;

                    float targetBlockChance = Random.Range(1, 101);
                    if (unitsSelected[i].GetCurDefense() >= targetBlockChance)
                        blocked = true;
                    else
                        blocked = false;
                    //Debug.Log(temp4);

                    // Cause power
                    if (activeSkill.curSkillType == SkillData.SkillType.OFFENSE)
                    {
                        orderCount = 2;
                        if (hasBeenLuckyHit)
                        {
                            hasBeenLuckyHit = false;
                            orderCount--;
                        }
                        unitsSelected[i].StartCoroutine(unitsSelected[i].SpawnPowerUI((int)newPower, false, true, null, blocked));

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

                        unitsSelected[i].StartCoroutine(unitsSelected[i].SpawnPowerUI(newHealingPower, false, false, null));
                    }

                    // Increase health on targeting unit If target did not block
                    if (activeSkill.curSkillType == SkillData.SkillType.SUPPORT && !blocked)
                        unitsSelected[i].UpdateUnitCurHealth((int)newHealingPower);

                    // Decrease health on targeting unit
                    if (activeSkill.curSkillType == SkillData.SkillType.OFFENSE && !blocked)
                    {
                        float health = newPower;         
                        unitsSelected[i].UpdateUnitCurHealth((int)health, true);
                    }

                    // If active skill has an effect AND it's not a self cast, apply it to selected targets
                    if (GetActiveSkill().effect != null && !GetActiveSkill().isSelfCast)
                        unitsSelected[i].AddUnitEffect(GetActiveSkill().effect, unitsSelected[i], GetActiveSkill().effectTurnLength, 1);

                    /*
                    // Reset unit's prev power text for future power texts
                    if (x == activeSkill.skillAttackAccMult - 1)
                        unitsSelected[i].ResetPreviousPowerUI();
                    */

#if !UNITY_EDITOR
                        Vibration.Vibrate(15);
#endif
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

        GetActiveUnitFunctionality().ToggleHitsRemainingText(false);

        // If skill is self cast, do it here
        if (GetActiveSkill().isSelfCast)
        {
            if (GetActiveSkill().effect != null)
            {
                GetActiveUnitFunctionality().AddUnitEffect(GetActiveSkill().effect, GetActiveUnitFunctionality(), GetActiveSkill().effectTurnLength, effectHitAcc);
                
                #if !UNITY_EDITOR
                    Vibration.Vibrate(15);
                #endif
            }
        }

        // Disable unit selection just before attack
        for (int y = 0; y < unitsSelected.Count; y++)
        {
            unitsSelected[y].ToggleSelected(false);
        }

        int index = 0;

        for (int i = 0; i < GetActiveUnitFunctionality().GetAllSkills().Count; i++)
        {
            if (GetActiveUnitFunctionality().GetSkill(i).skillName == GetActiveSkill().skillName)
            {
                //Debug.Log("Skill name " + GetActiveUnitFunctionality().GetSkill(i).skillName);
                //Debug.Log("used Skill name " + GetActiveSkill().skillName);
                index = i;
                break;
            }
        }

        //Debug.Log("index is " + index);

        if (index == 0)
            GetActiveUnitFunctionality().SetSkill0CooldownMax();
        else if (index == 1)
            GetActiveUnitFunctionality().SetSkill1CooldownMax();
        else if (index == 2)
            GetActiveUnitFunctionality().SetSkill2CooldownMax();
        else if (index == 3)
            GetActiveUnitFunctionality().SetSkill3CooldownMax();

        yield return new WaitForSeconds(postHitWaitTime);

        // If active unit is player, setup player UI
        if (GetActiveUnitFunctionality().unitData.curUnitType == UnitData.UnitType.PLAYER)
        {
            // Resume combat music
            AudioManager.Instance.PauseCombatMusic(false);

            Weapon.Instance.ResetAcc();

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
        List<ItemPiece> items = GetActiveUnitFunctionality().GetEquipItems();
        int count = GetActiveUnitFunctionality().GetEquipItems().Count;

        for (int g = 0; g < count; g++)
        {
            int procChance = items[g].procChance;

            power = AdjustSkillPowerTargetEffectAmp(power);
            float absPowerLucky = Mathf.Abs(power);
            //float tempPowerLucky = (unitTarget.curRecieveDamageAmp / 100f) * absPowerLucky;
            float newPowerLucky = absPowerLucky;

            if (items[g].itemName == "Lucky Dice")
            {
                if (CheckItemHitChance(GetActiveUnitFunctionality().GetEquipItemCount("Lucky Dice"), procChance))
                {
                    hasBeenLuckyHit = true;
                    unitTarget.StartCoroutine(unitTarget.SpawnPowerUI(GetActiveUnitFunctionality().GetPowerIncPerLv() * newPowerLucky, false, true, null));
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

                    unitCaster.StartCoroutine(unitCaster.SpawnPowerUI(GetActiveUnitFunctionality().GetPowerIncPerLv() * newPower, false, true, null));
                    unitCaster.UpdateUnitCurHealth((int)newPower, false);
                    continue;
                }
            }
        }
    }

    public void SetupPlayerUI()
    {
        SetupPlayerSkillsUI();
        UpdateSkillDetails(GetActiveUnitFunctionality().GetNoCDSkill());
        UpdateAllSkillIconAvailability();
        UpdateUnitSelection(GetActiveUnitFunctionality().GetNoCDSkill());
        UpdateUnitsSelectedText();
         //EnableFreeSkillSelection();
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
            activeRoomAllies[i].ToggleActionNextBar(toggle);
        }
    }
    #endregion
    #region Update Skill Icons Overlay UI
    public void UpdateAllSkillIconAvailability()
    {
        // Update all skill icon Energy text + unavailable image
        if (GetActiveUnitFunctionality().GetSkillCurCooldown(GetActiveUnitFunctionality().GetSkill(0)) > 0)
        {
            //GetActiveUnitFunctionality().DecreaseSkill1Cooldown();

            if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                UpdateSkillIconAvailability(skill0IconUnavailableImage, skill0IconCooldownUIText, GetActiveUnitFunctionality().unitData.GetSkill0().skillCooldown.ToString(), false);
                DisableButton(skill0Button);
                skill0IconCooldownUIText.UpdateUIText(GetActiveUnitFunctionality().GetSkillCurCooldown(GetActiveUnitFunctionality().GetSkill(0)).ToString());
            }
        }
        else
        {
            if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                UpdateSkillIconAvailability(skill0IconUnavailableImage, skill0IconCooldownUIText, GetActiveUnitFunctionality().unitData.GetSkill0().skillCooldown.ToString(), true);
                EnableButton(skill0Button);
                skill0IconCooldownUIText.UpdateUIText(GetActiveUnitFunctionality().GetSkillCurCooldown(GetActiveUnitFunctionality().GetSkill(0)).ToString());
            }
        }

        // Update all skill icon Energy text + unavailable image
        if (GetActiveUnitFunctionality().GetSkillCurCooldown(GetActiveUnitFunctionality().GetSkill(1)) > 0)
        {
            //GetActiveUnitFunctionality().DecreaseSkill1Cooldown();

            if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                UpdateSkillIconAvailability(skill1IconUnavailableImage, skill1IconCooldownUIText, GetActiveUnitFunctionality().unitData.GetSkill1().skillCooldown.ToString(), false);
                DisableButton(skill1Button);
                skill1IconCooldownUIText.UpdateUIText(GetActiveUnitFunctionality().GetSkillCurCooldown(GetActiveUnitFunctionality().GetSkill(1)).ToString());
            }
        }
        else
        {
            if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                UpdateSkillIconAvailability(skill1IconUnavailableImage, skill1IconCooldownUIText, GetActiveUnitFunctionality().unitData.GetSkill1().skillCooldown.ToString(), true);
                EnableButton(skill1Button);
                skill1IconCooldownUIText.UpdateUIText(GetActiveUnitFunctionality().GetSkillCurCooldown(GetActiveUnitFunctionality().GetSkill(1)).ToString());
            }
        }

        if (GetActiveUnitFunctionality().GetSkillCurCooldown(GetActiveUnitFunctionality().GetSkill(2)) > 0)
        {
            //GetActiveUnitFunctionality().DecreaseSkill2Cooldown();

            if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                UpdateSkillIconAvailability(skill2IconUnavailableImage, skill2IconCooldownUIText, GetActiveUnitFunctionality().unitData.GetSkill2().skillCooldown.ToString(), false);
                DisableButton(skill2Button);
                skill2IconCooldownUIText.UpdateUIText(GetActiveUnitFunctionality().GetSkillCurCooldown(GetActiveUnitFunctionality().GetSkill(2)).ToString());
            }
        }
        else
        {
            if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                UpdateSkillIconAvailability(skill2IconUnavailableImage, skill2IconCooldownUIText, GetActiveUnitFunctionality().unitData.GetSkill2().skillCooldown.ToString(), true);
                EnableButton(skill2Button);
                skill2IconCooldownUIText.UpdateUIText(GetActiveUnitFunctionality().GetSkillCurCooldown(GetActiveUnitFunctionality().GetSkill(2)).ToString());
            }
        }

        if (GetActiveUnitFunctionality().GetSkillCurCooldown(GetActiveUnitFunctionality().GetSkill(3)) > 0)
        {
            //GetActiveUnitFunctionality().DecreaseSkill1Cooldown();

            if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                UpdateSkillIconAvailability(skill3IconUnavailableImage, skill3IconCooldownUIText, GetActiveUnitFunctionality().unitData.GetSkill3().skillCooldown.ToString(), false);
                DisableButton(skill3Button);
                skill3IconCooldownUIText.UpdateUIText(GetActiveUnitFunctionality().GetSkillCurCooldown(GetActiveUnitFunctionality().GetSkill(3)).ToString());
            }
        }
        else
        {
            if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                UpdateSkillIconAvailability(skill3IconUnavailableImage, skill3IconCooldownUIText, GetActiveUnitFunctionality().unitData.GetSkill3().skillCooldown.ToString(), true);
                EnableButton(skill3Button);
                skill3IconCooldownUIText.UpdateUIText(GetActiveUnitFunctionality().GetSkillCurCooldown(GetActiveUnitFunctionality().GetSkill(3)).ToString());
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


    public void EnableFreeSkillSelection()
    {
        SkillsTabManager.Instance.UpdateLockedSkills();

        if (SkillsTabManager.Instance.skillBase1.GetIsLocked())
            playerSkill0.ToggleHiddenImage(true);
        else
            playerSkill0.ToggleHiddenImage(false);

        if (SkillsTabManager.Instance.skillBase2.GetIsLocked())
            playerSkill1.ToggleHiddenImage(true);
        else
            playerSkill1.ToggleHiddenImage(false);

        if (SkillsTabManager.Instance.skillBase3.GetIsLocked())
            playerSkill2.ToggleHiddenImage(true);
        else
            playerSkill2.ToggleHiddenImage(false);

        if (SkillsTabManager.Instance.skillBase4.GetIsLocked())
            playerSkill3.ToggleHiddenImage(true);
        else
            playerSkill3.ToggleHiddenImage(false);


        // todo work with new locked skills
        for (int i = 0; i < GetActiveUnitFunctionality().GetAllSkills().Count; i++)
        {
            // If Skill has NO cooldown, and IS NOT locked, select the first one it finds
            if (GetActiveUnitFunctionality().GetSkillCurCooldown(GetActiveUnitFunctionality().GetSkill(0)) == 0 && !SkillsTabManager.Instance.skillBase1.GetIsLocked())
            {
                skill0.ToggleSelected(true);

                UpdateActiveSkill(GetActiveUnitFunctionality().GetAllSkills()[0]);
                UpdateSkillDetails(GetActiveUnitFunctionality().GetAllSkills()[0]);

                break;
            }
            else if (GetActiveUnitFunctionality().GetSkillCurCooldown(GetActiveUnitFunctionality().GetSkill(1)) == 0 && !SkillsTabManager.Instance.skillBase2.GetIsLocked())
            {
                skill1.ToggleSelected(true);

                UpdateActiveSkill(GetActiveUnitFunctionality().GetAllSkills()[1]);
                UpdateSkillDetails(GetActiveUnitFunctionality().GetAllSkills()[1]);

                break;
            }
            else if (GetActiveUnitFunctionality().GetSkillCurCooldown(GetActiveUnitFunctionality().GetSkill(2)) == 0 && !SkillsTabManager.Instance.skillBase3.GetIsLocked())
            {
                skill2.ToggleSelected(true);

                UpdateActiveSkill(GetActiveUnitFunctionality().GetAllSkills()[2]);
                UpdateSkillDetails(GetActiveUnitFunctionality().GetAllSkills()[2]);

                break;
            }
            else if (GetActiveUnitFunctionality().GetSkillCurCooldown(GetActiveUnitFunctionality().GetSkill(3)) == 0 && !SkillsTabManager.Instance.skillBase4.GetIsLocked())
            {
                skill3.ToggleSelected(true);

                UpdateActiveSkill(GetActiveUnitFunctionality().GetAllSkills()[3]);
                UpdateSkillDetails(GetActiveUnitFunctionality().GetAllSkills()[3]);

                break;
            }

            //if (SkillsTabManager.Instance.skillBase1.GetIsLocked())
        }
    }

    public void ToggleSkillVisibility(bool toggle)
    {
        skill0.GetComponent<CanvasGroup>().interactable = toggle;
        skill0.GetComponent<CanvasGroup>().blocksRaycasts = toggle;
        skill1.GetComponent<CanvasGroup>().interactable = toggle;
        skill1.GetComponent<CanvasGroup>().blocksRaycasts = toggle;
        skill2.GetComponent<CanvasGroup>().interactable = toggle;
        skill2.GetComponent<CanvasGroup>().blocksRaycasts = toggle;
        skill3.GetComponent<CanvasGroup>().interactable = toggle;
        skill3.GetComponent<CanvasGroup>().blocksRaycasts = toggle;

        if (toggle)
        {
            skill0.GetComponent<CanvasGroup>().alpha = 1;
            skill1.GetComponent<CanvasGroup>().alpha = 1;
            skill2.GetComponent<CanvasGroup>().alpha = 1;
            skill3.GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            skill0.GetComponent<CanvasGroup>().alpha = 0;
            skill1.GetComponent<CanvasGroup>().alpha = 0;
            skill2.GetComponent<CanvasGroup>().alpha = 0;
            skill3.GetComponent<CanvasGroup>().alpha = 0;
        }
    }
    #endregion

    public void UpdateSkillDetails(SkillData skill)
    {
        if (skill == null)
            return;

        ToggleUIElement(playerAbilityDesc, true);

        bool tempAttack = false;

        if (skill.curSkillType == SkillData.SkillType.OFFENSE)
            tempAttack = true;
        else
            tempAttack = false;


        UnitFunctionality activeUnit = GetActiveUnitFunctionality();

        if (skill.curSkillType == SkillData.SkillType.OFFENSE)
        {
            abilityDetailsUI.UpdateSkillUI(skill.skillName, skill.skillDescr, skill.GetCalculatedSkillPowerStat(),
                skill.skillAttackCount + activeUnit.GetUnitDamageHits(), tempAttack, skill.GetCalculatedSkillSelectionCount(),
                skill.GetCalculatedSkillPowerStat(), skill.skillCooldown, skill.skillAttackAccMult, skill.skillPowerIcon, skill.skillSprite, skill.special);
        }
        else
        {
            abilityDetailsUI.UpdateSkillUI(skill.skillName, skill.skillDescr, skill.GetCalculatedSkillPowerStat(),
                skill.skillAttackCount + activeUnit.GetUnitHealingHits(), tempAttack, skill.GetCalculatedSkillSelectionCount(),
                skill.GetCalculatedSkillPowerStat(), skill.skillCooldown, skill.skillAttackAccMult, skill.skillPowerIcon, skill.skillSprite, skill.special);
        }


        /*
        // Update Unit Overlay Energy
        abilityDetailsUI.UpdateUnitOverlayEnergyUI(GetActiveUnitFunctionality(),
            activeUnit.GetUnitCurEnergy(), activeUnit.GetUnitMaxEnergy());
        */

        // Update Unit Overlay Health
        abilityDetailsUI.UpdateUnitOverlayHealthUI(activeUnit, activeUnit.GetUnitCurHealth(), activeUnit.GetUnitMaxHealth());
    }

    public void RemoveUnitFromTurnOrder(UnitFunctionality unit)
    {
        if (activeRoomAllUnitFunctionalitys.Contains(unit))
            activeRoomAllUnitFunctionalitys.Remove(unit);

        AddExperienceGained(unit.GetUnitExpKillGained());
        UpdateEnemiesKilled(unit);
    }

    public void AddUnitFromTurnOrder(UnitFunctionality unit)
    {
        activeRoomAllUnitFunctionalitys.Add(unit);
    }

    public void RemoveUnit(UnitFunctionality unitFunctionality)
    {
        if (activeRoomEnemies.Contains(unitFunctionality))
            activeRoomEnemies.Remove(unitFunctionality);

        if (activeRoomAllies.Contains(unitFunctionality))
            activeRoomAllies.Remove(unitFunctionality);

        if (activeRoomAllUnitFunctionalitys.Contains(unitFunctionality))
            activeRoomAllUnitFunctionalitys.Remove(unitFunctionality);
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

    public void AdjustSpeedBuffUnit(bool inc = true)
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
                    if (inc)
                    {
                        if (activeRoomAllUnitFunctionalitys[i].GetIsSpeedUp())
                            break;
                    }
                    else
                    {
                        if (activeRoomAllUnitFunctionalitys[i].GetIsSpeedDown())
                            break;
                    }

                    activeRoomAllUnitFunctionalitys[i].ToggleIsSpeedUp(true);

                    // units current speed
                    float newSpeed = 0;
                    float curSpeed = activeRoomAllUnitFunctionalitys[i].curSpeed;
                    if (inc)
                    {
                        newSpeed = curSpeed + (((activeRoomAllUnitFunctionalitys[i].GetEffects()[x].powerPercent / 100f) * curSpeed) * activeRoomAllUnitFunctionalitys[i].GetSpeedIncPerLv());
                    }
                    else
                    {
                        newSpeed = curSpeed - (((activeRoomAllUnitFunctionalitys[i].GetEffects()[x].powerPercent / 100f) * curSpeed) * activeRoomAllUnitFunctionalitys[i].GetSpeedIncPerLv());
                    }

                    // units new speed
                    activeRoomAllUnitFunctionalitys[i].UpdateUnitOldSpeed((int)curSpeed);

                    // updating new speed
                    activeRoomAllUnitFunctionalitys[i].UpdateUnitSpeed((int)newSpeed);
                    activeRoomAllUnitFunctionalitys[i].CalculateUnitAttackChargeTurnStart();
                    break;
                }
            }
        }
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
                        float curDef = activeRoomAllUnitFunctionalitys[i].GetCurDefense();
                        // units new def
                        float newDef = curDef + ((activeRoomAllUnitFunctionalitys[i].GetEffects()[x].powerPercent / 100f)) * curDef;
                        newDef += 4;

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

                        // units new def
                        float newDef = activeRoomAllUnitFunctionalitys[i].GetOldDefense();

                        // updating new defense
                        activeRoomAllUnitFunctionalitys[i].UpdateUnitDefense((int)newDef);
                        break;
                    }
                }
            }
        }
    }

    public void RemoveSpeedUpEffectUnit(UnitFunctionality unit)
    {
        unit.UpdateUnitSpeed((int)GetActiveUnitFunctionality().GetOldSpeed());
        unit.ResetOldSpeed();
        unit.ToggleIsSpeedUp(false);
    }
    public void RemoveSpeedDownEffectUnit(UnitFunctionality unit)
    {
        unit.UpdateUnitSpeed((int)GetActiveUnitFunctionality().GetOldSpeed());
        unit.ResetOldSpeed();
        unit.ToggleIsSpeedDown(false);
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
            activeRoomAllUnitFunctionalitys[i].UpdateUnitCurAttackCharge();
            //activeRoomAllUnits[i].UpdateUnitCurAttackCharge(activeRoomAllUnitFunctionalitys[i].GetCurAttackCharge());
        }
    }

    void ResetAllUnitPowerUIHeight()
    {
        for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
        {
            activeRoomAllUnitFunctionalitys[i].ResetDamageHealCount();
        }
    }

    void ToggleUnitEffectTooltipsOff(bool onlyEnemies = false)
    {
        ButtonFunctionality[] buttons = GameObject.FindObjectsOfType<ButtonFunctionality>();

        foreach (ButtonFunctionality button in buttons)
        {
            StartCoroutine(button.HideEffectTooltipOvertime(onlyEnemies));
        }
    }

    public void UpdateTurnOrder()
    {
        if (combatOver)
            return;

        #region Check if Player Team Won or Lost, Then end Battle

        int allyCount = 0;
        int enemyCount = 0;
        for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
        {
            if (activeRoomAllUnitFunctionalitys[i].curUnitType == UnitFunctionality.UnitType.PLAYER)
                allyCount++;
            else
                enemyCount++;
        }

        if (allyCount == 0)
        {
            playerWon = false;
            StartCoroutine(SetupPostBattleUI(playerWon));
            return;
        }
        else if (enemyCount == 0)
        {
            playerWon = true;
            SetupItemRewards();
            return;
        }

        #endregion

        ToggleUnitEffectTooltipsOff(true);
        //ToggleSkillVisibility(false);

        ToggleUIElement(turnOrder, true);   // Enable turn order UI

        ResetSelectedUnits();
        ToggleSelectingUnits(true);
        ToggleAllowSelection(true);

        UpdateAllSkillIconAvailability();   // Update active unit's skill cooldowns to toggle 'On Cooldown' before switching to new active unit

        GetActiveUnitFunctionality().StartCoroutine(GetActiveUnitFunctionality().DecreaseEffectTurnsLeft(false));

        DetermineTurnOrder();

        // Only decrease skill CDs during combat
        GetActiveUnitFunctionality().DecreaseSkill0Cooldown();
        GetActiveUnitFunctionality().DecreaseSkill1Cooldown();
        GetActiveUnitFunctionality().DecreaseSkill2Cooldown();
        GetActiveUnitFunctionality().DecreaseSkill3Cooldown();

        ResetAllUnitPowerUIHeight();

        IncreaseAttackBarAllUnits();

        UpdateAllSkillIconAvailability();   // Update active unit's skill cooldowns to toggle 'On Cooldown' before switching to new active unit

        GetActiveUnitFunctionality().StartFocusUnit();

        // Reset active unit attack charge
        GetActiveUnitFunctionality().ResetUnitCurAttackCharge();
        GetActiveUnitFunctionality().unitData.UpdateUnitCurAttackCharge(GetActiveUnitFunctionality().GetCurAttackCharge());

        //Trigger Start turn effects
        GetActiveUnitFunctionality().StartCoroutine(GetActiveUnitFunctionality().DecreaseEffectTurnsLeft(true, false));
    }

    public void ContinueTurnOrder()
    {
        // Trigger Unit Energy regen 
        //UpdateActiveUnitEnergyBar(true, true, GetActiveUnitFunctionality().unitStartTurnEnergyGain);


        // Toggle player UI accordingly if it's their turn or not
        if (activeRoomAllUnitFunctionalitys[0].curUnitType == UnitFunctionality.UnitType.PLAYER)
        {
            playerUIElement.UpdateAlpha(1);
            SetupPlayerUI();
            SetupPlayerSkillsUI();
            ToggleSkillVisibility(true);
            UpdatePlayerAbilityUI();
            UpdateActiveUnitNameText(GetActiveUnitFunctionality().GetUnitName());

            // Set the basic skill to be on by default to begin with
            //activeSkill = GetActiveUnitFunctionality().unitData.skill0;

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
            //UpdateSkillDetails(activeSkill);

            UpdateUnitSelection(activeSkill);
            UpdateEnemyPosition(true);
        }

        UpdateActiveUnitTurnArrow();

        // Update allies into position for battle/shop
        UpdateAllAlliesPosition(false, GetActiveUnitType());

        // Skip unit turn if all skills are on cooldown
        if (GetActiveUnitFunctionality().GetSkillCurCooldown(GetActiveUnitFunctionality().GetSkill(0)) > 0 || SkillsTabManager.Instance.skillBase1.GetIsLocked())
        {
            if (GetActiveUnitFunctionality().GetSkillCurCooldown(GetActiveUnitFunctionality().GetSkill(1)) > 0 || SkillsTabManager.Instance.skillBase2.GetIsLocked())
            {
                if (GetActiveUnitFunctionality().GetSkillCurCooldown(GetActiveUnitFunctionality().GetSkill(2)) > 0 || SkillsTabManager.Instance.skillBase3.GetIsLocked())
                {
                    if (GetActiveUnitFunctionality().GetSkillCurCooldown(GetActiveUnitFunctionality().GetSkill(3)) > 0 || SkillsTabManager.Instance.skillBase4.GetIsLocked())
                    {
                        StartCoroutine(SkipTurnAfterWait());
                    }
                }
            }
        }

        bool byPass = false;
        bool deadTargetsRemain = false;

        // If only available skill for unit targets dead allies, and there are none available, force end turn
        for (int i = 0; i < 4; i++)
        {
            if (GetActiveUnitFunctionality().GetUnitLevel() <= 2)
            {
                if (GetActiveUnitFunctionality().GetSkill(0).curskillSelectionAliveType == SkillData.SkillSelectionAliveType.DEAD)
                {
                    byPass = true;

                    for (int x = 0; x < activeRoomAllies.Count; x++)
                    {
                        if (activeRoomAllies[x].curUnitType == UnitFunctionality.UnitType.PLAYER)
                        {
                            if (activeRoomAllies[x].isDead)
                                deadTargetsRemain = true;
                        }
                    }
                }
            }
        }

        // If skill 0 with only 1 skill slot available, if the skill targets dead allies, and there are none, force skip turn.
        if (!deadTargetsRemain && byPass && GetActiveUnitFunctionality().GetUnitLevel() <= 2)
        {
            StartCoroutine(SkipTurnAfterWait());
        }
    }

    IEnumerator SkipTurnAfterWait()
    {
        yield return new WaitForSeconds(.75f);

        UpdateEnemyPosition(false);
        UpdateTurnOrder();
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

        if (usedSkill.curskillSelectionAliveType == SkillData.SkillSelectionAliveType.ALIVE)
        {
            // If skill selection type is only on ENEMIES
            if (usedSkill.curSkillSelectionType == SkillData.SkillSelectionType.ENEMIES)
            {
                // if the skill user is a PLAYER
                if (GetActiveUnitFunctionality().unitData.curUnitType == UnitData.UnitType.PLAYER)
                {
                    // If any enemies are taunting, select them
                    if (IsEnemyTaunting().Count >= 1)
                    {
                        for (int i = IsEnemyTaunting().Count - 1; i >= 0; i--)
                        {
                            selectedAmount++;
                            targetUnit(IsEnemyTaunting()[i]);

                            // If enough units have been selected FOR ability max targets, or max amount of enemy units tanking
                            if (selectedAmount == usedSkill.GetCalculatedSkillSelectionCount() || selectedAmount == IsEnemyTaunting().Count)
                                return;
                        }
                    }

                    // only select the closest ENEMY units
                    for (int x = activeRoomAllUnitFunctionalitys.Count - 1; x >= 0; x--)
                    {
                        if (activeRoomAllUnitFunctionalitys[x].curUnitType == UnitFunctionality.UnitType.ENEMY)
                        {
                            // if no enemies are taunting, start selecting
                            selectedAmount++;
                            targetUnit(activeRoomAllUnitFunctionalitys[x]);

                            // If enough units have been selected (in order of closest)
                            if (selectedAmount == usedSkill.GetCalculatedSkillSelectionCount())
                                return;
                        }

                    }
                }
                // If the skill user is an ENEMY
                else if (GetActiveUnitFunctionality().unitData.curUnitType == UnitData.UnitType.ENEMY)
                {
                    // only select PLAYER units, in random fashion
                    for (int x = 0; x < 20; x++)
                    {
                        int rand = Random.Range(0, activeRoomAllUnitFunctionalitys.Count);

                        if (activeRoomAllUnitFunctionalitys[rand].curUnitType == UnitFunctionality.UnitType.PLAYER)
                        {
                            if (activeRoomAllUnitFunctionalitys[rand].IsSelected())
                                continue;
                            else
                                targetUnit(activeRoomAllUnitFunctionalitys[rand]);

                            selectedAmount++;

                            // If enough units have been selected, toggle the display
                            if (selectedAmount == usedSkill.GetCalculatedSkillSelectionCount())
                                return;
                        }
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
                    for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
                    {
                        if (activeRoomAllUnitFunctionalitys[i].curUnitType == UnitFunctionality.UnitType.PLAYER)
                        {
                            selectedAmount++;

                            // If self cast, cast on self, otherwise, continue for whomever
                            if (usedSkill.isSelfCast)
                                targetUnit(GetActiveUnitFunctionality());
                            else
                                targetUnit(activeRoomAllUnitFunctionalitys[i]);

                            // If enough units have been selected (in order of closest)
                            if (selectedAmount == usedSkill.GetCalculatedSkillSelectionCount())
                                return;
                        }
                    }
                }
                // If the skill user is an ENEMY
                else if (GetActiveUnitFunctionality().unitData.curUnitType == UnitData.UnitType.ENEMY)
                {
                    //Debug.Log("selecting targets");
                    // only select ENEMY units
                    for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
                    {
                        //Debug.Log("selected target " + activeRoomAllUnitFunctionalitys[i].GetUnitName());

                        if (activeRoomAllUnitFunctionalitys[i].curUnitType == UnitFunctionality.UnitType.ENEMY)
                        {
                            int rand = Random.Range(0, activeRoomAllUnitFunctionalitys.Count);

                            if (activeRoomAllUnitFunctionalitys[rand].curUnitType != UnitFunctionality.UnitType.ENEMY)
                            {
                                if (i > 0)
                                    i--;

                                continue;
                            }

                            selectedAmount++;

                            // If self cast, cast on self, otherwise, continue for whomever
                            if (usedSkill.isSelfCast)
                                targetUnit(GetActiveUnitFunctionality());
                            else
                            {
                                if (!activeRoomAllUnitFunctionalitys[rand].IsSelected())
                                    targetUnit(activeRoomAllUnitFunctionalitys[rand]);
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
                            if (selectedAmount == usedSkill.GetCalculatedSkillSelectionCount())
                                return;

                            // If skill requires only 1 unit selection - (full team target wont work without this)
                            if (usedSkill.GetCalculatedSkillSelectionCount() == 1)
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
                        }

                    }
                }
            }
        }
        
        // If skill is for dead targets
        else
        {
            for (int x = 0; x < usedSkill.skillSelectionCount; x++)
            {
                // Select dead allies
                if (usedSkill.curSkillSelectionType == SkillData.SkillSelectionType.PLAYERS)
                {
                    for (int i = 0; i < activeRoomAllies.Count; i++)
                    {
                        if (activeRoomAllies[i].isDead)
                        {
                            targetUnit(activeRoomAllies[i]);
                        }
                    }
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
            if (unitsSelected[i] != null)
            {
                unitsSelected[i].ToggleSelected(false);

                // Tell unit button that it is d 
                unitsSelected[i].selectUnitButton.unitIsSelected = false;
            }
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
        /*
        if (devMode)
        {
            if (activeSkill)
                activeSkill.skillSelectionCount = 6;
        }
        */

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
        DisableAllSkillSelections();
        UpdateActiveSkill(GetActiveUnitFunctionality().GetBaseSelectSkill());
        UpdateSkillDetails(GetActiveUnitFunctionality().GetBaseSelectSkill());

        // Update player skill portraits
        playerSkill0.UpdatePortrait(GetActiveUnitFunctionality().GetSkill(0).skillSprite);
        playerSkill1.UpdatePortrait(GetActiveUnitFunctionality().GetSkill(1).skillSprite);
        playerSkill2.UpdatePortrait(GetActiveUnitFunctionality().GetSkill(2).skillSprite);
        playerSkill3.UpdatePortrait(GetActiveUnitFunctionality().GetSkill(3).skillSprite);

        playerSkill0.ToggleSelectImage(false);
        playerSkill1.ToggleSelectImage(false);
        playerSkill2.ToggleSelectImage(false);
        playerSkill3.ToggleSelectImage(false);

        ToggleSkillVisibility(true);

        EnableFreeSkillSelection();
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
    public void targetUnit(UnitFunctionality unit)
    {
        //Debug.Log("Targeting unit " + unit.GetUnitName());

        // Ensure units cant change their selection before asd after attack
        // Allows hero rooms to still allow selection if room is defeated
        if (roomDefeated)
        {

        }
        else if (!GetAllowSelection() || !GetSelectingUnitsAllowed())
        {
            //Debug.Log("ending");
            return;
        }

        if (activeSkill)
        {
            // Ensure skills that target alive cant select dead targets
            if (unit.isDead && activeSkill.curskillSelectionAliveType == SkillData.SkillSelectionAliveType.ALIVE)
                return;
            // Ensure skills that target dead cant select alive targets
            else if (!unit.isDead && activeSkill.curskillSelectionAliveType == SkillData.SkillSelectionAliveType.DEAD)
                return;
        }

        // If current room is a shop
        if (RoomManager.Instance.GetActiveRoom().curRoomType == RoomMapIcon.RoomType.SHOP)
        {
            ToggleAllowSelection(false);
            ShopManager.Instance.shopSelectAllyPrompt.UpdateAlpha(0);

            unit.AddOwnedItems(ShopManager.Instance.GetUnassignedItem());

            if (ShopManager.Instance.GetUnassignedItem().healthItem)
            {
                float healthToRegen = (ShopManager.Instance.GetUnassignedItem().power / 100f) * unit.GetUnitMaxHealth();
                unit.StartCoroutine(unit.SpawnPowerUI(healthToRegen, false, false, null, false));
                unit.UpdateUnitCurHealth((int)healthToRegen, false, false);
            }

            ShopManager.Instance.UpdateUnAssignedItem(null);

            // If item is health item, do the effect of it
            StartCoroutine(WaitTimeThenDeselect(shopRemoveSelectTime, unit));            
        }

        if (activeSkill && !combatOver)
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
                {
                    //Debug.Log("ending 2");
                    return;
                }
                if (activeSkill.curSkillSelectionType == SkillData.SkillSelectionType.ENEMIES && unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                {
                    //Debug.Log("ending 3");
                    return;
                }
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
            //UnSelectUnit(unit);
            //UpdateUnitsSelectedText();
            // If its not a hero room, dont attack on unselecting
            if (!combatOver)
            {
                ToggleSelectingUnits(false);
                ToggleAllowSelection(false);
                PlayerAttack();
                return;
            }

            // Select targeted unit
            UnSelectUnit(unit);
            //unit.ToggleSelected(true);

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
                            if (unitsSelected.Count == GetActiveSkill().GetCalculatedSkillSelectionCount())
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
                        if (unitsSelected.Count == GetActiveSkill().GetCalculatedSkillSelectionCount())
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
                if (GetActiveSkill() != null)
                {
                    if (unitsSelected.Count == GetActiveSkill().GetCalculatedSkillSelectionCount())
                    {
                        unitsSelected[0].ToggleSelected(false);
                        unitsSelected.RemoveAt(0);
                    }
                }

                // Select targeted unit
                unitsSelected.Add(unit);
                unit.ToggleSelected(true);
                UpdateUnitsSelectedText();

                bool noEnemiesLeft = true;
                // If there are no enemies remaining AND its a hero room, AND hero room has no been offered yet
                for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
                {
                    if (activeRoomAllUnitFunctionalitys[i].curUnitType == UnitFunctionality.UnitType.ENEMY)
                    {
                        noEnemiesLeft = false;
                    }
                }

                if (noEnemiesLeft && RoomManager.Instance.GetActiveRoom().curRoomType == RoomMapIcon.RoomType.HERO
                    && !HeroRoomManager.Instance.GetPlayerOffered())
                {
                    //Debug.Log("toggling prompt");
                    StartCoroutine(ToggleHeroSelectPrompt());
                }
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

    IEnumerator ToggleHeroSelectPrompt()
    {
        yield return new WaitForSeconds(.3f);
        HeroRoomManager.Instance.TogglePrompt(true);
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

        ToggleSkillVisibility(false);

        GetActiveUnitFunctionality().TriggerTextAlert(GetActiveSkill().skillName, 1, false);

        ToggleUnitEffectTooltipsOff();

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
            UpdateUnitsSelectedText(unitsSelected.Count, activeSkill.GetCalculatedSkillSelectionCount());
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
            activeRoomEnemies[i].UpdateUnitMaxHealth(1, true, false);
        }
    }

    public void ReduceAllPlayerHealth()
    {
        if (activeRoomAllies.Count == 0)
            return;

        for (int i = 0; i < activeRoomAllies.Count; i++)
        {
            activeRoomAllies[i].UpdateUnitMaxHealth(1, true, false);
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
