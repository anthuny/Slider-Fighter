using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonFunctionality : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum MasteryType { Skill1, Skill2, Skill3, Skill4, BG, NOTHING };
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

    public Slot slot;

    public float heldTimer;
    public bool isHeldDownUnit;
    public bool isHeldDownStat;
    public bool unitIsSelected;
    public UnitFunctionality unit;
    public bool locked;


    public float effectHeldTimer;
    public bool isHeldDownEffect;
    public bool isEffectButton;
    public bool isEnabled;
    public bool isLocked;
    public ButtonFunctionality mainButton;
    public UIElement mainUIelement;
    public bool isStatButton;

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
        yield return new WaitForSeconds(1.5f);

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

    public void ButtonAddSkillPointIncreaseTargets()
    {
        //Debug.Log("a");
        SkillsTabManager.Instance.SkillPointAdd(0);
    }

    public void ButtonAddSkillPointIncreasePower()
    {
        SkillsTabManager.Instance.SkillPointAdd(1);
    }

    public void ButtonAddSkillPointIncreaseEffect()
    {
        SkillsTabManager.Instance.SkillPointAdd(2);
    }

    public void ButtonOpenMap()
    {
        // If owned gear is opened, do not allow map button to be selected
        if (OwnedLootInven.Instance.GetOwnedLootOpened())
            return;

        SettingsManager.Instance.ToggleSettingsButton(true);

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        // Map open SFX
        AudioManager.Instance.Play("Map_Open");

        if (playMapMusic)
            AudioManager.Instance.PauseMapMusic(false);

        MapManager.Instance.mapOverlay.ToggleEnterRoomButton(false);
        MapManager.Instance.mapOverlay.ToggleTeamPageButton(true);

        TeamGearManager.Instance.ToggleGearButtons(false);

        // Disable to map button
        GameManager.Instance.toMapButton.UpdateAlpha(0);

        ShopManager.Instance.ToggleExitShopButton(false);
        ShopManager.Instance.ToggleRandomiser(false);

        TeamGearManager.Instance.ToggleAllSlotsClickable(false, true);

        // Toggle Map back on
        GameManager.Instance.ToggleMap(true, false, false, false);

        ShopManager.Instance.CloseShop();

        //GameManager.Instance.activeRoomAllUnitFunctionalitys = GameManager.Instance.oldActiveRoomAllUnitFunctionalitys;

        GameManager.Instance.SkillsTabChangeAlly(false, false, false);
        TeamGearManager.Instance.ToggleTeamGear(false);
        TeamItemsManager.Instance.ToggleTeamItems(false);

        SkillsTabManager.Instance.ToggleToMapButton(false);
        TeamGearManager.Instance.ToggleToMapButton(false);
        TeamItemsManager.Instance.ToggleToMapButton(false);

        GameManager.Instance.UpdateAllyVisibility(false);
        // Reset room, enemes only
        GameManager.Instance.ResetRoom(true);
    }

    public void ResetMasteryTree()
    {
        SkillsTabManager.Instance.ResetTeamSetup();
    }

    /*
    public void MasteryAddPoint()
    {
        SkillsTabManager.Instance.SkillAddPoint();
    }

    public void MasteryRemovePoint()
    {
        if (SkillsTabManager.Instance.GetSelectedSkillSlot() == null)
            return;

        
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
        

        SkillsTabManager.Instance.StatRemovePoint();
    }

    */
    /*
    public void MasteryPageChangeInc()
    {
        TeamSetup.Instance.ChangeStatPage(true);
    }

    public void MasteryPageChangeDec()
    {
        TeamSetup.Instance.ChangeStatPage(false);
    }
    */


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

        if (RoomManager.Instance.GetActiveRoom().GetRoomType() == RoomMapIcon.RoomType.HERO)
            StartCoroutine(GameManager.Instance.HeroRetrievalScene());
        else
            StartCoroutine(GameManager.Instance.SetupPostBattleUI(GameManager.Instance.playerWon));
    }

    public void ButtonSlotDetails(bool openedOwnedSlots = true)
    {
        // If button selected is unlocked, open owned stuff
        if (mainButton != null)
        {
            if (mainButton.isLocked)
                return;
        }
        if (SkillsTabManager.Instance.playerInSkillTab)
        {
            //SkillSlotSelection();
            Debug.Log("aa");
            //SkillsTabManager.Instance.UpdateSelectedSkillBase(this);

            if (!OwnedLootInven.Instance.ownedLootOpened)
            {
                if (openedOwnedSlots && SkillsTabManager.Instance.activeSkillBase != null)
                    OwnedLootInven.Instance.ToggleOwnedGearDisplay(true, "Ally Skills");
            }
        }
        else if (TeamGearManager.Instance.playerInGearTab)
            OwnedLootInven.Instance.ToggleOwnedGearDisplay(true, "Owned Gear");
        else if (TeamItemsManager.Instance.playerInItemTab)
            OwnedLootInven.Instance.ToggleOwnedGearDisplay(true, "Owned Items");
    }

    public void ButtonSelectGear()
    {
        if (OwnedLootInven.Instance.ownedLootOpened)
            return;

        // If owned gear tab is opened, do not allow selecting a new gear
        if (slot != null)
        {
            // Do not allow selection for base gear slots that already have gear in it
            if (!slot.isEmpty && OwnedLootInven.Instance.GetOwnedLootOpened())
                return;

            if (OwnedLootInven.Instance.GetOwnedLootOpened() && slot.GetCurGearStatis() == Slot.SlotStatis.UNOWNED)
                return;

            // If player selects an owned gear piece (todo: need to make this functional
            //if (OwnedGearInven.Instance.GetOwnedGearOpened() && gear.GetCurGearStatis() == Gear.GearStatis.OWNED)
            //    return;

            // If player selects a gear in the rewards for post game battle screen, stop
            if (slot.GetCurGearStatis() == Slot.SlotStatis.REWARD)
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

        if (slot != null)
            TeamGearManager.Instance.GearSelection(slot);

        // Display inven
        if (slot.isEmpty && slot.curSlotStatis == Slot.SlotStatis.DEFAULT)
        {
            TeamGearManager.Instance.ToggleAllSlotsClickable(true, true, true, true);

            OwnedLootInven.Instance.ToggleOwnedGearDisplay(true, "Owned Gear");
            //OwnedGearInven.Instance.ToggleOwnedGearEquipButton(true);

        }
    }

    // Equipping gear with Gear Button
    public void EquipGear()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");


        if (slot != null)
        {
            if (TeamGearManager.Instance.playerInGearTab)
            {
                TeamGearManager.Instance.UnequipGear();

                TeamGearManager.Instance.GearSelection(slot);
            }
            else if (SkillsTabManager.Instance.playerInSkillTab)
            {
                //SkillsTabManager.Instance.UnequipSkill();



                //SkillsTabManager.Instance.UpdateSelectedSkillBase(this);

                Debug.Log("ccc");
                SkillsTabManager.Instance.UpdateSelectedSkillBase(SkillsTabManager.Instance.selectedSkillBase.buttonCG.GetComponent<ButtonFunctionality>());

                SkillsTabManager.Instance.SkillSelection(slot);



                //ButtonSlotDetails(false);

                //SkillsTabManager.Instance.UpdateSkillStatDetails();
                SkillsTabManager.Instance.selectedSkillBase.buttonCG.GetComponent<ButtonFunctionality>().SkillSlotSelection();

                // doing this doesnt work get rid of if statement figre outa dif way 

                //if (SkillsTabManager.Instance.selectedSkillBase.curStatType = UIElement.StatType.SKILLSLOT1)

                
                SkillsTabManager.Instance.UpdateSkillStatDetails();
            }
        }
    }

    public void SelectBaseItem()
    {
        OwnedLootInven.Instance.ClearOwnedItemsSlotsSelection();

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        Item item = slot.gameObject.GetComponent<Item>();
        TeamItemsManager.Instance.ItemSelection(slot, item);
        
        // Display inven
        if (slot.isEmpty)
        {
            OwnedLootInven.Instance.ToggleOwnedGearDisplay(true, "Owned Items");
            //OwnedGearInven.Instance.ToggleOwnedGearEquipButton(true);
        }
    }

    public void HideOwnedGear()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        if (TeamGearManager.Instance.playerInGearTab)
            TeamGearManager.Instance.ToggleAllSlotsClickable(false, true, false, false);

        OwnedLootInven.Instance.ToggleOwnedGearDisplay(false, "", true);
    }

    public void GearUnEquip()
    {
        if (OwnedLootInven.Instance.ownedLootOpened)
            return;

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

    public void SkillSlotSelection()
    {
        // If the BG is selected
        if (curMasteryType == MasteryType.BG || isLocked)
            return;

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        OwnedLootInven.Instance.ClearOwnedItemsSlotsSelection();

        if (curMasteryType == MasteryType.Skill1)
        {
            //SkillsTabManager.Instance.ResetSkillsBaseSelection();
            //SkillsTabManager.Instance.UpdateSkillDescription("");
            SkillsTabManager.Instance.ToggleSkillBaseSelection(SkillsTabManager.Instance.skillBase1, true);
            //SkillsTabManager.Instance.skillBase1.UpdateContentSubText(SkillsTabManager.Instance.skillBase1.GetStatPointsAdded().ToString());
        }
        else if (curMasteryType == MasteryType.Skill2)
        {
            //SkillsTabManager.Instance.ResetSkillsBaseSelection();
            //SkillsTabManager.Instance.UpdateSkillDescription("");
            SkillsTabManager.Instance.ToggleSkillBaseSelection(SkillsTabManager.Instance.skillBase2, true);
            //SkillsTabManager.Instance.skillBase2.UpdateContentSubText(SkillsTabManager.Instance.skillBase2.GetStatPointsAdded().ToString());
        }
        else if (curMasteryType == MasteryType.Skill3)
        {
            //SkillsTabManager.Instance.ResetSkillsBaseSelection();
            //SkillsTabManager.Instance.UpdateSkillDescription("");
            SkillsTabManager.Instance.ToggleSkillBaseSelection(SkillsTabManager.Instance.skillBase3, true);
            //SkillsTabManager.Instance.skillBase3.UpdateContentSubText(SkillsTabManager.Instance.skillBase3.GetStatPointsAdded().ToString());
        }
        else if (curMasteryType == MasteryType.Skill4)
        {
            //SkillsTabManager.Instance.ResetSkillsBaseSelection();
            //SkillsTabManager.Instance.UpdateSkillDescription("");
            SkillsTabManager.Instance.ToggleSkillBaseSelection(SkillsTabManager.Instance.skillBase4, true);
            //SkillsTabManager.Instance.skillBase4.UpdateContentSubText(SkillsTabManager.Instance.skillBase4.GetStatPointsAdded().ToString());
        }

        Debug.Log("bbb");
        SkillsTabManager.Instance.UpdateSelectedSkillBase(this);

        if (slot != null)
        {
            SkillsTabManager.Instance.SkillSelection(slot, true);
        }


        SkillsTabManager.Instance.UpdateUnspentPointsText(SkillsTabManager.Instance.CalculateUnspentSkillPoints());


        //OwnedLootInven.Instance.ToggleOwnedGearDisplay(true, "Ally Skills");
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

    // Gear tab from map
    public void ButtonGearTabFromMap()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        TeamItemsManager.Instance.playerInItemTab = false;
        SkillsTabManager.Instance.playerInSkillTab = false;
        TeamGearManager.Instance.playerInGearTab = true;

        SkillsTabManager.Instance.ResetStatPageCount();

        // Toggle button / display for skill stats
        GameManager.Instance.abilityDetailsUI.ToggleAllStats(false);

        // Disable map UI
        GameManager.Instance.ToggleMap(false);

        // temp 
        //GameManager.Instance.SkillsTabChangeAlly(true);
        TeamGearManager.Instance.ToggleTeamGear(true);

        SkillsTabManager.Instance.ToggleToMapButton(false);
        TeamGearManager.Instance.ToggleToMapButton(true);
        TeamItemsManager.Instance.ToggleToMapButton(false);

        TeamGearManager.Instance.ToggleGearButtons();

        // Toggle unequip button
        TeamItemsManager.Instance.ToggleUnequipButton(false);
        TeamGearManager.Instance.ToggleUnequipButton(true);

        MapManager.Instance.mapOverlay.ToggleTeamPageButton(false);

        GameManager.Instance.ToggleSkillVisibility(false);

        SettingsManager.Instance.ToggleSettingsButton(false);

        // Enable To Map Button
        //GameManager.Instance.toMapButton.UpdateAlpha(1);

        //TeamSetup.Instance.ToggleToMapButton(true);
    }

    // Gear tab to Skill tab
    public void ButtonSkillsTabFromGearTab()
    {        
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        // Disable Owned Gear Display
        OwnedLootInven.Instance.ToggleOwnedGearDisplay(false);

        TeamItemsManager.Instance.playerInItemTab = false;
        TeamGearManager.Instance.playerInGearTab = false;
        SkillsTabManager.Instance.playerInSkillTab = true;

        //TeamSetup.Instance.ResetStatPageCount();

        SkillsTabManager.Instance.ToggleOwnedSkillSlotsClickable(true);

        GameManager.Instance.SkillsTabChangeAlly(true, false, true);
        
        TeamGearManager.Instance.ToggleTeamGear(false);
        TeamItemsManager.Instance.ToggleTeamItems(false);

        TeamGearManager.Instance.ToggleGearButtons(false);

        SkillsTabManager.Instance.ToggleToMapButton(true);
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

    // Skill tab to Item Tab
    public void ButtonGearTab()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        OwnedLootInven.Instance.ToggleOwnedGearDisplay(false);

        TeamGearManager.Instance.playerInGearTab = false;
        SkillsTabManager.Instance.playerInSkillTab = false;
        TeamItemsManager.Instance.playerInItemTab = true;

        SkillsTabManager.Instance.ToggleOwnedSkillSlotsClickable(true);

        GameManager.Instance.SkillsTabChangeAlly(false, true, false);
        TeamItemsManager.Instance.ToggleTeamItems(true);

        SkillsTabManager.Instance.ToggleToMapButton(false);
        TeamGearManager.Instance.ToggleToMapButton(false);
        TeamItemsManager.Instance.ToggleToMapButton(true);

        // Toggle unequip button
        TeamItemsManager.Instance.ToggleUnequipButton(true);
        TeamGearManager.Instance.ToggleUnequipButton(false);
    }


    // Item tab to Gear tab 
    public void ButtonTeamSetupTab()
    {
        // Disable Owned Gear Display
        OwnedLootInven.Instance.ToggleOwnedGearDisplay(false);

        TeamItemsManager.Instance.playerInItemTab = false;
        SkillsTabManager.Instance.playerInSkillTab = false;
        TeamGearManager.Instance.playerInGearTab = true;

        TeamItemsManager.Instance.ToggleTeamItems(false);
        GameManager.Instance.SkillsTabChangeAlly(false, true, false);

        TeamGearManager.Instance.ToggleGearButtons(true);

        SkillsTabManager.Instance.ToggleToMapButton(false);
        TeamGearManager.Instance.ToggleToMapButton(true);
        TeamItemsManager.Instance.ToggleToMapButton(false);

        // Toggle unequip button
        TeamItemsManager.Instance.ToggleUnequipButton(false);
        TeamGearManager.Instance.ToggleUnequipButton(true);

        ButtonGearTabFromMap();
    }

    public void MasteryChangeUnit()
    {


        //TeamSetup.Instance.ResetStatPageCount();

        //TeamSetup.Instance.GetActiveUnit().UpdateLastOpenedMastery(TeamSetup.Instance.activeStatType);
        Debug.Log("Sssssssssssss");
        //if (GameManager.Instance.activeTeam.Count != 1)
        GameManager.Instance.SkillsTabChangeAlly(true, false, true, true);

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");
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

        // Update unit sorting
        GameManager.Instance.UpdateAllUnitsSorting(GameManager.Instance.unitTabSortingLevel);


        if (GameManager.Instance.activeRoomAllies.Count > 0)
        {
            if (GameManager.Instance.GetActiveAlly())
            {
                GameManager.Instance.SkillsTabChangeAlly(true);
                GameManager.Instance.SkillsTabChangeAlly(false, true);
            }
        }
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
        //Debug.Log("Raw - Toggling " + toggle);

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

    public void SelectSkill0()
    {
        //GameManager.Instance.UpdateAllSkillIconAvailability();

        if (disabled)
            return;

        // If skill is on cooldown, stop
        if (GameManager.Instance.GetActiveUnitFunctionality().GetSkillCurCooldown(GameManager.Instance.GetActiveUnitFunctionality().GetSkill(0)) > 0)
            return;

        // If skill is locked, stop
        if (SkillsTabManager.Instance.skillBase1.GetIsLocked())
            return;

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        GameManager.Instance.ResetSelectedUnits();

        GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnitFunctionality().GetSkill(0));
        GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnitFunctionality().GetSkill(0));

        // If selected, unselect it, dont do skill more stuff
        if (GetIfSelected())
        {
            // Check if all other skills are on cooldown AND check if this skill is 0 cooldown, if not allow this deselect


            //GameManager.Instance.DisableAllSkillSelections();
            //ToggleSelected(false);
            //GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnitFunctionality().GetNoCDSkill());
            //GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnitFunctionality().GetNoCDSkill());
            //GameManager.Instance.EnableFreeSkillSelection();
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
        if (GameManager.Instance.GetActiveUnitFunctionality().GetSkillCurCooldown(GameManager.Instance.GetActiveUnitFunctionality().GetSkill(1)) > 0)
            return;

        // If skill is locked, stop
        if (SkillsTabManager.Instance.skillBase2.GetIsLocked())
            return;

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        GameManager.Instance.ResetSelectedUnits();

        GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnitFunctionality().GetSkill(1));
        GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnitFunctionality().GetSkill(1));

        // If selected, unselect it, dont do skill more stuff
        if (GetIfSelected())
        {
            /*
            GameManager.Instance.DisableAllSkillSelections(true);
            //GameManager.Instance.EnableFreeSkillSelection();
            ToggleSelected(false);
            //GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnitFunctionality().GetNoCDSkill());
            //GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnitFunctionality().GetNoCDSkill());
            GameManager.Instance.EnableFreeSkillSelection();
            */
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
        if (GameManager.Instance.GetActiveUnitFunctionality().GetSkillCurCooldown(GameManager.Instance.GetActiveUnitFunctionality().GetSkill(2)) > 0)
            return;

        // If skill is locked, stop
        if (SkillsTabManager.Instance.skillBase3.GetIsLocked())
            return;

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        GameManager.Instance.ResetSelectedUnits();
        //GameManager.Instance.UpdateAllSkillIconAvailability();

        GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnitFunctionality().GetSkill(2));
        GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnitFunctionality().GetSkill(2));

        // If selected, unselect it, dont do skill more stuff
        if (GetIfSelected())
        {
            /*
            GameManager.Instance.DisableAllSkillSelections(true);
            //GameManager.Instance.EnableFreeSkillSelection();
            ToggleSelected(false);
            //GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnitFunctionality().GetNoCDSkill());
            //GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnitFunctionality().GetNoCDSkill());
            GameManager.Instance.EnableFreeSkillSelection();
            */
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
        if (GameManager.Instance.GetActiveUnitFunctionality().GetSkillCurCooldown(GameManager.Instance.GetActiveUnitFunctionality().GetSkill(3)) > 0)
            return;

        // If skill is locked, stop
        if (SkillsTabManager.Instance.skillBase4.GetIsLocked())
            return;

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        GameManager.Instance.ResetSelectedUnits();
        //GameManager.Instance.UpdateAllSkillIconAvailability();

        GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnitFunctionality().GetSkill(3));
        GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnitFunctionality().GetSkill(3));

        // If selected, unselect it, dont do skill more stuff
        if (GetIfSelected())
        {
            /*
            GameManager.Instance.DisableAllSkillSelections(true);
            //GameManager.Instance.EnableFreeSkillSelection();
            ToggleSelected(false);
            //GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnitFunctionality().GetNoCDSkill());
            //GameManager.Instance.UpdateSkillDetails(GameManager.Instance.GetActiveUnitFunctionality().GetNoCDSkill());
            GameManager.Instance.EnableFreeSkillSelection();
            */
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

    void Update()
    {
        if (isHeldDownUnit)
            heldTimer += Time.deltaTime;
        else if (isHeldDownStat)
            heldTimer += Time.deltaTime;


        // If effect
        if (isHeldDownEffect)
        {
            effectHeldTimer += Time.deltaTime;
        }

        if (effectHeldTimer > 0 && isHeldDownEffect)
            SelectEffect();

        // If unit
        if (heldTimer > GameManager.Instance.maxHeldTimeTooltip)
        {
            if (GetComponentInParent<UnitFunctionality>() != null)
            {
                UnitFunctionality unit = GetComponentInParent<UnitFunctionality>();

                unit.ToggleTooltipStats(true);
                locked = true;
            }
            // If stats
            else
            {
                if (mainUIelement != null && isStatButton)
                {
                    locked = true;
                    SkillsTabManager.Instance.holdingButton = mainUIelement;
                    SkillsTabManager.Instance.holdingButton.ToggleTooltipStats(true);
                }
            }
        }
    }

    public void SelectEffect()
    {
        if (effectHeldTimer > 0 && !isEnabled)
        {
            isEnabled = true;
            Effect effect = GetComponent<Effect>();
            GetComponentInParent<UnitFunctionality>().ToggleTooltipEffect(true, effect.effectName);
        }
    }

    public void SelectUnit()
    {
        if (!GameManager.Instance.combatOver)
        {
            // If unit has held the unit down for x seconds, do not allow a selection from the release of that tap
            if (heldTimer >= GameManager.Instance.maxHeldTimeTooltip || locked)
                return;
        }

        // If player is in hero room view AND the unit linked with this button is NOT  the offered hero unit
        if (unit)
        {
            if (HeroRoomManager.Instance.playerInHeroRoomView && !unit.heroRoomUnit)
                return;
        }

        StartCoroutine(SelectUnitCo());
    }

    IEnumerator SelectUnitCo()
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
                    {
                        GameManager.Instance.targetUnit(unitFunctionality);
                        unitIsSelected = true;
                    }
                    else
                    {
                        // Check if active skill targets allies, and is player type, then allow it
                        if (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT && GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
                        {
                            unitIsSelected = true;
                            GameManager.Instance.targetUnit(unitFunctionality);
                        }

                    }
                }

                yield break;
            }
            else
            {
                // Do not ally selection of enemy units  behind the hero offering, it obstructs selecting new hero
                if (unitFunctionality.curUnitType == UnitFunctionality.UnitType.ENEMY && HeroRoomManager.Instance.playerInHeroRoomView)
                    yield break;
                else
                {
                    isHeldDownUnit = false;
                    StartCoroutine(HeldDownCooldown());
                    GetComponentInParent<UnitFunctionality>().ToggleTooltipStats(false);

                    unitIsSelected = true;
                    GameManager.Instance.targetUnit(unitFunctionality);
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("a");
        // If not pressing effect button, instead pressing a unit
        if (!isEffectButton)
        {
            if (GameManager.Instance.playerInCombat)
            {
                if (GetComponentInParent<UnitFunctionality>() && !GetComponentInParent<UnitFunctionality>().isDead)
                {
                    isHeldDownUnit = true;
                    //Debug.Log("Pointer Down");

                }
                else
                {
                    //Debug.Log("b");
                    if (isStatButton)
                    {
                        isHeldDownStat = true;
                        //Debug.Log("c");
                    }
                }
            }
            else
            {
                if (isStatButton)
                {
                    isHeldDownStat = true;
                    //Debug.Log("c");
                }
            }
        }
        else
        {
            if (GameManager.Instance.playerInCombat)
                isHeldDownEffect = true;
            //SelectEffect();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isEffectButton)
        {
            if (GetComponentInParent<UnitFunctionality>())
            {
                //Debug.Log("Pointer Up");
                isHeldDownUnit = false;
                heldTimer = 0;
                StartCoroutine(HeldDownCooldown());
                GetComponentInParent<UnitFunctionality>().ToggleTooltipStats(false);
            }
            // If skill page stat
            else
            {
                if (isStatButton)
                {
                    isHeldDownStat = false;
                    heldTimer = 0;
                    StartCoroutine(HeldDownCooldown());

                    if (SkillsTabManager.Instance.holdingButton != null)
                        SkillsTabManager.Instance.holdingButton.ToggleTooltipStats(false);
                }
            }
        }
        else
        {
            isHeldDownEffect = false;
            effectHeldTimer = 0;
            isEnabled = false;
            //Debug.Log("hiding effect tooltip");
            GetComponentInParent<UnitFunctionality>().ToggleTooltipEffect(false);
            StartCoroutine(HideEffectTooltipOvertime());
        }
    }

    IEnumerator HeldDownCooldown()
    {
        yield return new WaitForSeconds(.1f);

        locked = false;
    }

    public IEnumerator HideEffectTooltipOvertime(bool onlyEnemies = false)
    {
        if (GetComponentInParent<UnitFunctionality>())
        {
            if (!onlyEnemies)
            {
                yield return new WaitForSeconds(0);
                isHeldDownEffect = false;
                effectHeldTimer = 0;
                isEnabled = false;
                GetComponentInParent<UnitFunctionality>().ToggleTooltipEffect(false);
            }
            else
            {
                if (GetComponentInParent<UnitFunctionality>().curUnitType == UnitFunctionality.UnitType.ENEMY)
                {
                    isHeldDownEffect = false;
                    effectHeldTimer = 0;
                    isEnabled = false;
                    GetComponentInParent<UnitFunctionality>().ToggleTooltipEffect(false);
                }
            }
        }
    }
}
