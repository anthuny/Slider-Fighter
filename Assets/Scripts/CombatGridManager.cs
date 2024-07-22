using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatGridManager : MonoBehaviour
{
    public static CombatGridManager Instance;
    public float unitMoveArrowOnAlpha = 0.75f;

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
    [SerializeField] private ButtonFunctionality buttonAttackMovement;
    [SerializeField] private ButtonFunctionality buttonSkillsItem;

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

    public ButtonFunctionality GetButtonSkillsItems()
    {
        return buttonSkillsItem;
    }

    public ButtonFunctionality GetButtonAttackMovement()
    {
        return buttonAttackMovement;
    }

    public bool GetIsMovementAllowed()
    {
        return isMovementAllowed;
    }

    public void ToggleIsMovementAllowed(bool toggle = true)
    {
        isMovementAllowed = toggle;
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
        UpdateCombatSlotsIndex();
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

                    StartCoroutine(AutoSwapOutOfMovementModeAndLockSkills());
                }

                movingUnit.SetPositionAndParent(GetSelectedCombatSlotMove().transform);

                CheckToUnlinkCombatSlot();

                UpdateUnitMoveRange(movingUnit);

                //GameManager.Instance.UpdateActiveSkill(movingUnit.GetBaseSelectSkill());

                //movingUnit = null;

                //if (movingUnit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                //GameManager.Instance.UpdatePlayerAbilityUI(true);
            }
        }
    }

    IEnumerator AutoSwapOutOfMovementMode()
    {
        yield return new WaitForSeconds(.25f);

        GameManager.Instance.isSkillsMode = true;
        UpdateAttackMovementMode(false, true, true);

        //GetButtonSkillsItems().ButtonCombatItemsTab(false);
    }

    IEnumerator AutoSwapOutOfMovementModeAndLockSkills()
    {
        yield return new WaitForSeconds(.25f);

        GameManager.Instance.isSkillsMode = false;
        GetButtonAttackMovement().ButtonCombatAttackMovement(false);
        //UpdateAttackMovementMode(false, true);      
        GetButtonSkillsItems().ButtonCombatItemsTab(true);        
    }

    public void AutoSelectMovement(UnitFunctionality unit)
    {
        List<CombatSlot> combatSlots = new List<CombatSlot>();

        for (int i = 0; i < GetAllCombatSlots().Count; i++)
        {
            if (GetCombatSlot(i).GetAllowed())
            {
                combatSlots.Add(GetCombatSlot(i));
                //GetCombatSlot(i)
            }
        }

        bool runChosen = false;
        bool chaseChosen = false;
        bool left = false;
        bool up = false;
        int index = 30;
        int range = 30;
        Vector2 targetVector2 = Vector2.zero;

        // If an enemy unit is within alert range of this unit for movement, 
        // and selected skill has a minimum range, try move away from that closest unit
        for (int i = 0; i < GetAllCombatSlots().Count; i++)
        {
            //Vector2 vector2 = combatSlots[i].GetSlotIndex();
            //if (vector2.x

            int xDiff = 0;
            int yDiff = 0;

            if (GetAllCombatSlots()[i].GetLinkedUnit())
            {
                if (GetAllCombatSlots()[i].GetSlotIndex().x > unit.GetActiveCombatSlot().GetSlotIndex().x)
                    xDiff = (int)GetAllCombatSlots()[i].GetSlotIndex().x - (int)unit.GetActiveCombatSlot().GetSlotIndex().x;
                else
                    xDiff = (int)unit.GetActiveCombatSlot().GetSlotIndex().x - (int)GetAllCombatSlots()[i].GetSlotIndex().x;

                if (GetAllCombatSlots()[i].GetSlotIndex().y > unit.GetActiveCombatSlot().GetSlotIndex().y)
                    yDiff = (int)GetAllCombatSlots()[i].GetSlotIndex().y - (int)unit.GetActiveCombatSlot().GetSlotIndex().y;
                else
                    yDiff = (int)unit.GetActiveCombatSlot().GetSlotIndex().y - (int)GetAllCombatSlots()[i].GetSlotIndex().y;

                // Move away from nearest enemy if they're close enough
                if (GameManager.Instance.GetActiveSkill().skillIgnoreRange > 0)
                {
                    if (GetAllCombatSlots()[i].GetLinkedUnit() != unit &&
                        GetAllCombatSlots()[i].GetLinkedUnit().curUnitType != unit.curUnitType)
                    {
                        if (xDiff <= 2 || yDiff <= 2)
                        {
                            if (xDiff > yDiff)
                            {
                                // Get direction of closest unit
                                if (GetAllCombatSlots()[i].GetLinkedUnit().GetActiveCombatSlot().GetSlotIndex().x > unit.GetActiveCombatSlot().GetSlotIndex().x &&
                                    index > (int)GetAllCombatSlots()[i].GetLinkedUnit().GetActiveCombatSlot().GetSlotIndex().x - (int)unit.GetActiveCombatSlot().GetSlotIndex().x)
                                {
                                    index = xDiff;

                                    left = false;
                                }
                                else if (GetAllCombatSlots()[i].GetLinkedUnit().GetActiveCombatSlot().GetSlotIndex().x < unit.GetActiveCombatSlot().GetSlotIndex().x &&
                                    index > (int)GetAllCombatSlots()[i].GetLinkedUnit().GetActiveCombatSlot().GetSlotIndex().x - (int)unit.GetActiveCombatSlot().GetSlotIndex().x)
                                {
                                    index = xDiff;

                                    left = true;
                                }
                            }
                            else
                            {
                                if (GetAllCombatSlots()[i].GetLinkedUnit().GetActiveCombatSlot().GetSlotIndex().y > unit.GetActiveCombatSlot().GetSlotIndex().y &&
                                    index > (int)GetAllCombatSlots()[i].GetLinkedUnit().GetActiveCombatSlot().GetSlotIndex().y - (int)unit.GetActiveCombatSlot().GetSlotIndex().y)
                                {
                                    index = yDiff;

                                    up = false;
                                }
                                else if (GetAllCombatSlots()[i].GetLinkedUnit().GetActiveCombatSlot().GetSlotIndex().y < unit.GetActiveCombatSlot().GetSlotIndex().y &&
                                    index > (int)GetAllCombatSlots()[i].GetLinkedUnit().GetActiveCombatSlot().GetSlotIndex().y - (int)unit.GetActiveCombatSlot().GetSlotIndex().y)
                                {
                                    index = yDiff;

                                    up = true;
                                }
                            }
                        }
                    }
                }
                // Move toward ideal target for skill
                else
                {
                    if (GetAllCombatSlots()[i].GetLinkedUnit() != unit &&
                        GetAllCombatSlots()[i].GetLinkedUnit().curUnitType != unit.curUnitType &&
                        GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE)
                    {
                        if (xDiff > yDiff)
                        {
                            // Get direction of closest unit
                            if (GetAllCombatSlots()[i].GetLinkedUnit().GetActiveCombatSlot().GetSlotIndex().x > unit.GetActiveCombatSlot().GetSlotIndex().x &&
                                index > (int)GetAllCombatSlots()[i].GetLinkedUnit().GetActiveCombatSlot().GetSlotIndex().x - (int)unit.GetActiveCombatSlot().GetSlotIndex().x)
                            {
                                index = xDiff;

                                left = true;
                            }
                            else if (GetAllCombatSlots()[i].GetLinkedUnit().GetActiveCombatSlot().GetSlotIndex().x < unit.GetActiveCombatSlot().GetSlotIndex().x &&
                                index > (int)GetAllCombatSlots()[i].GetLinkedUnit().GetActiveCombatSlot().GetSlotIndex().x - (int)unit.GetActiveCombatSlot().GetSlotIndex().x)
                            {
                                index = xDiff;

                                left = false;
                            }
                        }
                        else
                        {
                            if (GetAllCombatSlots()[i].GetLinkedUnit().GetActiveCombatSlot().GetSlotIndex().y > unit.GetActiveCombatSlot().GetSlotIndex().y &&
                                index > (int)GetAllCombatSlots()[i].GetLinkedUnit().GetActiveCombatSlot().GetSlotIndex().y - (int)unit.GetActiveCombatSlot().GetSlotIndex().y)
                            {
                                index = yDiff;

                                up = true;
                            }
                            else if (GetAllCombatSlots()[i].GetLinkedUnit().GetActiveCombatSlot().GetSlotIndex().y < unit.GetActiveCombatSlot().GetSlotIndex().y &&
                                index > (int)GetAllCombatSlots()[i].GetLinkedUnit().GetActiveCombatSlot().GetSlotIndex().y - (int)unit.GetActiveCombatSlot().GetSlotIndex().y)
                            {
                                index = yDiff;

                                up = false;
                            }
                        }
                    }
                    else if (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT)
                    {


                        // Move towards nearest ally?
                        for (int a = 0; a < GameManager.Instance.activeRoomAllUnitFunctionalitys.Count; a++)
                        {
                            if (GameManager.Instance.activeRoomAllUnitFunctionalitys[a] != unit)
                            {
                                if (range > GameManager.Instance.activeRoomAllUnitFunctionalitys[a].GetRangeToUnit(unit))
                                {
                                    range = GameManager.Instance.activeRoomAllUnitFunctionalitys[a].GetRangeToUnit(unit);
                                    targetVector2 = GameManager.Instance.activeRoomAllUnitFunctionalitys[a].GetActiveCombatSlot().GetSlotIndex();
                                }
                            }
                        }
                    }
                }
            }
        }

        List<CombatSlot> availableCombatSlots = new List<CombatSlot>();

        if (range != 30)
        {
            availableCombatSlots.Add(GetCombatSlot(targetVector2));
        }

        for (int x = 0; x < combatSlots.Count; x++)
        {
            if (left)
            {
                if (combatSlots[x].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x - 1 &&
                    combatSlots[x].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y)
                {
                    // Select combatSlots[x] GOING LEFT 

                    availableCombatSlots.Add(combatSlots[x]);
                    //combatSlots[x].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                }
            }
            else
            {
                if (combatSlots[x].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x + 1 &&
                    combatSlots[x].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y)
                {
                    // Select combatSlots[x] GOING RIGHT 
                    availableCombatSlots.Add(combatSlots[x]);
                    //combatSlots[x].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                }
            }

            if (up)
            {
                if (combatSlots[x].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x &&
                    combatSlots[x].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y + 1)
                {
                    // Select combatSlots[x] GOING UP 
                    availableCombatSlots.Add(combatSlots[x]);
                    //combatSlots[x].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                }
            }
            else
            {
                if (combatSlots[x].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x &&
                    combatSlots[x].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y - 1)
                {
                    // Select combatSlots[x] GOING DOWN 
                    availableCombatSlots.Add(combatSlots[x]);
                    //combatSlots[x].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                }
            }
        }

        if (availableCombatSlots.Count != 0)
        {
            for (int i = 0; i < 10; i++)
            {
                int rand = Random.Range(0, availableCombatSlots.Count);

                if (availableCombatSlots[rand])
                {
                    if (availableCombatSlots[rand].GetLinkedUnit() != null)
                    {
                        //if (i > 0)
                        //    i--;
                        continue;
                    }
                    else
                    {
                        isCombatMode = false;
                        availableCombatSlots[rand].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
                        break;
                    }
                }
                else
                    break;
            }
        }
        else
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
                    break;
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

        if (unitMovementRange <= 0 && unit.usedExtraMove)
            return;

        UnselectAllSelectedCombatSlots();

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
                    && GetCombatSlot(new Vector2(unitCombatIndex.x , unitCombatIndex.y + x)).GetLinkedUnit() == null)
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

            // Target self slot if skill can target self slot
            if (GameManager.Instance.GetActiveSkill().canTargetSelf)
            {
                if (allCombatSlots[i].GetSlotIndex() == unitCombatIndex)
                {
                    combatSlot = allCombatSlots[i];
                    combatSlot.ToggleSlotAllowed(true);
                }
            }

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
                // If combat slot is an ignored slot, un allow it
                if (xDiff <= GameManager.Instance.GetActiveSkill().skillIgnoreRange &&
                    yDiff <= GameManager.Instance.GetActiveSkill().skillIgnoreRange)
                {
                    combatSlot = allCombatSlots[i];
                    combatSlot.ToggleSlotAllowed(false);
                }
                // If combat slot should be an allowed slot, make it
                else
                {
                    combatSlot = allCombatSlots[i];
                    combatSlot.ToggleSlotAllowed(true);
                }
            }
        }

        UpdateUnitAttackHitArea(unit);
    }

    public void UpdateUnitAttackHitArea(UnitFunctionality unit)
    {
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
                // For a 1x1 target skill
                if (GameManager.Instance.GetActiveSkill().skillRangeHitArea.x == 1 &&
                   GameManager.Instance.GetActiveSkill().skillRangeHitArea.y == 1)
                {
                    allowedCombatSlots.Add(allCombatSlots[i]);
                }
                // For a skill that targets all allowed slots
                else if (GameManager.Instance.GetActiveSkill().attackAllSelected)
                {
                    allowedCombatSlots.Add(allCombatSlots[i]);
                }
            }
        }

        List<CombatSlot> combatSelectedCombatSlots = new List<CombatSlot>();

        int count = 0;
        if (!GameManager.Instance.GetActiveSkill().attackAllSelected)
            count = (int)GameManager.Instance.GetActiveSkill().skillRangeHitArea.x;
        else
            count = 1;

        bool endTurn = false;

        for (int i = 0; i < count; i++)
        {
            if (!GameManager.Instance.GetActiveSkill().attackAllSelected)
            {
                for (int l = 0; l < allowedCombatSlots.Count; l++)
                {
                    bool enemy = false;
                    if (allowedCombatSlots[l].GetLinkedUnit() != null &&
                        !allowedCombatSlots[l].GetLinkedUnit().isDead &&
                        allowedCombatSlots[l].GetLinkedUnit() != unit)
                    {
                        if (allowedCombatSlots[l].GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY)
                            enemy = true;
                        else
                            enemy = false;

                        if (enemy && GameManager.Instance.GetActiveSkill().curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.ENEMIES)
                        {
                            combatSelectedCombatSlots.Add(allowedCombatSlots[l]);

                            if (GameManager.Instance.GetActiveSkill().skillRangeHitArea.x == 1 &&
                                GameManager.Instance.GetActiveSkill().skillRangeHitArea.y == 1 &&
                                !GameManager.Instance.GetActiveSkill().attackAllUnits)
                            {
                                break;
                            }
                        }
                        else if (!enemy && GameManager.Instance.GetActiveSkill().curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.PLAYERS)
                        {
                            combatSelectedCombatSlots.Add(allowedCombatSlots[l]);

                            if (GameManager.Instance.GetActiveSkill().skillRangeHitArea.x == 1 &&
                                GameManager.Instance.GetActiveSkill().skillRangeHitArea.y == 1 &&
                                !GameManager.Instance.GetActiveSkill().attackAllUnits)
                            {
                                break;
                            }
                        }
                    }
                }

                if (combatSelectedCombatSlots.Count == 0 && unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                {
                    unit.skillRangeIssue = true;
                    endTurn = true;
                    break;
                }
            }
            else
            {
                for (int l = 0; l < allowedCombatSlots.Count; l++)
                {
                    combatSelectedCombatSlots.Add(allowedCombatSlots[l]);
                }
            }
        }

        if (endTurn)
        {
            StartCoroutine(EndUnitTurnAfterWait(unit));
            return;
        }

        for (int i = 0; i < combatSelectedCombatSlots.Count; i++)
        {
            combatSelectedCombatSlots[i].ToggleCombatSelected(true);

            if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                combatSelectedCombatSlots[i].GetComponentInChildren<ButtonFunctionality>().ButtonSelectCombatSlot(true);
        }
        // Toggle Combat unit slot available around them for movement

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
        yield return new WaitForSeconds(.35f);

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
