using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamItemsManager : MonoBehaviour
{
    public static TeamItemsManager Instance;

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
    [SerializeField] private UIElement itemsNameText;

    public Slot selectedItemSlot;
    public Slot selectedBaseItemSlot;
    [Space(2)]
    [Header("Main Ally")]
    public List<ItemPiece> equippedItemsMain = new List<ItemPiece>();
    [Space(2)]
    [Header("Second Ally")]
    public List<ItemPiece> equippedItemsSecond = new List<ItemPiece>();
    [Space(2)]
    [Header("Third Ally")]
    public List<ItemPiece> equippedItemsThird = new List<ItemPiece>();

    [Space(2)]
    public bool playerInItemTab;

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

    public void UpdateGearStatDetails()
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

            // Gear Stat Name Update
            itemsNameText.UpdateContentText(GetSelectedItemSlot().GetSlotName());
        }

        // Gear Stat Description Update
        itemsDescUI.UpdateContentText(GetSelectedItemSlot().linkedItemPiece.itemDesc);
    }

    // Start is called before the first frame update
    void Start()
    {
        ToggleTeamItems(false);
        ClearItemSlots();
        ClearAllGearStats();
        ResetAllItemSelections();
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

        gear.ToggleSlotSelection(true);
    }

    public void UpdateItemSlotsBase(bool ally1 = false, bool ally2 = false, bool ally3 = false)
    {
        // Ensure each gear slot has correct bg gear sprite

        if (ally1)
        {
            for (int i = 0; i < ally1ItemsSlots.Count; i++)
            {
                // Place 1st Item
                if (i == 0)
                {
                    if (equippedItemsMain.Count >= 1)
                    {
                        if (equippedItemsMain[0] != null)
                            ally1ItemsSlots[i].UpdateSlotImage(equippedItemsMain[0].itemSprite);
                        else
                        {
                            ally1ItemsSlots[i].isEmpty = true;
                            ally1ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
                        }
                    }
                    else
                    {
                        ally1ItemsSlots[i].isEmpty = true;
                        ally1ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
                    }
                }
                // Place 2nd Item
                if (i == 1)
                {
                    if (equippedItemsMain.Count >= 2)
                    {
                        if (equippedItemsMain[1] != null)
                            ally1ItemsSlots[i].UpdateSlotImage(equippedItemsMain[1].itemSprite);
                        else
                        {
                            ally1ItemsSlots[i].isEmpty = true;
                            ally1ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
                        }
                    }
                    else
                    {
                        ally1ItemsSlots[i].isEmpty = true;
                        ally1ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
                    }
                }
                // Place 3rd Item
                if (i == 2)
                {
                    if (equippedItemsMain.Count >= 3)
                    {
                        if (equippedItemsMain[2] != null)
                            ally1ItemsSlots[i].UpdateSlotImage(equippedItemsMain[2].itemSprite);
                        else
                        {
                            ally1ItemsSlots[i].isEmpty = true;
                            ally1ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
                        }
                    }
                    else
                    {
                        ally1ItemsSlots[i].isEmpty = true;
                        ally1ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
                    }
                }

                ally1ItemsSlots[i].UpdateGearStatis(Slot.SlotStatis.DEFAULT);
                ally1ItemsSlots[i].UpdateGearOwnedBy(Slot.SlotOwnedBy.MAIN);
            }
        }

        if (ally2)
        {
            for (int i = 0; i < ally2ItemsSlots.Count; i++)
            {
                // Place 1st Item
                if (i == 0)
                {
                    if (equippedItemsSecond.Count >= 1)
                    {
                        if (equippedItemsSecond[0] != null)
                            ally2ItemsSlots[i].UpdateSlotImage(equippedItemsSecond[0].itemSprite);
                        else
                        {
                            ally2ItemsSlots[i].isEmpty = true;
                            ally2ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
                        }
                    }
                    else
                    {
                        ally2ItemsSlots[i].isEmpty = true;
                        ally2ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
                    }
                }
                // Place 2nd Item
                if (i == 1)
                {
                    if (equippedItemsSecond.Count >= 2)
                    {
                        if (equippedItemsSecond[1] != null)
                            ally2ItemsSlots[i].UpdateSlotImage(equippedItemsSecond[1].itemSprite);
                        else
                        {
                            ally2ItemsSlots[i].isEmpty = true;
                            ally2ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
                        }
                    }
                    else
                    {
                        ally2ItemsSlots[i].isEmpty = true;
                        ally2ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
                    }
                }
                // Place 3rd Item
                if (i == 2)
                {
                    if (equippedItemsSecond.Count >= 3)
                    {
                        if (equippedItemsSecond[2] != null)
                            ally2ItemsSlots[i].UpdateSlotImage(equippedItemsSecond[2].itemSprite);
                        else
                        {
                            ally2ItemsSlots[i].isEmpty = true;
                            ally2ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
                        }
                    }
                    else
                    {
                        ally2ItemsSlots[i].isEmpty = true;
                        ally2ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
                    }
                }

                ally2ItemsSlots[i].UpdateGearStatis(Slot.SlotStatis.DEFAULT);
                ally2ItemsSlots[i].UpdateGearOwnedBy(Slot.SlotOwnedBy.SECOND);
            }
        }

        if (ally3)
        {
            for (int i = 0; i < ally3ItemsSlots.Count; i++)
            {
                // Place 1st Item
                if (i == 0)
                {
                    if (equippedItemsThird.Count >= 1)
                    {
                        if (equippedItemsThird[0] != null)
                            ally3ItemsSlots[i].UpdateSlotImage(equippedItemsThird[0].itemSprite);
                        else
                        {
                            ally3ItemsSlots[i].isEmpty = true;
                            ally3ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
                        }
                    }
                    else
                    {
                        ally3ItemsSlots[i].isEmpty = true;
                        ally3ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
                    }
                }
                // Place 2nd Item
                if (i == 1)
                {
                    if (equippedItemsThird.Count >= 2)
                    {
                        if (equippedItemsThird[1] != null)
                            ally3ItemsSlots[i].UpdateSlotImage(equippedItemsThird[1].itemSprite);
                        else
                        {
                            ally3ItemsSlots[i].isEmpty = true;
                            ally3ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
                        }
                    }
                    else
                    {
                        ally3ItemsSlots[i].isEmpty = true;
                        ally3ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
                    }
                }
                // Place 3rd Item
                if (i == 2)
                {
                    if (equippedItemsThird.Count >= 3)
                    {
                        if (equippedItemsThird[2] != null)
                            ally3ItemsSlots[i].UpdateSlotImage(equippedItemsThird[2].itemSprite);
                        else
                        {
                            ally3ItemsSlots[i].isEmpty = true;
                            ally3ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
                        }
                    }
                    else
                    {
                        ally3ItemsSlots[i].isEmpty = true;
                        ally3ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
                    }
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
        if (GameManager.Instance.activeTeam.Count == 1)
            UpdateItemSlotsBase(true);
        else if (GameManager.Instance.activeTeam.Count == 2)
            UpdateItemSlotsBase(true, true);
        else if (GameManager.Instance.activeTeam.Count == 3)
            UpdateItemSlotsBase(true, true, true);
    }

    public void ClearItemSlots()
    {
        for (int i = 0; i < ally3ItemsSlots.Count; i++)
        {
            ally1ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
            ally2ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
            ally3ItemsSlots[i].UpdateSlotImage(clearSlotSprite);
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
            teamItemsTabUI.UpdateAlpha(1);

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
                ally1ItemsSlots[x].ToggleEquipButton(true);
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
                ally2ItemsSlots[y].ToggleEquipButton(true);
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
                ally3ItemsSlots[z].ToggleEquipButton(true);
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
                        // Add gear into owned gear
                        //OwnedLootInven.Instance.AddOwnedItems(OwnedLootInven.Instance.GetWornItemMainAlly()[x]);

                        // Remove worn gear
                        //OwnedLootInven.Instance.RemoveWornItemAllyMain(OwnedLootInven.Instance.GetWornItemMainAlly()[x]);
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
                        // Add gear into owned gear
                        //OwnedLootInven.Instance.AddOwnedItems(OwnedLootInven.Instance.GetWornItemSecondAlly()[x]);

                        // Remove worn gear
                        //OwnedLootInven.Instance.RemoveWornItemAllySecond(OwnedLootInven.Instance.GetWornItemSecondAlly()[x]);
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
                        // Add gear into owned gear
                        //OwnedLootInven.Instance.AddOwnedItems(OwnedLootInven.Instance.GetWornItemThirdAlly()[x]);

                        // Remove worn gear
                        //OwnedLootInven.Instance.RemoveWornItemAllyThird(OwnedLootInven.Instance.GetWornItemThirdAlly()[x]);
                        break;
                    }
                }
            }
        }
        

        // Remove gear from owned gear list when equipping
        // Add gear to worn gear
        for (int i = 0; i < OwnedLootInven.Instance.ownedItems.Count; i++)
        {
            if (OwnedLootInven.Instance.ownedItems[i].GetSlotName() == item.GetSlotName())
            {
                removedItem = OwnedLootInven.Instance.ownedItems[i];

                Debug.Log("doing thing");
                OwnedLootInven.Instance.RemoveOwnedItem(removedItem);

                if (selectedBaseItemSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.MAIN)
                {
                    Debug.Log("12");
                    OwnedLootInven.Instance.AddWornItemAllyMain(removedItem);
                }
                else if (selectedBaseItemSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.SECOND)
                    OwnedLootInven.Instance.AddWornItemAllySecond(removedItem);
                else if (selectedBaseItemSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.THIRD)
                    OwnedLootInven.Instance.AddWornItemAllyThird(removedItem);
                break;
            }
        }

        GetSelectedBaseItemSlot().UpdateSlotImage(item.GetSlotImage());
        GetSelectedBaseItemSlot().UpdateSlotName(item.GetSlotName());
        GetSelectedBaseItemSlot().linkedItemPiece = item.linkedItemPiece;
        /*
        GetSelectedBaseGearSlot().UpdateGearBonusHealth(item.GetBonusHealth());
        GetSelectedBaseGearSlot().UpdateGearBonusHealing(item.GetBonusHealing());
        GetSelectedBaseGearSlot().UpdateGearBonusDefense(item.GetBonusDefense());
        GetSelectedBaseGearSlot().UpdateGearBonusDamage(item.GetBonusDamage());
        GetSelectedBaseGearSlot().UpdateGearBonusSpeed(item.GetBonusSpeed());
        */
        //UpdateGearStatDetails();

        // Update unit stats with stats from gear
        //UpdateUnitStatsEquip(item);

        // Show combined calculated values next to unit
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
                        equippedItemsMain.Remove(equippedItemsMain[i]);
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
                        equippedItemsSecond.Remove(equippedItemsSecond[i]);
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
                        equippedItemsThird.Remove(equippedItemsThird[i]);
                }
            }
        }
    }

    public void ItemSelection(Slot item, bool select = false)
    {
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
                if (select)
                    EquipItem(item);
            }
        }

        // If gear is NOT empty, put gear in it
        if (!item.isEmpty)
        {
            // Update UI
            UpdateGearStatDetails();
            UpdateItemNameText(item.GetSlotName());
        }

        // If gear IS empty, dont put gear in it, display it as empty
        else
        {
            UpdateItemNameText("");
            UpdateItemDesc("");
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
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornItemMainAlly()[x].GetSlotName() == GetSelectedBaseItemSlot().GetSlotName())
                    {
                        // Remove saved equipped gear piece (data side)
                        UpdateEquippedItemPiece("ItemMain", OwnedLootInven.Instance.GetWornItemMainAlly()[x].linkedItemPiece, false);

                        // Update unit stats when unequiping
                        UpdateUnitStatsUnEquip(OwnedLootInven.Instance.GetWornItemMainAlly()[x]);

                        // Add gear into owned gear
                        OwnedLootInven.Instance.AddOwnedItems(OwnedLootInven.Instance.GetWornItemMainAlly()[x]);

                        // Remove worn gear
                        OwnedLootInven.Instance.RemoveWornItemAllyMain(OwnedLootInven.Instance.GetWornItemMainAlly()[x]);
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
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornItemSecondAlly()[x].GetSlotName() == GetSelectedBaseItemSlot().GetSlotName())
                    {
                        // Remove saved equipped gear piece (data side)
                        UpdateEquippedItemPiece("ItemMain", OwnedLootInven.Instance.GetWornItemSecondAlly()[x].linkedItemPiece, false);

                        // Update unit stats when unequiping
                        UpdateUnitStatsUnEquip(OwnedLootInven.Instance.GetWornItemSecondAlly()[x]);

                        // Add gear into owned gear
                        OwnedLootInven.Instance.AddOwnedItems(OwnedLootInven.Instance.GetWornItemSecondAlly()[x]);

                        // Remove worn gear
                        OwnedLootInven.Instance.RemoveWornItemAllyMain(OwnedLootInven.Instance.GetWornItemSecondAlly()[x]);
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
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornItemThirdAlly()[x].GetSlotName() == GetSelectedBaseItemSlot().GetSlotName())
                    {
                        // Remove saved equipped gear piece (data side)
                        UpdateEquippedItemPiece("ItemMain", OwnedLootInven.Instance.GetWornItemThirdAlly()[x].linkedItemPiece, false);

                        // Update unit stats when unequiping
                        UpdateUnitStatsUnEquip(OwnedLootInven.Instance.GetWornItemThirdAlly()[x]);

                        // Add gear into owned gear
                        OwnedLootInven.Instance.AddOwnedItems(OwnedLootInven.Instance.GetWornItemThirdAlly()[x]);

                        // Remove worn gear
                        OwnedLootInven.Instance.RemoveWornItemAllyMain(OwnedLootInven.Instance.GetWornItemThirdAlly()[x]);
                        break;
                    }
                }
            }
        }


        GetSelectedBaseItemSlot().isEmpty = true;
        // Remove gear icon display
        GetSelectedBaseItemSlot().ResetSlot(true, true);

        // Remove gear icon details (name / stats
        UpdateItemDesc("");
        UpdateItemNameText("");
        ClearAllGearStats();
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
                if (ally1ItemsSlots[i].isEmpty)
                    ally1ItemsSlots[i].UpdateSlotImage(TeamGearManager.Instance.clearSlotSprite);

                ally1ItemsSlots[i].UpdateGearStatis(Slot.SlotStatis.DEFAULT);
                ally1ItemsSlots[i].UpdateGearOwnedBy(Slot.SlotOwnedBy.MAIN);
            }
        }

        if (ally2)
        {
            for (int x = 0; x < ally2ItemsSlots.Count; x++)
            {
                if (ally2ItemsSlots[x].isEmpty)
                    ally2ItemsSlots[x].UpdateSlotImage(TeamGearManager.Instance.clearSlotSprite);

                ally2ItemsSlots[x].UpdateGearStatis(Slot.SlotStatis.DEFAULT);
                ally2ItemsSlots[x].UpdateGearOwnedBy(Slot.SlotOwnedBy.SECOND);
            }
        }

        if (ally3)
        {
            for (int y = 0; y < ally3ItemsSlots.Count; y++)
            {
                if (ally3ItemsSlots[y].isEmpty)
                    ally3ItemsSlots[y].UpdateSlotImage(TeamGearManager.Instance.clearSlotSprite);

                ally3ItemsSlots[y].UpdateGearStatis(Slot.SlotStatis.DEFAULT);
                ally3ItemsSlots[y].UpdateGearOwnedBy(Slot.SlotOwnedBy.THIRD);
            }
        }


    }

    public void SellGear()
    {

    }
}
