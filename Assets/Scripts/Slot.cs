using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public enum SlotType { HELMET, CHESTPIECE, BOOTS, SKILL, ITEM, EMPTY }
    public SlotType curGearType;

    public enum Rarity { COMMON, RARE, EPIC, LEGENDARY }
    public Rarity curRarity;

    public enum SlotStatis { OWNED, UNOWNED, REWARD, DEFAULT }
    public SlotStatis curSlotStatis;

    public enum SlotRace { HUMAN, BEAST, ETHEREAL, ALL }
    public SlotRace curSlotRace;

    public enum SlotOwnedBy { MAIN, SECOND, THIRD, NOONE}
    public SlotOwnedBy curGearOwnedBy;

    public enum SlotPosition { FIRST, SECOND, THIRD }
    public SlotPosition curSlotPosition;

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
    [SerializeField] private UIElement equipSlotButton;
    [SerializeField] private UIElement equipSlotButtonCover;
    [SerializeField] private UIElement coverUI;
    [SerializeField] private UIElement raceIcon;
    [SerializeField] private UIElement rarityBG;
    [SerializeField] private UIElement remainingUsesUI;
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

    public enum ItemRarity { COMMON, RARE, EPIC }
    public ItemRarity curItemRarity;
    public int itemUseCount = 0;

    public bool maxSet = false;
    private void Start()
    {
        ToggleSlotSelection(false);
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

    public int GetCalculatedItemUsesRemaining()
    {
        return linkedSlot.linkedItemPiece.maxUsesPerCombat - linkedSlot.itemUses;
    }

    
    public void UpdateRaceIcon(Sprite newIcon)
    {
        raceIcon.UpdateContentImage(newIcon);
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

        if (itemRarity == ItemRarity.COMMON)
        {
            rarityBG.UpdateColour(ItemRewardManager.Instance.commonColour);
        }
        else if (itemRarity == ItemRarity.RARE)
        {
            rarityBG.UpdateColour(ItemRewardManager.Instance.rareColour);
        }
        else if (itemRarity == ItemRarity.EPIC)
        {
            rarityBG.UpdateColour(ItemRewardManager.Instance.epicColour);
        }
    }

    public int GetItemUseCount()
    {
        return itemUseCount;
    }

    public void IncItemUseCount()
    {
        itemUseCount++;
    }

    public void ResetItemUseCount()
    {
        itemUseCount = 0;
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
    public void UpdateSlotDetails()
    {
        Slot.ItemRarity curSlotRarity = Slot.ItemRarity.COMMON;
        Slot.SlotRace curSlotRace = Slot.SlotRace.ALL;
        string activeStatus = "";
        int itemUsesRemaining = 0;

        if (linkedItemPiece == null)
        {
            UpdateRarityBG(curSlotRarity, true);
            UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);

            equipSlotButton.ToggleButton(true);
            equipSlotButton.UpdateAlpha(1);

            ownedSlotButton.UpdateAlpha(0);

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
        }
        else
        {
            // Update Linked Slot for main slot
            //TeamItemsManager.Instance.UpdateMainSlotLinkedSlot();

            UpdateSlotName(linkedItemPiece.itemName);

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

            if (linkedItemPiece.curRarity == ItemPiece.Rarity.COMMON)
                curSlotRarity = Slot.ItemRarity.COMMON;
            else if (linkedItemPiece.curRarity == ItemPiece.Rarity.RARE)
                curSlotRarity = Slot.ItemRarity.RARE;
            else if (linkedItemPiece.curRarity == ItemPiece.Rarity.EPIC)
                curSlotRarity = Slot.ItemRarity.EPIC;

            //UpdateSlotDetails(activeStatus, false, itemUsesRemaining, curSlotRace, curSlotRarity, false);

            // Update BG rarity of SlOT
            UpdateRarityBG(curSlotRarity);

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
                UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);
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

            equipSlotButton.ToggleButton(false);
            equipSlotButton.UpdateAlpha(0);

            ownedSlotButton.UpdateAlpha(1);
        }
    }

    public void ToggleSkillUpgradeButtons(bool toggle)
    {
        if (SkillsTabManager.Instance.GetActiveSkillBase().curSkillLevel >= 5)
            toggle = false;

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

            UpdateSlotImage(TeamItemsManager.Instance.clearSlotSprite);

            if (linkedItemPiece != null)
                UpdateLinkedItemPiece(null);

            UpdateSlotDetails();
        }

        UpdateSlotName("");
        UpdateGearBonusHealth(0);
        UpdateGearBonusDefense(0);
        UpdateGearBonusHealing(0);
        UpdateGearBonusDamage(0);
        UpdateGearBonusSpeed(0);

        isEmpty = true;
        // Disable gear equip button if its empty
        TeamGearManager.Instance.UpdateGearNameText("");

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

    public SlotType GetCurGearType()
    {
        return curGearType;
    }

    public void UpdateCurSlotType(SlotType gearType)
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

    public void ToggleEquipButton(bool toggle)
    {
        //Debug.Log("togging " + gameObject.name + " equip slot button " + toggle);

        if (equipSlotButton != null)
        {
            //equipSlotButton.gameObject.transform.GetChild(0).gameObject.GetComponent<UIElement>().UpdateImage(toggle);

            if (toggle)
            {
                equipSlotButton.UpdateAlpha(1);
            }
            else
            {
                equipSlotButton.UpdateAlpha(0);
            }

            equipSlotButton.ToggleButton(toggle);
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

        if (curGearType == SlotType.SKILL)
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
