using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAreaManager : MonoBehaviour
{
    [SerializeField] private Transform hitAreasTrans;
    [SerializeField] private Transform bgHitAreaTrans;
    [SerializeField] private float minYPos;
    [SerializeField] private float maxYPos;

    [SerializeField] private List<WeaponRanges> weaponRanges = new List<WeaponRanges>();

    RectTransform rt2;
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
            
            //rt2 = bgHitAreaTrans.GetComponent<RectTransform>();

            if (bgHitAreaTrans != null)
            {
                if (bgHitAreaTrans.GetComponent<RectTransform>())
                    rt2 = bgHitAreaTrans.GetComponent<RectTransform>();
            }

            rt.localPosition = new Vector3(rt.position.x, rand, 0);

            if (rt2 != null)
            {
                //rt2.position = new Vector3(rt2.position.x, rt.position.y, 0);

                rt2.localPosition = new Vector3(rt2.localPosition.x, rt.localPosition.y, 0);
                rt2.localPosition = new Vector3(rt2.localPosition.x, rt.localPosition.y, rt.position.z);
            }

            Debug.Log("updated position " + rt.position.y);
        }
        else
        {
            Debug.LogError("No Weapon Ranges set");
        }
    }
}