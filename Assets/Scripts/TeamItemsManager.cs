using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamItemsManager : MonoBehaviour
{
    public static TeamItemsManager Instance;

    [SerializeField] private UIElement selectedItemRarityText;

    [SerializeField] private UIElement fighterRaceIcon;
    [SerializeField] private UIElement teamItemsTabUI;
    [SerializeField] private ButtonFunctionality toMapButton;
    [SerializeField] private ButtonFunctionality unEquipButton;

    public Sprite clearSlotSprite;

    public ButtonFunctionality teamSetupTabArrowLeftButton;
    public ButtonFunctionality teamSetupTabArrowRightButton;

    [SerializeField] private UIElement ally1ItemsTabUI;
    [SerializeField] private UIElement ally2ItemsTabUI;
    [SerializeField] private UIElement ally3ItemsTabUI;

    [SerializeField] private MenuUnitDisplay ally1MenuUnitDisplay;
    [SerializeField] private MenuUnitDisplay ally2MenuUnitDisplay;
    [SerializeField] private MenuUnitDisplay ally3MenuUnitDisplay;

    public List<Slot> ally1ItemsSlots = new List<Slot>();
    public List<Slot> ally2ItemsSlots = new List<Slot>();
    public List<Slot> ally3ItemsSlots = new List<Slot>();

    [SerializeField] private UIElement itemsStatsUI;
    [SerializeField] private GameObject itemsStatGO;
    [SerializeField] private UIElement itemsDescUI;
    public UIElement itemsNameText;

    public Slot selectedItemSlot;
    public Slot selectedBaseItemSlot;

    public UIElement skillBase1;
    public UIElement skillBase2;
    public UIElement skillBase3;
    public UIElement skillBase4;

    [Space(2)]
    [Header("Main Ally")]
    public List<ItemPiece> equippedItemsMain = new List<ItemPiece>();
    [Space(2)]
    [Header("Second Ally")]
    public List<ItemPiece> equippedItemsSecond = new List<ItemPiece>();
    [Space(2)]
    [Header("Third Ally")]
    public List<ItemPiece> equippedItemsThird = new List<ItemPiece>();

    private int itemsSpawned;

    public void ToggleItemRarityText(bool toggle = true)
    {
        if (toggle)
        {
            selectedItemRarityText.UpdateAlpha(1);
        }
        else
        {
            selectedItemRarityText.UpdateAlpha(0);
        }

        if (toggle)
        {
            if (GetSelectedItemSlot().linkedItemPiece)
            {
                if (GetSelectedItemSlot().linkedItemPiece.curRarity == ItemPiece.Rarity.COMMON)
                {
                    selectedItemRarityText.UpdateContentText("COMMON");
                    selectedItemRarityText.UpdateContentTextColour(ItemRewardManager.Instance.commonColour);
                }
                else if (GetSelectedItemSlot().linkedItemPiece.curRarity == ItemPiece.Rarity.RARE)
                {
                    selectedItemRarityText.UpdateContentText("RARE");
                    selectedItemRarityText.UpdateContentTextColour(ItemRewardManager.Instance.rareColour);
                }
                else if (GetSelectedItemSlot().linkedItemPiece.curRarity == ItemPiece.Rarity.EPIC)
                {
                    selectedItemRarityText.UpdateContentText("EPIC");
                    selectedItemRarityText.UpdateContentTextColour(ItemRewardManager.Instance.epicColour);
                }
                else if (GetSelectedItemSlot().linkedItemPiece.curRarity == ItemPiece.Rarity.LEGENDARY)
                {
                    selectedItemRarityText.UpdateContentText("LEGENDARY");
                    selectedItemRarityText.UpdateContentTextColour(ItemRewardManager.Instance.legendaryColour);
                }
            }
        }
    }

    public void IncItemsSpawned()
    {
        itemsSpawned++;
    }

    public void ResetItemsSpawned()
    {
        itemsSpawned = 0;
    }

    public int GetItemsSpawned()
    {
        return itemsSpawned;
    }
    [Space(2)]
    public bool playerInItemTab;

    public void IncItemUseCount(Slot itemSlot)
    {
        itemSlot.IncUseCount();
    }

    public void RemoveMainItem(ItemPiece item)
    {
        equippedItemsMain.Remove(item);
    }
    public void RemoveSecondItem(ItemPiece item)
    {
        equippedItemsSecond.Remove(item);
    }

    public void RemoveThirdItem(ItemPiece item)
    {
        equippedItemsThird.Remove(item);
    }


    public void UpdateLockedItems()
    {
        skillBase1.ToggleLockedMainSlot();
        skillBase2.ToggleLockedMainSlot();
        skillBase3.ToggleLockedMainSlot();
        skillBase4.ToggleLockedMainSlot();
    }

    public void UpdateUnequiptItemAlert()
    {
        bool toggle = false;

        if (OwnedLootInven.Instance.ownedItems.Count > 0)
            toggle = true;

        if (toggle)
        {
            if (OwnedLootInven.Instance.GetWornItemThirdAlly().Count < 3 && GameManager.Instance.activeRoomHeroes.Count == 3 
                || OwnedLootInven.Instance.GetWornItemSecondAlly().Count < 3 && GameManager.Instance.activeRoomHeroes.Count >= 2
                || OwnedLootInven.Instance.GetWornItemMainAlly().Count < 3 && GameManager.Instance.activeRoomHeroes.Count >= 1)
            {
                MapManager.Instance.mapOverlay.alertItemUnequipt.gameObject.SetActive(true);
                MapManager.Instance.mapOverlay.alertItemUnequipt.UpdateAlpha(1);
            }
            else
            {
                MapManager.Instance.mapOverlay.alertItemUnequipt.gameObject.SetActive(false);
                MapManager.Instance.mapOverlay.alertItemUnequipt.UpdateAlpha(0);
            }
        }
        else
        {
            MapManager.Instance.mapOverlay.alertItemUnequipt.gameObject.SetActive(false);
            MapManager.Instance.mapOverlay.alertItemUnequipt.UpdateAlpha(0);
        }
    }

    public void ToggleToMapButton(bool toggle)
    {
        toMapButton.ToggleButton(toggle);
    }

    public void ToggleUnequipButton(bool toggle)
    {
        unEquipButton.ToggleButton(toggle);
    }

    public void UpdateItemNameText(string name)
    {
        itemsNameText.UpdateContentText(name);
    }

    public void UpdateItemDesc(string desc)
    {
        // Gear Stat Description Update
        itemsDescUI.UpdateContentText(desc);
    }

    public void ClearAllGearStats()
    {
        // Clear all gear stats
        GameObject gearStatGO = itemsStatsUI.gameObject;
        for (int i = 0; i < gearStatGO.transform.childCount; i++)
        {
            Destroy(gearStatGO.transform.GetChild(i).gameObject);
        }

        GameObject gearDescGO = itemsDescUI.gameObject;
        for (int x = 0; x < gearDescGO.transform.childCount; x++)
        {
            Destroy(gearDescGO.transform.GetChild(x).gameObject);
        }
    }

    public void UpdateUnitStatsEquip(Slot gear)
    {
        if (selectedBaseItemSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.MAIN)
        {
            UnitFunctionality unitFunc = GameManager.Instance.activeRoomAllUnitFunctionalitys[0];

            unitFunc.UpdateUnitMaxHealth(gear.GetBonusHealth(), false, true);
            unitFunc.UpdateUnitCurHealth(gear.GetBonusHealth(), false, false);
            unitFunc.UpdateUnitPowerHits(gear.GetBonusDamage(), true);
            unitFunc.UpdateUnitHealingHits(gear.GetBonusHealing(), true);
            unitFunc.UpdateUnitDefenseChange(gear.GetBonusDefense(), true);
            unitFunc.UpdateUnitSpeedChange(gear.GetBonusSpeed(), true);

            ally1MenuUnitDisplay.UpdateUnitStats(unitFunc);
        }
        else if (selectedBaseItemSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.SECOND)
        {
            UnitFunctionality unitFunc = GameManager.Instance.activeRoomAllUnitFunctionalitys[1];

            unitFunc.UpdateUnitMaxHealth(gear.GetBonusHealth(), false, true);
            unitFunc.UpdateUnitCurHealth(gear.GetBonusHealth(), false, false);
            unitFunc.UpdateUnitPowerHits(gear.GetBonusDamage(), true);
            unitFunc.UpdateUnitHealingHits(gear.GetBonusHealing(), true);
            unitFunc.UpdateUnitDefenseChange(gear.GetBonusDefense(), true);
            unitFunc.UpdateUnitSpeedChange(gear.GetBonusSpeed(), true);

            ally2MenuUnitDisplay.UpdateUnitStats(unitFunc);
        }
        else if (selectedBaseItemSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.THIRD)
        {
            UnitFunctionality unitFunc = GameManager.Instance.activeRoomAllUnitFunctionalitys[2];

            unitFunc.UpdateUnitMaxHealth(gear.GetBonusHealth(), false, true);
            unitFunc.UpdateUnitCurHealth(gear.GetBonusHealth(), false, false);
            unitFunc.UpdateUnitPowerHits(gear.GetBonusDamage(), true);
            unitFunc.UpdateUnitHealingHits(gear.GetBonusHealing(), true);
            unitFunc.UpdateUnitDefenseChange(gear.GetBonusDefense(), true);
            unitFunc.UpdateUnitSpeedChange(gear.GetBonusSpeed(), true);

            ally3MenuUnitDisplay.UpdateUnitStats(unitFunc);
        }
    }

    public void UpdateUnitStatsUnEquip(Slot gear)
    {
        if (selectedBaseItemSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.MAIN)
        {
            UnitFunctionality unitFunc = GameManager.Instance.activeRoomAllUnitFunctionalitys[0];

            unitFunc.UpdateUnitCurHealth(gear.GetBonusHealth(), true, false, false);
            unitFunc.UpdateUnitMaxHealth(gear.GetBonusHealth(), false, false);
            unitFunc.UpdateUnitPowerHits(gear.GetBonusDamage(), false);
            unitFunc.UpdateUnitHealingHits(gear.GetBonusHealing(), false);
            unitFunc.UpdateUnitDefenseChange(gear.GetBonusDefense(), false);
            unitFunc.UpdateUnitSpeedChange(gear.GetBonusSpeed(), false);

            ally1MenuUnitDisplay.UpdateUnitStats(unitFunc);
        }
        else if (selectedBaseItemSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.SECOND)
        {
            UnitFunctionality unitFunc = GameManager.Instance.activeRoomAllUnitFunctionalitys[1];

            unitFunc.UpdateUnitCurHealth(gear.GetBonusHealth(), true, false, false);
            unitFunc.UpdateUnitMaxHealth(gear.GetBonusHealth(), false, false);
            unitFunc.UpdateUnitPowerHits(gear.GetBonusDamage(), false);
            unitFunc.UpdateUnitHealingHits(gear.GetBonusHealing(), false);
            unitFunc.UpdateUnitDefenseChange(gear.GetBonusDefense(), false);
            unitFunc.UpdateUnitSpeedChange(gear.GetBonusSpeed(), false);

            ally2MenuUnitDisplay.UpdateUnitStats(unitFunc);
        }
        else if (selectedBaseItemSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.THIRD)
        {
            UnitFunctionality unitFunc = GameManager.Instance.activeRoomAllUnitFunctionalitys[2];

            unitFunc.UpdateUnitCurHealth(gear.GetBonusHealth(), true, false, false);
            unitFunc.UpdateUnitMaxHealth(gear.GetBonusHealth(), false, false);
            unitFunc.UpdateUnitPowerHits(gear.GetBonusDamage(), false);
            unitFunc.UpdateUnitHealingHits(gear.GetBonusHealing(), false);
            unitFunc.UpdateUnitDefenseChange(gear.GetBonusDefense(), false);
            unitFunc.UpdateUnitSpeedChange(gear.GetBonusSpeed(), false);

            ally3MenuUnitDisplay.UpdateUnitStats(unitFunc);
        }
    }

    public void UpdateItemStatDetails()
    {
        ClearAllGearStats();

        // Gear Stats Update
        for (int i = 0; i < 5; i++)
        {
            GameObject spawnedStat = Instantiate(itemsStatGO, itemsStatsUI.transform.position, Quaternion.identity);
            spawnedStat.transform.SetParent(itemsStatsUI.transform);
            spawnedStat.transform.localPosition = Vector2.zero;
            spawnedStat.transform.localScale = Vector2.one;

            UIElement statUI = spawnedStat.GetComponent<UIElement>();

            // Update gear stat UI
            if (i == 0)
                statUI.UpdateContentText(GetSelectedItemSlot().GetBonusHealth().ToString());
            else if (i == 1)
                statUI.UpdateContentText(GetSelectedItemSlot().GetBonusDamage().ToString());
            else if (i == 2)
                statUI.UpdateContentText(GetSelectedItemSlot().GetBonusHealing().ToString());
            else if (i == 3)
                statUI.UpdateContentText(GetSelectedItemSlot().GetBonusDefense().ToString());
            else if (i == 4)
                statUI.UpdateContentText(GetSelectedItemSlot().GetBonusSpeed().ToString());
        }

        // Gear Stat Name Update
        itemsNameText.UpdateContentText(GetSelectedItemSlot().linkedItemPiece.itemName);

        // Gear Stat Description Update
        UpdateItemDesc(GetSelectedItemSlot().linkedItemPiece.itemDesc);

        if (playerInItemTab)
            ToggleItemRarityText(true);
        else
            ToggleItemRarityText(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        ToggleTeamItems(false);
        ClearItemSlots();
        ClearAllGearStats();
        ResetAllItemSelections();
        ToggleItemRarityText(false);
        ToggleTeamItemEquipMainButton(false);
    }

    private void Awake()
    {
        Instance = this;
    }

    public Slot GetSelectedItemSlot()
    {
        return selectedItemSlot;
    }

    public void UpdateSelectedItemSlot(Slot gear)
    {
        selectedItemSlot = gear;
    }

    public Slot GetSelectedBaseItemSlot()
    {
        return selectedBaseItemSlot;
    }

    public void UpdateSelectedBaseItemSlot(Slot gear)
    {
        selectedBaseItemSlot = gear;

        if (gear != null)
        gear.ToggleSlotSelection(true);
    }

    public void ReloadItemUses()
    {
        if (OwnedLootInven.Instance.GetWornItemMainAlly().Count > 0)
        {
            if (OwnedLootInven.Instance.GetWornItemMainAlly()[0])
            {
                if (OwnedLootInven.Instance.GetWornItemMainAlly()[0].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.REFILLABLE)
                    OwnedLootInven.Instance.GetWornItemMainAlly()[0].UpdateItemUses(0);
                else if (OwnedLootInven.Instance.GetWornItemMainAlly()[0].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                    OwnedLootInven.Instance.GetWornItemMainAlly()[0].ReduceItemUses();
            }
        }

        if (OwnedLootInven.Instance.GetWornItemMainAlly().Count > 1)
        {
            if (OwnedLootInven.Instance.GetWornItemMainAlly()[1])
            {
                if (OwnedLootInven.Instance.GetWornItemMainAlly()[1].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.REFILLABLE)
                    OwnedLootInven.Instance.GetWornItemMainAlly()[1].UpdateItemUses(0);
                else if (OwnedLootInven.Instance.GetWornItemMainAlly()[1].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                    OwnedLootInven.Instance.GetWornItemMainAlly()[1].ReduceItemUses();
            }

        }

        if (OwnedLootInven.Instance.GetWornItemMainAlly().Count > 2)
        {
            if (OwnedLootInven.Instance.GetWornItemMainAlly()[2])
            {
                if (OwnedLootInven.Instance.GetWornItemMainAlly()[2].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.REFILLABLE)
                    OwnedLootInven.Instance.GetWornItemMainAlly()[2].UpdateItemUses(0);
                else if (OwnedLootInven.Instance.GetWornItemMainAlly()[2].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                    OwnedLootInven.Instance.GetWornItemMainAlly()[2].ReduceItemUses();
            }
        }

        if (OwnedLootInven.Instance.GetWornItemSecondAlly().Count > 0)
        {
            if (OwnedLootInven.Instance.GetWornItemSecondAlly()[0])
            {
                if (OwnedLootInven.Instance.GetWornItemSecondAlly()[0].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.REFILLABLE)
                    OwnedLootInven.Instance.GetWornItemSecondAlly()[0].UpdateItemUses(0);
                else if (OwnedLootInven.Instance.GetWornItemSecondAlly()[0].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                    OwnedLootInven.Instance.GetWornItemSecondAlly()[0].ReduceItemUses();
            }
        }

        if (OwnedLootInven.Instance.GetWornItemSecondAlly().Count > 1)
        {
            if (OwnedLootInven.Instance.GetWornItemSecondAlly()[1])
            {
                if (OwnedLootInven.Instance.GetWornItemSecondAlly()[1].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.REFILLABLE)
                    OwnedLootInven.Instance.GetWornItemSecondAlly()[1].UpdateItemUses(0);
                else if (OwnedLootInven.Instance.GetWornItemSecondAlly()[1].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                    OwnedLootInven.Instance.GetWornItemSecondAlly()[1].ReduceItemUses();
            }
        }

        if (OwnedLootInven.Instance.GetWornItemSecondAlly().Count > 2)
        {
            if (OwnedLootInven.Instance.GetWornItemSecondAlly()[2])
            {
                if (OwnedLootInven.Instance.GetWornItemSecondAlly()[2].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.REFILLABLE)
                    OwnedLootInven.Instance.GetWornItemSecondAlly()[2].UpdateItemUses(0);
                else if (OwnedLootInven.Instance.GetWornItemSecondAlly()[2].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                    OwnedLootInven.Instance.GetWornItemSecondAlly()[1].ReduceItemUses();
            }
        }

        if (OwnedLootInven.Instance.GetWornItemThirdAlly().Count > 0)
        {
            if (OwnedLootInven.Instance.GetWornItemThirdAlly()[0])
            {
                if (OwnedLootInven.Instance.GetWornItemThirdAlly()[0].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.REFILLABLE)
                    OwnedLootInven.Instance.GetWornItemThirdAlly()[0].UpdateItemUses(0);
                else if (OwnedLootInven.Instance.GetWornItemThirdAlly()[0].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                    OwnedLootInven.Instance.GetWornItemThirdAlly()[0].ReduceItemUses();
            }
        }

        if (OwnedLootInven.Instance.GetWornItemThirdAlly().Count > 1)
        {
            if (OwnedLootInven.Instance.GetWornItemThirdAlly()[1])
            {
                if (OwnedLootInven.Instance.GetWornItemThirdAlly()[1].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.REFILLABLE)
                    OwnedLootInven.Instance.GetWornItemThirdAlly()[1].UpdateItemUses(0);
                else if (OwnedLootInven.Instance.GetWornItemThirdAlly()[1].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                    OwnedLootInven.Instance.GetWornItemThirdAlly()[1].ReduceItemUses();
            }
        }

        if (OwnedLootInven.Instance.GetWornItemThirdAlly().Count > 2)
        {
            if (OwnedLootInven.Instance.GetWornItemThirdAlly()[2])
            {
                if (OwnedLootInven.Instance.GetWornItemThirdAlly()[2].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.REFILLABLE)
                    OwnedLootInven.Instance.GetWornItemThirdAlly()[2].UpdateItemUses(0);
                else if (OwnedLootInven.Instance.GetWornItemThirdAlly()[2].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                    OwnedLootInven.Instance.GetWornItemThirdAlly()[2].ReduceItemUses();
            }
        }
    }

    public void ToggleTeamItemEquipMainButton(bool toggle = true)
    {
        for (int i = 0; i < ally1ItemsSlots.Count; i++)
        {
            ally1ItemsSlots[i].ToggleEquipMainButton(toggle);
        }
        for (int i = 0; i < ally2ItemsSlots.Count; i++)
        {
            ally2ItemsSlots[i].ToggleEquipMainButton(toggle);
        }
        for (int i = 0; i < ally3ItemsSlots.Count; i++)
        {
            ally3ItemsSlots[i].ToggleEquipMainButton(toggle);
        }
    }

    /*
    public void ResetItemTabSlots()
    {
        for (int i = 0; i < ally1ItemsSlots.Count; i++)
        {
            ally1ItemsSlots[i].ResetSlot();
        }
    }
    */

    public void UpdateItemSlotsBase(bool ally1 = false, bool ally2 = false, bool ally3 = false)
    {
        // Ensure each gear slot has correct bg gear sprite

        if (ally1)
        {
            for (int i = 0; i < ally1ItemsSlots.Count; i++)
            {
                if (!ally1ItemsSlots[i].linkedItemPiece)
                {
                    ally1ItemsSlots[i].isEmpty = true;
                    ally1ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
                }
                else
                {
                    if (ally1ItemsSlots[i].GetCalculatedItemUsesRemaining() <= 0 && ally1ItemsSlots[i].linkedSlot.linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                        ally1ItemsSlots[i].ResetSlot(true, true);
                }

                ally1ItemsSlots[i].UpdateGearStatis(Slot.SlotStatis.DEFAULT);
                ally1ItemsSlots[i].UpdateGearOwnedBy(Slot.SlotOwnedBy.MAIN);
            }
        }

        if (ally2)
        {
            for (int i = 0; i < ally2ItemsSlots.Count; i++)
            {
                if (!ally2ItemsSlots[i].linkedItemPiece)
                {
                    ally2ItemsSlots[i].isEmpty = true;
                    ally2ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
                }
                else
                {
                    if (ally2ItemsSlots[i].GetCalculatedItemUsesRemaining() <= 0 && ally2ItemsSlots[i].linkedSlot.linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                        ally2ItemsSlots[i].ResetSlot(true, true);
                }

                ally2ItemsSlots[i].UpdateGearStatis(Slot.SlotStatis.DEFAULT);
                ally2ItemsSlots[i].UpdateGearOwnedBy(Slot.SlotOwnedBy.SECOND);
            }
        }

        if (ally3)
        {
            for (int i = 0; i < ally3ItemsSlots.Count; i++)
            {
                if (!ally3ItemsSlots[i].linkedItemPiece)
                {
                    ally3ItemsSlots[i].isEmpty = true;
                    ally3ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
                }
                else
                {
                    if (ally3ItemsSlots[i].GetCalculatedItemUsesRemaining() <= 0 && ally3ItemsSlots[i].linkedSlot.linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                        ally3ItemsSlots[i].ResetSlot(true, true);
                }

                ally3ItemsSlots[i].UpdateGearStatis(Slot.SlotStatis.DEFAULT);
                ally3ItemsSlots[i].UpdateGearOwnedBy(Slot.SlotOwnedBy.THIRD);
            }
        }
    }

    public void ResetItemsTab()
    {
        if (GameManager.Instance.activeTeam.Count == 1)
            ResetHeroItemOwned(1);
        else if (GameManager.Instance.activeTeam.Count == 2)
            ResetHeroItemOwned(2);
    }

    public void ClearEmptyItemSlots()
    {
        UpdateItemSlotsBase(true, true, true);
    }

    public void ClearItemSlots()
    {
        for (int i = 0; i < ally3ItemsSlots.Count; i++)
        {
            ally1ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
            ally2ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
            ally3ItemsSlots[i].UpdateSlotImage(clearSlotSprite);

            ally1ItemsSlots[i].UpdateLinkedItemPiece(null);
            ally2ItemsSlots[i].UpdateLinkedItemPiece(null);
            ally3ItemsSlots[i].UpdateLinkedItemPiece(null);

            ally1ItemsSlots[i].UpdateSlotDetails();
            ally2ItemsSlots[i].UpdateSlotDetails();
            ally3ItemsSlots[i].UpdateSlotDetails();
        }
    }



    void ClearEmptyGearSlots()
    {
        /*
        if (GameManager.Instance.activeTeam.Count == 1)
            UpdateGearSlotsBase(true);
        else if (GameManager.Instance.activeTeam.Count == 2)
            UpdateGearSlotsBase(true, true);
        else if (GameManager.Instance.activeTeam.Count == 3)
            UpdateGearSlotsBase(true, true, true);
        */
    }

    public void ToggleAllyGearSets()
    {
        // If ally team has 1 total allies
        if (GameManager.Instance.activeTeam.Count == 1)
        {
            // Toggle correct gear tabs
            ally2ItemsTabUI.UpdateAlpha(0);
            ally3ItemsTabUI.UpdateAlpha(0);
            ally1ItemsTabUI.UpdateAlpha(1);

            // Display unit level image
            ally1MenuUnitDisplay.ToggleUnitLevelImage(true, GameManager.Instance.activeRoomHeroes[0].GetUnitLevel());

            // Update visible character ally 
            ally1MenuUnitDisplay.UpdateUnitDisplay(GameManager.Instance.activeTeam[0].unitName);

            // Ensure each gear slot has correct bg gear sprite
            //UpdateGearSlotsBase(true);
        }
        // If ally team has 2 total allies
        else if (GameManager.Instance.activeTeam.Count == 2)
        {
            // Toggle correct gear tabs
            ally3ItemsTabUI.UpdateAlpha(0);
            ally1ItemsTabUI.UpdateAlpha(1);
            ally2ItemsTabUI.UpdateAlpha(1);

            // Display unit level image
            ally1MenuUnitDisplay.ToggleUnitLevelImage(true, GameManager.Instance.activeRoomHeroes[0].GetUnitLevel());
            ally2MenuUnitDisplay.ToggleUnitLevelImage(true, GameManager.Instance.activeRoomHeroes[1].GetUnitLevel());

            // Update visible character ally 
            ally1MenuUnitDisplay.UpdateUnitDisplay(GameManager.Instance.activeTeam[0].unitName);
            ally2MenuUnitDisplay.UpdateUnitDisplay(GameManager.Instance.activeTeam[1].unitName);

            // Ensure each gear slot has correct bg gear sprite
            //UpdateGearSlotsBase(true, true);
        }
        // If ally team has 3 total allies
        else if (GameManager.Instance.activeTeam.Count == 3)
        {
            // Toggle correct gear tabs
            ally1ItemsTabUI.UpdateAlpha(1);
            ally2ItemsTabUI.UpdateAlpha(1);
            ally3ItemsTabUI.UpdateAlpha(1);

            // Display unit level image
            ally1MenuUnitDisplay.ToggleUnitLevelImage(true, GameManager.Instance.activeRoomHeroes[0].GetUnitLevel());
            ally2MenuUnitDisplay.ToggleUnitLevelImage(true, GameManager.Instance.activeRoomHeroes[1].GetUnitLevel());
            ally3MenuUnitDisplay.ToggleUnitLevelImage(true, GameManager.Instance.activeRoomHeroes[2].GetUnitLevel());

            // Update visible character ally 
            ally1MenuUnitDisplay.UpdateUnitDisplay(GameManager.Instance.activeTeam[0].unitName);
            ally2MenuUnitDisplay.UpdateUnitDisplay(GameManager.Instance.activeTeam[1].unitName);
            ally3MenuUnitDisplay.UpdateUnitDisplay(GameManager.Instance.activeTeam[2].unitName);

            // Ensure each gear slot has correct bg gear sprite
            //UpdateGearSlotsBase(true, true, true);
        }
    }

    public void ToggleTeamItems(bool toggle)
    {
        if (toggle)
        {
            for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
            {
                GameManager.Instance.activeRoomHeroes[i].ToggleUnitDisplay(false);
            }

            teamItemsTabUI.UpdateAlpha(1);


            ToggleTeamItemEquipMainButton(true);

            ToggleItemSlotSelection(true);

            for (int i = 0; i < ally1ItemsSlots.Count; i++)
            {
                if (!ally1ItemsSlots[i].linkedItemPiece)
                {
                    ally1ItemsSlots[i].UpdateLinkedItemPiece(null);
                    ally1ItemsSlots[i].UpdateSlotDetails();
                    ally1ItemsSlots[i].ToggleEquipButton(true);
                }
                else
                {
                    ally1ItemsSlots[i].UpdateSlotDetails();
                    ally1ItemsSlots[i].ToggleEquipButton(false);
                }
            }

            for (int i = 0; i < ally2ItemsSlots.Count; i++)
            {
                if (!ally2ItemsSlots[i].linkedItemPiece)
                {
                    ally2ItemsSlots[i].UpdateLinkedItemPiece(null);
                    ally2ItemsSlots[i].UpdateSlotDetails();
                    ally2ItemsSlots[i].ToggleEquipButton(true);
                }
                else
                {
                    ally2ItemsSlots[i].UpdateSlotDetails();
                    ally2ItemsSlots[i].ToggleEquipButton(false);
                }
            }

            for (int i = 0; i < ally3ItemsSlots.Count; i++)
            {
                if (!ally3ItemsSlots[i].linkedItemPiece)
                {
                    ally3ItemsSlots[i].UpdateLinkedItemPiece(null);
                    ally3ItemsSlots[i].UpdateSlotDetails();
                    ally3ItemsSlots[i].ToggleEquipButton(true);
                }
                else
                {
                    ally3ItemsSlots[i].UpdateSlotDetails();
                    ally3ItemsSlots[i].ToggleEquipButton(false);
                }
            }

            OwnedLootInven.Instance.ToggleOwnedGearDisplay(false);

            // Active unit level image for team page
            for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
            {
                GameManager.Instance.activeRoomHeroes[i].ToggleUnitLevelImage(true);
            }

            // Toggle to map button for team gear
            ToggleToMapButton(true);
            // Toggle Team setup to map button off for team gear page
            SkillsTabManager.Instance.ToggleToMapButton(false);

            // Disable team setup tab
            GameManager.Instance.SkillsTabChangeAlly(false);

            ToggleAllSlotsClickable(true, false);

            // Hide team setup tab arrow buttons
            SkillsTabManager.Instance.gearTabArrowLeftButton.ToggleButton(false);
            SkillsTabManager.Instance.gearTabArrowRightButton.ToggleButton(false);

            // Display gear tab arrow buttons
            teamSetupTabArrowLeftButton.ToggleButton(true);
            teamSetupTabArrowRightButton.ToggleButton(true);

            ToggleAllyGearSets();

            // Clears empty base gear slots if empty
            ClearEmptyGearSlots();

            ClearAllGearStats();

            if (GameManager.Instance.activeTeam.Count == 1)
            {
                string unitName = GameManager.Instance.activeTeam[0].unitName;

                for (int i = 0; i < GameManager.Instance.activeRoomAllUnitFunctionalitys.Count; i++)
                {
                    if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetUnitName() == unitName)
                    {
                        ally1MenuUnitDisplay.UpdateUnitStats(GameManager.Instance.activeRoomAllUnitFunctionalitys[i]);
                    }
                }
            }

            if (GameManager.Instance.activeTeam.Count == 2)
            {
                string unitName1 = GameManager.Instance.activeTeam[0].unitName;
                string unitName2 = GameManager.Instance.activeTeam[1].unitName;

                for (int i = 0; i < GameManager.Instance.activeRoomAllUnitFunctionalitys.Count; i++)
                {
                    if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetUnitName() == unitName1)
                    {
                        ally1MenuUnitDisplay.UpdateUnitStats(GameManager.Instance.activeRoomAllUnitFunctionalitys[i]);
                    }

                    if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetUnitName() == unitName2)
                    {
                        ally2MenuUnitDisplay.UpdateUnitStats(GameManager.Instance.activeRoomAllUnitFunctionalitys[i]);
                    }
                }
            }
            if (GameManager.Instance.activeTeam.Count == 3)
            {
                string unitName1 = GameManager.Instance.activeTeam[0].unitName;
                string unitName2 = GameManager.Instance.activeTeam[1].unitName;
                string unitName3 = GameManager.Instance.activeTeam[2].unitName;

                for (int i = 0; i < GameManager.Instance.activeRoomAllUnitFunctionalitys.Count; i++)
                {
                    if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetUnitName() == unitName1)
                    {
                        ally1MenuUnitDisplay.UpdateUnitStats(GameManager.Instance.activeRoomAllUnitFunctionalitys[i]);
                    }

                    if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetUnitName() == unitName2)
                    {
                        ally2MenuUnitDisplay.UpdateUnitStats(GameManager.Instance.activeRoomAllUnitFunctionalitys[i]);
                    }

                    if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetUnitName() == unitName3)
                    {
                        ally3MenuUnitDisplay.UpdateUnitStats(GameManager.Instance.activeRoomAllUnitFunctionalitys[i]);
                    }
                }
            }
        }
        else
        {
            teamItemsTabUI.UpdateAlpha(0);

            ToggleItemRarityText(false);

            UpdateSelectedBaseItemSlot(null);
            UpdateSelectedItemSlot(null);

            UpdateItemNameText("");
            UpdateItemDesc("");

            ToggleTeamItemEquipMainButton(false);

            // Toggle to map button for team gear
            ToggleToMapButton(false);
            SkillsTabManager.Instance.ToggleToMapButton(true);

            ResetAllItemSelections();

            ToggleAllSlotsClickable(false, false);

            // Hide team setup tab arrow buttons
            teamSetupTabArrowLeftButton.ToggleButton(false);
            teamSetupTabArrowRightButton.ToggleButton(false);

            // Display team setup tab arrow buttons
            SkillsTabManager.Instance.gearTabArrowLeftButton.ToggleButton(true);
            SkillsTabManager.Instance.gearTabArrowRightButton.ToggleButton(true);

            //UpdateEquipItemsOrder(true, true, true);

            ToggleItemSlotSelection(false);
        }
    }

    public void ToggleItemSlotSelection(bool toggle = true)
    {
        // Main Item Slots
        for (int i = 0; i < ally1ItemsSlots.Count; i++)
        {
            if (toggle)
                ally1ItemsSlots[i].GetRaceIcon().ToggleRaceIconButton(true);
            else
                ally1ItemsSlots[i].GetRaceIcon().ToggleRaceIconButton(false);
        }

        for (int i = 0; i < ally2ItemsSlots.Count; i++)
        {
            if (toggle)
                ally2ItemsSlots[i].GetRaceIcon().ToggleRaceIconButton(true);
            else
                ally2ItemsSlots[i].GetRaceIcon().ToggleRaceIconButton(false);
        }

        for (int i = 0; i < ally3ItemsSlots.Count; i++)
        {
            if (toggle)
                ally3ItemsSlots[i].GetRaceIcon().ToggleRaceIconButton(true);
            else
                ally3ItemsSlots[i].GetRaceIcon().ToggleRaceIconButton(false);
        }

        // Owned Item Slots
        for (int i = 0; i < OwnedLootInven.Instance.ownedLootSlots.Count; i++)
        {
            if (toggle)
                OwnedLootInven.Instance.ownedLootSlots[i].GetRaceIcon().ToggleRaceIconButton(true);
            else
                OwnedLootInven.Instance.ownedLootSlots[i].GetRaceIcon().ToggleRaceIconButton(false);
        }     
    }
    /*
    public void UpdateMainSlotLinkedSlot()
    {
        for (int i = 0; i < ally1ItemsSlots.Count; i++)
        {
            for (int y = 0; y < OwnedLootInven.Instance.ownedItems.Count; y++)
            {
                if (ally1ItemsSlots[i].linkedItemPiece == OwnedLootInven.Instance.ownedItems[y].linkedItemPiece)
                {

                }
            }


            for (int x = 0; x < OwnedLootInven.Instance.wornItemsMainAlly.Count; x++)
            {
                if (OwnedLootInven.Instance.wornItemsMainAlly[x].linkedItemPiece == ally1ItemsSlots[i].linkedItemPiece)
                {
                    //ally1ItemsSlots[i].UpdateLinkedSlot(OwnedLootInven.Instance.wornItemsMainAlly[x]);
                    ally1ItemsSlots[i].linkedSlot.slotIndex = i;
                }
            }
        }

        for (int i = 0; i < ally2ItemsSlots.Count; i++)
        {
            for (int x = 0; x < OwnedLootInven.Instance.wornItemsSecondAlly.Count; x++)
            {
                if (OwnedLootInven.Instance.wornItemsSecondAlly[x].linkedItemPiece == ally2ItemsSlots[i].linkedItemPiece)
                {
                    //ally2ItemsSlots[i].UpdateLinkedSlot(OwnedLootInven.Instance.wornItemsSecondAlly[x]);
                    ally2ItemsSlots[i].linkedSlot.slotIndex = i;
                }
            }
        }

        for (int i = 0; i < ally3ItemsSlots.Count; i++)
        {
            for (int x = 0; x < OwnedLootInven.Instance.wornItemsThirdAlly.Count; x++)
            {
                if (OwnedLootInven.Instance.wornItemsThirdAlly[x].linkedItemPiece == ally3ItemsSlots[i].linkedItemPiece)
                {
                    //ally3ItemsSlots[i].UpdateLinkedSlot(OwnedLootInven.Instance.wornItemsThirdAlly[x]);
                    ally3ItemsSlots[i].linkedSlot.slotIndex = i;
                }
            }
        }
    }
    */
    public void UpdateOwnedSlotsLinkedSlot()
    {
        for (int i = 0; i < OwnedLootInven.Instance.ownedLootSlots.Count; i++)
        {
            if (OwnedLootInven.Instance.ownedItems.Count > i)
                OwnedLootInven.Instance.ownedLootSlots[i].linkedSlot = OwnedLootInven.Instance.ownedItems[i];
        }
    }

    // They are both needing something from the other, but this order is right, need to make it work 
    public void UpdateEquipItemsOrder(bool ally1 = false, bool ally2 = false, bool ally3 = false)
    {
        if (GameManager.Instance.activeTeam.Count >= 1)
        {
            List<ItemPiece> newItemOrder = new List<ItemPiece>();

            for (int i = 0; i < 3; i++)
            {
                if (ally1ItemsSlots[i].linkedItemPiece)
                {
                    newItemOrder.Add(ally1ItemsSlots[i].linkedItemPiece);
                }
            }

            equippedItemsMain = newItemOrder;
        }

        if (GameManager.Instance.activeTeam.Count >= 2)
        {
            List<ItemPiece> newItemOrder = new List<ItemPiece>();

            for (int i = 0; i < 3; i++)
            {
                if (ally2ItemsSlots[i].linkedItemPiece)
                {
                    newItemOrder.Add(ally2ItemsSlots[i].linkedItemPiece);
                }
            }

            equippedItemsSecond = newItemOrder;
        }

        if (GameManager.Instance.activeTeam.Count >= 2)
        {
            List<ItemPiece> newItemOrder = new List<ItemPiece>();

            for (int i = 0; i < 3; i++)
            {
                if (ally3ItemsSlots[i].linkedItemPiece)
                {
                    newItemOrder.Add(ally3ItemsSlots[i].linkedItemPiece);
                }
            }

            equippedItemsThird = newItemOrder;
        }
        
        List<Slot> ownedItems = new List<Slot>();
        List<Slot> newOwnedItems = new List<Slot>();

        if (ally1)
        {
            ownedItems = OwnedLootInven.Instance.wornItemsMainAlly;
            if (ownedItems.Count > 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int z = 0; z < ownedItems.Count; z++)
                    {
                        if (ally1ItemsSlots[i].linkedItemPiece)
                        {
                            if (ally1ItemsSlots[i].linkedSlot == ownedItems[z])
                            {
                                newOwnedItems.Add(ownedItems[z]);
                                break;
                            }
                        }
                    }
                }

                OwnedLootInven.Instance.wornItemsMainAlly = newOwnedItems;
                ownedItems.Clear();
            }
        }
        if (ally2)
        {
            ownedItems = OwnedLootInven.Instance.wornItemsSecondAlly;
            if (ownedItems.Count > 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int z = 0; z < ownedItems.Count; z++)
                    {
                        if (ally2ItemsSlots[i].linkedItemPiece)
                        {
                            if (ally2ItemsSlots[i].linkedSlot == ownedItems[z])
                            {
                                newOwnedItems.Add(ownedItems[z]);
                                break;
                            }
                        }
                    }
                }

                OwnedLootInven.Instance.wornItemsSecondAlly = newOwnedItems;
                ownedItems.Clear();
            }
        }
        if (ally3)
        {
            ownedItems = OwnedLootInven.Instance.wornItemsThirdAlly;
            if (ownedItems.Count > 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int z = 0; z < ownedItems.Count; z++)
                    {
                        if (ally3ItemsSlots[i].linkedItemPiece)
                        {
                            if (ally3ItemsSlots[i].linkedSlot == ownedItems[z])
                            {
                                newOwnedItems.Add(ownedItems[z]);
                                break;
                            }
                        }

                    }
                }

                OwnedLootInven.Instance.wornItemsThirdAlly = newOwnedItems;
                ownedItems.Clear();
            }
        }     
    }

    public void ResetAllItemSelections()
    {
        //Debug.Log("resetting");
        for (int x = 0; x < ally1ItemsSlots.Count; x++)
        {
            ally1ItemsSlots[x].ToggleSlotSelection(false);
        }
        for (int y = 0; y < ally2ItemsSlots.Count; y++)
        {
            ally2ItemsSlots[y].ToggleSlotSelection(false);
        }
        for (int z = 0; z < ally3ItemsSlots.Count; z++)
        {
            ally3ItemsSlots[z].ToggleSlotSelection(false);
        }
    }

    public void ToggleAllSlotsClickable(bool toggle, bool doOwnedSlots = true)
    {
        for (int x = 0; x < ally1ItemsSlots.Count; x++)
        {
            if (toggle)
            {
                ally1ItemsSlots[x].GetSlotUI().ToggleButton(true);
                ally1ItemsSlots[x].ToggleMainSlot(true);
                ally1ItemsSlots[x].ToggleOwnedGearButton(true);

                if (ally1ItemsSlots[x].linkedItemPiece == null)
                {
                    ally1ItemsSlots[x].ToggleEquipButton(true);
                }
            }
            else
            {
                ally1ItemsSlots[x].GetSlotUI().ToggleButton(false);
                ally1ItemsSlots[x].ToggleMainSlot(false);
                ally1ItemsSlots[x].ToggleOwnedGearButton(false);
                ally1ItemsSlots[x].ToggleEquipButton(false);
            }
        }

        for (int y = 0; y < ally2ItemsSlots.Count; y++)
        {
            if (toggle)
            {
                ally2ItemsSlots[y].GetSlotUI().ToggleButton(true);
                ally2ItemsSlots[y].ToggleMainSlot(true);
                ally2ItemsSlots[y].ToggleOwnedGearButton(true);

                if (ally2ItemsSlots[y].linkedItemPiece == null)
                {
                    ally2ItemsSlots[y].ToggleEquipButton(true);
                }
            }
            else
            {
                ally2ItemsSlots[y].GetSlotUI().ToggleButton(false);
                ally2ItemsSlots[y].ToggleMainSlot(false);
                ally2ItemsSlots[y].ToggleOwnedGearButton(false);
                ally2ItemsSlots[y].ToggleEquipButton(false);
            }
        }

        for (int z = 0; z < ally3ItemsSlots.Count; z++)
        {
            if (toggle)
            {
                ally3ItemsSlots[z].GetSlotUI().ToggleButton(true);
                ally3ItemsSlots[z].ToggleMainSlot(true);
                ally3ItemsSlots[z].ToggleOwnedGearButton(true);

                if (ally3ItemsSlots[z].linkedItemPiece == null)
                {
                    ally3ItemsSlots[z].ToggleEquipButton(true);
                }
            }
            else
            {
                ally3ItemsSlots[z].GetSlotUI().ToggleButton(false);
                ally3ItemsSlots[z].ToggleMainSlot(false);
                ally3ItemsSlots[z].ToggleOwnedGearButton(false);
                ally3ItemsSlots[z].ToggleEquipButton(false);
            }
        }
        
        if (doOwnedSlots)
        {
            int count = OwnedLootInven.Instance.ownedLootSlots.Count;

            Debug.Log("Doing owned slots " + toggle);
            for (int i = 0; i < count; i++)
            {
                if (toggle)
                {
                    OwnedLootInven.Instance.ownedLootSlots[i].GetSlotUI().ToggleButton(true);
                    OwnedLootInven.Instance.ownedLootSlots[i].ToggleMainSlot(true);
                    //OwnedLootInven.Instance.ownedLootSlots[i].ToggleOwnedItemsButton(true);
                    if (OwnedLootInven.Instance.ownedLootSlots[i].isEmpty)
                        OwnedLootInven.Instance.ownedLootSlots[i].ToggleEquipButton(false);
                    else
                        OwnedLootInven.Instance.ownedLootSlots[i].ToggleEquipButton(true);
                }
                else
                {
                    OwnedLootInven.Instance.ownedLootSlots[i].GetSlotUI().ToggleButton(false);
                    OwnedLootInven.Instance.ownedLootSlots[i].ToggleMainSlot(false);
                    //OwnedLootInven.Instance.ownedLootSlots[i].ToggleOwnedItemsButton(false);
                    OwnedLootInven.Instance.ownedLootSlots[i].ToggleEquipButton(false);
                }
            }
        }
    }

    public void EquipItem(Slot item)
    {
        OwnedLootInven.Instance.ToggleOwnedGearDisplay(false);

        Slot removedItem = null;

        // Remove current equipt item, and place into owned gear
        
        if (selectedBaseItemSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.MAIN)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornItemMainAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornItemMainAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornItemMainAlly()[x].GetSlotName() == item.GetSlotName())
                    {
                        break;
                    }
                }
            }
        }
        else if (selectedBaseItemSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.SECOND)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornItemSecondAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornItemSecondAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornItemSecondAlly()[x].GetSlotName() == item.GetSlotName())
                    {
                        break;
                    }
                }
            }
        }
        else if (selectedBaseItemSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.THIRD)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornItemThirdAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornItemThirdAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornItemThirdAlly()[x].GetSlotName() == item.GetSlotName())
                    {
                        break;
                    }
                }
            }
        }
        

        // Remove gear from owned gear list when equipping
        // Add gear to worn gear
        for (int i = 0; i < OwnedLootInven.Instance.ownedItems.Count; i++)
        {
            if (OwnedLootInven.Instance.ownedItems[i] == item)
            {
                GetSelectedBaseItemSlot().UpdateLinkedSlot(OwnedLootInven.Instance.ownedItems[i]);

                removedItem = OwnedLootInven.Instance.ownedItems[i];

                //Debug.Log("doing thing");
                OwnedLootInven.Instance.RemoveOwnedItem(removedItem);

                if (selectedBaseItemSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.MAIN)
                {
                    OwnedLootInven.Instance.AddWornItemAllyMain(removedItem);
                    break;
                }
                else if (selectedBaseItemSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.SECOND)
                {
                    OwnedLootInven.Instance.AddWornItemAllySecond(removedItem);
                    break;
                }
                else if (selectedBaseItemSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.THIRD)
                {
                    OwnedLootInven.Instance.AddWornItemAllyThird(removedItem);
                    break;
                }
            }
        }

        GetSelectedBaseItemSlot().UpdateSlotImage(item.GetSlotImage());
        GetSelectedBaseItemSlot().UpdateSlotName(item.GetSlotName());
        GetSelectedBaseItemSlot().linkedItemPiece = item.linkedItemPiece;

        if (GetSelectedBaseItemSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.MAIN)
            UpdateEquipItemsOrder(true, false, false);
        else if (GetSelectedBaseItemSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.SECOND)
            UpdateEquipItemsOrder(false, true, false);
        else if (GetSelectedBaseItemSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.THIRD)
            UpdateEquipItemsOrder(false, false, true);

        GetSelectedBaseItemSlot().UpdateSlotDetails();

        UpdateItemNameText(item.linkedItemPiece.itemName);
    }

    public void ResetHeroItemOwned(int heroIndex)
    {
        if (heroIndex == 0)
        {
            equippedItemsMain.Clear();

            OwnedLootInven.Instance.ResetWornItemsAllyMain();

            //ally1MenuUnitDisplay.ResetUnitStats();

            for (int i = 0; i < 3; i++)
            {
                ally1ItemsSlots[i].isEmpty = true;
                ally1ItemsSlots[i].ResetSlot(true, true);
            }

            ClearAllGearStats();
            UpdateItemNameText("");
        }
        else if (heroIndex == 1)
        {
            equippedItemsSecond.Clear();

            OwnedLootInven.Instance.ResetWornItemsAllySecond();

            //ally2MenuUnitDisplay.ResetUnitStats();

            for (int i = 0; i < 3; i++)
            {
                ally2ItemsSlots[i].isEmpty = true;
                ally2ItemsSlots[i].ResetSlot(true, true);
            }

            ClearAllGearStats();
            UpdateItemNameText("");
        }
        else if (heroIndex == 2)
        {
            equippedItemsThird.Clear();

            OwnedLootInven.Instance.ResetWornItemsAllyThird();

            //ally1MenuUnitDisplay.ResetUnitStats();

            for (int i = 0; i < 3; i++)
            {
                ally3ItemsSlots[i].isEmpty = true;
                ally3ItemsSlots[i].ResetSlot(true, true);
            }

            ClearAllGearStats();
            UpdateItemNameText("");
        }
    }

    public void ResetItemOwned()
    {
        /*
        equippedItemsMain = null;
        equippedItemsSecond = null;
        equippedItemsThird = null;
        */

        equippedItemsMain.Clear();
        equippedItemsSecond.Clear();
        equippedItemsThird.Clear();

        OwnedLootInven.Instance.ResetOwnedItems();

        OwnedLootInven.Instance.ResetWornItemsAllyMain();
        OwnedLootInven.Instance.ResetWornItemsAllySecond();
        OwnedLootInven.Instance.ResetWornItemsAllyThird();

        ally1MenuUnitDisplay.ResetUnitStats();
        ally2MenuUnitDisplay.ResetUnitStats();
        ally3MenuUnitDisplay.ResetUnitStats();

        for (int i = 0; i < 3; i++)
        {
            ally1ItemsSlots[i].isEmpty = true;
            ally2ItemsSlots[i].isEmpty = true;
            ally3ItemsSlots[i].isEmpty = true;
            ally1ItemsSlots[i].ResetSlot(true, true);
            ally2ItemsSlots[i].ResetSlot(true, true);
            ally3ItemsSlots[i].ResetSlot(true, true);
        }
        ClearAllGearStats();
        UpdateItemNameText("");
    }

    public void UpdateEquippedItemPiece(string gearPieceTypeName, ItemPiece newItemPiece, bool replacing = true)
    {
        if (gearPieceTypeName == "ItemMain")
        {
            if (replacing)
                equippedItemsMain.Add(newItemPiece);
            else
            {
                for (int i = 0; i < equippedItemsMain.Count; i++)
                {
                    if (equippedItemsMain[i].itemName == newItemPiece.itemName)
                    {
                        equippedItemsMain.Remove(equippedItemsMain[i]);
                        break;
                    }
                }
            }
        }
        else if (gearPieceTypeName == "ItemSecond")
        {
            if (replacing)
                equippedItemsSecond.Add(newItemPiece);
            else
            {
                for (int i = 0; i < equippedItemsSecond.Count; i++)
                {
                    if (equippedItemsSecond[i].itemName == newItemPiece.itemName)
                    {
                        equippedItemsSecond.Remove(equippedItemsSecond[i]);
                        break;
                    }
                }
            }
        }
        else if (gearPieceTypeName == "ItemThird")
        {
            if (replacing)
                equippedItemsThird.Add(newItemPiece);
            else
            {
                for (int i = 0; i < equippedItemsThird.Count; i++)
                {
                    if (equippedItemsThird[i].itemName == newItemPiece.itemName)
                    {
                        equippedItemsThird.Remove(equippedItemsThird[i]);
                        break;
                    }
                }
            }
        }
    }

    public void UpdateFighterRaceIcon(string fighterRace)
    {
        if (fighterRace == "HUMAN")
        {
            fighterRaceIcon.UpdateContentImage(GameManager.Instance.humanRaceIcon);
            fighterRaceIcon.tooltipStats.UpdateTooltipStatsText("HUMAN");
        }           
        else if (fighterRace == "BEAST")
        {
            fighterRaceIcon.UpdateContentImage(GameManager.Instance.beastRaceIcon);
            fighterRaceIcon.tooltipStats.UpdateTooltipStatsText("BEAST");
        }          
        else if (fighterRace == "ETHEREAL")
        {
            fighterRaceIcon.UpdateContentImage(GameManager.Instance.etherealRaceIcon);
            fighterRaceIcon.tooltipStats.UpdateTooltipStatsText("ETHEREAL");
        }       
    }

    public void ToggleFighterRaceIcon(bool toggle = true)
    {
        if (toggle)
        {
            fighterRaceIcon.UpdateAlpha(1);
        }
        else
        {
            fighterRaceIcon.UpdateAlpha(0);
        }
    }

    public void ItemSelection(Slot item, bool select = false)
    {
        if (item.linkedItemPiece != null)
        {
            ToggleFighterRaceIcon(true);

            if (item.linkedItemPiece.curRace == ItemPiece.RaceSpecific.HUMAN)
                UpdateFighterRaceIcon("HUMAN");
            else if (item.linkedItemPiece.curRace == ItemPiece.RaceSpecific.BEAST)
                UpdateFighterRaceIcon("BEAST");
            else if (item.linkedItemPiece.curRace == ItemPiece.RaceSpecific.ETHEREAL)
                UpdateFighterRaceIcon("ETHEREAL");   
            else if (item.linkedItemPiece.curRace == ItemPiece.RaceSpecific.ALL)
                ToggleFighterRaceIcon(false);

            if (playerInItemTab && OwnedLootInven.Instance.ownedLootOpened)
            {
                if (item.curSlotStatis == Slot.SlotStatis.OWNED)
                {
                    if (GetSelectedBaseItemSlot().curGearOwnedBy == Slot.SlotOwnedBy.MAIN)
                    {
                        for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
                        {
                            if (GameManager.Instance.activeRoomHeroes[i].teamIndex == 0)
                            {
                                if (item.linkedItemPiece.curRace == ItemPiece.RaceSpecific.HUMAN)
                                {
                                    if (GameManager.Instance.activeRoomHeroes[i].curUnitRace != UnitFunctionality.UnitRace.HUMAN)
                                    {
                                        item.ToggleCoverUI(true);
                                    }
                                    else
                                    {
                                        item.ToggleCoverUI(false);
                                    }
                                }
                                else if (item.linkedItemPiece.curRace == ItemPiece.RaceSpecific.BEAST)
                                {
                                    if (GameManager.Instance.activeRoomHeroes[i].curUnitRace != UnitFunctionality.UnitRace.BEAST)
                                    {
                                        item.ToggleCoverUI(true);
                                    }
                                    else
                                    {
                                        item.ToggleCoverUI(false);
                                    }
                                }
                                else if (item.linkedItemPiece.curRace == ItemPiece.RaceSpecific.ETHEREAL)
                                {
                                    if (GameManager.Instance.activeRoomHeroes[i].curUnitRace != UnitFunctionality.UnitRace.ETHEREAL)
                                    {
                                        item.ToggleCoverUI(true);
                                    }
                                    else
                                    {
                                        item.ToggleCoverUI(false);
                                    }
                                }
                                else if (item.linkedItemPiece.curRace == ItemPiece.RaceSpecific.ALL)
                                {
                                    item.ToggleCoverUI(false);
                                }
                            }
                        }
                    }
                    else if (GetSelectedBaseItemSlot().curGearOwnedBy == Slot.SlotOwnedBy.SECOND)
                    {
                        for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
                        {
                            if (GameManager.Instance.activeRoomHeroes[i].teamIndex == 1)
                            {
                                if (item.linkedItemPiece.curRace == ItemPiece.RaceSpecific.HUMAN)
                                {
                                    if (GameManager.Instance.activeRoomHeroes[i].curUnitRace != UnitFunctionality.UnitRace.HUMAN)
                                    {
                                        item.ToggleCoverUI(true);
                                    }
                                    else
                                    {
                                        item.ToggleCoverUI(false);
                                    }
                                }
                                else if (item.linkedItemPiece.curRace == ItemPiece.RaceSpecific.BEAST)
                                {
                                    if (GameManager.Instance.activeRoomHeroes[i].curUnitRace != UnitFunctionality.UnitRace.BEAST)
                                    {
                                        item.ToggleCoverUI(true);
                                    }
                                    else
                                    {
                                        item.ToggleCoverUI(false);
                                    }
                                }
                                else if (item.linkedItemPiece.curRace == ItemPiece.RaceSpecific.ETHEREAL)
                                {
                                    if (GameManager.Instance.activeRoomHeroes[i].curUnitRace != UnitFunctionality.UnitRace.ETHEREAL)
                                    {
                                        item.ToggleCoverUI(true);
                                    }
                                    else
                                    {
                                        item.ToggleCoverUI(false);
                                    }
                                }
                                else if (item.linkedItemPiece.curRace == ItemPiece.RaceSpecific.ALL)
                                {
                                    item.ToggleCoverUI(false);
                                }
                            }
                        }
                    }
                    else if (GetSelectedBaseItemSlot().curGearOwnedBy == Slot.SlotOwnedBy.THIRD)
                    {
                        for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
                        {
                            if (GameManager.Instance.activeRoomHeroes[i].teamIndex == 2)
                            {
                                if (item.linkedItemPiece.curRace == ItemPiece.RaceSpecific.HUMAN)
                                {
                                    if (GameManager.Instance.activeRoomHeroes[i].curUnitRace != UnitFunctionality.UnitRace.HUMAN)
                                    {
                                        item.ToggleCoverUI(true);
                                    }
                                    else
                                    {
                                        item.ToggleCoverUI(false);
                                    }
                                }
                                else if (item.linkedItemPiece.curRace == ItemPiece.RaceSpecific.BEAST)
                                {
                                    if (GameManager.Instance.activeRoomHeroes[i].curUnitRace != UnitFunctionality.UnitRace.BEAST)
                                    {
                                        item.ToggleCoverUI(true);
                                    }
                                    else
                                    {
                                        item.ToggleCoverUI(false);
                                    }
                                }
                                else if (item.linkedItemPiece.curRace == ItemPiece.RaceSpecific.ETHEREAL)
                                {
                                    if (GameManager.Instance.activeRoomHeroes[i].curUnitRace != UnitFunctionality.UnitRace.ETHEREAL)
                                    {
                                        item.ToggleCoverUI(true);
                                    }
                                    else
                                    {
                                        item.ToggleCoverUI(false);
                                    }
                                }
                                else if (item.linkedItemPiece.curRace == ItemPiece.RaceSpecific.ALL)
                                {
                                    item.ToggleCoverUI(false);
                                }
                            }
                        }
                    }
                }
            }
            else if (TeamGearManager.Instance.playerInGearTab)
            {
                item.ToggleCoverUI(false);
            }
        }
        else
        {
            ToggleFighterRaceIcon(false);
        }

        // Disable all gear selection border
        ResetAllItemSelections();

        // Enable selected gear slot border
        item.ToggleSlotSelection(true);

        if (item.curSlotStatis == Slot.SlotStatis.DEFAULT)
        {
            UpdateSelectedBaseItemSlot(item);

            UpdateSelectedItemSlot(item);

            OwnedLootInven.Instance.EnableOwnedItemsSlotSelection(GetSelectedItemSlot());

            // Toggle main gear selection on
            GetSelectedBaseItemSlot().ToggleSlotSelection(true);
            GetSelectedItemSlot().ToggleSlotSelection(true);
        }
        else
        {
            UpdateSelectedItemSlot(item);
            OwnedLootInven.Instance.EnableOwnedItemsSlotSelection(GetSelectedItemSlot());
            SkillsTabManager.Instance.UpdateSelectedOwnedSlot(item);

            GetSelectedItemSlot().ToggleSlotSelection(true);

            if (OwnedLootInven.Instance.ownedLootOpened)
            {
                if (!select && OwnedLootInven.Instance.ownedLootSlots[OwnedLootInven.Instance.ownedLootSlots.IndexOf(item)] != null)
                {
                    OwnedLootInven.Instance.ResetOwnedSlotEquipButton();
                    OwnedLootInven.Instance.ownedLootSlots[OwnedLootInven.Instance.ownedLootSlots.IndexOf(item)].ToggleEquipButton(true);
                }
            }

            if (select)
            {
                ItemPiece itemPiece = new ItemPiece();
                itemPiece = item.linkedItemPiece;

                itemPiece.UpdateItemPiece(item.GetSlotName(), item.GetRarity().ToString(), item.GetSlotImage());
                //Debug.Log("111");
                if (GetSelectedBaseItemSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.MAIN)
                {
                    //Debug.Log("aaa");
                    itemPiece.UpdateItemPiece(item.GetSlotName(), item.GetRarity().ToString(), item.GetSlotImage());
                    UpdateEquippedItemPiece("ItemMain", itemPiece);
                }
                if (GetSelectedBaseItemSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.SECOND)
                {
                    //Debug.Log("bbb");
                    itemPiece.UpdateItemPiece(item.GetSlotName(), item.GetRarity().ToString(), item.GetSlotImage());
                    UpdateEquippedItemPiece("ItemSecond", itemPiece);
                }
                if (GetSelectedBaseItemSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.THIRD)
                {
                    itemPiece.UpdateItemPiece(item.GetSlotName(), item.GetRarity().ToString(), item.GetSlotImage());
                    UpdateEquippedItemPiece("ItemThird", itemPiece);
                }

                // Display inven
                EquipItem(item.linkedSlot);
            }
        }

        // If gear is NOT empty, put gear in it
        if (!item.isEmpty)
        {
            // Update UI
            UpdateItemStatDetails();
            UpdateItemNameText(item.GetSlotName());

        }

        // If gear IS empty, dont put gear in it, display it as empty
        else
        {
            UpdateItemNameText("");
            UpdateItemDesc("");
            ToggleItemRarityText(false);
        }
    }

    public void UnequipItem()
    {
        if (GetSelectedBaseItemSlot() == null)
            return;

        if (GetSelectedBaseItemSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.MAIN)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornItemMainAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornItemMainAlly().Count; x++)
                {
                    //Debug.Log("slot name " + GetSelectedBaseItemSlot().GetSlotName());

                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornItemMainAlly()[x] == GetSelectedBaseItemSlot().linkedSlot)
                    {
                        // Remove saved equipped gear piece (data side)
                        UpdateEquippedItemPiece("ItemMain", OwnedLootInven.Instance.GetWornItemMainAlly()[x].linkedItemPiece, false);

                        //Debug.Log("Linked item piece = " + OwnedLootInven.Instance.GetWornItemMainAlly()[x].linkedItemPiece.itemName);

                        // Update unit stats when unequiping
                        UpdateUnitStatsUnEquip(OwnedLootInven.Instance.GetWornItemMainAlly()[x]);

                        // Add gear into owned gear ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                        //OwnedLootInven.Instance.AddOwnedItems(OwnedLootInven.Instance.GetWornItemMainAlly()[x]);
                        OwnedLootInven.Instance.AddOwnedItems(GetSelectedBaseItemSlot().linkedSlot);  

                        // Remove worn gear
                        OwnedLootInven.Instance.RemoveWornItemAllyMain(OwnedLootInven.Instance.GetWornItemMainAlly()[x]);

                        GetSelectedBaseItemSlot().ResetSlot(true, true);
                        break;
                    }
                }


            }
        }
        else if (GetSelectedBaseItemSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.SECOND)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornItemSecondAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornItemSecondAlly().Count; x++)
                {
                    //Debug.Log("slot name " + GetSelectedBaseItemSlot().GetSlotName());

                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornItemSecondAlly()[x] == GetSelectedBaseItemSlot().linkedSlot)
                    {
                        // Remove saved equipped gear piece (data side)
                        UpdateEquippedItemPiece("ItemSecond", OwnedLootInven.Instance.GetWornItemSecondAlly()[x].linkedItemPiece, false);

                        //Debug.Log("Linked item piece = " + OwnedLootInven.Instance.GetWornItemSecondAlly()[x].linkedItemPiece.itemName);

                        // Update unit stats when unequiping
                        UpdateUnitStatsUnEquip(OwnedLootInven.Instance.GetWornItemSecondAlly()[x]);

                        // Add gear into owned gear
                        OwnedLootInven.Instance.AddOwnedItems(GetSelectedBaseItemSlot().linkedSlot);

                        // Remove worn gear
                        OwnedLootInven.Instance.RemoveWornItemAllySecond(OwnedLootInven.Instance.GetWornItemSecondAlly()[x]);

                        GetSelectedBaseItemSlot().ResetSlot(true, true);
                        break;
                    }
                }


            }
        }
        else if (GetSelectedBaseItemSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.THIRD)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornItemThirdAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornItemThirdAlly().Count; x++)
                {
                    //Debug.Log("slot name " + GetSelectedBaseItemSlot().GetSlotName());

                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornItemThirdAlly()[x] == GetSelectedBaseItemSlot().linkedSlot)
                    {
                        // Remove saved equipped gear piece (data side)
                        UpdateEquippedItemPiece("ItemThird", OwnedLootInven.Instance.GetWornItemThirdAlly()[x].linkedItemPiece, false);

                        //Debug.Log("Linked item piece = " + OwnedLootInven.Instance.GetWornItemThirdAlly()[x].linkedItemPiece.itemName);
                        // Update unit stats when unequiping
                        UpdateUnitStatsUnEquip(OwnedLootInven.Instance.GetWornItemThirdAlly()[x]);

                        // Add gear into owned gear
                        OwnedLootInven.Instance.AddOwnedItems(GetSelectedBaseItemSlot().linkedSlot);

                        // Remove worn gear
                        OwnedLootInven.Instance.RemoveWornItemAllyThird(OwnedLootInven.Instance.GetWornItemThirdAlly()[x]);

                        GetSelectedBaseItemSlot().ResetSlot(true, true);
                        break;
                    }
                }


            }
        }


        GetSelectedBaseItemSlot().isEmpty = true;
        // Remove gear icon display
        GetSelectedBaseItemSlot().ResetSlot(true);

        // Remove gear icon details (name / stats
        UpdateItemDesc("");
        UpdateItemNameText("");
        ClearAllGearStats();
        ToggleItemRarityText(false);
    }

    public void UpdateSlotsBaseDefault(Slot slot = null, Item item = null, bool ally1 = false, bool ally2 = false, bool ally3 = false)
    {
        //Debug.Log("fixing thing");
        string removedPieceType = "";
        if (slot != null)
        {
            removedPieceType = slot.GetCurGearType().ToString();
        }
        if (item != null)
        {

        }

        if (ally1)
        {
            for (int i = 0; i < ally1ItemsSlots.Count; i++)
            {
                ally1ItemsSlots[i].UpdateGearStatis(Slot.SlotStatis.DEFAULT);
                ally1ItemsSlots[i].UpdateGearOwnedBy(Slot.SlotOwnedBy.MAIN);

                if (ally1ItemsSlots[i].isEmpty)
                {
                    ally1ItemsSlots[i].UpdateSlotImage(TeamGearManager.Instance.clearSlotSprite);
                    ally1ItemsSlots[i].UpdateLinkedItemPiece(null);
                    ally1ItemsSlots[i].UpdateSlotDetails();
                    ally1ItemsSlots[i].ToggleEquipButton(true);
                }
            }
        }

        if (ally2)
        {
            for (int x = 0; x < ally2ItemsSlots.Count; x++)
            {
                ally2ItemsSlots[x].UpdateGearStatis(Slot.SlotStatis.DEFAULT);
                ally2ItemsSlots[x].UpdateGearOwnedBy(Slot.SlotOwnedBy.SECOND);

                if (ally2ItemsSlots[x].isEmpty)
                {
                    ally2ItemsSlots[x].UpdateSlotImage(TeamGearManager.Instance.clearSlotSprite);
                    ally2ItemsSlots[x].UpdateLinkedItemPiece(null);
                    ally2ItemsSlots[x].UpdateSlotDetails();
                    ally2ItemsSlots[x].ToggleEquipButton(true);
                }
            }
        }

        if (ally3)
        {
            for (int y = 0; y < ally3ItemsSlots.Count; y++)
            {
                ally3ItemsSlots[y].UpdateGearStatis(Slot.SlotStatis.DEFAULT);
                ally3ItemsSlots[y].UpdateGearOwnedBy(Slot.SlotOwnedBy.THIRD);

                if (ally3ItemsSlots[y].isEmpty)
                {
                    ally3ItemsSlots[y].UpdateSlotImage(TeamGearManager.Instance.clearSlotSprite);
                    ally3ItemsSlots[y].UpdateLinkedItemPiece(null);
                    ally3ItemsSlots[y].UpdateSlotDetails();
                    ally3ItemsSlots[y].ToggleEquipButton(true);
                }
            }
        }
    }

    public void SellGear()
    {

    }
}
