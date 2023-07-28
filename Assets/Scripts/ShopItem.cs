using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private string shopItemName;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image image;
    [SerializeField] private UIElement imageUI;
    [SerializeField] private UIElement textUI;
    [SerializeField] private Animator animator;


    public Button itemButton;

    private int price;
    private bool purchased;

    public void UpdateAnimatorController(RuntimeAnimatorController ac)
    {
        animator.runtimeAnimatorController = ac;
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

    public void PurchaseShopItem(bool shopCombatItem)
    {
        // If the player must first select an ally to give an item, do not allow purchase of another item.
        if (ShopManager.Instance.selectAlly)
            return;

        int playerGold = ShopManager.Instance.GetPlayerGold();
        if (playerGold < price)     // If player cannot afford the item, cancel.
            return;
        else
            ShopManager.Instance.UpdatePlayerGold(-price);

        // Purchase the item
        imageUI.UpdateAlpha(0);
        textUI.UpdateAlpha(0);

        // Combat Item
        if (shopCombatItem)
        {
            int combatCount = ShopManager.Instance.GetShopCombatItems().Count;
            for (int i = 0; i < combatCount; i++)
            {
                if (shopItemName == ShopManager.Instance.GetShopCombatItems()[i].itemName)
                {
                    // Disable exit button until player has selected ally with item
                    MapManager.Instance.exitShopRoom.UpdateAlpha(0);

                    Item item = ShopManager.Instance.GetShopCombatItems()[i];
                    ShopManager.Instance.GetActiveRoom().AddPurchasedItems(item);
                    ShopManager.Instance.UpdateUnAssignedItem(item);
                    // Prompt player on who to give item
                    GameManager.Instance.ToggleAllowSelection(true);
                    ShopManager.Instance.shopSelectAllyPrompt.UpdateAlpha(1);
                    ShopManager.Instance.selectAlly = true;
            
                    UpdatePurchased(true);

                    return;
                }
            }
        }
        // Health Item
        else
        {
            int healthCount = ShopManager.Instance.GetShopHealthItems().Count;
            for (int i = 0; i < healthCount; i++)
            {
                if (shopItemName == ShopManager.Instance.GetShopHealthItems()[i].itemName)
                {
                    // Disable exit button until player has selected ally with item
                    MapManager.Instance.exitShopRoom.UpdateAlpha(0);

                    Item item = ShopManager.Instance.GetShopHealthItems()[i];
                    ShopManager.Instance.GetActiveRoom().AddPurchasedItems(item);
                    ShopManager.Instance.UpdateUnAssignedItem(item);
                    // Prompt player on who to give item
                    GameManager.Instance.ToggleAllowSelection(true);
                    ShopManager.Instance.shopSelectAllyPrompt.UpdateAlpha(1);
                    ShopManager.Instance.selectAlly = true;

                    UpdatePurchased(true);

                    return;
                }
            }
        }

    }
}
