using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    //[HideInInspector]
    public int playerGold;
    public int playerStartingGold;

    [SerializeField] private int shopMaxCombatItems = 3;
    [SerializeField] private int shopMaxHealthItems = 3;
    [SerializeField] private List<Item> shopHealthItems = new List<Item>();
    [SerializeField] private List<Item> shopCombatItems = new List<Item>();

    [SerializeField] private UIElement itemsParent;
    [SerializeField] private UIElement randomiser;

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
    public UIElement totalGoldText;
    [SerializeField] private UIElement refreshItem;
    private int refreshShopPrice;
    public int refreshShopStartingCost;
    public int refreshShopCostPerLv;

    private bool activeRoomEntered;

    [SerializeField] private List<ShopItem> shopItems = new List<ShopItem>();

    RoomMapIcon activeRoom;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        totalGoldText.UpdateAlpha(0);
        refreshItem.UpdateAlpha(0);

        UpdatePlayerGold(playerStartingGold);
    }

    public void CloseShop()
    {
        ToggleShopGoldText(false);
        ToggleRefreshItem(false);

        itemsParent.UpdateAlpha(0);
    }

    void ToggleRefreshItem(bool toggle)
    {
        if (toggle)
        {
            UpdateRefreshShop();
            refreshItem.UpdateAlpha(1);
        }

        else
            refreshItem.UpdateAlpha(0);
    }

    public void UpdateRefreshShop()
    {
        refreshShopPrice = refreshShopStartingCost + (refreshShopCostPerLv * MapManager.Instance.activeFloor.floorLevel);
        refreshItem.UpdateContentText(refreshShopPrice.ToString());
    }

    public int GetRefreshShopPrice()
    {
        return refreshShopPrice;
    }

    public void ToggleShopGoldText(bool toggle)
    {
        if (toggle)
            totalGoldText.UpdateAlpha(1);
        else
            totalGoldText.UpdateAlpha(0);

        string goldString = GetPlayerGold().ToString();

        // Update shop and Map overlay gold counts
        totalGoldText.UpdateContentText(goldString);
        MapManager.Instance.mapOverlay.UpdatePlayerGoldText(goldString);
    }

    public void AddShopItems(ShopItem shopItem)
    {
        shopItems.Add(shopItem);
    }

    public void ResetShopItems()
    {
        shopItems.Clear();
    }

    public List<ShopItem> GetShopItems()
    {
        return shopItems;
    }

    public int GetPlayerGold()
    {
        return playerGold;
    }

    public void UpdatePlayerGold(int goldAdded)
    {
        playerGold += goldAdded;

        string goldString = GetPlayerGold().ToString();

        // Update gold visual for shop 
        totalGoldText.UpdateContentText(goldString);
        MapManager.Instance.mapOverlay.UpdatePlayerGoldText(goldString);
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

    public void ToggleRandomiser(bool toggle)
    {
        if (toggle)
            randomiser.UpdateAlpha(1);
        else
            randomiser.UpdateAlpha(0);
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

    public void ClearShopItems(bool hideShop = false)
    {
        if (GetActiveRoom())
        {
            //if (GetActiveRoom().GetIsVisited())
             //   return;

            //GetActiveRoom().ClearShopRoomCombatItems();
            //GetActiveRoom().ClearShopRoomHealthItems();

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

            if (hideShop)
            {
                ToggleShopVisibility(false);

                GetActiveRoom().ClearShopRoomCombatItems();
                GetActiveRoom().ClearShopRoomHealthItems();
            }
            else
            {
                ToggleShopVisibility(true);

            }

            ResetShopItems();
        }
    }
    
    public void ToggleExitShopButton(bool toggle)
    {
        buttonExitShop.ToggleButton(toggle);
    }

    public void ToggleActiveRoomEntered(bool toggle)
    {
        GetActiveRoom().hasEntered = toggle;
    }

    public bool GetActiveRoomEntered()
    {
        return GetActiveRoom().hasEntered;
    }
    public void FillShopItems(bool clearItems, bool refreshItems)
    {
        SetActiveRoom(RoomManager.Instance.GetActiveRoom());

        ToggleRandomiser(true);
        ToggleExitShopButton(true);

        ClearShopItems(clearItems);

        ToggleShopVisibility(true);
        ToggleShopGoldText(true);
        ToggleRefreshItem(true);

        itemsParent.UpdateAlpha(1);

        MapManager.Instance.exitShopRoom.UpdateAlpha(1);

        GameManager.Instance.ResetActiveUnitTurnArrow();

        ShopItem shopItem = null;

        Item itemCombat = null;

        // Spawn Combat Items
        for (int i = 0; i < shopMaxCombatItems; i++)
        {
            // Spawn items
            GameObject go = Instantiate(shopItemPrefab, itemsParent.gameObject.transform);

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
            shopItem = go.GetComponent<ShopItem>();

            AddShopItems(shopItem);

            int randInt = Random.Range(0, shopCombatItems.Count);
            // If shop room has not been opened before, spawn new ones
            if (!activeRoom.hasEntered)
                itemCombat = shopCombatItems[randInt];
            else if (activeRoom.hasEntered)
            {
                itemCombat = activeRoom.GetShopRoomCombatItems()[i];
            }

            // If item hasn't been purchased from this shop before
            shopItem.UpdateShopItemName(itemCombat.itemName);
            shopItem.UpdatePriceText(itemCombat.basePrice.ToString());
            shopItem.UpdateShopItemSprite(itemCombat.itemSprite);
            shopItem.gameObject.GetComponent<UIElement>().UpdateAlpha(1);
            shopItem.itemButton.enabled = true;

            if (itemCombat.ac)
                shopItem.UpdateAnimatorController(itemCombat.ac);

            // If active room has not been visited yet, store shop items to room
            if (!GetActiveRoom().hasEntered)
                activeRoom.AddShopRoomCombatItems(itemCombat);
        }

        Item itemHealth = null;

        // Spawn Health Items
        for (int y = 0; y < shopMaxHealthItems; y++)
        {
            // Spawn items
            GameObject go = Instantiate(shopItemPrefab, itemsParent.gameObject.transform);

            if (y == 0)
                go.transform.SetParent(shopItem7Parent);
            else if (y == 1)
                go.transform.SetParent(shopItem8Parent);

            go.transform.localScale = new Vector2(1, 1);
            go.transform.localPosition = Vector2.zero;

            // Update price and sprite
            shopItem = go.GetComponent<ShopItem>();
            AddShopItems(shopItem);

 
            int rand = Random.Range(0, shopHealthItems.Count);
            // If shop room has not been opened before, spawn new ones
            if (!activeRoom.hasEntered)
                itemHealth = shopHealthItems[rand];
            else if (activeRoom.hasEntered)
                itemHealth = activeRoom.GetShopRoomHealthItems()[y];

            // If item hasn't been purchased from this shop before
            shopItem.UpdateShopItemName(itemHealth.itemName);
            shopItem.UpdatePriceText(itemHealth.basePrice.ToString());
            shopItem.UpdateShopItemSprite(itemHealth.itemSprite);
            shopItem.gameObject.GetComponent<UIElement>().UpdateAlpha(1);
            shopItem.itemButton.enabled = true;

            if (itemHealth.ac)
                shopItem.UpdateAnimatorController(itemHealth.ac);

            // If active room has not been visited yet, store shop items to room
            if (!GetActiveRoom().hasEntered)
                activeRoom.AddShopRoomHealthItems(itemHealth);
        }

        // Hiding Purchased Items
        if (refreshItems)
        {
            // Loop each item that has been purchased
            for (int b = 0; b < GetActiveRoom().GetPurchasedItems().Count; b++)
            {
                // Loop through all actual items in shop
                for (int x = 0; x < GetShopItems().Count; x++)
                {
                    // If a shop item name matches with the purchased item on first loop, make it invis
                    if (GetShopItems()[x].GetShopItemName() == GetActiveRoom().GetPurchasedItems()[b].itemName)
                    {
                        // Make all items that are purchased invisible             
                        ShopItem shopItemHidden = GetShopItems()[x];

                        shopItemHidden.UpdateShopItemName("");
                        shopItemHidden.UpdatePriceText("");
                        shopItemHidden.UpdateShopItemSprite(null);
                        shopItemHidden.gameObject.GetComponent<UIElement>().UpdateAlpha(0);
                        shopItemHidden.itemButton.enabled = false;
                        shopItemHidden.UpdatePurchased(true);

                        // loop through how many have been purchased of that item 
                        if (GetActiveRoom().GetShopRoomPurchasedItemsAmount(GetActiveRoom().GetPurchasedItems()[b].itemName) > 1)
                            continue;
                        else
                            break;
                    }
                }
            }
        }

        // After filling shop items, room is now considered Visited.
        activeRoom.UpdateIsVisited(true);

        ToggleActiveRoomEntered(true);
    }

    void DisableShopItem()
    {

    }

}
