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

    public UIElement ownedSlotButton;
    [SerializeField] private UIElement equipSlotButton;
    public UIElement goldtextUI;

    public bool isGold;
    public bool isEmpty = true;
    //public bool isLocked;

    private void Start()
    {
        ToggleSlotSelection(false);
    }

    public void ResetGearSlot(bool byPass = false, bool allowGearDefaultClear = false)
    {
        if (GetCurGearStatis() == SlotStatis.DEFAULT && !byPass)
            return;

        if (allowGearDefaultClear)
        {
            if (GetGearOwnedBy() == SlotOwnedBy.MAIN)
                TeamGearManager.Instance.UpdateSlotsBaseDefault(this, null, true, false, false);
            else if (GetGearOwnedBy() == SlotOwnedBy.SECOND)
                TeamGearManager.Instance.UpdateSlotsBaseDefault(this, null, false, true, false);
            else if (GetGearOwnedBy() == SlotOwnedBy.THIRD)
                TeamGearManager.Instance.UpdateSlotsBaseDefault(this, null, false, false, true);
        }
        else
        {


            /*
            if (GetCurGearType() == GearType.HELMET)
                UpdateGearImage(TeamGearManager.Instance.helmetSlotSprite);
            else if (GetCurGearType() == GearType.CHESTPIECE)
                UpdateGearImage(TeamGearManager.Instance.chestSlotSprite);
            else if (GetCurGearType() == GearType.LEGGINGS)
                UpdateGearImage(TeamGearManager.Instance.legsSlotSprite);
            else if (GetCurGearType() == GearType.BOOTS)
                UpdateGearImage(TeamGearManager.Instance.bootsSlotSprite);
                */
        }

        UpdateSlotName("");
        UpdateGearBonusHealth(0);
        UpdateGearBonusDefense(0);
        UpdateGearBonusHealing(0);
        UpdateGearBonusDamage(0);
        UpdateGearBonusSpeed(0);

        isEmpty = true;
        // Disable gear equip button if its empty
        //Debug.Log("about to reset " + gameObject.name);
        //ToggleEquipButton(false);


        TeamGearManager.Instance.UpdateGearNameText("");

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

    public SlotOwnedBy GetGearOwnedBy()
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
}
