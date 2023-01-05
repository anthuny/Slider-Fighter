using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    [SerializeField] private List<FloorData> totalFloors = new List<FloorData>();
    [SerializeField] private int startingFloorDifficulty;
    [SerializeField] private float floorDifficultyRandomAmount;

    private FloorData activeFloor;
    private int floorCount = 0;
    

    private void Awake()
    {
        instance = this;
    }

    public void ResetFloorCount()
    {
        floorCount = 0;
    }

    public void SelectFloor()
    {
        activeFloor = totalFloors[floorCount];
    }

    public void FloorCompleted()
    {
        IncrementFloorCount();
        SelectFloor();
    }

    public FloorData GetActiveFloor()
    {
        return activeFloor;
    }

    public int GetFloorCount()
    {
        return floorCount;
    }

    public int GetFloorDifficulty()
    {
        float minA = ((floorDifficultyRandomAmount / 100f) * (float)startingFloorDifficulty);
        float minB = startingFloorDifficulty - minA;
        float maxB = startingFloorDifficulty + minA;
        int rand = Random.RandomRange((int)minB, (int)maxB);


        return rand;
    }

    void IncrementFloorCount()
    {
        floorCount++;
    }
}
