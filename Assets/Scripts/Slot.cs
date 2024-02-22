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

    public enum SlotOwnedBy { MAIN, SECOND, THIRD, NOONE}
    public SlotOwnedBy curGearOwnedBy;

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
    public UIElement goldtextUI;

    public bool isGold;
    public bool isEmpty = true;
    public int code = 0;
    private int pointsAdded = 0;
    //public bool isLocked;
    public GearPiece linkedGearPiece;
    public ItemPiece linkedItemPiece;
    public bool baseSlot = false;
    public bool isSelected;
    private void Start()
    {
        ToggleSlotSelection(false);
    }

    public void ToggleSkillUpgradeButtons(bool toggle)
    {
        buttonSkillUpgrade1.ToggleButton(toggle);

        if (toggle)
            buttonSkillUpgrade1.UpdateAlpha(1);
        else
            buttonSkillUpgrade1.UpdateAlpha(0);


        buttonSkillUpgrade2.ToggleButton(toggle);

        if (toggle)
            buttonSkillUpgrade2.UpdateAlpha(1);
        else
            buttonSkillUpgrade2.UpdateAlpha(0);


        buttonSkillUpgrade3.ToggleButton(toggle);

        if (toggle)
            buttonSkillUpgrade3.UpdateAlpha(1);
        else
            buttonSkillUpgrade3.UpdateAlpha(0);
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

        TeamItemsManager.Instance.UpdateItemNameText("");
        TeamItemsManager.Instance.UpdateItemDesc("");
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
            rt.sizeDelta = new Vector2(200, 200);
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
        goldtextUI.UpdateContentText(gold.ToString());
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
        equipSlotButton.ToggleButton(toggle);

        if (toggle)
            ownedSlotButton.UpdateAlpha(1);
        else
            ownedSlotButton.UpdateAlpha(0);
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
