using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAreaManager : MonoBehaviour
{
    [SerializeField] private Transform hitAreasTrans;
    [SerializeField] private int minYPos;
    [SerializeField] private int maxYPos;

    public void UpdateHitAreaPos()
    {
        float rand = Random.Range(minYPos, maxYPos);
        RectTransform rt = hitAreasTrans.GetComponent<RectTransform>();
        rt.localPosition = new Vector3(rt.position.x, rand, 0);
    }
}