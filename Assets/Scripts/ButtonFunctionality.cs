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

    public ShopItem shopItem;
    [SerializeField] private bool disabled;
    [SerializeField] private UIElement UIbutton;
    public UIElement itemParent;


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
    public UIElement selectBorder;
    public bool postBattleButtonPressed = false;

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

    #region Menu Overlay (Kill Run / Pause  Game)
    public void ToggleMenuOverlay()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        if (MenuOverlay.Instance.menuOverlayOpened)
        {
            MenuOverlay.Instance.ToggleMenuOverlay(false);
        }
        else
            MenuOverlay.Instance.ToggleMenuOverlay(true);
    }

    public void ToggleKillRunPrompt()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        MenuOverlay.Instance.ToggleKillRunPrompt(true);
    }

    public void DisableKillRunPrompt()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");
        MenuOverlay.Instance.ToggleKillRunPrompt(false);
    }

    public void KillRunResponse(bool killRun = false)
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");
        MenuOverlay.Instance.KillRunPrompt(killRun);
    }
    #endregion

    public void ButtonFallenFighterPromptYes()
    {
        MapManager.Instance.mapOverlay.ToggleFallenFighterPrompt(false);

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        ButtonEnterRoom(true);
    }

    public void ButtonFallenFighterPromptNo()
    {
        MapManager.Instance.mapOverlay.ToggleFallenFighterPrompt(false);

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");
    }

    public void ButtonHardRoomPromptYes()
    {
        MapManager.Instance.mapOverlay.ToggleHardRoomPrompt(false);

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        ButtonEnterRoom(true);
    }

    public void ButtonHardRoomPromptNo()
    {
        MapManager.Instance.mapOverlay.ToggleHardRoomPrompt(false);

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");
    }

    bool menuPlayButtonPressed;

    public void MenuSelectHero()
    {
        if (!menuPlayButtonPressed)
        {
            if (!buttonLocked)
            {
                menuPlayButtonPressed = true;

                GameManager.Instance.startingFighterChosen = true;

                // Button Click SFX
                AudioManager.Instance.Play("Button_Click");

                CharacterCarasel.Instance.SelectAlly(this);

                UpdateLog.Instance.ToggleUpdateLog(false);
                UpdateLog.Instance.ToggleUpdateLogbutton(false);

                StartCoroutine(MenuSelectHeroCo());
            }
        }
    }

    IEnumerator MenuSelectHeroCo()
    {
        yield return new WaitForSeconds(2.75f);

        menuPlayButtonPressed = false;
    }

    public void ButtonMenuToggleUpdateLog()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        if (UpdateLog.Instance.updateLogIsOpen)
            UpdateLog.Instance.ToggleUpdateLog(false);
        else
            UpdateLog.Instance.ToggleUpdateLog(true);
    }

    public IEnumerator MenuUnitSelection()
    {
        // Allow button selection
        buttonLocked = false;

        yield return new WaitForSeconds(2.25f);

        GameManager.Instance.transitionSprite.resetMap = true;

        GameManager.Instance.TriggerTransitionSequence();
    }

    public void MenuLeftArrowCarasel()
    {
        if (GameManager.Instance.startingFighterChosen)
            return;

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        CharacterCarasel.Instance.SpinCarasel(true);
    }

    public void MenuRightArrowCarasel()
    {
        if (GameManager.Instance.startingFighterChosen)
            return;

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

    bool enterRoomButtonPressed = false;

    public void ButtonEnterRoom(bool byPass = false)
    {
        if (!enterRoomButtonPressed)
        {
            enterRoomButtonPressed = true;

            if (!byPass)
            {
                // Button Click SFX
                AudioManager.Instance.Play("Button_Click");

                if (RoomManager.Instance.GetActiveRoom().GetRoomType() != RoomMapIcon.RoomType.SHOP 
                && RoomManager.Instance.GetActiveRoom().GetRoomType() != RoomMapIcon.RoomType.STARTING)
                {
                    if (GameManager.Instance.fallenHeroes.Count > 0)
                    {
                        MapManager.Instance.mapOverlay.ToggleFallenFighterPrompt(true);

                        enterRoomButtonPressed = false;
                        return;
                    }
                } 
                if (RoomManager.Instance.GetActiveRoom().GetRoomType() == RoomMapIcon.RoomType.ITEM || RoomManager.Instance.GetActiveRoom().GetRoomType() == RoomMapIcon.RoomType.HERO
                    || RoomManager.Instance.GetActiveRoom().GetRoomType() == RoomMapIcon.RoomType.BOSS)
                {
                    if (MapManager.Instance.CheckToEnableHardRoomPrompt())
                    {
                        if (RoomManager.Instance.GetFloorCount() >= RoomManager.Instance.highestFloorCountRun)
                        {
                            MapManager.Instance.mapOverlay.ToggleHardRoomPrompt(true);

                            enterRoomButtonPressed = false;
                            return;
                        }
                    }
                }
            }

            // Map Close SFX
            AudioManager.Instance.Play("Map_Close");

            if (RoomManager.Instance.GetActiveRoom().curRoomType != RoomMapIcon.RoomType.STARTING && !RoomManager.Instance.GetActiveRoom().isCompleted)
                GameManager.Instance.TriggerTransitionSequence();

            StartCoroutine(ButtonEnterRoomCo());
        }

    }

    IEnumerator ButtonEnterRoomCo()
    {
        if (RoomManager.Instance.GetFloorCount() != 0 && RoomManager.Instance.GetActiveRoom().curRoomType == RoomMapIcon.RoomType.STARTING)
        {
            MapManager.Instance.LoadPreviousFloor();
            enterRoomButtonPressed = false;

            yield break;
        }
        else if (RoomManager.Instance.GetActiveRoom().curRoomType == RoomMapIcon.RoomType.BOSS && RoomManager.Instance.GetActiveRoom().isCompleted)
        {
            MapManager.Instance.LoadFutureSavedFloor();
            enterRoomButtonPressed = false;

            yield break;
        }

        yield return new WaitForSeconds(1f);

        //GameManager.Instance.UpdateAllyVisibility(true);

        // Disable map UI
        GameManager.Instance.ToggleMap(false);

        // Enable combat UI
        //GameManager.Instance.Setup();

        // Face left for combat

        /*
        // Make all allies face left for combat
        for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
        {
            GameManager.Instance.activeRoomHeroes[i].UpdateFacingDirection(false);
        }
        */

        GameManager.Instance.UpdateAllysPositionCombat();

        enterRoomButtonPressed = false;
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

    public void ButtonSelectCombatSlot(bool flag = false)
    {
        if (RoomManager.Instance.GetActiveRoom().curRoomType == RoomMapIcon.RoomType.HERO && HeroRoomManager.Instance.playerInHeroRoomView)
            return;

        if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY &&
            !flag)
        {
            return;
        }

        if (!GameManager.Instance.GetAllowSelection())
            return;


        CombatSlot combatSlot = GetComponentInParent<CombatSlot>();

        if (combatSlot)
        {
            CombatGridManager.Instance.ResetAllowedSlotAnims();

            if (!CombatGridManager.Instance.isCombatMode)
            {
                if (CombatGridManager.Instance.GetIsMovementAllowed())
                {
                    // Button Click SFX
                    if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
                        AudioManager.Instance.Play("Button_Click");

                    if (combatSlot.GetAllowed())
                    {
                        if (combatSlot.GetSelected())
                            combatSlot.ToggleSlotSelected(false);
                        else
                            combatSlot.ToggleSlotSelected(true);

                        CombatGridManager.Instance.UpdateSelectedCombatSlotMove(combatSlot);
                        CombatGridManager.Instance.MoveUnitToNewSlot(GameManager.Instance.GetActiveUnitFunctionality());
                    }
                    else
                    {
                        AudioManager.Instance.Play("SFX_ShopBuyFail");
                    }
                }
                else
                {
                    AudioManager.Instance.Play("SFX_ShopBuyFail");
                }
            }
            // Combat Mode
            else
            {
                
                

                // Button Click SFX
                if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
                    AudioManager.Instance.Play("Button_Click");

                // If this combat slot is further x axis then the active unit, face the active unit right, otherwise left
                //if (combatSlot.GetLocalIndexFromSlot(GameManager.Instance.GetActiveUnitFunctionality().GetActiveCombatSlot()).x > 0)
                //    GameManager.Instance.GetActiveUnitFunctionality().UpdateUnitLookDirection(false);
               // else if (combatSlot.GetLocalIndexFromSlot(GameManager.Instance.GetActiveUnitFunctionality().GetActiveCombatSlot()).x < 0)
                //    GameManager.Instance.GetActiveUnitFunctionality().UpdateUnitLookDirection(true);

                //if (combatSlot.GetAllowed())
                //{
                if (combatSlot.combatSelected)
                {
                    // Launch attack on all combat selected slots

                    UnitFunctionality selectedUnit = null;

                    if (combatSlot.GetLinkedUnit())
                    {
                        if (GameManager.Instance.isSkillsMode)
                        {
                            if (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE &&
                                GameManager.Instance.GetActiveSkill().curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.ENEMIES &&
                                GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
                            {
                                if (combatSlot.GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER)
                                    return;
                            }
                            else if (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE &&
                                    GameManager.Instance.GetActiveSkill().curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.PLAYERS &&
                                    GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY)
                            {
                                if (combatSlot.GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY)
                                    return;
                            }
                            else if (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT &&
                                    GameManager.Instance.GetActiveSkill().curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.PLAYERS &&
                                    GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
                            {
                                if (combatSlot.GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY)
                                    return;
                            }
                            else if (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT &&
                                    GameManager.Instance.GetActiveSkill().curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.PLAYERS &&
                                    GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY)
                            {
                                if (combatSlot.GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER)
                                    return;
                            }
                            else if (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT &&
                                GameManager.Instance.GetActiveSkill().curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.ENEMIES &&
                                GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
                            {
                                if (combatSlot.GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER)
                                    return;
                            }
                        }
                    }

                    if (combatSlot.GetLinkedUnit())
                        selectedUnit = combatSlot.GetLinkedUnit();
                    else if (combatSlot.GetFallenUnits().Count > 0 &&
                            GameManager.Instance.GetActiveSkill().curskillSelectionAliveType == SkillData.SkillSelectionAliveType.DEAD)
                    {
                        if (combatSlot.GetFallenUnits().Count > 0)
                            selectedUnit = combatSlot.GetFallenUnits()[0];
                    }

                    if (!GameManager.Instance.GetActiveSkill().attackAllSelected)
                    {
                        if (selectedUnit || combatSlot.GetFallenUnits().Count > 0 && 
                            GameManager.Instance.GetActiveSkill().curskillSelectionAliveType == SkillData.SkillSelectionAliveType.DEAD)
                        {
                            if (!selectedUnit.IsSelected())
                            {
                                selectedUnit.ToggleSelected(true);

                                CombatGridManager.Instance.done = true;

                                GameManager.Instance.targetUnit(selectedUnit);


                            }
                            else
                            {
                                if (selectedUnit.GetActiveCombatSlot().combatSelected)
                                {
                                    // If selected skill is out of range, choose another skill
                                    if (selectedUnit.GetRangeFromUnit(GameManager.Instance.GetActiveUnitFunctionality()) > GameManager.Instance.GetActiveSkill().curSkillRange
                                        && !GameManager.Instance.GetActiveSkill().isSelfCast)
                                    {
                                        if (combatSlot.GetLinkedUnit())
                                        {
                                            GameManager.Instance.unitsSelected.Add(combatSlot.GetLinkedUnit());
                                            combatSlot.GetLinkedUnit().ToggleSelected(true);
                                        }
                                    }                                
                                    else
                                    {
                                        GameManager.Instance.unitsSelected.Add(combatSlot.GetLinkedUnit());
                                        selectedUnit.ToggleSelected(true);

                                        CombatGridManager.Instance.done = true;

                                        GameManager.Instance.targetUnit(selectedUnit, true);
                                    }
                                }

                            }
                        }
                        else
                        {
                            
                            CombatGridManager.Instance.RemoveAllCombatSelectedCombatSlots();
                            CombatGridManager.Instance.ToggleAllCombatSlotOutlines();
                            CombatGridManager.Instance.UpdateUnitAttackHitArea(GameManager.Instance.GetActiveUnitFunctionality(), combatSlot);
                            //CombatGridManager.Instance.UpdateUnitAttackHitArea(GameManager.Instance.GetActiveUnitFunctionality(), combatSlot);
                            //GameManager.Instance.GetActiveUnitFunctionality().hasAttacked = false;

                            //StartCoroutine(GameManager.Instance.GetActiveUnitFunctionality().StartUnitTurn(false));
                        }
                    }
                    else
                    {
                        if (selectedUnit)
                        {
                            // add all units ontop of a combat selected slot to units selected.
                            //combatSlot.GetLinkedUnit().ToggleSelected(true);

                            for (int i = 0; i < GameManager.Instance.activeRoomAllUnitFunctionalitys.Count; i++)
                            {
                                if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].GetActiveCombatSlot().combatSelected)
                                {
                                    GameManager.Instance.activeRoomAllUnitFunctionalitys[i].ToggleSelected(true);

                                    CombatGridManager.Instance.done = true;
                                    GameManager.Instance.targetUnit(GameManager.Instance.activeRoomAllUnitFunctionalitys[i]);
                                }
                            }

                            if (selectedUnit)
                            {
                                if (selectedUnit.IsSelected())
                                    GameManager.Instance.targetUnit(selectedUnit, true);
                            }
                        }
                        else
                        {
                            CombatGridManager.Instance.RemoveAllCombatSelectedCombatSlots();
                            CombatGridManager.Instance.ToggleAllCombatSlotOutlines();
                            CombatGridManager.Instance.UpdateUnitAttackHitArea(GameManager.Instance.GetActiveUnitFunctionality(), combatSlot);
                            //StartCoroutine(GameManager.Instance.GetActiveUnitFunctionality().StartUnitTurn(false));
                        }
                    }
                }
                else
                {
                    // If attack area of skill is 1x1, replace other combat selected slot with this one
                    if (GameManager.Instance.GetActiveSkill().skillAreaHitCount == 1)
                    {
                        if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY)
                        {
                            if (combatSlot.GetLinkedUnit())
                            {
                                //if ()
                                GameManager.Instance.unitsSelected.Add(combatSlot.GetLinkedUnit());
                                combatSlot.GetLinkedUnit().ToggleSelected(true);
                                //GameManager.Instance.targetUnit(combatSlot.GetLinkedUnit(), true);

                                if (combatSlot.GetLinkedUnit().isSelected)
                                    GameManager.Instance.targetUnit(combatSlot.GetLinkedUnit(), true);
                                else
                                    combatSlot.combatSelected = true;
                            }
                            else if (!combatSlot.GetLinkedUnit())
                            {
                                GameManager.Instance.GetActiveUnitFunctionality().hasAttacked = false;
                            }
                        }
                        else
                        {
                            CombatGridManager.Instance.RemoveAllCombatSelectedCombatSlots();
                            CombatGridManager.Instance.ToggleAllCombatSlotOutlines();
                            CombatGridManager.Instance.UpdateUnitAttackHitArea(GameManager.Instance.GetActiveUnitFunctionality(), combatSlot);
                        }
                    }
                    else
                    {
                        if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY)
                        {
                            if (combatSlot.GetLinkedUnit())
                            {
                                GameManager.Instance.unitsSelected.Add(combatSlot.GetLinkedUnit());
                                combatSlot.GetLinkedUnit().ToggleSelected(true);
                                GameManager.Instance.targetUnit(combatSlot.GetLinkedUnit(), true);

                                //if (combatSlot.GetLinkedUnit().isSelected)
                                  //  GameManager.Instance.targetUnit(combatSlot.GetLinkedUnit(), true);
                            }
                            else if (!combatSlot.GetLinkedUnit())
                            {
                                GameManager.Instance.GetActiveUnitFunctionality().hasAttacked = false;    
                            }
                        }
                    }
                }
            }
        }
    }
    public void ButtonOpenMap()
    {
        //Debug.Log("!");
        // If owned gear is opened, do not allow map button to be selected
        //if (OwnedLootInven.Instance.GetOwnedLootOpened())
        //   return;

        OwnedLootInven.Instance.ToggleOwnedGearDisplay(false);

        TeamGearManager.Instance.playerInGearTab = false;

        //GameManager.Instance.activeRoomHeroesBase.Reverse();
        /*
        if (TeamGearManager.Instance.playerInGearTab || TeamItemsManager.Instance.playerInItemTab || SkillsTabManager.Instance.playerInSkillTab)
            GameManager.Instance.SetActiveHeroToBase();
        else
            GameManager.Instance.SetHeroFormation();
        //
        */
        SettingsManager.Instance.ToggleSettingsButton(true);

        ShopManager.Instance.ClearFallenHeroesVisuals();

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

        TeamGearManager.Instance.ToggleTeamGear(false);
        TeamItemsManager.Instance.ToggleTeamItems(false);

        // Toggle Map back on
        GameManager.Instance.ToggleMap(true, false, false, false);

        ShopManager.Instance.CloseShop();

        //GameManager.Instance.activeRoomAllUnitFunctionalitys = GameManager.Instance.oldActiveRoomAllUnitFunctionalitys;

        GameManager.Instance.SkillsTabChangeAlly(false, false, false);



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

        if (!ShopManager.Instance.playerInShopRoom)
        {
            // Put selection on seleted item
            if (itemParent != null)
            {
                //ItemRewardManager.Instance.selectedItemName = shopItem.GetShopItemName();
                ItemRewardManager.Instance.selectedItemName = itemParent.GetItemName();
                ItemRewardManager.Instance.selectedItem = ItemRewardManager.Instance.GetItem(ItemRewardManager.Instance.selectedItemName);

                itemParent.ToggleSelected(true, true);

                ItemRewardManager.Instance.ToggleItemSelectedRaceIcon(true);  

                if (ItemRewardManager.Instance.selectedItem.curRace == ItemPiece.RaceSpecific.HUMAN)
                {
                    ItemRewardManager.Instance.UpdateItemSelectedRaceIcon("HUMAN");
                }
                else if (ItemRewardManager.Instance.selectedItem.curRace == ItemPiece.RaceSpecific.BEAST)
                {
                    ItemRewardManager.Instance.UpdateItemSelectedRaceIcon("BEAST");
                }
                else if (ItemRewardManager.Instance.selectedItem.curRace == ItemPiece.RaceSpecific.ETHEREAL)
                {
                    ItemRewardManager.Instance.UpdateItemSelectedRaceIcon("ETHEREAL");
                }      
                else if (ItemRewardManager.Instance.selectedItem.curRace == ItemPiece.RaceSpecific.ALL)
                    ItemRewardManager.Instance.ToggleItemSelectedRaceIcon(false);                       
            }

            ItemRewardManager.Instance.UpdateItemDescription(true);
            

            ItemRewardManager.Instance.ToggleConfirmItemButton(true);
        }
        else
        {
            // Put selection on seleted item
            if (shopItem != null)
            {
                //ItemRewardManager.Instance.selectedItemName = shopItem.GetShopItemName();
                ItemRewardManager.Instance.selectedItemName = shopItem.GetShopItemName();
                ItemRewardManager.Instance.selectedItem = ItemRewardManager.Instance.GetItem(ItemRewardManager.Instance.selectedItemName);

                //itemParent.ToggleSelected(true, true);
            }

            //ItemRewardManager.Instance.UpdateItemDescription(true);

            //ItemRewardManager.Instance.ToggleConfirmItemButton(true);
        }
    }

    public void ConfirmItem()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        if (RoomManager.Instance.GetActiveRoom().GetRoomType() == RoomMapIcon.RoomType.HERO)
        {
            if (RoomManager.Instance.GetActiveRoom().curRoomType == RoomMapIcon.RoomType.HERO)
            {
                GameManager.Instance.EnsureHeroIsDead();
                StartCoroutine(GameManager.Instance.HeroRetrievalScene(false));

                StartCoroutine(ConfrimItemCo());
            }
        }

        else
            StartCoroutine(GameManager.Instance.SetupPostBattleUI(GameManager.Instance.playerWon));
    }

    IEnumerator ConfrimItemCo()
    {
        yield return new WaitForSeconds(.35f);

        HeroRoomManager.Instance.SpawnHeroGameManager();
    }

    public void ButtonSlotDetails(bool openedOwnedSlots = true)
    {
        // If button selected is unlocked, open owned stuff
        if (mainButton != null)
        {
            if (mainButton.isLocked)
                return;
        }

        AudioManager.Instance.Play("Button_Click");

        if (SkillsTabManager.Instance.playerInSkillTab)
        {
            if (!OwnedLootInven.Instance.ownedLootOpened)
            {
                if (openedOwnedSlots && SkillsTabManager.Instance.activeSkillBase != null)
                    OwnedLootInven.Instance.ToggleOwnedGearDisplay(true, "Ally Skills");
            }
        }
        else if (TeamGearManager.Instance.playerInGearTab)
        {
            if (!OwnedLootInven.Instance.ownedLootOpened)
            {
                if (openedOwnedSlots)
                {
                    TeamGearManager.Instance.ResetAllGearSelections();
                    TeamGearManager.Instance.UpdateSelectedBaseGearSlot(slot);
                    TeamGearManager.Instance.UpdateSelectedGearSlot(slot);
                    slot.ToggleSlotSelection(true);
                    OwnedLootInven.Instance.ToggleOwnedGearDisplay(true, "Owned Gear");
                }
            }
        }

        else if (TeamItemsManager.Instance.playerInItemTab)
        {


            if (!OwnedLootInven.Instance.ownedLootOpened)
            {
                if (openedOwnedSlots)
                {
                    TeamItemsManager.Instance.ResetAllItemSelections();
                    TeamItemsManager.Instance.UpdateSelectedBaseItemSlot(slot);
                    TeamItemsManager.Instance.UpdateSelectedItemSlot(slot);
                    slot.ToggleSlotSelection(true);
                    OwnedLootInven.Instance.ToggleOwnedGearDisplay(true, "Owned Items");
                }
            }
        }
    }

    // Owned slot select main
    public void ButtonSelectGear()
    {
        if (TeamGearManager.Instance.playerInGearTab)
        {
            // If owned gear tab is opened, do not allow selecting a new gear
            if (slot != null)
            {
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

            if (slot.isEmpty)
            {
                if (OwnedLootInven.Instance.ownedLootOpened)
                {
                    OwnedLootInven.Instance.ResetOwnedSlotEquipButton();
                    return;
                }
            }

            OwnedLootInven.Instance.ClearOwnedItemsSlotsSelection();
            if (slot != null)
            {
                if (!OwnedLootInven.Instance.ownedLootOpened)
                {
                    //if (slot.isEmpty)
                    TeamGearManager.Instance.GearSelection(slot, false);

                    if (slot.isEmpty)
                        ButtonSlotDetails();
                }
                else
                {
                    if (!slot.isEmpty && !slot.baseSlot)
                    {
                        // Button Click SFX
                        AudioManager.Instance.Play("Button_Click");
                        TeamGearManager.Instance.GearSelection(slot, false);
                    }
                }
            }
        }
        else if (TeamItemsManager.Instance.playerInItemTab)
        {
            // If owned gear tab is opened, do not allow selecting a new gear
            if (slot != null)
            {
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

            if (slot.isEmpty)
            {
                if (OwnedLootInven.Instance.ownedLootOpened)
                {
                    OwnedLootInven.Instance.ResetOwnedSlotEquipButton();
                    return;
                }
            }

            OwnedLootInven.Instance.ClearOwnedItemsSlotsSelection();
            if (slot != null)
            {
                if (!OwnedLootInven.Instance.ownedLootOpened)
                {
                    //if (slot.isEmpty)
                    TeamItemsManager.Instance.ItemSelection(slot);

                    if (slot.isEmpty)
                        ButtonSlotDetails();
                }

                else
                {
                    if (!slot.isEmpty && !slot.baseSlot)
                        TeamItemsManager.Instance.ItemSelection(slot);
                }
            }
        }
        else if (SkillsTabManager.Instance.playerInSkillTab)
        {
            // If owned gear tab is opened, do not allow selecting a new gear
            if (slot != null)
            {
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
            {
                if (!slot.isEmpty)
                    SkillsTabManager.Instance.SkillSelection(slot, false);
            }
        }
    }

    // Equipping gear with Gear Button
    public void EquipGear()
    {
        // Button Click SFX
        //AudioManager.Instance.Play("Button_Click");

        if (slot != null)
        {
            if (TeamGearManager.Instance.playerInGearTab)
            {
                AudioManager.Instance.Play("Button_Click");

                TeamGearManager.Instance.UnequipGear();

                TeamGearManager.Instance.GearSelection(slot, true);
                SkillsTabManager.Instance.UpdateSkillStatDetails();
            }
            else if (TeamItemsManager.Instance.playerInItemTab)
            {
                if (slot.coverOn)
                {
                    AudioManager.Instance.Play("SFX_ShopBuyFail");
                    return;
                }

                AudioManager.Instance.Play("Button_Click");

                TeamItemsManager.Instance.UnequipItem();

                TeamItemsManager.Instance.ItemSelection(slot, true);
                SkillsTabManager.Instance.UpdateSkillStatDetails();
            }
            else if (SkillsTabManager.Instance.playerInSkillTab)
            {
                //Debug.Log("ccc");
                SkillsTabManager.Instance.UpdateSelectedSkillBase(SkillsTabManager.Instance.selectedSkillBase.buttonCG.GetComponent<ButtonFunctionality>());

                SkillsTabManager.Instance.SkillSelection(slot, true);

                //ButtonSlotDetails(false);

                //SkillsTabManager.Instance.UpdateSkillStatDetails();
                SkillsTabManager.Instance.selectedSkillBase.buttonCG.GetComponent<ButtonFunctionality>().SkillSlotSelection();

                SkillsTabManager.Instance.UpdateSkillStatDetails();
            }
        }
    }

    public void SelectBaseItem()
    {
        if (OwnedLootInven.Instance.ownedLootOpened)
            return;

        OwnedLootInven.Instance.ClearOwnedItemsSlotsSelection();

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        Item item = slot.gameObject.GetComponent<Item>();

        if (slot.isEmpty)
            TeamItemsManager.Instance.ItemSelection(slot, false);
        else
            TeamItemsManager.Instance.ItemSelection(slot, true);

        // Display inven
        if (slot.isEmpty)
        {
            OwnedLootInven.Instance.ToggleOwnedGearDisplay(true, "Owned Items");
            //OwnedGearInven.Instance.ToggleOwnedGearEquipButton(true);
        }
        else
        {
            TeamItemsManager.Instance.itemsNameText.UpdateContentText(TeamItemsManager.Instance.GetSelectedItemSlot().linkedItemPiece.itemName);
            /*
            if (TeamItemsManager.Instance.GetSelectedBaseItemSlot())
            {
                if (TeamItemsManager.Instance.GetSelectedBaseItemSlot().linkedItemPiece)                    
            }
            */
        }
    }

    public void HideOwnedGear()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        if (TeamGearManager.Instance.playerInGearTab)
            TeamGearManager.Instance.ToggleAllSlotsClickable(false, true, false, false);

        OwnedLootInven.Instance.ToggleOwnedGearDisplay(false, "", true);

        SkillsTabManager.Instance.UpdateSelectedOwnedSlot(null);
        SkillsTabManager.Instance.UpdateSkillStatDetailsSpecific(SkillsTabManager.Instance.GetActiveSkillBase());

        if (TeamGearManager.Instance.playerInGearTab)
        {
            TeamGearManager.Instance.UpdateGearNameText("");
            TeamGearManager.Instance.ClearAllGearStats();
        }
        else if (TeamItemsManager.Instance.playerInItemTab)
        {
            TeamItemsManager.Instance.UpdateItemNameText("");
            TeamItemsManager.Instance.UpdateItemDesc("");
            TeamItemsManager.Instance.ToggleFighterRaceIcon(false);
        }
    }

    public void GearUnEquip()
    {
        if (OwnedLootInven.Instance.ownedLootOpened)
            return;

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        if (TeamGearManager.Instance.playerInGearTab)
            TeamGearManager.Instance.UnequipGear();
        else if (TeamItemsManager.Instance.playerInItemTab)
            TeamItemsManager.Instance.UnequipItem();
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

        //Debug.Log("bbb");
        SkillsTabManager.Instance.UpdateSelectedSkillBase(this);

        if (slot != null)
        {
            SkillsTabManager.Instance.SkillSelection(slot, true);
        }


        if (!OwnedLootInven.Instance.ownedLootOpened)
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
        //ShopManager.Instance.FillShopItems(true, false);



        // refresh item price
    }

    // Select Shop Item
    public void SelectShopItem()
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
                shopItem.SelectShopItem(true);
                return;
            }
        }
    }

    public void PurchaseTryShopItem()
    {
        ShopManager.Instance.GetSelectedShopItem().PurchaseShopItem();
    }

    // Gear tab from map
    public void ButtonGearTabFromMap()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        CombatGridManager.Instance.ToggleCombatGrid(true);

        OwnedLootInven.Instance.DisableCoverForOwnedSlots();

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

        GameManager.Instance.ToggleMainSlotVisibility(false);

        SettingsManager.Instance.ToggleSettingsButton(false);

        // Enable To Map Button
        //GameManager.Instance.toMapButton.UpdateAlpha(1);

        //TeamSetup.Instance.ToggleToMapButton(true);
    }

    // Gear tab to Skill tab
    public void ButtonSkillsTabFromGearTab()
    {
        //Debug.Log("@");
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        //GameManager.Instance.SetHeroFormation();

        // Disable Owned Gear Display
        OwnedLootInven.Instance.ToggleOwnedGearDisplay(false);

        TeamItemsManager.Instance.playerInItemTab = false;
        TeamGearManager.Instance.playerInGearTab = false;
        SkillsTabManager.Instance.playerInSkillTab = true;

        //TeamSetup.Instance.ResetStatPageCount();

        //SkillsTabManager.Instance.
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

        GameManager.Instance.ToggleMainSlotVisibility(false);

        //TeamSetup.Instance.ToggleToMapButton(true);

        // Enable To Map Button
        GameManager.Instance.toMapButton.UpdateAlpha(1);
        
    }

    // Skill tab to Item Tab
    public void ButtonGearTab()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        TeamItemsManager.Instance.ToggleFighterRaceIcon(false);

        OwnedLootInven.Instance.ToggleOwnedGearDisplay(false);

        //GameManager.Instance.SetActiveHeroToBase();

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
        //if (GameManager.Instance.activeTeam.Count != 1)
        GameManager.Instance.SkillsTabChangeAlly(true, false, true, true);

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");
    }

    public void PostBattleToMapButton()
    {
        if (!postBattleButtonPressed)
        {
            postBattleButtonPressed = true;

            // Button Click SFX
            AudioManager.Instance.Play("Button_Click");

            if (RoomManager.Instance.GetActiveRoom().curRoomType == RoomMapIcon.RoomType.BOSS)
                GameManager.Instance.transitionSprite.incMapFloor = true;
            else
                GameManager.Instance.transitionSprite.goToMap = true;

            GameManager.Instance.TriggerTransitionSequence();

            StartCoroutine(PostBattleToMapButtonCo());
        }
    }

    public IEnumerator PostBattleToMapButtonCo()
    {
        yield return new WaitForSeconds(1f);

        // Map open SFX
        AudioManager.Instance.Play("Map_Open");

        // Disable post battle to map button for next post battle scene
        StartCoroutine(PostBattle.Instance.ToggleButtonPostBattleMap(false));

        // Toggle Map back on
        GameManager.Instance.ToggleMap(true, false, false, false);

        // Disable post battle UI
        GameManager.Instance.postBattleUI.TogglePostBattleUI(false);

        GameManager.Instance.ResetRoom(true);

        //Debug.Log("playerWon = " + GameManager.Instance.playerWon);
        GameManager.Instance.map.ClearRoom(true, false, GameManager.Instance.playerWon);


        // Update unit sorting
        GameManager.Instance.UpdateAllUnitsSorting(GameManager.Instance.unitTabSortingLevel);

        if (GameManager.Instance.activeRoomHeroes.Count > 0)
        {
            if (GameManager.Instance.GetActiveAlly())
            {
                GameManager.Instance.SkillsTabChangeAlly(true);
                GameManager.Instance.SkillsTabChangeAlly(false, true);
            }
        }

        TeamItemsManager.Instance.ReloadItemUses();

        for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
        {
            GameManager.Instance.activeRoomHeroes[i].ToggleUnitDisplay(false);
        }

        //CombatGridManager.Instance.TogleCombatGrid(false);

        postBattleButtonPressed = false;
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

        //StartCoroutine(AttackButtonCont());
    }

    /*
    IEnumerator AttackButtonCont()
    {
        yield return new WaitForSeconds(GameManager.Instance.skillAlertAppearTime/2);

        if (!GameManager.Instance.GetActiveUnitFunctionality().mindControlled)
            GameManager.Instance.SetupPlayerWeaponUI();
        else
            WeaponManager.Instance.SetEnemyWeapon(GameManager.Instance.GetActiveUnitFunctionality(), true);

    }
    */

    public void IncHero1HP()
    {
        GameManager.Instance.activeRoomHeroes[0].UpdateUnitCurHealth((int)GameManager.Instance.activeRoomHeroes[0].GetUnitMaxHealth(), true, true);
    }

    public void IncHero2HP()
    {
        GameManager.Instance.activeRoomHeroes[1].UpdateUnitCurHealth((int)GameManager.Instance.activeRoomHeroes[1].GetUnitMaxHealth(), true, true);
    }

    public void IncHero3HP()
    {
        GameManager.Instance.activeRoomHeroes[2].UpdateUnitCurHealth((int)GameManager.Instance.activeRoomHeroes[2].GetUnitMaxHealth(), true, true);
    }

    public void DecHero1HP()
    {
        GameManager.Instance.activeRoomHeroes[0].UpdateUnitCurHealth(1, true, true);
    }

    public void DecHero2HP()
    {
        GameManager.Instance.activeRoomHeroes[1].UpdateUnitCurHealth(1, true, true);
    }

    public void DecHero3HP()
    {
        GameManager.Instance.activeRoomHeroes[2].UpdateUnitCurHealth(1, true, true);
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

    public void AddEnemyHPButton()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        GameManager.Instance.IncreaseAllEnemyHealth();
    }
    public void AddAllyHPButton()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        GameManager.Instance.IncreaseAllPlayerHealth();
    }

    public void EndTurnButton()
    {
        if (disabled)
            return;

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        GameManager.Instance.ToggleEndTurnButton(false);

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

        //if (unitFunctionality)
            //Debug.Log("disabling 2 " + toggle + " " + unitFunctionality.GetUnitName());

        if (UIbutton == null)
            return;

        UIbutton.gameObject.GetComponent<CanvasGroup>().interactable = toggle;
        UIbutton.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = toggle;
        gameObject.GetComponent<Button>().interactable = toggle;
    }

    bool GetIfSelected()
    {
        return selected;
    }

    public void FallenHeroPromptYes()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        int cost = 0;

        for (int i = 0; i < ShopManager.Instance.fallenHeroButtons.Count; i++)
        {
            if (ShopManager.Instance.fallenHeroButtons[i].gameObject.GetComponentInParent<MenuUnitDisplay>())
            {
                cost = ShopManager.Instance.fallenHeroButtons[i].gameObject.GetComponentInParent<MenuUnitDisplay>().cost;
                
                //Debug.Log("cost = " + cost);

                if (cost <= ShopManager.Instance.GetPlayerGold())
                {
                    ShopManager.Instance.UpdatePlayerGold(-ShopManager.Instance.fallenHeroButtons[i].gameObject.GetComponentInParent<MenuUnitDisplay>().cost);

                    ShopManager.Instance.ToggleFallenHeroPrompt(false);

                    ShopManager.Instance.ToggleAllFallenHeroSelection(false);

                    ShopManager.Instance.ReviveFallenHero(ShopManager.Instance.selectedFallenUnitName);
                }
                else
                {
                    AudioManager.Instance.Play("SFX_ShopBuyFail");
                }
            }
        }
    }

    public void ButtonInventoryAdd()
    {
        GameManager.Instance.SpawnItem();
    }
    public void FallenHeroPromptNo()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        ShopManager.Instance.ToggleFallenHeroPrompt(false);

        ShopManager.Instance.ToggleAllFallenHeroSelection(false);
    }


    public void HeroRoomPromptYes()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        HeroRoomManager.Instance.TogglePrompt(false, false, true);
    }

    public void HeroRoomPromptNo()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        HeroRoomManager.Instance.TogglePrompt(false, true, false);
    }

    public void ButtonCombatAttackTab()
    {
        // Button Click SFX
        if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            AudioManager.Instance.Play("Button_Click");

        CombatGridManager.Instance.isCombatMode = true;

        GameManager.Instance.ResetSelectedUnits();

        GetComponent<UIElement>().AnimateUI(false);

        if (!GameManager.Instance.GetActiveUnitFunctionality().hasAttacked)
            GameManager.Instance.isSkillsMode = true;

        CombatGridManager.Instance.UpdateAttackMovementMode(false, true, true);

        if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
        {
            CombatGridManager.Instance.ToggleTabButtons("Attack");
        }
    }
    public void ButtonCombatMovementTab()
    {
        // Button Click SFX
        if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            AudioManager.Instance.Play("Button_Click");

        CombatGridManager.Instance.isCombatMode = false;

        GameManager.Instance.ResetSelectedUnits();

        GetComponent<UIElement>().AnimateUI(false);

        CombatGridManager.Instance.UpdateAttackMovementMode(true, false, false);

        if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
        {
            CombatGridManager.Instance.ToggleTabButtons("Movement");
        }
    }

    public void ButtonCombatItemTab()
    {
        if (!GameManager.Instance.GetAllowSelection())
            return;

        // Button Click SFX
        if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            AudioManager.Instance.Play("Button_Click");

        GameManager.Instance.isSkillsMode = false;

        GameManager.Instance.ResetSelectedUnits();

        if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            GameManager.Instance.UpdatePlayerAbilityUI(GameManager.Instance.isSkillsMode);

        GameManager.Instance.UpdateDetailsBanner();

        if (!GameManager.Instance.isSkillsMode)
        {
            GameManager.Instance.fighterMainSlot1.ToggleSelectImage(false);
            GameManager.Instance.fighterMainSlot2.ToggleSelectImage(false);
            GameManager.Instance.fighterMainSlot3.ToggleSelectImage(false);
            GameManager.Instance.fighterMainSlot4.ToggleSelectImage(false);

            GameManager.Instance.ResetActiveItem();

            if (GameManager.Instance.GetActiveItem())
            {
                GameManager.Instance.UpdateMainIconDetails(null, GameManager.Instance.GetActiveItem());

                if (GameManager.Instance.GetActiveItem().itemName == GameManager.Instance.fighterMainSlot1.GetItemName())
                {
                    GameManager.Instance.fighterMainSlot1.ToggleSelectImage(true);
                }
                else if (GameManager.Instance.GetActiveItem().itemName == GameManager.Instance.fighterMainSlot2.GetItemName())
                {
                    GameManager.Instance.fighterMainSlot1.ToggleSelectImage(true);
                }
                else if (GameManager.Instance.GetActiveItem().itemName == GameManager.Instance.fighterMainSlot3.GetItemName())
                {
                    GameManager.Instance.fighterMainSlot1.ToggleSelectImage(true);
                }
            }
            else
            {
                if (GameManager.Instance.GetActiveItemSlot())
                {
                    if (GameManager.Instance.GetActiveItemSlot().GetCalculatedItemsUsesRemaining2() > 0)
                    {
                        GameManager.Instance.UpdateMainIconDetails(null, GameManager.Instance.GetActiveUnitFunctionality().GetBaseSelectedItem());
                        // Select main item slot
                        GameManager.Instance.EnableFirstMainSlotSelection(false);
                    }
                }

                if (GameManager.Instance.GetActiveItem() == null)
                {
                    CombatGridManager.Instance.ToggleAllCombatSlotOutlines();
                    CombatGridManager.Instance.UnselectAllSelectedCombatSlots();
                    CombatGridManager.Instance.ToggleTabButtons("Items");
                    GameManager.Instance.UpdateMainIconDetails(null, null);
                    OverlayUI.Instance.UpdateItemUI("", "", 0, 0, Vector2.zero, TeamItemsManager.Instance.clearSlotSprite);
                    return;
                }

                if (GameManager.Instance.GetActiveUnitFunctionality().GetBaseSelectedItem())
                {
                    if (GameManager.Instance.GetActiveUnitFunctionality().GetBaseSelectedItem().itemName == GameManager.Instance.fighterMainSlot1.GetItemName())
                    {
                        GameManager.Instance.fighterMainSlot1.ToggleSelectImage(true);
                    }
                    else if (GameManager.Instance.GetActiveUnitFunctionality().GetBaseSelectedItem().itemName == GameManager.Instance.fighterMainSlot2.GetItemName())
                    {
                        GameManager.Instance.fighterMainSlot2.ToggleSelectImage(true);
                    }
                    else if (GameManager.Instance.GetActiveUnitFunctionality().GetBaseSelectedItem().itemName == GameManager.Instance.fighterMainSlot3.GetItemName())
                    {
                        GameManager.Instance.fighterMainSlot3.ToggleSelectImage(true);
                    }
                }
            }

            if (!GameManager.Instance.GetActiveUnitFunctionality().GetBaseSelectedItem())
            {
                CombatGridManager.Instance.ToggleAllCombatSlotOutlines();
                CombatGridManager.Instance.UnselectAllSelectedCombatSlots();
            }
        }
        else
        {
            GameManager.Instance.ToggleSelectingUnits(true);

            GameManager.Instance.fighterMainSlot1.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
            GameManager.Instance.fighterMainSlot2.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
            GameManager.Instance.fighterMainSlot3.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
            GameManager.Instance.fighterMainSlot4.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
        }

        GetComponent<UIElement>().AnimateUI(false);

        if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
        {
            GameManager.Instance.selectingUnitsAllowed = true;
            GameManager.Instance.allowSelection = true;

            //CombatGridManager.Instance.ToggleTabButtons("Items");
        }

        if (GameManager.Instance.GetActiveItem())
        {
            if (GameManager.Instance.GetActiveItem().curActiveType == ItemPiece.ActiveType.PASSIVE)
            {
                CombatGridManager.Instance.ToggleAllCombatSlotOutlines();
                CombatGridManager.Instance.UnselectAllSelectedCombatSlots();
            }
        }
    }

    public void ButtonCombatSkillsTab()
    {
        if (!GameManager.Instance.GetAllowSelection())
            return;

        // Button Click SFX
        if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            AudioManager.Instance.Play("Button_Click");

        GameManager.Instance.isSkillsMode = true;

        GameManager.Instance.ResetSelectedUnits();

        if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            GameManager.Instance.UpdatePlayerAbilityUI(GameManager.Instance.isSkillsMode);

        GameManager.Instance.UpdateDetailsBanner();

        if (!GameManager.Instance.isSkillsMode)
        {
            GameManager.Instance.UpdateMainIconDetails(null, null);

            GameManager.Instance.fighterMainSlot1.ToggleSelectImage(false);
            GameManager.Instance.fighterMainSlot2.ToggleSelectImage(false);
            GameManager.Instance.fighterMainSlot3.ToggleSelectImage(false);
            GameManager.Instance.fighterMainSlot4.ToggleSelectImage(false);

            GameManager.Instance.ResetActiveItem();
        }
        else
        {
            GameManager.Instance.ToggleSelectingUnits(true);

            GameManager.Instance.fighterMainSlot1.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
            GameManager.Instance.fighterMainSlot2.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
            GameManager.Instance.fighterMainSlot3.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
            GameManager.Instance.fighterMainSlot4.UpdateMainIconBGColour(OwnedLootInven.Instance.GetSkillSlotBGColour());
        }

        GetComponent<UIElement>().AnimateUI(false);

        if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
        {
            CombatGridManager.Instance.ToggleTabButtons("Skills");
        }
    }

    public void SelectMainIcon1()
    {
        if (!GameManager.Instance.GetAllowSelection())
            return;

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        GameManager.Instance.ResetSelectedUnits();

        if (GameManager.Instance.isSkillsMode)
        {
            if (disabled)
                return;

            // If skill is on cooldown, stop
            if (GameManager.Instance.GetActiveUnitFunctionality().GetSkillCurCooldown(GameManager.Instance.GetActiveUnitFunctionality().GetSkill(0)) > 0)
                return;

            // If skill is locked, stop
            if (SkillsTabManager.Instance.skillBase1.GetIsLocked())
                return;

            GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnitFunctionality().GetSkill(0));
            GameManager.Instance.UpdateMainIconDetails(GameManager.Instance.GetActiveUnitFunctionality().GetSkill(0));

            // If selected, unselect it, dont do skill more stuff
            if (GetIfSelected())
            {

            }
            // If skill not already selected, select it and proceed
            else
            {
                GameManager.Instance.DisableAllMainSlotSelections();
                ToggleSelected(true);
            }

            //GameManager.Instance.UpdateUnitSelection(GameManager.Instance.activeSkill);
            //GameManager.Instance.UpdateUnitsSelectedText();
        }
        // Item
        else
        {
            for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
            {
                if (GameManager.Instance.GetActiveUnitFunctionality() == GameManager.Instance.activeRoomHeroes[i])
                {
                    if (i == 0)
                    {
                        if (TeamItemsManager.Instance.equippedItemsMain.Count > 0
                            && OwnedLootInven.Instance.GetWornItemMainAlly()[0].GetCalculatedItemsUsesRemaining2() > 0)
                        {
                            GameManager.Instance.UpdateActiveItem(TeamItemsManager.Instance.equippedItemsMain[0]);
                            GameManager.Instance.UpdateActiveItemSlot(OwnedLootInven.Instance.GetWornItemMainAlly()[0]);

                            GameManager.Instance.UpdateMainIconDetails(null, TeamItemsManager.Instance.equippedItemsMain[0]);
                            //GameManager.Instance.UpdateUnitSelection(null, TeamItemsManager.Instance.equippedItemsMain[0]);
                            //GameManager.Instance.UpdateUnitsSelectedText();

                            GameManager.Instance.DisableAllMainSlotSelections();
                            ToggleSelected(true);
                            GameManager.Instance.ToggleSelectingUnits(true);
                            break;
                        }
                        else
                        {
                            GameManager.Instance.UpdateActiveItem(null);
                            GameManager.Instance.UpdateMainIconDetails(null, null);
                            //GameManager.Instance.UpdateUnitSelection(null, null);
                            //GameManager.Instance.UpdateUnitsSelectedText();
                        }
                    }
                    else if (i == 1)
                    {
                        if (TeamItemsManager.Instance.equippedItemsSecond.Count > 0
                            && OwnedLootInven.Instance.GetWornItemSecondAlly()[0].GetCalculatedItemsUsesRemaining2() > 0)
                        {
                            GameManager.Instance.UpdateActiveItem(TeamItemsManager.Instance.equippedItemsSecond[0]);
                            GameManager.Instance.UpdateActiveItemSlot(OwnedLootInven.Instance.GetWornItemSecondAlly()[0]);

                            GameManager.Instance.UpdateMainIconDetails(null, TeamItemsManager.Instance.equippedItemsSecond[0]);
                            //GameManager.Instance.UpdateUnitSelection(null, TeamItemsManager.Instance.equippedItemsSecond[0]);
                            //GameManager.Instance.UpdateUnitsSelectedText();

                            GameManager.Instance.DisableAllMainSlotSelections();
                            ToggleSelected(true);
                            GameManager.Instance.ToggleSelectingUnits(true);
                            break;
                        }
                        else
                        {
                            GameManager.Instance.UpdateActiveItem(null);
                            GameManager.Instance.UpdateMainIconDetails(null, null);
                            //GameManager.Instance.UpdateUnitSelection(null, null);
                            //GameManager.Instance.UpdateUnitsSelectedText();
                        }
                    }
                    else if (i == 2)
                    {
                        if (TeamItemsManager.Instance.equippedItemsThird.Count > 0
                            && OwnedLootInven.Instance.GetWornItemThirdAlly()[0].GetCalculatedItemsUsesRemaining2() > 0)
                        {
                            GameManager.Instance.UpdateActiveItem(TeamItemsManager.Instance.equippedItemsThird[0]);
                            GameManager.Instance.UpdateActiveItemSlot(OwnedLootInven.Instance.GetWornItemThirdAlly()[0]);

                            GameManager.Instance.UpdateMainIconDetails(null, TeamItemsManager.Instance.equippedItemsThird[0]);
                            //GameManager.Instance.UpdateUnitSelection(null, TeamItemsManager.Instance.equippedItemsThird[0]);
                            //GameManager.Instance.UpdateUnitsSelectedText();

                            GameManager.Instance.DisableAllMainSlotSelections();
                            ToggleSelected(true);
                            GameManager.Instance.ToggleSelectingUnits(true);
                            break;
                        }
                        else
                        {
                            GameManager.Instance.UpdateActiveItem(null);
                            GameManager.Instance.UpdateMainIconDetails(null, null);
                           // GameManager.Instance.UpdateUnitSelection(null, null);
                            //GameManager.Instance.UpdateUnitsSelectedText();
                        }
                    }
                }
            }

            //GameManager.Instance.UpdateUnitSelection(null, GameManager.Instance.GetActiveItem());
            //GameManager.Instance.UpdateUnitsSelectedText();
        }
    }

    public void SelectMainIcon2()
    {
        if (!GameManager.Instance.GetAllowSelection())
            return;

        // If unit has two of the same item in this slot, and SLOT 1
        // Auto select slot 1 instead
        if (!GameManager.Instance.isSkillsMode)
        {
            if (GameManager.Instance.GetActiveUnitFunctionality().teamIndex == 0)
            {
                if (TeamItemsManager.Instance.equippedItemsMain.Count >= 2)
                {
                    if (GameManager.Instance.fighterMainSlot2.GetItemName() == GameManager.Instance.fighterMainSlot1.GetItemName())
                    {
                        GameManager.Instance.skill0Button.SelectMainIcon1();
                        return;
                    }
                }
            }
            else if (GameManager.Instance.GetActiveUnitFunctionality().teamIndex == 1)
            {
                if (TeamItemsManager.Instance.equippedItemsSecond.Count >= 2)
                {
                    if (GameManager.Instance.fighterMainSlot2.GetItemName() == GameManager.Instance.fighterMainSlot1.GetItemName())
                    {
                        GameManager.Instance.skill0Button.SelectMainIcon1();
                        return;
                    }
                }
            }
            else if (GameManager.Instance.GetActiveUnitFunctionality().teamIndex == 2)
            {
                if (TeamItemsManager.Instance.equippedItemsThird.Count >= 2)
                {
                    if (GameManager.Instance.fighterMainSlot2.GetItemName() == GameManager.Instance.fighterMainSlot1.GetItemName())
                    {
                        GameManager.Instance.skill0Button.SelectMainIcon1();
                        return;
                    }
                }
            }
        }

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        GameManager.Instance.ResetSelectedUnits();

        if (GameManager.Instance.isSkillsMode)
        {
            if (disabled)
                return;

            // If skill is on cooldown, stop
            if (GameManager.Instance.GetActiveUnitFunctionality().GetSkillCurCooldown(GameManager.Instance.GetActiveUnitFunctionality().GetSkill(1)) > 0)
                return;

            // If skill is locked, stop
            if (SkillsTabManager.Instance.skillBase2.GetIsLocked())
                return;

            GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnitFunctionality().GetSkill(1));
            GameManager.Instance.UpdateMainIconDetails(GameManager.Instance.GetActiveUnitFunctionality().GetSkill(1));

            // If selected, unselect it, dont do skill more stuff
            if (GetIfSelected())
            {

            }
            // If skill not already selected, select it and proceed
            else
            {
                GameManager.Instance.DisableAllMainSlotSelections();
                ToggleSelected(true);
            }

            //GameManager.Instance.UpdateUnitSelection(GameManager.Instance.activeSkill);
            //GameManager.Instance.UpdateUnitsSelectedText();
        }
        // Item
        else
        {
            for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
            {
                if (GameManager.Instance.GetActiveUnitFunctionality() == GameManager.Instance.activeRoomHeroes[i])
                {
                    if (i == 0)
                    {
                        if (TeamItemsManager.Instance.equippedItemsMain.Count > 1
                            && OwnedLootInven.Instance.GetWornItemMainAlly()[1].GetCalculatedItemsUsesRemaining2() > 0)
                        {
                            GameManager.Instance.UpdateActiveItem(TeamItemsManager.Instance.equippedItemsMain[1]);
                            GameManager.Instance.UpdateActiveItemSlot(OwnedLootInven.Instance.GetWornItemMainAlly()[1]);

                            GameManager.Instance.UpdateMainIconDetails(null, TeamItemsManager.Instance.equippedItemsMain[1]);
                            //GameManager.Instance.UpdateUnitSelection(null, TeamItemsManager.Instance.equippedItemsMain[1]);
                            //GameManager.Instance.UpdateUnitsSelectedText();

                            GameManager.Instance.DisableAllMainSlotSelections();
                            ToggleSelected(true);
                            GameManager.Instance.ToggleSelectingUnits(true);
                            break;
                        }
                        else
                        {
                            GameManager.Instance.UpdateActiveItem(null);
                            GameManager.Instance.UpdateMainIconDetails(null, null);
                            //GameManager.Instance.UpdateUnitSelection(null, null);
                            //GameManager.Instance.UpdateUnitsSelectedText();
                        }
                    }
                    else if (i == 1)
                    {
                        if (TeamItemsManager.Instance.equippedItemsSecond.Count > 1
                            && OwnedLootInven.Instance.GetWornItemSecondAlly()[1].GetCalculatedItemsUsesRemaining2() > 0)
                        {
                            GameManager.Instance.UpdateActiveItem(TeamItemsManager.Instance.equippedItemsSecond[1]);
                            GameManager.Instance.UpdateActiveItemSlot(OwnedLootInven.Instance.GetWornItemSecondAlly()[1]);

                            GameManager.Instance.UpdateMainIconDetails(null, TeamItemsManager.Instance.equippedItemsSecond[1]);
                            //GameManager.Instance.UpdateUnitSelection(null, TeamItemsManager.Instance.equippedItemsSecond[1]);
                            //GameManager.Instance.UpdateUnitsSelectedText();

                            GameManager.Instance.DisableAllMainSlotSelections();
                            ToggleSelected(true);
                            GameManager.Instance.ToggleSelectingUnits(true);
                            break;
                        }
                        else
                        {
                            GameManager.Instance.UpdateActiveItem(null);
                            GameManager.Instance.UpdateMainIconDetails(null, null);
                            //GameManager.Instance.UpdateUnitSelection(null, null);
                            //GameManager.Instance.UpdateUnitsSelectedText();
                        }
                    }
                    else if (i == 2)
                    {
                        if (TeamItemsManager.Instance.equippedItemsThird.Count > 1
                            && OwnedLootInven.Instance.GetWornItemThirdAlly()[1].GetCalculatedItemsUsesRemaining2() > 0)
                        {
                            GameManager.Instance.UpdateActiveItem(TeamItemsManager.Instance.equippedItemsThird[1]);
                            GameManager.Instance.UpdateActiveItemSlot(OwnedLootInven.Instance.GetWornItemThirdAlly()[1]);

                            GameManager.Instance.UpdateMainIconDetails(null, TeamItemsManager.Instance.equippedItemsThird[1]);
                            //GameManager.Instance.UpdateUnitSelection(null, TeamItemsManager.Instance.equippedItemsThird[1]);
                            //GameManager.Instance.UpdateUnitsSelectedText();

                            GameManager.Instance.DisableAllMainSlotSelections();
                            ToggleSelected(true);
                            GameManager.Instance.ToggleSelectingUnits(true);
                            break;
                        }
                        else
                        {
                            GameManager.Instance.UpdateActiveItem(null);
                            GameManager.Instance.UpdateMainIconDetails(null, null);
                            //GameManager.Instance.UpdateUnitSelection(null, null);
                            //GameManager.Instance.UpdateUnitsSelectedText();
                        }
                    }
                }
            }

            // If selected, unselect it, dont do skill more stuff
            if (GetIfSelected())
            {

            }
            // If skill not already selected, select it and proceed
            else
            {
                GameManager.Instance.DisableAllMainSlotSelections();
                ToggleSelected(true);
            }

            //GameManager.Instance.UpdateUnitSelection(null, GameManager.Instance.GetActiveItem());
            //GameManager.Instance.UpdateUnitsSelectedText();
        }
    }

    public void SelectMainIcon3()
    {
        if (!GameManager.Instance.GetAllowSelection())
            return;

        // If unit has two of the same item in this slot, and SLOT 1
        // Auto select slot 1 instead
        if (!GameManager.Instance.isSkillsMode)
        {
            if (GameManager.Instance.GetActiveUnitFunctionality().teamIndex == 0)
            {
                if (TeamItemsManager.Instance.equippedItemsMain.Count >= 3)
                {
                    if (GameManager.Instance.fighterMainSlot3.GetItemName() == GameManager.Instance.fighterMainSlot1.GetItemName())
                    {
                        GameManager.Instance.skill0Button.SelectMainIcon1();
                        return;
                    }
                    else if (GameManager.Instance.fighterMainSlot3.GetItemName() == GameManager.Instance.fighterMainSlot2.GetItemName())
                    {
                        GameManager.Instance.skill1Button.SelectMainIcon2();
                        return;
                    }
                }
            }
            else if (GameManager.Instance.GetActiveUnitFunctionality().teamIndex == 1)
            {
                if (TeamItemsManager.Instance.equippedItemsSecond.Count >= 3)
                {
                    if (GameManager.Instance.fighterMainSlot3.GetItemName() == GameManager.Instance.fighterMainSlot1.GetItemName())
                    {
                        GameManager.Instance.skill0Button.SelectMainIcon1();
                        return;
                    }
                    else if (GameManager.Instance.fighterMainSlot3.GetItemName() == GameManager.Instance.fighterMainSlot2.GetItemName())
                    {
                        GameManager.Instance.skill1Button.SelectMainIcon2();
                        return;
                    }
                }
            }
            else if (GameManager.Instance.GetActiveUnitFunctionality().teamIndex == 2)
            {
                if (TeamItemsManager.Instance.equippedItemsThird.Count >= 3)
                {
                    if (GameManager.Instance.fighterMainSlot3.GetItemName() == GameManager.Instance.fighterMainSlot1.GetItemName())
                    {
                        GameManager.Instance.skill0Button.SelectMainIcon1();
                        return;
                    }
                    else if (GameManager.Instance.fighterMainSlot3.GetItemName() == GameManager.Instance.fighterMainSlot2.GetItemName())
                    {
                        GameManager.Instance.skill1Button.SelectMainIcon2();
                        return;
                    }
                }
            }
        }

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        GameManager.Instance.ResetSelectedUnits();

        if (GameManager.Instance.isSkillsMode)
        {
            if (disabled)
                return;

            // If skill is on cooldown, stop
            if (GameManager.Instance.GetActiveUnitFunctionality().GetSkillCurCooldown(GameManager.Instance.GetActiveUnitFunctionality().GetSkill(2)) > 0)
                return;

            // If skill is locked, stop
            if (SkillsTabManager.Instance.skillBase3.GetIsLocked())
                return;

            GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnitFunctionality().GetSkill(2));
            GameManager.Instance.UpdateMainIconDetails(GameManager.Instance.GetActiveUnitFunctionality().GetSkill(2));

            // If selected, unselect it, dont do skill more stuff
            if (GetIfSelected())
            {

            }
            // If skill not already selected, select it and proceed
            else
            {
                GameManager.Instance.DisableAllMainSlotSelections();
                ToggleSelected(true);
            }

            //GameManager.Instance.UpdateUnitSelection(GameManager.Instance.activeSkill);
            //GameManager.Instance.UpdateUnitsSelectedText();
        }
        // Item
        else
        {
            for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
            {
                if (GameManager.Instance.GetActiveUnitFunctionality() == GameManager.Instance.activeRoomHeroes[i])
                {
                    if (i == 0)
                    {
                        if (TeamItemsManager.Instance.equippedItemsMain.Count > 2
                            && OwnedLootInven.Instance.GetWornItemMainAlly()[2].GetCalculatedItemsUsesRemaining2() > 0)
                        {
                            GameManager.Instance.UpdateActiveItem(TeamItemsManager.Instance.equippedItemsMain[2]);
                            GameManager.Instance.UpdateActiveItemSlot(OwnedLootInven.Instance.GetWornItemMainAlly()[2]);

                            GameManager.Instance.UpdateMainIconDetails(null, TeamItemsManager.Instance.equippedItemsMain[2]);
                            //GameManager.Instance.UpdateUnitSelection(null, TeamItemsManager.Instance.equippedItemsMain[2]);
                            //GameManager.Instance.UpdateUnitsSelectedText();

                            GameManager.Instance.DisableAllMainSlotSelections();
                            ToggleSelected(true);
                            GameManager.Instance.ToggleSelectingUnits(true);
                            break;
                        }
                        else
                        {
                            GameManager.Instance.UpdateActiveItem(null);
                            GameManager.Instance.UpdateMainIconDetails(null, null);
                            //GameManager.Instance.UpdateUnitSelection(null, null);
                            //GameManager.Instance.UpdateUnitsSelectedText();
                        }
                    }
                    else if (i == 1)
                    {
                        if (TeamItemsManager.Instance.equippedItemsSecond.Count > 2
                            && OwnedLootInven.Instance.GetWornItemSecondAlly()[2].GetCalculatedItemsUsesRemaining2() > 0)
                        {
                            GameManager.Instance.UpdateActiveItem(TeamItemsManager.Instance.equippedItemsSecond[2]);
                            GameManager.Instance.UpdateActiveItemSlot(OwnedLootInven.Instance.GetWornItemSecondAlly()[2]);

                            GameManager.Instance.UpdateMainIconDetails(null, TeamItemsManager.Instance.equippedItemsSecond[2]);
                            //GameManager.Instance.UpdateUnitSelection(null, TeamItemsManager.Instance.equippedItemsSecond[2]);
                            //GameManager.Instance.UpdateUnitsSelectedText();

                            GameManager.Instance.DisableAllMainSlotSelections();
                            ToggleSelected(true);
                            GameManager.Instance.ToggleSelectingUnits(true);
                            break;
                        }
                        else
                        {
                            GameManager.Instance.UpdateActiveItem(null);
                            GameManager.Instance.UpdateMainIconDetails(null, null);
                            //GameManager.Instance.UpdateUnitSelection(null, null);
                            //GameManager.Instance.UpdateUnitsSelectedText();
                        }
                    }
                    else if (i == 2)
                    {
                        if (TeamItemsManager.Instance.equippedItemsThird.Count > 2
                            && OwnedLootInven.Instance.GetWornItemThirdAlly()[2].GetCalculatedItemsUsesRemaining2() > 0)
                        {
                            GameManager.Instance.UpdateActiveItem(TeamItemsManager.Instance.equippedItemsThird[2]);
                            GameManager.Instance.UpdateActiveItemSlot(OwnedLootInven.Instance.GetWornItemThirdAlly()[2]);

                            GameManager.Instance.UpdateMainIconDetails(null, TeamItemsManager.Instance.equippedItemsThird[2]);
                            //GameManager.Instance.UpdateUnitSelection(null, TeamItemsManager.Instance.equippedItemsThird[2]);
                            //GameManager.Instance.UpdateUnitsSelectedText();

                            GameManager.Instance.DisableAllMainSlotSelections();
                            ToggleSelected(true);
                            GameManager.Instance.ToggleSelectingUnits(true);
                            break;
                        }
                        else
                        {
                            GameManager.Instance.UpdateActiveItem(null);
                            GameManager.Instance.UpdateMainIconDetails(null, null);
                            //GameManager.Instance.UpdateUnitSelection(null, null);
                            //GameManager.Instance.UpdateUnitsSelectedText();
                        }
                    }
                }
            }

            // If selected, unselect it, dont do skill more stuff
            if (GetIfSelected())
            {

            }
            // If skill not already selected, select it and proceed
            else
            {
                GameManager.Instance.DisableAllMainSlotSelections();
                ToggleSelected(true);
            }

            //GameManager.Instance.UpdateUnitSelection(null, GameManager.Instance.GetActiveItem());
            //GameManager.Instance.UpdateUnitsSelectedText();
        }
    }

    public void SelectMainIcon4()
    {
        if (!GameManager.Instance.GetAllowSelection())
            return;

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        GameManager.Instance.ResetSelectedUnits();

        if (GameManager.Instance.isSkillsMode)
        {
            if (disabled)
                return;

            // If skill is on cooldown, stop
            if (GameManager.Instance.GetActiveUnitFunctionality().GetSkillCurCooldown(GameManager.Instance.GetActiveUnitFunctionality().GetSkill(3)) > 0)
                return;

            // If skill is locked, stop
            if (SkillsTabManager.Instance.skillBase4.GetIsLocked())
                return;

            GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnitFunctionality().GetSkill(3));
            GameManager.Instance.UpdateMainIconDetails(GameManager.Instance.GetActiveUnitFunctionality().GetSkill(3));

            // If selected, unselect it, dont do skill more stuff
            if (GetIfSelected())
            {

            }
            // If skill not already selected, select it and proceed
            else
            {
                GameManager.Instance.DisableAllMainSlotSelections();
                ToggleSelected(true);
            }

            //GameManager.Instance.UpdateUnitSelection(GameManager.Instance.activeSkill);
            //GameManager.Instance.UpdateUnitsSelectedText();
        }
        // Item
        else
        {
            for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
            {
                if (GameManager.Instance.GetActiveUnitFunctionality() == GameManager.Instance.activeRoomHeroes[i])
                {
                    if (i == 0)
                    {
                        if (TeamItemsManager.Instance.equippedItemsMain.Count > 3)
                        {
                            GameManager.Instance.UpdateActiveItem(TeamItemsManager.Instance.equippedItemsMain[3]);
                            GameManager.Instance.UpdateMainIconDetails(null, TeamItemsManager.Instance.equippedItemsMain[3]);
                            //GameManager.Instance.UpdateUnitSelection(null, TeamItemsManager.Instance.equippedItemsMain[3]);
                            //GameManager.Instance.UpdateUnitsSelectedText();

                            GameManager.Instance.DisableAllMainSlotSelections();
                            ToggleSelected(true);
                            GameManager.Instance.ToggleSelectingUnits(true);
                            break;
                        }
                        else
                        {
                            GameManager.Instance.UpdateActiveItem(null);
                            GameManager.Instance.UpdateMainIconDetails(null, null);
                            //GameManager.Instance.UpdateUnitSelection(null, null);
                            //GameManager.Instance.UpdateUnitsSelectedText();
                        }
                    }
                    else if (i == 1)
                    {
                        if (TeamItemsManager.Instance.equippedItemsSecond.Count > 3)
                        {
                            GameManager.Instance.UpdateActiveItem(TeamItemsManager.Instance.equippedItemsSecond[3]);
                            GameManager.Instance.UpdateMainIconDetails(null, TeamItemsManager.Instance.equippedItemsSecond[3]);
                            //GameManager.Instance.UpdateUnitSelection(null, TeamItemsManager.Instance.equippedItemsSecond[3]);
                            //GameManager.Instance.UpdateUnitsSelectedText();

                            GameManager.Instance.DisableAllMainSlotSelections();
                            ToggleSelected(true);
                            GameManager.Instance.ToggleSelectingUnits(true);
                            break;
                        }
                        else
                        {
                            GameManager.Instance.UpdateActiveItem(null);
                            GameManager.Instance.UpdateMainIconDetails(null, null);
                            //GameManager.Instance.UpdateUnitSelection(null, null);
                            //GameManager.Instance.UpdateUnitsSelectedText();
                        }
                    }
                    else if (i == 2)
                    {
                        if (TeamItemsManager.Instance.equippedItemsThird.Count > 3)
                        {
                            GameManager.Instance.UpdateActiveItem(TeamItemsManager.Instance.equippedItemsThird[3]);
                            GameManager.Instance.UpdateMainIconDetails(null, TeamItemsManager.Instance.equippedItemsThird[3]);
                            //GameManager.Instance.UpdateUnitSelection(null, TeamItemsManager.Instance.equippedItemsThird[3]);
                            //GameManager.Instance.UpdateUnitsSelectedText();

                            GameManager.Instance.DisableAllMainSlotSelections();
                            ToggleSelected(true);
                            GameManager.Instance.ToggleSelectingUnits(true);
                            break;
                        }
                        else
                        {
                            GameManager.Instance.UpdateActiveItem(null);
                            GameManager.Instance.UpdateMainIconDetails(null, null);
                            //GameManager.Instance.UpdateUnitSelection(null, null);
                            //GameManager.Instance.UpdateUnitsSelectedText();
                        }
                    }
                }
            }

            // If selected, unselect it, dont do skill more stuff
            if (GetIfSelected())
            {

            }
            // If skill not already selected, select it and proceed
            else
            {
                GameManager.Instance.DisableAllMainSlotSelections();
                ToggleSelected(true);
            }

            //GameManager.Instance.UpdateUnitSelection(null, GameManager.Instance.GetActiveItem());
            //GameManager.Instance.UpdateUnitsSelectedText();
        }
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
            if (mainUIelement != null && isStatButton)
            {
                locked = true;
                SkillsTabManager.Instance.holdingButton = mainUIelement;
                SkillsTabManager.Instance.holdingButton.ToggleTooltipStats(true);
            }
            else if (GetComponentInParent<UnitFunctionality>() != null)
            {
                UnitFunctionality unit = GetComponentInParent<UnitFunctionality>();

                unit.ToggleTooltipStats(true);

                locked = true;
            }
            // If stats
            else
            {

            }
        }
    }

    public void SelectEffect()
    {
        if (effectHeldTimer > 0 && !isEnabled)
        {
            isEnabled = true;
            Effect effect = GetComponent<Effect>();
            if (unit == null)
            {
                unit = GetComponentInParent<UnitFunctionality>();
            }

            unit.ToggleTooltipEffect(true, effect.effectName);
        }
    }

    public void ToggleSelection(bool toggle = true)
    {
        selectBorder.ToggleSelected(toggle);
    }

    public void SelectFallenHero()
    {
        ShopManager.Instance.ToggleAllFallenHeroSelection(false);

        ToggleSelection(true);

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        ShopManager.Instance.ToggleFallenHeroPrompt(true);

        ShopManager.Instance.selectedFallenUnitName = GetComponentInParent<MenuUnitDisplay>().unitName; 
    }

    public void SelectShopkeeper()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        ShopManager.Instance.ToggleShopKeeperSelected(!ShopManager.Instance.GetShopKeeperSelected());
    }

    public void ButtonRerollShop()
    {
        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        ShopManager.Instance.CalculatePurchaseAcceptReroll();
    }

    public void ButtonSellItem()
    {
        //Debug.Log("sell item");
    }

    public void SelectUnit()
    {
        // If a unit is selected that is dead during movement, do not select the unit
        if (unit.isDead && GameManager.Instance.GetActiveSkill().curskillSelectionAliveType == SkillData.SkillSelectionAliveType.ALIVE)
        {
            ButtonSelectCombatSlot(true);
            return;
        }

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

        if (!CombatGridManager.Instance.isCombatMode && !GameManager.Instance.GetActiveUnitFunctionality().hasAttacked)
        {
            // Button Click SFX
            AudioManager.Instance.Play("Button_Click");
            GameManager.Instance.isSkillsMode = true;
            CombatGridManager.Instance.isCombatMode = true;

            //GameManager.Instance.skill0Button.SelectMainIcon1();

            if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
                CombatGridManager.Instance.ToggleTabButtons("Skills");

            CombatGridManager.Instance.UpdateAttackMovementMode(false, true, true);
            return;

        }


        if (ShopManager.Instance.playerInShopRoom)
        {

        }
        else if (!CombatGridManager.Instance.isCombatMode || !GameManager.Instance.GetAllowSelection())
            return;
        
        // If player's turn, and unit is selected, attempt to select the tile too
        if (GameManager.Instance.playerInCombat && GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
        {
            if (unit.GetActiveCombatSlot())
            {
                unit.GetActiveCombatSlot().GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot();
            }
        }

        if (!GameManager.Instance.playerInCombat || RoomManager.Instance.GetActiveRoom().curRoomType == RoomMapIcon.RoomType.HERO)
            StartCoroutine(SelectUnitCo());
    }

    IEnumerator SelectUnitCo()
    {
        if (unit != null)
        {
            if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER && !GameManager.Instance.CheckSkipUnitTurn(GameManager.Instance.GetActiveUnitFunctionality()) || HeroRoomManager.Instance.playerInHeroRoomView)
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
                    if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY && HeroRoomManager.Instance.playerInHeroRoomView)
                        yield break;
                    else
                    {
                        isHeldDownUnit = false;
                        StartCoroutine(HeldDownCooldown());

                        if (!ShopManager.Instance.playerInShopRoom)
                            GetComponentInParent<UnitFunctionality>().ToggleTooltipStats(false);



                        if (GameManager.Instance.playerInCombat && !PostBattle.Instance.isInPostBattle || HeroRoomManager.Instance.playerInHeroRoomView || ShopManager.Instance.playerInShopRoom)
                        {
                            if (unit.IsSelected() || HeroRoomManager.Instance.playerInHeroRoomView || ShopManager.Instance.playerInShopRoom)
                            {
                                GameManager.Instance.targetUnit(unit, false);
                            }
                        }

                        unitIsSelected = true;
                    }
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (ShopManager.Instance.playerInShopRoom)
            return;

        // If not pressing effect button, instead pressing a unit
        if (!isEffectButton)
        {
            if (GameManager.Instance.playerInCombat)
            {
                if (isStatButton)
                {
                    isHeldDownStat = true;
                    //Debug.Log("c");
                }
                else if (GetComponentInParent<UnitFunctionality>() && !GetComponentInParent<UnitFunctionality>().isDead)
                {
                    isHeldDownUnit = true;
                    //Debug.Log("Pointer Down");

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
        if (ShopManager.Instance.playerInShopRoom)
            return;

        if (!isEffectButton)
        {
            if (isStatButton)
            {
                isHeldDownStat = false;
                heldTimer = 0;
                StartCoroutine(HeldDownCooldown());

                if (SkillsTabManager.Instance.holdingButton != null)
                    SkillsTabManager.Instance.holdingButton.ToggleTooltipStats(false);
            }
            else if (GetComponentInParent<UnitFunctionality>())
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

            }
        }
        else
        {
            isHeldDownEffect = false;
            effectHeldTimer = 0;
            isEnabled = false;
            //Debug.Log("hiding effect tooltip");
            unit.ToggleTooltipEffect(false);
            HideEffectTooltipOvertime();
        }
    }

    IEnumerator HeldDownCooldown()
    {
        yield return new WaitForSeconds(.1f);

        locked = false;
    }

    public void HideEffectTooltipOvertime(bool onlyEnemies = false)
    {
        if (unit)
        {
            if (!onlyEnemies)
            {
                isHeldDownEffect = false;
                effectHeldTimer = 0;
                isEnabled = false;
                //unit.ToggleTooltipEffect(false);
            }
            else
            {
                if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                {
                    isHeldDownEffect = false;
                    effectHeldTimer = 0;
                    isEnabled = false;
                    //unit.ToggleTooltipEffect(false);
                }
            }
        }
    }
}
