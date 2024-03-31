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

    public void SetHitLinePosition()
    {
        //Debug.Log("Setting y pos " + gameObject.transform.localPosition.y);
        WeaponManager.Instance.hitLine.localPosition = new Vector2(WeaponManager.Instance.hitLine.localPosition.x, gameObject.transform.localPosition.y);
    }

    public void ToggleHitAreaDisplay(bool toggle = true)
    {
        if (toggle)
            gameObject.GetComponent<UIElement>().UpdateAlpha(1);
        else
            gameObject.GetComponent<UIElement>().UpdateAlpha(0);
    }

    public IEnumerator HitArea()
    {
        if (curHitAreaType == HitAreaType.PERFECT)
        {
            WeaponManager.Instance.UpdateHitAreaType(WeaponManager.HitAreaType.PERFECT);

            //WeaponManager.Instance.CalculatePower(curHitAreaType);
            WeaponManager.Instance.TriggerHitAlertText(curHitAreaType);

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
            WeaponManager.Instance.UpdateHitAreaType(WeaponManager.HitAreaType.GREAT);

            //WeaponManager.Instance.CalculatePower(curHitAreaType);
            WeaponManager.Instance.TriggerHitAlertText(curHitAreaType);

            AudioManager.Instance.Play("AttackBar_Hit");

            AudioManager.Instance.Play("AttackBar_Bad");
            yield return new WaitForSeconds(.15f);
            AudioManager.Instance.Play("AttackBar_Good");
            yield return new WaitForSeconds(.15f);
            AudioManager.Instance.Play("AttackBar_Great");
        }
        else if (curHitAreaType == HitAreaType.GOOD)
        {
            WeaponManager.Instance.UpdateHitAreaType(WeaponManager.HitAreaType.GOOD);

            //WeaponManager.Instance.CalculatePower(curHitAreaType);
            WeaponManager.Instance.TriggerHitAlertText(curHitAreaType);

            AudioManager.Instance.Play("AttackBar_Hit");

            AudioManager.Instance.Play("AttackBar_Bad");
            yield return new WaitForSeconds(.15f);
            AudioManager.Instance.Play("AttackBar_Good");
        }
        else if (curHitAreaType == HitAreaType.BAD)
        {
            WeaponManager.Instance.UpdateHitAreaType(WeaponManager.HitAreaType.BAD);

            //WeaponManager.Instance.CalculatePower(curHitAreaType);
            WeaponManager.Instance.TriggerHitAlertText(curHitAreaType);

            AudioManager.Instance.Play("AttackBar_Hit");

            AudioManager.Instance.Play("AttackBar_Bad");
        }
        else if (curHitAreaType == HitAreaType.MISS)
        {
            WeaponManager.Instance.UpdateHitAreaType(WeaponManager.HitAreaType.MISS);

            //WeaponManager.Instance.CalculatePower(curHitAreaType);
            WeaponManager.Instance.TriggerHitAlertText(curHitAreaType);

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