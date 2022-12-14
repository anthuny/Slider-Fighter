using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OverlayUI : MonoBehaviour
{
    public Text skillDetailsName;
    public Text skillDetailsDesc;
    public Text skillDetailsCurCD;
    public Text skillDetailsMaxCD;
    public Text skillDetailsPower;
    public Text skillDetailsCDText;
    public Text skillDetailsEnergyCostText;
    public Image skillDetailsPowerIcon;
    public Image skillDetailsIcon;

    public Text unitOverlayCurEnergyText;
    public Text unitOverlayCurHealthText;
    public Image unitOverlayCurEnergyImage;
    public Image unitOverlayCurHealthImage;

    public void UpdateSkillUI(string skillName, string skillDesc, int skillDescPower, int skillAttackCount, bool attack,
        int skillTargetCount, int skillPower, int skillEnergyCost, Sprite skillPowerImage, Sprite skillIcon)
    {
        UpdateSkillDetailsSkillName(skillName);
        UpdateSkillDetailsDesc(skillDesc, skillDescPower, skillAttackCount, skillTargetCount, attack);
        UpdateSkillPowerText(skillPower);
        UpdateSkillDetailsEnergyText(skillEnergyCost);
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

    private void UpdateSkillDetailsDesc(string mainText, int power, int skillAttackCount, int skillTargetCount, bool attack)
    {
        if (attack)
            skillDetailsDesc.text = mainText + " " + skillTargetCount + " enemies for " + skillAttackCount + " x " + power.ToString();
        else
            skillDetailsDesc.text = mainText + " " + skillTargetCount + " allies for " + skillAttackCount + " x " + power.ToString();
    }

    private void UpdateSkillPowerText(int power)
    {
        skillDetailsPower.text = power.ToString();
    }

    private void UpdateSkillDetailsEnergyText(int energy)
    {
        skillDetailsEnergyCostText.text = energy.ToString();
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
