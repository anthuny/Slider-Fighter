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
    [SerializeField] private bool disabled;
    [SerializeField] private UIElement UIbutton;


    [SerializeField] private bool startDisabled;
    private void Awake()
    {
        unitFunctionality = transform.parent.GetComponent<UnitFunctionality>();

        if (UIbutton == null)
            return;

        if (startDisabled)
        {
            ToggleButton(false);
            ToggleSelected(false);
        }
        else
        {
            ToggleButton(true);
            ToggleSelected(true);
        }
    }

    public void ButtonEnterRoom()
    {
        GameManager.Instance.UpdateAllyVisibility(true);

        // Disable map UI
        GameManager.Instance.ToggleMap(false);

        // Enable combat UI
        GameManager.Instance.Setup();

        // Face left for combat

        // Make all allies face left for combat
        for (int i = 0; i < GameManager.Instance.activeRoomAllies.Count; i++)
        {
            GameManager.Instance.activeRoomAllies[i].UpdateFacingDirection(false);
        }

        GameManager.Instance.UpdateAllysPositionCombat();
    }

    public void ButtonOpenMap()
    {
        // Disable to map button
        GameManager.Instance.toMapButton.UpdateAlpha(0);
        GameManager.Instance.UpdateAllyVisibility(false);
        ShopManager.Instance.ToggleExitShopButton(false);
        ShopManager.Instance.ToggleRandomiser(false);

        // Toggle Map back on
        GameManager.Instance.ToggleMap(true, false);

        ShopManager.Instance.CloseShop();

        //GameManager.Instance.activeRoomAllUnitFunctionalitys = GameManager.Instance.oldActiveRoomAllUnitFunctionalitys;

        GameManager.Instance.ToggleTeamSetup(false);

        // Reset room, enemes only
        GameManager.Instance.ResetRoom(true);
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

    public void MasteryPageChangeInc()
    {
        TeamSetup.Instance.ChangeMasteryType(true);
    }

    public void MasteryPageChangeDec()
    {
        TeamSetup.Instance.ChangeMasteryType(false);
    }

    public void Mastery()
    {
        // If the BG is selected
        if (curMasteryType == MasteryType.BG)
            return;

        UnitFunctionality unit = GameManager.Instance.GetActiveUnitFunctionality();
        UnitData unitData = GameManager.Instance.GetUnitData(0);

        TeamSetup.Instance.UpdateSelectedMastery(this);

        // If this mastery is locked, stop
        if (TeamSetup.Instance.GetSelectedMastery().GetIsLocked())
        {
            return;
        }


        else if (curMasteryType == MasteryType.L1)
        {
            TeamSetup.Instance.ResetMasterySelection();
            TeamSetup.Instance.UpdateMasteryDescription(unit.GetCurrentMastery(0));
            TeamSetup.Instance.ToggleMasterySelection(TeamSetup.Instance.masteryL1, true);
            TeamSetup.Instance.masteryL1.UpdateContentSubText(TeamSetup.Instance.masteryL1.GetMasteryPointsAdded().ToString() + " / " + unit.GetCurrentMastery(0).masteryMaxAmount);
        }
        else if (curMasteryType == MasteryType.L2)
        {
            TeamSetup.Instance.ResetMasterySelection();
            TeamSetup.Instance.UpdateMasteryDescription(unit.GetCurrentMastery(1));
            TeamSetup.Instance.ToggleMasterySelection(TeamSetup.Instance.masteryL2, true);
            TeamSetup.Instance.masteryL2.UpdateContentSubText(TeamSetup.Instance.masteryL2.GetMasteryPointsAdded().ToString() + " / " + unit.GetCurrentMastery(1).masteryMaxAmount);
        }
        else if (curMasteryType == MasteryType.L3)
        {
            TeamSetup.Instance.ResetMasterySelection();
            TeamSetup.Instance.UpdateMasteryDescription(unit.GetCurrentMastery(2));
            TeamSetup.Instance.ToggleMasterySelection(TeamSetup.Instance.masteryL3, true);
            TeamSetup.Instance.masteryL3.UpdateContentSubText(TeamSetup.Instance.masteryL3.GetMasteryPointsAdded().ToString() + " / " + unit.GetCurrentMastery(2).masteryMaxAmount);
        }
        else if (curMasteryType == MasteryType.L4)
        {
            TeamSetup.Instance.ResetMasterySelection();
            TeamSetup.Instance.UpdateMasteryDescription(unit.GetCurrentMastery(3));
            TeamSetup.Instance.ToggleMasterySelection(TeamSetup.Instance.masteryL4, true);
            TeamSetup.Instance.masteryL4.UpdateContentSubText(TeamSetup.Instance.masteryL4.GetMasteryPointsAdded().ToString() + " / " + unit.GetCurrentMastery(3).masteryMaxAmount);
        }
        else if (curMasteryType == MasteryType.R1)
        {
            TeamSetup.Instance.ResetMasterySelection();
            TeamSetup.Instance.UpdateMasteryDescription(unit.GetCurrentMastery(4));
            TeamSetup.Instance.ToggleMasterySelection(TeamSetup.Instance.masteryR1, true);
            TeamSetup.Instance.masteryR1.UpdateContentSubText(TeamSetup.Instance.masteryR1.GetMasteryPointsAdded().ToString() + " / " + unit.GetCurrentMastery(4).masteryMaxAmount);
        }
        else if (curMasteryType == MasteryType.R2)
        {
            TeamSetup.Instance.ResetMasterySelection();
            TeamSetup.Instance.UpdateMasteryDescription(unit.GetCurrentMastery(5));
            TeamSetup.Instance.ToggleMasterySelection(TeamSetup.Instance.masteryR2, true);
            TeamSetup.Instance.masteryR2.UpdateContentSubText(TeamSetup.Instance.masteryR2.GetMasteryPointsAdded().ToString() + " / " + unit.GetCurrentMastery(5).masteryMaxAmount);
        }
        else if (curMasteryType == MasteryType.R3)
        {
            TeamSetup.Instance.ResetMasterySelection();
            TeamSetup.Instance.UpdateMasteryDescription(unit.GetCurrentMastery(6));
            TeamSetup.Instance.ToggleMasterySelection(TeamSetup.Instance.masteryR3, true);
            TeamSetup.Instance.masteryR3.UpdateContentSubText(TeamSetup.Instance.masteryR3.GetMasteryPointsAdded().ToString() + " / " + unit.GetCurrentMastery(6).masteryMaxAmount);
        }
        else if (curMasteryType == MasteryType.R4)
        {
            TeamSetup.Instance.ResetMasterySelection();
            TeamSetup.Instance.UpdateMasteryDescription(unit.GetCurrentMastery(7));
            TeamSetup.Instance.ToggleMasterySelection(TeamSetup.Instance.masteryR4, true);
            TeamSetup.Instance.masteryR4.UpdateContentSubText(TeamSetup.Instance.masteryR4.GetMasteryPointsAdded().ToString() + " / " + unit.GetCurrentMastery(7).masteryMaxAmount);
        }
    }

    public void RefreshShop()
    {
        //  Go to purchase it like a normal item

        int playerGold = ShopManager.Instance.GetPlayerGold();

        if (playerGold < ShopManager.Instance.GetRefreshShopPrice())    // If player cannot afford the item, cancel.
            return;
        else
            ShopManager.Instance.UpdatePlayerGold(-ShopManager.Instance.GetRefreshShopPrice());

        ShopManager.Instance.GetActiveRoom().hasEntered = false;
        ShopManager.Instance.GetActiveRoom().ClearPurchasedItems();
        ShopManager.Instance.FillShopItems(true, false);



        // refresh item price
    }
    public void PurchaseShopItem()
    {
        // Get access to root parent of gameobject
        shopItem = transform.parent.parent.GetComponent<ShopItem>();

        // Combat Item
        List<Item> shopCombatItems = new List<Item>();
        shopCombatItems = ShopManager.Instance.GetShopCombatItems();
        int shopCombatCount = shopCombatItems.Count;

        // Search for the item
        for (int i = 0; i < shopCombatCount; i++)
        {
            if (shopItem.GetShopItemName() == shopCombatItems[i].itemName)
            {
                // unit purchased the item
                shopItem.PurchaseShopItem(true);
                return;
            }
        }

        // Health Item
        List<Item> shopHealthItems = new List<Item>();
        shopHealthItems = ShopManager.Instance.GetShopHealthItems();
        int shopItemCount = shopHealthItems.Count;

        // Search for the item
        for (int x = 0; x < shopItemCount; x++)
        {
            if (shopItem.GetShopItemName() == shopHealthItems[x].itemName)
            {
                // unit purchased the item
                shopItem.PurchaseShopItem(false);
                return;
            }
        }
    }

    public void ButtonTeamPage()
    {
        // Disable map UI
        GameManager.Instance.ToggleMap(false);

        GameManager.Instance.ToggleTeamSetup(true);

        GameManager.Instance.UpdateAllyVisibility(true, true);

        // Enable To Map Button
        GameManager.Instance.toMapButton.UpdateAlpha(1);
    }

    public void MasteryChangeUnit()
    {
        TeamSetup.Instance.GetActiveUnit().UpdateLastOpenedMastery(TeamSetup.Instance.activeMasteryType);

        GameManager.Instance.UpdateMasteryAllyPosition();

        GameManager.Instance.ToggleTeamSetup(true);
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
        // Prevent players from backing out of attacking before the final damage is finalised.
        if (disabled)
            return;

        // Return unit energy
        //GameManager.Instance.ReturnEnergyToUnit();

        GameManager.Instance.ResetButton(GameManager.Instance.attackButton);    // Allow attack button clicks

        GameManager.Instance.SetupPlayerSkillsUI(GameManager.Instance.GetActiveSkill());

        GameManager.Instance.UpdateEnemyPosition(true);
    }

    public void EnableButton()
    {
        disabled = false;
    }

    public void DisableButton()
    {
        disabled = true;
    }

    public void AttackButton()
    {
        /*
        // If unit doesnt have enough energy, do not allow skill to play out
        if (!GameManager.Instance.CheckIfEnergyAvailableSkill())
            return;
        */

        // If no units are selected, stop
        if (!GameManager.Instance.CheckIfAnyUnitsSelected())
            return;

        if (!disabled)
            disabled = true;
        else
            return;

        //GameManager.Instance.DisableButton(GameManager.Instance.endTurnButton);
        GameManager.Instance.DisableButton(GameManager.Instance.skill1Button);
        GameManager.Instance.DisableButton(GameManager.Instance.skill2Button);
        GameManager.Instance.DisableButton(GameManager.Instance.skill3Button);


        //GameManager.Instance.UpdateActiveUnitHealthBar(false);

        // Trigger Skill alert UI
        GameManager.Instance.GetActiveUnitFunctionality().TriggerTextAlert(GameManager.Instance.GetActiveSkill().skillName, 1, false);

        StartCoroutine(AttackButtonCont());
    }

    IEnumerator AttackButtonCont()
    {
        yield return new WaitForSeconds(GameManager.Instance.skillAlertAppearTime/2);
        GameManager.Instance.SetupPlayerWeaponUI();
    }

    public void MinusEnemyHPButton()
    {
        GameManager.Instance.ReduceAllEnemyHealth();
    }
    public void MinusAllyHPButton()
    {
        GameManager.Instance.ReduceAllPlayerHealth();
    }

    public void EndTurnButton()
    {
        if (disabled)
            return;

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

    public void ToggleButton(bool toggle)
    {
        if (toggle)
            UIbutton.UpdateAlpha(1);
        else
            UIbutton.UpdateAlpha(0);

        UIbutton.gameObject.GetComponent<CanvasGroup>().interactable = toggle;
        UIbutton.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = toggle;
        gameObject.GetComponent<Button>().interactable = toggle;
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
        //GameManager.Instance.UpdateAllSkillIconAvailability();

        if (disabled)
            return;

        // If skill is on cooldown, stop
        if (GameManager.Instance.GetActiveUnitFunctionality().GetSkill1CurCooldown() > 0)
            return;

        GameManager.Instance.ResetSelectedUnits();


        GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnit().skill1);
        GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnit().skill1);

        // If selected, unselect it, dont do skill more stuff
        if (GetIfSelected())
        {
            GameManager.Instance.DisableAllSkillSelections();
            ToggleSelected(false);
            GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnit().skill0);
            GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnit().skill0);
        }
        // If skill not already selected, select it and proceed
        else
        {
            GameManager.Instance.DisableAllSkillSelections();
            ToggleSelected(true);
        }

        GameManager.Instance.UpdateUnitSelection(GameManager.Instance.activeSkill);
        GameManager.Instance.UpdateUnitsSelectedText();
    }

    public void SelectSkill2()
    {
        if (disabled)
            return;

        // If skill is on cooldown, stop
        if (GameManager.Instance.GetActiveUnitFunctionality().GetSkill2CurCooldown() > 0)
            return;

        GameManager.Instance.ResetSelectedUnits();
        //GameManager.Instance.UpdateAllSkillIconAvailability();

        GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnit().skill2);
        GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnit().skill2);

        // If selected, unselect it, dont do skill more stuff
        if (GetIfSelected())
        {
            GameManager.Instance.DisableAllSkillSelections();
            ToggleSelected(false);
            GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnit().skill0);
            GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnit().skill0);
        }
        // If skill not already selected, select it and proceed
        else
        {
            GameManager.Instance.DisableAllSkillSelections();
            ToggleSelected(true);
        }

        GameManager.Instance.UpdateUnitSelection(GameManager.Instance.activeSkill);
        GameManager.Instance.UpdateUnitsSelectedText();
    }

    public void SelectSkill3()
    {
        if (disabled)
            return;

        // If skill is on cooldown, stop
        if (GameManager.Instance.GetActiveUnitFunctionality().GetSkill3CurCooldown() > 0)
            return;

        GameManager.Instance.ResetSelectedUnits();
        //GameManager.Instance.UpdateAllSkillIconAvailability();

        GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnit().skill3);
        GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnit().skill3);

        // If selected, unselect it, dont do skill more stuff
        if (GetIfSelected())
        {
            GameManager.Instance.DisableAllSkillSelections();
            ToggleSelected(false);
            GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnit().skill0);
            GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnit().skill0);
        }
        // If skill not already selected, select it and proceed
        else
        {
            GameManager.Instance.DisableAllSkillSelections();
            ToggleSelected(true);
        }

        GameManager.Instance.UpdateUnitSelection(GameManager.Instance.activeSkill);
        GameManager.Instance.UpdateUnitsSelectedText();
    }

    public void StopWeaponHitLine()
     {
        GameManager.Instance.ResetButton(GameManager.Instance.attackButton);    // Allow attack button clicks
        GameManager.Instance.DisableButton(GameManager.Instance.weaponBackButton);

        Weapon.instance.StartCoroutine(Weapon.instance.StopHitLine());
        Weapon.instance.ToggleEnabled(false);
        //GameManager.Instance.UpdateActiveUnitEnergyBar(false);

        GameManager.Instance.DisableAllSkillSelections();
    }
}
