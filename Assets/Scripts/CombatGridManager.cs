using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatGridManager : MonoBehaviour
{
    public static CombatGridManager Instance;
    public float unitMoveArrowOnAlpha = 0.75f;

    [SerializeField] private List<CombatSlot> aimTargetCombatSlots = new List<CombatSlot>();

    public List<CombatSlot> targetedCombatSlots = new List<CombatSlot>();
    public Animation combatSlotIdle;
    public Animation combatSlotAttackIdle;

    public Color slotAllowedColour;
    public Color slotNotAllowedColour;
    public Color slotDisabledColour;
    public Color slotSelectedColour;
    public Color slotUnSelectedColour;
    public Color slotAggressiveColour;
    public Color slotSupportColour;

    public float moveTimer = 0;
    public float unitMoveSpeed = 1;
    [SerializeField] private ButtonFunctionality buttonSkills;
    [SerializeField] private ButtonFunctionality buttonItems;
    [SerializeField] private ButtonFunctionality buttonAttack;
    [SerializeField] private ButtonFunctionality buttonMovement;

    [SerializeField] private CombatSlot selectedCombatSlotMove;
    [SerializeField] private UIElement buttonAttackMovementToggle;
    [SerializeField] private UIElement combatMainSlots;
    [SerializeField] private List<CombatSlot> allCombatSlots = new List<CombatSlot>();

    [SerializeField] private CombatSlot newFighterCombatSlot;
    [SerializeField] private List<CombatSlot> fighterSpawnCombatSlots = new List<CombatSlot>();

    [SerializeField] private List<CombatSlot> fighterCombatSlots = new List<CombatSlot>();

    [SerializeField] private List<CombatSlot> enemySpawnCombatSlots = new List<CombatSlot>();

    public bool isCombatMode = false;
    [SerializeField] private bool isMovementAllowed = true;

    public Transform gridParent;


    public void ToggleCombatSlotsInput(bool toggle = true)
    {
        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            allCombatSlots[i].ToggleCombatSlotInput(toggle);
        }
    }
    public void ToggleCombatSlotsInput2(bool toggle = true)
    {
        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            allCombatSlots[i].ToggleCombatSlotInput2(toggle);
        }
    }

    public void Setup()
    {
        UpdateCombatSlotsIndex();

        DisableAllButtons();
    }

    public void ResetCombatSlots(bool destroy = false)
    {
        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            allCombatSlots[i].ResetSlotUnitData(destroy);
        }
    }
    public void DisableAllButtons()
    {
        ToggleButton(GetButtonItems(), false);
        ToggleButton(GetButtonAttack(), false);
        ToggleButton(GetButtonMovement(), false);
        ToggleButton(GetButtonSkills(), false);
    }

    public void ToggleTabButtons(string tabName = "")
    {
        Debug.Log("toggling button " + tabName);
        if (tabName == "Attack")
        {
            if (GameManager.Instance.isSkillsMode)
            {
                ToggleButton(GetButtonAttack(), false, true);
                ToggleButton(GetButtonSkills(), false, true);

                if (GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() > 0)
                    ToggleButton(GetButtonMovement(), true, true);
                else if (GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() <= 0)
                {
                    if (GameManager.Instance.GetActiveUnitFunctionality().hasAttacked)
                        ToggleButton(GetButtonMovement(), false, true);
                    else
                        ToggleButton(GetButtonMovement(), true, true);
                }

                if (!GameManager.Instance.GetActiveUnitFunctionality().reanimated)
                    ToggleButton(GetButtonItems(), true, true);
                else
                    ToggleButton(GetButtonItems(), false, true);
            }
            else
            {
                ToggleButton(GetButtonAttack(), false, true);
                ToggleButton(GetButtonItems(), false, true);

                if (GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() > 0)
                    ToggleButton(GetButtonMovement(), true, true);
                else if (GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() <= 0)
                {
                    if (GameManager.Instance.GetActiveUnitFunctionality().hasAttacked)
                        ToggleButton(GetButtonMovement(), false, true);
                    else
                        ToggleButton(GetButtonMovement(), true, true);
                }

                if (GameManager.Instance.GetActiveUnitFunctionality().hasAttacked)
                    ToggleButton(GetButtonSkills(), false, true);
                else
                    ToggleButton(GetButtonSkills(), true, true);
            }
        }
        else if (tabName == "Movement")
        {
            ToggleButton(GetButtonSkills(), false, true);
            ToggleButton(GetButtonMovement(), false, true);
            ToggleButton(GetButtonItems(), false, true);

            ToggleButton(GetButtonAttack(), true, true);
        }
        else if (tabName == "Items")
        {
            ToggleButton(GetButtonItems(), false, true);
            ToggleButton(GetButtonAttack(), false, true);

            if (GameManager.Instance.GetActiveUnitFunctionality().hasAttacked)
                ToggleButton(GetButtonSkills(), false, true);
            else
                ToggleButton(GetButtonSkills(), true, true);

            if (GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() > 0)
                ToggleButton(GetButtonMovement(), true, true);
            else if (GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() <= 0)
            {
                if (GameManager.Instance.GetActiveUnitFunctionality().hasAttacked)
                    ToggleButton(GetButtonMovement(), false, true);
                else
                    ToggleButton(GetButtonMovement(), true, true);
            }
        }
        else if (tabName == "Skills")
        {
            ToggleButton(GetButtonAttack(), false, true);
            ToggleButton(GetButtonSkills(), false, true);

            if (GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() > 0)
                ToggleButton(GetButtonMovement(), true, true);
            else if (GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() <= 0)
            {
                if (GameManager.Instance.GetActiveUnitFunctionality().hasAttacked)
                    ToggleButton(GetButtonMovement(), false, true);
                else
                    ToggleButton(GetButtonMovement(), true, true);
            }

            if (!GameManager.Instance.GetActiveUnitFunctionality().reanimated)
                ToggleButton(GetButtonItems(), true, true);
            else
                ToggleButton(GetButtonItems(), false, true);
        }

        if (GameManager.Instance.GetActiveUnitFunctionality().reanimated)
            ToggleButton(GetButtonItems(), false, true);
    }

    public void ToggleButton(ButtonFunctionality button, bool toggle = true, bool allowHide = false)
    {
        if (toggle)
        {
            if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY)
            {
                if (button == buttonSkills || button == buttonItems || button == buttonAttack || button == buttonMovement)
                    return;
            }


            button.GetComponent<UIElement>().ToggleButton(true);
            button.GetComponent<UIElement>().UpdateAlpha(1);
        }
        else
        {
            button.GetComponent<UIElement>().ToggleButton(false);

            if (GameManager.Instance.GetActiveUnitFunctionality())
            {
                if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY)
                    allowHide = false;
            }

            if (allowHide)
                button.GetComponent<UIElement>().UpdateAlpha(.225f);
            else
                button.GetComponent<UIElement>().UpdateAlpha(0);
        }
    }

    public ButtonFunctionality GetButtonAttack()
    {
        return buttonAttack;
    }

    public ButtonFunctionality GetButtonItems()
    {
        return buttonItems;
    }


    public ButtonFunctionality GetButtonSkills()
    {
        return buttonSkills;
    }

    public ButtonFunctionality GetButtonMovement()
    {
        return buttonMovement;
    }

    public bool GetIsMovementAllowed()
    {
        return isMovementAllowed;
    }

    public void ToggleIsMovementAllowed(bool toggle = true)
    {
        isMovementAllowed = toggle;
    }

    public void ResetAllowedSlotAnims()
    {
        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            if (allCombatSlots[i].GetAllowed())
            {
                allCombatSlots[i].ResetAnimation();
            }
        }
    }

    public void UpdateAttackMovementMode(bool forceMovement = false, bool forceCombat = false, bool enabled = false)
    {
        if (forceMovement)
        {
            isCombatMode = enabled;
        }
        else if (forceCombat)
        {
            isCombatMode = enabled;
        }

        if (!forceMovement && !forceCombat)
            isCombatMode = !isCombatMode;

        if (isCombatMode)
        {
            // Disable extra move prompt
            OverlayUI.Instance.extraMovePrompt.UpdateAlpha(0);

            if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
                GameManager.Instance.ToggleSkillsItemToggleButton(true);

            UnselectAllSelectedCombatSlots();

            if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER || GameManager.Instance.GetActiveUnitFunctionality().reanimated)
            {
                GameManager.Instance.SetupPlayerUI();
            }

            if (GameManager.Instance.isSkillsMode)
            {
                OverlayUI.Instance.ToggleAllStats(true, true, false);

                if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
                    GameManager.Instance.UpdatePlayerAbilityUI(true);

                if (GameManager.Instance.GetActiveSkill() && GameManager.Instance.GetActiveItem())
                    GameManager.Instance.UpdateMainIconDetails(GameManager.Instance.GetActiveSkill(), GameManager.Instance.GetActiveItem());
                else if (GameManager.Instance.GetActiveSkill())
                    GameManager.Instance.UpdateMainIconDetails(GameManager.Instance.GetActiveSkill(), null);
            }
            else
            {
                OverlayUI.Instance.ToggleAllStats(true, false, false);

                if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
                    GameManager.Instance.UpdatePlayerAbilityUI(false);

                if (GameManager.Instance.GetActiveItem())
                    GameManager.Instance.UpdateMainIconDetails(null, GameManager.Instance.GetActiveItem());
            }

            isCombatMode = true;
            GameManager.Instance.UpdateDetailsBanner();

            if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER
                && !GameManager.Instance.isSkillsMode && GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() <= 0)
            {
                for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
                {
                    if (GameManager.Instance.activeRoomHeroes[i].teamIndex == 0)
                    {
                        if (TeamItemsManager.Instance.equippedItemsMain.Count == 0)
                        {
                            StartCoroutine(GameManager.Instance.GetActiveUnitFunctionality().UnitEndTurn());
                            break;
                        }
                    }
                    else if (GameManager.Instance.activeRoomHeroes[i].teamIndex == 1)
                    {
                        if (TeamItemsManager.Instance.equippedItemsSecond.Count == 0)
                        {
                            StartCoroutine(GameManager.Instance.GetActiveUnitFunctionality().UnitEndTurn());
                            break;
                        }
                    }
                    else if (GameManager.Instance.activeRoomHeroes[i].teamIndex == 2)
                    {
                        if (TeamItemsManager.Instance.equippedItemsThird.Count == 0)
                        {
                            StartCoroutine(GameManager.Instance.GetActiveUnitFunctionality().UnitEndTurn());
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            GameManager.Instance.ToggleSkillsItemToggleButton(false);

            if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
                GameManager.Instance.UpdatePlayerAbilityUI(false, false, true);

            GameManager.Instance.UpdateMainIconDetails(null, null);

            OverlayUI.Instance.ToggleAllStats(true, false, true);

            UpdateUnitMoveRange(GameManager.Instance.GetActiveUnitFunctionality());

            // Enable extra move prompt if unit has 0 moves left
            if (GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() == 0)
                OverlayUI.Instance.extraMovePrompt.UpdateAlpha(1);
            else
                OverlayUI.Instance.extraMovePrompt.UpdateAlpha(0);

            isCombatMode = false;

            GameManager.Instance.UpdateDetailsBanner();
            ToggleAllCombatSlotOutlines();
            GameManager.Instance.ResetSelectedUnits();
        }

        UpdateCombatMainSlots();
    }

    public void UpdateCombatMainSlots()
    {
        ToggleCombatMainSlots(isCombatMode);
    }

    public void ToggleButtonAttackMovement(bool toggle = true)
    {
        if (toggle)
        {
            buttonAttackMovementToggle.UpdateAlpha(1);
        }
        else
        {
            buttonAttackMovementToggle.UpdateAlpha(0);
        }
    }

    public void ToggleCombatMainSlots(bool toggle = true)
    {
        if (toggle)
        {
            combatMainSlots.UpdateAlpha(1);
        }
        else
        {
            combatMainSlots.UpdateAlpha(0);
        }
    }

    public CombatSlot GetNewFighterCombatSlot()
    {
        return newFighterCombatSlot;
    }

    public List<CombatSlot> GetEnemySpawnCombatSlots()
    {
        return enemySpawnCombatSlots;
    }
    public CombatSlot GetEnemySpawnCombatSlot(int index = 0)
    {
        return enemySpawnCombatSlots[index];
    }

    public CombatSlot GetFighterSpawnCombatSlot(int index = 0)
    {
        return fighterSpawnCombatSlots[index];
    }

    public List<CombatSlot> GetFighterSpawnCombatSlots()
    {
        return fighterSpawnCombatSlots;
    }

    public CombatSlot GetFighterCombatSlots(int index = 0)
    {
        return fighterCombatSlots[index];
    }

    public List<CombatSlot> GetFighterCombatSlots()
    {
        return fighterCombatSlots;
    }

    void Start()
    {
        Setup();
    }

    public void CheckToUnlinkCombatSlot()
    {
        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            if (allCombatSlots[i].GetLinkedUnit())
            {
                if (!allCombatSlots[i].GetComponentInChildren<UnitFunctionality>())
                    allCombatSlots[i].UpdateLinkedUnit(null);
            }
        }
    }

    public void UnselectAllSelectedCombatSlots()
    {
        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            allCombatSlots[i].ToggleCombatSelected(false);
            allCombatSlots[i].ToggleSlotAllowed(false);
            allCombatSlots[i].ToggleSlotSelected(false);
            allCombatSlots[i].ToggleSlotSelectedSize(true);
        }
    }

    public void RemoveAllCombatSelectedCombatSlots()
    {
        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            if (allCombatSlots[i].GetLinkedUnit())
            {
                if (allCombatSlots[i].GetLinkedUnit().isSelected)
                if (allCombatSlots[i].combatSelected)
                    allCombatSlots[i].combatSelected = false;
            }


            allCombatSlots[i].ToggleCombatSelected(false);
        }

        GameManager.Instance.ResetSelectedUnits();
    }

    bool allowMovement = false;
    UnitFunctionality movingUnit;
    Vector2 startingPos;
    Vector2 endingPos;

    private void FixedUpdate()
    {
        if (allowMovement)
        {
            moveTimer += Time.deltaTime * unitMoveSpeed;
            movingUnit.transform.position = Vector3.Lerp(startingPos, endingPos, moveTimer);

            movingUnit.transform.localPosition = new Vector3(movingUnit.transform.localPosition.x, movingUnit.transform.localPosition.y, 0);
            //if (movingUnit.transform.position == new Vector3(endingPos.x, endingPos.y, 0));
            if (moveTimer >= 1 && allowMovement)
            {
                allowMovement = false;
                moveTimer = 1;

                GameManager.Instance.UpdateAllUnitStatBars();
                ToggleIsMovementAllowed(true);
                movingUnit.UpdatCurMovementUses(movingUnit.GetCurMovementUses() - 1);

                // Update unit look direction
                //movingUnit.UpdateUnitLookDirection();

                if (movingUnit.GetCurMovementUses() == 0)
                    StartCoroutine(AutoSwapOutOfMovementMode());

                if (movingUnit.GetCurMovementUses() < 0)
                {
                    if (!movingUnit.usedExtraMove)
                        movingUnit.usedExtraMove = true;

                    if (movingUnit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                        StartCoroutine(AutoSwapOutOfMovementModeAndLockSkills());
                }

                movingUnit.skill1OutOfRange = false;
                movingUnit.skill2OutOfRange = false;
                movingUnit.skill3OutOfRange = false;
                movingUnit.skill4OutOfRange = false;

                movingUnit.SetPositionAndParent(GetSelectedCombatSlotMove().transform);

                CheckToUnlinkCombatSlot();

                if (movingUnit.curUnitType == UnitFunctionality.UnitType.ENEMY || movingUnit.reanimated)
                {
                    //if (movingUnit.GetCurMovementUses() <= 0 && movingUnit.hasAttacked)
                        //movingUnit.StartCoroutine(movingUnit.UnitEndTurn());
                }
            }

            if (movingUnit.reanimated)
                ToggleButton(GetButtonItems(), false, true);
        }
    }

    IEnumerator AutoSwapOutOfMovementMode()
    {
        //if (movingUnit != GameManager.Instance.GetActiveUnitFunctionality())
        //    yield break;

        if (movingUnit.reanimated)
            ToggleButton(GetButtonItems(), false, true);

        yield return new WaitForSeconds(0);

        UnitFunctionality unit = movingUnit;
        // If fighter HASNT attacked, display skills tab
        if (!unit.hasAttacked)
        {
            if (unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                ToggleTabButtons("Skills");

            GameManager.Instance.isSkillsMode = true;
            UpdateAttackMovementMode(false, true, true);

            if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                StartCoroutine(unit.StartUnitTurn(false));
        }
        // If fighter HAS attacked, display items tab
        else
        {
            if (unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                ToggleTabButtons("Items");

            if (unit.curUnitType == UnitFunctionality.UnitType.PLAYER &&
                !unit.reanimated)
            {
                GameManager.Instance.isSkillsMode = false;
                UpdateAttackMovementMode(false, true, true);
            }
            else
            {
                GameManager.Instance.isSkillsMode = true;
                UpdateAttackMovementMode(false, true, true);
                StartCoroutine(unit.UnitEndTurn(true));
            }

            if (unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                ToggleTabButtons("Items");

            if (unit.reanimated)
                StartCoroutine(unit.UnitEndTurn(true));
        }

        if (movingUnit.reanimated)
            ToggleButton(GetButtonItems(), false, true);
    }

    IEnumerator AutoSwapOutOfMovementModeAndLockSkills()
    {
        if (movingUnit.reanimated)
            ToggleButton(GetButtonItems(), false, true);

        yield return new WaitForSeconds(.25f);

        ToggleAllCombatSlotOutlines();
        UnselectAllSelectedCombatSlots();
        RemoveAllCombatSelectedCombatSlots();

        GetButtonAttack().ButtonCombatAttackTab();
        //UpdateAttackMovementMode(false, true);      
        GetButtonItems().ButtonCombatItemTab();

        ToggleButton(GetButtonSkills(), false, true);
        ToggleButton(GetButtonMovement(), false, true);




        int count = 0;
        for (int i = 0; i < GameManager.Instance.activeRoomAllUnitFunctionalitys.Count; i++)
        {
            if (GameManager.Instance.activeRoomAllUnitFunctionalitys[i].curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                if (GameManager.Instance.GetActiveUnitFunctionality() == GameManager.Instance.activeRoomAllUnitFunctionalitys[i])
                {
                    if (i == 0)
                    {
                        for (int x = 0; x < OwnedLootInven.Instance.GetWornItemMainAlly().Count; x++)
                        {
                            if (OwnedLootInven.Instance.GetWornItemMainAlly()[x].GetCalculatedItemsUsesRemaining2() > 0
                                && OwnedLootInven.Instance.GetWornItemMainAlly()[x].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                            {
                                count++;
                                continue;
                            }
                        }

                        if (count == 0)
                        {
                            StartCoroutine(EndUnitTurnAfterWait(movingUnit));
                            //break;
                        }
                    }
                    else if (i == 1)
                    {
                        for (int x = 0; x < OwnedLootInven.Instance.GetWornGearSecondAlly().Count; x++)
                        {
                            if (OwnedLootInven.Instance.GetWornGearSecondAlly()[x].GetCalculatedItemsUsesRemaining2() > 0
                                && OwnedLootInven.Instance.GetWornGearSecondAlly()[x].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                            {
                                count++;
                                continue;
                            }
                        }

                        if (count == 0)
                        {
                            StartCoroutine(EndUnitTurnAfterWait(movingUnit));
                            //break;
                        }
                    }
                    else if (i == 2)
                    {
                        for (int x = 0; x < OwnedLootInven.Instance.GetWornGearThirdAlly().Count; x++)
                        {
                            if (OwnedLootInven.Instance.GetWornGearThirdAlly()[x].GetCalculatedItemsUsesRemaining2() > 0
                                && OwnedLootInven.Instance.GetWornGearThirdAlly()[x].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                            {
                                count++;
                                continue;
                            }
                        }

                        if (count == 0)
                        {
                            StartCoroutine(EndUnitTurnAfterWait(movingUnit));
                            //break;
                        }
                    }
                }
            }
        }

        if (movingUnit.reanimated)
            ToggleButton(GetButtonItems(), false, true);
    }

    public int GetRangeXToUnit(UnitFunctionality fromUnit, UnitFunctionality toUnit)
    {
        int rangeX = 0;

        if (fromUnit.GetActiveCombatSlot().GetSlotIndex().x > toUnit.GetActiveCombatSlot().GetSlotIndex().x)
        {
            rangeX = (int)fromUnit.GetActiveCombatSlot().GetSlotIndex().x - (int)toUnit.GetActiveCombatSlot().GetSlotIndex().x;
        }
        else
        {
            rangeX = (int)toUnit.GetActiveCombatSlot().GetSlotIndex().x - (int)fromUnit.GetActiveCombatSlot().GetSlotIndex().x;
        }

        int totalRange = rangeX;
        return totalRange;
    }

    public int GetRangeYToUnit(UnitFunctionality fromUnit, UnitFunctionality toUnit)
    {
        int rangeY = 0;

        if (fromUnit.GetActiveCombatSlot().GetSlotIndex().y > toUnit.GetActiveCombatSlot().GetSlotIndex().y)
        {
            rangeY = (int)fromUnit.GetActiveCombatSlot().GetSlotIndex().y - (int)toUnit.GetActiveCombatSlot().GetSlotIndex().y;
        }
        else
        {
            rangeY = (int)toUnit.GetActiveCombatSlot().GetSlotIndex().y - (int)fromUnit.GetActiveCombatSlot().GetSlotIndex().y;
        }

        int totalRange = rangeY;
        return totalRange;
    }

    private int CompareUnitRangeX(UnitFunctionality unitA, UnitFunctionality unitB)
    {
        if (GetRangeXToUnit(unitA, unitB) < GetRangeXToUnit(unitA, unitB))
            return 1;
        if (GetRangeXToUnit(unitA, unitB) > GetRangeXToUnit(unitA, unitB))
            return -1;
        else
            return 0;
    }

    private int CompareUnitRangeY(UnitFunctionality unitA, UnitFunctionality unitB)
    {
        if (GetRangeYToUnit(unitA, unitB) < GetRangeYToUnit(unitA, unitB))
            return 1;
        if (GetRangeYToUnit(unitA, unitB) > GetRangeYToUnit(unitA, unitB))
            return -1;
        else
            return 0;
    }

    public int CompareSlotRangeFromUnit(CombatSlot combatSlotA, CombatSlot combatSlotB)
    {
        if (combatSlotA.GetRangeFromActiveUnit() < combatSlotB.GetRangeFromActiveUnit())
            return 1;
        if (combatSlotA.GetRangeFromActiveUnit() > combatSlotB.GetRangeFromActiveUnit())
            return -1;
        else
            return 0;
    }

    public int CompareUnitHealth(CombatSlot combatSlotA, CombatSlot combatSlotB)
    {
        if (combatSlotA.GetLinkedUnit().GetUnitHealthPerc() < combatSlotB.GetLinkedUnit().GetUnitHealthPerc())
            return 1;
        if (combatSlotA.GetLinkedUnit().GetUnitHealthPerc() > combatSlotB.GetLinkedUnit().GetUnitHealthPerc())
            return -1;
        else
            return 0;
    }

    public List<CombatSlot> GetTargetCombatSlots()
    {
        return aimTargetCombatSlots;
    }

    public void MoveRandomly(UnitFunctionality unit, List<CombatSlot> combatSlots)
    {
        UpdateUnitMoveRange(unit);

        //unit.UnitEndTurn(true);

        bool flag = false;

        // Move randomly
        if (unit.GetCurMovementUses() > 0)
        {
            for (int i = 0; i < 35; i++)
            {
                int rand = Random.Range(0, combatSlots.Count-1);
                //Debug.Log("rand = " + rand);

                if (rand > combatSlots.Count - 1)
                {
                    unit.StartCoroutine(unit.UnitEndTurn());
                    return;
                }
                else if (combatSlots[rand])
                {
                    if (combatSlots[rand].GetAllowed())
                    {
                        if (combatSlots[rand])
                        {
                            if (combatSlots[rand].GetLinkedUnit() != null)
                            {
                                //if (i > 0)
                                //    i--;
                                continue;
                            }
                            else
                            {
                                isCombatMode = false;
                                combatSlots[rand].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                                break;
                            }
                        }
                        else
                            continue;
                    }
                }
                else
                {
                    flag = true;
                    StartCoroutine(EndUnitTurnAfterWait(unit));
                    break;
                }

            }

        }

        if (!flag)
        {
            if (unit.hasAttacked)
            {
                //StartCoroutine(EndUnitTurnAfterWait(unit));
            }
            else if (!unit.hasAttacked)
            {
                UpdateUnitAttackRange(unit);
            }
            else if (unit.GetCurMovementUses() > 0)
            {
                UpdateUnitMoveRange(unit);
            }
        }

    }

    bool moved = false;

    void SelectSlotToMove(UnitFunctionality unit, List<CombatSlot> combatSlots, string moveDirection)
    {
        moved = false;

        //isCombatMode = false;
        UpdateUnitMoveRange(unit);

        if (moveDirection == "Up")
        {
            // Move top if available
            if (!moved)
            {
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y + 1 &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            if (!moved)
            {
                // Move Up Left if available
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x - 1 &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y + 1 &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            if (!moved)
            {
                // Move Up Right if available
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x + 1 &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y + 1 &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            if (!moved)
            {
                // Move Left if available
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x - 1 &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            if (!moved)
            {
                // Move Right if available
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x + 1 &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }
            // Move random if not possible
            if (!moved)
            {
                MoveRandomly(unit, combatSlots);
                return;
            }
        }
        else if (moveDirection == "Down")
        {
            // Move down if available
            if (!moved)
            {
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y - 1 &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            if (!moved)
            {
                // Move down Left if available
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x - 1 &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y - 1 &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            if (!moved)
            {
                // Move down Right if available
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x + 1 &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y - 1 &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            if (!moved)
            {
                // Move Left if available
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x - 1 &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            if (!moved)
            {
                // Move Right if available
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x + 1 &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }
            // Move random if not possible
            if (!moved)
            {
                MoveRandomly(unit, combatSlots);
                return;
            }

        }
        else if (moveDirection == "Left")
        {
            // Move left if available
            if (!moved)
            {
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x - 1 &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            if (!moved)
            {
                // Move left up if available
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x - 1 &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y + 1 &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            if (!moved)
            {
                // Move left down if available
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x - 1 &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y - 1 &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            if (!moved)
            {
                // Move up if available
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y + 1 &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            if (!moved)
            {
                // Move down if available
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y - 1 &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }
            // Move random if not possible
            if (!moved)
            {
                MoveRandomly(unit, combatSlots);
                return;
            }

        }
        else if (moveDirection == "Right")
        {
            // Move right if available
            if (!moved)
            {
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x + 1 &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            if (!moved)
            {
                // Move Up right if available
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x + 1 &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y + 1 &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            if (!moved)
            {
                // Move down Right if available
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x + 1 &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y - 1 &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            if (!moved)
            {
                // Move up if available
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y + 1 &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            if (!moved)
            {
                // Move down if available
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y - 1 &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }
            // Move random if not possible
            if (!moved)
            {
                MoveRandomly(unit, combatSlots);
                return;
            }

        }

        else if (moveDirection == "UpLeft")
        {
            // Move up left if available
            if (!moved)
            {
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x - 1 &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y + 1 &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            // Move up if available
            if (!moved)
            {
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y + 1 &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            // Move left if available
            if (!moved)
            {
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x - 1 &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }
            // Move random if not possible
            if (!moved)
            {
                //Debug.Log("combat slots = " + combatSlots);
                //Debug.Log("combat slots = " + combatSlots.Count);
                MoveRandomly(unit, combatSlots);
                return;
            }

        }
        else if (moveDirection == "UpRight")
        {
            // Move up right if available
            if (!moved)
            {
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x + 1 &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y + 1 &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            // Move up if available
            if (!moved)
            {
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y + 1 &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            // Move right if available
            if (!moved)
            {
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x + 1 &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }
            // Move random if not possible
            if (!moved)
            {
                MoveRandomly(unit, combatSlots);
                return;
            }

        }
        else if (moveDirection == "DownLeft")
        {
            // Move down left if available
            if (!moved)
            {
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x - 1 &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y - 1 &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            // Move down if available
            if (!moved)
            {
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y - 1 &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            // Move left if available
            if (!moved)
            {
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x - 1 &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            // Move random if not possible
            if (!moved)
            {
                MoveRandomly(unit, combatSlots);
                return;
            }
        }
        else if (moveDirection == "DownRight")
        {
            // Move down right if available
            if (!moved)
            {
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x + 1 &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y - 1 &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            // Move down if available
            if (!moved)
            {
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y - 1 &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            // Move right if available
            if (!moved)
            {
                for (int i = 0; i < combatSlots.Count; i++)
                {
                    if (combatSlots[i].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x + 1 &&
                        combatSlots[i].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y &&
                        !combatSlots[i].GetLinkedUnit())
                    {
                        isCombatMode = false;
                        moved = true;
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

            // Move random if not possible
            if (!moved)
            {
                MoveRandomly(unit, combatSlots);
                return;
            }
        }
    }

    bool hold = false;
    bool chase = false;
    bool run = false;

    void UpdateActiveUnitBrain()
    {
        hold = false;
        chase = false;
        run = false;

        int distance = 15;

        if (GetTargetCombatSlots().Count >= 1)
        {
            //Debug.Log("")
            distance = GetTargetCombatSlots()[0].GetLinkedUnit().GetRangeFromUnit(GameManager.Instance.GetActiveUnitFunctionality());
        }
        
        // Make melee enemies keep their distance for their targets
        if (GameManager.Instance.GetActiveUnitFunctionality().unitData.curUnitBehaviour == UnitData.UnitBehaviour.R_AGGRESSIVE ||
            GameManager.Instance.GetActiveUnitFunctionality().unitData.curUnitBehaviour == UnitData.UnitBehaviour.R_SUPPORT)
        {
            if (distance == GameManager.Instance.GetActiveSkill().curSkillRange)
            {
                hold = true;
            }
            else if (distance > GameManager.Instance.GetActiveSkill().curSkillRange)
            {
                chase = true;
            }
            else if (distance < GameManager.Instance.GetActiveSkill().curSkillRange)
            {
                run = true;
            }
        }
        // Make melee enemies chase targets
        else if (GameManager.Instance.GetActiveUnitFunctionality().unitData.curUnitBehaviour == UnitData.UnitBehaviour.M_AGGRESSIVE ||
        GameManager.Instance.GetActiveUnitFunctionality().unitData.curUnitBehaviour == UnitData.UnitBehaviour.M_SUPPORT)
        {
            chase = true;
        }
    }


    public void PerformBotAction(UnitFunctionality unit)
    {
        // Update UI for player
        if (unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
        {
            if (unit.GetCurMovementUses() > 0)
            {
                GameManager.Instance.SetupPlayerUI();
                UpdateAttackMovementMode(true, false, false);
                OverlayUI.Instance.ToggleFighterDetailsTab(true);
            }
            else
            {
                // Send to items
                GameManager.Instance.SetupPlayerUI();
                GameManager.Instance.isSkillsMode = false;
                UpdateAttackMovementMode(false, true, true);
                OverlayUI.Instance.ToggleFighterDetailsTab(true);
                //unit.UnitEndTurn(false);
                GameManager.Instance.UpdateMainIconDetails(null, null, false);
                GameManager.Instance.ToggleAllowSelection(true);
                return;
            }
        }



        List<CombatSlot> combatSlots = new List<CombatSlot>();
        if (unit.GetCurMovementUses() > 0)
        {
            unit.UnitMove();
        }

        UpdateAttackSelection(unit);

        for (int i = 0; i < GetAllCombatSlots().Count; i++)
        {
            if (GetCombatSlot(i).GetAllowed())
            {
                combatSlots.Add(GetCombatSlot(i));
                //GetCombatSlot(i)
            }
        }
        //Debug.Log("COMBATSLOTS = " + combatSlots.Count);

        bool switched = false;

        if (isCombatMode)
        {
            // If there are no selections in current position
            if (GetTargetCombatSlots().Count == 0 || unit.hasAttacked && unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
            {
                //StartCoroutine(EndUnitTurnAfterWait(unit));
                //return;
            }
        }


        if (GetTargetCombatSlots().Count != 0)
        {
            int slotsInRange = 0;

            if (GameManager.Instance.GetActiveSkill().isSelfCast)
            {
                slotsInRange++;
            }
            else
            {
                // Loop through all targeted combat slots
                for (int i = 0; i < GetTargetCombatSlots().Count; i++)
                {
                    if (GetTargetCombatSlots()[i].GetLinkedUnit())
                    {
                        if (GetTargetCombatSlots()[i].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER &&
                            unit.curUnitType == UnitFunctionality.UnitType.ENEMY && GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE)
                        {
                            // If targeted slots are in range for current skill
                            if (GetTargetCombatSlots()[i].GetLinkedUnit().GetRangeFromUnit(unit) <= GameManager.Instance.GetActiveSkill().curSkillRange)
                            {
                                slotsInRange++;
                            }
                        }
                        else if (GetTargetCombatSlots()[i].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY &&
                            unit.curUnitType == UnitFunctionality.UnitType.ENEMY && GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT)
                        {
                            // If targeted slots are in range for current skill
                            if (GetTargetCombatSlots()[i].GetLinkedUnit().GetRangeFromUnit(unit) <= GameManager.Instance.GetActiveSkill().curSkillRange)
                            {
                                slotsInRange++;
                            }
                        }
                        else if (GetTargetCombatSlots()[i].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY &&
                             unit.curUnitType == UnitFunctionality.UnitType.PLAYER && GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE)
                        {
                            // If targeted slots are in range for current skill
                            if (GetTargetCombatSlots()[i].GetLinkedUnit().GetRangeFromUnit(unit) <= GameManager.Instance.GetActiveSkill().curSkillRange)
                            {
                                slotsInRange++;
                            }
                        }
                        else if (GetTargetCombatSlots()[i].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER &&
                             unit.curUnitType == UnitFunctionality.UnitType.PLAYER && GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT)
                        {
                            // If targeted slots are in range for current skill
                            if (GetTargetCombatSlots()[i].GetLinkedUnit().GetRangeFromUnit(unit) <= GameManager.Instance.GetActiveSkill().curSkillRange)
                            {
                                slotsInRange++;
                            }
                        }
                    }
                }
            }



            // Slots are in range, attack from here, then try move after if can.
            if (slotsInRange > 0 && !unit.hasAttacked && !unit.skillRangeIssue)
            {
                UpdateUnitAttackRange(unit);
                return;
            }
            // No slots in range, try move towards targets
            else
            {
                if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                {
                    // If unit has movement remaining, select and move to desired direction (N S E W)
                    if (unit.GetCurMovementUses() > 0)
                    {
                        if (slotsInRange > 0 && unit.hasAttacked && unit.GetCurMovementUses() <= 0)
                        {
                            unit.StartCoroutine(unit.UnitEndTurn());
                            return;
                        }

                        // Make unit choose which direction to move in, and whether to do it or not
                        UpdateActiveUnitBrain();

                        isCombatMode = false;
                        UpdateAttackMovementMode(true, false, true);
                        for (int b = 0; b < 4; b++)
                        {
                            if (switched)
                                break;

                            // If unit is to stay still, do not allow it to continue into moving, stop it?
                            if (!unit.hasAttacked)
                            {
                                if (hold && unit.unitData.curUnitBehaviour == UnitData.UnitBehaviour.R_SUPPORT || hold && unit.unitData.curUnitBehaviour == UnitData.UnitBehaviour.R_AGGRESSIVE)
                                {
                                    unit.StartCoroutine(unit.UnitEndTurn());
                                    //UpdateUnitMoveRange(unit);
                                    return;
                                }
                            }

                            // If target is self
                            if (GetTargetCombatSlots()[0].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x &&
                                GetTargetCombatSlots()[0].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y)
                            {
                                MoveRandomly(unit, GetTargetCombatSlots());
                                break;
                            }
                            // If target is top right
                            else if (GetTargetCombatSlots()[0].GetSlotIndex().x > unit.GetActiveCombatSlot().GetSlotIndex().x &&
                                GetTargetCombatSlots()[0].GetSlotIndex().y > unit.GetActiveCombatSlot().GetSlotIndex().y)
                            {
                                if (run)
                                {
                                    switched = true;
                                    SelectSlotToMove(unit, combatSlots, "DownLeft");
                                    break;
                                }
                                else if (chase)
                                {
                                    switched = true;
                                    SelectSlotToMove(unit, combatSlots, "UpRight");
                                    break;
                                }
                            }
                            // If target is bottom left
                            else if (GetTargetCombatSlots()[0].GetSlotIndex().x < unit.GetActiveCombatSlot().GetSlotIndex().x &&
                                    GetTargetCombatSlots()[0].GetSlotIndex().y < unit.GetActiveCombatSlot().GetSlotIndex().y)
                            {
                                if (run)
                                {
                                    switched = true;
                                    SelectSlotToMove(unit, combatSlots, "UpRight");
                                    break;
                                }
                                else if (chase)
                                {
                                    switched = true;
                                    SelectSlotToMove(unit, combatSlots, "DownLeft");
                                    break;
                                }
                            }
                            // If target is Top Left
                            else if (GetTargetCombatSlots()[0].GetSlotIndex().x < unit.GetActiveCombatSlot().GetSlotIndex().x &&
                                GetTargetCombatSlots()[0].GetSlotIndex().y > unit.GetActiveCombatSlot().GetSlotIndex().y)
                            {
                                if (run)
                                {
                                    switched = true;
                                    SelectSlotToMove(unit, combatSlots, "DownRight");
                                    break;
                                }
                                else if (chase)
                                {
                                    switched = true;
                                    SelectSlotToMove(unit, combatSlots, "UpLeft");
                                    break;
                                }
                            }
                            // If target is bottom right
                            else if (GetTargetCombatSlots()[0].GetSlotIndex().x > unit.GetActiveCombatSlot().GetSlotIndex().x &&
                                GetTargetCombatSlots()[0].GetSlotIndex().y < unit.GetActiveCombatSlot().GetSlotIndex().y)
                            {
                                if (run)
                                {
                                    switched = true;
                                    // Select Up/left slot to move
                                    SelectSlotToMove(unit, combatSlots, "UpLeft");
                                    break;
                                }
                                else if (chase)
                                {
                                    switched = true;
                                    // Select down/right slot to move
                                    SelectSlotToMove(unit, combatSlots, "DownRight");
                                    break;
                                }
                            }

                            // If target is directly Above
                            if (GetTargetCombatSlots()[0].GetSlotIndex().y > unit.GetActiveCombatSlot().GetSlotIndex().y)
                            {
                                // Target slot is Above the unit
                                if (run)
                                {
                                    switched = true;
                                    // Select Down slot to move
                                    SelectSlotToMove(unit, combatSlots, "Down");
                                    break;
                                }
                                else if (chase)
                                {
                                    switched = true;
                                    // Select Up slot to move
                                    SelectSlotToMove(unit, combatSlots, "Up");
                                    break;
                                }
                            }
                            // If target is directly Below
                            else if (GetTargetCombatSlots()[0].GetSlotIndex().y < unit.GetActiveCombatSlot().GetSlotIndex().y)
                            {
                                // Target slot is below the unit
                                if (run)
                                {
                                    switched = true;
                                    // Select Down slot to move
                                    SelectSlotToMove(unit, combatSlots, "Up");
                                    break;
                                }
                                else if (chase)
                                {
                                    switched = true;
                                    // Select Up slot to move
                                    SelectSlotToMove(unit, combatSlots, "Down");
                                    break;
                                }
                            }

                            // If target is directly right
                            if (GetTargetCombatSlots()[0].GetSlotIndex().x > unit.GetActiveCombatSlot().GetSlotIndex().x)
                            {
                                switched = true;

                                // Target slot is on the right of unit
                                if (run)
                                {
                                    // Select left slot to move
                                    SelectSlotToMove(unit, combatSlots, "Left");
                                    break;
                                }
                                else if (chase)
                                {
                                    // Select right slot to move
                                    SelectSlotToMove(unit, combatSlots, "Right");
                                    break;
                                }
                            }
                            // If target is directly Left
                            else if (GetTargetCombatSlots()[0].GetSlotIndex().x < unit.GetActiveCombatSlot().GetSlotIndex().x)
                            {
                                switched = true;

                                // Target slot is on the Left of unit
                                if (run)
                                {
                                    // Select Down slot to move
                                    SelectSlotToMove(unit, combatSlots, "Right");
                                    break;
                                }
                                else if (chase)
                                {
                                    // Select Up slot to move
                                    SelectSlotToMove(unit, combatSlots, "Left");
                                    break;
                                }
                            }
           

                            if (hold && unit.hasAttacked)
                            {
                                unit.StartCoroutine(unit.UnitEndTurn());
                                //UpdateUnitMoveRange(unit);
                                return;
                            }
                        }

                        switched = false;
                    }
                }
                else
                {
                    UpdateAttackMovementMode(true, false, false); // << ??
                }
            }
            ToggleIsMovementAllowed(true);

            if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY)
                UpdateUnitMoveRange(GameManager.Instance.GetActiveUnitFunctionality());

            if (isCombatMode)
            {
                if (unit.skillRangeIssue && unit.curUnitType == UnitFunctionality.UnitType.ENEMY &&
                    unit.GetCurMovementUses() <= 0)
                {
                    unit.StartCoroutine(unit.UnitEndTurn());
                    return;
                }
            }

            if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY &&
                unit.hasAttacked && slotsInRange > 0 && !allowMovement)
            {
                unit.StartCoroutine(unit.UnitEndTurn());
                return;
            }
        }
        else if (unit.hasAttacked)
        {
            unit.StartCoroutine(unit.UnitEndTurn());
        }
    }

    public void MoveUnitToNewSlot(UnitFunctionality unit)
    {
        // Update unit to look in direction of movement
        // Moving right
        if (GetSelectedCombatSlotMove().GetSlotIndex().x > unit.GetActiveCombatSlot().GetSlotIndex().x)
        {
            unit.UpdateUnitLookDirection(false);
        }

        // Moving left
        else if (GetSelectedCombatSlotMove().GetSlotIndex().x < unit.GetActiveCombatSlot().GetSlotIndex().x)
        {
            unit.UpdateUnitLookDirection(true);
        }

        unit.GetActiveCombatSlot().UpdateLinkedUnit(null);
        ToggleIsMovementAllowed(false);
        movingUnit = unit;

        startingPos = unit.gameObject.transform.position;
        endingPos = GetSelectedCombatSlotMove().transform.position;

        startingPos = new Vector3(startingPos.x, startingPos.y, 0);
        endingPos = new Vector3(endingPos.x, endingPos.y, 0);

        moveTimer = 0;
        allowMovement = true;

        unit.UpdateActiveCombatSlot(GetSelectedCombatSlotMove());
        GetSelectedCombatSlotMove().UpdateLinkedUnit(unit);



        UnselectAllSelectedCombatSlots();
        GetSelectedCombatSlotMove().ToggleSlotSelected(true);
        GetSelectedCombatSlotMove().ToggleSlotAllowed(true);
    }

    public void UpdateSelectedCombatSlotMove(CombatSlot slot)
    {
        selectedCombatSlotMove = slot;
    }

    public CombatSlot GetSelectedCombatSlotMove()
    {
        return selectedCombatSlotMove;
    }

    public void UpdateUnitMoveRange(UnitFunctionality unit)
    {
        Vector2 unitCombatIndex = Vector2.zero;

        if (unit.GetActiveCombatSlot())
        {
            CombatSlot unitCombatSlot = null;
            unitCombatSlot = unit.GetActiveCombatSlot();

            unitCombatIndex = unitCombatSlot.GetSlotIndex();
        }

        int unitMovementRange = unit.GetCurMovementUses();
        //unitMovementRange--;

        if (unitMovementRange == 0 && unit.hasAttacked)
            return;
        else if (unitMovementRange <= -1 && unit.hasAttacked)
            return;

        UnselectAllSelectedCombatSlots();

        GameManager.Instance.ToggleAllowSelection(true);

        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            for (int x = 1; x < 2; x++)
            {
                CombatSlot combatSlot = null;

                // Select left combat slots
                if (GetCombatSlot(new Vector2(unitCombatIndex.x - x, unitCombatIndex.y)) != null
                    && allCombatSlots[i].GetSlotIndex() == new Vector2(unitCombatIndex.x - x, unitCombatIndex.y)
                    && GetCombatSlot(new Vector2(unitCombatIndex.x - x, unitCombatIndex.y)).GetLinkedUnit() == null)
                {
                    combatSlot = GetCombatSlot(new Vector2(unitCombatIndex.x - x, unitCombatIndex.y));
                }
                // Select right combat slots
                else if (GetCombatSlot(new Vector2(unitCombatIndex.x + x, unitCombatIndex.y)) != null
                    && allCombatSlots[i].GetSlotIndex() == new Vector2(unitCombatIndex.x + x, unitCombatIndex.y)
                    && GetCombatSlot(new Vector2(unitCombatIndex.x + x, unitCombatIndex.y)).GetLinkedUnit() == null)
                {
                    combatSlot = GetCombatSlot(new Vector2(unitCombatIndex.x + x, unitCombatIndex.y));
                }
                // Select Up combat slots
                else if (GetCombatSlot(new Vector2(unitCombatIndex.x, unitCombatIndex.y + x)) != null
                    && allCombatSlots[i].GetSlotIndex() == new Vector2(unitCombatIndex.x, unitCombatIndex.y + x)
                    && GetCombatSlot(new Vector2(unitCombatIndex.x, unitCombatIndex.y + x)).GetLinkedUnit() == null)
                {
                    combatSlot = GetCombatSlot(new Vector2(unitCombatIndex.x, unitCombatIndex.y + x));
                }
                // Select Down combat slots
                else if (GetCombatSlot(new Vector2(unitCombatIndex.x, unitCombatIndex.y - x)) != null
                    && allCombatSlots[i].GetSlotIndex() == new Vector2(unitCombatIndex.x, unitCombatIndex.y - x)
                    && GetCombatSlot(new Vector2(unitCombatIndex.x, unitCombatIndex.y - x)).GetLinkedUnit() == null)
                {
                    combatSlot = GetCombatSlot(new Vector2(unitCombatIndex.x, unitCombatIndex.y - x));
                }

                // Select Up Left combat slots
                if (GetCombatSlot(new Vector2(unitCombatIndex.x - x, unitCombatIndex.y + x)) != null
                    && allCombatSlots[i].GetSlotIndex() == new Vector2(unitCombatIndex.x - x, unitCombatIndex.y + x)
                    && GetCombatSlot(new Vector2(unitCombatIndex.x - x, unitCombatIndex.y + x)).GetLinkedUnit() == null)
                {
                    combatSlot = GetCombatSlot(new Vector2(unitCombatIndex.x - x, unitCombatIndex.y + x));
                }
                // Select Down Left combat slots
                else if (GetCombatSlot(new Vector2(unitCombatIndex.x - x, unitCombatIndex.y - x)) != null
                    && allCombatSlots[i].GetSlotIndex() == new Vector2(unitCombatIndex.x - x, unitCombatIndex.y - x)
                    && GetCombatSlot(new Vector2(unitCombatIndex.x - x, unitCombatIndex.y - x)).GetLinkedUnit() == null)
                {
                    combatSlot = GetCombatSlot(new Vector2(unitCombatIndex.x - x, unitCombatIndex.y - x));
                }
                // Select Up Right combat slots
                else if (GetCombatSlot(new Vector2(unitCombatIndex.x + x, unitCombatIndex.y + x)) != null
                    && allCombatSlots[i].GetSlotIndex() == new Vector2(unitCombatIndex.x + x, unitCombatIndex.y + x)
                    && GetCombatSlot(new Vector2(unitCombatIndex.x + x, unitCombatIndex.y + x)).GetLinkedUnit() == null)
                {
                    combatSlot = GetCombatSlot(new Vector2(unitCombatIndex.x + x, unitCombatIndex.y + x));
                }
                // Select Down Right combat slots
                else if (GetCombatSlot(new Vector2(unitCombatIndex.x + x, unitCombatIndex.y - x)) != null
                    && allCombatSlots[i].GetSlotIndex() == new Vector2(unitCombatIndex.x + x, unitCombatIndex.y - x)
                    && GetCombatSlot(new Vector2(unitCombatIndex.x + x, unitCombatIndex.y - x)).GetLinkedUnit() == null)
                {
                    combatSlot = GetCombatSlot(new Vector2(unitCombatIndex.x + x, unitCombatIndex.y - x));
                }

                if (combatSlot != null)
                {
                    combatSlot.ToggleSlotAllowed(true);
                }
            }
        }
        // Toggle Combat unit slot available around them for movement

    }

    public void ToggleAllSlotsAllowedOff()
    {
        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            if (allCombatSlots[i].GetAllowed())
            {
                allCombatSlots[i].GetAnimator().SetBool("CombatAllowed", false);
                allCombatSlots[i].GetAnimator().StopPlayback();


                allCombatSlots[i].GetAnimator().SetBool("CombatAllowed", true);
            }
        }
    }
    public void UpdateUnitAttackRange(UnitFunctionality unit)
    {
        ToggleAllCombatSlotOutlines();

        if (GameManager.Instance.GetActiveUnitFunctionality().hasAttacked && unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
        {
            if (unit.GetCurMovementUses() > 0)
            {
                isCombatMode = false;
                UpdateUnitMoveRange(unit);
                return;
            }
            else
            {
                //StartCoroutine(EndUnitTurnAfterWait(unit));
                //return;
            }
        }

        UpdateAttackSelection(unit);

        Vector2 unitCombatIndex = Vector2.zero;

        if (unit.GetActiveCombatSlot())
        {
            CombatSlot unitCombatSlot = null;
            unitCombatSlot = unit.GetActiveCombatSlot();

            unitCombatIndex = unitCombatSlot.GetSlotIndex();
        }

        UnselectAllSelectedCombatSlots();


        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            CombatSlot combatSlot = null;

            int xDiff = 0;
            int yDiff = 0;

            if (allCombatSlots[i].GetSlotIndex().x > unit.GetActiveCombatSlot().GetSlotIndex().x)
                xDiff = (int)allCombatSlots[i].GetSlotIndex().x - (int)unit.GetActiveCombatSlot().GetSlotIndex().x;
            else
                xDiff = (int)unit.GetActiveCombatSlot().GetSlotIndex().x - (int)allCombatSlots[i].GetSlotIndex().x;

            if (allCombatSlots[i].GetSlotIndex().y > unit.GetActiveCombatSlot().GetSlotIndex().y)
                yDiff = (int)allCombatSlots[i].GetSlotIndex().y - (int)unit.GetActiveCombatSlot().GetSlotIndex().y;
            else
                yDiff = (int)unit.GetActiveCombatSlot().GetSlotIndex().y - (int)allCombatSlots[i].GetSlotIndex().y;

            int range = 0;
            // Skill mode
            if (GameManager.Instance.isSkillsMode)
            {
                if (GameManager.Instance.GetActiveSkill())
                    range = GameManager.Instance.GetActiveSkill().curSkillRange;
            }
            else
            {
                if (GameManager.Instance.GetActiveItem())
                    range = GameManager.Instance.GetActiveItem().range;
            }

            if (xDiff <= range &&
                yDiff <= range)
            {
                // If combat slot should be an allowed slot, make it
                if (xDiff <= range &&
                    yDiff <= range)
                {
                    combatSlot = allCombatSlots[i];
                    combatSlot.ToggleSlotAllowed(true);
                }

                if (GameManager.Instance.isSkillsMode && GameManager.Instance.GetActiveSkill())
                {
                    // If combat slot is an ignored slot, un allow it
                    if (GameManager.Instance.GetActiveSkill().skillIgnoreRange > 0)
                    {
                        if (xDiff <= GameManager.Instance.GetActiveSkill().skillIgnoreRange &&
                            yDiff <= GameManager.Instance.GetActiveSkill().skillIgnoreRange)
                        {
                            combatSlot = allCombatSlots[i];
                            combatSlot.ToggleSlotAllowed(false);
                        }
                    }

                    // Target self slot if skill can target self slot
                    if (GameManager.Instance.GetActiveSkill().canTargetSelf)
                    {
                        if (allCombatSlots[i].GetSlotIndex() == unitCombatIndex)
                        {
                            combatSlot = allCombatSlots[i];
                            combatSlot.ToggleSlotAllowed(true);
                        }
                    }
                }
                else if (!GameManager.Instance.isSkillsMode && GameManager.Instance.GetActiveItem())
                {
                    // Target self slot if skill can target self slot
                    if (GameManager.Instance.GetActiveItem().canTargetSelf)
                    {
                        if (allCombatSlots[i].GetSlotIndex() == unitCombatIndex)
                        {
                            combatSlot = allCombatSlots[i];
                            combatSlot.ToggleSlotAllowed(true);
                        }
                    }
                }
            }
        }

        UpdateUnitAttackHitArea(unit);
    }

    public bool done = false;

    public List<CombatSlot> ShuffleList(List<CombatSlot> slots)
    {
        for (int i = slots.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            CombatSlot temp = slots[i];
            slots[i] = slots[j];
            slots[j] = temp;
        }
        return slots;
    }

    public void ToggleAllCombatSlotOutlines()
    {
        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            allCombatSlots[i].ToggleSelectBorder(allCombatSlots[i].GetRightSelectBorder(), false);
            allCombatSlots[i].ToggleSelectBorder(allCombatSlots[i].GetLeftSelectBorder(), false);
            allCombatSlots[i].ToggleSelectBorder(allCombatSlots[i].GetTopSelectBorder(), false);
            allCombatSlots[i].ToggleSelectBorder(allCombatSlots[i].GetBottomSelectBorder(), false);
        }
    }

    void UpdateCombatSlotOutlines(List<CombatSlot> selectedCombatSlots)
    {
        ToggleAllCombatSlotOutlines();

        for (int b = 0; b < selectedCombatSlots.Count; b++)
        {
            selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetRightSelectBorder(), false);
            selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetLeftSelectBorder(), false);
            selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetTopSelectBorder(), false);
            selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetBottomSelectBorder(), false);

            // Right side
            if (GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x + 1, selectedCombatSlots[b].GetSlotIndex().y)))
            {
                if (!GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x + 1, selectedCombatSlots[b].GetSlotIndex().y)).combatSelected)
                    selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetRightSelectBorder(), true);
                else
                    selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetRightSelectBorder(), false);
            }
            else
                selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetRightSelectBorder(), true);

            // Left side
            if (GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x - 1, selectedCombatSlots[b].GetSlotIndex().y)))
            {
                if (!GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x - 1, selectedCombatSlots[b].GetSlotIndex().y)).combatSelected)
                    selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetLeftSelectBorder(), true);
                else
                    selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetLeftSelectBorder(), false);
            }
            else
                selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetLeftSelectBorder(), true);

            // Up side
            if (GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x, selectedCombatSlots[b].GetSlotIndex().y + 1)))
            {
                if (!GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x, selectedCombatSlots[b].GetSlotIndex().y + 1)).combatSelected)
                    selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetTopSelectBorder(), true);
                else
                    selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetTopSelectBorder(), false);
            }
            else
                selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetTopSelectBorder(), true);

            // Down side
            if (GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x, selectedCombatSlots[b].GetSlotIndex().y - 1)))
            {
                if (!GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x, selectedCombatSlots[b].GetSlotIndex().y - 1)).combatSelected)
                    selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetBottomSelectBorder(), true);
                else
                    selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetBottomSelectBorder(), false);
            }
            else
                selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetBottomSelectBorder(), true);
        }


    }

    public void UpdateUnitAttackHitArea(UnitFunctionality unit, CombatSlot targetedSlot = null)
    {
        done = false;

        List<CombatSlot> allowedCombatSlots = new List<CombatSlot>();
        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            if (allCombatSlots[i].GetAllowed())
            {
                allowedCombatSlots.Add(allCombatSlots[i]);
            }
        }

        // ??
        if (GameManager.Instance.isSkillsMode && GameManager.Instance.GetActiveSkill())
        {
            if (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE)
            {
                allowedCombatSlots = ShuffleList(allowedCombatSlots);
            }
        }

        List<CombatSlot> combatSelectedCombatSlots = new List<CombatSlot>();

        int unitsTargeted = 0;

        int selectionsAllowed = 0;
        if (GameManager.Instance.isSkillsMode)
        {
            if (GameManager.Instance.GetActiveSkill())
            {
                if (GameManager.Instance.GetActiveSkill().skillRangeHitAreas.Count != 0)
                    selectionsAllowed = GameManager.Instance.GetActiveSkill().skillRangeHitAreas.Count;
            }
        }
        else
        {
            if (GameManager.Instance.GetActiveItem())
                selectionsAllowed = GameManager.Instance.GetActiveItem().itemRangeHitAreas.Count;
        }

        List<CombatSlot> selectedCombatSlots = new List<CombatSlot>();
        SkillData activeSkill = GameManager.Instance.GetActiveSkill();
        ItemPiece activeItem = GameManager.Instance.GetActiveItem();

        if (activeItem == null && !GameManager.Instance.isSkillsMode)
        {
            ToggleAllCombatSlotOutlines();
            UnselectAllSelectedCombatSlots();
            GameManager.Instance.UpdateMainIconDetails(null, null);
            OverlayUI.Instance.UpdateItemUI("", "", 0, 0, Vector2.zero, TeamItemsManager.Instance.clearSlotSprite);
            return;
        }
        else if (GameManager.Instance.GetActiveItemSlot() && !GameManager.Instance.isSkillsMode)
        {
            if (GameManager.Instance.GetActiveItemSlot().GetCalculatedItemsUsesRemaining2() <= 0)
            {
                ToggleAllCombatSlotOutlines();
                UnselectAllSelectedCombatSlots();
                GameManager.Instance.UpdateMainIconDetails(null, null);
                OverlayUI.Instance.UpdateItemUI("", "", 0, 0, Vector2.zero, TeamItemsManager.Instance.clearSlotSprite);
                return;
            }
        }
        int selectedSlots = 0;

        if (targetedSlot != null)
        {
            if (GameManager.Instance.isSkillsMode)
            {
                // Select a group of combat slots based on skill
                for (int b = 0; b < activeSkill.skillRangeHitAreas.Count; b++)
                {
                    if (GetCombatSlot(new Vector2((int)targetedSlot.GetSlotIndex().x + (int)activeSkill.skillRangeHitAreas[b].x,
                        (int)targetedSlot.GetSlotIndex().y + (int)activeSkill.skillRangeHitAreas[b].y)))
                    {
                        if (GetCombatSlot(new Vector2((int)targetedSlot.GetSlotIndex().x + (int)activeSkill.skillRangeHitAreas[b].x,
                            (int)targetedSlot.GetSlotIndex().y + (int)activeSkill.skillRangeHitAreas[b].y)).GetAllowed())
                        {
                            // Target slot/unit
                            selectedCombatSlots.Add(GetCombatSlot(new Vector2((int)targetedSlot.GetSlotIndex().x + (int)activeSkill.skillRangeHitAreas[b].x,
                            (int)targetedSlot.GetSlotIndex().y + (int)activeSkill.skillRangeHitAreas[b].y)));
                        }
                    }
                    else if (GetCombatSlot(new Vector2((int)targetedSlot.GetSlotIndex().x - (int)activeSkill.skillRangeHitAreas[b].x,
                        (int)targetedSlot.GetSlotIndex().y + (int)activeSkill.skillRangeHitAreas[b].y)))
                    {
                        if (GetCombatSlot(new Vector2((int)targetedSlot.GetSlotIndex().x - (int)activeSkill.skillRangeHitAreas[b].x,
                            (int)targetedSlot.GetSlotIndex().y + (int)activeSkill.skillRangeHitAreas[b].y)).GetAllowed())
                        {
                            // Target slot/unit
                            selectedCombatSlots.Add(GetCombatSlot(new Vector2((int)targetedSlot.GetSlotIndex().x - (int)activeSkill.skillRangeHitAreas[b].x,
                            (int)targetedSlot.GetSlotIndex().y + (int)activeSkill.skillRangeHitAreas[b].y)));
                        }
                    }
                    else if (GetCombatSlot(new Vector2((int)targetedSlot.GetSlotIndex().x - (int)activeSkill.skillRangeHitAreas[b].x,
                        (int)targetedSlot.GetSlotIndex().y - (int)activeSkill.skillRangeHitAreas[b].y)))
                    {
                        if (GetCombatSlot(new Vector2((int)targetedSlot.GetSlotIndex().x - (int)activeSkill.skillRangeHitAreas[b].x,
                            (int)targetedSlot.GetSlotIndex().y - (int)activeSkill.skillRangeHitAreas[b].y)).GetAllowed())
                        {
                            // Target slot/unit
                            selectedCombatSlots.Add(GetCombatSlot(new Vector2((int)targetedSlot.GetSlotIndex().x - (int)activeSkill.skillRangeHitAreas[b].x,
                            (int)targetedSlot.GetSlotIndex().y - (int)activeSkill.skillRangeHitAreas[b].y)));
                        }
                    }
                }

                if (targetedSlot.GetLinkedUnit())
                {

                }
                else
                {
                    if (targetedSlot.GetFallenUnits().Count > 0)
                    {
                        if (activeSkill.curskillSelectionAliveType == SkillData.SkillSelectionAliveType.DEAD)
                        {
                            GameManager.Instance.AddUnitsSelected(targetedSlot.GetFallenUnits()[0]);
                            targetedSlot.GetFallenUnits()[0].ToggleSelected(true);
                        }
                    }
                }
                for (int i = 0; i < selectedCombatSlots.Count; i++)
                {


                    if (selectedCombatSlots[i].GetLinkedUnit())
                    {
                        // Necro skill 2
                        if (activeSkill.curSkillType == SkillData.SkillType.SUPPORT && activeSkill.curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.ENEMIES &&
                            unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                        {
                            if (selectedCombatSlots[i].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER)
                                continue;
                        }
                        else if (activeSkill.curSkillType == SkillData.SkillType.OFFENSE && activeSkill.curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.ENEMIES &&
                            unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                        {
                            if (selectedCombatSlots[i].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY)
                                continue;
                        }
                        else if (activeSkill.curSkillType == SkillData.SkillType.OFFENSE && activeSkill.curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.ENEMIES &&
                            unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                        {
                            if (selectedCombatSlots[i].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER)
                                continue;
                        }
                        else if (activeSkill.curSkillType == SkillData.SkillType.SUPPORT && activeSkill.curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.PLAYERS &&
                            unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                        {
                            if (selectedCombatSlots[i].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY)
                                continue;
                        }
                        else if (activeSkill.curSkillType == SkillData.SkillType.SUPPORT && activeSkill.curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.PLAYERS &&
                            unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                        {
                            if (selectedCombatSlots[i].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER)
                                continue;
                        }
                    }

                    combatSelectedCombatSlots.Add(selectedCombatSlots[i]);
                    selectedCombatSlots[i].ToggleCombatSelected(true);

                    if (selectedCombatSlots[i].GetLinkedUnit())
                    {
                        selectedCombatSlots[i].GetLinkedUnit().ToggleSelected(true);
                        GameManager.Instance.AddUnitsSelected(selectedCombatSlots[i].GetLinkedUnit());
                    }

                }

                unitsTargeted++;

                // Do combat slot outlines
                UpdateCombatSlotOutlines(combatSelectedCombatSlots);
            }
            // Items mode
            else
            {
                // Select a group of combat slots based on skill
                for (int b = 0; b < activeItem.itemRangeHitAreas.Count; b++)
                {
                    if (GetCombatSlot(new Vector2((int)targetedSlot.GetSlotIndex().x + (int)activeItem.itemRangeHitAreas[b].x,
                        (int)targetedSlot.GetSlotIndex().y + (int)activeItem.itemRangeHitAreas[b].y)))
                    {
                        if (GetCombatSlot(new Vector2((int)targetedSlot.GetSlotIndex().x + (int)activeItem.itemRangeHitAreas[b].x,
                            (int)targetedSlot.GetSlotIndex().y + (int)activeItem.itemRangeHitAreas[b].y)).GetAllowed())
                        {
                            // Target slot/unit
                            selectedCombatSlots.Add(GetCombatSlot(new Vector2((int)targetedSlot.GetSlotIndex().x + (int)activeItem.itemRangeHitAreas[b].x,
                            (int)targetedSlot.GetSlotIndex().y + (int)activeItem.itemRangeHitAreas[b].y)));
                        }
                    }
                    else if (GetCombatSlot(new Vector2((int)targetedSlot.GetSlotIndex().x - (int)activeItem.itemRangeHitAreas[b].x,
                        (int)targetedSlot.GetSlotIndex().y + (int)activeItem.itemRangeHitAreas[b].y)))
                    {
                        if (GetCombatSlot(new Vector2((int)targetedSlot.GetSlotIndex().x - (int)activeItem.itemRangeHitAreas[b].x,
                            (int)targetedSlot.GetSlotIndex().y + (int)activeItem.itemRangeHitAreas[b].y)).GetAllowed())
                        {
                            // Target slot/unit
                            selectedCombatSlots.Add(GetCombatSlot(new Vector2((int)targetedSlot.GetSlotIndex().x - (int)activeItem.itemRangeHitAreas[b].x,
                            (int)targetedSlot.GetSlotIndex().y + (int)activeItem.itemRangeHitAreas[b].y)));
                        }
                    }
                    else if (GetCombatSlot(new Vector2((int)targetedSlot.GetSlotIndex().x - (int)activeItem.itemRangeHitAreas[b].x,
                        (int)targetedSlot.GetSlotIndex().y - (int)activeItem.itemRangeHitAreas[b].y)))
                    {
                        if (GetCombatSlot(new Vector2((int)targetedSlot.GetSlotIndex().x - (int)activeItem.itemRangeHitAreas[b].x,
                            (int)targetedSlot.GetSlotIndex().y - (int)activeItem.itemRangeHitAreas[b].y)).GetAllowed())
                        {
                            // Target slot/unit
                            selectedCombatSlots.Add(GetCombatSlot(new Vector2((int)targetedSlot.GetSlotIndex().x - (int)activeItem.itemRangeHitAreas[b].x,
                            (int)targetedSlot.GetSlotIndex().y - (int)activeItem.itemRangeHitAreas[b].y)));
                        }
                    }
                }

                if (targetedSlot.GetLinkedUnit())
                {

                }
                else
                {
                    if (targetedSlot.GetFallenUnits().Count > 0)
                    {
                        if (activeItem.curTargetType == ItemPiece.TargetType.DEAD)
                        {
                            GameManager.Instance.AddUnitsSelected(targetedSlot.GetFallenUnits()[0]);
                            targetedSlot.GetFallenUnits()[0].ToggleSelected(true);
                        }
                    }
                }
                for (int i = 0; i < selectedCombatSlots.Count; i++)
                {
                    combatSelectedCombatSlots.Add(selectedCombatSlots[i]);
                    selectedCombatSlots[i].ToggleCombatSelected(true);

                    if (selectedCombatSlots[i].GetLinkedUnit())
                    {
                        // Necro skill 2
                        if (activeItem.curItemType == ItemPiece.ItemType.SUPPORT && activeItem.curSelectionType == ItemPiece.SelectionType.ENEMIES &&
                            unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                        {
                            if (selectedCombatSlots[i].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER)
                                continue;
                        }
                        else if (activeItem.curItemType == ItemPiece.ItemType.OFFENSE && activeItem.curSelectionType == ItemPiece.SelectionType.ENEMIES &&
                            unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                        {
                            if (selectedCombatSlots[i].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY)
                                continue;
                        }
                        else if (activeItem.curItemType == ItemPiece.ItemType.OFFENSE && activeItem.curSelectionType == ItemPiece.SelectionType.ENEMIES &&
                            unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                        {
                            if (selectedCombatSlots[i].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER)
                                continue;
                        }
                        else if (activeItem.curItemType == ItemPiece.ItemType.SUPPORT && activeItem.curSelectionType == ItemPiece.SelectionType.ALLIES &&
                            unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                        {
                            if (selectedCombatSlots[i].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY)
                                continue;
                        }

                        selectedCombatSlots[i].GetLinkedUnit().ToggleSelected(true);
                        GameManager.Instance.AddUnitsSelected(selectedCombatSlots[i].GetLinkedUnit());
                    }
                }

                unitsTargeted++;

                // Do combat slot outlines
                UpdateCombatSlotOutlines(combatSelectedCombatSlots);
            }
        }
        else
        {
            // Skills mode
            if (GameManager.Instance.isSkillsMode)
            {
                if (GameManager.Instance.GetActiveSkill())
                {
                    for (int i = 0; i < GameManager.Instance.GetActiveSkill().skillAreaHitCount; i++)
                    {
                        if (!GameManager.Instance.GetActiveSkill().attackAllSelected)
                        {
                            int targetCount = 0;
                            for (int l = 0; l < allowedCombatSlots.Count; l++)
                            {
                                if (GameManager.Instance.GetActiveSkill().skillRangeHitArea == Vector2.one)
                                {
                                    UnitFunctionality targetunit = null;

                                    if (GameManager.Instance.GetActiveSkill().curskillSelectionAliveType == SkillData.SkillSelectionAliveType.DEAD 
                                        && allowedCombatSlots[l].GetFallenUnits().Count > 0)
                                        targetunit = allowedCombatSlots[l].GetFallenUnits()[0];
                                    else
                                    {
                                        if (allowedCombatSlots[l].GetLinkedUnit())
                                            targetunit = allowedCombatSlots[l].GetLinkedUnit();
                                    }



                                    if (targetunit != null)
                                    {
                                        if (unitsTargeted >= GameManager.Instance.GetActiveSkill().skillAreaHitCount)
                                            break;

                                        if (allowedCombatSlots[l].GetFallenUnits().Count > 0 && activeSkill.curskillSelectionAliveType == SkillData.SkillSelectionAliveType.DEAD)
                                        {
                                            if (activeSkill.curskillSelectionAliveType == SkillData.SkillSelectionAliveType.DEAD && allowedCombatSlots[l].GetFallenUnits()[0])
                                            {
                                                combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                                allowedCombatSlots[l].ToggleCombatSelected(true);
                                                targetunit.ToggleSelected(true);
                                                GameManager.Instance.AddUnitsSelected(targetunit);
                                                unitsTargeted++;
                                                UpdateCombatSlotOutlines(combatSelectedCombatSlots);
                                                continue;
                                            }
                                        }

                                        if (allowedCombatSlots[l].GetFallenUnits().Count == 0 && targetunit == GetTargetCombatSlots()[targetCount].GetLinkedUnit() 
                                            && activeSkill.curskillSelectionAliveType == SkillData.SkillSelectionAliveType.ALIVE)
                                        {
                                            targetCount++;
                                            combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                            allowedCombatSlots[l].ToggleCombatSelected(true);
                                            targetunit.ToggleSelected(true);
                                            GameManager.Instance.AddUnitsSelected(targetunit);
                                            unitsTargeted++;
                                            UpdateCombatSlotOutlines(combatSelectedCombatSlots);
                                            continue;
                                        }
                                    }

                                    if (targetunit)
                                    {
                                        if (unitsTargeted >= GameManager.Instance.GetActiveSkill().skillAreaHitCount)
                                            break;

                                        if (targetunit.isDead && activeSkill.curskillSelectionAliveType == SkillData.SkillSelectionAliveType.ALIVE)
                                            continue;

                                        if (targetunit == unit && GameManager.Instance.GetActiveSkill().isSelfCast)
                                        {
                                            combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                            allowedCombatSlots[l].ToggleCombatSelected(true);
                                            targetunit.ToggleSelected(true);
                                            GameManager.Instance.AddUnitsSelected(targetunit);
                                            unitsTargeted++;
                                        }

                                        UpdateCombatSlotOutlines(combatSelectedCombatSlots);
                                    }
                                }
                                else
                                {
                                    if (allowedCombatSlots[l].GetFallenUnits().Count > 0 && activeSkill.curskillSelectionAliveType == SkillData.SkillSelectionAliveType.DEAD)
                                    {
                                        if (allowedCombatSlots[l].GetFallenUnits()[0])
                                        {
                                            if (!allowedCombatSlots[l].GetFallenUnits()[0].reanimated)
                                            {
                                                combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                                allowedCombatSlots[l].GetFallenUnits()[0].ToggleSelected(true);
                                                allowedCombatSlots[l].ToggleCombatSelected(true);
                                                GameManager.Instance.AddUnitsSelected(allowedCombatSlots[l].GetFallenUnits()[0]);
                                                unitsTargeted++;
                                                continue;
                                            }
                                            else
                                                continue;
                                        }
                                        else
                                            continue;
                                    }

                                    // Ensure skills cant split attack (bot ai)
                                    if (selectedSlots < GameManager.Instance.GetActiveSkill().skillRangeHitAreas.Count)
                                    {
                                        // Select a group of combat slots based on skill
                                        for (int b = 0; b < activeSkill.skillRangeHitAreas.Count; b++)
                                        {
                                            if (allowedCombatSlots[l].GetLinkedUnit())
                                            {
                                                if (allowedCombatSlots[l].GetLinkedUnit().isDead)
                                                    continue;

                                                if (allowedCombatSlots[l].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY &&
                                                    unit.curUnitType == UnitFunctionality.UnitType.PLAYER && activeSkill.curSkillType == SkillData.SkillType.OFFENSE && !activeSkill.isSpecial ||
                                                    allowedCombatSlots[l].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY &&
                                                    unit.curUnitType == UnitFunctionality.UnitType.ENEMY && activeSkill.curSkillType == SkillData.SkillType.SUPPORT && !activeSkill.isSpecial ||
                                                    allowedCombatSlots[l].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY &&
                                                    unit.curUnitType == UnitFunctionality.UnitType.PLAYER && activeSkill.curSkillType == SkillData.SkillType.SUPPORT && activeSkill.isSpecial)
                                                {
                                                    if (GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x + (int)activeSkill.skillRangeHitAreas[b].x,
                                                        (int)allowedCombatSlots[l].GetSlotIndex().y + (int)activeSkill.skillRangeHitAreas[b].y)))
                                                    {
                                                        if (GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x + (int)activeSkill.skillRangeHitAreas[b].x,
                                                            (int)allowedCombatSlots[l].GetSlotIndex().y + (int)activeSkill.skillRangeHitAreas[b].y)).GetAllowed())
                                                        {
                                                            // Target slot/unit
                                                            selectedCombatSlots.Add(GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x + (int)activeSkill.skillRangeHitAreas[b].x,
                                                            (int)allowedCombatSlots[l].GetSlotIndex().y + (int)activeSkill.skillRangeHitAreas[b].y)));
                                                            selectedSlots++;
                                                            unitsTargeted++;
                                                        }
                                                    }
                                                }
                                                if (allowedCombatSlots[l].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER &&
                                                    unit.curUnitType == UnitFunctionality.UnitType.ENEMY && activeSkill.curSkillType == SkillData.SkillType.OFFENSE && !activeSkill.isSpecial ||
                                                    allowedCombatSlots[l].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER &&
                                                    unit.curUnitType == UnitFunctionality.UnitType.PLAYER && activeSkill.curSkillType == SkillData.SkillType.SUPPORT && !activeSkill.isSpecial)
                                                {
                                                    if (GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x + (int)activeSkill.skillRangeHitAreas[b].x,
                                                        (int)allowedCombatSlots[l].GetSlotIndex().y + (int)activeSkill.skillRangeHitAreas[b].y)))
                                                    {
                                                        if (GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x + (int)activeSkill.skillRangeHitAreas[b].x,
                                                            (int)allowedCombatSlots[l].GetSlotIndex().y + (int)activeSkill.skillRangeHitAreas[b].y)).GetAllowed())
                                                        {
                                                            // Target slot/unit
                                                            selectedCombatSlots.Add(GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x + (int)activeSkill.skillRangeHitAreas[b].x,
                                                            (int)allowedCombatSlots[l].GetSlotIndex().y + (int)activeSkill.skillRangeHitAreas[b].y)));
                                                            selectedSlots++;
                                                            unitsTargeted++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }


                                    if (selectedCombatSlots.Count > selectionsAllowed)
                                        continue;
                                    else
                                    {
                                        if (selectedCombatSlots.Count != 0)
                                        {
                                            for (int b = 0; b < selectedCombatSlots.Count; b++)
                                            {
                                                if (allowedCombatSlots[b].GetLinkedUnit())
                                                {
                                                    if (allowedCombatSlots[b].GetLinkedUnit().isDead)
                                                        continue;
                                                }

                                                if (selectedCombatSlots[b].GetLinkedUnit())
                                                {
                                                    if (activeSkill.curSkillType == SkillData.SkillType.OFFENSE && activeSkill.curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.ENEMIES &&
                                                        unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                                                    {
                                                        if (selectedCombatSlots[b].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER)
                                                            continue;
                                                    }
                                                    // Nero Skill 2 functionality
                                                    else if (activeSkill.curSkillType == SkillData.SkillType.SUPPORT && activeSkill.curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.ENEMIES &&
                                                        unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                                                    {
                                                        if (selectedCombatSlots[b].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER)
                                                            continue;
                                                    }
                                                    else if (activeSkill.curSkillType == SkillData.SkillType.OFFENSE && activeSkill.curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.ENEMIES &&
                                                            unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                                                    {
                                                        if (selectedCombatSlots[b].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY)
                                                            continue;
                                                    }
                                                    else if (activeSkill.curSkillType == SkillData.SkillType.SUPPORT && activeSkill.curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.PLAYERS &&
                                                        unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                                                    {
                                                        if (selectedCombatSlots[b].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY)
                                                            continue;
                                                    }
                                                    else if (activeSkill.curSkillType == SkillData.SkillType.OFFENSE && activeSkill.curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.ENEMIES &&
                                                        unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                                                    {
                                                        if (selectedCombatSlots[b].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER)
                                                            continue;
                                                    }
                                                    else if (activeSkill.curSkillType == SkillData.SkillType.SUPPORT && activeSkill.curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.PLAYERS &&
                                                        unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                                                    {
                                                        if (selectedCombatSlots[b].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER)
                                                            continue;
                                                    }
                                                }

                                                combatSelectedCombatSlots.Add(allowedCombatSlots[b]);
                                                selectedCombatSlots[b].ToggleCombatSelected(true);
                                                if (selectedCombatSlots[b].GetLinkedUnit())
                                                {
                                                    isCombatMode = true;
                                                    selectedCombatSlots[b].GetLinkedUnit().ToggleSelected(true);
                                                    GameManager.Instance.AddUnitsSelected(selectedCombatSlots[b].GetLinkedUnit());
                                                    selectedSlots++;
                                                }

                                                if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                                                {
                                                    //isCombatMode = true;
                                                    //selectedCombatSlots[b].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                                                }

                                                unitsTargeted++;
                                            }

                                            UpdateCombatSlotOutlines(selectedCombatSlots);
                                        }
                                    }
                                }
                            }


                            // Do combat slot outlines
                            //UpdateCombatSlotOutlines(selectedCombatSlots);
                        }
                        else
                        {
                            for (int l = 0; l < allowedCombatSlots.Count; l++)
                            {
                                if (allowedCombatSlots[l].GetLinkedUnit())
                                {
                                    if (allowedCombatSlots[l].GetLinkedUnit().isDead)
                                        continue;

                                    if (allowedCombatSlots[l].GetLinkedUnit() == unit && GameManager.Instance.GetActiveSkill().isSelfCast)
                                    {
                                        combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                        allowedCombatSlots[l].ToggleCombatSelected(true);
                                        allowedCombatSlots[l].GetLinkedUnit().ToggleSelected(true);
                                        GameManager.Instance.AddUnitsSelected(allowedCombatSlots[l].GetLinkedUnit());
                                        selectedSlots++;
                                    }
                                    else if (allowedCombatSlots[l].GetLinkedUnit() == unit && GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT
                                         && GameManager.Instance.GetActiveSkill().canTargetSelf)
                                    {
                                        combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                        allowedCombatSlots[l].ToggleCombatSelected(true);
                                        allowedCombatSlots[l].GetLinkedUnit().ToggleSelected(true);
                                        GameManager.Instance.AddUnitsSelected(allowedCombatSlots[l].GetLinkedUnit());
                                        unitsTargeted++;
                                        selectedSlots++;
                                    }
                                    // If combat slot is a slot that should be attack highlighted, add it to collection to be highlighted
                                    else if (allowedCombatSlots[l].GetLinkedUnit() != unit &&
                                        GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE &&
                                        unit.curUnitType == UnitFunctionality.UnitType.ENEMY &&
                                        allowedCombatSlots[l].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER)
                                    {
                                        combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                        allowedCombatSlots[l].ToggleCombatSelected(true);
                                        allowedCombatSlots[l].GetLinkedUnit().ToggleSelected(true);
                                        GameManager.Instance.AddUnitsSelected(allowedCombatSlots[l].GetLinkedUnit());
                                        selectedSlots++;
                                    }
                                    else if (allowedCombatSlots[l].GetLinkedUnit() != unit &&
                                        GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE &&
                                        unit.curUnitType == UnitFunctionality.UnitType.PLAYER &&
                                        allowedCombatSlots[l].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY)
                                    {
                                        combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                        allowedCombatSlots[l].ToggleCombatSelected(true);
                                        allowedCombatSlots[l].GetLinkedUnit().ToggleSelected(true);
                                        GameManager.Instance.AddUnitsSelected(allowedCombatSlots[l].GetLinkedUnit());
                                        selectedSlots++;
                                    }

                                    else if (allowedCombatSlots[l].GetLinkedUnit() != unit &&
                                        GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT &&
                                        unit.curUnitType == UnitFunctionality.UnitType.ENEMY &&
                                        allowedCombatSlots[l].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY)
                                    {
                                        combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                        allowedCombatSlots[l].ToggleCombatSelected(true);
                                        allowedCombatSlots[l].GetLinkedUnit().ToggleSelected(true);
                                        GameManager.Instance.AddUnitsSelected(allowedCombatSlots[l].GetLinkedUnit());
                                        selectedSlots++;
                                    }
                                    else if (allowedCombatSlots[l].GetLinkedUnit() != unit &&
                                        GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT &&
                                        unit.curUnitType == UnitFunctionality.UnitType.PLAYER &&
                                        allowedCombatSlots[l].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER)
                                    {
                                        combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                        allowedCombatSlots[l].ToggleCombatSelected(true);
                                        allowedCombatSlots[l].GetLinkedUnit().ToggleSelected(true);
                                        GameManager.Instance.AddUnitsSelected(allowedCombatSlots[l].GetLinkedUnit());
                                        selectedSlots++;
                                    }
                                }
                            }

                            UpdateCombatSlotOutlines(combatSelectedCombatSlots);
                        }
                    }
                }                
            }
            // Items mode
            else
            {
                for (int i = 0; i < activeItem.itemAreaHitCount; i++)
                {
                    if (!activeItem.attackAllSelected)
                    {
                        for (int l = 0; l < allowedCombatSlots.Count; l++)
                        {
                            if (activeItem.itemRangeHitArea == Vector2.one)
                            {
                                UnitFunctionality targetunit = null;

                                if (allowedCombatSlots[l].GetFallenUnits().Count > 0)
                                    targetunit = allowedCombatSlots[l].GetFallenUnits()[0];
                                else
                                {
                                    if (allowedCombatSlots[l].GetLinkedUnit())
                                        targetunit = allowedCombatSlots[l].GetLinkedUnit();
                                }

                                if (targetunit != null)
                                {
                                    if (unitsTargeted >= activeItem.itemAreaHitCount)
                                        break;

                                    if (allowedCombatSlots[l].GetFallenUnits().Count > 0)
                                    {
                                        if (activeItem.curTargetType == ItemPiece.TargetType.DEAD && allowedCombatSlots[l].GetFallenUnits()[0])
                                        {
                                            combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                            allowedCombatSlots[l].ToggleCombatSelected(true);
                                            targetunit.ToggleSelected(true);
                                            GameManager.Instance.AddUnitsSelected(targetunit);
                                            unitsTargeted++;
                                            UpdateCombatSlotOutlines(combatSelectedCombatSlots);
                                            break;
                                        }
                                    }
                                }


                                if (targetunit)
                                {
                                    if (unitsTargeted >= activeItem.itemAreaHitCount)
                                        break;

                                    if (targetunit.isDead)
                                        continue;

                                    if (targetunit == unit && activeItem.isSelfCast)
                                    {
                                        combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                        allowedCombatSlots[l].ToggleCombatSelected(true);
                                        targetunit.ToggleSelected(true);
                                        GameManager.Instance.AddUnitsSelected(targetunit);
                                        unitsTargeted++;
                                    }
                                    else if (targetunit == unit && activeItem.curItemType == ItemPiece.ItemType.SUPPORT
                                        && activeItem.canTargetSelf)
                                    {
                                        combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                        allowedCombatSlots[l].ToggleCombatSelected(true);
                                        targetunit.ToggleSelected(true);
                                        GameManager.Instance.AddUnitsSelected(targetunit);
                                        unitsTargeted++;
                                    }
                                    // If combat slot is a slot that should be attack highlighted, add it to collection to be highlighted
                                    else if (targetunit != unit &&
                                        activeItem.curItemType == ItemPiece.ItemType.OFFENSE &&
                                        unit.curUnitType == UnitFunctionality.UnitType.ENEMY &&
                                       targetunit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                                    {
                                        combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                        allowedCombatSlots[l].ToggleCombatSelected(true);
                                        targetunit.ToggleSelected(true);
                                        GameManager.Instance.AddUnitsSelected(targetunit);
                                        unitsTargeted++;
                                    }
                                    else if (targetunit != unit &&
                                        activeItem.curItemType == ItemPiece.ItemType.OFFENSE &&
                                        unit.curUnitType == UnitFunctionality.UnitType.PLAYER &&
                                       targetunit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                                    {
                                        combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                        allowedCombatSlots[l].ToggleCombatSelected(true);
                                        targetunit.ToggleSelected(true);
                                        GameManager.Instance.AddUnitsSelected(targetunit);
                                        unitsTargeted++;
                                    }

                                    else if (targetunit != unit &&
                                        activeItem.curItemType == ItemPiece.ItemType.SUPPORT &&
                                        unit.curUnitType == UnitFunctionality.UnitType.ENEMY &&
                                       targetunit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                                    {
                                        combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                        allowedCombatSlots[l].ToggleCombatSelected(true);
                                        targetunit.ToggleSelected(true);
                                        GameManager.Instance.AddUnitsSelected(targetunit);
                                        unitsTargeted++;
                                    }
                                    else if (targetunit != unit &&
                                        activeItem.curItemType == ItemPiece.ItemType.SUPPORT &&
                                        unit.curUnitType == UnitFunctionality.UnitType.PLAYER &&
                                       targetunit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                                    {
                                        combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                        allowedCombatSlots[l].ToggleCombatSelected(true);
                                        targetunit.ToggleSelected(true);
                                        GameManager.Instance.AddUnitsSelected(targetunit);
                                        unitsTargeted++;
                                    }
                                    UpdateCombatSlotOutlines(combatSelectedCombatSlots);
                                }
                            }
                            else
                            {
                                if (allowedCombatSlots[l].GetFallenUnits().Count > 0 && activeItem.curTargetType == ItemPiece.TargetType.DEAD)
                                {
                                    if (allowedCombatSlots[l].GetFallenUnits()[0])
                                    {
                                        if (!allowedCombatSlots[l].GetFallenUnits()[0].reanimated)
                                        {
                                            combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                            allowedCombatSlots[l].GetFallenUnits()[0].ToggleSelected(true);
                                            allowedCombatSlots[l].ToggleCombatSelected(true);
                                            GameManager.Instance.AddUnitsSelected(allowedCombatSlots[l].GetFallenUnits()[0]);
                                            unitsTargeted++;
                                            continue;
                                        }
                                        else
                                            continue;
                                    }
                                    else
                                        continue;
                                }

                                // Ensure skills cant split attack (bot ai)
                                if (selectedSlots < activeItem.itemRangeHitAreas.Count)
                                {
                                    // Select a group of combat slots based on skill
                                    for (int b = 0; b < activeItem.itemRangeHitAreas.Count; b++)
                                    {
                                        if (allowedCombatSlots[l].GetLinkedUnit())
                                        {
                                            if (allowedCombatSlots[l].GetLinkedUnit().isDead)
                                                continue;

                                            if (allowedCombatSlots[l].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY &&
                                                unit.curUnitType == UnitFunctionality.UnitType.PLAYER && activeItem.curItemType == ItemPiece.ItemType.OFFENSE ||
                                                allowedCombatSlots[l].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY &&
                                                unit.curUnitType == UnitFunctionality.UnitType.ENEMY && activeItem.curItemType == ItemPiece.ItemType.SUPPORT)
                                            {
                                                if (GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x + (int)activeItem.itemRangeHitAreas[b].x,
                                                    (int)allowedCombatSlots[l].GetSlotIndex().y + (int)activeItem.itemRangeHitAreas[b].y)))
                                                {
                                                    if (GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x + (int)activeItem.itemRangeHitAreas[b].x,
                                                        (int)allowedCombatSlots[l].GetSlotIndex().y + (int)activeItem.itemRangeHitAreas[b].y)).GetAllowed())
                                                    {
                                                        // Target slot/unit
                                                        selectedCombatSlots.Add(GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x + (int)activeItem.itemRangeHitAreas[b].x,
                                                        (int)allowedCombatSlots[l].GetSlotIndex().y + (int)activeItem.itemRangeHitAreas[b].y)));
                                                        selectedSlots++;
                                                        unitsTargeted++;
                                                    }
                                                }
                                                else if (GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x - (int)activeItem.itemRangeHitAreas[b].x,
                                                    (int)allowedCombatSlots[l].GetSlotIndex().y + (int)activeItem.itemRangeHitAreas[b].y)))
                                                {
                                                    if (GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x - (int)activeItem.itemRangeHitAreas[b].x,
                                                        (int)allowedCombatSlots[l].GetSlotIndex().y + (int)activeItem.itemRangeHitAreas[b].y)).GetAllowed())
                                                    {
                                                        // Target slot/unit
                                                        selectedCombatSlots.Add(GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x - (int)activeItem.itemRangeHitAreas[b].x,
                                                        (int)allowedCombatSlots[l].GetSlotIndex().y + (int)activeItem.itemRangeHitAreas[b].y)));
                                                        selectedSlots++;
                                                        unitsTargeted++;
                                                    }
                                                }
                                                else if (GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x - (int)activeItem.itemRangeHitAreas[b].x,
                                                    (int)allowedCombatSlots[l].GetSlotIndex().y - (int)activeItem.itemRangeHitAreas[b].y)))
                                                {
                                                    if (GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x - (int)activeItem.itemRangeHitAreas[b].x,
                                                        (int)allowedCombatSlots[l].GetSlotIndex().y - (int)activeItem.itemRangeHitAreas[b].y)).GetAllowed())
                                                    {
                                                        // Target slot/unit
                                                        selectedCombatSlots.Add(GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x - (int)activeItem.itemRangeHitAreas[b].x,
                                                        (int)allowedCombatSlots[l].GetSlotIndex().y - (int)activeItem.itemRangeHitAreas[b].y)));
                                                        selectedSlots++;
                                                        unitsTargeted++;
                                                    }
                                                }
                                            }
                                            if (allowedCombatSlots[l].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER &&
                                                unit.curUnitType == UnitFunctionality.UnitType.ENEMY && activeItem.curItemType == ItemPiece.ItemType.OFFENSE ||
                                                allowedCombatSlots[l].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER &&
                                                unit.curUnitType == UnitFunctionality.UnitType.PLAYER && activeItem.curItemType == ItemPiece.ItemType.SUPPORT)
                                            {
                                                if (GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x + (int)activeItem.itemRangeHitAreas[b].x,
                                                    (int)allowedCombatSlots[l].GetSlotIndex().y + (int)activeItem.itemRangeHitAreas[b].y)))
                                                {
                                                    if (GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x + (int)activeItem.itemRangeHitAreas[b].x,
                                                        (int)allowedCombatSlots[l].GetSlotIndex().y + (int)activeItem.itemRangeHitAreas[b].y)).GetAllowed())
                                                    {
                                                        // Target slot/unit
                                                        selectedCombatSlots.Add(GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x + (int)activeItem.itemRangeHitAreas[b].x,
                                                        (int)allowedCombatSlots[l].GetSlotIndex().y + (int)activeItem.itemRangeHitAreas[b].y)));
                                                        selectedSlots++;
                                                        unitsTargeted++;
                                                    }
                                                }
                                                else if (GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x - (int)activeItem.itemRangeHitAreas[b].x,
                                                    (int)allowedCombatSlots[l].GetSlotIndex().y + (int)activeItem.itemRangeHitAreas[b].y)))
                                                {
                                                    if (GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x - (int)activeItem.itemRangeHitAreas[b].x,
                                                        (int)allowedCombatSlots[l].GetSlotIndex().y + (int)activeItem.itemRangeHitAreas[b].y)).GetAllowed())
                                                    {
                                                        // Target slot/unit
                                                        selectedCombatSlots.Add(GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x - (int)activeItem.itemRangeHitAreas[b].x,
                                                        (int)allowedCombatSlots[l].GetSlotIndex().y + (int)activeItem.itemRangeHitAreas[b].y)));
                                                        selectedSlots++;
                                                        unitsTargeted++;
                                                    }
                                                }
                                                else if (GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x - (int)activeItem.itemRangeHitAreas[b].x,
                                                    (int)allowedCombatSlots[l].GetSlotIndex().y - (int)activeItem.itemRangeHitAreas[b].y)))
                                                {
                                                    if (GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x - (int)activeItem.itemRangeHitAreas[b].x,
                                                        (int)allowedCombatSlots[l].GetSlotIndex().y - (int)activeItem.itemRangeHitAreas[b].y)).GetAllowed())
                                                    {
                                                        // Target slot/unit
                                                        selectedCombatSlots.Add(GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x - (int)activeItem.itemRangeHitAreas[b].x,
                                                        (int)allowedCombatSlots[l].GetSlotIndex().y - (int)activeItem.itemRangeHitAreas[b].y)));
                                                        selectedSlots++;
                                                        unitsTargeted++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }


                                if (selectedCombatSlots.Count > selectionsAllowed)
                                    continue;
                                else
                                {
                                    if (selectedCombatSlots.Count != 0)
                                    {
                                        for (int b = 0; b < selectedCombatSlots.Count; b++)
                                        {
                                            if (allowedCombatSlots[b].GetLinkedUnit())
                                            {
                                                if (allowedCombatSlots[b].GetLinkedUnit().isDead)
                                                    continue;
                                            }

                                            if (selectedCombatSlots[b].GetLinkedUnit())
                                            {
                                                // Nero Skill 2 functionality
                                                if (activeItem.curItemType == ItemPiece.ItemType.SUPPORT && activeItem.curSelectionType == ItemPiece.SelectionType.ENEMIES &&
                                                    unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                                                {
                                                    if (selectedCombatSlots[b].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER)
                                                        continue;
                                                }
                                                else if (activeItem.curItemType == ItemPiece.ItemType.OFFENSE && activeItem.curSelectionType == ItemPiece.SelectionType.ENEMIES &&
                                                        unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                                                {
                                                    if (selectedCombatSlots[b].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY)
                                                        continue;
                                                }
                                                else if (activeItem.curItemType == ItemPiece.ItemType.SUPPORT && activeItem.curSelectionType == ItemPiece.SelectionType.ALLIES &&
                                                    unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                                                {
                                                    if (selectedCombatSlots[b].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY)
                                                        continue;
                                                }
                                                else if (activeItem.curItemType == ItemPiece.ItemType.OFFENSE && activeItem.curSelectionType == ItemPiece.SelectionType.ENEMIES &&
                                                    unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                                                {
                                                    if (selectedCombatSlots[b].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER)
                                                        continue;
                                                }
                                                else if (activeItem.curItemType == ItemPiece.ItemType.SUPPORT && activeItem.curSelectionType == ItemPiece.SelectionType.ALLIES &&
                                                    unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                                                {
                                                    if (selectedCombatSlots[b].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER)
                                                        continue;
                                                }
                                            }

                                            combatSelectedCombatSlots.Add(allowedCombatSlots[b]);
                                            selectedCombatSlots[b].ToggleCombatSelected(true);
                                            if (selectedCombatSlots[b].GetLinkedUnit())
                                            {
                                                isCombatMode = true;
                                                selectedCombatSlots[b].GetLinkedUnit().ToggleSelected(true);
                                                GameManager.Instance.AddUnitsSelected(selectedCombatSlots[b].GetLinkedUnit());
                                                selectedSlots++;
                                            }

                                            if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                                            {
                                                //isCombatMode = true;
                                                //selectedCombatSlots[b].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                                            }

                                            unitsTargeted++;
                                        }

                                        UpdateCombatSlotOutlines(selectedCombatSlots);
                                    }
                                }
                            }
                        }

                        // Do combat slot outlines
                        //UpdateCombatSlotOutlines(selectedCombatSlots);
                    }
                    else
                    {
                        for (int l = 0; l < allowedCombatSlots.Count; l++)
                        {
                            if (allowedCombatSlots[l].GetLinkedUnit())
                            {
                                if (allowedCombatSlots[l].GetLinkedUnit().isDead)
                                    continue;

                                if (allowedCombatSlots[l].GetLinkedUnit() == unit && activeItem.isSelfCast)
                                {
                                    combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                    allowedCombatSlots[l].ToggleCombatSelected(true);
                                    allowedCombatSlots[l].GetLinkedUnit().ToggleSelected(true);
                                    GameManager.Instance.AddUnitsSelected(allowedCombatSlots[l].GetLinkedUnit());
                                    selectedSlots++;
                                }
                                else if (allowedCombatSlots[l].GetLinkedUnit() == unit && activeItem.curItemType == ItemPiece.ItemType.SUPPORT
                                     && activeItem.canTargetSelf)
                                {
                                    combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                    allowedCombatSlots[l].ToggleCombatSelected(true);
                                    allowedCombatSlots[l].GetLinkedUnit().ToggleSelected(true);
                                    GameManager.Instance.AddUnitsSelected(allowedCombatSlots[l].GetLinkedUnit());
                                    unitsTargeted++;
                                    selectedSlots++;
                                }
                                // If combat slot is a slot that should be attack highlighted, add it to collection to be highlighted
                                else if (allowedCombatSlots[l].GetLinkedUnit() != unit &&
                                    activeItem.curItemType == ItemPiece.ItemType.OFFENSE &&
                                    unit.curUnitType == UnitFunctionality.UnitType.ENEMY &&
                                    allowedCombatSlots[l].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER)
                                {
                                    combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                    allowedCombatSlots[l].ToggleCombatSelected(true);
                                    allowedCombatSlots[l].GetLinkedUnit().ToggleSelected(true);
                                    GameManager.Instance.AddUnitsSelected(allowedCombatSlots[l].GetLinkedUnit());
                                    selectedSlots++;
                                }
                                else if (allowedCombatSlots[l].GetLinkedUnit() != unit &&
                                    activeItem.curItemType == ItemPiece.ItemType.OFFENSE &&
                                    unit.curUnitType == UnitFunctionality.UnitType.PLAYER &&
                                    allowedCombatSlots[l].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY)
                                {
                                    combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                    allowedCombatSlots[l].ToggleCombatSelected(true);
                                    allowedCombatSlots[l].GetLinkedUnit().ToggleSelected(true);
                                    GameManager.Instance.AddUnitsSelected(allowedCombatSlots[l].GetLinkedUnit());
                                    selectedSlots++;
                                }

                                else if (allowedCombatSlots[l].GetLinkedUnit() != unit &&
                                    activeItem.curItemType == ItemPiece.ItemType.SUPPORT &&
                                    unit.curUnitType == UnitFunctionality.UnitType.ENEMY &&
                                    allowedCombatSlots[l].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY)
                                {
                                    combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                    allowedCombatSlots[l].ToggleCombatSelected(true);
                                    allowedCombatSlots[l].GetLinkedUnit().ToggleSelected(true);
                                    GameManager.Instance.AddUnitsSelected(allowedCombatSlots[l].GetLinkedUnit());
                                    selectedSlots++;
                                }
                                else if (allowedCombatSlots[l].GetLinkedUnit() != unit &&
                                    activeItem.curItemType == ItemPiece.ItemType.SUPPORT &&
                                    unit.curUnitType == UnitFunctionality.UnitType.PLAYER &&
                                    allowedCombatSlots[l].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER)
                                {
                                    combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                    allowedCombatSlots[l].ToggleCombatSelected(true);
                                    allowedCombatSlots[l].GetLinkedUnit().ToggleSelected(true);
                                    GameManager.Instance.AddUnitsSelected(allowedCombatSlots[l].GetLinkedUnit());
                                    selectedSlots++;
                                }
                            }
                        }

                        UpdateCombatSlotOutlines(combatSelectedCombatSlots);
                    }
                }
            }


            //UpdateCombatSlotOutlines(combatSelectedCombatSlots);
        }

        // needed to actually perform attack after selecting, for bot ai
        if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY && GetTargetCombatSlots().Count > 0)
        {
            for (int i = 0; i < targetedCombatSlots.Count; i++)
            {
                if (targetedCombatSlots[i].GetLinkedUnit())
                {
                    if (targetedCombatSlots[i].GetLinkedUnit().isSelected)
                    {
                        isCombatMode = true;
                        targetedCombatSlots[i].button.ButtonSelectCombatSlot(true);
                        break;
                    }
                    else
                    {
                        isCombatMode = true;
                        targetedCombatSlots[i].button.ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }

        }
        else if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY && GetTargetCombatSlots().Count == 0)
        {
            StartCoroutine(unit.UnitEndTurn(true));
        }

        // If a dead unit is somehow selected, whilst a skill that only target alive units is selected. Force end turn.
        for (int i = 0; i < GetTargetCombatSlots().Count; i++)
        {
            if (GetTargetCombatSlots()[i].GetLinkedUnit())
            {
                if (GetTargetCombatSlots()[i].GetLinkedUnit().isDead && GameManager.Instance.GetActiveSkill().curskillSelectionAliveType == SkillData.SkillSelectionAliveType.ALIVE)
                    StartCoroutine(unit.UnitEndTurn(true));
            }
        }
    }
    
    public void UpdateAttackSelection(UnitFunctionality unit)
    {
        if (!GameManager.Instance.isSkillsMode || GameManager.Instance.GetActiveSkill() == null)
            return;

        CombatGridManager.Instance.GetTargetCombatSlots().Clear();

        UnitFunctionality targetedUnit = null;

        List<UnitFunctionality> selectedUnits = new List<UnitFunctionality>();
        int index = 0;
        for (int i = 0; i < CharacterCarasel.Instance.GetAllAllies().Count; i++)
        {
            if (CharacterCarasel.Instance.GetAllAllies()[i].unitName == unit.GetUnitName())
            {
                index = i;
            }
        }

        SkillData.ClassType classType = SkillData.ClassType.STANDARD;

        if (GameManager.Instance.GetActiveSkill().curClassType == SkillData.ClassType.AGGRESSIVE)
        {
            classType = SkillData.ClassType.AGGRESSIVE;
        }
        else if (GameManager.Instance.GetActiveSkill().curClassType == SkillData.ClassType.EVASIVE)
        {
            classType = SkillData.ClassType.EVASIVE;
        }

        // If active unit is agressive, find closest unit as target
        if (classType == SkillData.ClassType.AGGRESSIVE)
        {
            int x = 25;

            for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
            {
                if (GameManager.Instance.activeRoomHeroes[i].GetRangeFromUnit(unit) < x && !GameManager.Instance.activeRoomHeroes[i].isDead)
                {
                    x = GameManager.Instance.activeRoomHeroes[i].GetRangeFromUnit(unit);

                    targetedUnit = GameManager.Instance.activeRoomHeroes[i];
                }
            }
        }


        for (int i = 0; i < GameManager.Instance.activeRoomAllUnitFunctionalitys.Count; i++)
        {
            if (classType != SkillData.ClassType.AGGRESSIVE)
                targetedUnit = GameManager.Instance.activeRoomAllUnitFunctionalitys[i];

            // If target is dead and active skill targets only alive units, skip continuing in atking this unit
            if (targetedUnit.isDead && GameManager.Instance.GetActiveSkill().curskillSelectionAliveType == SkillData.SkillSelectionAliveType.ALIVE)
                continue;

            if (GameManager.Instance.GetActiveSkill().isSelfCast)// && targetedUnit == this)
            {
                CombatGridManager.Instance.GetTargetCombatSlots().Add(unit.GetActiveCombatSlot());
                break;
            }

            if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY || unit.reanimated)
            {
                if (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE)
                {
                    if (targetedUnit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                    {
                        CombatGridManager.Instance.GetTargetCombatSlots().Add(targetedUnit.GetActiveCombatSlot());
                        //selectedUnits.Add(targetedUnit);
                    }
                }
                else if (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT)
                {
                    if (targetedUnit.curUnitType == UnitFunctionality.UnitType.ENEMY || unit.reanimated)
                    {
                        CombatGridManager.Instance.GetTargetCombatSlots().Add(targetedUnit.GetActiveCombatSlot());
                        //selectedUnits.Add(targetedUnit);
                    }
                }
            }
            else if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                if (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE)
                {
                    if (targetedUnit.curUnitType == UnitFunctionality.UnitType.ENEMY || unit.reanimated)
                    {
                        CombatGridManager.Instance.GetTargetCombatSlots().Add(targetedUnit.GetActiveCombatSlot());
                        //selectedUnits.Add(targetedUnit);
                    }
                }
                else if (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT)
                {
                    if (targetedUnit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                    {
                        CombatGridManager.Instance.GetTargetCombatSlots().Add(targetedUnit.GetActiveCombatSlot());
                        //selectedUnits.Add(targetedUnit);
                    }
                }
            }
        }

        if (GetTargetCombatSlots().Count == 0)
        {
            StartCoroutine(GameManager.Instance.GetActiveUnitFunctionality().StartUnitTurn());
            return;
        }


        if (targetedCombatSlots.Count > 0)
        {
            if (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT)
            {
                // Sort selected slots by lowest health
                CombatGridManager.Instance.GetTargetCombatSlots().Sort(CombatGridManager.Instance.CompareUnitHealth);
                CombatGridManager.Instance.GetTargetCombatSlots().Reverse();
            }
            else
            {
                CombatGridManager.Instance.GetTargetCombatSlots().Sort(CombatGridManager.Instance.CompareSlotRangeFromUnit);
                CombatGridManager.Instance.GetTargetCombatSlots().Reverse();
            }
        }
    }

    public Vector2 GetCombatSlotIndex(int slotIndex)
    {
        Vector2 newSlotIndex = new Vector2(0,0);

        #region Calculate New Slot Index
        // 1st row
        if (slotIndex == 0)
        {
            newSlotIndex = new Vector2(0, 0);
        }
        else if (slotIndex == 1)
        {
            newSlotIndex = new Vector2(1, 0);
        }
        else if (slotIndex == 2)
        {
            newSlotIndex = new Vector2(2, 0);
        }
        else if (slotIndex == 3)
        {
            newSlotIndex = new Vector2(3, 0);
        }
        else if (slotIndex == 4)
        {
            newSlotIndex = new Vector2(4, 0);
        }
        // 2nd row
        else if (slotIndex == 5)
        {
            newSlotIndex = new Vector2(0, 1);
        }
        else if (slotIndex == 6)
        {
            newSlotIndex = new Vector2(1, 1);
        }
        else if (slotIndex == 7)
        {
            newSlotIndex = new Vector2(2, 1);
        }
        else if (slotIndex == 8)
        {
            newSlotIndex = new Vector2(3, 1);
        }
        else if (slotIndex == 9)
        {
            newSlotIndex = new Vector2(4, 1);
        }
        // 3nd row
        else if (slotIndex == 10)
        {
            newSlotIndex = new Vector2(0, 2);
        }
        else if (slotIndex == 11)
        {
            newSlotIndex = new Vector2(1, 2);
        }
        else if (slotIndex == 12)
        {
            newSlotIndex = new Vector2(2, 2);
        }
        else if (slotIndex == 13)
        {
            newSlotIndex = new Vector2(3, 2);
        }
        else if (slotIndex == 14)
        {
            newSlotIndex = new Vector2(4, 2);
        }

        // 4th row
        else if (slotIndex == 15)
        {
            newSlotIndex = new Vector2(0, 3);
        }
        else if (slotIndex == 16)
        {
            newSlotIndex = new Vector2(1, 3);
        }
        else if (slotIndex == 17)
        {
            newSlotIndex = new Vector2(2, 3);
        }
        else if (slotIndex == 18)
        {
            newSlotIndex = new Vector2(3, 3);
        }
        else if (slotIndex == 19)
        {
            newSlotIndex = new Vector2(4, 3);
        }
        // 5th row
        else if (slotIndex == 20)
        {
            newSlotIndex = new Vector2(0, 4);
        }
        else if (slotIndex == 21)
        {
            newSlotIndex = new Vector2(1, 4);
        }
        else if (slotIndex == 22)
        {
            newSlotIndex = new Vector2(2, 4);
        }
        else if (slotIndex == 23)
        {
            newSlotIndex = new Vector2(3, 4);
        }
        else if (slotIndex == 24)
        {
            newSlotIndex = new Vector2(4, 4);
        }
        // 6th row
        else if (slotIndex == 25)
        {
            newSlotIndex = new Vector2(0, 5);
        }
        else if (slotIndex == 26)
        {
            newSlotIndex = new Vector2(1, 5);
        }
        else if (slotIndex == 27)
        {
            newSlotIndex = new Vector2(2, 5);
        }
        else if (slotIndex == 28)
        {
            newSlotIndex = new Vector2(3, 5);
        }
        else if (slotIndex == 29)
        {
            newSlotIndex = new Vector2(4, 5);
        }
        #endregion
        return newSlotIndex;
    }

    IEnumerator EndUnitTurnAfterWait(UnitFunctionality unit)
    {
        yield return new WaitForSeconds(.85f);

        StartCoroutine(unit.UnitEndTurn(true));
    }

    public CombatSlot GetCombatSlot(Vector2 index)
    {
        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            if (allCombatSlots[i].GetSlotIndex() == index)
                return allCombatSlots[i];
        }

        return null;
    }
    public void UpdateCombatSlotsIndex()
    {
        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            allCombatSlots[i].UpdateSlotIndex(GetCombatSlotIndex(i));
        }
    }

    public List<CombatSlot> GetAllCombatSlots()
    {
        return allCombatSlots;
    }

    public CombatSlot GetCombatSlot(int index = 0)
    {
        return allCombatSlots[index];
    }

    public void ToggleCombatGrid(bool toggle = true)
    {
        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            if (toggle)
                allCombatSlots[i].GetComponent<UIElement>().UpdateAlpha(1);
            else
                allCombatSlots[i].GetComponent<UIElement>().UpdateAlpha(0);
        }
        /*
        if (toggle)
            combatGrid.UpdateAlpha(1);  
        else
            combatGrid.UpdateAlpha(0);
        */
    }

    public void ToggleCombatGrid2(bool toggle = true)
    {
        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            if (toggle)
                allCombatSlots[i].GetComponent<UIElement>().UpdateAlpha2(1);
            else
                allCombatSlots[i].GetComponent<UIElement>().UpdateAlpha2(0);
        }
        /*
        if (toggle)
            combatGrid.UpdateAlpha(1);  
        else
            combatGrid.UpdateAlpha(0);
        */
    }

    private void Awake()
    {
        Instance = this;
    }
}
