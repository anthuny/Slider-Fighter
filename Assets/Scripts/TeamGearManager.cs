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

    public Color gearIconColour;

    public float timeStatIncColour = 1;
    public float timeStatDecColour = 1;

    public Sprite clearSlotSprite;
    public Sprite helmetSlotSprite;
    public Sprite chestSlotSprite;
    public Sprite bootsSlotSprite;
    public Sprite earringSlotSprite;
    public Sprite necklessSlotSprite;
    public Sprite beltSlotSprite;
    public Sprite gloveSlotSprite;
    public Sprite ringSlotSprite;

    public Sprite helmetWhiteSlotSprite;
    public Sprite chestWhiteSlotSprite;
    public Sprite bootsWhiteSlotSprite;
    public Sprite earringWhiteSlotSprite;
    public Sprite necklessWhiteSlotSprite;
    public Sprite beltWhiteSlotSprite;
    public Sprite gloveWhiteSlotSprite;
    public Sprite ringWhiteSlotSprite;

    public ButtonFunctionality teamSetupTabArrowLeftButton;
    public ButtonFunctionality teamSetupTabArrowRightButton;

    [SerializeField] private UIElement ally1GearTabUI;
    [SerializeField] private UIElement ally2GearTabUI;
    [SerializeField] private UIElement ally3GearTabUI;

    public MenuUnitDisplay activeFighterMenuUnitDisplay;
    public MenuUnitDisplay ally2MenuUnitDisplay;
    public MenuUnitDisplay ally3MenuUnitDisplay;

    public List<Slot> mainFighterGearSlots = new List<Slot>();
    public List<Slot> ally2GearSlots = new List<Slot>();
    public List<Slot> ally3GearSlots = new List<Slot>();

    [SerializeField] private UIElement gearStatsUI;
    [SerializeField] private GameObject gearStatGO;
    [SerializeField] private UIElement gearDescUI;
    [SerializeField] private UIElement gearNameText;
    [SerializeField] private UIElement gearRarityText;
    [SerializeField] private UIElement gearTypeText;
    public Slot selectedGearSlot;
    public Slot selectedBaseGearSlot;
    [Space(2)]
    [Header("Main Ally")]
    public GearPiece equippedHelmetMain;
    public GearPiece equippedChestpieceMain;
    public GearPiece equippedBootsMain;
    public GearPiece equippedNecklessMain;
    public GearPiece equippedEarringMain;
    public GearPiece equippedBeltMain;
    public GearPiece equippedGloveMain;
    public GearPiece equippedRing1Main;
    public GearPiece equippedRing2Main;
    [Space(2)]
    [Header("Second Ally")]
    public GearPiece equippedHelmetSec;
    public GearPiece equippedChestpieceSec;
    public GearPiece equippedBootsSec;
    public GearPiece equippedNecklessSec;
    public GearPiece equippedEarringSec;
    public GearPiece equippedBeltSec;
    public GearPiece equippedGloveSec;
    public GearPiece equippedRing1Sec;
    public GearPiece equippedRing2Sec;
    [Space(2)]
    [Header("Third Ally")]
    public GearPiece equippedHelmetThi;
    public GearPiece equippedChestpieceThi;
    public GearPiece equippedBootsThi;
    public GearPiece equippedNecklessThi;
    public GearPiece equippedEarringThi;
    public GearPiece equippedBeltThi;
    public GearPiece equippedGloveThi;
    public GearPiece equippedRing1Thi;
    public GearPiece equippedRing2Thi;
    [Space(2)]
    public bool playerInGearTab;

    public UIElement statParent;

    [SerializeField] private UIElement nextFighterArrow;

    public void ToggleNextFighterArrow(bool toggle = true)
    {
        nextFighterArrow.ToggleButton(toggle);
    }

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

    public void UpdateGearRarityText(string text)
    {
        gearRarityText.UpdateContentText(text);

        if (text == "common" || text == "COMMON")
            gearRarityText.UpdateContentTextColour(ItemRewardManager.Instance.commonColour);
        else if (text == "rare" || text == "RARE")
            gearRarityText.UpdateContentTextColour(ItemRewardManager.Instance.rareColour);
        else if (text == "epic" || text == "EPIC")
            gearRarityText.UpdateContentTextColour(ItemRewardManager.Instance.epicColour);
        else if (text == "legendary" || text == "LEGENDARY")
            gearRarityText.UpdateContentTextColour(ItemRewardManager.Instance.legendaryColour);
    }

    public void UpdateGearTypeText(string text)
    {
        if (text == "neckless")
            text = "pendant";

        gearTypeText.UpdateContentText(text);
    }

    public void ClearAllGearStats()
    {
        // Clear all gear stats
        GameObject gearStatGO = gearStatsUI.gameObject;
        for (int i = 0; i < gearStatGO.transform.childCount; i++)
        {
            //Destroy(gearStatGO.transform.GetChild(i).gameObject);
            gearStatGO.transform.GetChild(i).GetComponent<UIElement>().UpdateContentText("");
        }

        /*
        GameObject gearDescGO = gearDescUI.gameObject;
        for (int x = 0; x < gearDescGO.transform.childCount; x++)
        {
            Destroy(gearDescGO.transform.GetChild(x).gameObject);
        }
        */
    }

    UnitFunctionality GetKnownUnitFunct(bool ally1 = false, bool ally2 = false, bool ally3 = false)
    {
        if (ally1)
        {
            string unitName = GameManager.Instance.activeRoomHeroes[0].GetUnitName();

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
            string unitName2 = GameManager.Instance.activeRoomHeroes[1].GetUnitName();

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
            string unitName3 = GameManager.Instance.activeRoomHeroes[2].GetUnitName();

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

    public void UpdateUnitStatsEquip(GearPiece gear)
    {
        UnitFunctionality unitFunc = GameManager.Instance.activeRoomHeroes[0];
        // If new gear has a higher stat then current, make the stat bounce
        unitFunc.prevStatHealth = (int)unitFunc.GetUnitMaxHealth();
        unitFunc.prevStatPower = (int)unitFunc.curPower;
        unitFunc.prevStatHealingPower = (int)unitFunc.curHealingPower;
        unitFunc.prevStatDefense = (int)unitFunc.GetCurDefense();
        unitFunc.prevStatSpeed = (int)unitFunc.GetUnitSpeed();


        // Equipping a gear piece, needs to increase max hp by a set amount
        // set amount = unit starting max health
        unitFunc.UpdateUnitMaxHealth(gear.bonusHealth, false, true);
        unitFunc.UpdateUnitCurHealth(gear.bonusHealth, false, false);
        unitFunc.UpdateUnitPower(gear.bonusDamage, false);
        unitFunc.UpdateHealingPower(gear.bonusHealing, false, true);
        unitFunc.UpdateUnitDefenseChange(gear.bonusDefense, true);
        unitFunc.UpdateUnitSpeedChange(gear.bonusSpeed, true);

        activeFighterMenuUnitDisplay.UpdateUnitStats(unitFunc);
    }

    public void UpdateUnitStatsUnEquip(Slot loot, bool skipStatPopup = false)
    {
        UnitFunctionality unitFunc = GameManager.Instance.activeRoomHeroes[0];

        /*
        unitFunc.UpdateUnitCurHealth(gear.GetBonusHealth(), true, false, false);
        unitFunc.UpdateUnitMaxHealth(gear.GetBonusHealth(), false, false);
        unitFunc.UpdateUnitPower(gear.GetBonusDamage(), false, false);
        unitFunc.UpdateHealingPower(gear.GetBonusHealing(), false, false);
        unitFunc.UpdateUnitDefenseChange(gear.GetBonusDefense(), false);
        unitFunc.UpdateUnitSpeedChange(gear.GetBonusSpeed(), false);
        */

        unitFunc.UpdateUnitCurHealth(loot.linkedGearPiece.bonusHealth, true, false);
        unitFunc.UpdateUnitMaxHealth(loot.linkedGearPiece.bonusHealth, false, false);
        unitFunc.UpdateUnitPower(loot.linkedGearPiece.bonusDamage, false, false);
        unitFunc.UpdateHealingPower(loot.linkedGearPiece.bonusHealing, false, false);
        unitFunc.UpdateUnitDefenseChange(loot.linkedGearPiece.bonusDefense, false);
        unitFunc.UpdateUnitSpeedChange(loot.linkedGearPiece.bonusSpeed, false);

        activeFighterMenuUnitDisplay.UpdateUnitStats(unitFunc, skipStatPopup);
    }

    public void UpdateGearStatDetails()
    {
        //ClearAllGearStats();

        if (GetSelectedGearSlot())
        {
            if (GetSelectedGearSlot().linkedGearPiece == null)
                return;

            // Gear Stats Update
            for (int i = 0; i < statParent.transform.childCount; i++)
            {
                //GameObject spawnedStat = Instantiate(gearStatGO, gearStatsUI.transform.position, Quaternion.identity);
                //spawnedStat.transform.SetParent(gearStatsUI.transform);
                //spawnedStat.transform.localPosition = Vector2.zero;
                //spawnedStat.transform.localScale = Vector2.one;

                // Update gear stat UI
                if (i == 0)
                    statParent.transform.GetChild(0).GetComponent<UIElement>().UpdateContentText(GetSelectedGearSlot().linkedGearPiece.bonusHealth.ToString());
                else if (i == 1)
                    statParent.transform.GetChild(1).GetComponent<UIElement>().UpdateContentText(GetSelectedGearSlot().linkedGearPiece.bonusDamage.ToString());
                else if (i == 2)
                    statParent.transform.GetChild(2).GetComponent<UIElement>().UpdateContentText(GetSelectedGearSlot().linkedGearPiece.bonusHealing.ToString());
                else if (i == 3)
                    statParent.transform.GetChild(3).GetComponent<UIElement>().UpdateContentText(GetSelectedGearSlot().linkedGearPiece.bonusDefense.ToString());
                else if (i == 4)
                    statParent.transform.GetChild(4).GetComponent<UIElement>().UpdateContentText(GetSelectedGearSlot().linkedGearPiece.bonusSpeed.ToString());

                // Gear Stat Name Update

            }
        }



        // Gear Stat Description Update
    }

    // Start is called before the first frame update
    void Start()
    {
        ToggleTeamGear(false);
        ClearGearSlots();
        //ClearAllGearStats();
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
        for (int i = 0; i < mainFighterGearSlots.Count; i++)
        {
            mainFighterGearSlots[i].UpdateSlotImage(clearSlotSprite);
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
            for (int i = 0; i < mainFighterGearSlots.Count; i++)
            {
                // Place helmet
                if (i == 0)
                {
                    if (removedPieceType == "helmet")
                        mainFighterGearSlots[i].UpdateSlotImage(helmetSlotSprite);
                }
                // Place chestpiece
                if (i == 1)
                {
                    if (removedPieceType == "chestpiece")
                        mainFighterGearSlots[i].UpdateSlotImage(chestSlotSprite);
                }
                // Place boots 
                if (i == 2)
                {
                    if (removedPieceType == "boots")
                        mainFighterGearSlots[i].UpdateSlotImage(bootsSlotSprite);
                }
                if (i == 3)
                {
                    if (removedPieceType == "neckless" || removedPieceType == "pendant")
                        mainFighterGearSlots[i].UpdateSlotImage(necklessSlotSprite);
                }
                if (i == 4)
                {
                    if (removedPieceType == "earring")
                        mainFighterGearSlots[i].UpdateSlotImage(earringSlotSprite);
                }
                if (i == 5)
                {
                    if (removedPieceType == "belt")
                        mainFighterGearSlots[i].UpdateSlotImage(beltSlotSprite);
                }
                if (i == 6)
                {
                    if (removedPieceType == "glove")
                        mainFighterGearSlots[i].UpdateSlotImage(gloveSlotSprite);
                }
                if (i == 7)
                {
                    if (removedPieceType == "ring")
                    {
                        if (GetSelectedBaseGearSlot().curRingType == Slot.RingType.ring1)
                            mainFighterGearSlots[i].UpdateSlotImage(ringSlotSprite);
                    }
                }
                if (i == 8)
                {
                    if (removedPieceType == "ring")
                    {
                        if (GetSelectedBaseGearSlot().curRingType == Slot.RingType.ring2)
                            mainFighterGearSlots[i].UpdateSlotImage(ringSlotSprite);
                    }
                }

                mainFighterGearSlots[i].UpdateGearStatis(Slot.SlotStatis.DEFAULT);
                mainFighterGearSlots[i].UpdateGearOwnedBy(Slot.SlotOwnedBy.MAIN);
            }
        }

       
    }

    public void ToggleMainSlotRarityBorder()
    {
        if (OwnedLootInven.Instance.ownedLootOpened)
        {
            for (int i = 0; i < mainFighterGearSlots.Count; i++)
            {
                mainFighterGearSlots[i].ToggleRarityBorder(false);
            }
        }
        else
        {
            for (int i = 0; i < mainFighterGearSlots.Count; i++)
            {
                mainFighterGearSlots[i].ToggleRarityBorder(true);
            }
        }
    }

    public void UpdateGearSlotsBase(bool ally1 = false, bool ally2 = false, bool ally3 = false)
    {
        // Ensure each gear slot has correct bg gear sprite

        if (ally1)
        {
            for (int i = 0; i < mainFighterGearSlots.Count; i++)
            {
                if (GameManager.Instance.activeRoomHeroes[0].teamIndex == 0)
                {
                    // Place helmet
                    if (i == 0)
                    {
                        if (equippedHelmetMain == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "helmet");
                            mainFighterGearSlots[i].UpdateSlotImage(helmetSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedHelmetMain, false, "helmet");
                        }
                    }
                    // Place chestpiece
                    if (i == 1)
                    {
                        if (equippedChestpieceMain == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "chestpiece");
                            mainFighterGearSlots[i].UpdateSlotImage(chestSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedChestpieceMain, false, "chestpiece");
                        }
                    }                   
                    // Place boots 
                    if (i == 2)
                    {
                        if (equippedBootsMain == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "boots");
                            mainFighterGearSlots[i].UpdateSlotImage(bootsSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedBootsMain, false, "boots");
                        }
                    }
                    if (i == 3)
                    {
                        if (equippedNecklessMain == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "neckless");
                            mainFighterGearSlots[i].UpdateSlotImage(necklessSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedNecklessMain, false, "neckless");
                        }
                    }
                    if (i == 4)
                    {
                        if (equippedEarringMain == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "earring");
                            mainFighterGearSlots[i].UpdateSlotImage(earringSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedEarringMain, false, "earring");
                        }
                    }
                    if (i == 5)
                    {
                        if (equippedBeltMain == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "belt");
                            mainFighterGearSlots[i].UpdateSlotImage(beltSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedBeltMain, false, "belt");
                        }
                    }
                    if (i == 6)
                    {
                        if (equippedGloveMain == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "glove");
                            mainFighterGearSlots[i].UpdateSlotImage(gloveSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedGloveMain, false, "glove");
                        }
                    }
                    if (i == 7)
                    {
                        if (equippedRing1Main == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "ring");
                            mainFighterGearSlots[i].UpdateSlotImage(ringSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedRing1Main, false, "ring");
                        }
                    }
                    if (i == 8)
                    {
                        if (equippedRing2Main == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "ring");
                            mainFighterGearSlots[i].UpdateSlotImage(ringSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedRing2Main, false, "ring");
                        }
                    }
                    mainFighterGearSlots[i].UpdateGearStatis(Slot.SlotStatis.DEFAULT);
                    mainFighterGearSlots[i].UpdateGearOwnedBy(Slot.SlotOwnedBy.MAIN);
                }
                else if (GameManager.Instance.activeRoomHeroes[0].teamIndex == 1)
                {
                    // Place helmet
                    if (i == 0)
                    {
                        if (equippedHelmetSec == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "helmet");
                            mainFighterGearSlots[i].UpdateSlotImage(helmetSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedHelmetSec, false, "helmet");
                        }
                    }
                    // Place chestpiece
                    if (i == 1)
                    {
                        if (equippedChestpieceSec == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "chestpiece");
                            mainFighterGearSlots[i].UpdateSlotImage(chestSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedChestpieceSec, false, "chestpiece");
                        }
                    }
                    // Place boots 
                    if (i == 2)
                    {
                        if (equippedBootsSec == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "boots");
                            mainFighterGearSlots[i].UpdateSlotImage(bootsSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedBootsSec, false, "boots");
                        }
                    }
                    if (i == 3)
                    {
                        if (equippedNecklessSec == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "neckless");
                            mainFighterGearSlots[i].UpdateSlotImage(necklessSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedNecklessSec, false, "neckless");
                        }
                    }
                    if (i == 4)
                    {
                        if (equippedEarringSec == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "earring");
                            mainFighterGearSlots[i].UpdateSlotImage(earringSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedEarringSec, false, "earring");
                        }
                    }
                    if (i == 5)
                    {
                        if (equippedBeltSec == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "belt");
                            mainFighterGearSlots[i].UpdateSlotImage(beltSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedBeltSec, false, "belt");
                        }
                    }
                    if (i == 6)
                    {
                        if (equippedGloveSec == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "glove");
                            mainFighterGearSlots[i].UpdateSlotImage(gloveSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedGloveSec, false, "glove");
                        }
                    }
                    if (i == 7)
                    {
                        if (equippedRing1Sec == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "ring");
                            mainFighterGearSlots[i].UpdateSlotImage(ringSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedRing1Sec, false, "ring");
                        }
                    }
                    if (i == 8)
                    {
                        if (equippedRing2Sec == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "ring");
                            mainFighterGearSlots[i].UpdateSlotImage(ringSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedRing2Sec, false, "ring");
                        }
                    }
                    mainFighterGearSlots[i].UpdateGearStatis(Slot.SlotStatis.DEFAULT);
                    mainFighterGearSlots[i].UpdateGearOwnedBy(Slot.SlotOwnedBy.SECOND);
                }
                else if (GameManager.Instance.activeRoomHeroes[0].teamIndex == 2)
                {
                    // Place helmet
                    if (i == 0)
                    {
                        if (equippedHelmetThi == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "helmet");
                            mainFighterGearSlots[i].UpdateSlotImage(helmetSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedHelmetThi, false, "helmet");
                        }
                    }
                    // Place chestpiece
                    if (i == 1)
                    {
                        if (equippedChestpieceThi == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "chestpiece");
                            mainFighterGearSlots[i].UpdateSlotImage(chestSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedChestpieceThi, false, "chestpiece");
                        }
                    }
                    // Place boots 
                    if (i == 2)
                    {
                        if (equippedBootsThi == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "boots");
                            mainFighterGearSlots[i].UpdateSlotImage(bootsSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedBootsThi, false, "boots");
                        }
                    }
                    if (i == 3)
                    {
                        if (equippedNecklessThi == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "neckless");
                            mainFighterGearSlots[i].UpdateSlotImage(necklessSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedNecklessThi, false, "neckless");
                        }
                    }
                    if (i == 4)
                    {
                        if (equippedEarringThi == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "earring");
                            mainFighterGearSlots[i].UpdateSlotImage(earringSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedEarringThi, false, "earring");
                        }
                    }
                    if (i == 5)
                    {
                        if (equippedBeltThi == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "belt");
                            mainFighterGearSlots[i].UpdateSlotImage(beltSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedBeltThi, false, "belt");
                        }
                    }
                    if (i == 6)
                    {
                        if (equippedGloveThi == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "glove");
                            mainFighterGearSlots[i].UpdateSlotImage(gloveSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedGloveThi, false, "glove");
                        }
                    }
                    if (i == 7)
                    {
                        if (equippedRing1Thi == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "ring");
                            mainFighterGearSlots[i].UpdateSlotImage(ringSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedRing1Thi, false, "ring");
                        }
                    }
                    if (i == 8)
                    {
                        if (equippedRing2Thi == null)
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(null, true, "ring");
                            mainFighterGearSlots[i].UpdateSlotImage(ringSlotSprite);
                        }
                        else
                        {
                            mainFighterGearSlots[i].UpdateSlotDetails(equippedRing2Thi, false, "ring");
                        }
                    }
                    mainFighterGearSlots[i].UpdateGearStatis(Slot.SlotStatis.DEFAULT);
                    mainFighterGearSlots[i].UpdateGearOwnedBy(Slot.SlotOwnedBy.THIRD);
                }

                mainFighterGearSlots[i].UpdateSlotDetails();
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
        for (int x = 0; x < mainFighterGearSlots.Count; x++)
        {
            mainFighterGearSlots[x].ToggleSlotSelection(false);
        }
    }

    public void ToggleAllyGearSets()
    {
        if (GameManager.Instance.activeRoomHeroes[0] == null)
            return;

        ally1GearTabUI.UpdateAlpha(1);

        if (GameManager.Instance.activeRoomHeroes[0])
        {
            // Display unit level image
            activeFighterMenuUnitDisplay.ToggleUnitLevelImage(true, GameManager.Instance.activeRoomHeroes[0].GetUnitLevel());
        }


        // Update visible character ally 
        activeFighterMenuUnitDisplay.UpdateUnitDisplay(GameManager.Instance.activeRoomHeroes[0].GetUnitName());

        // Ensure each gear slot has correct bg gear sprite
        UpdateGearSlotsBase(true);   
    }
    static int SortByIndex(UnitFunctionality p1, UnitFunctionality p2)
    {
        return p1.teamIndex.CompareTo(p2.teamIndex);
    }
    public void ToggleTeamGear(bool toggle)
    {
        if (toggle)
        {
            GameManager.Instance.activeRoomHeroes.Sort(SortByIndex);

            teamGearTabUI.UpdateAlpha(1);

            ToggleNextFighterArrow(true);

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

            string unitName = GameManager.Instance.activeTeam[0].unitName;

            for (int i = 0; i < GameManager.Instance.activeRoomAllUnitFunctionalitys.Count; i++)
            {
                if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetUnitName() == unitName)
                {
                    activeFighterMenuUnitDisplay.UpdateUnitStats(GameManager.Instance.activeRoomAllUnitFunctionalitys[i]);
                }
            }

            UpdateGearSlotsBase(true);

            UpdateGearNameText("");
            UpdateGearRarityText("");
            UpdateGearTypeText("");
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
        for (int x = 0; x < mainFighterGearSlots.Count; x++)
        {
            mainFighterGearSlots[x].ToggleSlotSelection(false);
        }
    }

    public void ToggleAllSlotsClickable(bool toggle, bool doOwnedSlots = true, bool doBaseGearSlots = true, bool doOwnedSlotsVisual = true)
    {
        //Debug.Log("toggling clickable " + toggle);

        if (doBaseGearSlots)
        {
            for (int x = 0; x < mainFighterGearSlots.Count; x++)
            {
                if (toggle)
                {
                    mainFighterGearSlots[x].GetSlotUI().ToggleButton(true);
                    mainFighterGearSlots[x].ToggleMainSlot(true);
                    mainFighterGearSlots[x].ToggleOwnedGearButton(true);
                    mainFighterGearSlots[x].ToggleEquipButton(true, true);
                }
                else
                {
                    mainFighterGearSlots[x].GetSlotUI().ToggleButton(false);
                    mainFighterGearSlots[x].ToggleMainSlot(false);
                    //ally1GearSlots[x].ToggleOwnedGearButton(false);
                    //ally1GearSlots[x].ToggleEquipButton(false);
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
                    //OwnedLootInven.Instance.ownedLootSlots[i].ToggleEquipButton(true);
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

    public void EquipGear(Slot slot, UnitFunctionality unit = null, GearPiece gear = null, ItemPiece item = null)
    {
        OwnedLootInven.Instance.ToggleOwnedGearDisplay(false);

        Slot removedGear = null;

        // Remove gear from owned gear list when equipping
        // Add gear to worn gear

        AudioManager.Instance.Play("SFX_EquipGear");

        // Update unit stats with stats from gear
        UpdateUnitStatsEquip(slot.linkedGearPiece);

        // shop 
        if (!playerInGearTab)
        {
            if (unit.teamIndex == 0)
            {
                for (int i = 0; i < OwnedLootInven.Instance.wornGearMainAlly.Count; i++)
                {
                    if (OwnedLootInven.Instance.wornGearMainAlly[i].linkedGearPiece.gearType == gear.gearType)
                    {
                        UnequipGear(false, unit, OwnedLootInven.Instance.wornGearMainAlly[i].linkedGearPiece, item, true);
                        break;
                    }
                }
            }
            else if (unit.teamIndex == 1)
            {
                for (int i = 0; i < OwnedLootInven.Instance.wornGearSecondAlly.Count; i++)
                {
                    if (OwnedLootInven.Instance.wornGearSecondAlly[i].linkedGearPiece.gearType == gear.gearType)
                    {
                        UnequipGear(false, unit, OwnedLootInven.Instance.wornGearSecondAlly[i].linkedGearPiece, item, true);
                        break;
                    }
                }
            }
            else if (unit.teamIndex == 2)
            {
                for (int i = 0; i < OwnedLootInven.Instance.wornGearThirdAlly.Count; i++)
                {
                    if (OwnedLootInven.Instance.wornGearThirdAlly[i].linkedGearPiece.gearType == gear.gearType)
                    {
                        UnequipGear(false, unit, OwnedLootInven.Instance.wornGearThirdAlly[i].linkedGearPiece, item, true);
                        break;
                    }
                }
            }
        }
        else if (playerInGearTab)
        {
            if (!unit)
                unit = GameManager.Instance.activeRoomHeroes[0];

            if (GetSelectedBaseGearSlot())
            {
                if (GetSelectedBaseGearSlot().linkedGearPiece)
                {
                    // Remove worn gear
                    UnequipGear(false, unit, GetSelectedBaseGearSlot().linkedGearPiece, item, true);

                }
            }
        }

        OwnedLootInven.Instance.RemoveOwnedGear(slot);
        //OwnedLootInven.Instance.RemoveFighterEquippedGear(unit, gear.linkedGearPiece);

        if (unit.teamIndex == 0)
        {
            OwnedLootInven.Instance.AddWornGearAllyMain(slot);
            OwnedLootInven.Instance.AddFighterEquippedGear(unit, slot.linkedGearPiece);
            UpdateEquippedGearPiece(slot.linkedGearPiece.gearType + "Main", slot.linkedGearPiece);
        }
        else if (unit.teamIndex == 1)
        {
            OwnedLootInven.Instance.AddWornGearAllySecond(slot);
            OwnedLootInven.Instance.AddFighterEquippedGear(unit, slot.linkedGearPiece);
            UpdateEquippedGearPiece(slot.linkedGearPiece.gearType + "Second", slot.linkedGearPiece);
        }
        else if (unit.teamIndex == 2)
        {
            OwnedLootInven.Instance.AddWornGearAllyThird(slot);
            OwnedLootInven.Instance.AddFighterEquippedGear(unit, slot.linkedGearPiece);
            UpdateEquippedGearPiece(slot.linkedGearPiece.gearType + "Third", slot.linkedGearPiece);
        }

        GearRewards.Instance.IncrementSpawnedGearCount();

        if (GetSelectedBaseGearSlot())
        {
            GetSelectedBaseGearSlot().UpdateSlotCode(GearRewards.Instance.spawnedGearCount);

            GetSelectedBaseGearSlot().UpdateSlotImage(slot.linkedGearPiece.gearIcon);
            GetSelectedBaseGearSlot().UpdateSlotName(slot.linkedGearPiece.gearName);
            GetSelectedBaseGearSlot().UpdateGearBonusHealth(slot.linkedGearPiece.bonusHealth);
            GetSelectedBaseGearSlot().UpdateGearBonusHealing(slot.linkedGearPiece.bonusHealing);
            GetSelectedBaseGearSlot().UpdateGearBonusDefense(slot.linkedGearPiece.bonusDefense);
            GetSelectedBaseGearSlot().UpdateGearBonusDamage(slot.linkedGearPiece.bonusDamage);
            GetSelectedBaseGearSlot().UpdateGearBonusSpeed(slot.linkedGearPiece.bonusSpeed);

            GetSelectedBaseGearSlot().linkedGearPiece = slot.linkedGearPiece;
        }
        UpdateGearStatDetails();
        ClearAllGearStats();


        UpdateGearSlotsBase(true);

        if (playerInGearTab)
            ToggleAllSlotsClickable(true, false, true);

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
            equippedNecklessMain = null;
            equippedEarringMain = null;
            equippedBeltMain = null;
            equippedGloveMain = null;
            equippedRing1Main = null;
            equippedRing2Main = null;

            //OwnedLootInven.Instance.ResetWornGearAllyMain();

            activeFighterMenuUnitDisplay.ResetUnitStats();

            for (int i = 0; i < 3; i++)
            {
                mainFighterGearSlots[i].isEmpty = true;
            }

           // ClearAllGearStats();
            UpdateGearNameText("");
            UpdateGearRarityText("");
            UpdateGearTypeText("");

            ClearEmptyGearSlots();
        }
        else if (heroIndex == 1)
        {
            
            equippedHelmetSec = null;
            equippedChestpieceSec = null;
            equippedBootsSec = null;
            equippedNecklessSec = null;
            equippedEarringSec = null;
            equippedBeltSec = null;
            equippedGloveSec = null;
            equippedRing1Sec = null;
            equippedRing2Sec = null;
            //OwnedLootInven.Instance.ResetWornGearAllySecond();

            //ally2MenuUnitDisplay.ResetUnitStats();
            /*
            for (int i = 0; i < 3; i++)
            {
                ally2GearSlots[i].isEmpty = true;
            }
            */
            //ClearAllGearStats();
            UpdateGearNameText("");
            UpdateGearRarityText("");
            UpdateGearTypeText("");

            ClearEmptyGearSlots();
        }
        else if (heroIndex == 2)
        {           
            equippedHelmetThi = null;
            equippedChestpieceThi = null;
            equippedBootsThi = null;
            equippedNecklessThi = null;
            equippedEarringThi = null;
            equippedBeltThi = null;
            equippedGloveThi = null;
            equippedRing1Thi = null;
            equippedRing2Thi = null;
            //OwnedLootInven.Instance.ResetWornGearAllyThird();

            //ally3MenuUnitDisplay.ResetUnitStats();
            /*
            for (int i = 0; i < 3; i++)
            {
                ally3GearSlots[i].isEmpty = true;
            }
            */

            //ClearAllGearStats();
            UpdateGearNameText("");
            UpdateGearRarityText("");
            UpdateGearTypeText("");

            ClearEmptyGearSlots();
        }
    }

    public void ResetGearOwned()
    {
        equippedHelmetMain = null;
        equippedChestpieceMain = null;
        equippedBootsMain = null;
        equippedNecklessMain = null;
        equippedEarringMain = null;
        equippedBeltMain = null;
        equippedGloveMain = null;
        equippedRing1Main = null;
        equippedRing2Main = null;

        equippedHelmetSec = null;
        equippedChestpieceSec = null;
        equippedBootsSec = null;
        equippedNecklessSec = null;
        equippedEarringSec = null;
        equippedBeltSec = null;
        equippedGloveSec = null;
        equippedRing1Sec = null;
        equippedRing2Sec = null;

        equippedHelmetThi = null;
        equippedChestpieceThi = null;
        equippedBootsThi = null;
        equippedNecklessThi = null;
        equippedEarringThi = null;
        equippedBeltThi = null;
        equippedGloveThi = null;
        equippedRing1Thi = null;
        equippedRing2Thi = null;

        OwnedLootInven.Instance.ResetWornGearAllyMain();
        OwnedLootInven.Instance.ResetWornGearAllySecond();
        OwnedLootInven.Instance.ResetWornGearAllyThird();

        activeFighterMenuUnitDisplay.ResetUnitStats();

        for (int i = 0; i < mainFighterGearSlots.Count; i++)
        {
            mainFighterGearSlots[i].isEmpty = true;
        }

        //ClearAllGearStats();
        UpdateGearNameText("");
        UpdateGearRarityText("");
        UpdateGearTypeText("");

        ClearEmptyGearSlots();
    }

    public void ToggleGearButtons(bool toggle = true)
    {
        for (int i = 0; i < mainFighterGearSlots.Count; i++)
        {
            mainFighterGearSlots[i].ownedSlotButton.ToggleButton(toggle);
            mainFighterGearSlots[i].ToggleEquipButton(toggle);
            /*
            ally2GearSlots[i].ownedSlotButton.ToggleButton(toggle);
            ally2GearSlots[i].ToggleEquipButton(toggle);
            ally3GearSlots[i].ownedSlotButton.ToggleButton(toggle);
            ally3GearSlots[i].ToggleEquipButton(toggle);
            */
        }
    }

    public void UpdateEquippedGearPiece(string gearPieceTypeName, GearPiece newGearPiece, bool replacing = true)
    {
        /*
        GearPiece replacedGear = null;
        if (replacing)
        {
            if (equippedHelmetMain)
                replacedGear = equippedHelmetMain;
        }

        OwnedLootInven.Instance.AddWornGearAllyMain()
        */

        if (gearPieceTypeName == "helmMain" || gearPieceTypeName == "helmetMain")
        {
            if (replacing)
            {
                equippedHelmetMain = newGearPiece;
            }
            else
                equippedHelmetMain = null;
        }
        else if (gearPieceTypeName == "chestMain" || gearPieceTypeName == "chestpieceMain")
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
        else if (gearPieceTypeName == "necklessMain")
        {
            if (replacing)
                equippedNecklessMain = newGearPiece;
            else
                equippedNecklessMain = null;
        }
        else if (gearPieceTypeName == "earringMain")
        {
            if (replacing)
                equippedEarringMain = newGearPiece;
            else
                equippedEarringMain = null;
        }
        else if (gearPieceTypeName == "beltMain")
        {
            if (replacing)
                equippedBeltMain = newGearPiece;
            else
                equippedBeltMain = null;
        }
        else if (gearPieceTypeName == "gloveMain")
        {
            if (replacing)
                equippedGloveMain = newGearPiece;
            else
                equippedGloveMain = null;
        }
        else if (gearPieceTypeName == "ring1Main" || gearPieceTypeName == "ringMain")
        {
            if (replacing)
            {
                if (GetSelectedBaseGearSlot())
                {
                    if (GetSelectedBaseGearSlot().curRingType == Slot.RingType.ring1)
                        equippedRing1Main = newGearPiece;
                    else
                        equippedRing2Main = newGearPiece;
                }
                else
                {
                    if (equippedRing1Main)
                    {
                        if (!equippedRing2Main)
                            equippedRing2Main = newGearPiece;
                        else
                        {
                            equippedRing1Main = newGearPiece;
                        }
                    }
                    else
                    {
                        equippedRing1Main = newGearPiece;
                    }

                }
            }
            else
                equippedRing1Main = null;
        }
        else if (gearPieceTypeName == "ring2Main" || gearPieceTypeName == "ringMain")
        {
            if (replacing)
            {
                if (GetSelectedBaseGearSlot())
                {
                    if (GetSelectedBaseGearSlot().curRingType == Slot.RingType.ring1)
                        equippedRing1Main = newGearPiece;
                    else
                        equippedRing2Main = newGearPiece;
                }
                else
                {
                    if (equippedRing1Main)
                    {
                        if (!equippedRing2Main)
                            equippedRing2Main = newGearPiece;
                        else
                        {
                            equippedRing2Main = newGearPiece;
                        }
                    }
                    else
                    {
                        equippedRing1Main = newGearPiece;
                    }

                }
            }
            else
                equippedRing2Main = null;
        }

        else if (gearPieceTypeName == "helmSecond" || gearPieceTypeName == "helmetSecond")
        {
            if (replacing)
                equippedHelmetSec = newGearPiece;
            else
                equippedHelmetSec = null;
        }
        else if (gearPieceTypeName == "chestSecond" || gearPieceTypeName == "chestpieceSecond")
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
        else if (gearPieceTypeName == "necklessSecond")
        {
            if (replacing)
                equippedNecklessSec = newGearPiece;
            else
                equippedNecklessSec = null;
        }
        else if (gearPieceTypeName == "earringSecond")
        {
            if (replacing)
                equippedEarringSec = newGearPiece;
            else
                equippedEarringSec = null;
        }
        else if (gearPieceTypeName == "beltSecond")
        {
            if (replacing)
                equippedBeltSec = newGearPiece;
            else
                equippedBeltSec = null;
        }
        else if (gearPieceTypeName == "gloveSecond")
        {
            if (replacing)
                equippedGloveSec = newGearPiece;
            else
                equippedGloveSec = null;
        }
        else if (gearPieceTypeName == "ring1Second" || gearPieceTypeName == "ringSecond")
        {
            if (replacing)
            {
                if (GetSelectedBaseGearSlot())
                {
                    if (GetSelectedBaseGearSlot().curRingType == Slot.RingType.ring1)
                        equippedRing1Sec = newGearPiece;
                    else
                        equippedRing2Sec = newGearPiece;
                }
                else
                {
                    if (equippedRing1Main)
                    {
                        if (!equippedRing2Sec)
                            equippedRing2Sec = newGearPiece;
                        else
                        {
                            equippedRing1Sec = newGearPiece;
                        }
                    }
                    else
                    {
                        equippedRing1Sec = newGearPiece;
                    }

                }
            }
            else
                equippedRing1Sec = null;
        }
        else if (gearPieceTypeName == "ring2Second" || gearPieceTypeName == "ringSecond")
        {
            if (replacing)
            {
                if (GetSelectedBaseGearSlot())
                {
                    if (GetSelectedBaseGearSlot().curRingType == Slot.RingType.ring1)
                        equippedRing1Sec = newGearPiece;
                    else
                        equippedRing2Sec = newGearPiece;
                }
                else
                {
                    if (equippedRing1Sec)
                    {
                        if (!equippedRing2Sec)
                            equippedRing2Sec = newGearPiece;
                        else
                        {
                            equippedRing2Sec = newGearPiece;
                        }
                    }
                    else
                    {
                        equippedRing1Sec = newGearPiece;
                    }

                }
            }
            else
                equippedRing2Sec = null;
        }

        else if (gearPieceTypeName == "helmThird" || gearPieceTypeName == "helmetThird")
        {
            if (replacing)
                equippedHelmetThi = newGearPiece;
            else
                equippedHelmetThi = null;
        }
        else if (gearPieceTypeName == "chestThird" || gearPieceTypeName == "chestpieceThird")
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
        else if (gearPieceTypeName == "necklessThird")
        {
            if (replacing)
                equippedNecklessThi = newGearPiece;
            else
                equippedNecklessThi = null;
        }
        else if (gearPieceTypeName == "earringThird")
        {
            if (replacing)
                equippedEarringThi = newGearPiece;
            else
                equippedEarringThi = null;
        }
        else if (gearPieceTypeName == "beltThird")
        {
            if (replacing)
                equippedBeltThi = newGearPiece;
            else
                equippedBeltThi = null;
        }
        else if (gearPieceTypeName == "gloveThird")
        {
            if (replacing)
                equippedGloveThi = newGearPiece;
            else
                equippedGloveThi = null;
        }
        else if (gearPieceTypeName == "ring1Third" || gearPieceTypeName == "ringThird")
        {
            if (replacing)
            {
                if (GetSelectedBaseGearSlot())
                {
                    if (GetSelectedBaseGearSlot().curRingType == Slot.RingType.ring1)
                        equippedRing1Thi = newGearPiece;
                    else
                        equippedRing2Thi = newGearPiece;
                }
                else
                {
                    if (equippedRing1Main)
                    {
                        if (!equippedRing2Thi)
                            equippedRing2Thi = newGearPiece;
                        else
                        {
                            equippedRing1Thi = newGearPiece;
                        }
                    }
                    else
                    {
                        equippedRing1Thi = newGearPiece;
                    }

                }
            }
            else
                equippedRing1Thi = null;
        }
        else if (gearPieceTypeName == "ring2Third" || gearPieceTypeName == "ringThird")
        {
            if (replacing)
            {
                if (GetSelectedBaseGearSlot())
                {
                    if (GetSelectedBaseGearSlot().curRingType == Slot.RingType.ring1)
                        equippedRing1Thi = newGearPiece;
                    else
                        equippedRing2Thi = newGearPiece;
                }
                else
                {
                    if (equippedRing1Thi)
                    {
                        if (!equippedRing2Thi)
                            equippedRing2Thi = newGearPiece;
                        else
                        {
                            equippedRing2Thi = newGearPiece;
                        }
                    }
                    else
                    {
                        equippedRing1Thi = newGearPiece;
                    }

                }
            }
            else
                equippedRing2Thi = null;
        }
    }

    public void GearSelection(Slot slot, bool select = false)
    {
        if (playerInGearTab)
        {
            if (slot.curSlotStatis == Slot.SlotStatis.OWNED)
            {
                slot.ToggleCoverUI(false);
            }
        }

        ClearAllGearStats();

        // Disable all gear selection border
        ResetAllBaseGearSelections();

        // Enable selected gear slot border
        slot.ToggleSlotSelection(true);
        //OwnedGearInven.Instance.FillOwnedGearSlots();

        // Bug todo - 2nd / 3rd ally arent having their gear saved.

        if (slot.curSlotStatis == Slot.SlotStatis.DEFAULT)
        {
            UpdateSelectedBaseGearSlot(slot);

            UpdateSelectedGearSlot(slot);

            OwnedLootInven.Instance.EnableOwnedItemsSlotSelection(GetSelectedBaseGearSlot());

            // Toggle main gear selection on
            GetSelectedBaseGearSlot().ToggleSlotSelection(true);
            GetSelectedGearSlot().ToggleSlotSelection(true);
        }
        else
        {
            UpdateSelectedGearSlot(slot);
            OwnedLootInven.Instance.EnableOwnedItemsSlotSelection(GetSelectedGearSlot());
            SkillsTabManager.Instance.UpdateSelectedOwnedSlot(slot);

            GetSelectedGearSlot().ToggleSlotSelection(true);

            if (!select)
            {
                OwnedLootInven.Instance.ResetOwnedSlotEquipButton();
                OwnedLootInven.Instance.ownedLootSlots[OwnedLootInven.Instance.ownedLootSlots.IndexOf(slot)].ToggleEquipButton(true);
            }

            if (select)
            {
                GearPiece gear = null;

                for (int i = 0; i < OwnedLootInven.Instance.ownedGear.Count; i++)
                {
                    if (OwnedLootInven.Instance.ownedGear[i].linkedGearPiece.gearName == slot.linkedGearPiece.gearName)
                    {
                        slot = OwnedLootInven.Instance.ownedGear[i];
                    }
                }
                if (slot.linkedGearPiece)
                    gear = slot.linkedGearPiece;

                if (GetSelectedBaseGearSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.MAIN && gear)
                {
                    if (slot.curGearType == Slot.SlotPieceType.helmet)
                    {
                        UpdateEquippedGearPiece("helmMain", gear);
                    }
                    else if (slot.curGearType == Slot.SlotPieceType.chestpiece)
                    {
                        UpdateEquippedGearPiece("chestMain", gear);
                    }
                    else if (slot.curGearType == Slot.SlotPieceType.boots)
                    {
                        UpdateEquippedGearPiece("bootsMain", gear);
                    }
                    else if (slot.curGearType == Slot.SlotPieceType.neckless)
                    {
                        UpdateEquippedGearPiece("necklessMain", gear);
                    }
                    else if (slot.curGearType == Slot.SlotPieceType.earring)
                    {
                        UpdateEquippedGearPiece("earringMain", gear);
                    }
                    else if (slot.curGearType == Slot.SlotPieceType.belt)
                    {
                        UpdateEquippedGearPiece("beltMain", gear);
                    }
                    else if (slot.curGearType == Slot.SlotPieceType.glove)
                    {
                        UpdateEquippedGearPiece("gloveMain", gear);
                    }
                    else if (slot.curGearType == Slot.SlotPieceType.ring)
                    {
                        UpdateEquippedGearPiece("ringMain", gear);
                    }
                    if (select)
                        EquipGear(slot, GameManager.Instance.activeRoomHeroes[0]);
                }
                if (GetSelectedBaseGearSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.SECOND && gear)
                {
                    if (slot.curGearType == Slot.SlotPieceType.helmet)
                    {
                        UpdateEquippedGearPiece("helmetSecond", gear);
                    }
                    else if (slot.curGearType == Slot.SlotPieceType.chestpiece)
                    {
                        UpdateEquippedGearPiece("chestSecond", gear);
                    }
                    else if (slot.curGearType == Slot.SlotPieceType.boots)
                    {
                        UpdateEquippedGearPiece("bootsSecond", gear);
                    }
                    else if (slot.curGearType == Slot.SlotPieceType.neckless)
                    {
                        UpdateEquippedGearPiece("necklessSecond", gear);
                    }
                    else if (slot.curGearType == Slot.SlotPieceType.earring)
                    {
                        UpdateEquippedGearPiece("earringSecond", gear);
                    }
                    else if (slot.curGearType == Slot.SlotPieceType.belt)
                    {
                        UpdateEquippedGearPiece("beltSecond", gear);
                    }
                    else if (slot.curGearType == Slot.SlotPieceType.glove)
                    {
                        UpdateEquippedGearPiece("gloveSecond", gear);
                    }
                    else if (slot.curGearType == Slot.SlotPieceType.ring)
                    {
                        UpdateEquippedGearPiece("ringSecond", gear);
                    }

                    if (select)
                        EquipGear(slot, GameManager.Instance.activeRoomHeroes[0]);
                }
                if (GetSelectedBaseGearSlot().GetSlotOwnedBy() == Slot.SlotOwnedBy.THIRD && gear)
                {
                    if (slot.curGearType == Slot.SlotPieceType.helmet)
                    {
                        UpdateEquippedGearPiece("helmThird", gear);
                    }
                    else if (slot.curGearType == Slot.SlotPieceType.chestpiece)
                    {
                        UpdateEquippedGearPiece("chestThird", gear);
                    }
                    else if (slot.curGearType == Slot.SlotPieceType.boots)
                    {
                        UpdateEquippedGearPiece("bootsThird", gear);
                    }
                    else if (slot.curGearType == Slot.SlotPieceType.neckless)
                    {
                        UpdateEquippedGearPiece("necklessThird", gear);
                    }
                    else if (slot.curGearType == Slot.SlotPieceType.earring)
                    {
                        UpdateEquippedGearPiece("earringThird", gear);
                    }
                    else if (slot.curGearType == Slot.SlotPieceType.belt)
                    {
                        UpdateEquippedGearPiece("beltThird", gear);
                    }
                    else if (slot.curGearType == Slot.SlotPieceType.glove)
                    {
                        UpdateEquippedGearPiece("gloveThird", gear);
                    }
                    else if (slot.curGearType == Slot.SlotPieceType.ring)
                    {
                        UpdateEquippedGearPiece("ringThird", gear);
                    }

                    if (select)
                        EquipGear(slot, GameManager.Instance.activeRoomHeroes[0]);
                }
            }
        }

        // Reference owned item, if the player currently already owns the selected piece of gear.
        //GearPiece newGearPiece = OwnedGearInven.Instance.GetGearPiece(gear);


        // If gear is NOT empty, put gear in it
        if (!slot.isEmpty)
        {
            UpdateGearStatDetails();
            UpdateGearNameText(GetSelectedGearSlot().linkedGearPiece.gearName);
            UpdateGearRarityText(GetSelectedGearSlot().linkedGearPiece.gearRarity);
            UpdateGearTypeText(GetSelectedGearSlot().linkedGearPiece.gearType);
        }

        // If gear IS empty, dont put gear in it, display it as empty
        else
        {
            ClearAllGearStats();
            UpdateGearNameText("");
            UpdateGearRarityText("");
            UpdateGearTypeText("");
        }
    }

    public void UnequipGear(bool gearTab = true, UnitFunctionality unit = null, GearPiece gear = null, bool skipStatPopup = false, bool removeGear = false)
    {
        if (unit.teamIndex == 0)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornGearMainAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornGearMainAlly().Count; x++)
                {
                    if (gear)
                    {
                        // if equipped gear name is the same as any worn gear
                        if (OwnedLootInven.Instance.GetWornGearMainAlly()[x].linkedGearPiece.gearName == gear.gearName)
                        {
                            // Remove saved equipped gear piece (data side)
                            if (gear.gearType == "helmet" ||
                                gear.gearType == "HELMET")
                            {
                                UpdateEquippedGearPiece("helmMain", null, false);
                            }
                            if (gear.gearType == "chestpiece" ||
                                gear.gearType == "CHESTPIECE")
                            {
                                UpdateEquippedGearPiece("chestMain", null, false);
                            }
                            if (gear.gearType == "boots" ||
                                gear.gearType == "BOOTS")
                            {
                                UpdateEquippedGearPiece("bootsMain", null, false);
                            }
                            if (gear.gearType == "neckless" ||
                                gear.gearType == "NECKLESS" ||
                                gear.gearType == "pendant" ||
                                gear.gearType == "PENDANT")
                            {
                                UpdateEquippedGearPiece("necklessMain", null, false);
                            }
                            if (gear.gearType == "earring" ||
                                gear.gearType == "EARRING")
                            {
                                UpdateEquippedGearPiece("earringMain", null, false);
                            }
                            if (gear.gearType == "belt" ||
                                gear.gearType == "BELT")
                            {
                                UpdateEquippedGearPiece("beltMain", null, false);
                            }
                            if (gear.gearType == "glove" ||
                                gear.gearType == "GLOVE")
                            {
                                UpdateEquippedGearPiece("gloveMain", null, false);
                            }
                            if (gear.gearType == "ring" ||
                                gear.gearType == "RING")
                            {
                                if (GetSelectedBaseGearSlot().curRingType == Slot.RingType.ring1)
                                    UpdateEquippedGearPiece("ring1Main", null, false);
                                else if (GetSelectedBaseGearSlot().curRingType == Slot.RingType.ring2)
                                    UpdateEquippedGearPiece("ring2Main", null, false);
                            }

                            // Update unit stats when unequiping
                            UpdateUnitStatsUnEquip(OwnedLootInven.Instance.GetWornGearMainAlly()[x], skipStatPopup);

                            // Add gear into owned gear
                            if (!removeGear)
                                OwnedLootInven.Instance.AddOwnedGear(OwnedLootInven.Instance.GetWornGearMainAlly()[x]);
                            OwnedLootInven.Instance.RemoveWornGearAllyMain(OwnedLootInven.Instance.GetWornGearMainAlly()[x]);
                            break;

                        }
                    }
                    
                }
            }
        }
        else if (unit.teamIndex == 1)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornGearSecondAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornGearSecondAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornGearSecondAlly()[x].linkedGearPiece.gearName == gear.gearName)
                    {
                        // Remove saved equipped gear piece (data side)
                        if (gear.gearType == "helmet" ||
                            gear.gearType == "HELMET")
                        {
                            UpdateEquippedGearPiece("helmSecond", null, false);
                        }
                        if (gear.gearType == "chestpiece" ||
                            gear.gearType == "CHESTPIECE")
                        {
                            UpdateEquippedGearPiece("chestSecond", null, false);
                        }
                        if (gear.gearType == "boots" ||
                            gear.gearType == "BOOTS")
                        {
                            UpdateEquippedGearPiece("bootsSecond", null, false);
                        }
                        if (gear.gearType == "neckless" ||
                            gear.gearType == "NECKLESS" ||
                            gear.gearType == "pendant" ||
                            gear.gearType == "PENDANT")
                        {
                            UpdateEquippedGearPiece("necklessSecond", null, false);
                        }
                        if (gear.gearType == "earring" ||
                            gear.gearType == "EARRING")
                        {
                            UpdateEquippedGearPiece("earringSecond", null, false);
                        }
                        if (gear.gearType == "belt" ||
                            gear.gearType == "BELT")
                        {
                            UpdateEquippedGearPiece("beltSecond", null, false);
                        }
                        if (gear.gearType == "glove" ||
                            gear.gearType == "GLOVE")
                        {
                            UpdateEquippedGearPiece("gloveSecond", null, false);
                        }
                        if (gear.gearType == "ring" ||
                            gear.gearType == "RING")
                        {
                            if (GetSelectedBaseGearSlot().curRingType == Slot.RingType.ring1)
                                UpdateEquippedGearPiece("ring1Second", null, false);
                            else if (GetSelectedBaseGearSlot().curRingType == Slot.RingType.ring2)
                                UpdateEquippedGearPiece("ring2Second", null, false);
                        }

                        // Update unit stats when unequiping
                        UpdateUnitStatsUnEquip(OwnedLootInven.Instance.GetWornGearSecondAlly()[x], skipStatPopup);

                        // Add gear into owned gear
                        if (!removeGear)
                            OwnedLootInven.Instance.AddOwnedGear(OwnedLootInven.Instance.GetWornGearSecondAlly()[x]);
                        OwnedLootInven.Instance.RemoveWornGearAllySecond(OwnedLootInven.Instance.GetWornGearSecondAlly()[x]);
                        break;

                    }
                }
            }
        }
        else if (unit.teamIndex == 2)
        {
            // If play ownst at least 1 item
            if (OwnedLootInven.Instance.GetWornGearThirdAlly().Count > 0)
            {
                // Loop through all worn gear
                for (int x = 0; x < OwnedLootInven.Instance.GetWornGearThirdAlly().Count; x++)
                {
                    // if equipped gear name is the same as any worn gear
                    if (OwnedLootInven.Instance.GetWornGearThirdAlly()[x].linkedGearPiece.gearName == gear.gearName)
                    {
                        // Remove saved equipped gear piece (data side)
                        if (gear.gearType == "helmet" ||
                            gear.gearType == "HELMET")
                        {
                            UpdateEquippedGearPiece("helmThird", null, false);
                        }
                        if (gear.gearType == "chestpiece" ||
                            gear.gearType == "CHESTPIECE")
                        {
                            UpdateEquippedGearPiece("chestThird", null, false);
                        }
                        if (gear.gearType == "boots" ||
                            gear.gearType == "BOOTS")
                        {
                            UpdateEquippedGearPiece("bootsThird", null, false);
                        }
                        if (gear.gearType == "neckless" ||
                            gear.gearType == "NECKLESS" ||
                            gear.gearType == "pendant" ||
                            gear.gearType == "PENDANT")
                        {
                            UpdateEquippedGearPiece("necklessThird", null, false);
                        }
                        if (gear.gearType == "earring" ||
                            gear.gearType == "EARRING")
                        {
                            UpdateEquippedGearPiece("earringThird", null, false);
                        }
                        if (gear.gearType == "belt" ||
                            gear.gearType == "BELT")
                        {
                            UpdateEquippedGearPiece("beltThird", null, false);
                        }
                        if (gear.gearType == "glove" ||
                            gear.gearType == "GLOVE")
                        {
                            UpdateEquippedGearPiece("gloveThird", null, false);
                        }
                        if (gear.gearType == "ring" ||
                            gear.gearType == "RING")
                        {
                            if (GetSelectedBaseGearSlot().curRingType == Slot.RingType.ring1)
                                UpdateEquippedGearPiece("ring1Third", null, false);
                            else if (GetSelectedBaseGearSlot().curRingType == Slot.RingType.ring2)
                                UpdateEquippedGearPiece("ring2Third", null, false);
                        }

                        // Update unit stats when unequiping
                        UpdateUnitStatsUnEquip(OwnedLootInven.Instance.GetWornGearThirdAlly()[x], skipStatPopup);

                        // Add gear into owned gear
                        if (!removeGear)
                            OwnedLootInven.Instance.AddOwnedGear(OwnedLootInven.Instance.GetWornGearThirdAlly()[x]);
                        OwnedLootInven.Instance.RemoveWornGearAllyThird(OwnedLootInven.Instance.GetWornGearThirdAlly()[x]);
                        break;

                    }
                }
            }
        }

        if (playerInGearTab)
        {
            GetSelectedBaseGearSlot().ResetSlot(true, true);
        }


        // Remove gear icon details (name / stats)
        //ClearAllGearStats();
        AudioManager.Instance.Play("SFX_UnequipGear");
    }

    public void SellGear()
    {

    }
}
