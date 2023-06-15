using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image image;

    public void UpdatePriceText(string priceText)
    {
        this.priceText.text = priceText;
    }

    public void UpdateShopItemSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
