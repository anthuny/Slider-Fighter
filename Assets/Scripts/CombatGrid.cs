using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatGrid : MonoBehaviour
{
    public List<CombatSlot> allCombatSlots = new List<CombatSlot>();
    public List<CombatSlot> fighterSpawnCombatSlots = new List<CombatSlot>();
    public List<CombatSlot> fighterShopCombatSlots = new List<CombatSlot>();
    public CombatSlot newFighterCombatSlot;
    public Transform gridParent;
}
