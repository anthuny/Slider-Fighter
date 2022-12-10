using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitArea : MonoBehaviour
{
    public enum HitAreaType { PERFECT, GREAT, GOOD, BAD, MISS }
    public HitAreaType curHitAreaType;

    private Collider2D collider;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    public void HitArea()
    {
        Weapon.instance.CalculatePower(curHitAreaType);
        Weapon.instance.TriggerHitAlertText(curHitAreaType);
    }

    public bool CheckIfHitLineHit(Vector2 hitLinePos)
    {
        if (collider.bounds.Contains(hitLinePos))
            return true;
        else
            return false;
    }
}
