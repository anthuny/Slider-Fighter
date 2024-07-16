using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSlot : MonoBehaviour
{
    [SerializeField] private Vector2 slotIndex;
    [SerializeField] private UIElement slotBG;
    [SerializeField] private UIElement slotTargetedUI;
    public bool selected = false;

    private bool sizeIncreased;

    public void ToggleSlotAllowed(bool toggle = true)
    {
        if (toggle)
            slotTargetedUI.UpdateAlpha(1, false, 0, false, false);
        else
            slotTargetedUI.UpdateAlpha(0, false, 0, false, false);
    }

    public void ToggleSlotSelectedSize()
    {
        sizeIncreased = !sizeIncreased;

        if (!sizeIncreased)
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
            slotBG.UpdateColour(CombatGridManager.Instance.slotAllowedColour);
            UpdateSlotSelectedColour(CombatGridManager.Instance.slotSelectedColour);
            ToggleSlotAllowed(true);
            ToggleSlotSelectedSize();
        }
        else
        {
            selected = false;
            slotBG.UpdateColour(CombatGridManager.Instance.slotNotAllowedColour);
            UpdateSlotSelectedColour(CombatGridManager.Instance.slotUnSelectedColour);
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
