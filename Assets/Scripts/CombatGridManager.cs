using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System.Linq;
using System;

public class CombatGridManager : MonoBehaviour
{
    public static CombatGridManager Instance;


    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public float unitMoveArrowOnAlpha = 0.75f;

    [SerializeField] private List<CombatSlot> aimTargetCombatSlots = new List<CombatSlot>();

    public List<CombatSlot> targetedCombatSlots = new List<CombatSlot>();
    public Animation combatSlotIdle;
    public Animation combatSlotAttackIdle;

    public Color slotAllowedColour;
    public Color slotNotAllowedColour;
    public Color slotDisabledColour;
    public Color slotSelectedColour;
    public Color slotMovementSelectedColour;
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
    [SerializeField] private List<CombatSlot> fighterShopCombatSlots = new List<CombatSlot>();

    [SerializeField] private List<CombatSlot> fighterCombatSlots = new List<CombatSlot>();

    [SerializeField] private List<CombatSlot> enemySpawnCombatSlots = new List<CombatSlot>();

    public bool isCombatMode = false;
    [SerializeField] private bool isMovementAllowed = true;

    public Transform gridParent;
    [SerializeField] private float camZoomAmount = 15f;
    [SerializeField] private float camZoomMin = 15f;
    [SerializeField] private float camZoomMax = 15f;
    [SerializeField] private UIElement combatUIElement;
    [SerializeField] private GraphicRaycaster graycaster;
    [SerializeField] private Camera mainCam;
    [SerializeField] private CinemachineVirtualCamera virtCam;
    [SerializeField] private UIElement scaleButton;
    [SerializeField] private UIElement deScaleButton;

    [SerializeField] private Transform scalingButtonParent;
    [SerializeField] private Transform TopScalingLocation;
    [SerializeField] private Transform BotScalingLocation;
    public List<CombatSlot> finalPath = new List<CombatSlot>();
    List<CombatSlot> openList = new List<CombatSlot>();
    List<CombatSlot> closedList = new List<CombatSlot>();

    public bool spawnedGhostTiles = false;
    bool moved = false;

    public void ResetVirtCam()
    {
        virtCam.Follow = null;
    }

    public List<CombatSlot> FindPath(CombatSlot start, CombatSlot end)
    {
        CombatSlot startSlot = start;

        openList = new List<CombatSlot> { start };
        closedList = new List<CombatSlot>();

        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            allCombatSlots[i].gCost = int.MaxValue;
            allCombatSlots[i].CalculateFCost();

            allCombatSlots[i].previousSlot = null;
        }

        start.gCost = 0;
        start.hCost = GetDistance(start, end);
        startSlot.CalculateFCost();

        while (openList.Count > 0)
        {
            CombatSlot curSlot = GetLowestFCostSlot(openList);
            if (curSlot == end)
            {
                return CalculatePath(end);
            }

            openList.Remove(curSlot);
            closedList.Add(curSlot);

            foreach (CombatSlot neighbour in GetNeighbourList(curSlot))
            {
                if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
                {
                    if (closedList.Contains(neighbour) || neighbour.GetLinkedUnit() || !neighbour.walkable)
                        continue;
                }
                else
                {
                    if (neighbour != end)
                    {
                        if (closedList.Contains(neighbour) || neighbour.GetLinkedUnit() || !neighbour.walkable)
                            continue;
                    }

                }

                int tentativeGCost = curSlot.gCost + GetDistance(curSlot, neighbour);
                if (tentativeGCost < neighbour.gCost)
                {
                    neighbour.previousSlot = curSlot;
                    neighbour.gCost = tentativeGCost;
                    neighbour.hCost = GetDistance(neighbour, end);
                    neighbour.CalculateFCost();

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }

        // out of nodes on the openlist
        return null;
    }

    private List<CombatSlot> CalculatePath(CombatSlot endSlot)
    {
        List<CombatSlot> path = new List<CombatSlot>();
        path.Add(endSlot);
        CombatSlot curSlot = endSlot;
        
        while (curSlot.previousSlot != null)
        {
            path.Add(curSlot.previousSlot);
            curSlot = curSlot.previousSlot;
        }

        path.Reverse();
        return path;
    }

    private int GetDistance(CombatSlot start, CombatSlot neighbour)
    {
        int xDistance = 0;
        int yDistance = 0;
        int remaining = 0;
        if (start && neighbour)
        {
            xDistance = (int)Mathf.Abs(start.GetSlotIndex().x - neighbour.GetSlotIndex().x);
            yDistance = (int)Mathf.Abs(start.GetSlotIndex().y - neighbour.GetSlotIndex().y);
            remaining = Mathf.Abs(xDistance - yDistance);
        }
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private CombatSlot GetLowestFCostSlot(List<CombatSlot> pathList)
    {
        CombatSlot lowestFCostNode = pathList[0];

        for (int i = 0; i < pathList.Count; i++)
        {
            if (pathList[i].fCost < lowestFCostNode.fCost)
                lowestFCostNode = pathList[i];
        }

        return lowestFCostNode;
    }


    private List<CombatSlot> GetNeighbourList(CombatSlot curSlot)
    {
        List<CombatSlot> neighbours = new List<CombatSlot>();

        if (curSlot.GetSlotIndex().x - 1 >= 0)
        {
            // Left
            neighbours.Add(GetCombatSlot(new Vector2(curSlot.GetSlotIndex().x - 1, curSlot.GetSlotIndex().y)));
            // Left Down
            if (curSlot.GetSlotIndex().y - 1 >= 0)
            {
                neighbours.Add(GetCombatSlot(new Vector2(curSlot.GetSlotIndex().x - 1, curSlot.GetSlotIndex().y - 1)));
            }
            // Left up
            if (curSlot.GetSlotIndex().y < 18)
            {
                neighbours.Add(GetCombatSlot(new Vector2(curSlot.GetSlotIndex().x - 1, curSlot.GetSlotIndex().y + 1)));
            }
        }

        if (curSlot.GetSlotIndex().x + 1 <= 15)
        {
            // Right
            neighbours.Add(GetCombatSlot(new Vector2(curSlot.GetSlotIndex().x + 1, curSlot.GetSlotIndex().y)));
            // Right Down
            if (curSlot.GetSlotIndex().y - 1 >= 0)
            {
                neighbours.Add(GetCombatSlot(new Vector2(curSlot.GetSlotIndex().x + 1, curSlot.GetSlotIndex().y - 1)));
            }
            // Right up
            if (curSlot.GetSlotIndex().y + 1 <= 18)
            {
                neighbours.Add(GetCombatSlot(new Vector2(curSlot.GetSlotIndex().x + 1, curSlot.GetSlotIndex().y + 1)));
            }
        }

        // Down
        if (curSlot.GetSlotIndex().y - 1 >= 0)
        {
            neighbours.Add(GetCombatSlot(new Vector2(curSlot.GetSlotIndex().x, curSlot.GetSlotIndex().y +- 1)));
        }
        // Up
        if (curSlot.GetSlotIndex().y + 1 <= 18)
        {
            neighbours.Add(GetCombatSlot(new Vector2(curSlot.GetSlotIndex().x, curSlot.GetSlotIndex().y + 1)));
        }

        return neighbours;
    }

    public void ToggleScaleButtons(bool toggle = true)
    {
        if (toggle)
        {
            scaleButton.UpdateAlpha(1);
            scaleButton.ToggleButton(true);
            deScaleButton.UpdateAlpha(1);
            deScaleButton.ToggleButton(true);

            if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            {
                scalingButtonParent.SetParent(TopScalingLocation);
                scalingButtonParent.localPosition = Vector3.zero;
            }
            else
            {
                scalingButtonParent.SetParent(BotScalingLocation);
                scalingButtonParent.localPosition = Vector3.zero;
            }
        }
        else
        {
            scaleButton.UpdateAlpha(0);
            scaleButton.ToggleButton(false);
            deScaleButton.UpdateAlpha(0);
            deScaleButton.ToggleButton(false);

            scalingButtonParent.SetParent(BotScalingLocation);
            scalingButtonParent.localPosition = Vector3.zero;
        }
    }

    public bool doingCoroutine = false;

    Coroutine coroutine = null;
    public void UpdateCameraToUnit(UnitFunctionality unit = null, bool attack = false)
    {
        // If unit is more then 2 tiles away from existing target, do not add new unit
        if (targetedCombatSlots.Count > 0)
        {
            for (int i = 0; i < targetedCombatSlots.Count; i++)
            {
                if (targetedCombatSlots[i].GetRangeFromActiveCombatSlot(GameManager.Instance.GetActiveUnitFunctionality().GetActiveCombatSlot()) <= 2)
                    return;

            }
        }

        if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
            GetComponent<ScrollRect>().enabled = false;


        GridTargetGroup.Instance.AddTarget(unit);
        virtCam.Follow = GridTargetGroup.Instance.GetTargetGroupTrans();
        if (!attack)
        {
            if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
                StartCoroutine(StopFollowingUnit());

        }

    }

    IEnumerator StopFollowingUnit()
    {
        //doingCoroutine = true;
        yield return new WaitForSeconds(.35f);

        GetComponent<ScrollRect>().enabled = true;
        virtCam.Follow = null;
        //doingCoroutine = false;
    }
    public void ResetVCamera()
    {
        virtCam.ForceCameraPosition(Vector3.zero, Quaternion.identity);
    }

    public void UpdateGridScale(bool inc = true, UIElement ui = null)
    {
        if (moved)
            return;

        if (ui)
        {
            ui.AnimateUI(false);
        }

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        if (inc)
        {
            if (gridParent.localScale.x < camZoomMax)
            {
                gridParent.localScale = new Vector3(gridParent.localScale.x + camZoomAmount, gridParent.localScale.y + camZoomAmount, 0);
                AudioManager.Instance.Play("SFX_CameraZoomIn");
            }
            else
            {
                AudioManager.Instance.Play("SFX_ShopBuyFail");
            }
        }
        else
        {
            if (gridParent.localScale.x > camZoomMin)
            {
                gridParent.localScale = new Vector3(gridParent.localScale.x - camZoomAmount, gridParent.localScale.y - camZoomAmount, 0);
                AudioManager.Instance.Play("SFX_CameraZoomOut");
            }
            else
            {
                AudioManager.Instance.Play("SFX_ShopBuyFail");
            }
        }
    }



    public void ToggleCombatUIElement(bool toggle = true)
    {
        combatUIElement.GetComponent<CanvasGroup>().interactable = toggle;
        combatUIElement.GetComponent<CanvasGroup>().blocksRaycasts = toggle;
        graycaster.enabled = toggle;
    }

    public void ToggleCombatSlotsInput(bool toggle = true)
    {
        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            allCombatSlots[i].ToggleCombatSlotInput(toggle);
            allCombatSlots[i].buttonUI.ToggleButton(toggle);
            if (toggle)
                allCombatSlots[i].buttonUI.UpdateAlpha(1);
            else
                allCombatSlots[i].buttonUI.UpdateAlpha(0);
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

    public void ToggleTabButtons(string tabName = "", bool flag = false)
    {
        if (!GameManager.Instance.GetActiveUnitFunctionality())
        {
            ToggleButton(GetButtonAttack(), false, false);
            ToggleButton(GetButtonSkills(), false, false);
            ToggleButton(GetButtonMovement(), false, false);
            ToggleButton(GetButtonItems(), false, false);
            return;
        }

        if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY || flag)
        {
            ToggleButton(GetButtonAttack(), false, false);
            ToggleButton(GetButtonSkills(), false, false);
            ToggleButton(GetButtonMovement(), false, false);
            ToggleButton(GetButtonItems(), false, false);
            return;
        }
        else
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


                if (!GameManager.Instance.GetActiveUnitFunctionality().reanimated)
                    ToggleButton(GetButtonAttack(), true, true);
                else
                {
                    if (GameManager.Instance.GetActiveUnitFunctionality().hasAttacked)
                        ToggleButton(GetButtonAttack(), false, true);
                    else
                        ToggleButton(GetButtonAttack(), true, true);
                }
            }
            else if (tabName == "Items")
            {
                // Toggle end turn button on when it should be
                if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
                {
                    bool hasItem = false;

                    if (GameManager.Instance.GetActiveUnitFunctionality().teamIndex == 0)
                    {
                        for (int z = 0; z < OwnedLootInven.Instance.GetWornItemMainAlly().Count; z++)
                        {
                            if (OwnedLootInven.Instance.GetWornItemMainAlly()[z])
                            {
                                //If this item of fighter is an active item, do nothing, allow end button to remain, because turn shouldnt auto end.
                                if (OwnedLootInven.Instance.GetWornItemMainAlly()[z].linkedItemPiece.curActiveType == ItemPiece.ActiveType.ACTIVE
                                    && OwnedLootInven.Instance.GetWornItemMainAlly()[z].GetCalculatedItemsUsesRemaining2() > 0)
                                {
                                    hasItem = true;
                                    break;
                                }
                            }
                        }
                    }
                    else if (GameManager.Instance.GetActiveUnitFunctionality().teamIndex == 1)
                    {
                        for (int e = 0; e < OwnedLootInven.Instance.GetWornItemSecondAlly().Count; e++)
                        {
                            if (OwnedLootInven.Instance.GetWornItemSecondAlly()[e])
                            {
                                //If this item of fighter is an active item, do nothing, allow end button to remain, because turn shouldnt auto end.
                                if (OwnedLootInven.Instance.GetWornItemSecondAlly()[e].linkedItemPiece.curActiveType == ItemPiece.ActiveType.ACTIVE
                                    && OwnedLootInven.Instance.GetWornItemSecondAlly()[e].GetCalculatedItemsUsesRemaining2() > 0)
                                {
                                    hasItem = true;
                                    break;
                                }
                            }
                        }
                    }
                    else if (GameManager.Instance.GetActiveUnitFunctionality().teamIndex == 2)
                    {
                        for (int f = 0; f < OwnedLootInven.Instance.GetWornItemThirdAlly().Count; f++)
                        {
                            if (OwnedLootInven.Instance.GetWornItemThirdAlly()[f])
                            {
                                //If this item of fighter is an active item, do nothing, allow end button to remain, because turn shouldnt auto end.
                                if (OwnedLootInven.Instance.GetWornItemThirdAlly()[f].linkedItemPiece.curActiveType == ItemPiece.ActiveType.ACTIVE
                                    && OwnedLootInven.Instance.GetWornItemThirdAlly()[f].GetCalculatedItemsUsesRemaining2() > 0)
                                {
                                    hasItem = true;
                                    break;
                                }
                            }
                        }
                    }
                    //GameManager.Instance.ResetSelectedUnits();

                    if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER
                        && GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() == 0
                        && GameManager.Instance.GetActiveUnitFunctionality().hasAttacked
                        && !hasItem
                        && !GameManager.Instance.GetActiveUnitFunctionality().reanimated)
                    {
                        StartCoroutine(GameManager.Instance.GetActiveUnitFunctionality().UnitEndTurn(true, true));
                        //GameManager.Instance.ToggleEndTurnButton(true);
                    }
                    else if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER
                        && GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() <= -1
                        && !GameManager.Instance.GetActiveUnitFunctionality().hasAttacked
                        && !hasItem
                        && !GameManager.Instance.GetActiveUnitFunctionality().reanimated)
                    {
                        StartCoroutine(GameManager.Instance.GetActiveUnitFunctionality().UnitEndTurn(true, true));
                        //GameManager.Instance.ToggleEndTurnButton(true);
                    }
                    else
                    {
                        GameManager.Instance.ToggleEndTurnButton(true);
                        GameManager.Instance.UpdateMainIconDetails(null, GameManager.Instance.GetActiveItem());
                    }

                    ToggleButton(GetButtonItems(), false, true);
                    ToggleButton(GetButtonAttack(), false, true);

                    if (GameManager.Instance.GetActiveUnitFunctionality().hasAttacked || GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() <= -1)
                        ToggleButton(GetButtonSkills(), false, true);
                    else
                        ToggleButton(GetButtonSkills(), true, true);

                    if (GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() > 0)
                        ToggleButton(GetButtonMovement(), true, true);
                    else if (GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() <= 0)
                    {
                        if (GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() == 0 && GameManager.Instance.GetActiveUnitFunctionality().hasAttacked)
                            ToggleButton(GetButtonMovement(), false, true);
                        else if (GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() < 0 && !GameManager.Instance.GetActiveUnitFunctionality().hasAttacked)
                            ToggleButton(GetButtonMovement(), false, true);
                    }
                }
            }
            else if (tabName == "Skills")
            {
                ToggleButton(GetButtonAttack(), false, true);
                ToggleButton(GetButtonSkills(), false, true);

                if (GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() > -1)
                    ToggleButton(GetButtonMovement(), true, true);
                else if (GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() <= 0)
                {
                    if (GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() == 0 && GameManager.Instance.GetActiveUnitFunctionality().hasAttacked)
                        ToggleButton(GetButtonMovement(), false, true);
                    else if (GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() < 0 && !GameManager.Instance.GetActiveUnitFunctionality().hasAttacked)
                        ToggleButton(GetButtonMovement(), false, true);
                }

                if (!GameManager.Instance.GetActiveUnitFunctionality().reanimated)
                    ToggleButton(GetButtonItems(), true, true);
                else
                    ToggleButton(GetButtonItems(), false, true);

                if (!GameManager.Instance.GetActiveUnitFunctionality().hasAttacked)
                    GameManager.Instance.ToggleEndTurnButton(true);
            }
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
                {
                    button.GetComponent<UIElement>().ToggleButton(false);
                    button.GetComponent<UIElement>().UpdateAlpha(0);
                    return;
                }
            }


            button.GetComponent<UIElement>().ToggleButton(true);
            button.GetComponent<UIElement>().UpdateAlpha(1);
        }
        else
        {
            if (GameManager.Instance.GetActiveUnitFunctionality())
            {
                if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY)
                {
                    if (button == buttonSkills || button == buttonItems || button == buttonAttack || button == buttonMovement)
                    {
                        button.GetComponent<UIElement>().ToggleButton(false);
                        button.GetComponent<UIElement>().UpdateAlpha(0);
                        return;
                    }

                }
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
            else
            {
                button.GetComponent<UIElement>().ToggleButton(false);
                button.GetComponent<UIElement>().UpdateAlpha(0);
            }
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
                //allCombatSlots[i].ResetAnimation();
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
            ToggleAllCombatSlotOutlines();
            // Disable extra move prompt
            OverlayUI.Instance.extraMovePrompt.UpdateAlpha(0);

            //if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER || GameManager.Instance.GetActiveUnitFunctionality().reanimated)
                //GameManager.Instance.ToggleSkillsItemToggleButton(true);

            UnselectAllSelectedCombatSlots();

            if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER || GameManager.Instance.GetActiveUnitFunctionality().reanimated)
            {
                //GameManager.Instance.SetupPlayerUI();
            }

            if (GameManager.Instance.isSkillsMode)
            {
                OverlayUI.Instance.ToggleAllStats(true, true, false);

                if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER || GameManager.Instance.GetActiveUnitFunctionality().reanimated)
                    GameManager.Instance.UpdatePlayerAbilityUI(true);

                //if (GameManager.Instance.GetActiveSkill() && GameManager.Instance.GetActiveItem())
                    //GameManager.Instance.UpdateMainIconDetails(GameManager.Instance.GetActiveSkill(), GameManager.Instance.GetActiveItem());
                //else if (GameManager.Instance.GetActiveSkill())
                    //GameManager.Instance.UpdateMainIconDetails(GameManager.Instance.GetActiveSkill(), null);
            }
            else
            {
                OverlayUI.Instance.ToggleAllStats(true, false, false);

                if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER || GameManager.Instance.GetActiveUnitFunctionality().reanimated)
                    GameManager.Instance.UpdatePlayerAbilityUI(false);

                if (GameManager.Instance.GetActiveItem())
                    GameManager.Instance.UpdateMainIconDetails(null, GameManager.Instance.GetActiveItem());
            }

            isCombatMode = true;
            GameManager.Instance.UpdateDetailsBanner();
            ResetCombatSlotMovementSelected();
        }
        else
        {
            ToggleAllCombatSlotOutlines();
            // Disable rarity bg for skills + race icon
            GameManager.Instance.fighterMainSlot1.UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);
            GameManager.Instance.fighterMainSlot2.UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);
            GameManager.Instance.fighterMainSlot3.UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);
            GameManager.Instance.fighterMainSlot4.UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);

            GameManager.Instance.ToggleSkillsItemToggleButton(false);

            if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER || GameManager.Instance.GetActiveUnitFunctionality().reanimated)
                GameManager.Instance.UpdatePlayerAbilityUI(false, false, true);

            GameManager.Instance.UpdateMainIconDetails(null, null);

            if (GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() > 0)
                OverlayUI.Instance.ToggleAllStats(true, false, true);
            else
                OverlayUI.Instance.ToggleAllStats(false, false, true);

            UpdateUnitMoveRange(GameManager.Instance.GetActiveUnitFunctionality());

            // Enable extra move prompt if unit has 0 moves left
            if (GameManager.Instance.GetActiveUnitFunctionality().GetCurMovementUses() == 0)
                OverlayUI.Instance.extraMovePrompt.UpdateAlpha(1);
            else
                OverlayUI.Instance.extraMovePrompt.UpdateAlpha(0);

            isCombatMode = false;

            GameManager.Instance.UpdateDetailsBanner();
            //ToggleAllCombatSlotOutlines();
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

    public CombatSlot GetFighterShopCombatSlots(int index = 0)
    {
        return fighterShopCombatSlots[index];
    }

    public List<CombatSlot> GetFighterCombatSlots(int index = 0)
    {
        return fighterCombatSlots;
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
            allCombatSlots[i].ToggleSlotAllowed(false, false);
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
        if (virtCam.transform.localPosition.z != 5)
            virtCam.transform.localPosition = new Vector3(virtCam.transform.localPosition.x, virtCam.transform.localPosition.y, 5);

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
                moved = false;
                GameManager.Instance.UpdateAllUnitStatBars();
                ToggleIsMovementAllowed(true);


                // Update unit look direction
                //movingUnit.UpdateUnitLookDirection();

                if (movingUnit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                    UpdateCameraToUnit(movingUnit, true);
                else
                    UpdateCameraToUnit(movingUnit, false);


                movingUnit.skill1OutOfRange = false;
                movingUnit.skill2OutOfRange = false;
                movingUnit.skill3OutOfRange = false;
                movingUnit.skill4OutOfRange = false;
            }

            if (movingUnit.reanimated)
                ToggleButton(GetButtonItems(), false, true);
        }
    }

    IEnumerator AutoSwapOutOfMovementMode()
    {
        yield return new WaitForSeconds(0);

        UnitFunctionality unit = movingUnit;

        if (unit != GameManager.Instance.GetActiveUnitFunctionality())
            yield break;

        if (movingUnit)
        {
            if (movingUnit.reanimated)
                ToggleButton(GetButtonItems(), false, true);
        }

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
                if (!GameManager.Instance.GetActiveUnitFunctionality().reanimated)
                    StartCoroutine(unit.UnitEndTurn(true));
            }

            if (unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                ToggleTabButtons("Items");

            if (unit.reanimated)
                StartCoroutine(unit.UnitEndTurn(true));
        }

        if (movingUnit)
        {
            if (movingUnit.reanimated)
                ToggleButton(GetButtonItems(), false, true);
        }
    }

    IEnumerator AutoSwapOutOfMovementModeAndLockSkills()
    {
        if (movingUnit.reanimated)
            ToggleButton(GetButtonItems(), false, true);

        yield return new WaitForSeconds(.25f);

        ToggleAllCombatSlotOutlines();
        UnselectAllSelectedCombatSlots();
        RemoveAllCombatSelectedCombatSlots();

        //GetButtonAttack().ButtonCombatAttackTab();
        //UpdateAttackMovementMode(false, true);      
        //GetButtonItems().ButtonCombatItemTab();

        ToggleButton(GetButtonSkills(), false, true);
        ToggleButton(GetButtonMovement(), false, true);




        int count = 0;


        if (GameManager.Instance.GetActiveUnitFunctionality().teamIndex == 0)
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
            }
        }
        else if (GameManager.Instance.GetActiveUnitFunctionality().teamIndex == 1)
        {
            for (int x = 0; x < OwnedLootInven.Instance.GetWornItemSecondAlly().Count; x++)
            {
                if (OwnedLootInven.Instance.GetWornItemSecondAlly()[x].GetCalculatedItemsUsesRemaining2() > 0
                    && OwnedLootInven.Instance.GetWornItemSecondAlly()[x].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                {
                    count++;
                    continue;
                }
            }

            if (count == 0)
            {
                StartCoroutine(EndUnitTurnAfterWait(movingUnit));
            }
        }
        else if (GameManager.Instance.GetActiveUnitFunctionality().teamIndex == 2)
        {
            for (int x = 0; x < OwnedLootInven.Instance.GetWornItemThirdAlly().Count; x++)
            {
                if (OwnedLootInven.Instance.GetWornItemThirdAlly()[x].GetCalculatedItemsUsesRemaining2() > 0
                    && OwnedLootInven.Instance.GetWornItemThirdAlly()[x].linkedItemPiece.curItemCombatType == ItemPiece.ItemCombatType.CONSUMABLE)
                {
                    count++;
                    continue;
                }
            }

            if (count == 0)
            {
                StartCoroutine(EndUnitTurnAfterWait(movingUnit));
            }
        }

        if (count != 0)
        {
            GameManager.Instance.UpdateMainIconDetails(null, GameManager.Instance.GetActiveItem());
            GameManager.Instance.UpdatePlayerAbilityUI(false);
            UpdateAttackMovementMode(false, true);
            GetButtonItems().ButtonCombatItemTab();
            GameManager.Instance.ToggleMainSlotVisibility(true);
            GameManager.Instance.skillIconsUI.UpdateAlpha(1);

            OverlayUI.Instance.ToggleAllStats(true, false, false);

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
        if (combatSlotA.GetRangeFromActiveCombatSlot(combatSlotA) < combatSlotB.GetRangeFromActiveCombatSlot(combatSlotB))
            return 1;
        if (combatSlotA.GetRangeFromActiveCombatSlot(combatSlotA) > combatSlotB.GetRangeFromActiveCombatSlot(combatSlotB))
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
                int rand = UnityEngine.Random.Range(0, combatSlots.Count-1);
                //Debug.Log("rand = " + rand);

                if (rand > combatSlots.Count - 1)
                {
                    unit.StartCoroutine(unit.UnitEndTurn(true));
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
            if (GetTargetCombatSlots()[0].GetLinkedUnit())
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
                //OverlayUI.Instance.ToggleFighterDetailsTab(true);
            }
            else
            {
                // Send to items
                //GameManager.Instance.SetupPlayerUI();
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
        if (unit.GetCurMovementUses() > 0 && !unit.enemyMoved)
        {
            unit.UnitMove();
        }

        //UpdateAttackSelection(unit);

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
                            unit.StartCoroutine(unit.UnitEndTurn(true));
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
                                    unit.StartCoroutine(unit.UnitEndTurn(true));
                                    //UpdateUnitMoveRange(unit);
                                    return;
                                }
                            }
           

                            if (hold && unit.hasAttacked)
                            {
                                unit.StartCoroutine(unit.UnitEndTurn(true));
                                //UpdateUnitMoveRange(unit);
                                return;
                            }

                            // If target is self
                            if (GetTargetCombatSlots()[0].GetSlotIndex().x == unit.GetActiveCombatSlot().GetSlotIndex().x &&
                                GetTargetCombatSlots()[0].GetSlotIndex().y == unit.GetActiveCombatSlot().GetSlotIndex().y)
                            {
                                MoveRandomly(unit, GetTargetCombatSlots());
                                break;
                            }
                            else
                            {
                                SetGhostTiles(unit);
                                break;
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
                    unit.StartCoroutine(unit.UnitEndTurn(true));
                    return;
                }
            }

            if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY &&
                unit.hasAttacked && slotsInRange > 0 && !allowMovement)
            {
                unit.StartCoroutine(unit.UnitEndTurn(true));
                return;
            }
        }
        else if (unit.hasAttacked && unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
        {
            unit.StartCoroutine(unit.UnitEndTurn(true));
        }
    }

    public void SetGhostTiles(UnitFunctionality unit)
    {
        ResetCombatSlotMovementSelected();

        if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
            finalPath = FindPath(unit.GetActiveCombatSlot(), GetTargetCombatSlots()[0]);
        else
            finalPath = FindPath(unit.GetActiveCombatSlot(), GetSelectedCombatSlotMove());

        if (finalPath.Count > 0)
            finalPath.RemoveAt(0);


        for (int i = 0; i < finalPath.Count; i++)
        {
            if (finalPath[i].GetLinkedUnit())
            {
                finalPath.RemoveAt(i);
                break;
            }

            if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY && i + 1 == unit.GetCurMovementUses())
                UpdateSelectedCombatSlotMove(finalPath[i]);

            if (i < unit.GetCurMovementUses())
            {
                finalPath[i].ToggleMovementSelected(true);
            }



            // Update unit to look in direction of movement
            // Moving right
            if (i == finalPath.Count-1)
            {
                // Moving left
                if (finalPath[i].GetSlotIndex().x > unit.GetActiveCombatSlot().GetSlotIndex().x)
                {
                    unit.UpdateUnitLookDirection(false);
                }
                // Moving Right
                else if (finalPath[i].GetSlotIndex().x < unit.GetActiveCombatSlot().GetSlotIndex().x)
                {
                    unit.UpdateUnitLookDirection(true);
                }
            }
        }

        spawnedGhostTiles = true;

        if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
            StartCoroutine(MoveUnitToNewSlot(unit));
    }

    public IEnumerator MoveUnitToNewSlot(UnitFunctionality unit)
    {
        if (unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
        {
            finalPath = FindPath(unit.GetActiveCombatSlot(), GetSelectedCombatSlotMove());
            finalPath.RemoveAt(0);
        }

        GameManager.Instance.ToggleEndTurnButton(false);



        GetComponent<ScrollRect>().enabled = false;

        //GetSelectedCombatSlotMove().ToggleMovementSelected(true);
        //ResetCombatSlotMovementSelected();

        if (spawnedGhostTiles && GetSelectedCombatSlotMove().movementSelected || unit.curUnitType == UnitFunctionality.UnitType.ENEMY && finalPath.Count > 0)
        {
            ToggleCombatSelectedSlotOutlines();

            // Start moving unit
            for (int i = 0; i < finalPath.Count; i++)
            {
                if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                {
                    if (GetTargetCombatSlots()[0].GetRangeFromActiveCombatSlot(unit.GetActiveCombatSlot()) <=
                        GameManager.Instance.GetActiveSkill().curSkillRange)
                    {
                        break;
                    }
                }

                // Update unit to look in direction of movement
                // Moving right
                if (finalPath.Count > i)
                {
                    // Moving left
                    if (finalPath[i].GetSlotIndex().x > unit.GetActiveCombatSlot().GetSlotIndex().x)
                    {
                        unit.UpdateUnitLookDirection(false);
                    }
                }

                if (finalPath.Count > i)
                {
                    // Moving left
                    if (finalPath[i].GetSlotIndex().x < unit.GetActiveCombatSlot().GetSlotIndex().x)
                    {
                        unit.UpdateUnitLookDirection(true);
                    }
                }

                movingUnit = unit;

                if (movingUnit.GetCurMovementUses() > 0)
                {

                    //Debug.Log("movements left = " + movingUnit.GetCurMovementUses());

                    startingPos = unit.GetActiveCombatSlot().transform.position;

                    unit.GetActiveCombatSlot().UpdateLinkedUnit(null);
                    ToggleIsMovementAllowed(false);

                    if (finalPath.Count > i)
                        endingPos = finalPath[i].transform.position;

                    startingPos = new Vector3(startingPos.x, startingPos.y, 0);
                    endingPos = new Vector3(endingPos.x, endingPos.y, 0);

                    moveTimer = 0;

                    unit.UpdateActiveCombatSlot(finalPath[i]);
                    finalPath[i].UpdateLinkedUnit(unit);

                    UnselectAllSelectedCombatSlots();
                    finalPath[i].ToggleSlotSelected(true);
                    finalPath[i].ToggleSlotAllowed(true);
                    allowMovement = true;
                    movingUnit.UpdateCurMovementUses(movingUnit.GetCurMovementUses() - 1);

                    movingUnit.UpdateActiveCombatSlot(finalPath[i]);
                    movingUnit.SetParent(finalPath[i].transform);
                    yield return new WaitForSeconds(.5f);
                    CheckToUnlinkCombatSlot();
                }
                else
                    break;
            }
        }


        GetComponent<ScrollRect>().enabled = true;

        if (finalPath.Count > 0)
        {
            if (unit.GetCurMovementUses() == 0 && unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                StartCoroutine(AutoSwapOutOfMovementMode());
            else if (unit.GetCurMovementUses() >= 0 && unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
                StartCoroutine(AutoSwapOutOfMovementMode());

            if (unit.GetCurMovementUses() < 0)
            {
                if (!unit.usedExtraMove)
                    movingUnit.usedExtraMove = true;

                if (unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
                    StartCoroutine(AutoSwapOutOfMovementModeAndLockSkills());
            }
        }


        unit.enemyMoved = true;


        //ResetCombatSlotMovementSelected();
        ResetCombatSlotMovementSelected();
        spawnedGhostTiles = false;


        if (unit.GetCurMovementUses() > 0 && unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
        {
            unit.UnitMove();
        }
        else if (unit.GetCurMovementUses() == 0 && !unit.attacked && unit.curUnitType == UnitFunctionality.UnitType.PLAYER)
        {
            unit.UnitMove();
        }

        GameManager.Instance.ToggleEndTurnButton(true);
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
        isCombatMode = false;

        UnselectAllSelectedCombatSlots();

        if (unit.GetCurMovementUses() <= 0)
            return;
        if (GameManager.Instance.GetActiveSkill() && unit.curUnitType == UnitFunctionality.UnitType.ENEMY && GetTargetCombatSlots().Count > 0)
        {
            if (unit.enemyMoved || unit.hasAttacked && GameManager.Instance.GetActiveSkill().curSkillRange >= GetTargetCombatSlots()[0].GetRangeFromActiveCombatSlot(unit.GetActiveCombatSlot()))
            {
                return;
            }

        }


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
            if (unit.GetCurMovementUses() >= 1)
            {
                if (allCombatSlots[i].GetRangeFromActiveCombatSlot(unit.GetActiveCombatSlot()) <= unit.GetCurMovementUses())
                {
                    if (allCombatSlots != null && allCombatSlots[i].walkable && !allCombatSlots[i].GetLinkedUnit())
                    {
                        allCombatSlots[i].ToggleSlotAllowed(true);
                    }
                }
            }
        }
        List<CombatSlot> allowedSlots = new List<CombatSlot>();

        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            if (allCombatSlots[i].GetAllowed())
            {
                allowedSlots.Add(allCombatSlots[i]);
            }
        }

        UpdateCombatSlotOutlines(allowedSlots, false, true);
    }

    public void ResetCombatSlotMovementSelected()
    {
        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            if (allCombatSlots[i].movementSelected)
            {
                allCombatSlots[i].ToggleMovementSelected(false);
            }
        }
    }

    public void UpdateUnitAttackRange(UnitFunctionality unit, CombatSlot slot = null)
    {
        isCombatMode = true;
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

        List<CombatSlot> allowedSlots = new List<CombatSlot>();

        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            if (allCombatSlots[i].GetAllowed())
            {
                allowedSlots.Add(allCombatSlots[i]);
            }
        }

        UpdateCombatSlotOutlines(allowedSlots, false);

        UpdateUnitAttackHitArea(unit, slot);
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

    public void ToggleCombatSelectedSlotOutlines()
    {
        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            allCombatSlots[i].ToggleSelectBorder(allCombatSlots[i].GetRightSelectBorder(), false);
            allCombatSlots[i].ToggleSelectBorder(allCombatSlots[i].GetLeftSelectBorder(), false);
            allCombatSlots[i].ToggleSelectBorder(allCombatSlots[i].GetTopSelectBorder(), false);
            allCombatSlots[i].ToggleSelectBorder(allCombatSlots[i].GetBottomSelectBorder(), false);
        }
    }

    void UpdateCombatSlotOutlines(List<CombatSlot> selectedCombatSlots, bool skill = true, bool movement = false)
    {
        if (!skill)
            ToggleAllCombatSlotOutlines();

        for (int b = 0; b < selectedCombatSlots.Count; b++)
        {
            selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetRightSelectBorder(), false);
            selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetLeftSelectBorder(), false);
            selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetTopSelectBorder(), false);
            selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetBottomSelectBorder(), false);

            if (!movement)
            {
                if (skill)
                {
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

                // skill range
                else
                {
                    // Right side
                    if (GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x + 1, selectedCombatSlots[b].GetSlotIndex().y)))
                    {
                        if (!GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x + 1, selectedCombatSlots[b].GetSlotIndex().y)).GetAllowed())
                            selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetRightSelectBorder(), true);
                        else
                            selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetRightSelectBorder(), false);
                    }
                    else
                        selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetRightSelectBorder(), true);

                    // Left side
                    if (GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x - 1, selectedCombatSlots[b].GetSlotIndex().y)))
                    {
                        if (!GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x - 1, selectedCombatSlots[b].GetSlotIndex().y)).GetAllowed())
                            selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetLeftSelectBorder(), true);
                        else
                            selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetLeftSelectBorder(), false);
                    }
                    else
                        selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetLeftSelectBorder(), true);

                    // Up side
                    if (GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x, selectedCombatSlots[b].GetSlotIndex().y + 1)))
                    {
                        if (!GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x, selectedCombatSlots[b].GetSlotIndex().y + 1)).GetAllowed())
                            selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetTopSelectBorder(), true);
                        else
                            selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetTopSelectBorder(), false);
                    }
                    else
                        selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetTopSelectBorder(), true);

                    // Down side
                    if (GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x, selectedCombatSlots[b].GetSlotIndex().y - 1)))
                    {
                        if (!GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x, selectedCombatSlots[b].GetSlotIndex().y - 1)).GetAllowed())
                            selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetBottomSelectBorder(), true);
                        else
                            selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetBottomSelectBorder(), false);
                    }
                    else
                        selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetBottomSelectBorder(), true);
                }
            }
            else
            {
                // Right side
                if (GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x + 1, selectedCombatSlots[b].GetSlotIndex().y)))
                {
                    if (!GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x + 1, selectedCombatSlots[b].GetSlotIndex().y)).GetAllowed() &&
                        (!GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x + 1, selectedCombatSlots[b].GetSlotIndex().y)).movementSelected))
                        selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetRightSelectBorder(), true);
                    else
                        selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetRightSelectBorder(), false);
                }
                else
                    selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetRightSelectBorder(), true);

                // Left side
                if (GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x - 1, selectedCombatSlots[b].GetSlotIndex().y)))
                {
                    if (!GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x - 1, selectedCombatSlots[b].GetSlotIndex().y)).GetAllowed() &&
                        (!GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x - 1, selectedCombatSlots[b].GetSlotIndex().y)).movementSelected))
                        selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetLeftSelectBorder(), true);
                    else
                        selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetLeftSelectBorder(), false);
                }
                else
                    selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetLeftSelectBorder(), true);

                // Up side
                if (GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x, selectedCombatSlots[b].GetSlotIndex().y + 1)))
                {
                    if (!GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x, selectedCombatSlots[b].GetSlotIndex().y + 1)).GetAllowed() &&
                        (!GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x, selectedCombatSlots[b].GetSlotIndex().y + 1)).movementSelected))
                        selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetTopSelectBorder(), true);
                    else
                        selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetTopSelectBorder(), false);
                }
                else
                    selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetTopSelectBorder(), true);

                // Down side
                if (GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x, selectedCombatSlots[b].GetSlotIndex().y - 1)))
                {
                    if (!GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x, selectedCombatSlots[b].GetSlotIndex().y - 1)).GetAllowed() &&
                        (!GetCombatSlot(new Vector2(selectedCombatSlots[b].GetSlotIndex().x, selectedCombatSlots[b].GetSlotIndex().y - 1)).movementSelected))
                        selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetBottomSelectBorder(), true);
                    else
                        selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetBottomSelectBorder(), false);
                }
                else
                    selectedCombatSlots[b].ToggleSelectBorder(selectedCombatSlots[b].GetBottomSelectBorder(), true);
            }
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
            //ToggleAllCombatSlotOutlines();
            UnselectAllSelectedCombatSlots();
            GameManager.Instance.UpdateMainIconDetails(null, null);
            OverlayUI.Instance.UpdateItemDetailsUI("", "", 0, 0, Vector2.zero, TeamItemsManager.Instance.clearSlotSprite);
            return;
        }
        else if (GameManager.Instance.GetActiveItemSlot() && !GameManager.Instance.isSkillsMode)
        {
            if (GameManager.Instance.GetActiveItemSlot().GetCalculatedItemsUsesRemaining2() <= 0)
            {
                ToggleAllCombatSlotOutlines();
                UnselectAllSelectedCombatSlots();
                GameManager.Instance.UpdateMainIconDetails(null, null);
                OverlayUI.Instance.UpdateItemDetailsUI("", "", 0, 0, Vector2.zero, TeamItemsManager.Instance.clearSlotSprite);
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
                                /*
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

                                        // fail safe?
                                        if (GetTargetCombatSlots().Count > 0)
                                        {
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
                                */
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

                                    // fail safe?
                                    if (GetTargetCombatSlots().Count > 0)
                                    {
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

                                }


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

                                if (GameManager.Instance.GetActiveSkill().curskillSelectionAliveType == SkillData.SkillSelectionAliveType.ALIVE)
                                {
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

            if (!GameManager.Instance.GetActiveUnitFunctionality().reanimated)
            {
                for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
                {
                    if (GameManager.Instance.activeRoomHeroes[i].GetRangeFromUnit(unit) < x && !GameManager.Instance.activeRoomHeroes[i].isDead)
                    {
                        x = GameManager.Instance.activeRoomHeroes[i].GetRangeFromUnit(unit);

                        targetedUnit = GameManager.Instance.activeRoomHeroes[i];
                    }
                }
            }
            // If enemy is reanimated, determine target based on if skill targets allies or enemies
            else
            {
                if (GameManager.Instance.GetActiveSkill().curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.PLAYERS)
                {
                    for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
                    {
                        if (GameManager.Instance.activeRoomHeroes[i].GetRangeFromUnit(unit) < x && !GameManager.Instance.activeRoomHeroes[i].isDead)
                        {
                            x = GameManager.Instance.activeRoomHeroes[i].GetRangeFromUnit(unit);

                            targetedUnit = GameManager.Instance.activeRoomHeroes[i];
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < GameManager.Instance.activeRoomEnemies.Count; i++)
                    {
                        if (GameManager.Instance.activeRoomEnemies[i].GetRangeFromUnit(unit) < x && !GameManager.Instance.activeRoomEnemies[i].isDead)
                        {
                            x = GameManager.Instance.activeRoomEnemies[i].GetRangeFromUnit(unit);

                            targetedUnit = GameManager.Instance.activeRoomEnemies[i];
                        }
                    }
                }

            }
        }
        else if (classType == SkillData.ClassType.STANDARD)
        {
            int x = 25;

            if (GameManager.Instance.GetActiveSkill().curSkillSelectionUnitType == SkillData.SkillSelectionUnitType.PLAYERS &&
                (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT))
            {
                if (!unit.reanimated)
                {
                    for (int i = 0; i < GameManager.Instance.activeRoomEnemies.Count; i++)
                    {
                        if (GameManager.Instance.activeRoomEnemies[i].GetRangeFromUnit(unit) < x && !GameManager.Instance.activeRoomEnemies[i].isDead)
                        {
                            x = GameManager.Instance.activeRoomEnemies[i].GetRangeFromUnit(unit);

                            targetedUnit = GameManager.Instance.activeRoomEnemies[i];
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
                    {
                        if (GameManager.Instance.activeRoomHeroes[i].GetRangeFromUnit(unit) < x && !GameManager.Instance.activeRoomHeroes[i].isDead)
                        {
                            x = GameManager.Instance.activeRoomHeroes[i].GetRangeFromUnit(unit);

                            targetedUnit = GameManager.Instance.activeRoomHeroes[i];
                        }
                    }
                }
            }
        }


        for (int i = 0; i < GameManager.Instance.activeRoomAllUnitFunctionalitys.Count; i++)
        {
            //if (classType != SkillData.ClassType.AGGRESSIVE && !GameManager.Instance.GetActiveUnitFunctionality().allyLow)
                //targetedUnit = GameManager.Instance.activeRoomAllUnitFunctionalitys[i];
            //if (classType != SkillData.ClassType.AGGRESSIVE && GameManager.Instance.GetActiveUnitFunctionality().allyLow)
                //targetedUnit = GameManager.Instance.GetLowestHealthUnit(false);

            if (targetedUnit == null)
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
                    CombatGridManager.Instance.GetTargetCombatSlots().Add(targetedUnit.GetActiveCombatSlot());
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

        //if (GetTargetCombatSlots().Count == 0 && unit.hasAttacked)
        //{
            //StartCoroutine(GameManager.Instance.GetActiveUnitFunctionality().StartUnitTurn());
            //return;
        //}


        /*
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
        */
    }

    public CombatSlot GetCombatSlotByIndex(Vector2 index)
    {
        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            if (allCombatSlots[i].GetSlotIndex() == index)
            {
                return allCombatSlots[i];
            }
        }

        return null;
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
        else if (slotIndex == 5)
        {
            newSlotIndex = new Vector2(5, 0);
        }
        else if (slotIndex == 6)
        {
            newSlotIndex = new Vector2(6, 0);
        }
        else if (slotIndex == 7)
        {
            newSlotIndex = new Vector2(7, 0);
        }
        else if (slotIndex == 8)
        {
            newSlotIndex = new Vector2(8, 0);
        }
        else if (slotIndex == 9)
        {
            newSlotIndex = new Vector2(9, 0);
        }
        else if (slotIndex == 10)
        {
            newSlotIndex = new Vector2(10, 0);
        }
        else if (slotIndex == 11)
        {
            newSlotIndex = new Vector2(11, 0);
        }
        else if (slotIndex == 12)
        {
            newSlotIndex = new Vector2(12, 0);
        }
        else if (slotIndex == 13)
        {
            newSlotIndex = new Vector2(13, 0);
        }
        else if (slotIndex == 14)
        {
            newSlotIndex = new Vector2(14, 0);
        }
        else if (slotIndex == 15)
        {
            newSlotIndex = new Vector2(15, 0);
        }
        // 2nd row
        else if (slotIndex == 16)
        {
            newSlotIndex = new Vector2(0, 1);
        }
        else if (slotIndex == 17)
        {
            newSlotIndex = new Vector2(1, 1);
        }
        else if (slotIndex == 18)
        {
            newSlotIndex = new Vector2(2, 1);
        }
        else if (slotIndex == 19)
        {
            newSlotIndex = new Vector2(3, 1);
        }
        else if (slotIndex == 20)
        {
            newSlotIndex = new Vector2(4, 1);
        }
        else if (slotIndex == 21)
        {
            newSlotIndex = new Vector2(5, 1);
        }
        else if (slotIndex == 22)
        {
            newSlotIndex = new Vector2(6, 1);
        }
        else if (slotIndex == 23)
        {
            newSlotIndex = new Vector2(7, 1);
        }
        else if (slotIndex == 24)
        {
            newSlotIndex = new Vector2(8, 1);
        }
        else if (slotIndex == 25)
        {
            newSlotIndex = new Vector2(9, 1);
        }
        else if (slotIndex == 26)
        {
            newSlotIndex = new Vector2(10, 1);
        }
        else if (slotIndex == 27)
        {
            newSlotIndex = new Vector2(11, 1);
        }
        else if (slotIndex == 28)
        {
            newSlotIndex = new Vector2(12, 1);
        }
        else if (slotIndex == 29)
        {
            newSlotIndex = new Vector2(13, 1);
        }
        else if (slotIndex == 30)
        {
            newSlotIndex = new Vector2(14, 1);
        }
        else if (slotIndex == 31)
        {
            newSlotIndex = new Vector2(15, 1);
        }

        // 3rd row
        else if (slotIndex == 32)
        {
            newSlotIndex = new Vector2(0, 2);
        }
        else if (slotIndex == 33)
        {
            newSlotIndex = new Vector2(1, 2);
        }
        else if (slotIndex == 34)
        {
            newSlotIndex = new Vector2(2, 2);
        }
        else if (slotIndex == 35)
        {
            newSlotIndex = new Vector2(3, 2);
        }
        else if (slotIndex == 36)
        {
            newSlotIndex = new Vector2(4, 2);
        }
        else if (slotIndex == 37)
        {
            newSlotIndex = new Vector2(5, 2);
        }
        else if (slotIndex == 38)
        {
            newSlotIndex = new Vector2(6, 2);
        }
        else if (slotIndex == 39)
        {
            newSlotIndex = new Vector2(7, 2);
        }
        else if (slotIndex == 40)
        {
            newSlotIndex = new Vector2(8, 2);
        }
        else if (slotIndex == 41)
        {
            newSlotIndex = new Vector2(9, 2);
        }
        else if (slotIndex == 42)
        {
            newSlotIndex = new Vector2(10, 2);
        }
        else if (slotIndex == 43)
        {
            newSlotIndex = new Vector2(11, 2);
        }
        else if (slotIndex == 44)
        {
            newSlotIndex = new Vector2(12, 2);
        }
        else if (slotIndex == 45)
        {
            newSlotIndex = new Vector2(13, 2);
        }
        else if (slotIndex == 46)
        {
            newSlotIndex = new Vector2(14, 2);
        }
        else if (slotIndex == 47)
        {
            newSlotIndex = new Vector2(15, 2);
        }

        // 4th row
        else if (slotIndex == 48)
        {
            newSlotIndex = new Vector2(0, 3);
        }
        else if (slotIndex == 49)
        {
            newSlotIndex = new Vector2(1, 3);
        }
        else if (slotIndex == 50)
        {
            newSlotIndex = new Vector2(2, 3);
        }
        else if (slotIndex == 51)
        {
            newSlotIndex = new Vector2(3, 3);
        }
        else if (slotIndex == 52)
        {
            newSlotIndex = new Vector2(4, 3);
        }
        else if (slotIndex == 53)
        {
            newSlotIndex = new Vector2(5, 3);
        }
        else if (slotIndex == 54)
        {
            newSlotIndex = new Vector2(6, 3);
        }
        else if (slotIndex == 55)
        {
            newSlotIndex = new Vector2(7, 3);
        }
        else if (slotIndex == 56)
        {
            newSlotIndex = new Vector2(8, 3);
        }
        else if (slotIndex == 57)
        {
            newSlotIndex = new Vector2(9, 3);
        }
        else if (slotIndex == 58)
        {
            newSlotIndex = new Vector2(10, 3);
        }
        else if (slotIndex == 59)
        {
            newSlotIndex = new Vector2(11, 3);
        }
        else if (slotIndex == 60)
        {
            newSlotIndex = new Vector2(12, 3);
        }
        else if (slotIndex == 61)
        {
            newSlotIndex = new Vector2(13, 3);
        }
        else if (slotIndex == 62)
        {
            newSlotIndex = new Vector2(14, 3);
        }
        else if (slotIndex == 63)
        {
            newSlotIndex = new Vector2(15, 3);
        }

        // 5th row
        else if (slotIndex == 64)
        {
            newSlotIndex = new Vector2(0, 4);
        }
        else if (slotIndex == 65)
        {
            newSlotIndex = new Vector2(1, 4);
        }
        else if (slotIndex == 66)
        {
            newSlotIndex = new Vector2(2, 4);
        }
        else if (slotIndex == 67)
        {
            newSlotIndex = new Vector2(3, 4);
        }
        else if (slotIndex == 68)
        {
            newSlotIndex = new Vector2(4, 4);
        }
        else if (slotIndex == 69)
        {
            newSlotIndex = new Vector2(5, 4);
        }
        else if (slotIndex == 70)
        {
            newSlotIndex = new Vector2(6, 4);
        }
        else if (slotIndex == 71)
        {
            newSlotIndex = new Vector2(7, 4);
        }
        else if (slotIndex == 72)
        {
            newSlotIndex = new Vector2(8, 4);
        }
        else if (slotIndex == 73)
        {
            newSlotIndex = new Vector2(9, 4);
        }
        else if (slotIndex == 74)
        {
            newSlotIndex = new Vector2(10, 4);
        }
        else if (slotIndex == 75)
        {
            newSlotIndex = new Vector2(11, 4);
        }
        else if (slotIndex == 76)
        {
            newSlotIndex = new Vector2(12, 4);
        }
        else if (slotIndex == 77)
        {
            newSlotIndex = new Vector2(13, 4);
        }
        else if (slotIndex == 78)
        {
            newSlotIndex = new Vector2(14, 4);
        }
        else if (slotIndex == 79)
        {
            newSlotIndex = new Vector2(15, 4);
        }

        // 6th row
        else if (slotIndex == 80)
        {
            newSlotIndex = new Vector2(0, 5);
        }
        else if (slotIndex == 81)
        {
            newSlotIndex = new Vector2(1, 5);
        }
        else if (slotIndex == 82)
        {
            newSlotIndex = new Vector2(2, 5);
        }
        else if (slotIndex == 83)
        {
            newSlotIndex = new Vector2(3, 5);
        }
        else if (slotIndex == 84)
        {
            newSlotIndex = new Vector2(4, 5);
        }
        else if (slotIndex == 85)
        {
            newSlotIndex = new Vector2(5, 5);
        }
        else if (slotIndex == 86)
        {
            newSlotIndex = new Vector2(6, 5);
        }
        else if (slotIndex == 87)
        {
            newSlotIndex = new Vector2(7, 5);
        }
        else if (slotIndex == 88)
        {
            newSlotIndex = new Vector2(8, 5);
        }
        else if (slotIndex == 89)
        {
            newSlotIndex = new Vector2(9, 5);
        }
        else if (slotIndex == 90)
        {
            newSlotIndex = new Vector2(10, 5);
        }
        else if (slotIndex == 91)
        {
            newSlotIndex = new Vector2(11, 5);
        }
        else if (slotIndex == 92)
        {
            newSlotIndex = new Vector2(12, 5);
        }
        else if (slotIndex == 93)
        {
            newSlotIndex = new Vector2(13, 5);
        }
        else if (slotIndex == 94)
        {
            newSlotIndex = new Vector2(14, 5);
        }
        else if (slotIndex == 95)
        {
            newSlotIndex = new Vector2(15, 5);
        }

        // 7th row
        else if (slotIndex == 96)
        {
            newSlotIndex = new Vector2(0, 6);
        }
        else if (slotIndex == 97)
        {
            newSlotIndex = new Vector2(1, 6);
        }
        else if (slotIndex == 98)
        {
            newSlotIndex = new Vector2(2, 6);
        }
        else if (slotIndex == 99)
        {
            newSlotIndex = new Vector2(3, 6);
        }
        else if (slotIndex == 100)
        {
            newSlotIndex = new Vector2(4, 6);
        }
        else if (slotIndex == 101)
        {
            newSlotIndex = new Vector2(5, 6);
        }
        else if (slotIndex == 102)
        {
            newSlotIndex = new Vector2(6, 6);
        }
        else if (slotIndex == 103)
        {
            newSlotIndex = new Vector2(7, 6);
        }
        else if (slotIndex == 104)
        {
            newSlotIndex = new Vector2(8, 6);
        }
        else if (slotIndex == 105)
        {
            newSlotIndex = new Vector2(9, 6);
        }
        else if (slotIndex == 106)
        {
            newSlotIndex = new Vector2(10, 6);
        }
        else if (slotIndex == 107)
        {
            newSlotIndex = new Vector2(11, 6);
        }
        else if (slotIndex == 108)
        {
            newSlotIndex = new Vector2(12, 6);
        }
        else if (slotIndex == 109)
        {
            newSlotIndex = new Vector2(13, 6);
        }
        else if (slotIndex == 110)
        {
            newSlotIndex = new Vector2(14, 6);
        }
        else if (slotIndex == 111)
        {
            newSlotIndex = new Vector2(15, 6);
        }

        // 8th row
        else if (slotIndex == 112)
        {
            newSlotIndex = new Vector2(0, 7);
        }
        else if (slotIndex == 113)
        {
            newSlotIndex = new Vector2(1, 7);
        }
        else if (slotIndex == 114)
        {
            newSlotIndex = new Vector2(2, 7);
        }
        else if (slotIndex == 115)
        {
            newSlotIndex = new Vector2(3, 7);
        }
        else if (slotIndex == 116)
        {
            newSlotIndex = new Vector2(4, 7);
        }
        else if (slotIndex == 117)
        {
            newSlotIndex = new Vector2(5, 7);
        }
        else if (slotIndex == 118)
        {
            newSlotIndex = new Vector2(6, 7);
        }
        else if (slotIndex == 119)
        {
            newSlotIndex = new Vector2(7, 7);
        }
        else if (slotIndex == 120)
        {
            newSlotIndex = new Vector2(8, 7);
        }
        else if (slotIndex == 121)
        {
            newSlotIndex = new Vector2(9, 7);
        }
        else if (slotIndex == 122)
        {
            newSlotIndex = new Vector2(10, 7);
        }
        else if (slotIndex == 123)
        {
            newSlotIndex = new Vector2(11, 7);
        }
        else if (slotIndex == 124)
        {
            newSlotIndex = new Vector2(12, 7);
        }
        else if (slotIndex == 125)
        {
            newSlotIndex = new Vector2(13, 7);
        }
        else if (slotIndex == 126)
        {
            newSlotIndex = new Vector2(14, 7);
        }
        else if (slotIndex == 127)
        {
            newSlotIndex = new Vector2(15, 7);
        }

        // 9th row
        else if (slotIndex == 128)
        {
            newSlotIndex = new Vector2(0, 8);
        }
        else if (slotIndex == 129)
        {
            newSlotIndex = new Vector2(1, 8);
        }
        else if (slotIndex == 130)
        {
            newSlotIndex = new Vector2(2, 8);
        }
        else if (slotIndex == 131)
        {
            newSlotIndex = new Vector2(3, 8);
        }
        else if (slotIndex == 132)
        {
            newSlotIndex = new Vector2(4, 8);
        }
        else if (slotIndex == 133)
        {
            newSlotIndex = new Vector2(5, 8);
        }
        else if (slotIndex == 134)
        {
            newSlotIndex = new Vector2(6, 8);
        }
        else if (slotIndex == 135)
        {
            newSlotIndex = new Vector2(7, 8);
        }
        else if (slotIndex == 136)
        {
            newSlotIndex = new Vector2(8, 8);
        }
        else if (slotIndex == 137)
        {
            newSlotIndex = new Vector2(9, 8);
        }
        else if (slotIndex == 138)
        {
            newSlotIndex = new Vector2(10, 8);
        }
        else if (slotIndex == 139)
        {
            newSlotIndex = new Vector2(11, 8);
        }
        else if (slotIndex == 140)
        {
            newSlotIndex = new Vector2(12, 8);
        }
        else if (slotIndex == 141)
        {
            newSlotIndex = new Vector2(13, 8);
        }
        else if (slotIndex == 142)
        {
            newSlotIndex = new Vector2(14, 8);
        }
        else if (slotIndex == 143)
        {
            newSlotIndex = new Vector2(15, 8);
        }

        // 10th row
        else if (slotIndex == 144)
        {
            newSlotIndex = new Vector2(0, 9);
        }
        else if (slotIndex == 145)
        {
            newSlotIndex = new Vector2(1, 9);
        }
        else if (slotIndex == 146)
        {
            newSlotIndex = new Vector2(2, 9);
        }
        else if (slotIndex == 147)
        {
            newSlotIndex = new Vector2(3, 9);
        }
        else if (slotIndex == 148)
        {
            newSlotIndex = new Vector2(4, 9);
        }
        else if (slotIndex == 149)
        {
            newSlotIndex = new Vector2(5, 9);
        }
        else if (slotIndex == 150)
        {
            newSlotIndex = new Vector2(6, 9);
        }
        else if (slotIndex == 151)
        {
            newSlotIndex = new Vector2(7, 9);
        }
        else if (slotIndex == 152)
        {
            newSlotIndex = new Vector2(8, 9);
        }
        else if (slotIndex == 153)
        {
            newSlotIndex = new Vector2(9, 9);
        }
        else if (slotIndex == 154)
        {
            newSlotIndex = new Vector2(10, 9);
        }
        else if (slotIndex == 155)
        {
            newSlotIndex = new Vector2(11, 9);
        }
        else if (slotIndex == 156)
        {
            newSlotIndex = new Vector2(12, 9);
        }
        else if (slotIndex == 157)
        {
            newSlotIndex = new Vector2(13, 9);
        }
        else if (slotIndex == 158)
        {
            newSlotIndex = new Vector2(14, 9);
        }
        else if (slotIndex == 159)
        {
            newSlotIndex = new Vector2(15, 9);
        }

        // 11th row
        else if (slotIndex == 160)
        {
            newSlotIndex = new Vector2(0, 10);
        }
        else if (slotIndex == 161)
        {
            newSlotIndex = new Vector2(1, 10);
        }
        else if (slotIndex == 162)
        {
            newSlotIndex = new Vector2(2, 10);
        }
        else if (slotIndex == 163)
        {
            newSlotIndex = new Vector2(3, 10);
        }
        else if (slotIndex == 164)
        {
            newSlotIndex = new Vector2(4, 10);
        }
        else if (slotIndex == 165)
        {
            newSlotIndex = new Vector2(5, 10);
        }
        else if (slotIndex == 166)
        {
            newSlotIndex = new Vector2(6, 10);
        }
        else if (slotIndex == 167)
        {
            newSlotIndex = new Vector2(7, 10);
        }
        else if (slotIndex == 168)
        {
            newSlotIndex = new Vector2(8, 10);
        }
        else if (slotIndex == 169)
        {
            newSlotIndex = new Vector2(9, 10);
        }
        else if (slotIndex == 170)
        {
            newSlotIndex = new Vector2(10, 10);
        }
        else if (slotIndex == 171)
        {
            newSlotIndex = new Vector2(11, 10);
        }
        else if (slotIndex == 172)
        {
            newSlotIndex = new Vector2(12, 10);
        }
        else if (slotIndex == 173)
        {
            newSlotIndex = new Vector2(13, 10);
        }
        else if (slotIndex == 174)
        {
            newSlotIndex = new Vector2(14, 10);
        }
        else if (slotIndex == 175)
        {
            newSlotIndex = new Vector2(15, 10);
        }

        // 12th row
        else if (slotIndex == 176)
        {
            newSlotIndex = new Vector2(0, 11);
        }
        else if (slotIndex == 177)
        {
            newSlotIndex = new Vector2(1, 11);
        }
        else if (slotIndex == 178)
        {
            newSlotIndex = new Vector2(2, 11);
        }
        else if (slotIndex == 179)
        {
            newSlotIndex = new Vector2(3, 11);
        }
        else if (slotIndex == 180)
        {
            newSlotIndex = new Vector2(4, 11);
        }
        else if (slotIndex == 181)
        {
            newSlotIndex = new Vector2(5, 11);
        }
        else if (slotIndex == 182)
        {
            newSlotIndex = new Vector2(6, 11);
        }
        else if (slotIndex == 183)
        {
            newSlotIndex = new Vector2(7, 11);
        }
        else if (slotIndex == 184)
        {
            newSlotIndex = new Vector2(8, 11);
        }
        else if (slotIndex == 185)
        {
            newSlotIndex = new Vector2(9, 11);
        }
        else if (slotIndex == 186)
        {
            newSlotIndex = new Vector2(10, 11);
        }
        else if (slotIndex == 187)
        {
            newSlotIndex = new Vector2(11, 11);
        }
        else if (slotIndex == 188)
        {
            newSlotIndex = new Vector2(12, 11);
        }
        else if (slotIndex == 189)
        {
            newSlotIndex = new Vector2(13, 11);
        }
        else if (slotIndex == 190)
        {
            newSlotIndex = new Vector2(14, 11);
        }
        else if (slotIndex == 191)
        {
            newSlotIndex = new Vector2(15, 11);
        }

        // 13th row
        else if (slotIndex == 192)
        {
            newSlotIndex = new Vector2(0, 12);
        }
        else if (slotIndex == 193)
        {
            newSlotIndex = new Vector2(1, 12);
        }
        else if (slotIndex == 194)
        {
            newSlotIndex = new Vector2(2, 12);
        }
        else if (slotIndex == 195)
        {
            newSlotIndex = new Vector2(3, 12);
        }
        else if (slotIndex == 196)
        {
            newSlotIndex = new Vector2(4, 12);
        }
        else if (slotIndex == 197)
        {
            newSlotIndex = new Vector2(5, 12);
        }
        else if (slotIndex == 198)
        {
            newSlotIndex = new Vector2(6, 12);
        }
        else if (slotIndex == 199)
        {
            newSlotIndex = new Vector2(7, 12);
        }
        else if (slotIndex == 200)
        {
            newSlotIndex = new Vector2(8, 12);
        }
        else if (slotIndex == 201)
        {
            newSlotIndex = new Vector2(9, 12);
        }
        else if (slotIndex == 202)
        {
            newSlotIndex = new Vector2(10, 12);
        }
        else if (slotIndex == 203)
        {
            newSlotIndex = new Vector2(11, 12);
        }
        else if (slotIndex == 204)
        {
            newSlotIndex = new Vector2(12, 12);
        }
        else if (slotIndex == 205)
        {
            newSlotIndex = new Vector2(13, 12);
        }
        else if (slotIndex == 206)
        {
            newSlotIndex = new Vector2(14, 12);
        }
        else if (slotIndex == 207)
        {
            newSlotIndex = new Vector2(15, 12);
        }

        // 14th row
        else if (slotIndex == 208)
        {
            newSlotIndex = new Vector2(0, 13);
        }
        else if (slotIndex == 209)
        {
            newSlotIndex = new Vector2(1, 13);
        }
        else if (slotIndex == 210)
        {
            newSlotIndex = new Vector2(2, 13);
        }
        else if (slotIndex == 211)
        {
            newSlotIndex = new Vector2(3, 13);
        }
        else if (slotIndex == 212)
        {
            newSlotIndex = new Vector2(4, 13);
        }
        else if (slotIndex == 213)
        {
            newSlotIndex = new Vector2(5, 13);
        }
        else if (slotIndex == 214)
        {
            newSlotIndex = new Vector2(6, 13);
        }
        else if (slotIndex == 215)
        {
            newSlotIndex = new Vector2(7, 13);
        }
        else if (slotIndex == 216)
        {
            newSlotIndex = new Vector2(8, 13);
        }
        else if (slotIndex == 217)
        {
            newSlotIndex = new Vector2(9, 13);
        }
        else if (slotIndex == 218)
        {
            newSlotIndex = new Vector2(10, 13);
        }
        else if (slotIndex == 219)
        {
            newSlotIndex = new Vector2(11, 13);
        }
        else if (slotIndex == 220)
        {
            newSlotIndex = new Vector2(12, 13);
        }
        else if (slotIndex == 221)
        {
            newSlotIndex = new Vector2(13, 13);
        }
        else if (slotIndex == 222)
        {
            newSlotIndex = new Vector2(14, 13);
        }
        else if (slotIndex == 223)
        {
            newSlotIndex = new Vector2(15, 13);
        }

        // 15th row
        else if (slotIndex == 224)
        {
            newSlotIndex = new Vector2(0, 14);
        }
        else if (slotIndex == 225)
        {
            newSlotIndex = new Vector2(1, 14);
        }
        else if (slotIndex == 226)
        {
            newSlotIndex = new Vector2(2, 14);
        }
        else if (slotIndex == 227)
        {
            newSlotIndex = new Vector2(3, 14);
        }
        else if (slotIndex == 228)
        {
            newSlotIndex = new Vector2(4, 14);
        }
        else if (slotIndex == 229)
        {
            newSlotIndex = new Vector2(5, 14);
        }
        else if (slotIndex == 230)
        {
            newSlotIndex = new Vector2(6, 14);
        }
        else if (slotIndex == 231)
        {
            newSlotIndex = new Vector2(7, 14);
        }
        else if (slotIndex == 232)
        {
            newSlotIndex = new Vector2(8, 14);
        }
        else if (slotIndex == 233)
        {
            newSlotIndex = new Vector2(9, 14);
        }
        else if (slotIndex == 234)
        {
            newSlotIndex = new Vector2(10, 14);
        }
        else if (slotIndex == 235)
        {
            newSlotIndex = new Vector2(11, 14);
        }
        else if (slotIndex == 236)
        {
            newSlotIndex = new Vector2(12, 14);
        }
        else if (slotIndex == 237)
        {
            newSlotIndex = new Vector2(13, 14);
        }
        else if (slotIndex == 238)
        {
            newSlotIndex = new Vector2(14, 14);
        }
        else if (slotIndex == 239)
        {
            newSlotIndex = new Vector2(15, 14);
        }

        // 16th row
        else if (slotIndex == 240)
        {
            newSlotIndex = new Vector2(0, 15);
        }
        else if (slotIndex == 241)
        {
            newSlotIndex = new Vector2(1, 15);
        }
        else if (slotIndex == 242)
        {
            newSlotIndex = new Vector2(2, 15);
        }
        else if (slotIndex == 243)
        {
            newSlotIndex = new Vector2(3, 15);
        }
        else if (slotIndex == 244)
        {
            newSlotIndex = new Vector2(4, 15);
        }
        else if (slotIndex == 245)
        {
            newSlotIndex = new Vector2(5, 15);
        }
        else if (slotIndex == 246)
        {
            newSlotIndex = new Vector2(6, 15);
        }
        else if (slotIndex == 247)
        {
            newSlotIndex = new Vector2(7, 15);
        }
        else if (slotIndex == 248)
        {
            newSlotIndex = new Vector2(8, 15);
        }
        else if (slotIndex == 249)
        {
            newSlotIndex = new Vector2(9, 15);
        }
        else if (slotIndex == 250)
        {
            newSlotIndex = new Vector2(10, 15);
        }
        else if (slotIndex == 251)
        {
            newSlotIndex = new Vector2(11, 15);
        }
        else if (slotIndex == 252)
        {
            newSlotIndex = new Vector2(12, 15);
        }
        else if (slotIndex == 253)
        {
            newSlotIndex = new Vector2(13, 15);
        }
        else if (slotIndex == 254)
        {
            newSlotIndex = new Vector2(14, 15);
        }
        else if (slotIndex == 255)
        {
            newSlotIndex = new Vector2(15, 15);
        }

        // 17th row
        else if (slotIndex == 256)
        {
            newSlotIndex = new Vector2(0, 16);
        }
        else if (slotIndex == 257)
        {
            newSlotIndex = new Vector2(1, 16);
        }
        else if (slotIndex == 258)
        {
            newSlotIndex = new Vector2(2, 16);
        }
        else if (slotIndex == 259)
        {
            newSlotIndex = new Vector2(3, 16);
        }
        else if (slotIndex == 260)
        {
            newSlotIndex = new Vector2(4, 16);
        }
        else if (slotIndex == 261)
        {
            newSlotIndex = new Vector2(5, 16);
        }
        else if (slotIndex == 262)
        {
            newSlotIndex = new Vector2(6, 16);
        }
        else if (slotIndex == 263)
        {
            newSlotIndex = new Vector2(7, 16);
        }
        else if (slotIndex == 264)
        {
            newSlotIndex = new Vector2(8, 16);
        }
        else if (slotIndex == 265)
        {
            newSlotIndex = new Vector2(9, 16);
        }
        else if (slotIndex == 266)
        {
            newSlotIndex = new Vector2(10, 16);
        }
        else if (slotIndex == 267)
        {
            newSlotIndex = new Vector2(11, 16);
        }
        else if (slotIndex == 268)
        {
            newSlotIndex = new Vector2(12, 16);
        }
        else if (slotIndex == 269)
        {
            newSlotIndex = new Vector2(13, 16);
        }
        else if (slotIndex == 270)
        {
            newSlotIndex = new Vector2(14, 16);
        }
        else if (slotIndex == 271)
        {
            newSlotIndex = new Vector2(15, 16);
        }

        // 18th row
        else if (slotIndex == 272)
        {
            newSlotIndex = new Vector2(0, 17);
        }
        else if (slotIndex == 273)
        {
            newSlotIndex = new Vector2(1, 17);
        }
        else if (slotIndex == 274)
        {
            newSlotIndex = new Vector2(2, 17);
        }
        else if (slotIndex == 275)
        {
            newSlotIndex = new Vector2(3, 17);
        }
        else if (slotIndex == 276)
        {
            newSlotIndex = new Vector2(4, 17);
        }
        else if (slotIndex == 277)
        {
            newSlotIndex = new Vector2(5, 17);
        }
        else if (slotIndex == 278)
        {
            newSlotIndex = new Vector2(6, 17);
        }
        else if (slotIndex == 279)
        {
            newSlotIndex = new Vector2(7, 17);
        }
        else if (slotIndex == 280)
        {
            newSlotIndex = new Vector2(8, 17);
        }
        else if (slotIndex == 281)
        {
            newSlotIndex = new Vector2(9, 17);
        }
        else if (slotIndex == 282)
        {
            newSlotIndex = new Vector2(10, 17);
        }
        else if (slotIndex == 283)
        {
            newSlotIndex = new Vector2(11, 17);
        }
        else if (slotIndex == 284)
        {
            newSlotIndex = new Vector2(12, 17);
        }
        else if (slotIndex == 285)
        {
            newSlotIndex = new Vector2(13, 17);
        }
        else if (slotIndex == 286)
        {
            newSlotIndex = new Vector2(14, 17);
        }
        else if (slotIndex == 287)
        {
            newSlotIndex = new Vector2(15, 17);
        }

        // 19th row
        else if (slotIndex == 288)
        {
            newSlotIndex = new Vector2(0, 18);
        }
        else if (slotIndex == 289)
        {
            newSlotIndex = new Vector2(1, 18);
        }
        else if (slotIndex == 290)
        {
            newSlotIndex = new Vector2(2, 18);
        }
        else if (slotIndex == 291)
        {
            newSlotIndex = new Vector2(3, 18);
        }
        else if (slotIndex == 292)
        {
            newSlotIndex = new Vector2(4, 18);
        }
        else if (slotIndex == 293)
        {
            newSlotIndex = new Vector2(5, 18);
        }
        else if (slotIndex == 294)
        {
            newSlotIndex = new Vector2(6, 18);
        }
        else if (slotIndex == 295)
        {
            newSlotIndex = new Vector2(7, 18);
        }
        else if (slotIndex == 296)
        {
            newSlotIndex = new Vector2(8, 18);
        }
        else if (slotIndex == 297)
        {
            newSlotIndex = new Vector2(9, 18);
        }
        else if (slotIndex == 298)
        {
            newSlotIndex = new Vector2(10, 18);
        }
        else if (slotIndex == 299)
        {
            newSlotIndex = new Vector2(11, 18);
        }
        else if (slotIndex == 300)
        {
            newSlotIndex = new Vector2(12, 18);
        }
        else if (slotIndex == 301)
        {
            newSlotIndex = new Vector2(13, 18);
        }
        else if (slotIndex == 302)
        {
            newSlotIndex = new Vector2(14, 18);
        }
        else if (slotIndex == 303)
        {
            newSlotIndex = new Vector2(15, 18);
        }
        #endregion
        return newSlotIndex;
    }

    IEnumerator EndUnitTurnAfterWait(UnitFunctionality unit)
    {
        yield return new WaitForSeconds(.2f);

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
            {
                if (allCombatSlots[i].walkable)
                    allCombatSlots[i].GetComponent<UIElement>().UpdateAlpha(1);
                else
                    allCombatSlots[i].GetComponent<UIElement>().UpdateAlpha(0);
            }
            else
                allCombatSlots[i].GetComponent<UIElement>().UpdateAlpha(0);
        }
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

        ToggleCombatUIElement(false);
        ToggleScaleButtons(false);
    }
}
