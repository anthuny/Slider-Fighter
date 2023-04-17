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

    private void Start()
    {
        ToggleEnterRoomButton(false);
    }
    public void UpdateOverlayRoomName(RoomMapIcon.RoomType roomType)
    {
        if (roomType == RoomMapIcon.RoomType.ENEMY)
        {
            UpdateRoomTypeText("ENEMY");
            UpdateRoomTypeTextColour(MapManager.instance.roomEnemyColour);
            UpdateRoomSubText("ROOM");
        }
        else if (roomType == RoomMapIcon.RoomType.SHOP)
        {
            UpdateRoomTypeText("SHOP");
            UpdateRoomTypeTextColour(MapManager.instance.roomShopColour);
            UpdateRoomSubText("ROOM");
        }
        else if (roomType == RoomMapIcon.RoomType.BOSS)
        {
            UpdateRoomTypeText("BOSS");
            UpdateRoomTypeTextColour(MapManager.instance.roomBossColour);
            UpdateRoomSubText("ROOM");
        }
        else
        {
            UpdateRoomTypeText("");
            UpdateRoomTypeTextColour(MapManager.instance.roomEnemyColour);
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
