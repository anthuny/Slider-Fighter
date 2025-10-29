using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class OverlayUI : MonoBehaviour
{

    public static OverlayUI Instance;

    [SerializeField] private UIElement activeItemTriggerStatus;
    [SerializeField] private UIElement activeItemUseCountText;

    [SerializeField] private UIElement activeItemRaceSpecificIcon;

    [SerializeField] private Color activeItemTextColour;
    [SerializeField] private Color passiveItemTextColour;
    [SerializeField] private string targetCountTextColour;
    [SerializeField] private string damagingTextColour;
    [SerializeField] private string healingTextColour;
    [SerializeField] private string damageWordTextColour;
    [SerializeField] private string healWordTextColour;
    [SerializeField] private string skillMultihitColour;
    public Sprite healthSprite;
    public TextMeshProUGUI skillDetailsName;
    public TextMeshProUGUI skillDetailsDesc;

    public UIElement skillDetailsPower;
    public UIElement skillDetailsHitsRemaininguI;
    public UIElement skillDetailsBaseHitsUI;
    public UIElement skillDetailsMaxCdUi;
    public UIElement skillDetailsAccuracyuI;
    public UIElement skillDetailsRangeuI;
    public UIElement skillDetailsHitAreauI;
    public UIElement skillsItemsSwitchButton;

    public Sprite hitArea1x1;
    public Sprite hitArea2x1;
    public Sprite hitArea3x1;
    public Sprite hitArea1x2;
    public Sprite hitArea1x3;
    public Sprite hitArea2x2;
    public Sprite hitArea2x3;
    public Sprite hitArea3x2;
    public Sprite hitArea3x3;
    public Sprite hitAreaPlus;

    public Image skillDetailsPowerIcon;
    public Image activeObjectIcon;

    public TextMeshProUGUI unitOverlayCurEnergyText;
    public TextMeshProUGUI unitOverlayCurHealthText;
    public Image unitOverlayCurEnergyImage;
    public Image unitOverlayCurHealthImage;

    public UIElement itemRarityTextUI;
    public TextMeshProUGUI itemRarityText;
    [SerializeField] private UIElement remainingMovementUsesUI;
    [SerializeField] private UIElement remainingMovementUsesText;
    public UIElement extraMovePrompt;
    public CanvasGroup cg;

    public UIElement shopDetailsUI;
    public UIElement combatDetailsUI;

    public UIElement gearDetailsTopBanner;
    public UIElement gearDetailsBottomBanner;

    public UIElement gearDetailsHealthStatUI;
    public UIElement gearDetailsDamageStatUI;
    public UIElement gearDetailsHealingStatUI;
    public UIElement gearDetailsDefenseStatUI;
    public UIElement gearDetailsSpeedStatUI;
    public UIElement gearDetailsActiveGearTypeUI;

    [SerializeField] private Color powerDamageColour;
    [SerializeField] private Color powerHealColour;

    [SerializeField] private UIElement combatDetailsParent;

    public UIElement buttonCloseDetails;
    public UIElement buttonSellItemDetails;
    public UIElement sellItemCostText;

    [SerializeField] private UIElement combatBorderParent;
    [SerializeField] private UIElement topBorder;
    [SerializeField] private UIElement botBorder;
    [SerializeField] private UIElement leftBorder;
    [SerializeField] private UIElement rightBorder;

    [SerializeField] private UIElement enemiesRemainingText;

    public void UpdateEnemiesRemainingText(int count)
    {
        if (count != 0)
        {
            ToggleEnemiesRemainingText(true);

            if (enemiesRemainingText.contentText.text != count.ToString())
                enemiesRemainingText.AnimateUI(true);

            enemiesRemainingText.UpdateContentText(count.ToString());
        }
        else
        {
            ToggleEnemiesRemainingText(false);
            enemiesRemainingText.UpdateContentText("");
        }

    }

    public void ToggleEnemiesRemainingText(bool toggle = true)
    {
        if (toggle)
        {
            enemiesRemainingText.UpdateAlpha(1);
        }
        else
        {
            enemiesRemainingText.UpdateAlpha(0);
        }
    }

    public void ToggleCombatBorder(bool toggle = true)
    {
        if (toggle)
        {
            UpdateCombatBorderColour();
            combatBorderParent.UpdateAlpha(1);
        }
        else
        {
            combatBorderParent.UpdateAlpha(0);
        }
    }

    public void UpdateCombatBorderColour()
    {
        if (CombatGridManager.Instance.isCombatMode)
        {
            if (GameManager.Instance.isSkillsMode)
            {
                topBorder.UpdateColour(GameManager.Instance.skillsDetailsTabColour);
                botBorder.UpdateColour(GameManager.Instance.skillsDetailsTabColour);
                leftBorder.UpdateColour(GameManager.Instance.skillsDetailsTabColour);
                rightBorder.UpdateColour(GameManager.Instance.skillsDetailsTabColour);
            }
            else
            {
                topBorder.UpdateColour(GameManager.Instance.itemsDetailsTabColour);
                botBorder.UpdateColour(GameManager.Instance.itemsDetailsTabColour);
                leftBorder.UpdateColour(GameManager.Instance.itemsDetailsTabColour);
                rightBorder.UpdateColour(GameManager.Instance.itemsDetailsTabColour);
            }
        }
        else
        {
            topBorder.UpdateColour(GameManager.Instance.movementDetailsTabColour);
            botBorder.UpdateColour(GameManager.Instance.movementDetailsTabColour);
            leftBorder.UpdateColour(GameManager.Instance.movementDetailsTabColour);
            rightBorder.UpdateColour(GameManager.Instance.movementDetailsTabColour);
        }
    }

    public void ToggleShopDetailsActiveGearType(bool toggle = true)
    {
        if (toggle)
        {
            gearDetailsActiveGearTypeUI.UpdateAlpha(1);
        }
        else
        {
            gearDetailsActiveGearTypeUI.UpdateAlpha(0);
        }
    }
    public void ToggleShopDetailsItemRarityText(bool toggle = true)
    {
        if (toggle)
        {
            itemRarityTextUI.UpdateAlpha(1);
        }
        else
        {
            itemRarityTextUI.UpdateAlpha(0);
        }
    }

    public void UpdateSellItemPrice(int newCost = 0)
    {
        sellItemCostText.UpdateContentText(newCost.ToString());
    }

    public void ToggleSellItemCostText(bool toggle = true)
    {
        if (toggle)
            sellItemCostText.UpdateAlpha(1);
        else
            sellItemCostText.UpdateAlpha(0);
    }

    public void ToggleCombatDetailsParent(bool toggle = true)
    {
        if (toggle)
            combatDetailsParent.UpdateAlpha(1);
        else
            combatDetailsParent.UpdateAlpha(0);
    }

    public void ToggleCombatDetailsGO(bool toggle = true)
    {
        combatDetailsParent.transform.gameObject.SetActive(toggle);
    }

    private void Start()
    {
        ToggleShopDetailsBanner(false);
        ToggleCombatDetailsBanner(false);
    }

    public void ToggleShopDetailsBanner(bool toggle = true)
    {
        if (toggle)
        {
            shopDetailsUI.UpdateAlpha(1);
            gearDetailsTopBanner.UpdateAlpha(1);
            gearDetailsBottomBanner.UpdateAlpha(1);

            buttonCloseDetails.UpdateAlpha(1);
            buttonCloseDetails.ToggleButton(true);

            ToggleShopDetailsActiveGearType(true);
            ToggleShopDetailsItemRarityText(true);
        }
        else
        {
            shopDetailsUI.UpdateAlpha(0);

            buttonCloseDetails.UpdateAlpha(0);
            buttonCloseDetails.ToggleButton(false);

            buttonSellItemDetails.UpdateAlpha(0);
            buttonSellItemDetails.ToggleButton(false);
            ToggleSellItemCostText(false);
            ToggleShopDetailsActiveGearType(false);
            ToggleShopDetailsItemRarityText(false);
        }
    }

    public void ToggleSellButton(bool toggle = true)
    {
        if (toggle)
        {
            buttonSellItemDetails.UpdateAlpha(1);
            buttonSellItemDetails.ToggleButton(toggle);
        }
        else
        {
            buttonSellItemDetails.UpdateAlpha(0);
            buttonSellItemDetails.ToggleButton(toggle);
        }
    }

    public void ToggleCombatDetailsBanner(bool toggle = true)
    {
        if (toggle)
        {
            combatDetailsUI.UpdateAlpha(1);

            buttonCloseDetails.UpdateAlpha(1);
            buttonCloseDetails.ToggleButton(true);

            buttonSellItemDetails.UpdateAlpha(0);
            buttonSellItemDetails.ToggleButton(false);
        }
        else
        {
            combatDetailsUI.UpdateAlpha(0);

            buttonCloseDetails.UpdateAlpha(0);
            buttonCloseDetails.ToggleButton(false);

            buttonSellItemDetails.UpdateAlpha(0);
            buttonSellItemDetails.ToggleButton(false);
        }
    }

    public void UpdateShopDetailsBanner(ShopItem shopItem = null, GearPiece gearPiece = null, ItemPiece itemPiece = null)
    {
        if (shopItem)
        {
            if (shopItem.GetPurchased())
            {
                gearDetailsActiveGearTypeUI.UpdateContentText("");
                gearDetailsActiveGearTypeUI.UpdateContentImage(TeamItemsManager.Instance.clearSlotSprite);
                gearDetailsHealthStatUI.UpdateContentText("");
                gearDetailsHealthStatUI.UpdateContentImage(TeamItemsManager.Instance.clearSlotSprite);
                gearDetailsDamageStatUI.UpdateContentText("");
                gearDetailsHealingStatUI.UpdateContentText("");
                gearDetailsDefenseStatUI.UpdateContentText("");
                gearDetailsSpeedStatUI.UpdateContentText("");
                ToggleShopDetailsBanner(false);
                return;
            }
        }

        ToggleOverlay(true);
        ToggleShopDetailsBanner(true);

        GearPiece gear = null;
        ItemPiece item = null;

        if (shopItem)
        {
            if (shopItem.linkedGearPiece != null)
                gear = shopItem.linkedGearPiece;
            else if (shopItem.linkedItemPiece)
            {
                item = shopItem.linkedItemPiece;
            }
        }
        else
        {
            if (gearPiece)
                gear = gearPiece;
            else if (itemPiece)
                item = itemPiece;
        }

        if (gear)
        {
            UpdateActiveItemUseCountText(0);
            UpdateActiveItemTriggerStatus(false);

            gearDetailsHealthStatUI.contentImageUI.UpdateAlpha(1);
            gearDetailsHealthStatUI.UpdateContentText(gear.bonusHealth.ToString());
            gearDetailsHealthStatUI.UpdateContentImage(healthSprite);
            gearDetailsDamageStatUI.UpdateContentText(gear.bonusDamage.ToString());
            gearDetailsHealingStatUI.UpdateContentText(gear.bonusHealing.ToString());
            gearDetailsDefenseStatUI.UpdateContentText(gear.bonusDefense.ToString());
            gearDetailsSpeedStatUI.UpdateContentText(gear.bonusSpeed.ToString());

            gearDetailsActiveGearTypeUI.UpdateContentTextColour(TeamGearManager.Instance.gearIconColour);

            if (gear.gearType == "neckless" || gear.gearType == "pendant")
            {
                gearDetailsActiveGearTypeUI.UpdateContentText("Pendant");
                gearDetailsActiveGearTypeUI.UpdateContentImage(TeamGearManager.Instance.necklessSlotSprite);
            }
            else if (gear.gearType == "earring")
            {
                gearDetailsActiveGearTypeUI.UpdateContentText("Earring");
                gearDetailsActiveGearTypeUI.UpdateContentImage(TeamGearManager.Instance.earringSlotSprite);
            }
            else if (gear.gearType == "belt")
            {
                gearDetailsActiveGearTypeUI.UpdateContentText("belt");
                gearDetailsActiveGearTypeUI.UpdateContentImage(TeamGearManager.Instance.beltSlotSprite);
            }
            else if (gear.gearType == "glove")
            {
                gearDetailsActiveGearTypeUI.UpdateContentText("glove");
                gearDetailsActiveGearTypeUI.UpdateContentImage(TeamGearManager.Instance.gloveSlotSprite);
            }
            else if (gear.gearType == "ring")
            {
                gearDetailsActiveGearTypeUI.UpdateContentText("ring");
                gearDetailsActiveGearTypeUI.UpdateContentImage(TeamGearManager.Instance.ringSlotSprite);
            }
            else if (gear.gearType == "helmet")
            {
                gearDetailsActiveGearTypeUI.UpdateContentText("helmet");
                gearDetailsActiveGearTypeUI.UpdateContentImage(TeamGearManager.Instance.helmetSlotSprite);
            }
            else if (gear.gearType == "chestpiece")
            {
                gearDetailsActiveGearTypeUI.UpdateContentText("chest");
                gearDetailsActiveGearTypeUI.UpdateContentImage(TeamGearManager.Instance.chestSlotSprite);
            }
            else if (gear.gearType == "boots")
            {
                gearDetailsActiveGearTypeUI.UpdateContentText("boots");
                gearDetailsActiveGearTypeUI.UpdateContentImage(TeamGearManager.Instance.bootsSlotSprite);
            }

            ToggleCombatDetailsBanner(false);
            ToggleShopDetailsBanner(true);
        }
        else
        // item
        {

            gearDetailsActiveGearTypeUI.UpdateContentText("Item");
            gearDetailsActiveGearTypeUI.UpdateContentTextColour(GameManager.Instance.itemsDetailsTabColour);
            gearDetailsActiveGearTypeUI.UpdateContentImage(item.itemSpriteItemTab);

            gearDetailsActiveGearTypeUI.UpdateContentImage(TeamItemsManager.Instance.clearSlotSprite);


            gearDetailsDamageStatUI.UpdateContentText("");
            gearDetailsHealingStatUI.UpdateContentText("");
            gearDetailsDefenseStatUI.UpdateContentText("");
            gearDetailsSpeedStatUI.UpdateContentText("");

            //ToggleShopDetailsBanner(false);
            ToggleCombatDetailsBanner(true);

            UpdateItemDetailsUI(item.itemName, item.itemDesc, item.itemPower, item.range, item.itemRangeHitArea, item.itemSpriteCombat);

            gearDetailsHealthStatUI.UpdateContentText("");
            gearDetailsHealthStatUI.UpdateContentImage(TeamItemsManager.Instance.clearSlotSprite);

            gearDetailsHealthStatUI.contentImageUI.UpdateAlpha(0);
        }
    }

    public void ToggleOverlay(bool toggle = true)
    {
        if (toggle)
        {
            GameManager.Instance.ToggleCombatSkillIcons(true);
            cg.alpha = 1;
        }
        else
        {
            cg.alpha = 0;
            CombatGridManager.Instance.ResetVirtCam();
            GameManager.Instance.ToggleCombatSkillIcons(false);
            CombatGridManager.Instance.ToggleTabButtons("", true);
            CombatGridManager.Instance.ToggleScaleButtons(false);
            GameManager.Instance.UpdateEndTurnButton(false);
            CombatGridManager.Instance.DisableAllButtons();
        }
    }

    void Awake()
    {
        Instance = this;
    }

    public void UpdateActiveItemRaceSpecificIcon(string raceSpecific = "")
    {
        if (raceSpecific == "" || raceSpecific == "ALL")
        {
            activeItemRaceSpecificIcon.UpdateAlpha(0);
            activeItemRaceSpecificIcon.ToggleButton(false);
        }
        else
        {
            activeItemRaceSpecificIcon.UpdateAlpha(1);
            activeItemRaceSpecificIcon.ToggleButton(true);

            string text = "";

            if (raceSpecific == "HUMAN")
            {
                activeItemRaceSpecificIcon.UpdateContentImage(GameManager.Instance.humanRaceIcon);
                activeItemRaceSpecificIcon.contentImageUI.UpdateColour(GameManager.Instance.humanRaceColour);
                text = "Item can only be equipped by humans";
            }
            else if (raceSpecific == "BEAST")
            {
                activeItemRaceSpecificIcon.UpdateContentImage(GameManager.Instance.beastRaceIcon);
                activeItemRaceSpecificIcon.contentImageUI.UpdateColour(GameManager.Instance.beastRaceColour);
                text = "Item can only be equipped by beasts";
            }
            else if (raceSpecific == "ETHEREAL")
            {
                activeItemRaceSpecificIcon.UpdateContentImage(GameManager.Instance.etherealRaceIcon);
                activeItemRaceSpecificIcon.contentImageUI.UpdateColour(GameManager.Instance.etherealRaceColour);
                text = "Item can only be equipped by ethereal";
            }

            activeItemRaceSpecificIcon.tooltipStats.UpdateTooltipStatsText(text);
        }
    }

    public void ToggleItemRarityTextUI(bool toggle = true)
    {
        if (toggle)
        {
            itemRarityTextUI.UpdateAlpha(1);
        }
        else
        {
            itemRarityTextUI.UpdateAlpha(0);
        }
    }

    public void UpdateItemRarityText(string text)
    {
        itemRarityText.text = text;

        if (text == "COMMON" || text == "common")
            itemRarityText.color = ItemRewardManager.Instance.commonColour;
        else if (text == "RARE" || text == "rare")
            itemRarityText.color = ItemRewardManager.Instance.rareColour;
        else if (text == "EPIC" || text == "epic")
            itemRarityText.color = ItemRewardManager.Instance.epicColour;
        else if (text == "LEGENDARY" || text == "legendary")
            itemRarityText.color = ItemRewardManager.Instance.legendaryColour;

    }

    public void ToggleSkillItemSwitchButton(bool toggle = true)
    {/*
        if (toggle)
        {
            skillsItemsSwitchButton.UpdateAlpha(1);
            skillsItemsSwitchButton.ToggleButton(true);
        }
        else
        {
            skillsItemsSwitchButton.UpdateAlpha(0);
            skillsItemsSwitchButton.ToggleButton(false);
        }
        */
    }

    public void ToggleFighterDetailsTab(bool toggle = false)
    {
        /*
        if (!toggle)
        {
            GetComponent<CanvasGroup>().alpha = 0;

            ToggleSkillItemSwitchButton(false);
        }
        else
        {
            GetComponent<CanvasGroup>().alpha = 1;
            if (GameManager.Instance.playerInCombat)
                ToggleSkillItemSwitchButton(true);
        }
        */
    }

    public Vector2 GetHitAreaType()
    {
        if (GameManager.Instance.isSkillsMode)
        {
            if (GameManager.Instance.GetActiveSkill())
            {
                SkillData skill = GameManager.Instance.GetActiveSkill();
                return skill.skillRangeHitArea;
            }
            else
                return Vector2.one;
        }
        else
        {
            if (GameManager.Instance.GetActiveItem())
            {
                ItemPiece item = GameManager.Instance.GetActiveItem();
                return item.itemRangeHitArea;
            }
            else
                return Vector2.one;
        }
    }

    public void UpdateSkillUI(string skillName, string skillDesc, int skillDescPower, int baseHitCount,
        int range, Vector2 hitArea, int skillPower, int skillCooldown, int hitAttemptCount, float accuracyCount, Sprite skillPowerImage, Sprite skillIcon, bool special = false)
    {
        if (skillName != "")
        {
            if (!CombatGridManager.Instance.isCombatMode)
            {
                if (GameManager.Instance.isSkillsMode)
                    ToggleAllStats(true, true, false);
                else
                    ToggleAllStats(true, false, false);
                return;
            }
            else
            {
                ToggleAllStats(true, true, false);
            }
        }

        UpdateMainSlotDetailsName(skillName);
        UpdateMainSlotDetailsDesc(skillDesc);

        UpdateActiveBaseSlotIcon(skillIcon);
        //UpdateSkillDetailsPowerImage(skillPowerImage);

        UpdateSelectedObjectPowerText(skillPower);
        UpdateSkillDetailsHitsRemainingText(hitAttemptCount);
        UpdateSkillDetailsBaseHits(baseHitCount);
        UpdateSkillDetailsCooldownText(skillCooldown);

        UpdateSkillDetailsAccuracyText((int)accuracyCount);
        UpdateSelectedObjectRangeText(range);
        UpdateSelectedObjectHitAreaSprite(hitArea);
    }

    public void ResetDetailsUI()
    {
        // Unselect
        ToggleOverlay(false);
        ToggleShopDetailsBanner(false);
        ShopManager.Instance.UpdateSelectedShopItem(null);
        ShopManager.Instance.ResetShopItemSelectBorder();
        FighterInventorManager.Instance.ResetFighterInventorySelections();

        buttonCloseDetails.UpdateAlpha(0);
        buttonCloseDetails.ToggleButton(false);

        buttonSellItemDetails.UpdateAlpha(0);
        buttonSellItemDetails.ToggleButton(false);

        CombatGridManager.Instance.ResetSlotCovers();
        FighterInventorManager.Instance.ResetSelectedInventorySlot();
    }

    public void UpdateGearDetailsUI(string gearName, string desc, int gearHealth, int gearDamage = 0, int gearHealing = 0, int gearDefense = 0, int gearSpeed = 0, Sprite itemIcon = null)
    {
        // stuffs, to do
        UpdateMainSlotDetailsName(gearName);
        UpdateMainSlotDetailsDesc(desc);
        UpdateSelectedObjectPowerText(gearDamage);
        UpdateActiveBaseSlotIcon(itemIcon, true);
    }
    public void UpdateItemDetailsUI(string itemName, string itemDesc, int itemPower, int range, Vector2 hitArea, Sprite itemIcon)
    {
        if (itemName != "")
        {
            if (!CombatGridManager.Instance.isCombatMode || GameManager.Instance.isSkillsMode)
            {
                if (RoomManager.Instance.GetActiveRoom().curRoomType != RoomMapIcon.RoomType.SHOP)
                {
                    //ToggleAllStats(true, false, true);
                    //return;
                }

            }
        }

        if (CombatGridManager.Instance.isCombatMode)
        {
            if (GameManager.Instance.isSkillsMode)
                ToggleAllStats(true, true);
            else
                ToggleAllStats(true, false);
        }
        else
            ToggleAllStats(true, false, false);

        UpdateMainSlotDetailsName(itemName);
        UpdateMainSlotDetailsDesc(itemDesc);

        UpdateActiveBaseSlotIcon(itemIcon);

        UpdateSelectedObjectPowerText(itemPower);
        UpdateSelectedObjectRangeText(range);
        UpdateSelectedObjectHitAreaSprite(hitArea);

        remainingMovementUsesUI.UpdateAlpha(0);

    }
    public void ToggleActiveItemTriggerStatus(bool toggle = true)
    {
        if (toggle)
        {
            activeItemTriggerStatus.UpdateAlpha(1);
        }
        else
        {
            activeItemTriggerStatus.UpdateAlpha(0);
        }
    }
    public void UpdateActiveItemTriggerStatus(bool toggle = true, bool gear = false)
    {
        if (gear)
        {
            activeItemTriggerStatus.UpdateContentText("G");
            activeItemTriggerStatus.UpdateContentTextColourTMP(TeamGearManager.Instance.gearIconColour);
            return;
        }

        if (toggle)
        {
            activeItemTriggerStatus.UpdateContentText("A");
            activeItemTriggerStatus.UpdateContentTextColourTMP(activeItemTextColour);
        }
        else
        {
            activeItemTriggerStatus.UpdateContentText("P");
            activeItemTriggerStatus.UpdateContentTextColourTMP(passiveItemTextColour);
        }
    }

    public void UpdateActiveItemUseCountText(int count)
    {
        if (count == 0)
            activeItemUseCountText.UpdateContentText("");
        else
            activeItemUseCountText.UpdateContentText(count.ToString());
    }
    private void UpdateMainSlotDetailsName(string text)
    {
        skillDetailsName.text = text;
    }

    private void UpdateMainSlotDetailsDesc(string mainText, int power = 0, int skillTargetCount = 0, bool attack = false, bool special = false)
    {
        skillDetailsDesc.text = mainText;

        /*
        string targetType = "";
        string targetType2 = "";
        if (attack)
        {
            if (skillTargetCount == 1)
                targetType = "enemy";
            else
                targetType = "enemies";

            targetType2 = "DAMAGING";
        }
        else
        {
            if (skillTargetCount == 1)
                targetType = "ally";
            else
                targetType = "allies";

            targetType2 = "HEALING";
        }

        if (special)
        {
            skillDetailsDesc.text = mainText;
            return;
        }

        if (GameManager.Instance.GetActiveSkill())
        {
            if (GameManager.Instance.GetActiveSkill().giveExtraDesc)
            {
                if (attack)
                    skillDetailsDesc.text = $"{mainText},<color={damageWordTextColour}> {targetType2}</color> for<color={damagingTextColour}> {power}</color>";// x <color={skillMultihitColour}>{skillAttackCount}+</color>";
                else
                    skillDetailsDesc.text = $"{mainText},<color={healWordTextColour}> {targetType2}</color> for<color={healingTextColour}> {power}</color>";// x <color={skillMultihitColour}>{skillAttackCount}+</color>";
            }
            else
                skillDetailsDesc.text = mainText;
        }
        */
    }

    public void UpdateRemainingMovementUsesText(int uses)
    {
        remainingMovementUsesText.UpdateContentText(uses.ToString());
        remainingMovementUsesText.AnimateUI(false);
    }

    public void ToggleAllStats(bool toggle = true, bool skill = true, bool movement = false)
    {
        //Debug.Log("toggle = " + toggle + " skill = " + skill + " movement = " + movement);

        if (movement)
        {
            remainingMovementUsesUI.UpdateAlpha(1);
            remainingMovementUsesUI.AnimateUI(false);
            remainingMovementUsesText.UpdateAlpha(1);

            skillDetailsPower.UpdateAlpha(0);
            skillDetailsPower.ToggleButton(false);

            skillDetailsMaxCdUi.UpdateAlpha(0);
            skillDetailsMaxCdUi.ToggleButton(false);

            skillDetailsHitsRemaininguI.UpdateAlpha(0);
            skillDetailsHitsRemaininguI.ToggleButton(false);

            skillDetailsBaseHitsUI.UpdateAlpha(0);
            skillDetailsBaseHitsUI.ToggleButton(false);

            skillDetailsAccuracyuI.UpdateAlpha(0);
            skillDetailsAccuracyuI.ToggleButton(false);

            skillDetailsRangeuI.UpdateAlpha(0);
            skillDetailsRangeuI.ToggleButton(false);
        }
        else
        {
            remainingMovementUsesUI.UpdateAlpha(0);
            remainingMovementUsesText.UpdateAlpha(0);

            if (skill)
            {
                if (toggle)
                    skillDetailsPower.UpdateAlpha(1);
                else
                    skillDetailsPower.UpdateAlpha(0);
                skillDetailsPower.ToggleButton(toggle);

                if (toggle)
                    skillDetailsMaxCdUi.UpdateAlpha(1);
                else
                    skillDetailsMaxCdUi.UpdateAlpha(0);
                skillDetailsMaxCdUi.ToggleButton(toggle);

                if (toggle)
                    skillDetailsHitsRemaininguI.UpdateAlpha(1);
                else
                    skillDetailsHitsRemaininguI.UpdateAlpha(0);
                skillDetailsHitsRemaininguI.ToggleButton(toggle);

                if (toggle)
                    skillDetailsBaseHitsUI.UpdateAlpha(1);
                else
                    skillDetailsBaseHitsUI.UpdateAlpha(0);
                skillDetailsBaseHitsUI.ToggleButton(toggle);

                if (toggle)
                    skillDetailsAccuracyuI.UpdateAlpha(1);
                else
                    skillDetailsAccuracyuI.UpdateAlpha(0);
                skillDetailsAccuracyuI.ToggleButton(toggle);

                if (toggle)
                    skillDetailsRangeuI.UpdateAlpha(1);
                else
                    skillDetailsRangeuI.UpdateAlpha(0);
                skillDetailsRangeuI.ToggleButton(toggle);
            }
            // Item
            else if (!skill)
            {
                if (GameManager.Instance.GetActiveItem())
                {
                    if (GameManager.Instance.GetActiveItem().curActiveType == ItemPiece.ActiveType.PASSIVE)
                    {
                        skillDetailsPower.UpdateAlpha(0);
                        skillDetailsPower.ToggleButton(false);

                        skillDetailsRangeuI.UpdateAlpha(0);
                        skillDetailsRangeuI.ToggleButton(false);

                        skillDetailsHitAreauI.UpdateAlpha(0);
                        skillDetailsHitAreauI.ToggleButton(false);

                        skillDetailsMaxCdUi.UpdateAlpha(0);
                        skillDetailsMaxCdUi.ToggleButton(false);

                        skillDetailsHitsRemaininguI.UpdateAlpha(0);
                        skillDetailsHitsRemaininguI.ToggleButton(false);

                        skillDetailsBaseHitsUI.UpdateAlpha(0);
                        skillDetailsBaseHitsUI.ToggleButton(false);

                        skillDetailsAccuracyuI.UpdateAlpha(0);
                        skillDetailsAccuracyuI.ToggleButton(false);
                    }
                    else
                    {
                        if (toggle)
                            skillDetailsPower.UpdateAlpha(1);
                        else
                            skillDetailsPower.UpdateAlpha(0);
                        skillDetailsPower.ToggleButton(toggle);

                        if (toggle)
                            skillDetailsRangeuI.UpdateAlpha(1);
                        else
                            skillDetailsRangeuI.UpdateAlpha(0);
                        skillDetailsRangeuI.ToggleButton(toggle);

                        if (toggle)
                            skillDetailsHitAreauI.UpdateAlpha(1);
                        else
                            skillDetailsHitAreauI.UpdateAlpha(0);
                        skillDetailsHitAreauI.ToggleButton(false);

                        skillDetailsMaxCdUi.UpdateAlpha(0);
                        skillDetailsMaxCdUi.ToggleButton(false);

                        skillDetailsHitsRemaininguI.UpdateAlpha(0);
                        skillDetailsHitsRemaininguI.ToggleButton(false);

                        skillDetailsBaseHitsUI.UpdateAlpha(0);
                        skillDetailsBaseHitsUI.ToggleButton(false);

                        skillDetailsAccuracyuI.UpdateAlpha(0);
                        skillDetailsAccuracyuI.ToggleButton(false);
                    }
                }
                else
                {
                    skillDetailsPower.UpdateAlpha(0);
                    skillDetailsPower.ToggleButton(toggle);

                    skillDetailsRangeuI.UpdateAlpha(0);
                    skillDetailsRangeuI.ToggleButton(toggle);

                    skillDetailsHitAreauI.UpdateAlpha(0);
                    skillDetailsHitAreauI.ToggleButton(false);

                    skillDetailsMaxCdUi.UpdateAlpha(0);
                    skillDetailsMaxCdUi.ToggleButton(false);

                    skillDetailsHitsRemaininguI.UpdateAlpha(0);
                    skillDetailsHitsRemaininguI.ToggleButton(false);

                    skillDetailsBaseHitsUI.UpdateAlpha(0);
                    skillDetailsBaseHitsUI.ToggleButton(false);

                    skillDetailsAccuracyuI.UpdateAlpha(0);
                    skillDetailsAccuracyuI.ToggleButton(false);
                }
            }
        }
    }

    private void UpdateSelectedObjectPowerText(int power)
    {    
        skillDetailsPower.UpdateContentText(power.ToString());
    }

    public void UpdateSkillDetailsCooldownText(int cooldown)
    {
        skillDetailsMaxCdUi.UpdateContentText(cooldown.ToString());
    }

    private void UpdateSkillDetailsHitsRemainingText(int count)
    {
        skillDetailsHitsRemaininguI.UpdateContentText(count.ToString());
    }

    private void UpdateSkillDetailsBaseHits(int count)
    {
        skillDetailsBaseHitsUI.UpdateContentText(count.ToString());

        //if (count != oldHits)
          //  skillDetailsBaseHitsUI.AnimateUI();

        //oldHits = count;
    }
    private void UpdateSkillDetailsAccuracyText(int count)
    {
        skillDetailsAccuracyuI.UpdateContentText(count.ToString());
    }

    private void UpdateSelectedObjectRangeText(int count)
    {
        skillDetailsRangeuI.UpdateContentText(count.ToString());
    }

    private void UpdateSelectedObjectHitAreaSprite(Vector2 hitArea)
    {   
        if (GameManager.Instance.isSkillsMode)
        {
            if (hitArea == Vector2.zero)
            {
                skillDetailsHitAreauI.UpdateAlpha(0);
            }
            else
            {
                skillDetailsHitAreauI.UpdateAlpha(1);

                if (hitArea == new Vector2(1, 1))
                {
                    skillDetailsHitAreauI.UpdateContentImage(hitArea1x1);
                }
                else if (hitArea == new Vector2(1, 2))
                {
                    skillDetailsHitAreauI.UpdateContentImage(hitArea1x2);
                }
                else if (hitArea == new Vector2(1, 3))
                {
                    skillDetailsHitAreauI.UpdateContentImage(hitArea1x3);
                }
                else if (hitArea == new Vector2(2, 1))
                {
                    skillDetailsHitAreauI.UpdateContentImage(hitArea2x1);
                }
                else if (hitArea == new Vector2(3, 1))
                {
                    skillDetailsHitAreauI.UpdateContentImage(hitArea3x1);
                }
                else if (hitArea == new Vector2(2, 2))
                {
                    skillDetailsHitAreauI.UpdateContentImage(hitArea2x2);
                }
                else if (hitArea == new Vector2(2, 3))
                {
                    skillDetailsHitAreauI.UpdateContentImage(hitArea2x3);
                }
                else if (hitArea == new Vector2(3, 2))
                {
                    skillDetailsHitAreauI.UpdateContentImage(hitArea3x2);
                }
                else if (hitArea == new Vector2(3, 3))
                {
                    if (GameManager.Instance.GetActiveSkill())
                    {
                        // plus
                        if (GameManager.Instance.GetActiveSkill().skillRangeHitAreas.Count == 5)
                        {
                            skillDetailsHitAreauI.UpdateContentImage(hitAreaPlus);
                        }
                        // 3x3
                        else
                        {
                            skillDetailsHitAreauI.UpdateContentImage(hitArea3x3);
                        }
                    }
                }
            }       
        }
        else
        {
            if (GameManager.Instance.GetActiveItem())
            {
                if (hitArea == Vector2.zero || GameManager.Instance.GetActiveItem().curActiveType == ItemPiece.ActiveType.PASSIVE)
                {
                    skillDetailsHitAreauI.UpdateAlpha(0);
                }
                else
                {
                    skillDetailsHitAreauI.UpdateAlpha(1);

                    if (hitArea == new Vector2(1, 1))
                    {
                        skillDetailsHitAreauI.UpdateContentImage(hitArea1x1);
                    }
                    else if (hitArea == new Vector2(1, 2))
                    {
                        skillDetailsHitAreauI.UpdateContentImage(hitArea1x2);
                    }
                    else if (hitArea == new Vector2(1, 3))
                    {
                        skillDetailsHitAreauI.UpdateContentImage(hitArea1x3);
                    }
                    else if (hitArea == new Vector2(2, 1))
                    {
                        skillDetailsHitAreauI.UpdateContentImage(hitArea2x1);
                    }
                    else if (hitArea == new Vector2(3, 1))
                    {
                        skillDetailsHitAreauI.UpdateContentImage(hitArea3x1);
                    }
                    else if (hitArea == new Vector2(2, 2))
                    {
                        skillDetailsHitAreauI.UpdateContentImage(hitArea2x2);
                    }
                    else if (hitArea == new Vector2(2, 3))
                    {
                        skillDetailsHitAreauI.UpdateContentImage(hitArea2x3);
                    }
                    else if (hitArea == new Vector2(3, 2))
                    {
                        skillDetailsHitAreauI.UpdateContentImage(hitArea3x2);
                    }
                    else if (hitArea == new Vector2(3, 3))
                    {
                        if (GameManager.Instance.GetActiveSkill())
                        {
                            // plus
                            if (GameManager.Instance.GetActiveSkill().skillRangeHitAreas.Count == 5)
                            {
                                skillDetailsHitAreauI.UpdateContentImage(hitAreaPlus);
                            }
                            // 3x3
                            else
                            {
                                skillDetailsHitAreauI.UpdateContentImage(hitArea3x3);
                            }
                        }
                    }
                }
            }
        }

    }

    private void UpdateSkillDetailsPowerImage(Sprite sprite)
    {
        skillDetailsPowerIcon.sprite = sprite;
    }

    private void UpdateActiveBaseSlotIcon(Sprite sprite, bool gear = false)
    {
        activeObjectIcon.sprite = sprite;

        if (gear)
        {
            activeObjectIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
        }
        else
            activeObjectIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 400);
    }
}
