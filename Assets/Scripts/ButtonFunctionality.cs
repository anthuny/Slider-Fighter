using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFunctionality : MonoBehaviour
{
    public enum MasteryType { L1, L2, L3, L4, R1, R2, R3, R4, BG };
    public MasteryType curMasteryType;
    UnitFunctionality unitFunctionality;
    [SerializeField] private CanvasGroup buttonSelectionCG;
    bool selected;

    private MasteryType masteryType;

    private ShopItem shopItem;


    [SerializeField] private bool startDisabled;
    private void Awake()
    {
        unitFunctionality = transform.parent.GetComponent<UnitFunctionality>();

        if (startDisabled)
            ToggleSelected(false);
        else
            ToggleSelected(true);
    }

    public void ButtonEnterRoom()
    {
        // Disable map UI
        GameManager.Instance.ToggleMap(false);

        // Enable combat UI
        GameManager.Instance.Setup();
    }

    public void ButtonOpenMap()
    {
        // Disable to map button
        GameManager.Instance.toMapButton.UpdateAlpha(0);
        // Toggle Map back on
        GameManager.Instance.ToggleMap(true, false);

        GameManager.Instance.ToggleTeamSetup(false);

        GameManager.Instance.ResetRoom();
    }

    public void ResetMasteryTree()
    {
        TeamSetup.Instance.ResetTeamSetup();
    }

    public void MasteryAddPoint()
    {
        TeamSetup.Instance.MasteryAddPoint();
    }

    public void MasteryRemovePoint()
    {
        if (TeamSetup.Instance.GetSelectedMastery() == null)
            return;

        UIElement.MasteryType masteryType = TeamSetup.Instance.GetSelectedMastery().curMasteryType;

        if (masteryType == UIElement.MasteryType.L1 || masteryType == UIElement.MasteryType.R1)
        {
            if (TeamSetup.Instance.masteryL2.GetMasteryPointsAdded() != 0 || TeamSetup.Instance.masteryR2.GetMasteryPointsAdded() != 0)
                return;
            else if (TeamSetup.Instance.masteryL3.GetMasteryPointsAdded() != 0 || TeamSetup.Instance.masteryR3.GetMasteryPointsAdded() != 0)
                return;
            else if (TeamSetup.Instance.masteryL4.GetMasteryPointsAdded() != 0 || TeamSetup.Instance.masteryR4.GetMasteryPointsAdded() != 0)
                return;
        }
        else if (masteryType == UIElement.MasteryType.L2 || masteryType == UIElement.MasteryType.R2)
        {
            if (TeamSetup.Instance.masteryL3.GetMasteryPointsAdded() != 0 || TeamSetup.Instance.masteryR3.GetMasteryPointsAdded() != 0)
                return;
            else if (TeamSetup.Instance.masteryL4.GetMasteryPointsAdded() != 0 || TeamSetup.Instance.masteryR4.GetMasteryPointsAdded() != 0)
                return;
        }
        else if (masteryType == UIElement.MasteryType.L3 || masteryType == UIElement.MasteryType.R3)
        {
            if (TeamSetup.Instance.masteryL4.GetMasteryPointsAdded() != 0 || TeamSetup.Instance.masteryR4.GetMasteryPointsAdded() != 0)
                return;
        }

        TeamSetup.Instance.MasteryRemovePoint();
    }

    public void Mastery()
    {
        // If the BG is selected
        if (curMasteryType == MasteryType.BG)
            return;

        UnitFunctionality unit = GameManager.Instance.GetActiveUnitFunctionality();

        TeamSetup.Instance.ResetMasterySelection();

        TeamSetup.Instance.UpdateSelectedMastery(this);

        // If this mastery is locked, stop
        if (TeamSetup.Instance.GetSelectedMastery().GetIsLocked())
            return;

        else if (curMasteryType == MasteryType.L1)
        {
            TeamSetup.Instance.UpdateMasteryDescription(unit.GetMastery(0));
            TeamSetup.Instance.ToggleMasterySelection(TeamSetup.Instance.masteryL1, true);
            TeamSetup.Instance.masteryL1.UpdateContentSubText(TeamSetup.Instance.masteryL1.GetMasteryPointsAdded().ToString() + " / " + unit.GetMastery(0).masteryMaxAmount);
        }
        else if (curMasteryType == MasteryType.L2)
        {
            TeamSetup.Instance.UpdateMasteryDescription(unit.GetMastery(1));
            TeamSetup.Instance.ToggleMasterySelection(TeamSetup.Instance.masteryL2, true);
            TeamSetup.Instance.masteryL2.UpdateContentSubText(TeamSetup.Instance.masteryL2.GetMasteryPointsAdded().ToString() + " / " + unit.GetMastery(1).masteryMaxAmount);
        }
        else if (curMasteryType == MasteryType.L3)
        {
            TeamSetup.Instance.UpdateMasteryDescription(unit.GetMastery(2));
            TeamSetup.Instance.ToggleMasterySelection(TeamSetup.Instance.masteryL3, true);
            TeamSetup.Instance.masteryL3.UpdateContentSubText(TeamSetup.Instance.masteryL3.GetMasteryPointsAdded().ToString() + " / " + unit.GetMastery(2).masteryMaxAmount);
        }
        else if (curMasteryType == MasteryType.L4)
        {
            TeamSetup.Instance.UpdateMasteryDescription(unit.GetMastery(3));
            TeamSetup.Instance.ToggleMasterySelection(TeamSetup.Instance.masteryL4, true);
            TeamSetup.Instance.masteryL4.UpdateContentSubText(TeamSetup.Instance.masteryL4.GetMasteryPointsAdded().ToString() + " / " + unit.GetMastery(3).masteryMaxAmount);
        }
        else if (curMasteryType == MasteryType.R1)
        {
            TeamSetup.Instance.UpdateMasteryDescription(unit.GetMastery(4));
            TeamSetup.Instance.ToggleMasterySelection(TeamSetup.Instance.masteryR1, true);
            TeamSetup.Instance.masteryR1.UpdateContentSubText(TeamSetup.Instance.masteryR1.GetMasteryPointsAdded().ToString() + " / " + unit.GetMastery(4).masteryMaxAmount);
        }
        else if (curMasteryType == MasteryType.R2)
        {
            TeamSetup.Instance.UpdateMasteryDescription(unit.GetMastery(5));
            TeamSetup.Instance.ToggleMasterySelection(TeamSetup.Instance.masteryR2, true);
            TeamSetup.Instance.masteryR2.UpdateContentSubText(TeamSetup.Instance.masteryR2.GetMasteryPointsAdded().ToString() + " / " + unit.GetMastery(5).masteryMaxAmount);
        }
        else if (curMasteryType == MasteryType.R3)
        {
            TeamSetup.Instance.UpdateMasteryDescription(unit.GetMastery(6));
            TeamSetup.Instance.ToggleMasterySelection(TeamSetup.Instance.masteryR3, true);
            TeamSetup.Instance.masteryR3.UpdateContentSubText(TeamSetup.Instance.masteryR3.GetMasteryPointsAdded().ToString() + " / " + unit.GetMastery(6).masteryMaxAmount);
        }
        else if (curMasteryType == MasteryType.R4)
        {
            TeamSetup.Instance.UpdateMasteryDescription(unit.GetMastery(7));
            TeamSetup.Instance.ToggleMasterySelection(TeamSetup.Instance.masteryR4, true);
            TeamSetup.Instance.masteryR4.UpdateContentSubText(TeamSetup.Instance.masteryR4.GetMasteryPointsAdded().ToString() + " / " + unit.GetMastery(7).masteryMaxAmount);
        }

        //TeamSetup.Instance.GetSelectedMastery().UpdateContentSubText(
        //TeamSetup.Instance.GetSelectedMastery().GetMasteryPointsAdded().ToString() + " / " + TeamSetup.Instance.GetActiveMastery().masteryMaxAmount);
    }
    public void PurchaseShopItem()
    {
        // Get access to root parent of gameobject
        shopItem = transform.parent.parent.GetComponent<ShopItem>();

        List<Item> items = new List<Item>();
        items = ShopManager.Instance.GetShopItems();
        int count = items.Count;

        // Search for the item
        for (int i = 0; i < count; i++)
        {
            if (shopItem.GetShopItemName() == items[i].itemName)
            {
                // unit purchased the item
                shopItem.PurchaseShopItem();
                return;
            }
        }
    }

    public void ButtonTeamPage()
    {
        // Disable map UI
        GameManager.Instance.ToggleMap(false);

        GameManager.Instance.ToggleTeamSetup(true);

        // Enable To Map Button
        GameManager.Instance.toMapButton.UpdateAlpha(1);
    }
    public void PostBattleToMapButton()
    {
        // Disable post battle UI
        GameManager.Instance.postBattleUI.TogglePostBattleUI(false);

        GameManager.Instance.map.ClearRoom();

        if (!GameManager.Instance.playerLost)
            GameManager.Instance.ToggleMap(true, false);
        else
            GameManager.Instance.ToggleMap(true, true);
    }

    public void WeaponBackButton()
    {
        // Return unit energy
        GameManager.Instance.ReturnEnergyToUnit();

        GameManager.Instance.SetupPlayerSkillsUI();

        GameManager.Instance.UpdateEnemyPosition(true);
    }

    public void AttackButton()
    {
        // If unit doesnt have enough energy, do not allow skill to play out
        if (!GameManager.Instance.CheckIfEnergyAvailableSkill())
            return;

        // If no units are selected, stop
        if (!GameManager.Instance.CheckIfAnyUnitsSelected())
            return;

        // If the energy DOESNT cost any energy, make energy cost ui appear on casting unit DOESNT APPEAR
        if (GameManager.Instance.activeSkill.skillEnergyCost != 0)
        {
            // Trigger current unit's turn energy count to deplete for skill use
            GameManager.Instance.UpdateActiveUnitEnergyBar(true, false, GameManager.Instance.activeSkill.skillEnergyCost);
            GameManager.Instance.UpdateActiveUnitHealthBar(false);
        }
        else
            GameManager.Instance.SetupPlayerWeaponUI();

        // Trigger Skill alert UI
        GameManager.Instance.GetActiveUnitFunctionality().TriggerTextAlert(GameManager.Instance.GetActiveSkill().skillName, 1, false);
    }

    public void EndTurnButton()
    {
        GameManager.Instance.ToggleEndTurnButton(false);
        GameManager.Instance.UpdateEnemyPosition(false);

        GameManager.Instance.UpdateTurnOrder();
    }

    public void ToggleSelected(bool toggle)
    {
        selected = toggle;

        if (buttonSelectionCG == null)
            return;

            //buttonSelectionCG = GetComponent<CanvasGroup>();

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
        if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
        {
            if (GameManager.Instance.IsEnemyTaunting().Count >= 1)
            {
                for (int i = 0; i < GameManager.Instance.IsEnemyTaunting().Count; i++)
                {
                    if (GameManager.Instance.IsEnemyTaunting()[i] == unitFunctionality)
                        GameManager.Instance.SelectUnit(unitFunctionality);
                    else
                        continue;
                }

                return;
            }
            else
                GameManager.Instance.SelectUnit(unitFunctionality);
        }
    }

    public void SelectSkill1()
    {
        GameManager.Instance.ResetSelectedUnits();
        GameManager.Instance.UpdateAllSkillIconAvailability();

        GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnit().skill1);
        GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnit().skill1);

        // If selected, unselect it, dont do skill more stuff
        if (GetIfSelected())
        {
            GameManager.Instance.DisableAllSkillSelections();
            ToggleSelected(false);
            GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnit().basicSkill);
            GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnit().basicSkill);
        }
        // If skill not already selected, select it and proceed
        else
        {
            GameManager.Instance.DisableAllSkillSelections();
            ToggleSelected(true);
        }

        // If unit doesnt have enough energy, do not allow skill to play out
        if (!GameManager.Instance.CheckIfEnergyAvailableSkill())
            return;

        GameManager.Instance.UpdateUnitSelection(GameManager.Instance.activeSkill);
        GameManager.Instance.UpdateUnitsSelectedText();
    }

    public void SelectSkill2()
    {
        GameManager.Instance.ResetSelectedUnits();
        GameManager.Instance.UpdateAllSkillIconAvailability();

        GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnit().skill2);
        GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnit().skill2);

        // If selected, unselect it, dont do skill more stuff
        if (GetIfSelected())
        {
            GameManager.Instance.DisableAllSkillSelections();
            ToggleSelected(false);
            GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnit().basicSkill);
            GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnit().basicSkill);
        }
        // If skill not already selected, select it and proceed
        else
        {
            GameManager.Instance.DisableAllSkillSelections();
            ToggleSelected(true);
        }

        // If unit doesnt have enough energy, do not allow skill to play out
        if (!GameManager.Instance.CheckIfEnergyAvailableSkill())
            return;

        GameManager.Instance.UpdateUnitSelection(GameManager.Instance.activeSkill);
        GameManager.Instance.UpdateUnitsSelectedText();
    }

    public void SelectSkill3()
    {
        GameManager.Instance.ResetSelectedUnits();
        GameManager.Instance.UpdateAllSkillIconAvailability();

        GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnit().skill3);
        GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnit().skill3);

        // If selected, unselect it, dont do skill more stuff
        if (GetIfSelected())
        {
            GameManager.Instance.DisableAllSkillSelections();
            ToggleSelected(false);
            GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnit().basicSkill);
            GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnit().basicSkill);
        }
        // If skill not already selected, select it and proceed
        else
        {
            GameManager.Instance.DisableAllSkillSelections();
            ToggleSelected(true);
        }

        // If unit doesnt have enough energy, do not allow skill to play out
        if (!GameManager.Instance.CheckIfEnergyAvailableSkill())
            return;

        GameManager.Instance.UpdateUnitSelection(GameManager.Instance.activeSkill);
        GameManager.Instance.UpdateUnitsSelectedText();
    }

    public void StopWeaponHitLine()
     {
        Weapon.instance.StartCoroutine(Weapon.instance.StopHitLine());

        //GameManager.instance.UpdateUnitCurrentEnergy();
        GameManager.Instance.UpdateActiveUnitEnergyBar(false);

        GameManager.Instance.DisableAllSkillSelections();
    }
}
