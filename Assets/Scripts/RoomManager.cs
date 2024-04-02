using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;
    [SerializeField] private List<FloorData> totalFloors = new List<FloorData>();
    [SerializeField] private int startingFloorDifficulty;
    public int floorMaxRoomCount;
    public int floorMaxMainRoomTotalCount;
    public int floorMaxSideRoomTotalCount;
    public int floorMaxShopRoomCount;
    [SerializeField] private float floorDifficultyRandomAmount;
    [SerializeField] private RoomMapIcon activeRoom;
    private int enemyRoomsCleared;

    private CanvasGroup cg;

    private FloorData activeFloor;
    private int floorCount = 1;   
    private int minRoomCountBonus;
    private int maxRoomCountBonus;

    private void Awake()
    {
        Instance = this;

        cg = gameObject.GetComponent<CanvasGroup>();
    }

    public void ToggleInteractable(bool toggle)
    {
        cg.interactable = toggle;
        cg.blocksRaycasts = toggle;
    }

    public void UpdateActiveRoom(RoomMapIcon room)
    {
        activeRoom = room;
    }

    public RoomMapIcon GetActiveRoom()
    {
        return activeRoom;
    }

    public void ResetFloorCount()
    {
        floorCount = 1;

        ResetRoomsCleared();
    }

    public void SelectFloor()
    {
        // If a floor remains, select it
        if (GetFloorCount() < totalFloors.Count - 1)
            activeFloor = totalFloors[floorCount];
        // If no floor remains, repeat the last floor
        else
            activeFloor = totalFloors[totalFloors.Count - 1];
    }

    private void Update()
    {
       if (Input.GetKey(KeyCode.I))
       {
           PlayerPrefs.SetInt("HighestFloor", 1);
           CharacterCarasel.Instance.highestFloorReachedCountText.UpdateContentText(PlayerPrefs.GetInt("HighestFloor").ToString());
        }
    }

    public void HideFloorIncreaseAlert()
    {
        GameManager.Instance.transitionSprite.floorIncreaseAlertUI.UpdateAlpha(0);
    }
    public void FloorCompleted()
    {
        IncrementFloorCount();
        SelectFloor();

        GameManager.Instance.transitionSprite.floorIncreaseAlertUI.UpdateContentText((GetFloorCount()-1).ToString());
        GameManager.Instance.transitionSprite.floorIncreaseAlertUI.UpdateAlpha(1);

        StartCoroutine(FloorCompletedCo());
        //IncreaseMaxRoomCount();

        AudioManager.Instance.Play("SFX_FloorIncrease");

        SaveLoadHighestFloor();
    }

    public void SaveLoadHighestFloor()
    {
        if (PlayerPrefs.HasKey("HighestFloor"))
        {
            if (GetFloorCount() >= PlayerPrefs.GetInt("HighestFloor"))
            {
                PlayerPrefs.SetInt("HighestFloor", GetFloorCount());

                CharacterCarasel.Instance.highestFloorReachedCountText.UpdateContentText(PlayerPrefs.GetInt("HighestFloor").ToString());
            }
            else
                CharacterCarasel.Instance.highestFloorReachedCountText.UpdateContentText(PlayerPrefs.GetInt("HighestFloor").ToString());
        }
        else
        {
            PlayerPrefs.SetInt("HighestFloor", GetFloorCount());

            CharacterCarasel.Instance.highestFloorReachedCountText.UpdateContentText(PlayerPrefs.GetInt("HighestFloor").ToString());
        }
    }

    IEnumerator FloorCompletedCo()
    {
        yield return new WaitForSeconds(.5f);

        GameManager.Instance.transitionSprite.floorIncreaseAlertUI.UpdateContentText(GetFloorCount().ToString());
        GameManager.Instance.transitionSprite.floorIncreaseAlertUI.UpdateAlpha(1);
    }

    public void IncrementDefaultRoomsCleared()
    {
        enemyRoomsCleared++;
    }

    public void ResetRoomsCleared()
    {
        enemyRoomsCleared = 0;
    }

    public int GetRoomsCleared()
    {
        return enemyRoomsCleared;
    }

    public void IncreaseMaxRoomCount()
    {
        minRoomCountBonus++;
        maxRoomCountBonus++;
    }

    public int GetMinRoomCountBonus()
    {
        return minRoomCountBonus;
    }

    public int GetMaxRoomCountBonus()
    {
        return maxRoomCountBonus;
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
