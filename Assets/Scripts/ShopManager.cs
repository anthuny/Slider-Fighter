using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [SerializeField] private int maxShopItems = 3;
    [SerializeField] private List<Item> shopItems = new List<Item>();

    [SerializeField] private Transform shopItemsParent;
    [SerializeField] private GameObject shopItemPrefab;

    private void Awake()
    {
        Instance = this;
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
            items.Add(GetRandomShopItem());
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

        for (int i = 0; i < maxShopItems; i++)
        {
            // Add new items
            GameObject go = Instantiate(shopItemPrefab, shopItemsParent);
            go.transform.SetParent(shopItemsParent);
            go.transform.localScale = new Vector2(1, 1);

            // Update price and sprite
            ShopItem shopItem = go.GetComponent<ShopItem>();
            shopItem.UpdatePriceText(ChooseItems()[i].basePrice.ToString());
            shopItem.UpdateShopItemSprite(ChooseItems()[i].itemSprite);
        }
    }
}
