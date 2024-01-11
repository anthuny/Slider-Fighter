using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFunctionality : MonoBehaviour
{
    public enum MasteryType { STATSTANDARD1, STATSTANDARD2, STATSTANDARD3, STATSTANDARD4, STATSTANDARD5, STATADVANCED1, STATADVANCED2, STATADVANCED3, STATADVANCED4, BG };
    public MasteryType curMasteryType;

    UnitFunctionality unitFunctionality;
    [SerializeField] private CanvasGroup buttonSelectionCG;
    bool selected;

    private MasteryType masteryType;

    private ShopItem shopItem;
    [SerializeField] private bool disabled;
    [SerializeField] private UIElement UIbutton;
    [SerializeField] private UIElement itemParent;


    [SerializeField] private bool startDisabled;
    [SerializeField] private bool playMapMusic;

    private bool settingsOpened = false;
    public bool buttonLocked;

    [SerializeField] private Gear gear;

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

    public void MenuSelectAlly()
    {
        if (!buttonLocked)
        {
            // Button Click SFX
            AudioManager.Instance.Play("Button_Click");

            CharacterCarasel.Instance.SelectAlly(this);
        }
    }

    public IEnumerator MenuUnitSelection()
    {
        yield return new WaitForSeconds(1.45f);

        CharacterCarasel.Instance.ToggleMenu(false);

        // Allow button selection
        buttonLocked = false;
    }
    public void MenuLeftArrowCarasel()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        CharacterCarasel.Instance.SpinCarasel(true);
    }

    public void MenuRightArrowCarasel()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        CharacterCarasel.Instance.SpinCarasel(false);
    }

    public void AdjustMusicVolume()
    {
        float val = GetComponent<Slider>().value;

        AudioManager.Instance.AdjustMusicTrackVolume(val);
    }

    public void AdjustSFXVolume()
    {
        float val = GetComponent<Slider>().value;

        AudioManager.Instance.AdjustSFXVolume(val);
    }

    public void ToggleSfxVolume()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        SettingsManager.Instance.ToggleSFX();
    }

    public void ToggleMusicVolume()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        SettingsManager.Instance.ToggleMusic();
    }

    public void ToggleSettingsTab()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        SettingsManager.Instance.ToggleSettingsTab();
    }

    public void ButtonEnterRoom()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        // Map Close SFX
        AudioManager.Instance.Play("Map_Close");

        //GameManager.Instance.UpdateAllyVisibility(true);

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
        // If owned gear is opened, do not allow map button to be selected
        if (OwnedLootInven.Instance.GetOwnedLootOpened())
            return;

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        // Map open SFX
        AudioManager.Instance.Play("Map_Open");

        if (playMapMusic)
            AudioManager.Instance.PauseMapMusic(false);

        MapManager.Instance.mapOverlay.ToggleEnterRoomButton(false);
        MapManager.Instance.mapOverlay.ToggleTeamPageButton(true);

        // Disable to map button
        GameManager.Instance.toMapButton.UpdateAlpha(0);
        GameManager.Instance.UpdateAllyVisibility(false);
        ShopManager.Instance.ToggleExitShopButton(false);
        ShopManager.Instance.ToggleRandomiser(false);

        // Toggle Map back on
        GameManager.Instance.ToggleMap(true, false, false, false);

        ShopManager.Instance.CloseShop();

        //GameManager.Instance.activeRoomAllUnitFunctionalitys = GameManager.Instance.oldActiveRoomAllUnitFunctionalitys;

        GameManager.Instance.ToggleTeamSetup(false);
        TeamGearManager.Instance.ToggleTeamGear(false);
        TeamItemsManager.Instance.ToggleTeamItems(false);

        TeamSetup.Instance.ToggleToMapButton(false);
        TeamGearManager.Instance.ToggleToMapButton(false);
        TeamItemsManager.Instance.ToggleToMapButton(false);

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
        if (TeamSetup.Instance.GetSelectedStat() == null)
            return;

        /*
        UIElement.MasteryType masteryType = TeamSetup.Instance.GetSelectedMastery().curMasteryType;

        if (masteryType == UIElement.MasteryType.L1 || masteryType == UIElement.MasteryType.R1)
        {
            if (TeamSetup.Instance.statsBase2.GetMasteryPointsAdded() != 0 || TeamSetup.Instance.masteryR2.GetMasteryPointsAdded() != 0)
                return;
            else if (TeamSetup.Instance.statsBase3.GetMasteryPointsAdded() != 0 || TeamSetup.Instance.masteryR3.GetMasteryPointsAdded() != 0)
                return;
            else if (TeamSetup.Instance.statsBase4.GetMasteryPointsAdded() != 0 || TeamSetup.Instance.masteryR4.GetMasteryPointsAdded() != 0)
                return;
        }
        else if (masteryType == UIElement.MasteryType.L2 || masteryType == UIElement.MasteryType.R2)
        {
            if (TeamSetup.Instance.statsBase3.GetMasteryPointsAdded() != 0 || TeamSetup.Instance.masteryR3.GetMasteryPointsAdded() != 0)
                return;
            else if (TeamSetup.Instance.statsBase4.GetMasteryPointsAdded() != 0 || TeamSetup.Instance.masteryR4.GetMasteryPointsAdded() != 0)
                return;
        }
        else if (masteryType == UIElement.MasteryType.L3 || masteryType == UIElement.MasteryType.R3)
        {
            if (TeamSetup.Instance.statsBase4.GetMasteryPointsAdded() != 0 || TeamSetup.Instance.masteryR4.GetMasteryPointsAdded() != 0)
                return;
        }
        */

        TeamSetup.Instance.StatRemovePoint();
    }

    public void MasteryPageChangeInc()
    {
        TeamSetup.Instance.ChangeStatPage(true);
    }

    public void MasteryPageChangeDec()
    {
        TeamSetup.Instance.ChangeStatPage(false);
    }



    public void ButtonSelectItem()
    {
        // Ensure the player cant select the same item
        if (ItemRewardManager.Instance.selectedItemName != itemParent.GetItemName())
            ButtonSelectItemCo();
    }

    public void ButtonSelectItemCo()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        // Put selection on seleted item
        if (itemParent != null)
        {
            ItemRewardManager.Instance.selectedItemName = itemParent.GetItemName();
            itemParent.ToggleSelected(true, true);
        }

        ItemRewardManager.Instance.UpdateItemDescription(true);

        ItemRewardManager.Instance.ToggleConfirmItemButton(true);
    }

    public void ConfirmItem()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        StartCoroutine(GameManager.Instance.SetupPostBattleUI(GameManager.Instance.playerWon));
    }

    public void ButtonSelectGear()
    {
        // If owned gear tab is opened, do not allow selecting a new gear
        if (gear != null)
        {
            if (OwnedLootInven.Instance.GetOwnedLootOpened() && gear.GetCurGearStatis() == Gear.GearStatis.UNOWNED)
                return;

            // If player selects an owned gear piece (todo: need to make this functional
            //if (OwnedGearInven.Instance.GetOwnedGearOpened() && gear.GetCurGearStatis() == Gear.GearStatis.OWNED)
            //    return;

            // If player selects a gear in the rewards for post game battle screen, stop
            if (gear.GetCurGearStatis() == Gear.GearStatis.REWARD)
                return;
        }

        // If the BG is selected
        if (curMasteryType == MasteryType.BG)
        {
            OwnedLootInven.Instance.ToggleOwnedGearDisplay(false);
            return;
        }

        OwnedLootInven.Instance.ClearOwnedItemsSlotsSelection();

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        if (gear != null)
        {
            if (gear.GetCurGearType() == Gear.GearType.HELMET)
                TeamGearManager.Instance.GearSelection(gear);
            else if (gear.GetCurGearType() == Gear.GearType.CHESTPIECE)
                TeamGearManager.Instance.GearSelection(gear);
            else if (gear.GetCurGearType() == Gear.GearType.LEGGINGS)
                TeamGearManager.Instance.GearSelection(gear);
            else if (gear.GetCurGearType() == Gear.GearType.BOOTS)
                TeamGearManager.Instance.GearSelection(gear);
        }

        // Display inven
        if (gear.isEmpty && gear.curGearStatis == Gear.GearStatis.DEFAULT)
        {
            OwnedLootInven.Instance.ToggleOwnedGearDisplay(true, "Owned Gear");
            //OwnedGearInven.Instance.ToggleOwnedGearEquipButton(true);
        }
    }

    public void EquipGear()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        if (gear != null)
        {
            TeamGearManager.Instance.UnequipGear();

            TeamGearManager.Instance.GearSelection(gear, true);
        }
    }

    public void SelectBaseItem()
    {
        OwnedLootInven.Instance.ClearOwnedItemsSlotsSelection();

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        TeamItemsManager.Instance.ItemSelection(gear, true);
        
        // Display inven
        if (gear.isEmpty)
        {
            OwnedLootInven.Instance.ToggleOwnedGearDisplay(true, "Owned Items");
            //OwnedGearInven.Instance.ToggleOwnedGearEquipButton(true);
        }
    }

    public void DisplayOwnedGear()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        if (gear != null)
        {
            TeamGearManager.Instance.GearSelection(gear);

            if (TeamGearManager.Instance.playerInGearTab)
                OwnedLootInven.Instance.ToggleOwnedGearDisplay(true, "Owned Gear");
            else
                OwnedLootInven.Instance.ToggleOwnedGearDisplay(true, "Owned Items");
        }
    }

    public void HideOwnedGear()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        OwnedLootInven.Instance.ToggleOwnedGearDisplay(false);
    }

    public void GearUnEquip()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        TeamGearManager.Instance.UnequipGear();
    }

    public void GearSell()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        TeamGearManager.Instance.SellGear();
        Debug.Log("b pressed");
    }

    public void Mastery()
    {
        // If the BG is selected
        if (curMasteryType == MasteryType.BG)
            return;

        UnitFunctionality unit = GameManager.Instance.GetActiveUnitFunctionality();
        UnitData unitData = GameManager.Instance.GetUnitData(0);

        TeamSetup.Instance.UpdateSelectedStat(this);

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        // If this mastery is locked, stop
        if (TeamSetup.Instance.GetSelectedStat().GetIsLocked())
        {
            Debug.Log("locked");
            return;
        }
        else if (curMasteryType == MasteryType.STATSTANDARD1)
        {
            TeamSetup.Instance.ResetStatSelection();
            TeamSetup.Instance.UpdateStatDescription(unit.GetStandardMastery(0));
            TeamSetup.Instance.ToggleStatSelection(TeamSetup.Instance.statsBase1, true);
            TeamSetup.Instance.statsBase1.UpdateContentSubText(TeamSetup.Instance.statsBase1.GetStatPointsAdded().ToString() + " / " + unit.GetStandardMastery(0).statMaxAmount);
        }
        else if (curMasteryType == MasteryType.STATSTANDARD2)
        {
            TeamSetup.Instance.ResetStatSelection();
            TeamSetup.Instance.UpdateStatDescription(unit.GetStandardMastery(1));
            TeamSetup.Instance.ToggleStatSelection(TeamSetup.Instance.statsBase2, true);
            TeamSetup.Instance.statsBase2.UpdateContentSubText(TeamSetup.Instance.statsBase2.GetStatPointsAdded().ToString() + " / " + unit.GetStandardMastery(1).statMaxAmount);
        }
        else if (curMasteryType == MasteryType.STATSTANDARD3)
        {
            TeamSetup.Instance.ResetStatSelection();
            TeamSetup.Instance.UpdateStatDescription(unit.GetStandardMastery(2));
            TeamSetup.Instance.ToggleStatSelection(TeamSetup.Instance.statsBase3, true);
            TeamSetup.Instance.statsBase3.UpdateContentSubText(TeamSetup.Instance.statsBase3.GetStatPointsAdded().ToString() + " / " + unit.GetStandardMastery(2).statMaxAmount);
        }
        else if (curMasteryType == MasteryType.STATSTANDARD4)
        {
            TeamSetup.Instance.ResetStatSelection();
            TeamSetup.Instance.UpdateStatDescription(unit.GetStandardMastery(3));
            TeamSetup.Instance.ToggleStatSelection(TeamSetup.Instance.statsBase4, true);
            TeamSetup.Instance.statsBase4.UpdateContentSubText(TeamSetup.Instance.statsBase4.GetStatPointsAdded().ToString() + " / " + unit.GetStandardMastery(3).statMaxAmount);
        }
        else if (curMasteryType == MasteryType.STATSTANDARD5)
        {
            TeamSetup.Instance.ResetStatSelection();
            TeamSetup.Instance.UpdateStatDescription(unit.GetStandardMastery(4));
            TeamSetup.Instance.ToggleStatSelection(TeamSetup.Instance.statsBase5, true);
            TeamSetup.Instance.statsBase5.UpdateContentSubText(TeamSetup.Instance.statsBase5.GetStatPointsAdded().ToString() + " / " + unit.GetStandardMastery(3).statMaxAmount);
        }
        else if (curMasteryType == MasteryType.STATADVANCED1)
        {
            TeamSetup.Instance.ResetStatSelection();
            TeamSetup.Instance.UpdateStatDescription(unit.GetAdvancedMastery(0));
            TeamSetup.Instance.ToggleStatSelection(TeamSetup.Instance.statsAdvanced1, true);
            TeamSetup.Instance.statsAdvanced1.UpdateContentSubText(TeamSetup.Instance.statsAdvanced1.GetStatPointsAdded().ToString() + " / " + unit.GetAdvancedMastery(0).statMaxAmount);
        }
        else if (curMasteryType == MasteryType.STATADVANCED2)
        {
            TeamSetup.Instance.ResetStatSelection();
            TeamSetup.Instance.UpdateStatDescription(unit.GetAdvancedMastery(1));
            TeamSetup.Instance.ToggleStatSelection(TeamSetup.Instance.statsAdvanced2, true);
            TeamSetup.Instance.statsAdvanced2.UpdateContentSubText(TeamSetup.Instance.statsAdvanced2.GetStatPointsAdded().ToString() + " / " + unit.GetAdvancedMastery(1).statMaxAmount);
        }
        else if (curMasteryType == MasteryType.STATADVANCED3)
        {
            TeamSetup.Instance.ResetStatSelection();
            TeamSetup.Instance.UpdateStatDescription(unit.GetAdvancedMastery(2));
            TeamSetup.Instance.ToggleStatSelection(TeamSetup.Instance.statsAdvanced3, true);
            TeamSetup.Instance.statsAdvanced3.UpdateContentSubText(TeamSetup.Instance.statsAdvanced3.GetStatPointsAdded().ToString() + " / " + unit.GetAdvancedMastery(2).statMaxAmount);
        }
        else if (curMasteryType == MasteryType.STATADVANCED4)
        {
            TeamSetup.Instance.ResetStatSelection();
            TeamSetup.Instance.UpdateStatDescription(unit.GetAdvancedMastery(3));
            TeamSetup.Instance.ToggleStatSelection(TeamSetup.Instance.statsAdvanced4, true);
            TeamSetup.Instance.statsAdvanced4.UpdateContentSubText(TeamSetup.Instance.statsAdvanced4.GetStatPointsAdded().ToString() + " / " + unit.GetAdvancedMastery(3).statMaxAmount);
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

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

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
        List<ItemPiece> shopCombatItems = new List<ItemPiece>();
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
        List<ItemPiece> shopHealthItems = new List<ItemPiece>();
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
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        TeamItemsManager.Instance.playerInItemTab = false;
        TeamGearManager.Instance.playerInGearTab = false;
        TeamSetup.Instance.playerInTeamTab = true;

        TeamSetup.Instance.ResetStatPageCount();

        // Disable map UI
        GameManager.Instance.ToggleMap(false);

        // temp 
        //GameManager.Instance.ToggleTeamSetup(true);
        TeamGearManager.Instance.ToggleTeamGear(true);

        TeamSetup.Instance.ToggleToMapButton(false);
        TeamGearManager.Instance.ToggleToMapButton(true);
        TeamItemsManager.Instance.ToggleToMapButton(false);
        
        // Toggle unequip button
        TeamItemsManager.Instance.ToggleUnequipButton(false);
        TeamGearManager.Instance.ToggleUnequipButton(true);

        MapManager.Instance.mapOverlay.ToggleTeamPageButton(false);

        GameManager.Instance.ToggleSkillVisibility(false);

        // Enable To Map Button
        //GameManager.Instance.toMapButton.UpdateAlpha(1);

        //TeamSetup.Instance.ToggleToMapButton(true);
    }

    // Gear to Team Setup
    public void ButtonTeamPageFromGear()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        // Disable Owned Gear Display
        OwnedLootInven.Instance.ToggleOwnedGearDisplay(false);

        TeamItemsManager.Instance.playerInItemTab = false;
        TeamGearManager.Instance.playerInGearTab = false;
        TeamSetup.Instance.playerInTeamTab = true;

        //TeamSetup.Instance.ResetStatPageCount();

        GameManager.Instance.ToggleTeamSetup(true);
        TeamGearManager.Instance.ToggleTeamGear(false);
        TeamItemsManager.Instance.ToggleTeamItems(false);

        TeamSetup.Instance.ToggleToMapButton(true);
        TeamGearManager.Instance.ToggleToMapButton(false);
        TeamItemsManager.Instance.ToggleToMapButton(false);

        // Toggle unequip button
        TeamItemsManager.Instance.ToggleUnequipButton(false);
        TeamGearManager.Instance.ToggleUnequipButton(false);

        GameManager.Instance.ToggleSkillVisibility(false);

        //TeamSetup.Instance.ToggleToMapButton(true);

        // Enable To Map Button
        GameManager.Instance.toMapButton.UpdateAlpha(1);
    }

    // Team Setup to Item Tab
    public void ButtonGearTab()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        TeamSetup.Instance.playerInTeamTab = false;
        TeamItemsManager.Instance.playerInItemTab = true;

        GameManager.Instance.ToggleTeamSetup(false);
        TeamItemsManager.Instance.ToggleTeamItems(true);

        TeamSetup.Instance.ToggleToMapButton(false);
        TeamGearManager.Instance.ToggleToMapButton(false);
        TeamItemsManager.Instance.ToggleToMapButton(true);

        // Toggle unequip button
        TeamItemsManager.Instance.ToggleUnequipButton(true);
        TeamGearManager.Instance.ToggleUnequipButton(false);
    }


    // Items to Gear
    public void ButtonTeamSetupTab()
    {
        // Disable Owned Gear Display
        OwnedLootInven.Instance.ToggleOwnedGearDisplay(false);

        TeamItemsManager.Instance.playerInItemTab = false;
        TeamSetup.Instance.playerInTeamTab = true;
        TeamGearManager.Instance.playerInGearTab = false;

        TeamItemsManager.Instance.ToggleTeamItems(false);
        GameManager.Instance.ToggleTeamSetup(true);

        TeamSetup.Instance.ToggleToMapButton(false);
        TeamGearManager.Instance.ToggleToMapButton(true);
        TeamItemsManager.Instance.ToggleToMapButton(false);

        // Toggle unequip button
        TeamItemsManager.Instance.ToggleUnequipButton(false);
        TeamGearManager.Instance.ToggleUnequipButton(true);

        ButtonTeamPage();
    }

    public void MasteryChangeUnit()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        TeamSetup.Instance.ResetStatPageCount();

        TeamSetup.Instance.GetActiveUnit().UpdateLastOpenedMastery(TeamSetup.Instance.activeStatType);

        GameManager.Instance.UpdateMasteryAllyPosition();

        GameManager.Instance.ToggleTeamSetup(true);
    }

    public void PostBattleToMapButton()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        // Map open SFX
        AudioManager.Instance.Play("Map_Open");

        // Disable post battle to map button for next post battle scene
        StartCoroutine(PostBattle.Instance.ToggleButtonPostBattleMap(false));

        // Toggle Map back on
        GameManager.Instance.ToggleMap(true, false, false, false);

        // Disable post battle UI
        GameManager.Instance.postBattleUI.TogglePostBattleUI(false);

        GameManager.Instance.map.ClearRoom(true);

        GameManager.Instance.ResetRoom(true);
    }

    public void WeaponBackButton()
    {
        // Prevent players from backing out of attacking before the final damage is finalised.
        if (disabled)
            return;

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        // Return unit energy
        //GameManager.Instance.ReturnEnergyToUnit();

        GameManager.Instance.ResetButton(GameManager.Instance.attackButton);    // Allow attack button clicks

        GameManager.Instance.SetupPlayerSkillsUI(GameManager.Instance.GetActiveSkill());

        GameManager.Instance.UpdateEnemyPosition(true);

        GameManager.Instance.ToggleSelectingUnits(true);
        GameManager.Instance.ToggleAllowSelection(true);
        //GameManager.Instance.ResetSelectedUnits();
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

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        GameManager.Instance.PlayerAttack();

        StartCoroutine(AttackButtonCont());
    }

    IEnumerator AttackButtonCont()
    {
        yield return new WaitForSeconds(GameManager.Instance.skillAlertAppearTime/2);
        GameManager.Instance.SetupPlayerWeaponUI();
    }

    public void MinusEnemyHPButton()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        GameManager.Instance.ReduceAllEnemyHealth();
    }
    public void MinusAllyHPButton()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        GameManager.Instance.ReduceAllPlayerHealth();
    }

    public void EndTurnButton()
    {
        if (disabled)
            return;

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

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

    public void HeroRoomPromptYes()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        HeroRoomManager.Instance.TogglePrompt(false, false);
    }

    public void HeroRoomPromptNo()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        HeroRoomManager.Instance.TogglePrompt(false);
    }
    public void SelectUnit()
    {
        if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
        {
            // Button Click SFX
            AudioManager.Instance.Play("Button_Click");

            if (GameManager.Instance.IsEnemyTaunting().Count >= 1)
            {
                for (int i = 0; i < GameManager.Instance.IsEnemyTaunting().Count; i++)
                {
                    if (GameManager.Instance.IsEnemyTaunting()[i] == unitFunctionality)
                        GameManager.Instance.SelectUnit(unitFunctionality);
                    else
                    {
                        // Check if active skill targets allies, and is player type, then allow it
                        if (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT && GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
                            GameManager.Instance.SelectUnit(unitFunctionality);
                    }
                }

                return;
            }
            else
                GameManager.Instance.SelectUnit(unitFunctionality);
        }
    }

    public void SelectSkill0()
    {
        //GameManager.Instance.UpdateAllSkillIconAvailability();

        if (disabled)
            return;

        // If skill is on cooldown, stop
        if (GameManager.Instance.GetActiveUnitFunctionality().GetSkill0CurCooldown() > 0)
            return;

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        GameManager.Instance.ResetSelectedUnits();

        GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnitFunctionality().unitData.skill0);
        GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnitFunctionality().unitData.skill0);

        // If selected, unselect it, dont do skill more stuff
        if (GetIfSelected())
        {
            //GameManager.Instance.DisableAllSkillSelections();
            //ToggleSelected(false);
            //GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnitFunctionality().unitData.skill0);
            //GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnitFunctionality().unitData.skill0);
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

    public void SelectSkill1()
    {
        //GameManager.Instance.UpdateAllSkillIconAvailability();

        if (disabled)
            return;

        // If skill is on cooldown, stop
        if (GameManager.Instance.GetActiveUnitFunctionality().GetSkill1CurCooldown() > 0)
            return;

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        GameManager.Instance.ResetSelectedUnits();

        GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnitFunctionality().unitData.skill1);
        GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnitFunctionality().unitData.skill1);

        // If selected, unselect it, dont do skill more stuff
        if (GetIfSelected())
        {
            GameManager.Instance.DisableAllSkillSelections(true);
            GameManager.Instance.EnableSkill0Selection();
            ToggleSelected(false);
            GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnitFunctionality().unitData.skill0);
            GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnitFunctionality().unitData.skill0);
        }
        // If skill not already selected, select it and proceed
        else
        {
            GameManager.Instance.DisableAllSkillSelections(false);
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

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        GameManager.Instance.ResetSelectedUnits();
        //GameManager.Instance.UpdateAllSkillIconAvailability();

        GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnitFunctionality().unitData.skill2);
        GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnitFunctionality().unitData.skill2);

        // If selected, unselect it, dont do skill more stuff
        if (GetIfSelected())
        {
            GameManager.Instance.DisableAllSkillSelections(true);
            GameManager.Instance.EnableSkill0Selection();
            ToggleSelected(false);
            GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnitFunctionality().unitData.skill0);
            GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnitFunctionality().unitData.skill0);
        }
        // If skill not already selected, select it and proceed
        else
        {
            GameManager.Instance.DisableAllSkillSelections(false);
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

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        GameManager.Instance.ResetSelectedUnits();
        //GameManager.Instance.UpdateAllSkillIconAvailability();

        GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnitFunctionality().unitData.skill3);
        GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnitFunctionality().unitData.skill3);

        // If selected, unselect it, dont do skill more stuff
        if (GetIfSelected())
        {
            GameManager.Instance.DisableAllSkillSelections(true);
            GameManager.Instance.EnableSkill0Selection();
            ToggleSelected(false);
            GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnitFunctionality().unitData.skill0);
            GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnitFunctionality().unitData.skill0);
        }
        // If skill not already selected, select it and proceed
        else
        {
            GameManager.Instance.DisableAllSkillSelections(false);
            ToggleSelected(true);
        }

        GameManager.Instance.UpdateUnitSelection(GameManager.Instance.activeSkill);
        GameManager.Instance.UpdateUnitsSelectedText();
    }

    public void MapUpArrow()
    {
        Debug.Log("up");
    }

    public void MapDownArrow()
    {
        Debug.Log("down");
    }
}
