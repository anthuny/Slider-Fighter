using System.Collections.Generic;
using System.Collections;
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

    public IEnumerator HitArea()
    {
        if (curHitAreaType == HitAreaType.PERFECT)
        {
            Weapon.Instance.UpdateHitAreaType(Weapon.HitAreaType.PERFECT);

            Weapon.Instance.CalculatePower(curHitAreaType);
            Weapon.Instance.TriggerHitAlertText(curHitAreaType);

            AudioManager.Instance.Play("AttackBar_Hit");

            AudioManager.Instance.Play("AttackBar_Bad");
            yield return new WaitForSeconds(.15f);
            AudioManager.Instance.Play("AttackBar_Good");
            yield return new WaitForSeconds(.15f);
            AudioManager.Instance.Play("AttackBar_Great");
            yield return new WaitForSeconds(.15f);
            AudioManager.Instance.Play("AttackBar_Perfect");
        }
        else if (curHitAreaType == HitAreaType.GREAT)
        {
            Weapon.Instance.UpdateHitAreaType(Weapon.HitAreaType.GREAT);

            Weapon.Instance.CalculatePower(curHitAreaType);
            Weapon.Instance.TriggerHitAlertText(curHitAreaType);

            AudioManager.Instance.Play("AttackBar_Hit");

            AudioManager.Instance.Play("AttackBar_Bad");
            yield return new WaitForSeconds(.15f);
            AudioManager.Instance.Play("AttackBar_Good");
            yield return new WaitForSeconds(.15f);
            AudioManager.Instance.Play("AttackBar_Great");
        }
        else if (curHitAreaType == HitAreaType.GOOD)
        {
            Weapon.Instance.UpdateHitAreaType(Weapon.HitAreaType.GOOD);

            Weapon.Instance.CalculatePower(curHitAreaType);
            Weapon.Instance.TriggerHitAlertText(curHitAreaType);

            AudioManager.Instance.Play("AttackBar_Hit");

            AudioManager.Instance.Play("AttackBar_Bad");
            yield return new WaitForSeconds(.15f);
            AudioManager.Instance.Play("AttackBar_Good");
        }
        else if (curHitAreaType == HitAreaType.BAD)
        {
            Weapon.Instance.UpdateHitAreaType(Weapon.HitAreaType.BAD);

            Weapon.Instance.CalculatePower(curHitAreaType);
            Weapon.Instance.TriggerHitAlertText(curHitAreaType);

            AudioManager.Instance.Play("AttackBar_Hit");

            AudioManager.Instance.Play("AttackBar_Bad");
        }
        else if (curHitAreaType == HitAreaType.MISS)
        {
            Weapon.Instance.UpdateHitAreaType(Weapon.HitAreaType.MISS);

            Weapon.Instance.CalculatePower(curHitAreaType);
            Weapon.Instance.TriggerHitAlertText(curHitAreaType);

            AudioManager.Instance.Play("AttackBar_Hit");

            AudioManager.Instance.Play("AttackBar_Miss");
        }
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