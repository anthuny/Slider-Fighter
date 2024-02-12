using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class OverlayUI : MonoBehaviour
{
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



    public Image skillDetailsPowerIcon;
    public Image activeSkill;

    public TextMeshProUGUI unitOverlayCurEnergyText;
    public TextMeshProUGUI unitOverlayCurHealthText;
    public Image unitOverlayCurEnergyImage;
    public Image unitOverlayCurHealthImage;

    [SerializeField] private Color powerDamageColour;
    [SerializeField] private Color powerHealColour;

    public void UpdateSkillUI(string skillName, string skillDesc, int skillDescPower, int baseHitCount, bool attack,
        int skillTargetCount, int skillPower, int skillCooldown, int hitAttemptCount, float accuracyCount, Sprite skillPowerImage, Sprite skillIcon, bool special = false)
    {
        UpdateSkillDetailsSkillName(skillName);
        UpdateSkillDetailsDesc(skillDesc, skillDescPower, baseHitCount, skillTargetCount, attack, special);

        UpdateActiveSkillIcon(skillIcon);
        //UpdateSkillDetailsPowerImage(skillPowerImage);

        UpdateSkillPowerText(skillPower, attack);
        UpdateSkillDetailsHitsRemainingText(hitAttemptCount);
        UpdateSkillDetailsBaseHits(baseHitCount);
        UpdateSkillDetailsCooldownText(skillCooldown);

        UpdateSkillDetailsAccuracyText((int)accuracyCount);
        UpdateSkillDetailsMaxTargetsText(skillTargetCount);
    }

    /*
    public void UpdateUnitOverlayHealthUI(UnitFunctionality unit, float curHealth, float maxHealth)
    {
        // Update overlay health current text
        unitOverlayCurHealthText.text = unit.GetUnitCurHealth().ToString();

        // Update Unit Health Fill Progress
        float fillAmount = unit.GetUnitCurHealth() / unit.GetUnitMaxHealth();
        unitOverlayCurHealthImage.fillAmount = fillAmount;
    }
    */


    private void UpdateSkillDetailsSkillName(string text)
    {
        skillDetailsName.text = text;
    }

    private void UpdateSkillDetailsDesc(string mainText, int power, int skillAttackCount, int skillTargetCount, bool attack, bool special = false)
    {
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

    }

    public void ToggleAllStats(bool toggle = true)
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

    private void UpdateSkillPowerText(int power, bool damaging = true)
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
    }
    private void UpdateSkillDetailsAccuracyText(int count)
    {
        skillDetailsAccuracyuI.UpdateContentText(count.ToString());
    }

    private void UpdateSkillDetailsMaxTargetsText(int count)
    {
        skillDetailsMaxTargetsuI.UpdateContentText(count.ToString());
    }

    private void UpdateSkillDetailsPowerImage(Sprite sprite)
    {
        skillDetailsPowerIcon.sprite = sprite;
    }

    private void UpdateActiveSkillIcon(Sprite sprite)
    {
        activeSkill.sprite = sprite;
    }
}
