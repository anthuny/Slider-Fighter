using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    [SerializeField] private List<Floor> allFloors = new List<Floor>();

    [SerializeField] private Image mapSpawnBounds;
    [SerializeField] private Transform roomIconsParent;
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject pathPrefab;
    [SerializeField] private UIElement map;
    [SerializeField] private RoomMapIcon startingRoom;
    [SerializeField] private RoomMapIcon endingRoom;
    [SerializeField] private Transform leftBorder;
    [SerializeField] private Transform rightBorder;

    [SerializeField] private List<GameObject> spawnedRoomsA = new List<GameObject>();
    [SerializeField] private List<GameObject> spawnedAllRooms = new List<GameObject>(); // List of spawned objects
    [SerializeField] private List<GameObject> spawnedAdditionalRooms = new List<GameObject>();
    [SerializeField] private List<GameObject> spawnedPaths = new List<GameObject>(); // List of spawned paths
    [SerializeField] private List<GameObject> spawnedAdditionalPaths = new List<GameObject>();

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
    public Vector2 roomEnemySize;
    public Vector2 roomShopSize;
    public Vector2 roomBossSize;
    public Vector2 rooomStartingSize;
    public Color roomEnemyColour;
    public Color roomShopColour;
    public Color roomBossColour;
    public Color roomStartingColour;

    [HideInInspector]
    public Floor activeFloor;

    // The minimum and maximum values for the x and y positions of the spawned objects
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private void Start()
    {
        ToggleMapVisibility(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            ToggleMapVisibility(true);
        if (Input.GetKeyDown(KeyCode.N))
            ResetMap();
    }

    public void UpdateActiveFloor(Floor floor = null)
    {
        activeFloor = floor;

        if (floor != null)
            ToggleMapVisibility(true);
        else
            ToggleMapVisibility(false);
    }

    public void ToggleMapVisibility(bool toggle)
    {
        if (toggle)
        {
            map.UpdateAlpha(1);
            GenerateMap();
        }
        else
            map.UpdateAlpha(0);
    }
    
    Vector3 GetRoomSpawnRandomPos()
    {
        Bounds bounds = mapSpawnBounds.GetComponent<BoxCollider2D>().bounds;

        float offsetX = Random.Range(-bounds.extents.x, bounds.extents.x);
        float offsetY = Random.Range(-bounds.extents.y, bounds.extents.y);

        Vector3 newPos = bounds.center + new Vector3(offsetX, offsetY, 0);
        return newPos;

    }

    void UpdateStartEndRoomHorizontalPos(float minX, float maxX)
    {
        startingRoom.UpdateHorizontalPos(minX, maxX);
        endingRoom.UpdateHorizontalPos(minX, maxX);
    }

    void ResetMap()
    {
        spawnedAllRooms.Clear();
        spawnedAdditionalRooms.Clear();
        spawnedPaths.Clear();
        spawnedAdditionalPaths.Clear();
        failedCurAttempts = 0;
        curRoomCount = 0;
        spawnedPaths.Clear();
        spawnedAdditionalPaths.Clear();
        spawnedRoomsA.Clear();

        foreach (Transform child in roomIconsParent)
        {
            Destroy(child.gameObject);
        }

        activeFloor = null;

        if (allFloors.Count != 0)
            activeFloor = allFloors[0];
    }

    float distance;


    public void GenerateMap()
    {
        ResetMap();

        SpawnRoomGenerationA();
        GenerationPathsA();

        SpawnRoomGenerationB();
        GenerationPathsB();

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
                //room.transform.position = new Vector3(randomPos.x, randomPos.y, 0);

                Vector3 randomPos = GetRoomSpawnRandomPos();
                room.transform.position = randomPos;

                // Check if the spawned object is too close to any other objects
                bool isTooClose = false;

                foreach (GameObject obj in spawnedAllRooms)
                {
                    float distance = Vector2.Distance(obj.transform.position, randomPos);

                    if (distance < minDistanceBetweenSpawnedObjects)
                    {
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
                        continue;
                    }
                }
            }
        }

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

                // Check if the spawned object is too close to any other objects
                bool isTooClose = false;

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
                    continue;
                }
            }
        }

        spawnedAllRooms.Sort(CompareRoomYValue);
        spawnedAdditionalRooms.Sort(CompareRoomYValue);

        // Set starting room
        UpdateRoomIconType(startingRoom, "starting");

        failedCurAttempts = 0;

        // Set shop rooms
        for (int i = 0; i < activeFloor.shopRoomCount; i++)
        {
            // Make enough shops from the additional room spawns, until enough has been hit for floor
            int rand = Random.Range(0, spawnedAdditionalRooms.Count);

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

    void UpdateRoomIconType(RoomMapIcon roomMapIcon, string roomTypeName)
    {
        if (roomTypeName == "enemy")
        {
            roomMapIcon.UpdateRoomIcon(roomEnemySprite);
            roomMapIcon.UpdateRoomiconSize(roomEnemySize);
            roomMapIcon.UpdateRoomIconColour(roomEnemyColour);
            roomMapIcon.UpdateRoomType(RoomMapIcon.RoomType.ENEMY);
        }
        else if (roomTypeName == "starting")
        {
            roomMapIcon.UpdateRoomIcon(roomStartingSprite);
            roomMapIcon.UpdateRoomiconSize(rooomStartingSize);
            roomMapIcon.UpdateRoomIconColour(roomStartingColour);
            roomMapIcon.UpdateRoomType(RoomMapIcon.RoomType.STARTING);
        }
        else if (roomTypeName == "shop")
        {
            roomMapIcon.UpdateRoomIcon(roomShopSprite);
            roomMapIcon.UpdateRoomiconSize(roomShopSize);
            roomMapIcon.UpdateRoomIconColour(roomShopColour);
            roomMapIcon.UpdateRoomType(RoomMapIcon.RoomType.SHOP);
        }
        else if (roomTypeName == "boss")
        {
            roomMapIcon.UpdateRoomIcon(roomBossSprite);
            roomMapIcon.UpdateRoomiconSize(roomBossSize);
            roomMapIcon.UpdateRoomIconColour(roomBossColour);
            roomMapIcon.UpdateRoomType(RoomMapIcon.RoomType.BOSS);
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
            //go.transform.position = Vector2.zero;

            go.transform.SetParent(roomIconsParent, true);
            //go.transform.localScale = Vector3.one;
            go.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);

            MapPath mapPath = go.GetComponent<MapPath>();

            // Update map path using pos a: this room pos, pos b: nearest room pos
            mapPath.UpdateMapPath(spawnedAdditionalRooms[i].transform.position, GetClosestRoomA(spawnedAdditionalRooms[i].transform.position).position);
        }

        UpdateSpawnedPaths();
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

    void GenerationPathsA()
    {
        for (int i = 0; i < spawnedAllRooms.Count; i++)
        {
            if (i != spawnedAllRooms.Count-1)
            {
                GameObject go = Instantiate(pathPrefab, roomIconsParent.position, Quaternion.identity);
                spawnedPaths.Add(go);
                //go.transform.position = Vector2.zero;

                go.transform.SetParent(roomIconsParent, true);
                //go.transform.localScale = Vector3.one;
                go.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);

                curPathCount++;

                MapPath mapPath = go.GetComponent<MapPath>();
                mapPath.UpdateMapPath(spawnedAllRooms[i].transform.position, spawnedAllRooms[i + 1].transform.position);
            }
        }

        // Spawn path for starting room 
        curPathCount++;
        GameObject startingRoomPath = Instantiate(pathPrefab, roomIconsParent.position, Quaternion.identity);
        spawnedPaths.Add(startingRoomPath);
        startingRoomPath.transform.SetParent(roomIconsParent, true);
        startingRoomPath.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);
        MapPath mapPathStarting = startingRoomPath.GetComponent<MapPath>();
        mapPathStarting.UpdateMapPath(this.startingRoom.transform.position, spawnedAllRooms[0].transform.position);


        // Spawn path for ending room 
        curPathCount++;
        GameObject endingRoomPath = Instantiate(pathPrefab, roomIconsParent.position, Quaternion.identity);
        spawnedPaths.Add(endingRoomPath);
        endingRoomPath.transform.SetParent(roomIconsParent, true);
        endingRoomPath.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);
        MapPath mapPathEnding = endingRoomPath.GetComponent<MapPath>();
        mapPathEnding.UpdateMapPath(this.endingRoom.transform.position, spawnedAllRooms[spawnedAllRooms.Count-1].transform.position);

        UpdateSpawnedPaths();
    }

    void UpdateSpawnedPaths()
    {
        for (int i = 0; i < spawnedPaths.Count; i++)
        {
            //spawnedPaths[i].transform.position = Vector3.zero;
            RectTransform spawnedPath = spawnedPaths[i].GetComponent<RectTransform>();
            spawnedPath.localPosition = new Vector3(spawnedPath.localPosition.x, spawnedPath.localPosition.y, -1);
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
}
