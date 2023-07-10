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
            Weapon.instance.UpdateHitAreaType(Weapon.HitAreaType.PERFECT);
        else if (curHitAreaType == HitAreaType.GREAT)
            Weapon.instance.UpdateHitAreaType(Weapon.HitAreaType.GREAT);
        else if (curHitAreaType == HitAreaType.GOOD)
            Weapon.instance.UpdateHitAreaType(Weapon.HitAreaType.GOOD);
        else if (curHitAreaType == HitAreaType.BAD)
            Weapon.instance.UpdateHitAreaType(Weapon.HitAreaType.BAD);
        else if (curHitAreaType == HitAreaType.MISS)
            Weapon.instance.UpdateHitAreaType(Weapon.HitAreaType.MISS);

        Weapon.instance.CalculatePower(curHitAreaType);
        Weapon.instance.TriggerHitAlertText(curHitAreaType);
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