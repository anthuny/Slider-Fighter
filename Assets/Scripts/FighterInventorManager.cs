using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterInventorManager : MonoBehaviour
{
    public static FighterInventorManager Instance;

    [SerializeField] private Transform fighterInventoryParent;
    [SerializeField] private GameObject fighterInventoryGO;
    [SerializeField] private GameObject inventorySlotGO;

    [SerializeField] private UIElement inventoryGearButton;
    [SerializeField] private UIElement inventoryItemsButton;

    [SerializeField] private List<FighterInventory> fighterInventorys = new List<FighterInventory>();

    [SerializeField] private FighterInventory unequippedLootInventory;
    [SerializeField] private UIElement selectedInventorySlot;


    public bool inventoryModeGear = true;
    public bool unequippedLootInventoryOpened = false;

    public void ResetInvenSlotBG()
    {
        for (int i = 0; i < fighterInventorys.Count; i++)
        {
            fighterInventorys[i].ResetInvenSlotItemBG();
        }
    }

    public UIElement GetSelectedInventorySlot()
    {
        return selectedInventorySlot;
    }

    public void UpdateSelectedInventorySlot(UIElement uielement)
    {
        selectedInventorySlot = uielement;
    }

    public void ResetSelectedInventorySlot()
    {
        selectedInventorySlot = null;
    }

    public void DisableAllBagIconSelections()
    {
        for (int x = 0; x < unequippedLootInventory.unequippedInvenSlots.Count; x++)
        {
            if (unequippedLootInventory.unequippedInvenSlots[x])
            {
                unequippedLootInventory.unequippedInvenSlots[x].contentImage2UI.UpdateAlpha(0);
            }
        }
    }

    public void UpdateEquippedInvenSlots()
    {
        for (int i = 0; i < fighterInventorys.Count; i++)
        {
            fighterInventorys[i].item1Slot.UpdateInvenSlot();
            fighterInventorys[i].item2Slot.UpdateInvenSlot();
            fighterInventorys[i].item3Slot.UpdateInvenSlot();
        }
    }

    public void ToggleEquippedLootInventorySlots(bool toggle = true)
    {
        for (int i = 0; i < fighterInventorys.Count; i++)
        {
            if (toggle)
            {
                fighterInventorys[i].helmetSlot.UpdateAlpha(1);
                fighterInventorys[i].helmetSlot.ToggleButton(true);
                fighterInventorys[i].chestpieceSlot.UpdateAlpha(1);
                fighterInventorys[i].chestpieceSlot.ToggleButton(true);
                fighterInventorys[i].bootsSlot.UpdateAlpha(1);
                fighterInventorys[i].bootsSlot.ToggleButton(true);
                fighterInventorys[i].pendantSlot.UpdateAlpha(1);
                fighterInventorys[i].pendantSlot.ToggleButton(true);
                fighterInventorys[i].earringSlot.UpdateAlpha(1);
                fighterInventorys[i].earringSlot.ToggleButton(true);
                fighterInventorys[i].beltSlot.UpdateAlpha(1);
                fighterInventorys[i].beltSlot.ToggleButton(true);
                fighterInventorys[i].gloveSlot.UpdateAlpha(1);
                fighterInventorys[i].gloveSlot.ToggleButton(true);
                fighterInventorys[i].ring1Slot.UpdateAlpha(1);
                fighterInventorys[i].ring1Slot.ToggleButton(true);
                fighterInventorys[i].ring2Slot.UpdateAlpha(1);
                fighterInventorys[i].ring2Slot.ToggleButton(true);
                fighterInventorys[i].item1Slot.UpdateAlpha(1);
                fighterInventorys[i].item1Slot.ToggleButton(true);
                fighterInventorys[i].item2Slot.UpdateAlpha(1);
                fighterInventorys[i].item2Slot.ToggleButton(true);
                fighterInventorys[i].item3Slot.UpdateAlpha(1);
                fighterInventorys[i].item3Slot.ToggleButton(true);
            }
            else
            {
                fighterInventorys[i].helmetSlot.UpdateAlpha(0);
                fighterInventorys[i].helmetSlot.ToggleButton(false);
                fighterInventorys[i].chestpieceSlot.UpdateAlpha(0);
                fighterInventorys[i].chestpieceSlot.ToggleButton(false);
                fighterInventorys[i].bootsSlot.UpdateAlpha(0);
                fighterInventorys[i].bootsSlot.ToggleButton(false);
                fighterInventorys[i].pendantSlot.UpdateAlpha(0);
                fighterInventorys[i].pendantSlot.ToggleButton(false);
                fighterInventorys[i].earringSlot.UpdateAlpha(0);
                fighterInventorys[i].earringSlot.ToggleButton(false);
                fighterInventorys[i].beltSlot.UpdateAlpha(0);
                fighterInventorys[i].beltSlot.ToggleButton(false);
                fighterInventorys[i].gloveSlot.UpdateAlpha(0);
                fighterInventorys[i].gloveSlot.ToggleButton(false);
                fighterInventorys[i].ring1Slot.UpdateAlpha(0);
                fighterInventorys[i].ring1Slot.ToggleButton(false);
                fighterInventorys[i].ring2Slot.UpdateAlpha(0);
                fighterInventorys[i].ring2Slot.ToggleButton(false);
                fighterInventorys[i].item1Slot.UpdateAlpha(0);
                fighterInventorys[i].item1Slot.ToggleButton(false);
                fighterInventorys[i].item2Slot.UpdateAlpha(0);
                fighterInventorys[i].item2Slot.ToggleButton(false);
                fighterInventorys[i].item3Slot.UpdateAlpha(0);
                fighterInventorys[i].item3Slot.ToggleButton(false);
            }
        }
    }

    public void ToggleUnequippedLootInventory(bool toggle = true, bool byPass = false)
    {
        unequippedLootInventory.ToggleUIElement(toggle);


        for (int x = 0; x < unequippedLootInventory.unequippedInvenSlots.Count; x++)
        {
            if (unequippedLootInventory.unequippedInvenSlots[x])
            {
                unequippedLootInventory.unequippedInvenSlots[x].UpdateContentImage(TeamGearManager.Instance.clearSlotSprite);
                unequippedLootInventory.unequippedInvenSlots[x].UpdateRarityBG(false);
                unequippedLootInventory.unequippedInvenSlots[x].linkedGearPiece = null;
                unequippedLootInventory.unequippedInvenSlots[x].linkedItemPiece = null;
                unequippedLootInventory.unequippedInvenSlots[x].isEmpty = true;
                unequippedLootInventory.unequippedInvenSlots[x].ToggleButton(true);
            }
        }

        if (toggle == true)
        {
            // Place gear
            for (int i = 0; i < OwnedLootInven.Instance.ownedGear.Count; i++)
            {
                if (i >= OwnedLootInven.Instance.ownedGear.Count)
                    break;

                unequippedLootInventory.unequippedInvenSlots[i].UpdateInvenSlot(OwnedLootInven.Instance.ownedGear[i].linkedGearPiece);
                if (ShopManager.Instance.GetSelectedShopItem())
                {
                    if (ShopManager.Instance.GetSelectedShopItem().linkedGearPiece)
                    {
                        if (byPass && i == OwnedLootInven.Instance.ownedGear.Count - 1 && ShopManager.Instance.GetSelectedShopItem().linkedGearPiece.gearName == OwnedLootInven.Instance.ownedGear[i].linkedGearPiece.gearName)
                            unequippedLootInventory.unequippedInvenSlots[i].AnimateUI(false);
                    }
                }
            }

            int count = 0;

            // Place Items
            for (int i = 0; i < unequippedLootInventory.unequippedInvenSlots.Count; i++)
            {
                if (unequippedLootInventory.unequippedInvenSlots[i].isEmpty)
                {
                    if (count >= OwnedLootInven.Instance.ownedItems.Count)
                        break;

                    unequippedLootInventory.unequippedInvenSlots[i].UpdateInvenSlot(null, OwnedLootInven.Instance.ownedItems[count].linkedItemPiece);
                    if (ShopManager.Instance.GetSelectedShopItem())
                    {
                        if (ShopManager.Instance.GetSelectedShopItem().linkedItemPiece)
                        {
                            if (byPass && count == OwnedLootInven.Instance.ownedItems.Count - 1 && ShopManager.Instance.GetSelectedShopItem().linkedItemPiece.itemName == OwnedLootInven.Instance.ownedItems[count].linkedItemPiece.itemName)
                                unequippedLootInventory.unequippedInvenSlots[i].AnimateUI(false);
                        }
                    }


                    count++;
                }
            }
        }
        else
        {
            DisableAllBagIconSelections();
            for (int x = 0; x < unequippedLootInventory.unequippedInvenSlots.Count; x++)
            {
                if (unequippedLootInventory.unequippedInvenSlots[x])
                {
                    unequippedLootInventory.unequippedInvenSlots[x].ToggleButton(false);
                }
            }

            for (int i = 0; i < fighterInventorys.Count; i++)
            {
                fighterInventorys[i].helmetSlot.ToggleButton(false);
                fighterInventorys[i].chestpieceSlot.ToggleButton(false);
                fighterInventorys[i].bootsSlot.ToggleButton(false);
                fighterInventorys[i].pendantSlot.ToggleButton(false);
                fighterInventorys[i].earringSlot.ToggleButton(false);
                fighterInventorys[i].beltSlot.ToggleButton(false);
                fighterInventorys[i].gloveSlot.ToggleButton(false);
                fighterInventorys[i].ring1Slot.ToggleButton(false);
                fighterInventorys[i].ring2Slot.ToggleButton(false);
                fighterInventorys[i].item1Slot.ToggleButton(false);
                fighterInventorys[i].item2Slot.ToggleButton(false);
                fighterInventorys[i].item3Slot.ToggleButton(false);
            }

            for (int x = 0; x < unequippedLootInventory.unequippedInvenSlots.Count; x++)
            {
                if (unequippedLootInventory.unequippedInvenSlots[x])
                {
                    unequippedLootInventory.unequippedInvenSlots[x].ToggleButton(false);
                }
            }
        }
    }

    public void ToggleInventoryGearButton(bool toggle = true, bool makePartVisible = false)
    {
        if (toggle)
        {
            inventoryGearButton.UpdateAlpha(1);
            inventoryGearButton.ToggleButton(true);
        }

        else
        {
            inventoryGearButton.UpdateAlpha(0);
            inventoryGearButton.ToggleButton(false);
        }

        if (makePartVisible)
        {
            inventoryGearButton.UpdateAlpha(.35f);
        }
    }

    public void ToggleInventoryItemsButton(bool toggle = true, bool makePartVisible = false)
    {
        if (toggle)
        {
            inventoryItemsButton.UpdateAlpha(1);
            inventoryItemsButton.ToggleButton(true);
        }

        else
        {
            inventoryItemsButton.UpdateAlpha(0);
            inventoryItemsButton.ToggleButton(false);
        }

        if (makePartVisible)
        {
            inventoryItemsButton.UpdateAlpha(.35f);
        }
    }

    public void ToggleInventoryMode(bool forceGear = false, bool forceItems = false, bool byPassPopop = false)
    {
        inventoryModeGear = !inventoryModeGear;

        DisableAllBagIconSelections();

        if (forceGear)
            inventoryModeGear = true;

        if (forceItems)
            inventoryModeGear = false;

        UpdateFightersInventory(byPassPopop);

        if (inventoryModeGear)
        {
            ToggleInventoryItemsButton(true);
            ToggleInventoryGearButton(false, true);
            ToggleUnequippedLootInventory(false);
            UpdateFighterInventorySelection();

            // Toggle items off
            for (int i = 0; i < fighterInventorys.Count; i++)
            {
                fighterInventorys[i].item1Slot.ToggleButton(false);
                fighterInventorys[i].item2Slot.ToggleButton(false);
                fighterInventorys[i].item3Slot.ToggleButton(false);

                fighterInventorys[i].helmetSlot.ToggleButton(true);
                fighterInventorys[i].chestpieceSlot.ToggleButton(true);
                fighterInventorys[i].bootsSlot.ToggleButton(true);
                fighterInventorys[i].pendantSlot.ToggleButton(true);
                fighterInventorys[i].earringSlot.ToggleButton(true);
                fighterInventorys[i].beltSlot.ToggleButton(true);
                fighterInventorys[i].gloveSlot.ToggleButton(true);
                fighterInventorys[i].ring1Slot.ToggleButton(true);
                fighterInventorys[i].ring2Slot.ToggleButton(true);
            }
        }
        else
        {
            ResetFighterInventorySelections();

            ToggleInventoryItemsButton(false, true);
            ToggleInventoryGearButton(true);
            ToggleUnequippedLootInventory(false);

            // toggle gear off
            for (int i = 0; i < fighterInventorys.Count; i++)
            {
                fighterInventorys[i].helmetSlot.ToggleButton(false);
                fighterInventorys[i].chestpieceSlot.ToggleButton(false);
                fighterInventorys[i].bootsSlot.ToggleButton(false);
                fighterInventorys[i].pendantSlot.ToggleButton(false);
                fighterInventorys[i].earringSlot.ToggleButton(false);
                fighterInventorys[i].beltSlot.ToggleButton(false);
                fighterInventorys[i].gloveSlot.ToggleButton(false);
                fighterInventorys[i].ring1Slot.ToggleButton(false);
                fighterInventorys[i].ring2Slot.ToggleButton(false);

                fighterInventorys[i].item1Slot.ToggleButton(true);
                fighterInventorys[i].item2Slot.ToggleButton(true);
                fighterInventorys[i].item3Slot.ToggleButton(true);
            }
        }


        //ToggleEquippedLootInventorySlots(true);
    }


    public GameObject GetInventorySlotGO()
    {
        return inventorySlotGO;
    }

    private void Start()
    {
        Instance = this;

        Setup();
    }

    public void Setup()
    {
        ToggleInventoryGearButton(false);
        ToggleInventoryItemsButton(false);
        HideFighterInventorys();
        ToggleUnequippedLootInventory(false);   
    }
    public void HideFighterInventorys()
    {     
        foreach (Transform trans in fighterInventoryParent)
        {
            trans.gameObject.GetComponent<FighterInventory>().ToggleFighterInventory(false);
        }
    }

    public void DisableFighterInventoryUI()
    {
        ToggleInventoryItemsButton(false);
        ToggleInventoryGearButton(false);
    }

    public void ResetFighterInventorySelections()
    {
        for (int i = 0; i < fighterInventoryParent.childCount; i++)
        {
            if (fighterInventoryParent.GetChild(i))
            {
                FighterInventory fighterInventory = fighterInventoryParent.GetChild(i).GetComponent<FighterInventory>();
                fighterInventory.ResetSlotSelections();
            }
        }
    }

    public void UpdateFighterInventorySelection()
    {
        for (int i = 0; i < fighterInventoryParent.childCount; i++)
        {
            if (fighterInventoryParent.GetChild(i))
            {
                FighterInventory fighterInventory = fighterInventoryParent.GetChild(i).GetComponent<FighterInventory>();
                fighterInventory.ResetSlotSelections();
                if (ShopManager.Instance.GetSelectedShopItem() && GameManager.Instance.activeRoomHeroes.Count > i)
                {
                    if (ShopManager.Instance.GetSelectedShopItem().linkedGearPiece)
                    {
                        GearPiece gear = ShopManager.Instance.GetSelectedShopItem().linkedGearPiece;
                        if (gear.gearType == "helmet")
                        {
                            fighterInventory.UpdateSlotsSelection(fighterInventory.helmetSlot);
                        }
                        else if (gear.gearType == "chestpiece")
                        {
                            fighterInventory.UpdateSlotsSelection(fighterInventory.chestpieceSlot);
                        }
                        else if (gear.gearType == "boots")
                        {
                            fighterInventory.UpdateSlotsSelection(fighterInventory.bootsSlot);
                        }
                        else if (gear.gearType == "pendant" || gear.gearType == "neckless")
                        {
                            fighterInventory.UpdateSlotsSelection(fighterInventory.pendantSlot);
                        }
                        else if (gear.gearType == "earring")
                        {
                            fighterInventory.UpdateSlotsSelection(fighterInventory.earringSlot);
                        }
                        else if (gear.gearType == "belt")
                        {
                            fighterInventory.UpdateSlotsSelection(fighterInventory.beltSlot);
                        }
                        else if (gear.gearType == "glove")
                        {
                            fighterInventory.UpdateSlotsSelection(fighterInventory.gloveSlot);
                        }
                        else if (gear.gearType == "ring")
                        {
                            fighterInventory.UpdateSlotsSelection(fighterInventory.ring1Slot);
                            fighterInventory.UpdateSlotsSelection(fighterInventory.ring2Slot);
                        }
                    }
                }
            }
        }
    }

    public void UpdateFightersInventory(bool byPassPopUp = false)
    {
        HideFighterInventorys();

        if (inventoryModeGear)
        {
            ToggleInventoryItemsButton();
            ToggleInventoryGearButton(false, true);

            //ToggleInventoryMode(true);
        }
        else
        {
            ToggleInventoryItemsButton(false, true);
            ToggleInventoryGearButton();

            //ToggleInventoryMode(false);
        }


        for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
        {
            if (GameManager.Instance.activeRoomHeroes[i])
            {
                fighterInventorys[i].UpdateIconContents(GameManager.Instance.activeRoomHeroes[i], byPassPopUp);
                fighterInventorys[i].ToggleFighterInventory(true);

                fighterInventorys[i].helmetSlot.ToggleButton(true);
                fighterInventorys[i].chestpieceSlot.ToggleButton(true);
                fighterInventorys[i].bootsSlot.ToggleButton(true);
                fighterInventorys[i].pendantSlot.ToggleButton(true);
                fighterInventorys[i].earringSlot.ToggleButton(true);
                fighterInventorys[i].beltSlot.ToggleButton(true);
                fighterInventorys[i].gloveSlot.ToggleButton(true);
                fighterInventorys[i].ring1Slot.ToggleButton(true);
                fighterInventorys[i].ring2Slot.ToggleButton(true);
                fighterInventorys[i].item1Slot.ToggleButton(true);
                fighterInventorys[i].item2Slot.ToggleButton(true);
                fighterInventorys[i].item3Slot.ToggleButton(true);
            }
        }

        if (inventoryModeGear)
            UpdateFighterInventorySelection();
    }
}
