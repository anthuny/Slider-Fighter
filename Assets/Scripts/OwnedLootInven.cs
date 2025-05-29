using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnedLootInven : MonoBehaviour
{
    public static OwnedLootInven Instance;

    [SerializeField] private UIElement ownedLootUI;
    [SerializeField] private UIElement ownedTitleTextUI;
    public List<Slot> ownedLootSlots = new List<Slot>();

    public bool ownedLootOpened = false;

    public List<Slot> ownedGear = new List<Slot>();
    public List<Slot> ownedItems = new List<Slot>();
    public List<SkillData> ownedSkills = new List<SkillData>();

    //public List<Slot> ownedSkillsSlots = new List<Slot>();
    public List<GearPiece> startingGearPieces = new List<GearPiece>();
    public List<ItemPiece> startingItemPieces = new List<ItemPiece>();
    //public List<Gear> ownedGear = new List<Gear>();
    public List<Slot> wornGearMainAlly = new List<Slot>();
    public List<Slot> wornGearSecondAlly = new List<Slot>();
    public List<Slot> wornGearThirdAlly = new List<Slot>();
    public List<Slot> wornItemsMainAlly = new List<Slot>();
    public List<Slot> wornItemsSecondAlly = new List<Slot>();
    public List<Slot> wornItemsThirdAlly = new List<Slot>();
    public List<Slot> wornSkillsAlly = new List<Slot>();
    public List<Slot> gears = new List<Slot>();
    public List<Slot> skills = new List<Slot>();
    public List<ItemPiece> items = new List<ItemPiece>();

    [SerializeField] private Color skillSlotBGColour;
    [SerializeField] private Color otherSlotBGColour;

    [SerializeField] private UIElement buttonExitOwnedGear;

    [SerializeField] private List<GearPiece> fighterMainEquippedGear = new List<GearPiece>();
    [SerializeField] private List<GearPiece> fighterSecEquippedGear = new List<GearPiece>();
    [SerializeField] private List<GearPiece> fighterThiEquippedGear = new List<GearPiece>();

    [SerializeField] private List<ItemPiece> fighterMainEquippedItems = new List<ItemPiece>();
    [SerializeField] private List<ItemPiece> fighterSecEquippedItems = new List<ItemPiece>();
    [SerializeField] private List<ItemPiece> fighterThiEquippedItems = new List<ItemPiece>();

    public List<GearPiece> GetFighterMainEquippedGear()
    {
        return fighterMainEquippedGear;
    }
    public List<GearPiece> GetFighterSecEquippedGear()
    {
        return fighterSecEquippedGear;
    }
    public List<GearPiece> GetFighterThiEquippedGear()
    {
        return fighterThiEquippedGear;
    }
    public List<ItemPiece> GetFighterMainEquippedItems()
    {
        return fighterMainEquippedItems;
    }
    public List<ItemPiece> GetFighterSecEquippedItems()
    {
        return fighterSecEquippedItems;
    }
    public List<ItemPiece> GetFighterThiEquippedItems()
    {
        return fighterThiEquippedItems;
    }

    public void AddFighterEquippedGear(UnitFunctionality unit, GearPiece gear)
    {
        if (unit.gearIndex == 0)
            fighterMainEquippedGear.Add(gear);
        else if (unit.gearIndex == 1)
            fighterSecEquippedGear.Add(gear);
        else if (unit.gearIndex == 2)
            fighterThiEquippedGear.Add(gear);
    }

    public void RemoveFighterEquippedGear(UnitFunctionality unit, GearPiece gear)
    {
        if (unit.gearIndex == 0)
            fighterMainEquippedGear.Remove(gear);
        else if (unit.gearIndex == 1)
            fighterSecEquippedGear.Remove(gear);
        else if (unit.gearIndex == 2)
            fighterThiEquippedGear.Remove(gear);
    }

    public void ClearFighterEquippedGear()
    {
        fighterMainEquippedGear.Clear();
        fighterSecEquippedGear.Clear();
        fighterThiEquippedGear.Clear();
    }

    public void AddFighterEquippedItem(UnitFunctionality unit, ItemPiece item)
    {
        if (unit.gearIndex == 0)
            fighterMainEquippedItems.Add(item);
        else if (unit.gearIndex == 1)
            fighterSecEquippedItems.Add(item);
        else if (unit.gearIndex == 2)
            fighterThiEquippedItems.Add(item);
    }

    public void RemoveFighterEquippedItem(UnitFunctionality unit, ItemPiece item)
    {
        if (unit.gearIndex == 0)
            fighterMainEquippedItems.Remove(item);
        else if (unit.gearIndex == 1)
            fighterSecEquippedItems.Remove(item);
        else if (unit.gearIndex == 2)
            fighterThiEquippedItems.Remove(item);
    }

    public void ClearFighterEquippedItem()
    {
        fighterMainEquippedItems.Clear();
        fighterSecEquippedItems.Clear();
        fighterThiEquippedItems.Clear();
    }

    public Color GetSkillSlotBGColour()
    {
        return skillSlotBGColour;
    }
    public Color GetOtherSlotBGColour()
    {
        return otherSlotBGColour;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void DisableCoverForOwnedSlots()
    {
        
        if (TeamGearManager.Instance.playerInGearTab)
        {
            for (int i = 0; i < ownedLootSlots.Count; i++)
            {
                ownedLootSlots[i].ToggleCoverUI(false);
            }
        }
        
    }

    public void UpdateWornLootOwning()
    {
        bool doOnce = false;

        for (int i = 0; i < GameManager.Instance.fallenHeroes.Count; i++)
        {
            // SWAP GEAR AND ITEMS FROM POSITION 2 TO 1
            for (int x = 0; x < GameManager.Instance.activeRoomHeroes.Count; x++)
            {
                if (GameManager.Instance.activeRoomHeroes[x].GetUnitName() == GameManager.Instance.fallenHeroes[i].GetUnitName())
                {
                    if (GameManager.Instance.fallenHeroes.Count == 1)
                    {
                        if (x == 0)
                        {
                            // lost a main hero, put 2 to 1
                            if (GameManager.Instance.activeRoomHeroes.Count == 2)
                            {
                                #region Gear Main
                                wornGearMainAlly.Clear();

                                // 2 to 1
                                for (int t = 0; t < wornGearSecondAlly.Count; t++)
                                {
                                    wornGearMainAlly.Add(wornGearSecondAlly[t]);
                                }

                                TeamGearManager.Instance.equippedHelmetMain = TeamGearManager.Instance.equippedHelmetSec;
                                TeamGearManager.Instance.equippedChestpieceMain = TeamGearManager.Instance.equippedChestpieceSec;
                                TeamGearManager.Instance.equippedBootsMain = TeamGearManager.Instance.equippedBootsSec;

                                wornGearSecondAlly.Clear();

                                TeamGearManager.Instance.equippedHelmetSec = null;
                                TeamGearManager.Instance.equippedChestpieceSec = null;
                                TeamGearManager.Instance.equippedBootsSec = null;

                                bool doneHelm = false;
                                bool doneChest = false;
                                bool doneBoots = false;

                                for (int l = 0; l < wornGearMainAlly.Count; l++)
                                {
                                    if (TeamGearManager.Instance.equippedHelmetMain != null && !doneHelm)
                                    {
                                        doneHelm = true;

                                        TeamGearManager.Instance.mainFighterGearSlots[0].isEmpty = false;
                                        TeamGearManager.Instance.mainFighterGearSlots[0].UpdateSlotName(TeamGearManager.Instance.equippedHelmetMain.gearName);
                                        TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusHealth(TeamGearManager.Instance.equippedHelmetMain.bonusHealth);
                                        TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusHealing(TeamGearManager.Instance.equippedHelmetMain.bonusHealing);
                                        TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusDefense(TeamGearManager.Instance.equippedHelmetMain.bonusDefense);
                                        TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusDamage(TeamGearManager.Instance.equippedHelmetMain.bonusDamage);
                                        TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedHelmetMain.bonusSpeed);
                                    }

                                    else if (TeamGearManager.Instance.equippedChestpieceMain != null && !doneChest)
                                    {
                                        doneChest = true;

                                        TeamGearManager.Instance.mainFighterGearSlots[1].isEmpty = false;
                                        TeamGearManager.Instance.mainFighterGearSlots[1].UpdateSlotName(TeamGearManager.Instance.equippedChestpieceMain.gearName);
                                        TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusHealth(TeamGearManager.Instance.equippedChestpieceMain.bonusHealth);
                                        TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusHealing(TeamGearManager.Instance.equippedChestpieceMain.bonusHealing);
                                        TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusDefense(TeamGearManager.Instance.equippedChestpieceMain.bonusDefense);
                                        TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusDamage(TeamGearManager.Instance.equippedChestpieceMain.bonusDamage);
                                        TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedChestpieceMain.bonusSpeed);
                                    }
                                    else if (TeamGearManager.Instance.equippedBootsMain != null && !doneBoots)
                                    {
                                        doneBoots = true;

                                        TeamGearManager.Instance.mainFighterGearSlots[2].isEmpty = false;
                                        TeamGearManager.Instance.mainFighterGearSlots[2].UpdateSlotName(TeamGearManager.Instance.equippedBootsMain.gearName);
                                        TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusHealth(TeamGearManager.Instance.equippedBootsMain.bonusHealth);
                                        TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusHealing(TeamGearManager.Instance.equippedBootsMain.bonusHealing);
                                        TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusDefense(TeamGearManager.Instance.equippedBootsMain.bonusDefense);
                                        TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusDamage(TeamGearManager.Instance.equippedBootsMain.bonusDamage);
                                        TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedBootsMain.bonusSpeed);
                                    }
                                }

                                for (int l = 0; l < wornGearSecondAlly.Count; l++)
                                {
                                    if (TeamGearManager.Instance.equippedHelmetSec != null && !doneHelm)
                                    {
                                        doneHelm = true;

                                        TeamGearManager.Instance.ally2GearSlots[0].isEmpty = false;
                                        TeamGearManager.Instance.ally2GearSlots[0].UpdateSlotName(TeamGearManager.Instance.equippedHelmetSec.gearName);
                                        TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusHealth(TeamGearManager.Instance.equippedHelmetSec.bonusHealth);
                                        TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusHealing(TeamGearManager.Instance.equippedHelmetSec.bonusHealing);
                                        TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusDefense(TeamGearManager.Instance.equippedHelmetSec.bonusDefense);
                                        TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusDamage(TeamGearManager.Instance.equippedHelmetSec.bonusDamage);
                                        TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedHelmetSec.bonusSpeed);
                                    }

                                    else if (TeamGearManager.Instance.equippedChestpieceSec != null && !doneChest)
                                    {
                                        doneChest = true;

                                        TeamGearManager.Instance.ally2GearSlots[1].isEmpty = false;
                                        TeamGearManager.Instance.ally2GearSlots[1].UpdateSlotName(TeamGearManager.Instance.equippedChestpieceSec.gearName);
                                        TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusHealth(TeamGearManager.Instance.equippedChestpieceSec.bonusHealth);
                                        TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusHealing(TeamGearManager.Instance.equippedChestpieceSec.bonusHealing);
                                        TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusDefense(TeamGearManager.Instance.equippedChestpieceSec.bonusDefense);
                                        TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusDamage(TeamGearManager.Instance.equippedChestpieceSec.bonusDamage);
                                        TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedChestpieceSec.bonusSpeed);
                                    }
                                    else if (TeamGearManager.Instance.equippedBootsSec != null && !doneBoots)
                                    {
                                        doneBoots = true;

                                        TeamGearManager.Instance.ally2GearSlots[2].isEmpty = false;
                                        TeamGearManager.Instance.ally2GearSlots[2].UpdateSlotName(TeamGearManager.Instance.equippedBootsSec.gearName);
                                        TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusHealth(TeamGearManager.Instance.equippedBootsSec.bonusHealth);
                                        TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusHealing(TeamGearManager.Instance.equippedBootsSec.bonusHealing);
                                        TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusDefense(TeamGearManager.Instance.equippedBootsSec.bonusDefense);
                                        TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusDamage(TeamGearManager.Instance.equippedBootsSec.bonusDamage);
                                        TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedBootsSec.bonusSpeed);
                                    }
                                }

                                for (int l = 0; l < wornGearThirdAlly.Count; l++)
                                {
                                    if (TeamGearManager.Instance.equippedHelmetThi != null && !doneHelm)
                                    {
                                        doneHelm = true;

                                        TeamGearManager.Instance.ally3GearSlots[0].isEmpty = false;
                                        TeamGearManager.Instance.ally3GearSlots[0].UpdateSlotName(TeamGearManager.Instance.equippedHelmetThi.gearName);
                                        TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusHealth(TeamGearManager.Instance.equippedHelmetThi.bonusHealth);
                                        TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusHealing(TeamGearManager.Instance.equippedHelmetThi.bonusHealing);
                                        TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusDefense(TeamGearManager.Instance.equippedHelmetThi.bonusDefense);
                                        TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusDamage(TeamGearManager.Instance.equippedHelmetThi.bonusDamage);
                                        TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedHelmetThi.bonusSpeed);
                                    }

                                    else if (TeamGearManager.Instance.equippedChestpieceThi != null && !doneChest)
                                    {
                                        doneChest = true;

                                        TeamGearManager.Instance.ally3GearSlots[1].isEmpty = false;
                                        TeamGearManager.Instance.ally3GearSlots[1].UpdateSlotName(TeamGearManager.Instance.equippedChestpieceThi.gearName);
                                        TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusHealth(TeamGearManager.Instance.equippedChestpieceThi.bonusHealth);
                                        TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusHealing(TeamGearManager.Instance.equippedChestpieceThi.bonusHealing);
                                        TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusDefense(TeamGearManager.Instance.equippedChestpieceThi.bonusDefense);
                                        TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusDamage(TeamGearManager.Instance.equippedChestpieceThi.bonusDamage);
                                        TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedChestpieceThi.bonusSpeed);
                                    }
                                    else if (TeamGearManager.Instance.equippedBootsThi != null && !doneBoots)
                                    {
                                        doneBoots = true;

                                        TeamGearManager.Instance.ally3GearSlots[2].isEmpty = false;
                                        TeamGearManager.Instance.ally3GearSlots[2].UpdateSlotName(TeamGearManager.Instance.equippedBootsThi.gearName);
                                        TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusHealth(TeamGearManager.Instance.equippedBootsThi.bonusHealth);
                                        TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusHealing(TeamGearManager.Instance.equippedBootsThi.bonusHealing);
                                        TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusDefense(TeamGearManager.Instance.equippedBootsThi.bonusDefense);
                                        TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusDamage(TeamGearManager.Instance.equippedBootsThi.bonusDamage);
                                        TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedBootsThi.bonusSpeed);
                                    }
                                }
                                #endregion

                                #region Items Main
                                wornItemsMainAlly.Clear();

                                // 2 to 1
                                for (int t = 0; t < wornItemsSecondAlly.Count; t++)
                                {
                                    wornItemsMainAlly.Add(wornItemsSecondAlly[t]);
                                }

                                wornItemsSecondAlly.Clear();


                                TeamItemsManager.Instance.equippedItemsMain.Clear();

                                for (int h = 0; h < TeamItemsManager.Instance.equippedItemsSecond.Count; h++)
                                {
                                    TeamItemsManager.Instance.equippedItemsMain.Add(TeamItemsManager.Instance.equippedItemsSecond[h]);
                                }

                                TeamItemsManager.Instance.equippedItemsSecond.Clear();

                                bool done1st = false;
                                bool done2nd = false;
                                bool done3rd = false;

                                for (int l = 0; l < wornItemsMainAlly.Count; l++)
                                {
                                    if (TeamItemsManager.Instance.equippedItemsMain.Count == 1 && !done1st)
                                    {
                                        done1st = true;

                                        TeamItemsManager.Instance.ally1ItemsSlots[0].isEmpty = false;
                                        TeamItemsManager.Instance.ally1ItemsSlots[0].UpdateSlotName(TeamItemsManager.Instance.equippedItemsMain[0].itemName);
                                    }
                                    else if (TeamItemsManager.Instance.equippedItemsMain.Count == 2 && !done2nd)
                                    {
                                        done2nd = true;

                                        TeamItemsManager.Instance.ally1ItemsSlots[1].isEmpty = false;
                                        TeamItemsManager.Instance.ally1ItemsSlots[1].UpdateSlotName(TeamItemsManager.Instance.equippedItemsMain[1].itemName);
                                    }
                                    else if (TeamItemsManager.Instance.equippedItemsMain.Count == 3 && !done3rd)
                                    {
                                        done3rd = true;

                                        TeamItemsManager.Instance.ally1ItemsSlots[2].isEmpty = false;
                                        TeamItemsManager.Instance.ally1ItemsSlots[2].UpdateSlotName(TeamItemsManager.Instance.equippedItemsMain[2].itemName);
                                    }
                                }

                                for (int l = 0; l < wornItemsSecondAlly.Count; l++)
                                {
                                    if (TeamItemsManager.Instance.equippedItemsSecond.Count == 1 && !done1st)
                                    {
                                        done1st = true;

                                        TeamItemsManager.Instance.ally2ItemsSlots[0].isEmpty = false;
                                        TeamItemsManager.Instance.ally2ItemsSlots[0].UpdateSlotName(TeamItemsManager.Instance.equippedItemsSecond[0].itemName);
                                    }
                                    else if (TeamItemsManager.Instance.equippedItemsSecond.Count == 2 && !done2nd)
                                    {
                                        done2nd = true;

                                        TeamItemsManager.Instance.ally2ItemsSlots[1].isEmpty = false;
                                        TeamItemsManager.Instance.ally2ItemsSlots[1].UpdateSlotName(TeamItemsManager.Instance.equippedItemsSecond[1].itemName);
                                    }
                                    else if (TeamItemsManager.Instance.equippedItemsSecond.Count == 3 && !done3rd)
                                    {
                                        done3rd = true;

                                        TeamItemsManager.Instance.ally2ItemsSlots[2].isEmpty = false;
                                        TeamItemsManager.Instance.ally2ItemsSlots[2].UpdateSlotName(TeamItemsManager.Instance.equippedItemsSecond[2].itemName);
                                    }
                                }

                                for (int l = 0; l < wornItemsThirdAlly.Count; l++)
                                {
                                    if (TeamItemsManager.Instance.equippedItemsThird.Count == 1 && !done1st)
                                    {
                                        done1st = true;

                                        TeamItemsManager.Instance.ally3ItemsSlots[0].isEmpty = false;
                                        TeamItemsManager.Instance.ally3ItemsSlots[0].UpdateSlotName(TeamItemsManager.Instance.equippedItemsThird[0].itemName);
                                    }
                                    else if (TeamItemsManager.Instance.equippedItemsThird.Count == 2 && !done2nd)
                                    {
                                        done2nd = true;

                                        TeamItemsManager.Instance.ally3ItemsSlots[1].isEmpty = false;
                                        TeamItemsManager.Instance.ally3ItemsSlots[1].UpdateSlotName(TeamItemsManager.Instance.equippedItemsThird[1].itemName);
                                    }
                                    else if (TeamItemsManager.Instance.equippedItemsThird.Count == 3 && !done3rd)
                                    {
                                        done3rd = true;

                                        TeamItemsManager.Instance.ally3ItemsSlots[2].isEmpty = false;
                                        TeamItemsManager.Instance.ally3ItemsSlots[2].UpdateSlotName(TeamItemsManager.Instance.equippedItemsThird[2].itemName);
                                    }
                                }
                                #endregion
                                continue;

                            }
                            else if (GameManager.Instance.activeRoomHeroes.Count == 3)
                            {
                                #region Gear Main
                                wornGearMainAlly.Clear();

                                // 2 to 1
                                for (int t = 0; t < wornGearSecondAlly.Count; t++)
                                {
                                    wornGearMainAlly.Add(wornGearSecondAlly[t]);
                                }

                                TeamGearManager.Instance.equippedHelmetMain = TeamGearManager.Instance.equippedHelmetSec;
                                TeamGearManager.Instance.equippedChestpieceMain = TeamGearManager.Instance.equippedChestpieceSec;
                                TeamGearManager.Instance.equippedBootsMain = TeamGearManager.Instance.equippedBootsSec;

                                wornGearSecondAlly.Clear();

                                TeamGearManager.Instance.equippedHelmetSec = null;
                                TeamGearManager.Instance.equippedChestpieceSec = null;
                                TeamGearManager.Instance.equippedBootsSec = null;

                                wornGearSecondAlly.Clear();

                                // 3 to 2
                                for (int t = 0; t < wornGearThirdAlly.Count; t++)
                                {
                                    wornGearSecondAlly.Add(wornGearThirdAlly[t]);
                                }

                                TeamGearManager.Instance.equippedHelmetSec = TeamGearManager.Instance.equippedHelmetThi;
                                TeamGearManager.Instance.equippedChestpieceSec = TeamGearManager.Instance.equippedChestpieceThi;
                                TeamGearManager.Instance.equippedBootsSec = TeamGearManager.Instance.equippedBootsThi;

                                wornGearThirdAlly.Clear();

                                TeamGearManager.Instance.equippedHelmetThi = null;
                                TeamGearManager.Instance.equippedChestpieceThi = null;
                                TeamGearManager.Instance.equippedBootsThi = null;

                                bool doneHelm = false;
                                bool doneChest = false;
                                bool doneBoots = false;

                                for (int l = 0; l < wornGearMainAlly.Count; l++)
                                {
                                    if (TeamGearManager.Instance.equippedHelmetMain != null && !doneHelm)
                                    {
                                        doneHelm = true;

                                        TeamGearManager.Instance.mainFighterGearSlots[0].isEmpty = false;
                                        TeamGearManager.Instance.mainFighterGearSlots[0].UpdateSlotName(TeamGearManager.Instance.equippedHelmetMain.gearName);
                                        TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusHealth(TeamGearManager.Instance.equippedHelmetMain.bonusHealth);
                                        TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusHealing(TeamGearManager.Instance.equippedHelmetMain.bonusHealing);
                                        TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusDefense(TeamGearManager.Instance.equippedHelmetMain.bonusDefense);
                                        TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusDamage(TeamGearManager.Instance.equippedHelmetMain.bonusDamage);
                                        TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedHelmetMain.bonusSpeed);
                                    }
                                    else if (TeamGearManager.Instance.equippedChestpieceMain != null && !doneChest)
                                    {
                                        doneChest = true;

                                        TeamGearManager.Instance.mainFighterGearSlots[1].isEmpty = false;
                                        TeamGearManager.Instance.mainFighterGearSlots[1].UpdateSlotName(TeamGearManager.Instance.equippedChestpieceMain.gearName);
                                        TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusHealth(TeamGearManager.Instance.equippedChestpieceMain.bonusHealth);
                                        TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusHealing(TeamGearManager.Instance.equippedChestpieceMain.bonusHealing);
                                        TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusDefense(TeamGearManager.Instance.equippedChestpieceMain.bonusDefense);
                                        TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusDamage(TeamGearManager.Instance.equippedChestpieceMain.bonusDamage);
                                        TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedChestpieceMain.bonusSpeed);
                                    }
                                    else if (TeamGearManager.Instance.equippedBootsMain != null && !doneBoots)
                                    {
                                        doneBoots = true;

                                        TeamGearManager.Instance.mainFighterGearSlots[2].isEmpty = false;
                                        TeamGearManager.Instance.mainFighterGearSlots[2].UpdateSlotName(TeamGearManager.Instance.equippedBootsMain.gearName);
                                        TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusHealth(TeamGearManager.Instance.equippedBootsMain.bonusHealth);
                                        TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusHealing(TeamGearManager.Instance.equippedBootsMain.bonusHealing);
                                        TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusDefense(TeamGearManager.Instance.equippedBootsMain.bonusDefense);
                                        TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusDamage(TeamGearManager.Instance.equippedBootsMain.bonusDamage);
                                        TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedBootsMain.bonusSpeed);
                                    }
                                }

                                bool doneHelm2 = false;
                                bool doneChest2 = false;
                                bool doneBoots2 = false;

                                for (int r = 0; r < wornGearSecondAlly.Count; r++)
                                {
                                    if (TeamGearManager.Instance.equippedHelmetSec != null && !doneHelm2)
                                    {
                                        doneHelm2 = true;

                                        TeamGearManager.Instance.ally2GearSlots[0].isEmpty = false;
                                        TeamGearManager.Instance.ally2GearSlots[0].UpdateSlotName(TeamGearManager.Instance.equippedHelmetSec.gearName);
                                        TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusHealth(TeamGearManager.Instance.equippedHelmetSec.bonusHealth);
                                        TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusHealing(TeamGearManager.Instance.equippedHelmetSec.bonusHealing);
                                        TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusDefense(TeamGearManager.Instance.equippedHelmetSec.bonusDefense);
                                        TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusDamage(TeamGearManager.Instance.equippedHelmetSec.bonusDamage);
                                        TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedHelmetSec.bonusSpeed);
                                    }

                                    else if (TeamGearManager.Instance.equippedChestpieceSec != null && !doneChest2)
                                    {
                                        doneChest2 = true;

                                        TeamGearManager.Instance.ally2GearSlots[1].isEmpty = false;
                                        TeamGearManager.Instance.ally2GearSlots[1].UpdateSlotName(TeamGearManager.Instance.equippedChestpieceSec.gearName);
                                        TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusHealth(TeamGearManager.Instance.equippedChestpieceSec.bonusHealth);
                                        TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusHealing(TeamGearManager.Instance.equippedChestpieceSec.bonusHealing);
                                        TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusDefense(TeamGearManager.Instance.equippedChestpieceSec.bonusDefense);
                                        TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusDamage(TeamGearManager.Instance.equippedChestpieceSec.bonusDamage);
                                        TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedChestpieceSec.bonusSpeed);
                                    }
                                    else if (TeamGearManager.Instance.equippedBootsSec != null && !doneBoots2)
                                    {
                                        doneBoots2 = true;

                                        TeamGearManager.Instance.ally2GearSlots[2].isEmpty = false;
                                        TeamGearManager.Instance.ally2GearSlots[2].UpdateSlotName(TeamGearManager.Instance.equippedBootsSec.gearName);
                                        TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusHealth(TeamGearManager.Instance.equippedBootsSec.bonusHealth);
                                        TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusHealing(TeamGearManager.Instance.equippedBootsSec.bonusHealing);
                                        TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusDefense(TeamGearManager.Instance.equippedBootsSec.bonusDefense);
                                        TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusDamage(TeamGearManager.Instance.equippedBootsSec.bonusDamage);
                                        TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedBootsSec.bonusSpeed);
                                    }
                                }

                                bool doneHelm3 = false;
                                bool doneChest3 = false;
                                bool doneBoots3 = false;

                                for (int e = 0; e < wornGearThirdAlly.Count; e++)
                                {
                                    if (TeamGearManager.Instance.equippedHelmetThi != null && !doneHelm3)
                                    {
                                        doneHelm3 = true;

                                        TeamGearManager.Instance.ally3GearSlots[0].isEmpty = false;
                                        TeamGearManager.Instance.ally3GearSlots[0].UpdateSlotName(TeamGearManager.Instance.equippedHelmetThi.gearName);
                                        TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusHealth(TeamGearManager.Instance.equippedHelmetThi.bonusHealth);
                                        TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusHealing(TeamGearManager.Instance.equippedHelmetThi.bonusHealing);
                                        TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusDefense(TeamGearManager.Instance.equippedHelmetThi.bonusDefense);
                                        TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusDamage(TeamGearManager.Instance.equippedHelmetThi.bonusDamage);
                                        TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedHelmetThi.bonusSpeed);
                                    }

                                    else if (TeamGearManager.Instance.equippedChestpieceThi != null && !doneChest3)
                                    {
                                        doneChest3 = true;

                                        TeamGearManager.Instance.ally3GearSlots[1].isEmpty = false;
                                        TeamGearManager.Instance.ally3GearSlots[1].UpdateSlotName(TeamGearManager.Instance.equippedChestpieceThi.gearName);
                                        TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusHealth(TeamGearManager.Instance.equippedChestpieceThi.bonusHealth);
                                        TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusHealing(TeamGearManager.Instance.equippedChestpieceThi.bonusHealing);
                                        TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusDefense(TeamGearManager.Instance.equippedChestpieceThi.bonusDefense);
                                        TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusDamage(TeamGearManager.Instance.equippedChestpieceThi.bonusDamage);
                                        TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedChestpieceThi.bonusSpeed);
                                    }
                                    else if (TeamGearManager.Instance.equippedBootsThi != null && !doneBoots3)
                                    {
                                        doneBoots3 = true;

                                        TeamGearManager.Instance.ally3GearSlots[2].isEmpty = false;
                                        TeamGearManager.Instance.ally3GearSlots[2].UpdateSlotName(TeamGearManager.Instance.equippedBootsThi.gearName);
                                        TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusHealth(TeamGearManager.Instance.equippedBootsThi.bonusHealth);
                                        TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusHealing(TeamGearManager.Instance.equippedBootsThi.bonusHealing);
                                        TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusDefense(TeamGearManager.Instance.equippedBootsThi.bonusDefense);
                                        TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusDamage(TeamGearManager.Instance.equippedBootsThi.bonusDamage);
                                        TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedBootsThi.bonusSpeed);
                                    }
                                }

                                #endregion

                                #region Items Main

                                // 2 to 1
                                wornItemsMainAlly.Clear();

                                for (int t = 0; t < wornItemsSecondAlly.Count; t++)
                                {
                                    wornItemsMainAlly.Add(wornItemsSecondAlly[t]);
                                }

                                wornItemsSecondAlly.Clear();
                                TeamItemsManager.Instance.equippedItemsMain.Clear();

                                for (int h = 0; h < TeamItemsManager.Instance.equippedItemsSecond.Count; h++)
                                {
                                    TeamItemsManager.Instance.equippedItemsMain.Add(TeamItemsManager.Instance.equippedItemsSecond[h]);
                                }

                                TeamItemsManager.Instance.equippedItemsSecond.Clear();


                                // 3 to 2
                                wornItemsSecondAlly.Clear();

                                for (int t = 0; t < wornItemsThirdAlly.Count; t++)
                                {
                                    wornItemsSecondAlly.Add(wornItemsThirdAlly[t]);
                                }

                                wornItemsThirdAlly.Clear();

                                TeamItemsManager.Instance.equippedItemsSecond.Clear();

                                for (int h = 0; h < TeamItemsManager.Instance.equippedItemsThird.Count; h++)
                                {
                                    TeamItemsManager.Instance.equippedItemsSecond.Add(TeamItemsManager.Instance.equippedItemsThird[h]);
                                }

                                TeamItemsManager.Instance.equippedItemsThird.Clear();

                                bool done1st = false;
                                bool done2nd = false;
                                bool done3rd = false;

                                for (int l = 0; l < wornItemsMainAlly.Count; l++)
                                {
                                    if (TeamItemsManager.Instance.equippedItemsMain.Count == 1 && !done1st)
                                    {
                                        done1st = true;

                                        TeamItemsManager.Instance.ally1ItemsSlots[0].isEmpty = false;
                                        TeamItemsManager.Instance.ally1ItemsSlots[0].UpdateSlotName(TeamItemsManager.Instance.equippedItemsMain[0].itemName);
                                    }
                                    else if (TeamItemsManager.Instance.equippedItemsMain.Count == 2 && !done2nd)
                                    {
                                        done2nd = true;

                                        TeamItemsManager.Instance.ally1ItemsSlots[1].isEmpty = false;
                                        TeamItemsManager.Instance.ally1ItemsSlots[1].UpdateSlotName(TeamItemsManager.Instance.equippedItemsMain[1].itemName);
                                    }
                                    else if (TeamItemsManager.Instance.equippedItemsMain.Count == 3 && !done3rd)
                                    {
                                        done3rd = true;

                                        TeamItemsManager.Instance.ally1ItemsSlots[2].isEmpty = false;
                                        TeamItemsManager.Instance.ally1ItemsSlots[2].UpdateSlotName(TeamItemsManager.Instance.equippedItemsMain[2].itemName);
                                    }
                                }

                                for (int l = 0; l < wornItemsSecondAlly.Count; l++)
                                {
                                    if (TeamItemsManager.Instance.equippedItemsSecond.Count == 1 && !done1st)
                                    {
                                        done1st = true;

                                        TeamItemsManager.Instance.ally2ItemsSlots[0].isEmpty = false;
                                        TeamItemsManager.Instance.ally2ItemsSlots[0].UpdateSlotName(TeamItemsManager.Instance.equippedItemsSecond[0].itemName);
                                    }
                                    else if (TeamItemsManager.Instance.equippedItemsSecond.Count == 2 && !done2nd)
                                    {
                                        done2nd = true;

                                        TeamItemsManager.Instance.ally2ItemsSlots[1].isEmpty = false;
                                        TeamItemsManager.Instance.ally2ItemsSlots[1].UpdateSlotName(TeamItemsManager.Instance.equippedItemsSecond[1].itemName);
                                    }
                                    else if (TeamItemsManager.Instance.equippedItemsSecond.Count == 3 && !done3rd)
                                    {
                                        done3rd = true;

                                        TeamItemsManager.Instance.ally2ItemsSlots[2].isEmpty = false;
                                        TeamItemsManager.Instance.ally2ItemsSlots[2].UpdateSlotName(TeamItemsManager.Instance.equippedItemsSecond[2].itemName);
                                    }
                                }

                                for (int l = 0; l < wornItemsThirdAlly.Count; l++)
                                {
                                    if (TeamItemsManager.Instance.equippedItemsThird.Count == 1 && !done1st)
                                    {
                                        done1st = true;

                                        TeamItemsManager.Instance.ally3ItemsSlots[0].isEmpty = false;
                                        TeamItemsManager.Instance.ally3ItemsSlots[0].UpdateSlotName(TeamItemsManager.Instance.equippedItemsThird[0].itemName);
                                    }
                                    else if (TeamItemsManager.Instance.equippedItemsThird.Count == 2 && !done2nd)
                                    {
                                        done2nd = true;

                                        TeamItemsManager.Instance.ally3ItemsSlots[1].isEmpty = false;
                                        TeamItemsManager.Instance.ally3ItemsSlots[1].UpdateSlotName(TeamItemsManager.Instance.equippedItemsThird[1].itemName);
                                    }
                                    else if (TeamItemsManager.Instance.equippedItemsThird.Count == 3 && !done3rd)
                                    {
                                        done3rd = true;

                                        TeamItemsManager.Instance.ally3ItemsSlots[2].isEmpty = false;
                                        TeamItemsManager.Instance.ally3ItemsSlots[2].UpdateSlotName(TeamItemsManager.Instance.equippedItemsThird[2].itemName);
                                    }
                                }
                                #endregion
                                continue;
                            }
                        }
                        else if (x == 1)
                        {
                            #region Gear 2nd
                            // lost a 2nd hero, put 3 to 2
                            wornGearSecondAlly.Clear();

                            // 3 to 2
                            for (int t = 0; t < wornGearThirdAlly.Count; t++)
                            {
                                wornGearSecondAlly.Add(wornGearThirdAlly[t]);
                            }

                            TeamGearManager.Instance.equippedHelmetSec = TeamGearManager.Instance.equippedHelmetThi;
                            TeamGearManager.Instance.equippedChestpieceSec = TeamGearManager.Instance.equippedChestpieceThi;
                            TeamGearManager.Instance.equippedBootsSec = TeamGearManager.Instance.equippedBootsThi;

                            wornGearThirdAlly.Clear();

                            TeamGearManager.Instance.equippedHelmetThi = null;
                            TeamGearManager.Instance.equippedChestpieceThi = null;
                            TeamGearManager.Instance.equippedBootsThi = null;

                            bool doneHelm4 = false;
                            bool doneChest4 = false;
                            bool doneBoots4 = false;

                            for (int l = 0; l < wornGearMainAlly.Count; l++)
                            {
                                if (TeamGearManager.Instance.equippedHelmetMain != null && !doneHelm4)
                                {
                                    doneHelm4 = true;

                                    TeamGearManager.Instance.mainFighterGearSlots[0].isEmpty = false;
                                    TeamGearManager.Instance.mainFighterGearSlots[0].UpdateSlotName(TeamGearManager.Instance.equippedHelmetMain.gearName);
                                    TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusHealth(TeamGearManager.Instance.equippedHelmetMain.bonusHealth);
                                    TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusHealing(TeamGearManager.Instance.equippedHelmetMain.bonusHealing);
                                    TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusDefense(TeamGearManager.Instance.equippedHelmetMain.bonusDefense);
                                    TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusDamage(TeamGearManager.Instance.equippedHelmetMain.bonusDamage);
                                    TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedHelmetMain.bonusSpeed);
                                }

                                else if (TeamGearManager.Instance.equippedChestpieceMain != null && !doneChest4)
                                {
                                    doneChest4 = true;

                                    TeamGearManager.Instance.mainFighterGearSlots[1].isEmpty = false;
                                    TeamGearManager.Instance.mainFighterGearSlots[1].UpdateSlotName(TeamGearManager.Instance.equippedChestpieceMain.gearName);
                                    TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusHealth(TeamGearManager.Instance.equippedChestpieceMain.bonusHealth);
                                    TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusHealing(TeamGearManager.Instance.equippedChestpieceMain.bonusHealing);
                                    TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusDefense(TeamGearManager.Instance.equippedChestpieceMain.bonusDefense);
                                    TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusDamage(TeamGearManager.Instance.equippedChestpieceMain.bonusDamage);
                                    TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedChestpieceMain.bonusSpeed);
                                }
                                else if (TeamGearManager.Instance.equippedBootsMain != null && !doneBoots4)
                                {
                                    doneBoots4 = true;

                                    TeamGearManager.Instance.mainFighterGearSlots[2].isEmpty = false;
                                    TeamGearManager.Instance.mainFighterGearSlots[2].UpdateSlotName(TeamGearManager.Instance.equippedBootsMain.gearName);
                                    TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusHealth(TeamGearManager.Instance.equippedBootsMain.bonusHealth);
                                    TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusHealing(TeamGearManager.Instance.equippedBootsMain.bonusHealing);
                                    TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusDefense(TeamGearManager.Instance.equippedBootsMain.bonusDefense);
                                    TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusDamage(TeamGearManager.Instance.equippedBootsMain.bonusDamage);
                                    TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedBootsMain.bonusSpeed);
                                }
                            }

                            bool doneHelm5 = false;
                            bool doneChest5 = false;
                            bool doneBoots5 = false;

                            for (int l = 0; l < wornGearSecondAlly.Count; l++)
                            {
                                if (TeamGearManager.Instance.equippedHelmetSec != null && !doneHelm5)
                                {
                                    doneHelm5 = true;

                                    TeamGearManager.Instance.ally2GearSlots[0].isEmpty = false;
                                    TeamGearManager.Instance.ally2GearSlots[0].UpdateSlotName(TeamGearManager.Instance.equippedHelmetSec.gearName);
                                    TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusHealth(TeamGearManager.Instance.equippedHelmetSec.bonusHealth);
                                    TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusHealing(TeamGearManager.Instance.equippedHelmetSec.bonusHealing);
                                    TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusDefense(TeamGearManager.Instance.equippedHelmetSec.bonusDefense);
                                    TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusDamage(TeamGearManager.Instance.equippedHelmetSec.bonusDamage);
                                    TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedHelmetSec.bonusSpeed);
                                }

                                else if (TeamGearManager.Instance.equippedChestpieceSec != null && !doneChest5)
                                {
                                    doneChest5 = true;

                                    TeamGearManager.Instance.ally2GearSlots[1].isEmpty = false;
                                    TeamGearManager.Instance.ally2GearSlots[1].UpdateSlotName(TeamGearManager.Instance.equippedChestpieceSec.gearName);
                                    TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusHealth(TeamGearManager.Instance.equippedChestpieceSec.bonusHealth);
                                    TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusHealing(TeamGearManager.Instance.equippedChestpieceSec.bonusHealing);
                                    TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusDefense(TeamGearManager.Instance.equippedChestpieceSec.bonusDefense);
                                    TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusDamage(TeamGearManager.Instance.equippedChestpieceSec.bonusDamage);
                                    TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedChestpieceSec.bonusSpeed);
                                }
                                else if (TeamGearManager.Instance.equippedBootsSec != null && !doneBoots5)
                                {
                                    doneBoots5 = true;

                                    TeamGearManager.Instance.ally2GearSlots[2].isEmpty = false;
                                    TeamGearManager.Instance.ally2GearSlots[2].UpdateSlotName(TeamGearManager.Instance.equippedBootsSec.gearName);
                                    TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusHealth(TeamGearManager.Instance.equippedBootsSec.bonusHealth);
                                    TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusHealing(TeamGearManager.Instance.equippedBootsSec.bonusHealing);
                                    TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusDefense(TeamGearManager.Instance.equippedBootsSec.bonusDefense);
                                    TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusDamage(TeamGearManager.Instance.equippedBootsSec.bonusDamage);
                                    TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedBootsSec.bonusSpeed);
                                }
                            }

                            bool doneHelm6 = false;
                            bool doneChest6 = false;
                            bool doneBoots6 = false;

                            for (int l = 0; l < wornGearThirdAlly.Count; l++)
                            {
                                if (TeamGearManager.Instance.equippedHelmetThi != null && !doneHelm6)
                                {
                                    doneHelm6 = true;

                                    TeamGearManager.Instance.ally3GearSlots[0].isEmpty = false;
                                    TeamGearManager.Instance.ally3GearSlots[0].UpdateSlotName(TeamGearManager.Instance.equippedHelmetThi.gearName);
                                    TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusHealth(TeamGearManager.Instance.equippedHelmetThi.bonusHealth);
                                    TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusHealing(TeamGearManager.Instance.equippedHelmetThi.bonusHealing);
                                    TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusDefense(TeamGearManager.Instance.equippedHelmetThi.bonusDefense);
                                    TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusDamage(TeamGearManager.Instance.equippedHelmetThi.bonusDamage);
                                    TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedHelmetThi.bonusSpeed);
                                }

                                else if (TeamGearManager.Instance.equippedChestpieceThi != null && !doneChest6)
                                {
                                    doneChest6 = true;

                                    TeamGearManager.Instance.ally3GearSlots[1].isEmpty = false;
                                    TeamGearManager.Instance.ally3GearSlots[1].UpdateSlotName(TeamGearManager.Instance.equippedChestpieceThi.gearName);
                                    TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusHealth(TeamGearManager.Instance.equippedChestpieceThi.bonusHealth);
                                    TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusHealing(TeamGearManager.Instance.equippedChestpieceThi.bonusHealing);
                                    TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusDefense(TeamGearManager.Instance.equippedChestpieceThi.bonusDefense);
                                    TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusDamage(TeamGearManager.Instance.equippedChestpieceThi.bonusDamage);
                                    TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedChestpieceThi.bonusSpeed);
                                }
                                else if (TeamGearManager.Instance.equippedBootsThi != null && !doneBoots6)
                                {
                                    doneBoots6 = true;

                                    TeamGearManager.Instance.ally3GearSlots[2].isEmpty = false;
                                    TeamGearManager.Instance.ally3GearSlots[2].UpdateSlotName(TeamGearManager.Instance.equippedBootsThi.gearName);
                                    TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusHealth(TeamGearManager.Instance.equippedBootsThi.bonusHealth);
                                    TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusHealing(TeamGearManager.Instance.equippedBootsThi.bonusHealing);
                                    TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusDefense(TeamGearManager.Instance.equippedBootsThi.bonusDefense);
                                    TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusDamage(TeamGearManager.Instance.equippedBootsThi.bonusDamage);
                                    TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedBootsThi.bonusSpeed);
                                }
                            }

                            #endregion;
                            #region Items 2nd

                            // 3 to 2
                            wornItemsSecondAlly.Clear();

                            for (int t = 0; t < wornItemsThirdAlly.Count; t++)
                            {
                                wornItemsSecondAlly.Add(wornItemsThirdAlly[t]);
                            }

                            wornItemsThirdAlly.Clear();

                            TeamItemsManager.Instance.equippedItemsSecond.Clear();

                            for (int h = 0; h < TeamItemsManager.Instance.equippedItemsThird.Count; h++)
                            {
                                TeamItemsManager.Instance.equippedItemsSecond.Add(TeamItemsManager.Instance.equippedItemsThird[h]);
                            }

                            TeamItemsManager.Instance.equippedItemsThird.Clear();

                            bool done1st = false;
                            bool done2nd = false;
                            bool done3rd = false;

                            for (int l = 0; l < wornItemsMainAlly.Count; l++)
                            {
                                if (TeamItemsManager.Instance.equippedItemsMain.Count == 1 && !done1st)
                                {
                                    done1st = true;

                                    TeamItemsManager.Instance.ally1ItemsSlots[0].isEmpty = false;
                                    TeamItemsManager.Instance.ally1ItemsSlots[0].UpdateSlotName(TeamItemsManager.Instance.equippedItemsMain[0].itemName);
                                }
                                else if (TeamItemsManager.Instance.equippedItemsMain.Count == 2 && !done2nd)
                                {
                                    done2nd = true;

                                    TeamItemsManager.Instance.ally1ItemsSlots[1].isEmpty = false;
                                    TeamItemsManager.Instance.ally1ItemsSlots[1].UpdateSlotName(TeamItemsManager.Instance.equippedItemsMain[1].itemName);
                                }
                                else if (TeamItemsManager.Instance.equippedItemsMain.Count == 3 && !done3rd)
                                {
                                    done3rd = true;

                                    TeamItemsManager.Instance.ally1ItemsSlots[2].isEmpty = false;
                                    TeamItemsManager.Instance.ally1ItemsSlots[2].UpdateSlotName(TeamItemsManager.Instance.equippedItemsMain[2].itemName);
                                }
                            }

                            for (int l = 0; l < wornItemsSecondAlly.Count; l++)
                            {
                                if (TeamItemsManager.Instance.equippedItemsSecond.Count == 1 && !done1st)
                                {
                                    done1st = true;

                                    TeamItemsManager.Instance.ally2ItemsSlots[0].isEmpty = false;
                                    TeamItemsManager.Instance.ally2ItemsSlots[0].UpdateSlotName(TeamItemsManager.Instance.equippedItemsSecond[0].itemName);
                                }
                                else if (TeamItemsManager.Instance.equippedItemsSecond.Count == 2 && !done2nd)
                                {
                                    done2nd = true;

                                    TeamItemsManager.Instance.ally2ItemsSlots[1].isEmpty = false;
                                    TeamItemsManager.Instance.ally2ItemsSlots[1].UpdateSlotName(TeamItemsManager.Instance.equippedItemsSecond[1].itemName);
                                }
                                else if (TeamItemsManager.Instance.equippedItemsSecond.Count == 3 && !done3rd)
                                {
                                    done3rd = true;

                                    TeamItemsManager.Instance.ally2ItemsSlots[2].isEmpty = false;
                                    TeamItemsManager.Instance.ally2ItemsSlots[2].UpdateSlotName(TeamItemsManager.Instance.equippedItemsSecond[2].itemName);
                                }
                            }

                            for (int l = 0; l < wornItemsThirdAlly.Count; l++)
                            {
                                if (TeamItemsManager.Instance.equippedItemsThird.Count == 1 && !done1st)
                                {
                                    done1st = true;

                                    TeamItemsManager.Instance.ally3ItemsSlots[0].isEmpty = false;
                                    TeamItemsManager.Instance.ally3ItemsSlots[0].UpdateSlotName(TeamItemsManager.Instance.equippedItemsThird[0].itemName);
                                }
                                else if (TeamItemsManager.Instance.equippedItemsThird.Count == 2 && !done2nd)
                                {
                                    done2nd = true;

                                    TeamItemsManager.Instance.ally3ItemsSlots[1].isEmpty = false;
                                    TeamItemsManager.Instance.ally3ItemsSlots[1].UpdateSlotName(TeamItemsManager.Instance.equippedItemsThird[1].itemName);
                                }
                                else if (TeamItemsManager.Instance.equippedItemsThird.Count == 3 && !done3rd)
                                {
                                    done3rd = true;

                                    TeamItemsManager.Instance.ally3ItemsSlots[2].isEmpty = false;
                                    TeamItemsManager.Instance.ally3ItemsSlots[2].UpdateSlotName(TeamItemsManager.Instance.equippedItemsThird[2].itemName);
                                }
                            }
                            #endregion
                        }
                        else if (x == 2)
                        {
                            #region Gear 3rd
                            wornGearThirdAlly.Clear();

                            TeamGearManager.Instance.equippedHelmetThi = null;
                            TeamGearManager.Instance.equippedChestpieceThi = null;
                            TeamGearManager.Instance.equippedBootsThi = null;

                            // lost a 3rd hero, put 2 to 1, and 3 to 2
                            bool doneHelm7 = false;
                            bool doneChest7 = false;
                            bool doneBoots7 = false;

                            for (int l = 0; l < wornGearMainAlly.Count; l++)
                            {
                                if (TeamGearManager.Instance.equippedHelmetMain != null && !doneHelm7)
                                {
                                    doneHelm7 = true;

                                    TeamGearManager.Instance.mainFighterGearSlots[0].isEmpty = false;
                                    TeamGearManager.Instance.mainFighterGearSlots[0].UpdateSlotName(TeamGearManager.Instance.equippedHelmetMain.gearName);
                                    TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusHealth(TeamGearManager.Instance.equippedHelmetMain.bonusHealth);
                                    TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusHealing(TeamGearManager.Instance.equippedHelmetMain.bonusHealing);
                                    TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusDefense(TeamGearManager.Instance.equippedHelmetMain.bonusDefense);
                                    TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusDamage(TeamGearManager.Instance.equippedHelmetMain.bonusDamage);
                                    TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedHelmetMain.bonusSpeed);
                                }

                                else if (TeamGearManager.Instance.equippedChestpieceMain != null && !doneChest7)
                                {
                                    doneChest7 = true;

                                    TeamGearManager.Instance.mainFighterGearSlots[1].isEmpty = false;
                                    TeamGearManager.Instance.mainFighterGearSlots[1].UpdateSlotName(TeamGearManager.Instance.equippedChestpieceMain.gearName);
                                    TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusHealth(TeamGearManager.Instance.equippedChestpieceMain.bonusHealth);
                                    TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusHealing(TeamGearManager.Instance.equippedChestpieceMain.bonusHealing);
                                    TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusDefense(TeamGearManager.Instance.equippedChestpieceMain.bonusDefense);
                                    TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusDamage(TeamGearManager.Instance.equippedChestpieceMain.bonusDamage);
                                    TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedChestpieceMain.bonusSpeed);
                                }
                                else if (TeamGearManager.Instance.equippedBootsMain != null && !doneBoots7)
                                {
                                    doneBoots7 = true;

                                    TeamGearManager.Instance.mainFighterGearSlots[2].isEmpty = false;
                                    TeamGearManager.Instance.mainFighterGearSlots[2].UpdateSlotName(TeamGearManager.Instance.equippedBootsMain.gearName);
                                    TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusHealth(TeamGearManager.Instance.equippedBootsMain.bonusHealth);
                                    TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusHealing(TeamGearManager.Instance.equippedBootsMain.bonusHealing);
                                    TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusDefense(TeamGearManager.Instance.equippedBootsMain.bonusDefense);
                                    TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusDamage(TeamGearManager.Instance.equippedBootsMain.bonusDamage);
                                    TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedBootsMain.bonusSpeed);
                                }
                            }

                            bool doneHelm8 = false;
                            bool doneChest8 = false;
                            bool doneBoots8 = false;

                            for (int l = 0; l < wornGearSecondAlly.Count; l++)
                            {
                                if (TeamGearManager.Instance.equippedHelmetSec != null && !doneHelm8)
                                {
                                    doneHelm8 = true;

                                    TeamGearManager.Instance.ally2GearSlots[0].isEmpty = false;
                                    TeamGearManager.Instance.ally2GearSlots[0].UpdateSlotName(TeamGearManager.Instance.equippedHelmetSec.gearName);
                                    TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusHealth(TeamGearManager.Instance.equippedHelmetSec.bonusHealth);
                                    TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusHealing(TeamGearManager.Instance.equippedHelmetSec.bonusHealing);
                                    TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusDefense(TeamGearManager.Instance.equippedHelmetSec.bonusDefense);
                                    TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusDamage(TeamGearManager.Instance.equippedHelmetSec.bonusDamage);
                                    TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedHelmetSec.bonusSpeed);
                                }

                                else if (TeamGearManager.Instance.equippedChestpieceSec != null && !doneChest8)
                                {
                                    doneChest8 = true;

                                    TeamGearManager.Instance.ally2GearSlots[1].isEmpty = false;
                                    TeamGearManager.Instance.ally2GearSlots[1].UpdateSlotName(TeamGearManager.Instance.equippedChestpieceSec.gearName);
                                    TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusHealth(TeamGearManager.Instance.equippedChestpieceSec.bonusHealth);
                                    TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusHealing(TeamGearManager.Instance.equippedChestpieceSec.bonusHealing);
                                    TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusDefense(TeamGearManager.Instance.equippedChestpieceSec.bonusDefense);
                                    TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusDamage(TeamGearManager.Instance.equippedChestpieceSec.bonusDamage);
                                    TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedChestpieceSec.bonusSpeed);
                                }
                                else if (TeamGearManager.Instance.equippedBootsSec != null && !doneBoots8)
                                {
                                    doneBoots8 = true;

                                    TeamGearManager.Instance.ally2GearSlots[2].isEmpty = false;
                                    TeamGearManager.Instance.ally2GearSlots[2].UpdateSlotName(TeamGearManager.Instance.equippedBootsSec.gearName);
                                    TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusHealth(TeamGearManager.Instance.equippedBootsSec.bonusHealth);
                                    TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusHealing(TeamGearManager.Instance.equippedBootsSec.bonusHealing);
                                    TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusDefense(TeamGearManager.Instance.equippedBootsSec.bonusDefense);
                                    TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusDamage(TeamGearManager.Instance.equippedBootsSec.bonusDamage);
                                    TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedBootsSec.bonusSpeed);
                                }
                            }

                            bool doneHelm9 = false;
                            bool doneChest9 = false;
                            bool doneBoots9 = false;

                            for (int l = 0; l < wornGearThirdAlly.Count; l++)
                            {
                                if (TeamGearManager.Instance.equippedHelmetThi != null && !doneHelm9)
                                {
                                    doneHelm9 = true;

                                    TeamGearManager.Instance.ally3GearSlots[0].isEmpty = false;
                                    TeamGearManager.Instance.ally3GearSlots[0].UpdateSlotName(TeamGearManager.Instance.equippedHelmetThi.gearName);
                                    TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusHealth(TeamGearManager.Instance.equippedHelmetThi.bonusHealth);
                                    TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusHealing(TeamGearManager.Instance.equippedHelmetThi.bonusHealing);
                                    TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusDefense(TeamGearManager.Instance.equippedHelmetThi.bonusDefense);
                                    TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusDamage(TeamGearManager.Instance.equippedHelmetThi.bonusDamage);
                                    TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedHelmetThi.bonusSpeed);
                                }

                                else if (TeamGearManager.Instance.equippedChestpieceThi != null && !doneChest9)
                                {
                                    doneChest9 = true;

                                    TeamGearManager.Instance.ally3GearSlots[1].isEmpty = false;
                                    TeamGearManager.Instance.ally3GearSlots[1].UpdateSlotName(TeamGearManager.Instance.equippedChestpieceThi.gearName);
                                    TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusHealth(TeamGearManager.Instance.equippedChestpieceThi.bonusHealth);
                                    TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusHealing(TeamGearManager.Instance.equippedChestpieceThi.bonusHealing);
                                    TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusDefense(TeamGearManager.Instance.equippedChestpieceThi.bonusDefense);
                                    TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusDamage(TeamGearManager.Instance.equippedChestpieceThi.bonusDamage);
                                    TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedChestpieceThi.bonusSpeed);
                                }
                                else if (TeamGearManager.Instance.equippedBootsThi != null && !doneBoots9)
                                {
                                    doneBoots9 = true;

                                    TeamGearManager.Instance.ally3GearSlots[2].isEmpty = false;
                                    TeamGearManager.Instance.ally3GearSlots[2].UpdateSlotName(TeamGearManager.Instance.equippedBootsThi.gearName);
                                    TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusHealth(TeamGearManager.Instance.equippedBootsThi.bonusHealth);
                                    TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusHealing(TeamGearManager.Instance.equippedBootsThi.bonusHealing);
                                    TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusDefense(TeamGearManager.Instance.equippedBootsThi.bonusDefense);
                                    TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusDamage(TeamGearManager.Instance.equippedBootsThi.bonusDamage);
                                    TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedBootsThi.bonusSpeed);
                                }
                            }
                            #endregion;
                            #region Items 3rd

                            wornItemsThirdAlly.Clear();
                            TeamItemsManager.Instance.equippedItemsThird.Clear();

                            bool done1st = false;
                            bool done2nd = false;
                            bool done3rd = false;

                            for (int l = 0; l < wornItemsMainAlly.Count; l++)
                            {
                                if (TeamItemsManager.Instance.equippedItemsMain.Count == 1 && !done1st)
                                {
                                    done1st = true;

                                    TeamItemsManager.Instance.ally1ItemsSlots[0].isEmpty = false;
                                    TeamItemsManager.Instance.ally1ItemsSlots[0].UpdateSlotName(TeamItemsManager.Instance.equippedItemsMain[0].itemName);
                                }
                                else if (TeamItemsManager.Instance.equippedItemsMain.Count == 2 && !done2nd)
                                {
                                    done2nd = true;

                                    TeamItemsManager.Instance.ally1ItemsSlots[1].isEmpty = false;
                                    TeamItemsManager.Instance.ally1ItemsSlots[1].UpdateSlotName(TeamItemsManager.Instance.equippedItemsMain[1].itemName);
                                }
                                else if (TeamItemsManager.Instance.equippedItemsMain.Count == 3 && !done3rd)
                                {
                                    done3rd = true;

                                    TeamItemsManager.Instance.ally1ItemsSlots[2].isEmpty = false;
                                    TeamItemsManager.Instance.ally1ItemsSlots[2].UpdateSlotName(TeamItemsManager.Instance.equippedItemsMain[2].itemName);
                                }
                            }

                            for (int l = 0; l < wornItemsSecondAlly.Count; l++)
                            {
                                if (TeamItemsManager.Instance.equippedItemsSecond.Count == 1 && !done1st)
                                {
                                    done1st = true;

                                    TeamItemsManager.Instance.ally2ItemsSlots[0].isEmpty = false;
                                    TeamItemsManager.Instance.ally2ItemsSlots[0].UpdateSlotName(TeamItemsManager.Instance.equippedItemsSecond[0].itemName);
                                }
                                else if (TeamItemsManager.Instance.equippedItemsSecond.Count == 2 && !done2nd)
                                {
                                    done2nd = true;

                                    TeamItemsManager.Instance.ally2ItemsSlots[1].isEmpty = false;
                                    TeamItemsManager.Instance.ally2ItemsSlots[1].UpdateSlotName(TeamItemsManager.Instance.equippedItemsSecond[1].itemName);
                                }
                                else if (TeamItemsManager.Instance.equippedItemsSecond.Count == 3 && !done3rd)
                                {
                                    done3rd = true;

                                    TeamItemsManager.Instance.ally2ItemsSlots[2].isEmpty = false;
                                    TeamItemsManager.Instance.ally2ItemsSlots[2].UpdateSlotName(TeamItemsManager.Instance.equippedItemsSecond[2].itemName);
                                }
                            }

                            for (int l = 0; l < wornItemsThirdAlly.Count; l++)
                            {
                                if (TeamItemsManager.Instance.equippedItemsThird.Count == 1 && !done1st)
                                {
                                    done1st = true;

                                    TeamItemsManager.Instance.ally3ItemsSlots[0].isEmpty = false;
                                    TeamItemsManager.Instance.ally3ItemsSlots[0].UpdateSlotName(TeamItemsManager.Instance.equippedItemsThird[0].itemName);
                                }
                                else if (TeamItemsManager.Instance.equippedItemsThird.Count == 2 && !done2nd)
                                {
                                    done2nd = true;

                                    TeamItemsManager.Instance.ally3ItemsSlots[1].isEmpty = false;
                                    TeamItemsManager.Instance.ally3ItemsSlots[1].UpdateSlotName(TeamItemsManager.Instance.equippedItemsThird[1].itemName);
                                }
                                else if (TeamItemsManager.Instance.equippedItemsThird.Count == 3 && !done3rd)
                                {
                                    done3rd = true;

                                    TeamItemsManager.Instance.ally3ItemsSlots[2].isEmpty = false;
                                    TeamItemsManager.Instance.ally3ItemsSlots[2].UpdateSlotName(TeamItemsManager.Instance.equippedItemsThird[2].itemName);
                                }
                            }
                            #endregion
                        }

                    }
                    else if (GameManager.Instance.fallenHeroes.Count == 2 && !doOnce)
                    {
                        // Heroes 1 and 2 dying
                        if (GameManager.Instance.fallenHeroes[0].teamIndex == 0 && GameManager.Instance.fallenHeroes[1].teamIndex == 1
                            || GameManager.Instance.fallenHeroes[0].teamIndex == 1 && GameManager.Instance.fallenHeroes[1].teamIndex == 0)
                        {
                            doOnce = true;
                            
                            // Gear
                            
                            // put 3 to 1
                            wornGearMainAlly.Clear();

                            // 3 to 1
                            for (int t = 0; t < wornGearThirdAlly.Count; t++)
                            {
                                wornGearMainAlly.Add(wornGearThirdAlly[t]);
                            }

                            TeamGearManager.Instance.equippedHelmetMain = TeamGearManager.Instance.equippedHelmetThi;
                            TeamGearManager.Instance.equippedChestpieceMain = TeamGearManager.Instance.equippedChestpieceThi;
                            TeamGearManager.Instance.equippedBootsMain = TeamGearManager.Instance.equippedBootsThi;


                            // Items
                            
                            wornItemsMainAlly.Clear();

                            for (int b = 0; b < wornItemsThirdAlly.Count; b++)
                            {
                                wornItemsMainAlly.Add(wornItemsThirdAlly[b]);
                            }

                            wornItemsThirdAlly.Clear();

                            TeamItemsManager.Instance.equippedItemsMain.Clear();

                            for (int h = 0; h < TeamItemsManager.Instance.equippedItemsThird.Count; h++)
                            {
                                TeamItemsManager.Instance.equippedItemsMain.Add(TeamItemsManager.Instance.equippedItemsThird[h]);
                            }

                            TeamItemsManager.Instance.equippedItemsThird.Clear();
                        }
                        // Heroes 1 and 3 dying
                        else if (GameManager.Instance.fallenHeroes[0].teamIndex == 0 && GameManager.Instance.fallenHeroes[1].teamIndex == 2
                            || GameManager.Instance.fallenHeroes[0].teamIndex == 2 && GameManager.Instance.fallenHeroes[1].teamIndex == 0)
                        {                          
                            doOnce = true;

                            // Gear 

                            // put 2 to 1
                            wornGearMainAlly.Clear();

                            // 2 to 1
                            for (int t = 0; t < wornGearSecondAlly.Count; t++)
                            {
                                wornGearMainAlly.Add(wornGearSecondAlly[t]);
                            }

                            TeamGearManager.Instance.equippedHelmetMain = TeamGearManager.Instance.equippedHelmetSec;
                            TeamGearManager.Instance.equippedChestpieceMain = TeamGearManager.Instance.equippedChestpieceSec;
                            TeamGearManager.Instance.equippedBootsMain = TeamGearManager.Instance.equippedBootsSec;


                            // Items

                            wornItemsMainAlly.Clear();

                            for (int b = 0; b < wornItemsSecondAlly.Count; b++)
                            {
                                wornItemsMainAlly.Add(wornItemsSecondAlly[b]);
                            }

                            wornItemsSecondAlly.Clear();

                            TeamItemsManager.Instance.equippedItemsMain.Clear();

                            for (int h = 0; h < TeamItemsManager.Instance.equippedItemsSecond.Count; h++)
                            {
                                TeamItemsManager.Instance.equippedItemsMain.Add(TeamItemsManager.Instance.equippedItemsSecond[h]);
                            }

                            TeamItemsManager.Instance.equippedItemsSecond.Clear();
                        }

                        #region Gear

                        // Remove unit 2 gear
                        wornGearSecondAlly.Clear();

                        TeamGearManager.Instance.equippedHelmetSec = null;
                        TeamGearManager.Instance.equippedChestpieceSec = null;
                        TeamGearManager.Instance.equippedBootsSec = null;

                        // Remove unit 3 gear
                        wornGearThirdAlly.Clear();

                        TeamGearManager.Instance.equippedHelmetThi = null;
                        TeamGearManager.Instance.equippedChestpieceThi = null;
                        TeamGearManager.Instance.equippedBootsThi = null;


                        // lost a 3rd hero, put 2 to 1, and 3 to 2
                        bool doneHelm7 = false;
                        bool doneChest7 = false;
                        bool doneBoots7 = false;

                        // Update gear tab slot visuals
                        for (int l = 0; l < wornGearMainAlly.Count; l++)
                        {
                            if (TeamGearManager.Instance.equippedHelmetMain != null && !doneHelm7)
                            {
                                doneHelm7 = true;

                                TeamGearManager.Instance.mainFighterGearSlots[0].isEmpty = false;
                                TeamGearManager.Instance.mainFighterGearSlots[0].UpdateSlotName(TeamGearManager.Instance.equippedHelmetMain.gearName);
                                TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusHealth(TeamGearManager.Instance.equippedHelmetMain.bonusHealth);
                                TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusHealing(TeamGearManager.Instance.equippedHelmetMain.bonusHealing);
                                TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusDefense(TeamGearManager.Instance.equippedHelmetMain.bonusDefense);
                                TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusDamage(TeamGearManager.Instance.equippedHelmetMain.bonusDamage);
                                TeamGearManager.Instance.mainFighterGearSlots[0].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedHelmetMain.bonusSpeed);
                            }

                            else if (TeamGearManager.Instance.equippedChestpieceMain != null && !doneChest7)
                            {
                                doneChest7 = true;

                                TeamGearManager.Instance.mainFighterGearSlots[1].isEmpty = false;
                                TeamGearManager.Instance.mainFighterGearSlots[1].UpdateSlotName(TeamGearManager.Instance.equippedChestpieceMain.gearName);
                                TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusHealth(TeamGearManager.Instance.equippedChestpieceMain.bonusHealth);
                                TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusHealing(TeamGearManager.Instance.equippedChestpieceMain.bonusHealing);
                                TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusDefense(TeamGearManager.Instance.equippedChestpieceMain.bonusDefense);
                                TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusDamage(TeamGearManager.Instance.equippedChestpieceMain.bonusDamage);
                                TeamGearManager.Instance.mainFighterGearSlots[1].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedChestpieceMain.bonusSpeed);
                            }
                            else if (TeamGearManager.Instance.equippedBootsMain != null && !doneBoots7)
                            {
                                doneBoots7 = true;

                                TeamGearManager.Instance.mainFighterGearSlots[2].isEmpty = false;
                                TeamGearManager.Instance.mainFighterGearSlots[2].UpdateSlotName(TeamGearManager.Instance.equippedBootsMain.gearName);
                                TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusHealth(TeamGearManager.Instance.equippedBootsMain.bonusHealth);
                                TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusHealing(TeamGearManager.Instance.equippedBootsMain.bonusHealing);
                                TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusDefense(TeamGearManager.Instance.equippedBootsMain.bonusDefense);
                                TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusDamage(TeamGearManager.Instance.equippedBootsMain.bonusDamage);
                                TeamGearManager.Instance.mainFighterGearSlots[2].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedBootsMain.bonusSpeed);
                            }
                        }

                        bool doneHelm8 = false;
                        bool doneChest8 = false;
                        bool doneBoots8 = false;

                        for (int l = 0; l < wornGearSecondAlly.Count; l++)
                        {
                            if (TeamGearManager.Instance.equippedHelmetSec != null && !doneHelm8)
                            {
                                doneHelm8 = true;

                                TeamGearManager.Instance.ally2GearSlots[0].isEmpty = false;
                                TeamGearManager.Instance.ally2GearSlots[0].UpdateSlotName(TeamGearManager.Instance.equippedHelmetSec.gearName);
                                TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusHealth(TeamGearManager.Instance.equippedHelmetSec.bonusHealth);
                                TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusHealing(TeamGearManager.Instance.equippedHelmetSec.bonusHealing);
                                TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusDefense(TeamGearManager.Instance.equippedHelmetSec.bonusDefense);
                                TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusDamage(TeamGearManager.Instance.equippedHelmetSec.bonusDamage);
                                TeamGearManager.Instance.ally2GearSlots[0].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedHelmetSec.bonusSpeed);
                            }

                            else if (TeamGearManager.Instance.equippedChestpieceSec != null && !doneChest8)
                            {
                                doneChest8 = true;

                                TeamGearManager.Instance.ally2GearSlots[1].isEmpty = false;
                                TeamGearManager.Instance.ally2GearSlots[1].UpdateSlotName(TeamGearManager.Instance.equippedChestpieceSec.gearName);
                                TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusHealth(TeamGearManager.Instance.equippedChestpieceSec.bonusHealth);
                                TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusHealing(TeamGearManager.Instance.equippedChestpieceSec.bonusHealing);
                                TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusDefense(TeamGearManager.Instance.equippedChestpieceSec.bonusDefense);
                                TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusDamage(TeamGearManager.Instance.equippedChestpieceSec.bonusDamage);
                                TeamGearManager.Instance.ally2GearSlots[1].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedChestpieceSec.bonusSpeed);
                            }
                            else if (TeamGearManager.Instance.equippedBootsSec != null && !doneBoots8)
                            {
                                doneBoots8 = true;

                                TeamGearManager.Instance.ally2GearSlots[2].isEmpty = false;
                                TeamGearManager.Instance.ally2GearSlots[2].UpdateSlotName(TeamGearManager.Instance.equippedBootsSec.gearName);
                                TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusHealth(TeamGearManager.Instance.equippedBootsSec.bonusHealth);
                                TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusHealing(TeamGearManager.Instance.equippedBootsSec.bonusHealing);
                                TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusDefense(TeamGearManager.Instance.equippedBootsSec.bonusDefense);
                                TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusDamage(TeamGearManager.Instance.equippedBootsSec.bonusDamage);
                                TeamGearManager.Instance.ally2GearSlots[2].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedBootsSec.bonusSpeed);
                            }
                        }

                        bool doneHelm9 = false;
                        bool doneChest9 = false;
                        bool doneBoots9 = false;

                        for (int l = 0; l < wornGearThirdAlly.Count; l++)
                        {
                            if (TeamGearManager.Instance.equippedHelmetThi != null && !doneHelm9)
                            {
                                doneHelm9 = true;

                                TeamGearManager.Instance.ally3GearSlots[0].isEmpty = false;
                                TeamGearManager.Instance.ally3GearSlots[0].UpdateSlotName(TeamGearManager.Instance.equippedHelmetThi.gearName);
                                TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusHealth(TeamGearManager.Instance.equippedHelmetThi.bonusHealth);
                                TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusHealing(TeamGearManager.Instance.equippedHelmetThi.bonusHealing);
                                TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusDefense(TeamGearManager.Instance.equippedHelmetThi.bonusDefense);
                                TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusDamage(TeamGearManager.Instance.equippedHelmetThi.bonusDamage);
                                TeamGearManager.Instance.ally3GearSlots[0].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedHelmetThi.bonusSpeed);
                            }

                            else if (TeamGearManager.Instance.equippedChestpieceThi != null && !doneChest9)
                            {
                                doneChest9 = true;

                                TeamGearManager.Instance.ally3GearSlots[1].isEmpty = false;
                                TeamGearManager.Instance.ally3GearSlots[1].UpdateSlotName(TeamGearManager.Instance.equippedChestpieceThi.gearName);
                                TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusHealth(TeamGearManager.Instance.equippedChestpieceThi.bonusHealth);
                                TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusHealing(TeamGearManager.Instance.equippedChestpieceThi.bonusHealing);
                                TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusDefense(TeamGearManager.Instance.equippedChestpieceThi.bonusDefense);
                                TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusDamage(TeamGearManager.Instance.equippedChestpieceThi.bonusDamage);
                                TeamGearManager.Instance.ally3GearSlots[1].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedChestpieceThi.bonusSpeed);
                            }
                            else if (TeamGearManager.Instance.equippedBootsThi != null && !doneBoots9)
                            {
                                doneBoots9 = true;

                                TeamGearManager.Instance.ally3GearSlots[2].isEmpty = false;
                                TeamGearManager.Instance.ally3GearSlots[2].UpdateSlotName(TeamGearManager.Instance.equippedBootsThi.gearName);
                                TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusHealth(TeamGearManager.Instance.equippedBootsThi.bonusHealth);
                                TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusHealing(TeamGearManager.Instance.equippedBootsThi.bonusHealing);
                                TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusDefense(TeamGearManager.Instance.equippedBootsThi.bonusDefense);
                                TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusDamage(TeamGearManager.Instance.equippedBootsThi.bonusDamage);
                                TeamGearManager.Instance.ally3GearSlots[2].UpdateGearBonusSpeed(TeamGearManager.Instance.equippedBootsThi.bonusSpeed);
                            }
                        }

                        #endregion

                        #region Items

                        // Remove unit 2 items
                        wornItemsSecondAlly.Clear();
                        TeamItemsManager.Instance.equippedItemsSecond.Clear();

                        // Remove unit 3 items
                        wornItemsThirdAlly.Clear();

                        TeamItemsManager.Instance.equippedItemsThird.Clear();


                        bool done1st = false;
                        bool done2nd = false;
                        bool done3rd = false;

                        for (int l = 0; l < wornItemsMainAlly.Count; l++)
                        {
                            if (TeamItemsManager.Instance.equippedItemsMain.Count == 1 && !done1st)
                            {
                                done1st = true;

                                TeamItemsManager.Instance.ally1ItemsSlots[0].isEmpty = false;
                                TeamItemsManager.Instance.ally1ItemsSlots[0].UpdateSlotName(TeamItemsManager.Instance.equippedItemsMain[0].itemName);
                            }
                            else if (TeamItemsManager.Instance.equippedItemsMain.Count == 2 && !done2nd)
                            {
                                done2nd = true;

                                TeamItemsManager.Instance.ally1ItemsSlots[1].isEmpty = false;
                                TeamItemsManager.Instance.ally1ItemsSlots[1].UpdateSlotName(TeamItemsManager.Instance.equippedItemsMain[1].itemName);
                            }
                            else if (TeamItemsManager.Instance.equippedItemsMain.Count == 3 && !done3rd)
                            {
                                done3rd = true;

                                TeamItemsManager.Instance.ally1ItemsSlots[2].isEmpty = false;
                                TeamItemsManager.Instance.ally1ItemsSlots[2].UpdateSlotName(TeamItemsManager.Instance.equippedItemsMain[2].itemName);
                            }
                        }

                        for (int l = 0; l < wornItemsSecondAlly.Count; l++)
                        {
                            if (TeamItemsManager.Instance.equippedItemsSecond.Count == 1 && !done1st)
                            {
                                done1st = true;

                                TeamItemsManager.Instance.ally2ItemsSlots[0].isEmpty = false;
                                TeamItemsManager.Instance.ally2ItemsSlots[0].UpdateSlotName(TeamItemsManager.Instance.equippedItemsSecond[0].itemName);
                            }
                            else if (TeamItemsManager.Instance.equippedItemsSecond.Count == 2 && !done2nd)
                            {
                                done2nd = true;

                                TeamItemsManager.Instance.ally2ItemsSlots[1].isEmpty = false;
                                TeamItemsManager.Instance.ally2ItemsSlots[1].UpdateSlotName(TeamItemsManager.Instance.equippedItemsSecond[1].itemName);
                            }
                            else if (TeamItemsManager.Instance.equippedItemsSecond.Count == 3 && !done3rd)
                            {
                                done3rd = true;

                                TeamItemsManager.Instance.ally2ItemsSlots[2].isEmpty = false;
                                TeamItemsManager.Instance.ally2ItemsSlots[2].UpdateSlotName(TeamItemsManager.Instance.equippedItemsSecond[2].itemName);
                            }
                        }

                        for (int l = 0; l < wornItemsThirdAlly.Count; l++)
                        {
                            if (TeamItemsManager.Instance.equippedItemsThird.Count == 1 && !done1st)
                            {
                                done1st = true;

                                TeamItemsManager.Instance.ally3ItemsSlots[0].isEmpty = false;
                                TeamItemsManager.Instance.ally3ItemsSlots[0].UpdateSlotName(TeamItemsManager.Instance.equippedItemsThird[0].itemName);
                            }
                            else if (TeamItemsManager.Instance.equippedItemsThird.Count == 2 && !done2nd)
                            {
                                done2nd = true;

                                TeamItemsManager.Instance.ally3ItemsSlots[1].isEmpty = false;
                                TeamItemsManager.Instance.ally3ItemsSlots[1].UpdateSlotName(TeamItemsManager.Instance.equippedItemsThird[1].itemName);
                            }
                            else if (TeamItemsManager.Instance.equippedItemsThird.Count == 3 && !done3rd)
                            {
                                done3rd = true;

                                TeamItemsManager.Instance.ally3ItemsSlots[2].isEmpty = false;
                                TeamItemsManager.Instance.ally3ItemsSlots[2].UpdateSlotName(TeamItemsManager.Instance.equippedItemsThird[2].itemName);
                            }
                        }
                        #endregion

                        break;
                    }
                }
            }

            break;
        }
    }

    public void AddWornGearAllyMain(Slot gear)
    {
        wornGearMainAlly.Add(gear);
    }
    public void AddWornGearAllySecond(Slot gear)
    {
        wornGearSecondAlly.Add(gear);
    }
    public void AddWornGearAllyThird(Slot gear)
    {
        wornGearThirdAlly.Add(gear);
    }

    public List<Slot> GetWornGearMainAlly()
    {
        return wornGearMainAlly;
    }
    public List<Slot> GetWornGearSecondAlly()
    {
        return wornGearSecondAlly;
    }
    public List<Slot> GetWornGearThirdAlly()
    {
        return wornGearThirdAlly;
    }

    public void RemoveOwnedGear(Slot gear)
    {
        for (int i = 0; i < ownedGear.Count; i++)
        {
            if (ownedGear[i].linkedGearPiece.gearName == gear.linkedGearPiece.gearName)
            {
                ownedGear.Remove(ownedGear[i]);
                break;
            }
        }
    }

    public void RemoveWornGearAllyMain(Slot gear)
    {
        wornGearMainAlly.Remove(gear);
    }
    public void RemoveWornGearAllySecond(Slot gear)
    {
        wornGearSecondAlly.Remove(gear);
    }
    public void RemoveWornGearAllyThird(Slot gear)
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



    public void AddWornItemAllyMain(Slot item)
    {
        wornItemsMainAlly.Add(item);
    }
    public void AddWornItemAllySecond(Slot item)
    {
        wornItemsSecondAlly.Add(item);
    }
    public void AddWornItemAllyThird(Slot item)
    {
        wornItemsThirdAlly.Add(item);
    }

    public List<Slot> GetWornItemMainAlly()
    {
        return wornItemsMainAlly;
    }
    public List<Slot> GetWornItemSecondAlly()
    {
        return wornItemsSecondAlly;
    }
    public List<Slot> GetWornItemThirdAlly()
    {
        return wornItemsThirdAlly;
    }

    public void RemoveOwnedItem(Slot item)
    {
        ownedItems.Remove(item);

        /*
        int count = ownedItems.Count;
        List<Slot> newList = new List<Slot>();

        newList = ownedItems;
        for (int i = 0; i < newList.Count; i++)
        {
            if (newList[i] == null)
            {

            }

        }
        */
    }

    public void RemoveWornItemAllyMain(Slot item)
    {
        wornItemsMainAlly.Remove(item);
    }
    public void RemoveWornItemAllySecond(Slot item)
    {
        wornItemsSecondAlly.Remove(item);
    }
    public void RemoveWornItemAllyThird(Slot item)
    {
        wornItemsThirdAlly.Remove(item);
    }

    public void ResetWornItemsAllyMain()
    {
        wornItemsMainAlly.Clear();
    }

    public void ResetWornItemsAllySecond()
    {
        wornItemsSecondAlly.Clear();
    }

    public void ResetWornItemsAllyThird()
    {
        wornItemsThirdAlly.Clear();
    }

    public bool GetOwnedLootOpened()
    {
        return ownedLootOpened;
    }

    public void MoveLootToOwnedLootTab(Transform trans)
    {
        List<Transform> allTrans = new List<Transform>();

        for (int x = 0; x < trans.childCount; x++)
        {
            if (trans.GetChild(x).GetComponent<Slot>())
                allTrans.Add(trans.GetChild(x));
        }

        //Debug.Log("All Gear count = " + allTrans.Count);

        for (int i = 0; i < allTrans.Count; i++)
        {
            GameObject go = allTrans[i].gameObject;
            go.transform.SetParent(ownedLootUI.transform);

            // 1000 pos due to objects spawning in screen and blocking input (they are invisible data carrying objects)
            go.transform.localPosition = new Vector2(1000, 1000);
            go.transform.localScale = Vector2.one;

            Slot gear = go.GetComponent<Slot>();
            //ownedGear.Add(gear);

            gear.UpdateLootGearAlpha(false);

            int newIndex = ownedGear.IndexOf(gear);

            if (ownedGear.Count > newIndex)
                return;

            ownedGear[newIndex].UpdateSlotImage(trans.GetChild(i).GetComponent<Slot>().linkedGearPiece.gearIcon);
            if (trans.GetChild(i).GetComponent<Slot>().linkedGearPiece.gearType == "helmet")
                ownedGear[newIndex].UpdateCurSlotType(Slot.SlotPieceType.helmet);
            else if (trans.GetChild(i).GetComponent<Slot>().linkedGearPiece.gearType == "chestpiece")
                ownedGear[newIndex].UpdateCurSlotType(Slot.SlotPieceType.chestpiece);
            else if (trans.GetChild(i).GetComponent<Slot>().linkedGearPiece.gearType == "boots")
                ownedGear[newIndex].UpdateCurSlotType(Slot.SlotPieceType.boots);
            else if (trans.GetChild(i).GetComponent<Slot>().linkedGearPiece.gearType == "pendant" || trans.GetChild(i).GetComponent<Slot>().linkedGearPiece.gearType == "neckless")
                ownedGear[newIndex].UpdateCurSlotType(Slot.SlotPieceType.neckless);
            else if (trans.GetChild(i).GetComponent<Slot>().linkedGearPiece.gearType == "earring")
                ownedGear[newIndex].UpdateCurSlotType(Slot.SlotPieceType.earring);
            else if (trans.GetChild(i).GetComponent<Slot>().linkedGearPiece.gearType == "belt")
                ownedGear[newIndex].UpdateCurSlotType(Slot.SlotPieceType.belt);
            else if (trans.GetChild(i).GetComponent<Slot>().linkedGearPiece.gearType == "glove")
                ownedGear[newIndex].UpdateCurSlotType(Slot.SlotPieceType.glove);
            else if (trans.GetChild(i).GetComponent<Slot>().linkedGearPiece.gearType == "ring")
                ownedGear[newIndex].UpdateCurSlotType(Slot.SlotPieceType.ring);
            //GearRewards.Instance.IncrementSpawnedGearCount();

            ownedGear[newIndex].UpdateSlotCode(GearRewards.Instance.spawnedGearCount);
            ownedGear[newIndex].UpdateSlotName(trans.GetChild(i).GetComponent<Slot>().linkedGearPiece.gearName);
            ownedGear[newIndex].UpdateGearBonusHealth(trans.GetChild(i).GetComponent<Slot>().linkedGearPiece.bonusHealth);
            ownedGear[newIndex].UpdateGearBonusDamage(trans.GetChild(i).GetComponent<Slot>().linkedGearPiece.bonusDamage);
            ownedGear[newIndex].UpdateGearBonusHealing(trans.GetChild(i).GetComponent<Slot>().linkedGearPiece.bonusHealing);
            ownedGear[newIndex].UpdateGearBonusDefense(trans.GetChild(i).GetComponent<Slot>().linkedGearPiece.bonusDefense);
            ownedGear[newIndex].UpdateGearBonusSpeed(trans.GetChild(i).GetComponent<Slot>().linkedGearPiece.bonusSpeed);
            ownedGear[newIndex].UpdateGearStatis(Slot.SlotStatis.OWNED);
            ownedGear[newIndex].ToggleEquipButton(false);            
        }
    }

    public void MoveGearAndItemsToOwnedLoot()
    {
        // Items main hero
        if (GameManager.Instance.activeRoomHeroes[0].isDead)
        {
            for (int x = 0; x < wornItemsMainAlly.Count; x++)
            {
                string itemName = wornItemsMainAlly[x].GetSlotName();

                for (int a = 0; a < GearRewards.Instance.allItemPiecesCommon.Count; a++)
                {
                    if (itemName == GearRewards.Instance.allItemPiecesCommon[a].itemName)
                    {
                        GameObject go = Instantiate(GearRewards.Instance.gearGO, ownedLootUI.transform);
                        // 1000 pos due to objects spawning in screen and blocking input (they are invisible data carrying objects)
                        go.transform.localPosition = new Vector2(1000, 1000);
                        go.transform.localScale = Vector2.one;

                        Slot gear = go.GetComponent<Slot>();
                        ownedItems.Add(gear);


                        gear.UpdateLootGearAlpha(false);
                        gear.UpdateSlotImage(GearRewards.Instance.allItemPiecesCommon[a].itemSpriteItemTab);
                        gear.UpdateCurSlotType(Slot.SlotPieceType.ITEM);
                        GearRewards.Instance.IncrementSpawnedGearCount();
                        //gear.UpdateSlotCode(GearRewards.Instance.spawnedGearCount);
                        gear.UpdateSlotName(GearRewards.Instance.allItemPiecesCommon[a].itemName);
                        gear.UpdateGearStatis(Slot.SlotStatis.OWNED);
                        gear.UpdateLinkedItemPiece(GearRewards.Instance.allItemPiecesCommon[a]);
                        gear.ToggleEquipButton(false);

                        //Debug.Log("adding " + gear.GetSlotName());
                    }
                }

                for (int b = 0; b < GearRewards.Instance.allItemPiecesRare.Count; b++)
                {
                    if (itemName == GearRewards.Instance.allItemPiecesRare[b].itemName)
                    {
                        GameObject go = Instantiate(GearRewards.Instance.gearGO, ownedLootUI.transform);
                        // 1000 pos due to objects spawning in screen and blocking input (they are invisible data carrying objects)
                        go.transform.localPosition = new Vector2(1000, 1000);
                        go.transform.localScale = Vector2.one;

                        Slot gear = go.GetComponent<Slot>();
                        ownedItems.Add(gear);

                        gear.UpdateLootGearAlpha(false);
                        gear.UpdateSlotImage(GearRewards.Instance.allItemPiecesRare[b].itemSpriteItemTab);
                        gear.UpdateCurSlotType(Slot.SlotPieceType.ITEM);
                        //GearRewards.Instance.IncrementSpawnedGearCount();
                        gear.UpdateSlotCode(GearRewards.Instance.spawnedGearCount);
                        gear.UpdateSlotName(GearRewards.Instance.allItemPiecesRare[b].itemName);
                        gear.UpdateGearStatis(Slot.SlotStatis.OWNED);
                        gear.UpdateLinkedItemPiece(GearRewards.Instance.allItemPiecesRare[b]);
                        gear.ToggleEquipButton(false);

                        //Debug.Log("adding " + gear.GetSlotName());
                    }
                }
            }

            // gear main hero
            for (int x = 0; x < wornGearMainAlly.Count; x++)
            {
                string gearName = wornGearMainAlly[x].GetSlotName();

                for (int a = 0; a < GearRewards.Instance.allGearPiecesCommon.Count; a++)
                {
                    if (gearName == GearRewards.Instance.allGearPiecesCommon[a].gearName)
                    {
                        GameObject go = Instantiate(GearRewards.Instance.gearGO, ownedLootUI.transform);
                        // 1000 pos due to objects spawning in screen and blocking input (they are invisible data carrying objects)
                        go.transform.localPosition = new Vector2(1000, 1000);
                        go.transform.localScale = Vector2.one;

                        Slot gear = go.GetComponent<Slot>();
                        ownedGear.Add(gear);


                        gear.UpdateLootGearAlpha(false);

                        gear.UpdateSlotImage(GearRewards.Instance.allGearPiecesCommon[a].gearIcon);
                        if (GearRewards.Instance.allGearPiecesCommon[a].gearType == "helmet")
                            gear.UpdateCurSlotType(Slot.SlotPieceType.helmet);
                        else if (GearRewards.Instance.allGearPiecesCommon[a].gearType == "chestpiece")
                            gear.UpdateCurSlotType(Slot.SlotPieceType.chestpiece);
                        else if (GearRewards.Instance.allGearPiecesCommon[a].gearType == "boots")
                            gear.UpdateCurSlotType(Slot.SlotPieceType.boots);
                        else if (GearRewards.Instance.allGearPiecesRare[a].gearType == "pendant" || GearRewards.Instance.allGearPiecesRare[a].gearType == "neckless")
                            gear.UpdateCurSlotType(Slot.SlotPieceType.neckless);
                        else if (GearRewards.Instance.allGearPiecesRare[a].gearType == "earring")
                            gear.UpdateCurSlotType(Slot.SlotPieceType.earring);
                        else if (GearRewards.Instance.allGearPiecesRare[a].gearType == "belt")
                            gear.UpdateCurSlotType(Slot.SlotPieceType.belt);
                        else if (GearRewards.Instance.allGearPiecesRare[a].gearType == "glove")
                            gear.UpdateCurSlotType(Slot.SlotPieceType.glove);
                        else if (GearRewards.Instance.allGearPiecesRare[a].gearType == "ring")
                            gear.UpdateCurSlotType(Slot.SlotPieceType.ring);
                        GearRewards.Instance.IncrementSpawnedGearCount();

                        gear.UpdateSlotCode(GearRewards.Instance.spawnedGearCount);
                        gear.UpdateSlotName(GearRewards.Instance.allGearPiecesCommon[a].gearName);
                        gear.UpdateGearBonusHealth(GearRewards.Instance.allGearPiecesCommon[a].bonusHealth);
                        gear.UpdateGearBonusDamage(GearRewards.Instance.allGearPiecesCommon[a].bonusDamage);
                        gear.UpdateGearBonusHealing(GearRewards.Instance.allGearPiecesCommon[a].bonusHealing);
                        gear.UpdateGearBonusDefense(GearRewards.Instance.allGearPiecesCommon[a].bonusDefense);
                        gear.UpdateGearBonusSpeed(GearRewards.Instance.allGearPiecesCommon[a].bonusSpeed);
                        gear.UpdateGearStatis(Slot.SlotStatis.OWNED);
                        gear.ToggleEquipButton(false);
                        gear.linkedGearPiece = GearRewards.Instance.allGearPiecesCommon[a];

                        //Debug.Log("adding " + gear.GetSlotName());
                    }
                }

                for (int b = 0; b < GearRewards.Instance.allGearPiecesRare.Count; b++)
                {
                    if (gearName == GearRewards.Instance.allGearPiecesRare[b].gearName)
                    {
                        GameObject go = Instantiate(GearRewards.Instance.gearGO, ownedLootUI.transform);
                        // 1000 pos due to objects spawning in screen and blocking input (they are invisible data carrying objects)
                        go.transform.localPosition = new Vector2(1000, 1000);
                        go.transform.localScale = Vector2.one;

                        Slot gear = go.GetComponent<Slot>();
                        ownedGear.Add(gear);

                        gear.UpdateLootGearAlpha(false);

                        gear.UpdateSlotImage(GearRewards.Instance.allGearPiecesRare[b].gearIcon);
                        if (GearRewards.Instance.allGearPiecesRare[b].gearType == "helmet")
                            gear.UpdateCurSlotType(Slot.SlotPieceType.helmet);
                        else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "chestpiece")
                            gear.UpdateCurSlotType(Slot.SlotPieceType.chestpiece);
                        else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "boots")
                            gear.UpdateCurSlotType(Slot.SlotPieceType.boots);
                        else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "pendant" || GearRewards.Instance.allGearPiecesRare[b].gearType == "neckless")
                            gear.UpdateCurSlotType(Slot.SlotPieceType.neckless);
                        else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "earring")
                            gear.UpdateCurSlotType(Slot.SlotPieceType.earring);
                        else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "belt")
                            gear.UpdateCurSlotType(Slot.SlotPieceType.belt);
                        else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "glove")
                            gear.UpdateCurSlotType(Slot.SlotPieceType.glove);
                        else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "ring")
                            gear.UpdateCurSlotType(Slot.SlotPieceType.ring);
                        GearRewards.Instance.IncrementSpawnedGearCount();

                        gear.UpdateSlotCode(GearRewards.Instance.spawnedGearCount);
                        gear.UpdateSlotName(GearRewards.Instance.allGearPiecesRare[b].gearName);
                        gear.UpdateGearBonusHealth(GearRewards.Instance.allGearPiecesRare[b].bonusHealth);
                        gear.UpdateGearBonusDamage(GearRewards.Instance.allGearPiecesRare[b].bonusDamage);
                        gear.UpdateGearBonusHealing(GearRewards.Instance.allGearPiecesRare[b].bonusHealing);
                        gear.UpdateGearBonusDefense(GearRewards.Instance.allGearPiecesRare[b].bonusDefense);
                        gear.UpdateGearBonusSpeed(GearRewards.Instance.allGearPiecesRare[b].bonusSpeed);
                        gear.UpdateGearStatis(Slot.SlotStatis.OWNED);
                        gear.ToggleEquipButton(false);
                        gear.linkedGearPiece = GearRewards.Instance.allGearPiecesRare[b];
                        //Debug.Log("adding " + gear.GetSlotName());
                    }
                }
            }
        }

        // gear 2nd hero
        if (GameManager.Instance.activeRoomHeroes.Count >= 2)
        {
            if (GameManager.Instance.activeRoomHeroes[1].isDead)
            {
                // Items 2nd hero
                for (int x = 0; x < wornItemsSecondAlly.Count; x++)
                {
                    string itemName = wornItemsSecondAlly[x].GetSlotName();

                    for (int a = 0; a < GearRewards.Instance.allItemPiecesCommon.Count; a++)
                    {
                        if (itemName == GearRewards.Instance.allItemPiecesCommon[a].itemName)
                        {
                            GameObject go = Instantiate(GearRewards.Instance.gearGO, ownedLootUI.transform);
                            // 1000 pos due to objects spawning in screen and blocking input (they are invisible data carrying objects)
                            go.transform.localPosition = new Vector2(1000, 1000);
                            go.transform.localScale = Vector2.one;

                            Slot gear = go.GetComponent<Slot>();
                            ownedItems.Add(gear);


                            gear.UpdateLootGearAlpha(false);
                            gear.UpdateSlotImage(GearRewards.Instance.allItemPiecesCommon[a].itemSpriteItemTab);
                            gear.UpdateCurSlotType(Slot.SlotPieceType.ITEM);
                            GearRewards.Instance.IncrementSpawnedGearCount();
                            //gear.UpdateSlotCode(GearRewards.Instance.spawnedGearCount);
                            gear.UpdateSlotName(GearRewards.Instance.allItemPiecesCommon[a].itemName);
                            gear.UpdateGearStatis(Slot.SlotStatis.OWNED);
                            gear.UpdateLinkedItemPiece(GearRewards.Instance.allItemPiecesCommon[a]);
                            gear.ToggleEquipButton(false);

                            //Debug.Log("adding " + gear.GetSlotName());
                        }
                    }

                    for (int b = 0; b < GearRewards.Instance.allItemPiecesRare.Count; b++)
                    {
                        if (itemName == GearRewards.Instance.allItemPiecesRare[b].itemName)
                        {
                            GameObject go = Instantiate(GearRewards.Instance.gearGO, ownedLootUI.transform);
                            // 1000 pos due to objects spawning in screen and blocking input (they are invisible data carrying objects)
                            go.transform.localPosition = new Vector2(1000, 1000);
                            go.transform.localScale = Vector2.one;

                            Slot gear = go.GetComponent<Slot>();
                            ownedItems.Add(gear);

                            gear.UpdateLootGearAlpha(false);
                            gear.UpdateSlotImage(GearRewards.Instance.allItemPiecesRare[b].itemSpriteItemTab);
                            gear.UpdateCurSlotType(Slot.SlotPieceType.ITEM);
                            //GearRewards.Instance.IncrementSpawnedGearCount();
                            gear.UpdateSlotCode(GearRewards.Instance.spawnedGearCount);
                            gear.UpdateSlotName(GearRewards.Instance.allItemPiecesRare[b].itemName);
                            gear.UpdateGearStatis(Slot.SlotStatis.OWNED);
                            gear.UpdateLinkedItemPiece(GearRewards.Instance.allItemPiecesRare[b]);
                            gear.ToggleEquipButton(false);

                            //Debug.Log("adding " + gear.GetSlotName());
                        }
                    }
                }

                //  2nd Gear
                for (int x = 0; x < wornGearSecondAlly.Count; x++)
                {
                    string gearName = wornGearSecondAlly[x].GetSlotName();

                    for (int a = 0; a < GearRewards.Instance.allGearPiecesCommon.Count; a++)
                    {
                        if (gearName == GearRewards.Instance.allGearPiecesCommon[a].gearName)
                        {
                            GameObject go = Instantiate(GearRewards.Instance.gearGO, ownedLootUI.transform);
                            // 1000 pos due to objects spawning in screen and blocking input (they are invisible data carrying objects)
                            go.transform.localPosition = new Vector2(1000, 1000);
                            go.transform.localScale = Vector2.one;

                            Slot gear = go.GetComponent<Slot>();
                            ownedGear.Add(gear);


                            gear.UpdateLootGearAlpha(false);

                            gear.UpdateSlotImage(GearRewards.Instance.allGearPiecesCommon[a].gearIcon);
                            if (GearRewards.Instance.allGearPiecesCommon[a].gearType == "helmet")
                                gear.UpdateCurSlotType(Slot.SlotPieceType.helmet);
                            else if (GearRewards.Instance.allGearPiecesCommon[a].gearType == "chestpiece")
                                gear.UpdateCurSlotType(Slot.SlotPieceType.chestpiece);
                            else if (GearRewards.Instance.allGearPiecesCommon[a].gearType == "boots")
                                gear.UpdateCurSlotType(Slot.SlotPieceType.boots);
                            else if (GearRewards.Instance.allGearPiecesRare[a].gearType == "pendant" || GearRewards.Instance.allGearPiecesRare[a].gearType == "neckless")
                                gear.UpdateCurSlotType(Slot.SlotPieceType.neckless);
                            else if (GearRewards.Instance.allGearPiecesRare[a].gearType == "earring")
                                gear.UpdateCurSlotType(Slot.SlotPieceType.earring);
                            else if (GearRewards.Instance.allGearPiecesRare[a].gearType == "belt")
                                gear.UpdateCurSlotType(Slot.SlotPieceType.belt);
                            else if (GearRewards.Instance.allGearPiecesRare[a].gearType == "glove")
                                gear.UpdateCurSlotType(Slot.SlotPieceType.glove);
                            else if (GearRewards.Instance.allGearPiecesRare[a].gearType == "ring")
                                gear.UpdateCurSlotType(Slot.SlotPieceType.ring);
                            GearRewards.Instance.IncrementSpawnedGearCount();

                            gear.UpdateSlotCode(GearRewards.Instance.spawnedGearCount);
                            gear.UpdateSlotName(GearRewards.Instance.allGearPiecesCommon[a].gearName);
                            gear.UpdateGearBonusHealth(GearRewards.Instance.allGearPiecesCommon[a].bonusHealth);
                            gear.UpdateGearBonusDamage(GearRewards.Instance.allGearPiecesCommon[a].bonusDamage);
                            gear.UpdateGearBonusHealing(GearRewards.Instance.allGearPiecesCommon[a].bonusHealing);
                            gear.UpdateGearBonusDefense(GearRewards.Instance.allGearPiecesCommon[a].bonusDefense);
                            gear.UpdateGearBonusSpeed(GearRewards.Instance.allGearPiecesCommon[a].bonusSpeed);
                            gear.UpdateGearStatis(Slot.SlotStatis.OWNED);
                            gear.ToggleEquipButton(false);

                            //Debug.Log("adding " + gear.GetSlotName());
                        }
                    }

                    for (int b = 0; b < GearRewards.Instance.allGearPiecesRare.Count; b++)
                    {
                        if (gearName == GearRewards.Instance.allGearPiecesRare[b].gearName)
                        {
                            GameObject go = Instantiate(GearRewards.Instance.gearGO, ownedLootUI.transform);
                            // 1000 pos due to objects spawning in screen and blocking input (they are invisible data carrying objects)
                            go.transform.localPosition = new Vector2(1000, 1000);
                            go.transform.localScale = Vector2.one;

                            Slot gear = go.GetComponent<Slot>();
                            ownedGear.Add(gear);

                            gear.UpdateLootGearAlpha(false);

                            gear.UpdateSlotImage(GearRewards.Instance.allGearPiecesRare[b].gearIcon);
                            if (GearRewards.Instance.allGearPiecesRare[b].gearType == "helmet")
                                gear.UpdateCurSlotType(Slot.SlotPieceType.helmet);
                            else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "chestpiece")
                                gear.UpdateCurSlotType(Slot.SlotPieceType.chestpiece);
                            else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "boots")
                                gear.UpdateCurSlotType(Slot.SlotPieceType.boots);
                            else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "pendant" || GearRewards.Instance.allGearPiecesRare[b].gearType == "neckless")
                                gear.UpdateCurSlotType(Slot.SlotPieceType.neckless);
                            else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "earring")
                                gear.UpdateCurSlotType(Slot.SlotPieceType.earring);
                            else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "belt")
                                gear.UpdateCurSlotType(Slot.SlotPieceType.belt);
                            else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "glove")
                                gear.UpdateCurSlotType(Slot.SlotPieceType.glove);
                            else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "ring")
                                gear.UpdateCurSlotType(Slot.SlotPieceType.ring);
                            GearRewards.Instance.IncrementSpawnedGearCount();

                            gear.UpdateSlotCode(GearRewards.Instance.spawnedGearCount);
                            gear.UpdateSlotName(GearRewards.Instance.allGearPiecesRare[b].gearName);
                            gear.UpdateGearBonusHealth(GearRewards.Instance.allGearPiecesRare[b].bonusHealth);
                            gear.UpdateGearBonusDamage(GearRewards.Instance.allGearPiecesRare[b].bonusDamage);
                            gear.UpdateGearBonusHealing(GearRewards.Instance.allGearPiecesRare[b].bonusHealing);
                            gear.UpdateGearBonusDefense(GearRewards.Instance.allGearPiecesRare[b].bonusDefense);
                            gear.UpdateGearBonusSpeed(GearRewards.Instance.allGearPiecesRare[b].bonusSpeed);
                            gear.UpdateGearStatis(Slot.SlotStatis.OWNED);
                            gear.ToggleEquipButton(false);

                            //Debug.Log("adding " + gear.GetSlotName());
                        }
                    }
                }
            }

            if (GameManager.Instance.activeRoomHeroes.Count == 3)
            {
                if (GameManager.Instance.activeRoomHeroes[2].isDead)
                {
                    // Items 3rd hero
                    for (int x = 0; x < wornItemsThirdAlly.Count; x++)
                    {
                        string itemName = wornItemsThirdAlly[x].GetSlotName();

                        for (int a = 0; a < GearRewards.Instance.allItemPiecesCommon.Count; a++)
                        {
                            if (itemName == GearRewards.Instance.allItemPiecesCommon[a].itemName)
                            {
                                GameObject go = Instantiate(GearRewards.Instance.gearGO, ownedLootUI.transform);
                                // 1000 pos due to objects spawning in screen and blocking input (they are invisible data carrying objects)
                                go.transform.localPosition = new Vector2(1000, 1000);
                                go.transform.localScale = Vector2.one;

                                Slot gear = go.GetComponent<Slot>();
                                ownedItems.Add(gear);


                                gear.UpdateLootGearAlpha(false);
                                gear.UpdateSlotImage(GearRewards.Instance.allItemPiecesCommon[a].itemSpriteItemTab);
                                gear.UpdateCurSlotType(Slot.SlotPieceType.ITEM);
                                GearRewards.Instance.IncrementSpawnedGearCount();
                                //gear.UpdateSlotCode(GearRewards.Instance.spawnedGearCount);
                                gear.UpdateSlotName(GearRewards.Instance.allItemPiecesCommon[a].itemName);
                                gear.UpdateGearStatis(Slot.SlotStatis.OWNED);
                                gear.UpdateLinkedItemPiece(GearRewards.Instance.allItemPiecesCommon[a]);
                                gear.ToggleEquipButton(false);

                                //Debug.Log("adding " + gear.GetSlotName());
                            }
                        }

                        for (int b = 0; b < GearRewards.Instance.allItemPiecesRare.Count; b++)
                        {
                            if (itemName == GearRewards.Instance.allItemPiecesRare[b].itemName)
                            {
                                GameObject go = Instantiate(GearRewards.Instance.gearGO, ownedLootUI.transform);
                                // 1000 pos due to objects spawning in screen and blocking input (they are invisible data carrying objects)
                                go.transform.localPosition = new Vector2(1000, 1000);
                                go.transform.localScale = Vector2.one;

                                Slot gear = go.GetComponent<Slot>();
                                ownedItems.Add(gear);

                                gear.UpdateLootGearAlpha(false);
                                gear.UpdateSlotImage(GearRewards.Instance.allItemPiecesRare[b].itemSpriteItemTab);
                                gear.UpdateCurSlotType(Slot.SlotPieceType.ITEM);
                                //GearRewards.Instance.IncrementSpawnedGearCount();
                                gear.UpdateSlotCode(GearRewards.Instance.spawnedGearCount);
                                gear.UpdateSlotName(GearRewards.Instance.allItemPiecesRare[b].itemName);
                                gear.UpdateGearStatis(Slot.SlotStatis.OWNED);
                                gear.UpdateLinkedItemPiece(GearRewards.Instance.allItemPiecesRare[b]);
                                gear.ToggleEquipButton(false);

                                //Debug.Log("adding " + gear.GetSlotName());
                            }
                        }
                    }

                    // gear 3rd hero
                    for (int x = 0; x < wornGearThirdAlly.Count; x++)
                    {
                        string gearName = wornGearThirdAlly[x].GetSlotName();

                        for (int a = 0; a < GearRewards.Instance.allGearPiecesCommon.Count; a++)
                        {
                            if (gearName == GearRewards.Instance.allGearPiecesCommon[a].gearName)
                            {
                                GameObject go = Instantiate(GearRewards.Instance.gearGO, ownedLootUI.transform);
                                // 1000 pos due to objects spawning in screen and blocking input (they are invisible data carrying objects)
                                go.transform.localPosition = new Vector2(1000, 1000);
                                go.transform.localScale = Vector2.one;

                                Slot gear = go.GetComponent<Slot>();
                                ownedGear.Add(gear);


                                gear.UpdateLootGearAlpha(false);

                                gear.UpdateSlotImage(GearRewards.Instance.allGearPiecesCommon[a].gearIcon);
                                if (GearRewards.Instance.allGearPiecesCommon[a].gearType == "helmet")
                                    gear.UpdateCurSlotType(Slot.SlotPieceType.helmet);
                                else if (GearRewards.Instance.allGearPiecesCommon[a].gearType == "chestpiece")
                                    gear.UpdateCurSlotType(Slot.SlotPieceType.chestpiece);
                                else if (GearRewards.Instance.allGearPiecesCommon[a].gearType == "boots")
                                    gear.UpdateCurSlotType(Slot.SlotPieceType.boots);
                                else if (GearRewards.Instance.allGearPiecesRare[a].gearType == "pendant" || GearRewards.Instance.allGearPiecesRare[a].gearType == "neckless")
                                    gear.UpdateCurSlotType(Slot.SlotPieceType.neckless);
                                else if (GearRewards.Instance.allGearPiecesRare[a].gearType == "earring")
                                    gear.UpdateCurSlotType(Slot.SlotPieceType.earring);
                                else if (GearRewards.Instance.allGearPiecesRare[a].gearType == "belt")
                                    gear.UpdateCurSlotType(Slot.SlotPieceType.belt);
                                else if (GearRewards.Instance.allGearPiecesRare[a].gearType == "glove")
                                    gear.UpdateCurSlotType(Slot.SlotPieceType.glove);
                                else if (GearRewards.Instance.allGearPiecesRare[a].gearType == "ring")
                                    gear.UpdateCurSlotType(Slot.SlotPieceType.ring);
                                GearRewards.Instance.IncrementSpawnedGearCount();

                                gear.UpdateSlotCode(GearRewards.Instance.spawnedGearCount);
                                gear.UpdateSlotName(GearRewards.Instance.allGearPiecesCommon[a].gearName);
                                gear.UpdateGearBonusHealth(GearRewards.Instance.allGearPiecesCommon[a].bonusHealth);
                                gear.UpdateGearBonusDamage(GearRewards.Instance.allGearPiecesCommon[a].bonusDamage);
                                gear.UpdateGearBonusHealing(GearRewards.Instance.allGearPiecesCommon[a].bonusHealing);
                                gear.UpdateGearBonusDefense(GearRewards.Instance.allGearPiecesCommon[a].bonusDefense);
                                gear.UpdateGearBonusSpeed(GearRewards.Instance.allGearPiecesCommon[a].bonusSpeed);
                                gear.UpdateGearStatis(Slot.SlotStatis.OWNED);
                                gear.ToggleEquipButton(false);

                                //Debug.Log("adding " + gear.GetSlotName());
                            }
                        }

                        for (int b = 0; b < GearRewards.Instance.allGearPiecesRare.Count; b++)
                        {
                            if (gearName == GearRewards.Instance.allGearPiecesRare[b].gearName)
                            {
                                GameObject go = Instantiate(GearRewards.Instance.gearGO, ownedLootUI.transform);
                                // 1000 pos due to objects spawning in screen and blocking input (they are invisible data carrying objects)
                                go.transform.localPosition = new Vector2(1000, 1000);
                                go.transform.localScale = Vector2.one;

                                Slot gear = go.GetComponent<Slot>();
                                ownedGear.Add(gear);

                                gear.UpdateLootGearAlpha(false);

                                gear.UpdateSlotImage(GearRewards.Instance.allGearPiecesRare[b].gearIcon);
                                if (GearRewards.Instance.allGearPiecesRare[b].gearType == "helmet")
                                    gear.UpdateCurSlotType(Slot.SlotPieceType.helmet);
                                else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "chestpiece")
                                    gear.UpdateCurSlotType(Slot.SlotPieceType.chestpiece);
                                else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "boots")
                                    gear.UpdateCurSlotType(Slot.SlotPieceType.boots);
                                else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "pendant" || GearRewards.Instance.allGearPiecesRare[b].gearType == "neckless")
                                    gear.UpdateCurSlotType(Slot.SlotPieceType.neckless);
                                else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "earring")
                                    gear.UpdateCurSlotType(Slot.SlotPieceType.earring);
                                else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "belt")
                                    gear.UpdateCurSlotType(Slot.SlotPieceType.belt);
                                else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "glove")
                                    gear.UpdateCurSlotType(Slot.SlotPieceType.glove);
                                else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "ring")
                                    gear.UpdateCurSlotType(Slot.SlotPieceType.ring);

                                GearRewards.Instance.IncrementSpawnedGearCount();

                                gear.UpdateSlotCode(GearRewards.Instance.spawnedGearCount);
                                gear.UpdateSlotName(GearRewards.Instance.allGearPiecesRare[b].gearName);
                                gear.UpdateGearBonusHealth(GearRewards.Instance.allGearPiecesRare[b].bonusHealth);
                                gear.UpdateGearBonusDamage(GearRewards.Instance.allGearPiecesRare[b].bonusDamage);
                                gear.UpdateGearBonusHealing(GearRewards.Instance.allGearPiecesRare[b].bonusHealing);
                                gear.UpdateGearBonusDefense(GearRewards.Instance.allGearPiecesRare[b].bonusDefense);
                                gear.UpdateGearBonusSpeed(GearRewards.Instance.allGearPiecesRare[b].bonusSpeed);
                                gear.UpdateGearStatis(Slot.SlotStatis.OWNED);
                                gear.ToggleEquipButton(false);

                                //Debug.Log("adding " + gear.GetSlotName());
                            }
                        }
                    }
                }
            }
        }
    }

    public void LoadStartingLoot()
    {
        for (int i = 0; i < startingGearPieces.Count; i++)
        {
            GameObject go = Instantiate(GearRewards.Instance.gearGO, ownedLootUI.transform);
            // 1000 pos due to objects spawning in screen and blocking input (they are invisible data carrying objects)
            go.transform.localPosition = new Vector2(1000, 1000);
            go.transform.localScale = Vector2.one;

            Slot gear = go.GetComponent<Slot>();
            ownedGear.Add(gear);

            gear.UpdateLootGearAlpha(false);

            ownedGear[i].UpdateSlotImage(startingGearPieces[i].gearIcon);
            if (startingGearPieces[i].gearType == "helmet")
                ownedGear[i].UpdateCurSlotType(Slot.SlotPieceType.helmet);
            else if (startingGearPieces[i].gearType == "chestpiece")
                ownedGear[i].UpdateCurSlotType(Slot.SlotPieceType.chestpiece);
            else if (startingGearPieces[i].gearType == "boots")
                ownedGear[i].UpdateCurSlotType(Slot.SlotPieceType.boots);
            else if (startingGearPieces[i].gearType == "neckless" || startingGearPieces[i].gearType == "pendant")
                ownedGear[i].UpdateCurSlotType(Slot.SlotPieceType.neckless);
            else if (startingGearPieces[i].gearType == "earring")
                ownedGear[i].UpdateCurSlotType(Slot.SlotPieceType.earring);
            else if (startingGearPieces[i].gearType == "belt")
                ownedGear[i].UpdateCurSlotType(Slot.SlotPieceType.belt);
            else if (startingGearPieces[i].gearType == "glove")
                ownedGear[i].UpdateCurSlotType(Slot.SlotPieceType.glove);
            else if (startingGearPieces[i].gearType == "ring")
                ownedGear[i].UpdateCurSlotType(Slot.SlotPieceType.ring);

            GearRewards.Instance.IncrementSpawnedGearCount();

            ownedGear[i].UpdateSlotCode(GearRewards.Instance.spawnedGearCount);
            ownedGear[i].UpdateSlotName(startingGearPieces[i].gearName);
            ownedGear[i].UpdateGearBonusHealth(startingGearPieces[i].bonusHealth);
            ownedGear[i].UpdateGearBonusDamage(startingGearPieces[i].bonusDamage);
            ownedGear[i].UpdateGearBonusHealing(startingGearPieces[i].bonusHealing);
            ownedGear[i].UpdateGearBonusDefense(startingGearPieces[i].bonusDefense);
            ownedGear[i].UpdateGearBonusSpeed(startingGearPieces[i].bonusSpeed);
            ownedGear[i].UpdateGearStatis(Slot.SlotStatis.OWNED);
            ownedGear[i].ToggleEquipButton(false);
            ownedGear[i].linkedGearPiece = startingGearPieces[i];

        }

        for (int x = 0; x < startingItemPieces.Count; x++)
        {
            GameObject go = Instantiate(GearRewards.Instance.itemGO, ownedLootUI.transform);
            //ownedItems[x].UpdateSlotCode(TeamItemsManager.Instance.GetItemsSpawned());

            // 1000 pos due to objects spawning in screen and blocking input (they are invisible data carrying objects)
            go.transform.localPosition = new Vector2(1000, 1000);
            go.transform.localScale = Vector2.one;

            Slot item = go.GetComponent<Slot>();
            ownedItems.Add(item);

            //item.UpdateGearAlpha(false);

            TeamItemsManager.Instance.IncItemsSpawned();
            ownedItems[x].UpdateSlotCode(TeamItemsManager.Instance.GetItemsSpawned());

            ownedItems[x].UpdateSlotImage(startingItemPieces[x].itemSpriteItemTab);

            ownedItems[x].UpdateSlotName(startingItemPieces[x].itemName);
            ownedItems[x].linkedItemPiece = startingItemPieces[x];
            ownedItems[x].gameObject.name = startingItemPieces[x].itemName + " " + TeamItemsManager.Instance.GetItemsSpawned();
            ownedItems[x].ToggleEquipButton(false);
        }
    }

    private void Start()
    {
        //ToggleOwnedGearDisplay(false);
        ToggleOwnedSlotEquipButton(false);


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

    public void AddOwnedItems(Slot item)
    {
        ownedItems.Add(item);
    }

    public void ResetOwnedItems()
    {
        ownedItems.Clear();
    }

    public void AddOwnedGear(Slot gear)
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
    public void ToggleOwnedGearDisplay(bool toggle, string titleText = "", bool disableSelections = false)
    {
        if (toggle)
        {
            if (titleText == "Owned Gear")
                FillOwnedLootSlots(0);
            else if (titleText == "Owned Items")
                FillOwnedLootSlots(1);
            else if (titleText == "Ally Skills")
            {
                if (SkillsTabManager.Instance.activeSkillBase != null)
                    FillOwnedLootSlots(2);
                else
                    return;
            }

            //Debug.Log("starting 3");
            ownedLootUI.UpdateAlpha(1);

            ownedLootOpened = true;

            UpdateOwnedTitleTextUI(titleText);

        }
        else
        {
            ownedLootUI.UpdateAlpha(0);

            TeamItemsManager.Instance.UpdateItemNameText("");
            TeamItemsManager.Instance.UpdateItemDesc("");

            TeamItemsManager.Instance.ToggleItemRarityText(false);

            SkillsTabManager.Instance.ToggleSelectedSlotDetailsButton(false);
            ToggleOwnedSlotEquipButton(false);
            ownedLootOpened = false;

            //TeamGearManager.Instance.ToggleGearButtons(false);

            if (disableSelections)
            {
                // Update UI
                //SkillsTabManager.Instance.activeSkillBase = null;
                SkillsTabManager.Instance.UpdateSkillStatDetails();
                SkillsTabManager.Instance.UpdateActiveSkillNameText("");
                SkillsTabManager.Instance.ResetSkillsBaseSelection();
            }

            //TeamItemsManager.Instance.ToggleTeamItems(true);
        }

        TeamGearManager.Instance.ToggleMainSlotRarityBorder();
        TeamItemsManager.Instance.ToggleMainSlotRarityBorder();

        buttonExitOwnedGear.ToggleButton(toggle);
    }

    public void ClearOwnedItemsSlotsSelection()
    {
        for (int i = 0; i < ownedLootSlots.Count; i++)
        {
            ownedLootSlots[i].ToggleSlotSelection(false);
        }
    }

    public void ClearOwnedItemSlots()
    {
        for (int x = 0; x < ownedLootSlots.Count; x++)
        {
            ownedLootSlots[x].ResetSlot(true, false);

            ownedLootSlots[x].UpdateLinkedItemPiece(null);
            ownedLootSlots[x].UpdateLinkedGearPiece(null);
            ownedLootSlots[x].linkedSlot = null;
            ownedLootSlots[x].UpdateSlotDetails();
        }
    }

    public void EnableOwnedItemsSlotSelection(Slot ownedSlot)
    {
        ownedSlot.ToggleSlotSelection(true);
    }

    public void FillOwnedLootSlots(int slotType = 0)
    {
        //Debug.Log("starting 2");
        //ClearOwnedItemSlots();
        TeamItemsManager.Instance.UpdateItemNameText("");
        TeamItemsManager.Instance.UpdateItemDesc("");
        TeamItemsManager.Instance.ToggleTeamItemEquipMainButton(false);
        //ClearOwnedItemsSlotsSelection();

        int ownedItemSlotIndex = 0;

        ClearOwnedItemSlots();

        // If item
        if (slotType == 1)
        {
            ownedItemSlotIndex = 0;
            wornSkillsAlly.Clear();

            for (int i = 0; i < ownedLootSlots.Count; i++)
            {
                ownedLootSlots[i].curGearType = Slot.SlotPieceType.ITEM;
            }

            TeamGearManager.Instance.ToggleAllSlotsClickable(true, true, false);

            // int index = 0;
            for (int i = 0; i < ownedLootSlots.Count; i++)
            {
                // Safety
                if (ownedItems.Count > i)
                {

                    ToggleOwnedSlotEquipButton(true);
                    ItemPiece itemPiece = ownedItems[i].linkedItemPiece;
                    ownedItems[i].isDiscovered = true;

                    //Debug.Log("owned skills count " + ownedSkills.Count);
                    //Debug.Log("index " + ownedGearSlotIndex);

                    if (ownedItems.Count > ownedItemSlotIndex)
                    {
                        #region Toggle Cover UI
                        ownedLootSlots[ownedItemSlotIndex].ToggleCoverUI(false);

                        // If selected base slot is MAIN, and owned item does not equal human
                        if (TeamItemsManager.Instance.GetSelectedBaseItemSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.MAIN
                            && GameManager.Instance.activeTeam[0].curRaceType == UnitData.RaceType.HUMAN
                            && itemPiece.curRace != ItemPiece.RaceSpecific.HUMAN)
                        {
                            ownedLootSlots[ownedItemSlotIndex].ToggleCoverUI(true);
                        }

                        if (TeamItemsManager.Instance.GetSelectedBaseItemSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.MAIN
                            && GameManager.Instance.activeTeam[0].curRaceType == UnitData.RaceType.ETHEREAL
                            && itemPiece.curRace != ItemPiece.RaceSpecific.ETHEREAL)
                        {
                            ownedLootSlots[ownedItemSlotIndex].ToggleCoverUI(true);
                        }

                        if (TeamItemsManager.Instance.GetSelectedBaseItemSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.MAIN
                            && GameManager.Instance.activeTeam[0].curRaceType == UnitData.RaceType.BEAST
                            && itemPiece.curRace != ItemPiece.RaceSpecific.BEAST)
                        {
                            ownedLootSlots[ownedItemSlotIndex].ToggleCoverUI(true);
                        }

                        if (GameManager.Instance.activeTeam.Count >= 2)
                        { 
                            if (TeamItemsManager.Instance.GetSelectedBaseItemSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.SECOND
                            && GameManager.Instance.activeTeam[1].curRaceType == UnitData.RaceType.HUMAN
                            && itemPiece.curRace != ItemPiece.RaceSpecific.HUMAN)
                            {
                                ownedLootSlots[ownedItemSlotIndex].ToggleCoverUI(true);
                            }

                            if (TeamItemsManager.Instance.GetSelectedBaseItemSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.SECOND
                                && GameManager.Instance.activeTeam[1].curRaceType == UnitData.RaceType.ETHEREAL
                                && itemPiece.curRace != ItemPiece.RaceSpecific.ETHEREAL)
                            {
                                ownedLootSlots[ownedItemSlotIndex].ToggleCoverUI(true);
                            }

                            if (TeamItemsManager.Instance.GetSelectedBaseItemSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.SECOND
                                && GameManager.Instance.activeTeam[1].curRaceType == UnitData.RaceType.BEAST
                                && itemPiece.curRace != ItemPiece.RaceSpecific.BEAST)
                            {
                                ownedLootSlots[ownedItemSlotIndex].ToggleCoverUI(true);
                            }
                        }

                        if (GameManager.Instance.activeTeam.Count >= 3)
                        {
                            if (TeamItemsManager.Instance.GetSelectedBaseItemSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.THIRD
                                && GameManager.Instance.activeTeam[2].curRaceType == UnitData.RaceType.HUMAN
                                && itemPiece.curRace != ItemPiece.RaceSpecific.HUMAN)
                            {
                                ownedLootSlots[ownedItemSlotIndex].ToggleCoverUI(true);
                            }

                            if (TeamItemsManager.Instance.GetSelectedBaseItemSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.THIRD
                                && GameManager.Instance.activeTeam[2].curRaceType == UnitData.RaceType.ETHEREAL
                                && itemPiece.curRace != ItemPiece.RaceSpecific.ETHEREAL)
                            {
                                ownedLootSlots[ownedItemSlotIndex].ToggleCoverUI(true);
                            }

                            if (TeamItemsManager.Instance.GetSelectedBaseItemSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.THIRD
                                && GameManager.Instance.activeTeam[2].curRaceType == UnitData.RaceType.BEAST
                                && itemPiece.curRace != ItemPiece.RaceSpecific.BEAST)
                            {
                                ownedLootSlots[ownedItemSlotIndex].ToggleCoverUI(true);
                            }
                        }

                        if (itemPiece.curRace == ItemPiece.RaceSpecific.ALL)
                            ownedLootSlots[ownedItemSlotIndex].ToggleCoverUI(false);

                        #endregion

                        // Update gear icon
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotImage(itemPiece.itemSpriteItemTab);
                        //ownedLootSlots[ownedGearSlotIndex].UpdateCurSlotType(Slot.SlotType.ITEM);
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotName(itemPiece.itemName);

                        ownedLootSlots[ownedItemSlotIndex].UpdateIconSkillSize(false);

                        ownedLootSlots[ownedItemSlotIndex].UpdateGearStatis(Slot.SlotStatis.OWNED);
                        ownedLootSlots[ownedItemSlotIndex].ToggleEquipButton(false);
                        ownedLootSlots[ownedItemSlotIndex].isEmpty = false;
                        ownedLootSlots[ownedItemSlotIndex].UpdateLinkedItemPiece(itemPiece);

                        // Update owned item Race Icon
                        if (ownedLootSlots[ownedItemSlotIndex].linkedItemPiece.curRace == ItemPiece.RaceSpecific.HUMAN)
                        {
                            ownedLootSlots[ownedItemSlotIndex].UpdateRaceIcon(GameManager.Instance.humanRaceIcon);
                        }
                        else if (ownedLootSlots[ownedItemSlotIndex].linkedItemPiece.curRace == ItemPiece.RaceSpecific.BEAST)
                        {
                            ownedLootSlots[ownedItemSlotIndex].UpdateRaceIcon(GameManager.Instance.beastRaceIcon);
                        }
                        else if (ownedLootSlots[ownedItemSlotIndex].linkedItemPiece.curRace == ItemPiece.RaceSpecific.ETHEREAL)
                        {
                            ownedLootSlots[ownedItemSlotIndex].UpdateRaceIcon(GameManager.Instance.etherealRaceIcon);
                        }
                        else if (ownedLootSlots[ownedItemSlotIndex].linkedItemPiece.curRace == ItemPiece.RaceSpecific.ALL)
                        {
                            ownedLootSlots[ownedItemSlotIndex].UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);
                        }

                        if (ownedLootSlots[ownedItemSlotIndex].linkedItemPiece.curRarity == ItemPiece.Rarity.common)
                            ownedLootSlots[ownedItemSlotIndex].UpdateRarityBG(Slot.ItemRarity.common);
                        else if (ownedLootSlots[ownedItemSlotIndex].linkedItemPiece.curRarity == ItemPiece.Rarity.rare)
                            ownedLootSlots[ownedItemSlotIndex].UpdateRarityBG(Slot.ItemRarity.rare);
                        else if (ownedLootSlots[ownedItemSlotIndex].linkedItemPiece.curRarity == ItemPiece.Rarity.epic)
                            ownedLootSlots[ownedItemSlotIndex].UpdateRarityBG(Slot.ItemRarity.epic);
                        else if (ownedLootSlots[ownedItemSlotIndex].linkedItemPiece.curRarity == ItemPiece.Rarity.legendary)
                            ownedLootSlots[ownedItemSlotIndex].UpdateRarityBG(Slot.ItemRarity.legendary, false);

                        ownedLootSlots[i].linkedSlot = ownedItems[i];
                        ownedItems[i].isDiscovered = true;

                        //ownedLootSlots[ownedItemSlotIndex].UpdateSlotCode()
                        //TeamItemsManager.Instance.UpdateOwnedSlotsLinkedSlot();

                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotDetails();
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotBGRarity();

                        ownedItemSlotIndex++;
                    }
                }
            }

            for (int i = 0; i < ownedLootSlots.Count; i++)
            {
                if (ownedLootSlots[i].isEmpty)
                {
                    ownedLootSlots[i].UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);
                    ownedLootSlots[i].ToggleCoverUI(false);
                    ownedLootSlots[i].UpdateRarityBG(Slot.ItemRarity.common, true);
                    ownedLootSlots[i].UpdateSlotBGRarity();
                }
            }
        }
        // If skill
        else if (slotType == 2)
        {
            int index = 0;

            skills.Clear();

            ownedSkills.Clear();
            List<SkillData> ownedSkillData = new List<SkillData>();
            ownedItemSlotIndex = 0;
            wornSkillsAlly.Clear();

            for (int x = 0; x < 4; x++)
            {
                ownedSkills.Add(GameManager.Instance.GetActiveAlly().GetSkill(x));
            }

            for (int i = 0; i < ownedLootSlots.Count; i++)
            {
                ownedLootSlots[i].curGearType = Slot.SlotPieceType.SKILL;
            }

            TeamGearManager.Instance.ToggleAllSlotsClickable(true, true, false);

            // int index = 0;
            for (int i = 0; i < ownedLootSlots.Count; i++)
            {
                
                if (SkillsTabManager.Instance.GetSelectedSkillSlot() == null)
                {
                    Debug.LogError("No selected skill slot when opening owned loot");
                    break;
                }



                // Safety
                if (ownedSkills.Count > i)
                {
                    // If unit is currently selecting a skill, AND check if not already hidden, then dont display it in owned skills
                    if (ownedSkills[i].skillName == SkillsTabManager.Instance.activeSkillBase.skillName && !ownedSkillData.Contains(ownedSkills[i]))
                    {
                        ownedSkillData.Add(ownedSkills[i]);
                        //ToggleOwnedGearEquipButton(false);


                        ownedLootSlots[i].ToggleEquipButton(false);
                        ownedLootSlots[i].UpdateSlotImage(TeamGearManager.Instance.clearSlotSprite);
                        //ownedLootSlots[i].UpdateCurSlotType(Slot.SlotType.EMPTY);
                        ownedLootSlots[i].isEmpty = true;

                        //index++;
                        //Debug.Log(SkillsTabManager.Instance.activeSkillBase.skillName);
                    }

                    // Ensure only the active unit's skills show in owned skills
                    else
                    {
                        // if an item is equipped, skip it in the own inventory display
                        //if (!gears.Contains(wornGear[i]))                      

                        ToggleOwnedSlotEquipButton(true);

                        //Debug.Log("owned skills count " + ownedSkills.Count);
                        //Debug.Log("index " + ownedGearSlotIndex);

                        if (ownedSkills.Count > ownedItemSlotIndex)
                        {
                            /*
                            if (ownedSkillData.Contains(ownedSkills[i]))
                            {
                                if (i != 0)
                                    i--;
                            }
                            */

                            //wornSkillsAlly.Add(SkillsTabManager.Instance.selectedOwnedSlot);
                            // Update gear icon
                            ownedLootSlots[ownedItemSlotIndex].UpdateSlotImage(ownedSkills[i].skillSprite);
                            //ownedLootSlots[ownedGearSlotIndex].UpdateCurSlotType(Slot.SlotType.ITEM);
                            ownedLootSlots[ownedItemSlotIndex].UpdateSlotName(ownedSkills[i].skillName);

                            ownedLootSlots[ownedItemSlotIndex].UpdateIconSkillSize(true);
                            /*
                            ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusHealth(ownedSkills[i].GetBonusHealth());
                            ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusDamage(ownedSkills[i].GetBonusDamage());
                            ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusHealing(ownedSkills[i].GetBonusHealing());
                            ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusDefense(ownedSkills[i].GetBonusDefense());
                            ownedLootSlots[ownedGearSlotIndex].UpdateGearBonusSpeed(ownedSkills[i].GetBonusSpeed());
                            */
                            ownedLootSlots[ownedItemSlotIndex].UpdateGearStatis(Slot.SlotStatis.OWNED);
                            ownedLootSlots[ownedItemSlotIndex].ToggleEquipButton(false);
                            ownedLootSlots[ownedItemSlotIndex].isEmpty = false;

                            ownedLootSlots[ownedItemSlotIndex].skill = ownedSkills[i];

                            ownedLootSlots[ownedItemSlotIndex].UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);
                            ownedLootSlots[ownedItemSlotIndex].UpdateSlotDetails();

                            ownedItemSlotIndex++;
                        }
                    }
                }

                if (ownedLootSlots[ownedItemSlotIndex].isEmpty)
                    ownedLootSlots[ownedItemSlotIndex].UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);

                ownedLootSlots[i].UpdateSlotDetails();

            }
            for (int i = 0; i < ownedLootSlots.Count; i++)
            {
                if (ownedLootSlots[i].isEmpty)
                {
                    ownedLootSlots[i].UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);
                    ownedLootSlots[i].ToggleCoverUI(false);
                    ownedLootSlots[i].UpdateRarityBG(Slot.ItemRarity.common, true);
                }
            }



        }
        // If Gear
        else if (slotType == 0)
        {
            //Debug.Log("starting 1");
            int index = 0;

            gears.Clear();

            for (int i = 0; i < ownedLootSlots.Count; i++)
            {
                ownedLootSlots[i].curGearType = Slot.SlotPieceType.helmet;
            }

            TeamGearManager.Instance.ToggleAllSlotsClickable(true, true);

            // int index = 0;
            for (int i = 0; i < ownedGear.Count; i++)
            {
                if (ownedItemSlotIndex > ownedLootSlots.Count-1)
                    break;

                //ebug.Log("looping owned gear");
                if (TeamGearManager.Instance.GetSelectedGearSlot() == null)
                {
                    Debug.LogError("No Selected gear slot when opening owned gear");
                    break;
                }

                ownedLootSlots[ownedItemSlotIndex].UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);

                // if an item is equipped, skip it in the own inventory display
                //if (!gears.Contains(wornGear[i]))

                //ToggleOwnedGearEquipButton(true);
                // If selected armor piece is helmet, display only owned helmets
                if (TeamGearManager.Instance.GetSelectedGearSlot().GetCurGearType() == Slot.SlotPieceType.helmet)
                {
                    if (ownedGear[i].linkedGearPiece.gearType == "helmet" ||
                        ownedGear[i].linkedGearPiece.gearType == "HELMET")
                    {
                        //Debug.Log("loading owned gear helm");
                        // Update gear icon
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotImage(ownedGear[i].linkedGearPiece.gearIcon);
                        ownedLootSlots[ownedItemSlotIndex].UpdateCurSlotType(Slot.SlotPieceType.helmet);
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotName(ownedGear[i].linkedGearPiece.gearName);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusHealth(ownedGear[i].linkedGearPiece.bonusHealth);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusDamage(ownedGear[i].linkedGearPiece.bonusDamage);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusHealing(ownedGear[i].linkedGearPiece.bonusHealing);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusDefense(ownedGear[i].linkedGearPiece.bonusDefense);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusSpeed(ownedGear[i].linkedGearPiece.bonusSpeed);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearStatis(Slot.SlotStatis.OWNED);
                        ownedLootSlots[ownedItemSlotIndex].ToggleEquipButton(true);
                        ownedLootSlots[ownedItemSlotIndex].isEmpty = false;
                        ownedLootSlots[ownedItemSlotIndex].ToggleOwnedGearButton(true);
                        ownedLootSlots[ownedItemSlotIndex].linkedGearPiece = ownedGear[i].linkedGearPiece;
                        ownedLootSlots[ownedItemSlotIndex].UpdateIconSkillSize(false);
                        ownedGear[i].isDiscovered = true;
                        ownedItemSlotIndex++;
                    }
                }
                else if (TeamGearManager.Instance.GetSelectedGearSlot().GetCurGearType() == Slot.SlotPieceType.chestpiece)
                {
                    if (ownedGear[i].linkedGearPiece.gearType == "chestpiece" ||
                        ownedGear[i].linkedGearPiece.gearType == "CHESTPIECE")
                    {
                        //Debug.Log("loading owned gear helm");
                        // Update gear icon
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotImage(ownedGear[i].linkedGearPiece.gearIcon);
                        ownedLootSlots[ownedItemSlotIndex].UpdateCurSlotType(Slot.SlotPieceType.chestpiece);
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotName(ownedGear[i].linkedGearPiece.gearName);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusHealth(ownedGear[i].linkedGearPiece.bonusHealth);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusDamage(ownedGear[i].linkedGearPiece.bonusDamage);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusHealing(ownedGear[i].linkedGearPiece.bonusHealing);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusDefense(ownedGear[i].linkedGearPiece.bonusDefense);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusSpeed(ownedGear[i].linkedGearPiece.bonusSpeed);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearStatis(Slot.SlotStatis.OWNED);
                        ownedLootSlots[ownedItemSlotIndex].ToggleEquipButton(true);
                        ownedLootSlots[ownedItemSlotIndex].isEmpty = false;
                        ownedLootSlots[ownedItemSlotIndex].ToggleOwnedGearButton(true);
                        ownedLootSlots[ownedItemSlotIndex].linkedGearPiece = ownedGear[i].linkedGearPiece;
                        ownedLootSlots[ownedItemSlotIndex].UpdateIconSkillSize(false);
                        ownedGear[i].isDiscovered = true;
                        ownedItemSlotIndex++;
                    }
                }
                else if (TeamGearManager.Instance.GetSelectedGearSlot().GetCurGearType() == Slot.SlotPieceType.boots)
                {
                    if (ownedGear[i].linkedGearPiece.gearType == "boots" ||
                        ownedGear[i].linkedGearPiece.gearType == "BOOTS")
                    {
                        //Debug.Log("loading owned gear helm");
                        // Update gear icon
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotImage(ownedGear[i].linkedGearPiece.gearIcon);
                        ownedLootSlots[ownedItemSlotIndex].UpdateCurSlotType(Slot.SlotPieceType.boots);
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotName(ownedGear[i].linkedGearPiece.gearName);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusHealth(ownedGear[i].linkedGearPiece.bonusHealth);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusDamage(ownedGear[i].linkedGearPiece.bonusDamage);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusHealing(ownedGear[i].linkedGearPiece.bonusHealing);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusDefense(ownedGear[i].linkedGearPiece.bonusDefense);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusSpeed(ownedGear[i].linkedGearPiece.bonusSpeed);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearStatis(Slot.SlotStatis.OWNED);
                        ownedLootSlots[ownedItemSlotIndex].ToggleEquipButton(true);
                        ownedLootSlots[ownedItemSlotIndex].isEmpty = false;
                        ownedLootSlots[ownedItemSlotIndex].ToggleOwnedGearButton(true);
                        ownedLootSlots[ownedItemSlotIndex].linkedGearPiece = ownedGear[i].linkedGearPiece;
                        ownedLootSlots[ownedItemSlotIndex].UpdateIconSkillSize(false);
                        ownedGear[i].isDiscovered = true;
                        ownedItemSlotIndex++;
                    }
                }
                else if (TeamGearManager.Instance.GetSelectedGearSlot().GetCurGearType() == Slot.SlotPieceType.neckless)
                {
                    if (ownedGear[i].linkedGearPiece.gearType == "neckless" ||
                        ownedGear[i].linkedGearPiece.gearType == "NECKLESS" ||
                        ownedGear[i].linkedGearPiece.gearType == "pendant" ||
                        ownedGear[i].linkedGearPiece.gearType == "PENDANT")
                    {
                        //Debug.Log("loading owned gear helm");
                        // Update gear icon
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotImage(ownedGear[i].linkedGearPiece.gearIcon);
                        ownedLootSlots[ownedItemSlotIndex].UpdateCurSlotType(Slot.SlotPieceType.neckless);
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotName(ownedGear[i].linkedGearPiece.gearName);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusHealth(ownedGear[i].linkedGearPiece.bonusHealth);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusDamage(ownedGear[i].linkedGearPiece.bonusDamage);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusHealing(ownedGear[i].linkedGearPiece.bonusHealing);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusDefense(ownedGear[i].linkedGearPiece.bonusDefense);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusSpeed(ownedGear[i].linkedGearPiece.bonusSpeed);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearStatis(Slot.SlotStatis.OWNED);
                        ownedLootSlots[ownedItemSlotIndex].ToggleEquipButton(true);
                        ownedLootSlots[ownedItemSlotIndex].isEmpty = false;
                        ownedLootSlots[ownedItemSlotIndex].ToggleOwnedGearButton(true);
                        ownedLootSlots[ownedItemSlotIndex].linkedGearPiece = ownedGear[i].linkedGearPiece;
                        ownedLootSlots[ownedItemSlotIndex].UpdateIconSkillSize(false);
                        ownedGear[i].isDiscovered = true;
                        ownedItemSlotIndex++;
                    }
                }
                else if (TeamGearManager.Instance.GetSelectedGearSlot().GetCurGearType() == Slot.SlotPieceType.earring)
                {
                    if (ownedGear[i].linkedGearPiece.gearType == "earring" ||
                        ownedGear[i].linkedGearPiece.gearType == "EARRING")
                    {
                        //Debug.Log("loading owned gear helm");
                        // Update gear icon
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotImage(ownedGear[i].linkedGearPiece.gearIcon);
                        ownedLootSlots[ownedItemSlotIndex].UpdateCurSlotType(Slot.SlotPieceType.earring);
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotName(ownedGear[i].linkedGearPiece.gearName);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusHealth(ownedGear[i].linkedGearPiece.bonusHealth);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusDamage(ownedGear[i].linkedGearPiece.bonusDamage);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusHealing(ownedGear[i].linkedGearPiece.bonusHealing);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusDefense(ownedGear[i].linkedGearPiece.bonusDefense);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusSpeed(ownedGear[i].linkedGearPiece.bonusSpeed);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearStatis(Slot.SlotStatis.OWNED);
                        ownedLootSlots[ownedItemSlotIndex].ToggleEquipButton(true);
                        ownedLootSlots[ownedItemSlotIndex].isEmpty = false;
                        ownedLootSlots[ownedItemSlotIndex].ToggleOwnedGearButton(true);
                        ownedLootSlots[ownedItemSlotIndex].linkedGearPiece = ownedGear[i].linkedGearPiece;
                        ownedLootSlots[ownedItemSlotIndex].UpdateIconSkillSize(false);
                        ownedGear[i].isDiscovered = true;
                        ownedItemSlotIndex++;
                    }
                }
                else if (TeamGearManager.Instance.GetSelectedGearSlot().GetCurGearType() == Slot.SlotPieceType.belt)
                {
                    if (ownedGear[i].linkedGearPiece.gearType == "belt" ||
                        ownedGear[i].linkedGearPiece.gearType == "BELT")
                    {
                        //Debug.Log("loading owned gear helm");
                        // Update gear icon
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotImage(ownedGear[i].linkedGearPiece.gearIcon);
                        ownedLootSlots[ownedItemSlotIndex].UpdateCurSlotType(Slot.SlotPieceType.belt);
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotName(ownedGear[i].linkedGearPiece.gearName);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusHealth(ownedGear[i].linkedGearPiece.bonusHealth);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusDamage(ownedGear[i].linkedGearPiece.bonusDamage);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusHealing(ownedGear[i].linkedGearPiece.bonusHealing);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusDefense(ownedGear[i].linkedGearPiece.bonusDefense);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusSpeed(ownedGear[i].linkedGearPiece.bonusSpeed);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearStatis(Slot.SlotStatis.OWNED);
                        ownedLootSlots[ownedItemSlotIndex].ToggleEquipButton(true);
                        ownedLootSlots[ownedItemSlotIndex].isEmpty = false;
                        ownedLootSlots[ownedItemSlotIndex].ToggleOwnedGearButton(true);
                        ownedLootSlots[ownedItemSlotIndex].linkedGearPiece = ownedGear[i].linkedGearPiece;
                        ownedLootSlots[ownedItemSlotIndex].UpdateIconSkillSize(false);
                        ownedGear[i].isDiscovered = true;
                        ownedItemSlotIndex++;
                    }
                }
                else if (TeamGearManager.Instance.GetSelectedGearSlot().GetCurGearType() == Slot.SlotPieceType.glove)
                {
                    if (ownedGear[i].linkedGearPiece.gearType == "glove" ||
                        ownedGear[i].linkedGearPiece.gearType == "GLOVE")
                    {
                        //Debug.Log("loading owned gear helm");
                        // Update gear icon
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotImage(ownedGear[i].linkedGearPiece.gearIcon);
                        ownedLootSlots[ownedItemSlotIndex].UpdateCurSlotType(Slot.SlotPieceType.glove);
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotName(ownedGear[i].linkedGearPiece.gearName);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusHealth(ownedGear[i].linkedGearPiece.bonusHealth);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusDamage(ownedGear[i].linkedGearPiece.bonusDamage);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusHealing(ownedGear[i].linkedGearPiece.bonusHealing);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusDefense(ownedGear[i].linkedGearPiece.bonusDefense);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusSpeed(ownedGear[i].linkedGearPiece.bonusSpeed);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearStatis(Slot.SlotStatis.OWNED);
                        ownedLootSlots[ownedItemSlotIndex].ToggleEquipButton(true);
                        ownedLootSlots[ownedItemSlotIndex].isEmpty = false;
                        ownedLootSlots[ownedItemSlotIndex].ToggleOwnedGearButton(true);
                        ownedLootSlots[ownedItemSlotIndex].linkedGearPiece = ownedGear[i].linkedGearPiece;
                        ownedLootSlots[ownedItemSlotIndex].UpdateIconSkillSize(false);
                        ownedGear[i].isDiscovered = true;
                        ownedItemSlotIndex++;
                    }
                }
                else if (TeamGearManager.Instance.GetSelectedGearSlot().GetCurGearType() == Slot.SlotPieceType.ring)
                {
                    if (ownedGear[i].linkedGearPiece.gearType == "ring" ||
                        ownedGear[i].linkedGearPiece.gearType == "RING")
                    {
                        //Debug.Log("loading owned gear helm");
                        // Update gear icon
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotImage(ownedGear[i].linkedGearPiece.gearIcon);
                        ownedLootSlots[ownedItemSlotIndex].UpdateCurSlotType(Slot.SlotPieceType.ring);
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotName(ownedGear[i].linkedGearPiece.gearName);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusHealth(ownedGear[i].linkedGearPiece.bonusHealth);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusDamage(ownedGear[i].linkedGearPiece.bonusDamage);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusHealing(ownedGear[i].linkedGearPiece.bonusHealing);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusDefense(ownedGear[i].linkedGearPiece.bonusDefense);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusSpeed(ownedGear[i].linkedGearPiece.bonusSpeed);
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearStatis(Slot.SlotStatis.OWNED);
                        ownedLootSlots[ownedItemSlotIndex].ToggleEquipButton(true);
                        ownedLootSlots[ownedItemSlotIndex].isEmpty = false;
                        ownedLootSlots[ownedItemSlotIndex].ToggleOwnedGearButton(true);
                        ownedLootSlots[ownedItemSlotIndex].linkedGearPiece = ownedGear[i].linkedGearPiece;
                        ownedLootSlots[ownedItemSlotIndex].UpdateIconSkillSize(false);
                        ownedGear[i].isDiscovered = true;
                        ownedItemSlotIndex++;
                    }
                }

                if (ownedGear[i] == null)
                {
                    ownedLootSlots[ownedItemSlotIndex].ToggleEquipButton(false);
                    ownedLootSlots[ownedItemSlotIndex].UpdateSlotImage(TeamGearManager.Instance.clearSlotSprite);
                    ownedLootSlots[ownedItemSlotIndex].UpdateCurSlotType(Slot.SlotPieceType.EMPTY);
                    ownedLootSlots[ownedItemSlotIndex].isEmpty = true;
                    ownedLootSlots[ownedItemSlotIndex].linkedGearPiece = null;
                    ownedItemSlotIndex++;
                }
            }

            for (int x = 0; x < ownedLootSlots.Count; x++)
            {
                ownedLootSlots[x].UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);
                //ownedLootSlots[x].UpdateRarityBG(Slot.ItemRarity.COMMON, true);
                if (!ownedLootSlots[x].isEmpty)
                {
                    ownedLootSlots[x].UpdateSlotBGRarity();
                    ownedLootSlots[x].ToggleRarityBorder(true);
                    ownedLootSlots[x].UpdateRarityBorderColour();
                    ownedLootSlots[x].UpdateSlotDetails();
                    //ownedLootSlots[x].isDiscovered = true;
                }
                else
                {
                    ownedLootSlots[x].UpdateSlotBGRarity();
                    ownedLootSlots[x].ToggleRarityBorder(false);
                }
            }
        }

        // Make all empty owned gear slots display nothing 
        for (int x = 0; x < ownedLootSlots.Count; x++)
        {
            //Debug.Log("setting empty");
            ownedLootSlots[x].ToggleEquipButton(false);

            //if (ownedLootSlots[x].isEmpty)
            //ownedLootSlots[x].UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);


            if (ownedLootSlots[x].isEmpty)
            {
                //Debug.Log("setting empty");
                ownedLootSlots[x].ToggleEquipButton(false);
                //ToggleOwnedGearEquipButton(false);
                ownedLootSlots[x].UpdateSlotImage(TeamGearManager.Instance.clearSlotSprite);
                //ownedGear[i].UpdateCurGearType(Gear.GearType.EMPTY);
                ownedLootSlots[x].isEmpty = true;
                ownedLootSlots[x].UpdateSlotBGRarity();
            }
        }
    }

    public void ToggleOwnedSlotEquipButton(bool toggle)
    {
        for (int i = 0; i < ownedLootSlots.Count; i++)
        {
            ownedLootSlots[i].gameObject.GetComponent<UIElement>().ToggleButton(toggle);
            ownedLootSlots[i].gameObject.GetComponent<UIElement>().ToggleButton2(toggle);
        }
    }

    public void ResetOwnedSlotEquipButton()
    {
        for (int i = 0; i < ownedLootSlots.Count; i++)
        {
            ownedLootSlots[i].ToggleEquipButton(false);
        }
    }
}
