using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamGearManager : MonoBehaviour
{
    public static TeamGearManager Instance;

    [SerializeField] private UIElement teamGearTabUI;

    public Sprite clearSlotSprite;
    public Sprite helmetSlotSprite;
    public Sprite chestSlotSprite;
    public Sprite legsSlotSprite;
    public Sprite bootsSlotSprite;

    public ButtonFunctionality teamSetupTabArrowLeftButton;
    public ButtonFunctionality teamSetupTabArrowRightButton;

    [SerializeField] private UIElement ally1GearTabUI;
    [SerializeField] private UIElement ally2GearTabUI;
    [SerializeField] private UIElement ally3GearTabUI;

    [SerializeField] private MenuUnitDisplay ally1MenuUnitDisplay;
    [SerializeField] private MenuUnitDisplay ally2MenuUnitDisplay;
    [SerializeField] private MenuUnitDisplay ally3MenuUnitDisplay;

    [SerializeField] private List<Gear> ally1GearSlots = new List<Gear>();
    [SerializeField] private List<Gear> ally2GearSlots = new List<Gear>();
    [SerializeField] private List<Gear> ally3GearSlots = new List<Gear>();

    [SerializeField] private UIElement gearStatsUI;
    [SerializeField] private GameObject gearStatGO;
    [SerializeField] private UIElement gearDescUI;
    [SerializeField] private UIElement gearNameText;

    public Gear selectedGearSlot;
    public Gear selectedBaseGearSlot;
    [Space(2)]
    [Header("Main Ally")]
    public Gear equippedHelmetMain;
    public Gear equippedChestpieceMain;
    public Gear equippedLeggingsMain;
    public Gear equippedBootsMain;
    [Space(2)]
    [Header("Second Ally")]
    public Gear equippedHelmetSec;
    public Gear equippedChestpieceSec;
    public Gear equippedLeggingsSec;
    public Gear equippedBootsSec;
    [Space(2)]
    [Header("Third Ally")]
    public Gear equippedHelmetThi;
    public Gear equippedChestpieceThi;
    public Gear equippedLeggingsThi;
    public Gear equippedBootsThi;
    [Space(2)]
    public bool playerInGearTab;



    public void UpdateGearNameText(string name)
    {
        gearNameText.UpdateContentText(name);
    }

    public void ClearAllGearStats()
    {
        // Clear all gear stats
        GameObject gearStatGO = gearStatsUI.gameObject;
        for (int i = 0; i < gearStatGO.transform.childCount; i++)
        {
            Destroy(gearStatGO.transform.GetChild(i).gameObject);
        }

        GameObject gearDescGO = gearDescUI.gameObject;
        for (int x = 0; x < gearDescGO.transform.childCount; x++)
        {
            Destroy(gearDescGO.transform.GetChild(x).gameObject);
        }
    }
    public void UpdateGearStatDetails()
    {
        ClearAllGearStats();

        // Gear Stats Update
        for (int i = 0; i < 5; i++)
        {
            GameObject spawnedStat = Instantiate(gearStatGO, gearStatsUI.transform.position, Quaternion.identity);
            spawnedStat.transform.SetParent(gearStatsUI.transform);
            spawnedStat.transform.localPosition = Vector2.zero;
            spawnedStat.transform.localScale = Vector2.one;

            UIElement statUI = spawnedStat.GetComponent<UIElement>();

            // Update gear stat UI
            if (i == 0)
                statUI.UpdateContentText("+ " + GetSelectedGearSlot().GetBonusHealth() + " Health");
            else if (i == 1)
                statUI.UpdateContentText("+ " + GetSelectedGearSlot().GetBonusDamage() + " Damage");
            else if (i == 2)
                statUI.UpdateContentText("+ " + GetSelectedGearSlot().GetBonusDamage() + " Healing");
            else if (i == 3)
                statUI.UpdateContentText("+ " + GetSelectedGearSlot().GetBonusDamage() + " Defense");
            else if (i == 4)
                statUI.UpdateContentText("+ " + GetSelectedGearSlot().GetBonusDamage() + " Speed");

            // Gear Stat Name Update
            gearNameText.UpdateContentText(GetSelectedGearSlot().GetGearName());
        }

        // Gear Stat Description Update


    }



    // Start is called before the first frame update
    void Start()
    {
        ToggleTeamGear(false);
        ClearGearSlots();
        ClearAllGearStats();
        //ResetAllGearSelections();
    }

    private void Awake()
    {
        Instance = this;
    }

    public Gear GetSelectedGearSlot()
    {
        return selectedGearSlot;
    }

    public void UpdateSelectedGearSlot(Gear gear)
    {
        selectedGearSlot = gear;
    }

    public Gear GetSelectedBaseGearSlot()
    {
        return selectedBaseGearSlot;
    }

    public void UpdateSelectedBaseGearSlot(Gear gear)
    {
        selectedBaseGearSlot = gear;

        gear.ToggleGearSelected(true);
    }

    public void ClearGearSlots()
    {
        for (int i = 0; i < ally3GearSlots.Count; i++)
        {
            ally1GearSlots[i].UpdateGearImage(clearSlotSprite);
            ally2GearSlots[i].UpdateGearImage(clearSlotSprite);
            ally3GearSlots[i].UpdateGearImage(clearSlotSprite);
        }
    }

    public void UpdateGearSlotsBaseDefault(Gear gear = null, bool ally1 = false, bool ally2 = false, bool ally3 = false)
    {
        string removedPieceType = "";
        if (gear != null)
        {
            removedPieceType = gear.GetCurGearType().ToString();
        }

        if (ally1)
        {
            for (int i = 0; i < ally1GearSlots.Count; i++)
            {
                // Place helmet
                if (i == 0)
                {
                    if (removedPieceType == "HELMET")
                        ally1GearSlots[i].UpdateGearImage(helmetSlotSprite);
                }
                // Place chestpiece
                if (i == 1)
                {
                    if (removedPieceType == "CHESTPIECE")
                        ally1GearSlots[i].UpdateGearImage(chestSlotSprite);
                }
                // Place leggings
                if (i == 2)
                {
                    if (removedPieceType == "LEGGINGS")
                        ally1GearSlots[i].UpdateGearImage(legsSlotSprite);
                }
                // Place boots 
                if (i == 3)
                {
                    if (removedPieceType == "BOOTS")
                        ally1GearSlots[i].UpdateGearImage(bootsSlotSprite);
                }

                ally1GearSlots[i].UpdateGearStatis(Gear.GearStatis.DEFAULT);
                ally1GearSlots[i].UpdateGearOwnedBy(Gear.GearOwnedBy.MAIN);
            }
        }

        if (ally2)
        {
            for (int x = 0; x < ally2GearSlots.Count; x++)
            {
                // Place helmet
                if (x == 0)
                {
                    if (removedPieceType == "HELMET")
                        ally2GearSlots[x].UpdateGearImage(helmetSlotSprite);
                }
                // Place chestpiece
                if (x == 1)
                {
                    if (removedPieceType == "CHESTPIECE")
                        ally2GearSlots[x].UpdateGearImage(chestSlotSprite);
                }
                // Place leggings
                if (x == 2)
                {
                    if (removedPieceType == "LEGGINGS")
                        ally2GearSlots[x].UpdateGearImage(legsSlotSprite);
                }
                // Place boots 
                if (x == 3)
                {
                    if (removedPieceType == "BOOTS")
                        ally2GearSlots[x].UpdateGearImage(bootsSlotSprite);
                }

                ally2GearSlots[x].UpdateGearStatis(Gear.GearStatis.DEFAULT);
                ally2GearSlots[x].UpdateGearOwnedBy(Gear.GearOwnedBy.SECOND);
            }
        }

        if (ally3)
        {
            for (int y = 0; y < ally3GearSlots.Count; y++)
            {
                // Place helmet
                if (y == 0)
                {
                    if (removedPieceType == "HELMET")
                        ally1GearSlots[y].UpdateGearImage(helmetSlotSprite);
                }
                // Place chestpiece
                if (y == 1)
                {
                    if (removedPieceType == "CHESTPIECE")
                        ally1GearSlots[y].UpdateGearImage(chestSlotSprite);
                }
                // Place leggings
                if (y == 2)
                {
                    if (removedPieceType == "LEGGINGS")
                        ally1GearSlots[y].UpdateGearImage(legsSlotSprite);
                }
                // Place boots 
                if (y == 3)
                {
                    if (removedPieceType == "BOOTS")
                        ally1GearSlots[y].UpdateGearImage(bootsSlotSprite);
                }

                ally3GearSlots[y].UpdateGearStatis(Gear.GearStatis.DEFAULT);
                ally3GearSlots[y].UpdateGearOwnedBy(Gear.GearOwnedBy.THIRD);
            }
        }
    }

    public void UpdateGearSlotsBase(bool ally1 = false, bool ally2 = false, bool ally3 = false)
    {
        // Ensure each gear slot has correct bg gear sprite

        if (ally1)
        {
            for (int i = 0; i < ally1GearSlots.Count; i++)
            {
                // Place helmet
                if (i == 0)
                {
                    if (equippedHelmetMain == null)
                        ally1GearSlots[i].UpdateGearImage(helmetSlotSprite);
                    else
                        ally1GearSlots[i].UpdateGearImage(equippedHelmetMain.GetGearImage());
                }
                // Place chestpiece
                if (i == 1)
                {
                    if (equippedChestpieceMain == null)
                        ally1GearSlots[i].UpdateGearImage(chestSlotSprite);
                    else
                        ally1GearSlots[i].UpdateGearImage(equippedChestpieceMain.GetGearImage());
                }
                // Place leggings
                if (i == 2)
                {
                    if (equippedLeggingsMain == null)
                        ally1GearSlots[i].UpdateGearImage(legsSlotSprite);
                    else
                        ally1GearSlots[i].UpdateGearImage(equippedLeggingsMain.GetGearImage());
                }
                // Place boots 
                if (i == 3)
                {
                    if (equippedBootsMain == null)
                        ally1GearSlots[i].UpdateGearImage(bootsSlotSprite);
                    else
                        ally1GearSlots[i].UpdateGearImage(equippedBootsMain.GetGearImage());
                }

                ally1GearSlots[i].UpdateGearStatis(Gear.GearStatis.DEFAULT);
                ally1GearSlots[i].UpdateGearOwnedBy(Gear.GearOwnedBy.MAIN);
            }
        }

        if (ally2)
        {
            for (int x = 0; x < ally2GearSlots.Count; x++)
            {
                // Place helmet
                if (x == 0)
                {
                    if (equippedHelmetSec == null)
                        ally2GearSlots[x].UpdateGearImage(helmetSlotSprite);
                    else
                        ally2GearSlots[x].UpdateGearImage(equippedHelmetSec.GetGearImage());
                }
                // Place chestpiece
                if (x == 1)
                {
                    if (equippedChestpieceSec == null)
                        ally2GearSlots[x].UpdateGearImage(chestSlotSprite);
                    else
                        ally2GearSlots[x].UpdateGearImage(equippedChestpieceSec.GetGearImage());
                }
                // Place leggings
                if (x == 2)
                {
                    if (equippedLeggingsSec == null)
                        ally2GearSlots[x].UpdateGearImage(legsSlotSprite);
                    else
                        ally2GearSlots[x].UpdateGearImage(equippedLeggingsSec.GetGearImage());
                }
                // Place boots 
                if (x == 3)
                {
                    if (equippedBootsSec == null)
                        ally2GearSlots[x].UpdateGearImage(bootsSlotSprite);
                    else
                        ally2GearSlots[x].UpdateGearImage(equippedBootsSec.GetGearImage());
                }

                ally2GearSlots[x].UpdateGearStatis(Gear.GearStatis.DEFAULT);
                ally2GearSlots[x].UpdateGearOwnedBy(Gear.GearOwnedBy.SECOND);
            }
        }

        if (ally3)
        {
            for (int y = 0; y < ally3GearSlots.Count; y++)
            {
                // Place helmet
                if (y == 0)
                {
                    if (equippedHelmetThi == null)
                        ally3GearSlots[y].UpdateGearImage(helmetSlotSprite);
                    else
                        ally3GearSlots[y].UpdateGearImage(equippedHelmetThi.GetGearImage());
                }
                // Place chestpiece
                if (y == 1)
                {
                    if (equippedChestpieceThi == null)
                        ally3GearSlots[y].UpdateGearImage(chestSlotSprite);
                    else
                        ally3GearSlots[y].UpdateGearImage(equippedChestpieceThi.GetGearImage());
                }
                // Place leggings
                if (y == 2)
                {
                    if (equippedLeggingsThi == null)
                        ally3GearSlots[y].UpdateGearImage(legsSlotSprite);
                    else
                        ally3GearSlots[y].UpdateGearImage(equippedLeggingsThi.GetGearImage());
                }
                // Place boots 
                if (y == 3)
                {
                    if (equippedBootsThi == null)
                        ally3GearSlots[y].UpdateGearImage(bootsSlotSprite);
                    else
                        ally3GearSlots[y].UpdateGearImage(equippedBootsThi.GetGearImage());
                }

                ally3GearSlots[y].UpdateGearStatis(Gear.GearStatis.DEFAULT);
                ally3GearSlots[y].UpdateGearOwnedBy(Gear.GearOwnedBy.THIRD);
            }
        }
    }

    public void ToggleAllyGearSets()
    {
        // If ally team has 1 total allies
        if (GameManager.Instance.activeTeam.Count == 1)
        {
            // Toggle correct gear tabs
            ally2GearTabUI.UpdateAlpha(0);
            ally3GearTabUI.UpdateAlpha(0);
            ally1GearTabUI.UpdateAlpha(1);

            // Update visible character ally 
            ally1MenuUnitDisplay.UpdateUnitDisplay(GameManager.Instance.activeTeam[0].unitName);

            // Ensure each gear slot has correct bg gear sprite
            UpdateGearSlotsBase(true);
        }
        // If ally team has 2 total allies
        else if (GameManager.Instance.activeTeam.Count == 2)
        {
            // Toggle correct gear tabs
            ally3GearTabUI.UpdateAlpha(0);
            ally1GearTabUI.UpdateAlpha(1);
            ally2GearTabUI.UpdateAlpha(1);

            // Update visible character ally 
            ally1MenuUnitDisplay.UpdateUnitDisplay(GameManager.Instance.activeTeam[0].unitName);
            ally2MenuUnitDisplay.UpdateUnitDisplay(GameManager.Instance.activeTeam[1].unitName);

            // Ensure each gear slot has correct bg gear sprite
            UpdateGearSlotsBase(true, true);
        }
        // If ally team has 3 total allies
        else if (GameManager.Instance.activeTeam.Count == 3)
        {
            // Toggle correct gear tabs
            ally1GearTabUI.UpdateAlpha(1);
            ally2GearTabUI.UpdateAlpha(1);
            ally3GearTabUI.UpdateAlpha(1);

            // Update visible character ally 
            ally1MenuUnitDisplay.UpdateUnitDisplay(GameManager.Instance.activeTeam[0].unitName);
            ally2MenuUnitDisplay.UpdateUnitDisplay(GameManager.Instance.activeTeam[1].unitName);
            ally3MenuUnitDisplay.UpdateUnitDisplay(GameManager.Instance.activeTeam[2].unitName);

            // Ensure each gear slot has correct bg gear sprite
            UpdateGearSlotsBase(true, true, true);
        }
    }

    public void ToggleTeamGear(bool toggle)
    {
        if (toggle)
        {
            teamGearTabUI.UpdateAlpha(1);

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

            ClearAllGearStats();
        }
        else
        {
            teamGearTabUI.UpdateAlpha(0);

            ResetAllGearSelections();

            ToggleGearSlotsClickable(false);

            // Hide team setup tab arrow buttons
            teamSetupTabArrowLeftButton.ToggleButton(false);
            teamSetupTabArrowRightButton.ToggleButton(false);

            // Display team setup tab arrow buttons
            TeamSetup.Instance.gearTabArrowLeftButton.ToggleButton(true);
            TeamSetup.Instance.gearTabArrowRightButton.ToggleButton(true);
        }
    }

    public void ResetAllGearSelections()
    {
        for (int x = 0; x < ally1GearSlots.Count; x++)
        {
            ally1GearSlots[x].ToggleGearSelected(false);
        }
        for (int y = 0; y < ally2GearSlots.Count; y++)
        {
            ally2GearSlots[y].ToggleGearSelected(false);
        }
        for (int z = 0; z < ally3GearSlots.Count; z++)
        {
            ally3GearSlots[z].ToggleGearSelected(false);
        }
    }

    public void ToggleGearSlotsClickable(bool toggle)
    {
        for (int x = 0; x < ally1GearSlots.Count; x++)
        {
            if (toggle)
                ally1GearSlots[x].GetGearSelection().ToggleButton(true);
            else
                ally1GearSlots[x].GetGearSelection().ToggleButton(false);
        }
        for (int y = 0; y < ally2GearSlots.Count; y++)
        {
            if (toggle)
                ally2GearSlots[y].GetGearSelection().ToggleButton(true);
            else
                ally2GearSlots[y].GetGearSelection().ToggleButton(false);
        }
        for (int z = 0; z < ally3GearSlots.Count; z++)
        {
            if (toggle)
                ally3GearSlots[z].GetGearSelection().ToggleButton(true);
            else
                ally3GearSlots[z].GetGearSelection().ToggleButton(false);
        }
    }

    public void EquipItem(Gear gear)
    {
        OwnedGearInven.Instance.ToggleOwnedGearDisplay(false);

        Gear removedGear = null;

        // Remove current equipt item, and place into owned gear

        if (selectedBaseGearSlot.GetGearOwnedBy() == Gear.GearOwnedBy.MAIN)
        {
            // If play ownst at least 1 item
            if (OwnedGearInven.Instance.GetWornGearMainAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedGearInven.Instance.GetWornGearMainAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedGearInven.Instance.GetWornGearMainAlly()[x].GetGearName() == gear.GetGearName())
                    {
                        // Add gear into owned gear
                        OwnedGearInven.Instance.AddOwnedGear(OwnedGearInven.Instance.GetWornGearMainAlly()[x]);

                        // Remove worn gear
                        OwnedGearInven.Instance.RemoveWornGearAllyMain(OwnedGearInven.Instance.GetWornGearMainAlly()[x]);
                        break;
                    }
                }
            }
        }
        else if (selectedBaseGearSlot.GetGearOwnedBy() == Gear.GearOwnedBy.SECOND)
        {
            // If play ownst at least 1 item
            if (OwnedGearInven.Instance.GetWornGearSecondAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedGearInven.Instance.GetWornGearSecondAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedGearInven.Instance.GetWornGearSecondAlly()[x].GetGearName() == gear.GetGearName())
                    {
                        // Add gear into owned gear
                        OwnedGearInven.Instance.AddOwnedGear(OwnedGearInven.Instance.GetWornGearSecondAlly()[x]);

                        // Remove worn gear
                        OwnedGearInven.Instance.RemoveWornGearAllySecond(OwnedGearInven.Instance.GetWornGearSecondAlly()[x]);
                        break;
                    }
                }
            }
        }
        else if (selectedBaseGearSlot.GetGearOwnedBy() == Gear.GearOwnedBy.THIRD)
        {
            // If play ownst at least 1 item
            if (OwnedGearInven.Instance.GetWornGearThirdAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedGearInven.Instance.GetWornGearThirdAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedGearInven.Instance.GetWornGearThirdAlly()[x].GetGearName() == gear.GetGearName())
                    {
                        // Add gear into owned gear
                        OwnedGearInven.Instance.AddOwnedGear(OwnedGearInven.Instance.GetWornGearThirdAlly()[x]);

                        // Remove worn gear
                        OwnedGearInven.Instance.RemoveWornGearAllyThird(OwnedGearInven.Instance.GetWornGearThirdAlly()[x]);
                        break;
                    }
                }
            }
        }


        // Remove gear from owned gear list when equipping
        // Add gear to worn gear
        for (int i = 0; i < OwnedGearInven.Instance.ownedGear.Count; i++)
        {
            if (OwnedGearInven.Instance.ownedGear[i].GetGearName() == gear.GetGearName())
            {
                removedGear = OwnedGearInven.Instance.ownedGear[i];

                OwnedGearInven.Instance.RemoveOwnedGear(removedGear);

                if (selectedBaseGearSlot.GetGearOwnedBy() == Gear.GearOwnedBy.MAIN)
                    OwnedGearInven.Instance.AddWornGearAllyMain(removedGear);
                else if (selectedBaseGearSlot.GetGearOwnedBy() == Gear.GearOwnedBy.SECOND)
                    OwnedGearInven.Instance.AddWornGearAllySecond(removedGear);
                else if (selectedBaseGearSlot.GetGearOwnedBy() == Gear.GearOwnedBy.THIRD)
                    OwnedGearInven.Instance.AddWornGearAllyThird(removedGear);
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

        // Show combined calculated values next to unit
    }

    public void GearSelection(Gear gear, bool select = false)
    {
        // Disable all gear selection border
        ResetAllGearSelections();

        // Enable selected gear slot border
        gear.ToggleGearSelected(true);
        //OwnedGearInven.Instance.FillOwnedGearSlots();

        if (gear.curGearStatis == Gear.GearStatis.DEFAULT)
        {
            UpdateSelectedBaseGearSlot(gear);

            UpdateSelectedGearSlot(gear);

            OwnedGearInven.Instance.EnableOwnedItemsSlotSelection(GetSelectedGearSlot());
        }
        else
        {
            UpdateSelectedGearSlot(gear);
            OwnedGearInven.Instance.EnableOwnedItemsSlotSelection(GetSelectedGearSlot());

            if (!gear.isEmpty && select)
            {
                if (gear.GetGearOwnedBy()  == Gear.GearOwnedBy.MAIN)
                {
                    if (gear.curGearType == Gear.GearType.HELMET)
                    {
                        equippedHelmetMain = gear;
                    }
                    else if (gear.curGearType == Gear.GearType.CHESTPIECE)
                    {
                        equippedChestpieceMain = gear;
                    }
                    else if (gear.curGearType == Gear.GearType.LEGGINGS)
                    {
                        equippedLeggingsMain = gear;
                    }
                    else if (gear.curGearType == Gear.GearType.BOOTS)
                    {
                        equippedBootsMain = gear;
                    }
                }
                else if (gear.GetGearOwnedBy() == Gear.GearOwnedBy.SECOND)
                {
                    if (gear.curGearType == Gear.GearType.HELMET)
                    {
                        equippedHelmetSec = gear;
                    }
                    else if (gear.curGearType == Gear.GearType.CHESTPIECE)
                    {
                        equippedChestpieceSec = gear;
                    }
                    else if (gear.curGearType == Gear.GearType.LEGGINGS)
                    {
                        equippedLeggingsSec = gear;
                    }
                    else if (gear.curGearType == Gear.GearType.BOOTS)
                    {
                        equippedBootsSec = gear;
                    }
                }
                else if (gear.GetGearOwnedBy() == Gear.GearOwnedBy.THIRD)
                {
                    if (gear.curGearType == Gear.GearType.HELMET)
                    {
                        equippedHelmetThi = gear;
                    }
                    else if (gear.curGearType == Gear.GearType.CHESTPIECE)
                    {
                        equippedChestpieceThi = gear;
                    }
                    else if (gear.curGearType == Gear.GearType.LEGGINGS)
                    {
                        equippedLeggingsThi = gear;
                    }
                    else if (gear.curGearType == Gear.GearType.BOOTS)
                    {
                        equippedBootsThi = gear;
                    }
                }

                // Display inven
                EquipItem(gear);
            }
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
            // Initialize gear base data
            gear.ResetGearSlot(true);
            
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
            if (OwnedGearInven.Instance.GetWornGearMainAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedGearInven.Instance.GetWornGearMainAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedGearInven.Instance.GetWornGearMainAlly()[x].GetGearName() == GetSelectedGearSlot().GetGearName())
                    {
                        // Add gear into owned gear
                        OwnedGearInven.Instance.AddOwnedGear(OwnedGearInven.Instance.GetWornGearMainAlly()[x]);

                        // Remove worn gear
                        OwnedGearInven.Instance.RemoveWornGearAllyMain(OwnedGearInven.Instance.GetWornGearMainAlly()[x]);
                        break;
                    }
                }
            }
        }
        else if (GetSelectedBaseGearSlot().GetGearOwnedBy() == Gear.GearOwnedBy.SECOND)
        {
            // If play ownst at least 1 item
            if (OwnedGearInven.Instance.GetWornGearSecondAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedGearInven.Instance.GetWornGearSecondAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedGearInven.Instance.GetWornGearSecondAlly()[x].GetGearName() == GetSelectedGearSlot().GetGearName())
                    {
                        // Add gear into owned gear
                        OwnedGearInven.Instance.AddOwnedGear(OwnedGearInven.Instance.GetWornGearSecondAlly()[x]);

                        // Remove worn gear
                        OwnedGearInven.Instance.RemoveWornGearAllySecond(OwnedGearInven.Instance.GetWornGearSecondAlly()[x]);
                        break;
                    }
                }
            }
        }
        else if (GetSelectedBaseGearSlot().GetGearOwnedBy() == Gear.GearOwnedBy.THIRD)
        {
            // If play ownst at least 1 item
            if (OwnedGearInven.Instance.GetWornGearThirdAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedGearInven.Instance.GetWornGearThirdAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedGearInven.Instance.GetWornGearThirdAlly()[x].GetGearName() == GetSelectedGearSlot().GetGearName())
                    {
                        // Add gear into owned gear
                        OwnedGearInven.Instance.AddOwnedGear(OwnedGearInven.Instance.GetWornGearThirdAlly()[x]);

                        // Remove worn gear
                        OwnedGearInven.Instance.RemoveWornGearAllyThird(OwnedGearInven.Instance.GetWornGearThirdAlly()[x]);
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
