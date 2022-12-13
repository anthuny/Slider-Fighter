using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFunctionality : MonoBehaviour
{
    UnitFunctionality unitFunctionality;
    [SerializeField] private CanvasGroup buttonSelectionCG;
    bool selected;
    private void Awake()
    {
        unitFunctionality = transform.parent.GetComponent<UnitFunctionality>();

        if (buttonSelectionCG != null)
            ToggleSelected(false);
    }
    public void WeaponBackButton()
    {
        // Return unit energy
        GameManager.instance.ReturnEnergyToUnit();

        GameManager.instance.SetupPlayerSkillsUI();
    }

    public void AttackButton()
    {
        // If unit doesnt have enough energy, do not allow skill to play out
        if (!GameManager.instance.CheckIfEnergyAvailableSkill())
            return;

        // Trigger current unit's turn energy count to deplete for skill use
        GameManager.instance.UpdateActiveUnitEnergyBar(true, false, GameManager.instance.activeSkill.skillEnergyCost);
        GameManager.instance.UpdateActiveUnitHealthBar(false);
    }

    public void EndTurnButton()
    {
        GameManager.instance.ToggleEndTurnButton(false);
        GameManager.instance.UpdateTurnOrder();
    }

    public void ToggleSelected(bool toggle)
    {
        selected = toggle;

        if (toggle)
            buttonSelectionCG.alpha = 1;
        else
            buttonSelectionCG.alpha = 0;
    }

    bool GetIfSelected()
    {
        return selected;
    }

    public void SelectUnit()
    {
        GameManager.instance.SelectUnit(unitFunctionality);
    }

    public void SelectSkill1()
    {
        GameManager.instance.ResetSelectedUnits();
        GameManager.instance.UpdateAllSkillIconAvailability();

        GameManager.instance.UpdateActiveSkill(GameManager.instance.GetActiveUnit().skill1);
        GameManager.instance.UpdateSkillDetails(GameManager.instance.GetActiveUnit().skill1);

        // If selected, unselect it, dont do skill more stuff
        if (GetIfSelected())
        {
            GameManager.instance.DisableAllSkillSelections();
            ToggleSelected(false);
            GameManager.instance.UpdateActiveSkill(GameManager.instance.GetActiveUnit().basicSkill);
            GameManager.instance.UpdateSkillDetails(GameManager.instance.GetActiveUnit().basicSkill);
        }
        // If skill not already selected, select it and proceed
        else
        {
            GameManager.instance.DisableAllSkillSelections();
            ToggleSelected(true);
        }

        // If unit doesnt have enough energy, do not allow skill to play out
        if (!GameManager.instance.CheckIfEnergyAvailableSkill())
            return;

        GameManager.instance.UpdateUnitSelection(GameManager.instance.activeSkill);
        GameManager.instance.UpdateUnitsSelectedText();
    }

    public void SelectSkill2()
    {
        GameManager.instance.ResetSelectedUnits();
        GameManager.instance.UpdateAllSkillIconAvailability();

        GameManager.instance.UpdateActiveSkill(GameManager.instance.GetActiveUnit().skill2);
        GameManager.instance.UpdateSkillDetails(GameManager.instance.GetActiveUnit().skill2);

        // If selected, unselect it, dont do skill more stuff
        if (GetIfSelected())
        {
            GameManager.instance.DisableAllSkillSelections();
            ToggleSelected(false);
            GameManager.instance.UpdateActiveSkill(GameManager.instance.GetActiveUnit().basicSkill);
            GameManager.instance.UpdateSkillDetails(GameManager.instance.GetActiveUnit().basicSkill);
        }
        // If skill not already selected, select it and proceed
        else
        {
            GameManager.instance.DisableAllSkillSelections();
            ToggleSelected(true);
        }


        // If unit doesnt have enough energy, do not allow skill to play out
        if (!GameManager.instance.CheckIfEnergyAvailableSkill())
            return;

        GameManager.instance.UpdateUnitSelection(GameManager.instance.activeSkill);
        GameManager.instance.UpdateUnitsSelectedText();
    }

    public void SelectSkill3()
    {
        GameManager.instance.ResetSelectedUnits();
        GameManager.instance.UpdateAllSkillIconAvailability();

        GameManager.instance.UpdateActiveSkill(GameManager.instance.GetActiveUnit().skill3);
        GameManager.instance.UpdateSkillDetails(GameManager.instance.GetActiveUnit().skill3);

        // If selected, unselect it, dont do skill more stuff
        if (GetIfSelected())
        {
            GameManager.instance.DisableAllSkillSelections();
            ToggleSelected(false);
            GameManager.instance.UpdateActiveSkill(GameManager.instance.GetActiveUnit().basicSkill);
            GameManager.instance.UpdateSkillDetails(GameManager.instance.GetActiveUnit().basicSkill);
        }
        // If skill not already selected, select it and proceed
        else
        {
            GameManager.instance.DisableAllSkillSelections();
            ToggleSelected(true);
        }

        // If unit doesnt have enough energy, do not allow skill to play out
        if (!GameManager.instance.CheckIfEnergyAvailableSkill())
            return;

        GameManager.instance.UpdateUnitSelection(GameManager.instance.activeSkill);
        GameManager.instance.UpdateUnitsSelectedText();
    }

    public void StopWeaponHitLine()
     {
        //GameManager.instance.UpdateUnitCurrentEnergy();
        GameManager.instance.UpdateActiveUnitEnergyBar(false);

        Weapon.instance.StartCoroutine(Weapon.instance.StopHitLine());

        GameManager.instance.DisableAllSkillSelections();
    }
}
