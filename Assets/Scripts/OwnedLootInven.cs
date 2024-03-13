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
    public List<GearPiece> ownedGearPieces = new List<GearPiece>();
    public List<ItemPiece> ownedItemPieces = new List<ItemPiece>();

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

    [SerializeField] private UIElement buttonExitOwnedGear;

    private void Awake()
    {
        Instance = this;
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
        ownedGear.Remove(gear);
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
                ownedGear[newIndex].UpdateCurSlotType(Slot.SlotType.HELMET);
            else if (trans.GetChild(i).GetComponent<Slot>().linkedGearPiece.gearType == "chestpiece")
                ownedGear[newIndex].UpdateCurSlotType(Slot.SlotType.CHESTPIECE);
            else if (trans.GetChild(i).GetComponent<Slot>().linkedGearPiece.gearType == "boots")
                ownedGear[newIndex].UpdateCurSlotType(Slot.SlotType.BOOTS);

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

    public void MoveGearAndItemsToOwnedLoot(UnitFunctionality unit)
    {
        for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
        {
            if (unit.GetUnitName() == GameManager.Instance.activeRoomHeroes[i].GetUnitName())
            {
                if (i == 0)
                {
                    // Items
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
                                gear.UpdateSlotImage(GearRewards.Instance.allItemPiecesCommon[a].itemSprite);
                                gear.UpdateCurSlotType(Slot.SlotType.ITEM);
                                GearRewards.Instance.IncrementSpawnedGearCount();
                                //gear.UpdateSlotCode(GearRewards.Instance.spawnedGearCount);
                                gear.UpdateSlotName(GearRewards.Instance.allItemPiecesCommon[a].itemName);
                                gear.UpdateGearStatis(Slot.SlotStatis.OWNED);
                                gear.UpdateLinkedItemPiece(GearRewards.Instance.allItemPiecesCommon[a]);
                                gear.ToggleEquipButton(false);

                                Debug.Log("adding " + gear.GetSlotName());
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
                                gear.UpdateSlotImage(GearRewards.Instance.allItemPiecesRare[b].itemSprite);
                                gear.UpdateCurSlotType(Slot.SlotType.ITEM);
                                //GearRewards.Instance.IncrementSpawnedGearCount();
                                gear.UpdateSlotCode(GearRewards.Instance.spawnedGearCount);
                                gear.UpdateSlotName(GearRewards.Instance.allItemPiecesRare[b].itemName);
                                gear.UpdateGearStatis(Slot.SlotStatis.OWNED);
                                gear.UpdateLinkedItemPiece(GearRewards.Instance.allItemPiecesRare[b]);
                                gear.ToggleEquipButton(false);

                                Debug.Log("adding " + gear.GetSlotName());
                            }
                        }
                    }

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
                                    gear.UpdateCurSlotType(Slot.SlotType.HELMET);
                                else if (GearRewards.Instance.allGearPiecesCommon[a].gearType == "chestpiece")
                                    gear.UpdateCurSlotType(Slot.SlotType.CHESTPIECE);
                                else if (GearRewards.Instance.allGearPiecesCommon[a].gearType == "boots")
                                    gear.UpdateCurSlotType(Slot.SlotType.BOOTS);

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
                                    gear.UpdateCurSlotType(Slot.SlotType.HELMET);
                                else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "chestpiece")
                                    gear.UpdateCurSlotType(Slot.SlotType.CHESTPIECE);
                                else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "boots")
                                    gear.UpdateCurSlotType(Slot.SlotType.BOOTS);

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
                else if (i == 1)
                {
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
                                gear.UpdateSlotImage(GearRewards.Instance.allItemPiecesCommon[a].itemSprite);
                                gear.UpdateCurSlotType(Slot.SlotType.ITEM);
                                //GearRewards.Instance.IncrementSpawnedGearCount();
                                gear.UpdateSlotCode(GearRewards.Instance.spawnedGearCount);
                                gear.UpdateSlotName(GearRewards.Instance.allItemPiecesCommon[a].itemName);
                                gear.UpdateGearStatis(Slot.SlotStatis.OWNED);
                                gear.UpdateLinkedItemPiece(GearRewards.Instance.allItemPiecesCommon[a]);
                                gear.ToggleEquipButton(false);

                                Debug.Log("adding " + gear.GetSlotName());
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
                                gear.UpdateSlotImage(GearRewards.Instance.allItemPiecesRare[b].itemSprite);
                                gear.UpdateCurSlotType(Slot.SlotType.ITEM);
                                //GearRewards.Instance.IncrementSpawnedGearCount();
                                gear.UpdateSlotCode(GearRewards.Instance.spawnedGearCount);
                                gear.UpdateSlotName(GearRewards.Instance.allItemPiecesRare[b].itemName);
                                gear.UpdateGearStatis(Slot.SlotStatis.OWNED);
                                gear.UpdateLinkedItemPiece(GearRewards.Instance.allItemPiecesRare[b]);
                                gear.ToggleEquipButton(false);

                                Debug.Log("adding " + gear.GetSlotName());
                            }
                        }
                    }

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
                                    gear.UpdateCurSlotType(Slot.SlotType.HELMET);
                                else if (GearRewards.Instance.allGearPiecesCommon[a].gearType == "chestpiece")
                                    gear.UpdateCurSlotType(Slot.SlotType.CHESTPIECE);
                                else if (GearRewards.Instance.allGearPiecesCommon[a].gearType == "boots")
                                    gear.UpdateCurSlotType(Slot.SlotType.BOOTS);

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
                                    gear.UpdateCurSlotType(Slot.SlotType.HELMET);
                                else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "chestpiece")
                                    gear.UpdateCurSlotType(Slot.SlotType.CHESTPIECE);
                                else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "boots")
                                    gear.UpdateCurSlotType(Slot.SlotType.BOOTS);

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
                else if (i == 2)
                {
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
                                gear.UpdateSlotImage(GearRewards.Instance.allItemPiecesCommon[a].itemSprite);
                                gear.UpdateCurSlotType(Slot.SlotType.ITEM);
                                //GearRewards.Instance.IncrementSpawnedGearCount();
                                gear.UpdateSlotCode(GearRewards.Instance.spawnedGearCount);
                                gear.UpdateSlotName(GearRewards.Instance.allItemPiecesCommon[a].itemName);
                                gear.UpdateGearStatis(Slot.SlotStatis.OWNED);
                                gear.UpdateLinkedItemPiece(GearRewards.Instance.allItemPiecesCommon[a]);
                                gear.ToggleEquipButton(false);

                                Debug.Log("adding " + gear.GetSlotName());
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
                                gear.UpdateSlotImage(GearRewards.Instance.allItemPiecesRare[b].itemSprite);
                                gear.UpdateCurSlotType(Slot.SlotType.ITEM);
                                //GearRewards.Instance.IncrementSpawnedGearCount();
                                gear.UpdateSlotCode(GearRewards.Instance.spawnedGearCount);
                                gear.UpdateSlotName(GearRewards.Instance.allItemPiecesRare[b].itemName);
                                gear.UpdateGearStatis(Slot.SlotStatis.OWNED);
                                gear.UpdateLinkedItemPiece(GearRewards.Instance.allItemPiecesRare[b]);
                                gear.ToggleEquipButton(false);

                                Debug.Log("adding " + gear.GetSlotName());
                            }
                        }
                    }

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
                                    gear.UpdateCurSlotType(Slot.SlotType.HELMET);
                                else if (GearRewards.Instance.allGearPiecesCommon[a].gearType == "chestpiece")
                                    gear.UpdateCurSlotType(Slot.SlotType.CHESTPIECE);
                                else if (GearRewards.Instance.allGearPiecesCommon[a].gearType == "boots")
                                    gear.UpdateCurSlotType(Slot.SlotType.BOOTS);

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
                                    gear.UpdateCurSlotType(Slot.SlotType.HELMET);
                                else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "chestpiece")
                                    gear.UpdateCurSlotType(Slot.SlotType.CHESTPIECE);
                                else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "boots")
                                    gear.UpdateCurSlotType(Slot.SlotType.BOOTS);

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



                /*
                else if (i == 1)
                {
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
                                    gear.UpdateCurSlotType(Slot.SlotType.HELMET);
                                else if (GearRewards.Instance.allGearPiecesCommon[a].gearType == "chestpiece")
                                    gear.UpdateCurSlotType(Slot.SlotType.CHESTPIECE);
                                else if (GearRewards.Instance.allGearPiecesCommon[a].gearType == "boots")
                                    gear.UpdateCurSlotType(Slot.SlotType.BOOTS);

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
                                    gear.UpdateCurSlotType(Slot.SlotType.HELMET);
                                else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "chestpiece")
                                    gear.UpdateCurSlotType(Slot.SlotType.CHESTPIECE);
                                else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "boots")
                                    gear.UpdateCurSlotType(Slot.SlotType.BOOTS);

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
                else if (i == 2)
                {
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
                                    gear.UpdateCurSlotType(Slot.SlotType.HELMET);
                                else if (GearRewards.Instance.allGearPiecesCommon[a].gearType == "chestpiece")
                                    gear.UpdateCurSlotType(Slot.SlotType.CHESTPIECE);
                                else if (GearRewards.Instance.allGearPiecesCommon[a].gearType == "boots")
                                    gear.UpdateCurSlotType(Slot.SlotType.BOOTS);

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
                                    gear.UpdateCurSlotType(Slot.SlotType.HELMET);
                                else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "chestpiece")
                                    gear.UpdateCurSlotType(Slot.SlotType.CHESTPIECE);
                                else if (GearRewards.Instance.allGearPiecesRare[b].gearType == "boots")
                                    gear.UpdateCurSlotType(Slot.SlotType.BOOTS);

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
                */
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
                ownedGear[i].UpdateCurSlotType(Slot.SlotType.HELMET);
            else if (startingGearPieces[i].gearType == "chestpiece")
                ownedGear[i].UpdateCurSlotType(Slot.SlotType.CHESTPIECE);
            else if (startingGearPieces[i].gearType == "boots")
                ownedGear[i].UpdateCurSlotType(Slot.SlotType.BOOTS);

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
        }

        for (int x = 0; x < startingItemPieces.Count; x++)
        {
            GameObject go = Instantiate(GearRewards.Instance.itemGO, ownedLootUI.transform);
            // 1000 pos due to objects spawning in screen and blocking input (they are invisible data carrying objects)
            go.transform.localPosition = new Vector2(1000, 1000);
            go.transform.localScale = Vector2.one;

            Slot item = go.GetComponent<Slot>();
            ownedItems.Add(item);

            //item.UpdateGearAlpha(false);

            ownedItems[x].UpdateSlotImage(startingItemPieces[x].itemSprite);

            ownedItems[x].UpdateSlotName(startingItemPieces[x].itemName);
            ownedItems[x].linkedItemPiece = startingItemPieces[x];
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
                FillOwnedGearSlots(0);
            else if (titleText == "Owned Items")
                FillOwnedGearSlots(1);
            else if (titleText == "Ally Skills")
            {
                if (SkillsTabManager.Instance.activeSkillBase != null)
                    FillOwnedGearSlots(2);
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
        }

        buttonExitOwnedGear.ToggleButton(toggle);
    }

    public void ClearOwnedItemsSlotsSelection()
    {
        for (int i = 0; i < ownedLootSlots.Count; i++)
        {
            ownedLootSlots[i].ToggleSlotSelection(false);
        }
    }

    public void EnableOwnedItemsSlotSelection(Slot ownedSlot)
    {
        ownedSlot.ToggleSlotSelection(true);
    }

    public void FillOwnedGearSlots(int slotType = 0)
    {
        //Debug.Log("starting 2");
        //ClearOwnedItemSlots();

        //ClearOwnedItemsSlotsSelection();

        int ownedItemSlotIndex = 0;

        for (int x = 0; x < ownedLootSlots.Count; x++)
        {
            ownedLootSlots[x].ResetSlot(true, false);
        }

        // If item
        if (slotType == 1)
        {
            ownedItemPieces.Clear();
            List<ItemPiece> ownedItemPiece = new List<ItemPiece>();
            ownedItemSlotIndex = 0;
            wornSkillsAlly.Clear();

            for (int x = 0; x < ownedItems.Count; x++)
            {
                ownedItemPieces.Add(ownedItems[x].linkedItemPiece);
            }

            TeamGearManager.Instance.ToggleAllSlotsClickable(true, true, false);

            // int index = 0;
            for (int i = 0; i < ownedLootSlots.Count; i++)
            {
                // Safety
                if (ownedItemPieces.Count > i)
                {
                    /*
                    // If unit is currently selecting a skill, AND check if not already hidden, then dont display it in owned skills
                    if (!ownedItemPiece.Contains(ownedItemPieces[i]))
                    {
                        ownedItemPiece.Add(ownedItemPieces[i]);
                        //ToggleOwnedGearEquipButton(false);


                        ownedLootSlots[i].ToggleEquipButton(false);
                        ownedLootSlots[i].UpdateSlotImage(TeamGearManager.Instance.clearSlotSprite);
                        ownedLootSlots[i].UpdateCurSlotType(Slot.SlotType.EMPTY);
                        ownedLootSlots[i].isEmpty = true;

                        //index++;
                        Debug.Log("aaa");
                    }
                    */
                    // Ensure only the active unit's skills show in owned skills

                    //Debug.Log("bbb");
                    // if an item is equipped, skip it in the own inventory display
                    //if (!gears.Contains(wornGear[i]))                      

                    ToggleOwnedSlotEquipButton(true);

                    //Debug.Log("owned skills count " + ownedSkills.Count);
                    //Debug.Log("index " + ownedGearSlotIndex);

                    if (ownedItemPieces.Count > ownedItemSlotIndex)
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
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotImage(ownedItemPieces[i].itemSprite);
                        //ownedLootSlots[ownedGearSlotIndex].UpdateCurSlotType(Slot.SlotType.ITEM);
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotName(ownedItemPieces[i].itemName);

                        ownedLootSlots[ownedItemSlotIndex].UpdateIconSkillSize(false);
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
                        ownedLootSlots[ownedItemSlotIndex].UpdateLinkedItemPiece(ownedItemPieces[i]);

                        //ownedLootSlots[ownedItemSlotIndex].skill = ownedSkills[i];

                        ownedItemSlotIndex++;
                    }
                }
                /*
                else
                {
                    ownedSkillData.Add(ownedSkills[i]);
                    //ToggleOwnedGearEquipButton(false);


                    ownedLootSlots[i].ToggleEquipButton(false);
                    ownedLootSlots[i].UpdateSlotImage(TeamGearManager.Instance.clearSlotSprite);
                    ownedLootSlots[i].UpdateCurSlotType(Slot.SlotType.EMPTY);
                    ownedLootSlots[i].isEmpty = true;
                }
                */

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
                        ownedLootSlots[i].UpdateCurSlotType(Slot.SlotType.EMPTY);
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

                            ownedItemSlotIndex++;
                        }
                    }
                }
                /*
                else
                {
                    ownedSkillData.Add(ownedSkills[i]);
                    //ToggleOwnedGearEquipButton(false);


                    ownedLootSlots[i].ToggleEquipButton(false);
                    ownedLootSlots[i].UpdateSlotImage(TeamGearManager.Instance.clearSlotSprite);
                    ownedLootSlots[i].UpdateCurSlotType(Slot.SlotType.EMPTY);
                    ownedLootSlots[i].isEmpty = true;
                }
                */

            }
        }
        // If Gear
        else if (slotType == 0)
        {
            //Debug.Log("starting 1");
            int index = 0;

            gears.Clear();

            TeamGearManager.Instance.ToggleAllSlotsClickable(true, true);

            // int index = 0;
            for (int i = 0; i < ownedGear.Count; i++)
            {
                //ebug.Log("looping owned gear");
                if (TeamGearManager.Instance.GetSelectedGearSlot() == null)
                {
                    Debug.LogError("No Selected gear slot when opening owned gear");
                    break;
                }

                // Safety
                if (wornGearMainAlly.Count > i)
                {
                    if (ownedGear.Contains(wornGearMainAlly[i]) && !gears.Contains(wornGearMainAlly[i]))
                    {
                        gears.Add(wornGearMainAlly[i]);
                        //ToggleOwnedGearEquipButton(false);

                        ownedGear[i].ToggleEquipButton(false);
                        ownedGear[i].UpdateSlotImage(TeamGearManager.Instance.clearSlotSprite);
                        ownedGear[i].UpdateCurSlotType(Slot.SlotType.EMPTY);
                        ownedGear[i].isEmpty = true;

                        index++;
                        //Debug.Log("1");
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
                        ownedGear[i].UpdateSlotImage(TeamGearManager.Instance.clearSlotSprite);
                        ownedGear[i].UpdateCurSlotType(Slot.SlotType.EMPTY);
                        ownedGear[i].isEmpty = true;

                        index++;
                        //Debug.Log("2");
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
                        ownedGear[i].UpdateSlotImage(TeamGearManager.Instance.clearSlotSprite);
                        ownedGear[i].UpdateCurSlotType(Slot.SlotType.EMPTY);
                        ownedGear[i].isEmpty = true;

                        index++;
                        //Debug.Log("3");
                        continue;
                    }
                }

                // if an item is equipped, skip it in the own inventory display
                //if (!gears.Contains(wornGear[i]))

                //ToggleOwnedGearEquipButton(true);
                // If selected armor piece is helmet, display only owned helmets
                if (TeamGearManager.Instance.GetSelectedGearSlot().GetCurGearType() == Slot.SlotType.HELMET)
                {
                    if (ownedGear[i].GetCurGearType() == Slot.SlotType.HELMET)
                    {
                        //Debug.Log("loading owned gear helm");
                        // Update gear icon
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotImage(ownedGear[i].GetSlotImage());
                        ownedLootSlots[ownedItemSlotIndex].UpdateCurSlotType(Slot.SlotType.HELMET);
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotName(ownedGear[i].GetSlotName());
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusHealth(ownedGear[i].GetBonusHealth());
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusDamage(ownedGear[i].GetBonusDamage());
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusHealing(ownedGear[i].GetBonusHealing());
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusDefense(ownedGear[i].GetBonusDefense());
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusSpeed(ownedGear[i].GetBonusSpeed());
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearStatis(Slot.SlotStatis.OWNED);
                        ownedLootSlots[ownedItemSlotIndex].ToggleEquipButton(true);
                        ownedLootSlots[ownedItemSlotIndex].isEmpty = false;
                        ownedLootSlots[ownedItemSlotIndex].ToggleOwnedGearButton(true);
                        ownedItemSlotIndex++;
                        //Debug.Log("asdasdasd");
                    }
                }
                else if (TeamGearManager.Instance.GetSelectedGearSlot().GetCurGearType() == Slot.SlotType.CHESTPIECE)
                {
                    if (ownedGear[i].GetCurGearType() == Slot.SlotType.CHESTPIECE)
                    {
                        //Debug.Log("loading owned gear chest");
                        // Update gear icon
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotImage(ownedGear[i].GetSlotImage());
                        ownedLootSlots[ownedItemSlotIndex].UpdateCurSlotType(Slot.SlotType.CHESTPIECE);
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotName(ownedGear[i].GetSlotName());
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusHealth(ownedGear[i].GetBonusHealth());
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusDamage(ownedGear[i].GetBonusDamage());
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusHealing(ownedGear[i].GetBonusHealing());
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusDefense(ownedGear[i].GetBonusDefense());
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusSpeed(ownedGear[i].GetBonusSpeed());
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearStatis(Slot.SlotStatis.OWNED);
                        ownedLootSlots[ownedItemSlotIndex].ToggleEquipButton(true);
                        ownedLootSlots[ownedItemSlotIndex].isEmpty = false;
                        ownedLootSlots[ownedItemSlotIndex].ToggleOwnedGearButton(true);

                        ownedItemSlotIndex++;
                        //Debug.Log("123123123");
                    }
                }
                else if (TeamGearManager.Instance.GetSelectedGearSlot().GetCurGearType() == Slot.SlotType.BOOTS)
                {
                    if (ownedGear[i].GetCurGearType() == Slot.SlotType.BOOTS)
                    {
                        //Debug.Log("loading owned gear boots");
                        // Update gear icon
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotImage(ownedGear[i].GetSlotImage());
                        ownedLootSlots[ownedItemSlotIndex].UpdateCurSlotType(Slot.SlotType.BOOTS);
                        ownedLootSlots[ownedItemSlotIndex].UpdateSlotName(ownedGear[i].GetSlotName());
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusHealth(ownedGear[i].GetBonusHealth());
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusDamage(ownedGear[i].GetBonusDamage());
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusHealing(ownedGear[i].GetBonusHealing());
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusDefense(ownedGear[i].GetBonusDefense());
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearBonusSpeed(ownedGear[i].GetBonusSpeed());
                        ownedLootSlots[ownedItemSlotIndex].UpdateGearStatis(Slot.SlotStatis.OWNED);
                        ownedLootSlots[ownedItemSlotIndex].ToggleEquipButton(true);
                        ownedLootSlots[ownedItemSlotIndex].isEmpty = false;
                        ownedLootSlots[ownedItemSlotIndex].ToggleOwnedGearButton(true);
                        ownedItemSlotIndex++;
                        //Debug.Log("456456456");
                    }
                }

                ownedLootSlots[ownedItemSlotIndex].UpdateIconSkillSize(false);
            }
        }

        // Make all empty owned gear slots display nothing 
        for (int x = 0; x < ownedLootSlots.Count; x++)
        {
            //Debug.Log("setting empty");
            ownedLootSlots[x].ToggleEquipButton(false);

            if (ownedLootSlots[x].isEmpty)
            {
                //Debug.Log("setting empty");
                ownedLootSlots[x].ToggleEquipButton(false);
                //ToggleOwnedGearEquipButton(false);
                ownedLootSlots[x].UpdateSlotImage(TeamGearManager.Instance.clearSlotSprite);
                //ownedGear[i].UpdateCurGearType(Gear.GearType.EMPTY);
                ownedLootSlots[x].isEmpty = true;
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
