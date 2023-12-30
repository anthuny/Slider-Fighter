using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamGearManager : MonoBehaviour
{
    public static TeamGearManager Instance;

    [SerializeField] private UIElement teamGearTabUI;

    public Color statDefaultColour;
    public Color statIncreasedColour;
    public Color statDecreasedColour;

    public float timeStatIncColour = 1;
    public float timeStatDecColour = 1;

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
    public GearPiece equippedHelmetMain;
    public GearPiece equippedChestpieceMain;
    public GearPiece equippedLeggingsMain;
    public GearPiece equippedBootsMain;
    [Space(2)]
    [Header("Second Ally")]
    public GearPiece equippedHelmetSec;
    public GearPiece equippedChestpieceSec;
    public GearPiece equippedLeggingsSec;
    public GearPiece equippedBootsSec;
    [Space(2)]
    [Header("Third Ally")]
    public GearPiece equippedHelmetThi;
    public GearPiece equippedChestpieceThi;
    public GearPiece equippedLeggingsThi;
    public GearPiece equippedBootsThi;
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

    public void UpdateUnitStatsEquip(Gear gear)
    {
        if (selectedBaseGearSlot.GetGearOwnedBy() == Gear.GearOwnedBy.MAIN)
        {
            UnitFunctionality unitFunc = GameManager.Instance.GetUnitFunctionality(GameManager.Instance.activeTeam[0]);

            unitFunc.UpdateUnitMaxHealth(gear.GetBonusHealth(), false, true);
            unitFunc.UpdateUnitCurHealth(gear.GetBonusHealth(), false, false);
            unitFunc.UpdateUnitDamageHits(gear.GetBonusDamage(), true);
            unitFunc.UpdateUnitHealingHits(gear.GetBonusHealing(), true);
            unitFunc.UpdateUnitDefenseChange(gear.GetBonusDefense(), true);
            unitFunc.UpdateUnitSpeedChange(gear.GetBonusSpeed(), true);

            ally1MenuUnitDisplay.UpdateUnitStats(unitFunc);
        }
        else if (selectedBaseGearSlot.GetGearOwnedBy() == Gear.GearOwnedBy.SECOND)
        {
            UnitFunctionality unitFunc = GameManager.Instance.GetUnitFunctionality(GameManager.Instance.activeTeam[1]);

            unitFunc.UpdateUnitMaxHealth(gear.GetBonusHealth(), false, true);
            unitFunc.UpdateUnitCurHealth(gear.GetBonusHealth(), false, false);
            unitFunc.UpdateUnitDamageHits(gear.GetBonusDamage(), true);
            unitFunc.UpdateUnitHealingHits(gear.GetBonusHealing(), true);
            unitFunc.UpdateUnitDefenseChange(gear.GetBonusDefense(), true);
            unitFunc.UpdateUnitSpeedChange(gear.GetBonusSpeed(), true);

            ally2MenuUnitDisplay.UpdateUnitStats(unitFunc);
        }
        else if (selectedBaseGearSlot.GetGearOwnedBy() == Gear.GearOwnedBy.THIRD)
        {
            UnitFunctionality unitFunc = GameManager.Instance.GetUnitFunctionality(GameManager.Instance.activeTeam[2]);

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
        if (selectedBaseGearSlot.GetGearOwnedBy() == Gear.GearOwnedBy.MAIN)
        {
            UnitFunctionality unitFunc = GameManager.Instance.GetUnitFunctionality(GameManager.Instance.activeTeam[0]);

            unitFunc.UpdateUnitCurHealth(gear.GetBonusHealth(), true, false, false);
            unitFunc.UpdateUnitMaxHealth(gear.GetBonusHealth(), false, false);
            unitFunc.UpdateUnitDamageHits(gear.GetBonusDamage(), false);
            unitFunc.UpdateUnitHealingHits(gear.GetBonusHealing(), false);
            unitFunc.UpdateUnitDefenseChange(gear.GetBonusDefense(), false);
            unitFunc.UpdateUnitSpeedChange(gear.GetBonusSpeed(), false);

            ally1MenuUnitDisplay.UpdateUnitStats(unitFunc);
        }
        else if (selectedBaseGearSlot.GetGearOwnedBy() == Gear.GearOwnedBy.SECOND)
        {
            UnitFunctionality unitFunc = GameManager.Instance.GetUnitFunctionality(GameManager.Instance.activeTeam[1]);

            unitFunc.UpdateUnitCurHealth(gear.GetBonusHealth(), true, false, false);
            unitFunc.UpdateUnitMaxHealth(gear.GetBonusHealth(), false, false);
            unitFunc.UpdateUnitDamageHits(gear.GetBonusDamage(), false);
            unitFunc.UpdateUnitHealingHits(gear.GetBonusHealing(), false);
            unitFunc.UpdateUnitDefenseChange(gear.GetBonusDefense(), false);
            unitFunc.UpdateUnitSpeedChange(gear.GetBonusSpeed(), false);

            ally2MenuUnitDisplay.UpdateUnitStats(unitFunc);
        }
        else if (selectedBaseGearSlot.GetGearOwnedBy() == Gear.GearOwnedBy.THIRD)
        {
            UnitFunctionality unitFunc = GameManager.Instance.GetUnitFunctionality(GameManager.Instance.activeTeam[2]);

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
            GameObject spawnedStat = Instantiate(gearStatGO, gearStatsUI.transform.position, Quaternion.identity);
            spawnedStat.transform.SetParent(gearStatsUI.transform);
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
                        ally1GearSlots[i].UpdateGearImage(equippedHelmetMain.gearIcon);
                }
                // Place chestpiece
                if (i == 1)
                {
                    if (equippedChestpieceMain == null)
                        ally1GearSlots[i].UpdateGearImage(chestSlotSprite);
                    else
                        ally1GearSlots[i].UpdateGearImage(equippedChestpieceMain.gearIcon);
                }
                // Place leggings
                if (i == 2)
                {
                    if (equippedLeggingsMain == null)
                        ally1GearSlots[i].UpdateGearImage(legsSlotSprite);
                    else
                        ally1GearSlots[i].UpdateGearImage(equippedLeggingsMain.gearIcon);
                }
                // Place boots 
                if (i == 3)
                {
                    if (equippedBootsMain == null)
                        ally1GearSlots[i].UpdateGearImage(bootsSlotSprite);
                    else
                        ally1GearSlots[i].UpdateGearImage(equippedBootsMain.gearIcon);
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
                        ally2GearSlots[x].UpdateGearImage(equippedHelmetSec.gearIcon);
                }
                // Place chestpiece
                if (x == 1)
                {
                    if (equippedChestpieceSec == null)
                        ally2GearSlots[x].UpdateGearImage(chestSlotSprite);
                    else
                        ally2GearSlots[x].UpdateGearImage(equippedChestpieceSec.gearIcon);
                }
                // Place leggings
                if (x == 2)
                {
                    if (equippedLeggingsSec == null)
                        ally2GearSlots[x].UpdateGearImage(legsSlotSprite);
                    else
                        ally2GearSlots[x].UpdateGearImage(equippedLeggingsSec.gearIcon);
                }
                // Place boots 
                if (x == 3)
                {
                    if (equippedBootsSec == null)
                        ally2GearSlots[x].UpdateGearImage(bootsSlotSprite);
                    else
                        ally2GearSlots[x].UpdateGearImage(equippedBootsSec.gearIcon);
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
                        ally3GearSlots[y].UpdateGearImage(equippedHelmetThi.gearIcon);
                }
                // Place chestpiece
                if (y == 1)
                {
                    if (equippedChestpieceThi == null)
                        ally3GearSlots[y].UpdateGearImage(chestSlotSprite);
                    else
                        ally3GearSlots[y].UpdateGearImage(equippedChestpieceThi.gearIcon);
                }
                // Place leggings
                if (y == 2)
                {
                    if (equippedLeggingsThi == null)
                        ally3GearSlots[y].UpdateGearImage(legsSlotSprite);
                    else
                        ally3GearSlots[y].UpdateGearImage(equippedLeggingsThi.gearIcon);
                }
                // Place boots 
                if (y == 3)
                {
                    if (equippedBootsThi == null)
                        ally3GearSlots[y].UpdateGearImage(bootsSlotSprite);
                    else
                        ally3GearSlots[y].UpdateGearImage(equippedBootsThi.gearIcon);
                }

                ally3GearSlots[y].UpdateGearStatis(Gear.GearStatis.DEFAULT);
                ally3GearSlots[y].UpdateGearOwnedBy(Gear.GearOwnedBy.THIRD);
            }
        }
    }

    void ClearEmptyGearSlots()
    {
        if (GameManager.Instance.activeTeam.Count == 1)
            UpdateGearSlotsBase(true);
        else if (GameManager.Instance.activeTeam.Count == 2)
            UpdateGearSlotsBase(true, true);
        else if (GameManager.Instance.activeTeam.Count == 3)
            UpdateGearSlotsBase(true, true, true);
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

            // Clears empty base gear slots if empty
            ClearEmptyGearSlots();

            ClearAllGearStats();

            if (GameManager.Instance.activeTeam.Count == 1)
                ally1MenuUnitDisplay.UpdateUnitStats(GameManager.Instance.GetUnitFunctionality(GameManager.Instance.activeTeam[0]));
            if (GameManager.Instance.activeTeam.Count == 2)
            {
                ally1MenuUnitDisplay.UpdateUnitStats(GameManager.Instance.GetUnitFunctionality(GameManager.Instance.activeTeam[0]));
                ally2MenuUnitDisplay.UpdateUnitStats(GameManager.Instance.GetUnitFunctionality(GameManager.Instance.activeTeam[1]));
            }
            if (GameManager.Instance.activeTeam.Count == 3)
            {
                ally1MenuUnitDisplay.UpdateUnitStats(GameManager.Instance.GetUnitFunctionality(GameManager.Instance.activeTeam[0]));
                ally2MenuUnitDisplay.UpdateUnitStats(GameManager.Instance.GetUnitFunctionality(GameManager.Instance.activeTeam[1]));
                ally3MenuUnitDisplay.UpdateUnitStats(GameManager.Instance.GetUnitFunctionality(GameManager.Instance.activeTeam[2]));
            }
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
            {
                ally1GearSlots[x].GetGearSelection().ToggleButton(true);
                ally1GearSlots[x].ToggleMainGear(true);
                ally1GearSlots[x].ToggleOwnedGearButton(true);
                ally1GearSlots[x].ToggleEquipButton(true);
            }
            else
            {
                ally1GearSlots[x].GetGearSelection().ToggleButton(false);
                ally1GearSlots[x].ToggleMainGear(false);
                ally1GearSlots[x].ToggleOwnedGearButton(false);
                ally2GearSlots[x].ToggleEquipButton(false);
            }
        }
        for (int y = 0; y < ally2GearSlots.Count; y++)
        {
            if (toggle)
            {
                ally2GearSlots[y].GetGearSelection().ToggleButton(true);
                ally2GearSlots[y].ToggleMainGear(true);
                ally2GearSlots[y].ToggleOwnedGearButton(true);
                ally2GearSlots[y].ToggleEquipButton(true);
            }

            else
            {
                ally2GearSlots[y].GetGearSelection().ToggleButton(false);
                ally2GearSlots[y].ToggleMainGear(false);
                ally2GearSlots[y].ToggleOwnedGearButton(false);
                ally2GearSlots[y].ToggleEquipButton(false);
            }
        }
        for (int z = 0; z < ally3GearSlots.Count; z++)
        {
            if (toggle)
            {
                ally3GearSlots[z].GetGearSelection().ToggleButton(true);
                ally3GearSlots[z].ToggleMainGear(true);
                ally3GearSlots[z].ToggleOwnedGearButton(true);
                ally3GearSlots[z].ToggleEquipButton(true);
            }
            else
            {
                ally3GearSlots[z].GetGearSelection().ToggleButton(false);
                ally3GearSlots[z].ToggleMainGear(false);
                ally3GearSlots[z].ToggleOwnedGearButton(false);
                ally3GearSlots[z].ToggleEquipButton(false);
            }
        }

        int count = OwnedGearInven.Instance.ownedGearSlots.Count;

        for (int i = 0; i < count; i++)
        {
            if (toggle)
            {
                OwnedGearInven.Instance.ownedGearSlots[i].GetGearSelection().ToggleButton(true);
                OwnedGearInven.Instance.ownedGearSlots[i].ToggleMainGear(true);
                //OwnedGearInven.Instance.ownedGearSlots[i].ToggleOwnedGearButton(true);
                OwnedGearInven.Instance.ownedGearSlots[i].ToggleEquipButton(true);
            }
            else
            {
                OwnedGearInven.Instance.ownedGearSlots[i].GetGearSelection().ToggleButton(false);
                OwnedGearInven.Instance.ownedGearSlots[i].ToggleMainGear(false);
                //OwnedGearInven.Instance.ownedGearSlots[i].ToggleOwnedGearButton(false);
                OwnedGearInven.Instance.ownedGearSlots[i].ToggleEquipButton(false);
            }
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

        // Update unit stats with stats from gear
        UpdateUnitStatsEquip(gear);

        // Show combined calculated values next to unit
    }

    public void ResetGearOwned()
    {
        equippedHelmetMain = null;
        equippedChestpieceMain = null;
        equippedLeggingsMain = null;
        equippedBootsMain = null;

        equippedHelmetSec = null;
        equippedChestpieceSec = null;
        equippedLeggingsSec = null;
        equippedBootsSec = null;

        equippedHelmetThi = null;
        equippedChestpieceThi = null;
        equippedLeggingsThi = null;
        equippedBootsThi = null;

        OwnedGearInven.Instance.ResetWornGearAllyMain();
        OwnedGearInven.Instance.ResetWornGearAllySecond();
        OwnedGearInven.Instance.ResetWornGearAllyThird();

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
                equippedHelmetMain = newGearPiece;
            else
                equippedHelmetMain = null;
        }
        else if (gearPieceTypeName == "chestMain")
        {
            if (replacing)
                equippedChestpieceMain = newGearPiece;
            else
                equippedChestpieceMain = null;
        }
        else if (gearPieceTypeName == "legsMain")
        {
            if (replacing)
                equippedLeggingsMain = newGearPiece;
            else
                equippedLeggingsMain = null;
        }
        else if (gearPieceTypeName == "bootsMain")
        {
            if (replacing)
                equippedBootsMain = newGearPiece;
            else
                equippedBootsMain = null;
        }

        else if (gearPieceTypeName == "helmSecond")
        {
            if (replacing)
                equippedHelmetSec = newGearPiece;
            else
                equippedHelmetSec = null;
        }
        else if (gearPieceTypeName == "chestSecond")
        {
            if (replacing)
                equippedChestpieceSec = newGearPiece;
            else
                equippedChestpieceSec = null;
        }
        else if (gearPieceTypeName == "legsSecond")
        {
            if (replacing)
                equippedLeggingsSec = newGearPiece;
            else
                equippedLeggingsSec = null;
        }
        else if (gearPieceTypeName == "bootsSecond")
        {
            if (replacing)
                equippedBootsSec = newGearPiece;
            else
                equippedBootsSec = null;
        }

        else if (gearPieceTypeName == "helmThird")
        {
            if (replacing)
                equippedHelmetThi = newGearPiece;
            else
                equippedHelmetThi = null;
        }
        else if (gearPieceTypeName == "chestThird")
        {
            if (replacing)
                equippedChestpieceThi = newGearPiece;
            else
                equippedChestpieceThi = null;
        }
        else if (gearPieceTypeName == "legsThird")
        {
            if (replacing)
                equippedLeggingsThi = newGearPiece;
            else
                equippedLeggingsThi = null;
        }
        else if (gearPieceTypeName == "bootsThird")
        {
            if (replacing)
                equippedBootsThi = newGearPiece;
            else
                equippedBootsThi = null;
        }
    }

    public void GearSelection(Gear gear, bool select = false)
    {
        // Disable all gear selection border
        ResetAllGearSelections();

        // Enable selected gear slot border
        gear.ToggleGearSelected(true);
        //OwnedGearInven.Instance.FillOwnedGearSlots();

        // Bug todo - 2nd / 3rd ally arent having their gear saved.

        if (gear.curGearStatis == Gear.GearStatis.DEFAULT)
        {
            UpdateSelectedBaseGearSlot(gear);

            UpdateSelectedGearSlot(gear);

            OwnedGearInven.Instance.EnableOwnedItemsSlotSelection(GetSelectedGearSlot());
        }
        else
        {

            if (selectedBaseGearSlot != null)
            {


            }
            UpdateSelectedGearSlot(gear);
            OwnedGearInven.Instance.EnableOwnedItemsSlotSelection(GetSelectedGearSlot());

            if (!gear.isEmpty && select)
            {
                GearPiece gearPiece = new GearPiece();

                if (selectedBaseGearSlot.GetGearOwnedBy() == Gear.GearOwnedBy.MAIN)
                {
                    if (gear.curGearType == Gear.GearType.HELMET)
                    {
                        gearPiece.UpdateGearPiece(gear.GetGearName(), gear.GetCurGearType().ToString(), gear.GetRarity().ToString(), gear.GetGearImage(), gear.GetBonusHealth(), gear.GetBonusDamage(), gear.GetBonusHealing(), gear.GetBonusDefense(), gear.GetBonusSpeed());
                        UpdateEquippedGearPiece("helmMain", gearPiece);
                    }
                    else if (gear.curGearType == Gear.GearType.CHESTPIECE)
                    {
                        gearPiece.UpdateGearPiece(gear.GetGearName(), gear.GetCurGearType().ToString(), gear.GetRarity().ToString(), gear.GetGearImage(), gear.GetBonusHealth(), gear.GetBonusDamage(), gear.GetBonusHealing(), gear.GetBonusDefense(), gear.GetBonusSpeed());
                        UpdateEquippedGearPiece("chestMain", gearPiece);
                    }
                    else if (gear.curGearType == Gear.GearType.LEGGINGS)
                    {
                        gearPiece.UpdateGearPiece(gear.GetGearName(), gear.GetCurGearType().ToString(), gear.GetRarity().ToString(), gear.GetGearImage(), gear.GetBonusHealth(), gear.GetBonusDamage(), gear.GetBonusHealing(), gear.GetBonusDefense(), gear.GetBonusSpeed());
                        UpdateEquippedGearPiece("legsMain", gearPiece);
                    }
                    else if (gear.curGearType == Gear.GearType.BOOTS)
                    {
                        gearPiece.UpdateGearPiece(gear.GetGearName(), gear.GetCurGearType().ToString(), gear.GetRarity().ToString(), gear.GetGearImage(), gear.GetBonusHealth(), gear.GetBonusDamage(), gear.GetBonusHealing(), gear.GetBonusDefense(), gear.GetBonusSpeed());
                        UpdateEquippedGearPiece("bootsMain", gearPiece);
                    }
                }
                if (selectedBaseGearSlot.GetGearOwnedBy() == Gear.GearOwnedBy.SECOND)
                {
                    if (gear.curGearType == Gear.GearType.HELMET)
                    {
                        gearPiece.UpdateGearPiece(gear.GetGearName(), gear.GetCurGearType().ToString(), gear.GetRarity().ToString(), gear.GetGearImage(), gear.GetBonusHealth(), gear.GetBonusDamage(), gear.GetBonusHealing(), gear.GetBonusDefense(), gear.GetBonusSpeed());
                        UpdateEquippedGearPiece("helmSecond", gearPiece);
                    }
                    else if (gear.curGearType == Gear.GearType.CHESTPIECE)
                    {
                        gearPiece.UpdateGearPiece(gear.GetGearName(), gear.GetCurGearType().ToString(), gear.GetRarity().ToString(), gear.GetGearImage(), gear.GetBonusHealth(), gear.GetBonusDamage(), gear.GetBonusHealing(), gear.GetBonusDefense(), gear.GetBonusSpeed());
                        UpdateEquippedGearPiece("chestSecond", gearPiece);
                    }
                    else if (gear.curGearType == Gear.GearType.LEGGINGS)
                    {
                        gearPiece.UpdateGearPiece(gear.GetGearName(), gear.GetCurGearType().ToString(), gear.GetRarity().ToString(), gear.GetGearImage(), gear.GetBonusHealth(), gear.GetBonusDamage(), gear.GetBonusHealing(), gear.GetBonusDefense(), gear.GetBonusSpeed());
                        UpdateEquippedGearPiece("legsSecond", gearPiece);
                    }
                    else if (gear.curGearType == Gear.GearType.BOOTS)
                    {
                        gearPiece.UpdateGearPiece(gear.GetGearName(), gear.GetCurGearType().ToString(), gear.GetRarity().ToString(), gear.GetGearImage(), gear.GetBonusHealth(), gear.GetBonusDamage(), gear.GetBonusHealing(), gear.GetBonusDefense(), gear.GetBonusSpeed());
                        UpdateEquippedGearPiece("bootsSecond", gearPiece);
                    }
                }
                if (selectedBaseGearSlot.GetGearOwnedBy() == Gear.GearOwnedBy.THIRD)
                {
                    if (gear.curGearType == Gear.GearType.HELMET)
                    {
                        gearPiece.UpdateGearPiece(gear.GetGearName(), gear.GetCurGearType().ToString(), gear.GetRarity().ToString(), gear.GetGearImage(), gear.GetBonusHealth(), gear.GetBonusDamage(), gear.GetBonusHealing(), gear.GetBonusDefense(), gear.GetBonusSpeed());
                        UpdateEquippedGearPiece("helmThird", gearPiece);
                    }
                    else if (gear.curGearType == Gear.GearType.CHESTPIECE)
                    {
                        gearPiece.UpdateGearPiece(gear.GetGearName(), gear.GetCurGearType().ToString(), gear.GetRarity().ToString(), gear.GetGearImage(), gear.GetBonusHealth(), gear.GetBonusDamage(), gear.GetBonusHealing(), gear.GetBonusDefense(), gear.GetBonusSpeed());
                        UpdateEquippedGearPiece("chestThird", gearPiece);
                    }
                    else if (gear.curGearType == Gear.GearType.LEGGINGS)
                    {
                        gearPiece.UpdateGearPiece(gear.GetGearName(), gear.GetCurGearType().ToString(), gear.GetRarity().ToString(), gear.GetGearImage(), gear.GetBonusHealth(), gear.GetBonusDamage(), gear.GetBonusHealing(), gear.GetBonusDefense(), gear.GetBonusSpeed());
                        UpdateEquippedGearPiece("legsThird", gearPiece);
                    }
                    else if (gear.curGearType == Gear.GearType.BOOTS)
                    {
                        gearPiece.UpdateGearPiece(gear.GetGearName(), gear.GetCurGearType().ToString(), gear.GetRarity().ToString(), gear.GetGearImage(), gear.GetBonusHealth(), gear.GetBonusDamage(), gear.GetBonusHealing(), gear.GetBonusDefense(), gear.GetBonusSpeed());
                        UpdateEquippedGearPiece("bootsThird", gearPiece);
                    }
                }

                // Toggle main gear selection on
                GetSelectedBaseGearSlot().ToggleGearSelected(true);

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
            if (OwnedGearInven.Instance.GetWornGearMainAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedGearInven.Instance.GetWornGearMainAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedGearInven.Instance.GetWornGearMainAlly()[x].GetGearName() == GetSelectedGearSlot().GetGearName())
                    {
                        // Remove saved equipped gear piece (data side)
                        if (OwnedGearInven.Instance.GetWornGearMainAlly()[x].GetCurGearType() == Gear.GearType.HELMET)
                        {
                            UpdateEquippedGearPiece("helmMain", null, false);
                        }
                        else if (OwnedGearInven.Instance.GetWornGearMainAlly()[x].GetCurGearType() == Gear.GearType.CHESTPIECE)
                        {
                            UpdateEquippedGearPiece("chestMain", null, false);
                        }
                        else if (OwnedGearInven.Instance.GetWornGearMainAlly()[x].GetCurGearType() == Gear.GearType.LEGGINGS)
                        {
                            UpdateEquippedGearPiece("legsMain", null, false);
                        }
                        else if (OwnedGearInven.Instance.GetWornGearMainAlly()[x].GetCurGearType() == Gear.GearType.BOOTS)
                        {
                            UpdateEquippedGearPiece("bootsMain", null, false);
                        }

                        // Update unit stats when unequiping
                        UpdateUnitStatsUnEquip(OwnedGearInven.Instance.GetWornGearMainAlly()[x]);

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
                        // Remove saved equipped gear piece (data side)
                        if (OwnedGearInven.Instance.GetWornGearSecondAlly()[x].GetCurGearType() == Gear.GearType.HELMET)
                        {
                            UpdateEquippedGearPiece("helmSecond", null, false);
                        }
                        else if (OwnedGearInven.Instance.GetWornGearSecondAlly()[x].GetCurGearType() == Gear.GearType.CHESTPIECE)
                        {
                            UpdateEquippedGearPiece("chestSecond", null, false);
                        }
                        else if (OwnedGearInven.Instance.GetWornGearSecondAlly()[x].GetCurGearType() == Gear.GearType.LEGGINGS)
                        {
                            UpdateEquippedGearPiece("legsSecond", null, false);
                        }
                        else if (OwnedGearInven.Instance.GetWornGearSecondAlly()[x].GetCurGearType() == Gear.GearType.BOOTS)
                        {
                            UpdateEquippedGearPiece("bootsSecond", null, false);
                        }

                        // Update unit stats when unequiping
                        UpdateUnitStatsUnEquip(OwnedGearInven.Instance.GetWornGearSecondAlly()[x]);

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
                        // Remove saved equipped gear piece (data side)
                        if (OwnedGearInven.Instance.GetWornGearThirdAlly()[x].GetCurGearType() == Gear.GearType.HELMET)
                        {
                            UpdateEquippedGearPiece("helmThird", null, false);
                        }
                        else if (OwnedGearInven.Instance.GetWornGearThirdAlly()[x].GetCurGearType() == Gear.GearType.CHESTPIECE)
                        {
                            UpdateEquippedGearPiece("chestThird", null, false);
                        }
                        else if (OwnedGearInven.Instance.GetWornGearThirdAlly()[x].GetCurGearType() == Gear.GearType.LEGGINGS)
                        {
                            UpdateEquippedGearPiece("legsThird", null, false);
                        }
                        else if (OwnedGearInven.Instance.GetWornGearThirdAlly()[x].GetCurGearType() == Gear.GearType.BOOTS)
                        {
                            UpdateEquippedGearPiece("bootsThird", null, false);
                        }

                        // Update unit stats when unequiping
                        UpdateUnitStatsUnEquip(OwnedGearInven.Instance.GetWornGearThirdAlly()[x]);

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
