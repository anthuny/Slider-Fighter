using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitFunctionality : MonoBehaviour
{
    RectTransform rt;
    public enum UnitType { PLAYER, ENEMY };
    public UnitType curUnitType;

    public enum UnitRace { HUMAN, BEAST, ETHEREAL };
    public UnitRace curUnitRace;

    public enum LastOpenedMastery { STANDARD, ADVANCED };
    public LastOpenedMastery lastOpenedStatPage;

    [SerializeField] private UIElement fighterRaceIcon;
    [SerializeField] private UIElement heroHitsAccTextPos;
    [SerializeField] private UIElement enemyHitsAccTextPos;
    public HeroWeapon heroWeapon;
    public UIElement heroWeaponUI;
    public ButtonFunctionality selectUnitButton;
    [SerializeField] private Color effectTitleColour;
    [SerializeField] private GameObject toolTipItemGO;
    [SerializeField] private GameObject tooltipGearGO;
    [SerializeField] private UIElement tooltipStats;
    [SerializeField] private UIElement tooltipItems;
    [SerializeField] private UIElement tooltipGear;
    [SerializeField] private UIElement tooltipEffect;
    [SerializeField] private UIElement statHealth;
    [SerializeField] private UIElement statPower;
    [SerializeField] private UIElement statHealingPower;
    [SerializeField] private UIElement statDefense;
    [SerializeField] private UIElement statSpeed;
    [SerializeField] private UIElement unitLevelImage;
    [SerializeField] private UIElement selectionCircle;
    [SerializeField] private UIElement unitVisuals;
    private Sprite unitIcon;
    [SerializeField] private Transform unitVisualsParent;
    [SerializeField] private Transform powerUIParent;

    [SerializeField] private UIElement hitsRemainingText;
    [SerializeField] private Image unitHealthBar;
    [SerializeField] private Image unitAttackBar;
    [SerializeField] private Image unitAttackBarNext;
    public UIElement healthBarUIElement;
    public UIElement attackBarUIElement;
    public UIElement unitAttackBarNextUIElement;
    public UIElement itemVisualAlert;

    public Image unitImage;
    public UIElement curUnitTurnArrow;
    [Tooltip("Healing amplification this unit recives. This value is multiplied by the healing recieved. 1 = normal, 0 = nothing, 2 = double")]
    public float curHealingRecieved = 1;
    public int curSpeed;
    public int startingSpeed;
    public int curPower;
    public int curHealingPower;
    public float curDefense;

    [SerializeField] private int curHealth;
    [SerializeField] private int curMaxHealth;
    [SerializeField] private int startingMaxHealth;
    [SerializeField] private int curLevel;

    public int curPowerHits;
    public int powerHitsRoomStarting;
    public int curHealingHits;
    //public int curPowerHits;
    private float curExp;
    private float maxExp;
    [SerializeField] private int curSpeedIncPerLv = 0;
    [SerializeField] private int curPowerIncPerLv = 0;
    [SerializeField] private int curHealingPowerIncPerLv = 0;
    [SerializeField] private float curDefenseIncPerLv = 0;
    [SerializeField] private int maxHealthIncPerLv = 0;
    public int startingRoomSpeed;
    public float startingRoomDefense;
    public int startingRoomMaxHealth;
    public int startingHealth;
    public int startingDamage;
    public int startingHealing;
    public int startingDefense;
    [HideInInspector]
    public int unitStartTurnEnergyGain;
    public EnergyCost energyCostImage;
    [SerializeField] private Transform projectileParent;
    //s[SerializeField] private Sprite projectileSprite;
    [SerializeField] private UIElement unitExpBar;
    [SerializeField] private Text unitLevelText;
    [SerializeField] private Image unitExpBarImage;
    [SerializeField] private Text unitExpGainText;
    [SerializeField] private float fillAmountInterval;
    [SerializeField] private UIElement unitBg;
    [SerializeField] private UIElement unitUIElement;
    [SerializeField] private Transform effectsParent;
    public UIElement effects;
    public List<Effect> activeEffects = new List<Effect>();
    public int curRecieveDamageAmp = 100;
    [SerializeField] private Color unitColour;

    [SerializeField] private List<ItemPiece> equiptItems = new List<ItemPiece>();

    [SerializeField] private List<SkillData> curSkillBaseSlots = new List<SkillData>();

    [SerializeField] private List<SkillData> startingSkills = new List<SkillData>();
    [SerializeField] private List<SkillData> skills = new List<SkillData>();

    public UnitData unitData;

    [HideInInspector]
    public GameObject prevPowerUI;

    public bool isSelected;
    private int unitValue;
    private Animator animator;
    public Animator effectDisplayAnimator;

    // Team Stat Page
    public int statsBase1Added;
    public int statsBase2Added;
    public int statsBase3Added;
    public int statsBase4Added;
    public int statsBase5Added;

    public int statsAdv1Added;
    public int statsAdv2Added;
    public int statsAdv3Added;
    public int statsAdv4Added;

    public int spentMasteryTotalPoints = 0;
    public int SkillSpentPoints = 0;
    public int statsSpentAdvPoints = 0;

    public CombatUnitFocus unitFocus;

    public bool idleBattle;
    public bool isDead;
    public bool isTaunting;
    public bool isParrying;
    public bool attacked;
    public bool isVisible;


    private int curAttackCharge;
    private int attackChargeTurnStart;
    private int maxAttackCharge = 100;

    private float oldCurSpeed = 0;
    private float oldCurDefense = 0;
    public bool isSpeedUp;
    public bool isSpeedDown;
    public bool isDefenseUp;
    public bool isDefenseDown;
    public bool isPoison;
    public bool isPoisonLeaching;
    public int bonusDmgLines;
    public int bonusHealingLines;
    public int reducedCooldownsCount;
    public int rerollItemCount;

    public bool heroRoomUnit;

    SimpleFlash hitFlash;
    public UIElement uiElement;

    [HideInInspector]
    public AudioClip deathClip, hitRecievedClip;

    [HideInInspector]
    public int hitsRemaining;

    public int prevStatHealth;
    public int prevStatPower;
    public int prevStatHealingPower;
    public int prevStatDefense;
    public int prevStatSpeed;
    public string selectedEffect;

    public int skill0CurCooldown;
    public int skill1CurCooldown;
    public int skill2CurCooldown;
    public int skill3CurCooldown;

    public int item1CurUses;
    public int item2CurUses;
    public int item3CurUses;

    public Canvas visualCanvas;
    public int teamIndex;
    public bool purchased = false;
    //public bool unitDouble;

    public SkillData usedSkill;
    public int effectAddedCount;

    public bool reanimated;
    public bool beenAttacked = false;
    public UnitFunctionality holyLinkPartner;
    [SerializeField] private UIElement unitAlertTextParent;
    [SerializeField] private UIElement unitAlertStay;
    [SerializeField] private GameObject unitAlertText;
    public bool hitPerfect = false;

    public void UpdateFighterRaceIcon(string fighterRace)
    {
        if (fighterRace == "HUMAN")
        {
            fighterRaceIcon.UpdateContentImage(GameManager.Instance.humanRaceIcon);
            fighterRaceIcon.tooltipStats.UpdateTooltipStatsText("HUMAN");
        }           
        else if (fighterRace == "BEAST")
        {
            fighterRaceIcon.UpdateContentImage(GameManager.Instance.beastRaceIcon);
            fighterRaceIcon.tooltipStats.UpdateTooltipStatsText("BEAST");
        }          
        else if (fighterRace == "ETHEREAL")
        {
            fighterRaceIcon.UpdateContentImage(GameManager.Instance.etherealRaceIcon);
            fighterRaceIcon.tooltipStats.UpdateTooltipStatsText("ETHEREAL");
        }       
    }

    public void ToggleFighterRaceIcon(bool toggle = true)
    {
        if (toggle)
        {
            fighterRaceIcon.UpdateAlpha(1);
        }
        else
        {
            fighterRaceIcon.UpdateAlpha(0);
        }
    }
    public void UpdateTooltipItems(float maxCharges = 0f, float curCharges = 0f, int itemIndex = 0)
    {
        //Debug.Log("max charges = " + maxCharges);
        //Debug.Log("cur charges = " + curCharges);

        bool enabled = false;

        int count = 0;
        int index = 0;
        for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
        {
            if (GameManager.Instance.activeRoomHeroes[i] == this)
            {
                if (i == 0)
                {
                    count = TeamItemsManager.Instance.equippedItemsMain.Count;
                    index = 0;
                    break;
                }
                else if (i == 1)
                {
                    count = TeamItemsManager.Instance.equippedItemsSecond.Count;
                    index = 1;
                    break;
                }
                else if (i == 2)
                {
                    count = TeamItemsManager.Instance.equippedItemsThird.Count;
                    index = 2;
                    break;
                }
            }
        }

        if (count > 0)
        {
            enabled = true;
            tooltipItems.UpdateAlpha(1);
        }
        else
        {
            enabled = false;
            tooltipItems.UpdateAlpha(0);
        }

        // Destroy all existing items
        for (int i = 0; i < tooltipItems.transform.childCount; i++)
        {
            Destroy(tooltipItems.transform.GetChild(i).gameObject);
        }

        /*
        if (index == 0)
            count = TeamItemsManager.Instance.equippedItemsMain.Count;
        else if (index == 1)
            count = TeamItemsManager.Instance.equippedItemsSecond.Count;
        else if (index == 2)
            count = TeamItemsManager.Instance.equippedItemsThird.Count;
        */

        bool doneOnce = false;

        for (int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(toolTipItemGO, tooltipItems.transform.position, Quaternion.identity);
            go.transform.SetParent(tooltipItems.transform);

            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;

            int newCurCharges = -1;

            if (i == 0)
                newCurCharges = item1CurUses;
            else if (i == 1)
                newCurCharges = item2CurUses;
            else if (i == 2)
                newCurCharges = item3CurUses;

            // Set gear data
            if (index == 0)
            {
                go.GetComponent<UIElement>().UpdateContentImage(TeamItemsManager.Instance.equippedItemsMain[i].itemSpriteItemTab);
                go.GetComponent<UIElement>().UpdateSlider(TeamItemsManager.Instance.equippedItemsMain[i].maxUsesPerCombat, newCurCharges);
                //Debug.Log(TeamItemsManager.Instance.equippedItemsMain[i].maxUsesPerCombat - 1);
                //Debug.Log(item1CurUses);
            }
            else if (index == 1)
            {
                go.GetComponent<UIElement>().UpdateContentImage(TeamItemsManager.Instance.equippedItemsSecond[i].itemSpriteItemTab);
                go.GetComponent<UIElement>().UpdateSlider(TeamItemsManager.Instance.equippedItemsSecond[i].maxUsesPerCombat, newCurCharges);
                //Debug.Log(TeamItemsManager.Instance.equippedItemsSecond[i].maxUsesPerCombat - 1);
                //Debug.Log(item2CurUses);
            }
            else if (index == 2)
            {
                go.GetComponent<UIElement>().UpdateContentImage(TeamItemsManager.Instance.equippedItemsThird[i].itemSpriteItemTab);
                go.GetComponent<UIElement>().UpdateSlider(TeamItemsManager.Instance.equippedItemsThird[i].maxUsesPerCombat, newCurCharges);
                //Debug.Log(TeamItemsManager.Instance.equippedItemsThird[i].maxUsesPerCombat - 1);
                //Debug.Log(item3CurUses);
            }


            if (itemIndex == i && !doneOnce)
            {
                doneOnce = true;
                if (index == 0)
                {
                    go.GetComponent<UIElement>().UpdateContentImage(TeamItemsManager.Instance.equippedItemsMain[i].itemSpriteItemTab);
                    go.GetComponent<UIElement>().UpdateSlider(maxCharges, curCharges);
                }
                else if (index == 1)
                {
                    go.GetComponent<UIElement>().UpdateContentImage(TeamItemsManager.Instance.equippedItemsSecond[i].itemSpriteItemTab);
                    go.GetComponent<UIElement>().UpdateSlider(maxCharges, curCharges);
                }
                else if (index == 2)
                {
                    go.GetComponent<UIElement>().UpdateContentImage(TeamItemsManager.Instance.equippedItemsThird[i].itemSpriteItemTab);
                    go.GetComponent<UIElement>().UpdateSlider(maxCharges, curCharges);
                }

                //               continue;
            }
        }
    }

    public void UpdateToolTipGear()
    {
        bool enabled = false;

        int count = 0;
        int index = 0;

        for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
        {
            if (GameManager.Instance.activeRoomHeroes[i] == this)
            {
                if (i == 0)
                {
                    count = OwnedLootInven.Instance.GetWornGearMainAlly().Count;
                    index = 0;
                    break;
                }
                else if (i == 1)
                {
                    count = OwnedLootInven.Instance.GetWornGearSecondAlly().Count;
                    index = 1;
                    break;
                }
                else if (i == 2)
                {
                    count = OwnedLootInven.Instance.GetWornGearThirdAlly().Count;
                    index = 2;
                    break;
                }
            }
        }

        if (count > 0)
        {
            enabled = true;
            tooltipGear.UpdateAlpha(1);
        }
        else
        {
            enabled = false;
            tooltipGear.UpdateAlpha(0);
        }

        // Destroy all existing items
        for (int i = 0; i < tooltipGear.transform.childCount; i++)
        {
            Destroy(tooltipGear.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject go = Instantiate(tooltipGearGO, tooltipGear.transform.position, Quaternion.identity);
            go.transform.SetParent(tooltipGear.transform);

            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;

            if (index == 0)
            {
                // Set gear data
                if (i == 0)
                {
                    for (int c = 0; c < OwnedLootInven.Instance.GetWornGearMainAlly().Count; c++)
                    {
                        if (OwnedLootInven.Instance.GetWornGearMainAlly()[c].curGearType == Slot.SlotType.HELMET)
                            go.GetComponent<UIElement>().UpdateContentImage(OwnedLootInven.Instance.GetWornGearMainAlly()[c].GetSlotImage());
                    }
                }
                if (i == 1)
                {
                    for (int c = 0; c < OwnedLootInven.Instance.GetWornGearMainAlly().Count; c++)
                    {
                        if (OwnedLootInven.Instance.GetWornGearMainAlly()[c].curGearType == Slot.SlotType.CHESTPIECE)
                            go.GetComponent<UIElement>().UpdateContentImage(OwnedLootInven.Instance.GetWornGearMainAlly()[c].GetSlotImage());
                    }
                }
                if (i == 2)
                {
                    for (int c = 0; c < OwnedLootInven.Instance.GetWornGearMainAlly().Count; c++)
                    {
                        if (OwnedLootInven.Instance.GetWornGearMainAlly()[c].curGearType == Slot.SlotType.BOOTS)
                            go.GetComponent<UIElement>().UpdateContentImage(OwnedLootInven.Instance.GetWornGearMainAlly()[c].GetSlotImage());
                    }
                }
            }
            //                            go.GetComponent<UIElement>().UpdateContentImage(TeamGearManager.Instance.clearSlotSprite);
            else if (index == 1)
            {
                // Set gear data
                if (i == 0)
                {
                    for (int c = 0; c < OwnedLootInven.Instance.GetWornGearSecondAlly().Count; c++)
                    {
                        if (OwnedLootInven.Instance.GetWornGearSecondAlly()[c].curGearType == Slot.SlotType.HELMET)
                            go.GetComponent<UIElement>().UpdateContentImage(OwnedLootInven.Instance.GetWornGearSecondAlly()[c].GetSlotImage());
                        //else
                        // go.GetComponent<UIElement>().UpdateContentImage(TeamGearManager.Instance.clearSlotSprite);
                    }
                }
                if (i == 1)
                {
                    for (int c = 0; c < OwnedLootInven.Instance.GetWornGearSecondAlly().Count; c++)
                    {
                        if (OwnedLootInven.Instance.GetWornGearSecondAlly()[c].curGearType == Slot.SlotType.CHESTPIECE)
                            go.GetComponent<UIElement>().UpdateContentImage(OwnedLootInven.Instance.GetWornGearSecondAlly()[c].GetSlotImage());
                        //else
                        // go.GetComponent<UIElement>().UpdateContentImage(TeamGearManager.Instance.clearSlotSprite);
                    }
                }
                if (i == 2)
                {
                    for (int c = 0; c < OwnedLootInven.Instance.GetWornGearSecondAlly().Count; c++)
                    {
                        if (OwnedLootInven.Instance.GetWornGearSecondAlly()[c].curGearType == Slot.SlotType.BOOTS)
                            go.GetComponent<UIElement>().UpdateContentImage(OwnedLootInven.Instance.GetWornGearSecondAlly()[c].GetSlotImage());
                        //else
                        //  go.GetComponent<UIElement>().UpdateContentImage(TeamGearManager.Instance.clearSlotSprite);
                    }
                }
            }
            else if (index == 2)
            {
                // Set gear data
                if (i == 0)
                {
                    for (int c = 0; c < OwnedLootInven.Instance.GetWornGearThirdAlly().Count; c++)
                    {
                        if (OwnedLootInven.Instance.GetWornGearThirdAlly()[c].curGearType == Slot.SlotType.HELMET)
                            go.GetComponent<UIElement>().UpdateContentImage(OwnedLootInven.Instance.GetWornGearThirdAlly()[c].GetSlotImage());
                        //else
                        //go.GetComponent<UIElement>().UpdateContentImage(TeamGearManager.Instance.clearSlotSprite);
                    }
                }
                if (i == 1)
                {
                    for (int c = 0; c < OwnedLootInven.Instance.GetWornGearThirdAlly().Count; c++)
                    {
                        if (OwnedLootInven.Instance.GetWornGearThirdAlly()[c].curGearType == Slot.SlotType.CHESTPIECE)
                            go.GetComponent<UIElement>().UpdateContentImage(OwnedLootInven.Instance.GetWornGearThirdAlly()[c].GetSlotImage());
                        //else
                        // go.GetComponent<UIElement>().UpdateContentImage(TeamGearManager.Instance.clearSlotSprite);
                    }
                }
                if (i == 2)
                {
                    for (int c = 0; c < OwnedLootInven.Instance.GetWornGearThirdAlly().Count; c++)
                    {
                        if (OwnedLootInven.Instance.GetWornGearThirdAlly()[c].curGearType == Slot.SlotType.BOOTS)
                            go.GetComponent<UIElement>().UpdateContentImage(OwnedLootInven.Instance.GetWornGearThirdAlly()[c].GetSlotImage());
                        //else
                        //    go.GetComponent<UIElement>().UpdateContentImage(TeamGearManager.Instance.clearSlotSprite);
                    }
                }
            }
        }
    }

    public void ToggleHeroWeapon(bool toggle = true)
    {
        if (toggle)
            heroWeaponUI.UpdateAlpha(1);
        else
            heroWeaponUI.UpdateAlpha(0);
    }

    public void UpdateStartingMaxHealth(int health)
    {
        startingMaxHealth = health;
    }

    public int GetStartingMaxHealth()
    {
        return startingMaxHealth;
    }

    public void ReloadItemUses()
    {
        // Find all items that trigger on turn start that ally has
        for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
        {
            //Debug.Log("2");
            if (this == GameManager.Instance.activeRoomHeroes[i])
            {
                int index = i;

                if (index == 0)
                {
                    if (OwnedLootInven.Instance.GetWornItemMainAlly().Count > 0)
                    {
                        if (OwnedLootInven.Instance.GetWornItemMainAlly()[0])
                        {
                            if (OwnedLootInven.Instance.GetWornItemMainAlly()[0].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                                item1CurUses = TeamItemsManager.Instance.equippedItemsMain[0].maxUsesPerCombat - OwnedLootInven.Instance.GetWornItemMainAlly()[0].GetItemUseCount();
                        }
                    }
                    if (OwnedLootInven.Instance.GetWornItemMainAlly().Count > 1)
                    {
                        if (OwnedLootInven.Instance.GetWornItemMainAlly()[1])
                        {
                            if (OwnedLootInven.Instance.GetWornItemMainAlly()[1].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                                item2CurUses = TeamItemsManager.Instance.equippedItemsMain[1].maxUsesPerCombat - OwnedLootInven.Instance.GetWornItemMainAlly()[1].GetItemUseCount();
                        }
                    }
                    if (OwnedLootInven.Instance.GetWornItemMainAlly().Count > 2)
                    {
                        if (OwnedLootInven.Instance.GetWornItemMainAlly()[2])
                        {
                            if (OwnedLootInven.Instance.GetWornItemMainAlly()[2].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                                item3CurUses = TeamItemsManager.Instance.equippedItemsMain[2].maxUsesPerCombat - OwnedLootInven.Instance.GetWornItemMainAlly()[2].GetItemUseCount();
                        }
                    }
                }
                else if (index == 1)
                {
                    if (OwnedLootInven.Instance.GetWornItemSecondAlly().Count > 0)
                    {
                        if (OwnedLootInven.Instance.GetWornItemSecondAlly()[0])
                        {
                            if (OwnedLootInven.Instance.GetWornItemSecondAlly()[0].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                                item1CurUses = TeamItemsManager.Instance.equippedItemsSecond[0].maxUsesPerCombat - OwnedLootInven.Instance.GetWornItemSecondAlly()[0].GetItemUseCount();
                        }
                    }
                    if (OwnedLootInven.Instance.GetWornItemSecondAlly().Count > 1)
                    {
                        if (OwnedLootInven.Instance.GetWornItemSecondAlly()[1])
                        {
                            if (OwnedLootInven.Instance.GetWornItemSecondAlly()[1].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                                item2CurUses = TeamItemsManager.Instance.equippedItemsSecond[1].maxUsesPerCombat - OwnedLootInven.Instance.GetWornItemSecondAlly()[1].GetItemUseCount();
                        }
                    }
                    if (OwnedLootInven.Instance.GetWornItemSecondAlly().Count > 2)
                    {
                        if (OwnedLootInven.Instance.GetWornItemSecondAlly()[2])
                        {
                            if (OwnedLootInven.Instance.GetWornItemSecondAlly()[2].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                                item3CurUses = TeamItemsManager.Instance.equippedItemsSecond[2].maxUsesPerCombat - OwnedLootInven.Instance.GetWornItemSecondAlly()[2].GetItemUseCount();
                        }
                    }
                }
                else if (index == 2)
                {
                    if (OwnedLootInven.Instance.GetWornItemThirdAlly().Count > 0)
                    {
                        if (OwnedLootInven.Instance.GetWornItemThirdAlly()[0])
                        {
                            if (OwnedLootInven.Instance.GetWornItemThirdAlly()[0].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                                item1CurUses = TeamItemsManager.Instance.equippedItemsThird[0].maxUsesPerCombat - OwnedLootInven.Instance.GetWornItemThirdAlly()[0].GetItemUseCount();
                        }
                    }
                    if (OwnedLootInven.Instance.GetWornItemThirdAlly().Count > 1)
                    {
                        if (OwnedLootInven.Instance.GetWornItemThirdAlly()[1])
                        {
                            if (OwnedLootInven.Instance.GetWornItemThirdAlly()[1].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                                item2CurUses = TeamItemsManager.Instance.equippedItemsThird[1].maxUsesPerCombat - OwnedLootInven.Instance.GetWornItemThirdAlly()[1].GetItemUseCount();
                        }
                    }
                    if (OwnedLootInven.Instance.GetWornItemThirdAlly().Count > 2)
                    {
                        if (OwnedLootInven.Instance.GetWornItemThirdAlly()[2])
                        {
                            if (OwnedLootInven.Instance.GetWornItemThirdAlly()[2].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                                item3CurUses = TeamItemsManager.Instance.equippedItemsThird[2].maxUsesPerCombat - OwnedLootInven.Instance.GetWornItemThirdAlly()[2].GetItemUseCount();
                        }
                    }
                }
            }
        }
    }
    public void SetItemUsesMax()
    {
        // Find all items that trigger on turn start that ally has
        for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
        {
            //Debug.Log("2");
            if (this == GameManager.Instance.activeRoomHeroes[i])
            {
                int index = i;

                if (index == 0)
                {
                    //Debug.Log(OwnedLootInven.Instance.GetWornGearMainAlly()[index]);
                    if (OwnedLootInven.Instance.GetWornItemMainAlly().Count > 0)
                    {
                        if (OwnedLootInven.Instance.GetWornItemMainAlly()[0])
                        {
                            if (OwnedLootInven.Instance.GetWornItemMainAlly()[0].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.REFILLABLE)
                                item1CurUses = OwnedLootInven.Instance.GetWornItemMainAlly()[0].linkedItemPiece.maxUsesPerCombat;
                            else if (OwnedLootInven.Instance.GetWornItemMainAlly()[0].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE
                                && !OwnedLootInven.Instance.GetWornItemMainAlly()[0].maxSet)
                            {
                                OwnedLootInven.Instance.GetWornItemMainAlly()[0].maxSet = true;
                                item1CurUses = OwnedLootInven.Instance.GetWornItemMainAlly()[0].linkedItemPiece.maxUsesPerCombat;
                            }
                                
                        }
                            
                    }
                    if (OwnedLootInven.Instance.GetWornItemMainAlly().Count > 1)
                    {
                        if (OwnedLootInven.Instance.GetWornItemMainAlly()[1])
                        {
                            if (OwnedLootInven.Instance.GetWornItemMainAlly()[1].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.REFILLABLE)
                                item2CurUses = OwnedLootInven.Instance.GetWornItemMainAlly()[1].linkedItemPiece.maxUsesPerCombat;
                            else if (OwnedLootInven.Instance.GetWornItemMainAlly()[1].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE
                                && !OwnedLootInven.Instance.GetWornItemMainAlly()[1].maxSet)
                            {
                                OwnedLootInven.Instance.GetWornItemMainAlly()[1].maxSet = true;
                                item2CurUses = OwnedLootInven.Instance.GetWornItemMainAlly()[1].linkedItemPiece.maxUsesPerCombat;
                            }
                        }
                    }
                    if (OwnedLootInven.Instance.GetWornItemMainAlly().Count > 2)
                    {
                        if (OwnedLootInven.Instance.GetWornItemMainAlly()[2])
                        {
                            if (OwnedLootInven.Instance.GetWornItemMainAlly()[2].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.REFILLABLE)
                                item3CurUses = OwnedLootInven.Instance.GetWornItemMainAlly()[2].linkedItemPiece.maxUsesPerCombat;
                            else if (OwnedLootInven.Instance.GetWornItemMainAlly()[2].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE
                                && !OwnedLootInven.Instance.GetWornItemMainAlly()[2].maxSet)
                            {
                                OwnedLootInven.Instance.GetWornItemMainAlly()[2].maxSet = true;
                                item3CurUses = OwnedLootInven.Instance.GetWornItemMainAlly()[2].linkedItemPiece.maxUsesPerCombat;
                            }
                        }
                    }
                }
                else if (index == 1)
                {
                    //Debug.Log(OwnedLootInven.Instance.GetWornGearMainAlly()[index]);
                    if (OwnedLootInven.Instance.GetWornItemSecondAlly().Count > 0)
                    {
                        if (OwnedLootInven.Instance.GetWornItemSecondAlly()[0])
                        {
                            if (OwnedLootInven.Instance.GetWornItemSecondAlly()[0].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.REFILLABLE)
                                item1CurUses = OwnedLootInven.Instance.GetWornItemSecondAlly()[0].linkedItemPiece.maxUsesPerCombat;
                            else if (OwnedLootInven.Instance.GetWornItemSecondAlly()[0].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE
                                && !OwnedLootInven.Instance.GetWornItemSecondAlly()[0].maxSet)
                            {
                                OwnedLootInven.Instance.GetWornItemSecondAlly()[0].maxSet = true;
                                item1CurUses = OwnedLootInven.Instance.GetWornItemSecondAlly()[0].linkedItemPiece.maxUsesPerCombat;
                            }
                        }
                    }
                    if (OwnedLootInven.Instance.GetWornItemSecondAlly().Count > 1)
                    {
                        if (OwnedLootInven.Instance.GetWornItemSecondAlly()[1])
                        {
                            if (OwnedLootInven.Instance.GetWornItemSecondAlly()[1].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.REFILLABLE)
                                item2CurUses = OwnedLootInven.Instance.GetWornItemSecondAlly()[1].linkedItemPiece.maxUsesPerCombat;
                            else if (OwnedLootInven.Instance.GetWornItemSecondAlly()[1].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE
                                && !OwnedLootInven.Instance.GetWornItemSecondAlly()[1].maxSet)
                            {
                                OwnedLootInven.Instance.GetWornItemSecondAlly()[1].maxSet = true;
                                item1CurUses = OwnedLootInven.Instance.GetWornItemSecondAlly()[1].linkedItemPiece.maxUsesPerCombat;
                            }
                        }
                    }
                    if (OwnedLootInven.Instance.GetWornItemSecondAlly().Count > 2)
                    {
                        if (OwnedLootInven.Instance.GetWornItemSecondAlly()[2])
                        {
                            if (OwnedLootInven.Instance.GetWornItemSecondAlly()[2].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.REFILLABLE)
                                item3CurUses = OwnedLootInven.Instance.GetWornItemSecondAlly()[2].linkedItemPiece.maxUsesPerCombat;
                            else if (OwnedLootInven.Instance.GetWornItemSecondAlly()[2].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE
                                && !OwnedLootInven.Instance.GetWornItemSecondAlly()[2].maxSet)
                            {
                                OwnedLootInven.Instance.GetWornItemSecondAlly()[2].maxSet = true;
                                item1CurUses = OwnedLootInven.Instance.GetWornItemSecondAlly()[2].linkedItemPiece.maxUsesPerCombat;
                            }
                        }
                    }
                }
                else if (index == 2)
                {
                    //Debug.Log(OwnedLootInven.Instance.GetWornGearMainAlly()[index]);
                    if (OwnedLootInven.Instance.GetWornItemThirdAlly().Count > 0)
                    {
                        if (OwnedLootInven.Instance.GetWornItemThirdAlly()[0])
                        {
                            if (OwnedLootInven.Instance.GetWornItemThirdAlly()[0].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.REFILLABLE)
                                item1CurUses = OwnedLootInven.Instance.GetWornItemThirdAlly()[0].linkedItemPiece.maxUsesPerCombat;
                            else if (OwnedLootInven.Instance.GetWornItemThirdAlly()[0].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE
                                && !OwnedLootInven.Instance.GetWornItemThirdAlly()[0].maxSet)
                            {
                                OwnedLootInven.Instance.GetWornItemThirdAlly()[0].maxSet = true;
                                item1CurUses = OwnedLootInven.Instance.GetWornItemThirdAlly()[0].linkedItemPiece.maxUsesPerCombat;
                            }
                        }
                    }
                    if (OwnedLootInven.Instance.GetWornItemThirdAlly().Count > 1)
                    {
                        if (OwnedLootInven.Instance.GetWornItemThirdAlly()[1])
                        {
                            if (OwnedLootInven.Instance.GetWornItemThirdAlly()[1].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.REFILLABLE)
                                item2CurUses = OwnedLootInven.Instance.GetWornItemThirdAlly()[1].linkedItemPiece.maxUsesPerCombat;
                            else if (OwnedLootInven.Instance.GetWornItemThirdAlly()[1].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE
                                 && !OwnedLootInven.Instance.GetWornItemThirdAlly()[1].maxSet)
                            {
                                OwnedLootInven.Instance.GetWornItemThirdAlly()[1].maxSet = true;
                                item2CurUses = OwnedLootInven.Instance.GetWornItemThirdAlly()[1].linkedItemPiece.maxUsesPerCombat;
                            }
                        }
                    }
                    if (OwnedLootInven.Instance.GetWornItemThirdAlly().Count > 2)
                    {
                        if (OwnedLootInven.Instance.GetWornItemThirdAlly()[2])
                        {
                            if (OwnedLootInven.Instance.GetWornItemThirdAlly()[2].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.REFILLABLE)
                                item3CurUses = OwnedLootInven.Instance.GetWornItemThirdAlly()[2].linkedItemPiece.maxUsesPerCombat;
                            else if (OwnedLootInven.Instance.GetWornItemThirdAlly()[2].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE
                                 && !OwnedLootInven.Instance.GetWornItemThirdAlly()[2].maxSet)
                            {
                                OwnedLootInven.Instance.GetWornItemThirdAlly()[2].maxSet = true;
                                item3CurUses = OwnedLootInven.Instance.GetWornItemThirdAlly()[2].linkedItemPiece.maxUsesPerCombat;
                            }
                        }
                    }
                }
            }
        }
    }

    public void ToggleUnitHitsRemaining(bool hero = true)
    {
        if (hero)
            hitsRemainingText.gameObject.transform.SetParent(heroHitsAccTextPos.gameObject.transform);
        else
            hitsRemainingText.gameObject.transform.SetParent(enemyHitsAccTextPos.gameObject.transform);

        hitsRemainingText.gameObject.transform.localPosition = Vector2.zero;
    }

    public void DecreaseUsesItem1()
    {
        item1CurUses--;
        itemVisualAlert.UpdateContentText((item1CurUses).ToString());

        //if (item1CurUses < 0)
        //    item1CurUses = 0;

        int count = 0;

        int index = 0;

        for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
        {
            if (GameManager.Instance.activeRoomHeroes[i] == this)
            {
                if (i == 0)
                {
                    count = TeamItemsManager.Instance.equippedItemsMain.Count;
                    index = 0;
                    break;
                }
                else if (i == 1)
                {
                    count = TeamItemsManager.Instance.equippedItemsSecond.Count;
                    index = 1;
                    break;
                }
                else if (i == 2)
                {
                    count = TeamItemsManager.Instance.equippedItemsThird.Count;
                    index = 2;
                    break;
                }
            }
        }

        if (index == 0)
            UpdateTooltipItems(TeamItemsManager.Instance.equippedItemsMain[0].maxUsesPerCombat, item1CurUses, 0);
        else if (index == 1)
            UpdateTooltipItems(TeamItemsManager.Instance.equippedItemsSecond[0].maxUsesPerCombat, item1CurUses, 0);
        else if (index == 2)
            UpdateTooltipItems(TeamItemsManager.Instance.equippedItemsThird[0].maxUsesPerCombat, item1CurUses, 0);
    }
    public void DecreaseUsesItem2()
    {
        item2CurUses--;
        itemVisualAlert.UpdateContentText((item2CurUses).ToString());


        int count = 0;

        int index = 0;

        //Debug.Log("decreasing item 2");

        for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
        {
            if (GameManager.Instance.activeRoomHeroes[i] == this)
            {
                if (i == 0)
                {
                    count = TeamItemsManager.Instance.equippedItemsMain.Count;
                    index = 0;
                    break;
                }
                else if (i == 1)
                {
                    count = TeamItemsManager.Instance.equippedItemsSecond.Count;
                    index = 1;
                    break;
                }
                else if (i == 2)
                {
                    count = TeamItemsManager.Instance.equippedItemsThird.Count;
                    index = 2;
                    break;
                }
            }
        }

        if (index == 0)
            UpdateTooltipItems(TeamItemsManager.Instance.equippedItemsMain[1].maxUsesPerCombat, item2CurUses, 1);
        else if (index == 1)
            UpdateTooltipItems(TeamItemsManager.Instance.equippedItemsSecond[1].maxUsesPerCombat, item2CurUses, 1);
        else if (index == 2)
            UpdateTooltipItems(TeamItemsManager.Instance.equippedItemsThird[1].maxUsesPerCombat, item2CurUses, 1);
    }

    public void DecreaseUsesItem3()
    {
        item3CurUses--;
        itemVisualAlert.UpdateContentText((item3CurUses).ToString());

        int count = 0;
        int index = 0;

        for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
        {
            if (GameManager.Instance.activeRoomHeroes[i] == this)
            {
                if (i == 0)
                {
                    count = TeamItemsManager.Instance.equippedItemsMain.Count;
                    index = 0;
                    break;
                }
                else if (i == 1)
                {
                    count = TeamItemsManager.Instance.equippedItemsSecond.Count;
                    index = 1;
                    break;
                }
                else if (i == 2)
                {
                    count = TeamItemsManager.Instance.equippedItemsThird.Count;
                    index = 2;
                    break;
                }
            }
        }

        if (index == 0)
            UpdateTooltipItems(TeamItemsManager.Instance.equippedItemsMain[2].maxUsesPerCombat, item3CurUses, 2);
        else if (index == 1)
            UpdateTooltipItems(TeamItemsManager.Instance.equippedItemsSecond[2].maxUsesPerCombat, item3CurUses, 2);
        else if (index == 2)
            UpdateTooltipItems(TeamItemsManager.Instance.equippedItemsThird[2].maxUsesPerCombat, item3CurUses, 2);
    }

    public void TriggerItemVisualAlert(Sprite sprite, bool triggered = true, bool activeItem = false)
    {
        itemVisualAlert.UpdateContentImage(sprite);

        if (triggered)
        {
            if (activeItem)
                itemVisualAlert.UpdateContentText("");

            AudioManager.Instance.Play("SFX_ItemTrigger");
        }

        else
            itemVisualAlert.UpdateContentText("");

        itemVisualAlert.UpdateAlpha(1, false, 0, false, false);
    }

    public void ToggleUnitBottomStats(bool toggle)
    {
        ToggleUnitHealthBar(toggle);
        ToggleUnitAttackBar(toggle);
        ToggleActionNextBar(toggle);
        ToggleUnitLevelImage(toggle);
    }

    public void UpdateSorting(int newSortingVal)
    {
        visualCanvas.sortingOrder = newSortingVal;
    }

    public void ToggleSelectUnitButton(bool toggle)
    {
        //Debug.Log("disabling123");
        selectUnitButton.ToggleButton(toggle);
    }


    public void ToggleTooltipStats(bool toggle)
    {
        //Debug.Log("toggling " + toggle);

        if (toggle)
        {
            //Debug.Log(tooltipStats.isEnabled);

            if (!tooltipStats.isEnabled)
            {
                UpdateTooltipStats();
                tooltipStats.UpdateAlpha(1);

                //if (curUnitType == UnitType.PLAYER)
                //{
                int count = 0;

                int index = 0;


                for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
                {
                    if (GameManager.Instance.activeRoomHeroes[i] == this)
                    {
                        if (i == 0)
                        {
                            count = TeamItemsManager.Instance.equippedItemsMain.Count;
                            index = 0;
                            break;
                        }
                        else if (i == 1)
                        {
                            count = TeamItemsManager.Instance.equippedItemsSecond.Count;
                            index = 1;
                            break;
                        }
                        else if (i == 2)
                        {
                            count = TeamItemsManager.Instance.equippedItemsThird.Count;
                            index = 2;
                            break;
                        }
                    }
                }

                // see if this works 
                for (int x = 0; x < count; x++)
                {
                    if (x == 0)
                    {
                        if (index == 0 && TeamItemsManager.Instance.equippedItemsMain.Count > x)
                            UpdateTooltipItems(TeamItemsManager.Instance.equippedItemsMain[x].maxUsesPerCombat, item1CurUses, 0);
                        else if (index == 1 && TeamItemsManager.Instance.equippedItemsSecond.Count > x)
                            UpdateTooltipItems(TeamItemsManager.Instance.equippedItemsSecond[x].maxUsesPerCombat, item1CurUses, 0);
                        else if (index == 2 && TeamItemsManager.Instance.equippedItemsThird.Count > x)
                            UpdateTooltipItems(TeamItemsManager.Instance.equippedItemsThird[x].maxUsesPerCombat, item1CurUses, 0);
                    }
                    else if (x == 1)
                    {
                        if (index == 0 && TeamItemsManager.Instance.equippedItemsMain.Count > x)
                            UpdateTooltipItems(TeamItemsManager.Instance.equippedItemsMain[x].maxUsesPerCombat, item2CurUses, 1);
                        else if (index == 1 && TeamItemsManager.Instance.equippedItemsSecond.Count > x)
                            UpdateTooltipItems(TeamItemsManager.Instance.equippedItemsSecond[x].maxUsesPerCombat, item2CurUses, 1);
                        else if (index == 2 && TeamItemsManager.Instance.equippedItemsThird.Count > x)
                            UpdateTooltipItems(TeamItemsManager.Instance.equippedItemsThird[x].maxUsesPerCombat, item2CurUses, 1);
                    }
                    else if (x == 2)
                    {
                        if (index == 0 && TeamItemsManager.Instance.equippedItemsMain.Count > x)
                            UpdateTooltipItems(TeamItemsManager.Instance.equippedItemsMain[x].maxUsesPerCombat, item3CurUses, 2);
                        else if (index == 1 && TeamItemsManager.Instance.equippedItemsSecond.Count > x)
                            UpdateTooltipItems(TeamItemsManager.Instance.equippedItemsSecond[x].maxUsesPerCombat, item3CurUses, 2);
                        else if (index == 2 && TeamItemsManager.Instance.equippedItemsThird.Count > x)
                            UpdateTooltipItems(TeamItemsManager.Instance.equippedItemsThird[x].maxUsesPerCombat, item3CurUses, 2);
                    }
                }

                UpdateToolTipGear();

                if (count == 0)
                    tooltipItems.UpdateAlpha(0);
            }
        }
        else
            tooltipStats.UpdateAlpha(0);
    }

    public void ToggleTooltipEffect(bool toggle, string effectSelectedName = null)
    {
        if (toggle)
        {
            if (!tooltipEffect.isEnabled)
            {
                //Debug.Log("enabling");
                tooltipEffect.isEnabled = true;

                tooltipEffect.UpdateAlpha(1);
                UpdateTooltipEffect(effectSelectedName);
            }
        }
        else
        {
            //Debug.Log("disabling");
            tooltipEffect.UpdateAlpha(0);
        }
    }

    public void UpdateTooltipStats()
    {
        statHealth.UpdateContentText(GetUnitCurHealth().ToString());
        statPower.UpdateContentText(curPower.ToString());
        statHealingPower.UpdateContentText(GetUnitHealingHits().ToString());
        statDefense.UpdateContentText(GetCurDefense().ToString());
        statSpeed.UpdateContentText(GetUnitSpeed().ToString());
    }

    public void UpdateTooltipEffect(string effectSelectedName)
    {
        for (int i = 0; i < activeEffects.Count; i++)
        {
            if (effectSelectedName == activeEffects[i].effectName)
            {
                string newName = activeEffects[i].effectName;
                if (activeEffects[i].effectName == "HOLY_LINK")
                    newName = "HOLY LINK";
                else if (activeEffects[i].effectName == "OTHER_LINK")
                    newName = "OTHER LINK";
                else if (activeEffects[i].effectName == "POWERDOWN")
                    newName = "POWER DOWN";
                tooltipEffect.UpdateContentText(newName);
                tooltipEffect.UpdateContentSubTextTMP(activeEffects[i].effectDesc);
                tooltipEffect.UpdateContentTextColourTMP(activeEffects[i].titleTextColour);
            }
        }
    }

    public void UpdateHitsRemainingText(int remaining)
    {
        hitsRemainingText.UpdateContentText(remaining.ToString());
        hitsRemainingText.AnimateUI();
    }

    public void ToggleHitsRemainingText(bool toggle)
    {
        if (toggle)
        {
            hitsRemainingText.UpdateAlpha(1);
        }
        else
        {
            hitsRemainingText.UpdateAlpha(0);
        }
    }

    public void UpdateSpeed(float newVal)
    {
        curSpeed += (int)newVal;
    }
    public void UpdatePower(float newVal)
    {
        curPower += (int)newVal;
    }
    public void UpdateHealingPower(int newPower, bool setting = true, bool adding = true)
    {
        if (adding)
        {
            if (setting)
                curHealingPower = newPower;
            else
                curHealingPower += newPower;
        }
        else
        {
            if (setting)
                curHealingPower = newPower;
            else
                curHealingPower -= newPower;
        }
    }
    public void UpdateDefense(float newVal)
    {
        curDefense += newVal;
    }
    public void UpdateMaxHealth(float newVal)
    {
        curMaxHealth += (int)newVal;
    }

    public int GetSpeedIncPerLv()
    {
        return curSpeedIncPerLv;
    }
    public void UpdateSpeedIncPerLv(int newVal)
    {
        curSpeedIncPerLv += newVal;
    }

    public int GetPowerIncPerLv()
    {
        return curPowerIncPerLv;
    }
    public void UpdatePowerIncPerLv(int newVal)
    {
        curPowerIncPerLv += newVal;
    }

    public int GetHealingPowerIncPerLv()
    {
        return curHealingPowerIncPerLv;
    }
    public void UpdateHealingPowerIncPerLv(int newVal)
    {
        curHealingPowerIncPerLv += newVal;
    }

    public float GetDefenseIncPerLv()
    {
        return curDefenseIncPerLv;
    }
    public void UpdateDefenseIncPerLv(float newVal)
    {
        curDefenseIncPerLv += newVal;
    }

    public int GetMaxHealthIncPerLv()
    {
        return maxHealthIncPerLv;
    }
    public void UpdateMaxHealthIncPerLv(int newVal)
    {
        maxHealthIncPerLv += newVal;
    }
    public void StartFocusUnit()
    {
        unitFocus.StartFocusTransition();
    }

    public void IncCooldownReducBonus()
    {
        reducedCooldownsCount++;
    }

    public void RerollItemCount()
    {
        rerollItemCount++;
    }

    public int GetCooldownReducBonus()
    {
        return reducedCooldownsCount;
    }

    public int GetRerollItemCount()
    {
        return rerollItemCount;
    }

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // If player is holding down on unit, display stats
    }

    public void ResetUnitSkillOrder()
    {
        //GetSkillBaseSlot(0).
    }

    private void Start()
    {
        ToggleUnitExpVisual(false);
        ToggleUnitBG(false);
        ResetEffects(0);
        ToggleHitsRemainingText(false);
        ToggleHeroWeapon(false);

        //UpdateUnitPowerInc(1);
        //sUpdateUnitHealingPowerInc(1);

        if (!heroRoomUnit)
        {
            //Debug.Log("ddd");
            UpdateIsVisible(true);
        }

        UpdateUnitLevelImage();
        ToggleUnitLevelImage(true);
        //ToggleIsPoisonLeaching(false);

        SetupFlashHit();
    }

    void SetupFlashHit()
    {
        hitFlash = GetComponentInChildren<SimpleFlash>();

        if (hitFlash != null)
            uiElement = hitFlash.gameObject.GetComponent<UIElement>();
    }

    public SimpleFlash GetHitFlash()
    {
        return hitFlash;
    }
    public void ToggleUnitPoisoned(bool toggle)
    {
        isPoison = toggle;
    }

    public bool GetUnitPoisoned()
    {
        return isPoison;
    }

    public float GetUnitHealingRecieved()
    {
        return curHealingRecieved;
    }

    public void UpdateUnitHealingRecieved(float newVal, bool set = false)
    {
        //curHealingRecieved = newVal;
        if (!set)
            curHealingRecieved += newVal;
        else
            curHealingRecieved = newVal;

        if (curHealingRecieved < 0)
            curHealingRecieved = 0;
    }

    public void ResetUnitHealingRecieved(bool set = true)
    {
        UpdateUnitHealingRecieved(1, set);
    }

    public void ToggleUnitLevelImage(bool toggle)
    {
        UpdateUnitLevelImage();

        if (unitLevelImage == null)
            return;

        if (toggle)
            unitLevelImage.UpdateAlpha(1);
        else
            unitLevelImage.UpdateAlpha(0);
    }

    public void UpdateUnitLevelColour(Color color)
    {
        unitLevelImage.UpdateContentSubTextTMPColour(color);
    }

    public void UpdateUnitLevelImage()
    {
        if (unitLevelImage == null)
            return;

        unitLevelImage.UpdateContentSubTextTMP(curLevel.ToString());
    }

    public LastOpenedMastery GetLastOpenedMastery()
    {
        return lastOpenedStatPage;
    }

    public void UpdateFacingDirection(bool right = true)
    {
        if (right)
            gameObject.transform.localScale = new Vector3(-1, gameObject.transform.localScale.y);
        else
            gameObject.transform.localScale = new Vector3(1, gameObject.transform.localScale.y);
    }

    public void UpdateIsVisible(bool toggle)
    {
        isVisible = toggle;

        if (curUnitType == UnitType.PLAYER)
        {
            //Debug.Log(toggle);
            if (unitUIElement == null)
                return;

            if (isVisible)
                unitUIElement.UpdateAlpha(1);
            else
                unitUIElement.UpdateAlpha(0);
        }
    }

    public bool GetIsVisible()
    {
        return isVisible;
    }
    public void AddOwnedItems(ItemPiece item)
    {
        equiptItems.Add(item);
    }

    public List<ItemPiece> GetEquipItems()
    {
        return equiptItems;
    }

    public void UpdateCurrentSkills(List<SkillData> skillBaseSlots)
    {
        this.curSkillBaseSlots = skillBaseSlots;
    }

    public void UpdateUnitSkills(List<SkillData> skills)
    {
        //this.skills = skills;

        /*
        this.skills.Clear();
        for (int i = 0; i < 4; i++)
        {
            if (skills[i].originalIndex == 0)
                skills.Insert(0, skills[i]);
            else if (skills[i].originalIndex == 1)
                skills.Insert(1, skills[i]);
            else if (skills[i].originalIndex == 2)
                skills.Insert(2, skills[i]);
            else if (skills[i].originalIndex == 3)
                skills.Insert(3, skills[i]);
        }
        */

        this.skills = skills;
    }

    public ItemPiece GetBaseSelectedItem()
    {
        for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
        {
            if (GameManager.Instance.activeRoomHeroes[i] == this)
            {
                if (i == 0)
                {
                    if (TeamItemsManager.Instance.equippedItemsMain.Count > 0)
                    {
                        i = 10;
                        return TeamItemsManager.Instance.equippedItemsMain[0];
                    }
                }
                else if (i == 1)
                {
                    if (TeamItemsManager.Instance.equippedItemsSecond.Count > 0)
                    {
                        i = 10;
                        return TeamItemsManager.Instance.equippedItemsSecond[0];
                    }
                }
                else if (i == 2)
                {
                    if (TeamItemsManager.Instance.equippedItemsThird.Count > 0)
                    {
                        i = 10;
                        return TeamItemsManager.Instance.equippedItemsThird[0];
                    }
                }
            }
        }

        //Debug.Log("returning null! - unitfunctionality");
        return null;
    }

    public SkillData GetBaseSelectSkill()
    {
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].skillCooldown == 0)
            {
                return skills[i];
            }
        }

        return skills[0];
    }

    public SkillData GetSkill(int count)
    {
        if (skills.Count == 0)
            return null;
        else
            return skills[count];
    }

    public List<SkillData> GetAllSkills()
    {
        return skills;
    }

    public ItemPiece GetItem(int count)
    {
        if (GetEquipItems().Count == 0)
            return null;
        else
            return GetEquipItems()[count];
    }

    public List<ItemPiece> GetAllItems()
    {
        return GetEquipItems();
    }

    public void ClearSkillBaseSlots()
    {
        curSkillBaseSlots.Clear();
    }

    public int GetEquipItemCount(string itemName)
    {
        int amountOfItems = 0;

        for (int i = 0; i < GetEquipItems().Count; i++)
        {
            if (GetEquipItems()[i].itemName == itemName)
            {
                amountOfItems++;
            }
        }

        return amountOfItems;
    }

    public void SetPositionAndParent(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = new Vector3(0, 0, 0);
    }

    public void ToggleIsPoisonLeaching(bool toggle)
    {
        isPoisonLeaching = toggle;
    }

    public void TriggerTextAlert(string name, float alpha, bool effect, string gradient = null, bool levelUp = false, bool skill = false)
    {
        if (skill)
        {
            unitAlertStay.UpdateContentTextColour(GameManager.Instance.gradientSkillAlert);

            unitAlertStay.UpdateContentText(name);
            unitAlertStay.UpdateAlpha(alpha, false, 0, true);
        }

        ToggleTextAlert(true);

        if (alpha != 0 && !skill)
        {
            if (levelUp)
                unitAlertTextParent.UpdateContentTextColour(GameManager.Instance.gradientLevelUpAlert);
            else if (!levelUp && !skill)
            {
                GameObject go = Instantiate(unitAlertText, unitAlertTextParent.transform.position, Quaternion.identity);
                go.transform.SetParent(unitAlertTextParent.transform);
                go.transform.localPosition = new Vector3(0, 0, 0);
                go.transform.localScale = new Vector3(0.78f, 0.78f, 1);

                UnitAlertText unitAlertTextScript = go.GetComponent<UnitAlertText>();
                unitAlertTextScript.AllowMovingUp(name, alpha, effect, gradient);
            }
        }
    }

    public void ToggleIdleBattle(bool toggle)
    {
        idleBattle = toggle;
    }

    public bool GetIdleBattle()
    {
        return idleBattle;
    }

    public Animator GetAnimator()
    {
        return animator;
    }
    public void UpdateEffectVisualAnimator(RuntimeAnimatorController ac)
    {
        effectDisplayAnimator.runtimeAnimatorController = ac;

        effectDisplayAnimator.SetTrigger("animate");
        //UpdateIconSize();
        //StartWalkAnim();
    }

    public int GetSpentSkillPoints()
    {
        return spentMasteryTotalPoints;
    }

    public void UpdateSpentSkillPoints(int add)
    {
        spentMasteryTotalPoints += add;
    }

    public void ResetSpentStatPoints()
    {
        spentMasteryTotalPoints = 0;
        SkillSpentPoints = 0;
        statsSpentAdvPoints = 0;
    }

    public void ToggleHideEffects(bool toggle)
    {
        if (!toggle)
            effects.UpdateAlpha(1);
        else
            effects.UpdateAlpha(0);
    }

    public IEnumerator StartUnitTurn()
    {
        yield return new WaitForSeconds(GameManager.Instance.enemyEffectWaitTime);

        /*
        // Do unit's turn automatically if its on idle battle
        if (isDead)
        {
            GameManager.Instance.UpdateTurnOrder();
            yield break;
        }
        */

        if (GetIdleBattle() && GameManager.Instance.activeRoomHeroes.Count >= 1 && !isDead)
        {
            yield return new WaitForSeconds(GameManager.Instance.enemySkillThinkTime);

            // If unit has energy to choose a skill, choose one
            GameManager.Instance.UpdateActiveSkill(ChooseRandomSkill());

            // Trigger current unit's turn energy count to deplete for skill use
            //GameManager.Instance.UpdateActiveUnitEnergyBar(true, false, GameManager.Instance.activeSkill.skillEnergyCost, true);
            //GameManager.Instance.UpdateActiveUnitHealthBar(false);

            // Select units
            GameManager.Instance.UpdateUnitSelection(GameManager.Instance.activeSkill);

            TriggerTextAlert(GameManager.Instance.GetActiveSkill().skillName, 1, false, null, false, true);

            /*
            if (GameManager.Instance.GetActiveSkill().curAnimType == SkillData.SkillAnimType.DEFAULT)
            {
                animator.SetTrigger("AttackFlg");
            }
            else if (GameManager.Instance.GetActiveSkill().curAnimType == SkillData.SkillAnimType.SKILL)
            {
                animator.SetTrigger("SkillFlg");
            }
            */

            // Adjust power based on skill effect amp on target then send it 

            WeaponManager.Instance.SetEnemyWeapon(this, true);


            /*
            // End turn 
            GameManager.Instance.ToggleEndTurnButton(false);
            GameManager.Instance.UpdateTurnOrder();
            yield break;
            */
        }
    }

    public void StartEnemyAttack()
    {
        int totalPower = GameManager.Instance.activeSkill.GetCalculatedSkillPowerStat() + GameManager.Instance.GetActiveUnitFunctionality().curPower;

        //totalPower += //GameManager.Instance.randomBaseOffset*2;
        totalPower = GameManager.Instance.RandomisePower(totalPower);

        if (GameManager.Instance.activeSkill.curSkillPower == 0)
            totalPower = 0;

        int effectCount;


        if (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE)
            effectCount = GameManager.Instance.GetActiveSkill().baseEffectApplyCount + GetUnitPowerHits();
        else
            effectCount = GameManager.Instance.GetActiveSkill().baseEffectApplyCount + GetUnitHealingHits();


        int skillAttackCount;

        if (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE)
            skillAttackCount = GameManager.Instance.GetActiveSkill().GetCalculatedSkillHitAmount() + GetUnitPowerHits();
        else
            skillAttackCount = GameManager.Instance.GetActiveSkill().GetCalculatedSkillHitAmount() + GetUnitHealingHits();

        StartCoroutine(GameManager.Instance.WeaponAttackCommand(totalPower, skillAttackCount, effectCount));

    }
    public void DecreaseRandomNegativeEffect()
    {
        //Debug.Log("Attempting to remove effect");

        if (activeEffects.Count > 0)
        {
            int offensiveEffectCount = 0;

            for (int i = 0; i < activeEffects.Count; i++)
            {
                if (activeEffects[i].curEffectBenefitType == Effect.EffectBenefitType.DEBUFF)
                {
                    offensiveEffectCount++;
                }
            }

            int rand = Random.Range(0, activeEffects.Count);

            if (offensiveEffectCount > 0)
            {
                if (activeEffects[rand].curEffectBenefitType == Effect.EffectBenefitType.DEBUFF)
                {
                    string name = activeEffects[rand].effectName;

                    TriggerTextAlert(name, 1, true, "Trigger");
                    activeEffects[rand].ReduceTurnCountText(this);
                }
                else
                    DecreaseRandomNegativeEffect();
            }
        }
    }
    bool CheckIfItemSucceeds()
    {
        int rand = Random.Range(0, 2);

        /*
        if (rand == 0)
            Debug.Log("Item fails");
        else
            Debug.Log("Item Succeeds");


        if (rand == 0)
            return false;
        else
        */
        return true;
    }

    public IEnumerator TriggerItems(bool turnStart = false, bool turnEnd = false, bool skillAtack = false, bool alliesAttacked = false, bool enemiesHealed = false, bool selfAttacked = false)
    {
        //yield return new WaitForSeconds(.35f);

        if (turnStart)
        {
            // Find all items that trigger on turn start that ally has
            for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
            {
                //Debug.Log("2");
                if (this == GameManager.Instance.activeRoomHeroes[i])
                {
                    //Debug.Log("3");
                    if (turnStart)
                    {
                        //Debug.Log("4");
                        if (i == 0)
                        {
                            //Debug.Log("5");
                            for (int x = 0; x < TeamItemsManager.Instance.equippedItemsMain.Count; x++)
                            {
                                //Debug.Log("6");
                                if (TeamItemsManager.Instance.equippedItemsMain[x].activeOnTurnStart)
                                {
                                    // Covers small and big potion
                                    if (TeamItemsManager.Instance.equippedItemsMain[x].isBaseHealing)
                                    {
                                        // Check if unit is low enough health to trigger item
                                        if (((GetUnitCurHealth() / GetUnitMaxHealth()) * 100) <= TeamItemsManager.Instance.equippedItemsMain[x].threshHoldAmount)
                                        {
                                            if (x == 0 && item1CurUses >= 0)
                                            {
                                                DecreaseUsesItem1();
                                                if (item1CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 1 && item2CurUses >= 0)
                                            {
                                                DecreaseUsesItem2();
                                                if (item2CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 2 && item3CurUses >= 0)
                                            {
                                                DecreaseUsesItem3();
                                                if (item3CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }

                                            if (x == 0 && item1CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 1 && item2CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 2 && item3CurUses <= -1)
                                            {
                                                continue;
                                            }

                                            ItemPiece item = TeamItemsManager.Instance.equippedItemsMain[x];
                                            float healAmount = ((float)item.itemPower / 100f) * startingRoomMaxHealth;
                                            UpdateUnitCurHealth((int)healAmount, false, false, true, false, true);
                                            //StartCoroutine(SpawnPowerUI((int)healAmount, false, false, null, false));
                                            TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteItemTab);
                                            //AudioManager.Instance.Play("SFX_ItemTrigger");
                                            yield return new WaitForSeconds(.35f);
                                            continue;
                                        }
                                    }
                                    else if (TeamItemsManager.Instance.equippedItemsMain[x].effectAdded == EffectManager.instance.GetEffect("POWERUP"))
                                    {
                                        if (CheckIfItemSucceeds())
                                        {
                                            if (x == 0 && item1CurUses >= 0)
                                            {
                                                DecreaseUsesItem1();
                                                if (item1CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 1 && item2CurUses >= 0)
                                            {
                                                DecreaseUsesItem2();
                                                if (item2CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 2 && item3CurUses >= 0)
                                            {
                                                DecreaseUsesItem3();
                                                if (item3CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }

                                            if (x == 0 && item1CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 1 && item2CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 2 && item3CurUses <= -1)
                                            {
                                                continue;
                                            }

                                            EffectData effect = EffectManager.instance.GetEffect("POWERUP");
                                            AddUnitEffect(effect, this, 1, 1, false, true);
                                            TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteItemTab);
                                            //AudioManager.Instance.Play("SFX_ItemTrigger");
                                            yield return new WaitForSeconds(.35f);
                                            continue;
                                        }
                                        else
                                        {
                                            /*
                                            if (x == 0 && item1CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            if (x == 1 && item2CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            if (x == 2 && item3CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            */
                                        }
                                    }
                                    else if (TeamItemsManager.Instance.equippedItemsMain[x].effectAdded == EffectManager.instance.GetEffect("HEALTH UP"))
                                    {
                                        if (CheckIfItemSucceeds())
                                        {
                                            if (x == 0 && item1CurUses >= 0)
                                            {
                                                DecreaseUsesItem1();
                                                if (item1CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 1 && item2CurUses >= 0)
                                            {
                                                DecreaseUsesItem2();
                                                if (item2CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 2 && item3CurUses >= 0)
                                            {
                                                DecreaseUsesItem3();
                                                if (item3CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }

                                            if (x == 0 && item1CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 1 && item2CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 2 && item3CurUses <= -1)
                                            {
                                                continue;
                                            }

                                            EffectData effect = EffectManager.instance.GetEffect("HEALTH UP");
                                            AddUnitEffect(effect, this, 1, 1, false, true);
                                            TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteItemTab);
                                            //AudioManager.Instance.Play("SFX_ItemTrigger");
                                            yield return new WaitForSeconds(.35f);
                                            continue;
                                        }
                                        else
                                        {
                                            /*
                                            if (x == 0 && item1CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            if (x == 1 && item2CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            if (x == 2 && item3CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            */
                                        }
                                    }
                                    else if (TeamItemsManager.Instance.equippedItemsMain[x].effectAdded == EffectManager.instance.GetEffect("SPEED UP"))
                                    {
                                        if (CheckIfItemSucceeds())
                                        {
                                            if (x == 0 && item1CurUses >= 0)
                                            {
                                                DecreaseUsesItem1();
                                                if (item1CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 1 && item2CurUses >= 0)
                                            {
                                                DecreaseUsesItem2();
                                                if (item2CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 2 && item3CurUses >= 0)
                                            {
                                                DecreaseUsesItem3();
                                                if (item3CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }

                                            if (x == 0 && item1CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 1 && item2CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 2 && item3CurUses <= -1)
                                            {
                                                continue;
                                            }

                                            EffectData effect = EffectManager.instance.GetEffect("SPEED UP");
                                            AddUnitEffect(effect, this, 1, 1, false, true);
                                            TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteItemTab);


                                            yield return new WaitForSeconds(.35f);
                                            continue;
                                        }
                                        else
                                        {
                                            /*
                                            if (x == 0 && item1CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            if (x == 1 && item2CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            if (x == 2 && item3CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            */
                                        }
                                    }
                                    else if (TeamItemsManager.Instance.equippedItemsMain[x].effectAdded == EffectManager.instance.GetEffect("DEFENSE UP"))
                                    {
                                        if (CheckIfItemSucceeds())
                                        {
                                            if (x == 0 && item1CurUses >= 0)
                                            {
                                                DecreaseUsesItem1();
                                                if (item1CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 1 && item2CurUses >= 0)
                                            {
                                                DecreaseUsesItem2();
                                                if (item2CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 2 && item3CurUses >= 0)
                                            {
                                                DecreaseUsesItem3();
                                                if (item3CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }

                                            if (x == 0 && item1CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 1 && item2CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 2 && item3CurUses <= -1)
                                            {
                                                continue;
                                            }

                                            EffectData effect = EffectManager.instance.GetEffect("DEFENSE UP");
                                            AddUnitEffect(effect, this, 1, 1, false, true);
                                            TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteItemTab);
                                            //AudioManager.Instance.Play("SFX_ItemTrigger");
                                            yield return new WaitForSeconds(.35f);
                                            continue;
                                        }
                                        else
                                        {
                                            /*
                                            if (x == 0 && item1CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            if (x == 1 && item2CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            if (x == 2 && item3CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            */
                                        }
                                    }

                                    yield return new WaitForSeconds(.35f);
                                }
                            }
                        }
                        else if (i == 1)
                        {
                            //Debug.Log("5");
                            for (int x = 0; x < TeamItemsManager.Instance.equippedItemsSecond.Count; x++)
                            {
                                //Debug.Log("6");
                                if (TeamItemsManager.Instance.equippedItemsSecond[x].activeOnTurnStart)
                                {
                                    // Covers small and big potion
                                    if (TeamItemsManager.Instance.equippedItemsSecond[x].isBaseHealing)
                                    {
                                        // Check if unit is low enough health to trigger item
                                        if (((GetUnitCurHealth() / GetUnitMaxHealth()) * 100) <= TeamItemsManager.Instance.equippedItemsSecond[x].threshHoldAmount)
                                        {
                                            if (x == 0 && item1CurUses >= 0)
                                            {
                                                DecreaseUsesItem1();
                                                if (item1CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 1 && item2CurUses >= 0)
                                            {
                                                DecreaseUsesItem2();
                                                if (item2CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 2 && item3CurUses >= 0)
                                            {
                                                DecreaseUsesItem3();
                                                if (item3CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            if (x
                                                == 0 && item1CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 1 && item2CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 2 && item3CurUses <= -1)
                                            {
                                                continue;
                                            }

                                            ItemPiece item = TeamItemsManager.Instance.equippedItemsSecond[x];
                                            float healAmount = ((float)item.itemPower / 100f) * GetUnitMaxHealth();
                                            UpdateUnitCurHealth((int)healAmount, false, false, true, false, true);
                                            //StartCoroutine(SpawnPowerUI((int)healAmount, false, false, null, false));
                                            TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsSecond[x].itemSpriteItemTab);
                                            //AudioManager.Instance.Play("SFX_ItemTrigger");
                                            yield return new WaitForSeconds(.35f);
                                            continue;
                                        }
                                    }
                                    else if (TeamItemsManager.Instance.equippedItemsSecond[x].effectAdded == EffectManager.instance.GetEffect("POWERUP"))
                                    {
                                        if (CheckIfItemSucceeds())
                                        {
                                            if (x == 0 && item1CurUses >= 0)
                                            {
                                                DecreaseUsesItem1();
                                                if (item1CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 1 && item2CurUses >= 0)
                                            {
                                                DecreaseUsesItem2();
                                                if (item2CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 2 && item3CurUses >= 0)
                                            {
                                                DecreaseUsesItem3();
                                                if (item3CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }

                                            if (x == 0 && item1CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 1 && item2CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 2 && item3CurUses <= -1)
                                            {
                                                continue;
                                            }

                                            EffectData effect = EffectManager.instance.GetEffect("POWERUP");
                                            AddUnitEffect(effect, this, 1, 1, false, true);
                                            TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsSecond[x].itemSpriteItemTab);
                                            //AudioManager.Instance.Play("SFX_ItemTrigger");
                                            yield return new WaitForSeconds(.35f);
                                            continue;
                                        }
                                        else
                                        {
                                            /*
                                            if (x == 0 && item1CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            if (x == 1 && item2CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            if (x == 2 && item3CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            */
                                        }
                                    }
                                    else if (TeamItemsManager.Instance.equippedItemsSecond[x].effectAdded == EffectManager.instance.GetEffect("HEALTH UP"))
                                    {
                                        if (CheckIfItemSucceeds())
                                        {
                                            if (x == 0 && item1CurUses >= 0)
                                            {
                                                DecreaseUsesItem1();
                                                if (item1CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 1 && item2CurUses >= 0)
                                            {
                                                DecreaseUsesItem2();
                                                if (item2CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 2 && item3CurUses >= 0)
                                            {
                                                DecreaseUsesItem3();
                                                if (item3CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }

                                            if (x == 0 && item1CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 1 && item2CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 2 && item3CurUses <= -1)
                                            {
                                                continue;
                                            }

                                            EffectData effect = EffectManager.instance.GetEffect("HEALTH UP");
                                            AddUnitEffect(effect, this, 1, 1, false, true);
                                            TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsSecond[x].itemSpriteItemTab);
                                            //AudioManager.Instance.Play("SFX_ItemTrigger");
                                            yield return new WaitForSeconds(.35f);
                                            continue;
                                        }
                                        else
                                        {
                                            /*
                                            if (x == 0 && item1CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            if (x == 1 && item2CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            if (x == 2 && item3CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            */
                                        }
                                    }
                                    else if (TeamItemsManager.Instance.equippedItemsSecond[x].effectAdded == EffectManager.instance.GetEffect("SPEED UP"))
                                    {
                                        if (CheckIfItemSucceeds())
                                        {
                                            if (x == 0 && item1CurUses >= 0)
                                            {
                                                DecreaseUsesItem1();
                                                if (item1CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 1 && item2CurUses >= 0)
                                            {
                                                DecreaseUsesItem2();
                                                if (item2CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 2 && item3CurUses >= 0)
                                            {
                                                DecreaseUsesItem3();
                                                if (item3CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }

                                            if (x == 0 && item1CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 1 && item2CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 2 && item3CurUses <= -1)
                                            {
                                                continue;
                                            }

                                            EffectData effect = EffectManager.instance.GetEffect("SPEED UP");
                                            AddUnitEffect(effect, this, 1, 1, false, true);
                                            TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsSecond[x].itemSpriteItemTab);


                                            yield return new WaitForSeconds(.35f);
                                            continue;
                                        }
                                        else
                                        {
                                            /*
                                            if (x == 0 && item1CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            if (x == 1 && item2CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            if (x == 2 && item3CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            */
                                        }
                                    }
                                    else if (TeamItemsManager.Instance.equippedItemsSecond[x].effectAdded == EffectManager.instance.GetEffect("DEFENSE UP"))
                                    {
                                        if (CheckIfItemSucceeds())
                                        {
                                            if (x == 0 && item1CurUses >= 0)
                                            {
                                                DecreaseUsesItem1();
                                                if (item1CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 1 && item2CurUses >= 0)
                                            {
                                                DecreaseUsesItem2();
                                                if (item2CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 2 && item3CurUses >= 0)
                                            {
                                                DecreaseUsesItem3();
                                                if (item3CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }

                                            if (x == 0 && item1CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 1 && item2CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 2 && item3CurUses <= -1)
                                            {
                                                continue;
                                            }

                                            EffectData effect = EffectManager.instance.GetEffect("DEFENSE UP");
                                            AddUnitEffect(effect, this, 1, 1, false, true);
                                            TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsSecond[x].itemSpriteItemTab);
                                            //AudioManager.Instance.Play("SFX_ItemTrigger");
                                            yield return new WaitForSeconds(.35f);
                                            continue;
                                        }
                                        else
                                        {
                                            /*
                                            if (x == 0 && item1CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            if (x == 1 && item2CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            if (x == 2 && item3CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            */
                                        }
                                    }

                                    yield return new WaitForSeconds(.35f);
                                }
                            }
                        }
                        else if (i == 2)
                        {
                            //Debug.Log("5");
                            for (int x = 0; x < TeamItemsManager.Instance.equippedItemsThird.Count; x++)
                            {
                                //Debug.Log("6");
                                if (TeamItemsManager.Instance.equippedItemsThird[x].activeOnTurnStart)
                                {
                                    // Covers small and big potion
                                    if (TeamItemsManager.Instance.equippedItemsThird[x].isBaseHealing)
                                    {
                                        // Check if unit is low enough health to trigger item
                                        if (((GetUnitCurHealth() / GetUnitMaxHealth()) * 100) <= TeamItemsManager.Instance.equippedItemsThird[x].threshHoldAmount)
                                        {
                                            if (x == 0 && item1CurUses >= 0)
                                            {
                                                DecreaseUsesItem1();
                                                if (item1CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 1 && item2CurUses >= 0)
                                            {
                                                DecreaseUsesItem2();
                                                if (item2CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 2 && item3CurUses >= 0)
                                            {
                                                DecreaseUsesItem3();
                                                if (item3CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else
                                            {
                                                /*
                                                if (x == 0 && item1CurUses > 1)
                                                    TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                                if (x == 1 && item2CurUses > 1)
                                                    TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                                if (x == 2 && item3CurUses > 1)
                                                    TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                                */
                                            }

                                            ItemPiece item = TeamItemsManager.Instance.equippedItemsThird[x];
                                            float healAmount = ((float)item.itemPower / 100f) * GetUnitMaxHealth();
                                            UpdateUnitCurHealth((int)healAmount, false, false, true, false, true);
                                            //StartCoroutine(SpawnPowerUI((int)healAmount, false, false, null, false));
                                            TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsThird[x].itemSpriteItemTab);
                                            //AudioManager.Instance.Play("SFX_ItemTrigger");
                                            yield return new WaitForSeconds(.35f);
                                            continue;
                                        }
                                    }
                                    else if (TeamItemsManager.Instance.equippedItemsThird[x].effectAdded == EffectManager.instance.GetEffect("POWERUP"))
                                    {
                                        if (CheckIfItemSucceeds())
                                        {
                                            if (x == 0 && item1CurUses >= 0)
                                            {
                                                DecreaseUsesItem1();
                                                if (item1CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 1 && item2CurUses >= 0)
                                            {
                                                DecreaseUsesItem2();
                                                if (item2CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 2 && item3CurUses >= 0)
                                            {
                                                DecreaseUsesItem3();
                                                if (item3CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }

                                            if (x == 0 && item1CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 1 && item2CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 2 && item3CurUses <= -1)
                                            {
                                                continue;
                                            }

                                            EffectData effect = EffectManager.instance.GetEffect("POWERUP");
                                            AddUnitEffect(effect, this, 1, 1, false, true);
                                            TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsThird[x].itemSpriteItemTab);
                                            //AudioManager.Instance.Play("SFX_ItemTrigger");
                                            yield return new WaitForSeconds(.35f);
                                            continue;
                                        }
                                        else
                                        {
                                            /*
                                            if (x == 0 && item1CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            if (x == 1 && item2CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            if (x == 2 && item3CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            */
                                        }
                                    }
                                    else if (TeamItemsManager.Instance.equippedItemsThird[x].effectAdded == EffectManager.instance.GetEffect("HEALTH UP"))
                                    {
                                        if (CheckIfItemSucceeds())
                                        {
                                            if (x == 0 && item1CurUses >= 0)
                                            {
                                                DecreaseUsesItem1();
                                                if (item1CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 1 && item2CurUses >= 0)
                                            {
                                                DecreaseUsesItem2();
                                                if (item2CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 2 && item3CurUses >= 0)
                                            {
                                                DecreaseUsesItem3();
                                                if (item3CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }

                                            if (x == 0 && item1CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 1 && item2CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 2 && item3CurUses <= -1)
                                            {
                                                continue;
                                            }

                                            EffectData effect = EffectManager.instance.GetEffect("HEALTH UP");
                                            AddUnitEffect(effect, this, 1, 1, false, true);
                                            TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsThird[x].itemSpriteItemTab);
                                            //AudioManager.Instance.Play("SFX_ItemTrigger");
                                            yield return new WaitForSeconds(.35f);
                                            continue;
                                        }
                                        else
                                        {
                                            /*
                                            if (x == 0 && item1CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            if (x == 1 && item2CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            if (x == 2 && item3CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            */
                                        }
                                    }
                                    else if (TeamItemsManager.Instance.equippedItemsThird[x].effectAdded == EffectManager.instance.GetEffect("SPEED UP"))
                                    {
                                        if (CheckIfItemSucceeds())
                                        {
                                            if (x == 0 && item1CurUses >= 0)
                                            {
                                                DecreaseUsesItem1();
                                                if (item1CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 1 && item2CurUses >= 0)
                                            {
                                                DecreaseUsesItem2();
                                                if (item2CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 2 && item3CurUses >= 0)
                                            {
                                                DecreaseUsesItem3();
                                                if (item3CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }

                                            if (x == 0 && item1CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 1 && item2CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 2 && item3CurUses <= -1)
                                            {
                                                continue;
                                            }

                                            EffectData effect = EffectManager.instance.GetEffect("SPEED UP");
                                            AddUnitEffect(effect, this, 1, 1, false, true);
                                            TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsThird[x].itemSpriteItemTab);


                                            yield return new WaitForSeconds(.35f);
                                            continue;
                                        }
                                        else
                                        {
                                            /*
                                            if (x == 0 && item1CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            if (x == 1 && item2CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            if (x == 2 && item3CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            */
                                        }
                                    }
                                    else if (TeamItemsManager.Instance.equippedItemsThird[x].effectAdded == EffectManager.instance.GetEffect("DEFENSE UP"))
                                    {
                                        if (CheckIfItemSucceeds())
                                        {
                                            if (x == 0 && item1CurUses >= 0)
                                            {
                                                DecreaseUsesItem1();
                                                if (item1CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 1 && item2CurUses >= 0)
                                            {
                                                DecreaseUsesItem2();
                                                if (item2CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }
                                            else if (x == 2 && item3CurUses >= 0)
                                            {
                                                DecreaseUsesItem3();
                                                if (item3CurUses == 0)
                                                {
                                                    yield return new WaitForSeconds(.2f);
                                                    AudioManager.Instance.Play("SFX_ItemDepleted");
                                                    //TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSprite);
                                                    //continue;
                                                }
                                            }

                                            if (x == 0 && item1CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 1 && item2CurUses <= -1)
                                            {
                                                continue;
                                            }
                                            else if (x == 2 && item3CurUses <= -1)
                                            {
                                                continue;
                                            }

                                            EffectData effect = EffectManager.instance.GetEffect("DEFENSE UP");
                                            AddUnitEffect(effect, this, 1, 1, false, true);
                                            TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsThird[x].itemSpriteItemTab);
                                            //AudioManager.Instance.Play("SFX_ItemTrigger");
                                            yield return new WaitForSeconds(.35f);
                                            continue;
                                        }
                                        else
                                        {
                                            /*
                                            if (x == 0 && item1CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            if (x == 1 && item2CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            if (x == 2 && item3CurUses > 1)
                                                TriggerItemVisualAlert(TeamItemsManager.Instance.equippedItemsMain[x].itemSpriteFail, false);
                                            */
                                        }
                                    }

                                    yield return new WaitForSeconds(.35f);
                                }
                            }
                        }
                        break;
                    }
                }
                yield return new WaitForSeconds(.5f);
            }
        }
    }

    public IEnumerator DecreaseEffectTurnsLeft(bool turnStart, bool parry = false, bool immune = false, bool otherLink = false)
    {
        yield return new WaitForSeconds(0.35f);

        // If no effects remain on the unit, stop
        if (activeEffects.Count >= 1)
        {
            if (activeEffects[0] == null)
            {
                //if (turnStart)
                //GameManager.Instance.ContinueTurnOrder();

                //yield break;
            }
        }

        for (int i = 0; i < activeEffects.Count; i++)
        {
            if (activeEffects[i] != null)
            {
                if (otherLink)
                {
                    if (activeEffects[i].curEffectName == Effect.EffectName.OTHER_LINK)
                    {
                        //activeEffects[i].TriggerPowerEffect(this);
                        string name = "OTHER LINK";
                        TriggerTextAlert(name, 1, true, "Trigger");
                        activeEffects[i].ReduceTurnCountText(this, true);
                        break;
                    }
                }

                if (immune)
                {
                    if (activeEffects[i].curEffectName == Effect.EffectName.IMMUNITY)
                    {
                        activeEffects[i].TriggerPowerEffect(this);
                        TriggerTextAlert(activeEffects[i].effectName, 1, true, "Trigger");
                        activeEffects[i].ReduceTurnCountText(this);
                    }
                }
                if (parry && !immune)
                {
                    if (activeEffects[i].curEffectName == Effect.EffectName.PARRY)
                    {
                        activeEffects[i].TriggerPowerEffect(this);
                        TriggerTextAlert(activeEffects[i].effectName, 1, true, "Trigger");
                        activeEffects[i].ReduceTurnCountText(this);
                    }
                }

                if (turnStart && !immune)
                {
                    if (activeEffects[i].curEffectTrigger == Effect.EffectTrigger.TURNSTART)
                    {
                        activeEffects[i].TriggerPowerEffect(this);
                        string name = activeEffects[i].effectName;

                        if (activeEffects[i].effectName == "HOLY_LINK")
                            name = "HOLY LINK";
                        else if (activeEffects[i].effectName == "POWERDOWN")
                            name = "POWER DOWN";

                        TriggerTextAlert(name, 1, true, "Trigger");
                        activeEffects[i].ReduceTurnCountText(this);

                        yield return new WaitForSeconds(.5f);
                    }
                }
                else if (!turnStart)
                {
                    if (activeEffects[i].curEffectTrigger == Effect.EffectTrigger.TURNEND)
                    {
                        activeEffects[i].TriggerPowerEffect(this);

                        string name = activeEffects[i].effectName;
                        if (activeEffects[i].effectName == "POWERDOWN")
                            name = "POWER DOWN";
                        TriggerTextAlert(name, 1, true, "Trigger");

                        activeEffects[i].ReduceTurnCountText(this);

                        yield return new WaitForSeconds(.5f);
                    }
                }
            }
        }

        // If effect was removed, remove it here after all done
        for (int x = 0; x < activeEffects.Count; x++)
        {
            if (activeEffects[x] == null)
            {
                /*
                if (holyLinkPartner != null && activeEffects[x].curEffectName == Effect.EffectName.HOLY_LINK)
                {
                    holyLinkPartner.StartCoroutine(holyLinkPartner.DecreaseEffectTurnsLeft(false, false, false, true));
                }
                */

                GetEffects().RemoveAt(x);
            }
        }

        yield return new WaitForSeconds(.5f);

        // Continue turn order system after effects have been depleted from turn start
        if (turnStart && !isDead)
            GameManager.Instance.ContinueTurnOrder();
        //else if (turnStart && isDead)

    }


    public IEnumerator UnitEndTurn(bool waitLong = false, bool waitExtraLong = false)
    {
        if (waitLong)
            yield return new WaitForSeconds(0f);    // old was 1.25f
        else
            yield return new WaitForSeconds(GameManager.Instance.enemyAttackWaitTime);
        // End turn

        //Debug.Log("bbb");
        GameManager.Instance.UpdateTurnOrder();
    }

    public List<SkillData> GetStartingSkills()
    {
        return startingSkills;
    }

    public void SetStartingSkills(List<SkillData> skills)
    {
        List<SkillData> newStartingSkills = skills;
        startingSkills.Clear();

        for (int i = 0; i < skills.Count; i++)
        {
            //skills[i]
        }
        startingSkills = newStartingSkills;
    }

    public SkillData GetNoCDSkill()
    {
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].skillCooldown == 0)
            {
                return skills[i];
            }
        }

        return null;
    }

    public void ResetSkill0Cooldown()
    {
        if (curUnitType == UnitType.PLAYER)
        {
            UpdateSkillCooldown(GetSkill(0), 0);
            GameManager.Instance.skill0IconCooldownUIText.UpdateUIText(GetSkillCurCooldown(GetSkill(0)).ToString());
        }
        else
        {
            skill0CurCooldown = 0;
        }
    }
    public void ResetSkill1Cooldown()
    {
        if (curUnitType == UnitType.PLAYER)
        {
            UpdateSkillCooldown(GetSkill(1), 0);
            GameManager.Instance.skill1IconCooldownUIText.UpdateUIText(GetSkillCurCooldown(GetSkill(1)).ToString());
        }
        else
        {
            skill1CurCooldown = 0;
        }
    }
    public void ResetSkill2Cooldown()
    {
        if (curUnitType == UnitType.PLAYER)
        {
            UpdateSkillCooldown(GetSkill(2), 0);
            GameManager.Instance.skill2IconCooldownUIText.UpdateUIText(GetSkillCurCooldown(GetSkill(2)).ToString());
        }
        else
        {
            skill2CurCooldown = 0;
        }
    }
    public void ResetSkill3Cooldown()
    {
        if (curUnitType == UnitType.PLAYER)
        {
            UpdateSkillCooldown(GetSkill(3), 0);
            GameManager.Instance.skill3IconCooldownUIText.UpdateUIText(GetSkillCurCooldown(GetSkill(3)).ToString());
        }
        else
        {
            skill3CurCooldown = 0;
        }
    }

    public void SetSkill0CooldownMax()
    {
        if (curUnitType == UnitType.PLAYER)
        {
            UpdateSkillCooldown(GetSkill(0), GetSkillCooldownMax(GetSkill(0)));
            GameManager.Instance.skill0IconCooldownUIText.UpdateUIText(GetSkillCurCooldown(GetSkill(0)).ToString());
        }
        else
        {
            skill0CurCooldown = GetSkill(0).skillCooldown + 1;
        }
    }
    public void SetSkill1CooldownMax()
    {
        if (curUnitType == UnitType.PLAYER)
        {
            UpdateSkillCooldown(GetSkill(1), GetSkillCooldownMax(GetSkill(1)));
            GameManager.Instance.skill1IconCooldownUIText.UpdateUIText(GetSkillCurCooldown(GetSkill(1)).ToString());
        }
        else
        {
            skill1CurCooldown = GetSkill(1).skillCooldown + 1;
        }
    }
    public void SetSkill2CooldownMax()
    {
        if (curUnitType == UnitType.PLAYER)
        {
            UpdateSkillCooldown(GetSkill(2), GetSkillCooldownMax(GetSkill(2)));
            GameManager.Instance.skill2IconCooldownUIText.UpdateUIText(GetSkillCurCooldown(GetSkill(2)).ToString());
        }
        else
        {
            skill2CurCooldown = GetSkill(2).skillCooldown + 1;
        }
    }
    public void SetSkill3CooldownMax()
    {
        if (curUnitType == UnitType.PLAYER)
        {
            UpdateSkillCooldown(GetSkill(3), GetSkillCooldownMax(GetSkill(3)));
            GameManager.Instance.skill3IconCooldownUIText.UpdateUIText(GetSkillCurCooldown(GetSkill(3)).ToString());
        }
        else
        {
            skill3CurCooldown = GetSkill(3).skillCooldown + 1;
        }
    }

    public int GetSkillCurCooldown(SkillData skill)
    {
        return skill.curCooldown;
    }

    public int GetSkillCooldownMax(SkillData skill)
    {
        return skill.skillCooldown + 1;
    }

    public void UpdateSkillCooldown(SkillData skill, int cd)
    {
        skill.curCooldown = cd;
    }

    public void DecreaseSkill0Cooldown()
    {
        if (curUnitType == UnitType.PLAYER)
        {
            int curCooldown = GetSkillCurCooldown(GetSkill(0));
            curCooldown--;

            if (curCooldown <= 0)
                curCooldown = 0;

            UpdateSkillCooldown(GetSkill(0), curCooldown);

            GameManager.Instance.skill0IconCooldownUIText.UpdateUIText(GetSkillCurCooldown(GetSkill(0)).ToString());
        }
        else
        {
            skill0CurCooldown--;

            if (skill0CurCooldown <= 0)
                skill0CurCooldown = 0;
        }
    }
    public void DecreaseSkill1Cooldown()
    {
        if (curUnitType == UnitType.PLAYER)
        {
            int curCooldown = GetSkillCurCooldown(GetSkill(1));
            curCooldown--;

            if (curCooldown <= 0)
                curCooldown = 0;

            UpdateSkillCooldown(GetSkill(1), curCooldown);

            GameManager.Instance.skill1IconCooldownUIText.UpdateUIText(GetSkillCurCooldown(GetSkill(1)).ToString());
        }
        else
        {
            skill1CurCooldown--;

            if (skill1CurCooldown <= 0)
                skill1CurCooldown = 0;
        }
    }
    public void DecreaseSkill2Cooldown()
    {
        if (curUnitType == UnitType.PLAYER)
        {
            int curCooldown = GetSkillCurCooldown(GetSkill(2));
            curCooldown--;

            if (curCooldown <= 0)
                curCooldown = 0;

            UpdateSkillCooldown(GetSkill(2), curCooldown);

            GameManager.Instance.skill2IconCooldownUIText.UpdateUIText(GetSkillCurCooldown(GetSkill(2)).ToString());
        }
        else
        {
            skill2CurCooldown--;

            if (skill2CurCooldown <= 0)
                skill2CurCooldown = 0;
        }
    }
    public void DecreaseSkill3Cooldown()
    {
        if (curUnitType == UnitType.PLAYER)
        {
            int curCooldown = GetSkillCurCooldown(GetSkill(3));
            curCooldown--;

            if (curCooldown <= 0)
                curCooldown = 0;

            UpdateSkillCooldown(GetSkill(3), curCooldown);

            GameManager.Instance.skill3IconCooldownUIText.UpdateUIText(GetSkillCurCooldown(GetSkill(3)).ToString());
        }
        else
        {
            skill3CurCooldown--;

            if (skill3CurCooldown <= 0)
                skill3CurCooldown = 0;
        }
    }

    SkillData ChooseRandomSkill()
    {
        int unitEnemyIntelligence = 10;

        for (int i = 0; i < unitEnemyIntelligence; i++)
        {
            bool allyLow = false;
            for (int y = 0; y < GameManager.Instance.activeRoomAllUnitFunctionalitys.Count; y++)
            {
                if (GameManager.Instance.activeRoomAllUnitFunctionalitys[y].curUnitType == UnitType.ENEMY && !GameManager.Instance.activeRoomAllUnitFunctionalitys[y].isDead)
                {
                    if (GameManager.Instance.activeRoomAllUnitFunctionalitys[y].GetUnitCurHealth() / GameManager.Instance.activeRoomAllUnitFunctionalitys[y].GetUnitMaxHealth() <= GameManager.Instance.allyHealthThreshholdHeal)
                    {
                        allyLow = true;
                    }
                }
            }

            SkillData skill = null;

            // Healing skill prio low health allies
            for (int x = 0; x < 4; x++)
            {
                int cd = 0;
                if (x == 0)
                    cd = GameManager.Instance.GetActiveUnitFunctionality().skill0CurCooldown;
                else if (x == 1)
                    cd = GameManager.Instance.GetActiveUnitFunctionality().skill1CurCooldown;
                else if (x == 2)
                    cd = GameManager.Instance.GetActiveUnitFunctionality().skill2CurCooldown;
                else if (x == 3)
                    cd = GameManager.Instance.GetActiveUnitFunctionality().skill3CurCooldown;

                if (GameManager.Instance.GetActiveUnitFunctionality().GetSkill(x).curSkillType == SkillData.SkillType.SUPPORT
                    && GameManager.Instance.GetActiveUnitFunctionality().GetSkill(x).curSkillPower != 0 && cd == 0)
                {
                    if (allyLow)
                    {
                        return GameManager.Instance.GetActiveUnitFunctionality().GetSkill(x);
                    }
                    else
                        skill = GameManager.Instance.GetActiveUnitFunctionality().GetSkill(x);
                }
            }

            if (!allyLow)
            {
                // Dont choose healing

                // Do random picking
                int rand = Random.Range(1, 5);
                //Debug.Log(rand);

                //Debug.Log(rand);
                if (rand == 1)  // Skill 1
                {
                    if (skill1CurCooldown == 0 && !GameManager.Instance.GetActiveUnitFunctionality().GetSkill(1).isPassive
                        && GameManager.Instance.GetActiveUnitFunctionality().GetSkill(1) != skill)
                        return GameManager.Instance.GetActiveUnitFunctionality().GetSkill(1);
                    else
                        continue;
                }
                else if (rand == 2)  // Skill 2
                {
                    if (skill2CurCooldown == 0 && !GameManager.Instance.GetActiveUnitFunctionality().GetSkill(2).isPassive
                        && GameManager.Instance.GetActiveUnitFunctionality().GetSkill(2) != skill)
                        return GameManager.Instance.GetActiveUnitFunctionality().GetSkill(2);
                    else
                        continue;
                }
                else if (rand == 3)  // Skill 3
                {
                    if (skill3CurCooldown == 0 && !GameManager.Instance.GetActiveUnitFunctionality().GetSkill(3).isPassive
                        && GameManager.Instance.GetActiveUnitFunctionality().GetSkill(3) != skill)
                        return GameManager.Instance.GetActiveUnitFunctionality().GetSkill(3);
                    else
                        continue;
                }
                // Base skill
                else if (rand == 4)
                {
                    if (skill0CurCooldown == 0 && !GameManager.Instance.GetActiveUnitFunctionality().GetSkill(0).isPassive
                        && GameManager.Instance.GetActiveUnitFunctionality().GetSkill(0) != skill)
                        return GameManager.Instance.GetActiveUnitFunctionality().GetSkill(0);
                }
            }
        }

        return GameManager.Instance.GetActiveUnitFunctionality().GetSkill(0);
    }

    public List<Effect> GetEffects()
    {
        return activeEffects;
    }

    public Effect GetEffect(string name)
    {
        for (int i = 0; i < activeEffects.Count; i++)
        {
            if (activeEffects[i].effectName == name)
                return activeEffects[i];
        }

        return null;
    }

    public bool settingUpEffect = false;

    public void UpdateFighterPosition()
    {
        //-17.4, -33.9

        if (GetComponentInChildren<Animator>())
        {
            if (GetUnitName() == "Monk")
            {
                if (GetComponentInChildren<CharacterAnimation>())
                    GetComponentInChildren<CharacterAnimation>().gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-17.4f, -33.9f, 1);
            }

        }

    }

    public void ResetHolyLinkPartner()
    {
        holyLinkPartner = null;
    }

    public void AddUnitEffect(EffectData addedEffect, UnitFunctionality targetUnit, int turnDuration = 1, int effectHitAcc = -1, bool byPassAcc = true, bool item = false)
    {
        //Debug.Log("addedEffect 1 " + addedEffect.curEffectName);


        //SkillData activeSkill = null;
        // If player miss, do not apply effect
        if (effectHitAcc == 0 || targetUnit.isParrying || !GameManager.Instance.playerInCombat)
            return;

        //Debug.Log("addedEffect 2 " + addedEffect.curEffectName);
        //Debug.Log("Effect hit acc " + effectHitAcc);
        // If unit is already effected with this effect, add to the effect
        for (int i = 0; i < activeEffects.Count; i++)
        {
            if (addedEffect.effectName == activeEffects[i].effectName)
            {
                // Determining whether the effect hits, If it fails, stop
                // Add more stacks to the effect that the unit already has
                for (int x = 0; x < 1; x++)
                {
                    // Determining whether the effect hits, If it fails, stop
                    if (GameManager.Instance.GetActiveSkill().GetCalculatedSkillEffectStat() != 0 && byPassAcc && GameManager.Instance.isSkillsMode)
                    {
                        //Debug.Log(GameManager.Instance.GetActiveSkill().GetCalculatedSkillEffectStat());

                        int rand = Random.Range(1, 101);
                        if (rand <= GameManager.Instance.GetActiveSkill().GetCalculatedSkillEffectStat())
                        {
                            if (effectAddedCount < 1)
                                activeEffects[i].AddTurnCountText(1);

                            // Cause Effect. Do not trigger text alert if its casting a skill on self. (BECAUSE: Skill announce overtakes this).
                            effectAddedCount++;

                            settingUpEffect = true;

                            if (GameManager.Instance.maxUnitEffectTier >= activeEffects[i].effectPowerStacks)
                            {
                                activeEffects[i].EffectApply(this);
                                activeEffects[i].UpdateEffectTierImages();
                                if (activeEffects[i] != null)
                                    activeEffects[i].gameObject.GetComponent<UIElement>().AnimateUI(false);
                                else
                                {

                                }

                                string name = addedEffect.effectName;
                                if (addedEffect.effectName == "HOLY_LINK")
                                    name = "HOLY LINK";
                                else if (addedEffect.effectName == "OTHER_LINK")
                                    name = "OTHER LINK";
                                else if (addedEffect.effectName == "POWERDOWN")
                                    name = "POWER DOWN";
                                TriggerTextAlert(name, 1, true, "Inflict");

                                //Debug.Log("addedEffect 3 " + addedEffect.curEffectName);

                                if (addedEffect.effectName == "HOLY_LINK")
                                {
                                    holyLinkPartner = GameManager.Instance.GetActiveUnitFunctionality();
                                    GameManager.Instance.GetActiveUnitFunctionality().holyLinkPartner = this;
                                    GameManager.Instance.GetActiveUnitFunctionality().AddUnitEffect(GameManager.Instance.GetActiveSkill().effect2, GameManager.Instance.GetActiveUnitFunctionality(), 1, 1, true, false);
                                }

                                // If this unit has holy link, check all other fighters to see if they have other link, then give the effect thats
                                // being added to this unit, to the other linked fighter also
                                if (GetEffect("HOLY_LINK"))
                                {
                                    for (int X = 0; X < GameManager.Instance.activeRoomHeroes.Count; X++)
                                    {
                                        if (GameManager.Instance.activeRoomHeroes[X].GetEffect("OTHER_LINK") && addedEffect.curEffectName != EffectData.EffectName.HOLY_LINK)
                                        {
                                            GameManager.Instance.activeRoomHeroes[X].AddUnitEffect(addedEffect, targetUnit, turnDuration, effectHitAcc, byPassAcc, item);
                                        }
                                    }
                                }

                            }
                        }
                        else
                            continue;

                    }
                    else if (item || byPassAcc)
                    {
                        if (GameManager.Instance.maxUnitEffectTier >= activeEffects[i].effectPowerStacks)
                        {
                            //usedSkill = GameManager.Instance.GetActiveSkill();

                            activeEffects[i].AddTurnCountText(1);
                            activeEffects[i].EffectApply(this);
                            activeEffects[i].UpdateEffectTierImages();
                            if (activeEffects[i] != null)
                                activeEffects[i].gameObject.GetComponent<UIElement>().AnimateUI(false);
                            else
                            {

                            }
                            //return;

                            string name = addedEffect.effectName;
                            if (addedEffect.effectName == "HOLY_LINK")
                                name = "HOLY LINK";
                            else if (addedEffect.effectName == "OTHER_LINK")
                                name = "OTHER LINK";
                            else if (addedEffect.effectName == "POWERDOWN")
                                name = "POWER DOWN";
                            TriggerTextAlert(name, 1, true, "Inflict");

                            //Debug.Log("addedEffect 4 " + addedEffect.curEffectName);

                            if (addedEffect.effectName == "HOLY_LINK" && targetUnit != GameManager.Instance.GetActiveUnitFunctionality())
                            {
                                holyLinkPartner = GameManager.Instance.GetActiveUnitFunctionality();
                                GameManager.Instance.GetActiveUnitFunctionality().holyLinkPartner = this;
                                GameManager.Instance.GetActiveUnitFunctionality().AddUnitEffect(GameManager.Instance.GetActiveSkill().effect2, GameManager.Instance.GetActiveUnitFunctionality(), 1, 1, true, false);

                            }

                            // If this unit has holy link, check all other fighters to see if they have other link, then give the effect thats
                            // being added to this unit, to the other linked fighter also
                            if (GetEffect("HOLY_LINK"))
                            {
                                for (int X = 0; X < GameManager.Instance.activeRoomHeroes.Count; X++)
                                {
                                    if (GameManager.Instance.activeRoomHeroes[X].GetEffect("OTHER_LINK"))
                                    {
                                        GameManager.Instance.activeRoomHeroes[X].AddUnitEffect(addedEffect, targetUnit, turnDuration, effectHitAcc, byPassAcc, item);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // If unit DOES NOT currently have this effect, create it, and start the loop
        if (GetEffect(addedEffect.effectName) == null)
        {
            Effect effect = null;

            for (int m = 0; m < 1; m++)
            {
                GameObject go = null;

                // Determining whether the effect hits, If it fails, stop
                if (GameManager.Instance.GetActiveSkill().GetCalculatedSkillEffectStat() != 0 && byPassAcc && GameManager.Instance.isSkillsMode)
                {
                    if (m == 0)
                    {
                        int rand = Random.Range(1, 101);
                        if (rand <= GameManager.Instance.GetActiveSkill().GetCalculatedSkillEffectStat())
                        {
                            // Spawn new effect on target unit
                            go = Instantiate(EffectManager.instance.effectPrefab, effectsParent.transform);
                            go.transform.SetParent(effectsParent);
                            go.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                            go.transform.localScale = new Vector3(1, 1, 1);

                            effect = go.GetComponent<Effect>();
                            activeEffects.Add(effect);
                            effect.Setup(addedEffect, targetUnit, effectHitAcc, false);

                            string name = addedEffect.effectName;
                            if (addedEffect.effectName == "HOLY_LINK")
                                name = "HOLY LINK";
                            else if (addedEffect.effectName == "OTHER_LINK")
                                name = "OTHER LINK";
                            else if (addedEffect.effectName == "POWERDOWN")
                                name = "POWER DOWN";
                            TriggerTextAlert(name, 1, true, "Inflict");


                            //Debug.Log("addedEffect 5 " + addedEffect.curEffectName);

                            if (addedEffect.effectName == "HOLY_LINK" && targetUnit != GameManager.Instance.GetActiveUnitFunctionality())
                            {
                                holyLinkPartner = GameManager.Instance.GetActiveUnitFunctionality();
                                GameManager.Instance.GetActiveUnitFunctionality().holyLinkPartner = this;
                                GameManager.Instance.GetActiveUnitFunctionality().AddUnitEffect(GameManager.Instance.GetActiveSkill().effect2, GameManager.Instance.GetActiveUnitFunctionality(), 1, 1, true, false);
                            }

                            // If this unit has holy link, check all other fighters to see if they have other link, then give the effect thats
                            // being added to this unit, to the other linked fighter also
                            if (GetEffect("HOLY_LINK"))
                            {
                                for (int X = 0; X < GameManager.Instance.activeRoomHeroes.Count; X++)
                                {
                                    if (GameManager.Instance.activeRoomHeroes[X].GetEffect("OTHER_LINK"))
                                    {
                                        GameManager.Instance.activeRoomHeroes[X].AddUnitEffect(addedEffect, targetUnit, turnDuration, effectHitAcc, byPassAcc, item);
                                    }
                                }
                            }

                            effect.gameObject.GetComponent<UIElement>().AnimateUI(false);

                            settingUpEffect = true;
                        }
                    }
                }
                else if (item || byPassAcc)
                {
                    if (m == 0)
                    {
                        if (!item)
                        {
                            // Spawn new effect on target unit
                            go = Instantiate(EffectManager.instance.effectPrefab, effectsParent.transform);
                            go.transform.SetParent(effectsParent);
                            go.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                            go.transform.localScale = new Vector3(1, 1, 1);

                            effect = go.GetComponent<Effect>();
                            activeEffects.Add(effect);
                            effect.Setup(addedEffect, targetUnit, effectHitAcc);

                            string name = addedEffect.effectName;
                            if (addedEffect.effectName == "HOLY_LINK")
                                name = "HOLY LINK";
                            else if (addedEffect.effectName == "OTHER_LINK")
                                name = "OTHER LINK";
                            else if (addedEffect.effectName == "POWERDOWN")
                                name = "POWER DOWN";
                            TriggerTextAlert(name, 1, true, "Inflict");

                            //Debug.Log("addedEffect 6 " + addedEffect.curEffectName);

                            if (addedEffect.effectName == "HOLY_LINK" && targetUnit != GameManager.Instance.GetActiveUnitFunctionality())
                            {
                                holyLinkPartner = GameManager.Instance.GetActiveUnitFunctionality();
                                GameManager.Instance.GetActiveUnitFunctionality().holyLinkPartner = this;
                                GameManager.Instance.GetActiveUnitFunctionality().AddUnitEffect(GameManager.Instance.GetActiveSkill().effect2, GameManager.Instance.GetActiveUnitFunctionality(), 1, 1, true, false);
                            }

                            // If this unit has holy link, check all other fighters to see if they have other link, then give the effect thats
                            // being added to this unit, to the other linked fighter also
                            if (GetEffect("HOLY_LINK"))
                            {
                                for (int X = 0; X < GameManager.Instance.activeRoomHeroes.Count; X++)
                                {
                                    if (GameManager.Instance.activeRoomHeroes[X].GetEffect("OTHER_LINK") && addedEffect.effectName != "HOLY_LINK")
                                    {
                                        GameManager.Instance.activeRoomHeroes[X].AddUnitEffect(addedEffect, targetUnit, turnDuration, effectHitAcc, byPassAcc, item);
                                    }
                                }
                            }

                            effect.gameObject.GetComponent<UIElement>().AnimateUI(false);

                            settingUpEffect = true;
                        }
                        else
                        {
                            // Spawn new effect on target unit
                            go = Instantiate(EffectManager.instance.effectPrefab, effectsParent.transform);
                            go.transform.SetParent(effectsParent);
                            go.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                            go.transform.localScale = new Vector3(1, 1, 1);

                            effect = go.GetComponent<Effect>();
                            activeEffects.Add(effect);
                            effect.Setup(addedEffect, targetUnit, effectHitAcc);

                            string name = addedEffect.effectName;
                            if (addedEffect.effectName == "HOLY_LINK")
                                name = "HOLY LINK";
                            else if (addedEffect.effectName == "OTHER_LINK")
                                name = "OTHER LINK";
                            else if (addedEffect.effectName == "POWERDOWN")
                                name = "POWER DOWN";
                            TriggerTextAlert(name, 1, true, "Inflict");

                            //Debug.Log("addedEffect 7 " + addedEffect.curEffectName);

                            if (addedEffect.effectName == "HOLY_LINK" && targetUnit != GameManager.Instance.GetActiveUnitFunctionality())
                            {
                                holyLinkPartner = GameManager.Instance.GetActiveUnitFunctionality();
                                GameManager.Instance.GetActiveUnitFunctionality().holyLinkPartner = this;
                                GameManager.Instance.GetActiveUnitFunctionality().AddUnitEffect(GameManager.Instance.GetActiveSkill().effect2, GameManager.Instance.GetActiveUnitFunctionality(), 1, 1, true, false);
                            }

                            // If this unit has holy link, check all other fighters to see if they have other link, then give the effect thats
                            // being added to this unit, to the other linked fighter also
                            if (GetEffect("HOLY_LINK"))
                            {
                                for (int X = 0; X < GameManager.Instance.activeRoomHeroes.Count; X++)
                                {
                                    if (GameManager.Instance.activeRoomHeroes[X].GetEffect("OTHER_LINK"))
                                    {
                                        GameManager.Instance.activeRoomHeroes[X].AddUnitEffect(addedEffect, targetUnit, turnDuration, effectHitAcc, byPassAcc, item);
                                    }
                                }
                            }

                            effect.gameObject.GetComponent<UIElement>().AnimateUI(false);

                        }
                    }
                }
            }
        }
    }

    public void ResetEffects(float time = .2f)
    {
        if (effectsParent == null)
            return;

        for (int i = 0; i < activeEffects.Count; i++)
        {
            activeEffects[i].EffectRemove(this, false);
        }

        StartCoroutine(DeleteEffects(time));
    }

    IEnumerator DeleteEffects(float time = .2f)
    {
        yield return new WaitForSeconds(time);

        for (int i = 0; i < effectsParent.childCount; i++)
        {
            Destroy(effectsParent.GetChild(i).gameObject);
        }
    }
    public void UpdateUnitValue(int val)
    {
        unitValue = val;
    }

    public int GetUnitValue()
    {
        return unitValue;
    }

    public void UpdateUnitColour(Color color)
    {
        unitColour = color;
    }

    public Color GetUnitColour()
    {
        return unitColour;
    }

    public void UpdateUnitVisual(Sprite sprite)
    {
        unitImage.sprite = sprite;
    }

    public Sprite GetUnitSprite()
    {
        return unitImage.sprite;
    }

    public Sprite GetUnitIcon()
    {
        return unitIcon;
    }

    public void UpdateUnitIcon(Sprite sprite)
    {
        unitIcon = sprite;
    }

    public void ToggleTaunt(bool toggle)
    {
        isTaunting = toggle;
    }

    int healCount = 0;
    int damageCount = 0;

    public void ResetPowerUI(bool destroyPower = false)
    {
        damageCount = 0;
        healCount = 0;
        prevPowerUI = null;

        if (destroyPower)
        {
            for (int i = 0; i < powerUIParent.childCount; i++)
            {
                Destroy(powerUIParent.GetChild(i).gameObject);
            }
        }
    }

    public IEnumerator SpawnPowerUI(float power = 10f, bool isParrying = false, bool offense = false, Effect effect = null, bool isBlocked = false, bool effectTick = false)
    {
        //
        //Debug.Log("power = " + power);
        //if (GameManager.Instance.GetActiveSkill() == null)
        //    yield break;

        //Debug.Log("power = " + power);

        // If player in gear tab, stop power ui from appearing from armor equipping
        if (TeamGearManager.Instance.playerInGearTab || TeamItemsManager.Instance.playerInItemTab)
            yield break;

        // Play Audio
        if (offense)
        {
            //if (GameManager.Instance.GetActiveSkill().skillHit != null && GameManager.Instance.GetActiveSkill().skillHit != null)
            //    AudioManager.Instance.Play(GameManager.Instance.GetActiveSkill().skillHit.name);

            if (effect != null)
            {
                if (effect.curEffectName == Effect.EffectName.BLEED)
                    AudioManager.Instance.Play("Bleed");
                else if (effect.curEffectName == Effect.EffectName.POISON)
                    AudioManager.Instance.Play("Poison");
            }
        }
        else
        {
            AudioManager.Instance.Play("Heal");
        }

        if (GameManager.Instance.GetActiveItem() != null && !PostBattle.Instance.isInPostBattle && !GameManager.Instance.isSkillsMode
            && GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitType.PLAYER)
        {
            if (GameManager.Instance.GetActiveItem().projectileHit != null)
                AudioManager.Instance.Play(GameManager.Instance.GetActiveItem().projectileHit.name);
        }


        // LAST power UI hit
        if (damageCount >= GameManager.Instance.maxPowerUICount || healCount >= GameManager.Instance.maxPowerUICount)
        {
            if (offense)
                damageCount = 1;
            else
                healCount = 1;

            prevPowerUI = Instantiate(GameManager.Instance.powerUITextPrefab, powerUIParent.position, Quaternion.identity);
            prevPowerUI.transform.SetParent(powerUIParent);
            prevPowerUI.transform.localScale = Vector2.one;
        }
        // Starting power UI
        else if (damageCount == 0 && offense || healCount == 0 && !offense)
        {
            prevPowerUI = Instantiate(GameManager.Instance.powerUITextPrefab, powerUIParent.position, Quaternion.identity);
            prevPowerUI.transform.SetParent(powerUIParent);
            prevPowerUI.transform.localScale = Vector2.one;

            if (offense)
                damageCount++;
            else
                healCount++;
        }
        // Subsequent power UI hits
        else if (damageCount != 0 && offense || healCount != 0 && !offense)
        {
            if (offense)
            {
                // If power UI count has been reached from heal / damage, reset back to original Y position.

                prevPowerUI = Instantiate(GameManager.Instance.powerUITextPrefab, prevPowerUI.transform.position + new Vector3(0, GameManager.Instance.powerUIHeightLvInc), Quaternion.identity);
                damageCount++;

                prevPowerUI.transform.SetParent(powerUIParent);
                prevPowerUI.transform.localScale = Vector2.one;
            }
            else
            {
                // If power UI count has been reached from heal / damage, reset back to original Y position.
                prevPowerUI = Instantiate(GameManager.Instance.powerUITextPrefab, prevPowerUI.transform.position + new Vector3(0, GameManager.Instance.powerUIHeightLvInc), Quaternion.identity);
                healCount++;

                prevPowerUI.transform.SetParent(powerUIParent);
                prevPowerUI.transform.localScale = Vector2.one;
            }
        }

        prevPowerUI.transform.SetParent(powerUIParent);
        prevPowerUI.transform.localScale = Vector2.one;

        PowerText powerText = prevPowerUI.GetComponent<PowerText>();

        if (prevPowerUI == null)
        {
            yield break;
        }

        if (powerText != null)
        {
            if (isParrying || isBlocked)
            {
                powerText.UpdatePowerTextFontSize(GameManager.Instance.powerSkillParryFontSize);
                powerText.UpdatePowerTextColour(GameManager.Instance.gradientSkillParry);
                powerText.UpdatePowerText(GameManager.Instance.parryPowerText);
                yield break;
            }

            // If power is 0, display that it missed
            if (power <= 0)
            {
                //Debug.Log(power);
                powerText.UpdatePowerTextFontSize(GameManager.Instance.powerSkillParryFontSize);
                powerText.UpdatePowerTextColour(GameManager.Instance.gradientSkillParry);
                powerText.UpdatePowerText(GameManager.Instance.parryPowerText);

                if (GetEffect("IMMUNITY"))
                {
                    powerText.UpdatePowerTextFontSize(GameManager.Instance.powerSkillParryFontSize);
                    powerText.UpdatePowerTextColour(GameManager.Instance.gradientSkillParry);
                    powerText.UpdatePowerText(GameManager.Instance.parryPowerText);
                }
                //powerText.UpdatePowerText(power.ToString());   // Update Power Text
                yield break;
            }

            // Make Animate
            powerText.GetComponent<UIElement>().UpdateAlpha(1);

            float Randx = Random.Range(-GameManager.Instance.powerHorizontalRandomness, GameManager.Instance.powerHorizontalRandomness);
            prevPowerUI.transform.localPosition = new Vector2(Randx, prevPowerUI.transform.localPosition.y);

            //powerText.UpdateSortingOrder(powerUICount);

            // Otherwise, display the power
            powerText.UpdatePowerTextFontSize(GameManager.Instance.powerHitFontSize);

            // Update Text Colour
            if (effect == null)
            {
                if (offense)
                {
                    // Change power text colour to offense colour if the type of attack is offense
                    if (GameManager.Instance.activeSkill.curSkillType == SkillData.SkillType.OFFENSE)
                        powerText.UpdatePowerTextColour(GameManager.Instance.gradientSkillAttack);
                    // Change power text colour to support colour if the type of attack is support
                    else if (GameManager.Instance.activeSkill.curSkillType == SkillData.SkillType.SUPPORT)
                        powerText.UpdatePowerTextColour(GameManager.Instance.gradientSkillSupport);
                }
                else
                {
                    // Change power text colour to support colour if the type of attack is support
                    powerText.UpdatePowerTextColour(GameManager.Instance.gradientSkillSupport);
                }

            }
            else
            {
                // Change power text colour to offense colour if the type of attack is offense
                if (effect.curEffectType == Effect.EffectType.OFFENSE)
                    powerText.UpdatePowerTextColour(GameManager.Instance.gradientSkillAttack);
                // Change power text colour to support colour if the type of attack is support
                else if (effect.curEffectType == Effect.EffectType.SUPPORT)
                    powerText.UpdatePowerTextColour(GameManager.Instance.gradientSkillSupport);

                if (effect.curEffectName == Effect.EffectName.HEALTHUP && offense)
                    powerText.UpdatePowerTextColour(GameManager.Instance.gradientSkillAttack);

                if (effect.curEffectName == Effect.EffectName.RECOVER && !offense)
                    powerText.UpdatePowerTextColour(GameManager.Instance.gradientSkillSupport);
            }

            int finalPower = (int)power;
            float finalPower2 = finalPower;

            /*
            // Ensure only healing is cut
            if (!offense)
            {
                if (effect != null)
                    finalPower2 *= curHealingRecieved;
            }
            */

            int finalPower3 = (int)finalPower2;
            powerText.UpdatePowerText(finalPower3.ToString());   // Update Power Text
        }
        else
            Destroy(prevPowerUI.gameObject);

        if (offense && effectTick)
        {
            if (effect != null)
            {
                // If posion is about to tick, allow all other units to leach if they can
                if (effect.effectName == "POISON")
                {
                    // Loop through all units in current room
                    for (int i = 0; i < GameManager.Instance.activeRoomAllUnitFunctionalitys.Count; i++)
                    {
                        UnitFunctionality targetUnit = GameManager.Instance.activeRoomAllUnitFunctionalitys[i];

                        // If unit is able to leach, give them effects they should get.
                        if (targetUnit.isPoisonLeaching)
                        {
                            //yield return new WaitForSeconds(GameManager.Instance.leachEffectGainWait);

                            targetUnit.TriggerTextAlert("Poison Leach", 1, false);

                            yield return new WaitForSeconds(GameManager.Instance.leachEffectGainWait);

                            targetUnit.AddUnitEffect(EffectManager.instance.GetEffect("HEALTH UP"), targetUnit, 1, 1, true);

                            //yield return new WaitForSeconds(GameManager.Instance.leachEffectGainWait/2f);

                            /*
                            // Spawn new effect on target unit
                            targetUnit.AddUnitEffect(EffectManager.instance.GetEffect("RECOVER"), targetUnit, 1, 1, false);

                            yield return new WaitForSeconds(GameManager.Instance.leachEffectGainWait);
                            */
                        }
                    }
                }
            }
        }
    }

    public void SpawnProjectile(Transform target, bool skill = true)
    {
        GameObject go = Instantiate(GameManager.Instance.unitProjectile, projectileParent);
        go.transform.SetParent(projectileParent);

        // Set projectile to scale
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 400);

        go.transform.transform.localPosition = new Vector3(0, 0, 0);
        //go.transform.localScale = new Vector3(.75f, .75f);

        Projectile projectile = go.GetComponent<Projectile>();

        if (skill)
        {
            if (GameManager.Instance.GetActiveSkill().projectileAllowRandPosSpawn)
                projectile.allowRandomSpawnPosition = true;
            if (GameManager.Instance.GetActiveSkill().projectileAllowRandRotSpawn)
                projectile.allowRandomSpawnRotation = true;

            projectile.UpdateProjectileSprite(GameManager.Instance.GetActiveSkill().skillProjectile);
            projectile.UpdateProjectileAnimator(GameManager.Instance.GetActiveSkill().projectileAC);
            projectile.ToggleAllowSpin(GameManager.Instance.GetActiveSkill().projectileAllowSpin);
            projectile.ToggleAllowSpin(GameManager.Instance.GetActiveSkill().projectileAllowIdle);

            projectile.LookAtTarget(target);
            projectile.UpdateSpeed(GameManager.Instance.GetActiveSkill().projectileSpeed);

            if (GameManager.Instance.GetActiveSkill().projectileLaunch != null)
                AudioManager.Instance.Play(GameManager.Instance.GetActiveSkill().projectileLaunch.name);
        }

        // Item
        else
        {
            if (GameManager.Instance.GetActiveItem().allowRandomSpawnPosition)
                projectile.allowRandomSpawnPosition = true;
            if (GameManager.Instance.GetActiveItem().allowRandomSpawnRotation)
                projectile.allowRandomSpawnRotation = true;

            projectile.UpdateProjectileSprite(GameManager.Instance.GetActiveItem().itemSpriteCombatProjectile);
            //projectile.UpdateProjectileAnimator(GameManager.Instance.GetActiveSkill().projectileAC);
            projectile.ToggleAllowSpin(GameManager.Instance.GetActiveItem().projectileAllowSpin);

            projectile.LookAtTarget(target);
            projectile.UpdateSpeed(GameManager.Instance.GetActiveItem().projectileSpeed);

            if (GameManager.Instance.GetActiveItem().projectileLaunch != null)
                AudioManager.Instance.Play(GameManager.Instance.GetActiveItem().projectileLaunch.name);
        }


        if (curUnitType == UnitType.PLAYER)
            projectile.UpdateTeam(true);
        else
            projectile.UpdateTeam(false);
    }

    public void ToggleTextAlert(bool toggle)
    {
        if (toggle)
        {
            unitAlertTextParent.UpdateAlpha(1, false, 0, true);
        }
        else
        {
            unitAlertTextParent.UpdateAlpha(0, false, 0, true);
        }

    }

    public void ResetDamageHealCount()
    {
        healCount = 0;
        damageCount = 0;
    }
    public void ResetPosition()
    {
        //transform.position = Vector2.zero;
        rt.localPosition = Vector2.zero;
    }

    public void UpdateUnitName(string unitName)
    {
        gameObject.name = unitName;
    }

    public string GetUnitName()
    {
        return gameObject.name;
    }

    public void UpdateUnitSprite(GameObject spriteGO)
    {
        GameObject go = Instantiate(spriteGO, unitVisualsParent);

        go.transform.localPosition = new Vector3(0, 0, 0);
        go.transform.localScale = new Vector3(1, 1, 1);

        animator = go.GetComponent<Animator>();
    }

    public void UpdateUnitType(string unitType)
    {
        if (unitType == "Enemy")
            curUnitType = UnitType.ENEMY;
        else
            curUnitType = UnitType.PLAYER;
    }

    public void ToggleSelected(bool toggle)
    {
        isSelected = toggle;

        if (toggle)
            selectionCircle.UpdateAlpha(1);
        else
            selectionCircle.UpdateAlpha(0);
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    public bool CheckIfUnitIsDead()
    {
        // If unit's health is 0 or lower
        if (curHealth <= 0)
        {
            if (!GameManager.Instance.fallenHeroes.Contains(this) && curUnitType == UnitType.PLAYER && !reanimated)
            {
                GameManager.Instance.fallenHeroes.Add(this);

                ResetEffects();
                curPowerHits = powerHitsRoomStarting;

                UpdateUnitMaxHealth(startingRoomMaxHealth, true, false);
                curSpeed = startingRoomSpeed;
                curDefense = startingRoomDefense;
                ResetUnitHealingRecieved();

                return true;
            }
            else if (!GameManager.Instance.fallenEnemies.Contains(this) && curUnitType == UnitType.PLAYER
                || !GameManager.Instance.fallenEnemies.Contains(this) && reanimated)
            {
                GameManager.Instance.fallenEnemies.Add(this);

                ResetEffects();
                curPowerHits = powerHitsRoomStarting;

                UpdateUnitMaxHealth(startingRoomMaxHealth, true, false);
                curSpeed = startingRoomSpeed;
                curDefense = startingRoomDefense;
                ResetUnitHealingRecieved();

                return true;
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetIsDead()
    {
        isDead = false;

        //Debug.Log("true1");
        selectUnitButton.ToggleButton(true);
    }

    public void DisableDeadUnitButtons()
    {
        //Debug.Log("disabling " + GetUnitName());
        selectUnitButton.ToggleButton(false);
    }

    IEnumerator EnsureUnitIsDead(bool effect = false)
    {
        //Debug.Log("ensuring health");

        // If unit's health is 0 or lower
        if (curHealth <= 0 && !isDead)
        {
            isDead = true;

            curUnitTurnArrow.UpdateAlpha(0);

            ResetEffects();

            curHealth = 0;

            animator.SetTrigger("DeathFlg");

            ToggleFighterRaceIcon(false);

            AudioManager.Instance.Play(deathClip.name);

            if (curUnitType == UnitType.ENEMY)
            {
                GameManager.Instance.fallenEnemies.Add(this);
            }

            GameManager.Instance.RemoveUnitFromTurnOrder(this);

            if (reanimated)
            {
                if (GameManager.Instance.activeRoomHeroes.Contains(this))
                    GameManager.Instance.activeRoomHeroes.Remove(this);
            }

            if (effect)
            {
                int val = 0;
                for (int i = 0; i < GameManager.Instance.activeRoomAllUnitFunctionalitys.Count; i++)
                {
                    if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].curUnitType == UnitType.ENEMY)
                    {
                        val++;
                    }
                }
                if (curHealth <= 0 && val != 0)
                {
                    //Debug.Log("111");

                    if (curUnitType == UnitType.PLAYER && !reanimated)
                    {
                        yield return new WaitForSeconds(.75f);
                        StartCoroutine(UnitEndTurn(true));  // end unit turn
                    }
                }
            }

            yield return new WaitForSeconds(2.15f);

            ToggleUnitDisplay(false);
        }
    }

    public void CheckSwitchTeams()
    {
        // If unit has mindcontrolled skills from a previous enemy of the same type being mindcontrolled,
        // Change the back to a non-mind controlled version for this unit
        if (!reanimated)
        {
            for (int i = 0; i < skills.Count; i++)
            {
                if (skills[i].reanimated)
                    skills[i].SwitchTargetingTeam(false);
            }
        }
        else
        {
            for (int i = 0; i < skills.Count; i++)
            {
                skills[i].SwitchTargetingTeam(true);
            }
        }
    }

    public void SwitchTeams()
    {
        if (curUnitType == UnitType.PLAYER)
            curUnitType = UnitType.ENEMY;
        else
            curUnitType = UnitType.PLAYER;

        reanimated = true;

        CheckSwitchTeams();

        if (GameManager.Instance.activeRoomEnemies.Contains(this))
        {
            GameManager.Instance.activeRoomHeroes.Add(this);

            GameManager.Instance.activeRoomEnemies.Remove(this);
        }
        else if (GameManager.Instance.activeRoomHeroes.Contains(this))
        {
            GameManager.Instance.activeRoomEnemies.Add(this);
            GameManager.Instance.activeRoomHeroes.Remove(this);
        }

        GameManager.Instance.UpdateAllAlliesPosition(false, true, false, true);
    }

    public void ReviveUnit(int acc, bool fullhealth = false, bool enemy = false)
    {
        isDead = false;

        //Debug.Log("true2");

        selectUnitButton.ToggleButton(true);

        ToggleFighterRaceIcon(true);
        
        if (enemy)
            GameManager.Instance.AddUnitToTurnOrder(this);

        GameManager.Instance.AddActiveRoomAllUnitsFunctionality(this, enemy);

        if (GameManager.Instance.fallenEnemies.Contains(this))
            GameManager.Instance.fallenEnemies.Remove(this);

        GameManager.Instance.UpdateAllAlliesPosition(false, true, false, true);

        ToggleUnitDisplay(true);

        AudioManager.Instance.Play("SFX_HeroRevive");

        // Play revive SFX
        //AudioManager.Instance.Play(GameManager.Instance.GetActiveSkill().skillHit.name);
        if (GameManager.Instance.GetActiveSkill())
        {
            if (GameManager.Instance.GetActiveSkill().skillHitAdditional)
                AudioManager.Instance.Play(GameManager.Instance.GetActiveSkill().skillHitAdditional.name);
        }

        animator.SetTrigger("Idle");

        float valAcc = 0;
        float val = 0;
        if (GameManager.Instance.GetActiveSkill() != null)
        {
            // Heal ally
            valAcc = GameManager.Instance.GetActiveSkill().healPowerAmount + (acc * 15);
            val = (valAcc / 100f) * GetUnitMaxHealth();

            if (fullhealth)
            {
                //StartCoroutine(SpawnPowerUI(GetUnitMaxHealth(), false, false, null, false));
                UpdateUnitCurHealth((int)GetUnitMaxHealth(), false, false, false);
            }
            else
            {
                //StartCoroutine(SpawnPowerUI(val, false, false, null, false));
                UpdateUnitCurHealth((int)val, false, false, false);
            }
        }

        if (fullhealth)
        {
            //StartCoroutine(SpawnPowerUI(GetUnitMaxHealth(), false, false, null, false));
            UpdateUnitCurHealth((int)GetUnitMaxHealth(), false, false, false);
        }
        else
        {
            //StartCoroutine(SpawnPowerUI(val, false, false, null, false));
            UpdateUnitCurHealth((int)val, false, false, false);
        }
    }

    public void UpdateUnitExp(int gainedExp)
    {
        StartCoroutine(UpdateUnitExpVisual(gainedExp));
    }

    void ToggleExpGainedText(bool toggle, string text)
    {
        unitExpGainText.gameObject.SetActive(toggle);
        unitExpGainText.text = "+ " + text;
    }

    public void ToggleUnitBG(bool toggle)
    {
        if (unitBg == null)
            return;

        if (toggle)
            unitBg.UpdateAlpha(.1f);
        else
            unitBg.UpdateAlpha(0);
    }

    public IEnumerator UpdateUnitExpVisual(int gainedExp)
    {
        ToggleExpGainedText(true, gainedExp.ToString());

        float curFillAmount = GetCurExp() / GetMaxExp();

        // Update exp bar for current energy
        unitExpBarImage.fillAmount = curFillAmount;

        // Display exp visual
        ToggleUnitExpVisual(true);

        for (int i = 0; i < gainedExp; i++)
        {
            yield return new WaitForSeconds(GameManager.Instance.fillAmountIntervalTimeGap);

            // If unit leveled up
            if (GetCurExp() >= GetMaxExp())
            {
                int remainingExp = gainedExp - i;
                UpdateUnitLevel(1, remainingExp);

                TriggerTextAlert("LEVEL UP!", 1, false, "", true);

                // Level up SFX
                AudioManager.Instance.Play("LevelUp");

                // level up bonus
                UpdatePower(GetPowerIncPerLv());
                UpdateHealingPower(GetHealingPowerIncPerLv());
                UpdateSpeed(GetSpeedIncPerLv());
                UpdateDefense(GetDefenseIncPerLv());
                UpdateMaxHealth(GetMaxHealthIncPerLv());

                float levelUpHeal = ((float)GameManager.Instance.levelupHealPerc / 100f) * GetUnitMaxHealth();

                ResetPowerUI();

                UpdateUnitCurHealth((int)levelUpHeal, false, false);
                //StartCoroutine(SpawnPowerUI(levelUpHeal, false, false, null, false));

                // This isnt working
                //SpawnPowerUI(levelUpHeal, false, false, null, false);

                //UpdateUnitPowerHits(1, true);
                //UpdateUnitHealingHits(1, true);

                UpdateUnitLevelImage();
                yield break;
            }
            else
            {
                IncreaseCurExp(1);
                unitExpBarImage.fillAmount = GetCurExp() / GetMaxExp();
            }
        }

        yield return new WaitForSeconds(GameManager.Instance.timePostExp);


        // Enable post battle to map button for next post battle scene
        StartCoroutine(PostBattle.Instance.ToggleButtonPostBattleMap(true));
    }

    public void ToggleUnitExpVisual(bool toggle)
    {
        if (unitExpBar == null)
            return;

        if (toggle)
            unitExpBar.UpdateAlpha(1);
        else
            unitExpBar.UpdateAlpha(0);
    }

    public void UpdateUnitLevel(int level, int extraExp = 0, bool set = false)
    {
        if (!set)
            curLevel += level;
        else
            curLevel += level;

        UpdateUnitLevelVisual(curLevel);

        ResetUnitExp();
        UpdateUnitMaxExp();

        if (extraExp != 0)
        {
            //Debug.Log(gameObject.name + " extra exp = " + extraExp);
            UpdateUnitExp(extraExp);
        }

        //if (set)
        //UpdateUnitExp((int)GetMaxExp() * level);
    }

    void UpdateUnitLevelVisual(int level)
    {
        //Debug.Log(level);
        unitLevelText.text = level.ToString();
    }

    public int GetUnitLevel()
    {
        return curLevel;
    }

    void ResetUnitExp()
    {
        curExp = 0;
    }

    void IncreaseCurExp(float exp)
    {
        curExp += exp;

        if (GetCurExp() >= GetMaxExp())
        {
            curExp = (int)GetMaxExp();
        }
    }

    public void UpdateUnitMaxExp()
    {
        float temp;
        if (GetUnitLevel() != 1)
        {
            temp = GameManager.Instance.maxExpStarting + (GameManager.Instance.expIncPerLv * (GetUnitLevel() - 1));
            //temp = (GameManager.instance.maxExpLevel1 + ((GameManager.instance.expIncPerLv / GameManager.instance.maxExpLevel1) * 100f)) * GetUnitLevel();
            maxExp = (int)temp;
            //Debug.Log(gameObject.name + " " + maxExp);
        }
        else
        {
            temp = GameManager.Instance.maxExpStarting * GetUnitLevel();
            maxExp = (int)temp;
        }
    }

    public float GetCurExp()
    {
        return curExp;
    }

    public float GetMaxExp()
    {
        UpdateUnitMaxExp();
        return maxExp;
    }

    public void CleanseEffect(int count = 1)
    {
        List<Effect> effects = new List<Effect>();
        UnitFunctionality cleansingUnit = null;

        for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
        {
            if (GameManager.Instance.activeRoomHeroes[i].GetEffect("HOLY_LINK"))
            {
                cleansingUnit = GameManager.Instance.activeRoomHeroes[i];
            }
        }
        if (cleansingUnit != null)
        {
            for (int x = 0; x < cleansingUnit.activeEffects.Count; x++)
            {
                if (cleansingUnit.activeEffects[x].curEffectBenefitType == Effect.EffectBenefitType.DEBUFF)
                    effects.Add(cleansingUnit.activeEffects[x]);
            }
        }

        if (effects.Count >= 1 && cleansingUnit != null)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                if (i >= count)
                    break;

                int rand = Random.Range(0, cleansingUnit.activeEffects.Count);


                if (cleansingUnit.activeEffects[rand].curEffectBenefitType == Effect.EffectBenefitType.DEBUFF)
                {
                    cleansingUnit.activeEffects[rand].ReduceTurnCountText(GameManager.Instance.GetActiveUnitFunctionality());
                    cleansingUnit.TriggerTextAlert("CLEANSE", 1, true, "Trigger");
                }

            }
        }
    }

    public void UpdateUnitCurHealth(int power, bool damaging = false, bool setHealth = false, bool doExtras = true, bool triggerHitSFX = true, bool effect = false, bool isEffect = false)
    {
        if (isDead)
            return;

        //Debug.Log(gameObject.name + " " + power);

        float absPower = Mathf.Abs((float)power);

        if (!setHealth)
        {
            // Damaging
            if (damaging)
            {
                if (!isEffect)
                {
                    beenAttacked = true;
                }

                //float tempPower;
                //tempPower = (curRecieveDamageAmp / 100f) * absPower;
                //float newPower = absPower + tempPower;

                float newPower = 0;

                if (!TeamGearManager.Instance.playerInGearTab && !TeamItemsManager.Instance.playerInItemTab)
                {
                    if (GameManager.Instance.GetActiveSkill().isCleansingEffectRandom && GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE)
                    {
                        int rand = Random.Range(1, 101);
                        if (rand >= 25)
                            GameManager.Instance.GetActiveUnitFunctionality().CleanseEffect();
                    }

                    if (GetEffect("HOLY_LINK") && GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE)
                    {
                        //roll
                        int rand = Random.Range(1, 101);

                        int numb = GetEffect("HOLY_LINK").effectPowerStacks;

                        if (numb > 2)
                            numb = 2;

                        rand += numb * (int)GetEffect("HOLY_LINK").powerPercent;

                        if (rand >= 60)
                            rand = 60;

                        if (rand >= GetEffect("HOLY_LINK").powerPercent)
                        {
                            if (holyLinkPartner != null)
                            {
                                if (!holyLinkPartner.isDead)
                                {
                                    holyLinkPartner.curHealth -= (int)absPower;
                                    holyLinkPartner.StartCoroutine(holyLinkPartner.SpawnPowerUI((int)absPower, false, true, null, false, true));

                                    holyLinkPartner.UpdateUnitHealthVisual(effect);

                                    holyLinkPartner.animator.SetBool("DamageFlg", true);

                                    if (holyLinkPartner.GetHitFlash() == null)
                                    {

                                    }
                                    else
                                        holyLinkPartner.GetHitFlash().Flash();

                                    holyLinkPartner.uiElement.AnimateUI(false);

                                    if (doExtras)
                                    {
                                        CameraShake.instance.EnableCanShake();

                                        if (GameManager.Instance.GetActiveSkill().repeatLaunchSFX)
                                        {
                                            if (triggerHitSFX && GameManager.Instance.GetActiveSkill().skillHit != null)
                                                AudioManager.Instance.Play(GameManager.Instance.GetActiveSkill().skillHit.name);
                                        }

                                        holyLinkPartner.StartCoroutine(holyLinkPartner.PlaySoundDelay(.1f));

                                        holyLinkPartner.TriggerTextAlert("HOLY LINK", 1, true, "Trigger");
                                    }

                                    holyLinkPartner.StartCoroutine(holyLinkPartner.PlayIdleAnimation());

                                    return;
                                }
                            }
                        }

                    }

                    if (GetEffect("IMMUNITY"))
                    {
                        absPower = 0;

                        //GameManager.Instance.GetActiveUnitFunctionality().UpdateUnitCurHealth((int)finalHealingPower, false, false, true, true, true);
                        StartCoroutine(SpawnPowerUI((int)absPower, false, false, null, false, true));
                        GameManager.Instance.GetActiveUnitFunctionality().TriggerTextAlert("IMMUNITY", 1, true, "Trigger", false, false);
                        //Debug.Log("unit name " + GameManager.Instance.GetActiveUnitFunctionality().GetUnitName());

                        UpdateUnitHealthVisual(effect);
                    }

                    // if this unit has Reaping, heal caster
                    if (GetEffect("REAPING"))
                    {
                        for (int i = 0; i < activeEffects.Count; i++)
                        {
                            if (activeEffects[i].curEffectName == Effect.EffectName.REAPING)
                            {
                                if (power != 0)
                                {
                                    float number = activeEffects[i].effectPowerStacks * GetEffect("REAPING").powerPercent;

                                    if (number >= 80)
                                        number = 80;

                                    newPower = (number / 100f) * power;
                                    float finalHealingPower = newPower;

                                    float finalHealingPower1 = 0;

                                    if (GameManager.Instance.GetActiveUnitFunctionality().GetEffect("POISON"))
                                    {
                                        newPower = ((GameManager.Instance.GetActiveUnitFunctionality().GetEffect("POISON").effectPowerStacks * GameManager.Instance.GetActiveUnitFunctionality().GetEffect("POISON").powerPercent) / 100f) * power;
                                        finalHealingPower1 = power - newPower;
                                    }
                                    else
                                        finalHealingPower1 = newPower;


                                    if (GameManager.Instance.GetActiveUnitFunctionality().curHealth < GameManager.Instance.GetActiveUnitFunctionality().curMaxHealth)
                                        GameManager.Instance.GetActiveUnitFunctionality().curHealth += (int)finalHealingPower;

                                    if (GameManager.Instance.GetActiveUnitFunctionality().curHealth > GameManager.Instance.GetActiveUnitFunctionality().curMaxHealth)
                                        GameManager.Instance.GetActiveUnitFunctionality().curHealth = GameManager.Instance.GetActiveUnitFunctionality().curMaxHealth;


                                    //GameManager.Instance.GetActiveUnitFunctionality().UpdateUnitCurHealth((int)finalHealingPower, false, false, true, true, true);
                                    StartCoroutine(GameManager.Instance.GetActiveUnitFunctionality().SpawnPowerUI((int)finalHealingPower1, false, false, null, false, true));

                                    GameManager.Instance.GetActiveUnitFunctionality().TriggerTextAlert("REAPING", 1, true, "Trigger", false, false);
                                    //Debug.Log("unit name " + GameManager.Instance.GetActiveUnitFunctionality().GetUnitName());

                                    GameManager.Instance.GetActiveUnitFunctionality().UpdateUnitHealthVisual(effect);
                                    break;
                                }
                            }
                        }
                    }
                }

               
                curHealth -= (int)absPower;

                if (!TeamGearManager.Instance.playerInGearTab)
                {
                    // If no effect
                    if (!GetEffect("POISON") && !GetEffect("BLEED") && !GetEffect("IMMUNITY"))
                        StartCoroutine(SpawnPowerUI((int)absPower, false, true, null, false));
                    else if (GetEffect("POISON"))
                        StartCoroutine(SpawnPowerUI((int)absPower, false, true, GetEffect("POISON"), false));
                    else if (GetEffect("BLEED"))
                        StartCoroutine(SpawnPowerUI((int)absPower, false, true, GetEffect("BLEED"), false));
                }


                if (curHealth < 0)
                    curHealth = 0;

                //if (curHealth > 0)

                // If the hit wasnt a miss, or 0 dmg, cause hit recieved animation
                if (power != 0 && !TeamGearManager.Instance.playerInGearTab)
                {
                    animator.SetBool("DamageFlg", true);

                    if (GetHitFlash() == null)
                        return;
                    else
                        GetHitFlash().Flash();

                    uiElement.AnimateUI(false);

                    if (doExtras)
                    {
                        CameraShake.instance.EnableCanShake();
                       
                        if (GameManager.Instance.GetActiveSkill().repeatLaunchSFX)
                        {
                            if (triggerHitSFX && GameManager.Instance.GetActiveSkill().skillHit != null)
                                AudioManager.Instance.Play(GameManager.Instance.GetActiveSkill().skillHit.name);
                        }
                        
                        StartCoroutine(PlaySoundDelay(.1f));
                    }
                }

                StartCoroutine(PlayIdleAnimation());
            }

            // Healing
            else
            {
                // if this unit has poison, heal less
                if (GetEffect("POISON") && !TeamGearManager.Instance.playerInGearTab)
                {
                    float newPower = 0;

                    for (int i = 0; i < activeEffects.Count; i++)
                    {
                        if (activeEffects[i].curEffectName == Effect.EffectName.POISON)
                        {
                            if (GameManager.Instance.isSkillsMode)
                            {
                                if (power != 0 && GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT)
                                {
                                    newPower = ((activeEffects[i].effectPowerStacks * GetEffect("POISON").powerPercent) / 100f) * power;
                                    float finalHealingPower2 = power - newPower;

                                    //absPower *= curHealingRecieved;

                                    if (curHealth < curMaxHealth)
                                        curHealth += (int)finalHealingPower2;

                                    if (curHealth > curMaxHealth)
                                        curHealth = curMaxHealth;

                                    //GameManager.Instance.GetActiveUnitFunctionality().UpdateUnitCurHealth((int)finalHealingPower, false, false, true, true, true);
                                    StartCoroutine(SpawnPowerUI((int)finalHealingPower2, false, false, null, false));

                                    break;
                                }
                            }
                            else
                            {
                                if (power != 0 && GameManager.Instance.GetActiveItem().curItemType == ItemPiece.ItemType.SUPPORT)
                                {
                                    newPower = ((activeEffects[i].effectPowerStacks * GetEffect("POISON").powerPercent) / 100f) * power;
                                    float finalHealingPower2 = power - newPower;

                                    //absPower *= curHealingRecieved;

                                    if (curHealth < curMaxHealth)
                                        curHealth += (int)finalHealingPower2;

                                    if (curHealth > curMaxHealth)
                                        curHealth = curMaxHealth;

                                    //GameManager.Instance.GetActiveUnitFunctionality().UpdateUnitCurHealth((int)finalHealingPower, false, false, true, true, true);
                                    StartCoroutine(SpawnPowerUI((int)finalHealingPower2, false, false, null, false));

                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (curHealth < curMaxHealth)
                        curHealth += (int)absPower;

                    if (curHealth > curMaxHealth)
                        curHealth = curMaxHealth;

                    if (!TeamGearManager.Instance.playerInGearTab)
                        StartCoroutine(SpawnPowerUI((int)absPower, false, false, null, false));
                }
            }
        }
        else
        {
            curHealth = (int)absPower;

            if (curHealth > curMaxHealth)
                curHealth = curMaxHealth;

            if (curHealth < 0)
                curHealth = 0;
        }

        UpdateUnitHealthVisual(effect);
    }

    IEnumerator PlaySoundDelay(float time, bool damage = true)
    {
        yield return new WaitForSeconds(time);

        if (damage)
            AudioManager.Instance.Play(hitRecievedClip.name);
    }

    IEnumerator PlayIdleAnimation()
    {
        yield return new WaitForSeconds(.75f);
        ToggleDamageAnimOff();
    }
    public void ToggleDamageAnimOff()
    {
        animator.SetBool("DamageFlg", false);
    }

    public void UpdateUnitMaxHealth(int newMaxHealth, bool set = false, bool inc = true)
    {
        //maxHealth = newMaxHealth;

        if (inc)
        {
            if (set)
            {
                curMaxHealth = newMaxHealth;
                curHealth = curMaxHealth;
            }

            else
                curMaxHealth += newMaxHealth;
        }
        else
        {
            if (set)
                curMaxHealth = newMaxHealth;
            else
                curMaxHealth -= newMaxHealth;
        }

        if (curHealth > curMaxHealth)
            curHealth = curMaxHealth;

        UpdateUnitHealthVisual(true);
    }

    void UpdateUnitHealthVisual(bool effect = false)
    {
        ToggleUnitHealthBar(true);

        unitHealthBar.fillAmount = (float)curHealth / (float)curMaxHealth;

        if (CheckIfUnitIsDead() && !isDead)
            StartCoroutine(EnsureUnitIsDead(effect));
    }

    public int GetCurAttackCharge()
    {
        return curAttackCharge;
    }

    public void CalculateUnitAttackChargeTurnStart()
    {
        attackChargeTurnStart = curSpeed;
    }

    public void ResetUnitCurAttackCharge()
    {
        curAttackCharge = 0;

        attackChargeTurnStart = 0;

        UpdateUnitAttackBarVisual();
        UpdateUnitAttackBarNextVisual();
    }

    public void ResetAttackChargeTurnStart()
    {
        attackChargeTurnStart = 0;
    }
    public void UpdateUnitCurAttackCharge()
    {
        ResetAttackChargeTurnStart();

        CalculateUnitAttackChargeTurnStart();

        // If unit is player, give more exp the lower allies there are on team


        if (GameManager.Instance.activeRoomEnemies.Count == 1)
        {
            attackChargeTurnStart *= 6;
        }

        else if (GameManager.Instance.activeRoomEnemies.Count == 2)
        {
            attackChargeTurnStart *= 6;
        }

        else if (GameManager.Instance.activeRoomEnemies.Count == 3)
        {
            attackChargeTurnStart *= 6;
        }

        else if (GameManager.Instance.activeRoomEnemies.Count == 4)
        {
            attackChargeTurnStart *= 6;
        }

        else if (GameManager.Instance.activeRoomEnemies.Count == 5)
        {
            if (curUnitType == UnitType.ENEMY)
                attackChargeTurnStart *= 5;
            else
                attackChargeTurnStart *= 6;
        }

        else if (GameManager.Instance.activeRoomEnemies.Count == 6)
        {
            if (curUnitType == UnitType.ENEMY)
                attackChargeTurnStart *= 4;
            else
                attackChargeTurnStart *= 6;
        }

        int diff = GameManager.Instance.activeRoomEnemies.Count - GameManager.Instance.activeRoomHeroes.Count;
        if (diff > 1 && curUnitType == UnitType.ENEMY)
        {
            attackChargeTurnStart /= 4;
        }
        else if (diff <= 1 || curUnitType == UnitType.PLAYER)
        {
            attackChargeTurnStart /= 6;
        }

        //Debug.Log(GetUnitName() + " 's attack charge = " + attackChargeTurnStart);
        
        curAttackCharge += attackChargeTurnStart;

        if (curAttackCharge > 100)
            curAttackCharge = 100;

        UpdateUnitAttackBarVisual();
        UpdateUnitAttackBarNextVisual();
    }

    void UpdateUnitAttackBarVisual()
    {
        ToggleUnitAttackBar(true);

        //Debug.Log(GetUnitName() + " Attack Bar = " + (float)curAttackCharge / 100f);
        unitAttackBar.fillAmount = (float)curAttackCharge / 100f;
    }
    public void UpdateUnitAttackBarNextVisual()
    {
        //ToggleUnitAttackBar(true);
        //Debug.Log(GetUnitName() + " Next Bar = " + (float)attackChargeTurnStart / 100f);
        unitAttackBarNext.fillAmount = ((float)curAttackCharge + (float)attackChargeTurnStart) / 100f;
    }

    public void ToggleUnitHealthBar(bool toggle)
    {
        if (toggle)
            healthBarUIElement.UpdateAlpha(1);
        else
            healthBarUIElement.UpdateAlpha(0);  
    }

    public void ToggleActionNextBar(bool toggle)
    {
        if (toggle)
            unitAttackBarNextUIElement.UpdateAlpha(1);
        else
            unitAttackBarNextUIElement.UpdateAlpha(0);
    }

    public void ToggleUnitAttackBar(bool toggle)
    {
        if (toggle)
            attackBarUIElement.UpdateAlpha(1);
        else
            attackBarUIElement.UpdateAlpha(0);
    }

    public void UpdateUnitSpeed(int newSpeed, bool set = true)
    {
        if (set)
            curSpeed = newSpeed;
        else
            curSpeed += newSpeed;
    }

    public void UpdateUnitSpeedChange(int newSpeed, bool inc)
    {
        if (inc)
            curSpeed += newSpeed;
        else
            curSpeed -= newSpeed;
    }

    public int GetUnitSpeed()
    {
        return curSpeed;
    }

    public void UpdateUnitOldSpeed(int oldSpeed)
    {
        oldCurSpeed = oldSpeed;
    }

    /*
    public float GetOldSpeed()
    {
        return oldCurSpeed;
    }

    public void ResetOldSpeed()
    {
        oldCurSpeed = 0;
    }
    */

    public void UpdateUnitOldDefense(int def)
    {
        oldCurDefense = def;
    }

    public float GetOldDefense()
    {
        return oldCurDefense;
    }

    public void ResetOldDefense()
    {
        oldCurDefense = 0;
    }

    public bool GetIsSpeedUp()
    {
        return isSpeedUp;
    }

    public bool GetIsSpeedDown()
    {
        return isSpeedDown;
    }

    public bool GetIsDefenseUp()
    {
        return isDefenseUp;
    }

    public bool GetIsDefenseDown()
    {
        return isDefenseDown;
    }

    public void ToggleIsDefenseUp(bool toggle)
    {
        if (toggle)
            isDefenseUp = true;
        else
            isDefenseUp = false;
    }

    public void ToggleIsDefenseDown(bool toggle)
    {
        if (toggle)
            isDefenseDown = true;
        else
            isDefenseDown = false;
    }

    public void ToggleIsSpeedUp(bool toggle)
    {
        if (toggle)
            isSpeedUp = true;
        else
            isSpeedUp = false;
    }

    public void ToggleIsSpeedDown(bool toggle)
    {
        if (toggle)
            isSpeedDown = true;
        else
            isSpeedDown = false;
    }

    public void UpdateUnitPower(int newPower, bool setting = true, bool adding = true)
    {
        if (adding)
        {
            if (setting)
                curPower = newPower;
            else
                curPower += newPower;
        }
        else
        {
            if (setting)
                curPower = newPower;
            else
                curPower -= newPower;
        }
    }

    public void UpdateUnitHealingPower(int newHealingPower)
    {
        curHealingPower = newHealingPower;
    }

   
    public void UpdateUnitDefense(int newDefense, bool set = true)
    {
        if (set)
            curDefense = newDefense;
        else
            curDefense += newDefense;
    }

    public void ResetUnitDefense()
    {
        curDefense = startingRoomDefense;
    }

    public void UpdateUnitDefenseChange(int newDef, bool inc)
    {
        if (inc)
            curDefense += newDef;
        else
            curDefense -= newDef;
    }

    public float GetCurDefense()
    {
        return curDefense;
    }

    public float GetBlockChance()
    {
        //def calculate_block_chance(defense_level):
        float a = 9;
        float b = 0.000003f;

        float block_chance = (a * Mathf.Log(GetCurDefense() + 1)) / (b * GetCurDefense() + 1);

        block_chance -= 10;

        //Debug.Log("Block Chance " + block_chance);

        if (block_chance < 0)
            block_chance = 0;

        return block_chance;   
    }

    public float GetStartingRoomDefense()
    {
        return startingDefense;
    }
    
    public void UpdateUnitPowerHits(int newDmgHits, bool inc = true)
    {
        //Debug.Log(GetUnitName() + " updating power hits");
        if (inc)
            curPowerHits += newDmgHits;
        else
            curPowerHits -= newDmgHits;

        if (GameManager.Instance.isSkillsMode)
        {
            if (!HeroRoomManager.Instance.playerInHeroRoomView && GameManager.Instance.playerInCombat)
                GameManager.Instance.UpdateMainIconDetails(GameManager.Instance.GetActiveSkill(), null, true);
        }
    }

    public int GetUnitPowerHits()
    {
        return curPowerHits;
    }

    public void UpdateUnitHealingHits(int newHealHits, bool inc = true)
    {
        if (inc)
            curHealingHits += newHealHits;
        else
            curHealingHits -= newHealHits;
    }

    public int GetUnitHealingHits()
    {
        return curHealingHits;
    }
    /*
    public void AddUnitDefense(int armor)
    {
        curDefense += armor;
    }

    public void RemoveUnitArmor(int armor)
    {
        curDefense -= armor;
    }
    */

    public void UpdateUnitHealth(int newCurHealth, int newMaxHealth)
    {
        UpdateUnitMaxHealth(newMaxHealth, true);
        UpdateUnitCurHealth(newCurHealth, false, true);
    }

    public float GetUnitCurHealth()
    {
        return curHealth;
    }

    public float GetUnitMaxHealth()
    {
        return curMaxHealth;
    }

    /*
    public void UpdateUnitEnergy(int curEnergy, int maxEnergy)
    {
        this.curEnergy = curEnergy;
        this.maxEnergy = maxEnergy;
    }

    public void UpdateMaxEnergy(int maxEnergy)
    {
        this.maxEnergy = maxEnergy;
    }
    */

    /*
    public void UpdateUnitCurEnergy(int energy)
    {
        //effects.UpdateAlpha(1);

        this.curEnergy += energy;

        if (curEnergy > maxEnergy)
            curEnergy = maxEnergy;
    }
    */

    /*
    public void UpdateUnitStartTurnEnergy(int newUnitStartTurnEnergyGain)
    {
        unitStartTurnEnergyGain = newUnitStartTurnEnergyGain;
    }
    */

    public void ToggleUnitDisplay(bool toggle)
    {
        //Debug.Log("toggling " + toggle);

        if (toggle)
            unitVisuals.UpdateAlpha(1);
        else
        {
            unitVisuals.UpdateAlpha(0);
            ResetEffects();
        }
        //GameManager.instance.UpdateTurnOrder();
        //Destroy(gameObject);
    }

    public int GetUnitExpKillGained()
    {
        int expGained = (GetUnitLevel() * GameManager.Instance.expKillGainedPerLv) + GameManager.Instance.expKillGainedStarting;

        expGained = Random.Range(expGained - 2, expGained + 6);

        return expGained;


    }
}
