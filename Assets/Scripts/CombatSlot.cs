using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSlot : MonoBehaviour
{
    [SerializeField] private Vector2 slotIndex;
    [SerializeField] private UIElement slotBG;
    [SerializeField] private UIElement slotTargetedUI;
    [SerializeField] private bool selected = false;
    [SerializeField] private bool allowed = false;
    [SerializeField] private UnitFunctionality linkedUnit;

    private bool sizeIncreased;

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

    public void ToggleSlotAllowed(bool toggle = true)
    {
        UpdateAllowed(toggle);

        if (toggle)
        {
            slotTargetedUI.UpdateAlpha(1, false, 0, false, false);
        }
        else
            slotTargetedUI.UpdateAlpha(0, false, 0, false, false);

    }

    public void ToggleSlotSelectedSize(bool forceOff = false)
    {
        sizeIncreased = !sizeIncreased;

        if (forceOff)
            sizeIncreased = false;

        if (sizeIncreased)
        {
            UpdateSlotSelectedColour(CombatGridManager.Instance.slotSelectedColour);
            slotTargetedUI.gameObject.transform.localScale = new Vector3(2.75f, 2.75f, 1);
            //slotBG.UpdateColour(CombatGridManager.Instance.slotAllowedColour);
        }
        else
        {
            UpdateSlotSelectedColour(CombatGridManager.Instance.slotUnSelectedColour);
            slotTargetedUI.gameObject.transform.localScale = new Vector3(2.25f, 2.25f, 1);
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

    private void Start()
    {
        ToggleSlotAllowed(false);
        ToggleSlotSelected(false);

        UpdateSlotSelectedColour(CombatGridManager.Instance.slotUnSelectedColour);
        slotBG.UpdateColour(CombatGridManager.Instance.slotNotAllowedColour);
    }
}