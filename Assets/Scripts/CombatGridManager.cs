using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatGridManager : MonoBehaviour
{
    public static CombatGridManager Instance;
    public float unitMoveArrowOnAlpha = 0.75f;

    [SerializeField] private List<CombatSlot> targetCombatSlots = new List<CombatSlot>();
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

    public void ToggleCombatSlotsInput(bool toggle = true)
    {
        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            allCombatSlots[i].ToggleCombatSlotInput(toggle);
        }
    }

    public void Setup()
    {
        UpdateCombatSlotsIndex();

        DisableAllButtons();
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

                ToggleButton(GetButtonItems(), true, true);
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

            ToggleButton(GetButtonItems(), true, true);
        }
    }

    public void ToggleButton(ButtonFunctionality button, bool toggle = true, bool allowHide = false)
    {
        if (toggle)
        {
            button.GetComponent<UIElement>().ToggleButton(true);
            button.GetComponent<UIElement>().UpdateAlpha(1);
        }
        else
        {
            button.GetComponent<UIElement>().ToggleButton(false);

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
            //GameManager.Instance.UpdateActiveSkill(GameManager.Instance.GetActiveUnitFunctionality().GetBaseSelectSkill());
            // Disable extra move prompt
            OverlayUI.Instance.extraMovePrompt.UpdateAlpha(0);

            //GameManager.Instance.ToggleAllUnitButtons(true);

            if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
                GameManager.Instance.ToggleSkillsItemToggleButton(true);

            UnselectAllSelectedCombatSlots();

            if (GameManager.Instance.isSkillsMode)
            {
                OverlayUI.Instance.ToggleAllStats(true, true, false);

                GameManager.Instance.UpdatePlayerAbilityUI(true);
                if (GameManager.Instance.GetActiveSkill() && GameManager.Instance.GetActiveItem())
                    GameManager.Instance.UpdateMainIconDetails(GameManager.Instance.GetActiveSkill(), GameManager.Instance.GetActiveItem());
                else if (GameManager.Instance.GetActiveSkill())
                    GameManager.Instance.UpdateMainIconDetails(GameManager.Instance.GetActiveSkill(), null);
            }
            else
            {
                OverlayUI.Instance.ToggleAllStats(true, false, false);

                GameManager.Instance.UpdatePlayerAbilityUI(false);
                if (GameManager.Instance.GetActiveItem())
                    GameManager.Instance.UpdateMainIconDetails(null, GameManager.Instance.GetActiveItem());
            }
        }
        else
        {
            //GameManager.Instance.ToggleAllUnitButtons(false);

            GameManager.Instance.ToggleSkillsItemToggleButton(false);

            GameManager.Instance.UpdatePlayerAbilityUI(false, false, true);
            GameManager.Instance.UpdateMainIconDetails(null, null);

            OverlayUI.Instance.ToggleAllStats(true, false, true);

            UpdateUnitMoveRange(GameManager.Instance.GetActiveUnitFunctionality());

            // Enable extra move prompt if unit has 0 moves left
            if (GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() == 0)
                OverlayUI.Instance.extraMovePrompt.UpdateAlpha(1);
            else
                OverlayUI.Instance.extraMovePrompt.UpdateAlpha(0);

            //if (GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() < 0)
            //    GetButtonAttackMovement().ButtonCombatAttackMovement(true);
        }

        UpdateCombatMainSlots();

        GameManager.Instance.UpdateDetailsBanner();
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
                movingUnit.UpdateUnitLookDirection();

                if (movingUnit.GetCurMovementUses() == 0)
                    StartCoroutine(AutoSwapOutOfMovementMode());

                if (movingUnit.GetCurMovementUses() < 0)
                {
                    if (!movingUnit.usedExtraMove)
                        movingUnit.usedExtraMove = true;

                    if (movingUnit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                        StartCoroutine(AutoSwapOutOfMovementModeAndLockSkills());
                }

                movingUnit.SetPositionAndParent(GetSelectedCombatSlotMove().transform);

                CheckToUnlinkCombatSlot();

                if (movingUnit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                {
                    if (movingUnit.GetCurMovementUses() == 0 && movingUnit.hasAttacked)
                        StartCoroutine(EndUnitTurnAfterWait(movingUnit));
                }
            }
        }
    }

    IEnumerator AutoSwapOutOfMovementMode()
    {
        yield return new WaitForSeconds(.25f);

        // If fighter HASNT attacked, display skills tab
        if (!GameManager.Instance.GetActiveUnitFunctionality().hasAttacked)
        {
            if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
                ToggleTabButtons("Skills");

            GameManager.Instance.isSkillsMode = true;
            UpdateAttackMovementMode(false, true, true);
        }
        // If fighter HAS attacked, display items tab
        else
        {
            if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
                ToggleTabButtons("Items");

            if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                GameManager.Instance.isSkillsMode = false;
                UpdateAttackMovementMode(false, true, true);
            }
            else
            {
                GameManager.Instance.isSkillsMode = true;
                UpdateAttackMovementMode(false, true, true);
            }

            if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
                ToggleTabButtons("Items");
        }
    }

    IEnumerator AutoSwapOutOfMovementModeAndLockSkills()
    {
        yield return new WaitForSeconds(.25f);

        GetButtonAttack().ButtonCombatAttackTab();
        //UpdateAttackMovementMode(false, true);      
        GetButtonSkills().ButtonCombatItemTab();
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

    public List<CombatSlot> GetTargetCombatSlots()
    {
        return targetCombatSlots;
    }

    public void MoveRandomly(UnitFunctionality unit, List<CombatSlot> combatSlots)
    {
        UpdateUnitMoveRange(unit);

        //unit.UnitEndTurn(true);
        // Move randomly
        if (unit.GetCurMovementUses() > 0)
        {
            for (int i = 0; i < 10; i++)
            {
                int rand = Random.Range(0, combatSlots.Count);

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

        if (unit.hasAttacked || unit.GetCurMovementUses() <= 0)
        {
            StartCoroutine(EndUnitTurnAfterWait(unit));
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
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
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
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
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
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
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
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
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
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
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
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }
        }
        else if (moveDirection == "DownLeft")
        {
            // Move up left if available
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
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
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
                        combatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
            }
        }

        if (!moved)
        {
            //UpdateUnitAttackRange(unit);
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
                GameManager.Instance.UpdateMainIconDetails(null, GameManager.Instance.GetActiveItem(), false);
                GameManager.Instance.ToggleAllowSelection(true);
                return;
            }
        }

        ToggleIsMovementAllowed(true);

        UpdateUnitMoveRange(unit);

        List<CombatSlot> combatSlots = new List<CombatSlot>();

        for (int i = 0; i < GetAllCombatSlots().Count; i++)
        {
            if (GetCombatSlot(i).GetAllowed())
            {
                combatSlots.Add(GetCombatSlot(i));
                //GetCombatSlot(i)
            }
        }

        bool switched = false;

        if (isCombatMode)
        {
            // If there are no selections in current position
            if (GetTargetCombatSlots().Count == 0 || unit.hasAttacked && unit.curUnitType == UnitFunctionality.UnitType.ENEMY &&
                unit.GetCurMovementUses() <= 0)
            {
                StartCoroutine(EndUnitTurnAfterWait(unit));
                return;
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
            if (slotsInRange > 0 && !unit.hasAttacked)
            {
                UpdateUnitAttackRange(unit);
                return;
            }
            // No slots in range, move towards targets
            else
            {
                if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                {
                    // If unit has movement remaining, select and move to desired direction (N S E W)
                    if (unit.GetCurMovementUses() > 0)
                    {
                        isCombatMode = false;
                        UpdateAttackMovementMode(true, false, true);
                        for (int b = 0; b < 4; b++)
                        {
                            if (switched)
                                break;

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
                                if (GameManager.Instance.GetActiveSkill().skillIgnoreRange > 0)
                                {
                                    switched = true;
                                    SelectSlotToMove(unit, combatSlots, "DownLeft");
                                    break;
                                }
                                else
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
                                if (GameManager.Instance.GetActiveSkill().skillIgnoreRange > 0)
                                {
                                    switched = true;
                                    SelectSlotToMove(unit, combatSlots, "UpRight");
                                    break;
                                }
                                else
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
                                if (GameManager.Instance.GetActiveSkill().skillIgnoreRange > 0)
                                {
                                    switched = true;
                                    SelectSlotToMove(unit, combatSlots, "DownRight");
                                    break;
                                }
                                else
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
                                if (GameManager.Instance.GetActiveSkill().skillIgnoreRange > 0)
                                {
                                    switched = true;
                                    // Select Up/left slot to move
                                    SelectSlotToMove(unit, combatSlots, "UpLeft");
                                    break;
                                }
                                else
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
                                if (GameManager.Instance.GetActiveSkill().skillIgnoreRange > 0)
                                {
                                    switched = true;
                                    // Select Down slot to move
                                    SelectSlotToMove(unit, combatSlots, "Down");
                                    break;
                                }
                                else
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
                                if (GameManager.Instance.GetActiveSkill().skillIgnoreRange > 0)
                                {
                                    switched = true;
                                    // Select Down slot to move
                                    SelectSlotToMove(unit, combatSlots, "Up");
                                    break;
                                }
                                else
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
                                if (GameManager.Instance.GetActiveSkill().skillIgnoreRange > 0)
                                {
                                    // Select left slot to move
                                    SelectSlotToMove(unit, combatSlots, "Left");
                                    break;
                                }
                                else
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
                                if (GameManager.Instance.GetActiveSkill().skillIgnoreRange > 0)
                                {
                                    // Select Down slot to move
                                    SelectSlotToMove(unit, combatSlots, "Right");
                                    break;
                                }
                                else
                                {
                                    // Select Up slot to move
                                    SelectSlotToMove(unit, combatSlots, "Left");
                                    break;
                                }
                            }
                            /*
                            if (!switched)
                            {
                                UpdateUnitAttackRange(unit);
                                break;
                            }
                            */
                        }

                        switched = false;
                    }
                }
                else
                {
                    UpdateAttackMovementMode(true, false, false); // << ??
                }
            }
        }
    }

    public void MoveUnitToNewSlot(UnitFunctionality unit)
    {
        // Update unit to look in direction of movement
        // Moving right
        if (GetSelectedCombatSlotMove().GetSlotIndex().x > unit.GetActiveCombatSlot().GetSlotIndex().x)
            unit.UpdateUnitLookDirection(true, false);
        // Moving left
        else if (GetSelectedCombatSlotMove().GetSlotIndex().x < unit.GetActiveCombatSlot().GetSlotIndex().x)
            unit.UpdateUnitLookDirection(true, true);

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

        if (unitMovementRange <= 0 || unit.usedExtraMove)
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
    public void UpdateUnitAttackRange(UnitFunctionality unit)
    {
        if (unit.attacked)
            return;

        if (unit.hasAttacked)
            return;

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

            if (xDiff <= GameManager.Instance.GetActiveSkill().curSkillRange &&
                yDiff <= GameManager.Instance.GetActiveSkill().curSkillRange)
            {
                // If combat slot should be an allowed slot, make it
                if (xDiff <= GameManager.Instance.GetActiveSkill().curSkillRange &&
                    yDiff <= GameManager.Instance.GetActiveSkill().curSkillRange)
                {
                    combatSlot = allCombatSlots[i];
                    combatSlot.ToggleSlotAllowed(true);
                }

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

    public void UpdateUnitAttackHitArea(UnitFunctionality unit, CombatSlot targetedSlot = null)
    {
        done = false;

        Vector2 unitCombatIndex = Vector2.zero;

        if (unit.GetActiveCombatSlot())
        {
            CombatSlot unitCombatSlot = null;
            unitCombatSlot = unit.GetActiveCombatSlot();

            unitCombatIndex = unitCombatSlot.GetSlotIndex();
        }

        //UnselectAllSelectedCombatSlots();

        Vector2 combatSlotIndex = Vector2.zero;
        Vector2 unitLocationIndex = Vector2.zero;

        Vector2 skillAreaTargets = Vector2.zero;
        CombatSlot combatSlot = null;

        List<CombatSlot> allowedCombatSlots = new List<CombatSlot>();
        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            if (allCombatSlots[i].GetAllowed())
            {
                allowedCombatSlots.Add(allCombatSlots[i]);
            }
        }

        allowedCombatSlots = ShuffleList(allowedCombatSlots);

        List<CombatSlot> combatSelectedCombatSlots = new List<CombatSlot>();

        int countX = 0;
        int countY = 0;
        if (!GameManager.Instance.GetActiveSkill().attackAllSelected)
        {
            countX = (int)GameManager.Instance.GetActiveSkill().skillRangeHitArea.x;
            countY = (int)GameManager.Instance.GetActiveSkill().skillRangeHitArea.y;
        }
        else
        {
            countX = 1;
            countY = 1;
        }

        bool endTurn = false;

        // Update combat slots that are going to be local from units stand point

        int unitsTargeted = 0;

        int selectionsAllowed = 0;
        if (GameManager.Instance.GetActiveSkill().skillRangeHitAreas.Count != 0)
            selectionsAllowed = GameManager.Instance.GetActiveSkill().skillRangeHitAreas.Count;

        bool targetFighters = false;

        List<CombatSlot> selectedCombatSlots = new List<CombatSlot>();
        SkillData activeSkill = GameManager.Instance.GetActiveSkill();

        if (activeSkill.curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.ENEMIES)
            targetFighters = false;
        else if (activeSkill.curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.PLAYERS)
            targetFighters = true;

        if (targetedSlot != null)
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
                else if(GetCombatSlot(new Vector2((int)targetedSlot.GetSlotIndex().x - (int)activeSkill.skillRangeHitAreas[b].x,
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
            }

            for (int b = 0; b < selectedCombatSlots.Count; b++)
            {
                combatSelectedCombatSlots.Add(allowedCombatSlots[b]);
                selectedCombatSlots[b].ToggleCombatSelected(true);
                if (selectedCombatSlots[b].GetLinkedUnit())
                {
                    selectedCombatSlots[b].GetLinkedUnit().ToggleSelected(true);
                    GameManager.Instance.AddUnitsSelected(selectedCombatSlots[b].GetLinkedUnit());
                }

                if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                    selectedCombatSlots[b].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                unitsTargeted++;
            }

            if (selectedCombatSlots.Count != selectionsAllowed)
            {
                RemoveAllCombatSelectedCombatSlots();
                AudioManager.Instance.Play("SFX_ShopBuyFail");
                return;
            }
        }
        else
        {
            for (int i = 0; i < GameManager.Instance.GetActiveSkill().skillAreaHitCount; i++)
            {
                if (!GameManager.Instance.GetActiveSkill().attackAllSelected)
                {
                    for (int l = 0; l < allowedCombatSlots.Count; l++)
                    {
                        if (GameManager.Instance.GetActiveSkill().skillRangeHitArea == Vector2.one)
                        {
                            if (allowedCombatSlots[l].GetLinkedUnit())
                            {
                                if (unitsTargeted >= GameManager.Instance.GetActiveSkill().skillAreaHitCount)
                                    break;

                                if (allowedCombatSlots[l].GetLinkedUnit() == unit && GameManager.Instance.GetActiveSkill().isSelfCast)
                                {
                                    combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                    allowedCombatSlots[l].ToggleCombatSelected(true);
                                    allowedCombatSlots[l].GetLinkedUnit().ToggleSelected(true);
                                    GameManager.Instance.AddUnitsSelected(allowedCombatSlots[l].GetLinkedUnit());
                                    unitsTargeted++;
                                }
                                else if (allowedCombatSlots[l].GetLinkedUnit() == unit && GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT
                                    && GameManager.Instance.GetActiveSkill().canTargetSelf)
                                {
                                    combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                    allowedCombatSlots[l].ToggleCombatSelected(true);
                                    allowedCombatSlots[l].GetLinkedUnit().ToggleSelected(true);
                                    GameManager.Instance.AddUnitsSelected(allowedCombatSlots[l].GetLinkedUnit());
                                    unitsTargeted++;
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
                                    unitsTargeted++;
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
                                    unitsTargeted++;
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
                                    unitsTargeted++;
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
                                    unitsTargeted++;
                                }
                            }
                        }
                        else
                        {
                            // Select a group of combat slots based on skill
                            for (int b = 0; b < activeSkill.skillRangeHitAreas.Count; b++)
                            {
                                if (allowedCombatSlots[l].GetLinkedUnit())
                                {
                                    if (allowedCombatSlots[l].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY &&
                                        !targetFighters)
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
                                            }
                                        }
                                        else if (GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x - (int)activeSkill.skillRangeHitAreas[b].x,
                                            (int)allowedCombatSlots[l].GetSlotIndex().y + (int)activeSkill.skillRangeHitAreas[b].y)))
                                        {
                                            if (GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x - (int)activeSkill.skillRangeHitAreas[b].x,
                                                (int)allowedCombatSlots[l].GetSlotIndex().y + (int)activeSkill.skillRangeHitAreas[b].y)).GetAllowed())
                                            {
                                                // Target slot/unit
                                                selectedCombatSlots.Add(GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x - (int)activeSkill.skillRangeHitAreas[b].x,
                                                (int)allowedCombatSlots[l].GetSlotIndex().y + (int)activeSkill.skillRangeHitAreas[b].y)));
                                            }
                                        }
                                    }
                                    else if (allowedCombatSlots[l].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER &&
                                        targetFighters)
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
                                            }
                                        }
                                        else if (GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x - (int)activeSkill.skillRangeHitAreas[b].x,
                                            (int)allowedCombatSlots[l].GetSlotIndex().y + (int)activeSkill.skillRangeHitAreas[b].y)))
                                        {
                                            if (GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x - (int)activeSkill.skillRangeHitAreas[b].x,
                                                (int)allowedCombatSlots[l].GetSlotIndex().y + (int)activeSkill.skillRangeHitAreas[b].y)).GetAllowed())
                                            {
                                                // Target slot/unit
                                                selectedCombatSlots.Add(GetCombatSlot(new Vector2((int)allowedCombatSlots[l].GetSlotIndex().x - (int)activeSkill.skillRangeHitAreas[b].x,
                                                (int)allowedCombatSlots[l].GetSlotIndex().y + (int)activeSkill.skillRangeHitAreas[b].y)));
                                            }
                                        }
                                    }
                                }
                            }

                            if (selectedCombatSlots.Count != selectionsAllowed)
                                continue;
                            else
                            {
                                if (selectedCombatSlots.Count != 0)
                                {
                                    for (int b = 0; b < selectedCombatSlots.Count; b++)
                                    {
                                        combatSelectedCombatSlots.Add(allowedCombatSlots[b]);
                                        selectedCombatSlots[b].ToggleCombatSelected(true);
                                        if (selectedCombatSlots[b].GetLinkedUnit())
                                        {
                                            selectedCombatSlots[b].GetLinkedUnit().ToggleSelected(true);
                                            GameManager.Instance.AddUnitsSelected(selectedCombatSlots[b].GetLinkedUnit());
                                        }

                                        if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                                            selectedCombatSlots[b].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                                        unitsTargeted++;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int l = 0; l < allowedCombatSlots.Count; l++)
                    {
                        if (allowedCombatSlots[l].GetLinkedUnit())
                        {
                            if (allowedCombatSlots[l].GetLinkedUnit() == unit && GameManager.Instance.GetActiveSkill().isSelfCast)
                            {
                                combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                allowedCombatSlots[l].ToggleCombatSelected(true);
                                allowedCombatSlots[l].GetLinkedUnit().ToggleSelected(true);
                                GameManager.Instance.AddUnitsSelected(allowedCombatSlots[l].GetLinkedUnit());
                            }
                            else if (allowedCombatSlots[l].GetLinkedUnit() == unit && GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT
                                 && GameManager.Instance.GetActiveSkill().canTargetSelf)
                            {
                                combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                                allowedCombatSlots[l].ToggleCombatSelected(true);
                                allowedCombatSlots[l].GetLinkedUnit().ToggleSelected(true);
                                GameManager.Instance.AddUnitsSelected(allowedCombatSlots[l].GetLinkedUnit());
                                unitsTargeted++;
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
                            }
                        }
                    }
                }
            }
        }


        if (combatSelectedCombatSlots.Count == 0 && unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
        {
            unit.skillRangeIssue = true;
            endTurn = true;
            //break;
        }

        int selectedSlots = 0;
        for (int b = 0; b < combatSelectedCombatSlots.Count; b++)
        {
            if (combatSelectedCombatSlots[b].GetLinkedUnit() &&
                combatSelectedCombatSlots[b].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY &&
                GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE &&
                GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                selectedSlots++;
            }
            else if (combatSelectedCombatSlots[b].GetLinkedUnit() &&
                combatSelectedCombatSlots[b].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER &&
                GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE &&
                GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY)
            {
                selectedSlots++;
            }
            else if (combatSelectedCombatSlots[b].GetLinkedUnit() &&
                combatSelectedCombatSlots[b].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER &&
                GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT &&
                GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                selectedSlots++;
            }
            else if (combatSelectedCombatSlots[b].GetLinkedUnit() &&
                combatSelectedCombatSlots[b].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY &&
                GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT &&
                GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY)
            {
                selectedSlots++;
            }
        }

        GameManager.Instance.ToggleAllowSelection(true);

        if (selectedSlots == 0)
        {
            if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
            {
                StartCoroutine(EndUnitTurnAfterWait(unit));
                return;
            }
        }

        if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
        {
            isCombatMode = true;

            string dir = "";
            for (int i = 0; i < combatSelectedCombatSlots.Count; i++)
            {
                if (combatSelectedCombatSlots[i].GetLinkedUnit())
                {
                    if (combatSelectedCombatSlots[i].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY && GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE &&
                        GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
                    {
                        dir = combatSelectedCombatSlots[i].GetDirection(unit.GetActiveCombatSlot());
                        //if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                        //{
                        combatSelectedCombatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        //GameManager.Instance.AddUnitsSelected(combatSelectedCombatSlots[i].GetLinkedUnit());
                        //}
                    }
                    else if (combatSelectedCombatSlots[i].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER && GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE &&
                        GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY)
                    {
                        dir = combatSelectedCombatSlots[i].GetDirection(unit.GetActiveCombatSlot());
                        //if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                        //{
                        combatSelectedCombatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        //GameManager.Instance.AddUnitsSelected(combatSelectedCombatSlots[i].GetLinkedUnit());
                        //}
                    }
                    else if (combatSelectedCombatSlots[i].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.PLAYER && GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT &&
                        GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
                    {
                        dir = combatSelectedCombatSlots[i].GetDirection(unit.GetActiveCombatSlot());
                        //if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                        //{
                        combatSelectedCombatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        //GameManager.Instance.AddUnitsSelected(combatSelectedCombatSlots[i].GetLinkedUnit());
                        //}
                    }
                    else if (combatSelectedCombatSlots[i].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY && GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT &&
                            GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY)
                    {
                        dir = unit.GetActiveCombatSlot().GetDirection(combatSelectedCombatSlots[i]);
                        //if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                        //{
                        combatSelectedCombatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        //GameManager.Instance.AddUnitsSelected(combatSelectedCombatSlots[i].GetLinkedUnit());
                        // }

                    }
                }
            }

            if (dir == "UpRight" ||
                dir == "DownRight" ||
                dir == "Right")
            {
                unit.UpdateUnitLookDirection(true, true);
            }
            else if (dir == "UpLeft" ||
                dir == "DownLeft" ||
                dir == "Left")
            {
                unit.UpdateUnitLookDirection(true, false);
            }

            GameManager.Instance.ToggleSelectingUnits(false);
            GameManager.Instance.ToggleAllowSelection(false);
            GameManager.Instance.HideMainSlotDetails();
            GameManager.Instance.PlayerAttack();
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

    private void Awake()
    {
        Instance = this;
    }
}
