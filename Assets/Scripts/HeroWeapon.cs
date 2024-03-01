using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class HeroWeapon : MonoBehaviour
{
    public int lineSpeed;
    public string ownedUnitName;
    public Transform hitLine;
    public Transform topBarBorder;
    public Transform botBarBorder;
    public List<WeaponHitArea> weaponHitAreas = new List<WeaponHitArea>();
    public UIElement hitAlertText;
    public UIElement weaponUI;
    public HitAreaManager hitAreaManager;
    public UIElement flashUI;
    public UIElement hitsRemainingText;
    public UIElement hitsAccumulatedText;

    private Color heroColour;

    public void WeaponFlash(Color start, Color end, float duration)
    {
        StartCoroutine(WeaponFlashCo(start, end, duration));
    }
    public IEnumerator WeaponFlashCo(Color start, Color end, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
            heroColour = Color.Lerp(start, end, normalizedTime);
            flashUI.UpdateColour(heroColour);
            yield return null;
        }

        heroColour = end; //without this, the value will end at something like 0.9992367
    }
}
