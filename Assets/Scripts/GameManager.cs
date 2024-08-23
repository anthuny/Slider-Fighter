using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float unitStatBarOnAlpha = 1f;
    public float unitStatBarOffAlpha = .35f;
    public float unitStatBarSizeMax = 1f;
    public float unitStarBarSizeMin = .85f;

    public Color skillsDetailsTabColour;
    public Color itemsDetailsTabColour;
    public Color movementDetailsTabColour;

    public Color skillsDetailsTabTextColour;
    public Color itemsDetailsTabTextColour;
    public Color movementDetailsTextTabColour;

    [SerializeField] private UIElement detailsBannerStripUI;
    [SerializeField] private UIElement detailsBannerTitleText;
    [SerializeField] private UIElement detailsBannerTitleBG;
    [SerializeField] private UIElement skillsItemToggleButton;


    public GameObject fighterCarasel;
    public Canvas mainCamCanvas; 
    public Sprite humanRaceIcon;
    public Sprite beastRaceIcon;
    public Sprite etherealRaceIcon;
    public Color humanRaceColour;
    public Color beastRaceColour;
    public Color etherealRaceColour;
    public Color invisibleColour;
    public UnitData startingUnit;

    //public bool devMode;

    public bool startingFighterChosen = false;
    public UIElement transitionSequienceUI;
    public CombatUnitFocus transitionSprite;
    public UIElement nothingnessUI;
    [SerializeField] private float postBattleTime;
    public List<UnitData> activeTeam = new List<UnitData>();
    public UIElement currentRoom;
    [SerializeField] private GameObject baseUnit;
    public List<Transform> allySpawnPositions = new List<Transform>();
    [SerializeField] private Transform allyPostBattlePositionTransform;
    [SerializeField] private Transform allyTurnPositionTransform;
    [SerializeField] private Transform enemyTurnPositionTransform;
    [SerializeField] private Transform allyPositions;

    public List<UnitFunctionality> activeRoomAllUnitFunctionalitys = new List<UnitFunctionality>();
    public List<UnitFunctionality> oldActiveRoomAllUnitFunctionalitys = new List<UnitFunctionality>();
    public List<UnitFunctionality> activeRoomHeroes = new List<UnitFunctionality>();
    public List<UnitFunctionality> activeRoomHeroesBase = new List<UnitFunctionality>();
    public List<UnitFunctionality> activeRoomEnemies = new List<UnitFunctionality>();
    private List<UnitFunctionality> oldActiveRoomEnemies = new List<UnitFunctionality>();
    public List<UnitFunctionality> fallenHeroes = new List<UnitFunctionality>();
    public List<UnitFunctionality> fallenEnemies = new List<UnitFunctionality>();

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
    public int goldGainedPerUnitMin;
    public int goldGainedPerUnitMax;
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
    public int unitCombatSortingLevel = 99;
    public int unitTabSortingLevel = 250;
    public int maxUnitEffectTier = 8;
    [Header("Player UI")]
    public UIElement playerUIElement;
    public UIElement playerWeapon;
    public UIElement playerWeaponChild;
    public UIElement playerWeaponBackButton;
    public UIElement playerWeaponBG;
    public UIElement playerAbilities;
    public UIElement fighterSelectedMainSlotDesc;
    public PostBattle postBattleUI;
    public OverlayUI abilityDetailsUI;
    public Text curUnitsTargetedText;
    public Text maxUnitsTargetedText;
    public UIElement endTurnButtonUI;
    public TextMeshProUGUI activeUnitNameText;

    [Header("Player Ability UI")]
    public IconUI playerIcon;
    public IconUI fighterMainSlot1;
    public IconUI fighterMainSlot2;
    public IconUI fighterMainSlot3;
    public IconUI fighterMainSlot4;

    [Header("Skill Buttons")]
    public ButtonFunctionality mainSlot1;
    public ButtonFunctionality mainSlot2;
    public ButtonFunctionality mainSlot3;
    public ButtonFunctionality mainSlot4;
    public UIElement skill0IconUnavailableImage;
    public UIElement skill1IconUnavailableImage;
    public UIElement skill2IconUnavailableImage;
    public UIElement skill3IconUnavailableImage;
    public UIText skill0IconCooldownUIText;
    public UIText skill1IconCooldownUIText;
    public UIText skill2IconCooldownUIText;
    public UIText skill3IconCooldownUIText;
    public Color activeSkillColour;
    public Color passiveSkillColour;

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
    public ItemPiece activeItem;
    public Slot activeItemSlot;
    public TMP_ColorGradient gradientSkillAlert;
    public TMP_ColorGradient gradientLevelUpAlert;
    public Sprite hitArea1x1Sprite;
    public Sprite hitArea2x1Sprite;
    public Sprite hitArea3x1Sprite;
    public Sprite hitArea1x2Sprite;
    public Sprite hitArea1x3Sprite;
    public Sprite hitArea2x2Sprite;
    public Sprite hitArea3x2Sprite;
    public Sprite hitArea2x3Sprite;
    public Sprite hitArea3x3Sprite;

    [Header("Post Battle")]
    public GearRewards gearRewards;
    public DefeatedEnemies defeatedEnemies;

    [Header("Enemy")]
    public float enemyAttackWaitTime = 1f;
    public float enemyEffectWaitTime = 1f;
    public float enemySkillThinkTime = 1f;
    public float unitPowerUIWaitTime = .5f;
    public float allyHealthThreshholdHeal = .4f;

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

    public bool allowSelection;
    bool hasBeenLuckyHit;
    public bool selectingUnitsAllowed;
    bool firstTimeRoomStart = true;
    public WeaponManager activeWeapon;
    public int powerUISpawnCount;
    bool roomDefeated;

    public UnitData spawnedUnit;
    public UnitFunctionality spawnedUnitFunctionality;

    [HideInInspector]
    public bool playerWon;

    public bool combatOver;
    public int unitID;
    public bool playerInCombat;
    public bool isSkillsMode = true;

    public void ToggleSkillsItemToggleButton(bool toggle = true)
    {
        if (toggle)
        {
            skillsItemToggleButton.UpdateAlpha(1);
        }
        else
        {
            skillsItemToggleButton.UpdateAlpha(0);
        }
    }

    public void UpdateDetailsBanner()
    {
        detailsBannerTitleBG.AnimateUI(false);

        if (isSkillsMode)
        {
            detailsBannerTitleText.UpdateContentText("SKILLS");
            detailsBannerTitleText.UpdateContentTextColour(skillsDetailsTabTextColour);
            detailsBannerStripUI.UpdateColour(skillsDetailsTabColour);
        }
        else
        {
            detailsBannerTitleText.UpdateContentText("ITEMS");
            detailsBannerTitleText.UpdateContentTextColour(itemsDetailsTabTextColour);
            detailsBannerStripUI.UpdateColour(itemsDetailsTabColour);
        }

        if (!CombatGridManager.Instance.isCombatMode)
        {
            detailsBannerTitleText.UpdateContentText("MOVEMENT");
            detailsBannerTitleText.UpdateContentTextColour(movementDetailsTextTabColour);
            detailsBannerStripUI.UpdateColour(movementDetailsTabColour);
        }
        else
        {
            if (isSkillsMode)
            {
                detailsBannerTitleText.UpdateContentText("SKILLS");
                detailsBannerTitleText.UpdateContentTextColour(skillsDetailsTabTextColour);
                detailsBannerStripUI.UpdateColour(skillsDetailsTabColour);
            }
            else
            {
                detailsBannerTitleText.UpdateContentText("ITEMS");
                detailsBannerTitleText.UpdateContentTextColour(itemsDetailsTabTextColour);
                detailsBannerStripUI.UpdateColour(itemsDetailsTabColour);
            }
        }
    }

    public void ToggleFighterCarasel(bool toggle = true)
    {
        fighterCarasel.SetActive(toggle);
    }

    public void UpdateMainCamOrder()
    {
        mainCamCanvas.sortingOrder++;
    }
    private void Awake()
    {
        Instance = this;

        HideMainSlotDetails();

        UpdateMainCamOrder();

        //SpawnAllies(true);
    }

    public void TriggerTransitionSequence()
    {
        transitionSequienceUI.UpdateAlpha(1);
        transitionSprite.StartFocusTransition();
    }

    public void DisableTransitionSequence()
    {
        transitionSequienceUI.UpdateAlpha(0);
    }

    public void SetHeroFormation()
    {
        //Debug.Log("asd");

        /*
        List<UnitFunctionality> activeRoomHeroesTemp = new List<UnitFunctionality>();
        for (int i = 0; i < activeRoomHeroes.Count; i++)
        {
            if (activeRoomHeroes[i].teamIndex == 0)
            {
                activeRoomHeroesTemp.Add(activeRoomHeroes[i]);

                if (activeRoomHeroes[i].teamIndex == 1)
                {
                    activeRoomHeroesTemp.Add(activeRoomHeroes[i]);
                }
            }
        }

        UnitFunctionality[] array;
        */
        activeRoomHeroes.Sort(SortByIndex);
        /*
        activeRoomHeroes.Clear();

        for (int i = 0; i < activeRoomHeroesTemp.Count; i++)
        {
            activeRoomHeroes.Add(activeRoomHeroesTemp[i]);
        }


        */
        //activeRoomHeroes = activeRoomHeroesTemp;
    }

    static int SortByIndex(UnitFunctionality p1, UnitFunctionality p2)
    {
        return p1.teamIndex.CompareTo(p2.teamIndex);
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

        //map.Setup();
        //ShopManager.Instance.UpdatePlayerGold(0);
        //map.mapOverlay.ResetPlayerGoldText();
        SkillsTabChangeAlly(false, true);

        // Map music
        AudioManager.Instance.Play("MapLoop");
    }

    public void UpdateAllAlliesLevelColour()
    {
        for (int i = 0; i < activeRoomHeroes.Count; i++)
        {
            activeRoomHeroes[i].UpdateUnitLevelColour(easyDiffEnemyLvColour);
        }
    }

    public void UpdateAllEnemiesLevelColour()
    {
        int enemyLv = 0;
        float allyAverageLv = 0;

        int levelDiff = 0;

        for (int i = 0; i < activeRoomHeroes.Count; i++)
        {
            allyAverageLv += activeRoomHeroes[i].GetUnitLevel();
        }

        // Get average for ally levels
        allyAverageLv /= activeRoomHeroes.Count;


        // Update each enemy unit's level text colour
        for (int i = 0; i < activeRoomEnemies.Count; i++)
        {
            if (activeRoomEnemies[i].GetUnitLevel() == (int)allyAverageLv)
            {
                // green colour
                activeRoomEnemies[i].UpdateUnitLevelColour(easyDiffEnemyLvColour);
                continue;
            }
            else if (activeRoomEnemies[i].GetUnitLevel() == (int)allyAverageLv + 1)
            {
                // medium colour
                activeRoomEnemies[i].UpdateUnitLevelColour(mediumDiffEnemyLvColour);
                continue;
            }
            else if (activeRoomEnemies[i].GetUnitLevel() == (int)allyAverageLv + 2)
            {
                // hard colour
                activeRoomEnemies[i].UpdateUnitLevelColour(hardDiffEnemyLvColour);
                continue;
            }
            else if (activeRoomEnemies[i].GetUnitLevel() >= (int)allyAverageLv + 3)
            {
                // chaos colour
                activeRoomEnemies[i].UpdateUnitLevelColour(chaosDiffEnemyLvColour);
                continue;
            }

            /*
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
            */
        }
    }

    public WeaponManager GetActiveWeapon()
    {
        return activeWeapon;
    }

    public void SetActiveWeapon()
    {
        activeWeapon = playerWeapon.GetComponent<WeaponManager>();
    }

    public void AddUnitToTeam(UnitData unit)
    {
        activeTeam.Add(unit);
    }

    public UnitData GetUnitData(int count)
    {
        return activeTeam[count];
    }

    public UnitData GetUnitData(string name)
    {
        for (int i = 0; i < activeTeam.Count; i++)
        {
            if (activeTeam[i].unitName == name)
            {
                return activeTeam[i];
            }
        }

        return null;
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

    public void UpdateAllUnitsSorting(int sortingVal)
    {
        for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
        {
            activeRoomAllUnitFunctionalitys[i].UpdateSorting(sortingVal);
        }
    }

    public void Setup()
    {
        // Destroy previous room
        ResetRoom();

        TeamItemsManager.Instance.ResetItemsSpawned();

        RoomManager.Instance.SelectFloor();


        //TriggerTransitionSequence();

        //ToggleUIElement(turnOrder, false);

        //UpdateTurnOrder();

        ToggleUIElement(currentRoom, true);

        ToggleUIElement(playerWeapon, false);

        ToggleMap(false);
        postBattleUI.TogglePostBattleUI(false);

        StartRoom(RoomManager.Instance.GetActiveRoom(), RoomManager.Instance.GetActiveFloor());
    }

    public void SpawnFighter(bool spawnFighterAlly = false, bool byPass = false)
    {
        // Spawn player units
        if (activeRoomHeroes.Count == 0 || spawnFighterAlly)
        {
            UnitData unit = null;  // Initialize

            // Spawn ally
            for (int i = 0; i < 1; i++)
            {
                // If naturally spawning an ally, use default method
                if (!spawnFighterAlly)
                    unit = activeTeam[i];    // Reference

                GameObject go = null;
                // If spawning hero ally from end of hero room
                if (spawnFighterAlly)
                {
                    List<string> ownedUnitNames = new List<string>();

                    for (int x = 0; x < activeTeam.Count; x++)
                    {
                        ownedUnitNames.Add(activeTeam[x].unitName);
                    }

                    TeamGearManager.Instance.ResetGearTab();

                    go = Instantiate(baseUnit, HeroRoomManager.Instance.GetSpawnLocTrans());
                    go.transform.SetParent(HeroRoomManager.Instance.GetSpawnLocTrans());

                    // Loop through all allies in Characters Carasel
                    for (int t = 0; t < 25; t++)
                    {
                        //Debug.Log(CharacterCarasel.Instance.GetAllAllies()[t]);

                        int randIndex = Random.Range(0, CharacterCarasel.Instance.GetAllAllies().Count);


                        if (ownedUnitNames.Contains(CharacterCarasel.Instance.GetAllAllies()[randIndex].unitName))
                        {
                            if (t > 0 && ownedUnitNames.Count < CharacterCarasel.Instance.GetAllAllies().Count)
                                t--;
                        }
                        else
                        {
                            unit = CharacterCarasel.Instance.GetAlly(randIndex);    // Reference a unit that is not on the current ally team

                            UnitFunctionality unitFunct = go.GetComponent<UnitFunctionality>();

                            unitFunct.UpdateUnitName(unit.unitName);

                            unitFunct.ToggleUnitMoveActiveArrows(false);

                            if (unit.curRaceType == UnitData.RaceType.HUMAN)
                                unitFunct.curUnitRace = UnitFunctionality.UnitRace.HUMAN;
                            else if (unit.curRaceType == UnitData.RaceType.BEAST)
                                unitFunct.curUnitRace = UnitFunctionality.UnitRace.BEAST;
                            else if (unit.curRaceType == UnitData.RaceType.ETHEREAL)
                                unitFunct.curUnitRace = UnitFunctionality.UnitRace.ETHEREAL;

                            unitFunct.UpdateUnitSkills(unit.GetUnitSkills());
                            unitFunct.UpdateCurrentSkills(unit.GetUnitSkills());

                            unitFunct.ResetSkill0Cooldown();
                            unitFunct.ResetSkill1Cooldown();
                            unitFunct.ResetSkill2Cooldown();
                            unitFunct.ResetSkill3Cooldown();

                            int spawnedUnitLevel = GetAllyLevelAverage() - Random.Range(0, 2);

                            //spawnedUnitLevel -= (Random.Range(2,4) * RoomManager.Instance.GetFloorCount()) + (2 * RoomManager.Instance.GetFloorCount());
                            //Debug.Log("spawnedUnitLevel = " + spawnedUnitLevel);
                            //unitFunct.UpdateUnitLevel(spawnedUnitLevel, 0, true);
                            float newExpStarting = (maxExpStarting * spawnedUnitLevel) + Random.Range(0, maxExpStarting/2f);

                            unitFunct.UpdateUnitExp((int)newExpStarting);

                            //Debug.Log("unit max xp " + maxExpStarting);
                            
                            SkillsTabManager.Instance.ResetAllySkllls(unitFunct);

                            unitFunct.UpdateFighterPosition();
                            break;
                        }
                    }

                    if (unit == null && !byPass)
                    {
                        Debug.Log("No ally found that is not already on team!");
                        StartCoroutine(SetupPostBattleUI(playerWon));
                        return;
                    }



                    ItemRewardManager.Instance.ResetRewardsTable();
                }

                if (!spawnFighterAlly)
                {
                    go = Instantiate(baseUnit, CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).transform);
                    go.transform.SetParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).transform);
                }

                UnitFunctionality unitFunctionality = go.GetComponent<UnitFunctionality>();

                unitFunctionality.UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0));

                UIElement unitUI = go.GetComponent<UIElement>();

                HeroRoomManager.Instance.UpdateHero(unitUI);        

                // Set ally correct position based on team size
                if (!spawnFighterAlly)
                {
                    if (i == 0)
                    {
                        unitFunctionality.SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).transform);
                        unitFunctionality.UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0));
                        CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).UpdateLinkedUnit(unitFunctionality);
                    }
                    else if (i == 1)
                    {
                        unitFunctionality.SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(1).transform);
                        unitFunctionality.UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(1));
                        CombatGridManager.Instance.GetFighterSpawnCombatSlot(1).UpdateLinkedUnit(unitFunctionality);
                    }

                    else if (i == 2)
                    {
                        unitFunctionality.SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(2).transform);
                        unitFunctionality.UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(2));
                        CombatGridManager.Instance.GetFighterSpawnCombatSlot(2).UpdateLinkedUnit(unitFunctionality);
                    }
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

                if (unit.curRaceType == UnitData.RaceType.HUMAN)
                {
                    unitFunctionality.curUnitRace = UnitFunctionality.UnitRace.HUMAN;
                    unitFunctionality.UpdateFighterRaceIcon("HUMAN");
                }                
                else if (unit.curRaceType == UnitData.RaceType.BEAST)
                {
                    unitFunctionality.curUnitRace = UnitFunctionality.UnitRace.BEAST;
                    unitFunctionality.UpdateFighterRaceIcon("BEAST");
                }                   
                else if (unit.curRaceType == UnitData.RaceType.ETHEREAL)
                {
                    unitFunctionality.curUnitRace = UnitFunctionality.UnitRace.ETHEREAL;
                    unitFunctionality.UpdateFighterRaceIcon("ETHEREAL");
                }

                unitFunctionality.ToggleFighterRaceIcon(true);

                unitFunctionality.UpdateSpeedIncPerLv((int)unit.speedIncPerLv);
                unitFunctionality.UpdatePowerIncPerLv((int)unit.powerIncPerLv);
                unitFunctionality.UpdateHealingPowerIncPerLv((int)unit.healingPowerIncPerLv);
                unitFunctionality.UpdateDefenseIncPerLv(unit.defenseIncPerLv);
                unitFunctionality.UpdateMaxHealthIncPerLv((int)unit.maxHealthIncPerLv);
                unitFunctionality.UpdateStartingMaxHealth(unit.startingMaxHealth);

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

                unitFunctionality.UpdateFighterPosition();

                AddActiveRoomAllUnitsFunctionality(unitFunctionality, false);




                unitFunctionality.UpdateUnitLevel(1);
                UpdateAllAlliesLevelColour();


                //SkillsTabManager.Instance.SetupSkillsTab(unitFunctionality);

                if (spawnFighterAlly)
                    ToggleAllowSelection(true);

                unitFunctionality.ToggleUnitHealthBar(true);

                //unitFunctionality.ToggleUnitHealthBar(false);

                unitFunctionality.unitData = unit;


                if (byPass)
                {
                    if (activeRoomHeroes.Count == 0)
                    {
                        unitFunctionality.SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).transform);
                        unitFunctionality.UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0));
                        CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).UpdateLinkedUnit(unitFunctionality);
                    }
                    else if (activeRoomHeroes.Count == 1)
                    {
                        unitFunctionality.SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(1).transform);
                        unitFunctionality.UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(1));
                        CombatGridManager.Instance.GetFighterSpawnCombatSlot(1).UpdateLinkedUnit(unitFunctionality);
                    }
  
                    if (activeRoomHeroes.Count == 2)
                    {
                        unitFunctionality.SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(2).transform);
                        unitFunctionality.UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(2));
                        CombatGridManager.Instance.GetFighterSpawnCombatSlot(2).UpdateLinkedUnit(unitFunctionality);
                    }

                    unitFunctionality.heroRoomUnit = true;

                    spawnedUnit = unit;
                    spawnedUnitFunctionality = unitFunctionality;

                    AddUnitToTeam(unitFunctionality.unitData);
                }

                if (i == 0 && !spawnFighterAlly)
                    SkillsTabManager.Instance.UpdateActiveUnit(GetActiveAlly());

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

                //SkillsTabChangeAlly(true);
                //SkillsTabChangeAlly(false, true);
                
                if (spawnFighterAlly)
                {
                    unitFunctionality.ToggleUnitBottomStats(false);
                }
                
                
                // If spawn hero ally from hero room, only spawn 1 ally
                if (spawnFighterAlly)
                    break;
            }


        }     
    }


    public void UpdateActiveAllyDisplay()
    {
        /*
if (activeRoomAllUnitFunctionalitys.Count > 1)
{
UnitFunctionality unit = activeRoomAllUnitFunctionalitys[0];
activeRoomAllUnitFunctionalitys.RemoveAt(0);
activeRoomAllUnitFunctionalitys.Add(unit);

for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
{
    if ( i == 0)
        activeRoomAllUnitFunctionalitys[i].UpdateIsVisible(true);
    else if (i != 0)
        activeRoomAllUnitFunctionalitys[i].UpdateIsVisible(false);
}

activeRoomAllUnitFunctionalitys[0].transform.position = allyPositions.GetChild(0).transform.position;
}




for (int i = 0; i < activeRoomAllies.Count; i++)
{

if (i == 0)
{
    //Debug.Log("bb");
    if (activeRoomAllUnitFunctionalitys.Count >= 3)
    {
        UnitFunctionality oldUnit = activeRoomAllUnitFunctionalitys[2];
        activeRoomAllUnitFunctionalitys[0] = activeRoomAllUnitFunctionalitys[2];
        activeRoomAllUnitFunctionalitys[0] = oldUnit;
    }
    else
        activeRoomAllUnitFunctionalitys[i].UpdateIsVisible(true);
}
if (i == 1)
{
    UnitFunctionality oldUnit = activeRoomAllUnitFunctionalitys[0];
    activeRoomAllUnitFunctionalitys[0] = activeRoomAllUnitFunctionalitys[1];
    activeRoomAllUnitFunctionalitys[1] = oldUnit;

    // Update position 1 to be visiible, rest not
    activeRoomAllUnitFunctionalitys[i].UpdateIsVisible(false);
}
else if (i == 2)
{
    UnitFunctionality oldUnit = activeRoomAllUnitFunctionalitys[1];
    activeRoomAllUnitFunctionalitys[1] = activeRoomAllUnitFunctionalitys[2];
    activeRoomAllUnitFunctionalitys[2] = oldUnit;

    // Update position 1 to be visiible, rest not
    activeRoomAllUnitFunctionalitys[i].UpdateIsVisible(false);
}

activeRoomAllUnitFunctionalitys[0].transform.position = allyPositions.GetChild(0).transform.position;
}
*/
    }

    public void UpdateAllysPositionCombat()
    {
        if (activeRoomHeroes.Count == 1)
        {
            activeRoomHeroes[0].SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).transform);
            activeRoomHeroes[0].UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0));

            CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).UpdateLinkedUnit(activeRoomHeroes[0]);
        }

        else if (activeRoomHeroes.Count == 2)
        {
            activeRoomHeroes[0].SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).transform);
            activeRoomHeroes[1].SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(1).transform);

            activeRoomHeroes[0].UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0));
            activeRoomHeroes[1].UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(1));

            CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).UpdateLinkedUnit(activeRoomHeroes[0]);
            CombatGridManager.Instance.GetFighterSpawnCombatSlot(1).UpdateLinkedUnit(activeRoomHeroes[1]);
        }
        else if (activeRoomHeroes.Count == 3)
        {
            activeRoomHeroes[0].SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).transform);
            activeRoomHeroes[1].SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(1).transform);
            activeRoomHeroes[2].SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(2).transform);

            activeRoomHeroes[0].UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0));
            activeRoomHeroes[1].UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(1));
            activeRoomHeroes[2].UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(2));

            CombatGridManager.Instance.GetFighterSpawnCombatSlot(1).UpdateLinkedUnit(activeRoomHeroes[1]);
            CombatGridManager.Instance.GetFighterSpawnCombatSlot(2).UpdateLinkedUnit(activeRoomHeroes[2]);
        }
    }
    public void SkillsTabChangeAlly(bool toggle, bool byPass = false, bool teamPage = false, bool actuallyDoIt = false)
    {
        SkillsTabManager.Instance.statScrollView.SetActive(toggle);

        if (toggle)
        {
            if (actuallyDoIt)
            {
                // Shuffle active allies position
                UnitFunctionality unit = activeRoomHeroes[0];
                activeRoomHeroes.RemoveAt(0);
                activeRoomHeroes.Add(unit);

                SkillsTabManager.Instance.ToggleSelectedSlotDetailsButton(false);
                //Debug.Log("asdasd");
            }

            // Update which ally is active
            //UpdateActiveAllyDisplay();

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

            if (GetActiveAlly())
                SkillsTabManager.Instance.UpdateActiveUnit(GetActiveAlly());

            oldActiveRoomAllUnitFunctionalitys = activeRoomAllUnitFunctionalitys;
            
            SkillsTabManager.Instance.UpdateAllyNameText();

            SkillsTabManager.Instance.UpdateStatPage();

            UpdateAllyVisibility(true, teamPage);
        }
        else
        {
            SkillsTabManager.Instance.gearTabArrowLeftButton.ToggleButton(false);
            SkillsTabManager.Instance.gearTabArrowRightButton.ToggleButton(false);
            SkillsTabManager.Instance.ToggleToMapButton(false);
            skillsTab.UpdateAlpha(0);

            UpdateAllAlliesPosition(false, true, false);
            UpdateAllysPositionCombat();

            if (byPass)
            {

                UpdateAllyVisibility(false, false);
            }
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
                for (int i = 0; i < activeRoomHeroes.Count; i++)
                {
                    activeRoomHeroes[0].UpdateIsVisible(true);
                    activeRoomHeroes[0].ToggleUnitHealthBar(false);
                    activeRoomHeroes[0].ToggleUnitAttackBar(false);

                    activeRoomHeroes[0].gameObject.transform.position = allyPositions.GetChild(0).transform.position;

                    //activeRoomAllies[i].UpdateFacingDirection(true);

                    if (i != 0)
                    {
                        //Debug.Log("aa");
                        activeRoomHeroes[i].UpdateIsVisible(false);

                    }
                }

                // Ensure only 
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
            for (int i = 0; i < activeRoomHeroes.Count; i++)
            {
                activeRoomHeroes.RemoveAt(i);
            }

            activeRoomHeroes.Clear();

            if (activeRoomHeroes.Count == 0)
                activeTeam.Clear();

            activeRoomAllUnitFunctionalitys.Clear();
        }

        if (enemies)
        {
            for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
            {
                if (activeRoomAllUnitFunctionalitys[i].curUnitType == UnitFunctionality.UnitType.ENEMY)
                    activeRoomAllUnitFunctionalitys.RemoveAt(i);
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
        }
        // Turn map ON
        else
        {
            playerInCombat = false;

            for (int i = 0; i < activeRoomHeroes.Count; i++)
            {
                activeRoomHeroes[i].ResetPowerUI(true);
            }

            // Update unit sorting
            UpdateAllUnitsSorting(unitTabSortingLevel);

            HeroRoomManager.Instance.TogglePlayedOffered(false);

            if (!increaseFloor)
                ResetAlliesExpVisual();

            // Play Map music
            if (playMapLoop)
                AudioManager.Instance.PauseMapMusic(false);

            map.mapOverlay.ToggleBottomBG(true);
            map.gameObject.SetActive(true);

            map.ToggleMapVisibility(true, generateMap, increaseFloor);

            ToggleMainSlotVisibility(true);

            MapManager.Instance.UpdateEnterRoomButton();

            transitionSprite.resetMap = false;
            transitionSprite.AllowFadeOut();

        }
    }

    public UnitFunctionality GetLowestHealthUnit(bool ally = true, bool ignoreTeam = false)
    {
        int lowestHealth = 100;
        UnitFunctionality lowestHealthUnit = null;

        for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
        {
            if (ignoreTeam)
            {
                if (lowestHealth > ((activeRoomAllUnitFunctionalitys[i].GetUnitCurHealth() / activeRoomAllUnitFunctionalitys[i].GetUnitMaxHealth()) * 100))
                {
                    lowestHealth = (int)(activeRoomAllUnitFunctionalitys[i].GetUnitCurHealth() / activeRoomAllUnitFunctionalitys[i].GetUnitMaxHealth() * 100);
                    lowestHealthUnit = activeRoomAllUnitFunctionalitys[i];
                }
            }
            else
            {
                if (ally)
                {
                    if (activeRoomAllUnitFunctionalitys[i].curUnitType == UnitFunctionality.UnitType.PLAYER)
                    {
                        if (lowestHealth > ((activeRoomAllUnitFunctionalitys[i].GetUnitCurHealth() / activeRoomAllUnitFunctionalitys[i].GetUnitMaxHealth()) * 100))
                        {
                            lowestHealth = (int)(activeRoomAllUnitFunctionalitys[i].GetUnitCurHealth() / activeRoomAllUnitFunctionalitys[i].GetUnitMaxHealth() * 100);
                            lowestHealthUnit = activeRoomAllUnitFunctionalitys[i];
                        }
                    }
                }
                else
                {
                    if (activeRoomAllUnitFunctionalitys[i].curUnitType == UnitFunctionality.UnitType.ENEMY)
                    {
                        if (lowestHealth > ((activeRoomAllUnitFunctionalitys[i].GetUnitCurHealth() / activeRoomAllUnitFunctionalitys[i].GetUnitMaxHealth()) * 100))
                        {
                            lowestHealth = (int)(activeRoomAllUnitFunctionalitys[i].GetUnitCurHealth() / activeRoomAllUnitFunctionalitys[i].GetUnitMaxHealth() * 100);
                            lowestHealthUnit = activeRoomAllUnitFunctionalitys[i];
                        }
                    }
                }
            }
        }

        if (lowestHealthUnit != null)
            return lowestHealthUnit;
        else
            return null;
    }

    public void UpdateAllAlliesPosition(bool postBattle, bool playersTurn = true, bool skillsTab = false, bool shopPosition = false)
    {
        //Debug.Log('a');
        if (shopPosition)
        {
            allyPositions.SetParent(ShopManager.Instance.unitsPositionShopTrans);
            allyPositions.SetPositionAndRotation(new Vector2(0, 0), Quaternion.identity);
            allyPositions.position = ShopManager.Instance.unitsPositionShopTrans.position;

            if (activeRoomHeroes.Count == 1)
            {
                activeRoomHeroes[0].SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).transform);
                activeRoomHeroes[0].UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0));
                CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).UpdateLinkedUnit(activeRoomHeroes[0]);
            }

            else if (activeRoomHeroes.Count == 2)
            {
                activeRoomHeroes[0].SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).transform);
                activeRoomHeroes[1].SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(1).transform);

                activeRoomHeroes[0].UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0));
                activeRoomHeroes[1].UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(1));

                CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).UpdateLinkedUnit(activeRoomHeroes[0]);
                CombatGridManager.Instance.GetFighterSpawnCombatSlot(1).UpdateLinkedUnit(activeRoomHeroes[1]);
            }
            else if (activeRoomHeroes.Count == 3)
            {
                activeRoomHeroes[0].SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).transform);
                activeRoomHeroes[1].SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(1).transform);
                activeRoomHeroes[2].SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(2).transform);

                activeRoomHeroes[0].UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0));
                activeRoomHeroes[1].UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(1));
                activeRoomHeroes[2].UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(2));

                CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).UpdateLinkedUnit(activeRoomHeroes[0]);
                CombatGridManager.Instance.GetFighterSpawnCombatSlot(1).UpdateLinkedUnit(activeRoomHeroes[1]);
                CombatGridManager.Instance.GetFighterSpawnCombatSlot(2).UpdateLinkedUnit(activeRoomHeroes[2]);
            }
            return;
        }
        if (!postBattle)
        {
            if (skillsTab)
            {
                allyPositions.SetParent(SkillsTabManager.Instance.statAllyPosTrans);
                allyPositions.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
                allyPositions.position = SkillsTabManager.Instance.statAllyPosTrans.position;

                if (activeRoomHeroes.Count == 1)
                {
                    activeRoomHeroes[0].SetPositionAndParent(allySpawnPositions[0]);
                    //activeRoomAllies[0].Visual
                }
                else if (activeRoomHeroes.Count == 2)
                {
                    activeRoomHeroes[0].SetPositionAndParent(allySpawnPositions[0]);
                    activeRoomHeroes[1].SetPositionAndParent(allySpawnPositions[0]);
                }
                else if (activeRoomHeroes.Count == 3)
                {
                    activeRoomHeroes[0].SetPositionAndParent(allySpawnPositions[0]);
                    activeRoomHeroes[1].SetPositionAndParent(allySpawnPositions[0]);
                    activeRoomHeroes[2].SetPositionAndParent(allySpawnPositions[0]);
                }

                //Debug.Log("4");
                return;
            }
            else
            {
                if (activeRoomHeroes.Count == 1)
                {
                    activeRoomHeroes[0].SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).transform);
                    activeRoomHeroes[0].UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0));

                    CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).UpdateLinkedUnit(activeRoomHeroes[0]);
                }
                else if (activeRoomHeroes.Count == 2)
                {
                    activeRoomHeroes[0].SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).transform);
                    activeRoomHeroes[1].SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(1).transform);

                    activeRoomHeroes[0].UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0));
                    activeRoomHeroes[1].UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(1));

                    CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).UpdateLinkedUnit(activeRoomHeroes[0]);
                    CombatGridManager.Instance.GetFighterSpawnCombatSlot(1).UpdateLinkedUnit(activeRoomHeroes[1]);
                }
                else if (activeRoomHeroes.Count == 3)
                {
                    activeRoomHeroes[0].SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).transform);
                    activeRoomHeroes[1].SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(1).transform);
                    activeRoomHeroes[2].SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(2).transform);

                    activeRoomHeroes[0].UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0));
                    activeRoomHeroes[1].UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(1));
                    activeRoomHeroes[2].UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(2));

                    CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).UpdateLinkedUnit(activeRoomHeroes[0]);
                    CombatGridManager.Instance.GetFighterSpawnCombatSlot(1).UpdateLinkedUnit(activeRoomHeroes[1]);
                    CombatGridManager.Instance.GetFighterSpawnCombatSlot(2).UpdateLinkedUnit(activeRoomHeroes[2]);
                }

                //Debug.Log("3");
                allyPositions.SetParent(allyTurnPositionTransform);
                allyPositions.SetPositionAndRotation(new Vector2(0, 0), Quaternion.identity);
                allyPositions.position = allyTurnPositionTransform.position;
            }

            if (playersTurn)
            {
                //Debug.Log("2");
                allyPositions.SetParent(allyTurnPositionTransform);
                allyPositions.SetPositionAndRotation(new Vector2(0, 0), Quaternion.identity);
                allyPositions.position = allyTurnPositionTransform.position;

                if (activeRoomHeroes.Count == 1)
                {
                    activeRoomHeroes[0].SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).transform);
                    activeRoomHeroes[0].UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0));

                    CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).UpdateLinkedUnit(activeRoomHeroes[0]);
                }
                else if (activeRoomHeroes.Count == 2)
                {
                    activeRoomHeroes[0].SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).transform);
                    activeRoomHeroes[1].SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(1).transform);

                    activeRoomHeroes[0].UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0));
                    activeRoomHeroes[1].UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(1));

                    CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).UpdateLinkedUnit(activeRoomHeroes[0]);
                    CombatGridManager.Instance.GetFighterSpawnCombatSlot(1).UpdateLinkedUnit(activeRoomHeroes[1]);
                }
                else if (activeRoomHeroes.Count == 3)
                {
                    activeRoomHeroes[0].SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).transform);
                    activeRoomHeroes[1].SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(1).transform);
                    activeRoomHeroes[2].SetPositionAndParent(CombatGridManager.Instance.GetFighterSpawnCombatSlot(2).transform);

                    activeRoomHeroes[0].UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(0));
                    activeRoomHeroes[1].UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(1));
                    activeRoomHeroes[2].UpdateActiveCombatSlot(CombatGridManager.Instance.GetFighterSpawnCombatSlot(2));

                    CombatGridManager.Instance.GetFighterSpawnCombatSlot(0).UpdateLinkedUnit(activeRoomHeroes[0]);
                    CombatGridManager.Instance.GetFighterSpawnCombatSlot(1).UpdateLinkedUnit(activeRoomHeroes[1]);
                    CombatGridManager.Instance.GetFighterSpawnCombatSlot(2).UpdateLinkedUnit(activeRoomHeroes[2]);
                }
            }
            else
            {
                //Debug.Log("1");
                allyPositions.SetParent(enemyTurnPositionTransform);
                allyPositions.SetPositionAndRotation(new Vector2(0, 0), Quaternion.identity);
                allyPositions.position = enemyTurnPositionTransform.position;
            }
        }
        else
        {
            //Debug.Log("b");
            allyPositions.SetParent(allyPostBattlePositionTransform);
            allyPositions.SetPositionAndRotation(new Vector2(0, 0), Quaternion.identity);
            allyPositions.position = allyPostBattlePositionTransform.position;

            if (activeRoomHeroes.Count == 1)
            {
                activeRoomHeroes[0].SetPositionAndParent(allyPositions.GetChild(1));
            }
            else if (activeRoomHeroes.Count == 2)
            {
                activeRoomHeroes[0].SetPositionAndParent(allyPositions.GetChild(1));
                activeRoomHeroes[1].SetPositionAndParent(allyPositions.GetChild(0));
            }
            else if (activeRoomHeroes.Count == 3)
            {
                activeRoomHeroes[0].SetPositionAndParent(allyPositions.GetChild(1));
                activeRoomHeroes[1].SetPositionAndParent(allyPositions.GetChild(0));
                activeRoomHeroes[2].SetPositionAndParent(allyPositions.GetChild(2));
            }
        }
    }

    public void SetupItemRewards()
    {
        ResetSelectedUnits();

        combatOver = true;

        // Toggle player overlay and skill ui off
        ToggleUIElement(playerAbilities, false);
        ToggleUIElement(fighterSelectedMainSlotDesc, false);
        ToggleUIElement(endTurnButtonUI, false);
        ToggleUIElement(turnOrder, false);
        ResetActiveUnitTurnArrow();
        ToggleAllAlliesStatBar(false);

        // Remove all unit effects and level image for item rewards
        for (int i = 0; i < activeRoomHeroes.Count; i++)
        {
            activeRoomHeroes[i].ResetEffects();
            activeRoomHeroes[i].ToggleUnitLevelImage(false);
        }

        ToggleEnemyUnitSelection(false);

        StartCoroutine(SetupItemRewardsCo());
    }
    public void ToggleAllyUnitSelection(bool toggle)
    {
        for (int i = 0; i < activeRoomHeroes.Count; i++)
        {
            if (toggle)
            {
                if (ShopManager.Instance.playerInShopRoom)
                {
                    if (ShopManager.Instance.GetSelectedShopItem().curRaceSpecific == ShopItem.RaceSpecific.HUMAN
                    && activeRoomHeroes[i].curUnitRace == UnitFunctionality.UnitRace.HUMAN)
                    {
                        activeRoomHeroes[i].ToggleSelectUnitButton(toggle);
                        activeRoomHeroes[i].ToggleSelected(true);
                    }
                    else if (ShopManager.Instance.GetSelectedShopItem().curRaceSpecific == ShopItem.RaceSpecific.BEAST
                    && activeRoomHeroes[i].curUnitRace == UnitFunctionality.UnitRace.BEAST)
                    {
                        activeRoomHeroes[i].ToggleSelectUnitButton(toggle);
                        activeRoomHeroes[i].ToggleSelected(true);
                    }
                    else if (ShopManager.Instance.GetSelectedShopItem().curRaceSpecific == ShopItem.RaceSpecific.ETHEREAL
                    && activeRoomHeroes[i].curUnitRace == UnitFunctionality.UnitRace.ETHEREAL)
                    {
                        activeRoomHeroes[i].ToggleSelectUnitButton(toggle);
                        activeRoomHeroes[i].ToggleSelected(true);
                    }
                    else if (ShopManager.Instance.GetSelectedShopItem().curRaceSpecific == ShopItem.RaceSpecific.ALL)
                    {
                        activeRoomHeroes[i].ToggleSelectUnitButton(toggle);
                        activeRoomHeroes[i].ToggleSelected(true);
                    }
                    else if (ShopManager.Instance.GetSelectedShopItem().curRaceSpecific == ShopItem.RaceSpecific.HUMAN
                    && activeRoomHeroes[i].curUnitRace != UnitFunctionality.UnitRace.HUMAN)
                    {
                        activeRoomHeroes[i].ToggleSelectUnitButton(false);
                        activeRoomHeroes[i].ToggleSelected(false);
                    }
                    else if (ShopManager.Instance.GetSelectedShopItem().curRaceSpecific == ShopItem.RaceSpecific.BEAST
                    && activeRoomHeroes[i].curUnitRace != UnitFunctionality.UnitRace.BEAST)
                    {
                        activeRoomHeroes[i].ToggleSelectUnitButton(false);
                        activeRoomHeroes[i].ToggleSelected(false);
                    }
                    else if (ShopManager.Instance.GetSelectedShopItem().curRaceSpecific == ShopItem.RaceSpecific.ETHEREAL
                    && activeRoomHeroes[i].curUnitRace != UnitFunctionality.UnitRace.ETHEREAL)
                    {
                        activeRoomHeroes[i].ToggleSelectUnitButton(false);
                        activeRoomHeroes[i].ToggleSelected(false);
                    }
                    else
                    {
                        activeRoomHeroes[i].ToggleSelectUnitButton(false);
                        activeRoomHeroes[i].ToggleSelected(false);
                    }
                }
                else
                {
                    activeRoomHeroes[i].ToggleSelectUnitButton(true);
                    activeRoomHeroes[i].ToggleSelected(true);
                }

            }
            else
            {
                activeRoomHeroes[i].ToggleSelectUnitButton(false);
                activeRoomHeroes[i].ToggleSelected(false);
            }
        }
    }
    public void ToggleEnemyUnitSelection(bool toggle)
    {
        for (int i = 0; i < activeRoomEnemies.Count; i++)
        {
            activeRoomEnemies[i].ToggleSelectUnitButton(toggle);
        }
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

    public void EnsureHeroIsDead()
    {
        OwnedLootInven.Instance.MoveGearAndItemsToOwnedLoot();

        OwnedLootInven.Instance.UpdateWornLootOwning();

        // Remove dead allies from team when post battle starts, on a win
        for (int i = 0; i < fallenHeroes.Count; i++)
        {
            if (fallenHeroes[i].isDead)
            {
                //Debug.Log("Destroying hero " + fallenHeroes[i].GetUnitName());
                RemoveUnit(fallenHeroes[i]);

                if (activeRoomHeroes.Contains(fallenHeroes[i]))
                { 
                    activeRoomHeroes.Remove(fallenHeroes[i]);
                } 
            }
        }
    }
    #region Setup Multiple UIs
    public IEnumerator SetupPostBattleUI(bool playerWon)
    {
        if (!roomDefeated)
        {
            roomDefeated = true;
            combatOver = true;

            if (!playerWon)
            {
                activeRoomAllUnitFunctionalitys.Clear();
                CombatGridManager.Instance.ToggleButtonAttackMovement(false);
            }
            else
            {
                EnsureHeroIsDead();
                StartCoroutine(PostBattle.Instance.ToggleButtonPostBattleMap(false));
            }

            for (int i = 0; i < activeRoomHeroes.Count; i++)
            {
                if (!activeRoomHeroes[i].heroRoomUnit)
                {
                    activeRoomHeroes[i].ResetEffects();
                    activeRoomHeroes[i].curPowerHits = activeRoomHeroes[i].powerHitsRoomStarting;


                    activeRoomHeroes[i].UpdateUnitMaxHealth(activeRoomHeroes[i].startingRoomMaxHealth, true, false);
                    activeRoomHeroes[i].curSpeed = activeRoomHeroes[i].startingRoomSpeed;
                    activeRoomHeroes[i].curDefense = activeRoomHeroes[i].startingRoomDefense;
                    activeRoomHeroes[i].ResetUnitHealingRecieved();

                    activeRoomHeroes[i].ToggleUnitMoveActiveArrows(false);
                }
            }

            for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
            {
                activeRoomAllUnitFunctionalitys[i].ToggleUnitMoveActiveArrows(false);
            }

            ToggleEndTurnButton(false);
            CombatGridManager.Instance.DisableAllButtons();

            // Re-order activeroomallunitfunctionality order to default team order after a combat win
            if (playerWon)
            {
                CheckToRemoveItems();

                if (activeRoomAllUnitFunctionalitys.Count == 3)
                {
                    for (int x = 0; x < activeRoomHeroes.Count; x++)
                    {
                        //activeRoomHeroes[x].ReloadItemUses();

                        if (!activeRoomHeroes[x].isDead)
                        {
                            if (activeRoomHeroes[x].teamIndex == 0)
                            {
                                activeRoomAllUnitFunctionalitys[0] = activeRoomHeroes[0];
                            }
                            else if (activeRoomHeroes[x].teamIndex == 1)
                            {
                                activeRoomAllUnitFunctionalitys[1] = activeRoomHeroes[1];
                            }
                            else if (activeRoomHeroes[x].teamIndex == 2)
                            {
                                activeRoomAllUnitFunctionalitys[2] = activeRoomHeroes[2];
                            }
                        }
                    }
                }
                else if (activeRoomAllUnitFunctionalitys.Count == 2)
                {
                    for (int x = 0; x < activeRoomHeroes.Count; x++)
                    {
                        if (!activeRoomHeroes[x].isDead)
                        {
                            if (activeRoomHeroes[x].teamIndex == 0)
                            {
                                activeRoomAllUnitFunctionalitys[0] = activeRoomHeroes[0];
                            }
                            else if (activeRoomHeroes[x].teamIndex == 1)
                            {
                                activeRoomAllUnitFunctionalitys[1] = activeRoomHeroes[1];
                            }
                        }
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

            //roomDefeated = true;

            UpdateActiveSkill(null);

            //if (RoomManager.Instance.GetActiveRoom().curRoomType != RoomMapIcon.RoomType.HERO)
            //    StartCoroutine(SetupRoomPostBattle(playerWon));

            //Debug.Log("-a");
            UpdateAllAlliesPosition(true);


            // If completed room WAS NOT a hero room
            if (RoomManager.Instance.GetActiveRoom().curRoomType != RoomMapIcon.RoomType.HERO || !playerWon)
            {
                ToggleMainSlotVisibility(false);

                RoomManager.Instance.ToggleInteractable(false);

                // Enable unit level image for post combat
                for (int i = 0; i < activeRoomHeroes.Count; i++)
                {
                    activeRoomHeroes[i].ToggleUnitLevelImage(true);
                }
            }
            // If completed room WAS a hero room
            else
            {
                ToggleMainSlotVisibility(false);

                // If player has 3 allies already, do not offer a 4th, go to post battle (TEMP SOLUTION) TODO: Make prompt to swap
                if (activeTeam.Count < 3)
                {
                    //StartCoroutine(HeroRetrievalScene());
                }
                else
                {
                    ToggleMainSlotVisibility(false);

                    RoomManager.Instance.ToggleInteractable(false);

                    // Enable unit level image for post combat
                    for (int i = 0; i < activeRoomHeroes.Count; i++)
                    {
                        activeRoomHeroes[i].ToggleUnitLevelImage(true);
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
    }

    public IEnumerator HeroRetrievalScene(bool togglePromp = true)
    {
        ItemRewardManager.Instance.ToggleItemRewards(false);
        ItemRewardManager.Instance.ToggleConfirmItemButton(false);

        yield return new WaitForSeconds(postFightTimeWait);

        //RoomManager.Instance.ToggleInteractable(true);
        ToggleAllowSelection(true);
        ToggleSelectingUnits(true);

        // Toggle player overlay and skill ui off
        ToggleUIElement(playerAbilities, false);
        ToggleUIElement(fighterSelectedMainSlotDesc, false);
        ToggleUIElement(endTurnButtonUI, false);
        ToggleUIElement(turnOrder, false);
        ResetActiveUnitTurnArrow();

        if (togglePromp)
            HeroRoomManager.Instance.TogglePrompt(false, true, false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            SpawnFighter(true, true);

        if (Input.GetKeyDown(KeyCode.Y))
            TriggerTransitionSequence();
    }

    public void HideMainSlotDetails()
    {
        // Toggle player overlay and skill ui off
        ToggleUIElement(playerAbilities, false);
        ToggleUIElement(fighterSelectedMainSlotDesc, false);
        ToggleUIElement(endTurnButtonUI, false);
        ToggleUIElement(turnOrder, false);
        abilityDetailsUI.ToggleFighterDetailsTab(false);

        ToggleAllAlliesStatBar(true);
    }
    // Toggle UI accordingly
    public IEnumerator SetupRoomPostBattle(bool playerWon)
    {
        CombatGridManager.Instance.DisableAllButtons();
        //StartCoroutine(SetupPostBattleUI(playerWon));

        // Remove ally unit's effects

        for (int i = 0; i < activeRoomHeroes.Count; i++)
        {
            //activeRoomAllies[i].ResetEffects();
            activeRoomHeroes[i].ToggleSelected(false);
        }

        ToggleAllowSelection(false);
        ToggleSelectingUnits(false);
        //ResetSelectedUnits();

        HideMainSlotDetails();

        ResetActiveUnitTurnArrow();

        // Toggle post battle ui on
        postBattleUI.TogglePostBattleUI(true);

        yield return new WaitForSeconds(.3f);

        if (playerWon)
        {
            defeatedEnemies.DisplayDefeatedEnemies();
        }
        else
            ResetRoom(false);

        yield return new WaitForSeconds(1f);

        UnitFunctionality unitFunctionality = null;

        if (playerWon)
        {
            postBattleUI.ToggleExpGainedUI(true);

            yield return new WaitForSeconds(.5f);

            // Enable unit level image for post combat
            for (int i = 0; i < activeRoomHeroes.Count; i++)
            {
                if (!activeRoomHeroes[i].heroRoomUnit && !activeRoomHeroes[i].isDead)
                {
                    activeRoomHeroes[i].ToggleUnitLevelImage(true);

                    // Give combat win heal to alive allies
                    float combatWinHeal = ((float)combatCompleteHealPerc / 100f) * activeRoomHeroes[i].GetUnitMaxHealth();
                    activeRoomHeroes[i].ResetPowerUI();
                    activeRoomHeroes[i].UpdateUnitCurHealth((int)combatWinHeal, false, false);
                    //StartCoroutine(activeRoomHeroes[i].SpawnPowerUI(combatWinHeal, false, false, null, false));
                }
                else
                {
                    unitFunctionality = activeRoomHeroes[i];
                    unitFunctionality.ResetUnitCurAttackCharge();
                }

                yield return new WaitForSeconds(.2f);
            }

            yield return new WaitForSeconds(.5f);

            bool doneOnce = false;

            // Give Exp to ally units
            for (int i = 0; i < activeRoomHeroes.Count; i++)
            {
                if (!activeRoomHeroes[i].isDead)
                {
                    activeRoomHeroes[i].ToggleUnitBG(false);

                    // Give EXP to ally units, NOT a unit that was just added to player's party
                    if (!activeRoomHeroes[i].heroRoomUnit)
                    {
                        doneOnce = true;
                        float count = GetExperienceGained();
                        Debug.Log("exp given: " + count);
                        activeRoomHeroes[i].UpdateUnitExp((int)count);
                    }

                    else
                    {
                        unitFunctionality = activeRoomHeroes[i];
                        unitFunctionality.ResetUnitCurAttackCharge();
                    }

                    activeRoomHeroes[i].ToggleHideEffects(playerWon);

                    yield return new WaitForSeconds(.2f);
                }
            }

            yield return new WaitForSeconds(.5f);

            if (!doneOnce)
            {
                // Enable post battle to map button for next post battle scene
                StartCoroutine(PostBattle.Instance.ToggleButtonPostBattleMap(true));
            }

            // display gear rewards
            postBattleUI.ToggleRewardsUI(true);
            GearRewards.Instance.ToggleGearRewardsTab(true);
            GearRewards.Instance.FillGearRewardsTable();

            // Reset hero room unit to be a normal unit (code purpose only)
            if (unitFunctionality != null)
                unitFunctionality.heroRoomUnit = false;

            yield return new WaitForSeconds(0);

            playerLost = false;

            CombatGridManager.Instance.ResetCombatSlots(false);
        }
        // If player LOST, reset game
        else
        {
            // Enable unit level image for post combat
            for (int i = 0; i < activeRoomHeroes.Count; i++)
            {
                if (!activeRoomHeroes[i].isDead)
                    activeRoomHeroes[i].ToggleUnitLevelImage(true);
            }

            // Set correct post battle UI
            postBattleUI.ToggleExpGainedUI(false);
            postBattleUI.ToggleRewardsUI(false);

            playerLost = true;


            TeamGearManager.Instance.ResetGearOwned();
            TeamItemsManager.Instance.ResetItemOwned();
            CombatGridManager.Instance.ResetCombatSlots(true);

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
        ToggleUIElement(fighterSelectedMainSlotDesc, false);

        ToggleEndTurnButton(false);
    }

    public void SetupPlayerSkillsUI(SkillData activeSkill = null)
    {
        ToggleUIElement(playerWeaponBG, false);
        ToggleUIElement(playerWeapon, false);
        ToggleUIElementFull(playerWeaponChild, false);
        ToggleUIElement(playerWeaponBackButton, false);

        WeaponManager.Instance.ToggleAttackButtonInteractable(false);

        ToggleUIElement(playerAbilities, true);
        ToggleUIElement(fighterSelectedMainSlotDesc, true);

        if (!combatOver)
            ToggleEndTurnButton(true, true);

        UpdateUnitsSelectedText();

        UpdateEnemyPosition(true);
    }

    public void SetupPlayerWeaponUI()
    {
        //if (!CheckIfAnyUnitsSelected())
        //    return;

        WeaponManager.Instance.DisableAlertUI();
        WeaponManager.Instance.ToggleEnabled(true);

        // Update weapona accumulated hits with skill base hits
        SkillData skill = GameManager.Instance.GetActiveSkill();

        int powerHitsAdditional = 0;
        //Debug.Log(" skill thing = " + GetActiveSkill().skillBaseHitOutput);
        if (GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE)
            powerHitsAdditional = GetActiveUnitFunctionality().curPowerHits;
        else
            powerHitsAdditional = GetActiveUnitFunctionality().curPowerHits;

        if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            StartCoroutine(WeaponManager.Instance.UpdateWeaponAccumulatedHits(1 + GetActiveSkill().skillBaseHitOutput + GetActiveSkill().upgradeIncHitsCount + powerHitsAdditional, false));

        WeaponManager.Instance.StartHitLine();

        AudioManager.Instance.PauseCombatMusic(true);

        AudioManager.Instance.Play("AttackBarLoop");

        ToggleUIElement(playerAbilities, false);
        ToggleUIElement(fighterSelectedMainSlotDesc, false);

        ToggleUIElement(playerWeaponBG, true);
        ToggleUIElementFull(playerWeaponChild, true);

        ToggleUIElement(playerWeapon, true);
        SetActiveWeapon();

        ToggleUIElement(playerWeaponBackButton, true);
        ToggleEndTurnButton(false);

        WeaponManager.Instance.ToggleAttackButtonInteractable(true);
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
        for (int i = 0; i < activeRoomHeroes.Count; i++)
        {
            combinedPowerLevel += activeRoomHeroes[i].GetUnitLevel() + activeRoomHeroes[i].GetUnitValue();
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

    int GetAllyLevelAverage()
    {
        float count = 0;
        float total = 0;
        for (int i = 0; i < activeRoomHeroes.Count; i++)
        {
            count++;
            total += activeRoomHeroes[i].GetUnitLevel();
        }

        total /= count;

        int finalVal = (int)total;
        return finalVal;

    }
    public void StartRoom(RoomMapIcon room, FloorData activeFloor)
    {
        // Reset experience gained from killed enemies
        ResetExperienceGained();

        playerInCombat = true;

        StartCoroutine(PostBattle.Instance.ToggleButtonPostBattleMap(false));

        if (room == null)
            return;

        // If room type is enemy, spawn enemy room
        if (room.curRoomType == RoomMapIcon.RoomType.ENEMY || room.curRoomType == RoomMapIcon.RoomType.HERO || room.curRoomType == RoomMapIcon.RoomType.BOSS || room.curRoomType == RoomMapIcon.RoomType.ITEM)
        {
            // Stop Map music
            AudioManager.Instance.PauseMapMusic(true);

            CombatGridManager.Instance.isCombatMode = false;
            CombatGridManager.Instance.UpdateCombatMainSlots();
            ToggleSkillsItemToggleButton(false);
            CombatGridManager.Instance.ToggleCombatSlotsInput(true);

            //CombatGridManager.Instance.GetButtonAttackMovement().ButtonCombatAttackMovement(true);

            // Combat battle music
            AudioManager.Instance.Play("Combat");

            // Update background
            BackgroundManager.Instance.UpdateBackground(BackgroundManager.Instance.GetCombatForest());

            int spawnEnemyPosIndex = 0;

            FloorData floor = RoomManager.Instance.GetActiveFloor();

            int enemySpawnFirst = Random.Range(floor.minRoomValue, floor.maxRoomValue + 1);
            int enemySpawnValue = (enemySpawnFirst * (RoomManager.Instance.GetFloorCount()*2));


            enemySpawnValue *= activeTeam.Count;

            if (RoomManager.Instance.GetFloorCount() == 1)
            {
                // rand add +3 to +6 difficulty for the room
                enemySpawnValue += Random.Range(2, 4);
                //enemySpawnValue *= ((RoomManager.Instance.GetFloorCount() * RoomManager.Instance.GetFloorCount()) * 2);
            }
            else
            {
                // rand add +3 to +6 difficulty for the room
                enemySpawnValue += Random.Range(5, 9) * RoomManager.Instance.GetFloorCount();
                //enemySpawnValue *= ((RoomManager.Instance.GetFloorCount() * RoomManager.Instance.GetFloorCount()) * 3);
            }
            
            if (activeRoomHeroes.Count > 1)
            {
                enemySpawnValue += (activeRoomHeroes.Count * 2) - 1;
            }

            // If room is hero, spawn additional enemies
            if (room.curRoomType == RoomMapIcon.RoomType.HERO)
            {
                enemySpawnValue += (Random.Range(heroRoomMinEnemiesIncCount, heroRoomMaxEnemiesIncCount + 1) * (RoomManager.Instance.GetFloorCount())) - 1;
            }
            else if (room.curRoomType == RoomMapIcon.RoomType.ITEM)
            {
                enemySpawnValue += (Random.Range(heroRoomMinEnemiesIncCount, heroRoomMaxEnemiesIncCount + 1) * (RoomManager.Instance.GetFloorCount())) - 2;
            }
            else if (room.curRoomType == RoomMapIcon.RoomType.BOSS)
            {
                if (RoomManager.Instance.GetFloorCount() == 1)
                    enemySpawnValue += (Random.Range(heroRoomMinEnemiesIncCount, heroRoomMaxEnemiesIncCount) * (RoomManager.Instance.GetFloorCount()));
                else
                    enemySpawnValue += Random.Range(heroRoomMinEnemiesIncCount, heroRoomMaxEnemiesIncCount + 1) * (RoomManager.Instance.GetFloorCount());
            }

            enemySpawnValue += RoomManager.Instance.GetRoomsCleared() * ((RoomManager.Instance.GetFloorCount() * 4) - 1);

            //Debug.Log("Room value " + enemySpawnValue);
            // Spawn enemy type
            for (int i = 0; i < enemySpawnValue; i++)
            {
                GameObject go = null;

                int spawnEnemyIndex = Random.Range(0, activeFloor.enemyUnits.Count);

                UnitData unit = activeFloor.enemyUnits[spawnEnemyIndex];  // Reference

                int randCombatSlot = Random.Range(0, CombatGridManager.Instance.GetEnemySpawnCombatSlots().Count);

                // Check if there are remaining enemy unit spawn locations left
                if (spawnEnemyPosIndex <= CombatGridManager.Instance.GetEnemySpawnCombatSlots().Count -1)
                {
                    if (activeRoomHeroes.Count == 1)
                    {
                        if (RoomManager.Instance.GetActiveRoom().curRoomType == RoomMapIcon.RoomType.BOSS)
                        {
                            if (spawnEnemyPosIndex >= 3)
                                break;
                        }

                        // Force no higher then 2 enemies if fighter team size is 1
                        if (spawnEnemyPosIndex >= 2)
                            break;                           
                    }
                    else if (activeRoomHeroes.Count == 2)
                    {
                        if (RoomManager.Instance.GetActiveRoom().curRoomType == RoomMapIcon.RoomType.BOSS)
                        {
                            if (spawnEnemyPosIndex >= 5)
                                break;
                        }

                        // Force no higher then 4 enemies if fighter team size is 1
                        if (spawnEnemyPosIndex >= 4)
                            break;
                    }

                    go = Instantiate(baseUnit, CombatGridManager.Instance.GetEnemySpawnCombatSlot(randCombatSlot).transform);
                    UnitFunctionality unitFunctionality2 = go.GetComponent<UnitFunctionality>();

                    // If a unit is already on the attempted spawn slot, delete unit and start again
                    if (CombatGridManager.Instance.GetEnemySpawnCombatSlot(randCombatSlot).GetLinkedUnit() != null)
                    {
                        Destroy(go);
                        if (i >= 0)
                            i--;

                        continue;
                    }

                    CombatGridManager.Instance.GetEnemySpawnCombatSlot(randCombatSlot).UpdateLinkedUnit(unitFunctionality2);


                    unitFunctionality2.UpdateActiveCombatSlot(CombatGridManager.Instance.GetEnemySpawnCombatSlot(randCombatSlot));

                    spawnEnemyPosIndex++;
                }                
                else
                {
                    Destroy(go);
                    break;
                }

                UnitFunctionality unitFunctionality = go.GetComponent<UnitFunctionality>();

               // CombatGridManager.Instance.GetEnemySpawnCombatSlot(randCombatSlot).UpdateLinkedUnit(unitFunctionality);
                unitFunctionality.UpdateUnitValue(unit.GetUnitValue());

                int rand = Random.Range(RoomManager.Instance.GetFloorCount(), RoomManager.Instance.GetFloorCount()+100);

                // If unit random max level, check if there is enough room size for it, if not, downscale it down 1 lv
                if (rand >= RoomManager.Instance.GetFloorCount())
                {
                    /*
                    if (i + rand + 1 + unit.GetUnitValue() >= enemySpawnValue)
                    {
                        int val = (i + rand + 1 + unit.GetUnitValue()) - enemySpawnValue;
                        //rand--;
                        rand -= val;

                        if (rand < 0)
                            rand = 0;
                    }
                    */

                    if (RoomManager.Instance.GetFloorCount() == 1)
                    {
                        if (rand >= GetAllyLevelAverage() + 1)
                        {
                            //Debug.Log("rand = " + rand);
                            int val = 0;
                            if (room.curRoomType == RoomMapIcon.RoomType.BOSS)
                                val = rand - Random.Range(5, 7);
                            if (room.curRoomType == RoomMapIcon.RoomType.HERO)
                                val = rand - Random.Range(3, 5);
                            if (room.curRoomType == RoomMapIcon.RoomType.ITEM)
                                val = rand - Random.Range(3, 4);
                            if (room.curRoomType == RoomMapIcon.RoomType.ENEMY)
                            {
                                // Ensure first room is half the difficulty of a normal enemy room
                                if (RoomManager.Instance.GetRoomsCleared() == 0)
                                    val = rand - Random.Range(0, 2);
                                else
                                    val = rand - Random.Range(0, 3);
                            }

                            rand -= val;

                            if (rand < 0)
                                rand = 0;
                        }
                    }
                    else if (RoomManager.Instance.GetFloorCount() == 2)
                    {
                        if (rand >= GetAllyLevelAverage())
                        {
                            //Debug.Log("rand = " + rand);
                            int val = 0;
                            if (room.curRoomType == RoomMapIcon.RoomType.BOSS)
                                val = rand - Random.Range(3, 5) * RoomManager.Instance.GetFloorCount();
                            if (room.curRoomType == RoomMapIcon.RoomType.HERO)
                                val = rand - Random.Range(2, 4) * RoomManager.Instance.GetFloorCount();
                            if (room.curRoomType == RoomMapIcon.RoomType.ITEM)
                                val = rand - Random.Range(1, 3) * RoomManager.Instance.GetFloorCount();
                            if (room.curRoomType == RoomMapIcon.RoomType.ENEMY)
                                val = rand - Random.Range(-1, 2) * RoomManager.Instance.GetFloorCount();

                            rand -= val;

                            if (rand < 0)
                                rand = 0;
                        }
                    }
                    else if (RoomManager.Instance.GetFloorCount() == 3)
                    {
                        if (rand >= GetAllyLevelAverage())
                        {
                            //Debug.Log("rand = " + rand);
                            int val = 0;
                            if (room.curRoomType == RoomMapIcon.RoomType.BOSS)
                                val = rand - Random.Range(3, 5) * RoomManager.Instance.GetFloorCount();
                            if (room.curRoomType == RoomMapIcon.RoomType.HERO)
                                val = rand - Random.Range(2, 4) * RoomManager.Instance.GetFloorCount();
                            if (room.curRoomType == RoomMapIcon.RoomType.ITEM)
                                val = rand - Random.Range(1, 3) * RoomManager.Instance.GetFloorCount();
                            if (room.curRoomType == RoomMapIcon.RoomType.ENEMY)
                                val = rand - Random.Range(-1, 2) * RoomManager.Instance.GetFloorCount();

                            rand -= val;

                            if (rand < 0)
                                rand = 0;
                        }
                    }
                    else if (RoomManager.Instance.GetFloorCount() == 4)
                    {
                        if (rand >= GetAllyLevelAverage())
                        {
                            //Debug.Log("rand = " + rand);
                            int val = 0;
                            if (room.curRoomType == RoomMapIcon.RoomType.BOSS)
                                val = rand - Random.Range(3, 5) * RoomManager.Instance.GetFloorCount();
                            if (room.curRoomType == RoomMapIcon.RoomType.HERO)
                                val = rand - Random.Range(2, 4) * RoomManager.Instance.GetFloorCount();
                            if (room.curRoomType == RoomMapIcon.RoomType.ITEM)
                                val = rand - Random.Range(1, 3) * RoomManager.Instance.GetFloorCount();
                            if (room.curRoomType == RoomMapIcon.RoomType.ENEMY)
                                val = rand - Random.Range(-1, 2) * RoomManager.Instance.GetFloorCount();

                            rand -= val;

                            if (rand < 0)
                                rand = 0;
                        }
                    }
                    else if (RoomManager.Instance.GetFloorCount() == 5)
                    {
                        if (rand >= GetAllyLevelAverage())
                        {
                            //Debug.Log("rand = " + rand);
                            int val = 0;
                            if (room.curRoomType == RoomMapIcon.RoomType.BOSS)
                                val = rand - Random.Range(3, 5) * RoomManager.Instance.GetFloorCount();
                            if (room.curRoomType == RoomMapIcon.RoomType.HERO)
                                val = rand - Random.Range(2, 4) * RoomManager.Instance.GetFloorCount();
                            if (room.curRoomType == RoomMapIcon.RoomType.ITEM)
                                val = rand - Random.Range(1, 3) * RoomManager.Instance.GetFloorCount();
                            if (room.curRoomType == RoomMapIcon.RoomType.ENEMY)
                                val = rand - Random.Range(-1, 2) * RoomManager.Instance.GetFloorCount();

                            rand -= val;

                            if (rand < 0)
                                rand = 0;
                        }
                    }
                    else if (RoomManager.Instance.GetFloorCount() == 6)
                    {
                        //Debug.Log("rand = " + rand);

                        int val = 0;
                        if (room.curRoomType == RoomMapIcon.RoomType.BOSS)
                            val = rand - Random.Range(3, 5) * RoomManager.Instance.GetFloorCount();
                        if (room.curRoomType == RoomMapIcon.RoomType.HERO)
                            val = rand - Random.Range(2, 4) * RoomManager.Instance.GetFloorCount();
                        if (room.curRoomType == RoomMapIcon.RoomType.ITEM)
                            val = rand - Random.Range(1, 3) * RoomManager.Instance.GetFloorCount();
                        if (room.curRoomType == RoomMapIcon.RoomType.ENEMY)
                            val = rand - Random.Range(-1, 2) * RoomManager.Instance.GetFloorCount();

                        rand -= val;

                        if (rand < 0)
                            rand = 0;
                    }
                }
                if (room.curRoomType == RoomMapIcon.RoomType.BOSS)
                {
                    int val = Random.Range(0, 4);
                    if (val == 0)
                        rand /= 2;
                }

                int unitLevel = RoomManager.Instance.GetFloorCount() + rand;

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

                unitFunctionality.UpdateUnitLevel(unitLevel, 0, true);
                //Debug.Log("i = " + i);

                //Debug.Log("unit val " + unit.GetUnitValue());
                //Debug.Log("unit lv " + unitLevel);
                // Set unit stats
                unitFunctionality.UpdateUnitType("Enemy");
                unitFunctionality.ResetPosition();
                unitFunctionality.UpdateUnitName(unit.unitName);
                unitFunctionality.UpdateUnitSprite(unit.characterPrefab);

                if (unit.curRaceType == UnitData.RaceType.HUMAN)
                {
                    unitFunctionality.curUnitRace = UnitFunctionality.UnitRace.HUMAN;
                    unitFunctionality.UpdateFighterRaceIcon("HUMAN");
                }                   
                else if (unit.curRaceType == UnitData.RaceType.BEAST)
                {
                    unitFunctionality.curUnitRace = UnitFunctionality.UnitRace.BEAST;
                    unitFunctionality.UpdateFighterRaceIcon("BEAST");
                }                    
                else if (unit.curRaceType == UnitData.RaceType.ETHEREAL)
                {
                    unitFunctionality.curUnitRace = UnitFunctionality.UnitRace.ETHEREAL;
                    unitFunctionality.UpdateFighterRaceIcon("ETHEREAL");
                }
                    
                unitFunctionality.ToggleFighterRaceIcon(true);

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
                unitFunctionality.UpdateStartingMaxHealth(unit.startingMaxHealth);

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

                unitFunctionality.UpdateUnitPowerHits(unitFunctionality.GetUnitLevel());
                unitFunctionality.UpdateUnitHealingHits(unitFunctionality.GetUnitLevel());

                unitFunctionality.UpdateUnitVisual(unit.unitSprite);
                unitFunctionality.UpdateUnitIcon(unit.unitIcon);

                unitFunctionality.deathClip = unit.deathClip;
                unitFunctionality.hitRecievedClip = unit.hitRecievedClip;

                AddActiveRoomAllUnitsFunctionality(unitFunctionality, true);
            }

            int curChallengeCount = 0;

            // Determine enemy unit value
            int floorDiff = RoomManager.Instance.GetFloorDifficulty();

            // Loop through all units in room
            for (int x = 0; x < activeRoomAllUnitFunctionalitys.Count; x++)
            {
                activeRoomAllUnitFunctionalitys[x].ToggleTooltipItems(false);
                activeRoomAllUnitFunctionalitys[x].ToggleTooltipGear(false);
                activeRoomAllUnitFunctionalitys[x].ToggleTooltipStats(false);

                activeRoomAllUnitFunctionalitys[x].ToggleUnitBottomStats(true);

                // Update unit sorting
                activeRoomAllUnitFunctionalitys[x].UpdateSorting(unitCombatSortingLevel);

                activeRoomAllUnitFunctionalitys[x].UpdateStartingMaxHealth((int)activeRoomAllUnitFunctionalitys[x].GetUnitMaxHealth());
                activeRoomAllUnitFunctionalitys[x].startingRoomSpeed = activeRoomAllUnitFunctionalitys[x].GetUnitSpeed();
                activeRoomAllUnitFunctionalitys[x].startingRoomDefense = activeRoomAllUnitFunctionalitys[x].GetCurDefense();
                activeRoomAllUnitFunctionalitys[x].powerHitsRoomStarting = activeRoomAllUnitFunctionalitys[x].curPowerHits;
                activeRoomAllUnitFunctionalitys[x].startingRoomMaxHealth = (int)activeRoomAllUnitFunctionalitys[x].GetUnitMaxHealth();

                // Enable allies level image for combat
                if (activeRoomAllUnitFunctionalitys[x].curUnitType == UnitFunctionality.UnitType.PLAYER)
                {                   
                    activeRoomAllUnitFunctionalitys[x].ToggleUnitHitsRemaining(true);

                    // Disable unit level image in team setup tab
                    activeRoomAllUnitFunctionalitys[x].ToggleUnitLevelImage(true);

                    // Set all refillable items back to full uses remaining
                    activeRoomAllUnitFunctionalitys[x].SetItemUsesMax();

                    // Hide level up text
                    activeRoomAllUnitFunctionalitys[x].ToggleTextAlert(false);
                    
                    //activeRoomAllies[i].ToggleActionNextBar(true);
                }
                // If unit in room is an ENEMY
                else
                {
                    activeRoomAllUnitFunctionalitys[x].ToggleUnitHitsRemaining(false);
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
                int rand = Random.Range(0, CombatGridManager.Instance.GetEnemySpawnCombatSlots().Count);

                if (rands.Contains(rand))
                {
                    if (i != 0)
                        i--;
                    continue;
                }

                rands.Add(rand);

                //activeRoomEnemies[i].SetPositionAndParent(CombatGridManager.Instance.GetEnemySpawnCombatSlot(rand).transform);
                //CombatGridManager.Instance.GetEnemySpawnCombatSlot(rand).UpdateLinkedUnit(activeRoomEnemies[i]);
            }

            RoomManager.Instance.ToggleInteractable(true);

            UpdateAllyVisibility(true, false);

            playerUIElement.UpdateAlpha(1);     // Enable player UI
            ToggleUIElement(turnOrder, true);  // Enable turn order

            // Update allies into position for battle
            UpdateAllAlliesPosition(false, GetActiveUnitType());

            MapManager.Instance.mapOverlay.ToggleTeamPageButton(false);

            for (int i = 0; i < activeRoomHeroes.Count; i++)
            {
                activeRoomHeroes[i].ToggleUnitDisplay(true);
            }



            ToggleAllowSelection(true);
            ToggleAllyUnitSelection(true);
            ToggleEnemyUnitSelection(true);
            //Debug.Log("aaa");

            transitionSprite.AllowFadeOut();

            //MapManager.Instance.ToggleButtonSkillsTabCombat(true);
            //MapManager.Instance.ToggleButtonItemsTabCombat(true);


            Invoke("UpdateTurnOrder", 0);
        }

        // If room type is shop, spawn shop room
        else if (room.curRoomType == RoomMapIcon.RoomType.SHOP)
        {
            ShopManager.Instance.TogglePlayerInShopRoom();

            
            // Map open SFX
            AudioManager.Instance.Play("SFX_ShopEnterLeave");

            AudioManager.Instance.Play("M_ShopBG");

            // Stop Map music
            AudioManager.Instance.PauseMapMusic(true);

            // Update background
            BackgroundManager.Instance.UpdateBackground(BackgroundManager.Instance.GetShopForest());

            UpdateAllyVisibility(true, false, true);

            // Update unit energy bar off
            for (int i = 0; i < activeRoomHeroes.Count; i++)
            {
                UpdateActiveUnitStatBar(activeRoomHeroes[i], true, false);
                activeRoomHeroes[i].ToggleActionNextBar(false);
            }

            playerUIElement.UpdateAlpha(1);     // Disable player UI

            ShopManager.Instance.SetActiveRoom(RoomManager.Instance.GetActiveRoom());


            ToggleUIElement(turnOrder, false);  // Disable turn order
            ResetSelectedUnits();   // Disable all unit selections
            //ToggleAllAlliesHealthBar(false);    // Disable all unit health bar visual
            ToggleAllowSelection(false);
            ToggleAllyUnitSelection(false);
            ShopManager.Instance.FillShopItems(false, false);

            ShopManager.Instance.lastVisitedShopRoom = RoomManager.Instance.GetActiveRoom();
            // Update allies into position for shop
            UpdateAllAlliesPosition(false, GetActiveUnitType(), false, true);

            MapManager.Instance.mapOverlay.ToggleTeamPageButton(false);

            for (int i = 0; i < activeRoomHeroes.Count; i++)
            {
                activeRoomHeroes[i].ToggleUnitDisplay(true);

                #region Display Fighter Item Tooltip in shop if they have at least 1 item equipt
                if (i == 0)
                {
                    if (TeamItemsManager.Instance.equippedItemsMain.Count > 0)
                    {
                        activeRoomHeroes[i].ToggleTooltipStats(true, true);
                        activeRoomHeroes[i].UpdateTooltipItems(0, 0, 0);
                        activeRoomHeroes[i].ToggleTooltipItems(true);
                    }
                }
                else if (i == 1)
                {
                    if (TeamItemsManager.Instance.equippedItemsSecond.Count > 0)
                    {
                        activeRoomHeroes[i].ToggleTooltipStats(true, true);
                        activeRoomHeroes[i].UpdateTooltipItems(0, 0, 0);
                        activeRoomHeroes[i].ToggleTooltipItems(true);
                    }
                }
                else if (i == 2)
                {
                    if (TeamItemsManager.Instance.equippedItemsThird.Count > 0)
                    {
                        activeRoomHeroes[i].ToggleTooltipStats(true, true);
                        activeRoomHeroes[i].UpdateTooltipItems(0, 0, 0);
                        activeRoomHeroes[i].ToggleTooltipItems(true);
                    }
                }
                #endregion
            }

            ShopManager.Instance.ToggleExitShopButton(true);

            ShopManager.Instance.DisplayFallenHeroes();
            ShopManager.Instance.GetActiveRoom().UpdateIsVisited(true);

            transitionSprite.AllowFadeOut();

            
            return;
        }
    }

    public void SetAllFightersSelected(bool toggle = true)
    {
        for (int i = 0; i < activeRoomHeroes.Count; i++)
        {
            activeRoomHeroes[i].ToggleSelected(toggle);
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

    public void UpdateActiveSkill(SkillData skill, bool updateRange = true)
    {
        activeSkill = skill;

        if (skill != null)
        {
            // Update active unit attack range
            if (updateRange)
                CombatGridManager.Instance.UpdateUnitAttackRange(GetActiveUnitFunctionality());
        }
    }

    public void UpdateActiveItem(ItemPiece item)
    {
        activeItem = item;
    }

    public void UpdateActiveItemSlot(Slot newItemSlot)
    {
        activeItemSlot = newItemSlot;
    }

    public Slot GetActiveItemSlot()
    {
        return activeItemSlot;
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
        for (int i = 0; i < activeRoomHeroes.Count; i++)
        {
            if (activeRoomHeroes[i].isTaunting)
                return activeRoomHeroes[i];
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
        
    IEnumerator UpdateSelectedUnitsEffectVisual(bool skill = true)
    {
        if (skill)
        {
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

            yield return new WaitForSeconds(allyRangedSkillWaitTime / 3);

            // Display effect visual to each selected unit before the power is shown
            for (int i = 0; i < CombatGridManager.Instance.targetedCombatSlots.Count; i++)
            {
                if (GetActiveSkill().targetEffectVisualAC != null)
                {
                    CombatGridManager.Instance.targetedCombatSlots[i].UpdateEffectVisualAnimator(GetActiveSkill().targetEffectVisualAC);

                    if (GetActiveSkill().skillHit != null && GetActiveSkill().skillProjectile == null)
                        AudioManager.Instance.Play(GetActiveSkill().skillHit.name);

                    yield return new WaitForSeconds(timeTillNextTargetEffetVisual);
                }
            }
        }
        else
        {
            // Display effect visual to each selected unit before the power is shown
            if (GetActiveItem().itemAnnounce != null)
            {
                if (GetActiveItem().itemAnnounce == null)
                {
                    yield return new WaitForSeconds(allyRangedSkillWaitTime / 1.5f);

                    // Projectile Launch SFX
                    AudioManager.Instance.Play(GetActiveItem().itemAnnounce.name);
                }
            }

            //yield return new WaitForSeconds(allyRangedSkillWaitTime / 3);

            // Display effect visual to each selected unit before the power is shown
            for (int i = 0; i < CombatGridManager.Instance.targetedCombatSlots.Count; i++)
            {
                if (GetActiveItem().itemVisualAC != null)
                {
                    CombatGridManager.Instance.targetedCombatSlots[i].UpdateEffectVisualAnimator(GetActiveItem().itemVisualAC);

                    if (GetActiveItem().projectileHit != null)
                        AudioManager.Instance.Play(GetActiveItem().projectileHit.name);

                    yield return new WaitForSeconds(timeTillNextTargetEffetVisual);
                }
            }
        }
    }

    public IEnumerator WeaponAttackCommand(int power, int hitCount = 0, int effectHitAcc = -1, bool miss = false, List<UnitFunctionality> selectedUnits = null)
    {
        GetActiveUnitFunctionality().ToggleHeroWeapon(false);

        if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            GetActiveUnitFunctionality().TriggerTextAlert(GetActiveSkill().skillName, 1, false, "", false, true);

        if (miss)
            power = 0;

        if (unitsSelected.Count == 0)
            yield break;

        // Reset each units power UI
        for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
        {
            activeRoomAllUnitFunctionalitys[i].ResetPowerUI();
        }

        GetActiveUnitFunctionality().effects.UpdateAlpha(1);

        UnitFunctionality castingUnit = GetActiveUnitFunctionality();

        if (GetActiveSkill().curRangedType == SkillData.SkillRangedType.RANGED)
        {
            if (GetActiveSkill().curAnimType == SkillData.SkillAnimType.SKILL)
                GetActiveUnitFunctionality().GetAnimator().SetTrigger("SkillFlg");
            else
                GetActiveUnitFunctionality().GetAnimator().SetTrigger("AttackFlg");

            // Display active unit hits remaining text, Update hits remaining text
            GetActiveUnitFunctionality().ToggleHitsRemainingText(true);
            if (power == 0)
                GetActiveUnitFunctionality().UpdateHitsRemainingText(effectHitAcc);
            else
                GetActiveUnitFunctionality().UpdateHitsRemainingText(hitCount);

            if (miss)
                GetActiveUnitFunctionality().UpdateHitsRemainingText(1);

            // Display effect visual to each selected unit before the power is shown
            StartCoroutine(UpdateSelectedUnitsEffectVisual(true));

            yield return new WaitForSeconds(allyRangedSkillWaitTime);

            // If skill has no projectile, dont spawn it. stops white square from spawning
            if (GetActiveSkill().skillProjectile != null)
            {
                for (int w = 0; w < hitCount; w++)
                {
                    // Loop through all selected units, spawn projectiles, if target is dead stop.
                    for (int z = CombatGridManager.Instance.targetedCombatSlots.Count - 1; z >= 0; z--)
                    {
                        if (CombatGridManager.Instance.targetedCombatSlots[z] == null)
                            continue;
                        else
                        {
                            GetActiveUnitFunctionality().SpawnProjectile(CombatGridManager.Instance.targetedCombatSlots[z].transform);
                        }
                    }

                    int maxHitWorth = 15;
                    if (hitCount > maxHitWorth)
                        yield return new WaitForSeconds(timeBetweenProjectile - (0.0025f * (maxHitWorth - 1)));
                    else
                        yield return new WaitForSeconds(timeBetweenProjectile - (0.0025f * (hitCount - 1)));
                }
            }

            if (GetActiveSkill().targetEffectVisualAC != null)
                yield return new WaitForSeconds(unitPowerUIWaitTime);
            else
            {
                // Null check
                if (CombatGridManager.Instance.targetedCombatSlots.Count > 0)
                {
                    if (CombatGridManager.Instance.targetedCombatSlots[0].GetLinkedUnit())
                    {
                        if (CombatGridManager.Instance.targetedCombatSlots[0].GetLinkedUnit())
                        {
                            UnitFunctionality unit = CombatGridManager.Instance.targetedCombatSlots[0].GetLinkedUnit();
                            // Set timing for player allies projectiles
                            if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER && unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                            {

                            }
                            else if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER && unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                            {
                                yield return new WaitForSeconds(allyRangedSkillWaitTime / 2);
                            }

                            // set timing for enemy allies projectiles
                            if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY && unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                            {

                            }
                            else if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY && unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                            {
                                yield return new WaitForSeconds(allyRangedSkillWaitTime / 2);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (GetActiveSkill().curAnimType == SkillData.SkillAnimType.SKILL)
                GetActiveUnitFunctionality().GetAnimator().SetTrigger("SkillFlg");
            else
                GetActiveUnitFunctionality().GetAnimator().SetTrigger("AttackFlg");

            // Display active unit hits remaining text, Update hits remaining text
            GetActiveUnitFunctionality().ToggleHitsRemainingText(true);
            if (power == 0)
                GetActiveUnitFunctionality().UpdateHitsRemainingText(effectHitAcc);
            else
                GetActiveUnitFunctionality().UpdateHitsRemainingText(hitCount);

            if (miss)
                GetActiveUnitFunctionality().UpdateHitsRemainingText(1);

            // Display effect visual to each selected unit before the power is shown
            StartCoroutine(UpdateSelectedUnitsEffectVisual(true));

            yield return new WaitForSeconds(allyMeleeSkillWaitTime);

            GetActiveUnitFunctionality().ToggleTextAlert(false);

            // Attack launch SFX
            //AudioManager.Instance.Play("Attack_Sword");
            if (GetActiveSkill().targetEffectVisualAC != null)
                yield return new WaitForSeconds(unitPowerUIWaitTime);
            else
            {
                if (CombatGridManager.Instance.targetedCombatSlots.Count > 0)
                {
                    if (CombatGridManager.Instance.targetedCombatSlots[0].GetLinkedUnit())
                    {
                        if (CombatGridManager.Instance.targetedCombatSlots[0].GetLinkedUnit())
                        {
                            UnitFunctionality unit = CombatGridManager.Instance.targetedCombatSlots[0].GetLinkedUnit();

                            // Set timing for player allies projectiles
                            if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER && unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                            {

                            }
                            else if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER && unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                            {
                                yield return new WaitForSeconds(allyMeleeSkillWaitTime / 2);
                            }

                            // set timing for enemy allies projectiles
                            if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY && unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                            {

                            }
                            else if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY && unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                            {
                                yield return new WaitForSeconds(allyMeleeSkillWaitTime / 2);
                            }
                        }
                    }
                }
            }
        }

        if (GetActiveSkill().isLongerSkillAnim)
            yield return new WaitForSeconds(0.5f);

        // For no power skills
        if (GetActiveSkill().curSkillPower == 0 || GetActiveSkill().healPowerAmount != 0)
        {
            // GetActiveUnitFunctionality().GetAnimator().SetTrigger("SkillFlg");

            float lowestHealth = 9999999999;
            float health = 1;
            UnitFunctionality lowestHealthHero = null;

            int targets = 0;

            UnitFunctionality unit = null;

            // Loop through all selected units
            for (int x = 0; x < CombatGridManager.Instance.targetedCombatSlots.Count; x++)
            {
                if (!CombatGridManager.Instance.targetedCombatSlots[x].GetLinkedUnit() && activeSkill.curskillSelectionAliveType == SkillData.SkillSelectionAliveType.ALIVE)
                    continue;
                else if (CombatGridManager.Instance.targetedCombatSlots[x].GetLinkedUnit() && activeSkill.curskillSelectionAliveType == SkillData.SkillSelectionAliveType.DEAD)
                    continue;
                else
                    targets++;

                if (targets > CombatGridManager.Instance.targetedCombatSlots.Count)
                    break;

                if (CombatGridManager.Instance.targetedCombatSlots[x].GetLinkedUnit())
                {
                    if (CombatGridManager.Instance.targetedCombatSlots[x].GetLinkedUnit() && activeSkill.curskillSelectionAliveType == SkillData.SkillSelectionAliveType.ALIVE)
                    {
                        unit = CombatGridManager.Instance.targetedCombatSlots[x].GetLinkedUnit();
                    }
                }
                else if (CombatGridManager.Instance.targetedCombatSlots[x].GetFallenUnits().Count > 0)
                {
                    if (CombatGridManager.Instance.targetedCombatSlots[x].GetFallenUnits()[0] && activeSkill.curskillSelectionAliveType == SkillData.SkillSelectionAliveType.DEAD)
                    {
                        unit = CombatGridManager.Instance.targetedCombatSlots[x].GetFallenUnits()[0];
                    }
                }

                // If active skil has an effect AND it's not a self cast, apply it to selected targets
                if (GetActiveSkill().effect != null && !GetActiveSkill().isSelfCast)
                {
                    int val = effectHitAcc;

                    if (unit)
                    {
                        if (unit.IsSelected())
                        {
                            // If active skill doubles current effects, do it
                            if (GetActiveSkill().isDoublingEffect)
                            {
                                if (unit.GetEffect("POISON"))
                                {
                                    int val2 = unit.GetEffect("POISON").GetTurnCountRemaining();

                                    int maxEffectCount = EffectManager.instance.GetMaxEffectTurnsRemaining();
                                    if (val2 > maxEffectCount)
                                        val2 = maxEffectCount;

                                    if (!miss)
                                    {
                                        unit.AddUnitEffect(GetActiveSkill().effect, unit, effectHitAcc, val2, false);

                                        if (GetActiveSkill().effect2 != null)
                                        {
                                            if (GetActiveSkill().effect2.curEffectName != EffectData.EffectName.OTHER_LINK)
                                            {
                                                yield return new WaitForSeconds(0.4f);
                                                unit.AddUnitEffect(GetActiveSkill().effect2, unit, effectHitAcc, val2, false);
                                            }
                                        }
                                    }
                                }
                            }

                            if (!miss)
                            {
                                if (GetActiveSkill().effect != null)
                                {
                                    if (GetActiveSkill().effect.curEffectName == EffectData.EffectName.REANIMATE && activeRoomHeroes.Count <= 2
                                        || GetActiveSkill().effect.curEffectName == EffectData.EffectName.REANIMATE && fallenHeroes.Count > 0)
                                        unit.AddUnitEffect(GetActiveSkill().effect, unit, effectHitAcc, val, false);
                                    else if (GetActiveSkill().effect.curEffectName != EffectData.EffectName.REANIMATE)
                                        unit.AddUnitEffect(GetActiveSkill().effect, unit, effectHitAcc, val, false);
                                }

                                if (GetActiveSkill().effect2 != null)
                                {
                                    if (GetActiveSkill().effect2.curEffectName != EffectData.EffectName.OTHER_LINK && GetActiveSkill().effect2.curEffectName != EffectData.EffectName.REANIMATE)
                                    {
                                        yield return new WaitForSeconds(0.25f);
                                        unit.AddUnitEffect(GetActiveSkill().effect2, unit, effectHitAcc, val, false);
                                    }
                                }
                            }
                        }

                    }


                    yield return new WaitForSeconds(0.15f);

                    if (GetActiveSkill().isHealingFromResult)
                    {
                        lowestHealth = 9999999999;

                        for (int n = 0; n < hitCount; n++)
                        {
                            for (int v = 0; v < activeRoomHeroes.Count; v++)
                            {
                                if (!activeRoomHeroes[v].isDead)
                                {
                                    health = (float)activeRoomHeroes[v].GetUnitCurHealth() / (float)activeRoomHeroes[v].GetUnitMaxHealth();

                                    if (lowestHealth > health)
                                    {
                                        lowestHealthHero = activeRoomHeroes[v];
                                        lowestHealth = health;
                                    }
                                }
                            }

                            WeaponManager.Instance.CalculatePower(isSkillsMode);
                            float healAmount = 0;

                            healAmount = (int)WeaponManager.Instance.calculatedPower;

                            if (lowestHealthHero != null)
                                lowestHealthHero.UpdateUnitCurHealth((int)healAmount, false, false, true, false, false);
                            else
                                GetActiveUnitFunctionality().UpdateUnitCurHealth((int)healAmount, false, false, true, false, false);

                            int maxHitWorth = 15;
                            if (hitCount > maxHitWorth)
                                yield return new WaitForSeconds(timeBetweenProjectile - (0.0025f * (maxHitWorth - 1)) / 2.35f);
                            else
                                yield return new WaitForSeconds(timeBetweenProjectile - (0.0025f * (hitCount - 1)) / 2.35f);
                        }
                    }
                }

                if (!miss && unit)
                {
                    if (unit.IsSelected())
                    {
                        // If skill targets dead targets, do so
                        if (GetActiveSkill().curskillSelectionAliveType == SkillData.SkillSelectionAliveType.DEAD)
                        {
                            // If player scored higher then only a miss, and target is dead
                            if (unit.isDead && hitCount != 0)
                            {
                                unit.ReviveUnit(hitCount, false, false);

                                if (GetActiveSkill().skillName == "REANIMATE")
                                    unit.SwitchTeams();
                            }
                            //else
                            //unit.DecreaseEffectTurnsLeft(false, false, false);
                        }
                    }
                }

#if !UNITY_EDITOR
                    Vibration.Vibrate(15);
#endif

                yield return new WaitForSeconds(0.15f);
            }
        }
        else
        {
            for (int z = 0; z < activeRoomAllUnitFunctionalitys.Count; z++)
            {
                activeRoomAllUnitFunctionalitys[z].attacked = false;
                activeRoomAllUnitFunctionalitys[z].hasAttacked = true;
            }

            for (int i = 0; i < CombatGridManager.Instance.targetedCombatSlots.Count; i++)
            {
                UnitFunctionality unit = CombatGridManager.Instance.targetedCombatSlots[i].GetLinkedUnit();

                if (unit)
                {
                    if (unit.IsSelected())
                    {
                        // If skill removes random effects, do so
                        if (activeSkill.isCleansingEffectRandom)
                        {
                            for (int c = 0; c < activeSkill.cleanseCount; c++)
                            {
                                if (unit.activeEffects.Count > 0)
                                {
                                    unit.DecreaseRandomNegativeEffect();
                                    yield return new WaitForSeconds(.15f);
                                }
                                else
                                    break;
                            }
                        }
                    }
                }
            }
        }

        if (GetActiveUnitFunctionality() == null)
        {
            Debug.LogWarning("No unit found!");
            yield break;
        }

        // Perform Damage / Heal
        StartCoroutine(TriggerPowerUI(power, hitCount, miss, effectHitAcc));


        //CombatGridManager.Instance.ToggleCombatGrid(true);
    }


    // Called from end of TriggerPowerUI()
    IEnumerator WeaponAttackCommand2(bool miss = true, int effectHitAcc = 0)
    {
        GetActiveUnitFunctionality().ToggleHitsRemainingText(false);

        // If skill is self cast, do it here
        if (GetActiveSkill().isSelfCast)
        {
            if (GetActiveSkill().effect != null)
            {
                if (!miss)
                {
                    GetActiveUnitFunctionality().AddUnitEffect(GetActiveSkill().effect, GetActiveUnitFunctionality(), effectHitAcc, effectHitAcc, false);

                    if (GetActiveSkill().effect2 != null)
                    {
                        GetActiveUnitFunctionality().AddUnitEffect(GetActiveSkill().effect2, GetActiveUnitFunctionality(), effectHitAcc, effectHitAcc, false);
                    }
                }


#if !UNITY_EDITOR
                    Vibration.Vibrate(15);
#endif
            }
        }

        //yield return new WaitForSeconds(1);

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

        GetActiveUnitFunctionality().hasAttacked = true;

        CombatGridManager.Instance.ToggleAllCombatSlotOutlines();

        // If active unit is player, setup player UI
        if (GetActiveUnitFunctionality().unitData.curUnitType == UnitData.UnitType.PLAYER || GetActiveUnitFunctionality().reanimated)
        {
            // Resume combat music
            AudioManager.Instance.PauseCombatMusic(false);

            WeaponManager.Instance.ResetAcc();

            UpdateDetailsBanner();
            ToggleSkillsItemToggleButton(false);
            UpdatePlayerAbilityUI(false, false, true);
            UpdateMainIconDetails(null, null, false);

            if (GetActiveUnitFunctionality().GetCurMovementUses() > 0)
            {
                isSkillsMode = false;

                CombatGridManager.Instance.UnselectAllSelectedCombatSlots();
                CombatGridManager.Instance.PerformBotAction(GetActiveUnitFunctionality());
                CombatGridManager.Instance.ToggleTabButtons("Movement");
            }
            else
            {
                isSkillsMode = false;

                CombatGridManager.Instance.UnselectAllSelectedCombatSlots();
                CombatGridManager.Instance.PerformBotAction(GetActiveUnitFunctionality());
                CombatGridManager.Instance.ToggleTabButtons("Items");
            }

        }
        // If active unit is enemy, check whether to move after attacking
        else if (GetActiveUnitFunctionality().unitData.curUnitType == UnitData.UnitType.ENEMY)
        {
            if (!GetActiveUnitFunctionality().isDead)
            {
                //if (GetActiveUnitFunctionality().GetCurMovementUses() == 0)
                //StartCoroutine(GetActiveUnitFunctionality().UnitEndTurn());  // end unit turn
                //else
                //{
                WeaponManager.Instance.ResetAcc();

                CombatGridManager.Instance.UnselectAllSelectedCombatSlots();
                CombatGridManager.Instance.isCombatMode = false;
                StartCoroutine(GetActiveUnitFunctionality().StartUnitTurn());
                    //CombatGridManager.Instance.AutoSelectMovement(GetActiveUnitFunctionality());
                //}
            }
        }
    }
    public IEnumerator TriggerPowerUI(int power = 0, int hitCount = 0, bool miss = false, int effectHitAcc = 0)
    {
        // Loop as many times as power text will appear
        for (int x = 0; x < hitCount; x++)
        {
            // If 1 enemy is trying to target an ally, dont?
            if (CombatGridManager.Instance.targetedCombatSlots.Count == 0)
            {
                GetActiveUnitFunctionality().ToggleHitsRemainingText(false);
                break;
            }

            if (CombatGridManager.Instance.targetedCombatSlots[0] == null)
            {
                GetActiveUnitFunctionality().ToggleHitsRemainingText(false);
                continue;
            }

            if (!miss)
                GetActiveUnitFunctionality().UpdateHitsRemainingText(hitCount - x);
            else
                GetActiveUnitFunctionality().UpdateHitsRemainingText(0);

            bool flag = false;

            if (GetActiveSkill() && isSkillsMode)
            {
                if (GetActiveSkill().startingSkillPower == 0 || GetActiveSkill().isSpecial)
                {
                    flag = true;
                }
            }
            else if (GetActiveItem() && !isSkillsMode)
            {
                if (GetActiveItem().itemPower == 0 || GetActiveItem().isSpecial)
                {
                    flag = true;
                }
            }

            if (!flag)
            {
                // Loop through all selected units
                for (int i = CombatGridManager.Instance.targetedCombatSlots.Count - 1; i >= 0; i--)
                {
                    bool parrying = false;

                    WeaponManager.Instance.CalculatePower(isSkillsMode);
                    power = (int)WeaponManager.Instance.calculatedPower;

                    UnitFunctionality unit = null;

                    if (CombatGridManager.Instance.targetedCombatSlots[i].GetLinkedUnit())
                    {
                        unit = CombatGridManager.Instance.targetedCombatSlots[i].GetLinkedUnit();
                    }
                    else
                        continue;

                    if (miss)
                    {
                        power = 0;
                        break;
                    }

                    int originalPower = power;

                    // Helps catch the null error
                    if (i < CombatGridManager.Instance.targetedCombatSlots.Count && unit)
                    {
                        if (unit == null)
                            continue;

                        if (unit.isDead)
                            continue;

                        //if (unit.IsSelected())
                          //  continue;
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

                    int orderCount;

                    bool blocked = false;

                    float targetBlockChance = Random.Range(0, 101);
                    if (unit)
                    {
                        if (unit.GetBlockChance() >= targetBlockChance)
                            blocked = true;
                        else
                            blocked = false;
                    }

                    if (unit)
                    {
                        // Cause power
                        // Skill 
                        if (isSkillsMode)
                        {
                            if (GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE)
                            {
                                orderCount = 2;
                                if (hasBeenLuckyHit)
                                {
                                    hasBeenLuckyHit = false;
                                    orderCount--;
                                }

                                if (blocked)
                                {
                                    newPower = 0;
                                }
                                unit.UpdateUnitCurHealth((int)newPower, true, false, true, true, false);
                                //unitsSelected[i].StartCoroutine(unitsSelected[i].SpawnPowerUI((int)newPower, false, true, null, blocked));

                                CheckAttackForItem(unit, GetActiveUnitFunctionality(), (int)newPower, x, orderCount);
                            }
                            else if (GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT)
                            {
                                orderCount = 2;
                                if (hasBeenLuckyHit)
                                {
                                    hasBeenLuckyHit = false;
                                    orderCount--;
                                }

                                float finalHealingPower = newHealingPower * unit.curHealingRecieved;
                                unit.UpdateUnitCurHealth((int)finalHealingPower, false, false, true, true, false);
                            }
                        }
                        // Items
                        else
                        {
                            if (GetActiveItem().curItemType == ItemPiece.ItemType.OFFENSE)
                            {
                                orderCount = 2;
                                if (hasBeenLuckyHit)
                                {
                                    hasBeenLuckyHit = false;
                                    orderCount--;
                                }

                                if (blocked)
                                {
                                    newPower = 0;
                                }
                                unit.UpdateUnitCurHealth((int)newPower, true, false, true, true, false);
                                //unitsSelected[i].StartCoroutine(unitsSelected[i].SpawnPowerUI((int)newPower, false, true, null, blocked));

                                CheckAttackForItem(unit, GetActiveUnitFunctionality(), (int)newPower, x, orderCount);
                            }
                            else if (GetActiveItem().curItemType == ItemPiece.ItemType.SUPPORT)
                            {
                                orderCount = 2;
                                if (hasBeenLuckyHit)
                                {
                                    hasBeenLuckyHit = false;
                                    orderCount--;
                                }

                                float finalHealingPower = newHealingPower * unit.curHealingRecieved;
                                unit.UpdateUnitCurHealth((int)finalHealingPower, false, false, true, true, false);
                            }
                        }

                        if (isSkillsMode)
                        {
                            // If active skill has an effect AND it's not a self cast, apply it to selected targets
                            if (GetActiveSkill().effect != null && !GetActiveSkill().isSelfCast && !miss)
                            {
                                if (isSkillsMode)
                                    unit.AddUnitEffect(GetActiveSkill().effect, unit, effectHitAcc, effectHitAcc, false, false, false);
                                else


                                if (isSkillsMode && GetActiveSkill().effect2 != null)
                                {
                                    if (GetActiveSkill().effect2.curEffectName != EffectData.EffectName.OTHER_LINK)
                                        unit.AddUnitEffect(GetActiveSkill().effect2, unit, effectHitAcc, effectHitAcc, false, false, false);
                                }
                            }
                        }
                        else
                        {
                            unit.AddUnitEffect(GetActiveItem().effectAdded, unit, effectHitAcc, effectHitAcc, true, false, true, GetActiveItemSlot().linkedSlot);
                        }
                    }


#if !UNITY_EDITOR
                    Vibration.Vibrate(15);
#endif
                }
            }

            // Time wait in between attacks, shared across all targeted units
            int maxHitWorth = 15;
            if (hitCount > maxHitWorth)
                yield return new WaitForSeconds(timeBetweenPowerUIStack - (0.0025f * (maxHitWorth - 1)));
            else
                yield return new WaitForSeconds(timeBetweenPowerUIStack - (0.0025f * (hitCount - 1)));
        }

        if (isSkillsMode)
        {
            // Reset each units power UI
            for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
            {
                activeRoomAllUnitFunctionalitys[i].ResetPowerUI();
                activeRoomAllUnitFunctionalitys[i].usedSkill = null;
                activeRoomAllUnitFunctionalitys[i].effectAddedCount = 0;
                activeRoomAllUnitFunctionalitys[i].settingUpEffect = false;
            }

            StartCoroutine(WeaponAttackCommand2(miss, effectHitAcc));
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
        UpdateMainIconDetails(GetActiveUnitFunctionality().GetNoCDSkill());
        UpdateAllSkillIconAvailability();
        //UpdateUnitSelection(GetActiveUnitFunctionality().GetNoCDSkill());
        //UpdateUnitsSelectedText();
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
        for (int i = 0; i < activeRoomHeroes.Count; i++)
        {
            activeRoomHeroes[i].ToggleUnitHealthBar(toggle);
            activeRoomHeroes[i].ToggleUnitAttackBar(toggle);
            activeRoomHeroes[i].ToggleActionNextBar(toggle);
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

    public void DisableAllMainSlotSelections(bool igNoreMainSlot1 = false)
    {
        if (igNoreMainSlot1)
        {
            mainSlot2.ToggleSelected(false);
            mainSlot3.ToggleSelected(false);
            mainSlot4.ToggleSelected(false);
        }
        else
        {
            mainSlot1.ToggleSelected(false);
            mainSlot2.ToggleSelected(false);
            mainSlot3.ToggleSelected(false);
            mainSlot4.ToggleSelected(false);
        }
    }


    public void EnableFirstMainSlotSelection(bool skills = true)
    {
        if (skills)
        {
            GameManager.Instance.fighterMainSlot1.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
            GameManager.Instance.fighterMainSlot2.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
            GameManager.Instance.fighterMainSlot3.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
            GameManager.Instance.fighterMainSlot4.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());

            GameManager.Instance.UpdateAllSkillIconAvailability();
            SkillsTabManager.Instance.UpdateLockedSkills();

            if (SkillsTabManager.Instance.skillBase1.GetIsLocked())
                fighterMainSlot1.ToggleHiddenImage(true);
            else
                fighterMainSlot1.ToggleHiddenImage(false);

            if (SkillsTabManager.Instance.skillBase2.GetIsLocked())
                fighterMainSlot2.ToggleHiddenImage(true);
            else
                fighterMainSlot2.ToggleHiddenImage(false);

            if (SkillsTabManager.Instance.skillBase3.GetIsLocked())
                fighterMainSlot3.ToggleHiddenImage(true);
            else
                fighterMainSlot3.ToggleHiddenImage(false);

            if (SkillsTabManager.Instance.skillBase4.GetIsLocked())
                fighterMainSlot4.ToggleHiddenImage(true);
            else
                fighterMainSlot4.ToggleHiddenImage(false);


            if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                // todo work with new locked skills
                for (int i = 0; i < GetActiveUnitFunctionality().GetAllSkills().Count; i++)
                {
                    // If Skill has NO cooldown, and IS NOT locked, select the first one it finds
                    if (GetActiveUnitFunctionality().GetSkillCurCooldown(GetActiveUnitFunctionality().GetSkill(0)) == 0 && !SkillsTabManager.Instance.skillBase1.GetIsLocked())
                    {
                        mainSlot1.ToggleSelected(true);

                        //UpdateActiveSkill(GetActiveUnitFunctionality().GetAllSkills()[0]);
                        UpdateMainIconDetails(GetActiveUnitFunctionality().GetAllSkills()[0]);

                        break;
                    }
                    else if (GetActiveUnitFunctionality().GetSkillCurCooldown(GetActiveUnitFunctionality().GetSkill(1)) == 0 && !SkillsTabManager.Instance.skillBase2.GetIsLocked())
                    {
                        mainSlot2.ToggleSelected(true);

                        //UpdateActiveSkill(GetActiveUnitFunctionality().GetAllSkills()[1]);
                        UpdateMainIconDetails(GetActiveUnitFunctionality().GetAllSkills()[1]);

                        break;
                    }
                    else if (GetActiveUnitFunctionality().GetSkillCurCooldown(GetActiveUnitFunctionality().GetSkill(2)) == 0 && !SkillsTabManager.Instance.skillBase3.GetIsLocked())
                    {
                        mainSlot3.ToggleSelected(true);

                        //UpdateActiveSkill(GetActiveUnitFunctionality().GetAllSkills()[2]);
                        UpdateMainIconDetails(GetActiveUnitFunctionality().GetAllSkills()[2]);

                        break;
                    }
                    else if (GetActiveUnitFunctionality().GetSkillCurCooldown(GetActiveUnitFunctionality().GetSkill(3)) == 0 && !SkillsTabManager.Instance.skillBase4.GetIsLocked())
                    {
                        mainSlot4.ToggleSelected(true);

                        //UpdateActiveSkill(GetActiveUnitFunctionality().GetAllSkills()[3]);
                        UpdateMainIconDetails(GetActiveUnitFunctionality().GetAllSkills()[3]);

                        break;
                    }
                }
            }
            
        }
        // Items
        else
        {
            TeamItemsManager.Instance.UpdateLockedItems();

            fighterMainSlot1.ToggleSkillCooldownUI(false);
            fighterMainSlot2.ToggleSkillCooldownUI(false);
            fighterMainSlot3.ToggleSkillCooldownUI(false);
            fighterMainSlot4.ToggleSkillCooldownUI(false);

            fighterMainSlot1.ToggleHiddenImage(false);
            fighterMainSlot2.ToggleHiddenImage(false);
            fighterMainSlot3.ToggleHiddenImage(false);
            fighterMainSlot4.ToggleHiddenImage(false);

            // todo work with new locked skills
            for (int i = 0; i < activeRoomHeroes.Count; i++)
            {
                // Main
                if (i == 0 && GetActiveUnitFunctionality() == activeRoomHeroes[i])
                {
                    for (int x = 0; x < TeamItemsManager.Instance.equippedItemsMain.Count; x++)
                    {
                        // If Skill has NO cooldown, and IS NOT locked, select the first one it finds
                        if (TeamItemsManager.Instance.equippedItemsMain[x] != null)
                        {
                            if (x == 0) 
                                mainSlot1.ToggleSelected(true);
                            else if (x == 1)
                                mainSlot2.ToggleSelected(true);
                            else if (x == 2)
                                mainSlot3.ToggleSelected(true);
                            else if (x == 3)
                                mainSlot4.ToggleSelected(true);

                            //if (OwnedLootInven.Instance.GetWornItemMainAlly()[x].GetItemUseCount() < TeamItemsManager.Instance.equippedItemsMain[x].maxUsesPerCombat)
                                //UpdateActiveItem(TeamItemsManager.Instance.equippedItemsMain[x]);
                           // else
                            //{
                                //GameManager.Instance.UpdateUnitSelection(null, null);
                                UpdateActiveItem(null);
                           // }
                            UpdateMainIconDetails(null, TeamItemsManager.Instance.equippedItemsMain[x]);
                            //UpdateUnitSelection(null, TeamItemsManager.Instance.equippedItemsMain[x]);
                            //UpdateUnitsSelectedText();
                            break;
                        }
                    }

                    break;
                }

                // Second
                else if (i == 1 && GetActiveUnitFunctionality() == activeRoomHeroes[i])
                {
                    for (int x = 0; x < TeamItemsManager.Instance.equippedItemsSecond.Count; x++)
                    {
                        // If Skill has NO cooldown, and IS NOT locked, select the first one it finds
                        if (TeamItemsManager.Instance.equippedItemsSecond[x] != null)
                        {
                            if (x == 0)
                                mainSlot1.ToggleSelected(true);
                            else if (x == 1)
                                mainSlot2.ToggleSelected(true);
                            else if (x == 2)
                                mainSlot3.ToggleSelected(true);
                            else if (x == 3)
                                mainSlot4.ToggleSelected(true);

                            //if (OwnedLootInven.Instance.GetWornItemSecondAlly()[x].GetItemUseCount() < TeamItemsManager.Instance.equippedItemsSecond[x].maxUsesPerCombat)
                            //    UpdateActiveItem(TeamItemsManager.Instance.equippedItemsSecond[x]);
                            //else
                           // {
                                //GameManager.Instance.UpdateUnitSelection(null, null);
                                UpdateActiveItem(null);
                           // }
                            UpdateMainIconDetails(null, TeamItemsManager.Instance.equippedItemsSecond[x]);
                            //UpdateUnitSelection(null, TeamItemsManager.Instance.equippedItemsSecond[x]);
                            //UpdateUnitsSelectedText();
                            break;
                        }
                    }

                    break;
                }

                // Third
                else if (i == 2 && GetActiveUnitFunctionality() == activeRoomHeroes[i])
                {
                    for (int x = 0; x < TeamItemsManager.Instance.equippedItemsThird.Count; x++)
                    {
                        // If Skill has NO cooldown, and IS NOT locked, select the first one it finds
                        if (TeamItemsManager.Instance.equippedItemsThird[x] != null)
                        {
                            if (x == 0)
                                mainSlot1.ToggleSelected(true);
                            else if (x == 1)
                                mainSlot2.ToggleSelected(true);
                            else if (x == 2)
                                mainSlot3.ToggleSelected(true);
                            else if (x == 3)
                                mainSlot4.ToggleSelected(true);

                           // if (OwnedLootInven.Instance.GetWornItemThirdAlly()[x].GetItemUseCount() < TeamItemsManager.Instance.equippedItemsThird[x].maxUsesPerCombat)
                            //    UpdateActiveItem(TeamItemsManager.Instance.equippedItemsThird[x]);
                            //else
                           // {
                                //GameManager.Instance.UpdateUnitSelection(null, null);
                                UpdateActiveItem(null);
                           // }

                            UpdateMainIconDetails(null, TeamItemsManager.Instance.equippedItemsThird[x]);
                            //UpdateUnitSelection(null, TeamItemsManager.Instance.equippedItemsThird[x]);
                            //UpdateUnitsSelectedText();
                            break;
                        }
                    }

                    break;
                }
            }
        }
    }

    public void ToggleMainSlotVisibility(bool toggle)
    {
        mainSlot1.GetComponent<CanvasGroup>().interactable = toggle;
        mainSlot1.GetComponent<CanvasGroup>().blocksRaycasts = toggle;
        mainSlot2.GetComponent<CanvasGroup>().interactable = toggle;
        mainSlot2.GetComponent<CanvasGroup>().blocksRaycasts = toggle;
        mainSlot3.GetComponent<CanvasGroup>().interactable = toggle;
        mainSlot3.GetComponent<CanvasGroup>().blocksRaycasts = toggle;
        mainSlot4.GetComponent<CanvasGroup>().interactable = toggle;
        mainSlot4.GetComponent<CanvasGroup>().blocksRaycasts = toggle;

        if (toggle)
        {
            mainSlot1.GetComponent<CanvasGroup>().alpha = 1;
            mainSlot2.GetComponent<CanvasGroup>().alpha = 1;
            mainSlot3.GetComponent<CanvasGroup>().alpha = 1;
            mainSlot4.GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            mainSlot1.GetComponent<CanvasGroup>().alpha = 0;
            mainSlot2.GetComponent<CanvasGroup>().alpha = 0;
            mainSlot3.GetComponent<CanvasGroup>().alpha = 0;
            mainSlot4.GetComponent<CanvasGroup>().alpha = 0;
        }
    }
    #endregion

    public void UpdateMainIconDetails(SkillData skill = null, ItemPiece item = null, bool displaySkillDesc = false)
    {
        if (!displaySkillDesc)
            ToggleUIElement(fighterSelectedMainSlotDesc, true);

        UnitFunctionality activeUnit = GetActiveUnitFunctionality();

        // Toggle button / display for skill stats
        if (skill != null)
        {
            bool tempAttack = false;

            if (skill.curSkillType == SkillData.SkillType.OFFENSE)
                tempAttack = true;
            else
                tempAttack = false;

            abilityDetailsUI.ToggleAllStats(true);
        }
        else if (item != null)
            abilityDetailsUI.ToggleAllStats(true, false);

        if (skill != null)
        {
            if (skill.curSkillType == SkillData.SkillType.OFFENSE)
            {
                abilityDetailsUI.UpdateSkillUI(skill.skillName, skill.skillDescr, skill.GetCalculatedSkillPowerStat(),
                    skill.GetCalculatedSkillHitAmount() + activeUnit.GetUnitPowerHits(), skill.GetCalculatedSkillSelectionCount(),
                    skill.GetCalculatedSkillPowerStat(), skill.skillCooldown, skill.skillHitAttempts, skill.GetCalculatedSkillEffectStat(), skill.skillPowerIcon, skill.skillSprite, skill.special);
            }
            else
            {
                abilityDetailsUI.UpdateSkillUI(skill.skillName, skill.skillDescr, skill.GetCalculatedSkillPowerStat(),
                    skill.GetCalculatedSkillHitAmount() + activeUnit.GetUnitHealingHits(), skill.GetCalculatedSkillSelectionCount(),
                    skill.GetCalculatedSkillPowerStat(), skill.skillCooldown, skill.skillHitAttempts, skill.GetCalculatedSkillEffectStat(), skill.skillPowerIcon, skill.skillSprite, skill.special);
            }
        }
        else if (skill == null)
        {
            if (item == null)
            {
                abilityDetailsUI.UpdateItemUI("", "", 0, 0, TeamItemsManager.Instance.clearSlotSprite);
            }
            else
            {
                int newPower = item.itemPower;
                if (item.curHitType == ItemPiece.HitType.HITS)
                    newPower = GetActiveUnitFunctionality().curPower + 10;

                abilityDetailsUI.UpdateItemUI(item.itemName, item.itemDesc, newPower, item.targetCount, item.itemSpriteCombat);
            }
        }
    }

    public void RemoveUnitFromTurnOrder(UnitFunctionality unit)
    {
        if (activeRoomAllUnitFunctionalitys.Contains(unit))
            activeRoomAllUnitFunctionalitys.Remove(unit);

        if (!unit.GetEffect("REANIMATE"))
        {
            AddExperienceGained(unit.GetUnitExpKillGained());
            UpdateEnemiesKilled(unit);
        }
    }

    public void AddUnitToTurnOrder(UnitFunctionality unit)
    {
        activeRoomAllUnitFunctionalitys.Add(unit);
    }

    public void RemoveUnit(UnitFunctionality unitFunctionality)
    {
        if (activeRoomEnemies.Contains(unitFunctionality))
            activeRoomEnemies.Remove(unitFunctionality);



        if (activeRoomAllUnitFunctionalitys.Contains(unitFunctionality))
            activeRoomAllUnitFunctionalitys.Remove(unitFunctionality);

        for (int i = 0; i < fallenHeroes.Count; i++)
        {
            for (int x = 0; x < activeTeam.Count; x++)
            {
                if (fallenHeroes[i].GetUnitName() == activeTeam[x].unitName && fallenHeroes[i].isDead)
                {
                    //Debug.Log(activeTeam[i].unitName + "removing i = " + i);
                    activeTeam.Remove(activeTeam[x]);
                }
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

        CombatGridManager.Instance.UnselectAllSelectedCombatSlots();
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
                if (activeRoomAllUnitFunctionalitys[i].GetEffects()[x].curEffectName == Effect.EffectName.SPEEDUP || activeRoomAllUnitFunctionalitys[i].GetEffects()[x].curEffectName == Effect.EffectName.SPEEDDOWN)
                {
                    /*
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
                    */

                    if (inc)
                        activeRoomAllUnitFunctionalitys[i].ToggleIsSpeedUp(true);
                    else
                        activeRoomAllUnitFunctionalitys[i].ToggleIsSpeedDown(true);

                    // units current speed
                    float newSpeed = 0;
                    float curSpeed = activeRoomAllUnitFunctionalitys[i].startingRoomSpeed;

                    newSpeed = ((activeRoomAllUnitFunctionalitys[i].GetEffects()[x].powerPercent / 100f) * curSpeed);

                    //Debug.Log("Speed Added or minused = " + newSpeed);
                    // units new speed
                    //activeRoomAllUnitFunctionalitys[i].UpdateUnitOldSpeed((int)curSpeed);

                    // updating new speed
                    if (inc)
                        activeRoomAllUnitFunctionalitys[i].UpdateUnitSpeed((int)newSpeed, false);
                    else
                        activeRoomAllUnitFunctionalitys[i].UpdateUnitSpeed((int)-newSpeed, false);

                    activeRoomAllUnitFunctionalitys[i].CalculateUnitAttackChargeTurnStart();
                    activeRoomAllUnitFunctionalitys[i].UpdateUnitAttackBarNextVisual();
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
                        /*
                        if (activeRoomAllUnitFunctionalitys[i].GetIsDefenseUp())
                            break;
                        */

                        activeRoomAllUnitFunctionalitys[i].ToggleIsDefenseUp(true);
                        
                        // units current def
                        float curDef = activeRoomAllUnitFunctionalitys[i].startingRoomDefense;
                        // units new def
                        float newDef = (activeRoomAllUnitFunctionalitys[i].GetEffects()[x].powerPercent / 100f) * curDef;
                        newDef += 4;

                        Debug.Log("new defense added = " + newDef);
                        //activeRoomAllUnitFunctionalitys[i].UpdateUnitOldDefense((int)curDef);

                        // updating new defense
                        activeRoomAllUnitFunctionalitys[i].UpdateUnitDefense((int)newDef, false);
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
                        //if (activeRoomAllUnitFunctionalitys[i].GetIsDefenseDown())
                        //    break;

                        /*
                        activeRoomAllUnitFunctionalitys[i].ToggleIsDefenseDown(true);

                        // units current def
                        float curDef = activeRoomAllUnitFunctionalitys[i].startingRoomDefense;
                        // units new def
                        float newDef = curDef + ((activeRoomAllUnitFunctionalitys[i].GetEffects()[x].powerPercent / 100f)) * curDef;

                        // updating new defense
                        activeRoomAllUnitFunctionalitys[i].UpdateUnitDefense((int)-newDef, false);
                        break;
                        */
                        break;
                    }
                }
            }
        }
    }

    public void RemoveSpeedUpEffectUnit(UnitFunctionality unit)
    {
        unit.UpdateUnitSpeed(unit.startingRoomSpeed);
        //unit.ResetOldSpeed();
        unit.ToggleIsSpeedUp(false);
    }
    public void RemoveSpeedDownEffectUnit(UnitFunctionality unit)
    {
        unit.UpdateUnitSpeed(unit.startingRoomSpeed);
        //unit.ResetOldSpeed();
        unit.ToggleIsSpeedDown(false);

        // Update unit attack bar visual when speed down expires
        unit.CalculateUnitAttackChargeTurnStart();
        unit.UpdateUnitAttackBarNextVisual();
    }

    public void RemoveDefenseUpEffectUnit(UnitFunctionality unit)
    {
        unit.UpdateUnitDefense((int)unit.startingRoomDefense);
        //unit.ResetOldDefense();
        unit.ToggleIsDefenseUp(false);
    }

    public void RemoveDefenseDownEffectUnit(UnitFunctionality unit)
    {
        unit.UpdateUnitDefense((int)unit.GetOldDefense());
        //unit.ResetOldDefense();
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
        for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
        {
            if (onlyEnemies)
            {
                if (activeRoomAllUnitFunctionalitys[i].curUnitType == UnitFunctionality.UnitType.ENEMY)
                    activeRoomAllUnitFunctionalitys[i].ToggleHideEffects(false);
            }
            else
                activeRoomAllUnitFunctionalitys[i].ToggleHideEffects(false);
        }
    }

    IEnumerator PlayerLostWait()
    {
        yield return new WaitForSeconds(1.55f);

        StartCoroutine(SetupPostBattleUI(playerWon));
    }

    public bool CheckToEndCombat()
    {
        #region Check if Player Team Won or Lost, Then end Battle

        int allyCount = 0;
        int enemyCount = 0;
        for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
        {
            if (activeRoomAllUnitFunctionalitys[i].curUnitType == UnitFunctionality.UnitType.PLAYER)
                allyCount++;
            else
                enemyCount++;

            activeRoomAllUnitFunctionalitys[i].hitPerfect = false;
        }

        if (allyCount == 0)
        {
            CombatGridManager.Instance.ToggleCombatGrid(false);

            playerWon = false;
            StartCoroutine(PlayerLostWait());
            return true;
        }
        else if (enemyCount == 0)
        {
            CombatGridManager.Instance.ToggleCombatGrid(false);
            playerWon = true;

            for (int i = 0; i < activeRoomHeroes.Count; i++)
            {
                if (activeRoomHeroes[i].reanimated)
                {
                    activeRoomHeroes[i].UpdateUnitCurHealth(9999, true, false, true, false, true);
                }
            }

            // Disable unit buttons if their dead so 3rd slot hero can be selectable
            for (int i = 0; i < fallenHeroes.Count; i++)
            {
                //Debug.Log("asdasdasdasdasd");
                fallenHeroes[i].DisableDeadUnitButtons();
            }
            for (int i = 0; i < fallenEnemies.Count; i++)
            {
                //Debug.Log("asdasdasdasdasd");
                fallenEnemies[i].DisableDeadUnitButtons();
            }

            if (RoomManager.Instance.GetActiveRoom().curRoomType == RoomMapIcon.RoomType.ITEM || RoomManager.Instance.GetActiveRoom().curRoomType == RoomMapIcon.RoomType.BOSS)
            {
                HideMainSlotDetails();
                ResetFallenEnemies();
                SetupItemRewards();
            }
            else if (RoomManager.Instance.GetActiveRoom().curRoomType == RoomMapIcon.RoomType.HERO)
            {
                ResetFallenEnemies();
                HeroRoomManager.Instance.SpawnHeroGameManager();
            }
            else if (RoomManager.Instance.GetActiveRoom().curRoomType == RoomMapIcon.RoomType.ENEMY)
            {
                ResetFallenEnemies();
                StartCoroutine(SetupPostBattleUI(true));
            }

            HideMainSlotDetails();

            return true;
        }
        else
        {
            return false;
        }

        #endregion
    }
    public void UpdateTurnOrder()
    {
        CombatGridManager.Instance.ToggleAllCombatSlotOutlines();

        // Ensure combat tab is set as skills mode by default, instead of left from items tab from prev fighter turn
        CombatGridManager.Instance.ToggleTabButtons("Skills");
        CombatGridManager.Instance.ToggleTabButtons("Movement");

        if (combatOver)
            return;

        CombatGridManager.Instance.DisableAllButtons();

        //isSkillsMode = true;


        //Debug.Log("updated turn order");
        if (CheckToEndCombat())
            return;

        HideMainSlotDetails();

        abilityDetailsUI.ToggleFighterDetailsTab(true);

        ToggleUnitEffectTooltipsOff(true);
        //ToggleSkillVisibility(false);

        ToggleUIElement(turnOrder, true);   // Enable turn order UI

        ResetSelectedUnits();
        ToggleSelectingUnits(true);
        ToggleAllowSelection(true);

        UpdateAllSkillIconAvailability();   // Update active unit's skill cooldowns to toggle 'On Cooldown' before switching to new active unit

        // Turn end
        GetActiveUnitFunctionality().StartCoroutine(GetActiveUnitFunctionality().DecreaseEffectTurnsLeft(false));

        GetActiveUnitFunctionality().ToggleUnitMoveActiveArrows(false);

        for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
        {
            activeRoomAllUnitFunctionalitys[i].UpdateUnitLookDirection();

            if (activeRoomAllUnitFunctionalitys[i].GetEffect("IMMUNITY"))
            {
                if (activeRoomAllUnitFunctionalitys[i].beenAttacked)
                {
                    StartCoroutine(activeRoomAllUnitFunctionalitys[i].DecreaseEffectTurnsLeft(false, false, true));
                }
            }
        }

        for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
        {
            activeRoomAllUnitFunctionalitys[i].beenAttacked = false;
        }

        //GetActiveUnitFunctionality().ToggleTextAlert(false);

        GetActiveUnitFunctionality().skillRangeIssue = false;

        DetermineTurnOrder();

        GetActiveUnitFunctionality().skillRangeIssue = false;

        //UpdateActiveSkill(GetActiveUnitFunctionality().GetBaseSelectSkill());

        GetActiveUnitFunctionality().CheckSwitchTeams();

        /*
        if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER && GetActiveUnitFunctionality().isDead)
        {
            UpdateTurnOrder();
            //combatOver = true;
            return;
        }
        */

        // Only decrease skill CDs during combat
        GetActiveUnitFunctionality().DecreaseSkill0Cooldown();
        GetActiveUnitFunctionality().DecreaseSkill1Cooldown();
        GetActiveUnitFunctionality().DecreaseSkill2Cooldown();
        GetActiveUnitFunctionality().DecreaseSkill3Cooldown();

        ResetAllUnitPowerUIHeight();

        IncreaseAttackBarAllUnits();
        GetActiveUnitFunctionality().ResetAttackChargeTurnStart();

        UpdateAllSkillIconAvailability();   // Update active unit's skill cooldowns to toggle 'On Cooldown' before switching to new active unit

        GetActiveUnitFunctionality().StartFocusUnit();

        GetActiveUnitFunctionality().ResetMovementUses();
        GetActiveUnitFunctionality().usedExtraMove = false;

        //GetActiveUnitFunctionality().ToggleTextAlert(false);

        CombatGridManager.Instance.ToggleCombatGrid(true);

        CombatGridManager.Instance.CheckToUnlinkCombatSlot();



        // Update unit look direction
        GetActiveUnitFunctionality().UpdateUnitLookDirection();

        // Toggle player UI accordingly if it's their turn or not
        if (activeRoomAllUnitFunctionalitys[0].curUnitType == UnitFunctionality.UnitType.PLAYER)
        {
            OverlayUI.Instance.ToggleSkillItemSwitchButton(true);

            playerUIElement.UpdateAlpha(1);
            SetupPlayerUI();
            SetupPlayerSkillsUI();
            ToggleMainSlotVisibility(true);
            //UpdatePlayerAbilityUI(false, false, true);
            UpdateActiveUnitNameText(GetActiveUnitFunctionality().GetUnitName());

            //ToggleEndTurnButton(true);      // Toggle End Turn Button on

            CombatGridManager.Instance.ToggleButtonAttackMovement(true);
        }
        else
        {
            OverlayUI.Instance.ToggleSkillItemSwitchButton(false);

            HideMainSlotDetails();
            GetActiveUnitFunctionality().ToggleIdleBattle(true);
            playerUIElement.UpdateAlpha(0);

            ToggleEndTurnButton(false);      // Toggle End Turn Button on

            //UpdateEnemyPosition(false);

            CombatGridManager.Instance.ToggleButtonAttackMovement(false);
        }

        // Reset active unit attack charge
        GetActiveUnitFunctionality().ResetUnitCurAttackCharge();
        GetActiveUnitFunctionality().unitData.UpdateUnitCurAttackCharge(GetActiveUnitFunctionality().GetCurAttackCharge());

        if (GetActiveUnitFunctionality().unitData.curUnitType == UnitData.UnitType.PLAYER || GetActiveUnitFunctionality().reanimated)
        {
            //UpdateSkillDetails(activeSkill);

            //UpdateUnitSelection(activeSkill);
           // UpdateEnemyPosition(true);
        }

        UpdateActiveUnitTurnArrow();

        // Update allies into position for battle/shop
        //UpdateAllAlliesPosition(false, GetActiveUnitType());

        if (GetActiveUnitFunctionality().GetEffect("STUN"))
        {
            GetActiveUnitFunctionality().TriggerTextAlert("STUN", 1, true, "Trigger");
            GetActiveUnitFunctionality().StartCoroutine(GetActiveUnitFunctionality().UnitEndTurn());
            return;
        }

        //Trigger Start turn effects
        GetActiveUnitFunctionality().StartCoroutine(GetActiveUnitFunctionality().DecreaseEffectTurnsLeft(true, false));

        CombatGridManager.Instance.isCombatMode = false;
        CombatGridManager.Instance.UpdateCombatMainSlots();

        UpdateDetailsBanner();
        ToggleSkillsItemToggleButton(false);
        UpdatePlayerAbilityUI(false, false, true);
        UpdateMainIconDetails(null, null, false);

        UpdateAllUnitStatBars();
        CombatGridManager.Instance.ToggleIsMovementAllowed(true);
        CombatGridManager.Instance.GetButtonMovement().ButtonCombatMovementTab();
        //CombatGridManager.Instance.GetButtonSkillsItems().ButtonCombatItemsTab(true, true);
        GetActiveUnitFunctionality().hasAttacked = false;
    }

    public void ToggleAllUnitButtons(bool toggle = true)
    {
        for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
        {
            activeRoomAllUnitFunctionalitys[i].ToggleUnitButton(toggle);
        }
    }
    public void ContinueTurnOrder()
    {
        if (!PostBattle.Instance.isInPostBattle)
            GetActiveUnitFunctionality().StartCoroutine(GetActiveUnitFunctionality().TriggerItems(true));

        // Toggle player UI accordingly if it's their turn or not
        if (activeRoomAllUnitFunctionalitys[0].curUnitType == UnitFunctionality.UnitType.ENEMY && !activeRoomAllUnitFunctionalitys[0].reanimated)
        {
            // If no allies or enemies remain, do not resume enemies turn
            int playerCount = 0;
            int enemyCount = 0;
            for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
            {
                if (activeRoomAllUnitFunctionalitys[i].curUnitType == UnitFunctionality.UnitType.PLAYER)
                    playerCount++;

                if (activeRoomAllUnitFunctionalitys[i].curUnitType == UnitFunctionality.UnitType.ENEMY)
                    enemyCount++;
            }

            if (playerCount >= 1 && enemyCount >= 1)
            {
                //Debug.Log("ddd");
                StartCoroutine(activeRoomAllUnitFunctionalitys[0].StartUnitTurn());
                return;
            }
        }
      




        if (CheckSkipUnitTurn(GetActiveUnitFunctionality()))
            StartCoroutine(SkipTurnAfterWait());

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

                    for (int x = 0; x < activeRoomHeroes.Count; x++)
                    {
                        if (activeRoomHeroes[x].curUnitType == UnitFunctionality.UnitType.PLAYER)
                        {
                            if (activeRoomHeroes[x].isDead)
                                deadTargetsRemain = true;
                        }
                    }
                }
            }
        }

        if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
        {
            GetActiveUnitFunctionality().ToggleUnitMoveActiveArrows(true);

            // If skill 0 with only 1 skill slot available, if the skill targets dead allies, and there are none, force skip turn.
            if (!deadTargetsRemain && byPass)
            {
                //StartCoroutine(SkipTurnAfterWait());
            }
        }
    }

    public void UpdateAllUnitStatBars()
    {
        for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
        {
            if (activeRoomAllUnitFunctionalitys[i].DetectIfUnitBelow())
            {
                activeRoomAllUnitFunctionalitys[i].ToggleUnitStatBarAlpha(false);
            }
            else
            {
                activeRoomAllUnitFunctionalitys[i].ToggleUnitStatBarAlpha(true);
            }
        }
    }

    void ResetFallenEnemies()
    {
        fallenEnemies.Clear();
    }

    public bool CheckSkipUnitTurn(UnitFunctionality unitTarget)
    {
        if (!ShopManager.Instance.playerInShopRoom)
        {
            if (unitTarget.curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                // Skip unit turn if all skills are on cooldown
                if (unitTarget.GetSkillCurCooldown(unitTarget.GetSkill(0)) >= 1 || unitTarget.GetSkill(0).isReviving && fallenHeroes.Count == 0 && unitTarget.GetSkill(0).curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.PLAYERS
                    || unitTarget.GetSkill(0).isReviving && fallenEnemies.Count == 0 && unitTarget.GetSkill(0).curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.ENEMIES)
                {
                    if (unitTarget.GetSkillCurCooldown(unitTarget.GetSkill(1)) >= 1 || unitTarget.GetSkill(1).isReviving && fallenHeroes.Count == 0 && unitTarget.GetSkill(0).curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.PLAYERS
                        || unitTarget.GetSkill(0).isReviving && fallenEnemies.Count == 0 && unitTarget.GetSkill(1).curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.ENEMIES || unitTarget.GetUnitLevel() < 3)
                    {
                        if (unitTarget.GetSkillCurCooldown(unitTarget.GetSkill(2)) > 1 || unitTarget.GetSkill(2).isReviving && fallenHeroes.Count == 0 && unitTarget.GetSkill(0).curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.PLAYERS
                            || unitTarget.GetSkill(0).isReviving && fallenEnemies.Count == 0 && unitTarget.GetSkill(2).curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.ENEMIES || unitTarget.GetUnitLevel() < 6)
                        {
                            if (unitTarget.GetSkillCurCooldown(unitTarget.GetSkill(3)) >= 1 || unitTarget.GetSkill(3).isReviving && fallenHeroes.Count == 0 && unitTarget.GetSkill(0).curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.PLAYERS
                                || unitTarget.GetSkill(0).isReviving && fallenEnemies.Count == 0 && unitTarget.GetSkill(3).curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.ENEMIES || unitTarget.GetUnitLevel() < 9)
                            {
                                if (!HeroRoomManager.Instance.playerInHeroRoomView)
                                    return true;
                                else
                                    return false;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

    }
    IEnumerator SkipTurnAfterWait()
    {
        yield return new WaitForSeconds(.75f);

        //Debug.Log("skipping turn after wait");
        //UpdateEnemyPosition(false);
        //UpdateTurnOrder();
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
        for (int i = 0; i < activeRoomHeroes.Count; i++)
        {
            activeRoomHeroes[i].ToggleUnitExpVisual(false);
            activeRoomHeroes[i].StopCoroutine(activeRoomHeroes[i].UpdateUnitExpVisual(0));
        }
    }

    public void UpdateUnitSelection(SkillData usedSkill = null, ItemPiece item = null)
    {
        //Debug.Log("aa selecting unit " + usedSkill.skillName);
        int selectedAmount = 0;

        // Clear all current selections
        ResetSelectedUnits();

        if (usedSkill != null)
        {
            if (usedSkill.curskillSelectionAliveType == SkillData.SkillSelectionAliveType.ALIVE)
            {
                // If skill selection type is only on ENEMIES
                if (usedSkill.curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.ENEMIES)
                {
                    // if the skill user is a PLAYER
                    if (GetActiveUnitFunctionality().unitData.curUnitType == UnitData.UnitType.PLAYER || GetActiveUnitFunctionality().reanimated)
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
                            if (activeRoomAllUnitFunctionalitys[x].curUnitType == UnitFunctionality.UnitType.ENEMY && !activeRoomAllUnitFunctionalitys[x].isDead)
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
                if (usedSkill.curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.PLAYERS)
                {
                    // if the skill user is a PLAYER
                    if (GetActiveUnitFunctionality().unitData.curUnitType == UnitData.UnitType.PLAYER || GetActiveUnitFunctionality().reanimated)
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
                        //Debug.Log("b selecting unit");
                        //Debug.Log("selecting targets");
                        // only select ENEMY units
                        for (int i = 0; i < 25; i++)
                        {
                            if (usedSkill.curSkillType == SkillData.SkillType.SUPPORT && usedSkill.curSkillPower != 0)
                            {
                                int lowestHealthEnemy = 99999;
                                UnitFunctionality target = null;
                                // Choose lowest health target
                                for (int x = 0; x < activeRoomEnemies.Count; x++)
                                {
                                    if (!activeRoomEnemies[x].isDead)
                                    {
                                        if (activeRoomEnemies[x].GetUnitCurHealth() < lowestHealthEnemy)
                                        {
                                            lowestHealthEnemy = (int)activeRoomEnemies[x].GetUnitCurHealth();
                                            target = activeRoomEnemies[x];
                                        }
                                    }
                                }

                                if (target != null)
                                {
                                    targetUnit(target);
                                    break;
                                }
                            }


                            // Choose random target
                            int rand = Random.Range(0, activeRoomAllUnitFunctionalitys.Count);

                            if (activeRoomAllUnitFunctionalitys[rand].curUnitType != UnitFunctionality.UnitType.ENEMY)
                            {
                                if (i > 0)
                                    i--;

                                continue;
                            }
                            else
                            {
                                selectedAmount++;

                                //Debug.Log("b.c selecting unit");

                                // If self cast, cast on self, otherwise, continue for whomever
                                if (usedSkill.isSelfCast)
                                {
                                    targetUnit(GetActiveUnitFunctionality());
                                }
                                else
                                {
                                    //Debug.Log("b.d selecting unit");
                                    if (!activeRoomAllUnitFunctionalitys[rand].IsSelected())
                                    {
                                        //Debug.Log("c selecting unit");
                                        targetUnit(activeRoomAllUnitFunctionalitys[rand]);
                                    }
                                }

                                // If enough units have been selected, toggle the display
                                if (selectedAmount == usedSkill.GetCalculatedSkillSelectionCount())
                                    break;

                                // If skill requires only 1 unit selection - (full team target wont work without this)
                                if (usedSkill.GetCalculatedSkillSelectionCount() == 1)
                                {
                                    /*
                                    // If enemy chooses itself for ally attack, reselect to target any other ally 
                                    for (int a = 0; a < activeRoomAllUnitFunctionalitys.Count; a++)
                                    {
                                        if (activeRoomAllUnitFunctionalitys[a].curUnitType == UnitFunctionality.UnitType.ENEMY)
                                        {
                                            if (activeRoomAllUnitFunctionalitys[rand] == GetActiveUnitFunctionality())
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
                                    */
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
                    if (usedSkill.curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.PLAYERS)
                    {
                        for (int i = 0; i < activeRoomHeroes.Count; i++)
                        {
                            if (activeRoomHeroes[i].curUnitType == UnitFunctionality.UnitType.PLAYER && activeRoomHeroes[i].isDead && !activeRoomEnemies[i].isSelected)
                            {
                                targetUnit(activeRoomHeroes[i]);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < activeRoomEnemies.Count; i++)
                        {
                            if (activeRoomEnemies[i].curUnitType == UnitFunctionality.UnitType.ENEMY && activeRoomEnemies[i].isDead && !activeRoomEnemies[i].isSelected)
                            {
                                targetUnit(activeRoomEnemies[i]);
                            }
                        }
                    }
                }
            }
        }

        // Items
        else if (item != null)
        {
            if (item.curActiveType == ItemPiece.ActiveType.PASSIVE)
                return;

            if (item.curTargetType == ItemPiece.TargetType.ALIVE)
            {
                // If skill selection type is only on ENEMIES
                if (item.curSelectionType == ItemPiece.SelectionType.ENEMIES)
                {
                    // if the skill user is a PLAYER
                    if (GetActiveUnitFunctionality().unitData.curUnitType == UnitData.UnitType.PLAYER || GetActiveUnitFunctionality().reanimated)
                    {
                        // If any enemies are taunting, select them
                        if (IsEnemyTaunting().Count >= 1)
                        {
                            for (int i = IsEnemyTaunting().Count - 1; i >= 0; i--)
                            {
                                selectedAmount++;
                                targetUnit(IsEnemyTaunting()[i]);

                                // If enough units have been selected FOR ability max targets, or max amount of enemy units tanking
                                if (selectedAmount == item.targetCount || selectedAmount == IsEnemyTaunting().Count)
                                    return;
                            }
                        }

                        if (item.curItemTargetType == ItemPiece.ItemTargetType.NORMAL)
                        {
                            // only select the closest ENEMY units
                            for (int x = activeRoomAllUnitFunctionalitys.Count - 1; x >= 0; x--)
                            {
                                if (activeRoomAllUnitFunctionalitys[x].curUnitType == UnitFunctionality.UnitType.ENEMY && !activeRoomAllUnitFunctionalitys[x].isDead)
                                {
                                    // if no enemies are taunting, start selecting
                                    selectedAmount++;
                                    targetUnit(activeRoomAllUnitFunctionalitys[x]);

                                    // If enough units have been selected (in order of closest)
                                    if (selectedAmount == item.targetCount)
                                        return;
                                }
                            }
                        }
                        // If skill item target type is random, target all intended units
                        else
                        {
                            // only select the closest ENEMY units
                            for (int x = activeRoomAllUnitFunctionalitys.Count - 1; x >= 0; x--)
                            {
                                if (activeRoomAllUnitFunctionalitys[x].curUnitType == UnitFunctionality.UnitType.ENEMY && !activeRoomAllUnitFunctionalitys[x].isDead)
                                {
                                    // if no enemies are taunting, start selecting
                                    selectedAmount++;
                                    targetUnit(activeRoomAllUnitFunctionalitys[x]);
                                }
                            }
                        }
                    }
                    // If the skill user is an ENEMY
                    else if (GetActiveUnitFunctionality().unitData.curUnitType == UnitData.UnitType.ENEMY)
                    {
                        if (item.curItemTargetType == ItemPiece.ItemTargetType.NORMAL)
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
                                    if (selectedAmount == item.targetCount)
                                        return;
                                }
                            }
                        }
                        // If skill item target type is random, target all intended units
                        else
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
                                }
                            }
                        }
 
                    }
                }

                // If skill selection type is only on ALLIES
                if (item.curSelectionType == ItemPiece.SelectionType.ALLIES)
                {
                    // if the skill user is a PLAYER
                    if (GetActiveUnitFunctionality().unitData.curUnitType == UnitData.UnitType.PLAYER || GetActiveUnitFunctionality().reanimated)
                    {
                        if (item.curItemTargetType == ItemPiece.ItemTargetType.NORMAL)
                        {
                            // only select PLAYER units
                            for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
                            {
                                if (activeRoomAllUnitFunctionalitys[i].curUnitType == UnitFunctionality.UnitType.PLAYER)
                                {
                                    selectedAmount++;

                                    // If self cast, cast on self, otherwise, continue for whomever
                                    if (item.isSelfCast)
                                        targetUnit(GetActiveUnitFunctionality());
                                    else
                                        targetUnit(activeRoomAllUnitFunctionalitys[i]);

                                    // If enough units have been selected (in order of closest)
                                    if (selectedAmount == item.targetCount)
                                        return;
                                }
                            }
                        }
                        else
                        {
                            // only select PLAYER units
                            for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
                            {
                                if (activeRoomAllUnitFunctionalitys[i].curUnitType == UnitFunctionality.UnitType.PLAYER)
                                {
                                    selectedAmount++;

                                    // If self cast, cast on self, otherwise, continue for whomever
                                    if (item.isSelfCast)
                                        targetUnit(GetActiveUnitFunctionality());
                                    else
                                        targetUnit(activeRoomAllUnitFunctionalitys[i]);
                                }
                            }
                        }
                    }
                    // If the skill user is an ENEMY
                    else if (GetActiveUnitFunctionality().unitData.curUnitType == UnitData.UnitType.ENEMY)
                    {
                        if (item.curItemTargetType == ItemPiece.ItemTargetType.NORMAL)
                        {
                            // only select ENEMY units
                            for (int i = 0; i < 25; i++)
                            {
                                if (item.curItemType == ItemPiece.ItemType.SUPPORT && item.itemPower != 0)
                                {
                                    int lowestHealthEnemy = 99999;
                                    UnitFunctionality target = null;
                                    // Choose lowest health target
                                    for (int x = 0; x < activeRoomEnemies.Count; x++)
                                    {
                                        if (!activeRoomEnemies[x].isDead)
                                        {
                                            if (activeRoomEnemies[x].GetUnitCurHealth() < lowestHealthEnemy)
                                            {
                                                lowestHealthEnemy = (int)activeRoomEnemies[x].GetUnitCurHealth();
                                                target = activeRoomEnemies[x];
                                            }
                                        }
                                    }

                                    if (target != null)
                                    {
                                        targetUnit(target);
                                        break;
                                    }
                                }


                                // Choose random target
                                int rand = Random.Range(0, activeRoomAllUnitFunctionalitys.Count);

                                if (activeRoomAllUnitFunctionalitys[rand].curUnitType != UnitFunctionality.UnitType.ENEMY)
                                {
                                    if (i > 0)
                                        i--;

                                    continue;
                                }
                                else
                                {
                                    selectedAmount++;

                                    //Debug.Log("b.c selecting unit");

                                    // If self cast, cast on self, otherwise, continue for whomever
                                    if (item.isSelfCast)
                                    {
                                        targetUnit(GetActiveUnitFunctionality());
                                    }
                                    else
                                    {
                                        //Debug.Log("b.d selecting unit");
                                        if (!activeRoomAllUnitFunctionalitys[rand].IsSelected())
                                        {
                                            //Debug.Log("c selecting unit");
                                            targetUnit(activeRoomAllUnitFunctionalitys[rand]);
                                        }
                                    }

                                    // If enough units have been selected, toggle the display
                                    if (selectedAmount == activeItem.targetCount)
                                        break;

                                    // If skill requires only 1 unit selection - (full team target wont work without this)
                                    if (item.targetCount == 1)
                                    {

                                    }
                                }
                            }
                        }
                        else
                        {
                            // only select ENEMY units
                            for (int i = 0; i < 25; i++)
                            {
                                if (item.curItemType == ItemPiece.ItemType.SUPPORT && item.itemPower != 0)
                                {
                                    int lowestHealthEnemy = 99999;
                                    UnitFunctionality target = null;
                                    // Choose lowest health target
                                    for (int x = 0; x < activeRoomEnemies.Count; x++)
                                    {
                                        if (!activeRoomEnemies[x].isDead)
                                        {
                                            if (activeRoomEnemies[x].GetUnitCurHealth() < lowestHealthEnemy)
                                            {
                                                lowestHealthEnemy = (int)activeRoomEnemies[x].GetUnitCurHealth();
                                                target = activeRoomEnemies[x];
                                            }
                                        }
                                    }
                                }


                                // Choose random target
                                int rand = Random.Range(0, activeRoomAllUnitFunctionalitys.Count);

                                if (activeRoomAllUnitFunctionalitys[rand].curUnitType != UnitFunctionality.UnitType.ENEMY)
                                {
                                    if (i > 0)
                                        i--;

                                    continue;
                                }
                                else
                                {
                                    selectedAmount++;

                                    //Debug.Log("b.c selecting unit");

                                    // If self cast, cast on self, otherwise, continue for whomever
                                    if (item.isSelfCast)
                                    {
                                        targetUnit(GetActiveUnitFunctionality());
                                    }
                                    else
                                    {
                                        //Debug.Log("b.d selecting unit");
                                        if (!activeRoomAllUnitFunctionalitys[rand].IsSelected())
                                        {
                                            //Debug.Log("c selecting unit");
                                            targetUnit(activeRoomAllUnitFunctionalitys[rand]);
                                        }
                                    }

                                    // If skill requires only 1 unit selection - (full team target wont work without this)
                                    if (item.targetCount == 1)
                                    {

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
                for (int x = 0; x < item.targetCount; x++)
                {
                    // Select dead allies
                    if (item.curSelectionType == ItemPiece.SelectionType.ALLIES)
                    {
                        for (int i = 0; i < activeRoomHeroes.Count; i++)
                        {
                            if (activeRoomHeroes[i].curUnitType == UnitFunctionality.UnitType.PLAYER && activeRoomHeroes[i].isDead && !activeRoomEnemies[i].isSelected)
                            {
                                targetUnit(activeRoomHeroes[i]);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < activeRoomEnemies.Count; i++)
                        {
                            if (activeRoomEnemies[i].curUnitType == UnitFunctionality.UnitType.ENEMY && activeRoomEnemies[i].isDead && !activeRoomEnemies[i].isSelected)
                            {
                                targetUnit(activeRoomEnemies[i]);
                            }
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

    public ItemPiece GetActiveItem()
    {
        return activeItem;
    }

    public UnitFunctionality GetActiveUnitFunctionality()
    {
        if (activeRoomAllUnitFunctionalitys.Count > 0)
            return activeRoomAllUnitFunctionalitys[0];
        else
            return null;
    }

    public UnitFunctionality GetActiveAlly()
    {
        return activeRoomHeroes[0];
    }

    public void ToggleEndTurnButton(bool toggle, bool delay = false)
    {
        if (toggle)
        {
            if (!delay)
                endTurnButtonUI.UpdateAlpha(1);
        }
        else
            endTurnButtonUI.UpdateAlpha(0);

        if (delay)
        {
            StartCoroutine(ToggleEndTurnButtonWait());
        }
    }

    IEnumerator ToggleEndTurnButtonWait()
    {
        yield return new WaitForSeconds(1.35f);

        endTurnButtonUI.UpdateAlpha(1);
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

    public void AddActiveRoomAllUnitsFunctionality(UnitFunctionality unit, bool enemy = false)
    {
        activeRoomAllUnitFunctionalitys.Add(unit);

        if (unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
            activeRoomHeroes.Add(unit);
        else if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY && enemy)
            activeRoomEnemies.Add(unit);

        unit.teamIndex = activeRoomHeroes.Count-1;
        //GameManager.Instance.SetHeroFormation();
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

    public void UpdatePlayerAbilityUI(bool skills = true, bool itemDeleted = false, bool movement = false)
    {
        DisableAllMainSlotSelections();

        if (skills)
        {
            if (!GetActiveUnitFunctionality().skillRangeIssue)
            {
                if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
                {
                    UpdateActiveSkill(GetActiveUnitFunctionality().GetBaseSelectSkill());
                    UpdateMainIconDetails(GetActiveUnitFunctionality().GetBaseSelectSkill());
                }
                else
                {
                    UpdateActiveSkill(GetActiveSkill());
                }
                //UpdateActiveSkill(GetActiveUnitFunctionality().ChooseRandomSkill());
            }

            // Update player skill portraits
            fighterMainSlot1.UpdatePortrait(GetActiveUnitFunctionality().GetSkill(0).skillSprite);
            fighterMainSlot2.UpdatePortrait(GetActiveUnitFunctionality().GetSkill(1).skillSprite);
            fighterMainSlot3.UpdatePortrait(GetActiveUnitFunctionality().GetSkill(2).skillSprite);
            fighterMainSlot4.UpdatePortrait(GetActiveUnitFunctionality().GetSkill(3).skillSprite);

            fighterMainSlot1.ToggleSelectImage(false);
            fighterMainSlot2.ToggleSelectImage(false);
            fighterMainSlot3.ToggleSelectImage(false);
            fighterMainSlot4.ToggleSelectImage(false);

            fighterMainSlot1.UpdateSubText(GetActiveUnitFunctionality().GetSkill(0).curSkillLevel);
            fighterMainSlot2.UpdateSubText(GetActiveUnitFunctionality().GetSkill(1).curSkillLevel);
            fighterMainSlot3.UpdateSubText(GetActiveUnitFunctionality().GetSkill(2).curSkillLevel);
            fighterMainSlot4.UpdateSubText(GetActiveUnitFunctionality().GetSkill(3).curSkillLevel);
            
            // Disable rarity bg for skills + race icon
            fighterMainSlot1.UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);
            fighterMainSlot2.UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);
            fighterMainSlot3.UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);
            fighterMainSlot4.UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);

            fighterMainSlot1.UpdateRarity(IconUI.Rarity.COMMON, true);
            fighterMainSlot2.UpdateRarity(IconUI.Rarity.COMMON, true);
            fighterMainSlot3.UpdateRarity(IconUI.Rarity.COMMON, true);
            fighterMainSlot4.UpdateRarity(IconUI.Rarity.COMMON, true);

            if (GetActiveUnitFunctionality().GetSkill(0).curSkillActiveType == SkillData.SkillActiveType.ACTIVE)
                fighterMainSlot1.UpdatePassiveActiveType(true);
            else
                fighterMainSlot1.UpdatePassiveActiveType(false);

            if (GetActiveUnitFunctionality().GetSkill(1).curSkillActiveType == SkillData.SkillActiveType.ACTIVE)
                fighterMainSlot2.UpdatePassiveActiveType(true);
            else
                fighterMainSlot2.UpdatePassiveActiveType(false);

            if (GetActiveUnitFunctionality().GetSkill(2).curSkillActiveType == SkillData.SkillActiveType.ACTIVE)
                fighterMainSlot3.UpdatePassiveActiveType(true);
            else
                fighterMainSlot3.UpdatePassiveActiveType(false);

            if (GetActiveUnitFunctionality().GetSkill(3).curSkillActiveType == SkillData.SkillActiveType.ACTIVE)
                fighterMainSlot4.UpdatePassiveActiveType(true);
            else
                fighterMainSlot4.UpdatePassiveActiveType(false);

            //UpdateUnitSelection(activeSkill);
            //UpdateUnitsSelectedText();

            EnableFirstMainSlotSelection(true);
        }
        // Display Items
        else
        {
            fighterMainSlot1.UpdateRarity(IconUI.Rarity.COMMON, true);
            fighterMainSlot2.UpdateRarity(IconUI.Rarity.COMMON, true);
            fighterMainSlot3.UpdateRarity(IconUI.Rarity.COMMON, true);
            fighterMainSlot4.UpdateRarity(IconUI.Rarity.COMMON, true);

            if (GetActiveUnitFunctionality().teamIndex == 0)
            {
                if (TeamItemsManager.Instance.equippedItemsMain.Count > 0)
                {
                    if (OwnedLootInven.Instance.GetWornItemMainAlly()[0].GetCalculatedItemsUsesRemaining2() > 0)
                    {
                        UpdateActiveItem(OwnedLootInven.Instance.GetWornItemMainAlly()[0].linkedItemPiece);
                        UpdateActiveItemSlot(OwnedLootInven.Instance.GetWornItemMainAlly()[0]);
                    }
                    else
                        UpdateActiveItem(null);
                }
            }
            else if (GetActiveUnitFunctionality().teamIndex == 1)
            {
                if (TeamItemsManager.Instance.equippedItemsSecond.Count > 0)
                {
                    if (OwnedLootInven.Instance.GetWornItemSecondAlly()[0].GetCalculatedItemsUsesRemaining2() > 0)
                    {
                        UpdateActiveItem(OwnedLootInven.Instance.GetWornItemSecondAlly()[0].linkedItemPiece);
                        UpdateActiveItemSlot(OwnedLootInven.Instance.GetWornItemSecondAlly()[0]);
                    }
                    else
                        UpdateActiveItem(null);
                }
            }
            else if (GetActiveUnitFunctionality().teamIndex == 2)
            {
                if (TeamItemsManager.Instance.equippedItemsThird.Count > 0)
                {
                    if (OwnedLootInven.Instance.GetWornItemThirdAlly()[0].GetCalculatedItemsUsesRemaining2() > 0)
                    {
                        UpdateActiveItem(OwnedLootInven.Instance.GetWornItemThirdAlly()[0].linkedItemPiece);
                        UpdateActiveItemSlot(OwnedLootInven.Instance.GetWornItemThirdAlly()[0]);
                    }
                    else
                        UpdateActiveItem(null);
                }
            }

            // Update player skill portraits
            for (int i = 0; i < 4; i++)
            {
                if (i == 0)
                {
                    for (int x = 0; x < activeTeam.Count; x++)
                    {
                        // Main fighter
                        if (activeTeam[x].unitName == GetActiveUnitFunctionality().GetUnitName())
                        {
                            if (x == 0)
                            {
                                if (OwnedLootInven.Instance.GetWornItemMainAlly().Count > 0)
                                {
                                    if (OwnedLootInven.Instance.GetWornItemMainAlly()[0].GetCalculatedItemsUsesRemaining2() > 0)
                                    {
                                        fighterMainSlot1.UpdateMainIconBGColour(OwnedLootInven.Instance.GetOtherSlotBGColour());

                                        UpdateMainIconDetails(null, OwnedLootInven.Instance.GetWornItemMainAlly()[0].linkedItemPiece);
                                        fighterMainSlot1.UpdatePortrait(OwnedLootInven.Instance.GetWornItemMainAlly()[0].linkedItemPiece.itemSpriteCombat);
                                        fighterMainSlot1.UpdateItemName(OwnedLootInven.Instance.GetWornItemMainAlly()[0].linkedItemPiece.itemName);

                                        ItemPiece itemPiece = OwnedLootInven.Instance.GetWornItemMainAlly()[0].linkedItemPiece;

                                        if (itemPiece.curRarity == ItemPiece.Rarity.COMMON)
                                            fighterMainSlot1.UpdateRarity(IconUI.Rarity.COMMON);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.RARE)
                                            fighterMainSlot1.UpdateRarity(IconUI.Rarity.RARE);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.EPIC)
                                            fighterMainSlot1.UpdateRarity(IconUI.Rarity.EPIC);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.LEGENDARY)
                                            fighterMainSlot1.UpdateRarity(IconUI.Rarity.LEGENDARY);

                                        if (itemPiece.curRace == ItemPiece.RaceSpecific.HUMAN)
                                            fighterMainSlot1.UpdateRaceIcon(humanRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.BEAST)
                                            fighterMainSlot1.UpdateRaceIcon(beastRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.ETHEREAL)
                                            fighterMainSlot1.UpdateRaceIcon(humanRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.ALL)
                                            fighterMainSlot1.UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);

                                        if (OwnedLootInven.Instance.GetWornItemMainAlly()[0].linkedItemPiece.curActiveType == ItemPiece.ActiveType.ACTIVE)
                                            fighterMainSlot1.UpdatePassiveActiveType(true);
                                        else
                                            fighterMainSlot1.UpdatePassiveActiveType(false);
                                    }
                                    else
                                    {
                                        fighterMainSlot1.UpdatePortrait(TeamItemsManager.Instance.clearSlotSprite);
                                        fighterMainSlot1.UpdatePassiveActiveType(false, true);
                                        fighterMainSlot1.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
                                    }

                                    break;
                                }
                                else
                                {
                                    fighterMainSlot1.UpdatePortrait(TeamItemsManager.Instance.clearSlotSprite);
                                    fighterMainSlot1.UpdatePassiveActiveType(false, true);
                                    fighterMainSlot1.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
                                }
                            }
                            else if (x == 1)
                            {
                                if (OwnedLootInven.Instance.GetWornItemSecondAlly().Count > 0)
                                {
                                    if (OwnedLootInven.Instance.GetWornItemSecondAlly()[0].GetCalculatedItemsUsesRemaining2() > 0)
                                    {
                                        fighterMainSlot1.UpdateMainIconBGColour(OwnedLootInven.Instance.GetOtherSlotBGColour());

                                        UpdateMainIconDetails(null, OwnedLootInven.Instance.GetWornItemSecondAlly()[0].linkedItemPiece);
                                        fighterMainSlot1.UpdatePortrait(OwnedLootInven.Instance.GetWornItemSecondAlly()[0].linkedItemPiece.itemSpriteCombat);
                                        fighterMainSlot1.UpdateItemName(OwnedLootInven.Instance.GetWornItemSecondAlly()[0].linkedItemPiece.itemName);

                                        ItemPiece itemPiece = OwnedLootInven.Instance.GetWornItemSecondAlly()[0].linkedItemPiece;

                                        if (itemPiece.curRarity == ItemPiece.Rarity.COMMON)
                                            fighterMainSlot1.UpdateRarity(IconUI.Rarity.COMMON);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.RARE)
                                            fighterMainSlot1.UpdateRarity(IconUI.Rarity.RARE);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.EPIC)
                                            fighterMainSlot1.UpdateRarity(IconUI.Rarity.EPIC);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.LEGENDARY)
                                            fighterMainSlot1.UpdateRarity(IconUI.Rarity.LEGENDARY);

                                        if (itemPiece.curRace == ItemPiece.RaceSpecific.HUMAN)
                                            fighterMainSlot1.UpdateRaceIcon(humanRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.BEAST)
                                            fighterMainSlot1.UpdateRaceIcon(beastRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.ETHEREAL)
                                            fighterMainSlot1.UpdateRaceIcon(humanRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.ALL)
                                            fighterMainSlot1.UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);

                                        if (OwnedLootInven.Instance.GetWornItemSecondAlly()[0].linkedItemPiece.curActiveType == ItemPiece.ActiveType.ACTIVE)
                                            fighterMainSlot1.UpdatePassiveActiveType(true);
                                        else
                                            fighterMainSlot1.UpdatePassiveActiveType(false);
                                    }
                                    else
                                    {
                                        fighterMainSlot1.UpdatePortrait(TeamItemsManager.Instance.clearSlotSprite);
                                        fighterMainSlot1.UpdatePassiveActiveType(false, true);
                                        fighterMainSlot1.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
                                    }

                                    break;
                                }
                                else
                                {
                                    fighterMainSlot1.UpdatePortrait(TeamItemsManager.Instance.clearSlotSprite);
                                    fighterMainSlot1.UpdatePassiveActiveType(false, true);
                                    fighterMainSlot1.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
                                }
                            }
                            else if (x == 2)
                            {
                                if (OwnedLootInven.Instance.GetWornItemThirdAlly().Count > 0)
                                {
                                    if (OwnedLootInven.Instance.GetWornItemThirdAlly()[0].GetCalculatedItemsUsesRemaining2() > 0)
                                    {
                                        fighterMainSlot1.UpdateMainIconBGColour(OwnedLootInven.Instance.GetOtherSlotBGColour());

                                        UpdateMainIconDetails(null, OwnedLootInven.Instance.GetWornItemThirdAlly()[0].linkedItemPiece);
                                        fighterMainSlot1.UpdatePortrait(OwnedLootInven.Instance.GetWornItemThirdAlly()[0].linkedItemPiece.itemSpriteCombat);
                                        fighterMainSlot1.UpdateItemName(OwnedLootInven.Instance.GetWornItemThirdAlly()[0].linkedItemPiece.itemName);

                                        ItemPiece itemPiece = OwnedLootInven.Instance.GetWornItemThirdAlly()[0].linkedItemPiece;

                                        if (itemPiece.curRarity == ItemPiece.Rarity.COMMON)
                                            fighterMainSlot1.UpdateRarity(IconUI.Rarity.COMMON);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.RARE)
                                            fighterMainSlot1.UpdateRarity(IconUI.Rarity.RARE);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.EPIC)
                                            fighterMainSlot1.UpdateRarity(IconUI.Rarity.EPIC);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.LEGENDARY)
                                            fighterMainSlot1.UpdateRarity(IconUI.Rarity.LEGENDARY);

                                        if (itemPiece.curRace == ItemPiece.RaceSpecific.HUMAN)
                                            fighterMainSlot1.UpdateRaceIcon(humanRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.BEAST)
                                            fighterMainSlot1.UpdateRaceIcon(beastRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.ETHEREAL)
                                            fighterMainSlot1.UpdateRaceIcon(humanRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.ALL)
                                            fighterMainSlot1.UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);

                                        if (OwnedLootInven.Instance.GetWornItemThirdAlly()[0].linkedItemPiece.curActiveType == ItemPiece.ActiveType.ACTIVE)
                                            fighterMainSlot1.UpdatePassiveActiveType(true);
                                        else
                                            fighterMainSlot1.UpdatePassiveActiveType(false);
                                    }
                                    else
                                    {
                                        fighterMainSlot1.UpdatePortrait(TeamItemsManager.Instance.clearSlotSprite);
                                        fighterMainSlot1.UpdatePassiveActiveType(false, true);
                                        fighterMainSlot1.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
                                    }

                                    break;
                                }
                                else
                                {
                                    fighterMainSlot1.UpdatePortrait(TeamItemsManager.Instance.clearSlotSprite);
                                    fighterMainSlot1.UpdatePassiveActiveType(false, true);
                                    fighterMainSlot1.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
                                }
                            }

                            break;
                        }
                    }
                }
                else if (i == 1)
                {
                    for (int x = 0; x < activeTeam.Count; x++)
                    {
                        // Second fighter
                        if (activeTeam[x].unitName == GetActiveUnitFunctionality().GetUnitName())
                        {
                            if (x == 0)
                            {
                                if (OwnedLootInven.Instance.GetWornItemMainAlly().Count >= 2)
                                {
                                    if (OwnedLootInven.Instance.GetWornItemMainAlly()[1].GetCalculatedItemsUsesRemaining2() > 0)
                                    {
                                        fighterMainSlot2.UpdateMainIconBGColour(OwnedLootInven.Instance.GetOtherSlotBGColour());

                                        UpdateMainIconDetails(null, OwnedLootInven.Instance.GetWornItemMainAlly()[1].linkedItemPiece);
                                        fighterMainSlot2.UpdatePortrait(OwnedLootInven.Instance.GetWornItemMainAlly()[1].linkedItemPiece.itemSpriteCombat);
                                        fighterMainSlot2.UpdateItemName(OwnedLootInven.Instance.GetWornItemMainAlly()[1].linkedItemPiece.itemName);

                                        ItemPiece itemPiece = OwnedLootInven.Instance.GetWornItemMainAlly()[1].linkedItemPiece;

                                        if (itemPiece.curRarity == ItemPiece.Rarity.COMMON)
                                            fighterMainSlot2.UpdateRarity(IconUI.Rarity.COMMON);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.RARE)
                                            fighterMainSlot2.UpdateRarity(IconUI.Rarity.RARE);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.EPIC)
                                            fighterMainSlot2.UpdateRarity(IconUI.Rarity.EPIC);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.LEGENDARY)
                                            fighterMainSlot2.UpdateRarity(IconUI.Rarity.LEGENDARY);

                                        if (itemPiece.curRace == ItemPiece.RaceSpecific.HUMAN)
                                            fighterMainSlot2.UpdateRaceIcon(humanRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.BEAST)
                                            fighterMainSlot2.UpdateRaceIcon(beastRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.ETHEREAL)
                                            fighterMainSlot2.UpdateRaceIcon(humanRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.ALL)
                                            fighterMainSlot2.UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);

                                        if (OwnedLootInven.Instance.GetWornItemMainAlly()[1].linkedItemPiece.curActiveType == ItemPiece.ActiveType.ACTIVE)
                                            fighterMainSlot2.UpdatePassiveActiveType(true);
                                        else
                                            fighterMainSlot2.UpdatePassiveActiveType(false);
                                    }
                                    else
                                    {
                                        fighterMainSlot2.UpdatePortrait(TeamItemsManager.Instance.clearSlotSprite);
                                        fighterMainSlot2.UpdatePassiveActiveType(false, true);
                                        fighterMainSlot2.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
                                    }

                                    break;
                                }
                                else
                                {
                                    fighterMainSlot2.UpdatePortrait(TeamItemsManager.Instance.clearSlotSprite);
                                    fighterMainSlot2.UpdatePassiveActiveType(false, true);
                                    fighterMainSlot2.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
                                }
                            }
                            else if (x == 1)
                            {
                                if (OwnedLootInven.Instance.GetWornItemSecondAlly().Count >= 2)
                                {
                                    if (OwnedLootInven.Instance.GetWornItemSecondAlly()[1].GetCalculatedItemsUsesRemaining2() > 0)
                                    {
                                        fighterMainSlot2.UpdateMainIconBGColour(OwnedLootInven.Instance.GetOtherSlotBGColour());

                                        UpdateMainIconDetails(null, OwnedLootInven.Instance.GetWornItemSecondAlly()[1].linkedItemPiece);
                                        fighterMainSlot1.UpdatePortrait(OwnedLootInven.Instance.GetWornItemSecondAlly()[1].linkedItemPiece.itemSpriteCombat);
                                        fighterMainSlot1.UpdateItemName(OwnedLootInven.Instance.GetWornItemSecondAlly()[1].linkedItemPiece.itemName);

                                        ItemPiece itemPiece = OwnedLootInven.Instance.GetWornItemSecondAlly()[1].linkedItemPiece;

                                        if (itemPiece.curRarity == ItemPiece.Rarity.COMMON)
                                            fighterMainSlot2.UpdateRarity(IconUI.Rarity.COMMON);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.RARE)
                                            fighterMainSlot2.UpdateRarity(IconUI.Rarity.RARE);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.EPIC)
                                            fighterMainSlot2.UpdateRarity(IconUI.Rarity.EPIC);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.LEGENDARY)
                                            fighterMainSlot2.UpdateRarity(IconUI.Rarity.LEGENDARY);

                                        if (itemPiece.curRace == ItemPiece.RaceSpecific.HUMAN)
                                            fighterMainSlot2.UpdateRaceIcon(humanRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.BEAST)
                                            fighterMainSlot2.UpdateRaceIcon(beastRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.ETHEREAL)
                                            fighterMainSlot2.UpdateRaceIcon(humanRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.ALL)
                                            fighterMainSlot2.UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);

                                        if (OwnedLootInven.Instance.GetWornItemSecondAlly()[1].linkedItemPiece.curActiveType == ItemPiece.ActiveType.ACTIVE)
                                            fighterMainSlot2.UpdatePassiveActiveType(true);
                                        else
                                            fighterMainSlot2.UpdatePassiveActiveType(false);
                                    }
                                    else
                                    {
                                        fighterMainSlot2.UpdatePortrait(TeamItemsManager.Instance.clearSlotSprite);
                                        fighterMainSlot2.UpdatePassiveActiveType(false, true);
                                        fighterMainSlot2.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
                                    }

                                    break;
                                }
                                else
                                {
                                    fighterMainSlot2.UpdatePortrait(TeamItemsManager.Instance.clearSlotSprite);
                                    fighterMainSlot2.UpdatePassiveActiveType(false, true);
                                    fighterMainSlot2.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
                                }
                            }
                            else if (x == 2)
                            {
                                if (OwnedLootInven.Instance.GetWornItemThirdAlly().Count >= 2)
                                {
                                    if (OwnedLootInven.Instance.GetWornItemThirdAlly()[1].GetCalculatedItemsUsesRemaining2() > 0)
                                    {
                                        fighterMainSlot2.UpdateMainIconBGColour(OwnedLootInven.Instance.GetOtherSlotBGColour());

                                        UpdateMainIconDetails(null, OwnedLootInven.Instance.GetWornItemThirdAlly()[1].linkedItemPiece);
                                        fighterMainSlot2.UpdatePortrait(OwnedLootInven.Instance.GetWornItemThirdAlly()[1].linkedItemPiece.itemSpriteCombat);
                                        fighterMainSlot2.UpdateItemName(OwnedLootInven.Instance.GetWornItemThirdAlly()[1].linkedItemPiece.itemName);

                                        ItemPiece itemPiece = OwnedLootInven.Instance.GetWornItemThirdAlly()[1].linkedItemPiece;

                                        if (itemPiece.curRarity == ItemPiece.Rarity.COMMON)
                                            fighterMainSlot2.UpdateRarity(IconUI.Rarity.COMMON);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.RARE)
                                            fighterMainSlot2.UpdateRarity(IconUI.Rarity.RARE);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.EPIC)
                                            fighterMainSlot2.UpdateRarity(IconUI.Rarity.EPIC);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.LEGENDARY)
                                            fighterMainSlot2.UpdateRarity(IconUI.Rarity.LEGENDARY);

                                        if (itemPiece.curRace == ItemPiece.RaceSpecific.HUMAN)
                                            fighterMainSlot2.UpdateRaceIcon(humanRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.BEAST)
                                            fighterMainSlot2.UpdateRaceIcon(beastRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.ETHEREAL)
                                            fighterMainSlot2.UpdateRaceIcon(humanRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.ALL)
                                            fighterMainSlot2.UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);

                                        if (OwnedLootInven.Instance.GetWornItemThirdAlly()[1].linkedItemPiece.curActiveType == ItemPiece.ActiveType.ACTIVE)
                                            fighterMainSlot2.UpdatePassiveActiveType(true);
                                        else
                                            fighterMainSlot2.UpdatePassiveActiveType(false);
                                    }
                                    else
                                    {
                                        fighterMainSlot2.UpdatePortrait(TeamItemsManager.Instance.clearSlotSprite);
                                        fighterMainSlot2.UpdatePassiveActiveType(false, true);
                                        fighterMainSlot2.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
                                    }

                                    break;
                                }
                                else
                                {
                                    fighterMainSlot2.UpdatePortrait(TeamItemsManager.Instance.clearSlotSprite);
                                    fighterMainSlot2.UpdatePassiveActiveType(false, true);
                                    fighterMainSlot2.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
                                }
                            }

                            break;
                        }
                    }
                }
                else if (i == 2)
                {
                    for (int x = 0; x < activeTeam.Count; x++)
                    {
                        // Third fighter
                        if (activeTeam[x].unitName == GetActiveUnitFunctionality().GetUnitName())
                        {
                            if (x == 0)
                            {
                                if (OwnedLootInven.Instance.GetWornItemMainAlly().Count >= 3)
                                {
                                    if (OwnedLootInven.Instance.GetWornItemMainAlly()[2].GetCalculatedItemsUsesRemaining2() > 0)
                                    {
                                        fighterMainSlot3.UpdateMainIconBGColour(OwnedLootInven.Instance.GetOtherSlotBGColour());

                                        UpdateMainIconDetails(null, OwnedLootInven.Instance.GetWornItemMainAlly()[2].linkedItemPiece);
                                        fighterMainSlot3.UpdatePortrait(OwnedLootInven.Instance.GetWornItemMainAlly()[2].linkedItemPiece.itemSpriteCombat);
                                        fighterMainSlot3.UpdateItemName(OwnedLootInven.Instance.GetWornItemMainAlly()[2].linkedItemPiece.itemName);

                                        ItemPiece itemPiece = OwnedLootInven.Instance.GetWornItemMainAlly()[2].linkedItemPiece;

                                        if (itemPiece.curRarity == ItemPiece.Rarity.COMMON)
                                            fighterMainSlot3.UpdateRarity(IconUI.Rarity.COMMON);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.RARE)
                                            fighterMainSlot3.UpdateRarity(IconUI.Rarity.RARE);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.EPIC)
                                            fighterMainSlot3.UpdateRarity(IconUI.Rarity.EPIC);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.LEGENDARY)
                                            fighterMainSlot3.UpdateRarity(IconUI.Rarity.LEGENDARY);

                                        if (itemPiece.curRace == ItemPiece.RaceSpecific.HUMAN)
                                            fighterMainSlot3.UpdateRaceIcon(humanRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.BEAST)
                                            fighterMainSlot3.UpdateRaceIcon(beastRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.ETHEREAL)
                                            fighterMainSlot3.UpdateRaceIcon(humanRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.ALL)
                                            fighterMainSlot3.UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);

                                        if (OwnedLootInven.Instance.GetWornItemMainAlly()[2].linkedItemPiece.curActiveType == ItemPiece.ActiveType.ACTIVE)
                                            fighterMainSlot3.UpdatePassiveActiveType(true);
                                        else
                                            fighterMainSlot3.UpdatePassiveActiveType(false);
                                    }
                                    else
                                    {
                                        fighterMainSlot3.UpdatePortrait(TeamItemsManager.Instance.clearSlotSprite);
                                        fighterMainSlot3.UpdatePassiveActiveType(false, true);
                                        fighterMainSlot3.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
                                    }

                                    break;
                                }
                                else
                                {
                                    fighterMainSlot3.UpdatePortrait(TeamItemsManager.Instance.clearSlotSprite);
                                    fighterMainSlot3.UpdatePassiveActiveType(false, true);
                                    fighterMainSlot3.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
                                }
                            }
                            else if (x == 1)
                            {
                                if (OwnedLootInven.Instance.GetWornItemSecondAlly().Count >= 3)
                                {
                                    if (OwnedLootInven.Instance.GetWornItemSecondAlly()[2].GetCalculatedItemsUsesRemaining2() > 0)
                                    {
                                        fighterMainSlot3.UpdateMainIconBGColour(OwnedLootInven.Instance.GetOtherSlotBGColour());

                                        UpdateMainIconDetails(null, OwnedLootInven.Instance.GetWornItemSecondAlly()[2].linkedItemPiece);
                                        fighterMainSlot3.UpdatePortrait(OwnedLootInven.Instance.GetWornItemSecondAlly()[2].linkedItemPiece.itemSpriteCombat);
                                        fighterMainSlot3.UpdateItemName(OwnedLootInven.Instance.GetWornItemSecondAlly()[2].linkedItemPiece.itemName);

                                        ItemPiece itemPiece = OwnedLootInven.Instance.GetWornItemSecondAlly()[2].linkedItemPiece;

                                        if (itemPiece.curRarity == ItemPiece.Rarity.COMMON)
                                            fighterMainSlot3.UpdateRarity(IconUI.Rarity.COMMON);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.RARE)
                                            fighterMainSlot3.UpdateRarity(IconUI.Rarity.RARE);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.EPIC)
                                            fighterMainSlot3.UpdateRarity(IconUI.Rarity.EPIC);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.LEGENDARY)
                                            fighterMainSlot3.UpdateRarity(IconUI.Rarity.LEGENDARY);

                                        if (itemPiece.curRace == ItemPiece.RaceSpecific.HUMAN)
                                            fighterMainSlot3.UpdateRaceIcon(humanRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.BEAST)
                                            fighterMainSlot3.UpdateRaceIcon(beastRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.ETHEREAL)
                                            fighterMainSlot3.UpdateRaceIcon(humanRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.ALL)
                                            fighterMainSlot3.UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);

                                        if (OwnedLootInven.Instance.GetWornItemSecondAlly()[2].linkedItemPiece.curActiveType == ItemPiece.ActiveType.ACTIVE)
                                            fighterMainSlot3.UpdatePassiveActiveType(true);
                                        else
                                            fighterMainSlot3.UpdatePassiveActiveType(false);
                                    }
                                    else
                                    {
                                        fighterMainSlot3.UpdatePortrait(TeamItemsManager.Instance.clearSlotSprite);
                                        fighterMainSlot3.UpdatePassiveActiveType(false, true);
                                        fighterMainSlot3.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
                                    }

                                    break;
                                }
                                else
                                {
                                    fighterMainSlot3.UpdatePortrait(TeamItemsManager.Instance.clearSlotSprite);
                                    fighterMainSlot3.UpdatePassiveActiveType(false, true);
                                    fighterMainSlot3.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
                                }
                            }
                            else if (x == 2)
                            {
                                if (OwnedLootInven.Instance.GetWornItemThirdAlly().Count >= 3)
                                {
                                    if (OwnedLootInven.Instance.GetWornItemThirdAlly()[2].GetCalculatedItemsUsesRemaining2() > 0)
                                    {
                                        fighterMainSlot3.UpdateMainIconBGColour(OwnedLootInven.Instance.GetOtherSlotBGColour());

                                        UpdateMainIconDetails(null, OwnedLootInven.Instance.GetWornItemThirdAlly()[2].linkedItemPiece);
                                        fighterMainSlot3.UpdatePortrait(OwnedLootInven.Instance.GetWornItemThirdAlly()[2].linkedItemPiece.itemSpriteCombat);
                                        fighterMainSlot3.UpdateItemName(OwnedLootInven.Instance.GetWornItemThirdAlly()[2].linkedItemPiece.itemName);

                                        ItemPiece itemPiece = OwnedLootInven.Instance.GetWornItemThirdAlly()[2].linkedItemPiece;

                                        if (itemPiece.curRarity == ItemPiece.Rarity.COMMON)
                                            fighterMainSlot3.UpdateRarity(IconUI.Rarity.COMMON);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.RARE)
                                            fighterMainSlot3.UpdateRarity(IconUI.Rarity.RARE);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.EPIC)
                                            fighterMainSlot3.UpdateRarity(IconUI.Rarity.EPIC);
                                        else if (itemPiece.curRarity == ItemPiece.Rarity.LEGENDARY)
                                            fighterMainSlot3.UpdateRarity(IconUI.Rarity.LEGENDARY);

                                        if (itemPiece.curRace == ItemPiece.RaceSpecific.HUMAN)
                                            fighterMainSlot3.UpdateRaceIcon(humanRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.BEAST)
                                            fighterMainSlot3.UpdateRaceIcon(beastRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.ETHEREAL)
                                            fighterMainSlot3.UpdateRaceIcon(humanRaceIcon);
                                        else if (itemPiece.curRace == ItemPiece.RaceSpecific.ALL)
                                            fighterMainSlot3.UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);

                                        if (OwnedLootInven.Instance.GetWornItemThirdAlly()[2].linkedItemPiece.curActiveType == ItemPiece.ActiveType.ACTIVE)
                                            fighterMainSlot3.UpdatePassiveActiveType(true);
                                        else
                                            fighterMainSlot3.UpdatePassiveActiveType(false);
                                    }
                                    else
                                    {
                                        fighterMainSlot3.UpdatePortrait(TeamItemsManager.Instance.clearSlotSprite);
                                        fighterMainSlot3.UpdatePassiveActiveType(false, true);
                                        fighterMainSlot3.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
                                    }

                                    break;
                                }
                                else
                                {
                                    fighterMainSlot3.UpdatePortrait(TeamItemsManager.Instance.clearSlotSprite);
                                    fighterMainSlot3.UpdatePassiveActiveType(false, true);
                                    fighterMainSlot3.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
                                }
                            }

                            break;
                        }
                    }
                }
                else if (i == 3)
                {
                    fighterMainSlot4.UpdatePortrait(TeamItemsManager.Instance.clearSlotSprite);
                    fighterMainSlot4.UpdatePassiveActiveType(false, true);
                    fighterMainSlot4.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
                }
            }


            fighterMainSlot1.ToggleSelectImage(false);
            fighterMainSlot2.ToggleSelectImage(false);
            fighterMainSlot3.ToggleSelectImage(false);
            fighterMainSlot4.ToggleSelectImage(false);

            if (fighterMainSlot1.subText.text == "0")
                fighterMainSlot1.UpdateSubText(0, true, false, false);
            if (fighterMainSlot2.subText.text == "0")
                fighterMainSlot2.UpdateSubText(0, true, false, false);
            if (fighterMainSlot3.subText.text == "0")
                fighterMainSlot3.UpdateSubText(0, true, false, false);
            if (fighterMainSlot4.subText.text == "0")
                fighterMainSlot4.UpdateSubText(0, true, false, false);

            // Update items turns remaining text
            if (GetActiveUnitFunctionality().teamIndex == 0)
            {
                for (int z = 0; z < OwnedLootInven.Instance.GetWornItemMainAlly().Count; z++)
                {
                    if (OwnedLootInven.Instance.GetWornItemMainAlly()[z])
                    {
                        UpdateMainIconDetails(null, OwnedLootInven.Instance.GetWornItemMainAlly()[z].linkedItemPiece);

                        bool passive = false;

                        if (OwnedLootInven.Instance.GetWornItemMainAlly()[z].linkedItemPiece.curActiveType == ItemPiece.ActiveType.PASSIVE)
                            passive = true;

                        int subtext = OwnedLootInven.Instance.GetWornItemMainAlly()[z].linkedItemPiece.maxUsesPerCombat - OwnedLootInven.Instance.GetWornItemMainAlly()[z].GetItemUses();

                        //if (OwnedLootInven.Instance.GetWornItemMainAlly()[z].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.REFILLABLE)
                            //subtext = 0;

                        if (z == 0)
                        {
                            if (OwnedLootInven.Instance.GetWornItemMainAlly()[z])
                                fighterMainSlot1.UpdateSubText(subtext, true, passive, false);
                            else
                                fighterMainSlot1.UpdateSubText(0, true, passive, false);
                        }
                        else if (z == 1)
                        {
                            if (OwnedLootInven.Instance.GetWornItemMainAlly()[z])
                                fighterMainSlot2.UpdateSubText(subtext, true, passive, false);
                            else
                                fighterMainSlot2.UpdateSubText(0, true, passive, false);
                        }
                        else if (z == 2)
                        {
                            if (OwnedLootInven.Instance.GetWornItemMainAlly()[z])
                                fighterMainSlot3.UpdateSubText(subtext, true, passive, false);
                            else
                                fighterMainSlot3.UpdateSubText(0, true, passive, false);
                        }
                    }
                }
                if (OwnedLootInven.Instance.GetWornItemMainAlly().Count == 0)
                {
                    fighterMainSlot1.UpdateSubText(0, true, false, false);
                    fighterMainSlot2.UpdateSubText(0, true, false, false);
                    fighterMainSlot3.UpdateSubText(0, true, false, false);
                    fighterMainSlot4.UpdateSubText(0, true, false, false);
                }
            }
            if (GetActiveUnitFunctionality().teamIndex == 1)
            {
                for (int z = 0; z < OwnedLootInven.Instance.GetWornItemSecondAlly().Count; z++)
                {
                    if (OwnedLootInven.Instance.GetWornItemSecondAlly()[z])
                    {
                        UpdateMainIconDetails(null, OwnedLootInven.Instance.GetWornItemSecondAlly()[z].linkedItemPiece);

                        bool passive = false;

                        if (OwnedLootInven.Instance.GetWornItemSecondAlly()[z].linkedItemPiece.curActiveType == ItemPiece.ActiveType.PASSIVE)
                            passive = true;

                        int subtext = OwnedLootInven.Instance.GetWornItemSecondAlly()[z].linkedItemPiece.maxUsesPerCombat - OwnedLootInven.Instance.GetWornItemSecondAlly()[z].GetItemUses();

                        //if (OwnedLootInven.Instance.GetWornItemSecondAlly()[z].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.REFILLABLE)
                        //    subtext = 0;

                        if (z == 0)
                        {
                            if (OwnedLootInven.Instance.GetWornItemSecondAlly()[z])
                                fighterMainSlot1.UpdateSubText(subtext, true, passive, false);
                            else
                                fighterMainSlot1.UpdateSubText(0, true, passive, false);
                        }
                        else if (z == 1)
                        {
                            if (OwnedLootInven.Instance.GetWornItemSecondAlly()[z])
                                fighterMainSlot2.UpdateSubText(subtext, true, passive, false);
                            else
                                fighterMainSlot2.UpdateSubText(0, true, passive, false);
                        }
                        else if (z == 2)
                        {
                            if (OwnedLootInven.Instance.GetWornItemSecondAlly()[z])
                                fighterMainSlot3.UpdateSubText(subtext, true, passive, false);
                            else
                                fighterMainSlot3.UpdateSubText(0, true, passive, false);
                        }
                    }   
                }
                if (OwnedLootInven.Instance.GetWornItemSecondAlly().Count == 0)
                {
                    fighterMainSlot1.UpdateSubText(0, true, false, false);
                    fighterMainSlot2.UpdateSubText(0, true, false, false);
                    fighterMainSlot3.UpdateSubText(0, true, false, false);
                    fighterMainSlot4.UpdateSubText(0, true, false, false);
                }
            }
            if (GetActiveUnitFunctionality().teamIndex == 2)
            {
                for (int z = 0; z < OwnedLootInven.Instance.GetWornItemThirdAlly().Count; z++)
                {
                    if (OwnedLootInven.Instance.GetWornItemThirdAlly()[z])
                    {
                        UpdateMainIconDetails(null, OwnedLootInven.Instance.GetWornItemThirdAlly()[z].linkedItemPiece);

                        bool passive = false;

                        if (OwnedLootInven.Instance.GetWornItemThirdAlly()[z].linkedItemPiece.curActiveType == ItemPiece.ActiveType.PASSIVE)
                            passive = true;

                        int subtext = OwnedLootInven.Instance.GetWornItemThirdAlly()[z].linkedItemPiece.maxUsesPerCombat - OwnedLootInven.Instance.GetWornItemThirdAlly()[z].GetItemUses();

                        //if (OwnedLootInven.Instance.GetWornItemThirdAlly()[z].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.REFILLABLE)
                        //    subtext = 0;

                        if (z == 0)
                        {
                            if (OwnedLootInven.Instance.GetWornItemThirdAlly()[z])
                                fighterMainSlot1.UpdateSubText(subtext, true, passive, false);
                            else
                                fighterMainSlot1.UpdateSubText(0, true, passive, false);
                        }
                        else if (z == 1)
                        {
                            if (OwnedLootInven.Instance.GetWornItemThirdAlly()[z])
                                fighterMainSlot2.UpdateSubText(subtext, true, passive, false);
                            else
                                fighterMainSlot2.UpdateSubText(0, true, passive, false);
                        }
                        else if (z == 2)
                        {
                            if (OwnedLootInven.Instance.GetWornItemThirdAlly()[z])
                                fighterMainSlot3.UpdateSubText(subtext, true, passive, false);
                            else
                                fighterMainSlot3.UpdateSubText(0, true, passive, false);
                        }
                    }
                }
                if (OwnedLootInven.Instance.GetWornItemThirdAlly().Count == 0)
                {
                    fighterMainSlot1.UpdateSubText(0, true, false, false);
                    fighterMainSlot2.UpdateSubText(0, true, false, false);
                    fighterMainSlot3.UpdateSubText(0, true, false, false);
                    fighterMainSlot4.UpdateSubText(0, true, false, false);
                }

            }

            EnableFirstMainSlotSelection(false);

            fighterMainSlot4.UpdateSubText(0, true);
        }

        ToggleMainSlotVisibility(true);
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
            //MapManager.Instance.exitShopRoom.UpdateAlpha(1);
            ShopManager.Instance.ToggleExitShopButton(true);
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

    public void ResetActiveItem()
    {
        //GameManager.Instance.UpdateActiveItem(null);
        GameManager.Instance.UpdateMainIconDetails(null, null);
        //GameManager.Instance.UpdateUnitSelection(null, null);
        GameManager.Instance.UpdateUnitsSelectedText();
    }
    public IEnumerator DoItemAction()
    {
        ToggleAllowSelection(false);

        GetActiveUnitFunctionality().TriggerTextAlert(activeItem.itemName, 1, false, "Skill", false, true);

        if (GetActiveItem().itemAnnounce != null)
            AudioManager.Instance.Play(GetActiveItem().itemAnnounce.name);

        // Trigger Item Pop up
        GetActiveUnitFunctionality().TriggerItemVisualAlert(GetActiveItemSlot(), true, true);

        yield return new WaitForSeconds(0.35f);

        for (int i = 0; i < unitsSelected.Count; i++)
        {
            unitsSelected[i].ResetPowerUI();
        }

        // Spawn Projectiles
        if (activeItem.curHitType == ItemPiece.HitType.HITS)
        {
            for (int i = 0; i < activeItem.hitCount; i++)
            {
                // Loop through all selected units, spawn projectiles, if target is dead stop.
                for (int z = unitsSelected.Count - 1; z >= 0; z--)
                {
                    if (unitsSelected[z] == null)
                        continue;
                    else
                    {
                        GetActiveUnitFunctionality().SpawnProjectile(unitsSelected[z].transform, false);
                    }
                }

                int maxHitWorth = 15;
                if (activeItem.hitCount > maxHitWorth)
                    yield return new WaitForSeconds(timeBetweenProjectile - (0.0025f * (maxHitWorth + 2)));
                else
                    yield return new WaitForSeconds(timeBetweenProjectile - (0.0025f * (activeItem.hitCount + 2)));
            }
        }

        if (activeItem.curHitType == ItemPiece.HitType.HITS)
            yield return new WaitForSeconds(.35f);
        else
            yield return new WaitForSeconds(0);

        if (activeItem.curHitType == ItemPiece.HitType.HITS)
        {
            AudioManager.Instance.Play("SFX_ItemTrigger");

            for (int i = 0; i < activeItem.hitCount; i++)
            {
                /*
                if (unitsSelected.Count == 0)
                    yield break;
                */

                // Cause the item's functionality to go
                if (activeItem.curItemType == ItemPiece.ItemType.OFFENSE && GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER
                && unitsSelected[0].curUnitType == UnitFunctionality.UnitType.ENEMY)
                {
                    // Damage enemy
                    for (int x = 0; x < unitsSelected.Count; x++)
                    {
                        // Apply effect
                        if (i == 0)
                        {
                            if (activeItem.effectAdded != null)
                            {
                                unitsSelected[x].AddUnitEffect(activeItem.effectAdded, unitsSelected[x], activeItem.effectAddedTurnLength, 2, true, true, GetActiveItemSlot().linkedSlot);
                            }
                        }

                        int randomPower = RandomisePower(GetActiveUnitFunctionality().curPower+10);
                        unitsSelected[x].UpdateUnitCurHealth(randomPower, true, false, true, true, false);
                    }
                }
                else if (activeItem.curItemType == ItemPiece.ItemType.SUPPORT && GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER
                    && unitsSelected[0].curUnitType == UnitFunctionality.UnitType.PLAYER)
                {
                    // Heal ally
                    for (int t = 0; t < unitsSelected.Count; t++)
                    {
                        // Apply effect
                        if (i == 0)
                        {
                            if (activeItem.effectAdded != null)
                            {
                                unitsSelected[t].AddUnitEffect(activeItem.effectAdded, unitsSelected[t], 2, 2, true, true, true, GetActiveItemSlot().linkedSlot);
                            }
                        }

                        int randomPower = RandomisePower(GetActiveUnitFunctionality().curPower+10);
                        //Debug.Log("healing ally " + randomPower);

                        unitsSelected[t].UpdateUnitCurHealth(randomPower, false, false, true, true, false);
                    }
                }

                int maxHitWorth = 15;
                if (activeItem.hitCount > maxHitWorth)
                    yield return new WaitForSeconds(timeBetweenProjectile - (0.0025f * (maxHitWorth + 4)));
                else
                    yield return new WaitForSeconds(timeBetweenProjectile - (0.0025f * (activeItem.hitCount + 4)));
            }

            yield return new WaitForSeconds(0.15f);

            ResetSelectedUnits();

            ToggleAllowSelection(true);

            
        }
        else
        {
            // Do Visual Effect on selected units
            StartCoroutine(UpdateSelectedUnitsEffectVisual(false));

            yield return new WaitForSeconds(1);

            // Cleanse effect from unit
            if (GetActiveItem().effectCleansed != null)
            {
                for (int i = 0; i < GetActiveItem().cleanseCount; i++)
                {
                    for (int x = 0; x < unitsSelected.Count; x++)
                    {
                        if (GetActiveItem().effectCleansed.effectName == "POISON")
                        {
                            if (unitsSelected[x].GetEffect("POISON"))
                            {
                                string name = unitsSelected[x].GetEffect("POISON").effectName;

                                unitsSelected[x].TriggerTextAlert(name, 1, true, "Trigger");
                                unitsSelected[x].GetEffect("POISON").ReduceTurnCountText(unitsSelected[x]);
                            }
                        }

                        if (GetActiveItem().effectCleansed.effectName == "BLEED")
                        {
                            if (unitsSelected[x].GetEffect("BLEED"))
                            {
                                string name = unitsSelected[x].GetEffect("BLEED").effectName;

                                unitsSelected[x].TriggerTextAlert(name, 1, true, "Trigger");
                                unitsSelected[x].GetEffect("BLEED").ReduceTurnCountText(unitsSelected[x]);
                            }
                        }                       
                    }
                }
            }

            // Perform Damage / Heal
            StartCoroutine(TriggerPowerUI(GetActiveItem().itemPower, GetActiveItem().hitCount, false, 1));

            yield return new WaitForSeconds(1);

            ResetSelectedUnits();
        }

        ToggleAllowSelection(true);

        bool removeItem = false;

        //TeamItemsManager.Instance.IncItemUseCount();

        // Decrease Item Cooldown
        for (int i = 0; i < activeRoomHeroes.Count; i++)
        {
            if (activeRoomHeroes[i] == GetActiveUnitFunctionality())
            {
                if (i == 0)
                {
                    for (int x = 0; x < OwnedLootInven.Instance.GetWornItemMainAlly().Count; x++)
                    {
                        if (GetActiveItemSlot() == OwnedLootInven.Instance.GetWornItemMainAlly()[x])
                        {
                            if (x == 0)
                            {
                                if (fighterMainSlot1.GetItemName() == "")
                                    continue;

                                activeRoomHeroes[i].DecreaseUsesItem1(true);
                                int minus = OwnedLootInven.Instance.GetWornItemMainAlly()[x].GetCalculatedItemsUsesRemaining2();
                                fighterMainSlot1.UpdateSubText(minus, true);

                                if (minus <= 0)
                                {
                                    fighterMainSlot1.RemoveItemFromSlot();
                                    ResetActiveItem();
                                    removeItem = true;
                                }
                                break;
                            }
                            else if (x == 1)
                            {
                                if (fighterMainSlot2.GetItemName() == "")
                                    continue;

                                activeRoomHeroes[i].DecreaseUsesItem2(true);
                                int minus = OwnedLootInven.Instance.GetWornItemMainAlly()[x].GetCalculatedItemsUsesRemaining2();
                                fighterMainSlot2.UpdateSubText(minus, true);


                                if (minus <= 0)
                                {
                                    fighterMainSlot2.RemoveItemFromSlot();
                                    ResetActiveItem();
                                    removeItem = true;
                                }
                                break;
                            }
                            else if (x == 2)
                            {
                                if (fighterMainSlot3.GetItemName() == "")
                                    continue;

                                activeRoomHeroes[i].DecreaseUsesItem3(true);
                                int minus = OwnedLootInven.Instance.GetWornItemMainAlly()[x].GetCalculatedItemsUsesRemaining2();
                                fighterMainSlot3.UpdateSubText(minus, true);

                                if (minus <= 0)
                                {
                                    fighterMainSlot3.RemoveItemFromSlot();
                                    ResetActiveItem();
                                    removeItem = true;
                                }
                                break;
                            }
                        }
                    }
                }
                else if (i == 1)
                {
                    for (int x = 0; x < OwnedLootInven.Instance.GetWornItemSecondAlly().Count; x++)
                    {
                        if (GetActiveItemSlot() == OwnedLootInven.Instance.GetWornItemSecondAlly()[x])
                        {
                            if (x == 0)
                            {
                                if (fighterMainSlot1.GetItemName() == "")
                                    continue;

                                activeRoomHeroes[i].DecreaseUsesItem1(true);
                                int minus = OwnedLootInven.Instance.GetWornItemSecondAlly()[x].GetCalculatedItemsUsesRemaining2();
                                fighterMainSlot1.UpdateSubText(minus, true);

                                if (minus == 0)
                                {
                                    fighterMainSlot1.RemoveItemFromSlot();
                                    ResetActiveItem();
                                    removeItem = true;
                                }
                                break;
                            }
                            else if (x == 1)
                            {
                                if (fighterMainSlot2.GetItemName() == "")
                                    continue;

                                activeRoomHeroes[i].DecreaseUsesItem2(true);
                                int minus = OwnedLootInven.Instance.GetWornItemSecondAlly()[x].GetCalculatedItemsUsesRemaining2();
                                fighterMainSlot2.UpdateSubText(minus, true);


                                if (minus == 0)
                                {
                                    fighterMainSlot2.RemoveItemFromSlot();
                                    ResetActiveItem();
                                    removeItem = true;
                                }
                                break;
                            }
                            else if (x == 2)
                            {
                                if (fighterMainSlot3.GetItemName() == "")
                                    continue;

                                activeRoomHeroes[i].DecreaseUsesItem3(true);
                                int minus = OwnedLootInven.Instance.GetWornItemSecondAlly()[x].GetCalculatedItemsUsesRemaining2();
                                fighterMainSlot3.UpdateSubText(minus, true);


                                if (minus == 0)
                                {
                                    fighterMainSlot3.RemoveItemFromSlot();
                                    ResetActiveItem();
                                    removeItem = true;
                                }
                                break;
                            }
                        }
                    }
                }
                else if (i == 2)
                {
                    for (int x = 0; x < OwnedLootInven.Instance.GetWornItemThirdAlly().Count; x++)
                    {
                        if (GetActiveItemSlot() == OwnedLootInven.Instance.GetWornItemThirdAlly()[x])
                        {
                            if (x == 0)
                            {
                                if (fighterMainSlot1.GetItemName() == "")
                                    continue;

                                activeRoomHeroes[i].DecreaseUsesItem1(true);
                                int minus = OwnedLootInven.Instance.GetWornItemThirdAlly()[x].GetCalculatedItemsUsesRemaining2();
                                fighterMainSlot1.UpdateSubText(minus, true);

                                if (minus == 0)
                                {
                                    fighterMainSlot1.RemoveItemFromSlot();
                                    ResetActiveItem();
                                    removeItem = true;
                                }
                                break;
                            }
                            else if (x == 1)
                            {
                                if (fighterMainSlot2.GetItemName() == "")
                                    continue;

                                activeRoomHeroes[i].DecreaseUsesItem2(true);
                                int minus = OwnedLootInven.Instance.GetWornItemThirdAlly()[x].GetCalculatedItemsUsesRemaining2();
                                fighterMainSlot2.UpdateSubText(minus, true);


                                if (minus == 0)
                                {
                                    fighterMainSlot2.RemoveItemFromSlot();
                                    ResetActiveItem();
                                    removeItem = true;
                                }
                                break;
                            }
                            else if (x == 2)
                            {
                                if (fighterMainSlot3.GetItemName() == "")
                                    continue;

                                activeRoomHeroes[i].DecreaseUsesItem3(true);
                                int minus = OwnedLootInven.Instance.GetWornItemThirdAlly()[x].GetCalculatedItemsUsesRemaining2();
                                fighterMainSlot3.UpdateSubText(minus, true);

                                if (minus == 0)
                                {
                                    fighterMainSlot3.RemoveItemFromSlot();
                                    ResetActiveItem();
                                    removeItem = true;
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        if (removeItem)
            UpdateActiveItem(null);

        //if (activeItem == null)
            //UpdatePlayerAbilityUI(false, true);

        if (GetActiveItem() == null)
        {
            // Disable unit selection image
            // Disable being able to select fighters
            for (int i = 0; i < activeRoomAllUnitFunctionalitys.Count; i++)
            {
                activeRoomAllUnitFunctionalitys[i].ToggleSelected(false);
            }

            ToggleSelectingUnits(false);

            // Set item details to empty
            OverlayUI.Instance.UpdateItemUI("", "", 0, 0, TeamItemsManager.Instance.clearSlotSprite);
        }

        ToggleSelectingUnits(false);
        GameManager.Instance.UpdateMainIconDetails(null, null);

        GameManager.Instance.fighterMainSlot1.ToggleSelectImage(false);
        GameManager.Instance.fighterMainSlot2.ToggleSelectImage(false);
        GameManager.Instance.fighterMainSlot3.ToggleSelectImage(false);
        GameManager.Instance.fighterMainSlot4.ToggleSelectImage(false);

        CheckToEndCombat();
    }

    public void CheckToRemoveItems()
    {
        int count1 = OwnedLootInven.Instance.GetWornItemMainAlly().Count;
        for (int i = 0; i < count1; i++)
        {
            if (i >= OwnedLootInven.Instance.GetWornItemMainAlly().Count)
                break;

            if (OwnedLootInven.Instance.GetWornItemMainAlly()[i].GetCalculatedItemsUsesRemaining2() <= 0
                && OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
            {
                OwnedLootInven.Instance.GetWornItemMainAlly()[i].UpdateRemoved(true);
            }
        }
        for (int i = 0; i < count1; i++)
        {
            if (i >= OwnedLootInven.Instance.GetWornItemMainAlly().Count)
                break;

            if (OwnedLootInven.Instance.GetWornItemMainAlly()[i].GetRemoved())
            {
                TeamItemsManager.Instance.RemoveMainItem(OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece);
                OwnedLootInven.Instance.RemoveWornItemAllyMain(OwnedLootInven.Instance.GetWornItemMainAlly()[i]);
            }
        }

        int count2 = OwnedLootInven.Instance.GetWornItemSecondAlly().Count;
        for (int i = 0; i < count2; i++)
        {
            if (i >= OwnedLootInven.Instance.GetWornItemSecondAlly().Count)
                break;

            if (OwnedLootInven.Instance.GetWornItemSecondAlly()[i].GetCalculatedItemsUsesRemaining2() <= 0
                && OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
            {
                OwnedLootInven.Instance.GetWornItemSecondAlly()[i].UpdateRemoved(true);
            }
        }
        for (int i = 0; i < count2; i++)
        {
            if (i >= OwnedLootInven.Instance.GetWornItemSecondAlly().Count)
                break;

            if (OwnedLootInven.Instance.GetWornItemSecondAlly()[i].GetRemoved())
            {
                TeamItemsManager.Instance.RemoveSecondItem(OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece);
                OwnedLootInven.Instance.RemoveWornItemAllySecond(OwnedLootInven.Instance.GetWornItemSecondAlly()[i]);
            }
        }

        int count3 = OwnedLootInven.Instance.GetWornItemThirdAlly().Count;
        for (int i = 0; i < count3; i++)
        {
            if (i >= OwnedLootInven.Instance.GetWornItemThirdAlly().Count)
                break;

            if (OwnedLootInven.Instance.GetWornItemThirdAlly()[i].GetCalculatedItemsUsesRemaining2() <= 0
                && OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
            {
                OwnedLootInven.Instance.GetWornItemThirdAlly()[i].UpdateRemoved(true);
            }
        }
        for (int i = 0; i < count3; i++)
        {
            if (i > OwnedLootInven.Instance.GetWornItemThirdAlly().Count)
                break;

            if (OwnedLootInven.Instance.GetWornItemThirdAlly()[i].GetRemoved())
            {
                TeamItemsManager.Instance.RemoveThirdItem(OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece);
                OwnedLootInven.Instance.RemoveWornItemAllyThird(OwnedLootInven.Instance.GetWornItemThirdAlly()[i]);
            }
        }
    }

    public void SpawnItem()
    {
        GameObject go = Instantiate(ItemRewardManager.Instance.itemGO, nothingnessUI.gameObject.transform.position, Quaternion.identity);


        go.transform.SetParent(nothingnessUI.gameObject.transform);
        go.transform.localScale = new Vector2(1, 1);

        UIElement uIElement = go.GetComponent<UIElement>();
        Slot slot = go.GetComponent<Slot>();

        uIElement.ToggleButton(false);

        ItemPiece newItem = ItemRewardManager.Instance.selectedItem;

        OwnedLootInven.Instance.AddOwnedItems(slot);

        ItemPiece itemPiece = new ItemPiece();
        itemPiece = newItem;

        slot.UpdateLinkedItemPiece(itemPiece);
        slot.UpdateSlotName(slot.linkedItemPiece.itemName);
        slot.ToggleEquipButton(false);
        slot.isEmpty = false;
        slot.UpdateSlotImage(newItem.itemSpriteItemTab);
        slot.UpdateSlotName(newItem.itemName);
        slot.UpdateLinkedItemPiece(newItem);
        slot.UpdateLootGearAlpha(true);

        //TeamItemsManager.Instance.IncItemsSpawned();
        //slot.UpdateSlotCode(TeamItemsManager.Instance.GetItemsSpawned());

        ShopManager.Instance.UpdateUnAssignedItem(null);

        ToggleAllowSelection(false);

        SetAllFightersSelected(false);

        ShopManager.Instance.ToggleInventoryUI(false);
        ShopManager.Instance.shopSelectAllyPrompt.UpdateAlpha(0);

        if (ShopManager.Instance.playerIsYetToSelectAFighter)
            OverlayUI.Instance.ToggleFighterDetailsTab(false);

        ShopManager.Instance.playerIsYetToSelectAFighter = false;

        ShopManager.Instance.ToggleExitShopButton(true);

    }
    public void targetUnit(UnitFunctionality unit, bool attack = false)
    {
        //Debug.Log("earlier - selecting unit " + unit.GetUnitName());
        //Debug.Log("Targeting unit " + unit.GetUnitName());

        // If current room is a shop
        if (RoomManager.Instance.GetActiveRoom().curRoomType == RoomMapIcon.RoomType.SHOP && !unit.purchased && GetAllowSelection())
        {
            // Check if target unit has maximum equipt items, if so, do not allow this unit selection for item equipping
            if (unit.teamIndex == 0)
            {
                if (TeamItemsManager.Instance.equippedItemsMain.Count == 3)
                {
                    AudioManager.Instance.Play("SFX_ShopBuyFail");
                    return;
                }
            }
            else if (unit.teamIndex == 1)
            {
                if (TeamItemsManager.Instance.equippedItemsSecond.Count == 3)
                {
                    AudioManager.Instance.Play("SFX_ShopBuyFail");
                    return;
                }
            }
            else if (unit.teamIndex == 2)
            {
                if (TeamItemsManager.Instance.equippedItemsThird.Count == 3)
                {
                    AudioManager.Instance.Play("SFX_ShopBuyFail");
                    return;
                }
            }

            ToggleAllowSelection(false);

            SetAllFightersSelected(false);

            ShopManager.Instance.ToggleInventoryUI(false);
            ShopManager.Instance.shopSelectAllyPrompt.UpdateAlpha(0);

            if (ShopManager.Instance.playerIsYetToSelectAFighter)
                OverlayUI.Instance.ToggleFighterDetailsTab(false);

            ShopManager.Instance.playerIsYetToSelectAFighter = false;
            //unit.AddOwnedItems(ShopManager.Instance.GetUnassignedItem());

            GameObject go = Instantiate(ItemRewardManager.Instance.itemGO, nothingnessUI.gameObject.transform.position, Quaternion.identity);
            go.transform.SetParent(nothingnessUI.gameObject.transform);
            go.transform.localScale = new Vector2(1, 1);

            UIElement uIElement = go.GetComponent<UIElement>();
            Slot slot = go.GetComponent<Slot>();

            uIElement.ToggleButton(false);

            ItemPiece newItem = ItemRewardManager.Instance.selectedItem;

            //OwnedLootInven.Instance.AddOwnedItems(slot);

            ItemPiece itemPiece = new ItemPiece();
            itemPiece = newItem;

            slot.UpdateLinkedItemPiece(itemPiece);

            slot.ToggleEquipButton(false);
            slot.isEmpty = false;
            slot.UpdateSlotImage(newItem.itemSpriteItemTab);

            slot.UpdateSlotName(newItem.itemName);
            slot.UpdateSlotDetails();

            //OwnedLootInven.Instance.AddOwnedItems(slot);

            //Debug.Log("111");
            if (unit.teamIndex == 0)
            {
                //Debug.Log("aaa");
                itemPiece.UpdateItemPiece(newItem.itemName, newItem.curRarity.ToString(), newItem.itemSpriteItemTab);    

                for (int i = 0; i < TeamItemsManager.Instance.ally1ItemsSlots.Count; i++)
                {   
                    if (!TeamItemsManager.Instance.ally1ItemsSlots[i].linkedItemPiece)
                    {
                        OwnedLootInven.Instance.AddWornItemAllyMain(slot);

                        TeamItemsManager.Instance.ally1ItemsSlots[i].UpdateLinkedSlot(slot);
                        TeamItemsManager.Instance.ally1ItemsSlots[i].UpdateLinkedItemPiece(itemPiece);
                        TeamItemsManager.Instance.ally1ItemsSlots[i].UpdateSlotName(newItem.itemName);
                        TeamItemsManager.Instance.ally1ItemsSlots[i].UpdateSlotDetails();
                        break;
                    }
                }

                TeamItemsManager.Instance.UpdateEquippedItemPiece("ItemMain", itemPiece);
            }
            if (unit.teamIndex == 1)
            {
                //Debug.Log("aaa");
                itemPiece.UpdateItemPiece(newItem.itemName, newItem.curRarity.ToString(), newItem.itemSpriteItemTab);

                for (int i = 0; i < TeamItemsManager.Instance.ally2ItemsSlots.Count; i++)
                {
                    if (!TeamItemsManager.Instance.ally2ItemsSlots[i].linkedItemPiece)
                    {
                        OwnedLootInven.Instance.AddWornItemAllySecond(slot);

                        TeamItemsManager.Instance.ally2ItemsSlots[i].UpdateLinkedSlot(slot);
                        TeamItemsManager.Instance.ally2ItemsSlots[i].UpdateLinkedItemPiece(itemPiece);
                        TeamItemsManager.Instance.ally2ItemsSlots[i].UpdateSlotName(newItem.itemName);
                        TeamItemsManager.Instance.ally2ItemsSlots[i].UpdateSlotDetails();
                        break;
                    }
                }

                TeamItemsManager.Instance.UpdateEquippedItemPiece("ItemSecond", itemPiece);
            }
            if (unit.teamIndex == 2)
            {
                //Debug.Log("aaa");
                itemPiece.UpdateItemPiece(newItem.itemName, newItem.curRarity.ToString(), newItem.itemSpriteItemTab);

                for (int i = 0; i < TeamItemsManager.Instance.ally3ItemsSlots.Count; i++)
                {
                    if (!TeamItemsManager.Instance.ally3ItemsSlots[i].linkedItemPiece)
                    {
                        OwnedLootInven.Instance.AddWornItemAllyThird(slot);

                        TeamItemsManager.Instance.ally3ItemsSlots[i].UpdateLinkedSlot(slot);
                        TeamItemsManager.Instance.ally3ItemsSlots[i].UpdateLinkedItemPiece(itemPiece);
                        TeamItemsManager.Instance.ally3ItemsSlots[i].UpdateSlotName(newItem.itemName);
                        TeamItemsManager.Instance.ally3ItemsSlots[i].UpdateSlotDetails();
                        break;
                    }
                }

                TeamItemsManager.Instance.UpdateEquippedItemPiece("ItemThird", itemPiece);                
            }

            // Need to make Worn item ally actually be real

            // Set item
            uIElement.UpdateContentImage(ItemRewardManager.Instance.selectedItem.itemSpriteItemTab);
            uIElement.UpdateItemName(ItemRewardManager.Instance.selectedItem.itemName);

            if (ItemRewardManager.Instance.selectedItem.curRarity == ItemPiece.Rarity.LEGENDARY)
            {
                uIElement.UpdateRarityBorderColour(ItemRewardManager.Instance.legendaryColour);
            }
            else if (ItemRewardManager.Instance.selectedItem.curRarity == ItemPiece.Rarity.EPIC)
            {
                uIElement.UpdateRarityBorderColour(ItemRewardManager.Instance.epicColour);
            }
            else if (ItemRewardManager.Instance.selectedItem.curRarity == ItemPiece.Rarity.RARE)
            {
                uIElement.UpdateRarityBorderColour(ItemRewardManager.Instance.rareColour);
            }
            else if (ItemRewardManager.Instance.selectedItem.curRarity == ItemPiece.Rarity.COMMON)
            {
                uIElement.UpdateRarityBorderColour(ItemRewardManager.Instance.commonColour);
            }


            // Disable owned gear button for unowned loot
            //slot.ToggleOwnedGearButton(false);
            // Show full visibility of Gear
            slot.UpdateLootGearAlpha(true);

            /*
            if (ShopManager.Instance.GetUnassignedItem().healthItem)
            {
                float healthToRegen = (ShopManager.Instance.GetUnassignedItem().power / 100f) * unit.GetUnitMaxHealth();
               // unit.StartCoroutine(unit.SpawnPowerUI(healthToRegen, false, false, null, false));
                unit.UpdateUnitCurHealth((int)healthToRegen, false, false);
            }
            */

            ShopManager.Instance.UpdateUnAssignedItem(null);

            // If item is health item, do the effect of it

            StartCoroutine(WaitTimeThenDeselect(shopRemoveSelectTime, unit));
        }

        if (!unitsSelected.Contains(unit))
        {
            unitsSelected.Add(unit);
        }
        // Ensure units cant change their selection before asd after attack
        // Allows hero rooms to still allow selection if room is defeated
        if (roomDefeated)
        {

        }
        /*
        else if (!GetAllowSelection() || !GetSelectingUnitsAllowed())
        {
            //Debug.Log("ending");
            return;
        }
        */

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

        if (isSkillsMode)
        {
            if (activeSkill)
            {
                // Ensure skills that target alive cant select dead targets
                if (unit.isDead && activeSkill.curskillSelectionAliveType == SkillData.SkillSelectionAliveType.ALIVE)
                    return;
                // Ensure skills that target dead cant select alive targets
                else if (!unit.isDead && activeSkill.curskillSelectionAliveType == SkillData.SkillSelectionAliveType.DEAD)
                    return;
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
                    if (activeSkill.curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.PLAYERS && unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                    {
                        //Debug.Log("ending 2");
                        return;
                    }
                    if (activeSkill.curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.ENEMIES && unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                    {
                        //Debug.Log("ending 3");
                        return;
                    }
                }
            }

            // If user selects a unit that is already selected, unselect it, and go a different path
            if (GetSelectingUnitsAllowed())
            {
                //UnSelectUnit(unit);
                //UpdateUnitsSelectedText();
                // If its not a hero room, dont attack on unselecting
                if (!combatOver && attack)
                {
                    unit.ToggleSelected(true);

                    ToggleSelectingUnits(false);
                    ToggleAllowSelection(false);
                    HideMainSlotDetails();
                    PlayerAttack();
                    return;
                }

                // Select targeted unit
                //UnSelectUnit(unit);
                //unit.ToggleSelected(true);

                return;
            }
        }
        else
        {
            if (activeItem == null)
                return;

            if (activeItem.curActiveType == ItemPiece.ActiveType.PASSIVE)
                return;

            if (activeItem)
            {
                // Ensure skills that target alive cant select dead targets
                if (unit.isDead && activeItem.curTargetType == ItemPiece.TargetType.ALIVE)
                    return;
                // Ensure skills that target dead cant select alive targets
                else if (!unit.isDead && activeItem.curTargetType == ItemPiece.TargetType.DEAD)
                    return;
            }

            if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER && !HeroRoomManager.Instance.GetPlayerOffered())
            {
                if (activeItem.curSelectionType == ItemPiece.SelectionType.ALLIES && unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                {
                    //Debug.Log("ending 2");
                    return;
                }
                if (activeItem.curSelectionType == ItemPiece.SelectionType.ENEMIES && unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                {
                    //Debug.Log("ending 3");
                    return;
                }
                if (activeItem.isSelfCast && unit != GetActiveUnitFunctionality())
                    return;
            }

            // If user selects a unit that is already selected, unselect it, and go a different path
            if (unit.IsSelected() && GetSelectingUnitsAllowed())
            {
                //UnSelectUnit(unit);
                //UpdateUnitsSelectedText();
                // If its not a hero room, dont attack on unselecting
                if (!combatOver)
                {
                    if (activeItem.curItemTargetType == ItemPiece.ItemTargetType.NORMAL)
                    {
                        StartCoroutine(DoItemAction());
                    }
                    else
                    {
                        List<UnitFunctionality> removedUnits = new List<UnitFunctionality>();

                        if (unitsSelected.Count > GetActiveItem().targetCount)
                        {
                            int count = unitsSelected.Count - GetActiveItem().targetCount;

                            for (int i = 0; i < count; i++)
                            {
                                int rand1 = Random.Range(0, unitsSelected.Count);

                                if (!removedUnits.Contains(unitsSelected[rand1]))
                                {
                                    removedUnits.Add(unitsSelected[rand1]);
                                    unitsSelected[rand1].ToggleSelected(false);
                                }
                                else if (removedUnits.Contains(unitsSelected[rand1]))
                                {
                                    //if (i > 0)
                                    i--;
                                }
                            }
                        }

                        for (int i = 0; i < removedUnits.Count; i++)
                        {
                            unitsSelected.Remove(removedUnits[i]);

                            Debug.Log("removing selection - item");
                            //if (i > 0)
                        }

                        StartCoroutine(DoItemAction());
                    }
                    return;
                }

                // Select targeted unit
                UnSelectUnit(unit);
                //unit.ToggleSelected(true);

                return;
            }
        }

        if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
        {
            // If enemies are taunting, do not allow selection if this unit to select is not taunting also
            if (IsEnemyTaunting().Count >= 1)
            {
                for (int i = 0; i < IsEnemyTaunting().Count; i++)
                {
                    if (!isSkillsMode)
                    {
                        // If active unit is ally, and is selecting enemies, consider enemies parrying
                        if (GetActiveItem().curSelectionType == ItemPiece.SelectionType.ENEMIES)
                        {
                            if (IsEnemyTaunting()[i] == unit)
                            {
                                // remove previous target
                                if (unitsSelected.Count == GetActiveItem().targetCount)
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
                            if (unitsSelected.Count == GetActiveItem().targetCount)
                                ResetSelectedUnits();

                            // Select targeted unit
                            unitsSelected.Add(unit);
                            unit.ToggleSelected(true);
                            UpdateUnitsSelectedText();
                            break;
                        }
                    }
                    else
                    {
                        // If active unit is ally, and is selecting enemies, consider enemies parrying
                        if (GetActiveSkill().curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.ENEMIES)
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
            }
            else // If no enemies are taunting
            {
                if (!isSkillsMode)
                {
                    // remove previous target
                    if (GetActiveItem() != null)
                    {
                        if (unitsSelected.Count == GetActiveItem().targetCount && activeItem.curItemTargetType != ItemPiece.ItemTargetType.RANDOM)
                        {
                            unitsSelected[0].ToggleSelected(false);
                            unitsSelected.RemoveAt(0);
                        }
                    }
                }
                else
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
                //Debug.Log("selecting thingo");
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

        ToggleMainSlotVisibility(false);

        CombatGridManager.Instance.ToggleButtonAttackMovement(false);

        if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            GetActiveUnitFunctionality().TriggerTextAlert(GetActiveSkill().skillName, 1, false, "Trigger", false, true);

        ToggleUnitEffectTooltipsOff();

        //unitsSelected = unitsSelected.Distinct().ToList();

        for (int i = 0; i < unitsSelected.Count; i++)
        {
            unitsSelected[i].ToggleSelected(true, true);
        }

        ToggleAllowSelection(false);
        CombatGridManager.Instance.DisableAllButtons();

        StartCoroutine(AttackButtonCont());
    }

    public void AddUnitsSelected(UnitFunctionality unit)
    {
        if (!unitsSelected.Contains(unit))
        {
            unitsSelected.Add(unit);
        }
    }

    IEnumerator AttackButtonCont()
    {
        if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            yield return new WaitForSeconds(skillAlertAppearTime / 2);
        else
            yield return new WaitForSeconds(0);

        if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER && !GetActiveUnitFunctionality().reanimated)
        {
            WeaponManager.Instance.SetHeroWeapon(GetActiveUnitFunctionality().GetUnitName());
            SetupPlayerWeaponUI();
        }
        else if (GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY || GetActiveUnitFunctionality().reanimated)
            WeaponManager.Instance.SetEnemyWeapon(GetActiveUnitFunctionality(), true);
    }

    public void UpdateUnitsSelectedText()
    {
        if (isSkillsMode)
        {
            // If a skill is selected
            if (activeSkill != null)
            {
                UpdateUnitsSelectedText(unitsSelected.Count, activeSkill.GetCalculatedSkillSelectionCount());
            }
            else
            {
                UpdateUnitsSelectedText(0, 0);
            }
        }
        else
        {
            if (activeItem != null)
            {
                UpdateUnitsSelectedText(unitsSelected.Count, activeItem.targetCount);
            }
            else
            {
                UpdateUnitsSelectedText(0, 0);
            }
        }
    }

    private void UpdateUnitsSelectedText(int curUnitsSelected, int maxUnitsSelected)
    {
        curUnitsTargetedText.text = curUnitsSelected.ToString();
        maxUnitsTargetedText.text = maxUnitsSelected.ToString();

        if (curUnitsSelected == 0 && maxUnitsSelected == 0)
        {
            curUnitsTargetedText.text = "";
            maxUnitsTargetedText.text = "";
        }
    }

    public void ReduceAllEnemyHealth()
    {
        if (activeRoomEnemies.Count == 0)
            return;

        for (int i = 0; i < activeRoomEnemies.Count; i++)
        {
            activeRoomEnemies[i].UpdateUnitCurHealth(1, true, true);
        }
    }

    public void IncreaseAllEnemyHealth()
    {
        if (activeRoomEnemies.Count == 0)
            return;

        for (int i = 0; i < activeRoomEnemies.Count; i++)
        {
            activeRoomEnemies[i].UpdateUnitCurHealth(10000, false, true);
        }
    }

    public void ReduceAllPlayerHealth()
    {
        if (activeRoomHeroes.Count == 0)
            return;

        for (int i = 0; i < activeRoomHeroes.Count; i++)
        {
            activeRoomHeroes[i].UpdateUnitCurHealth(1, true, true);
        }
    }

    public void IncreaseAllPlayerHealth()
    {
        if (activeRoomHeroes.Count == 0)
            return;

        for (int i = 0; i < activeRoomHeroes.Count; i++)
        {
            activeRoomHeroes[i].UpdateUnitCurHealth(10000, false, true);
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
