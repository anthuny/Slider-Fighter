using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemPiece : ScriptableObject
{
    public enum Rarity { COMMON, RARE, EPIC, LEGENDARY }
    public Rarity curRarity;

    public enum RaceSpecific { ALL, HUMAN, BEAST, ETHEREAL }
    public RaceSpecific curRace;

    public enum ItemCombatType { REFILLABLE, CONSUMABLE }
    public ItemCombatType curItemCombatType;

    public enum ItemTargetType { NORMAL, RANDOM }
    public ItemTargetType curItemTargetType;
    public enum ItemType { OFFENSE, SUPPORT }
    public ItemType curItemType;

    public enum SelectionType { ENEMIES, ALLIES }
    public SelectionType curSelectionType;

    public enum TargetType { ALIVE, DEAD }
    public TargetType curTargetType;

    public enum ActiveType { ACTIVE, PASSIVE }
    public ActiveType curActiveType;

    public enum HitType { HITS, INSTANT }
    public HitType curHitType;

    public string itemName;
    public string itemDesc;
    public Sprite itemSpriteItemTab;
    public Sprite itemSpriteCombat;
    public Sprite itemSpriteCombatSmaller;
    public Sprite itemSpriteCombatProjectile;
    public Sprite itemSpriteFail;

    public EffectData effectAdded;
    public int effectAddedTurnLength = 1;

    public int itemPower;
    public int threshHoldAmount;

    public bool isSelfCast;
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
    public int cleanseCount = 0;

    public bool activeOnTurnStart = true;
    public bool activeOnSkillAttack;
    public bool activeOnAlliesAttacked;
    public bool activeOnEnemiesHealing;
    public bool activeOnSelfAttacked;

    [Space(2)]  

    public int basePrice;
    public bool purchased;
    public bool healthItem = false;
    public bool isSpecial;
    public int power;
    public int procChance;
    public RuntimeAnimatorController ac;
    public RuntimeAnimatorController itemVisualAC;
    public int maxUsesPerCombat = 5;
    public int targetCount;
    public int hitCount;
    public int projectileSpeed = 9;
    public bool allowRandomSpawnPosition;
    public bool allowRandomSpawnRotation;
    public bool projectileAllowSpin;
    public AudioClip itemAnnounce;
    public AudioClip projectileLaunch;
    public AudioClip projectileHit;

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

        this.itemSpriteItemTab = itemIcon;
    }
}
