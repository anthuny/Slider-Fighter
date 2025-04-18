using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    //[HideInInspector]

    [SerializeField] private GameObject shopItemsGO;
    [SerializeField] private UIElement partyNoRaceItemPrompt;
    [SerializeField] private UIElement inventoryUI;

    public RoomMapIcon lastVisitedShopRoom;
    public ShopItem selectedShopItem;
    public Animator commonItemAnimator;
    public Animator rareItemAnimator;
    public Animator EpicItemAnimator;
    //public Animator LegendaryItemAnimator;
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

    public GameObject shopItemPrefab;
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

    public RoomMapIcon activeRoom;
    [SerializeField] private List<string> savedHeroNames = new List<string>();
    [SerializeField] private List<int> savedHeroCosts = new List<int>();

    [SerializeField] private bool preventHumanItems = false;
    [SerializeField] private bool preventBeastItems = false;
    [SerializeField] private bool preventEtherealItems = false;

    [SerializeField] private UIElement shopKeeperButton;
    [SerializeField] private UIElement shopKeeperDetailParent;
    [SerializeField] private UIElement rerollPriceText;
    [SerializeField] private UIElement rerollButtonUI;
    [SerializeField] private UIElement sellButtonUI;

    [SerializeField] private bool shopkeeperSelected = false;

    public bool playerInShopRoom = false;

    public bool playerIsYetToSelectAFighter = false;

    public int spawnedItems;
    public int curRerollPrice = 0;


    public void ToggleShopKeeperSelected(bool toggle = true)
    {
        shopkeeperSelected = toggle;

        if (toggle)
        {
            ToggleShopkeeperDetails(true);
        }
        else
            ToggleShopkeeperDetails(false);
    }

    public bool GetShopKeeperSelected()
    {
        return shopkeeperSelected;
    }

    public void ToggleShopkeeperDetails(bool toggle = true, bool forceShopKeeperButtonOff = false)
    {
        if (toggle)
        {
            shopKeeperDetailParent.UpdateAlpha(1);

            //shopKeeperButton.ToggleButton(true);
            shopKeeperButton.GetComponentInChildren<Image>().raycastTarget = true;

            rerollButtonUI.UpdateAlpha(1);
            shopKeeperButton.GetComponent<GraphicRaycaster>().enabled = true;
            rerollButtonUI.ToggleButton(true);
            sellButtonUI.UpdateAlpha(1);
            sellButtonUI.ToggleButton(true);
        }
        else
        {
            //UpdateRerollPrice("");

            shopKeeperDetailParent.UpdateAlpha(0);
            rerollButtonUI.UpdateAlpha(0);
            rerollButtonUI.ToggleButton(false);
            sellButtonUI.UpdateAlpha(0);
            sellButtonUI.ToggleButton(false);


            if (forceShopKeeperButtonOff)
            {
                shopKeeperButton.ToggleButton(false);
                shopKeeperButton.GetComponent<GraphicRaycaster>().enabled = false;
                shopKeeperButton.GetComponentInChildren<Image>().raycastTarget = false;
            }
        }
    }

    public void UpdateRerollPrice(string newText)
    {
        rerollPriceText.UpdateContentText(newText);
    }

    public bool GetPreventHumanItems()
    {
        return preventHumanItems;
    }

    public bool GetPreventBeastItems()
    {
        return preventBeastItems;
    }

    public bool GetPreventEtherealItems()
    {
        return preventEtherealItems;
    }

    public void ResetShopItemSelectBorder()
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
            shopItems[i].UpdateShopItemSelectBorder(false);
        }
    }

    public void ToggleShopItemsGameObject(bool toggle = true)
    {
        shopItemsGO.SetActive(toggle);
    }

    public void ToggleInventoryUI(bool toggle = true)
    {
        if (toggle)
        {
            inventoryUI.UpdateAlpha(1);      
        }
        else
        {
            inventoryUI.UpdateAlpha(0);
        }

        inventoryUI.ToggleButton(toggle);
    }

    public void TogglePartyNoRacePrompt(bool toggle = true)
    {
        if (toggle)
        {
            partyNoRaceItemPrompt.UpdateAlpha(1);
        }
        else
        {
            partyNoRaceItemPrompt.UpdateAlpha(0);
        }
    }

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

        ToggleInventoryUI(false);

        ToggleShopkeeperDetails(false, true);
    }

    public void CloseShop()
    {
        ToggleShopGoldText(false);
        ToggleRefreshItem(false);

        shopKeeperButton.ToggleButton(false);
        shopKeeperButton.GetComponentInChildren<Image>().raycastTarget = false;
        shopKeeperButton.GetComponent<GraphicRaycaster>().enabled = false;

        itemsParent.UpdateAlpha(0);

        if (playerInShopRoom)
            AudioManager.Instance.Play("SFX_ShopEnterLeave");

        ToggleInventoryUI(false);

        for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
        {
            GameManager.Instance.activeRoomHeroes[i].ToggleTooltipStats(false);
        }
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

    public ShopItem GetSelectedShopItem()
    {
        return selectedShopItem;
    }

    public void DisableShopUI()
    {
        TogglePartyNoRacePrompt(false);
    }
    public void UpdateSelectedShopItem(ShopItem item)
    {
        selectedShopItem = item;
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

        UpdateRerollPriceTextColour();
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

    public void ToggleRandomiser(bool toggle)
    {
        if (toggle)
            randomiser.UpdateAlpha(1);
        else
            randomiser.UpdateAlpha(0);

        ToggleRandomiserButton(toggle);
    }

    public void ClearShopItems(bool hideShop = false)
    {
        if (GetActiveRoom())
        {            
            ResetShopItems();
            
            // Or last visited shop room does not equal to this one
            /*
            if (GetActiveRoom().GetIsVisited())
                return;
            }
            */

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

                //GetActiveRoom().ClearShopRoomShopItems();
                //Destroy(shopItem7Parent.GetChild(0).gameObject);
                //Destroy(shopItem8Parent.GetChild(0).gameObject);
            }

            
            if (hideShop)
            {
                ToggleShopVisibility(false);



                // ?????????????????? 
                GetActiveRoom().ClearShopRoomCombatItems();

                shopKeeperButton.ToggleButton(false);
                shopKeeperButton.GetComponentInChildren<Image>().raycastTarget = false;
            }
            else
            {
                ToggleShopVisibility(true);

            }
            

            
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
            unitDisplay.UpdateUnitDisplay(GameManager.Instance.fallenHeroes[i].GetUnitName(), true);

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

    public void UpdateAllShopItemPriceTextColour()
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
            shopItems[i].UpdatePriceTextColour();
        }
    }

    public bool CheckIfItemCanBeEquip()
    {
        bool partySuitable = false;

        for (int t = 0; t < GameManager.Instance.activeRoomHeroes.Count; t++)
        {
            if (GetSelectedShopItem().curRaceSpecific == ShopItem.RaceSpecific.HUMAN)
            {
                if (GameManager.Instance.activeRoomHeroes[t].curUnitRace == UnitFunctionality.UnitRace.HUMAN)
                {
                    partySuitable = true;
                    break;
                }
            }
            else if (GetSelectedShopItem().curRaceSpecific == ShopItem.RaceSpecific.BEAST)
            {
                if (GameManager.Instance.activeRoomHeroes[t].curUnitRace == UnitFunctionality.UnitRace.BEAST)
                {
                    partySuitable = true;
                    break;
                }
            }
            else if (GetSelectedShopItem().curRaceSpecific == ShopItem.RaceSpecific.ETHEREAL)
            {
                if (GameManager.Instance.activeRoomHeroes[t].curUnitRace == UnitFunctionality.UnitRace.ETHEREAL)
                {
                    partySuitable = true;
                    break;
                }
            }
            else if (GetSelectedShopItem().curRaceSpecific == ShopItem.RaceSpecific.ALL)
            {
                partySuitable = true;
                break;
            }               
        }

        return partySuitable;
    }

    public void ToggleShopItemButtons(bool toggle = true)
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
            shopItems[i].ToggleShopItemButton(toggle);
        }
    }

    public void UpdatePreventableSpawnedItems()
    {
        preventHumanItems = true;
        preventBeastItems = true;
        preventEtherealItems = true;

        for (int i = 0; i < GameManager.Instance.activeTeam.Count; i++)
        {
            if (GameManager.Instance.activeTeam[i].curRaceType == UnitData.RaceType.HUMAN)
            {
                preventHumanItems = false;
            }
            else if (GameManager.Instance.activeTeam[i].curRaceType == UnitData.RaceType.BEAST)
            {
                preventBeastItems = false;
            }
            else if (GameManager.Instance.activeTeam[i].curRaceType == UnitData.RaceType.ETHEREAL)
            {
                preventEtherealItems = false;
            }
        }
    }

    public void UpdateRerollPriceTextColour()
    {
        if (GetPlayerGold() >= GetCurRerollPrice())
        {
            rerollPriceText.contentText.color = shopItemCostAllow;
        }
        else
        {
            rerollPriceText.contentText.color = shopItemCostDeny;
        }
    }

    public int GetCurRerollPrice()
    {
        return curRerollPrice;
    }

    public void CalculatePurchaseAcceptReroll()
    {
        if (GetPlayerGold() >= GetCurRerollPrice())
        {
            UpdatePlayerGold(-GetCurRerollPrice());
            AudioManager.Instance.Play("Shop_Item_Buy");

            FillShopItems(false, true);
        }
        else
        {
            AudioManager.Instance.Play("SFX_ShopBuyFail");
        }
    }

    void CalculateRerollPrice(bool refreshItems)
    {
        // Update reroll price of Shop room
        if (!GetActiveRoom().isVisited || refreshItems)
        {
            int randPrice = Random.Range(4, 7) * 3;
            curRerollPrice = RoomManager.Instance.GetFloorCount() + randPrice;
            UpdateRerollPrice(curRerollPrice.ToString());
            UpdateRerollPriceTextColour();
        }
    }
    public void FillShopItems(bool clearItems, bool refreshItems)
    {
        ToggleShopItemsGameObject(true);

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

        UpdatePreventableSpawnedItems();

        if (refreshItems)
            GetActiveRoom().ClearShopRoomCombatItems();
        else
            ToggleShopKeeperSelected(false);

        CalculateRerollPrice(refreshItems);

        shopKeeperButton.ToggleButton(true);
        shopKeeperButton.GetComponentInChildren<Image>().raycastTarget = true;
        shopKeeperButton.GetComponent<GraphicRaycaster>().enabled = true;

        // Spawn Combat Items
        for (int i = 0; i < shopMaxCombatItems; i++)
        {
            if (i < 0)
                i = 0;

            int itemPrice = 0;

            if (!GetActiveRoom().isVisited || refreshItems)
            {


                bool getRare = false;
                bool getEpic = false;
                bool getLegendary = false;

                int randInt = 0;

                // Item Rarity Roll
                int rand = Random.Range(1, 101);

                rand += (RoomManager.Instance.GetFloorCount() - 1) * 2;

                if (rand > 100)
                    rand = 100;

                if (rand >= ItemRewardManager.Instance.itemEpicPerc)
                    getEpic = true;
                else if (rand >= ItemRewardManager.Instance.itemRarePerc)
                    getRare = true; 

                if (getLegendary)
                {   
                    List<ItemPiece> legItems = new List<ItemPiece>();
                    
                    for (int x = 0; x < shopCombatItems.Count; x++)
                    {
                        if (shopCombatItems[x].curRarity == ItemPiece.Rarity.LEGENDARY)
                        {
                            legItems.Add(shopCombatItems[x]);
                        }
                    }

                    randInt = Random.Range(0, legItems.Count);
                    if (legItems.Count > 0)
                    {
                        itemCombat = legItems[randInt];
                        itemPrice = itemCombat.basePrice;
                    }                   
                }
                else if (getEpic)
                {   
                    List<ItemPiece> epicItems = new List<ItemPiece>();
                    
                    for (int x = 0; x < shopCombatItems.Count; x++)
                    {
                        if (shopCombatItems[x].curRarity == ItemPiece.Rarity.EPIC)
                        {
                            epicItems.Add(shopCombatItems[x]);
                        }
                    }

                    randInt = Random.Range(0, epicItems.Count);
                    if (epicItems.Count > 0)
                    {
                        bool flag = false; 

                        // If item already exists in shop, dont spawn it, spawn another item
                        for (int t = 0; t < GetShopItems().Count; t++)
                        {
                            if (GetShopItems()[t].GetShopItemName() == epicItems[randInt].itemName)
                            {
                                flag = true;
                                continue;
                            }
                        }

                        if (flag)
                        {
                            i--;
                            continue;
                        }

                        if (epicItems[randInt].curRace == ItemPiece.RaceSpecific.HUMAN)
                        {
                            if (GetPreventHumanItems())
                            {
                                i--;
                                continue;
                            }
                        }
                        if (epicItems[randInt].curRace == ItemPiece.RaceSpecific.BEAST)
                        {
                            if (GetPreventBeastItems())
                            {
                                i--;
                                continue;
                            }
                        }
                        if (epicItems[randInt].curRace == ItemPiece.RaceSpecific.ETHEREAL)
                        {
                            if (GetPreventEtherealItems())
                            {
                                i--;
                                continue;
                            }
                        }

                        itemCombat = epicItems[randInt];
                        itemPrice = itemCombat.basePrice;
                    } 
                }
                else if (getRare)
                {   
                    List<ItemPiece> rareItems = new List<ItemPiece>();

                    for (int x = 0; x < shopCombatItems.Count; x++)
                    {
                        if (shopCombatItems[x].curRarity == ItemPiece.Rarity.RARE)
                        {
                            rareItems.Add(shopCombatItems[x]);
                        }
                    }

                    randInt = Random.Range(0, rareItems.Count);
                    if (rareItems.Count > 0)
                    {
                        bool flag = false;

                        // If item already exists in shop, dont spawn it, spawn another item
                        for (int t = 0; t < GetShopItems().Count; t++)
                        {
                            if (GetShopItems()[t].GetShopItemName() == rareItems[randInt].itemName)
                            {
                                flag = true;
                                continue;
                            }
                        }

                        if (flag)
                        {
                            i--;
                            continue;
                        }

                        if (rareItems[randInt].curRace == ItemPiece.RaceSpecific.HUMAN)
                        {
                            if (GetPreventHumanItems())
                            {
                                i--;
                                continue;
                            }
                        }
                        if (rareItems[randInt].curRace == ItemPiece.RaceSpecific.BEAST)
                        {
                            if (GetPreventBeastItems())
                            {
                                i--;
                                continue;
                            }
                        }
                        if (rareItems[randInt].curRace == ItemPiece.RaceSpecific.ETHEREAL)
                        {
                            if (GetPreventEtherealItems())
                            {
                                i--;
                                continue;
                            }
                        }

                        itemCombat = rareItems[randInt];
                        itemPrice = itemCombat.basePrice;
                    }
                }
                else
                {   
                    List<ItemPiece> commonItems = new List<ItemPiece>();
                    
                    for (int x = 0; x < shopCombatItems.Count; x++)
                    {
                        if (shopCombatItems[x].curRarity == ItemPiece.Rarity.COMMON)
                        {
                            commonItems.Add(shopCombatItems[x]);
                        }
                    }

                    randInt = Random.Range(0, commonItems.Count);
                    if (commonItems.Count > 0)
                    {
                        if (commonItems[randInt].curRace == ItemPiece.RaceSpecific.HUMAN)
                        {
                            if (GetPreventHumanItems())
                            {
                                i--;
                                continue;
                            }
                        }
                        if (commonItems[randInt].curRace == ItemPiece.RaceSpecific.BEAST)
                        {
                            if (GetPreventBeastItems())
                            {
                                i--;
                                continue;
                            }
                        }
                        if (commonItems[randInt].curRace == ItemPiece.RaceSpecific.ETHEREAL)
                        {
                            if (GetPreventEtherealItems())
                            {
                                i--;
                                continue;
                            }
                        }

                        itemCombat = commonItems[randInt];
                        itemPrice = itemCombat.basePrice;
                    }                  
                }

                if (itemCombat == null || itemPrice < 4)
                {
                    Debug.Log("Item Combat = " + itemCombat);
                    i--;

                    if (i < 0)
                        i = 0;
                    
                    continue;
                }


            }
        

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

            /*
            if (GetActiveRoom().GetShopRoomShopItems().Count > 0)
            {
                if (GetActiveRoom().GetShopRoomShopItems()[0] == null)
                {
                    GetActiveRoom().ClearShopRoomShopItems();
                    for (int v = 0; v < GetActiveRoom().GetShopRoomShopItems().Count; v++)
                    {
                        GetActiveRoom().GetShopRoomShopItems()[v] = shopItem;
                    }
                }
            }
            */

            if (GetActiveRoom().isVisited && !refreshItems)
            {
                itemCombat = GetActiveRoom().GetShopRoomCombatItems()[i];

                //shopItem = GetActiveRoom().GetShopRoomShopItems()[i];         
                if (i == 0)
                    shopItem.UpdatePrice(GetActiveRoom().item1Cost);
                else if (i == 1)
                    shopItem.UpdatePrice(GetActiveRoom().item2Cost);
                else if (i == 2)
                    shopItem.UpdatePrice(GetActiveRoom().item3Cost);
                else if (i == 3)
                    shopItem.UpdatePrice(GetActiveRoom().item4Cost);
                else if (i == 4)
                    shopItem.UpdatePrice(GetActiveRoom().item5Cost);
                else if (i == 5)
                    shopItem.UpdatePrice(GetActiveRoom().item6Cost);        
            }    

            AddShopItems(shopItem);

            // If active room has not been visited yet, store shop items to room
            if (!GetActiveRoom().isVisited || refreshItems)
            {
                activeRoom.AddShopRoomCombatItems(itemCombat);
                //activeRoom.AddShopRoomShopItems(shopItem);

                int rand2 = Random.Range(0,4);
                if (rand2 == 0)
                    itemPrice++;
                else if (rand2 == 1)
                    itemPrice--;
                else if (rand2 == 2)
                    itemPrice -= 2;

                shopItem.UpdatePrice(itemPrice);             
                if (i == 0)
                    GetActiveRoom().item1Cost = itemPrice;
                else if (i == 1)
                    GetActiveRoom().item2Cost = itemPrice;
                else if (i == 2)
                    GetActiveRoom().item3Cost = itemPrice;
                else if (i == 3)
                    GetActiveRoom().item4Cost = itemPrice;
                else if (i == 4)
                    GetActiveRoom().item5Cost = itemPrice;
                else if (i == 5)
                    GetActiveRoom().item6Cost = itemPrice;
            }
            
            shopItem.UpdateShopItemName(itemCombat.itemName);
            shopItem.UpdateItemIndex(i);

            shopItem.UpdateShopItemSprite(itemCombat.itemSpriteCombat);
            shopItem.gameObject.GetComponent<UIElement>().UpdateAlpha(1);
            shopItem.itemButton.enabled = true;

            if (itemCombat.ac)
                shopItem.UpdateAnimatorController(itemCombat.ac);

            if (itemCombat.curRarity == ItemPiece.Rarity.COMMON)
                shopItem.UpdateItemRarity(ShopItem.RarityType.COMMON);
            else if (itemCombat.curRarity == ItemPiece.Rarity.RARE)
                shopItem.UpdateItemRarity(ShopItem.RarityType.RARE);
            else if (itemCombat.curRarity == ItemPiece.Rarity.EPIC)
                shopItem.UpdateItemRarity(ShopItem.RarityType.EPIC);
            else if (itemCombat.curRarity == ItemPiece.Rarity.LEGENDARY)
                shopItem.UpdateItemRarity(ShopItem.RarityType.LEGENDARY);

            if (itemCombat.curRace == ItemPiece.RaceSpecific.ALL)
                shopItem.curRaceSpecific = ShopItem.RaceSpecific.ALL;
            else if (itemCombat.curRace == ItemPiece.RaceSpecific.HUMAN)
                shopItem.curRaceSpecific = ShopItem.RaceSpecific.HUMAN;
            else if (itemCombat.curRace == ItemPiece.RaceSpecific.BEAST)
                shopItem.curRaceSpecific = ShopItem.RaceSpecific.BEAST;
            else if (itemCombat.curRace == ItemPiece.RaceSpecific.ETHEREAL)
                shopItem.curRaceSpecific = ShopItem.RaceSpecific.ETHEREAL;

            shopItem.UpdatePriceTextColour();

            if (itemCombat.curRace == ItemPiece.RaceSpecific.HUMAN)
                shopItem.UpdateRaceIcon(GameManager.Instance.humanRaceIcon);
            else if (itemCombat.curRace == ItemPiece.RaceSpecific.BEAST)
                shopItem.UpdateRaceIcon(GameManager.Instance.beastRaceIcon);
            else if (itemCombat.curRace == ItemPiece.RaceSpecific.ETHEREAL)
                shopItem.UpdateRaceIcon(GameManager.Instance.etherealRaceIcon);
            else if (itemCombat.curRace == ItemPiece.RaceSpecific.ALL)
                shopItem.UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);

            // Hiding Purchased Items
            // Loop each item that has been purchased
            for (int b = 0; b < GetActiveRoom().GetPurchasedShopItems().Count; b++)
            {
                // Loop through all actual items in shop
                for (int x = 0; x < GetShopItems().Count; x++)
                {
                    // If a shop item name matches with the purchased item on first loop, make it invis
                    if (GetShopItems()[x].GetItemIndex() == GetActiveRoom().GetPurchasedShopItems()[b].GetItemIndex())
                    {
                        // Make all items that are purchased invisible             
                        ShopItem shopItemHidden = GetShopItems()[x];

                        shopItemHidden.UpdateShopItemName("");
                        shopItemHidden.UpdatePriceText("");
                        shopItemHidden.UpdateShopItemSprite(null);
                        shopItemHidden.gameObject.GetComponent<UIElement>().UpdateAlpha(0);
                        shopItemHidden.itemButton.enabled = false;
                        shopItemHidden.UpdatePurchased(true);

                        /*
                        // loop through how many have been purchased of that item 
                        if (GetActiveRoom().GetShopRoomPurchasedItemsAmount(GetActiveRoom().GetPurchasedItems()[b].itemName) > 0)
                            continue;
                        else
                            break;
                        */
                    }
                }
            }

        // After filling shop items, room is now considered Visited.
        //activeRoom.UpdateIsVisited(true);
        }

        UpdateAllShopItemPriceTextColour();

        ToggleActiveRoomEntered(true);
        GetActiveRoom().UpdateIsVisited(true);

        ToggleShopItemButtons(true);
    }

    void DisableShopItem()
    {

    }
}
