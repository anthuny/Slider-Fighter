using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public enum ItemType { HELMET, CHESTPIECE, LEGGINGS, BOOTS, EMPTY }
    public ItemType curGearType;

    public enum Rarity { COMMON, RARE, EPIC, LEGENDARY }
    public Rarity curRarity;

    public enum ItemStatis { OWNED, UNOWNED, REWARD, DEFAULT }
    public ItemStatis curGearStatis;

    public enum ItemOwnedBy { MAIN, SECOND, THIRD, NOONE }
    public ItemOwnedBy curGearOwnedBy;

    private string itemName;
    private int bonusHealth;
    private int bonusDamage;
    private int bonusHealing;
    private int bonusDefense;
    private int bonusSpeed;

    [SerializeField] private UIElement itemUI;
    [SerializeField] private UIElement itemSelectionUI;

    [SerializeField] private UIElement ownedItemButton;
    [SerializeField] private UIElement equipItemButton;
    public UIElement goldtextUI;

    public bool isGold;
    public bool isEmpty = true;


    private void Start()
    {
        ToggleItemEnabled(false);
    }

    public void ResetGearSlot(bool byPass = false, bool allowGearDefaultClear = false)
    {
        if (GetCurGearStatis() == ItemStatis.DEFAULT && !byPass)
            return;

        if (allowGearDefaultClear)
        {
            if (GetGearOwnedBy() == ItemOwnedBy.MAIN)
                TeamGearManager.Instance.UpdateSlotsBaseDefault(null, this, true, false, false);
            else if (GetGearOwnedBy() == ItemOwnedBy.SECOND)
                TeamGearManager.Instance.UpdateSlotsBaseDefault(null, this, false, true, false);
            else if (GetGearOwnedBy() == ItemOwnedBy.THIRD)
                TeamGearManager.Instance.UpdateSlotsBaseDefault(null, this, false, false, true);
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

        itemName = newName;
    }

    public string GetGearName()
    {
        return itemName;
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
        itemUI.UpdateContentImage(sprite);
    }

    public Sprite GetGearImage()
    {
        return itemUI.contentImage.sprite;
    }

    public void UpdateRarity(Rarity rarity)
    {
        curRarity = rarity;
    }

    public Rarity GetRarity()
    {
        return curRarity;
    }

    public void UpdateGearOwnedBy(ItemOwnedBy gearOwnedBy)
    {
        curGearOwnedBy = gearOwnedBy;
    }

    public ItemOwnedBy GetGearOwnedBy()
    {
        return curGearOwnedBy;
    }

    public void UpdateGearStatis(ItemStatis gearStatis)
    {
        curGearStatis = gearStatis;
    }

    public ItemType GetCurGearType()
    {
        return curGearType;
    }

    public void UpdateCurGearType(ItemType gearType)
    {
        curGearType = gearType;
    }

    public ItemStatis GetCurGearStatis()
    {
        return curGearStatis;
    }

    public void ToggleOwnedGearButton(bool toggle)
    {
        if (toggle)
            ownedItemButton.UpdateAlpha(1);
        else
            ownedItemButton.UpdateAlpha(0);
    }

    public void ToggleEquipButton(bool toggle)
    {
        if (equipItemButton != null)
        {
            equipItemButton.gameObject.transform.GetChild(0).gameObject.GetComponent<UIElement>().UpdateImage(toggle);

            if (toggle)
                equipItemButton.UpdateAlpha(1);
            else
                equipItemButton.UpdateAlpha(0);
        }
    }

    public void ToggleItemEnabled(bool toggle)
    {
        if (toggle)
        {
            itemSelectionUI.UpdateAlpha(1);
        }
        else
        {
            itemSelectionUI.UpdateAlpha(0);
        }
    }

    public UIElement GetGearSelection()
    {
        return itemUI;
    }

    public void UpdateGearAlpha(bool toggle)
    {
        gameObject.GetComponent<UIElement>().ToggleButton(toggle);
    }

    public void ToggleMainGear(bool toggle)
    {
        if (itemUI.doScalePunch)
        {
            itemUI.doScalePunch = false;
        }

        if (toggle)
            itemUI.UpdateAlpha(1);
        else
            itemUI.UpdateAlpha(0);
    }
}
