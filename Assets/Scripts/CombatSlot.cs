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
    [SerializeField] private Animator animator;
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    [SerializeField] private int rangeFromActiveUnit;

    private bool sizeIncreased;
    public bool combatSelected;

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

    public void ToggleCombatSelected(bool toggle = true)
    {
        combatSelected = toggle;

        if (toggle)
        {
            GetAnimator().SetBool("CombatAttack", true);

            if (GetSelected())
                ToggleSlotSelected();
            else
                ToggleSlotSelected(false);

            slotBG.UpdateColour(CombatGridManager.Instance.slotAggressiveColour);
            UpdateSlotSelectedColour(CombatGridManager.Instance.slotAggressiveColour);
        }
        else
        {
            GetAnimator().SetBool("CombatAttack", false);

            if (GetAllowed())
                ToggleSlotAllowed();
            else
                ToggleSlotAllowed(false);

            ToggleSlotSelectedSize(true);
            //slotBG.UpdateColour(CombatGridManager.Instance.slotSelectedColour);
            UpdateSlotSelectedColour(CombatGridManager.Instance.slotSelectedColour);
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
