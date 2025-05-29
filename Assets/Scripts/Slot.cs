using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public enum SlotPieceType { helmet, chestpiece, boots, neckless, earring, belt, glove, ring, SKILL, ITEM, EMPTY }
    public SlotPieceType curGearType;

    public enum RingType { ring1, ring2 }
    public RingType curRingType;

    public enum Rarity { COMMON, RARE, EPIC, LEGENDARY }
    public Rarity curRarity;

    public enum SlotStatis { OWNED, UNOWNED, REWARD, DEFAULT }
    public SlotStatis curSlotStatis;

    public enum SlotRace { HUMAN, BEAST, ETHEREAL, ALL }
    public SlotRace curSlotRace;

    public enum SlotOwnedBy { MAIN, SECOND, THIRD, NOONE}
    public SlotOwnedBy curGearOwnedBy;

    public enum SlotPosition { FIRST, SECOND, THIRD, NOONE }
    public SlotPosition curSlotPosition;

    public enum SlotType { NOT_REWARD, REWARD}
    public SlotType curSlotType;

    public enum SlotType2 { NOTBASE, BASE }
    public SlotType2 curSlotType2;


    private string slotName;
    private int gearBonusHealth;
    private int gearBonusDamage;
    private int gearBonusHealing;
    private int gearBonusDefense;
    private int gearBonusSpeed;

    public SkillData skill;

    [SerializeField] private UIElement slotUI;
    [SerializeField] private UIElement slotSelectionUI;

    public UIElement buttonSkillUpgrade1;
    public UIElement buttonSkillUpgrade2;
    public UIElement buttonSkillUpgrade3;

    public UIElement progressSlider;

    public UIElement ownedSlotButton;
    [SerializeField] private UIElement buttonOwnedLoot;
    [SerializeField] private UIElement bagSprite;
    [SerializeField] private UIElement alertBagSprite;
    [SerializeField] private UIElement coverUI;
    [SerializeField] private UIElement raceIcon;
    [SerializeField] private UIElement rarityBG;
    [SerializeField] private UIElement remainingUsesUI;
    [SerializeField] private UIElement rarityBorder;
    [SerializeField] private int remainingUses;
    [SerializeField] private int itemUses;
    public int slotIndex = -1;
    public bool coverOn;
    public UIElement activeStatusUI;

    public bool isGold;
    public bool isEmpty = true;
    public int code = 0;
    public int pointsAdded = 0;
    //public bool isLocked;
    public GearPiece linkedGearPiece;
    public ItemPiece linkedItemPiece;
    public bool baseSlot = false;
    public bool isSelected;
    public bool isMainSlot;
    public Slot linkedSlot;
    [SerializeField] private UIElement mainIconBG;

    public enum ItemRarity { common, rare, epic, legendary }
    public ItemRarity curItemRarity;

    public bool maxSet = false;
    public bool remove = false;

    [SerializeField] private UIElement itemRarityText;

    [SerializeField] private List<UIElement> skillUpgrades = new List<UIElement>();
    public bool isDiscovered = false;

    public void UpdateSlotBGRarity()
    {
        float onAlphaCommon = .6f;
        float onAlphaRare = .65f;
        float onAlphaEpic = .7f;
        float onAlphaLegendary = .75f;

        if (linkedGearPiece)
        {
            if (!isEmpty)
            {
                if (linkedGearPiece.gearRarity == "common" || linkedGearPiece.gearRarity == "COMMON")
                {
                    if (GetComponent<UIElement>().commonRarityBG)
                    {
                        ResetAllBGRarity();
                        GetComponent<UIElement>().commonRarityBG.GetComponent<UIElement>().UpdateAlpha(onAlphaCommon);
                    }
                }
                else if (linkedGearPiece.gearRarity == "rare" || linkedGearPiece.gearRarity == "RARE")
                {
                    if (GetComponent<UIElement>().rareRarityBG)
                    {
                        ResetAllBGRarity();
                        GetComponent<UIElement>().rareRarityBG.GetComponent<UIElement>().UpdateAlpha(onAlphaRare);
                    }
                }
                else if (linkedGearPiece.gearRarity == "epic" || linkedGearPiece.gearRarity == "EPIC")
                {
                    if (GetComponent<UIElement>().epicRarityBG)
                    {
                        ResetAllBGRarity();
                        GetComponent<UIElement>().epicRarityBG.GetComponent<UIElement>().UpdateAlpha(onAlphaEpic);
                    }
                }
                else if (linkedGearPiece.gearRarity == "legendary" || linkedGearPiece.gearRarity == "LEGENDARY")
                {
                    if (GetComponent<UIElement>().legendaryRarityBG)
                    {
                        ResetAllBGRarity();
                        GetComponent<UIElement>().legendaryRarityBG.GetComponent<UIElement>().UpdateAlpha(onAlphaLegendary);
                    }
                }
            }
            else
            {
                ResetAllBGRarity();
            }
        }
        else if (linkedItemPiece)
        {
            if (!isEmpty)
            {
                if (linkedItemPiece.curRarity == ItemPiece.Rarity.common)
                {
                    if (GetComponent<UIElement>().commonRarityBG)
                        GetComponent<UIElement>().commonRarityBG.GetComponent<UIElement>().UpdateAlpha(onAlphaCommon);
                }
                else if (linkedItemPiece.curRarity == ItemPiece.Rarity.rare)
                {
                    if (GetComponent<UIElement>().rareRarityBG)
                        GetComponent<UIElement>().rareRarityBG.GetComponent<UIElement>().UpdateAlpha(onAlphaRare);
                }
                else if (linkedItemPiece.curRarity == ItemPiece.Rarity.epic)
                {
                    if (GetComponent<UIElement>().epicRarityBG)
                        GetComponent<UIElement>().epicRarityBG.GetComponent<UIElement>().UpdateAlpha(onAlphaEpic);
                }
                else if (linkedItemPiece.curRarity == ItemPiece.Rarity.legendary)
                {
                    if (GetComponent<UIElement>().legendaryRarityBG)
                        GetComponent<UIElement>().legendaryRarityBG.GetComponent<UIElement>().UpdateAlpha(onAlphaLegendary);
                }
            }
            else
            {
                ResetAllBGRarity();
            }
        }
        else
        {
            ResetAllBGRarity();
        }
    }

    void ResetAllBGRarity()
    {
        if (GetComponent<UIElement>().commonRarityBG)
        {
            GetComponent<UIElement>().commonRarityBG.GetComponent<UIElement>().UpdateAlpha(0);
            GetComponent<UIElement>().rareRarityBG.GetComponent<UIElement>().UpdateAlpha(0);
            GetComponent<UIElement>().epicRarityBG.GetComponent<UIElement>().UpdateAlpha(0);
            GetComponent<UIElement>().legendaryRarityBG.GetComponent<UIElement>().UpdateAlpha(0);
        }
    }

    public void ToggleSkillUpgradesClickable(bool toggle = true)
    {
        if (skillUpgrades.Count > 0)
        {
            for (int i = 0; i < skillUpgrades.Count; i++)
            {
                skillUpgrades[i].ToggleButton(toggle);
                skillUpgrades[i].ToggleButton2(toggle);
            }
        }
    }

    public void ToggleItemRarityText(bool toggle = true)
    {
        if (toggle)
        {
            itemRarityText.UpdateAlpha(1);
        }
        else
        {
            itemRarityText.UpdateAlpha(0);
        }

        if (toggle)
        {
            if (linkedItemPiece)
            {
                if (linkedItemPiece.curRarity == ItemPiece.Rarity.common)
                {
                    itemRarityText.UpdateContentText("COMMON");
                    itemRarityText.UpdateContentTextColour(ItemRewardManager.Instance.commonColour);
                }
                else if (linkedItemPiece.curRarity == ItemPiece.Rarity.rare)
                {
                    itemRarityText.UpdateContentText("RARE");
                    itemRarityText.UpdateContentTextColour(ItemRewardManager.Instance.rareColour);
                }
                else if (linkedItemPiece.curRarity == ItemPiece.Rarity.epic)
                {
                    itemRarityText.UpdateContentText("EPIC");
                    itemRarityText.UpdateContentTextColour(ItemRewardManager.Instance.epicColour);
                }
                else if (linkedItemPiece.curRarity == ItemPiece.Rarity.legendary)
                {
                    itemRarityText.UpdateContentText("LEGENDARY");
                    itemRarityText.UpdateContentTextColour(ItemRewardManager.Instance.legendaryColour);
                }
            }
            else if (linkedGearPiece)
            {
                if (linkedGearPiece.gearRarity == "common" ||
                    linkedGearPiece.gearRarity == "COMMON")
                {
                    itemRarityText.UpdateContentText("COMMON");
                    itemRarityText.UpdateContentTextColour(ItemRewardManager.Instance.commonColour);
                }
                if (linkedGearPiece.gearRarity == "rare" ||
                    linkedGearPiece.gearRarity == "RARE")
                {
                    itemRarityText.UpdateContentText("RARE");
                    itemRarityText.UpdateContentTextColour(ItemRewardManager.Instance.rareColour);
                }
                if (linkedGearPiece.gearRarity == "epic" ||
                    linkedGearPiece.gearRarity == "EPIC")
                {
                    itemRarityText.UpdateContentText("EPIC");
                    itemRarityText.UpdateContentTextColour(ItemRewardManager.Instance.epicColour);
                }
                if (linkedGearPiece.gearRarity == "legendary" ||
                    linkedGearPiece.gearRarity == "LEGENDARY")
                {
                    itemRarityText.UpdateContentText("LEGENDARY");
                    itemRarityText.UpdateContentTextColour(ItemRewardManager.Instance.legendaryColour);
                }
            }
        }
    }

    public void ToggleEquipMainButton(bool toggle = true)
    {
        if (buttonOwnedLoot)
            buttonOwnedLoot.GetComponent<GraphicRaycaster>().enabled = toggle;

    }
    public void UpdateRemoved(bool toggle = true)
    {
        remove = toggle;
    }

    public bool GetRemoved()
    {
        return remove;
    }
    public void UpdateMainIconBGColour(Color color)
    {
        if (mainIconBG != null)
            mainIconBG.UpdateColour(color);
    }

    private void Start()
    {
        ToggleSlotSelection(false);
    }

    public void ToggleRarityBorder(bool toggle = true)
    {
        if (toggle)
            rarityBorder.UpdateAlpha(1);
        else
            rarityBorder.UpdateAlpha(0);
    }
    public void UpdateRarityBorderColour()
    {
        if (!isEmpty)
        {
            if (linkedItemPiece)
            {
                ToggleRarityBorder(true);

                if (linkedItemPiece.curRarity == ItemPiece.Rarity.common)
                    rarityBorder.UpdateRarityBorderColour(ItemRewardManager.Instance.commonColour);
                else if (linkedItemPiece.curRarity == ItemPiece.Rarity.rare)
                    rarityBorder.UpdateRarityBorderColour(ItemRewardManager.Instance.rareColour);
                else if (linkedItemPiece.curRarity == ItemPiece.Rarity.epic)
                    rarityBorder.UpdateRarityBorderColour(ItemRewardManager.Instance.epicColour);
                else if (linkedItemPiece.curRarity == ItemPiece.Rarity.legendary)
                    rarityBorder.UpdateRarityBorderColour(ItemRewardManager.Instance.legendaryColour);
            }
            else if (linkedGearPiece)
            {
                ToggleRarityBorder(true);

                if (linkedGearPiece.gearRarity == "common" || linkedGearPiece.gearRarity == "COMMON")
                    rarityBorder.UpdateRarityBorderColour(ItemRewardManager.Instance.commonColour);
                else if (linkedGearPiece.gearRarity == "rare" || linkedGearPiece.gearRarity == "RARE")
                    rarityBorder.UpdateRarityBorderColour(ItemRewardManager.Instance.rareColour);
                else if (linkedGearPiece.gearRarity == "epic" || linkedGearPiece.gearRarity == "EPIC")
                    rarityBorder.UpdateRarityBorderColour(ItemRewardManager.Instance.epicColour);
                else if (linkedGearPiece.gearRarity == "legendary" || linkedGearPiece.gearRarity == "LEGENDARY")
                    rarityBorder.UpdateRarityBorderColour(ItemRewardManager.Instance.legendaryColour);
            }
        }
        else
        {
            rarityBorder.UpdateRarityBorderColour(GameManager.Instance.invisibleColour);
        }
    }

    public void UpdateRemainingUses(int newRemainingUses)
    {
        remainingUses = newRemainingUses;
    }

    public int GetRemainingUses()
    {
        return remainingUses;
    }

    void AddItemUses(int newUses)
    {
        itemUses += newUses;
    }

    public void UpdateItemUses(int newUses)
    {
        itemUses = newUses;
    }

    public int GetItemUses()
    {
        return itemUses;
    }

    public void ReduceItemUses()
    {
        if (itemUses > 0)
            itemUses--;
    }

    public int GetCalculatedItemUsesRemaining()
    {
        return linkedSlot.linkedItemPiece.maxUsesPerCombat - linkedSlot.itemUses;
    }

    public int GetCalculatedItemsUsesRemaining2()
    {
        return linkedItemPiece.maxUsesPerCombat - itemUses;
    }

   
    public void UpdateRaceIcon(Sprite newIcon, bool toggle = true)
    {
        raceIcon.UpdateContentImage(newIcon);

        raceIcon.ToggleRaceIconEffectBG(toggle);
    }

    public UIElement GetRaceIcon()
    {
        return raceIcon;
    }

    public void UpdateRarityBG(ItemRarity itemRarity, bool clear = false)
    {
        if (clear)
        {
            rarityBG.UpdateColour(GameManager.Instance.invisibleColour);
            return;
        }

        if (itemRarity == ItemRarity.common)
        {
            rarityBG.UpdateColour(ItemRewardManager.Instance.commonColour);
        }
        else if (itemRarity == ItemRarity.rare)
        {
            rarityBG.UpdateColour(ItemRewardManager.Instance.rareColour);
        }
        else if (itemRarity == ItemRarity.epic)
        {
            rarityBG.UpdateColour(ItemRewardManager.Instance.epicColour);
        }
        else if (itemRarity == ItemRarity.legendary)
        {
            rarityBG.UpdateColour(ItemRewardManager.Instance.legendaryColour);
        }
    }


    public void IncUseCount()
    {
        AddItemUses(1);
    }

    public void UpdateLinkedSlot(Slot slot)
    {
        linkedSlot = slot;
    }

    /// <summary>
    /// Updates SLOT details: UPDATE RARITY BG COLOUR, UPDATE RACE ICON, UPDATE ACTIVE / PASSIVE STATUS, UPDATE REMAINING USES, TOGGLE PLUS BUTTON
    /// </summary>
    /// <param name="activeType"></param>
    /// <param name="togglePlusButton"></param>
    /// <param name="remainingUses"></param>
    /// <param name="curSlotRace"></param>
    /// <param name="curRarity"></param>
    /// <param name="hideRarityBG"></param>
    public void UpdateSlotDetails(bool flag = false)
    {
        if (curSlotStatis == SlotStatis.OWNED)
            ToggleEquipButton(false, false);

        Slot.ItemRarity curSlotRarity = Slot.ItemRarity.common;
        Slot.SlotRace curSlotRace = Slot.SlotRace.ALL;
        string activeStatus = "";
        int itemUsesRemaining = 0;

        if (TeamItemsManager.Instance.playerInItemTab)
            UpdateMainIconBGColour(OwnedLootInven.Instance.GetOtherSlotBGColour());
        else if (TeamGearManager.Instance.playerInGearTab)
            UpdateMainIconBGColour(OwnedLootInven.Instance.GetOtherSlotBGColour());
        else if (SkillsTabManager.Instance.playerInSkillTab)
            UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());

        if (linkedGearPiece)
        {
            if (linkedGearPiece.gearRarity == "common")
                curRarity = Rarity.COMMON;
            else if (linkedGearPiece.gearRarity == "rare")
                curRarity = Rarity.RARE;
            else if (linkedGearPiece.gearRarity == "epic")
                curRarity = Rarity.EPIC;
            else if (linkedGearPiece.gearRarity == "legendary")
                curRarity = Rarity.LEGENDARY;

            if (linkedGearPiece.gearRarity == "common" || linkedGearPiece.gearRarity == "COMMON")
                curRarity = Rarity.COMMON;
            else if (linkedGearPiece.gearRarity == "rare" || linkedGearPiece.gearRarity == "RARE")
                curRarity = Rarity.RARE;
            else if (linkedGearPiece.gearRarity == "epic" || linkedGearPiece.gearRarity == "EPIC")
                curRarity = Rarity.EPIC;
            else if (linkedGearPiece.gearRarity == "legendary" || linkedGearPiece.gearRarity == "LEGENDARY")
                curRarity = Rarity.LEGENDARY;
        }
        else if (linkedItemPiece)
        {
            if (linkedItemPiece.curRarity == ItemPiece.Rarity.common)
                curRarity = Rarity.COMMON;
            else if (linkedItemPiece.curRarity == ItemPiece.Rarity.rare)
                curRarity = Rarity.RARE;
            else if (linkedItemPiece.curRarity == ItemPiece.Rarity.epic)
                curRarity = Rarity.EPIC;
            else if (linkedItemPiece.curRarity == ItemPiece.Rarity.legendary)
                curRarity = Rarity.LEGENDARY;
        }

        if (curGearType == SlotPieceType.SKILL)
        {
            if (!flag)
                ToggleEquipButton(false, false);
            UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
        }
        if (curGearType == SlotPieceType.ITEM)
        {
            if (!flag)
                ToggleEquipButton(true, true);

            if (linkedItemPiece == null && linkedGearPiece == null)
            {
                if (curSlotPosition == SlotPosition.NOONE || isMainSlot)
                    isEmpty = true;

                if (curSlotType == SlotType.REWARD)
                    ToggleItemRarityText(true);
                else
                    ToggleItemRarityText(false);

                UpdateRarityBG(curSlotRarity, true);
                UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);

                buttonOwnedLoot.ToggleButton(true);
                buttonOwnedLoot.UpdateAlpha(1);

                //ownedSlotButton.UpdateAlpha(0);
                if (!flag)
                    ToggleEquipButton(true);

                // Update Active / Passive status
                activeStatusUI.UpdateContentText(activeStatus);
                if (activeStatus == "A")
                    activeStatusUI.UpdateContentTextColour(GameManager.Instance.activeSkillColour);
                else
                    activeStatusUI.UpdateContentTextColour(GameManager.Instance.passiveSkillColour);

                if (itemUsesRemaining == 0)
                    remainingUsesUI.UpdateContentText("");
                else
                    remainingUsesUI.UpdateContentText(itemUsesRemaining.ToString());

                UpdateSlotName("");

                if (linkedItemPiece)
                {
                    UpdateSlotImage(linkedItemPiece.itemSpriteItemTab);
                    UpdateSlotName(linkedItemPiece.itemName);
                }
                else
                {
                    UpdateSlotImage(TeamGearManager.Instance.clearSlotSprite);
                    UpdateSlotName("");
                }
                UpdateRarityBorderColour();
                UpdateSlotBGRarity();

            }
            else if (linkedItemPiece)
            {
                if (curSlotPosition == SlotPosition.NOONE || isMainSlot)
                    isEmpty = false;

                if (curSlotType == SlotType.REWARD)
                    ToggleItemRarityText(true);
                else
                    ToggleItemRarityText(false);

                if (TeamItemsManager.Instance.playerInItemTab)
                    UpdateMainIconBGColour(OwnedLootInven.Instance.GetOtherSlotBGColour());
                else if (TeamGearManager.Instance.playerInGearTab)
                    UpdateMainIconBGColour(OwnedLootInven.Instance.GetOtherSlotBGColour());
                else if (SkillsTabManager.Instance.playerInSkillTab)
                    UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());

                if (linkedItemPiece.curActiveType == ItemPiece.ActiveType.ACTIVE)
                    activeStatus = "A";
                else
                    activeStatus = "P";

                if (activeStatus == "P")
                    itemUsesRemaining = linkedItemPiece.maxUsesPerCombat;
                else
                {
                    if (linkedSlot)
                        itemUsesRemaining = linkedItemPiece.maxUsesPerCombat - linkedSlot.GetItemUses();
                    else
                        itemUsesRemaining = linkedItemPiece.maxUsesPerCombat;
                }

                UpdateRemainingUses(itemUsesRemaining);

                if (linkedItemPiece.curRace == ItemPiece.RaceSpecific.HUMAN)
                    curSlotRace = Slot.SlotRace.HUMAN;
                else if (linkedItemPiece.curRace == ItemPiece.RaceSpecific.BEAST)
                    curSlotRace = Slot.SlotRace.BEAST;
                else if (linkedItemPiece.curRace == ItemPiece.RaceSpecific.ETHEREAL)
                    curSlotRace = Slot.SlotRace.ETHEREAL;
                else if (linkedItemPiece.curRace == ItemPiece.RaceSpecific.ALL)
                    curSlotRace = Slot.SlotRace.ALL;

                if (linkedItemPiece.curRarity == ItemPiece.Rarity.common)
                    curSlotRarity = Slot.ItemRarity.common;
                else if (linkedItemPiece.curRarity == ItemPiece.Rarity.rare)
                    curSlotRarity = Slot.ItemRarity.rare;
                else if (linkedItemPiece.curRarity == ItemPiece.Rarity.epic)
                    curSlotRarity = Slot.ItemRarity.epic;

                // Update BG rarity of SlOT
                UpdateRarityBG(curSlotRarity);
                UpdateSlotBGRarity();
                // Update Race icon of SLOT
                if (curSlotRace == SlotRace.HUMAN)
                {
                    UpdateRaceIcon(GameManager.Instance.humanRaceIcon);
                }
                else if (curSlotRace == SlotRace.BEAST)
                {
                    UpdateRaceIcon(GameManager.Instance.beastRaceIcon);
                }
                else if (curSlotRace == SlotRace.ETHEREAL)
                {
                    UpdateRaceIcon(GameManager.Instance.etherealRaceIcon);
                }
                else if (curSlotRace == SlotRace.ALL)
                {
                    UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite, false);
                }

                // Update Active / Passive status
                activeStatusUI.UpdateContentText(activeStatus);
                if (activeStatus == "A")
                    activeStatusUI.UpdateContentTextColour(GameManager.Instance.activeSkillColour);
                else
                    activeStatusUI.UpdateContentTextColour(GameManager.Instance.passiveSkillColour);

                if (itemUsesRemaining == 0)
                    remainingUsesUI.UpdateContentText("");
                else
                    remainingUsesUI.UpdateContentText(itemUsesRemaining.ToString());

                //if (buttonOwnedLoot)
                //{
                    //buttonOwnedLoot.ToggleButton(false);
                    //buttonOwnedLoot.UpdateAlpha(0);
                //}

                if (ownedSlotButton)
                    ownedSlotButton.UpdateAlpha(1);

                if (linkedItemPiece)
                {
                    UpdateSlotImage(linkedItemPiece.itemSpriteItemTab);
                    UpdateSlotName(linkedItemPiece.itemName);
                }

                UpdateRarityBorderColour();
            }
        }
        else if (curGearType != SlotPieceType.SKILL)
        {
            if (curSlotType == SlotType.REWARD)
                ToggleItemRarityText(true);
            else
                ToggleItemRarityText(false);

            //if (!flag)
                //ToggleEquipButton(true, true);

            if (TeamGearManager.Instance.playerInGearTab)
                UpdateMainIconBGColour(OwnedLootInven.Instance.GetOtherSlotBGColour());

            if (linkedGearPiece)
            {
                isEmpty = false;

                UpdateSlotImage(linkedGearPiece.gearIcon);
                UpdateSlotName(linkedGearPiece.gearName);
                if (linkedGearPiece.gearRarity == "common" || linkedGearPiece.gearRarity == "COMMON")
                    curSlotRarity = Slot.ItemRarity.common;
                else if (linkedGearPiece.gearRarity == "rare" || linkedGearPiece.gearRarity == "RARE")
                    curSlotRarity = Slot.ItemRarity.rare;
                else if (linkedGearPiece.gearRarity == "epic" || linkedGearPiece.gearRarity == "EPIC")
                    curSlotRarity = Slot.ItemRarity.epic;
                else if (linkedGearPiece.gearRarity == "legendary" || linkedGearPiece.gearRarity == "LEGENDARY")
                    curSlotRarity = Slot.ItemRarity.legendary;
                UpdateRarityBG(curSlotRarity, false);
            }
            else
            {
                isEmpty = true;

                activeStatusUI.UpdateContentText("");
                remainingUsesUI.UpdateContentText("");
                UpdateSlotName("");
                UpdateRarityBG(curSlotRarity, true);
            }

            remainingUsesUI.UpdateAlpha(0);
            activeStatusUI.UpdateAlpha(0);

            UpdateSlotBGRarity();
            UpdateRarityBorderColour();
        }

        if (isEmpty)
        {
            UpdateRarityBG(curSlotRarity, true);
            UpdateRarityBorderColour();
        }
        else
        {
            UpdateRarityBG(curSlotRarity, false);
            UpdateRarityBorderColour();
        }
        UpdateRarityBorderColour();
    }

    public void ToggleSkillUpgradeButtons(bool toggle)
    {
        //if (SkillsTabManager.Instance.GetActiveSkillBase().curSkillLevel >= 5)
            //toggle = false;

        buttonSkillUpgrade1.ToggleButton(toggle);

        if (toggle)
            buttonSkillUpgrade1.UpdateAlpha(1);
        else
            buttonSkillUpgrade1.UpdateAlpha(0);

        /*
        buttonSkillUpgrade2.ToggleButton(toggle);

        if (toggle)
            buttonSkillUpgrade2.UpdateAlpha(1);
        else
            buttonSkillUpgrade2.UpdateAlpha(0);
        */

        buttonSkillUpgrade3.ToggleButton(toggle);

        if (toggle)
            buttonSkillUpgrade3.UpdateAlpha(1);
        else
            buttonSkillUpgrade3.UpdateAlpha(0);

        // Skill upgrade power inc
        if (SkillsTabManager.Instance.GetActiveSkillBase().startingSkillPower == 0)
        {
            buttonSkillUpgrade3.ToggleButton(false);

            buttonSkillUpgrade3.gameObject.transform.parent.GetComponent<UIElement>().UpdateAlpha(0);
            buttonSkillUpgrade3.gameObject.transform.parent.GetComponent<UIElement>().ToggleButton(false);

            buttonSkillUpgrade3.UpdateAlpha(0);
        }
        else
        {
            buttonSkillUpgrade3.ToggleButton(true);

            buttonSkillUpgrade3.gameObject.transform.parent.GetComponent<UIElement>().UpdateAlpha(1);
            buttonSkillUpgrade3.gameObject.transform.parent.GetComponent<UIElement>().ToggleButton(true);

            buttonSkillUpgrade3.UpdateAlpha(1);
        }

        if (!toggle)
            buttonSkillUpgrade3.ToggleButton(false);
    }

    public void UpdateProgressSlider(SkillData skillData, bool addPoints = true)
    {
        if (skillData == null)
        {
            progressSlider.contentImage.fillAmount = 0;
            return;
        }

        //Debug.Log("inc slider");
        if (addPoints)
        {
            skillData.pointsAdded++;

            if (skillData.pointsAdded % 3 == 0)
            {
                skillData.pointsAdded = 0;
                skillData.upgradeIncTargetCount++;
                SkillsTabManager.Instance.SkillPointAdd(0, false);

                StartCoroutine(ResetProgressSlider());
            }
        }

        float val = skillData.pointsAdded / 3f;

        //Debug.Log(skillData.skillName + " Points added: " + skillData.pointsAdded);
        progressSlider.contentImage.fillAmount = val;
    }

    IEnumerator ResetProgressSlider()
    {
        yield return new WaitForSeconds(.25f);

        progressSlider.contentImage.fillAmount = 0;
    }

    public void ResetSlot(bool byPass = false, bool allowGearDefaultClear = false)
    {
        if (GetCurGearStatis() == SlotStatis.DEFAULT && !byPass)
            return;

        if (allowGearDefaultClear)
        {
            if (GetSlotOwnedBy() == SlotOwnedBy.MAIN)
                TeamGearManager.Instance.UpdateSlotsBaseDefault(this, null, true, false, false);
            else if (GetSlotOwnedBy() == SlotOwnedBy.SECOND)
                TeamGearManager.Instance.UpdateSlotsBaseDefault(this, null, false, true, false);
            else if (GetSlotOwnedBy() == SlotOwnedBy.THIRD)
                TeamGearManager.Instance.UpdateSlotsBaseDefault(this, null, false, false, true);

            //Debug.Log("assdsdsd");
            if (GetSlotOwnedBy() == SlotOwnedBy.MAIN)
                TeamItemsManager.Instance.UpdateSlotsBaseDefault(this, null, true, false, false);
            else if (GetSlotOwnedBy() == SlotOwnedBy.SECOND)
                TeamItemsManager.Instance.UpdateSlotsBaseDefault(this, null, false, true, false);
            else if (GetSlotOwnedBy() == SlotOwnedBy.THIRD)
                TeamItemsManager.Instance.UpdateSlotsBaseDefault(this, null, false, false, true);

            if (curGearType == SlotPieceType.helmet)
                UpdateSlotImage(TeamGearManager.Instance.helmetSlotSprite);
            else if (curGearType == SlotPieceType.chestpiece)
                UpdateSlotImage(TeamGearManager.Instance.chestSlotSprite);
            else if (curGearType == SlotPieceType.boots)
                UpdateSlotImage(TeamGearManager.Instance.bootsSlotSprite);
            else if (curGearType == SlotPieceType.neckless)
                UpdateSlotImage(TeamGearManager.Instance.necklessSlotSprite);
            else if (curGearType == SlotPieceType.earring)
                UpdateSlotImage(TeamGearManager.Instance.earringSlotSprite);
            else if (curGearType == SlotPieceType.belt)
                UpdateSlotImage(TeamGearManager.Instance.beltSlotSprite);
            else if (curGearType == SlotPieceType.glove)
                UpdateSlotImage(TeamGearManager.Instance.gloveSlotSprite);
            else if (curGearType == SlotPieceType.ring)
                UpdateSlotImage(TeamGearManager.Instance.ringSlotSprite);
            else if (curGearType == SlotPieceType.ITEM)
                UpdateSlotImage(TeamGearManager.Instance.clearSlotSprite);

            if (linkedItemPiece != null)
                UpdateLinkedItemPiece(null);

            if (linkedGearPiece != null)
                UpdateLinkedGearPiece(null);

            if (linkedSlot != null)
                UpdateLinkedSlot(null);

            UpdateSlotDetails();
        }


        UpdateSlotName("");
        UpdateGearBonusHealth(0);
        UpdateGearBonusDefense(0);
        UpdateGearBonusHealing(0);
        UpdateGearBonusDamage(0);
        UpdateGearBonusSpeed(0);

        isEmpty = true;
        UpdateSlotBGRarity();
        // Disable gear equip button if its empty
        TeamGearManager.Instance.UpdateGearNameText("");
        TeamGearManager.Instance.UpdateGearRarityText("");
        TeamGearManager.Instance.UpdateGearTypeText("");
        //TeamItemsManager.Instance.UpdateItemNameText("");
        //TeamItemsManager.Instance.UpdateItemDesc("");
        //UpdateCurGearType(GearType.EMPTY);
    }

    public void UpdateIconSkillSize(bool skill = true)
    {
        RectTransform rt = null;

        if (ownedSlotButton.GetComponent<RectTransform>())
            rt = ownedSlotButton.GetComponent<RectTransform>();

        if (skill)
        {
            rt.sizeDelta = new Vector2(382, 413);
            rt.localPosition = new Vector3(1, -0.28f);
        }
        else
        {
            rt.sizeDelta = new Vector2(175, 175);
            rt.localPosition = new Vector3(0, 9.5f);
        }
    }

    public void UpdateSlotName(string newName)
    {
        if (newName != "")
            isEmpty = false;

        slotName = newName;
    }

    public string GetSlotName()
    {
        return slotName;
    }

    public void UpdateSlotCode(int code)
    {
        this.code = code;
    }

    public int GetSlotCode()
    {
        return code;
    }

    public void UpdateGearBonusHealth(int bonusHealth)
    {
        this.gearBonusHealth = bonusHealth;
    }

    public int GetBonusHealth()
    {
        return gearBonusHealth;
    }

    public void UpdateGearBonusDamage(int bonusDamage)
    {
        this.gearBonusDamage = bonusDamage;
    }

    public int GetBonusDamage()
    {
        return gearBonusDamage;
    }

    public void UpdateGearBonusHealing(int bonusHealing)
    {
        this.gearBonusHealing = bonusHealing;
    }

    public int GetBonusHealing()
    {
        return gearBonusHealing;
    }
    public void UpdateGearBonusDefense(int bonusDefense)
    {
        this.gearBonusDefense = bonusDefense;
    }

    public int GetBonusDefense()
    {
        return gearBonusDefense;
    }
    public void UpdateGearBonusSpeed(int bonusSpeed)
    {
        this.gearBonusSpeed = bonusSpeed;
    }
        
    public int GetBonusSpeed()
    {
        return gearBonusSpeed;
    }

    public void UpdateGoldText(int gold)
    {
        activeStatusUI.UpdateContentText(gold.ToString());
    }

    public void UpdateSlotImage(Sprite sprite)
    {
        //Debug.Log("Updating slot image " + sprite.name);
        slotUI.UpdateContentImage(sprite);
    }

    public Sprite GetSlotImage()
    {
        return slotUI.contentImage.sprite;
    }

    public void UpdateRarity(Rarity rarity)
    {
        curRarity = rarity;
    }

    public Rarity GetRarity()
    {
        return curRarity;
    }

    public void UpdateGearOwnedBy(SlotOwnedBy gearOwnedBy)
    {
        curGearOwnedBy = gearOwnedBy;
    }

    public SlotOwnedBy GetSlotOwnedBy()
    {
        return curGearOwnedBy;
    }
     
    public void UpdateGearStatis(SlotStatis gearStatis)
    {
        curSlotStatis = gearStatis;
    }

    public SlotPieceType GetCurGearType()
    {
        return curGearType;
    }

    public void UpdateCurSlotType(SlotPieceType gearType)
    {
        curGearType = gearType;
    }

    public SlotStatis GetCurGearStatis()
    {
        return curSlotStatis;
    }

    public void ToggleOwnedGearButton(bool toggle)
    {


        if (toggle)
            ownedSlotButton.UpdateAlpha(1);
        else
            ownedSlotButton.UpdateAlpha(0);
    }

    public void ToggleCoverUI(bool toggle = true)
    {
        if (toggle)
        {
            //equipSlotButtonCover.UpdateAlpha(1);
            coverUI.UpdateAlpha(1);
            //equipSlotButton.ToggleButton(false);
            coverOn = true;
        }
        else
        {
            //equipSlotButtonCover.UpdateAlpha(0);
            coverUI.UpdateAlpha(0);
            //equipSlotButton.ToggleButton(true);

            coverOn = false;
        }
    }

    public void ToggleEquipButton(bool toggle, bool flag = false)
    {
        //Debug.Log("togging " + gameObject.name + " equip slot button " + toggle);

        if (buttonOwnedLoot != null)
        {
            //equipSlotButton.gameObject.transform.GetChild(0).gameObject.GetComponent<UIElement>().UpdateImage(toggle);

            if (toggle)
            {
                buttonOwnedLoot.UpdateAlpha(1);
            }
            else
            {
                buttonOwnedLoot.UpdateAlpha(0);
            }

            if (!toggle)
            {
                if (curSlotType2 == SlotType2.NOTBASE)
                    buttonOwnedLoot.ToggleButton(toggle);
            }
            else
            {
                buttonOwnedLoot.ToggleButton(toggle);
            }

        }

        // Update equip button icon
        if (toggle && flag && bagSprite)
        {
            bagSprite.UpdateAlpha(1);

            if (curGearType != SlotPieceType.ITEM)
            {
                for (int i = 0; i < OwnedLootInven.Instance.ownedGear.Count; i++)
                {
                    if (OwnedLootInven.Instance.ownedGear.Count < i)
                        break;

                    if (!OwnedLootInven.Instance.ownedGear[i].isDiscovered &&
                        OwnedLootInven.Instance.ownedGear[i].linkedGearPiece.gearType == GetCurGearType().ToString())
                    {
                        alertBagSprite.UpdateAlpha(1);
                        break;
                    }
                    else
                    {
                        alertBagSprite.UpdateAlpha(0);
                    }
                }

                if (OwnedLootInven.Instance.ownedGear.Count == 0)
                    alertBagSprite.UpdateAlpha(0);
            }
            else if (curGearType == SlotPieceType.ITEM)
            {
                for (int i = 0; i < OwnedLootInven.Instance.ownedItems.Count; i++)
                {
                    if (OwnedLootInven.Instance.ownedItems[i].isDiscovered)
                    {
                        alertBagSprite.UpdateAlpha(0);
                    }
                    else if (!OwnedLootInven.Instance.ownedItems[i].isDiscovered)
                    {
                        alertBagSprite.UpdateAlpha(1);
                        break;
                    }
                }

                if (OwnedLootInven.Instance.ownedItems.Count == 0)
                    alertBagSprite.UpdateAlpha(0);
            }

        }
    }

    public void UpdateSlotDetails(GearPiece gear, bool toggle = true, string gearType = "")
    {
        linkedGearPiece = gear;

        if (!toggle)
        {
            if (gear)
                UpdateSlotImage(gear.gearIcon);
            isEmpty = false;
            UpdateSlotBGRarity();
        }
        else
        {
            if (gearType == "helmet")
                UpdateSlotImage(TeamGearManager.Instance.helmetSlotSprite);
            else if (gearType == "chestpiece")
                UpdateSlotImage(TeamGearManager.Instance.chestSlotSprite);
            else if (gearType == "boots")
                UpdateSlotImage(TeamGearManager.Instance.bootsSlotSprite);
            else if (gearType == "neckless")
                UpdateSlotImage(TeamGearManager.Instance.necklessSlotSprite);
            else if (gearType == "earring")
                UpdateSlotImage(TeamGearManager.Instance.earringSlotSprite);
            else if (gearType == "belt")
                UpdateSlotImage(TeamGearManager.Instance.beltSlotSprite);
            else if (gearType == "glove")
                UpdateSlotImage(TeamGearManager.Instance.gloveSlotSprite);
            else if (gearType == "ring")
                UpdateSlotImage(TeamGearManager.Instance.ringSlotSprite);

            isEmpty = true;
            UpdateSlotBGRarity();

        }
    }

    public void ToggleSlotSelection(bool toggle)
    {
        //Debug.Log("Toggling Slot " + toggle);
        isSelected = toggle;

        if (toggle)
            slotSelectionUI.UpdateAlpha(1);
        else
            slotSelectionUI.UpdateAlpha(0);

        if (curGearType == SlotPieceType.SKILL)
            SkillsTabManager.Instance.UpdateUnspentPointsText(1);
    }

    /*
    public void ToggleSkillSelected(bool toggle)
    {
        //Debug.Log("Toggling Skill " + toggle);

        if (toggle)
        {
            slotSelectionUI.UpdateAlpha(1);
        }
        else
        {
            slotSelectionUI.UpdateAlpha(0);
        }
    }
    */

    public UIElement GetSlotUI()
    {
        return slotUI;
    }

    public void UpdateLootGearAlpha(bool toggle)
    {
        gameObject.GetComponent<UIElement>().ToggleButton(toggle);
    }

    public void ToggleMainSlot(bool toggle)
    {
        //Debug.Log("Toggling main slot " + toggle);
        if (slotUI.doScalePunch)
        {
            slotUI.doScalePunch = false;
        }

        if (toggle)
            slotUI.UpdateAlpha(1);
        else
            slotUI.UpdateAlpha(0);
    }

    public void UpdateLinkedGearPiece(GearPiece gearPiece)
    {
        linkedGearPiece = gearPiece;
    }

    public void UpdateLinkedItemPiece(ItemPiece itemPiece)
    {
        linkedItemPiece = itemPiece;
    }
}
