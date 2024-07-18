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

    public TextMeshProUGUI skillDetailsName;
    public TextMeshProUGUI skillDetailsDesc;

    public UIElement skillDetailsPower;
    public UIElement skillDetailsHitsRemaininguI;
    public UIElement skillDetailsBaseHitsUI;
    public UIElement skillDetailsMaxCdUi;
    public UIElement skillDetailsAccuracyuI;
    public UIElement skillDetailsMaxTargetsuI;
    public UIElement skillsItemsSwitchButton;

    public Image skillDetailsPowerIcon;
    public Image activeSkill;

    public TextMeshProUGUI unitOverlayCurEnergyText;
    public TextMeshProUGUI unitOverlayCurHealthText;
    public Image unitOverlayCurEnergyImage;
    public Image unitOverlayCurHealthImage;

    public UIElement itemRarityTextUI;
    public TextMeshProUGUI itemRarityText;
    [SerializeField] private UIElement remainingMovementUsesUI;
    [SerializeField] private UIElement remainingMovementUsesText;
    public UIElement extraMovePrompt;

    [SerializeField] private Color powerDamageColour;
    [SerializeField] private Color powerHealColour;

    private int oldHits;

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

        if (text == "COMMON")
            itemRarityText.color = ItemRewardManager.Instance.commonColour;
        else if (text == "RARE")
            itemRarityText.color = ItemRewardManager.Instance.rareColour;
        else if (text == "EPIC")
            itemRarityText.color = ItemRewardManager.Instance.epicColour;
        else if (text == "LEGENDARY")
            itemRarityText.color = ItemRewardManager.Instance.legendaryColour;

    }

    public void ToggleSkillItemSwitchButton(bool toggle = true)
    {
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
    }

    public void ToggleFighterDetailsTab(bool toggle = false)
    {
        if (!toggle)
        {
            GetComponent<CanvasGroup>().alpha = 0;
            ToggleSkillItemSwitchButton(false);
        }
        else
        {
            GetComponent<CanvasGroup>().alpha = 1;
            ToggleSkillItemSwitchButton(true);
        }
    }

    public void UpdateSkillUI(string skillName, string skillDesc, int skillDescPower, int baseHitCount,
        int skillTargetCount, int skillPower, int skillCooldown, int hitAttemptCount, float accuracyCount, Sprite skillPowerImage, Sprite skillIcon, bool special = false)
    {
        UpdateMainSlotDetailsName(skillName);
        UpdateMainSlotDetailsDesc(skillDesc);

        UpdateActiveBaseSlotIcon(skillIcon);
        //UpdateSkillDetailsPowerImage(skillPowerImage);

        UpdateSelectedObjectPowerText(skillPower);
        UpdateSkillDetailsHitsRemainingText(hitAttemptCount);
        UpdateSkillDetailsBaseHits(baseHitCount);
        UpdateSkillDetailsCooldownText(skillCooldown);

        UpdateSkillDetailsAccuracyText((int)accuracyCount);
        UpdateSelectedObjectMaxTargetsText(skillTargetCount);
    }
    
    public void UpdateItemUI(string itemName, string itemDesc, int itemPower, int targetCount, Sprite itemIcon)
    {
        ToggleAllStats(true, false);

        UpdateMainSlotDetailsName(itemName);
        UpdateMainSlotDetailsDesc(itemDesc);

        UpdateActiveBaseSlotIcon(itemIcon);

        UpdateSelectedObjectPowerText(itemPower);
        UpdateSelectedObjectMaxTargetsText(targetCount);
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
    public void UpdateActiveItemTriggerStatus(bool toggle = true)
    {
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

            skillDetailsMaxTargetsuI.UpdateAlpha(0);
            skillDetailsMaxTargetsuI.ToggleButton(false);
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
                    skillDetailsMaxTargetsuI.UpdateAlpha(1);
                else
                    skillDetailsMaxTargetsuI.UpdateAlpha(0);
                skillDetailsMaxTargetsuI.ToggleButton(toggle);
            }
            // Item
            else if (!skill)
            {
                if (toggle)
                    skillDetailsPower.UpdateAlpha(1);
                else
                    skillDetailsPower.UpdateAlpha(0);
                skillDetailsPower.ToggleButton(toggle);

                if (toggle)
                    skillDetailsMaxTargetsuI.UpdateAlpha(1);
                else
                    skillDetailsMaxTargetsuI.UpdateAlpha(0);
                skillDetailsMaxTargetsuI.ToggleButton(toggle);

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

    private void UpdateSelectedObjectMaxTargetsText(int count)
    {
        skillDetailsMaxTargetsuI.UpdateContentText(count.ToString());
    }

    private void UpdateSkillDetailsPowerImage(Sprite sprite)
    {
        skillDetailsPowerIcon.sprite = sprite;
    }

    private void UpdateActiveBaseSlotIcon(Sprite sprite)
    {
        activeSkill.sprite = sprite;
    }
}
