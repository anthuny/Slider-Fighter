using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
        public enum RarityType { COMMON, RARE, EPIC, LEGENDARY }
    public RarityType curRarityType;

    [SerializeField] private GameObject rarityCommonGO;
    [SerializeField] private GameObject rarityRareGO;
    [SerializeField] private GameObject rarityEpicGO;
    [SerializeField] private GameObject rarityLegendaryGO;

    [SerializeField] private string shopItemName;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image image;
    [SerializeField] private UIElement imageUI;
    [SerializeField] private UIElement textUI;
    [SerializeField] private Animator itemAnimator;
    [SerializeField] private Animator rarityAnimator;


    public Button itemButton;

    private int price;
    private bool purchased;

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

    public void UpdatePriceText(string priceText)
    {
        this.priceText.text = priceText;

        if (priceText != "")
            price = int.Parse(priceText);
    }

    public void UpdateShopItemSprite(Sprite sprite)
    {
        image.sprite = sprite;
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
        int playerGold = ShopManager.Instance.GetPlayerGold();

        // Try Purchase Action
        if (playerGold < price)     // If player cannot afford the item, cancel.
            return;
        else
            ShopManager.Instance.UpdatePlayerGold(-price);

        // Purchase the item
        imageUI.UpdateAlpha(0);
        textUI.UpdateAlpha(0);

        GameManager.Instance.ToggleAllowSelection(true);
        GameManager.Instance.ToggleAllyUnitSelection(true);

        MapManager.Instance.UpdateMapGoldText();
        ShopManager.Instance.ToggleShopGoldText(true);

        int combatCount = ShopManager.Instance.GetShopCombatItems().Count;
        for (int i = 0; i < combatCount; i++)
        {
            if (shopItemName == ShopManager.Instance.GetShopCombatItems()[i].itemName)
            {
                // Disable exit button until player has selected ally with item
                //MapManager.Instance.exitShopRoom.UpdateAlpha(0);

                ShopManager.Instance.ToggleExitShopButton(false);

                ItemPiece item = ShopManager.Instance.GetShopCombatItems()[i];
                ShopManager.Instance.GetActiveRoom().AddPurchasedItems(item);
                ShopManager.Instance.UpdateUnAssignedItem(item);
                // Prompt player on who to give item
                GameManager.Instance.ToggleAllowSelection(true);
                //ShopManager.Instance.shopSelectAllyPrompt.UpdateAlpha(1);
                ShopManager.Instance.selectAlly = true;

                // Buy item SFX
                AudioManager.Instance.Play("Shop_Item_Buy");

                OverlayUI.Instance.ToggleFighterDetailsTab(true);
                OverlayUI.Instance.UpdateItemUI(ShopManager.Instance.GetShopCombatItems()[i].itemName, 
                ShopManager.Instance.GetShopCombatItems()[i].itemDesc, 
                ShopManager.Instance.GetShopCombatItems()[i].itemPower, 
                ShopManager.Instance.GetShopCombatItems()[i].targetCount, 
                ShopManager.Instance.GetShopCombatItems()[i].itemSpriteCombatSmaller);

                UpdatePurchased(true);

                return;
            }
        }
    }

    public void SelectShopItem(bool shopCombatItem)
    {
        // If the player must first select an ally to give an item, do not allow purchase of another item.
        //if (ShopManager.Instance.selectAlly)
        //    return;

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        gameObject.GetComponentInChildren<ButtonFunctionality>().ButtonSelectItemCo();

        int combatCount = ShopManager.Instance.GetShopCombatItems().Count;
        for (int i = 0; i < combatCount; i++)
        {
            if (shopItemName == ShopManager.Instance.GetShopCombatItems()[i].itemName)
            {
                OverlayUI.Instance.ToggleFighterDetailsTab(true);
                OverlayUI.Instance.UpdateItemUI(ShopManager.Instance.GetShopCombatItems()[i].itemName, 
                ShopManager.Instance.GetShopCombatItems()[i].itemDesc, 
                ShopManager.Instance.GetShopCombatItems()[i].itemPower, 
                ShopManager.Instance.GetShopCombatItems()[i].targetCount, 
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
                
                return;
            }
        }
    }
}
