using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnedLootInven : MonoBehaviour
{
    public static OwnedLootInven Instance;

    [SerializeField] private UIElement ownedLootUI;
    [SerializeField] private UIElement ownedTitleTextUI;
    public List<Gear> ownedLootSlots = new List<Gear>();

    private bool ownedLootOpened = false;

    public List<Gear> ownedGear = new List<Gear>();
    public List<Item> ownedItems = new List<Item>();
    public List<GearPiece> ownedGearPieces = new List<GearPiece>();
    public List<ItemPiece> ownedItemPieces = new List<ItemPiece>();
    public List<GearPiece> startingGearPieces = new List<GearPiece>();
    public List<ItemPiece> startingItemPieces = new List<ItemPiece>();
    //public List<Gear> ownedGear = new List<Gear>();
    public List<Gear> wornGearMainAlly = new List<Gear>();
    public List<Gear> wornGearSecondAlly = new List<Gear>();
    public List<Gear> wornGearThirdAlly = new List<Gear>();
    public List<Gear> wornItemsMainAlly = new List<Gear>();
    public List<Gear> wornItemsSecondAlly = new List<Gear>();
    public List<Gear> wornItemsThirdAlly = new List<Gear>();
    public List<Gear> gears = new List<Gear>();
    public List<ItemPiece> items = new List<ItemPiece>();

    [SerializeField] private UIElement buttonExitOwnedGear;

    private void Awake()
    {
        Instance = this;
    }

    public void AddWornGearAllyMain(Gear gear)
    {
        wornGearMainAlly.Add(gear);
    }
    public void AddWornGearAllySecond(Gear gear)
    {
        wornGearSecondAlly.Add(gear);
    }
    public void AddWornGearAllyThird(Gear gear)
    {
        wornGearThirdAlly.Add(gear);
    }

    public List<Gear> GetWornGearMainAlly()
    {
        return wornGearMainAlly;
    }
    public List<Gear> GetWornGearSecondAlly()
    {
        return wornGearSecondAlly;
    }
    public List<Gear> GetWornGearThirdAlly()
    {
        return wornGearThirdAlly;
    }

    public void RemoveOwnedGear(Gear gear)
    {
        ownedGear.Remove(gear);
    }

    public void RemoveWornGearAllyMain(Gear gear)
    {
        wornGearMainAlly.Remove(gear);
    }
    public void RemoveWornGearAllySecond(Gear gear)
    {
        wornGearSecondAlly.Remove(gear);
    }
    public void RemoveWornGearAllyThird(Gear gear)
    {
        wornGearThirdAlly.Remove(gear);
    }

    public void ResetWornGearAllyMain()
    {
        wornGearMainAlly.Clear();
    }

    public void ResetWornGearAllySecond()
    {
        wornGearSecondAlly.Clear();
    }

    public void ResetWornGearAllyThird()
    {
        wornGearThirdAlly.Clear();
    }

    public bool GetOwnedLootOpened()
    {
        return ownedLootOpened;
    }

    private void Start()
    {
        ToggleOwnedGearDisplay(false);
        ToggleOwnedGearEquipButton(false);

        for (int i = 0; i < startingGearPieces.Count; i++)
        {
            GameObject go = Instantiate(GearRewards.Instance.gearGO, ownedLootUI.transform);
            // 1000 pos due to objects spawning in screen and blocking input (they are invisible data carrying objects)
            go.transform.localPosition = new Vector2(1000,1000);
            go.transform.localScale = Vector2.one;

            Gear gear = go.GetComponent<Gear>();
            ownedGear.Add(gear);

            gear.UpdateGearAlpha(false);

            ownedGear[i].UpdateGearImage(startingGearPieces[i].gearIcon);
            if (startingGearPieces[i].gearType == "helmet")
                ownedGear[i].UpdateCurGearType(Gear.GearType.HELMET);
            else if (startingGearPieces[i].gearType == "chestpiece")
                ownedGear[i].UpdateCurGearType(Gear.GearType.CHESTPIECE);
            else if (startingGearPieces[i].gearType == "leggings")
                ownedGear[i].UpdateCurGearType(Gear.GearType.LEGGINGS);
            else if (startingGearPieces[i].gearType == "boots")
                ownedGear[i].UpdateCurGearType(Gear.GearType.BOOTS);

            ownedGear[i].UpdateGearName(startingGearPieces[i].gearName);
            ownedGear[i].UpdateGearBonusHealth(startingGearPieces[i].bonusHealth);
            ownedGear[i].UpdateGearBonusDamage(startingGearPieces[i].bonusDamage);
            ownedGear[i].UpdateGearBonusHealing(startingGearPieces[i].bonusHealing);
            ownedGear[i].UpdateGearBonusDefense(startingGearPieces[i].bonusDefense);
            ownedGear[i].UpdateGearBonusSpeed(startingGearPieces[i].bonusSpeed);
            ownedGear[i].UpdateGearStatis(Gear.GearStatis.OWNED);
            ownedGear[i].ToggleEquipButton(false);
        }

        for (int x = 0; x < startingItemPieces.Count; x++)
        {
            GameObject go = Instantiate(GearRewards.Instance.itemGO, ownedLootUI.transform);
            // 1000 pos due to objects spawning in screen and blocking input (they are invisible data carrying objects)
            go.transform.localPosition = new Vector2(1000, 1000);
            go.transform.localScale = Vector2.one;

            Item item = go.GetComponent<Item>();
            ownedItems.Add(item);

            item.UpdateGearAlpha(false);

            ownedItems[x].UpdateGearImage(startingItemPieces[x].itemSprite);

            /*
            if (startingGearPieces[x].gearType == "helmet")
                ownedItems[x].UpdateCurGearType(Gear.GearType.HELMET);
            else if (startingGearPieces[x].gearType == "chestpiece")
                ownedItems[x].UpdateCurGearType(Gear.GearType.CHESTPIECE);
            else if (startingGearPieces[x].gearType == "leggings")
                ownedItems[x].UpdateCurGearType(Gear.GearType.LEGGINGS);
            else if (startingGearPieces[x].gearType == "boots")
                ownedItems[x].UpdateCurGearType(Gear.GearType.BOOTS);
            */

            ownedItems[x].UpdateGearName(startingItemPieces[x].itemName);
            //ownedGear[x].UpdateGearBonusHealth(startingItemPieces[x].bonusHealth);
            //ownedGear[x].UpdateGearBonusDamage(startingItemPieces[x].bonusDamage);
            //ownedGear[x].UpdateGearBonusHealing(startingItemPieces[x].bonusHealing);
            //ownedGear[x].UpdateGearBonusDefense(startingItemPieces[x].bonusDefense);
            //ownedGear[x].UpdateGearBonusSpeed(startingItemPieces[x].bonusSpeed);
            //ownedItems[x].UpdateGearStatis(Gear.GearStatis.OWNED);
            ownedItems[x].ToggleEquipButton(false);
            //ownedItems
            
        }
    }

    /*
    public void AddOwnedGearPieces(GearPiece gearPieces)
    {
        ownedGearPieces.Add(gearPieces);
    }

    public void ResetOwnedGearPieces()
    {
        ownedGearPieces.Clear();
    }
    */

    public void AddOwnedGear(Gear gear)
    {
        ownedGear.Add(gear);
    }

    public void ResetOwnedGear()
    {
        ownedGear.Clear();
    }

    public void UpdateOwnedTitleTextUI(string text)
    {
        ownedTitleTextUI.UpdateContentText(text);
    }

    // TODO - Make this trigger true when player presses button on corner of item for comparing against owned items
    public void ToggleOwnedGearDisplay(bool toggle, string titleText = "")
    {
        if (toggle)
        {
            ownedLootUI.UpdateAlpha(1);

            ToggleOwnedGearEquipButton(true);

            if (titleText == "Owned Items")
                FillOwnedGearSlots(false);
            else
                FillOwnedGearSlots(true); 

            ownedLootOpened = true;

            UpdateOwnedTitleTextUI(titleText);
        }
        else
        {
            ownedLootUI.UpdateAlpha(0);

            ToggleOwnedGearEquipButton(false);
            ownedLootOpened = false;
        }

        buttonExitOwnedGear.ToggleButton(toggle);
    }

    void ClearOwnedItemSlots()
    {
        for (int i = 0; i < ownedLootSlots.Count; i++)
        {
            ownedLootSlots[i].UpdateGearImage(TeamGearManager.Instance.clearSlotSprite);
        }
    }

    public void ClearOwnedItemsSlotsSelection()
    {
        for (int i = 0; i < ownedLootSlots.Count; i++)
        {
            ownedLootSlots[i].ToggleGearEnabled(false);
        }
    }

    public void EnableOwnedItemsSlotSelection(Gear gear)
    {
        gear.ToggleGearEnabled(true);
    }

    public void FillOwnedGearSlots(bool gear = true)
    {
        //ClearOwnedItemSlots();
        ClearOwnedItemsSlotsSelection();

        int ownedGearSlotIndex = 0;

        for (int x = 0; x < ownedLootSlots.Count; x++)
        {
            ownedLootSlots[x].ResetGearSlot(true);
        }

        // If item
        if (!gear)
        {

        }
        // If Gear
        else if (gear)
        {
            bool stophelmetMain = false;
            bool stopChestPieceMain = false;
            bool stopLeggingsMain = false;
            bool stopBootsMain = false;

            bool stophelmetSec = false;
            bool stopChestPieceSec = false;
            bool stopLeggingsSec = false;
            bool stopBootsSec = false;

            bool stophelmetThi = false;
            bool stopChestPieceThi = false;
            bool stopLeggingsThi = false;
            bool stopBootsThi = false;

            int index = 0;

            gears.Clear();

            // int index = 0;
            for (int i = 0; i < ownedGear.Count; i++)
            {
                if (TeamGearManager.Instance.GetSelectedGearSlot() == null)
                    break;

                // Safety
                if (wornGearMainAlly.Count > i)
                {
                    if (ownedGear.Contains(wornGearMainAlly[i]) && !gears.Contains(wornGearMainAlly[i]))
                    {
                        gears.Add(wornGearMainAlly[i]);
                        //ToggleOwnedGearEquipButton(false);

                        ownedGear[i].ToggleEquipButton(false);
                        ownedGear[i].UpdateGearImage(TeamGearManager.Instance.clearSlotSprite);
                        ownedGear[i].UpdateCurGearType(Gear.GearType.EMPTY);
                        ownedGear[i].isEmpty = true;

                        index++;
                        continue;
                    }
                }

                if (wornGearSecondAlly.Count > i)
                {
                    if (ownedGear.Contains(wornGearSecondAlly[i]) && !gears.Contains(wornGearSecondAlly[i]))
                    {
                        gears.Add(wornGearSecondAlly[i]);
                        //ToggleOwnedGearEquipButton(false);

                        ownedGear[i].ToggleEquipButton(false);
                        ownedGear[i].UpdateGearImage(TeamGearManager.Instance.clearSlotSprite);
                        ownedGear[i].UpdateCurGearType(Gear.GearType.EMPTY);
                        ownedGear[i].isEmpty = true;

                        index++;
                        continue;
                    }
                }

                if (wornGearThirdAlly.Count > i)
                {
                    if (ownedGear.Contains(wornGearThirdAlly[i]) && !gears.Contains(wornGearThirdAlly[i]))
                    {
                        gears.Add(wornGearThirdAlly[i]);
                        //ToggleOwnedGearEquipButton(false);

                        ownedGear[i].ToggleEquipButton(false);
                        ownedGear[i].UpdateGearImage(TeamGearManager.Instance.clearSlotSprite);
                        ownedGear[i].UpdateCurGearType(Gear.GearType.EMPTY);
                        ownedGear[i].isEmpty = true;

                        index++;
                        continue;
                    }
                }

                // if an item is equipped, skip it in the own inventory display
                //if (!gears.Contains(wornGear[i]))

                //ToggleOwnedGearEquipButton(true);
                // If selected armor piece is helmet, display only owned helmets
                if (TeamGearManager.Instance.GetSelectedGearSlot().GetCurGearType() == Gear.GearType.HELMET)
                {
                    if (ownedGear[i].GetCurGearType() == Gear.GearType.HELMET)
                    {
                        // Update gear icon
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearImage(ownedGear[i].GetGearImage());
                        ownedLootSlots[ownedGearSlotIndex].UpdateCurGearType(Gear.GearType.HELMET);
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearName(ownedGear[i].GetGearName());
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusHealth(ownedGear[i].GetBonusHealth());
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusDamage(ownedGear[i].GetBonusDamage());
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusHealing(ownedGear[i].GetBonusHealing());
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusDefense(ownedGear[i].GetBonusDefense());
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusSpeed(ownedGear[i].GetBonusSpeed());
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearStatis(Gear.GearStatis.OWNED);
                        ownedLootSlots[ownedGearSlotIndex].ToggleEquipButton(true);
                        ownedLootSlots[ownedGearSlotIndex].isEmpty = false;

                        ownedGearSlotIndex++;
                    }
                }
                else if (TeamGearManager.Instance.GetSelectedGearSlot().GetCurGearType() == Gear.GearType.CHESTPIECE)
                {
                    if (ownedGear[i].GetCurGearType() == Gear.GearType.CHESTPIECE)
                    {
                        // Update gear icon
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearImage(ownedGear[i].GetGearImage());
                        ownedLootSlots[ownedGearSlotIndex].UpdateCurGearType(Gear.GearType.CHESTPIECE);
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearName(ownedGear[i].GetGearName());
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusHealth(ownedGear[i].GetBonusHealth());
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusDamage(ownedGear[i].GetBonusDamage());
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusHealing(ownedGear[i].GetBonusHealing());
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusDefense(ownedGear[i].GetBonusDefense());
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusSpeed(ownedGear[i].GetBonusSpeed());
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearStatis(Gear.GearStatis.OWNED);
                        ownedLootSlots[ownedGearSlotIndex].ToggleEquipButton(true);
                        ownedLootSlots[ownedGearSlotIndex].isEmpty = false;

                        ownedGearSlotIndex++;
                    }
                }
                else if (TeamGearManager.Instance.GetSelectedGearSlot().GetCurGearType() == Gear.GearType.LEGGINGS)
                {
                    if (ownedGear[i].GetCurGearType() == Gear.GearType.LEGGINGS)
                    {
                        // Update gear icon
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearImage(ownedGear[i].GetGearImage());
                        ownedLootSlots[ownedGearSlotIndex].UpdateCurGearType(Gear.GearType.LEGGINGS);
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearName(ownedGear[i].GetGearName());
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusHealth(ownedGear[i].GetBonusHealth());
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusDamage(ownedGear[i].GetBonusDamage());
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusHealing(ownedGear[i].GetBonusHealing());
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusDefense(ownedGear[i].GetBonusDefense());
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusSpeed(ownedGear[i].GetBonusSpeed());
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearStatis(Gear.GearStatis.OWNED);
                        ownedLootSlots[ownedGearSlotIndex].ToggleEquipButton(true);
                        ownedLootSlots[ownedGearSlotIndex].isEmpty = false;

                        ownedGearSlotIndex++;
                    }
                }
                else if (TeamGearManager.Instance.GetSelectedGearSlot().GetCurGearType() == Gear.GearType.BOOTS)
                {
                    if (ownedGear[i].GetCurGearType() == Gear.GearType.BOOTS)
                    {
                        // Update gear icon
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearImage(ownedGear[i].GetGearImage());
                        ownedLootSlots[ownedGearSlotIndex].UpdateCurGearType(Gear.GearType.BOOTS);
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearName(ownedGear[i].GetGearName());
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusHealth(ownedGear[i].GetBonusHealth());
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusDamage(ownedGear[i].GetBonusDamage());
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusHealing(ownedGear[i].GetBonusHealing());
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusDefense(ownedGear[i].GetBonusDefense());
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusSpeed(ownedGear[i].GetBonusSpeed());
                        ownedLootSlots[ownedGearSlotIndex].UpdateGearStatis(Gear.GearStatis.OWNED);
                        ownedLootSlots[ownedGearSlotIndex].ToggleEquipButton(true);
                        ownedLootSlots[ownedGearSlotIndex].isEmpty = false;
                        ownedGearSlotIndex++;
                    }
                }
            }
        }


        // Make all empty owned gear slots display nothing 
        for (int x = 0; x < ownedLootSlots.Count; x++)
        {
            if (ownedLootSlots[x].isEmpty)
            {
                ownedLootSlots[x].ToggleEquipButton(false);
                ownedLootSlots[x].UpdateGearImage(TeamGearManager.Instance.clearSlotSprite);
                //ownedGear[i].UpdateCurGearType(Gear.GearType.EMPTY);
                ownedLootSlots[x].isEmpty = true;
            }
        }
    }

    public void ToggleOwnedGearEquipButton(bool toggle)
    {
        for (int i = 0; i < ownedLootSlots.Count; i++)
        {
            ownedLootSlots[i].gameObject.GetComponent<UIElement>().ToggleButton(toggle);
            ownedLootSlots[i].gameObject.GetComponent<UIElement>().ToggleButton2(toggle);
        }
    }
}