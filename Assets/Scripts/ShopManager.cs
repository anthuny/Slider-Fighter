using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    //[HideInInspector]
    public int playerGold;
    public int playerStartingGold;
    [SerializeField] private int startingReviveCost = 50;
    public Color shopItemCostDeny;
    public Color shopItemCostAllow;
    [SerializeField] private UIElement shopKeeper;
    [SerializeField] private UIElement shopHeroSign;
    public List<ButtonFunctionality> fallenHeroButtons = new List<ButtonFunctionality>();
    [SerializeField] private UIElement fallenHeroPromptUI;
    [SerializeField] private UIElement fallenHeroesParent;
    [SerializeField] private GameObject fallenHeroPrefab;
    [SerializeField] private int shopMaxCombatItems = 3;
    [SerializeField] private int shopMaxHealthItems = 3;
    [SerializeField] private List<ItemPiece> shopHealthItems = new List<ItemPiece>();
    [SerializeField] private List<ItemPiece> shopCombatItems = new List<ItemPiece>();

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
    [SerializeField] private ItemPiece unassigedItem;
    [SerializeField] private ButtonFunctionality buttonExitShop;
    public Transform unitsPositionShopTrans;
    public UIElement totalGoldText;
    [SerializeField] private UIElement refreshItem;
    private int refreshShopPrice;
    public int refreshShopStartingCost;
    public int refreshShopCostPerLv;
    public string selectedFallenUnitName;
    private bool activeRoomEntered;

    [SerializeField] private List<ShopItem> shopItems = new List<ShopItem>();
    [SerializeField] private List<string> fallenHeroesNamesBase = new List<string>();

    RoomMapIcon activeRoom;
    [SerializeField] private List<string> savedHeroNames = new List<string>();
    [SerializeField] private List<int> savedHeroCosts = new List<int>();

    public bool playerInShopRoom = false;

    public void ToggleFallenHeroPrompt(bool toggle = true)
    {
        if (toggle)
        {
            fallenHeroPromptUI.UpdateAlpha(1);


            fallenHeroPromptUI.ToggleButton(true);
            fallenHeroPromptUI.ToggleButton2(true, true);
        }
        else
        {
            fallenHeroPromptUI.UpdateAlpha(0);

            fallenHeroPromptUI.ToggleButton(false);
            fallenHeroPromptUI.ToggleButton2(false, true);
        }
    }

    public void ToggleAllFallenHeroSelection(bool toggle = true)
    {
        for (int i = 0; i < fallenHeroButtons.Count; i++)
        {
            fallenHeroButtons[i].ToggleSelection(false);
        }
    }

    public void TogglePlayerInShopRoom(bool toggle = true)
    {
        playerInShopRoom = toggle;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        totalGoldText.UpdateAlpha(0);
        refreshItem.UpdateAlpha(0);

        // Disable randomiser button
        ToggleRandomiserButton(false);
        ClearFallenHeroesVisuals();
    }

    public void CloseShop()
    {
        ToggleShopGoldText(false);
        ToggleRefreshItem(false);

        itemsParent.UpdateAlpha(0);

        if (playerInShopRoom)
            AudioManager.Instance.Play("SFX_ShopEnterLeave");
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

        // Update gold visual for shop 
        totalGoldText.UpdateContentText(playerGold.ToString());
        MapManager.Instance.mapOverlay.UpdatePlayerGoldText(playerGold.ToString());
    }
    public void UpdateUnAssignedItem(ItemPiece item)
    {
        unassigedItem = item;
    }

    public ItemPiece GetUnassignedItem()
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

    public List<ItemPiece> GetShopCombatItems()
    {
        return shopCombatItems;
    }
    public List<ItemPiece> GetShopHealthItems()
    {
        return shopHealthItems;
    }

    ItemPiece GetRandomShopItem(bool combatItem)
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

        ToggleRandomiserButton(toggle);
    }

    List<ItemPiece> ChooseItems(bool combatItem)
    {
        List<ItemPiece> items = new List<ItemPiece>();

        if (combatItem)
        {
            for (int i = 0; i < shopCombatItems.Count; i++)
            {
                ItemPiece item = GetRandomShopItem(combatItem);
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
                ItemPiece item = GetRandomShopItem(combatItem);
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
        //Debug.Log("enabling shop button to " + toggle);
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

    void ToggleRandomiserButton(bool toggle)
    {
        randomiser.gameObject.transform.GetChild(0).GetComponent<Image>().raycastTarget = toggle;
    }

    public void ReviveFallenHero(string name)
    {
        for (int i = 0; i < GameManager.Instance.fallenHeroes.Count; i++)
        {
            if (GameManager.Instance.fallenHeroes[i].GetUnitName() == name)
            {
                if (selectedFallenUnitName == name)
                {
                    AudioManager.Instance.Play("SFX_ShopBuySuceed");

                    //GameManager.Instance.AddActiveRoomAllUnitsFunctionality(GameManager.Instance.fallenHeroes[i]);

                    GameManager.Instance.AddUnitToTeam(GameManager.Instance.fallenHeroes[i].unitData);

                    GameManager.Instance.fallenHeroes[i].ReviveUnit(100, true);


                    GameManager.Instance.fallenHeroes[i].purchased = true;

                    GameManager.Instance.fallenHeroes[i].GetAnimator().SetTrigger("Idle");

                    fallenHeroesNamesBase.Remove(GameManager.Instance.fallenHeroes[i].GetUnitName());

                    GameManager.Instance.fallenHeroes.Remove(GameManager.Instance.fallenHeroes[i]);

                    //savedHeroNames.Remove(GameManager.Instance.GetUnitData(GameManager.Instance.fallenHeroes[i].GetUnitName()).unitName);
                    //savedHeroCosts.Clear();

                    GameManager.Instance.ResetActiveUnitTurnArrow();

                    // re-display fallen heroes to update the fallen hero being revived and removed
                    DisplayFallenHeroes();
                }
            }
        }

        TeamGearManager.Instance.ResetGearTab();
        TeamItemsManager.Instance.ResetItemsTab();
    }

    public void ClearFallenHeroesVisuals()
    {
        // Clear all previous fallen allies
        for (int i = 0; i < fallenHeroesParent.gameObject.transform.childCount; i++)
        {
            Destroy(fallenHeroesParent.gameObject.transform.GetChild(i).gameObject);
        }

        fallenHeroButtons.Clear();
        shopKeeper.UpdateAlpha(0);
        shopHeroSign.UpdateAlpha(0);
    }
    public void DisplayFallenHeroes()
    {
        ToggleShopGoldText(true);

        ClearFallenHeroesVisuals();

        shopKeeper.UpdateAlpha(1);
        shopHeroSign.UpdateAlpha(1);

        int count = GameManager.Instance.fallenHeroes.Count;

        for (int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(fallenHeroPrefab, fallenHeroesParent.transform.position, Quaternion.identity);
            go.transform.SetParent(fallenHeroesParent.transform);
            go.transform.localScale = new Vector3(0.85f, 0.85f, 1);
            go.transform.position = new Vector3(0, 0, 0);
            go.transform.localPosition = new Vector3(0, 0, 0);
            go.transform.GetChild(1).transform.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            //go.transform.GetChild(0).transform.lossyScale = new Vector3(1, 1, 1);


            MenuUnitDisplay unitDisplay = go.GetComponent<MenuUnitDisplay>();
            unitDisplay.UpdateUnitDisplay(GameManager.Instance.fallenHeroes[i].GetUnitName());

            int cost = startingReviveCost * RoomManager.Instance.GetFloorCount();
            int rand2 = Random.Range(1, 3);
            if (rand2 == 1)
                cost -= rand2;
            else if (rand2 == 0)
                cost += rand2;

            //cost /= 10;
            //Mathf.Ceil(cost);
            //cost *= 10;


            // Only continue if this hero HAS NOT already been spawned in from the shop before
            if (!fallenHeroesNamesBase.Contains(unitDisplay.unitName))
            {
                fallenHeroesNamesBase.Add(unitDisplay.unitName);
                
                unitDisplay.UpdateFallenHeroCost(cost);
                unitDisplay.UpdateFallenHeroCostColour();

                savedHeroNames.Add(unitDisplay.unitName);
                savedHeroCosts.Add(cost);
            }
            // Continue if this hero HAS already spawned in from the shop before
            else
            {
                if (savedHeroNames.Contains(unitDisplay.unitName))
                {
                    if (savedHeroCosts.Count > i)
                    {
                        unitDisplay.UpdateFallenHeroCost(savedHeroCosts[i]);
                        unitDisplay.UpdateFallenHeroCost(savedHeroCosts[i]);

                        unitDisplay.UpdateFallenHeroCostColour();
                    }
                }
            }

            fallenHeroButtons.Add(go.transform.GetChild(1).GetComponent<ButtonFunctionality>());

            go.GetComponent<MenuUnitDisplay>().unitName = GameManager.Instance.fallenHeroes[i].GetUnitName();
            go.GetComponent<Animator>().SetTrigger("DeathLoop");
        }
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

        //MapManager.Instance.exitShopRoom.UpdateAlpha(1);

        GameManager.Instance.ResetActiveUnitTurnArrow();

        ShopItem shopItem = null;

        ItemPiece itemCombat = null;

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

        ItemPiece itemHealth = null;

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
        //activeRoom.UpdateIsVisited(true);

        ToggleActiveRoomEntered(true);
    }

    void DisableShopItem()
    {

    }

}
