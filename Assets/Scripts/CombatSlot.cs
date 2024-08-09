using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatSlot : MonoBehaviour
{
    [SerializeField] private Vector2 slotIndex;
    [SerializeField] private Vector2 localIndexFromSlot;
    [SerializeField] private UIElement slotBG;
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
    public Animator effectDisplayAnimator;

    private bool sizeIncreased;
    public bool combatSelected;

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

    public void ToggleSelectBorder(UIElement selectBorder, bool toggle = true)
    {
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

    public int GetRangeFromActiveUnit()
    {
        return rangeFromActiveUnit;
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
            if (destroy)
                Destroy(GetLinkedUnit().gameObject);
            UpdateLinkedUnit(null);
        }
        for (int i = 0; i < fallenUnits.Count; i++)
        {
            if (fallenUnits[i] && destroy)
                Destroy(fallenUnits[i].gameObject);
            ResetFallenUnits();
        }
    }

    public void UpdateAllowed(bool toggle = true)
    {
        allowed = toggle;

        if (GetAllowed())
            slotBG.UpdateColour(CombatGridManager.Instance.slotSelectedColour);
        else
            slotBG.UpdateColour(CombatGridManager.Instance.slotNotAllowedColour);

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
        combatSelected = toggle;

        if (toggle)
        {
            if (!CombatGridManager.Instance.targetedCombatSlots.Contains(this))
                CombatGridManager.Instance.targetedCombatSlots.Add(this);

            GetAnimator().SetBool("CombatAttack", true);

            if (GetSelected())
                ToggleSlotSelected();
            else
                ToggleSlotSelected(false);

            slotBG.UpdateColour(CombatGridManager.Instance.slotAggressiveColour);
            UpdateSlotSelectedColour(CombatGridManager.Instance.slotAggressiveColour);
            ToggleTransparentBG(true);
        }
        else
        {
            if (CombatGridManager.Instance.targetedCombatSlots.Contains(this))
                CombatGridManager.Instance.targetedCombatSlots.Remove(this);

            GetAnimator().SetBool("CombatAttack", false);

            if (GetAllowed())
                ToggleSlotAllowed();
            else
                ToggleSlotAllowed(false);

            ToggleSlotSelectedSize(true);
            //slotBG.UpdateColour(CombatGridManager.Instance.slotSelectedColour);
            UpdateSlotSelectedColour(CombatGridManager.Instance.slotSelectedColour);
            ToggleTransparentBG(false);
        }
    }

    public void ToggleSlotAllowed(bool toggle = true)
    {
        UpdateAllowed(toggle);

        GetAnimator().SetBool("CombatAllowed", toggle);
        
        //if (toggle)
        //{
        //slotTargetedUI.UpdateAlpha(1, false, 0, false, false);
        //}
        //else
        //slotTargetedUI.UpdateAlpha(0, false, 0, false, false);

    }

    public void ResetAnimation()
    {
        StartCoroutine(ResetAnimationCo());
    }

    IEnumerator ResetAnimationCo()
    {
        GetAnimator().SetBool("CombatAttack", false);
        GetAnimator().SetBool("CombatAllowed", false);

        yield return new WaitForSeconds(0.01f);
  
        //GetAnimator().SetBool
        GetAnimator().SetBool("CombatAllowed", true);
        //GetAnimator().SetBool("CombatAttack", true);
    }

    public void ToggleSlotSelectedSize(bool forceOff = false)
    {
        sizeIncreased = !sizeIncreased;

        if (forceOff)
            sizeIncreased = false;

        if (sizeIncreased)
        {
            UpdateSlotSelectedColour(CombatGridManager.Instance.slotSelectedColour);
            //slotTargetedUI.gameObject.transform.localScale = new Vector3(2.75f, 2.75f, 1);
            //slotBG.UpdateColour(CombatGridManager.Instance.slotAllowedColour);
        }
        else
        {
            UpdateSlotSelectedColour(CombatGridManager.Instance.slotUnSelectedColour);
            //slotTargetedUI.gameObject.transform.localScale = new Vector3(2.25f, 2.25f, 1);
            //slotBG.UpdateColour(CombatGridManager.Instance.slotAllowedColour);
        }
    }

    public void ToggleSlotSelected(bool toggle = true)
    {
        if (toggle)
        {
            selected = true;

            ToggleSlotAllowed(true);
            //UpdateSlotSelectedColour(CombatGridManager.Instance.slotSelectedColour);
            slotBG.UpdateColour(CombatGridManager.Instance.slotAllowedColour);

            ToggleSlotSelectedSize();
        }
        else
        {
            selected = false;

            //UpdateSlotSelectedColour(CombatGridManager.Instance.slotUnSelectedColour);
            slotBG.UpdateColour(CombatGridManager.Instance.slotNotAllowedColour);
            ToggleSlotSelectedSize();
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

        Debug.Log("index = " + localIndexFromSlot);
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
            return "Upleft";
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
        else
            return "";
    }
    public void UpdateLocalIndexFromSlot(Vector2 newIndex)
    {
        localIndexFromSlot = newIndex;
    }

    private void Start()
    {
        ToggleSlotAllowed(false);
        ToggleSlotSelected(false);

        UpdateSlotSelectedColour(CombatGridManager.Instance.slotUnSelectedColour);
        slotBG.UpdateColour(CombatGridManager.Instance.slotNotAllowedColour);
    }
}
