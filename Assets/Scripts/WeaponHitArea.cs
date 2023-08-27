using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHitArea : MonoBehaviour
{
    public enum HitAreaType { PERFECT, GREAT, GOOD, BAD, MISS }
    public HitAreaType curHitAreaType;

    private BoxCollider2D collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    public void HitArea()
    {
        if (curHitAreaType == HitAreaType.PERFECT)
            Weapon.Instance.UpdateHitAreaType(Weapon.HitAreaType.PERFECT);
        else if (curHitAreaType == HitAreaType.GREAT)
            Weapon.Instance.UpdateHitAreaType(Weapon.HitAreaType.GREAT);
        else if (curHitAreaType == HitAreaType.GOOD)
            Weapon.Instance.UpdateHitAreaType(Weapon.HitAreaType.GOOD);
        else if (curHitAreaType == HitAreaType.BAD)
            Weapon.Instance.UpdateHitAreaType(Weapon.HitAreaType.BAD);
        else if (curHitAreaType == HitAreaType.MISS)
            Weapon.Instance.UpdateHitAreaType(Weapon.HitAreaType.MISS);

        Weapon.Instance.CalculatePower(curHitAreaType);
        Weapon.Instance.TriggerHitAlertText(curHitAreaType);
    }

    public bool CheckIfHitLineHit(GameObject hitLine)
    {
        // bounds = GetComponent<Image>().GetPixelAdjustedRect();
        //Rect hitLineBounds = hitLine.GetComponent<Image>().GetPixelAdjustedRect();

        if (hitLine.GetComponent<BoxCollider2D>().IsTouching(collider))
            return true;
        else
            return false;
    }
}