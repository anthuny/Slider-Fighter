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
}
