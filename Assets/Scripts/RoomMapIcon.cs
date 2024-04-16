using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomMapIcon : MonoBehaviour
{
    public enum RoomType { ENEMY, SHOP, HERO, ITEM, BOSS, STARTING }
    public RoomType curRoomType;

    public enum RoomSize { MAIN, SIDE }
    public RoomSize curRoomSize;

    [SerializeField] private List<RoomMapIcon> linkedRooms = new List<RoomMapIcon>();
    public List<MapPath> linkedPaths = new List<MapPath>();
    [SerializeField] private Image roomIconImage;
    [SerializeField] private Image roomDetail;
    public UIElement roomSelectionImage;
    [SerializeField] private Button roomButton;
    [SerializeField] private ButtonRoom buttonRoom;
    [SerializeField] private List<ItemPiece> shopRoomCombatItems = new List<ItemPiece>();
    [SerializeField] private List<ItemPiece> shopRoomHealthItems = new List<ItemPiece>();
    [SerializeField] private List<ItemPiece> purchasedItems = new List<ItemPiece>();

    public bool isHidden;
    public bool isSelected;
    public bool isDiscovered;
    public bool isMainRoom;
    public bool isStartingRoom;
    public bool isCompleted;
    public bool isVisited;
    public bool bossRoomDefeated;

    public bool hasEntered;

    bool revealedOnce;

    private RectTransform rt;


    private void Awake()
    {
        Setup();
    }

    void Setup()
    {
        rt = roomIconImage.GetComponent<RectTransform>();
        ToggleRoomSelected(false);

        buttonRoom.room = this;
    }

    public void AddPurchasedItems(ItemPiece item)
    {
        purchasedItems.Add(item);
    }

    public List<ItemPiece> GetPurchasedItems()
    {
        return purchasedItems;
    }

    public void ClearPurchasedItems()
    {
        purchasedItems.Clear();
    }

    // Combat Items
    public void AddShopRoomCombatItems(ItemPiece item)
    {
        shopRoomCombatItems.Add(item);
    }

    public void ClearShopRoomCombatItems()
    {
        shopRoomCombatItems.Clear();
    }

    public List<ItemPiece> GetShopRoomCombatItems()
    {
        return shopRoomCombatItems;
    }

    public int GetShopRoomCombatItemsAmount(string itemName)
    {
        int count = 0;
        for (int i = 0; i < GetShopRoomCombatItems().Count; i++)
        {
            if (itemName == GetShopRoomCombatItems()[i].itemName)
                count++;
        }

        return count;
    }

    public int GetShopRoomPurchasedItemsAmount(string itemName)
    {
        int count = 0;
        for (int i = 0; i < GetPurchasedItems().Count; i++)
        {
            if (itemName == GetPurchasedItems()[i].itemName)
                count++;
        }

        return count;
    }
    // Health Items
    public void AddShopRoomHealthItems(ItemPiece item)
    {
        shopRoomHealthItems.Add(item);
    }

    public void ClearShopRoomHealthItems()
    {
        shopRoomHealthItems.Clear();
    }

    public List<ItemPiece> GetShopRoomHealthItems()
    {
        return shopRoomHealthItems;
    }

    public int GetShopRoomHealthItemsAmount(string itemName)
    {
        int count = 0;
        for (int i = 0; i < GetShopRoomHealthItems().Count; i++)
        {
            if (itemName == GetShopRoomHealthItems()[i].itemName)
                count++;
        }

        return count;
    }
    public void SelectRoom()
    {
        // Ensure only check if room is revealed
        if (!isHidden)
        {
            GameManager.Instance.map.UpdateSelectedRoom(this);

            MapManager.Instance.mapOverlay.UpdateRoomDifficultyIcons();

            GameManager.Instance.map.unitMapIcon.UpdateUnitPosition(transform.localPosition);

            GameManager.Instance.map.mapOverlay.UpdateOverlayRoomName(curRoomType);

            MapManager.Instance.RefocusCamera();

            if (!GameManager.Instance.map.CheckIfAnyHiddenMainRooms(1) && !isStartingRoom && isMainRoom)
            {

                ToggleDiscovered(true);
                GameManager.Instance.map.HideConnectingRooms();
                if (curRoomType == RoomType.BOSS)
                {
                    for (int i = 0; i < linkedPaths.Count; i++)
                    {
                        if (linkedPaths[i].belongsToFloorCount != RoomManager.Instance.GetFloorCount())
                            linkedPaths[i].TogglePathVisibility(false);
                    }
                }

                if (GetIsCompleted() && curRoomType != RoomType.BOSS)
                    MapManager.Instance.mapOverlay.ToggleEnterRoomButton(false);
                else
                {
                    MapManager.Instance.mapOverlay.ToggleEnterRoomButton(true);
                    MapManager.Instance.mapOverlay.ToggleTeamPageButton(true);
                }

                // Update GameManager active room
                RoomManager.Instance.UpdateActiveRoom(this);
                //MapManager.Instance.mapOverlay.UpdateRoomDifficultyIcons();

                // Button Click SFX
                AudioManager.Instance.Play("Button_Click");

                return;
            }
            else if (curRoomType == RoomType.STARTING)
            {
                //if (GetIsCompleted())
                    //MapManager.Instance.mapOverlay.ToggleEnterRoomButton(false);

                if (RoomManager.Instance.GetFloorCount() != 1)
                    MapManager.Instance.mapOverlay.ToggleEnterRoomButton(true);
                else
                    MapManager.Instance.mapOverlay.ToggleEnterRoomButton(false);

                MapManager.Instance.mapOverlay.ToggleTeamPageButton(true);

                // Update GameManager active room
                RoomManager.Instance.UpdateActiveRoom(this);
                //MapManager.Instance.mapOverlay.UpdateRoomDifficultyIcons();

                // Button Click SFX
                AudioManager.Instance.Play("Button_Click");

            }
            else if (curRoomType == RoomType.SHOP)
            {
                MapManager.Instance.mapOverlay.ToggleEnterRoomButton(true);
                MapManager.Instance.mapOverlay.ToggleTeamPageButton(true);

                // Update GameManager active room
                RoomManager.Instance.UpdateActiveRoom(this);
                //MapManager.Instance.mapOverlay.UpdateRoomDifficultyIcons();

                // Button Click SFX
                AudioManager.Instance.Play("Button_Click");
            }
            else if (!isMainRoom)
            {
                ToggleDiscovered(true);
                GameManager.Instance.map.UpdateSelectedRoom(this);

                if (GetIsCompleted())
                    MapManager.Instance.mapOverlay.ToggleEnterRoomButton(false);
                else
                {
                    MapManager.Instance.mapOverlay.ToggleEnterRoomButton(true);
                    MapManager.Instance.mapOverlay.ToggleTeamPageButton(true);
                }

                // Update GameManager active room
                RoomManager.Instance.UpdateActiveRoom(this);
                //MapManager.Instance.mapOverlay.UpdateRoomDifficultyIcons();

                // Button Click SFX
                AudioManager.Instance.Play("Button_Click");

                return;
            }
            else if (curRoomType == RoomType.BOSS)
            {
                ToggleDiscovered(true);
                GameManager.Instance.map.UpdateSelectedRoom(this);


                for (int i = 0; i < linkedPaths.Count; i++)
                {
                    if (linkedPaths[i].belongsToFloorCount != RoomManager.Instance.GetFloorCount())
                        linkedPaths[i].TogglePathVisibility(false);
                }

                /*
                if (GetIsCompleted())
                    MapManager.Instance.mapOverlay.ToggleEnterRoomButton(false);
                */
                if (!GetIsCompleted())
                {
                    MapManager.Instance.mapOverlay.ToggleEnterRoomButton(true);
                    MapManager.Instance.mapOverlay.ToggleTeamPageButton(true);
                }
                    
                // Update GameManager active room
                RoomManager.Instance.UpdateActiveRoom(this);
                //MapManager.Instance.mapOverlay.UpdateRoomDifficultyIcons();

                // Button Click SFX
                AudioManager.Instance.Play("Button_Click");
            }
        }
    }

    public void UpdateIsCompleted(bool toggle)
    {
        isCompleted = toggle;
    }

    public bool GetIsCompleted()
    {
        return isCompleted;
    }

    public void UpdateIsVisited(bool toggle)
    {
        isVisited = toggle;
    }

    public bool GetIsVisited()
    {
        return isVisited;
    }

    public void UpdateHorizontalPos(float minX, float maxX)
    {
        float rand = Random.Range(minX, maxX);
        //transform.position = new Vector2(rand, transform.position.y);
        //transform.GetComponent<RectTransform>().position = new Vector3(transform.GetComponent<RectTransform>().position.x, transform.GetComponent<RectTransform>().position.y, 0);
        transform.position = new Vector3(rand, transform.position.y, 0);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
    }

    public void UpdateRoomMapLinks(RoomMapIcon roomMapIcon)
    {
        linkedRooms.Add(roomMapIcon);
    }

    public void ResetLinkedRooms()
    {
        linkedRooms.Clear();
    }

    public void UpdateRoomPathLinks(MapPath mapPath)
    {
        if (!linkedPaths.Contains(mapPath))
            linkedPaths.Add(mapPath);
    }

    public void ResetLinkedPaths()
    {
        linkedPaths.Clear();
    }

    public List<MapPath> GetLinkedPaths()
    {
        return linkedPaths;
    }

    public List<RoomMapIcon> GetLinkedRooms()
    {
        return linkedRooms;
    }

    public void UpdateRoomDetail(Sprite sprite = null)
    {
        if (sprite != null)
            roomDetail.sprite = sprite;
        else
            roomDetail.sprite = null;
    }

    public void UpdateRoomiconSize(Vector2 size)
    {
        //rt = GetComponent<RectTransform>();
        rt.sizeDelta = size;
    }

    public void UpdateRoomIconColour(Color colour)
    {
        roomIconImage.color = colour;
    }

    public void UpdateRoomType(RoomType roomType)
    {
        curRoomType = roomType;
    }

    public void ToggleHiddenMode(bool toggle)
    {
        //Debug.Log("Toggling room icon " + toggle);
        // If turning hidden ON
        if (toggle)
        {
            UpdateRoomIconColour(GameManager.Instance.map.roomHiddenColour);
            UpdateRoomDetail(GameManager.Instance.map.detailHiddenSprite);
            UpdateRoomiconSize(GameManager.Instance.map.roomEnemySize);
        }
        // if turning hidden OFF
        else
        {
            ToggleDiscovered(true);
        }

        UpdateIsHidden(toggle);
    }

    void UpdateIsHidden(bool toggle)
    {
        isHidden = toggle;
    }

    public bool GetIsHidden()
    {
        return isHidden;
    }

    
    public void ToggleDiscovered(bool toggle)
    {
        isDiscovered = toggle;

        if (!isDiscovered)
        {

            //UpdateRoomVisuals(curRoomType);
            UpdateRoomIconColour(GameManager.Instance.map.roomUndiscoveredColour);
            UpdateRoomDetail(GameManager.Instance.map.detailHiddenSprite);
            UpdateRoomiconSize(GameManager.Instance.map.roomEnemySize);
        }
        else
        {
            UpdateIsHidden(false);
            UpdateRoomVisuals(curRoomType, true);
        }
    }

    public bool GetDiscovered()
    {
        return isDiscovered;
    }

    public void UpdateRoomVisuals(RoomType roomType, bool unDiscovered = false)
    {
        curRoomType = roomType;

        if (curRoomType == RoomType.ENEMY)
            GameManager.Instance.map.UpdateRoomIconType(this, "enemy");
        else if (curRoomType == RoomType.SHOP)
            GameManager.Instance.map.UpdateRoomIconType(this, "shop");
        else if (curRoomType == RoomType.HERO)
            GameManager.Instance.map.UpdateRoomIconType(this, "hero");
        else if (curRoomType == RoomType.ITEM)
            GameManager.Instance.map.UpdateRoomIconType(this, "item");
        else if (curRoomType == RoomType.BOSS)
            GameManager.Instance.map.UpdateRoomIconType(this, "boss");
        else if (curRoomType == RoomType.STARTING)
            GameManager.Instance.map.UpdateRoomIconType(this, "starting");
    }

    public void ToggleRoomSelected(bool toggle)
    {
        if (toggle)
        {
            isSelected = true;
            roomSelectionImage.UpdateAlpha(1);

            // Reset all other selections
        }
        else
        {
            isSelected = false;
            roomSelectionImage.UpdateAlpha(0);
        }
    }

    public void UpdateRoomColour(Color colour, bool doMain = false)
    {
        roomSelectionImage.UpdateColour(colour);

        if (doMain)
        {
            //Debug.Log("Updating room colour inner");

            UpdateRoomIconColour(colour);
        }
    }

    public RoomType GetRoomType()
    {
        return curRoomType;
    }

    public RoomSize GetRoomSize()
    {
        return curRoomSize;
    }

    public void UpdateRoomSize(RoomSize roomSize)
    {
        curRoomSize = roomSize;
    }

}
