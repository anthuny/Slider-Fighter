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


    public Button itemButton;

    private int price;
    public bool purchased;


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

    public void PurchaseShopItem()
    {
        // If the player must first select an ally to give an item, do not allow purchase of another item.
        if (ShopManager.Instance.selectAlly)
            return;

        int playerGold = ShopManager.Instance.GetPlayerGold();
        if (playerGold < price)     // If player cannot afford the item, cancel.
            return;

        // Purchase the item
        imageUI.UpdateAlpha(0);
        textUI.UpdateAlpha(0);

        int count = ShopManager.Instance.GetShopItems().Count;
        for (int i = 0; i < count; i++)
        {
            if (shopItemName == ShopManager.Instance.GetShopItems()[i].itemName)
            {
                // Disable exit button until player has selected ally with item
                MapManager.Instance.exitShopRoom.UpdateAlpha(0);

                Item item = ShopManager.Instance.GetShopItems()[i];
                ShopManager.Instance.GetActiveRoom().AddOwnedShopItems(item);
                ShopManager.Instance.UpdateUnAssignedItem(item);
                // Prompt player on who to give item
                GameManager.Instance.ToggleAllowSelection(true);
                ShopManager.Instance.shopSelectAllyPrompt.UpdateAlpha(1);
                ShopManager.Instance.selectAlly = true;
                return;
            }
        }


    }
}
