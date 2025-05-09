using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    [SerializeField] UIElement buttonSkillsTabCombat;
    [SerializeField] UIElement buttonItemsTabCombat;
    [SerializeField] private List<FloorData> allFloors = new List<FloorData>();

    public UnitMapIcon unitMapIcon;
    public MapOverlay mapOverlay;
    [SerializeField] private UIElement mapRoomOverlay;
    [SerializeField] private Image mapSpawnBounds;
    [SerializeField] private Transform roomIconsParent;
    public List<float> startingRoomX = new List<float>();
    public List<float> endingRoomX = new List<float>();
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject pathPrefab;
    [SerializeField] private GameObject cameraFocusPoint;
    [SerializeField] private Transform savedFloorParents;
    [SerializeField] private float cameraMoveDist;
    //[SerializeField] private Image scrollImage;
    public UIElement map;
    public RoomMapIcon startingRoom;
    public RoomMapIcon endingRoom;
    [SerializeField] private Transform leftBorder;
    [SerializeField] private Transform rightBorder;
    [SerializeField] private Transform botBorder;
    [SerializeField] private Transform topBorder;

    [SerializeField] private List<GameObject> spawnedRoomsA = new List<GameObject>();
    [SerializeField] private List<GameObject> spawnedAllRooms = new List<GameObject>(); // List of spawned objects
    [SerializeField] private List<GameObject> spawnedAdditionalRooms = new List<GameObject>();
    [SerializeField] private List<GameObject> spawnedPaths = new List<GameObject>(); // List of spawned paths
    [SerializeField] private List<GameObject> spawnedAdditionalPaths = new List<GameObject>();
    [SerializeField] private List<GameObject> spawnedShopRooms = new List<GameObject>();
    [SerializeField] private List<GameObject> spawnedHeroRooms = new List<GameObject>();
    [SerializeField] private List<GameObject> spawnedItemRooms = new List<GameObject>();

    [SerializeField] private List<RoomMapIcon> rooms = new List<RoomMapIcon>();
    [SerializeField] private List<MapPath> mapPaths = new List<MapPath>();

    [SerializeField] private int maxFailedAttempts = 100;
    [SerializeField] private int failedCurAttempts;

    // The current number of objects that have been spawned
    [SerializeField] private int curRoomCount = 0;
    [SerializeField] private int curPathCount = 0;
    [SerializeField] private int curShopRoomCount = 0;
    [SerializeField] private int curBossRoomCount = 0;

    // The minimum distance to maintain between spawned objects
    public float minDistanceBetweenSpawnedObjects = 0.2f;

    public Sprite roomEnemySprite;
    public Sprite roomShopSprite;
    public Sprite roomHeroSprite;
    public Sprite roomItemSprite;
    public Sprite roomBossSprite;
    public Sprite roomStartingSprite;

    public Sprite detailHiddenSprite;

    public Vector2 roomEnemySize;
    public Vector2 roomShopSize;
    public Vector2 roomHeroSize;
    public Vector2 roomBossSize;
    public Vector2 roomStartingSize;
    public Vector2 roomSelectedSizeInc;
    public Color roomEnemyColour;
    public Color roomShopColour;
    public Color roomHeroColour;
    public Color roomItemColour;
    public Color roomBossColour;
    public Color roomStartingColour;
    public Color roomHiddenColour;
    public Color roomUndiscoveredColour;

    public Color roomSelectedClearedColour;
    public Color roomSelectedUnclearedColour;

    [HideInInspector]
    public FloorData activeFloor;
    public RoomMapIcon revealedRoom;
    public RoomMapIcon selectedRoom;

    public UIElement exitShopRoom;
    [SerializeField] private UIElement mapManager;


    // The minimum and maximum values for the x and y positions of the spawned objects
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private bool spawnedStartingRoom;

    private bool storedRoomData;

    bool resetOnce;

    public float endingYPos;
    private float startingYPos;
    private RectTransform rt;
    private float roomDistance;

    public Sprite arrowUpSprite;
    public Sprite arrowDownSprite;
    public Sprite invisSprite;
    int allowBossRoom = 0;

    private void Awake()
    {
        Instance = this;

        rt = GetComponent<RectTransform>();
        startingYPos = transform.position.y;
    }

    public void ToggleButtonSkillsTabCombat(bool toggle = true)
    {
        if (toggle)
        {
            buttonSkillsTabCombat.UpdateAlpha(1);
            buttonSkillsTabCombat.ToggleButton(true);
        }
        else
        {
            buttonSkillsTabCombat.UpdateAlpha(0);
            buttonSkillsTabCombat.ToggleButton(false);
        }
    }

    public void ToggleButtonItemsTabCombat(bool toggle = true)
    {
        if (toggle)
        {
            buttonItemsTabCombat.UpdateAlpha(1);
            buttonItemsTabCombat.ToggleButton(true);
        }
        else
        {
            buttonItemsTabCombat.UpdateAlpha(0);
            buttonItemsTabCombat.ToggleButton(false);
        }
    }

    void ResetMapYPos()
    {
        transform.position = new Vector3(transform.position.x, startingYPos, transform.position.z);     
    }

    void EndMapYPos()
    {
        transform.position = new Vector3(transform.position.x, endingYPos, transform.position.z);
    }

    void Start()
    {

    }

    public void RefocusCamera()
    {
        //Debug.Log(selectedRoom.name);
        //transform.position = new Vector3(transform.position.x, selectedRoom.transform.position.y, transform.position.z);
    }

    float GetDistanceBetweenMovingRoom()
    {
        return 0;
    }

    public void UpdateMapGoldText()
    {
        string goldString = ShopManager.Instance.GetPlayerGold().ToString();

        // Update shop and Map overlay gold counts
        //totalGoldText.UpdateContentText(goldString);
        mapOverlay.UpdatePlayerGoldText(goldString);
    }

    private void Update()
    {
        // Reset Game
        if (Input.GetKeyDown(KeyCode.M))
            ToggleMapVisibility(true, true);
        // Complete Room
        if (Input.GetKeyDown(KeyCode.N) && !CheckIfAnyHiddenMainRooms() && selectedRoom != startingRoom)
            ClearRoom(false, false, true);
        if (Input.GetKeyDown(KeyCode.V))
            ResetMap();
    }

    public void Setup()
    {
        OwnedLootInven.Instance.ResetOwnedGear();
        OwnedLootInven.Instance.ResetOwnedItems();

        OwnedLootInven.Instance.LoadStartingLoot();

        // Spawn Starter Ally
        GameManager.Instance.SpawnFighter();

        GameManager.Instance.UpdateAllyVisibility(false);

        GameManager.Instance.UpdateAllUnitsSorting(GameManager.Instance.unitTabSortingLevel);

        //GameManager.Instance.TriggerTransitionSequence();

        ToggleMapVisibility(true, true);

        // Disable player weapon input 
        GameManager.Instance.ToggleUIElementFull(GameManager.Instance.playerWeaponChild, false);

        GameManager.Instance.transitionSprite.AllowFadeOut();

        GameManager.Instance.transitionSprite.resetMap = false;

        startingRoomX.Add(startingRoom.gameObject.transform.localPosition.y);
        endingRoomX.Add(endingRoom.gameObject.transform.localPosition.y);
    }

    public bool CheckIfAnyHiddenMainRooms(int extra = 0)
    {
        if (rooms.Count == 0)
            return true;

        int count = 0;

        for (int i = 0; i < rooms.Count; i++)
        {
            if (!rooms[i].GetDiscovered() && !rooms[i].GetIsHidden())
            {
                if (rooms[i].isMainRoom)
                {
                    count++;
                    //Debug.Log(rooms[i]);
                    continue;
                }
            }
        }

        if (count >= 2 + extra)
            return true;
        else
            return false;
    }

    public void ClearRoom(bool tryShowAd = false, bool forceEnd = false, bool playerWon = false)
    {
        // Do not allow room clear if selected room is the starting room
        if (selectedRoom == startingRoom)
        {
            ShowConnectingRooms();
            return;
        }



        //mapOverlay.ToggleEnterRoomButton(false);

        if (RoomManager.Instance.GetFloorCount() >= RoomManager.Instance.highestFloorCountRun)
            ShowConnectingRooms();

        // If player won
        if (playerWon)
        {
            RoomManager.Instance.IncrementDefaultRoomsCleared();

            if (RoomManager.Instance.GetActiveRoom().GetRoomType() == RoomMapIcon.RoomType.BOSS)
            {
                RoomManager.Instance.FloorCompleted();
                GameManager.Instance.ToggleMap(true, true, true);

                startingRoomX.Add(startingRoom.gameObject.transform.localPosition.y);
                endingRoomX.Add(endingRoom.transform.localPosition.y);
            }

            if (RoomManager.Instance.GetActiveRoom().GetRoomType() != RoomMapIcon.RoomType.BOSS && RoomManager.Instance.GetActiveRoom().GetRoomType() != RoomMapIcon.RoomType.STARTING)
            {
                selectedRoom.UpdateIsCompleted(true);
                GameManager.Instance.ToggleMap(true, false);
            }
        }
        // If player lost
        else
        {
            // Hide foor increase alert
            RoomManager.Instance.HideFloorIncreaseAlert();

            ResetEntireRun(tryShowAd);

            return;
        }

        selectedRoom.ToggleRoomSelected(true);

        if (selectedRoom.curRoomType != RoomMapIcon.RoomType.STARTING)
            selectedRoom.UpdateRoomColour(roomSelectedClearedColour, true);

        if (forceEnd)
            ResetEntireRun(tryShowAd);

        mapOverlay.ToggleTeamPageButton(true);
    }

    public void ResetEntireRun(bool tryShowAd)
    {
        if (tryShowAd)
            AdManager.Instance.ShowSkippableAd();

        RoomManager.Instance.ResetFloorCount();

        GameManager.Instance.ToggleFighterCarasel(true);
        CharacterCarasel.Instance.ResetMenu();

        GameManager.Instance.ResetRoom();
        GameManager.Instance.ResetRoom(false);
        GameManager.Instance.activeTeam.Clear();
        GameManager.Instance.playerLost = false;
        GameManager.Instance.fallenHeroes.Clear();
        ShopManager.Instance.ResetPlayerGold();
        ShopManager.Instance.UpdatePlayerGold(ShopManager.Instance.playerStartingGold);
        ResetSavedFloors();

        startingRoomX.Clear();
        endingRoomX.Clear();

        //AudioManager.Instance.Play("Room_Lose");
    }

    public void UpdateActiveFloor(FloorData floor = null)
    {
        activeFloor = floor;

        if (floor != null)
            ToggleMapVisibility(true);
        else
            ToggleMapVisibility(false);
    }

    public void UpdateSelectedRoom(RoomMapIcon room)
    {
        if (selectedRoom != null)
        {
            if (selectedRoom.gameObject.transform.position.y > room.transform.position.y)
            {
                roomDistance = Mathf.Abs(selectedRoom.gameObject.transform.position.y - room.transform.position.y);
            }
            else
            {
                roomDistance = Mathf.Abs(room.transform.position.y - selectedRoom.gameObject.transform.position.y);
            }
        }

        ResetAllSelectedRooms();

        selectedRoom = room;
        selectedRoom.ToggleRoomSelected(true);

        // If selected room is not the starting room, refocus cam
       //if (room.GetRoomType() != RoomMapIcon.RoomType.STARTING)
        //RefocusCamera();

        if (selectedRoom.GetIsCompleted() && room.curRoomType != RoomMapIcon.RoomType.STARTING)
        {
            mapOverlay.UpdateEnterRoomButtonText("");
            selectedRoom.UpdateRoomColour(roomSelectedClearedColour, true);
        }

        if (!selectedRoom.GetIsCompleted() && room.curRoomType != RoomMapIcon.RoomType.STARTING)
        {
            mapOverlay.UpdateEnterRoomButtonText("ENTER ROOM");
            mapOverlay.UpdateEnterRoomButtonImage(false, true);
        }

        if (room.curRoomType == RoomMapIcon.RoomType.STARTING && RoomManager.Instance.GetFloorCount() != 1)
        {
            mapOverlay.UpdateEnterRoomButtonText("");
            mapOverlay.UpdateEnterRoomButtonImage(false);
        }

        if (selectedRoom.GetIsCompleted() && selectedRoom.curRoomType == RoomMapIcon.RoomType.BOSS)
        {
            selectedRoom.UpdateRoomColour(roomSelectedClearedColour, true);

            mapOverlay.ToggleEnterRoomButton(true);
            mapOverlay.UpdateEnterRoomButtonText("");
            mapOverlay.UpdateEnterRoomButtonImage(true);

            GameManager.Instance.map.mapOverlay.UpdateOverlayRoomName(selectedRoom.curRoomType);
        }
    }

    public void ResetAllSelectedRooms(bool decFloor = false)
    {
        bool bossAllowed = true;

        foreach (Transform child in savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1))
        {
            if (child.gameObject.GetComponent<RoomMapIcon>())
            {
                child.gameObject.GetComponent<RoomMapIcon>().ToggleRoomSelected(false);

                if (child.gameObject.GetComponent<RoomMapIcon>().curRoomSize == RoomMapIcon.RoomSize.MAIN)
                {
                    if (!child.gameObject.GetComponent<RoomMapIcon>().GetIsCompleted())
                    {

                        bossAllowed = false;
                    }
                }
            }
        }

        startingRoom.ToggleRoomSelected(false);

        endingRoom.ToggleRoomSelected(false);

        //Debug.Log("boss allowed : " + bossAllowed);

        if (bossAllowed)
        {
            //endingRoom.ToggleHiddenMode(false);

            if (decFloor)
                endingRoom.UpdateIsCompleted(true);
        }

        /*
        if (RoomManager.Instance.GetActiveRoom())
        {
            if (RoomManager.Instance.GetActiveRoom().curRoomType == RoomMapIcon.RoomType.STARTING)
                startingRoom.ToggleRoomSelected(true);
        }
        */
    }

    public void ResetMapYPosition()
    {
        mapManager.SetYPosition();
    }

    public void ToggleMapVisibility(bool toggle, bool generateMap = false, bool increaseFloor = false)
    {
        if (toggle)
        {
            StartCoroutine(PostBattle.Instance.ToggleButtonPostBattleMap(false));

            PostBattle.Instance.ToggleToMapButton(false);

            CombatGridManager.Instance.ToggleCombatSlotsInput(false);
            CombatGridManager.Instance.ToggleAllCombatSlotOutlines();

            CombatGridManager.Instance.ToggleCombatGrid(false);
            CombatGridManager.Instance.ToggleButtonAttackMovement(false);

            TeamGearManager.Instance.ClearEmptyGearSlots();
            TeamItemsManager.Instance.ClearEmptyItemSlots();


            ShopManager.Instance.ToggleShopItemsGameObject(false);

            ToggleButtonSkillsTabCombat(false);
            ToggleButtonItemsTabCombat(false);

            ShopManager.Instance.ToggleShopItemButtons(false);

            ShopManager.Instance.DisableShopUI();

            OverlayUI.Instance.ToggleItemRarityTextUI(false);
            OverlayUI.Instance.UpdateItemRarityText("");
            OverlayUI.Instance.ToggleActiveItemTriggerStatus(false);
            OverlayUI.Instance.UpdateActiveItemUseCountText(0);
            OverlayUI.Instance.UpdateActiveItemTriggerStatus(false);

            OverlayUI.Instance.UpdateActiveItemRaceSpecificIcon("");

            OverlayUI.Instance.ToggleFighterDetailsTab(false);
            TeamItemsManager.Instance.ToggleFighterRaceIcon(false); 
            TeamItemsManager.Instance.UpdateUnequiptItemAlert();
            TeamGearManager.Instance.UpdateUnequiptGearAlert();

            TeamItemsManager.Instance.playerInItemTab = false;

            GameManager.Instance.startingFighterChosen = false;

            //Debug.Log("toggling map on");
            // Display exp visual
            AudioManager.Instance.ToggleShopMusic(false);

            for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
            {
                // Hide level up text
                GameManager.Instance.activeRoomHeroes[i].ToggleTextAlert(false);
                GameManager.Instance.activeRoomHeroes[i].ToggleUnitExpVisual(false);

                GameManager.Instance.activeRoomHeroes[i].ToggleTooltipItems(false);
                GameManager.Instance.activeRoomHeroes[i].ToggleTooltipGear(false);
                GameManager.Instance.activeRoomHeroes[i].ToggleTooltipStats(false);

                //GameManager.Instance.activeRoomHeroes[i].ReloadItemUses();
            }

            ShopManager.Instance.TogglePlayerInShopRoom(false);
            ShopManager.Instance.ToggleFallenHeroPrompt(false);
            ShopManager.Instance.ToggleAllFallenHeroSelection(false);

            UpdateMapGoldText();

            //GameManager.Instance.SetHeroFormation();

            //GameManager.Instance.UpdateAllyVisibility(false);

            // Update unit map icon
            if (GameManager.Instance.activeRoomAllUnitFunctionalitys.Count != 0)
            {
                string unitName = GameManager.Instance.activeTeam[0].unitName;

                for (int i = 0; i < GameManager.Instance.activeRoomAllUnitFunctionalitys.Count; i++)
                {
                    if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetUnitName() == unitName)
                    {
                        unitMapIcon.UpdateUnitName(GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetUnitName());
                        unitMapIcon.UpdateIconAnimator(GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetAnimator().runtimeAnimatorController);
                    }
                }
            }

            SkillsTabManager.Instance.gearTabArrowLeftButton.ToggleButton(false);
            SkillsTabManager.Instance.gearTabArrowRightButton.ToggleButton(false);

            GameManager.Instance.ToggleAllyUnitSelection(false);


            GearRewards.Instance.ResetRewards(true);

            // If items didnt get wiped from hero unit spawning scene, wipe them here (it blocks mid screen taps)
            ItemRewardManager.Instance.ResetRewardsTable();

            //TeamSetup.Instance.ToggleToMapButton(false);
            // Disable unit's post battle bg
            int count = GameManager.Instance.activeRoomHeroes.Count;
            for (int i = 0; i < count; i++)
            {
                GameManager.Instance.activeRoomHeroes[i].ToggleUnitBG(false);
            }

            map.UpdateAlpha(1);

            SkillsTabManager.Instance.ToggleToMapButton(false);

            //ToggleMapScroll(true);

            // Disable end turn button
            GameManager.Instance.endTurnButtonUI.UpdateAlpha(0);

            if (generateMap)
            {
                GameManager.Instance.ResetRoom(true);
                GenerateMap(!increaseFloor);
            }

            // Toggle Map bottom overlay buttons
            if (RoomManager.Instance.GetActiveRoom().GetRoomType() == RoomMapIcon.RoomType.STARTING && RoomManager.Instance.GetFloorCount() == 1)
            {
                mapOverlay.ToggleEnterRoomButton(false);
                mapOverlay.ToggleTeamPageButton(true);
            }
            else if (RoomManager.Instance.GetActiveRoom().GetRoomType() == RoomMapIcon.RoomType.STARTING && RoomManager.Instance.GetFloorCount() != 1)
            {
                mapOverlay.ToggleEnterRoomButton(true);
                mapOverlay.ToggleTeamPageButton(true);
            }
            
            
            if (RoomManager.Instance.GetActiveRoom().GetRoomType() == RoomMapIcon.RoomType.ENEMY && !RoomManager.Instance.GetActiveRoom().GetIsCompleted())
                mapOverlay.ToggleEnterRoomButton(true);
            else if (RoomManager.Instance.GetActiveRoom().GetRoomType() == RoomMapIcon.RoomType.ENEMY && RoomManager.Instance.GetActiveRoom().GetIsCompleted())
                mapOverlay.ToggleEnterRoomButton(false);
            else if (RoomManager.Instance.GetActiveRoom().GetRoomType() == RoomMapIcon.RoomType.SHOP)
                mapOverlay.ToggleEnterRoomButton(true);
            

            int diffCount = RoomManager.Instance.GetFloorCount();
            mapOverlay.UpdateRoomCountText(diffCount.ToString());
            mapOverlay.UpdateFloorNameText(activeFloor.floorName, activeFloor.floorColour);

            for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
            {
                //Debug.Log("Turning off unit dispslay");
                GameManager.Instance.activeRoomHeroes[i].ToggleUnitDisplay(false);
            }

            GameManager.Instance.SetHeroFormation();


            //GameManager.Instance.transitionSequienceUI.UpdateAlpha(0);

            mapOverlay.UpdateRoomDifficultyIcons();
        }
        else
        {
            //GameManager.Instance.UpdateAllyVisibility(true);

            map.UpdateAlpha(0);
            //ToggleMapScroll(false);
        }
    }

    public void UpdateEnterRoomButton()
    {
        if (RoomManager.Instance.GetActiveRoom().GetRoomType() == RoomMapIcon.RoomType.STARTING && RoomManager.Instance.GetFloorCount() == 1)
            mapOverlay.ToggleEnterRoomButton(false);
        else if (RoomManager.Instance.GetActiveRoom().GetRoomType() == RoomMapIcon.RoomType.STARTING && RoomManager.Instance.GetFloorCount() != 1)
            mapOverlay.ToggleEnterRoomButton(true);
        else if (RoomManager.Instance.GetActiveRoom().GetRoomType() != RoomMapIcon.RoomType.STARTING
            && !RoomManager.Instance.GetActiveRoom().GetIsCompleted())
        {
            mapOverlay.ToggleEnterRoomButton(true);
        }
        else if (RoomManager.Instance.GetActiveRoom().GetRoomType() != RoomMapIcon.RoomType.STARTING
    && RoomManager.Instance.GetActiveRoom().GetIsCompleted() && RoomManager.Instance.GetActiveRoom().GetRoomType() != RoomMapIcon.RoomType.BOSS)
        {
            mapOverlay.ToggleEnterRoomButton(false);
        }
    }
  
    Vector2 GetRoomSpawnRandomPos()
    {
        //Bounds bounds = mapSpawnBounds.GetComponent<BoxCollider2D>().bounds;
        //Debug.Log(bounds);
        //float offsetX = Random.Range(-bounds.extents.x/2.75f, bounds.extents.x/2.75f);
        //float offsetY = Random.Range(-bounds.extents.y/2.75f, bounds.extents.y/2.75f);

        float offsetX = Random.Range(leftBorder.position.x, rightBorder.position.x);
        float offsetY = Random.Range(botBorder.gameObject.transform.position.y, topBorder.gameObject.transform.position.y);

        Vector2 newPos = new Vector3(offsetX, offsetY, 0);
        return newPos;
    }

    void UpdateStartEndRoomHorizontalPos(float minX, float maxX)
    {
        startingRoom.UpdateHorizontalPos(minX, maxX);
        endingRoom.UpdateHorizontalPos(minX, maxX);
    }

    void UpdateRoomZPosition(Transform trans)
    {
        trans.localPosition = new Vector3(trans.localPosition.x, trans.localPosition.y, 0);
    }

    void ResetMap()
    {
        ResetMapYPosition();

        ResetMapYPos();

        spawnedStartingRoom = false;
        spawnedRoomsA.Clear();
        spawnedAllRooms.Clear();
        spawnedAdditionalRooms.Clear();
        spawnedPaths.Clear();
        spawnedAdditionalPaths.Clear();
        spawnedShopRooms.Clear();
        spawnedHeroRooms.Clear();
        spawnedItemRooms.Clear();

        startingRoom.ResetLinkedPaths();
        startingRoom.ResetLinkedRooms();

        endingRoom.ResetLinkedPaths();
        endingRoom.ResetLinkedRooms();

        rooms.Clear();
        mapPaths.Clear();

        failedCurAttempts = 0;
        curRoomCount = 0;
        curPathCount = 0;

        storedRoomData = false;

        foreach (Transform child in roomIconsParent)
        {
            Destroy(child.gameObject);
        }

        activeFloor = null;

        if (allFloors.Count != 0)
            activeFloor = allFloors[0];
    }

    public void GenerateMap(bool resetting = true)
    {
        // Player lost 
        if (resetting)
        {

        }
        // Beat the floor, resetting into floor increase
        //else
            //oomManager.Instance.IncreaseMaxRoomCount();

        //ShopManager.Instance.ResetPlayerGold();
        ResetMap();
        SpawnRoomGenerationA();
        GenerationPathsA();

        // Additional rooms / paths are booken
        SpawnRoomGenerationB();
        GenerationPathsB();

        StoreRooms();

        // Check if map is impossible
        CheckIfMapIsPossible(resetting);

        SaveFloor();

        ToggleHiddenModeRoom(true);

        UpdateStartingRoomAndPath();

        // Updating unit map icon starting position
        unitMapIcon.UpdateUnitPosition(startingRoom.transform.localPosition);

        SetPathsZvalue();
    }

    void SaveFloor()
    {
        foreach (Transform child in roomIconsParent)
        {
            child.SetParent(savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1));
        }

        foreach (Transform child in roomIconsParent)
        {
            child.SetParent(savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1));
        }

        for (int i = 0; i < 50; i++)
        {
            if (roomIconsParent.childCount >= 1)
            {
                if (roomIconsParent.GetChild(0))
                    roomIconsParent.GetChild(0).SetParent(savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1));
            }
            else
                break;
        }

        if (RoomManager.Instance.GetFloorCount() - 2 > -1)
            savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 2).gameObject.GetComponent<UIElement>().UpdateAlpha(0);

        if (RoomManager.Instance.GetFloorCount() - 2 > -1)
        {
            for (int i = 0; i < savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 2).childCount; i++)
            {
                if (savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 2).GetChild(i).gameObject.GetComponent<MapPath>())
                    savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 2).GetChild(i).gameObject.GetComponent<MapPath>().TogglePathVisibility(false);
            }
        }

        if (RoomManager.Instance.GetFloorCount() - 2 > -1)
        {
            for (int i = 0; i < savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 2).transform.childCount; i++)
            {
                if (savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 2).transform.GetChild(i).gameObject.GetComponent<CanvasGroup>())
                {
                    savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 2).transform.GetChild(i).gameObject.GetComponent<CanvasGroup>().interactable = false;
                    savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 2).transform.GetChild(i).gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
                }
            }
        }

        savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount()-1).gameObject.GetComponent<UIElement>().UpdateAlpha(1);
    }

    public void ResetSavedFloors()
    {
        for (int i = 0; i < savedFloorParents.childCount; i++)
        {
            for (int x = 0; x < savedFloorParents.GetChild(i).childCount; x++)
            {
                Destroy(savedFloorParents.GetChild(i).GetChild(x).gameObject);
            }
        }

        savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount()).gameObject.GetComponent<UIElement>().UpdateAlpha(1);
    }

    public void LoadFutureSavedFloor()
    {
        RoomManager.Instance.IncrementFloorCount();

        savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).gameObject.GetComponent<UIElement>().UpdateAlpha(1);

        for (int i = 0; i < savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).transform.childCount; i++)
        {
            if (savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).transform.GetChild(i).gameObject.GetComponent<CanvasGroup>())
            {
                savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).transform.GetChild(i).gameObject.GetComponent<CanvasGroup>().interactable = true;
                savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).transform.GetChild(i).gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }


        savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 2).gameObject.GetComponent<UIElement>().UpdateAlpha(0);

        for (int i = 0; i < savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 2).transform.childCount; i++)
        {
            if (savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 2).transform.GetChild(i).gameObject.GetComponent<CanvasGroup>())
            {
                savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 2).transform.GetChild(i).gameObject.GetComponent<CanvasGroup>().interactable = false;
                savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 2).transform.GetChild(i).gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }

        int diffCount = RoomManager.Instance.GetFloorCount();
        mapOverlay.UpdateRoomCountText(diffCount.ToString());

        mapOverlay.UpdateFloorNameText(activeFloor.floorName, activeFloor.floorColour);

        ResetAllSelectedRooms(true);

        startingRoom.transform.localPosition = new Vector3(startingRoom.transform.localPosition.x, startingRoomX[RoomManager.Instance.GetFloorCount()-1], 0);
        endingRoom.transform.localPosition = new Vector3(endingRoom.transform.localPosition.x, endingRoomX[RoomManager.Instance.GetFloorCount()-1], 0);

        // Updating unit map icon starting position
        unitMapIcon.UpdateUnitPosition(startingRoom.transform.localPosition);

        ResetMapYPos();

        // unelect ending 
        endingRoom.ToggleRoomSelected(false);

        // select starting room
        startingRoom.ToggleRoomSelected(true);

        UpdateSelectedRoom(startingRoom);


        for (int i = 0; i < savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 2).childCount; i++)
        {
            if (savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 2).GetChild(i).gameObject.GetComponent<MapPath>())
                savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 2).GetChild(i).gameObject.GetComponent<MapPath>().TogglePathVisibility(false);
        }

        for (int i = 0; i < savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).childCount; i++)
        {
            if (savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).GetChild(i).gameObject.GetComponent<MapPath>())
            {
                if (savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).GetChild(i).gameObject.GetComponent<MapPath>().isRevealed)
                    savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).GetChild(i).gameObject.GetComponent<MapPath>().TogglePathVisibility(true);
            }
        }

        RoomManager.Instance.UpdateActiveRoom(startingRoom);

        allowBossRoom = 0;

        // Determine if boss room should be unhidden, and uncompleted
        for (int i = 0; i < savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).childCount; i++)
        {
            if (savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).gameObject.transform.GetChild(i).gameObject.GetComponent<RoomMapIcon>())
            {
                if (savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).gameObject.transform.GetChild(i).gameObject.GetComponent<RoomMapIcon>().curRoomSize == RoomMapIcon.RoomSize.MAIN
                    && savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).gameObject.transform.GetChild(i).gameObject.GetComponent<RoomMapIcon>().curRoomType != RoomMapIcon.RoomType.STARTING
                    && savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).gameObject.transform.GetChild(i).gameObject.GetComponent<RoomMapIcon>().curRoomType != RoomMapIcon.RoomType.BOSS)
                {
                    if (savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).gameObject.transform.GetChild(i).gameObject.GetComponent<RoomMapIcon>().GetIsCompleted())
                    {

                    }
                    else
                    {
                        allowBossRoom++;
                        Debug.Log("room name that 'isnt' completed " + savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).gameObject.transform.GetChild(i).gameObject.name + " Floor: " + savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).gameObject.transform.GetChild(i).gameObject.transform.parent.name);
                    }
                }
            }
        }

        if (allowBossRoom == 0)
        {
            endingRoom.ToggleDiscovered(true);
            endingRoom.ToggleHiddenMode(false);
            endingRoom.UpdateRoomVisuals(RoomMapIcon.RoomType.BOSS);
            //Debug.Log("3");
        }
        else
        {
            endingRoom.ToggleDiscovered(false);
            endingRoom.ToggleHiddenMode(true);
            //Debug.Log("toggling hidden mode off for ending room");
            endingRoom.UpdateIsCompleted(false);
        }

        if (RoomManager.Instance.highestFloorCountRun <= RoomManager.Instance.GetFloorCount())
        {
            endingRoom.UpdateIsCompleted(false);
        }

        /*
        for (int i = 0; i < endingRoom.linkedPaths.Count; i++)
        {
            if (!endingRoom.GetIsCompleted())
                endingRoom.linkedPaths[i].TogglePathVisibility(false);
        }
        */

        mapOverlay.UpdateRoomTypeText("");
        mapOverlay.UpdateRoomSubText("");

        mapOverlay.UpdateRoomDifficultyIcons();
    }

    public bool CheckToEnableHardRoomPrompt()
    {
        allowBossRoom = 0;

        // Determine if boss room should be unhidden, and uncompleted
        for (int i = 0; i < savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).childCount; i++)
        {
            if (savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).gameObject.transform.GetChild(i).gameObject.GetComponent<RoomMapIcon>())
            {
                if (savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).gameObject.transform.GetChild(i).gameObject.GetComponent<RoomMapIcon>().curRoomType == RoomMapIcon.RoomType.ENEMY
                    && savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).gameObject.transform.GetChild(i).gameObject.GetComponent<RoomMapIcon>().GetDiscovered())
                {
                    if (savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).gameObject.transform.GetChild(i).gameObject.GetComponent<RoomMapIcon>().GetIsCompleted())
                    {

                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
    public void LoadPreviousFloor()
    {
        RoomManager.Instance.DecreaseFloorCount();

        if (RoomManager.Instance.GetFloorCount() > -1)
            savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount()).gameObject.GetComponent<UIElement>().UpdateAlpha(0);

        for (int i = 0; i < savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount()).transform.childCount; i++)
        {
            if (savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount()).transform.GetChild(i).gameObject.GetComponent<CanvasGroup>())
            {
                savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount()).transform.GetChild(i).gameObject.GetComponent<CanvasGroup>().interactable = false;
                savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount()).transform.GetChild(i).gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }


        if (RoomManager.Instance.GetFloorCount() > -1)
            savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).gameObject.GetComponent<UIElement>().UpdateAlpha(1);

        for (int i = 0; i < savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).transform.childCount; i++)
        {
            if (savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).transform.GetChild(i).gameObject.GetComponent<CanvasGroup>())
            {
                savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).transform.GetChild(i).gameObject.GetComponent<CanvasGroup>().interactable = true;
                savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).transform.GetChild(i).gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }

        int diffCount = RoomManager.Instance.GetFloorCount();
        mapOverlay.UpdateRoomCountText(diffCount.ToString());

        mapOverlay.UpdateFloorNameText(activeFloor.floorName, activeFloor.floorColour);

        ResetAllSelectedRooms(true);

        startingRoom.transform.localPosition = new Vector3(startingRoom.transform.localPosition.x, startingRoomX[RoomManager.Instance.GetFloorCount() - 1], 0);
        endingRoom.transform.localPosition = new Vector3(endingRoom.transform.localPosition.x, endingRoomX[RoomManager.Instance.GetFloorCount() - 1], 0);

        // Updating unit map icon starting position
        unitMapIcon.UpdateUnitPosition(endingRoom.transform.localPosition);

        EndMapYPos();

        // unelect ending 
        endingRoom.ToggleRoomSelected(true);

        // select starting room
        startingRoom.ToggleRoomSelected(false);

        UpdateSelectedRoom(endingRoom);

        if (RoomManager.Instance.GetFloorCount() > -1)
        {
            for (int i = 0; i < savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount()).childCount; i++)
            {
                if (savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount()).GetChild(i).gameObject.GetComponent<MapPath>())
                {
                    //if (savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 2).GetChild(i).gameObject.GetComponent<MapPath>().isRevealed)
                    savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount()).GetChild(i).gameObject.GetComponent<MapPath>().TogglePathVisibility(false);
                }
            }
        }

        for (int i = 0; i < savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount()-1).childCount; i++)
        {
            if (savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount()-1).GetChild(i).gameObject.GetComponent<MapPath>())
            {
                if (savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).GetChild(i).gameObject.GetComponent<MapPath>().isRevealed)
                    savedFloorParents.GetChild(RoomManager.Instance.GetFloorCount() - 1).GetChild(i).gameObject.GetComponent<MapPath>().TogglePathVisibility(true);
            }
        }

        RoomManager.Instance.UpdateActiveRoom(endingRoom);

        mapOverlay.UpdateRoomDifficultyIcons();

        endingRoom.ToggleHiddenMode(false);
    }

    void CheckIfMapIsPossible(bool resetting = true)
    {
        //Debug.Log("resetting");
        // If there is not enough shops on the floor, remake it
        if (activeFloor.shopRoomCount != 0)
        {
            if (spawnedShopRooms.Count == 0)
            {
                ToggleMapVisibility(true, true, resetting);
                return;
            }
        }

        // If there is not at least 1 hero room in floor, remake it
        if (spawnedHeroRooms.Count == 0)
        {
            ToggleMapVisibility(true, true, resetting);
            return;
        }

        // If there is not at least 1 item room in floor, remake it
        if (activeFloor.itemRoomCount != 0)
        {
            if (spawnedItemRooms.Count == 0)
            {
                ToggleMapVisibility(true, true, resetting);
                return;
            }
        }

        // Check if starting room has possible linked rooms, if not remake it 
        for (int i = 0; i < startingRoom.GetLinkedRooms().Count; i++)
        {
            if (startingRoom.GetLinkedRooms()[i].curRoomSize == RoomMapIcon.RoomSize.SIDE)
            {
                ToggleMapVisibility(true, true, resetting);
                return;
            }
        }

        // Check if ending room has possible linked rooms, if not remake it 
        for (int i = 0; i < endingRoom.GetLinkedRooms().Count; i++)
        {
            if (endingRoom.GetLinkedRooms()[i].curRoomSize == RoomMapIcon.RoomSize.SIDE)
            {
                ToggleMapVisibility(true, true, resetting);
                return;
            }
        }

        SetPathsZvalue();
    }

    void StoreRooms()
    {
        if (!storedRoomData)
        {
            storedRoomData = true;

            for (int i = 0; i < spawnedAllRooms.Count; i++)
            {
                rooms.Add(spawnedAllRooms[i].GetComponent<RoomMapIcon>());
            }

            rooms.Add(startingRoom);
            rooms.Add(endingRoom);

            for (int i = 0; i < spawnedPaths.Count; i++)
            {
                mapPaths.Add(spawnedPaths[i].GetComponent<MapPath>());
            }
        }
    }
    void UpdateStartingRoomAndPath()
    {
        ToggleFirstPath(true);
        //startingRoom.UpdateIsCompleted(true);
        startingRoom.ToggleRoomSelected(true);
        startingRoom.UpdateRoomColour(roomSelectedClearedColour);

        UpdateSelectedRoom(startingRoom);

        mapOverlay.UpdateOverlayRoomName(RoomMapIcon.RoomType.STARTING);

        RoomManager.Instance.UpdateActiveRoom(startingRoom);

        // Set starting room
        UpdateRoomIconType(startingRoom, "starting");
    }

    public void UpdateRevealedMainRoom(RoomMapIcon room)
    {
        revealedRoom = room;
    }

    public void HideConnectingRooms()
    {
        // Enable next paths, which then enable each of their rooms
        List<MapPath> linkedPaths = selectedRoom.GetLinkedPaths();
        for (int x = 0; x < linkedPaths.Count; x++)
        {
            linkedPaths[x].ToggleConnectingRoomsDiscovered(false);
        }
    }

    public void ShowConnectingRooms()
    {
        // Enable next paths, which then enable each of their rooms
        List<MapPath> linkedPaths = selectedRoom.GetLinkedPaths();
        for (int x = 0; x < linkedPaths.Count; x++)
        {
            if (linkedPaths[x].belongsToFloorCount == RoomManager.Instance.GetFloorCount())
                linkedPaths[x].ToggleConnectingRoomsDiscovered(true);
        }
    }

    void ToggleFirstPath(bool toggle)
    {
        MapPath startingPath = spawnedPaths[0].GetComponent<MapPath>();
        if (toggle)
            startingPath.ToggleHiddenMode(false);
        else
            startingPath.ToggleHiddenMode(true);

        startingRoom.ToggleDiscovered(true);
    }

    void ToggleHiddenModeRoom(bool toggle)
    {
        for (int i = 0; i < spawnedAllRooms.Count; i++)
        {
            spawnedAllRooms[i].GetComponent<RoomMapIcon>().ToggleHiddenMode(toggle);  // toggle cover mode on
        }
        for (int i = 0; i < spawnedPaths.Count; i++)
        {
            spawnedPaths[i].GetComponent<MapPath>().ToggleHiddenMode(toggle);   // Hide all paths
        }

        //Debug.Log("1");
        endingRoom.ToggleHiddenMode(true);
    }

    void SpawnRoomGenerationA()
    {
        // Calculate room count
        int min = activeFloor.minRoomCount + (RoomManager.Instance.GetFloorCount() * 2) -2;
        int max = activeFloor.maxRoomCount + (RoomManager.Instance.GetFloorCount() * 2) - 2;
        int roomSpawnCount = Random.Range(min, max+1);

        if (roomSpawnCount > RoomManager.Instance.floorMaxRoomCount * (RoomManager.Instance.GetFloorCount() * 2) - 2)
            roomSpawnCount = RoomManager.Instance.floorMaxRoomCount * (RoomManager.Instance.GetFloorCount() * 2) - 2;

        if (roomSpawnCount >= RoomManager.Instance.floorMaxMainRoomTotalCount)
            roomSpawnCount = RoomManager.Instance.floorMaxMainRoomTotalCount;

        UpdateStartEndRoomHorizontalPos(leftBorder.position.x, rightBorder.position.x);

        // Spawn RoomGenerationA
        for (int i = 0; i < roomSpawnCount; i++)
        {
            // Check if the spawned object is too close to any other objects
            bool isTooClose = false;
            float distance = 0;

            if (failedCurAttempts <= maxFailedAttempts)
            {
                Vector2 randomPos = GetRoomSpawnRandomPos();

                foreach (GameObject obj in spawnedAllRooms)
                {
                    distance = Vector2.Distance(obj.transform.position, randomPos);

                    if (distance < minDistanceBetweenSpawnedObjects)
                    {
                        //Debug.Log(distance);
                        isTooClose = true;
                        failedCurAttempts++;
                        //Debug.Log("DIDNT Spawn FROM ROOM" + distance);
                        //if (i != 0)
                        //    i--;

                        //Destroy(room);
                        break;
                    }
                }

                if (isTooClose)
                    continue;

                // Instantiate the object prefab
                GameObject room = Instantiate(roomPrefab);

                // Set the position of the spawned object
                room.transform.SetParent(roomIconsParent);
                room.transform.localScale = new Vector2(1, 1);

                room.transform.position = randomPos;

                UpdateRoomZPosition(room.transform);

                // If the spawned object is not too close to any other objects, spawn another till capped
                if (!isTooClose)
                {
                    //Debug.Log(room.gameObject.name + " " + distance);
                    // Update Room Icon 
                    RoomMapIcon roomMapIcon = room.GetComponent<RoomMapIcon>();
                    UpdateRoomIconType(roomMapIcon, "enemy");

                    // Add the spawned object to the list
                    spawnedAllRooms.Add(room);
                    spawnedRoomsA.Add(room);
                    curRoomCount++;
                    room.name = "Room - Main " + curRoomCount;
                    roomMapIcon.UpdateRoomSize(RoomMapIcon.RoomSize.MAIN);
                    continue;
                }

                //Debug.Log("Failed Current Attempts: " + failedCurAttempts);
            }
        }

        //Debug.Log("Gen A " + failedCurAttempts);

        spawnedRoomsA.Add(startingRoom.gameObject);
        spawnedRoomsA.Add(endingRoom.gameObject);
        spawnedAllRooms.Add(startingRoom.gameObject);
        spawnedAllRooms.Add(endingRoom.gameObject);

        spawnedAllRooms.Sort(CompareRoomYValue);
        spawnedRoomsA.Sort(CompareRoomYValue);
    }

    void SpawnRoomGenerationB()
    {
        #region Spawn Side Rooms
        failedCurAttempts = 0;

        int roomSpawnRoundB = activeFloor.sideRoomAmount * (RoomManager.Instance.GetFloorCount() * 2) - (2 * RoomManager.Instance.GetFloorCount());

        if (roomSpawnRoundB > RoomManager.Instance.floorMaxRoomCount * (RoomManager.Instance.GetFloorCount() * 2) - (2 * RoomManager.Instance.GetFloorCount()))
            roomSpawnRoundB = RoomManager.Instance.floorMaxRoomCount * (RoomManager.Instance.GetFloorCount() * 2) - (2 * RoomManager.Instance.GetFloorCount());

        if (roomSpawnRoundB >= RoomManager.Instance.floorMaxSideRoomTotalCount)
            roomSpawnRoundB = RoomManager.Instance.floorMaxSideRoomTotalCount;

        RoomMapIcon roomMapIcon;

        //Debug.Log(failedCurAttempts);
        for (int i = 0; i < roomSpawnRoundB; i++)
        {
            if (failedCurAttempts <= maxFailedAttempts)
            {
                Vector2 randomPos = GetRoomSpawnRandomPos();

                // Check if the spawned object is too close to any other objects
                bool isTooClose = false;
                float distance;

                foreach (GameObject obj in spawnedAllRooms)
                {
                    distance = Vector2.Distance(obj.transform.position, randomPos);

                    if (distance < minDistanceBetweenSpawnedObjects)
                    {
                        isTooClose = true;
                        failedCurAttempts++;
                        //Debug.Log("DIDNT Spawn FROM SIDE ROOM " + distance);
                        if (i != 0)
                            i--;

                        //Destroy(sideRoom);
                        break;
                    }
                }

                if (isTooClose)
                    continue;

                // working for reg rooms, gotta make sure other 2work ( chaisn and side rooms)
                if (!isTooClose)
                {
                    foreach (GameObject obj in spawnedPaths)
                    {
                        MapPath mapPath = obj.GetComponent<MapPath>();

                        distance = Vector2.Distance(mapPath.middleOfPath.transform.position, randomPos);

                        if (distance < minDistanceBetweenSpawnedObjects)
                        {
                            isTooClose = true;
                            failedCurAttempts++;
                            //Debug.Log("DIDNT Spawn FROM PATH " + distance);
                            if (i != 0)
                                i--;

                            //Destroy(sideRoom);
                            break;
                        }
                    }

                    if (isTooClose)
                        continue;
                }

                // Instantiate the object prefab
                GameObject sideRoom = Instantiate(roomPrefab);

                // Set the position of the spawned object
                sideRoom.transform.SetParent(roomIconsParent);
                sideRoom.transform.localScale = new Vector2(1, 1);
                //room.transform.position = new Vector3(randomPos.x, randomPos.y, 0);

                sideRoom.transform.position = randomPos;

                UpdateRoomZPosition(sideRoom.transform);

                // If the spawned object is not too close to any other objects, spawn another till capped
                if (!isTooClose)
                {
                    // Update Room Icon 
                    roomMapIcon = sideRoom.GetComponent<RoomMapIcon>();
                    UpdateRoomIconType(roomMapIcon, "enemy");

                    // Add the spawned object to the list
                    spawnedAllRooms.Add(sideRoom);
                    spawnedAdditionalRooms.Add(sideRoom);
                    curRoomCount++;
                    sideRoom.name = "Room - Side " + curRoomCount;
                    roomMapIcon.UpdateRoomSize(RoomMapIcon.RoomSize.SIDE);
                    continue;
                }
            }
        }

        //Debug.Log("Gen B " + failedCurAttempts);

        spawnedAllRooms.Sort(CompareRoomYValue);
        spawnedAdditionalRooms.Sort(CompareRoomYValue);

        // Set starting room
        //UpdateRoomIconType(startingRoom, "starting");

        failedCurAttempts = 0;
        int origShopRoomCount = Random.Range(activeFloor.shopRoomCount, activeFloor.shopRoomCount);
        int shopRoomCount = origShopRoomCount + (RoomManager.Instance.GetFloorCount() - 1);



        if (shopRoomCount > RoomManager.Instance.floorMaxShopRoomCount)
            shopRoomCount = RoomManager.Instance.floorMaxShopRoomCount;

        if (origShopRoomCount != 0)
        {
            // Set shop rooms
            for (int i = 0; i < shopRoomCount; i++)
            {
                // Make enough shops from the additional room spawns, until enough has been hit for floor
                int rand = Random.Range(0, spawnedAdditionalRooms.Count - 1);

                RoomMapIcon roomIcon = null;

                if (rand < spawnedAdditionalRooms.Count)
                    roomIcon = spawnedAdditionalRooms[rand].GetComponent<RoomMapIcon>();
                else
                {
                    failedCurAttempts++;
                    continue;
                }

                if (failedCurAttempts <= maxFailedAttempts)
                {
                    /*
                    // If there is already a shop room, skip it and reset so it can do it again
                    if (roomIcon.GetRoomType() == RoomMapIcon.RoomType.HERO || roomIcon.GetRoomType() == RoomMapIcon.RoomType.ITEM)
                    {
                        //i--;
                        //if (i < 0)
                        //    i = 0;
                        failedCurAttempts++;
                        continue;
                    }
                    */
                    // If room is not a shop, make it a shop
                    roomIcon.UpdateRoomType(RoomMapIcon.RoomType.SHOP);
                    UpdateRoomIconType(spawnedAdditionalRooms[rand].GetComponent<RoomMapIcon>(), "shop");
                    spawnedShopRooms.Add(spawnedAdditionalRooms[rand]);
                }
            }
        }
        

        int heroRoomCount = Random.Range(activeFloor.heroRoomCount, activeFloor.heroRoomCount + (RoomManager.Instance.GetFloorCount() - 1));

        // Set Hero rooms
        for (int i = 0; i < heroRoomCount; i++)
        {
            // Make enough hero rooms from the additional room spawns, until enough has been hit for floor
            int rand = Random.Range(0, spawnedAdditionalRooms.Count - 1);

            RoomMapIcon roomIcon = spawnedAdditionalRooms[rand].GetComponent<RoomMapIcon>();

            if (failedCurAttempts <= maxFailedAttempts)
            {
                // If there is already a shop room, skip it and reset so it can do it again
                if (roomIcon.GetRoomType() == RoomMapIcon.RoomType.SHOP)
                {
                    //i--;
                    //if (i < 0)
                    //    i = 0;
                    failedCurAttempts++;
                    continue;
                }

                // If room is not a hero room, make it a hero room
                roomIcon.UpdateRoomType(RoomMapIcon.RoomType.HERO);
                UpdateRoomIconType(spawnedAdditionalRooms[rand].GetComponent<RoomMapIcon>(), "hero");
                spawnedHeroRooms.Add(spawnedAdditionalRooms[rand]);
            }
        }

        int itemRoomCount = activeFloor.itemRoomCount + (RoomManager.Instance.GetFloorCount()-1);

        // Set Item rooms
        for (int i = 0; i < itemRoomCount; i++)
        {
            // Make enough item rooms from the additional room spawns, until enough has been hit for floor
            int rand = Random.Range(0, spawnedAdditionalRooms.Count - 1);

            RoomMapIcon roomIcon = spawnedAdditionalRooms[rand].GetComponent<RoomMapIcon>();

            if (failedCurAttempts <= maxFailedAttempts)
            {
                // If there is already an item room, skip it and reset so it can do it again
                if (roomIcon.GetRoomType() == RoomMapIcon.RoomType.HERO || roomIcon.GetRoomType() == RoomMapIcon.RoomType.SHOP)
                {
                    //i--;
                    //if (i < 0)
                    //    i = 0;
                    failedCurAttempts++;
                    continue;
                }

                // If room is not a hero room, make it a hero room
                roomIcon.UpdateRoomType(RoomMapIcon.RoomType.ITEM);
                UpdateRoomIconType(spawnedAdditionalRooms[rand].GetComponent<RoomMapIcon>(), "item");
                spawnedItemRooms.Add(spawnedAdditionalRooms[rand]);
            }
        }
       
        // Set boss room
        UpdateRoomIconType(endingRoom, "boss");
        #endregion
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.O))
            SetPathsZvalue();
    }
    public void SetPathsZvalue()
    {
        for (int i = 0; i < mapPaths.Count; i++)
        {
            mapPaths[i].gameObject.GetComponent<RectTransform>().localPosition = new Vector3(mapPaths[i].gameObject.GetComponent<RectTransform>().localPosition.x, mapPaths[i].gameObject.GetComponent<RectTransform>().position.y, 0);
        }
    }

    void GenerationPathsA()
    {
        for (int i = 0; i < spawnedRoomsA.Count; i++)
        {
            GameObject go = Instantiate(pathPrefab, roomIconsParent.position, Quaternion.identity);
            go.name = "Path - Main " + curPathCount;
            spawnedPaths.Add(go);
            UpdateSpawnedPaths();
            go.transform.SetParent(roomIconsParent, true);
            //go.transform.localScale = Vector3.one;
            go.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);

            curPathCount++;
           
            MapPath mapPath = go.GetComponent<MapPath>();

            mapPath.belongsToFloorCount = RoomManager.Instance.GetFloorCount();

            // If this is the final path of the map (to the boss level)
            if (i == spawnedRoomsA.Count-1)
            {
                mapPath.UpdateMapPath(spawnedRoomsA[i].transform.position, endingRoom.transform.position);
                mapPath.UpdateStartingRoom(spawnedRoomsA[i].GetComponent<RoomMapIcon>());
                mapPath.AddGoalRooms(endingRoom);

                RoomMapIcon curRoom = spawnedRoomsA[i].GetComponent<RoomMapIcon>();
                RoomMapIcon nextRoom = endingRoom;

                // Update current room and next room to reference eachother as a nearby room
                nextRoom.UpdateRoomMapLinks(curRoom);
                curRoom.UpdateRoomMapLinks(nextRoom);

                curRoom.UpdateRoomPathLinks(mapPath);
                nextRoom.UpdateRoomPathLinks(mapPath);

                curRoom.isMainRoom = true;
            }
            // If its not ^
            else
            {
                mapPath.UpdateMapPath(spawnedRoomsA[i].transform.position, spawnedRoomsA[i + 1].transform.position);
                mapPath.UpdateStartingRoom(spawnedRoomsA[i].GetComponent<RoomMapIcon>());
                mapPath.AddGoalRooms(spawnedRoomsA[i + 1].GetComponent<RoomMapIcon>());

                RoomMapIcon curRoom = spawnedRoomsA[i].GetComponent<RoomMapIcon>();
                RoomMapIcon nextRoom = spawnedRoomsA[i + 1].GetComponent<RoomMapIcon>();

                // Update current room and next room to reference eachother as a nearby room
                nextRoom.UpdateRoomMapLinks(curRoom);
                curRoom.UpdateRoomMapLinks(nextRoom);

                curRoom.UpdateRoomPathLinks(mapPath);
                nextRoom.UpdateRoomPathLinks(mapPath);

                curRoom.isMainRoom = true;
            }

            spawnedPaths.Sort(ComparedMiddleOfPathY);
            UpdateSpawnedPaths();

            go.GetComponent<RectTransform>().localPosition = new Vector3(go.GetComponent<RectTransform>().localPosition.x, go.GetComponent<RectTransform>().localPosition.y, 0);
        }
    }

    void GenerationPathsB()
    {
        for (int i = 0; i < spawnedAdditionalRooms.Count; i++)
        {
            GameObject go = Instantiate(pathPrefab, roomIconsParent.position, Quaternion.identity);
            spawnedPaths.Add(go);
            spawnedAdditionalPaths.Add(go);
            curPathCount++;
            go.name = "Path - Side " + curPathCount;

            // Set Position
            go.transform.SetParent(roomIconsParent, true);
            //go.transform.localScale = new Vector3(1, 1, 1);
            go.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);

            // Reference 
            MapPath mapPath = go.GetComponent<MapPath>();
            mapPath.belongsToFloorCount = RoomManager.Instance.GetFloorCount();

            Transform closestRoomTrans = GetClosestRoomA(spawnedAdditionalRooms[i].transform.position);

            // Update map path using pos a: this room pos, pos b: nearest room pos
            mapPath.UpdateMapPath(spawnedAdditionalRooms[i].transform.position, closestRoomTrans.position);
            RoomMapIcon closestRoom = closestRoomTrans.GetComponent<RoomMapIcon>();
            RoomMapIcon curRoom = spawnedAdditionalRooms[i].GetComponent<RoomMapIcon>();

            // Referencing
            mapPath.UpdateStartingRoom(closestRoom);
            mapPath.AddGoalRooms(curRoom);
            closestRoom.UpdateRoomMapLinks(curRoom);
            closestRoom.UpdateRoomPathLinks(mapPath);
            curRoom.UpdateRoomMapLinks(closestRoom);
            curRoom.UpdateRoomPathLinks(mapPath);


            go.GetComponent<RectTransform>().localPosition = new Vector3(go.GetComponent<RectTransform>().localPosition.x, go.GetComponent<RectTransform>().localPosition.y, 0);
        }

        UpdateSpawnedPaths();
        spawnedPaths.Sort(ComparedMiddleOfPathY);


    }

    public void UpdateRoomIconType(RoomMapIcon roomMapIcon, string roomTypeName)
    {
       //Debug.Log("Updating room icon " + roomTypeName);
        if (roomTypeName == "enemy")
        {
            roomMapIcon.UpdateRoomType(RoomMapIcon.RoomType.ENEMY);
            roomMapIcon.UpdateRoomDetail(roomEnemySprite);
            roomMapIcon.UpdateRoomiconSize(roomEnemySize);
            if (!roomMapIcon.GetIsCompleted())
                roomMapIcon.UpdateRoomIconColour(roomEnemyColour);
            roomMapIcon.roomSelectionImage.UpdateRectPos(new Vector2(roomSelectedSizeInc.x + roomEnemySize.x, roomSelectedSizeInc.y + roomEnemySize.y));
        }
        else if (roomTypeName == "starting")
        {
            roomMapIcon.UpdateRoomType(RoomMapIcon.RoomType.STARTING);
            roomMapIcon.UpdateRoomDetail(roomStartingSprite);
            roomMapIcon.UpdateRoomiconSize(roomStartingSize);
            roomMapIcon.UpdateRoomIconColour(roomStartingColour);
            roomMapIcon.roomSelectionImage.UpdateRectPos(new Vector2(roomSelectedSizeInc.x + roomStartingSize.x, roomSelectedSizeInc.y + roomStartingSize.y));
        }
        else if (roomTypeName == "shop")
        {
            roomMapIcon.UpdateRoomType(RoomMapIcon.RoomType.SHOP);
            roomMapIcon.UpdateRoomDetail(roomShopSprite);
            roomMapIcon.UpdateRoomiconSize(roomShopSize);
            roomMapIcon.UpdateRoomIconColour(roomShopColour);
            roomMapIcon.roomSelectionImage.UpdateRectPos(new Vector2(roomSelectedSizeInc.x + roomShopSize.x, roomSelectedSizeInc.y + roomShopSize.y));
        }
        else if (roomTypeName == "hero")
        {
            roomMapIcon.UpdateRoomType(RoomMapIcon.RoomType.HERO);
            roomMapIcon.UpdateRoomDetail(roomHeroSprite);
            roomMapIcon.UpdateRoomiconSize(roomHeroSize);
            if (!roomMapIcon.GetIsCompleted())
                roomMapIcon.UpdateRoomIconColour(roomHeroColour);
            roomMapIcon.roomSelectionImage.UpdateRectPos(new Vector2(roomSelectedSizeInc.x + roomHeroSize.x, roomSelectedSizeInc.y + roomHeroSize.y));
        }
        else if (roomTypeName == "item")
        {
            roomMapIcon.UpdateRoomType(RoomMapIcon.RoomType.ITEM);
            roomMapIcon.UpdateRoomDetail(roomItemSprite);
            roomMapIcon.UpdateRoomiconSize(roomHeroSize);
            if (!roomMapIcon.GetIsCompleted())
                roomMapIcon.UpdateRoomIconColour(roomItemColour);
            roomMapIcon.roomSelectionImage.UpdateRectPos(new Vector2(roomSelectedSizeInc.x + roomHeroSize.x, roomSelectedSizeInc.y + roomHeroSize.y));
        }
        else if (roomTypeName == "boss")
        {
            roomMapIcon.UpdateRoomType(RoomMapIcon.RoomType.BOSS);
            roomMapIcon.UpdateRoomDetail(roomBossSprite);
            roomMapIcon.UpdateRoomiconSize(roomBossSize);
            if (!roomMapIcon.GetIsCompleted())
                roomMapIcon.UpdateRoomIconColour(roomBossColour);
            roomMapIcon.roomSelectionImage.UpdateRectPos(new Vector2(roomSelectedSizeInc.x + roomBossSize.x, roomSelectedSizeInc.y + roomBossSize.y));
        }
        else if (roomTypeName == "undiscovered")
        {
            //roomMapIcon.UpdateRoomIconColour(roomUndiscoveredColour);
        }
    }

    Transform GetClosestRoomA(Vector2 comparedPos)
    {
        Transform closestRoom = null;
        float minDist = Mathf.Infinity;
        Vector2 currentPos = comparedPos;

        //spawnedRoomsA.Sort();

        GameObject[] roomGameObjects = spawnedRoomsA.ToArray();

        foreach (GameObject room in roomGameObjects)
        {
            float dist = Vector2.Distance(room.transform.position, currentPos);
            if (dist < minDist)
            {
                closestRoom = room.transform;
                minDist = dist;
            }
        }
        return closestRoom;
    }

    void UpdateSpawnedPaths()
    {
        for (int i = 0; i < spawnedPaths.Count; i++)
        {
            //spawnedPaths[i].transform.position = Vector3.zero;
            RectTransform spawnedPath = spawnedPaths[i].GetComponent<RectTransform>();
            spawnedPath.position = new Vector3(spawnedPath.position.x, spawnedPath.position.y, 1);
            //spawnedPath.localScale = new Vector3(300, 300, 1);
        }
    }

    private int CompareRoomYValue(GameObject roomA, GameObject roomB)
    {
        if (roomA.transform.position.y > roomB.transform.position.y)
            return 1;
        else if (roomA.transform.position.y < roomB.transform.position.y)
            return -1;
        return 0;
    }

    private int CompareRoomClosestToSpawningRoom(GameObject roomA, GameObject roomB, GameObject comparedGO)
    {
        if (roomA.transform.position.y > roomB.transform.position.y)
            return 1;
        else if (roomA.transform.position.y < roomB.transform.position.y)
            return -1;
        return 0;
    }

    public int ComparedMiddleOfPathY(GameObject MapPathA, GameObject MapPathB)
    {
        if (MapPathA.GetComponent<MapPath>().middleOfPath.transform.position.y < MapPathB.GetComponent<MapPath>().middleOfPath.transform.position.y)
            return -1;
        else if (MapPathA.GetComponent<MapPath>().middleOfPath.transform.position.y > MapPathB.GetComponent<MapPath>().middleOfPath.transform.position.y)
            return 1;
        return 0;
    }
}
