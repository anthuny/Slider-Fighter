using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatGridManager : MonoBehaviour
{
    public static CombatGridManager Instance;
    public float unitMoveArrowOnAlpha = 0.75f;

    public Color slotAllowedColour;
    public Color slotNotAllowedColour;
    public Color slotDisabledColour;
    public Color slotSelectedColour;
    public Color slotUnSelectedColour;
    public Color slotAggressiveColour;
    public Color slotSupportColour;

    [SerializeField] private UIElement buttonAttackMovementToggle;
    [SerializeField] private UIElement combatMainSlots;
    [SerializeField] private List<CombatSlot> allCombatSlots = new List<CombatSlot>();

    [SerializeField] private CombatSlot newFighterCombatSlot;
    [SerializeField] private List<CombatSlot> fighterSpawnCombatSlots = new List<CombatSlot>();

    [SerializeField] private List<CombatSlot> fighterCombatSlots = new List<CombatSlot>();

    [SerializeField] private List<CombatSlot> enemySpawnCombatSlots = new List<CombatSlot>();

    public bool isCombatMode = false;

    public void UpdateAttackMovementMode()
    {
        isCombatMode = !isCombatMode;

        if (isCombatMode)
        {
            GameManager.Instance.ToggleSkillsItemToggleButton(true);
            UnselectAllSelectedCombatSlots();

            if (GameManager.Instance.isSkillsMode)
            {
                GameManager.Instance.UpdatePlayerAbilityUI(true);
                if (GameManager.Instance.GetActiveSkill() && GameManager.Instance.GetActiveItem())
                    GameManager.Instance.UpdateMainIconDetails(GameManager.Instance.GetActiveSkill(), GameManager.Instance.GetActiveItem());
                else if (GameManager.Instance.GetActiveSkill())
                    GameManager.Instance.UpdateMainIconDetails(GameManager.Instance.GetActiveSkill(), null);
            }
            else
            {
                GameManager.Instance.UpdatePlayerAbilityUI(false);
                if (GameManager.Instance.GetActiveItem())
                    GameManager.Instance.UpdateMainIconDetails(null, GameManager.Instance.GetActiveItem());
            }
        }
        else
        {
            GameManager.Instance.ToggleSkillsItemToggleButton(false);
            UpdateUnitMoveRange(GameManager.Instance.GetActiveUnitFunctionality());

            GameManager.Instance.UpdatePlayerAbilityUI(false, false, true);
            GameManager.Instance.UpdateMainIconDetails(null, null);
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

    public void UnselectAllSelectedCombatSlots()
    {
        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            allCombatSlots[i].ToggleSlotAllowed(false);
            allCombatSlots[i].ToggleSlotSelected(false);
        }
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

        int unitMovementRange = unit.GetMovementRange();

        UnselectAllSelectedCombatSlots();

        for (int i = 0; i < allCombatSlots.Count; i++)
        {
            if (allCombatSlots[i].GetSlotIndex().x == unitCombatIndex.x 
                && allCombatSlots[i].GetSlotIndex().y == unitCombatIndex.y)
            {
                //self target
            }

            int negativeXdiff = (int)unitCombatIndex.x - (int)allCombatSlots[i].GetSlotIndex().x;
            int positiveXdiff = (int)allCombatSlots[i].GetSlotIndex().x - (int)unitCombatIndex.x;

            int negativeYdiff = (int)unitCombatIndex.y - (int)allCombatSlots[i].GetSlotIndex().y;
            int positiveYdiff = (int)allCombatSlots[i].GetSlotIndex().y - (int)unitCombatIndex.y;

            negativeXdiff = Mathf.Abs(negativeXdiff);
            positiveXdiff = Mathf.Abs(positiveXdiff);

            negativeYdiff = Mathf.Abs(negativeYdiff);
            positiveYdiff = Mathf.Abs(positiveYdiff);

  

            for (int x = 1; x < unitMovementRange+1; x++)
            {
                CombatSlot combatSlot = null;

                // Select left combat slots
                if (GetCombatSlot(new Vector2(unitCombatIndex.x - x, unitCombatIndex.y)) != null
                    && allCombatSlots[i].GetSlotIndex() == new Vector2(unitCombatIndex.x - x, unitCombatIndex.y))
                {
                    combatSlot = GetCombatSlot(new Vector2(unitCombatIndex.x - x, unitCombatIndex.y));
                }
                // Select right combat slots
                else if (GetCombatSlot(new Vector2(unitCombatIndex.x + x, unitCombatIndex.y)) != null
                    && allCombatSlots[i].GetSlotIndex() == new Vector2(unitCombatIndex.x + x, unitCombatIndex.y))
                {
                    combatSlot = GetCombatSlot(new Vector2(unitCombatIndex.x + x, unitCombatIndex.y));
                }
                // Select Up combat slots
                else if (GetCombatSlot(new Vector2(unitCombatIndex.x, unitCombatIndex.y + x)) != null
                    && allCombatSlots[i].GetSlotIndex() == new Vector2(unitCombatIndex.x, unitCombatIndex.y + x))
                {
                    combatSlot = GetCombatSlot(new Vector2(unitCombatIndex.x, unitCombatIndex.y + x));
                }
                // Select Down combat slots
                else if (GetCombatSlot(new Vector2(unitCombatIndex.x, unitCombatIndex.y - x)) != null
                    && allCombatSlots[i].GetSlotIndex() == new Vector2(unitCombatIndex.x, unitCombatIndex.y - x))
                {
                    combatSlot = GetCombatSlot(new Vector2(unitCombatIndex.x, unitCombatIndex.y - x));
                }

                // Select Up Left combat slots
                if (GetCombatSlot(new Vector2(unitCombatIndex.x - x, unitCombatIndex.y + x)) != null
                    && allCombatSlots[i].GetSlotIndex() == new Vector2(unitCombatIndex.x - x, unitCombatIndex.y + x))
                {
                    combatSlot = GetCombatSlot(new Vector2(unitCombatIndex.x - x, unitCombatIndex.y + x));
                }
                // Select Down Left combat slots
                else if (GetCombatSlot(new Vector2(unitCombatIndex.x - x, unitCombatIndex.y - x)) != null
                    && allCombatSlots[i].GetSlotIndex() == new Vector2(unitCombatIndex.x - x, unitCombatIndex.y - x))
                {
                    combatSlot = GetCombatSlot(new Vector2(unitCombatIndex.x - x, unitCombatIndex.y - x));
                }
                // Select Up Right combat slots
                else if (GetCombatSlot(new Vector2(unitCombatIndex.x + x, unitCombatIndex.y + x)) != null
                    && allCombatSlots[i].GetSlotIndex() == new Vector2(unitCombatIndex.x + x, unitCombatIndex.y + x))
                {
                    combatSlot = GetCombatSlot(new Vector2(unitCombatIndex.x + x, unitCombatIndex.y + x));
                }
                // Select Down Right combat slots
                else if (GetCombatSlot(new Vector2(unitCombatIndex.x + x, unitCombatIndex.y - x)) != null
                    && allCombatSlots[i].GetSlotIndex() == new Vector2(unitCombatIndex.x + x, unitCombatIndex.y - x))
                {
                    combatSlot = GetCombatSlot(new Vector2(unitCombatIndex.x + x, unitCombatIndex.y - x));
                }

                if (combatSlot != null)
                {
                    combatSlot.ToggleSlotAllowed(true);
                }
            }


            if (negativeXdiff >= unitMovementRange)
            {
                // If left combat slot exists, enable it
                if (GetCombatSlot(new Vector2((int)allCombatSlots[i].GetSlotIndex().x - 1, allCombatSlots[i].GetSlotIndex().y)))
                {
                
                }
            }
        }
        // Toggle Combat unit slot available around them for movement

    }

    public Vector2 GetCombatSlotIndex(int slotIndex)
    {
        Vector2 newSlotIndex = new Vector2(0,0);

        #region Caluclate New Slot Index
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
