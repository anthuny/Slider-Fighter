using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomMapIcon : MonoBehaviour
{
    [SerializeField] private List<RoomMapIcon> linkedRooms = new List<RoomMapIcon>();
    [SerializeField] private List<MapPath> linkedPaths = new List<MapPath>();
    [SerializeField] private Image roomIconImage;
    [SerializeField] private Image roomDetail;
    public UIElement roomSelectionImage;
    [SerializeField] private Button roomButton;
    [SerializeField] private ButtonRoom buttonRoom;
    public enum RoomType { ENEMY, SHOP, BOSS, STARTING}
    public RoomType curRoomType;

    public bool isHidden;
    public bool isSelected;
    public bool isDiscovered;
    public bool isMainRoom;

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

    public void SelectRoom()
    {
        // Ensure only check if room is revealed
        if (!isHidden)
        {
            if (isMainRoom && revealedOnce)
            {
                GameManager.instance.map.UpdateSelectedRoom(this);
                GameManager.instance.map.unitMapIcon.UpdateUnitPosition(transform.localPosition);
                return;
            }
            if (!revealedOnce && !GameManager.instance.map.CheckIfAnyHiddenMainRooms(1))
            {
                revealedOnce = true;
                GameManager.instance.map.UpdateSelectedRoom(this);
                GameManager.instance.map.unitMapIcon.UpdateUnitPosition(transform.localPosition);
                ToggleDiscovered(true);
                // < -- Use this as button for now to complete room instantly with this after thats completed
                return;
            }
            if (!isMainRoom)
            {
                GameManager.instance.map.UpdateSelectedRoom(this);
                GameManager.instance.map.unitMapIcon.UpdateUnitPosition(transform.localPosition);
                return;
            }
        }
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
        // If turning cover mode ON
        if (toggle)
        {
            UpdateRoomIconColour(GameManager.instance.map.roomHiddenColour);
            UpdateRoomDetail(GameManager.instance.map.detailHiddenSprite);
            UpdateRoomiconSize(GameManager.instance.map.roomEnemySize);
        }
        // if turning cover mode OFF
        else
        {
            ToggleDiscovered(false);
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
            UpdateRoomIconColour(GameManager.instance.map.roomUndiscoveredColour);
            UpdateRoomDetail(GameManager.instance.map.detailHiddenSprite);
            UpdateRoomiconSize(GameManager.instance.map.roomEnemySize);
        }
        else
        {
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
            GameManager.instance.map.UpdateRoomIconType(this, "enemy");
        else if (curRoomType == RoomType.SHOP)
            GameManager.instance.map.UpdateRoomIconType(this, "shop");
        else if (curRoomType == RoomType.BOSS)
            GameManager.instance.map.UpdateRoomIconType(this, "boss");
        else if (curRoomType == RoomType.STARTING)
            GameManager.instance.map.UpdateRoomIconType(this, "starting");
    }

    public void ToggleRoomSelected(bool toggle)
    {
        if (toggle)
        {
            isSelected = true;
            roomSelectionImage.UpdateAlpha(1);
        }
        else
        {
            isSelected = false;
            roomSelectionImage.UpdateAlpha(0);
        }
    }

    public RoomType GetRoomType()
    {
        return curRoomType;
    }

}
