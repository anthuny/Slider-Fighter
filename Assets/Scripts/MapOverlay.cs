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
    }

    public void ResetPlayerGoldText()
    {
        playerGoldText.UpdateContentText("0");
    }

    public void UpdateFloorNameText(string text, Color color)
    {
        floorName.UpdateContentText(text);
        floorName.UpdateContentTextColour(color);
    }

    public void UpdateRoomCountText(string text)
    {
        roomCountText.UpdateContentText(text);
    }
}
