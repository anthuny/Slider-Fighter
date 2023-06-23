using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [HideInInspector]
    public int playerGold;

    [SerializeField] private int maxShopItems = 3;
    [SerializeField] private List<Item> shopItems = new List<Item>();

    [SerializeField] private Transform shopItemsParent;
    [SerializeField] private GameObject shopItemPrefab;
    public UIElement shopSelectAllyPrompt;
    public bool selectAlly;
    [SerializeField] private Item unassigedItem;

    //public float luckyDiceTimeWait;

    RoomMapIcon activeRoom;

    private void Awake()
    {
        Instance = this;
    }

    public int GetPlayerGold()
    {
        return playerGold;
    }

    public void UpdatePlayerGold(int goldAdded)
    {
        playerGold += goldAdded;
    }

    public void UpdateUnAssignedItem(Item item)
    {
        unassigedItem = item;
    }

    public Item GetUnassignedItem()
    {
        return unassigedItem;
    }

    public void ResetPlayerGold()
    {
        playerGold = 0;
    }

    public RoomMapIcon GetActiveRoom()
    {
        return activeRoom;
    }

    public void SetActiveRoom(RoomMapIcon room)
    {
        activeRoom = room;
    }

    public List<Item> GetShopItems()
    {
        return shopItems;
    }

    Item GetRandomShopItem()
    {
        int rand = Random.Range(0, shopItems.Count);

        return shopItems[rand];
    }

    List<Item> ChooseItems()
    {
        List<Item> items = new List<Item>();
        for (int i = 0; i < maxShopItems; i++)
        {
            Item item = GetRandomShopItem();
            if (items.Contains(item))
            {
                if (i != 0)
                    i--;
            }
            else
                items.Add(item);
        }

        return items;
    }

    public void ClearShopItems()
    {
        // Clear previous items
        foreach (Transform child in shopItemsParent)
        {
            Destroy(child.gameObject);
        }
    }
    
    public void FillShopItems()
    {
        ClearShopItems();

        MapManager.Instance.exitShopRoom.UpdateAlpha(1);

        SetActiveRoom(RoomManager.Instance.GetActiveRoom());

        Item item = null;
        List<Item> items = new List<Item>();
        items = ChooseItems();

        for (int i = 0; i < maxShopItems; i++)
        {
            // Spawn items
            GameObject go = Instantiate(shopItemPrefab, shopItemsParent);
            go.transform.SetParent(shopItemsParent);
            go.transform.localScale = new Vector2(1, 1);

            // Update price and sprite
            ShopItem shopItem = go.GetComponent<ShopItem>();

            Item tempItem = null;

            // If shop room has not been opened before, spawn new ones
            if (!activeRoom.GetIsVisited())
                item = items[i];
            else if (activeRoom.GetIsVisited())
            {
                item = activeRoom.GetShopRoomItems()[i];
                tempItem = activeRoom.GetShopRoomItems()[i];
            }

            // If item hasn't been purchased from this shop before
            shopItem.UpdateShopItemName(item.itemName);
            shopItem.UpdatePriceText(item.basePrice.ToString());
            shopItem.UpdateShopItemSprite(item.itemSprite);
            shopItem.gameObject.GetComponent<UIElement>().UpdateAlpha(1);
            shopItem.itemButton.enabled = true;

            // If player owns an item from this shop
            for (int x = 0; x < activeRoom.GetOwnedShopItems().Count; x++)
            {
                // If item is already purchased from shop, make it invisible
                if (activeRoom.GetOwnedShopItems().Contains(tempItem))// && !invisItems.Contains(tempItem))
                {
                    shopItem.UpdateShopItemName("");
                    shopItem.UpdatePriceText("");
                    shopItem.UpdateShopItemSprite(null);
                    shopItem.gameObject.GetComponent<UIElement>().UpdateAlpha(0);
                    shopItem.itemButton.enabled = false;
                    shopItem.purchased = true;
                }
            }
            // If active room has not been visited yet, store shop items to room
            if (!activeRoom.GetIsVisited())
                activeRoom.AddShopRoomItems(item);
        }

        // After filling shop items, room is now considered Visited.
        activeRoom.UpdateIsVisited(true);
    }
}
