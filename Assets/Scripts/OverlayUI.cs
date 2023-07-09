using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OverlayUI : MonoBehaviour
{
    [SerializeField] private string targetCountTextColour;
    [SerializeField] private string damagingTextColour;
    [SerializeField] private string healingTextColour;
    [SerializeField] private string damageWordTextColour;
    [SerializeField] private string healWordTextColour;
    [SerializeField] private string skillMultihitColour;

    public Text skillDetailsName;
    public Text skillDetailsDesc;
    public Text skillDetailsCurCD;
    public Text skillDetailsMaxCD;
    public Text skillDetailsPower;
    public Text skillDetailsCDText;
    public Text skillDetailsEnergyCostText;
    public Text skillDetailsMutlihitCountText;
    public Image skillDetailsPowerIcon;
    public Image skillDetailsIcon;

    public Text unitOverlayCurEnergyText;
    public Text unitOverlayCurHealthText;
    public Image unitOverlayCurEnergyImage;
    public Image unitOverlayCurHealthImage;

    public void UpdateSkillUI(string skillName, string skillDesc, int skillDescPower, int skillAttackCount, bool attack,
        int skillTargetCount, int skillPower, int skillEnergyCost, int multihitCount, Sprite skillPowerImage, Sprite skillIcon, bool special = false)
    {
        UpdateSkillDetailsSkillName(skillName);
        UpdateSkillDetailsDesc(skillDesc, skillDescPower, skillAttackCount, skillTargetCount, attack, special);
        UpdateSkillPowerText(skillPower);
        UpdateSkillDetailsEnergyText(skillEnergyCost);
        UpdateSkillDetailsMutlihitCountText(multihitCount);
        UpdateSkillDetailsPowerImage(skillPowerImage);
        UpdateSkillDetailsIcon(skillIcon);
    }

    public void UpdateUnitOverlayEnergyUI(UnitFunctionality unit, float curEnergy, float maxEnergy)
    {
        // Update overlay energy current text
        unitOverlayCurEnergyText.text = unit.GetUnitCurEnergy().ToString();

        // Update Energy Image Fill Progress
        float fillAmount = curEnergy / maxEnergy;
        unitOverlayCurEnergyImage.fillAmount = fillAmount;
    }

    public void UpdateUnitOverlayHealthUI(UnitFunctionality unit, float curHealth, float maxHealth)
    {
        // Update overlay health current text
        unitOverlayCurHealthText.text = unit.GetUnitCurHealth().ToString();

        // Update Unit Health Fill Progress
        float fillAmount = unit.GetUnitCurHealth() / unit.GetUnitMaxHealth();
        unitOverlayCurHealthImage.fillAmount = fillAmount;
    }


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

        if (attack)
            skillDetailsDesc.text = $"{mainText}<color={targetCountTextColour}> {skillTargetCount}</color> <color={damageWordTextColour}>{targetType}</color>,<color={damageWordTextColour}> {targetType2}</color> for<color={damagingTextColour}> {power}</color> x <color={skillMultihitColour}>{skillAttackCount}</color>";
        else
            skillDetailsDesc.text = $"{mainText}<color={targetCountTextColour}> {skillTargetCount}</color> <color={healWordTextColour}>{targetType}</color>,<color={healWordTextColour}> {targetType2}</color> for<color={healingTextColour}> {power}</color> x <color={skillMultihitColour}>{skillAttackCount}</color>";
    }

    private void UpdateSkillPowerText(int power)
    {
        skillDetailsPower.text = power.ToString();
    }

    private void UpdateSkillDetailsEnergyText(int energy)
    {
        skillDetailsEnergyCostText.text = energy.ToString();
    }

    private void UpdateSkillDetailsMutlihitCountText(int count)
    {
        skillDetailsMutlihitCountText.text = count.ToString();
    }

    private void UpdateSkillDetailsPowerImage(Sprite sprite)
    {
        skillDetailsPowerIcon.sprite = sprite;
    }

    private void UpdateSkillDetailsIcon(Sprite sprite)
    {
        skillDetailsIcon.sprite = sprite;
    }
}
