using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterInventory : MonoBehaviour
{
    [SerializeField] private UIElement fighterIcon;
    [SerializeField] private Transform slotParent;
    [SerializeField] private UIElement fighterIconUI;

    public UIElement helmetSlot;
    public UIElement chestpieceSlot;
    public UIElement bootsSlot;
    public UIElement pendantSlot;
    public UIElement earringSlot;
    public UIElement beltSlot;
    public UIElement gloveSlot;
    public UIElement ring1Slot;
    public UIElement ring2Slot;

    public UIElement item1Slot;
    public UIElement item2Slot;
    public UIElement item3Slot;

    [SerializeField] private UIElement UIbackground;
    [SerializeField] private UIElement uielement;

    [SerializeField] public List<UIElement> unequippedInvenSlots = new List<UIElement>();

    public void ToggleUIElement(bool toggle = true)
    {
        if (toggle)
            uielement.UpdateAlpha(1);
        else
            uielement.UpdateAlpha(0);   
    }

    public UIElement GetUIelement()
    {
        return uielement;
    }
    public void ResetSlotSelections()
    {
        helmetSlot.ToggleSelection(false);
        chestpieceSlot.ToggleSelection(false);
        bootsSlot.ToggleSelection(false);
        pendantSlot.ToggleSelection(false);
        earringSlot.ToggleSelection(false);
        beltSlot.ToggleSelection(false);
        gloveSlot.ToggleSelection(false);
        ring1Slot.ToggleSelection(false);
        ring2Slot.ToggleSelection(false);
    }
    public void UpdateSlotsSelection(UIElement slot)
    {
        slot.ToggleSelection(true);
    }

    public void ToggleUIBackgroundColour(bool toggle = true)
    {
        if (toggle)
            UIbackground.UpdateColour(TeamGearManager.Instance.gearIconColour);
        else
            UIbackground.UpdateColour(GameManager.Instance.itemsDetailsTabColour);
    }

    public UIElement GetFighterIcon()
    {
        return fighterIcon;
    }

    public Transform GetSlotParent()
    {
        return slotParent;
    }

    public UIElement GetFighterIconUI()
    {
        return fighterIconUI;
    }

    public void UpdateFighterIcon(UnitFunctionality unit)
    {
        fighterIconUI.UpdateContentUINew(unit.GetUnitIcon());
    }

    public void ToggleFighterInventory(bool toggle = true)
    {
        if (toggle)
            gameObject.GetComponent<UIElement>().UpdateAlpha(1);
        else
            gameObject.GetComponent<UIElement>().UpdateAlpha(0);
    }

    public void ResetInvenSlotItemBG()
    {
        if (!item1Slot.linkedItemPiece)
            item1Slot.ToggleAllRaritiesBGOff();
        if (!item2Slot.linkedItemPiece)
            item2Slot.ToggleAllRaritiesBGOff();
        if (!item3Slot.linkedItemPiece)
            item3Slot.ToggleAllRaritiesBGOff();
    }

    public void UpdateIconContents(UnitFunctionality unit, bool bypassPopup = false)
    {
        UpdateFighterIcon(unit);


        if (FighterInventorManager.Instance.inventoryModeGear)
        {
            ToggleUIBackgroundColour(true);

            // Destroy previous contents
            helmetSlot.UpdateContentUINew(TeamGearManager.Instance.helmetWhiteSlotSprite);
            chestpieceSlot.UpdateContentUINew(TeamGearManager.Instance.chestWhiteSlotSprite);
            bootsSlot.UpdateContentUINew(TeamGearManager.Instance.bootsWhiteSlotSprite);
            pendantSlot.UpdateContentUINew(TeamGearManager.Instance.necklessWhiteSlotSprite);
            earringSlot.UpdateContentUINew(TeamGearManager.Instance.earringWhiteSlotSprite);
            beltSlot.UpdateContentUINew(TeamGearManager.Instance.beltWhiteSlotSprite);
            gloveSlot.UpdateContentUINew(TeamGearManager.Instance.gloveWhiteSlotSprite);
            ring1Slot.UpdateContentUINew(TeamGearManager.Instance.ringWhiteSlotSprite);
            ring2Slot.UpdateContentUINew(TeamGearManager.Instance.ringWhiteSlotSprite);

            helmetSlot.linkedUnit = unit;
            chestpieceSlot.linkedUnit = unit;
            bootsSlot.linkedUnit = unit;
            pendantSlot.linkedUnit = unit;
            earringSlot.linkedUnit = unit;
            beltSlot.linkedUnit = unit;
            gloveSlot.linkedUnit = unit;
            ring1Slot.linkedUnit = unit;
            ring2Slot.linkedUnit = unit;

            helmetSlot.isEquipped = false;
            chestpieceSlot.isEquipped = false;
            bootsSlot.isEquipped = false;
            pendantSlot.isEquipped = false;
            earringSlot.isEquipped = false;
            beltSlot.isEquipped = false;
            gloveSlot.isEquipped = false;
            ring1Slot.isEquipped = false;
            ring2Slot.isEquipped = false;

            float alpha = ShopManager.Instance.emptySlotTransparency;

            helmetSlot.UpdateAlpha(alpha, false, 0, false, false, false);
            chestpieceSlot.UpdateAlpha(alpha, false, 0, false, false, false);
            bootsSlot.UpdateAlpha(alpha, false, 0, false, false, false);
            pendantSlot.UpdateAlpha(alpha, false, 0, false, false, false);
            earringSlot.UpdateAlpha(alpha, false, 0, false, false, false);
            beltSlot.UpdateAlpha(alpha, false, 0, false, false, false);
            gloveSlot.UpdateAlpha(alpha, false, 0, false, false, false);
            ring1Slot.UpdateAlpha(alpha, false, 0, false, false, false);
            ring2Slot.UpdateAlpha(alpha, false, 0, false, false, false);

            item1Slot.UpdateAlpha(0);
            item2Slot.UpdateAlpha(0);
            item3Slot.UpdateAlpha(0);

            helmetSlot.GetComponent<Canvas>().sortingOrder = 420;
            chestpieceSlot.GetComponent<Canvas>().sortingOrder = 420;
            bootsSlot.GetComponent<Canvas>().sortingOrder = 420;
            pendantSlot.GetComponent<Canvas>().sortingOrder = 420;
            earringSlot.GetComponent<Canvas>().sortingOrder = 420;
            beltSlot.GetComponent<Canvas>().sortingOrder = 420;
            gloveSlot.GetComponent<Canvas>().sortingOrder = 420;
            ring1Slot.GetComponent<Canvas>().sortingOrder = 420;
            ring2Slot.GetComponent<Canvas>().sortingOrder = 420;

            if (unit.teamIndex == 0)
            {
                if (TeamGearManager.Instance.equippedHelmetMain)
                {
                    helmetSlot.UpdateContentUINew(TeamGearManager.Instance.equippedHelmetMain.gearIcon);
                    helmetSlot.UpdateAlpha(1);
                    helmetSlot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedHelmetMain.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedHelmetMain.gearRarity == "COMMON")
                        helmetSlot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedHelmetMain.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedHelmetMain.gearRarity == "RARE")
                        helmetSlot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedHelmetMain.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedHelmetMain.gearRarity == "EPIC")
                        helmetSlot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedHelmetMain.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedHelmetMain.gearRarity == "LEGENDARY")
                        helmetSlot.curRarity = UIElement.Rarity.legendary;

                    helmetSlot.UpdateRarityBG();
                    helmetSlot.linkedGearPiece = TeamGearManager.Instance.equippedHelmetMain;
                    helmetSlot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedHelmetMain.gearName
                        && !bypassPopup)
                            helmetSlot.AnimateUI(false);
                    }
                }
                if (TeamGearManager.Instance.equippedChestpieceMain)
                {
                    chestpieceSlot.UpdateContentUINew(TeamGearManager.Instance.equippedChestpieceMain.gearIcon);
                    chestpieceSlot.UpdateAlpha(1);
                    chestpieceSlot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedChestpieceMain.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedChestpieceMain.gearRarity == "COMMON")
                        chestpieceSlot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedChestpieceMain.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedChestpieceMain.gearRarity == "RARE")
                        chestpieceSlot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedChestpieceMain.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedChestpieceMain.gearRarity == "EPIC")
                        chestpieceSlot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedChestpieceMain.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedChestpieceMain.gearRarity == "LEGENDARY")
                        chestpieceSlot.curRarity = UIElement.Rarity.legendary;

                    chestpieceSlot.UpdateRarityBG();
                    chestpieceSlot.linkedGearPiece = TeamGearManager.Instance.equippedChestpieceMain;
                    chestpieceSlot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedChestpieceMain.gearName && !bypassPopup)
                            chestpieceSlot.AnimateUI(false);
                    }
                }
                if (TeamGearManager.Instance.equippedBootsMain)
                {
                    bootsSlot.UpdateContentUINew(TeamGearManager.Instance.equippedBootsMain.gearIcon);
                    bootsSlot.UpdateAlpha(1);
                    bootsSlot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedBootsMain.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedBootsMain.gearRarity == "COMMON")
                        bootsSlot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedBootsMain.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedBootsMain.gearRarity == "RARE")
                        bootsSlot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedBootsMain.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedBootsMain.gearRarity == "EPIC")
                        bootsSlot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedBootsMain.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedBootsMain.gearRarity == "LEGENDARY")
                        bootsSlot.curRarity = UIElement.Rarity.legendary;

                    bootsSlot.UpdateRarityBG();
                    bootsSlot.linkedGearPiece = TeamGearManager.Instance.equippedBootsMain;
                    bootsSlot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedBootsMain.gearName && !bypassPopup)
                            bootsSlot.AnimateUI(false);
                    }
                }
                if (TeamGearManager.Instance.equippedNecklessMain)
                {
                    pendantSlot.UpdateContentUINew(TeamGearManager.Instance.equippedNecklessMain.gearIcon);
                    pendantSlot.UpdateAlpha(1);
                    pendantSlot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedNecklessMain.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedNecklessMain.gearRarity == "COMMON")
                        pendantSlot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedNecklessMain.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedNecklessMain.gearRarity == "RARE")
                        pendantSlot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedNecklessMain.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedNecklessMain.gearRarity == "EPIC")
                        pendantSlot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedNecklessMain.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedNecklessMain.gearRarity == "LEGENDARY")
                        pendantSlot.curRarity = UIElement.Rarity.legendary;

                    pendantSlot.UpdateRarityBG();
                    pendantSlot.linkedGearPiece = TeamGearManager.Instance.equippedNecklessMain;
                    pendantSlot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedNecklessMain.gearName && !bypassPopup)
                            pendantSlot.AnimateUI(false);
                    }
                }
                if (TeamGearManager.Instance.equippedEarringMain)
                {
                    earringSlot.UpdateContentUINew(TeamGearManager.Instance.equippedEarringMain.gearIcon);
                    earringSlot.UpdateAlpha(1);
                    earringSlot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedEarringMain.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedEarringMain.gearRarity == "COMMON")
                        earringSlot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedEarringMain.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedEarringMain.gearRarity == "RARE")
                        earringSlot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedEarringMain.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedEarringMain.gearRarity == "EPIC")
                        earringSlot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedEarringMain.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedEarringMain.gearRarity == "LEGENDARY")
                        earringSlot.curRarity = UIElement.Rarity.legendary;

                    earringSlot.UpdateRarityBG();
                    earringSlot.linkedGearPiece = TeamGearManager.Instance.equippedEarringMain;
                    earringSlot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedEarringMain.gearName && !bypassPopup)
                            earringSlot.AnimateUI(false);
                    }
                }
                if (TeamGearManager.Instance.equippedBeltMain)
                {
                    beltSlot.UpdateContentUINew(TeamGearManager.Instance.equippedBeltMain.gearIcon);
                    beltSlot.UpdateAlpha(1);
                    beltSlot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedBeltMain.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedBeltMain.gearRarity == "COMMON")
                        beltSlot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedBeltMain.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedBeltMain.gearRarity == "RARE")
                        beltSlot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedBeltMain.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedBeltMain.gearRarity == "EPIC")
                        beltSlot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedBeltMain.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedBeltMain.gearRarity == "LEGENDARY")
                        beltSlot.curRarity = UIElement.Rarity.legendary;

                    beltSlot.UpdateRarityBG();
                    beltSlot.linkedGearPiece = TeamGearManager.Instance.equippedBeltMain;
                    beltSlot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedBeltMain.gearName && !bypassPopup)
                            beltSlot.AnimateUI(false);
                    }

                }
                if (TeamGearManager.Instance.equippedGloveMain)
                {
                    gloveSlot.UpdateContentUINew(TeamGearManager.Instance.equippedGloveMain.gearIcon);
                    gloveSlot.UpdateAlpha(1);
                    gloveSlot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedGloveMain.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedGloveMain.gearRarity == "COMMON")
                        gloveSlot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedGloveMain.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedGloveMain.gearRarity == "RARE")
                        gloveSlot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedGloveMain.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedGloveMain.gearRarity == "EPIC")
                        gloveSlot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedGloveMain.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedGloveMain.gearRarity == "LEGENDARY")
                        gloveSlot.curRarity = UIElement.Rarity.legendary;

                    gloveSlot.UpdateRarityBG();
                    gloveSlot.linkedGearPiece = TeamGearManager.Instance.equippedGloveMain;
                    gloveSlot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedGloveMain.gearName && !bypassPopup)
                            gloveSlot.AnimateUI(false);
                    }
                }
                if (TeamGearManager.Instance.equippedRing1Main)
                {
                    ring1Slot.UpdateContentUINew(TeamGearManager.Instance.equippedRing1Main.gearIcon);
                    ring1Slot.UpdateAlpha(1);
                    ring1Slot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedRing1Main.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedRing1Main.gearRarity == "COMMON")
                        ring1Slot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedRing1Main.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedRing1Main.gearRarity == "RARE")
                        ring1Slot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedRing1Main.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedRing1Main.gearRarity == "EPIC")
                        ring1Slot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedRing1Main.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedRing1Main.gearRarity == "LEGENDARY")
                        ring1Slot.curRarity = UIElement.Rarity.legendary;

                    ring1Slot.UpdateRarityBG();
                    ring1Slot.linkedGearPiece = TeamGearManager.Instance.equippedRing1Main;
                    ring1Slot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedRing1Main.gearName && !bypassPopup)
                            ring1Slot.AnimateUI(false);
                    }
                }
                if (TeamGearManager.Instance.equippedRing2Main)
                {
                    ring2Slot.UpdateContentUINew(TeamGearManager.Instance.equippedRing2Main.gearIcon);
                    ring2Slot.UpdateAlpha(1);
                    ring2Slot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedRing2Main.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedRing2Main.gearRarity == "COMMON")
                        ring2Slot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedRing2Main.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedRing2Main.gearRarity == "RARE")
                        ring2Slot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedRing2Main.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedRing2Main.gearRarity == "EPIC")
                        ring2Slot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedRing2Main.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedRing2Main.gearRarity == "LEGENDARY")
                        ring2Slot.curRarity = UIElement.Rarity.legendary;

                    ring2Slot.UpdateRarityBG();
                    ring2Slot.linkedGearPiece = TeamGearManager.Instance.equippedRing2Main;
                    ring2Slot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedRing2Main.gearName && !bypassPopup)
                            ring2Slot.AnimateUI(false);
                    }
                }
            }          
            else if (unit.teamIndex == 1)
            {
                if (TeamGearManager.Instance.equippedHelmetSec)
                {
                    helmetSlot.UpdateContentUINew(TeamGearManager.Instance.equippedHelmetSec.gearIcon);
                    helmetSlot.UpdateAlpha(1);
                    helmetSlot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedHelmetSec.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedHelmetSec.gearRarity == "COMMON")
                        helmetSlot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedHelmetSec.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedHelmetSec.gearRarity == "RARE")
                        helmetSlot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedHelmetSec.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedHelmetSec.gearRarity == "EPIC")
                        helmetSlot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedHelmetSec.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedHelmetSec.gearRarity == "LEGENDARY")
                        helmetSlot.curRarity = UIElement.Rarity.legendary;

                    helmetSlot.UpdateRarityBG();
                    helmetSlot.linkedGearPiece = TeamGearManager.Instance.equippedHelmetSec;
                    helmetSlot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedHelmetSec.gearName && !bypassPopup)
                            helmetSlot.AnimateUI(false);
                    }
                }
                if (TeamGearManager.Instance.equippedChestpieceSec)
                {
                    chestpieceSlot.UpdateContentUINew(TeamGearManager.Instance.equippedChestpieceSec.gearIcon);
                    chestpieceSlot.UpdateAlpha(1);
                    chestpieceSlot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedChestpieceSec.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedChestpieceSec.gearRarity == "COMMON")
                        chestpieceSlot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedChestpieceSec.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedChestpieceSec.gearRarity == "RARE")
                        chestpieceSlot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedChestpieceSec.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedChestpieceSec.gearRarity == "EPIC")
                        chestpieceSlot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedChestpieceSec.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedChestpieceSec.gearRarity == "LEGENDARY")
                        chestpieceSlot.curRarity = UIElement.Rarity.legendary;

                    chestpieceSlot.UpdateRarityBG();
                    chestpieceSlot.linkedGearPiece = TeamGearManager.Instance.equippedChestpieceSec;
                    chestpieceSlot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedChestpieceSec.gearName && !bypassPopup)
                            chestpieceSlot.AnimateUI(false);
                    }
                }
                if (TeamGearManager.Instance.equippedBootsSec)
                {
                    bootsSlot.UpdateContentUINew(TeamGearManager.Instance.equippedBootsSec.gearIcon);
                    bootsSlot.UpdateAlpha(1);
                    bootsSlot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedBootsSec.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedBootsSec.gearRarity == "COMMON")
                        bootsSlot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedBootsSec.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedBootsSec.gearRarity == "RARE")
                        bootsSlot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedBootsSec.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedBootsSec.gearRarity == "EPIC")
                        bootsSlot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedBootsSec.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedBootsSec.gearRarity == "LEGENDARY")
                        bootsSlot.curRarity = UIElement.Rarity.legendary;

                    bootsSlot.UpdateRarityBG();
                    bootsSlot.linkedGearPiece = TeamGearManager.Instance.equippedBootsSec;
                    bootsSlot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedBootsSec.gearName && !bypassPopup)
                            bootsSlot.AnimateUI(false);
                    }
                }
                if (TeamGearManager.Instance.equippedNecklessSec)
                {
                    pendantSlot.UpdateContentUINew(TeamGearManager.Instance.equippedNecklessSec.gearIcon);
                    pendantSlot.UpdateAlpha(1);
                    pendantSlot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedNecklessSec.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedNecklessSec.gearRarity == "COMMON")
                        pendantSlot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedNecklessSec.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedNecklessSec.gearRarity == "RARE")
                        pendantSlot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedNecklessSec.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedNecklessSec.gearRarity == "EPIC")
                        pendantSlot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedNecklessSec.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedNecklessSec.gearRarity == "LEGENDARY")
                        pendantSlot.curRarity = UIElement.Rarity.legendary;

                    pendantSlot.UpdateRarityBG();
                    pendantSlot.linkedGearPiece = TeamGearManager.Instance.equippedNecklessSec;
                    pendantSlot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedNecklessSec.gearName && !bypassPopup)
                            pendantSlot.AnimateUI(false);
                    }
                }
                if (TeamGearManager.Instance.equippedEarringSec)
                {
                    earringSlot.UpdateContentUINew(TeamGearManager.Instance.equippedEarringSec.gearIcon);
                    earringSlot.UpdateAlpha(1);
                    earringSlot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedEarringSec.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedEarringSec.gearRarity == "COMMON")
                        earringSlot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedEarringSec.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedEarringSec.gearRarity == "RARE")
                        earringSlot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedEarringSec.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedEarringSec.gearRarity == "EPIC")
                        earringSlot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedEarringSec.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedEarringSec.gearRarity == "LEGENDARY")
                        earringSlot.curRarity = UIElement.Rarity.legendary;

                    earringSlot.UpdateRarityBG();
                    earringSlot.linkedGearPiece = TeamGearManager.Instance.equippedEarringSec;
                    earringSlot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedEarringSec.gearName && !bypassPopup)
                            earringSlot.AnimateUI(false);
                    }
                }
                if (TeamGearManager.Instance.equippedBeltSec)
                {
                    beltSlot.UpdateContentUINew(TeamGearManager.Instance.equippedBeltSec.gearIcon);
                    beltSlot.UpdateAlpha(1);
                    beltSlot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedBeltSec.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedBeltSec.gearRarity == "COMMON")
                        beltSlot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedBeltSec.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedBeltSec.gearRarity == "RARE")
                        beltSlot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedBeltSec.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedBeltSec.gearRarity == "EPIC")
                        beltSlot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedBeltSec.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedBeltSec.gearRarity == "LEGENDARY")
                        beltSlot.curRarity = UIElement.Rarity.legendary;

                    beltSlot.UpdateRarityBG();
                    beltSlot.linkedGearPiece = TeamGearManager.Instance.equippedBeltSec;
                    beltSlot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedBeltSec.gearName && !bypassPopup)
                            beltSlot.AnimateUI(false);
                    }
                }
                if (TeamGearManager.Instance.equippedGloveSec)
                {
                    gloveSlot.UpdateContentUINew(TeamGearManager.Instance.equippedGloveSec.gearIcon);
                    gloveSlot.UpdateAlpha(1);
                    gloveSlot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedGloveSec.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedGloveSec.gearRarity == "COMMON")
                        gloveSlot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedGloveSec.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedGloveSec.gearRarity == "RARE")
                        gloveSlot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedGloveSec.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedGloveSec.gearRarity == "EPIC")
                        gloveSlot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedGloveSec.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedGloveSec.gearRarity == "LEGENDARY")
                        gloveSlot.curRarity = UIElement.Rarity.legendary;

                    gloveSlot.UpdateRarityBG();
                    gloveSlot.linkedGearPiece = TeamGearManager.Instance.equippedGloveSec;
                    gloveSlot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedGloveSec.gearName && !bypassPopup)
                            gloveSlot.AnimateUI(false);
                    }
                }
                if (TeamGearManager.Instance.equippedRing1Sec)
                {
                    ring1Slot.UpdateContentUINew(TeamGearManager.Instance.equippedRing1Sec.gearIcon);
                    ring1Slot.UpdateAlpha(1);
                    ring1Slot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedRing1Sec.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedRing1Sec.gearRarity == "COMMON")
                        ring1Slot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedRing1Sec.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedRing1Sec.gearRarity == "RARE")
                        ring1Slot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedRing1Sec.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedRing1Sec.gearRarity == "EPIC")
                        ring1Slot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedRing1Sec.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedRing1Sec.gearRarity == "LEGENDARY")
                        ring1Slot.curRarity = UIElement.Rarity.legendary;

                    ring1Slot.UpdateRarityBG();
                    ring1Slot.linkedGearPiece = TeamGearManager.Instance.equippedRing1Sec;
                    ring1Slot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedRing1Sec.gearName && !bypassPopup)
                            ring1Slot.AnimateUI(false);
                    }
                }
                if (TeamGearManager.Instance.equippedRing2Sec)
                {
                    ring2Slot.UpdateContentUINew(TeamGearManager.Instance.equippedRing2Sec.gearIcon);
                    ring2Slot.UpdateAlpha(1);
                    ring2Slot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedRing2Sec.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedRing2Sec.gearRarity == "COMMON")
                        ring2Slot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedRing2Sec.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedRing2Sec.gearRarity == "RARE")
                        ring2Slot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedRing2Sec.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedRing2Sec.gearRarity == "EPIC")
                        ring2Slot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedRing2Sec.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedRing2Sec.gearRarity == "LEGENDARY")
                        ring2Slot.curRarity = UIElement.Rarity.legendary;

                    ring2Slot.UpdateRarityBG();
                    ring2Slot.linkedGearPiece = TeamGearManager.Instance.equippedRing2Sec;
                    ring2Slot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedRing2Sec.gearName && !bypassPopup)
                            ring2Slot.AnimateUI(false);
                    }
                }
            }                    
            else if (unit.teamIndex == 2)
            {
                if (TeamGearManager.Instance.equippedHelmetThi)
                {
                    helmetSlot.UpdateContentUINew(TeamGearManager.Instance.equippedHelmetThi.gearIcon);
                    helmetSlot.UpdateAlpha(1);
                    helmetSlot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedHelmetThi.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedHelmetThi.gearRarity == "COMMON")
                        helmetSlot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedHelmetThi.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedHelmetThi.gearRarity == "RARE")
                        helmetSlot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedHelmetThi.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedHelmetThi.gearRarity == "EPIC")
                        helmetSlot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedHelmetThi.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedHelmetThi.gearRarity == "LEGENDARY")
                        helmetSlot.curRarity = UIElement.Rarity.legendary;

                    helmetSlot.UpdateRarityBG();
                    helmetSlot.linkedGearPiece = TeamGearManager.Instance.equippedHelmetThi;
                    helmetSlot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedHelmetThi.gearName && !bypassPopup)
                            helmetSlot.AnimateUI(false);
                    }
                }
                if (TeamGearManager.Instance.equippedChestpieceThi)
                {
                    chestpieceSlot.UpdateContentUINew(TeamGearManager.Instance.equippedChestpieceThi.gearIcon);
                    chestpieceSlot.UpdateAlpha(1);
                    chestpieceSlot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedChestpieceThi.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedChestpieceThi.gearRarity == "COMMON")
                        chestpieceSlot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedChestpieceThi.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedChestpieceThi.gearRarity == "RARE")
                        chestpieceSlot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedChestpieceThi.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedChestpieceThi.gearRarity == "EPIC")
                        chestpieceSlot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedChestpieceThi.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedChestpieceThi.gearRarity == "LEGENDARY")
                        chestpieceSlot.curRarity = UIElement.Rarity.legendary;

                    chestpieceSlot.UpdateRarityBG();
                    chestpieceSlot.linkedGearPiece = TeamGearManager.Instance.equippedChestpieceThi;
                    chestpieceSlot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedChestpieceThi.gearName && !bypassPopup)
                            chestpieceSlot.AnimateUI(false);
                    }
                }
                if (TeamGearManager.Instance.equippedBootsThi)
                {
                    bootsSlot.UpdateContentUINew(TeamGearManager.Instance.equippedBootsThi.gearIcon);
                    bootsSlot.UpdateAlpha(1);
                    bootsSlot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedBootsThi.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedBootsThi.gearRarity == "COMMON")
                        bootsSlot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedBootsThi.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedBootsThi.gearRarity == "RARE")
                        bootsSlot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedBootsThi.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedBootsThi.gearRarity == "EPIC")
                        bootsSlot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedBootsThi.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedBootsThi.gearRarity == "LEGENDARY")
                        bootsSlot.curRarity = UIElement.Rarity.legendary;

                    bootsSlot.UpdateRarityBG();
                    bootsSlot.linkedGearPiece = TeamGearManager.Instance.equippedBootsThi;
                    bootsSlot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedBootsThi.gearName && !bypassPopup)
                            bootsSlot.AnimateUI(false);
                    }
                }
                if (TeamGearManager.Instance.equippedNecklessThi)
                {
                    pendantSlot.UpdateContentUINew(TeamGearManager.Instance.equippedNecklessThi.gearIcon);
                    pendantSlot.UpdateAlpha(1);
                    pendantSlot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedNecklessThi.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedNecklessThi.gearRarity == "COMMON")
                        pendantSlot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedNecklessThi.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedNecklessThi.gearRarity == "RARE")
                        pendantSlot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedNecklessThi.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedNecklessThi.gearRarity == "EPIC")
                        pendantSlot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedNecklessThi.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedNecklessThi.gearRarity == "LEGENDARY")
                        pendantSlot.curRarity = UIElement.Rarity.legendary;

                    pendantSlot.UpdateRarityBG();
                    pendantSlot.linkedGearPiece = TeamGearManager.Instance.equippedNecklessThi;
                    pendantSlot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedNecklessThi.gearName && !bypassPopup)
                            pendantSlot.AnimateUI(false);
                    }
                }
                if (TeamGearManager.Instance.equippedEarringThi)
                {
                    earringSlot.UpdateContentUINew(TeamGearManager.Instance.equippedEarringThi.gearIcon);
                    earringSlot.UpdateAlpha(1);
                    earringSlot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedEarringThi.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedEarringThi.gearRarity == "COMMON")
                        earringSlot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedEarringThi.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedEarringThi.gearRarity == "RARE")
                        earringSlot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedEarringThi.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedEarringThi.gearRarity == "EPIC")
                        earringSlot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedEarringThi.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedEarringThi.gearRarity == "LEGENDARY")
                        earringSlot.curRarity = UIElement.Rarity.legendary;

                    earringSlot.UpdateRarityBG();
                    earringSlot.linkedGearPiece = TeamGearManager.Instance.equippedEarringThi;
                    earringSlot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedEarringThi.gearName && !bypassPopup)
                            earringSlot.AnimateUI(false);
                    }
                }
                if (TeamGearManager.Instance.equippedBeltThi)
                {
                    beltSlot.UpdateContentUINew(TeamGearManager.Instance.equippedBeltThi.gearIcon);
                    beltSlot.UpdateAlpha(1);
                    beltSlot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedBeltThi.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedBeltThi.gearRarity == "COMMON")
                        beltSlot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedBeltThi.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedBeltThi.gearRarity == "RARE")
                        beltSlot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedBeltThi.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedBeltThi.gearRarity == "EPIC")
                        beltSlot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedBeltThi.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedBeltThi.gearRarity == "LEGENDARY")
                        beltSlot.curRarity = UIElement.Rarity.legendary;

                    beltSlot.UpdateRarityBG();
                    beltSlot.linkedGearPiece = TeamGearManager.Instance.equippedBeltThi;
                    beltSlot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedBeltThi.gearName && !bypassPopup)
                            beltSlot.AnimateUI(false);
                    }
                }
                if (TeamGearManager.Instance.equippedGloveThi)
                {
                    gloveSlot.UpdateContentUINew(TeamGearManager.Instance.equippedGloveThi.gearIcon);
                    gloveSlot.UpdateAlpha(1);
                    gloveSlot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedGloveThi.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedGloveThi.gearRarity == "COMMON")
                        gloveSlot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedGloveThi.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedGloveThi.gearRarity == "RARE")
                        gloveSlot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedGloveThi.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedGloveThi.gearRarity == "EPIC")
                        gloveSlot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedGloveThi.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedGloveThi.gearRarity == "LEGENDARY")
                        gloveSlot.curRarity = UIElement.Rarity.legendary;

                    gloveSlot.UpdateRarityBG();
                    gloveSlot.linkedGearPiece = TeamGearManager.Instance.equippedGloveThi;
                    gloveSlot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedGloveThi.gearName && !bypassPopup)
                            beltSlot.AnimateUI(false);
                    }
                }
                if (TeamGearManager.Instance.equippedRing1Thi)
                {
                    ring1Slot.UpdateContentUINew(TeamGearManager.Instance.equippedRing1Thi.gearIcon);
                    ring1Slot.UpdateAlpha(1);
                    ring1Slot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedRing1Thi.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedRing1Thi.gearRarity == "COMMON")
                        ring1Slot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedRing1Thi.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedRing1Thi.gearRarity == "RARE")
                        ring1Slot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedRing1Thi.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedRing1Thi.gearRarity == "EPIC")
                        ring1Slot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedRing1Thi.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedRing1Thi.gearRarity == "LEGENDARY")
                        ring1Slot.curRarity = UIElement.Rarity.legendary;

                    ring1Slot.UpdateRarityBG();
                    ring1Slot.linkedGearPiece = TeamGearManager.Instance.equippedRing1Thi;
                    ring1Slot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedRing1Thi.gearName && !bypassPopup)
                            ring1Slot.AnimateUI(false);
                    }
                }
                if (TeamGearManager.Instance.equippedRing2Thi)
                {
                    ring2Slot.UpdateContentUINew(TeamGearManager.Instance.equippedRing2Thi.gearIcon);
                    ring2Slot.UpdateAlpha(1);
                    ring2Slot.GetComponent<Canvas>().sortingOrder = 421;
                    if (TeamGearManager.Instance.equippedRing2Thi.gearRarity == "common" ||
                        TeamGearManager.Instance.equippedRing2Thi.gearRarity == "COMMON")
                        ring2Slot.curRarity = UIElement.Rarity.common;
                    else if (TeamGearManager.Instance.equippedRing2Thi.gearRarity == "rare" ||
                        TeamGearManager.Instance.equippedRing2Thi.gearRarity == "RARE")
                        ring2Slot.curRarity = UIElement.Rarity.rare;
                    else if (TeamGearManager.Instance.equippedRing2Thi.gearRarity == "epic" ||
                        TeamGearManager.Instance.equippedRing2Thi.gearRarity == "EPIC")
                        ring2Slot.curRarity = UIElement.Rarity.epic;
                    else if (TeamGearManager.Instance.equippedRing2Thi.gearRarity == "legendary" ||
                        TeamGearManager.Instance.equippedRing2Thi.gearRarity == "LEGENDARY")
                        ring2Slot.curRarity = UIElement.Rarity.legendary;

                    ring2Slot.UpdateRarityBG();
                    ring2Slot.linkedGearPiece = TeamGearManager.Instance.equippedRing2Thi;
                    ring2Slot.isEquipped = true;
                    if (ShopManager.Instance.GetUnassignedGear())
                    {
                        if (ShopManager.Instance.GetUnassignedGear().gearName == TeamGearManager.Instance.equippedRing2Thi.gearName && !bypassPopup)
                            ring2Slot.AnimateUI(false);
                    }
                }
            }
        }
        // Item
        else
        {
            ToggleUIBackgroundColour(false);

            // Destroy previous contents
            helmetSlot.UpdateContentUINew(TeamGearManager.Instance.clearSlotSprite);
            chestpieceSlot.UpdateContentUINew(TeamGearManager.Instance.clearSlotSprite);
            bootsSlot.UpdateContentUINew(TeamGearManager.Instance.clearSlotSprite);
            pendantSlot.UpdateContentUINew(TeamGearManager.Instance.clearSlotSprite);
            earringSlot.UpdateContentUINew(TeamGearManager.Instance.clearSlotSprite);
            beltSlot.UpdateContentUINew(TeamGearManager.Instance.clearSlotSprite);
            gloveSlot.UpdateContentUINew(TeamGearManager.Instance.clearSlotSprite);
            ring1Slot.UpdateContentUINew(TeamGearManager.Instance.clearSlotSprite);
            ring2Slot.UpdateContentUINew(TeamGearManager.Instance.clearSlotSprite);

            item1Slot.UpdateContentUINew(TeamGearManager.Instance.clearSlotSprite);
            item2Slot.UpdateContentUINew(TeamGearManager.Instance.clearSlotSprite);
            item3Slot.UpdateContentUINew(TeamGearManager.Instance.clearSlotSprite);

            item1Slot.linkedUnit = unit;
            item2Slot.linkedUnit = unit;
            item3Slot.linkedUnit = unit;

            item1Slot.isEquipped = false;
            item2Slot.isEquipped = false;
            item3Slot.isEquipped = false;

            item1Slot.linkedItemPiece = null;
            item2Slot.linkedItemPiece = null;
            item3Slot.linkedItemPiece = null;


            float alpha = ShopManager.Instance.emptySlotTransparency;

            helmetSlot.UpdateAlpha(alpha, false, 0, false, false, false);
            chestpieceSlot.UpdateAlpha(alpha, false, 0, false, false, false);
            bootsSlot.UpdateAlpha(alpha, false, 0, false, false, false);
            pendantSlot.UpdateAlpha(alpha, false, 0, false, false, false);
            earringSlot.UpdateAlpha(alpha, false, 0, false, false, false);
            beltSlot.UpdateAlpha(alpha, false, 0, false, false, false);
            gloveSlot.UpdateAlpha(alpha, false, 0, false, false, false);
            ring1Slot.UpdateAlpha(alpha, false, 0, false, false, false);
            ring2Slot.UpdateAlpha(alpha, false, 0, false, false, false);

            item1Slot.UpdateAlpha(1);
            item2Slot.UpdateAlpha(1);
            item3Slot.UpdateAlpha(1);

            helmetSlot.GetComponent<Canvas>().sortingOrder = 420;
            chestpieceSlot.GetComponent<Canvas>().sortingOrder = 420;
            bootsSlot.GetComponent<Canvas>().sortingOrder = 420;
            pendantSlot.GetComponent<Canvas>().sortingOrder = 420;
            earringSlot.GetComponent<Canvas>().sortingOrder = 420;
            beltSlot.GetComponent<Canvas>().sortingOrder = 420;
            gloveSlot.GetComponent<Canvas>().sortingOrder = 420;
            ring1Slot.GetComponent<Canvas>().sortingOrder = 420;
            ring2Slot.GetComponent<Canvas>().sortingOrder = 420;

            if (unit.teamIndex == 0)
            {
                for (int i = 0; i < OwnedLootInven.Instance.GetWornItemMainAlly().Count; i++)
                {
                    if (OwnedLootInven.Instance.GetWornItemMainAlly()[i])
                    {
                        if (i == 0)
                        {
                            item1Slot.UpdateContentUINew(OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.itemSpriteItemTab);
                            item1Slot.UpdateAlpha(1);
                            item1Slot.GetComponent<Canvas>().sortingOrder = 421;
                            if (OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curRarity.ToString() == "common" || 
                                OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curRarity.ToString() == "COMMON")
                                item1Slot.curRarity = UIElement.Rarity.common;
                            else if (OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curRarity.ToString() == "rare" ||
                                OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curRarity.ToString() == "RARE")
                                item1Slot.curRarity = UIElement.Rarity.rare;
                            else if (OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curRarity.ToString() == "epic" ||
                                OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curRarity.ToString() == "EPIC")
                                item1Slot.curRarity = UIElement.Rarity.epic;
                            else if (OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curRarity.ToString() == "legendary" ||
                                OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curRarity.ToString() == "LEGENDARY")
                                item1Slot.curRarity = UIElement.Rarity.legendary;

                            item1Slot.UpdateRarityBG();
                            item1Slot.linkedItemPiece = OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece;
                            item1Slot.isEquipped = true;    
                            if (ShopManager.Instance.GetUnassignedItem())
                            {
                                if (ShopManager.Instance.GetUnassignedItem().itemName == OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.itemName && !bypassPopup)
                                    item1Slot.AnimateUI(false);
                            }
                        }
                        else if (i == 1)
                        {
                            item2Slot.UpdateContentUINew(OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.itemSpriteItemTab);
                            item2Slot.UpdateAlpha(1);
                            item2Slot.GetComponent<Canvas>().sortingOrder = 421;
                            if (OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curRarity.ToString() == "common" ||
                                OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curRarity.ToString() == "COMMON")
                                item2Slot.curRarity = UIElement.Rarity.common;
                            else if (OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curRarity.ToString() == "rare" ||
                                OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curRarity.ToString() == "RARE")
                                item2Slot.curRarity = UIElement.Rarity.rare;
                            else if (OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curRarity.ToString() == "epic" ||
                                OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curRarity.ToString() == "EPIC")
                                item2Slot.curRarity = UIElement.Rarity.epic;
                            else if (OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curRarity.ToString() == "legendary" ||
                                OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curRarity.ToString() == "LEGENDARY")
                                item2Slot.curRarity = UIElement.Rarity.legendary;

                            item2Slot.UpdateRarityBG();
                            item2Slot.linkedItemPiece = OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece;
                            item2Slot.isEquipped = true;
                            if (ShopManager.Instance.GetUnassignedItem())
                            {
                                if (ShopManager.Instance.GetUnassignedItem().itemName == OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.itemName && !bypassPopup)
                                    item2Slot.AnimateUI(false);
                            }
                        }
                        else if (i == 2)
                        {
                            item3Slot.UpdateContentUINew(OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.itemSpriteItemTab);
                            item3Slot.UpdateAlpha(1);
                            item3Slot.GetComponent<Canvas>().sortingOrder = 421;
                            if (OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curRarity.ToString() == "common" ||
                                OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curRarity.ToString() == "COMMON")
                                item3Slot.curRarity = UIElement.Rarity.common;
                            else if (OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curRarity.ToString() == "rare" ||
                                OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curRarity.ToString() == "RARE")
                                item3Slot.curRarity = UIElement.Rarity.rare;
                            else if (OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curRarity.ToString() == "epic" ||
                                OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curRarity.ToString() == "EPIC")
                                item3Slot.curRarity = UIElement.Rarity.epic;
                            else if (OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curRarity.ToString() == "legendary" ||
                                OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.curRarity.ToString() == "LEGENDARY")
                                item3Slot.curRarity = UIElement.Rarity.legendary;

                            item3Slot.UpdateRarityBG();
                            item3Slot.linkedItemPiece = OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece;
                            item3Slot.isEquipped = true;
                            if (ShopManager.Instance.GetUnassignedItem())
                            {
                                if (ShopManager.Instance.GetUnassignedItem().itemName == OwnedLootInven.Instance.GetWornItemMainAlly()[i].linkedItemPiece.itemName && !bypassPopup)
                                    item3Slot.AnimateUI(false);
                            }
                        }
                    }
                }
            }
            else if (unit.teamIndex == 1)
            {
                for (int i = 0; i < OwnedLootInven.Instance.GetWornItemSecondAlly().Count; i++)
                {
                    if (OwnedLootInven.Instance.GetWornItemSecondAlly()[i])
                    {
                        if (i == 0)
                        {
                            item1Slot.UpdateContentUINew(OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.itemSpriteItemTab);
                            item1Slot.UpdateAlpha(1);
                            item1Slot.GetComponent<Canvas>().sortingOrder = 421;
                            if (OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curRarity.ToString() == "common" ||
                                OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curRarity.ToString() == "COMMON")
                                item1Slot.curRarity = UIElement.Rarity.common;
                            else if (OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curRarity.ToString() == "rare" ||
                                OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curRarity.ToString() == "RARE")
                                item1Slot.curRarity = UIElement.Rarity.rare;
                            else if (OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curRarity.ToString() == "epic" ||
                                OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curRarity.ToString() == "EPIC")
                                item1Slot.curRarity = UIElement.Rarity.epic;
                            else if (OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curRarity.ToString() == "legendary" ||
                                OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curRarity.ToString() == "LEGENDARY")
                                item1Slot.curRarity = UIElement.Rarity.legendary;

                            item1Slot.UpdateRarityBG();
                            item1Slot.linkedItemPiece = OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece;
                            item1Slot.isEquipped = true;
                            if (ShopManager.Instance.GetUnassignedItem())
                            {
                                if (ShopManager.Instance.GetUnassignedItem().itemName == OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.itemName && !bypassPopup)
                                    item1Slot.AnimateUI(false);
                            }
                        }
                        else if (i == 1)
                        {
                            item2Slot.UpdateContentUINew(OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.itemSpriteItemTab);
                            item2Slot.UpdateAlpha(1);
                            item2Slot.GetComponent<Canvas>().sortingOrder = 421;
                            if (OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curRarity.ToString() == "common" ||
                                OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curRarity.ToString() == "COMMON")
                                item2Slot.curRarity = UIElement.Rarity.common;
                            else if (OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curRarity.ToString() == "rare" ||
                                OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curRarity.ToString() == "RARE")
                                item2Slot.curRarity = UIElement.Rarity.rare;
                            else if (OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curRarity.ToString() == "epic" ||
                                OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curRarity.ToString() == "EPIC")
                                item2Slot.curRarity = UIElement.Rarity.epic;
                            else if (OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curRarity.ToString() == "legendary" ||
                                OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curRarity.ToString() == "LEGENDARY")
                                item2Slot.curRarity = UIElement.Rarity.legendary;

                            item2Slot.UpdateRarityBG();
                            item2Slot.linkedItemPiece = OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece;
                            item2Slot.isEquipped = true;
                            if (ShopManager.Instance.GetUnassignedItem())
                            {
                                if (ShopManager.Instance.GetUnassignedItem().itemName == OwnedLootInven.Instance.GetWornItemSecondAlly()[1].linkedItemPiece.itemName && !bypassPopup)
                                    item2Slot.AnimateUI(false);
                            }
                        }
                        else if (i == 2)
                        {
                            item3Slot.UpdateContentUINew(OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.itemSpriteItemTab);
                            item3Slot.UpdateAlpha(1);
                            item3Slot.GetComponent<Canvas>().sortingOrder = 421;
                            if (OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curRarity.ToString() == "common" ||
                                OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curRarity.ToString() == "COMMON")
                                item3Slot.curRarity = UIElement.Rarity.common;
                            else if (OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curRarity.ToString() == "rare" ||
                                OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curRarity.ToString() == "RARE")
                                item3Slot.curRarity = UIElement.Rarity.rare;
                            else if (OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curRarity.ToString() == "epic" ||
                                OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curRarity.ToString() == "EPIC")
                                item3Slot.curRarity = UIElement.Rarity.epic;
                            else if (OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curRarity.ToString() == "legendary" ||
                                OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.curRarity.ToString() == "LEGENDARY")
                                item3Slot.curRarity = UIElement.Rarity.legendary;

                            item3Slot.UpdateRarityBG();
                            item3Slot.linkedItemPiece = OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece;
                            item3Slot.isEquipped = true;
                            if (ShopManager.Instance.GetUnassignedItem())
                            {
                                if (ShopManager.Instance.GetUnassignedItem().itemName == OwnedLootInven.Instance.GetWornItemSecondAlly()[i].linkedItemPiece.itemName && !bypassPopup)
                                    item3Slot.AnimateUI(false);
                            }
                        }
                    }
                }
            }
            else if (unit.teamIndex == 2)
            {
                for (int i = 0; i < OwnedLootInven.Instance.GetWornItemThirdAlly().Count; i++)
                {
                    if (OwnedLootInven.Instance.GetWornItemThirdAlly()[i])
                    {
                        if (i == 0)
                        {
                            item1Slot.UpdateContentUINew(OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.itemSpriteItemTab);
                            item1Slot.UpdateAlpha(1);
                            item1Slot.GetComponent<Canvas>().sortingOrder = 421;
                            if (OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curRarity.ToString() == "common" ||
                                OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curRarity.ToString() == "COMMON")
                                item1Slot.curRarity = UIElement.Rarity.common;
                            else if (OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curRarity.ToString() == "rare" ||
                                OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curRarity.ToString() == "RARE")
                                item1Slot.curRarity = UIElement.Rarity.rare;
                            else if (OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curRarity.ToString() == "epic" ||
                                OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curRarity.ToString() == "EPIC")
                                item1Slot.curRarity = UIElement.Rarity.epic;
                            else if (OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curRarity.ToString() == "legendary" ||
                                OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curRarity.ToString() == "LEGENDARY")
                                item1Slot.curRarity = UIElement.Rarity.legendary;

                            item1Slot.UpdateRarityBG();
                            item1Slot.linkedItemPiece = OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece;
                            item1Slot.isEquipped = true;
                            if (ShopManager.Instance.GetUnassignedItem())
                            {
                                if (ShopManager.Instance.GetUnassignedItem().itemName == OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.itemName && !bypassPopup)
                                    item1Slot.AnimateUI(false);
                            }
                        }
                        else if (i == 1)
                        {
                            item2Slot.UpdateContentUINew(OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.itemSpriteItemTab);
                            item2Slot.UpdateAlpha(1);
                            item2Slot.GetComponent<Canvas>().sortingOrder = 421;
                            if (OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curRarity.ToString() == "common" ||
                                OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curRarity.ToString() == "COMMON")
                                item2Slot.curRarity = UIElement.Rarity.common;
                            else if (OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curRarity.ToString() == "rare" ||
                                OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curRarity.ToString() == "RARE")
                                item2Slot.curRarity = UIElement.Rarity.rare;
                            else if (OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curRarity.ToString() == "epic" ||
                                OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curRarity.ToString() == "EPIC")
                                item2Slot.curRarity = UIElement.Rarity.epic;
                            else if (OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curRarity.ToString() == "legendary" ||
                                OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curRarity.ToString() == "LEGENDARY")
                                item2Slot.curRarity = UIElement.Rarity.legendary;

                            item2Slot.UpdateRarityBG();
                            item2Slot.linkedItemPiece = OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece;
                            item2Slot.isEquipped = true;
                            if (ShopManager.Instance.GetUnassignedItem())
                            {
                                if (ShopManager.Instance.GetUnassignedItem().itemName == OwnedLootInven.Instance.GetWornItemThirdAlly()[1].linkedItemPiece.itemName && !bypassPopup)
                                    item2Slot.AnimateUI(false);
                            }
                        }
                        else if (i == 2)
                        {
                            item3Slot.UpdateContentUINew(OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.itemSpriteItemTab);
                            item3Slot.UpdateAlpha(1);
                            item3Slot.GetComponent<Canvas>().sortingOrder = 421;
                            if (OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curRarity.ToString() == "common" ||
                                OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curRarity.ToString() == "COMMON")
                                item3Slot.curRarity = UIElement.Rarity.common;
                            else if (OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curRarity.ToString() == "rare" ||
                                OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curRarity.ToString() == "RARE")
                                item3Slot.curRarity = UIElement.Rarity.rare;
                            else if (OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curRarity.ToString() == "epic" ||
                                OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curRarity.ToString() == "EPIC")
                                item3Slot.curRarity = UIElement.Rarity.epic;
                            else if (OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curRarity.ToString() == "legendary" ||
                                OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.curRarity.ToString() == "LEGENDARY")
                                item3Slot.curRarity = UIElement.Rarity.legendary;

                            item3Slot.UpdateRarityBG();
                            item3Slot.linkedItemPiece = OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece;
                            item3Slot.isEquipped = true;
                            if (ShopManager.Instance.GetUnassignedItem())
                            {
                                if (ShopManager.Instance.GetUnassignedItem().itemName == OwnedLootInven.Instance.GetWornItemThirdAlly()[i].linkedItemPiece.itemName && !bypassPopup)
                                    item3Slot.AnimateUI(false);
                            }
                        }
                    }
                }
            }
        }


    }       
}
