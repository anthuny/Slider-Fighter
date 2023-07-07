using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    [SerializeField] private List<FloorData> allFloors = new List<FloorData>();

    public UnitMapIcon unitMapIcon;
    public MapOverlay mapOverlay;
    [SerializeField] private UIElement mapRoomOverlay;
    [SerializeField] private Image mapSpawnBounds;
    [SerializeField] private Transform roomIconsParent;
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject pathPrefab;
    [SerializeField] private Image scrollImage;
    public UIElement map;
    [SerializeField] private RoomMapIcon startingRoom;
    public RoomMapIcon endingRoom;
    [SerializeField] private Transform leftBorder;
    [SerializeField] private Transform rightBorder;

    [SerializeField] private List<GameObject> spawnedRoomsA = new List<GameObject>();
    [SerializeField] private List<GameObject> spawnedAllRooms = new List<GameObject>(); // List of spawned objects
    [SerializeField] private List<GameObject> spawnedAdditionalRooms = new List<GameObject>();
    [SerializeField] private List<GameObject> spawnedPaths = new List<GameObject>(); // List of spawned paths
    [SerializeField] private List<GameObject> spawnedAdditionalPaths = new List<GameObject>();

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
    public Sprite roomBossSprite;
    public Sprite roomStartingSprite;

    public Sprite detailHiddenSprite;

    public Vector2 roomEnemySize;
    public Vector2 roomShopSize;
    public Vector2 roomBossSize;
    public Vector2 rooomStartingSize;
    public Vector2 roomSelectedSizeInc;
    public Color roomEnemyColour;
    public Color roomShopColour;
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

    // The minimum and maximum values for the x and y positions of the spawned objects
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private bool spawnedStartingRoom;

    private bool storedRoomData;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Setup();
    }

    public void AddPlayerGold(int gold)
    {
        ShopManager.Instance.UpdatePlayerGold(gold);
        mapOverlay.UpdatePlayerGoldText(ShopManager.Instance.GetPlayerGold().ToString());
    }

    public void ResetPlayerGold()
    {
        ShopManager.Instance.ResetPlayerGold();
        mapOverlay.ResetPlayerGoldText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            ToggleMapVisibility(true, true);
        if (Input.GetKeyDown(KeyCode.N) && !CheckIfAnyHiddenMainRooms() && selectedRoom != startingRoom)
            ClearRoom();
        if (Input.GetKeyDown(KeyCode.V))
            ResetMap();
    }

    public void Setup()
    {
        ToggleMapVisibility(true, true);

        // Disable player weapon input 
        GameManager.Instance.ToggleUIElementFull(GameManager.Instance.playerWeaponChild, false);
    }

    void ToggleMapScroll(bool toggle)
    {
        scrollImage.enabled = toggle;
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
                    Debug.Log(rooms[i]);
                    continue;
                }
            }
        }

        if (count >= 2 + extra)
            return true;
        else
            return false;
    }

    public void ClearRoom()
    {
        // Do not allow room clear if selected room is the starting room
        if (selectedRoom == startingRoom)
        {
            ShowConnectingRooms();
            return;
        }

        selectedRoom.UpdateIsCompleted(true);
        selectedRoom.ToggleRoomSelected(true);
        selectedRoom.UpdateRoomSelectedColour(roomSelectedClearedColour);

        mapOverlay.ToggleEnterRoomButton(false);

        ShowConnectingRooms();
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
        ResetAllSelectedRooms();

        selectedRoom = room;
        selectedRoom.ToggleRoomSelected(true);

        if (selectedRoom.GetIsCompleted())
            selectedRoom.UpdateRoomSelectedColour(roomSelectedClearedColour);
        else
            selectedRoom.UpdateRoomSelectedColour(roomSelectedUnclearedColour);
    }

    public void ResetAllSelectedRooms()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i].ToggleRoomSelected(false);
        }

        startingRoom.ToggleRoomSelected(false);
    }

    public void ToggleMapVisibility(bool toggle, bool generateMap = false)
    {
        if (toggle)
        {
            ShopManager.Instance.ClearShopItems();
            exitShopRoom.UpdateAlpha(0);

            // Disable unit's post battle bg
            int count = GameManager.Instance.activeRoomAllies.Count;
            for (int i = 0; i < count; i++)
            {
                GameManager.Instance.activeRoomAllies[i].ToggleUnitBG(false);
            }

            map.UpdateAlpha(1);

            ToggleMapScroll(true);

            // Disable end turn button
            GameManager.Instance.endTurnButtonUI.UpdateAlpha(0);

            if (generateMap)
                GenerateMap();

            mapOverlay.UpdateRoomCountText(activeFloor.floorLevel.ToString());
            mapOverlay.UpdateFloorNameText(activeFloor.floorName, activeFloor.floorColour);
        }
        else
        {
            map.UpdateAlpha(0);
            ToggleMapScroll(false);
        }
    }
  
    Vector2 GetRoomSpawnRandomPos()
    {
        Bounds bounds = mapSpawnBounds.GetComponent<BoxCollider2D>().bounds;

        //float offsetX = Random.Range(-bounds.extents.x/2.75f, bounds.extents.x/2.75f);
        //float offsetY = Random.Range(-bounds.extents.y/2.75f, bounds.extents.y/2.75f);

        float offsetX = Random.Range(-bounds.extents.x, bounds.extents.x);
        float offsetY = Random.Range(-bounds.extents.y, bounds.extents.y);

        Vector2 newPos = bounds.center + new Vector3(offsetX, offsetY, 0);
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
        spawnedStartingRoom = false;
        spawnedRoomsA.Clear();
        spawnedAllRooms.Clear();
        spawnedAdditionalRooms.Clear();
        spawnedPaths.Clear();
        spawnedAdditionalPaths.Clear();

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

    public void GenerateMap()
    {
        ResetMap();
        SpawnRoomGenerationA();
        GenerationPathsA();

        // Additional rooms / paths are booken
        //SpawnRoomGenerationB();
        //GenerationPathsB();

        ToggleHiddenModeRoom(true);

        UpdateStartingRoomAndPath();

        StoreRooms();

        // Updating unit map icon starting position
        unitMapIcon.UpdateUnitPosition(startingRoom.transform.localPosition);
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
        startingRoom.UpdateIsCompleted(true);
        startingRoom.ToggleRoomSelected(true);
        startingRoom.UpdateRoomSelectedColour(roomSelectedClearedColour);

        UpdateSelectedRoom(startingRoom);

        mapOverlay.UpdateOverlayRoomName(RoomMapIcon.RoomType.STARTING);

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

        endingRoom.ToggleHiddenMode(true);
    }

    void SpawnRoomGenerationA()
    {
        // Calculate room count
        int roomSpawnCount = Random.Range(activeFloor.minRoomCount, activeFloor.maxRoomCount - 1);

        UpdateStartEndRoomHorizontalPos(leftBorder.position.x, rightBorder.position.x);

        // Spawn RoomGenerationA
        for (int i = 0; i < roomSpawnCount; i++)
        {
            if (failedCurAttempts <= maxFailedAttempts)
            {
                // Instantiate the object prefab
                GameObject room = Instantiate(roomPrefab);

                // Set the position of the spawned object
                room.transform.SetParent(roomIconsParent);
                room.transform.localScale = new Vector2(1, 1);


                Vector2 randomPos = GetRoomSpawnRandomPos();
                room.transform.position = randomPos;

                UpdateRoomZPosition(room.transform);

                // Check if the spawned object is too close to any other objects
                bool isTooClose = false;

                foreach (GameObject obj in spawnedAllRooms)
                {
                    float distance = Vector2.Distance(obj.transform.position, randomPos);

                    if (distance < minDistanceBetweenSpawnedObjects)
                    {
                        //Debug.Log(distance);
                        isTooClose = true;
                        failedCurAttempts++;
                        //Debug.Log("DIDNT Spawn FROM ROOM" + distance);
                        if (i != 0)
                            i--;

                        Destroy(room);
                        break;
                    }
                }

                // If the spawned object is not too close to any other objects, spawn another till capped
                if (!isTooClose)
                {
                    if (curRoomCount < activeFloor.maxRoomCount + activeFloor.sideRoomAmount)
                    {
                        // Update Room Icon 
                        RoomMapIcon roomMapIcon = room.GetComponent<RoomMapIcon>();
                        UpdateRoomIconType(roomMapIcon, "enemy");

                        // Add the spawned object to the list
                        spawnedAllRooms.Add(room);
                        spawnedRoomsA.Add(room);
                        curRoomCount++;
                        room.name = "Room - Main " + curRoomCount;
                        continue;
                    }
                }

                //Debug.Log("Failed Current Attempts: " + failedCurAttempts);
            }
        }

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

        int roomSpawnRoundB = activeFloor.sideRoomAmount;

        RoomMapIcon roomMapIcon;

        for (int i = 0; i < roomSpawnRoundB; i++)
        {
            if (failedCurAttempts <= maxFailedAttempts)
            {
                // Instantiate the object prefab
                GameObject sideRoom = Instantiate(roomPrefab);

                // Set the position of the spawned object
                sideRoom.transform.SetParent(roomIconsParent);
                sideRoom.transform.localScale = new Vector2(1, 1);
                //room.transform.position = new Vector3(randomPos.x, randomPos.y, 0);

                Vector3 randomPos = GetRoomSpawnRandomPos();
                sideRoom.transform.position = randomPos;

                UpdateRoomZPosition(sideRoom.transform);

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

                        Destroy(sideRoom);
                        break;
                    }
                }

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

                            Destroy(sideRoom);
                            break;
                        }
                    }
                }

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
                    continue;
                }
            }
        }

        spawnedAllRooms.Sort(CompareRoomYValue);
        spawnedAdditionalRooms.Sort(CompareRoomYValue);

        // Set starting room
        //UpdateRoomIconType(startingRoom, "starting");

        failedCurAttempts = 0;

        // Set shop rooms
        for (int i = 0; i < activeFloor.shopRoomCount; i++)
        {
            // Make enough shops from the additional room spawns, until enough has been hit for floor
            int rand = Random.Range(0, spawnedAdditionalRooms.Count-1);

            RoomMapIcon roomIcon = spawnedAdditionalRooms[rand].GetComponent<RoomMapIcon>();
            
            if (failedCurAttempts <= maxFailedAttempts)
            {
                // If there is already a shop room, skip it and reset so it can do it again
                if (roomIcon.GetRoomType() == RoomMapIcon.RoomType.SHOP)
                {
                    i--;
                    if (i < 0)
                        i = 0;
                    failedCurAttempts++;
                    continue;
                }

                // If room is not a shop, make it a shop
                roomIcon.UpdateRoomType(RoomMapIcon.RoomType.SHOP);
                UpdateRoomIconType(spawnedAdditionalRooms[rand].GetComponent<RoomMapIcon>(), "shop");
            }
        }

        // Set boss room
        UpdateRoomIconType(endingRoom, "boss");
        #endregion
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
        }

        UpdateSpawnedPaths();
        spawnedPaths.Sort(ComparedMiddleOfPathY);
    }

    public void UpdateRoomIconType(RoomMapIcon roomMapIcon, string roomTypeName)
    {
        if (roomTypeName == "enemy")
        {
            roomMapIcon.UpdateRoomType(RoomMapIcon.RoomType.ENEMY);
            roomMapIcon.UpdateRoomDetail(roomEnemySprite);
            roomMapIcon.UpdateRoomiconSize(roomEnemySize);
            roomMapIcon.UpdateRoomIconColour(roomEnemyColour);
            roomMapIcon.roomSelectionImage.UpdateRectPos(new Vector2(roomSelectedSizeInc.x + roomEnemySize.x, roomSelectedSizeInc.y + roomEnemySize.y));
        }
        else if (roomTypeName == "starting")
        {
            roomMapIcon.UpdateRoomType(RoomMapIcon.RoomType.STARTING);
            roomMapIcon.UpdateRoomDetail(roomStartingSprite);
            roomMapIcon.UpdateRoomiconSize(rooomStartingSize);
            roomMapIcon.UpdateRoomIconColour(roomStartingColour);
            roomMapIcon.roomSelectionImage.UpdateRectPos(new Vector2(roomSelectedSizeInc.x + rooomStartingSize.x, roomSelectedSizeInc.y + rooomStartingSize.y));

        }
        else if (roomTypeName == "shop")
        {
            roomMapIcon.UpdateRoomType(RoomMapIcon.RoomType.SHOP);
            roomMapIcon.UpdateRoomDetail(roomShopSprite);
            roomMapIcon.UpdateRoomiconSize(roomShopSize);
            roomMapIcon.UpdateRoomIconColour(roomShopColour);
            roomMapIcon.roomSelectionImage.UpdateRectPos(new Vector2(roomSelectedSizeInc.x + roomShopSize.x, roomSelectedSizeInc.y + roomShopSize.y));

        }
        else if (roomTypeName == "boss")
        {
            roomMapIcon.UpdateRoomType(RoomMapIcon.RoomType.BOSS);
            roomMapIcon.UpdateRoomDetail(roomBossSprite);
            roomMapIcon.UpdateRoomiconSize(roomBossSize);
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
