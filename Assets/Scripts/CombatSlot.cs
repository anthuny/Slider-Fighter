using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatSlot : MonoBehaviour
{
    [SerializeField] private Vector2 slotIndex;
    [SerializeField] private Vector2 localIndexFromSlot;
    [SerializeField] private UIElement slotUI;
    [SerializeField] private UIElement slotTargetedUI;
    [SerializeField] private bool selected = false;
    [SerializeField] private bool allowed = false;
    [SerializeField] private UnitFunctionality linkedUnit;
    [SerializeField] private List<UnitFunctionality> fallenUnits = new List<UnitFunctionality>();
    [SerializeField] private Animator animator;
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    [SerializeField] private int rangeFromActiveUnit;

    [SerializeField] private UIElement topSelectBorder;
    [SerializeField] private UIElement leftSelectBorder;
    [SerializeField] private UIElement rightSelectBorder;
    [SerializeField] private UIElement bottomSelectBorder;

    [SerializeField] private UIElement transparentBG;
    public ButtonFunctionality button;
    public UIElement buttonUI;
    public Animator effectDisplayAnimator;

    private bool sizeIncreased;
    public bool combatSelected;
    public bool walkable = true;
    public CombatSlot previousSlot;
    public int gCost;
    public int hCost;
    public int fCost;
    public bool movementSelected;

    [SerializeField] private UIElement slotCover;
    public bool coverOn;
    [SerializeField] private UIElement slotDirArrow;
    public bool arrowDisplayed = false;
    [SerializeField] private MenuUnitDisplay unitDisplayGhost;

    public bool ghostDisplaying = false;

    public void ToggleUnitDisplayGhost(bool toggle)
    {
        if (toggle)
        {
            CombatGridManager.Instance.ResetAllGhosts();
        }

        ghostDisplaying = toggle;

        if (toggle)
        {
            if (GameManager.Instance.GetActiveUnitFunctionality())
            {
                if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY)
                    return;
            }

            UpdateUnitDisplayGhost();
            unitDisplayGhost.GetComponent<UIElement>().UpdateAlpha(1);
        }
        else
            unitDisplayGhost.GetComponent<UIElement>().UpdateAlpha(0);
    }

    public void UpdateUnitDisplayGhost()
    {
        for (int i = 0; i < GameManager.Instance.activeTeam.Count; i++)
        {
            if (GameManager.Instance.GetActiveUnitFunctionality().GetUnitName() == GameManager.Instance.activeTeam[i].unitName)
            {
                unitDisplayGhost.UpdateUnitDisplay(GameManager.Instance.activeTeam[i].unitName, true, true);
                break;
            }
        }
    }

    public void UpdateSlotDirArrow(Sprite sprite)
    {
        ToggleSlotDirArrow(true);

        slotDirArrow.UpdateContentImage(sprite);
    }

    public void ToggleSlotDirArrow(bool toggle = true)
    {
        if (toggle)
        {
            slotDirArrow.UpdateAlpha(1);
            slotDirArrow.AnimateUI(false);
            arrowDisplayed = true;
        }
        else
        {
            slotDirArrow.UpdateAlpha(0);
            arrowDisplayed = false;
        }
    }

    public void ToggleSlotCover(bool toggle = true)
    {
        if (!slotCover)
            return;

        if (toggle)
        {
            if (GameManager.Instance.combatStarted && !PostBattle.Instance.isInPostBattle)
            {
                coverOn = true;
                slotCover.UpdateAlpha(1);
            }
        }
        else
        {
            coverOn = false;
            slotCover.UpdateAlpha(0);
        }
    }

    public void ToggleMovementSelected(bool toggle = false)
    {
        movementSelected = toggle;

        if (toggle)
        {
            slotUI.UpdateColour(CombatGridManager.Instance.slotMovementSelectedColour);
        }
        else
        {
            if (!CombatGridManager.Instance.isCombatMode)
            {
                //Debug.Log("asd1");
                slotUI.UpdateColour(CombatGridManager.Instance.slotSelectedColour);
            }

            else
                slotUI.UpdateColour(CombatGridManager.Instance.slotNotAllowedColour);
        }
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public List<UnitFunctionality> GetFallenUnits()
    {
        return fallenUnits;
    }

    public void AddFallenUnit(UnitFunctionality unit)
    {
        if (!fallenUnits.Contains(unit))
            fallenUnits.Add(unit);
    }

    public void RemoveFallenUnit(UnitFunctionality unit)
    {
        if (fallenUnits.Contains(unit))
            fallenUnits.Remove(unit);
    }

    public void ResetFallenUnits()
    {
        fallenUnits.Clear();
    }

    public void ToggleTransparentBG(bool toggle = true)
    {
        if (toggle)
            transparentBG.UpdateAlpha(.35f);
        else
            transparentBG.UpdateAlpha(0);
    }

    public UIElement GetTopSelectBorder()
    {
        return topSelectBorder;
    }

    public UIElement GetLeftSelectBorder()
    {
        return leftSelectBorder;
    }

    public UIElement GetRightSelectBorder()
    {
        return rightSelectBorder;
    }

    public UIElement GetBottomSelectBorder()
    {
        return bottomSelectBorder;
    }
    
    public void UpdateSlotBorderColour()
    {
        if (CombatGridManager.Instance.isCombatMode)
        {
            if (GameManager.Instance.isSkillsMode)
            {
                if (GameManager.Instance.GetActiveSkill())
                {
                    if (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE)
                    {
                        GetTopSelectBorder().UpdateColour(CombatGridManager.Instance.slotAggressiveColour);
                        GetBottomSelectBorder().UpdateColour(CombatGridManager.Instance.slotAggressiveColour);
                        GetLeftSelectBorder().UpdateColour(CombatGridManager.Instance.slotAggressiveColour);
                        GetRightSelectBorder().UpdateColour(CombatGridManager.Instance.slotAggressiveColour);
                    }
                    else
                    {
                        GetTopSelectBorder().UpdateColour(CombatGridManager.Instance.slotSupportColour);
                        GetBottomSelectBorder().UpdateColour(CombatGridManager.Instance.slotSupportColour);
                        GetLeftSelectBorder().UpdateColour(CombatGridManager.Instance.slotSupportColour);
                        GetRightSelectBorder().UpdateColour(CombatGridManager.Instance.slotSupportColour);
                    }
                }
                else
                {
                    GetTopSelectBorder().UpdateColour(CombatGridManager.Instance.slotAggressiveColour);
                    GetBottomSelectBorder().UpdateColour(CombatGridManager.Instance.slotAggressiveColour);
                    GetLeftSelectBorder().UpdateColour(CombatGridManager.Instance.slotAggressiveColour);
                    GetRightSelectBorder().UpdateColour(CombatGridManager.Instance.slotAggressiveColour);
                }
            }
            else
            {
                if (GameManager.Instance.GetActiveItem())
                {
                    if (GameManager.Instance.GetActiveItem().curItemType == ItemPiece.ItemType.OFFENSE)
                    {
                        GetTopSelectBorder().UpdateColour(CombatGridManager.Instance.slotAggressiveColour);
                        GetBottomSelectBorder().UpdateColour(CombatGridManager.Instance.slotAggressiveColour);
                        GetLeftSelectBorder().UpdateColour(CombatGridManager.Instance.slotAggressiveColour);
                        GetRightSelectBorder().UpdateColour(CombatGridManager.Instance.slotAggressiveColour);
                    }
                    else
                    {
                        GetTopSelectBorder().UpdateColour(CombatGridManager.Instance.slotSupportColour);
                        GetBottomSelectBorder().UpdateColour(CombatGridManager.Instance.slotSupportColour);
                        GetLeftSelectBorder().UpdateColour(CombatGridManager.Instance.slotSupportColour);
                        GetRightSelectBorder().UpdateColour(CombatGridManager.Instance.slotSupportColour);
                    }
                }
                else
                {
                    GetTopSelectBorder().UpdateColour(CombatGridManager.Instance.slotAggressiveColour);
                    GetBottomSelectBorder().UpdateColour(CombatGridManager.Instance.slotAggressiveColour);
                    GetLeftSelectBorder().UpdateColour(CombatGridManager.Instance.slotAggressiveColour);
                    GetRightSelectBorder().UpdateColour(CombatGridManager.Instance.slotAggressiveColour);
                }
            }
        }
        else
        {
            GetTopSelectBorder().UpdateColour(GameManager.Instance.movementDetailsTabColour);
            GetBottomSelectBorder().UpdateColour(GameManager.Instance.movementDetailsTabColour);
            GetLeftSelectBorder().UpdateColour(GameManager.Instance.movementDetailsTabColour);
            GetRightSelectBorder().UpdateColour(GameManager.Instance.movementDetailsTabColour);
        }
    }

    public void ToggleSelectBorder(UIElement selectBorder, bool toggle = true)
    {
        if (toggle)
            UpdateSlotBorderColour();

        if (selectBorder == GetTopSelectBorder())
        {
            if (toggle)
                GetTopSelectBorder().UpdateAlpha(1);
            else
                GetTopSelectBorder().UpdateAlpha(0);
        }
        else if (selectBorder == GetLeftSelectBorder())
        {
            if (toggle)
                GetLeftSelectBorder().UpdateAlpha(1);
            else
                GetLeftSelectBorder().UpdateAlpha(0);
        }
        else if (selectBorder == GetRightSelectBorder())
        {
            if (toggle)
                GetRightSelectBorder().UpdateAlpha(1);
            else
                GetRightSelectBorder().UpdateAlpha(0);
        }
        else if (selectBorder == GetBottomSelectBorder())
        {
            if (toggle)
                GetBottomSelectBorder().UpdateAlpha(1);
            else
                GetBottomSelectBorder().UpdateAlpha(0);
        }
    }

    public int GetRangeFromActiveCombatSlot(CombatSlot combatSlot)
    {
        int rangeX = 0;
        int rangeY = 0;

        if (combatSlot.GetSlotIndex().y > GetSlotIndex().y)
        {
            rangeY = (int)combatSlot.GetSlotIndex().y - (int)GetSlotIndex().y;
        }
        else if (combatSlot.GetSlotIndex().y < GetSlotIndex().y)
        {
            rangeY = (int)combatSlot.GetSlotIndex().y - (int)GetSlotIndex().y;
        }
        if (combatSlot.GetSlotIndex().x > GetSlotIndex().x)
        {
            rangeX = (int)combatSlot.GetSlotIndex().x - (int)GetSlotIndex().x;
        }
        else if (combatSlot.GetSlotIndex().x < GetSlotIndex().x)
        {
            rangeX = (int)combatSlot.GetSlotIndex().x - (int)GetSlotIndex().x;
        }

        int finalRange = 0;

        if (Mathf.Abs(rangeY) > Mathf.Abs(rangeX))
        {
            finalRange = Mathf.Abs(rangeY);
        }
        else
        {
            finalRange = Mathf.Abs(rangeX);
        }

        //finalRange ;
        //Debug.Log("range x = " + rangeX + " | range  y = " + rangeY);
        return finalRange;
    }

    public void UpdateRangeFromActiveUnit(int newRange)
    {
        rangeFromActiveUnit = newRange;
    }

    public void ToggleCombatSlotInput(bool toggle = true)
    {
        if (graphicRaycaster)
        {
            graphicRaycaster.enabled = toggle;
        }
    }

    public void ToggleCombatSlotInput2(bool toggle = true)
    {
        if (GetComponent<CanvasGroup>())
        {
            GetComponent<CanvasGroup>().interactable = toggle;
            GetComponent<CanvasGroup>().blocksRaycasts = toggle;
        }
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    public bool GetSelected()
    {
        return selected;
    }

    public void UpdateSelected(bool toggle = true)
    {
        selected = toggle;
    }

    public void UpdateLinkedUnit(UnitFunctionality newUnit)
    {
        linkedUnit = newUnit;
    }

    public void ResetSlotUnitData(bool destroy = false)
    {
        if (GetLinkedUnit())
        {
            if (destroy && GetLinkedUnit().curUnitType == UnitFunctionality.UnitType.ENEMY)
            {
                Destroy(GetLinkedUnit().gameObject);
                UpdateLinkedUnit(null);
            }
        }
        for (int i = 0; i < fallenUnits.Count; i++)
        {
            if (fallenUnits[i] && destroy)
            {
                if (fallenUnits[i].curUnitType == UnitFunctionality.UnitType.ENEMY)
                    Destroy(fallenUnits[i].gameObject);
            }

            ResetFallenUnits();
        }
    }

    public void UpdateAllowed(bool toggle = true, bool flag = true)
    {
        allowed = toggle;

        if (GetAllowed())
        {
            if (!CombatGridManager.Instance.isCombatMode)
            {
                //Debug.Log("asd2");
                if (flag)
                    slotUI.UpdateColour(CombatGridManager.Instance.slotSelectedColour);
                else
                    slotUI.UpdateColour(CombatGridManager.Instance.slotNotAllowedColour);
            }
            else
                slotUI.UpdateColour(CombatGridManager.Instance.slotNotAllowedColour);
        }
        else
            slotUI.UpdateColour(CombatGridManager.Instance.slotNotAllowedColour);

        if (movementSelected)
        {
            slotUI.UpdateColour(CombatGridManager.Instance.slotMovementSelectedColour);
        }

    }

    public bool GetAllowed()
    {
        return allowed;
    }

    public UnitFunctionality GetLinkedUnit()
    {
        return linkedUnit;
    }

    public void UpdateEffectVisualAnimator(RuntimeAnimatorController ac)
    {
        effectDisplayAnimator.runtimeAnimatorController = ac;

        effectDisplayAnimator.SetTrigger("animate");
        //UpdateIconSize();
        //StartWalkAnim();
    }

    public void ToggleCombatSelected(bool toggle = true)
    {
        if (toggle && !walkable)
            return;

        combatSelected = toggle;

        if (toggle)
        {
            if (!CombatGridManager.Instance.targetedCombatSlots.Contains(this))
                CombatGridManager.Instance.targetedCombatSlots.Add(this);

            //if (GetAnimator())  
            //    GetAnimator().SetBool("CombatAttack", true);

            if (GetSelected())
                ToggleSlotSelected();
            else
                ToggleSlotSelected(false);

            if (CombatGridManager.Instance.isCombatMode)
            {
                if (GameManager.Instance.isSkillsMode)
                {
                    if (GameManager.Instance.GetActiveSkill())
                    {
                        if (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE)
                        {
                            slotUI.UpdateColour(CombatGridManager.Instance.slotAggressiveColour);
                            UpdateSlotSelectedColour(CombatGridManager.Instance.slotAggressiveColour);
                        }
                        else
                        {
                            slotUI.UpdateColour(CombatGridManager.Instance.slotSupportColour);
                            UpdateSlotSelectedColour(CombatGridManager.Instance.slotSupportColour);
                        }
                    }
                }
                else if (!GameManager.Instance.isSkillsMode)
                {
                    if (GameManager.Instance.GetActiveItem())
                    {
                        if (GameManager.Instance.GetActiveItem().curItemType == ItemPiece.ItemType.OFFENSE)
                        {
                            slotUI.UpdateColour(CombatGridManager.Instance.slotAggressiveColour);
                            UpdateSlotSelectedColour(CombatGridManager.Instance.slotAggressiveColour);
                        }
                        else
                        {
                            slotUI.UpdateColour(CombatGridManager.Instance.slotSupportColour);
                            UpdateSlotSelectedColour(CombatGridManager.Instance.slotSupportColour);
                        }
                    }
                }
            }


            ToggleTransparentBG(true);
        }
        else
        {
            if (CombatGridManager.Instance.targetedCombatSlots.Contains(this))
                CombatGridManager.Instance.targetedCombatSlots.Remove(this);
            //if (GetAnimator())
             //   GetAnimator().SetBool("CombatAttack", false);

            if (GetAllowed())
                ToggleSlotAllowed();
            else
                ToggleSlotAllowed(false);

            ToggleSlotSelectedSize(true);
            //slotBG.UpdateColour(CombatGridManager.Instance.slotSelectedColour);
            if (!CombatGridManager.Instance.isCombatMode)
            {
                //Debug.Log("asd3");
                //slotUI.UpdateColour(CombatGridManager.Instance.slotSelectedColour);
            }
            else
                slotUI.UpdateColour(CombatGridManager.Instance.slotNotAllowedColour);
            //UpdateSlotSelectedColour(CombatGridManager.Instance.slotSelectedColour);
            ToggleTransparentBG(false);
        }
    }

    public void ToggleSlotAllowed(bool toggle = true, bool flag = true)
    {
        if (toggle && !walkable)
            return;

        UpdateAllowed(toggle, flag);
    }

    public void ResetAnimation()
    {
        StartCoroutine(ResetAnimationCo());
    }

    IEnumerator ResetAnimationCo()
    {
        if (GetAnimator())
        {
            GetAnimator().SetBool("CombatAttack", false);
            GetAnimator().SetBool("CombatAllowed", false);
        }


        yield return new WaitForSeconds(0.01f);

        //GetAnimator().SetBool
        if (GetAnimator())
        {
            GetAnimator().SetBool("CombatAllowed", true);
        }

        //GetAnimator().SetBool("CombatAttack", true);
    }

    public void ToggleSlotSelectedSize(bool forceOff = false)
    {
        sizeIncreased = !sizeIncreased;

        if (forceOff)
            sizeIncreased = false;

        if (sizeIncreased)
        {
            //UpdateSlotSelectedColour(CombatGridManager.Instance.slotSelectedColour);
            //slotTargetedUI.gameObject.transform.localScale = new Vector3(2.75f, 2.75f, 1);
            //slotBG.UpdateColour(CombatGridManager.Instance.slotAllowedColour);
        }
        else
        {
            //UpdateSlotSelectedColour(CombatGridManager.Instance.slotUnSelectedColour);
            //slotTargetedUI.gameObject.transform.localScale = new Vector3(2.25f, 2.25f, 1);
            //slotBG.UpdateColour(CombatGridManager.Instance.slotAllowedColour);
        }
    }

    public void ToggleSlotSelected(bool toggle = true)
    {
        if (toggle && !walkable)
            return;

        if (toggle)
        {
            selected = true;

            //ToggleSlotAllowed(true);
            //UpdateSlotSelectedColour(CombatGridManager.Instance.slotSelectedColour);
            //slotUI.UpdateColour(CombatGridManager.Instance.slotSelectedColour);

            //ToggleSlotSelectedSize();
        }
        else
        {
            selected = false;

            //UpdateSlotSelectedColour(CombatGridManager.Instance.slotUnSelectedColour);
            //slotUI.UpdateColour(CombatGridManager.Instance.slotNotAllowedColour);
            //ToggleSlotSelectedSize();
        }
    }

    public void UpdateSlotSelectedColour(Color color)
    {
        slotTargetedUI.UpdateColour(color);
    }

    public void UpdateSlotIndex(Vector2 newSlotIndex)
    {
        slotIndex = newSlotIndex;
    }

    public Vector2 GetSlotIndex()
    {
        return slotIndex;
    }

    public Vector2 GetLocalIndexFromSlot(CombatSlot fromSlot)
    {
        Vector2 newIndex = Vector2.zero;
        if (GetSlotIndex().x > fromSlot.GetSlotIndex().x)
        {
            newIndex.x = 1;
        }
        else if (GetSlotIndex().x < fromSlot.GetSlotIndex().x)
        {
            newIndex.x = -1;
        }
        else if (GetSlotIndex().x == fromSlot.GetSlotIndex().x)
        {
            newIndex.x = 0;
        }

        if (GetSlotIndex().y > fromSlot.GetSlotIndex().y)
        {
            newIndex.y = 1;
        }
        else if (GetSlotIndex().y < fromSlot.GetSlotIndex().y)
        {
            newIndex.y = -1;
        }
        else if (GetSlotIndex().y == fromSlot.GetSlotIndex().y)
        {
            newIndex.y = 0;
        }

        localIndexFromSlot = newIndex;

        //Debug.Log("index = " + localIndexFromSlot);
        return localIndexFromSlot;
    }

    public string GetDirection(CombatSlot fromSlot)
    {
        if (fromSlot.GetSlotIndex().x > GetSlotIndex().x &&
            fromSlot.GetSlotIndex().y > GetSlotIndex().y)
        {
            return "UpRight";
        }
        else if (fromSlot.GetSlotIndex().x < GetSlotIndex().x &&
            fromSlot.GetSlotIndex().y > GetSlotIndex().y)
        {
            return "UpLeft";
        }
        else if (fromSlot.GetSlotIndex().x > GetSlotIndex().x &&
            fromSlot.GetSlotIndex().y < GetSlotIndex().y)
        {
            return "DownRight";
        }
        else if (fromSlot.GetSlotIndex().x < GetSlotIndex().x &&
            fromSlot.GetSlotIndex().y < GetSlotIndex().y)
        {
            return "DownLeft";
        }
        else if (fromSlot.GetSlotIndex().x > GetSlotIndex().x)
        {
            return "Right";
        }
        else if (fromSlot.GetSlotIndex().x < GetSlotIndex().x)
        {
            return "Left";
        }
        else if (fromSlot.GetSlotIndex().y > GetSlotIndex().y)
        {
            return "Up";
        }
        else if (fromSlot.GetSlotIndex().y < GetSlotIndex().y)
        {
            return "Down";
        }
        else
            return "";
    }
    public void UpdateLocalIndexFromSlot(Vector2 newIndex)
    {
        localIndexFromSlot = newIndex;
    }

    private void Start()
    {
        if (!GameManager.Instance.playerInCombat)
        {
            ToggleSlotAllowed(false);
            ToggleSlotSelected(false);

            //UpdateSlotSelectedColour(CombatGridManager.Instance.slotUnSelectedColour);
            slotUI.UpdateColour(CombatGridManager.Instance.slotNotAllowedColour);
        }

    }
}
