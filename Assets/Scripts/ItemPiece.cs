using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemPiece : ScriptableObject
{
    public enum Rarity { COMMON, RARE, EPIC, LEGENDARY }
    public Rarity curRarity;

    public string itemName;
    public string itemDesc;
    public Sprite itemSprite;

    public EffectData effectAdded;
    public int effectAddedTurnLength = 1;

    public int itemPower;
    public int threshHoldAmount;

    public bool isBaseHealing;
    public bool isLifestealing;
    public bool isDamageNegating;
    public bool isIncWeaponHitSpace;
    public bool isVampHealing;
    public bool isReflectingEffect;
    public bool isEffectStrippingTarget;
    public bool isCleansingAll;
    public bool isCleansingRandom;
    public EffectData effectCleansed;

    public bool activeOnTurnStart = true;
    public bool activeOnSkillAttack;
    public bool activeOnAlliesAttacked;
    public bool activeOnEnemiesHealing;
    public bool activeOnSelfAttacked;

    [Space(2)]  

    public int basePrice;
    public bool purchased;
    public bool healthItem = false;
    public int power;
    public int procChance;
    public RuntimeAnimatorController ac;
    public int maxUsesPerCombat = 5;

    public void UpdateItemPiece(string newName, string itemRarity, Sprite itemIcon)
    {
        this.itemName = newName;
        //this.gearType = gearType;
        if (itemRarity == "common")
            curRarity = Rarity.COMMON;
        else if (itemRarity == "rare")
            curRarity = Rarity.RARE;
        else if (itemRarity == "epic")
            curRarity = Rarity.EPIC;
        else if (itemRarity == "legendary")
            curRarity = Rarity.LEGENDARY;

        this.itemSprite = itemIcon;
    }
}
