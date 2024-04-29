using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemRewardManager : MonoBehaviour
{
    public static ItemRewardManager Instance;

    public bool disableItemRewards = false;

    [Space(5)]

    public float postCombatTillItemTime2 = 3f;
    public float postCombatTillItemTime = 1.5f;
    public float postItemTillPostCombat = 1;

    public float itemPromptTextRevealPost = .35f;
    public float itemTimeBetweenReveal = .2f;
    public float itemTimeBetweenRaritySFX = .1f;

    [SerializeField] private Transform itemsParent;
    public GameObject itemGO;
    [SerializeField] private int itemChoiceCountRegular = 3;
    [SerializeField] private int itemChoiceCountHero = 3;
    [SerializeField] private int itemChoiceCountBoss = 3;

    [SerializeField] private UIElement chooseItemPromptText;
    [SerializeField] private ButtonFunctionality confirmItemButton;
    public UIElement itemDescriptionUI;

    [Space(5)]
    [Header("Item Rarity")]
    public Color commonColour;
    public Color rareColour;
    public Color epicColour;
    public Color legendaryColour;

    [Tooltip("% chance of item dropping as rarity Common")]
    [SerializeField] private int itemCommonPerc;
    [Tooltip("% chance of item dropping as rarity Rare")]
    [SerializeField] private int itemRarePerc;
    [Tooltip("% chance of item dropping as rarity Epic")]
    [SerializeField] private int itemEpicPerc;
    [Tooltip("% chance of item dropping as rarity Legendary")]
    [SerializeField] private int itemLegendaryPerc;

    [Space(5)]
    [SerializeField] private List<ItemPiece> allItems = new List<ItemPiece>();
    [Space(5)]
    public List<ItemPiece> allItemCommon = new List<ItemPiece>();
    [Space(3)]
    public List<ItemPiece> allItemRare = new List<ItemPiece>();
    [Space(3)]
    public List<ItemPiece> allItemEpic = new List<ItemPiece>();
    [Space(3)]
    public List<ItemPiece> allItemLegendary = new List<ItemPiece>();
    
    [HideInInspector]
    public UIElement uiElement;

    [HideInInspector]
    public bool itemSelected;
    public string selectedItemName;
    public ItemPiece selectedItem;
    public List<UIElement> offeredItemsUI = new List<UIElement>();

    private void Awake()
    {
        Instance = this;
        uiElement = GetComponent<UIElement>();
    }

    void Start()
    {
        SortItems();

        Setup();
        ToggleItemRewards(false);
    }

    void SortItems()
    {
        for (int i = 0; i < allItems.Count; i++)
        {
            if (allItems[i].curRarity == ItemPiece.Rarity.COMMON)
                allItemCommon.Add(allItems[i]);
            else if (allItems[i].curRarity == ItemPiece.Rarity.RARE)
                allItemRare.Add(allItems[i]);
            else if (allItems[i].curRarity == ItemPiece.Rarity.EPIC)
                allItemEpic.Add(allItems[i]);
            else if (allItems[i].curRarity == ItemPiece.Rarity.LEGENDARY)
                allItemLegendary.Add(allItems[i]);
        }
    }

    public void ToggleItemRewards(bool toggle)
    {
        if (toggle)
            uiElement.UpdateAlpha(1);
        else
            uiElement.UpdateAlpha(0);
    }

    public void ToggleConfirmItemButton(bool toggle)
    {
        confirmItemButton.ToggleButton(toggle);
    }

    public void Setup()
    {
        ResetRewardsTable();

        ToggleConfirmItemButton(false);

        offeredItemsUI.Clear();

        UpdateItemDescription(false);
        //itemSelected = false;
    }

    public void ClearItemSelection()
    {
        for (int i = 0; i < offeredItemsUI.Count; i++)
        {
            offeredItemsUI[i].ToggleSelected(false);
        }
    }

    public void FillItemsTable()
    {
        Setup();

        ResetRewardsTable();
        chooseItemPromptText.UpdateAlpha(0);

        ToggleItemRewards(true);

        // Give item(s)
        StartCoroutine(GiveItems());
    }

    public void ResetRewardsTable()
    {
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }
    }

    ItemPiece GetItem(string name)
    {
        for (int i = 0; i < allItems.Count; i++)
        {
            if (allItems[i].itemName == name)
            {
                return allItems[i];
            }
        }

        return null;
    }
    public void UpdateItemDescription(bool toggle)
    {
        if (toggle)
        {
            selectedItem = GetItem(selectedItemName);
            itemDescriptionUI.UpdateAlpha(1);
            itemDescriptionUI.UpdateContentText(selectedItem.itemName);
            itemDescriptionUI.UpdateContentSubText(selectedItem.itemDesc);

            //itemSelected = true;
        }
        else
        {
            itemDescriptionUI.UpdateAlpha(0);   
        }
    }

    IEnumerator GiveItems()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        yield return new WaitForSeconds(itemPromptTextRevealPost);

        chooseItemPromptText.UpdateAlpha(1);    

        int count;

        if (RoomManager.Instance.GetActiveRoom().GetRoomType() == RoomMapIcon.RoomType.ENEMY)
        {
            count = itemChoiceCountRegular;
        }
        else if (RoomManager.Instance.GetActiveRoom().GetRoomType() == RoomMapIcon.RoomType.HERO)
        {
            count = itemChoiceCountBoss;
        }
        else if (RoomManager.Instance.GetActiveRoom().GetRoomType() == RoomMapIcon.RoomType.BOSS)
        {
            count = itemChoiceCountBoss;
        }
        else
            count = 3;

        List<ItemPiece> offeredItemsTemp = new List<ItemPiece>();

        // Create as many items as needed
        for (int i = 0; i < count; i++)
        {
            // Spawn Item
            GameObject go = Instantiate(itemGO, itemsParent.position, Quaternion.identity);
            go.transform.SetParent(itemsParent);
            go.transform.localScale = new Vector2(1, 1);

            UIElement uIElement = go.GetComponent<UIElement>();

            // Roll rarity chance 
            int itemChance = Random.Range(0, 101);

            if (allItemLegendary.Count > 0)
            {
                if (itemChance > itemLegendaryPerc)
                {
                    // spawn legendary item
                    int rand = Random.Range(0, allItemLegendary.Count);

                    if (offeredItemsTemp.Contains(allItemLegendary[rand]))
                    {
                        Destroy(go);
                        if (i > 0)
                            i--;
                        continue;
                    }

                    uIElement.UpdateContentImage(allItemLegendary[rand].itemSpriteItemTab);
                    uIElement.UpdateItemName(allItemLegendary[rand].itemName);
                    uIElement.UpdateRarityBorderColour(legendaryColour);
                    uIElement.curRarity = UIElement.Rarity.LEGENDARY;
                    offeredItemsTemp.Add(allItemLegendary[rand]);
                    offeredItemsUI.Add(uIElement);
                    uIElement.AnimateUI(false);
                    // Button Click SFX
                    AudioManager.Instance.Play("Button_Click");

                    AudioManager.Instance.Play("AttackBar_Bad");
                    yield return new WaitForSeconds(itemTimeBetweenRaritySFX);
                    AudioManager.Instance.Play("AttackBar_Good");
                    yield return new WaitForSeconds(itemTimeBetweenRaritySFX);
                    AudioManager.Instance.Play("AttackBar_Great");
                    yield return new WaitForSeconds(itemTimeBetweenRaritySFX);
                    AudioManager.Instance.Play("AttackBar_Perfect");

                    yield return new WaitForSeconds(itemTimeBetweenReveal);

                    continue;
                }
            }
            if (allItemEpic.Count > 0)
            {
                if (itemChance > itemEpicPerc)
                {
                    // spawn Epic item
                    int rand = Random.Range(0, allItemEpic.Count);

                    // Check if randomised item is already offered, if yes, reset for diff item
                    if (offeredItemsTemp.Contains(allItemEpic[rand]))
                    {
                        Destroy(go);
                        if (i > 0)
                            i--;
                        continue;
                    }

                    // Set item
                    uIElement.UpdateContentImage(allItemEpic[rand].itemSpriteItemTab);
                    uIElement.UpdateItemName(allItemEpic[rand].itemName);
                    uIElement.UpdateRarityBorderColour(epicColour);
                    uIElement.curRarity = UIElement.Rarity.EPIC;
                    offeredItemsTemp.Add(allItemEpic[rand]);
                    offeredItemsUI.Add(uIElement);
                    uIElement.AnimateUI(false);
                    // Button Click SFX
                    AudioManager.Instance.Play("Button_Click");

                    AudioManager.Instance.Play("AttackBar_Bad");
                    yield return new WaitForSeconds(itemTimeBetweenRaritySFX);
                    AudioManager.Instance.Play("AttackBar_Good");
                    yield return new WaitForSeconds(itemTimeBetweenRaritySFX);
                    AudioManager.Instance.Play("AttackBar_Great");

                    yield return new WaitForSeconds(itemTimeBetweenReveal);

                    continue;
                }
            }
            if (allItemRare.Count > 0)
            {
                if (itemChance > itemRarePerc)
                {
                    // spawn Rare item
                    int rand = Random.Range(0, allItemRare.Count);

                    // Check if randomised item is already offered, if yes, reset for diff item
                    if (offeredItemsTemp.Contains(allItemRare[rand]))
                    {
                        Destroy(go);
                        if (i > 0)
                            i--;
                        continue;
                    }

                    // Set item
                    uIElement.UpdateContentImage(allItemRare[rand].itemSpriteItemTab);
                    uIElement.UpdateItemName(allItemRare[rand].itemName);
                    uIElement.UpdateRarityBorderColour(rareColour);
                    uIElement.curRarity = UIElement.Rarity.RARE;
                    offeredItemsTemp.Add(allItemRare[rand]);
                    offeredItemsUI.Add(uIElement);
                    uIElement.AnimateUI(false);
                    // Button Click SFX
                    AudioManager.Instance.Play("Button_Click");

                    AudioManager.Instance.Play("AttackBar_Bad");
                    yield return new WaitForSeconds(itemTimeBetweenRaritySFX);
                    AudioManager.Instance.Play("AttackBar_Good");

                    yield return new WaitForSeconds(itemTimeBetweenReveal);

                    continue;
                }
            }
            if (allItemCommon.Count > 0)
            {
                if (itemChance >= itemCommonPerc)
                {
                    // spawn Common item
                    int rand = Random.Range(0, allItemCommon.Count);

                    // Check if randomised item is already offered, if yes, reset for diff item
                    if (offeredItemsTemp.Contains(allItemCommon[rand]))
                    {
                        Destroy(go);
                        if (i > 0)
                            i--;
                        continue;
                    }

                    // Set item
                    uIElement.UpdateContentImage(allItemCommon[rand].itemSpriteItemTab);
                    uIElement.UpdateItemName(allItemCommon[rand].itemName);
                    uIElement.UpdateRarityBorderColour(commonColour);
                    uIElement.curRarity = UIElement.Rarity.COMMON;
                    offeredItemsTemp.Add(allItemCommon[rand]);
                    offeredItemsUI.Add(uIElement);
                    uIElement.AnimateUI(false);
                    // Button Click SFX
                    AudioManager.Instance.Play("Button_Click");

                    AudioManager.Instance.Play("AttackBar_Bad");

                    yield return new WaitForSeconds(itemTimeBetweenReveal);

                    continue;
                }
            }


        }
    }
            /*
        // Roll if item drops
        if (itemChance >= unitDropRatePerc)
        {
        GameObject go = Instantiate(itemGO, itemsParent.position, Quaternion.identity);
        go.transform.SetParent(itemsParent);
        go.transform.localScale = new Vector2(1, 1);



        Item item = go.GetComponent<Item>();

        //item.isGold = false;


        //item.UpdateGearStatis(Item.GearStatis.REWARD);


        // Determine item rarity
        int rarityChance = Random.Range(0, 101);

        if (itemLegendaryPerc >= rarityChance)
        {
        item.UpdateRarity(Item.Rarity.LEGENDARY);
        }
        else if (itemEpicPerc >= rarityChance)
        {
        item.UpdateRarity(Item.Rarity.EPIC);
        }
        else if (itemRarePerc >= rarityChance)
        {
        item.UpdateRarity(Item.Rarity.RARE);
        }
        else if (itemCommonPerc >= rarityChance)
        {
        item.UpdateRarity(Item.Rarity.COMMON);
        }

        GearPiece newGear = null;

        // Determine what gear this item is
        if (item.GetRarity() == Item.Rarity.COMMON)
        {
        int rand = Random.Range(0, allItemPiecesCommon.Count);
        newGear = allItemPiecesCommon[rand];

        OwnedGearInven.Instance.AddOwnedGear(item);
        }
        else if (item.GetRarity() == Item.Rarity.RARE)
        {
        int rand = Random.Range(0, allItemPiecesRare.Count);
        newGear = allItemPiecesRare[rand];
        item.UpdateGearImage(newGear.gearIcon);
        OwnedGearInven.Instance.AddOwnedGear(item);
        }
        else if (item.GetRarity() == Item.Rarity.EPIC)
        {
        int rand = Random.Range(0, allItemPiecesEpic.Count);
        newGear = allItemPiecesEpic[rand];
        item.UpdateGearImage(newGear.gearIcon);
        OwnedGearInven.Instance.AddOwnedGear(item);
        }
        else if (item.GetRarity() == Item.Rarity.LEGENDARY)
        {
        int rand = Random.Range(0, allItemPiecesLegendary.Count);
        newGear = allItemPiecesLegendary[rand];
        OwnedGearInven.Instance.AddOwnedGear(item);
        }

        // Update gear type
        if (newGear.gearType == "helmet")
        {
        item.UpdateCurGearType(Item.GearType.HELMET);
        }
        else if (newGear.gearType == "chestpiece")
        {
        item.UpdateCurGearType(Item.GearType.CHESTPIECE);
        }
        else if (newGear.gearType == "leggings")
        {
        item.UpdateCurGearType(Item.GearType.LEGGINGS);
        }
        else if (newGear.gearType == "boots")
        {
        item.UpdateCurGearType(Item.GearType.BOOTS);
        }

        // Initialize gear base data
        item.UpdateGearImage(newGear.gearIcon);
        item.UpdateGearImage(newGear.gearIcon);
        item.UpdateGearName(newGear.gearName);
        item.UpdateGearBonusHealth(newGear.bonusHealth);
        item.UpdateGearBonusDamage(newGear.bonusDamage);
        item.UpdateGearBonusHealing(newGear.bonusHealing);
        item.UpdateGearBonusDefense(newGear.bonusDefense);
        item.UpdateGearBonusSpeed(newGear.bonusSpeed);

        // Disable owned gear button for unowned loot
        item.ToggleOwnedGearButton(false);
        // Show full visibility of item
        item.UpdateGearAlpha(true);
        */
}
