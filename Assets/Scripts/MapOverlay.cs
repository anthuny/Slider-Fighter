using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapOverlay : MonoBehaviour
{
    [SerializeField] private UIElement curRoomTypeText;
    [SerializeField] private UIElement curRoomSubText;
    [SerializeField] private UIElement buttonEnterRoom;
    [SerializeField] private UIElement buttonTeamPage;
    [SerializeField] private UIElement playerGoldText;
    [SerializeField] private UIElement roomCountText;
    [SerializeField] private UIElement floorName;
    [SerializeField] private UIElement roomDifficultyIcons;
    [SerializeField] private GameObject roomDifficultyIconPrefab;

    [SerializeField] private UIElement playerGoldTextShopRoom;
    [SerializeField] private UIElement floorNameShopRoom;
    [SerializeField] private UIElement roomCountTextShopRoom;

    [HideInInspector]
    public UIElement uiElement;

    private void Awake()
    {
        uiElement = gameObject.GetComponent<UIElement>();
    }
    private void Start()
    {
        //ToggleMapOverlayButtons(false);
    }

    public void UpdateRoomDifficultyIcons()
    {
        // Reset icons
        foreach (Transform child in roomDifficultyIcons.gameObject.transform)
        {
            Destroy(child.gameObject);
        }  

        int count = 0;
        // Add icons
        if (MapManager.Instance.selectedRoom.curRoomType == RoomMapIcon.RoomType.ENEMY)
            count = 1;
        else if (MapManager.Instance.selectedRoom.curRoomType == RoomMapIcon.RoomType.ITEM)
            count = 2;
        else if (MapManager.Instance.selectedRoom.curRoomType == RoomMapIcon.RoomType.HERO)
            count = 3;
        else if (MapManager.Instance.selectedRoom.curRoomType == RoomMapIcon.RoomType.BOSS)
            count = 4;
        else if (MapManager.Instance.selectedRoom.curRoomType == RoomMapIcon.RoomType.STARTING)
            count = 0;
        else if (MapManager.Instance.selectedRoom.curRoomType == RoomMapIcon.RoomType.SHOP)
            count = 0;

        if (MapManager.Instance.selectedRoom.curRoomType != RoomMapIcon.RoomType.SHOP && MapManager.Instance.selectedRoom.curRoomType != RoomMapIcon.RoomType.STARTING)
            count += RoomManager.Instance.GetFloorCount() - 1;

        //if (RoomManager.Instance.GetActiveRoom().curRoomType == RoomMapIcon.RoomType.STARTING)
            //count = 0;
        //Debug.Log("count = " + count);

        for (int i = 0; i < count; i++)
        {
            GameObject go2 = Instantiate(roomDifficultyIconPrefab, roomDifficultyIcons.transform.position, Quaternion.identity);
            go2.transform.SetParent(roomDifficultyIcons.transform);
            go2.transform.localScale = new Vector3(2,2,1);
            //go2.transform.localRotation = new Quaternion(0, 0, -180, 0);
        }
    }

    public void ToggleBottomBG(bool toggle)
    {
        if (toggle)
        {
            //buttonEnterRoom.UpdateAlpha(1);
            uiElement.UpdateAlpha(1);
        }
        else
        {
            buttonEnterRoom.UpdateAlpha(0);
            uiElement.UpdateAlpha(0);
        }
    }

    public void UpdateOverlayRoomName(RoomMapIcon.RoomType roomType)
    {
        if (roomType == RoomMapIcon.RoomType.ENEMY)
        {
            UpdateRoomTypeText("ENEMY");
            UpdateRoomTypeTextColour(MapManager.Instance.roomEnemyColour);
            UpdateRoomSubText("ROOM");
        }
        else if (roomType == RoomMapIcon.RoomType.SHOP)
        {
            UpdateRoomTypeText("SHOP");
            UpdateRoomTypeTextColour(MapManager.Instance.roomShopColour);
            UpdateRoomSubText("ROOM");
        }
        else if (roomType == RoomMapIcon.RoomType.HERO)
        {
            UpdateRoomTypeText("ENEMY+");
            UpdateRoomTypeTextColour(MapManager.Instance.roomEnemyColour);
            UpdateRoomSubText("ROOM");
        }
        else if (roomType == RoomMapIcon.RoomType.ITEM)
        {
            UpdateRoomTypeText("ITEM");
            UpdateRoomTypeTextColour(MapManager.Instance.roomShopColour);
            UpdateRoomSubText("ROOM");
        }
        else if (roomType == RoomMapIcon.RoomType.BOSS)
        {
            UpdateRoomTypeText("BOSS");
            UpdateRoomTypeTextColour(MapManager.Instance.roomBossColour);
            UpdateRoomSubText("ROOM");
        }
        else
        {
            UpdateRoomTypeText("");
            UpdateRoomTypeTextColour(MapManager.Instance.roomEnemyColour);
            UpdateRoomSubText("");
        }
    }

    public void UpdateRoomTypeText(string text)
    {
        curRoomTypeText.UpdateContentText(text);
    }
    public void UpdateRoomTypeTextColour(Color color)
    {
        curRoomTypeText.UpdateContentTextColour(color);
    }

    public void UpdateRoomSubText(string text)
    {
        curRoomSubText.UpdateContentText(text);
    }
    public void UpdateSubTextColour(Color color)
    {
        curRoomSubText.UpdateContentTextColour(color);
    }

    public void ToggleMapOverlayButtons(bool toggle, bool teamPage = true)
    {
        ToggleEnterRoomButton(toggle);

        if (teamPage)
            ToggleTeamPageButton(toggle);
    }

    public void ToggleEnterRoomButton(bool toggle)
    {
        if (toggle)
            buttonEnterRoom.UpdateAlpha(1);
        else
            buttonEnterRoom.UpdateAlpha(0);
    }

    public void ToggleTeamPageButton(bool toggle)
    {
        if (toggle)
            buttonTeamPage.UpdateAlpha(1);
        else
            buttonTeamPage.UpdateAlpha(0);
    }

    public void UpdatePlayerGoldText(string text)
    {
        playerGoldText.UpdateContentText(text);
        //playerGoldTextShopRoom.UpdateContentText(text);
    }

    public void ResetPlayerGoldText()
    {
        playerGoldText.UpdateContentText("0");
        //playerGoldTextShopRoom.UpdateContentText("0");
    }

    public void UpdateFloorNameText(string text, Color color)
    {
        floorName.UpdateContentText(text);
        floorName.UpdateContentTextColour(color);

        //floorNameShopRoom.UpdateContentText(text);
        //floorNameShopRoom.UpdateContentTextColour(color);
    }

    public void UpdateRoomCountText(string text)
    {
        Debug.Log("text = " + text);
        roomCountText.UpdateContentText(text);
        //oomCountTextShopRoom.UpdateContentText(text);
    }
}
