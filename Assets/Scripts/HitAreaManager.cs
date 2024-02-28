using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAreaManager : MonoBehaviour
{
    [SerializeField] private Transform hitAreasTrans;
    [SerializeField] private float minYPos;
    [SerializeField] private float maxYPos;

    [SerializeField] private List<WeaponRanges> weaponRanges = new List<WeaponRanges>();

    public void UpdateHitAreaPos()
    {
        int index = 0;
        WeaponRanges selectedRange = null;

        if (weaponRanges.Count != 0)
        {
            index = Random.Range(0, weaponRanges.Count);
            selectedRange = weaponRanges[index];

            float rand = Random.Range(selectedRange.minFloat, selectedRange.maxFloat);
            RectTransform rt = hitAreasTrans.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(rt.position.x, rand, 0);
        }
        else
        {
            Debug.LogError("No Weapon Ranges set");
        }
    }
}
