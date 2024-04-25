using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterStatManager : MonoBehaviour
{
    public static FighterStatManager Instance;
    [SerializeField] private List<UIElement> defenseStatImages = new List<UIElement>();
    [SerializeField] private List<UIElement> attackStatImages = new List<UIElement>();
    [SerializeField] private List<UIElement> healingStatImages = new List<UIElement>();
    [SerializeField] private List<UIElement> utilityStatImages = new List<UIElement>();

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateFighterStats(UnitData unit)
    {
        for (int i = 0; i < defenseStatImages.Count; i++)
        {
            if (unit == null)
            {
                defenseStatImages[i].gameObject.SetActive(false);
                continue;
            }

            if (i < unit.statDefense)
                defenseStatImages[i].gameObject.SetActive(true);
            else
                defenseStatImages[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < attackStatImages.Count; i++)
        {
            if (unit == null)
            {
                attackStatImages[i].gameObject.SetActive(false);
                continue;
            }

            if (i < unit.statAttack)
                attackStatImages[i].gameObject.SetActive(true);
            else
                attackStatImages[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < healingStatImages.Count; i++)
        {
            if (unit == null)
            {
                healingStatImages[i].gameObject.SetActive(false);
                continue;
            }

            if (i < unit.statHealing)
                healingStatImages[i].gameObject.SetActive(true);
            else
                healingStatImages[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < utilityStatImages.Count; i++)
        {
            if (unit == null)
            {
                utilityStatImages[i].gameObject.SetActive(false);
                continue;
            }

            if (i < unit.statUtility)
                utilityStatImages[i].gameObject.SetActive(true);
            else
                utilityStatImages[i].gameObject.SetActive(false);
        }
    }
}
