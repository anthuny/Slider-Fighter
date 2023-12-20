using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnedGearInven : MonoBehaviour
{
    public static OwnedGearInven Instance;

    [SerializeField] private UIElement ownedGearUI;
    public List<Gear> ownedGearSlots = new List<Gear>();

    private bool ownedGearOpened = false;

    public List<Gear> ownedGear = new List<Gear>();
    public List<GearPiece> ownedGearPieces = new List<GearPiece>();
    public List<GearPiece> startingGearPieces = new List<GearPiece>();
    //public List<Gear> ownedGear = new List<Gear>();
    public List<Gear> wornGearMainAlly = new List<Gear>();
    public List<Gear> wornGearSecondAlly = new List<Gear>();
    public List<Gear> wornGearThirdAlly = new List<Gear>();
    public List<Gear> gears = new List<Gear>();
    [SerializeField] private UIElement buttonToggleOwnedGear;

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

    public bool GetOwnedGearOpened()
    {
        return ownedGearOpened;
    }

    private void Start()
    {
        ToggleOwnedGearDisplay(false);
        ToggleOwnedGearEquipButton(false);

        for (int i = 0; i < startingGearPieces.Count; i++)
        {
            GameObject go = Instantiate(GearRewards.Instance.gearGO, ownedGearUI.transform);
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

    // TODO - Make this trigger true when player presses button on corner of item for comparing against owned items
    public void ToggleOwnedGearDisplay(bool toggle)
    {
        if (toggle)
        {
            ownedGearUI.UpdateAlpha(1);

            ToggleOwnedGearEquipButton(true);
            FillOwnedGearSlots();
            ownedGearOpened = true;
        }
        else
        {
            ownedGearUI.UpdateAlpha(0);

            ToggleOwnedGearEquipButton(false);
            ownedGearOpened = false;
        }

        buttonToggleOwnedGear.ToggleButton(toggle);
    }

    void ClearOwnedItemSlots()
    {
        for (int i = 0; i < ownedGearSlots.Count; i++)
        {
            ownedGearSlots[i].UpdateGearImage(TeamGearManager.Instance.clearSlotSprite);
        }
    }

    public void ClearOwnedItemsSlotsSelection()
    {
        for (int i = 0; i < ownedGearSlots.Count; i++)
        {
            ownedGearSlots[i].ToggleGearEnabled(false);
        }
    }

    public void EnableOwnedItemsSlotSelection(Gear gear)
    {
        gear.ToggleGearEnabled(true);
    }

    public void FillOwnedGearSlots()
    {
        //ClearOwnedItemSlots();
        ClearOwnedItemsSlotsSelection();

        int ownedGearSlotIndex = 0;

        for (int x = 0; x < ownedGearSlots.Count; x++)
        {
            ownedGearSlots[x].ResetGearSlot(true);
        }

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
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearImage(ownedGear[i].GetGearImage());
                    ownedGearSlots[ownedGearSlotIndex].UpdateCurGearType(Gear.GearType.HELMET);
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearName(ownedGear[i].GetGearName());
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearBonusHealth(ownedGear[i].GetBonusHealth());
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearBonusDamage(ownedGear[i].GetBonusDamage());
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearBonusHealing(ownedGear[i].GetBonusHealing());
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearBonusDefense(ownedGear[i].GetBonusDefense());
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearBonusSpeed(ownedGear[i].GetBonusSpeed());
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearStatis(Gear.GearStatis.OWNED);
                    ownedGearSlots[ownedGearSlotIndex].ToggleEquipButton(true);
                    ownedGearSlots[ownedGearSlotIndex].isEmpty = false;

                    ownedGearSlotIndex++;
                }
            }
            else if (TeamGearManager.Instance.GetSelectedGearSlot().GetCurGearType() == Gear.GearType.CHESTPIECE)
            {
                if (ownedGear[i].GetCurGearType() == Gear.GearType.CHESTPIECE)
                {
                    // Update gear icon
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearImage(ownedGear[i].GetGearImage());
                    ownedGearSlots[ownedGearSlotIndex].UpdateCurGearType(Gear.GearType.CHESTPIECE);
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearName(ownedGear[i].GetGearName());
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearBonusHealth(ownedGear[i].GetBonusHealth());
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearBonusDamage(ownedGear[i].GetBonusDamage());
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearBonusHealing(ownedGear[i].GetBonusHealing());
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearBonusDefense(ownedGear[i].GetBonusDefense());
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearBonusSpeed(ownedGear[i].GetBonusSpeed());
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearStatis(Gear.GearStatis.OWNED);
                    ownedGearSlots[ownedGearSlotIndex].ToggleEquipButton(true);
                    ownedGearSlots[ownedGearSlotIndex].isEmpty = false;

                    ownedGearSlotIndex++;
                }
            }
            else if (TeamGearManager.Instance.GetSelectedGearSlot().GetCurGearType() == Gear.GearType.LEGGINGS)
            {
                if (ownedGear[i].GetCurGearType() == Gear.GearType.LEGGINGS)
                {
                    // Update gear icon
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearImage(ownedGear[i].GetGearImage());
                    ownedGearSlots[ownedGearSlotIndex].UpdateCurGearType(Gear.GearType.LEGGINGS);
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearName(ownedGear[i].GetGearName());
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearBonusHealth(ownedGear[i].GetBonusHealth());
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearBonusDamage(ownedGear[i].GetBonusDamage());
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearBonusHealing(ownedGear[i].GetBonusHealing());
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearBonusDefense(ownedGear[i].GetBonusDefense());
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearBonusSpeed(ownedGear[i].GetBonusSpeed());
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearStatis(Gear.GearStatis.OWNED);
                    ownedGearSlots[ownedGearSlotIndex].ToggleEquipButton(true);
                    ownedGearSlots[ownedGearSlotIndex].isEmpty = false;

                    ownedGearSlotIndex++;
                }
            }
            else if (TeamGearManager.Instance.GetSelectedGearSlot().GetCurGearType() == Gear.GearType.BOOTS)
            {
                if (ownedGear[i].GetCurGearType() == Gear.GearType.BOOTS)
                {
                    // Update gear icon
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearImage(ownedGear[i].GetGearImage());
                    ownedGearSlots[ownedGearSlotIndex].UpdateCurGearType(Gear.GearType.BOOTS);
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearName(ownedGear[i].GetGearName());
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearBonusHealth(ownedGear[i].GetBonusHealth());
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearBonusDamage(ownedGear[i].GetBonusDamage());
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearBonusHealing(ownedGear[i].GetBonusHealing());
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearBonusDefense(ownedGear[i].GetBonusDefense());
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearBonusSpeed(ownedGear[i].GetBonusSpeed());
                    ownedGearSlots[ownedGearSlotIndex].UpdateGearStatis(Gear.GearStatis.OWNED);
                    ownedGearSlots[ownedGearSlotIndex].ToggleEquipButton(true);
                    ownedGearSlots[ownedGearSlotIndex].isEmpty = false;
                    ownedGearSlotIndex++;
                }
            }
        }

        // Make all empty owned gear slots display nothing 
        for (int x = 0; x < ownedGearSlots.Count; x++)
        {
            if (ownedGearSlots[x].isEmpty)
            {
                ownedGearSlots[x].ToggleEquipButton(false);
                ownedGearSlots[x].UpdateGearImage(TeamGearManager.Instance.clearSlotSprite);
                //ownedGear[i].UpdateCurGearType(Gear.GearType.EMPTY);
                ownedGearSlots[x].isEmpty = true;
            }
        }
    }

    public void ToggleOwnedGearEquipButton(bool toggle)
    {
        for (int i = 0; i < ownedGearSlots.Count; i++)
        {
            ownedGearSlots[i].gameObject.GetComponent<UIElement>().ToggleButton(toggle);
        }
    }

}
