using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public enum RarityType { COMMON, RARE, EPIC, LEGENDARY }
    public RarityType curRarityType;

    public enum RaceSpecific { ALL, HUMAN, BEAST, ETHEREAL }
    public RaceSpecific curRaceSpecific;

    [SerializeField] private UIElement buttonPurchase;
    [SerializeField] private UIElement buttonPurchaseCover;
 
    public GameObject rarityCommonGO;
    public GameObject rarityRareGO;
    public GameObject rarityEpicGO;
    public GameObject rarityLegendaryGO;

    [SerializeField] private string shopItemName;
    [SerializeField] private UIElement raceIcon;
    public TextMeshProUGUI priceText;

    public int priceCount;
    [SerializeField] private Image image;
    [SerializeField] private UIElement imageUI;
    [SerializeField] private UIElement textUI;
    [SerializeField] private Animator itemAnimator;
    [SerializeField] private Animator rarityAnimator;
    [SerializeField] private UIElement shopItemSelectBorder;

    public ItemPiece linkedItemPiece = null;
    public GearPiece linkedGearPiece = null;

    public int itemIndex;
    public int gearIndex;

    public Button itemButton;

    public int price;
    private bool purchased;

    public UIElement GetImageUI()
    {
        return imageUI;
    }

    public void UpdateShopItemSelectBorder(bool toggle = true)
    {
        if (toggle)
            shopItemSelectBorder.UpdateAlpha(1);
        else
            shopItemSelectBorder.UpdateAlpha(0);    
    }

    public void UpdateRaceIcon(Sprite sprite)
    {
        raceIcon.UpdateContentImage(sprite);
    }
    public void ToggleShopItemButton(bool toggle)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = toggle;
        GetComponent<CanvasGroup>().interactable = toggle;
    }

    public void UpdateItemIndex(int newIndex)
    {
        itemIndex = newIndex;
    }

    public int GetItemIndex()
    {
        return itemIndex;
    }

    public void UpdateGearIndex(int newIndex)
    {
        gearIndex = newIndex;
    }

    public int GetGearIndex()
    {
        return gearIndex;
    }
    public void ToggleButtonPurchase(bool toggle = true)
    {
        if (toggle)
        {
            buttonPurchase.UpdateAlpha(1);
            buttonPurchase.ToggleButton(true);
        }
        else
        {
            buttonPurchase.UpdateAlpha(0);
            buttonPurchase.ToggleButton(false);
        }
    }

    public void ToggleButtonPurchaseCover(bool toggle = true)
    {
        if (toggle)
        {
            buttonPurchaseCover.UpdateAlpha(1);
            //buttonPurchase.ToggleButton(false);
        }
        else
        {
            buttonPurchaseCover.UpdateAlpha(0);
            //buttonPurchase.ToggleButton(true);
        }
    }

    public void UpdateAnimatorController(RuntimeAnimatorController ac)
    {
        itemAnimator.runtimeAnimatorController = ac;
    }

    public void UpdateItemRarity(RarityType rarity)
    {
        curRarityType = rarity;

        // Update animator visuals for rarity
        UpdateRarityAnimator(rarity);
    }

    public void UpdateRarityAnimator(RarityType rarity)
    {
        rarityCommonGO.SetActive(false);
        rarityRareGO.SetActive(false);
        rarityEpicGO.SetActive(false);
        rarityLegendaryGO.SetActive(false);

        if (rarity == RarityType.COMMON)
            rarityCommonGO.SetActive(true);
        else if (rarity == RarityType.RARE)
            rarityRareGO.SetActive(true);
        else if (rarity == RarityType.EPIC)
            rarityEpicGO.SetActive(true);
        else if (rarity == RarityType.LEGENDARY)
            rarityLegendaryGO.SetActive(true);
    }

    public void UpdatePurchased(bool toggle)
    {
        purchased = toggle;
    }

    public bool GetPurchased()
    {
        return purchased;
    }

    public void TogglePriceText(bool toggle = true)
    {
        if (toggle)
            priceText.GetComponent<UIElement>().UpdateAlpha(1);
        else
            priceText.GetComponent<UIElement>().UpdateAlpha(0);
    }
    
    public void TogglePurchaseButton(bool toggle = true)
    {
        if (toggle)
            buttonPurchase.UpdateAlpha(1);
        else
            buttonPurchase.UpdateAlpha(0);
    }

    public void ToggleRarityBG(bool toggle = false)
    {
        rarityCommonGO.SetActive(false);
        rarityRareGO.SetActive(false);
        rarityEpicGO.SetActive(false);
        rarityLegendaryGO.SetActive(false);
    }

    public void UpdatePrice(int price)
    {
        priceCount = price;

        UpdatePriceText(priceCount.ToString());
    }
    public void UpdatePriceText(string priceText)
    {
        this.priceText.text = priceText;

        if (priceText != "")
        {
            price = int.Parse(priceText);
            priceCount = price;
        }

        UpdatePriceTextColour();
    }


    public void UpdatePriceTextColour()
    {
        if (ShopManager.Instance.GetPlayerGold() < price)
            this.priceText.color = ShopManager.Instance.shopItemCostDeny;
        else
            this.priceText.color = ShopManager.Instance.shopItemCostAllow;
    }

    public void UpdateShopItemSprite(Sprite sprite, bool gear = false)
    {
        image.sprite = sprite;

        if (gear)
        {
            image.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
        }
        else
            image.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
    }

    public void UpdateShopItemName(string newName)
    {
        shopItemName = newName;
    }

    public string GetShopItemName()
    {
        return shopItemName;
    }

    // Make item bob

    public void PurchaseShopItem()
    {
        if (linkedItemPiece)
        {
            if (!ShopManager.Instance.CheckIfItemCanBeEquip())
            {
                AudioManager.Instance.Play("SFX_ShopBuyFail");
                return;
            }
        }


        int playerGold = ShopManager.Instance.GetPlayerGold();
  
        // Try Purchase Action
        if (playerGold < price)     // If player cannot afford the item, cancel.
            return;
        else
            ShopManager.Instance.UpdatePlayerGold(-price);

        ShopManager.Instance.playerIsYetToSelectAFighter = true;

        ShopManager.Instance.shopSelectAllyPrompt.UpdateAlpha(1);

        ShopManager.Instance.ResetShopItemSelectBorder();

        // Purchase the item
        //imageUI.UpdateAlpha(0);
        //textUI.UpdateAlpha(0);

        GameManager.Instance.ToggleAllowSelection(true);
        GameManager.Instance.ToggleAllyUnitSelection(true);

        MapManager.Instance.UpdateMapGoldText();
        ShopManager.Instance.ToggleShopGoldText(true);

        ShopManager.Instance.UpdateAllShopItemPriceTextColour();

        //GameManager.Instance.SetAllFightersSelected(true);

        int combatCount = 0;

        if (linkedItemPiece)
        {
            combatCount = ShopManager.Instance.GetShopCombatItems().Count;
            for (int i = 0; i < combatCount; i++)
            {
                if (shopItemName == ShopManager.Instance.GetShopCombatItems()[i].itemName)
                {
                    // Disable exit button until player has selected ally with item
                    //MapManager.Instance.exitShopRoom.UpdateAlpha(0);

                    ShopManager.Instance.ToggleExitShopButton(false);

                    ItemPiece item = ShopManager.Instance.GetShopCombatItems()[i];
                    ShopManager.Instance.GetActiveRoom().AddPurchasedItems(item);

                    GameObject go = Instantiate(ShopManager.Instance.shopItemPrefab, GameManager.Instance.nothingnessUI.transform.position, Quaternion.identity);
                    go.transform.SetParent(GameManager.Instance.nothingnessUI.gameObject.transform);
                    go.GetComponent<ShopItem>().UpdateShopItemName(ShopManager.Instance.GetSelectedShopItem().GetShopItemName());
                    go.GetComponent<ShopItem>().UpdateItemIndex(ShopManager.Instance.GetSelectedShopItem().GetItemIndex());
                    ShopManager.Instance.GetActiveRoom().AddPurchasedShopItems(go.GetComponent<ShopItem>());
                    ShopManager.Instance.UpdateUnAssignedItem(item);
                    // Prompt player on who to give item
                    GameManager.Instance.ToggleAllowSelection(true);
                    //ShopManager.Instance.shopSelectAllyPrompt.UpdateAlpha(1);
                    ShopManager.Instance.selectAlly = true;

                    // Buy item SFX
                    AudioManager.Instance.Play("Shop_Item_Buy");

                    OverlayUI.Instance.ToggleFighterDetailsTab(true);
                    OverlayUI.Instance.UpdateItemDetailsUI(ShopManager.Instance.GetShopCombatItems()[i].itemName,
                    ShopManager.Instance.GetShopCombatItems()[i].itemDesc,
                    ShopManager.Instance.GetShopCombatItems()[i].itemPower,
                    ShopManager.Instance.GetShopCombatItems()[i].range,
                    ShopManager.Instance.GetShopCombatItems()[i].itemRangeHitArea,
                    ShopManager.Instance.GetShopCombatItems()[i].itemSpriteCombatSmaller);
                    ShopManager.Instance.ToggleInventoryUI(true);

                    UpdatePurchased(true);

                    for (int x = 0; x < GameManager.Instance.activeRoomHeroes.Count; x++)
                    {
                        if (x == 0)
                        {
                            if (OwnedLootInven.Instance.GetWornItemMainAlly().Count >= 3)
                                break;
                        }
                        else if (x == 1)
                        {
                            if (OwnedLootInven.Instance.GetWornItemSecondAlly().Count >= 3)
                                break;
                        }
                        else if (x == 2)
                        {
                            if (OwnedLootInven.Instance.GetWornItemThirdAlly().Count >= 3)
                                break;
                        }

                        if (curRaceSpecific == RaceSpecific.HUMAN)
                        {
                            if (GameManager.Instance.activeRoomHeroes[x].curUnitRace == UnitFunctionality.UnitRace.HUMAN)
                            {
                                GameManager.Instance.activeRoomHeroes[x].ToggleSelected(true, false, true);
                            }

                        }
                        else if (curRaceSpecific == RaceSpecific.ETHEREAL)
                        {
                            if (GameManager.Instance.activeRoomHeroes[x].curUnitRace == UnitFunctionality.UnitRace.ETHEREAL)
                                GameManager.Instance.activeRoomHeroes[x].ToggleSelected(true, false, true);
                        }
                        else if (curRaceSpecific == RaceSpecific.BEAST)
                        {
                            if (GameManager.Instance.activeRoomHeroes[x].curUnitRace == UnitFunctionality.UnitRace.BEAST)
                                GameManager.Instance.activeRoomHeroes[x].ToggleSelected(true, false, true);
                        }
                        else if (curRaceSpecific == RaceSpecific.ALL)
                        {
                            GameManager.Instance.activeRoomHeroes[x].ToggleSelected(true, false, true);
                        }

                    }

                    return;
                }
            }
        }
        else if (linkedGearPiece)
        {
            combatCount = ShopManager.Instance.GetShopCombatGear().Count;
            for (int i = 0; i < combatCount; i++)
            {
                if (shopItemName == ShopManager.Instance.GetShopCombatGear()[i].gearName)
                {
                    // Disable exit button until player has selected ally with item
                    //MapManager.Instance.exitShopRoom.UpdateAlpha(0);

                    ShopManager.Instance.ToggleExitShopButton(false);

                    GearPiece gear = ShopManager.Instance.GetShopCombatGear()[i];
                    ShopManager.Instance.GetActiveRoom().AddPurchasedGearPiece(gear);

                    GameObject go = Instantiate(ShopManager.Instance.shopItemPrefab, GameManager.Instance.nothingnessUI.transform.position, Quaternion.identity);
                    go.transform.SetParent(GameManager.Instance.nothingnessUI.gameObject.transform);
                    go.GetComponent<ShopItem>().UpdateShopItemName(ShopManager.Instance.GetSelectedShopItem().linkedGearPiece.gearName);
                    go.GetComponent<ShopItem>().UpdateItemIndex(ShopManager.Instance.GetSelectedShopItem().GetItemIndex());
                    ShopManager.Instance.GetActiveRoom().AddPurchasedShopItems(go.GetComponent<ShopItem>());
                    ShopManager.Instance.UpdateUnAssignedGear(gear);
                    // Prompt player on who to give item
                    GameManager.Instance.ToggleAllowSelection(true);
                    //ShopManager.Instance.shopSelectAllyPrompt.UpdateAlpha(1);
                    ShopManager.Instance.selectAlly = true;

                    // Buy item SFX
                    AudioManager.Instance.Play("Shop_Item_Buy");

                    OverlayUI.Instance.ToggleFighterDetailsTab(true);
                    OverlayUI.Instance.UpdateGearDetailsUI(gear.gearName, "", gear.bonusHealth, gear.bonusDamage, gear.bonusHealing, gear.bonusDefense, gear.bonusSpeed, gear.gearIcon);
                    ShopManager.Instance.ToggleInventoryUI(true);

                    UpdatePurchased(true);

                    for (int x = 0; x < GameManager.Instance.activeRoomHeroes.Count; x++)
                    {
                        GameManager.Instance.activeRoomHeroes[x].ToggleSelected(true, false, true);
                    }
                    return;
                }
            }
        }
        

    }


    public void SelectShopItem(bool shopCombatItem)
    {
        // If the player must first select an ally to give an item, do not allow purchase of another item.
        if (ShopManager.Instance.playerIsYetToSelectAFighter || purchased)
            return;

        ShopManager.Instance.ResetShopItemSelectBorder();

        UpdateShopItemSelectBorder(true);

        ShopManager.Instance.UpdateSelectedShopItem(this);



        ShopManager.Instance.TogglePartyNoRacePrompt(true);

        OverlayUI.Instance.buttonSellItemDetails.UpdateAlpha(0);
        OverlayUI.Instance.buttonSellItemDetails.ToggleButton(false);
        OverlayUI.Instance.ToggleSellItemCostText(false);

        if (!ShopManager.Instance.CheckIfItemCanBeEquip())
        {
            ShopManager.Instance.TogglePartyNoRacePrompt(true);
        }
        else
        {
            ShopManager.Instance.TogglePartyNoRacePrompt(false);
        }
        
        for (int x = 0; x < ShopManager.Instance.GetShopObjects().Count; x++)
        {
            ShopManager.Instance.GetShopObjects()[x].ToggleButtonPurchase(false);
        }

        bool allowItemPurchase = false;
        for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
        {
            if (curRaceSpecific == RaceSpecific.HUMAN &&
                GameManager.Instance.activeRoomHeroes[i].curUnitRace == UnitFunctionality.UnitRace.HUMAN)
                allowItemPurchase = true;
            else if (curRaceSpecific == RaceSpecific.BEAST &&
                GameManager.Instance.activeRoomHeroes[i].curUnitRace == UnitFunctionality.UnitRace.BEAST)
                allowItemPurchase = true;
            else if (curRaceSpecific == RaceSpecific.ETHEREAL &&
                GameManager.Instance.activeRoomHeroes[i].curUnitRace == UnitFunctionality.UnitRace.ETHEREAL)
                allowItemPurchase = true;
            else if (curRaceSpecific == RaceSpecific.ALL)
                allowItemPurchase = true;
        }

        ToggleButtonPurchase(true);
        if (price > ShopManager.Instance.GetPlayerGold() || !allowItemPurchase)
            ToggleButtonPurchaseCover(true);
        else if (price <= ShopManager.Instance.GetPlayerGold())
            ToggleButtonPurchaseCover(false);
          
        AudioManager.Instance.Play("Button_Click");

        gameObject.GetComponentInChildren<ButtonFunctionality>().ButtonSelectItemCo();

        bool gear = true;
        if (linkedItemPiece)
            gear = false;

        int combatCount = 0;

        if (!gear)
            combatCount = ShopManager.Instance.GetShopCombatItems().Count;
        else
            combatCount = ShopManager.Instance.GetShopCombatGear().Count;
        for (int i = 0; i < combatCount; i++)
        {
            if (!gear)
            {
                if (shopItemName == ShopManager.Instance.GetShopCombatItems()[i].itemName)
                {
                    OverlayUI.Instance.ToggleFighterDetailsTab(true);
                    OverlayUI.Instance.UpdateItemDetailsUI(ShopManager.Instance.GetShopCombatItems()[i].itemName,
                    ShopManager.Instance.GetShopCombatItems()[i].itemDesc,
                    ShopManager.Instance.GetShopCombatItems()[i].itemPower,
                    ShopManager.Instance.GetShopCombatItems()[i].range,
                    ShopManager.Instance.GetShopCombatItems()[i].itemRangeHitArea,
                    ShopManager.Instance.GetShopCombatItems()[i].itemSpriteCombatSmaller);

                    GameManager.Instance.ToggleUIElement(GameManager.Instance.fighterSelectedMainSlotDesc, true);

                    OverlayUI.Instance.ToggleSkillItemSwitchButton(false);

                    OverlayUI.Instance.ToggleItemRarityTextUI(true);
                    OverlayUI.Instance.UpdateItemRarityText(ShopManager.Instance.GetShopCombatItems()[i].curRarity.ToString());
                    OverlayUI.Instance.ToggleActiveItemTriggerStatus(true);
                    if (ShopManager.Instance.GetShopCombatItems()[i].curActiveType == ItemPiece.ActiveType.ACTIVE)
                    {
                        OverlayUI.Instance.UpdateActiveItemUseCountText(ShopManager.Instance.GetShopCombatItems()[i].maxUsesPerCombat);
                        OverlayUI.Instance.UpdateActiveItemTriggerStatus(true);
                    }
                    else
                    {
                        OverlayUI.Instance.UpdateActiveItemUseCountText(0);
                        OverlayUI.Instance.UpdateActiveItemTriggerStatus(false);
                    }

                    string race = "";
                    race = ShopManager.Instance.GetShopCombatItems()[i].curRace.ToString();
                    OverlayUI.Instance.UpdateActiveItemRaceSpecificIcon(race);

                    FighterInventorManager.Instance.ToggleInventoryMode(false, true, true);
                    break;
                }
            }
            else
            {
                if (shopItemName == ShopManager.Instance.GetShopCombatGear()[i].gearName)
                {
                    OverlayUI.Instance.ToggleFighterDetailsTab(true);
                    OverlayUI.Instance.UpdateGearDetailsUI(ShopManager.Instance.GetShopCombatGear()[i].gearName,
                    "",
                    ShopManager.Instance.GetShopCombatGear()[i].bonusHealth,
                    ShopManager.Instance.GetShopCombatGear()[i].bonusDamage,
                    ShopManager.Instance.GetShopCombatGear()[i].bonusHealing,
                    ShopManager.Instance.GetShopCombatGear()[i].bonusSpeed, 0, ShopManager.Instance.GetShopCombatGear()[i].gearIcon);

                    OverlayUI.Instance.ToggleActiveItemTriggerStatus(true);
                    GameManager.Instance.ToggleUIElement(GameManager.Instance.fighterSelectedMainSlotDesc, true);

                    OverlayUI.Instance.ToggleSkillItemSwitchButton(false);

                    OverlayUI.Instance.ToggleItemRarityTextUI(true);
                    OverlayUI.Instance.UpdateItemRarityText(ShopManager.Instance.GetShopCombatGear()[i].gearRarity);
                    OverlayUI.Instance.UpdateActiveItemTriggerStatus(false, true);

                    string race = "";
                    //race = ShopManager.Instance.GetShopCombatGear()[i].curRace.ToString();
                    OverlayUI.Instance.UpdateActiveItemRaceSpecificIcon(race);

                    FighterInventorManager.Instance.ToggleInventoryMode(true, false, true);
                    FighterInventorManager.Instance.UpdateFighterInventorySelection();
                    break;
                }
            }

        }

        if (linkedGearPiece)
            OverlayUI.Instance.UpdateShopDetailsBanner(null, linkedGearPiece, null);
        else if (linkedItemPiece)
            OverlayUI.Instance.UpdateShopDetailsBanner(null, null, linkedItemPiece);

        OverlayUI.Instance.ToggleCombatBorder(false);
    }
}
