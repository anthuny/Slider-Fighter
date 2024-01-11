using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gear : MonoBehaviour
{
    public enum GearType { HELMET, CHESTPIECE, LEGGINGS, BOOTS, EMPTY }
    public GearType curGearType;

    public enum Rarity { COMMON, RARE, EPIC, LEGENDARY }
    public Rarity curRarity;

    public enum GearStatis { OWNED, UNOWNED, REWARD, DEFAULT }
    public GearStatis curGearStatis;

    public enum GearOwnedBy { MAIN, SECOND, THIRD, NOONE}
    public GearOwnedBy curGearOwnedBy;

    private string gearName;
    private int bonusHealth;
    private int bonusDamage;
    private int bonusHealing;
    private int bonusDefense;
    private int bonusSpeed;

    [SerializeField] private UIElement gearUI;
    [SerializeField] private UIElement gearSelectionUI;

    [SerializeField] private UIElement ownedGearButton;
    [SerializeField] private UIElement equipGearButton;
    public UIElement goldtextUI;

    public bool isGold;
    public bool isEmpty = true;


    private void Start()
    {
        ToggleGearEnabled(false);
    }

    public void ResetGearSlot(bool byPass = false, bool allowGearDefaultClear = false)
    {
        if (GetCurGearStatis() == GearStatis.DEFAULT && !byPass)
            return;

        if (allowGearDefaultClear)
        {
            if (GetGearOwnedBy() == GearOwnedBy.MAIN)
                TeamGearManager.Instance.UpdateGearSlotsBaseDefault(this, null, true, false, false);
            else if (GetGearOwnedBy() == GearOwnedBy.SECOND)
                TeamGearManager.Instance.UpdateGearSlotsBaseDefault(this, null, false, true, false);
            else if (GetGearOwnedBy() == GearOwnedBy.THIRD)
                TeamGearManager.Instance.UpdateGearSlotsBaseDefault(this, null, false, false, true);
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

        UpdateGearName("");
        UpdateGearBonusHealth(0);
        UpdateGearBonusDefense(0);
        UpdateGearBonusHealing(0);
        UpdateGearBonusDamage(0);
        UpdateGearBonusSpeed(0);

        isEmpty = true;
        // Disable gear equip button if its empty
        ToggleEquipButton(false);

        TeamGearManager.Instance.UpdateGearNameText("");

        //UpdateCurGearType(GearType.EMPTY);
    }

    public void UpdateGearName(string newName)
    {
        if (newName != "")
            isEmpty = false;

        gearName = newName;
    }

    public string GetGearName()
    {
        return gearName;
    }

    public void UpdateGearBonusHealth(int bonusHealth)
    {
        this.bonusHealth = bonusHealth;
    }

    public int GetBonusHealth()
    {
        return bonusHealth;
    }

    public void UpdateGearBonusDamage(int bonusDamage)
    {
        this.bonusDamage = bonusDamage;
    }

    public int GetBonusDamage()
    {
        return bonusDamage;
    }

    public void UpdateGearBonusHealing(int bonusHealing)
    {
        this.bonusHealing = bonusHealing;
    }

    public int GetBonusHealing()
    {
        return bonusHealing;
    }
    public void UpdateGearBonusDefense(int bonusDefense)
    {
        this.bonusDefense = bonusDefense;
    }

    public int GetBonusDefense()
    {
        return bonusDefense;
    }
    public void UpdateGearBonusSpeed(int bonusSpeed)
    {
        this.bonusSpeed = bonusSpeed;
    }
        
    public int GetBonusSpeed()
    {
        return bonusSpeed;
    }

    public void UpdateGoldText(int gold)
    {
        goldtextUI.UpdateContentText(gold.ToString());
    }

    public void UpdateGearImage(Sprite sprite)
    {
        gearUI.UpdateContentImage(sprite);
    }

    public Sprite GetGearImage()
    {
        return gearUI.contentImage.sprite;
    }

    public void UpdateRarity(Rarity rarity)
    {
        curRarity = rarity;
    }

    public Rarity GetRarity()
    {
        return curRarity;
    }

    public void UpdateGearOwnedBy(GearOwnedBy gearOwnedBy)
    {
        curGearOwnedBy = gearOwnedBy;
    }

    public GearOwnedBy GetGearOwnedBy()
    {
        return curGearOwnedBy;
    }
     
    public void UpdateGearStatis(GearStatis gearStatis)
    {
        curGearStatis = gearStatis;
    }

    public GearType GetCurGearType()
    {
        return curGearType;
    }

    public void UpdateCurGearType(GearType gearType)
    {
        curGearType = gearType;
    }

    public GearStatis GetCurGearStatis()
    {
        return curGearStatis;
    }

    public void ToggleOwnedGearButton(bool toggle)
    {
        if (toggle)
            ownedGearButton.UpdateAlpha(1);
        else
            ownedGearButton.UpdateAlpha(0);
    }

    public void ToggleEquipButton(bool toggle)
    {
        if (equipGearButton != null)
        {
            equipGearButton.gameObject.transform.GetChild(0).gameObject.GetComponent<UIElement>().UpdateImage(toggle);

            if (toggle)
                equipGearButton.UpdateAlpha(1);
            else
                equipGearButton.UpdateAlpha(0);
        }
    }

    public void ToggleGearEnabled(bool toggle)
    {
        gearSelectionUI.gameObject.SetActive(toggle);
    }

    public void ToggleGearSelected(bool toggle)
    {
        if (toggle)
        {
            gearSelectionUI.UpdateAlpha(1);
        }
        else
        {
            gearSelectionUI.UpdateAlpha(0);
        }
    }

    public UIElement GetGearSelection()
    {
        return gearUI;
    }

    public void UpdateGearAlpha(bool toggle)
    {
        gameObject.GetComponent<UIElement>().ToggleButton(toggle);
    }

    public void ToggleMainGear(bool toggle)
    {
        if (gearUI.doScalePunch)
        {
            gearUI.doScalePunch = false;
        }

        if (toggle)
            gearUI.UpdateAlpha(1);
        else
            gearUI.UpdateAlpha(0);
    }
}
