using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatGridManager : MonoBehaviour
{
    public static CombatGridManager Instance;

    [SerializeField] private List<CombatSlot> fighterSpawnCombatSlots = new List<CombatSlot>();

    [SerializeField] private List<CombatSlot> enemySpawnCombatSlots = new List<CombatSlot>();


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

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
