using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    [SerializeField] private Image mapSpawnBounds;
    [SerializeField] private Transform roomIconsParent;
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private UIElement map;

    [SerializeField] private List<GameObject> spawnedRooms = new List<GameObject>(); // List of spawned objects

    // The minimum and maximum values for the x and y positions of the spawned objects
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    // The maximum number of objects that can be spawned
    public int maxObjects = 10;
    [SerializeField] private int maxFailedAttempts = 100;
    [SerializeField] private int failedCurAttempts;

    // The current number of objects that have been spawned
    public int currentObjects = 0;

    // The radius to use when checking for overlapping colliders
    //public float checkRadius = 0.1f;

    // The minimum distance to maintain between spawned objects
    public float minDistanceBetweenRooms = 0.2f;

    private void Start()
    {
        ToggleMapVisibility(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            ToggleMapVisibility(true);
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

    public void GenerateMap()
    {
        for (int i = 0; i < maxObjects; i++)
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

                foreach (GameObject obj in spawnedRooms)
                {
                    float distance = Vector2.Distance(obj.transform.position, randomPos);

                    if (distance < minDistanceBetweenRooms)
                    {
                        isTooClose = true;
                        failedCurAttempts++;

                        if (i != 0)
                            i--;

                        Destroy(room);
                        break;
                    }
                }

                // If the spawned object is not too close to any other objects, spawn another till capped
                if (!isTooClose)
                {
                    if (currentObjects < maxObjects)
                    {
                        // Add the spawned object to the list
                        spawnedRooms.Add(room);
                        currentObjects++;
                        continue;
                    }
                }
            }
        }

        spawnedRooms.Sort(CompareRoomYValue);
    }

    private int CompareRoomYValue(GameObject roomA, GameObject roomB)
    {
        if (roomA.transform.position.y > roomB.transform.position.y)
            return 1;
        else if (roomA.transform.position.y < roomB.transform.position.y)
            return -1;
        return 0;
    }
}
