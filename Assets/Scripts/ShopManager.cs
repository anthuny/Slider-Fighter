using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [HideInInspector]
    public int playerGold;

    [SerializeField] private int shopMaxCombatItems = 3;
    [SerializeField] private int shopMaxHealthItems = 3;
    [SerializeField] private List<Item> shopHealthItems = new List<Item>();
    [SerializeField] private List<Item> shopCombatItems = new List<Item>();

    [SerializeField] private Transform itemsParent;

    [SerializeField] private Transform shopItem1Parent;
    [SerializeField] private Transform shopItem2Parent;
    [SerializeField] private Transform shopItem3Parent;
    [SerializeField] private Transform shopItem4Parent;
    [SerializeField] private Transform shopItem5Parent;
    [SerializeField] private Transform shopItem6Parent;
    [SerializeField] private Transform shopItem7Parent;
    [SerializeField] private Transform shopItem8Parent;
    [SerializeField] private Transform refreshItemParent;

    [SerializeField] private GameObject shopItemPrefab;
    public UIElement shop;
    public UIElement shopSelectAllyPrompt;
    public bool selectAlly;
    [SerializeField] private Item unassigedItem;
    [SerializeField] private ButtonFunctionality buttonExitShop;
    public Transform unitsPositionShopTrans;

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

    public void ResetPlayerGold()
    {
        playerGold = 0;
    }
    public void UpdateUnAssignedItem(Item item)
    {
        unassigedItem = item;
    }

    public Item GetUnassignedItem()
    {
        return unassigedItem;
    }

    public void ToggleShopVisibility(bool toggle)
    {
        if (toggle)
            shop.UpdateAlpha(1);
        else
            shop.UpdateAlpha(0);
    }

    public RoomMapIcon GetActiveRoom()
    {
        return activeRoom;
    }

    public void SetActiveRoom(RoomMapIcon room)
    {
        activeRoom = room;
    }

    public List<Item> GetShopCombatItems()
    {
        return shopCombatItems;
    }
    public List<Item> GetShopHealthItems()
    {
        return shopHealthItems;
    }

    Item GetRandomShopItem(bool combatItem)
    {
        int rand = 0;
        if (combatItem)
        {
            rand = Random.Range(0, shopCombatItems.Count);
            return shopCombatItems[rand];
        }
        else
        {
            rand = Random.Range(0, shopHealthItems.Count);
            return shopHealthItems[rand];
        }
    }

    List<Item> ChooseItems(bool combatItem)
    {
        List<Item> items = new List<Item>();

        if (combatItem)
        {
            for (int i = 0; i < shopCombatItems.Count; i++)
            {
                Item item = GetRandomShopItem(combatItem);
                if (items.Contains(item))
                {
                    if (i != 0)
                        i--;
                }
                else
                    items.Add(item);
            }
        }
        else
        {
            for (int n = 0; n < shopHealthItems.Count; n++)
            {
                Item item = GetRandomShopItem(combatItem);
                if (items.Contains(item))
                {
                    if (n != 0)
                        n--;
                }
                else
                    items.Add(item);
            }
        }

        return items;
    }

    public void CloseShop()
    {
        ClearShopItems();
    }

    public void ClearShopItems()
    {
        /*
        // Clear previous items
        if (shopItem1Parent.childCount >= 1)
        {
            Destroy(shopItem1Parent.GetChild(0).gameObject);
            Destroy(shopItem2Parent.GetChild(0).gameObject);
            Destroy(shopItem3Parent.GetChild(0).gameObject);
            Destroy(shopItem4Parent.GetChild(0).gameObject);
            Destroy(shopItem5Parent.GetChild(0).gameObject);
            Destroy(shopItem6Parent.GetChild(0).gameObject);
            Destroy(shopItem7Parent.GetChild(0).gameObject);
            Destroy(shopItem8Parent.GetChild(0).gameObject);

        }
        */

        ToggleShopVisibility(false);
    }
    
    public void ToggleExitShopButton(bool toggle)
    {
        buttonExitShop.ToggleButton(toggle);
    }

    public void FillShopItems()
    {
        ToggleExitShopButton(true);

        ClearShopItems();

        ToggleShopVisibility(true);

        MapManager.Instance.exitShopRoom.UpdateAlpha(1);

        SetActiveRoom(RoomManager.Instance.GetActiveRoom());

        Item item = null;

        // Spawn Combat Items
        for (int i = 0; i < shopCombatItems.Count; i++)
        {
            // Spawn items
            GameObject go = Instantiate(shopItemPrefab, itemsParent);
            if (i == 0)
                go.transform.SetParent(shopItem1Parent);
            else if (i == 1)
                go.transform.SetParent(shopItem2Parent);
            else if (i == 2)
                go.transform.SetParent(shopItem3Parent);
            else if (i == 3)
                go.transform.SetParent(shopItem4Parent);
            else if (i == 4)
                go.transform.SetParent(shopItem5Parent);
            else if (i == 5)
                go.transform.SetParent(shopItem6Parent);

            go.transform.localScale = new Vector2(1, 1);
            go.transform.localPosition = Vector2.zero;

            // Update price and sprite
            ShopItem shopItem = go.GetComponent<ShopItem>();

            Item tempItem = null;

            // If shop room has not been opened before, spawn new ones
            if (!activeRoom.GetIsVisited())
                item = shopCombatItems[i];
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

        Item item2 = null;

        // Spawn Health Items
        for (int y = 0; y < shopHealthItems.Count; y++)
        {
            // Spawn items
            GameObject go = Instantiate(shopItemPrefab, itemsParent);

            if (y == 0)
                go.transform.SetParent(shopItem7Parent);
            else if (y == 1)
                go.transform.SetParent(shopItem8Parent);

            go.transform.localScale = new Vector2(1, 1);
            go.transform.localPosition = Vector2.zero;

            // Update price and sprite
            ShopItem shopItem = go.GetComponent<ShopItem>();

            Item tempItem = null;

            // If shop room has not been opened before, spawn new ones
            if (!activeRoom.GetIsVisited())
                item2 = shopHealthItems[y];
            else if (activeRoom.GetIsVisited())
            {
                item2 = activeRoom.GetShopRoomItems()[y];
                tempItem = activeRoom.GetShopRoomItems()[y];
            }

            // If item hasn't been purchased from this shop before
            shopItem.UpdateShopItemName(item2.itemName);
            shopItem.UpdatePriceText(item2.basePrice.ToString());
            shopItem.UpdateShopItemSprite(item2.itemSprite);
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
                activeRoom.AddShopRoomItems(item2);
        }
            

        //shopMaxHealthItems
        //shopMaxHealthItems

        // After filling shop items, room is now considered Visited.
        activeRoom.UpdateIsVisited(true);
    }

}
