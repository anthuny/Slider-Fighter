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

    [SerializeField] private List<Gear> ally1ItemsSlots = new List<Gear>();
    [SerializeField] private List<Gear> ally2ItemsSlots = new List<Gear>();
    [SerializeField] private List<Gear> ally3ItemsSlots = new List<Gear>();

    [SerializeField] private UIElement itemsStatsUI;
    [SerializeField] private GameObject itemsStatGO;
    [SerializeField] private UIElement itemsDescUI;
    [SerializeField] private UIElement itemsNameText;

    public Gear selectedItemSlot;
    public Gear selectedBaseItemSlot;
    [Space(2)]
    [Header("Main Ally")]
    public GearPiece equippedItemMain1;
    public GearPiece equippedItemMain2;
    public GearPiece equippedItemMain3;
    public GearPiece equippedItemMain4;
    [Space(2)]
    [Header("Second Ally")]
    public GearPiece equippedItemSec1;
    public GearPiece equippedItemSec2;
    public GearPiece equippedItemSec3;
    public GearPiece equippedItemSec4;
    [Space(2)]
    [Header("Third Ally")]
    public GearPiece equippedItemThi1;
    public GearPiece equippedItemThi2;
    public GearPiece equippedItemThi3;
    public GearPiece equippedItemThi4;
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

    public void UpdateGearNameText(string name)
    {
        itemsNameText.UpdateContentText(name);
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

    public void UpdateUnitStatsEquip(Gear gear)
    {
        if (selectedBaseItemSlot.GetGearOwnedBy() == Gear.GearOwnedBy.MAIN)
        {
            UnitFunctionality unitFunc = GameManager.Instance.activeRoomAllUnitFunctionalitys[0];

            unitFunc.UpdateUnitMaxHealth(gear.GetBonusHealth(), false, true);
            unitFunc.UpdateUnitCurHealth(gear.GetBonusHealth(), false, false);
            unitFunc.UpdateUnitDamageHits(gear.GetBonusDamage(), true);
            unitFunc.UpdateUnitHealingHits(gear.GetBonusHealing(), true);
            unitFunc.UpdateUnitDefenseChange(gear.GetBonusDefense(), true);
            unitFunc.UpdateUnitSpeedChange(gear.GetBonusSpeed(), true);

            ally1MenuUnitDisplay.UpdateUnitStats(unitFunc);
        }
        else if (selectedBaseItemSlot.GetGearOwnedBy() == Gear.GearOwnedBy.SECOND)
        {
            UnitFunctionality unitFunc = GameManager.Instance.activeRoomAllUnitFunctionalitys[1];

            unitFunc.UpdateUnitMaxHealth(gear.GetBonusHealth(), false, true);
            unitFunc.UpdateUnitCurHealth(gear.GetBonusHealth(), false, false);
            unitFunc.UpdateUnitDamageHits(gear.GetBonusDamage(), true);
            unitFunc.UpdateUnitHealingHits(gear.GetBonusHealing(), true);
            unitFunc.UpdateUnitDefenseChange(gear.GetBonusDefense(), true);
            unitFunc.UpdateUnitSpeedChange(gear.GetBonusSpeed(), true);

            ally2MenuUnitDisplay.UpdateUnitStats(unitFunc);
        }
        else if (selectedBaseItemSlot.GetGearOwnedBy() == Gear.GearOwnedBy.THIRD)
        {
            UnitFunctionality unitFunc = GameManager.Instance.activeRoomAllUnitFunctionalitys[2];

            unitFunc.UpdateUnitMaxHealth(gear.GetBonusHealth(), false, true);
            unitFunc.UpdateUnitCurHealth(gear.GetBonusHealth(), false, false);
            unitFunc.UpdateUnitDamageHits(gear.GetBonusDamage(), true);
            unitFunc.UpdateUnitHealingHits(gear.GetBonusHealing(), true);
            unitFunc.UpdateUnitDefenseChange(gear.GetBonusDefense(), true);
            unitFunc.UpdateUnitSpeedChange(gear.GetBonusSpeed(), true);

            ally3MenuUnitDisplay.UpdateUnitStats(unitFunc);
        }
    }

    public void UpdateUnitStatsUnEquip(Gear gear)
    {
        if (selectedBaseItemSlot.GetGearOwnedBy() == Gear.GearOwnedBy.MAIN)
        {
            UnitFunctionality unitFunc = GameManager.Instance.activeRoomAllUnitFunctionalitys[0];

            unitFunc.UpdateUnitCurHealth(gear.GetBonusHealth(), true, false, false);
            unitFunc.UpdateUnitMaxHealth(gear.GetBonusHealth(), false, false);
            unitFunc.UpdateUnitDamageHits(gear.GetBonusDamage(), false);
            unitFunc.UpdateUnitHealingHits(gear.GetBonusHealing(), false);
            unitFunc.UpdateUnitDefenseChange(gear.GetBonusDefense(), false);
            unitFunc.UpdateUnitSpeedChange(gear.GetBonusSpeed(), false);

            ally1MenuUnitDisplay.UpdateUnitStats(unitFunc);
        }
        else if (selectedBaseItemSlot.GetGearOwnedBy() == Gear.GearOwnedBy.SECOND)
        {
            UnitFunctionality unitFunc = GameManager.Instance.activeRoomAllUnitFunctionalitys[1];

            unitFunc.UpdateUnitCurHealth(gear.GetBonusHealth(), true, false, false);
            unitFunc.UpdateUnitMaxHealth(gear.GetBonusHealth(), false, false);
            unitFunc.UpdateUnitDamageHits(gear.GetBonusDamage(), false);
            unitFunc.UpdateUnitHealingHits(gear.GetBonusHealing(), false);
            unitFunc.UpdateUnitDefenseChange(gear.GetBonusDefense(), false);
            unitFunc.UpdateUnitSpeedChange(gear.GetBonusSpeed(), false);

            ally2MenuUnitDisplay.UpdateUnitStats(unitFunc);
        }
        else if (selectedBaseItemSlot.GetGearOwnedBy() == Gear.GearOwnedBy.THIRD)
        {
            UnitFunctionality unitFunc = GameManager.Instance.activeRoomAllUnitFunctionalitys[2];

            unitFunc.UpdateUnitCurHealth(gear.GetBonusHealth(), true, false, false);
            unitFunc.UpdateUnitMaxHealth(gear.GetBonusHealth(), false, false);
            unitFunc.UpdateUnitDamageHits(gear.GetBonusDamage(), false);
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
                statUI.UpdateContentText(GetSelectedGearSlot().GetBonusHealth().ToString());
            else if (i == 1)
                statUI.UpdateContentText(GetSelectedGearSlot().GetBonusDamage().ToString());
            else if (i == 2)
                statUI.UpdateContentText(GetSelectedGearSlot().GetBonusHealing().ToString());
            else if (i == 3)
                statUI.UpdateContentText(GetSelectedGearSlot().GetBonusDefense().ToString());
            else if (i == 4)
                statUI.UpdateContentText(GetSelectedGearSlot().GetBonusSpeed().ToString());

            // Gear Stat Name Update
            itemsNameText.UpdateContentText(GetSelectedGearSlot().GetGearName());
        }

        // Gear Stat Description Update


    }



    // Start is called before the first frame update
    void Start()
    {
        ToggleTeamItems(false);
        ClearGearSlots();
        ClearAllGearStats();
        ResetAllItemSelections();
    }

    private void Awake()
    {
        Instance = this;
    }

    public Gear GetSelectedGearSlot()
    {
        return selectedItemSlot;
    }

    public void UpdateSelectedGearSlot(Gear gear)
    {
        selectedItemSlot = gear;
    }

    public Gear GetSelectedBaseGearSlot()
    {
        return selectedBaseItemSlot;
    }

    public void UpdateSelectedBaseGearSlot(Gear gear)
    {
        selectedBaseItemSlot = gear;

        gear.ToggleGearSelected(true);
    }

    public void ClearGearSlots()
    {
        for (int i = 0; i < ally3ItemsSlots.Count; i++)
        {
            ally1ItemsSlots[i].UpdateGearImage(clearSlotSprite);
            ally2ItemsSlots[i].UpdateGearImage(clearSlotSprite);
            ally3ItemsSlots[i].UpdateGearImage(clearSlotSprite);
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
            ally1MenuUnitDisplay.ToggleUnitLevelImage(true, GameManager.Instance.activeRoomAllies[0].GetUnitLevel());

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
            ally1MenuUnitDisplay.ToggleUnitLevelImage(true, GameManager.Instance.activeRoomAllies[0].GetUnitLevel());
            ally2MenuUnitDisplay.ToggleUnitLevelImage(true, GameManager.Instance.activeRoomAllies[1].GetUnitLevel());

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
            ally1MenuUnitDisplay.ToggleUnitLevelImage(true, GameManager.Instance.activeRoomAllies[0].GetUnitLevel());
            ally2MenuUnitDisplay.ToggleUnitLevelImage(true, GameManager.Instance.activeRoomAllies[1].GetUnitLevel());
            ally3MenuUnitDisplay.ToggleUnitLevelImage(true, GameManager.Instance.activeRoomAllies[2].GetUnitLevel());

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
            for (int i = 0; i < GameManager.Instance.activeRoomAllies.Count; i++)
            {
                GameManager.Instance.activeRoomAllies[i].ToggleUnitLevelImage(true);
            }

            // Toggle to map button for team gear
            ToggleToMapButton(true);
            // Toggle Team setup to map button off for team gear page
            TeamSetup.Instance.ToggleToMapButton(false);

            // Disable team setup tab
            GameManager.Instance.ToggleTeamSetup(false);

            ToggleGearSlotsClickable(true);

            // Hide team setup tab arrow buttons
            TeamSetup.Instance.gearTabArrowLeftButton.ToggleButton(false);
            TeamSetup.Instance.gearTabArrowRightButton.ToggleButton(false);

            // Display gear tab arrow buttons
            teamSetupTabArrowLeftButton.ToggleButton(true);
            teamSetupTabArrowRightButton.ToggleButton(true);

            ToggleAllyGearSets();

            // Clears empty base gear slots if empty
            ClearEmptyGearSlots();

            ClearAllGearStats();

            if (GameManager.Instance.activeTeam.Count == 1)
                ally1MenuUnitDisplay.UpdateUnitStats(GameManager.Instance.activeRoomAllUnitFunctionalitys[0]);
            if (GameManager.Instance.activeTeam.Count == 2)
            {
                ally1MenuUnitDisplay.UpdateUnitStats(GameManager.Instance.activeRoomAllUnitFunctionalitys[0]);
                ally2MenuUnitDisplay.UpdateUnitStats(GameManager.Instance.activeRoomAllUnitFunctionalitys[1]);
            }
            if (GameManager.Instance.activeTeam.Count == 3)
            {
                ally1MenuUnitDisplay.UpdateUnitStats(GameManager.Instance.activeRoomAllUnitFunctionalitys[0]);
                ally2MenuUnitDisplay.UpdateUnitStats(GameManager.Instance.activeRoomAllUnitFunctionalitys[1]);
                ally3MenuUnitDisplay.UpdateUnitStats(GameManager.Instance.activeRoomAllUnitFunctionalitys[2]);
            }
        }
        else
        {
            teamItemsTabUI.UpdateAlpha(0);

            // Toggle to map button for team gear
            ToggleToMapButton(false);
            TeamSetup.Instance.ToggleToMapButton(true);

            ResetAllItemSelections();

            ToggleGearSlotsClickable(false);

            // Hide team setup tab arrow buttons
            teamSetupTabArrowLeftButton.ToggleButton(false);
            teamSetupTabArrowRightButton.ToggleButton(false);

            // Display team setup tab arrow buttons
            TeamSetup.Instance.gearTabArrowLeftButton.ToggleButton(true);
            TeamSetup.Instance.gearTabArrowRightButton.ToggleButton(true);
        }
    }

    public void ResetAllItemSelections()
    {
        //Debug.Log("resetting");
        for (int x = 0; x < ally1ItemsSlots.Count; x++)
        {
            ally1ItemsSlots[x].ToggleGearSelected(false);
        }
        for (int y = 0; y < ally2ItemsSlots.Count; y++)
        {
            ally2ItemsSlots[y].ToggleGearSelected(false);
        }
        for (int z = 0; z < ally3ItemsSlots.Count; z++)
        {
            ally3ItemsSlots[z].ToggleGearSelected(false);
        }
    }

    public void ToggleGearSlotsClickable(bool toggle)
    {
        for (int x = 0; x < ally1ItemsSlots.Count; x++)
        {
            if (toggle)
            {
                ally1ItemsSlots[x].GetGearSelection().ToggleButton(true);
                ally1ItemsSlots[x].ToggleMainGear(true);
                ally1ItemsSlots[x].ToggleOwnedGearButton(true);
                ally1ItemsSlots[x].ToggleEquipButton(true);
            }
            else
            {
                ally1ItemsSlots[x].GetGearSelection().ToggleButton(false);
                ally1ItemsSlots[x].ToggleMainGear(false);
                ally1ItemsSlots[x].ToggleOwnedGearButton(false);
                ally2ItemsSlots[x].ToggleEquipButton(false);
            }
        }
        for (int y = 0; y < ally2ItemsSlots.Count; y++)
        {
            if (toggle)
            {
                ally2ItemsSlots[y].GetGearSelection().ToggleButton(true);
                ally2ItemsSlots[y].ToggleMainGear(true);
                ally2ItemsSlots[y].ToggleOwnedGearButton(true);
                ally2ItemsSlots[y].ToggleEquipButton(true);
            }

            else
            {
                ally2ItemsSlots[y].GetGearSelection().ToggleButton(false);
                ally2ItemsSlots[y].ToggleMainGear(false);
                ally2ItemsSlots[y].ToggleOwnedGearButton(false);
                ally2ItemsSlots[y].ToggleEquipButton(false);
            }
        }
        for (int z = 0; z < ally3ItemsSlots.Count; z++)
        {
            if (toggle)
            {
                ally3ItemsSlots[z].GetGearSelection().ToggleButton(true);
                ally3ItemsSlots[z].ToggleMainGear(true);
                ally3ItemsSlots[z].ToggleOwnedGearButton(true);
                ally3ItemsSlots[z].ToggleEquipButton(true);
            }
            else
            {
                ally3ItemsSlots[z].GetGearSelection().ToggleButton(false);
                ally3ItemsSlots[z].ToggleMainGear(false);
                ally3ItemsSlots[z].ToggleOwnedGearButton(false);
                ally3ItemsSlots[z].ToggleEquipButton(false);
            }
        }

        int count = OwnedLootInven.Instance.ownedLootSlots.Count;

        for (int i = 0; i < count; i++)
        {
            if (toggle)
            {
                OwnedLootInven.Instance.ownedLootSlots[i].GetGearSelection().ToggleButton(true);
                OwnedLootInven.Instance.ownedLootSlots[i].ToggleMainGear(true);
                //OwnedGearInven.Instance.ownedGearSlots[i].ToggleOwnedGearButton(true);
                OwnedLootInven.Instance.ownedLootSlots[i].ToggleEquipButton(true);
            }
            else
            {
                OwnedLootInven.Instance.ownedLootSlots[i].GetGearSelection().ToggleButton(false);
                OwnedLootInven.Instance.ownedLootSlots[i].ToggleMainGear(false);
                //OwnedGearInven.Instance.ownedGearSlots[i].ToggleOwnedGearButton(false);
                OwnedLootInven.Instance.ownedLootSlots[i].ToggleEquipButton(false);
            }
        }
    }

    public void EquipItem(Gear gear)
    {
        OwnedLootInven.Instance.ToggleOwnedGearDisplay(false);

        Gear removedGear = null;

        // Remove current equipt item, and place into owned gear

        if (selectedBaseItemSlot.GetGearOwnedBy() == Gear.GearOwnedBy.MAIN)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornGearMainAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornGearMainAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornGearMainAlly()[x].GetGearName() == gear.GetGearName())
                    {
                        // Add gear into owned gear
                        OwnedLootInven.Instance.AddOwnedGear(OwnedLootInven.Instance.GetWornGearMainAlly()[x]);

                        // Remove worn gear
                        OwnedLootInven.Instance.RemoveWornGearAllyMain(OwnedLootInven.Instance.GetWornGearMainAlly()[x]);
                        break;
                    }
                }
            }
        }
        else if (selectedBaseItemSlot.GetGearOwnedBy() == Gear.GearOwnedBy.SECOND)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornGearSecondAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornGearSecondAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornGearSecondAlly()[x].GetGearName() == gear.GetGearName())
                    {
                        // Add gear into owned gear
                        OwnedLootInven.Instance.AddOwnedGear(OwnedLootInven.Instance.GetWornGearSecondAlly()[x]);

                        // Remove worn gear
                        OwnedLootInven.Instance.RemoveWornGearAllySecond(OwnedLootInven.Instance.GetWornGearSecondAlly()[x]);
                        break;
                    }
                }
            }
        }
        else if (selectedBaseItemSlot.GetGearOwnedBy() == Gear.GearOwnedBy.THIRD)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornGearThirdAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornGearThirdAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornGearThirdAlly()[x].GetGearName() == gear.GetGearName())
                    {
                        // Add gear into owned gear
                        OwnedLootInven.Instance.AddOwnedGear(OwnedLootInven.Instance.GetWornGearThirdAlly()[x]);

                        // Remove worn gear
                        OwnedLootInven.Instance.RemoveWornGearAllyThird(OwnedLootInven.Instance.GetWornGearThirdAlly()[x]);
                        break;
                    }
                }
            }
        }


        // Remove gear from owned gear list when equipping
        // Add gear to worn gear
        for (int i = 0; i < OwnedLootInven.Instance.ownedGear.Count; i++)
        {
            if (OwnedLootInven.Instance.ownedGear[i].GetGearName() == gear.GetGearName())
            {
                removedGear = OwnedLootInven.Instance.ownedGear[i];

                OwnedLootInven.Instance.RemoveOwnedGear(removedGear);

                if (selectedBaseItemSlot.GetGearOwnedBy() == Gear.GearOwnedBy.MAIN)
                    OwnedLootInven.Instance.AddWornGearAllyMain(removedGear);
                else if (selectedBaseItemSlot.GetGearOwnedBy() == Gear.GearOwnedBy.SECOND)
                    OwnedLootInven.Instance.AddWornGearAllySecond(removedGear);
                else if (selectedBaseItemSlot.GetGearOwnedBy() == Gear.GearOwnedBy.THIRD)
                    OwnedLootInven.Instance.AddWornGearAllyThird(removedGear);
                break;
            }
        }

        GetSelectedBaseGearSlot().UpdateGearImage(gear.GetGearImage());
        GetSelectedBaseGearSlot().UpdateGearName(gear.GetGearName());
        GetSelectedBaseGearSlot().UpdateGearBonusHealth(gear.GetBonusHealth());
        GetSelectedBaseGearSlot().UpdateGearBonusHealing(gear.GetBonusHealing());
        GetSelectedBaseGearSlot().UpdateGearBonusDefense(gear.GetBonusDefense());
        GetSelectedBaseGearSlot().UpdateGearBonusDamage(gear.GetBonusDamage());
        GetSelectedBaseGearSlot().UpdateGearBonusSpeed(gear.GetBonusSpeed());

        UpdateGearStatDetails();

        // Update unit stats with stats from gear
        UpdateUnitStatsEquip(gear);

        // Show combined calculated values next to unit
    }

    public void ResetGearOwned()
    {
        equippedItemMain1 = null;
        equippedItemMain2 = null;
        equippedItemMain3 = null;
        equippedItemMain4 = null;

        equippedItemSec1 = null;
        equippedItemSec2 = null;
        equippedItemSec3 = null;
        equippedItemSec4 = null;

        equippedItemThi1 = null;
        equippedItemThi2 = null;
        equippedItemThi3 = null;
        equippedItemThi4 = null;

        OwnedLootInven.Instance.ResetWornGearAllyMain();
        OwnedLootInven.Instance.ResetWornGearAllySecond();
        OwnedLootInven.Instance.ResetWornGearAllyThird();

        ally1MenuUnitDisplay.ResetUnitStats();
        ally2MenuUnitDisplay.ResetUnitStats();
        ally3MenuUnitDisplay.ResetUnitStats();

        ClearAllGearStats();
        UpdateGearNameText("");
    }

    public void UpdateEquippedGearPiece(string gearPieceTypeName, GearPiece newGearPiece, bool replacing = true)
    {
        if (gearPieceTypeName == "helmMain")
        {
            if (replacing)
                equippedItemMain1 = newGearPiece;
            else
                equippedItemMain1 = null;
        }
        else if (gearPieceTypeName == "chestMain")
        {
            if (replacing)
                equippedItemMain2 = newGearPiece;
            else
                equippedItemMain2 = null;
        }
        else if (gearPieceTypeName == "legsMain")
        {
            if (replacing)
                equippedItemMain3 = newGearPiece;
            else
                equippedItemMain3 = null;
        }
        else if (gearPieceTypeName == "bootsMain")
        {
            if (replacing)
                equippedItemMain4 = newGearPiece;
            else
                equippedItemMain4 = null;
        }

        else if (gearPieceTypeName == "helmSecond")
        {
            if (replacing)
                equippedItemSec1 = newGearPiece;
            else
                equippedItemSec1 = null;
        }
        else if (gearPieceTypeName == "chestSecond")
        {
            if (replacing)
                equippedItemSec2 = newGearPiece;
            else
                equippedItemSec2 = null;
        }
        else if (gearPieceTypeName == "legsSecond")
        {
            if (replacing)
                equippedItemSec3 = newGearPiece;
            else
                equippedItemSec3 = null;
        }
        else if (gearPieceTypeName == "bootsSecond")
        {
            if (replacing)
                equippedItemSec4 = newGearPiece;
            else
                equippedItemSec4 = null;
        }

        else if (gearPieceTypeName == "helmThird")
        {
            if (replacing)
                equippedItemThi1 = newGearPiece;
            else
                equippedItemThi1 = null;
        }
        else if (gearPieceTypeName == "chestThird")
        {
            if (replacing)
                equippedItemThi2 = newGearPiece;
            else
                equippedItemThi2 = null;
        }
        else if (gearPieceTypeName == "legsThird")
        {
            if (replacing)
                equippedItemThi3 = newGearPiece;
            else
                equippedItemThi3 = null;
        }
        else if (gearPieceTypeName == "bootsThird")
        {
            if (replacing)
                equippedItemThi4 = newGearPiece;
            else
                equippedItemThi4 = null;
        }
    }

    public void ItemSelection(Gear gear, bool select = false)
    {
        // Disable all gear selection border
        ResetAllItemSelections();

        // Enable selected gear slot border
        gear.ToggleGearSelected(true);
        //OwnedGearInven.Instance.FillOwnedGearSlots();

        // Bug todo - 2nd / 3rd ally arent having their gear saved.

        if (gear.curGearStatis == Gear.GearStatis.DEFAULT)
        {
            UpdateSelectedBaseGearSlot(gear);

            UpdateSelectedGearSlot(gear);

            OwnedLootInven.Instance.EnableOwnedItemsSlotSelection(GetSelectedGearSlot());
        }
        else
        {

            if (selectedBaseItemSlot != null)
            {


            }
            UpdateSelectedGearSlot(gear);
            OwnedLootInven.Instance.EnableOwnedItemsSlotSelection(GetSelectedGearSlot());

         
        }

        // Reference owned item, if the player currently already owns the selected piece of gear.
        //GearPiece newGearPiece = OwnedGearInven.Instance.GetGearPiece(gear);


        // If gear is NOT empty, put gear in it
        if (!gear.isEmpty)
        {
            /*
            // Initialize gear base data
            gear.UpdateGearImage(newGearPiece.gearIcon);
            gear.UpdateGearName(newGearPiece.gearName);
            gear.UpdateGearBonusHealth(newGearPiece.bonusHealth);
            gear.UpdateGearBonusDamage(newGearPiece.bonusDamage);
            gear.UpdateGearBonusHealing(newGearPiece.bonusHealing);
            gear.UpdateGearBonusDefense(newGearPiece.bonusDefense);
            gear.UpdateGearBonusSpeed(newGearPiece.bonusSpeed);

            */
            // Update UI
            UpdateGearStatDetails();
            UpdateGearNameText(gear.GetGearName());
        }

        // If gear IS empty, dont put gear in it, display it as empty
        else
        {
            /*
            if (gear.GetCurGearStatis() != Gear.GearStatis.DEFAULT)
            {
                // Initialize gear base data
                gear.ResetGearSlot(true);

            }
            else
            {
                // Initialize gear base data
                //gear.ResetGearSlot(true, true);
            }
            */

            ClearAllGearStats();
            UpdateGearNameText("");
        }
    }

    public void UnequipGear()
    {
        if (GetSelectedBaseGearSlot() == null)
            return;

        if (GetSelectedBaseGearSlot().GetGearOwnedBy() == Gear.GearOwnedBy.MAIN)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornGearMainAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornGearMainAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornGearMainAlly()[x].GetGearName() == GetSelectedGearSlot().GetGearName())
                    {
                        // Remove saved equipped gear piece (data side)
                        if (OwnedLootInven.Instance.GetWornGearMainAlly()[x].GetCurGearType() == Gear.GearType.HELMET)
                        {
                            UpdateEquippedGearPiece("helmMain", null, false);
                        }
                        else if (OwnedLootInven.Instance.GetWornGearMainAlly()[x].GetCurGearType() == Gear.GearType.CHESTPIECE)
                        {
                            UpdateEquippedGearPiece("chestMain", null, false);
                        }
                        else if (OwnedLootInven.Instance.GetWornGearMainAlly()[x].GetCurGearType() == Gear.GearType.LEGGINGS)
                        {
                            UpdateEquippedGearPiece("legsMain", null, false);
                        }
                        else if (OwnedLootInven.Instance.GetWornGearMainAlly()[x].GetCurGearType() == Gear.GearType.BOOTS)
                        {
                            UpdateEquippedGearPiece("bootsMain", null, false);
                        }

                        // Update unit stats when unequiping
                        UpdateUnitStatsUnEquip(OwnedLootInven.Instance.GetWornGearMainAlly()[x]);

                        // Add gear into owned gear
                        OwnedLootInven.Instance.AddOwnedGear(OwnedLootInven.Instance.GetWornGearMainAlly()[x]);

                        // Remove worn gear
                        OwnedLootInven.Instance.RemoveWornGearAllyMain(OwnedLootInven.Instance.GetWornGearMainAlly()[x]);
                        break;
                    }
                }
            }
        }
        else if (GetSelectedBaseGearSlot().GetGearOwnedBy() == Gear.GearOwnedBy.SECOND)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornGearSecondAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornGearSecondAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornGearSecondAlly()[x].GetGearName() == GetSelectedGearSlot().GetGearName())
                    {
                        // Remove saved equipped gear piece (data side)
                        if (OwnedLootInven.Instance.GetWornGearSecondAlly()[x].GetCurGearType() == Gear.GearType.HELMET)
                        {
                            UpdateEquippedGearPiece("helmSecond", null, false);
                        }
                        else if (OwnedLootInven.Instance.GetWornGearSecondAlly()[x].GetCurGearType() == Gear.GearType.CHESTPIECE)
                        {
                            UpdateEquippedGearPiece("chestSecond", null, false);
                        }
                        else if (OwnedLootInven.Instance.GetWornGearSecondAlly()[x].GetCurGearType() == Gear.GearType.LEGGINGS)
                        {
                            UpdateEquippedGearPiece("legsSecond", null, false);
                        }
                        else if (OwnedLootInven.Instance.GetWornGearSecondAlly()[x].GetCurGearType() == Gear.GearType.BOOTS)
                        {
                            UpdateEquippedGearPiece("bootsSecond", null, false);
                        }

                        // Update unit stats when unequiping
                        UpdateUnitStatsUnEquip(OwnedLootInven.Instance.GetWornGearSecondAlly()[x]);

                        // Add gear into owned gear
                        OwnedLootInven.Instance.AddOwnedGear(OwnedLootInven.Instance.GetWornGearSecondAlly()[x]);

                        // Remove worn gear
                        OwnedLootInven.Instance.RemoveWornGearAllySecond(OwnedLootInven.Instance.GetWornGearSecondAlly()[x]);
                        break;
                    }
                }
            }
        }
        else if (GetSelectedBaseGearSlot().GetGearOwnedBy() == Gear.GearOwnedBy.THIRD)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornGearThirdAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornGearThirdAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornGearThirdAlly()[x].GetGearName() == GetSelectedGearSlot().GetGearName())
                    {
                        // Remove saved equipped gear piece (data side)
                        if (OwnedLootInven.Instance.GetWornGearThirdAlly()[x].GetCurGearType() == Gear.GearType.HELMET)
                        {
                            UpdateEquippedGearPiece("helmThird", null, false);
                        }
                        else if (OwnedLootInven.Instance.GetWornGearThirdAlly()[x].GetCurGearType() == Gear.GearType.CHESTPIECE)
                        {
                            UpdateEquippedGearPiece("chestThird", null, false);
                        }
                        else if (OwnedLootInven.Instance.GetWornGearThirdAlly()[x].GetCurGearType() == Gear.GearType.LEGGINGS)
                        {
                            UpdateEquippedGearPiece("legsThird", null, false);
                        }
                        else if (OwnedLootInven.Instance.GetWornGearThirdAlly()[x].GetCurGearType() == Gear.GearType.BOOTS)
                        {
                            UpdateEquippedGearPiece("bootsThird", null, false);
                        }

                        // Update unit stats when unequiping
                        UpdateUnitStatsUnEquip(OwnedLootInven.Instance.GetWornGearThirdAlly()[x]);

                        // Add gear into owned gear
                        OwnedLootInven.Instance.AddOwnedGear(OwnedLootInven.Instance.GetWornGearThirdAlly()[x]);

                        // Remove worn gear
                        OwnedLootInven.Instance.RemoveWornGearAllyThird(OwnedLootInven.Instance.GetWornGearThirdAlly()[x]);

                        break;
                    }
                }
            }
        }

        // Remove gear icon display
        GetSelectedBaseGearSlot().ResetGearSlot(true, true);

        // Remove gear icon details (name / stats)
        ClearAllGearStats();


    }

    public void SellGear()
    {

    }
}