using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamGearManager : MonoBehaviour
{
    public static TeamGearManager Instance;

    [SerializeField] private UIElement teamGearTabUI;
    [SerializeField] private ButtonFunctionality toMapButton;
    [SerializeField] private ButtonFunctionality unEquipButton;

    public Color statDefaultColour;
    public Color statIncreasedColour;
    public Color statDecreasedColour;

    public float timeStatIncColour = 1;
    public float timeStatDecColour = 1;

    public Sprite clearSlotSprite;
    public Sprite helmetSlotSprite;
    public Sprite chestSlotSprite;
    public Sprite bootsSlotSprite;

    public ButtonFunctionality teamSetupTabArrowLeftButton;
    public ButtonFunctionality teamSetupTabArrowRightButton;

    [SerializeField] private UIElement ally1GearTabUI;
    [SerializeField] private UIElement ally2GearTabUI;
    [SerializeField] private UIElement ally3GearTabUI;

    [SerializeField] private MenuUnitDisplay ally1MenuUnitDisplay;
    [SerializeField] private MenuUnitDisplay ally2MenuUnitDisplay;
    [SerializeField] private MenuUnitDisplay ally3MenuUnitDisplay;

    public List<Slot> ally1GearSlots = new List<Slot>();
    public List<Slot> ally2GearSlots = new List<Slot>();
    public List<Slot> ally3GearSlots = new List<Slot>();

    [SerializeField] private UIElement gearStatsUI;
    [SerializeField] private GameObject gearStatGO;
    [SerializeField] private UIElement gearDescUI;
    [SerializeField] private UIElement gearNameText;

    public Slot selectedGearSlot;
    public Slot selectedBaseGearSlot;
    [Space(2)]
    [Header("Main Ally")]
    public GearPiece equippedHelmetMain;
    public GearPiece equippedChestpieceMain;
    public GearPiece equippedBootsMain;
    [Space(2)]
    [Header("Second Ally")]
    public GearPiece equippedHelmetSec;
    public GearPiece equippedChestpieceSec;
    public GearPiece equippedBootsSec;
    [Space(2)]
    [Header("Third Ally")]
    public GearPiece equippedHelmetThi;
    public GearPiece equippedChestpieceThi;
    public GearPiece equippedBootsThi;
    [Space(2)]
    public bool playerInGearTab;

    public void UpdateUnequiptGearAlert()
    {
        bool toggle = false;

        if (OwnedLootInven.Instance.ownedGear.Count > 0)
            toggle = true;

        if (toggle)
        {
            if (OwnedLootInven.Instance.GetWornGearThirdAlly().Count < 3 && GameManager.Instance.activeRoomHeroes.Count == 3
                || OwnedLootInven.Instance.GetWornGearSecondAlly().Count < 3 && GameManager.Instance.activeRoomHeroes.Count >= 2
                || OwnedLootInven.Instance.GetWornGearMainAlly().Count < 3 && GameManager.Instance.activeRoomHeroes.Count >= 1)
            {
                MapManager.Instance.mapOverlay.alertGearUnequipt.gameObject.SetActive(true);
                MapManager.Instance.mapOverlay.alertGearUnequipt.UpdateAlpha(1);
            }
            else
            {
                MapManager.Instance.mapOverlay.alertGearUnequipt.gameObject.SetActive(false);
                MapManager.Instance.mapOverlay.alertGearUnequipt.UpdateAlpha(0);
            }
        }
        else
        {
            MapManager.Instance.mapOverlay.alertGearUnequipt.gameObject.SetActive(false);
            MapManager.Instance.mapOverlay.alertGearUnequipt.UpdateAlpha(0);
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

    UnitFunctionality GetKnownUnitFunct(bool ally1 = false, bool ally2 = false, bool ally3 = false)
    {
        if (ally1)
        {
            string unitName = GameManager.Instance.activeTeam[0].unitName;

            for (int i = 0; i < GameManager.Instance.activeRoomAllUnitFunctionalitys.Count; i++)
            {
                if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetUnitName() == unitName)
                {
                    return GameManager.Instance.activeRoomAllUnitFunctionalitys[i];
                }
            }
        }

        if (ally2)
        {
            string unitName2 = GameManager.Instance.activeTeam[1].unitName;

            for (int i = 0; i < GameManager.Instance.activeRoomAllUnitFunctionalitys.Count; i++)
            {
                if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetUnitName() == unitName2)
                {
                    return GameManager.Instance.activeRoomAllUnitFunctionalitys[i];
                }
            }
        }
        if (ally3)
        {
            string unitName3 = GameManager.Instance.activeTeam[2].unitName;

            for (int i = 0; i < GameManager.Instance.activeRoomAllUnitFunctionalitys.Count; i++)
            {
                if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetUnitName() == unitName3)
                {
                    return GameManager.Instance.activeRoomAllUnitFunctionalitys[i];
                }
            }
        }

        return null;
    }

    UnitFunctionality GetUnitFunct()
    {
        if (GameManager.Instance.activeTeam.Count == 1)
        {
            string unitName = GameManager.Instance.activeTeam[0].unitName;

            for (int i = 0; i < GameManager.Instance.activeRoomAllUnitFunctionalitys.Count; i++)
            {
                if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetUnitName() == unitName)
                {
                    return GameManager.Instance.activeRoomAllUnitFunctionalitys[i];
                }
            }
        }

        if (GameManager.Instance.activeTeam.Count == 2)
        {
            string unitName1 = GameManager.Instance.activeTeam[0].unitName;
            string unitName2 = GameManager.Instance.activeTeam[1].unitName;

            for (int i = 0; i < GameManager.Instance.activeRoomAllUnitFunctionalitys.Count; i++)
            {
                if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetUnitName() == unitName2)
                {
                    return GameManager.Instance.activeRoomAllUnitFunctionalitys[i];
                }
                if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetUnitName() == unitName1)
                {
                    return GameManager.Instance.activeRoomAllUnitFunctionalitys[i];
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
                if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetUnitName() == unitName3)
                {
                    return GameManager.Instance.activeRoomAllUnitFunctionalitys[i];
                }
                if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetUnitName() == unitName2)
                {
                    return GameManager.Instance.activeRoomAllUnitFunctionalitys[i];
                }
                if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetUnitName() == unitName1)
                {
                    return GameManager.Instance.activeRoomAllUnitFunctionalitys[i];
                }
            }
        }

        return null;
    }
    public void UpdateUnitStatsEquip(Slot gear)
    {
        if (selectedBaseGearSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.MAIN)
        {
            UnitFunctionality unitFunc = GetKnownUnitFunct(true, false, false);

            // Equipping a gear piece, needs to increase max hp by a set amount
            // set amount = unit starting max health
            unitFunc.UpdateUnitMaxHealth(gear.GetBonusHealth(), false, true);
            unitFunc.UpdateUnitCurHealth(gear.GetBonusHealth(), false, false);
            unitFunc.UpdateUnitPower(gear.GetBonusDamage(), false);
            unitFunc.UpdateHealingPower(gear.GetBonusHealing(), false, true);
            unitFunc.UpdateUnitDefenseChange(gear.GetBonusDefense(), true);
            unitFunc.UpdateUnitSpeedChange(gear.GetBonusSpeed(), true);

            ally1MenuUnitDisplay.UpdateUnitStats(unitFunc);
        }
        else if (selectedBaseGearSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.SECOND)
        {
            UnitFunctionality unitFunc = GetKnownUnitFunct(false, true, false);

            unitFunc.UpdateUnitMaxHealth(gear.GetBonusHealth(), false, true);
            unitFunc.UpdateUnitCurHealth(gear.GetBonusHealth(), false, false);
            unitFunc.UpdateUnitPower(gear.GetBonusDamage(), false);
            unitFunc.UpdateHealingPower(gear.GetBonusHealing(), false, true);
            unitFunc.UpdateUnitDefenseChange(gear.GetBonusDefense(), true);
            unitFunc.UpdateUnitSpeedChange(gear.GetBonusSpeed(), true);

            ally2MenuUnitDisplay.UpdateUnitStats(unitFunc);
        }
        else if (selectedBaseGearSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.THIRD)
        {
            UnitFunctionality unitFunc = GetKnownUnitFunct(false, false, true);

            unitFunc.UpdateUnitMaxHealth(gear.GetBonusHealth(), false, true);
            unitFunc.UpdateUnitCurHealth(gear.GetBonusHealth(), false, false);
            unitFunc.UpdateUnitPower(gear.GetBonusDamage(), false);
            unitFunc.UpdateHealingPower(gear.GetBonusHealing(), false, true);
            unitFunc.UpdateUnitDefenseChange(gear.GetBonusDefense(), true);
            unitFunc.UpdateUnitSpeedChange(gear.GetBonusSpeed(), true);

            ally3MenuUnitDisplay.UpdateUnitStats(unitFunc);
        }
    }

    public void UpdateUnitStatsUnEquip(Slot gear)
    {
        if (selectedBaseGearSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.MAIN)
        {
            UnitFunctionality unitFunc = GetKnownUnitFunct(true,false,false);

            unitFunc.UpdateUnitCurHealth(gear.GetBonusHealth(), true, false, false);
            unitFunc.UpdateUnitMaxHealth(gear.GetBonusHealth(), false, false);
            unitFunc.UpdateUnitPower(gear.GetBonusDamage(), false, false);
            unitFunc.UpdateHealingPower(gear.GetBonusHealing(), false, false);
            unitFunc.UpdateUnitDefenseChange(gear.GetBonusDefense(), false);
            unitFunc.UpdateUnitSpeedChange(gear.GetBonusSpeed(), false);

            ally1MenuUnitDisplay.UpdateUnitStats(unitFunc);
        }
        else if (selectedBaseGearSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.SECOND)
        {
            UnitFunctionality unitFunc = GetKnownUnitFunct(false,true,false);

            unitFunc.UpdateUnitCurHealth(gear.GetBonusHealth(), true, false, false);
            unitFunc.UpdateUnitMaxHealth(gear.GetBonusHealth(), false, false);
            unitFunc.UpdateUnitPower(gear.GetBonusDamage(), false, false);
            unitFunc.UpdateHealingPower(gear.GetBonusHealing(), false, false);
            unitFunc.UpdateUnitDefenseChange(gear.GetBonusDefense(), false);
            unitFunc.UpdateUnitSpeedChange(gear.GetBonusSpeed(), false);

            ally2MenuUnitDisplay.UpdateUnitStats(unitFunc);
        }
        else if (selectedBaseGearSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.THIRD)
        {
            UnitFunctionality unitFunc = GetKnownUnitFunct(false,false,true);

            unitFunc.UpdateUnitCurHealth(gear.GetBonusHealth(), true, false, false);
            unitFunc.UpdateUnitMaxHealth(gear.GetBonusHealth(), false, false);
            unitFunc.UpdateUnitPower(gear.GetBonusDamage(), false, false);
            unitFunc.UpdateHealingPower(gear.GetBonusHealing(), false, false);
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
            gearNameText.UpdateContentText(GetSelectedGearSlot().GetSlotName());
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

        StartCoroutine(ToggleGearButtonsCo());
    }

    IEnumerator ToggleGearButtonsCo()
    {
        yield return new WaitForSeconds(0.2f);
        ToggleGearButtons(false);
    }

    private void Awake()
    {
        Instance = this;
    }

    public Slot GetSelectedGearSlot()
    {
        return selectedGearSlot;
    }

    public void UpdateSelectedGearSlot(Slot gear)
    {
        selectedGearSlot = gear;
    }

    public Slot GetSelectedBaseGearSlot()
    {
        return selectedBaseGearSlot;
    }

    public void UpdateSelectedBaseGearSlot(Slot gear)
    {
        selectedBaseGearSlot = gear;

        gear.ToggleSlotSelection(true);
    }

    public void ClearGearSlots()
    {
        for (int i = 0; i < ally3GearSlots.Count; i++)
        {
            ally1GearSlots[i].UpdateSlotImage(clearSlotSprite);
            ally2GearSlots[i].UpdateSlotImage(clearSlotSprite);
            ally3GearSlots[i].UpdateSlotImage(clearSlotSprite);
        }
    }

    public void UpdateSlotsBaseDefault(Slot slot = null, Item item = null, bool ally1 = false, bool ally2 = false, bool ally3 = false)
    {
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
            for (int i = 0; i < ally1GearSlots.Count; i++)
            {
                // Place helmet
                if (i == 0)
                {
                    if (removedPieceType == "HELMET")
                        ally1GearSlots[i].UpdateSlotImage(helmetSlotSprite);
                }
                // Place chestpiece
                if (i == 1)
                {
                    if (removedPieceType == "CHESTPIECE")
                        ally1GearSlots[i].UpdateSlotImage(chestSlotSprite);
                }
                // Place boots 
                if (i == 2)
                {
                    if (removedPieceType == "BOOTS")
                        ally1GearSlots[i].UpdateSlotImage(bootsSlotSprite);
                }

                ally1GearSlots[i].UpdateGearStatis(Slot.SlotStatis.DEFAULT);
                ally1GearSlots[i].UpdateGearOwnedBy(Slot.SlotOwnedBy.MAIN);
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
                        ally2GearSlots[x].UpdateSlotImage(helmetSlotSprite);
                }
                // Place chestpiece
                if (x == 1)
                {
                    if (removedPieceType == "CHESTPIECE")
                        ally2GearSlots[x].UpdateSlotImage(chestSlotSprite);
                }
                // Place boots 
                if (x == 2)
                {
                    if (removedPieceType == "BOOTS")
                        ally2GearSlots[x].UpdateSlotImage(bootsSlotSprite);
                }

                ally2GearSlots[x].UpdateGearStatis(Slot.SlotStatis.DEFAULT);
                ally2GearSlots[x].UpdateGearOwnedBy(Slot.SlotOwnedBy.SECOND);
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
                        ally3GearSlots[y].UpdateSlotImage(helmetSlotSprite);
                }
                // Place chestpiece
                if (y == 1)
                {
                    if (removedPieceType == "CHESTPIECE")
                        ally3GearSlots[y].UpdateSlotImage(chestSlotSprite);
                }
                // Place boots 
                if (y == 2)
                {
                    if (removedPieceType == "BOOTS")
                        ally3GearSlots[y].UpdateSlotImage(bootsSlotSprite);
                }

                ally3GearSlots[y].UpdateGearStatis(Slot.SlotStatis.DEFAULT);
                ally3GearSlots[y].UpdateGearOwnedBy(Slot.SlotOwnedBy.THIRD);
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
                    {
                        ally1GearSlots[i].isEmpty = true;
                        ally1GearSlots[i].UpdateSlotImage(helmetSlotSprite);
                    }
                    else
                        ally1GearSlots[i].UpdateSlotImage(equippedHelmetMain.gearIcon);
                }
                // Place chestpiece
                if (i == 1)
                {
                    if (equippedChestpieceMain == null)
                    {
                        ally1GearSlots[i].isEmpty = true;
                        ally1GearSlots[i].UpdateSlotImage(chestSlotSprite);
                    }
                    else
                        ally1GearSlots[i].UpdateSlotImage(equippedChestpieceMain.gearIcon);
                }
                // Place boots 
                if (i == 2)
                {
                    if (equippedBootsMain == null)
                    {
                        ally1GearSlots[i].isEmpty = true;
                        ally1GearSlots[i].UpdateSlotImage(bootsSlotSprite);
                    }
                    else
                        ally1GearSlots[i].UpdateSlotImage(equippedBootsMain.gearIcon);
                }

                ally1GearSlots[i].UpdateGearStatis(Slot.SlotStatis.DEFAULT);
                ally1GearSlots[i].UpdateGearOwnedBy(Slot.SlotOwnedBy.MAIN);
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
                    {
                        ally2GearSlots[x].isEmpty = true;
                        ally2GearSlots[x].UpdateSlotImage(helmetSlotSprite);
                    }
                    else
                        ally2GearSlots[x].UpdateSlotImage(equippedHelmetSec.gearIcon);
                }
                // Place chestpiece
                if (x == 1)
                {
                    if (equippedChestpieceSec == null)
                    {
                        ally2GearSlots[x].isEmpty = true;
                        ally2GearSlots[x].UpdateSlotImage(chestSlotSprite);
                    }

                    else
                        ally2GearSlots[x].UpdateSlotImage(equippedChestpieceSec.gearIcon);
                }
                // Place boots 
                if (x == 2)
                {
                    if (equippedBootsSec == null)
                    {
                        ally2GearSlots[x].isEmpty = true;
                        ally2GearSlots[x].UpdateSlotImage(bootsSlotSprite);
                    }
                    else
                        ally2GearSlots[x].UpdateSlotImage(equippedBootsSec.gearIcon);
                }

                ally2GearSlots[x].UpdateGearStatis(Slot.SlotStatis.DEFAULT);
                ally2GearSlots[x].UpdateGearOwnedBy(Slot.SlotOwnedBy.SECOND);
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
                    {
                        ally3GearSlots[y].isEmpty = true;
                        ally3GearSlots[y].UpdateSlotImage(helmetSlotSprite);
                    }
                    else
                        ally3GearSlots[y].UpdateSlotImage(equippedHelmetThi.gearIcon);
                }
                // Place chestpiece
                if (y == 1)
                {
                    if (equippedChestpieceThi == null)
                    {
                        ally3GearSlots[y].isEmpty = true;
                        ally3GearSlots[y].UpdateSlotImage(chestSlotSprite);
                    }
                    else
                        ally3GearSlots[y].UpdateSlotImage(equippedChestpieceThi.gearIcon);
                }
                // Place boots 
                if (y == 2)
                {
                    if (equippedBootsThi == null)
                    {
                        ally3GearSlots[y].isEmpty = true;
                        ally3GearSlots[y].UpdateSlotImage(bootsSlotSprite);
                    }
                    else
                        ally3GearSlots[y].UpdateSlotImage(equippedBootsThi.gearIcon);
                }

                ally3GearSlots[y].UpdateGearStatis(Slot.SlotStatis.DEFAULT);
                ally3GearSlots[y].UpdateGearOwnedBy(Slot.SlotOwnedBy.THIRD);
            }
        }
    }

    public void ClearEmptyGearSlots()
    {
        if (GameManager.Instance.activeTeam.Count == 1)
            UpdateGearSlotsBase(true);
        else if (GameManager.Instance.activeTeam.Count == 2)
            UpdateGearSlotsBase(true, true);
        else if (GameManager.Instance.activeTeam.Count == 3)
            UpdateGearSlotsBase(true, true, true);
    }

    public void ResetAllGearSelections()
    {
        //Debug.Log("resetting");
        for (int x = 0; x < ally1GearSlots.Count; x++)
        {
            ally1GearSlots[x].ToggleSlotSelection(false);
        }
        for (int y = 0; y < ally1GearSlots.Count; y++)
        {
            ally2GearSlots[y].ToggleSlotSelection(false);
        }
        for (int z = 0; z < ally1GearSlots.Count; z++)
        {
            ally3GearSlots[z].ToggleSlotSelection(false);
        }
    }

    public void ToggleAllyGearSets()
    {
        if (GetKnownUnitFunct(true, false, false) == null)
            return;

        // If ally team has 1 total allies
        if (GameManager.Instance.activeTeam.Count == 1)
        {
            // Toggle correct gear tabs
            ally2GearTabUI.UpdateAlpha(0);
            ally3GearTabUI.UpdateAlpha(0);
            ally1GearTabUI.UpdateAlpha(1);

            // Display unit level image
            ally1MenuUnitDisplay.ToggleUnitLevelImage(true, GetKnownUnitFunct(true,false,false).GetUnitLevel());

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

            // Display unit level image
            ally1MenuUnitDisplay.ToggleUnitLevelImage(true, GetKnownUnitFunct(true, false, false).GetUnitLevel());
            ally2MenuUnitDisplay.ToggleUnitLevelImage(true, GetKnownUnitFunct(false, true, false).GetUnitLevel());

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

            // Display unit level image
            ally1MenuUnitDisplay.ToggleUnitLevelImage(true, GetKnownUnitFunct(true, false, false).GetUnitLevel());
            ally2MenuUnitDisplay.ToggleUnitLevelImage(true, GetKnownUnitFunct(false, true, false).GetUnitLevel());
            ally3MenuUnitDisplay.ToggleUnitLevelImage(true, GetKnownUnitFunct(false, false, true).GetUnitLevel());

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

            for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
            {
                GameManager.Instance.activeRoomHeroes[i].ToggleUnitDisplay(false);
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
           // GameManager.Instance.SkillsTabChangeAlly(false);

            ToggleAllSlotsClickable(true, false, true);

            // Hide team setup tab arrow buttons
            SkillsTabManager.Instance.gearTabArrowLeftButton.ToggleButton(true);
            SkillsTabManager.Instance.gearTabArrowRightButton.ToggleButton(true);

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

                    if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetUnitName() == unitName2)
                    {
                        ally2MenuUnitDisplay.UpdateUnitStats(GameManager.Instance.activeRoomAllUnitFunctionalitys[i]);
                    }

                    if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetUnitName() == unitName1)
                    {
                        ally1MenuUnitDisplay.UpdateUnitStats(GameManager.Instance.activeRoomAllUnitFunctionalitys[i]);
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
                    if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetUnitName() == unitName3)
                    {
                        ally3MenuUnitDisplay.UpdateUnitStats(GameManager.Instance.activeRoomAllUnitFunctionalitys[i]);
                    }
                    if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetUnitName() == unitName2)
                    {
                        ally2MenuUnitDisplay.UpdateUnitStats(GameManager.Instance.activeRoomAllUnitFunctionalitys[i]);
                    }
                    if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetUnitName() == unitName1)
                    {
                        ally1MenuUnitDisplay.UpdateUnitStats(GameManager.Instance.activeRoomAllUnitFunctionalitys[i]);
                    }
                }
            }
        }
        else
        {
            teamGearTabUI.UpdateAlpha(0);

            // Toggle to map button for team gear
            ToggleToMapButton(false);
            SkillsTabManager.Instance.ToggleToMapButton(true);

            ResetAllBaseGearSelections();

            ToggleAllSlotsClickable(false, false);

            // Hide team setup tab arrow buttons
            teamSetupTabArrowLeftButton.ToggleButton(false);
            teamSetupTabArrowRightButton.ToggleButton(false);

            // Display team setup tab arrow buttons
            SkillsTabManager.Instance.gearTabArrowLeftButton.ToggleButton(true);
            SkillsTabManager.Instance.gearTabArrowRightButton.ToggleButton(true);
        }
    }

    public void ResetAllBaseGearSelections()
    {
        for (int x = 0; x < ally1GearSlots.Count; x++)
        {
            ally1GearSlots[x].ToggleSlotSelection(false);
        }
        for (int y = 0; y < ally2GearSlots.Count; y++)
        {
            ally2GearSlots[y].ToggleSlotSelection(false);
        }
        for (int z = 0; z < ally3GearSlots.Count; z++)
        {
            ally3GearSlots[z].ToggleSlotSelection(false);
        }
    }

    public void ToggleAllSlotsClickable(bool toggle, bool doOwnedSlots = true, bool doBaseGearSlots = true, bool doOwnedSlotsVisual = true)
    {
        //Debug.Log("toggling clickable " + toggle);

        if (doBaseGearSlots)
        {
            for (int x = 0; x < ally1GearSlots.Count; x++)
            {
                if (toggle)
                {
                    ally1GearSlots[x].GetSlotUI().ToggleButton(true);
                    ally1GearSlots[x].ToggleMainSlot(true);
                    ally1GearSlots[x].ToggleOwnedGearButton(true);
                    ally1GearSlots[x].ToggleEquipButton(true);
                }
                else
                {
                    ally1GearSlots[x].GetSlotUI().ToggleButton(false);
                    ally1GearSlots[x].ToggleMainSlot(false);
                    //ally1GearSlots[x].ToggleOwnedGearButton(false);
                    //ally1GearSlots[x].ToggleEquipButton(false);
                }
            }

            for (int y = 0; y < ally2GearSlots.Count; y++)
            {
                if (toggle)
                {
                    ally2GearSlots[y].GetSlotUI().ToggleButton(true);
                    ally2GearSlots[y].ToggleMainSlot(true);
                    ally2GearSlots[y].ToggleOwnedGearButton(true);
                    ally2GearSlots[y].ToggleEquipButton(true);
                }

                else
                {
                    ally2GearSlots[y].GetSlotUI().ToggleButton(false);
                    ally2GearSlots[y].ToggleMainSlot(false);
                    ally2GearSlots[y].ToggleOwnedGearButton(false);
                    ally2GearSlots[y].ToggleEquipButton(false);
                }
            }

            for (int z = 0; z < ally3GearSlots.Count; z++)
            {
                if (toggle)
                {
                    ally3GearSlots[z].GetSlotUI().ToggleButton(true);
                    ally3GearSlots[z].ToggleMainSlot(true);
                    ally3GearSlots[z].ToggleOwnedGearButton(true);
                    ally3GearSlots[z].ToggleEquipButton(true);
                }
                else
                {
                    ally3GearSlots[z].GetSlotUI().ToggleButton(false);
                    ally3GearSlots[z].ToggleMainSlot(false);
                    ally3GearSlots[z].ToggleOwnedGearButton(false);
                    ally3GearSlots[z].ToggleEquipButton(false);
                }
            }
        }

        if (doOwnedSlots)
        {
            int count = OwnedLootInven.Instance.ownedLootSlots.Count;

            OwnedLootInven.Instance.ToggleOwnedSlotEquipButton(toggle);

            for (int i = 0; i < count; i++)
            {
                if (toggle)
                {
                    //Debug.Log("gear tab " + toggle);
                    OwnedLootInven.Instance.ownedLootSlots[i].GetSlotUI().ToggleButton(true);
                    OwnedLootInven.Instance.ownedLootSlots[i].ToggleMainSlot(true);
                    //OwnedLootInven.Instance.ownedLootSlots[i].ToggleOwnedGearEquipButton(true);
                    OwnedLootInven.Instance.ownedLootSlots[i].ToggleEquipButton(true);
                }
                else
                {
                    //Debug.Log("gear tab " + toggle);
                    OwnedLootInven.Instance.ownedLootSlots[i].GetSlotUI().ToggleButton(false);
                    if (doOwnedSlotsVisual)
                        OwnedLootInven.Instance.ownedLootSlots[i].ToggleMainSlot(false);
                    //OwnedLootInven.Instance.ownedLootSlots[i].ToggleOwnedGearEquipButton(false);
                    OwnedLootInven.Instance.ownedLootSlots[i].ToggleEquipButton(false);
                }
            }
        }
    }

    public void EquipGear(Slot gear)
    {
        OwnedLootInven.Instance.ToggleOwnedGearDisplay(false);

        Slot removedGear = null;

        // Remove current equipt item, and place into owned gear
        /*
        if (selectedBaseGearSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.MAIN)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornGearMainAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornGearMainAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornGearMainAlly()[x])
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
        else if (selectedBaseGearSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.SECOND)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornGearSecondAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornGearSecondAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornGearSecondAlly()[x])
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
        else if (selectedBaseGearSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.THIRD)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornGearThirdAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornGearThirdAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornGearThirdAlly()[x])
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
        */

        // Remove gear from owned gear list when equipping
        // Add gear to worn gear
        for (int i = 0; i < OwnedLootInven.Instance.ownedGear.Count; i++)
        {
            if (OwnedLootInven.Instance.ownedGear[i].GetSlotName() == gear.GetSlotName())
            {
                removedGear = OwnedLootInven.Instance.ownedGear[i];

                OwnedLootInven.Instance.RemoveOwnedGear(removedGear);

                if (selectedBaseGearSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.MAIN)
                    OwnedLootInven.Instance.AddWornGearAllyMain(removedGear);
                else if (selectedBaseGearSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.SECOND)
                    OwnedLootInven.Instance.AddWornGearAllySecond(removedGear);
                else if (selectedBaseGearSlot.GetSlotOwnedBy() == Slot.SlotOwnedBy.THIRD)
                    OwnedLootInven.Instance.AddWornGearAllyThird(removedGear);
                break;
            }
        }

        GearRewards.Instance.IncrementSpawnedGearCount();

        GetSelectedBaseGearSlot().UpdateSlotCode(GearRewards.Instance.spawnedGearCount);

        GetSelectedBaseGearSlot().UpdateSlotImage(gear.GetSlotImage());
        GetSelectedBaseGearSlot().UpdateSlotName(gear.GetSlotName());
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

    public void ResetGearTab()
    {
        if (GameManager.Instance.activeTeam.Count == 1)
            ResetHeroGearOwned(1);
        else if (GameManager.Instance.activeTeam.Count == 2)
            ResetHeroGearOwned(2);
    }
    public void ResetHeroGearOwned(int heroIndex)
    {
        if (heroIndex == 0)
        {
            equippedHelmetMain = null;
            equippedChestpieceMain = null;
            equippedBootsMain = null;

            //OwnedLootInven.Instance.ResetWornGearAllyMain();

            ally1MenuUnitDisplay.ResetUnitStats();

            for (int i = 0; i < 3; i++)
            {
                ally1GearSlots[i].isEmpty = true;
            }

            ClearAllGearStats();
            UpdateGearNameText("");

            ClearEmptyGearSlots();
        }
        else if (heroIndex == 1)
        {
            
            equippedHelmetSec = null;
            equippedChestpieceSec = null;
            equippedBootsSec = null;

            //OwnedLootInven.Instance.ResetWornGearAllySecond();

            ally2MenuUnitDisplay.ResetUnitStats();
            
            for (int i = 0; i < 3; i++)
            {
                ally2GearSlots[i].isEmpty = true;
            }

            ClearAllGearStats();
            UpdateGearNameText("");

            ClearEmptyGearSlots();
        }
        else if (heroIndex == 2)
        {           
            equippedHelmetThi = null;
            equippedChestpieceThi = null;
            equippedBootsThi = null;

            //OwnedLootInven.Instance.ResetWornGearAllyThird();

            ally3MenuUnitDisplay.ResetUnitStats();

            for (int i = 0; i < 3; i++)
            {
                ally3GearSlots[i].isEmpty = true;
            }
            

            ClearAllGearStats();
            UpdateGearNameText("");

            ClearEmptyGearSlots();
        }
    }

    public void ResetGearOwned()
    {
        equippedHelmetMain = null;
        equippedChestpieceMain = null;
        equippedBootsMain = null;

        equippedHelmetSec = null;
        equippedChestpieceSec = null;
        equippedBootsSec = null;

        equippedHelmetThi = null;
        equippedChestpieceThi = null;
        equippedBootsThi = null;

        OwnedLootInven.Instance.ResetWornGearAllyMain();
        OwnedLootInven.Instance.ResetWornGearAllySecond();
        OwnedLootInven.Instance.ResetWornGearAllyThird();

        ally1MenuUnitDisplay.ResetUnitStats();
        ally2MenuUnitDisplay.ResetUnitStats();
        ally3MenuUnitDisplay.ResetUnitStats();

        for (int i = 0; i < 3; i++)
        {
            ally1GearSlots[i].isEmpty = true;
            ally2GearSlots[i].isEmpty = true;
            ally3GearSlots[i].isEmpty = true;
        }


        ClearAllGearStats();
        UpdateGearNameText("");

        ClearEmptyGearSlots();
    }

    public void ToggleGearButtons(bool toggle = true)
    {
        for (int i = 0; i < ally1GearSlots.Count; i++)
        {
            ally1GearSlots[i].ownedSlotButton.ToggleButton(toggle);
            ally1GearSlots[i].ToggleEquipButton(toggle);
            ally2GearSlots[i].ownedSlotButton.ToggleButton(toggle);
            ally2GearSlots[i].ToggleEquipButton(toggle);
            ally3GearSlots[i].ownedSlotButton.ToggleButton(toggle);
            ally3GearSlots[i].ToggleEquipButton(toggle);
        }
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
        else if (gearPieceTypeName == "bootsThird")
        {
            if (replacing)
                equippedBootsThi = newGearPiece;
            else
                equippedBootsThi = null;
        }
    }

    public void GearSelection(Slot gear, bool select = false)
    {
        if (playerInGearTab)
        {
            if (gear.curSlotStatis == Slot.SlotStatis.OWNED)
            {
                gear.ToggleCoverUI(false);
            }
        }

        // Disable all gear selection border
        ResetAllBaseGearSelections();

        // Enable selected gear slot border
        gear.ToggleSlotSelection(true);
        //OwnedGearInven.Instance.FillOwnedGearSlots();

        // Bug todo - 2nd / 3rd ally arent having their gear saved.

        if (gear.curSlotStatis == Slot.SlotStatis.DEFAULT)
        {
            UpdateSelectedBaseGearSlot(gear);

            UpdateSelectedGearSlot(gear);

            OwnedLootInven.Instance.EnableOwnedItemsSlotSelection(GetSelectedBaseGearSlot());

            // Toggle main gear selection on
            GetSelectedBaseGearSlot().ToggleSlotSelection(true);
            GetSelectedGearSlot().ToggleSlotSelection(true);
        }
        else
        {
            UpdateSelectedGearSlot(gear);
            OwnedLootInven.Instance.EnableOwnedItemsSlotSelection(GetSelectedGearSlot());
            SkillsTabManager.Instance.UpdateSelectedOwnedSlot(gear);

            GetSelectedGearSlot().ToggleSlotSelection(true);

            if (!select)
            {
                OwnedLootInven.Instance.ResetOwnedSlotEquipButton();
                OwnedLootInven.Instance.ownedLootSlots[OwnedLootInven.Instance.ownedLootSlots.IndexOf(gear)].ToggleEquipButton(true);
            }

            if (select)
            {
                GearPiece gearPiece = new GearPiece();

                if (GetSelectedBaseGearSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.MAIN)
                {
                    if (gear.curGearType == Slot.SlotPieceType.HELMET)
                    {
                        gearPiece.UpdateGearPiece(gear.GetSlotName(), gear.GetCurGearType().ToString(), gear.GetRarity().ToString(), gear.GetSlotImage(), gear.GetBonusHealth(), gear.GetBonusDamage(), gear.GetBonusHealing(), gear.GetBonusDefense(), gear.GetBonusSpeed());
                        UpdateEquippedGearPiece("helmMain", gearPiece);
                    }
                    else if (gear.curGearType == Slot.SlotPieceType.CHESTPIECE)
                    {
                        gearPiece.UpdateGearPiece(gear.GetSlotName(), gear.GetCurGearType().ToString(), gear.GetRarity().ToString(), gear.GetSlotImage(), gear.GetBonusHealth(), gear.GetBonusDamage(), gear.GetBonusHealing(), gear.GetBonusDefense(), gear.GetBonusSpeed());
                        UpdateEquippedGearPiece("chestMain", gearPiece);
                    }
                    else if (gear.curGearType == Slot.SlotPieceType.BOOTS)
                    {
                        gearPiece.UpdateGearPiece(gear.GetSlotName(), gear.GetCurGearType().ToString(), gear.GetRarity().ToString(), gear.GetSlotImage(), gear.GetBonusHealth(), gear.GetBonusDamage(), gear.GetBonusHealing(), gear.GetBonusDefense(), gear.GetBonusSpeed());
                        UpdateEquippedGearPiece("bootsMain", gearPiece);
                    }
                }
                if (GetSelectedBaseGearSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.SECOND)
                {
                    if (gear.curGearType == Slot.SlotPieceType.HELMET)
                    {
                        gearPiece.UpdateGearPiece(gear.GetSlotName(), gear.GetCurGearType().ToString(), gear.GetRarity().ToString(), gear.GetSlotImage(), gear.GetBonusHealth(), gear.GetBonusDamage(), gear.GetBonusHealing(), gear.GetBonusDefense(), gear.GetBonusSpeed());
                        UpdateEquippedGearPiece("helmSecond", gearPiece);
                    }
                    else if (gear.curGearType == Slot.SlotPieceType.CHESTPIECE)
                    {
                        gearPiece.UpdateGearPiece(gear.GetSlotName(), gear.GetCurGearType().ToString(), gear.GetRarity().ToString(), gear.GetSlotImage(), gear.GetBonusHealth(), gear.GetBonusDamage(), gear.GetBonusHealing(), gear.GetBonusDefense(), gear.GetBonusSpeed());
                        UpdateEquippedGearPiece("chestSecond", gearPiece);
                    }
                    else if (gear.curGearType == Slot.SlotPieceType.BOOTS)
                    {
                        gearPiece.UpdateGearPiece(gear.GetSlotName(), gear.GetCurGearType().ToString(), gear.GetRarity().ToString(), gear.GetSlotImage(), gear.GetBonusHealth(), gear.GetBonusDamage(), gear.GetBonusHealing(), gear.GetBonusDefense(), gear.GetBonusSpeed());
                        UpdateEquippedGearPiece("bootsSecond", gearPiece);
                    }
                }
                if (GetSelectedBaseGearSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.THIRD)
                {
                    if (gear.curGearType == Slot.SlotPieceType.HELMET)
                    {
                        gearPiece.UpdateGearPiece(gear.GetSlotName(), gear.GetCurGearType().ToString(), gear.GetRarity().ToString(), gear.GetSlotImage(), gear.GetBonusHealth(), gear.GetBonusDamage(), gear.GetBonusHealing(), gear.GetBonusDefense(), gear.GetBonusSpeed());
                        UpdateEquippedGearPiece("helmThird", gearPiece);
                    }
                    else if (gear.curGearType == Slot.SlotPieceType.CHESTPIECE)
                    {
                        gearPiece.UpdateGearPiece(gear.GetSlotName(), gear.GetCurGearType().ToString(), gear.GetRarity().ToString(), gear.GetSlotImage(), gear.GetBonusHealth(), gear.GetBonusDamage(), gear.GetBonusHealing(), gear.GetBonusDefense(), gear.GetBonusSpeed());
                        UpdateEquippedGearPiece("chestThird", gearPiece);
                    }
                    else if (gear.curGearType == Slot.SlotPieceType.BOOTS)
                    {
                        gearPiece.UpdateGearPiece(gear.GetSlotName(), gear.GetCurGearType().ToString(), gear.GetRarity().ToString(), gear.GetSlotImage(), gear.GetBonusHealth(), gear.GetBonusDamage(), gear.GetBonusHealing(), gear.GetBonusDefense(), gear.GetBonusSpeed());
                        UpdateEquippedGearPiece("bootsThird", gearPiece);
                    }
                }



                // Display inven
                if (select)
                    EquipGear(gear);
            }
        }

        // Reference owned item, if the player currently already owns the selected piece of gear.
        //GearPiece newGearPiece = OwnedGearInven.Instance.GetGearPiece(gear);


        // If gear is NOT empty, put gear in it
        if (!gear.isEmpty && !select)
        {
            UpdateGearStatDetails();
            UpdateGearNameText(gear.GetSlotName());
        }

        // If gear IS empty, dont put gear in it, display it as empty
        else
        {
            ClearAllGearStats();
            UpdateGearNameText("");
        }
    }

    public void UnequipGear()
    {
        if (GetSelectedBaseGearSlot() == null)
            return;

        if (GetSelectedBaseGearSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.MAIN)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornGearMainAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornGearMainAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornGearMainAlly()[x].GetSlotName() == GetSelectedBaseGearSlot().GetSlotName())
                    {
                        // Remove saved equipped gear piece (data side)
                        if (OwnedLootInven.Instance.GetWornGearMainAlly()[x].GetCurGearType() == Slot.SlotPieceType.HELMET)
                        {
                            UpdateEquippedGearPiece("helmMain", null, false);
                        }
                        else if (OwnedLootInven.Instance.GetWornGearMainAlly()[x].GetCurGearType() == Slot.SlotPieceType.CHESTPIECE)
                        {
                            UpdateEquippedGearPiece("chestMain", null, false);
                        }
                        else if (OwnedLootInven.Instance.GetWornGearMainAlly()[x].GetCurGearType() == Slot.SlotPieceType.BOOTS)
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
        else if (GetSelectedBaseGearSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.SECOND)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornGearSecondAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornGearSecondAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornGearSecondAlly()[x].GetSlotName() == GetSelectedBaseGearSlot().GetSlotName())
                    {
                        // Remove saved equipped gear piece (data side)
                        if (OwnedLootInven.Instance.GetWornGearSecondAlly()[x].GetCurGearType() == Slot.SlotPieceType.HELMET)
                        {
                            UpdateEquippedGearPiece("helmSecond", null, false);
                        }
                        else if (OwnedLootInven.Instance.GetWornGearSecondAlly()[x].GetCurGearType() == Slot.SlotPieceType.CHESTPIECE)
                        {
                            UpdateEquippedGearPiece("chestSecond", null, false);
                        }
                        else if (OwnedLootInven.Instance.GetWornGearSecondAlly()[x].GetCurGearType() == Slot.SlotPieceType.BOOTS)
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
        else if (GetSelectedBaseGearSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.THIRD)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornGearThirdAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornGearThirdAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornGearThirdAlly()[x].GetSlotName() == GetSelectedBaseGearSlot().GetSlotName())
                    {
                        // Remove saved equipped gear piece (data side)
                        if (OwnedLootInven.Instance.GetWornGearThirdAlly()[x].GetCurGearType() == Slot.SlotPieceType.HELMET)
                        {
                            UpdateEquippedGearPiece("helmThird", null, false);
                        }
                        else if (OwnedLootInven.Instance.GetWornGearThirdAlly()[x].GetCurGearType() == Slot.SlotPieceType.CHESTPIECE)
                        {
                            UpdateEquippedGearPiece("chestThird", null, false);
                        }
                        else if (OwnedLootInven.Instance.GetWornGearThirdAlly()[x].GetCurGearType() == Slot.SlotPieceType.BOOTS)
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
        GetSelectedBaseGearSlot().ResetSlot(true, true);

        // Remove gear icon details (name / stats)
        ClearAllGearStats();
    }

    public void SellGear()
    {

    }
}
